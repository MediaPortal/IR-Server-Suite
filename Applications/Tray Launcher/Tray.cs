using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using NamedPipes;
using IrssUtils;

namespace TrayLauncher
{

  public class Tray
  {

    #region Constants

    const string DefaultKeyCode = "31730";

    static readonly string ConfigurationFile = Common.FolderAppData + "Tray Launcher\\Tray Launcher.xml";

    #endregion Constants

    #region Variables

    static Common.MessageHandler _handleMessage = null;

    bool _registered = false;
    bool _keepAlive = true;
    int _echoID = -1;
    Thread _keepAliveThread;

    string _serverHost;
    string _programFile;
    bool _autoRun;
    bool _launchOnLoad;
    string _launchKeyCode;

    string _localPipeName = null;

    NotifyIcon _notifyIcon;

    #endregion Variables

    #region Properties

    internal static Common.MessageHandler HandleMessage
    {
      get { return _handleMessage; }
      set { _handleMessage = value; }
    }

    #endregion Properties
    
    #region Constructor

    public Tray()
    {
      ContextMenuStrip contextMenu = new ContextMenuStrip();
      contextMenu.Items.Add(new ToolStripMenuItem("&Launch", null, new EventHandler(ClickLaunch)));
      contextMenu.Items.Add(new ToolStripMenuItem("&Setup", null, new EventHandler(ClickSetup)));
      contextMenu.Items.Add(new ToolStripMenuItem("&Quit", null, new EventHandler(ClickQuit)));

      _notifyIcon = new NotifyIcon();
      _notifyIcon.ContextMenuStrip = contextMenu;
      _notifyIcon.Icon = Properties.Resources.Icon16Connecting;
      _notifyIcon.Text = "Tray Launcher - Connecting ...";
      _notifyIcon.DoubleClick += new EventHandler(ClickSetup);
    }

    #endregion Constructor

    #region Implementation

    internal bool Start()
    {
      try
      {
        LoadSettings();

        bool didSetup = false;
        if (String.IsNullOrEmpty(_programFile) || String.IsNullOrEmpty(_serverHost))
        {
          if (!Configure())
            return false;

          didSetup = true;
        }

        if (OpenLocalPipe())
        {
          _notifyIcon.Visible = true;

          _keepAliveThread = new Thread(new ThreadStart(KeepAliveThread));
          _keepAliveThread.Start();

          if (!didSetup && _launchOnLoad)
            ClickLaunch(null, null);

          return true;
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
      }

      return false;
    }

    void Stop()
    {
      _notifyIcon.Visible = false;

      _keepAlive = false;

      try
      {
        if (_keepAliveThread != null && _keepAliveThread.IsAlive)
          _keepAliveThread.Abort();
      }
      catch { }

      try
      {
        if (_registered)
        {
          _registered = false;

          PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Unregister", null);
          PipeAccess.SendMessage(Common.ServerPipeName, _serverHost, message.ToString());
        }
      }
      catch { }

      try
      {
        if (PipeAccess.ServerRunning)
          PipeAccess.StopServer();
      }
      catch { }
    }

    void LoadSettings()
    {
      try
      {
        _autoRun = SystemRegistry.GetAutoRun("Tray Launcher");
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());

        _autoRun = false;
      }

      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationFile);

        _serverHost     = doc.DocumentElement.Attributes["ServerHost"].Value;
        _programFile    = doc.DocumentElement.Attributes["ProgramFile"].Value;
        _launchOnLoad   = bool.Parse(doc.DocumentElement.Attributes["LaunchOnLoad"].Value);
        _launchKeyCode  = doc.DocumentElement.Attributes["LaunchKeyCode"].Value;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());

        _serverHost     = String.Empty;
        _programFile    = String.Empty;
        _launchOnLoad   = false;
        _launchKeyCode  = DefaultKeyCode;
      }
    }
    void SaveSettings()
    {
      try
      {
        if (_autoRun)
          SystemRegistry.SetAutoRun("Tray Launcher", Application.ExecutablePath);
        else
          SystemRegistry.RemoveAutoRun("Tray Launcher");
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
      }

      try
      {
        XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, System.Text.Encoding.UTF8);
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 1;
        writer.IndentChar = (char)9;
        writer.WriteStartDocument(true);
        writer.WriteStartElement("settings"); // <settings>

        writer.WriteAttributeString("ServerHost", _serverHost);
        writer.WriteAttributeString("ProgramFile", _programFile);
        writer.WriteAttributeString("LaunchOnLoad", _launchOnLoad.ToString());
        writer.WriteAttributeString("LaunchKeyCode", _launchKeyCode);

        writer.WriteEndElement(); // </settings>
        writer.WriteEndDocument();
        writer.Close();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
      }
    }

    bool OpenLocalPipe()
    {
      try
      {
        int pipeNumber = 1;
        bool retry = false;

        do
        {
          string localPipeTest = String.Format(Common.LocalPipeFormat, pipeNumber);

          if (PipeAccess.PipeExists(String.Format("\\\\.\\pipe\\{0}", localPipeTest)))
          {
            if (++pipeNumber <= Common.MaximumLocalClientCount)
              retry = true;
            else
              throw new Exception(String.Format("Maximum local client limit ({0}) reached", Common.MaximumLocalClientCount));
          }
          else
          {
            if (!PipeAccess.StartServer(localPipeTest, new PipeMessageHandler(ReceivedMessage)))
              throw new Exception(String.Format("Failed to start local pipe server \"{0}\"", localPipeTest));

            _localPipeName = localPipeTest;
            retry = false;
          }
        }
        while (retry);

        return true;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
        return false;
      }
    }

    bool ConnectToServer()
    {
      try
      {
        PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Register", null);
        PipeAccess.SendMessage(Common.ServerPipeName, _serverHost, message.ToString());
        return true;
      }
      catch (AppModule.NamedPipes.NamedPipeIOException)
      {
        return false;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
        return false;
      }
    }

    void KeepAliveThread()
    {
      Random random = new Random((int)DateTime.Now.Ticks);
      bool reconnect;
      int attempt;

      _keepAlive = true;
      while (_keepAlive)
      {
        reconnect = true;

        _notifyIcon.Icon = Properties.Resources.Icon16Connecting;
        _notifyIcon.Text = "Tray Launcher - Connecting ...";

        #region Connect to server

        IrssLog.Info("Connecting ({0}) ...", _serverHost);
        attempt = 0;
        while (_keepAlive && reconnect)
        {
          if (ConnectToServer())
          {
            reconnect = false;
          }
          else
          {
            int wait;

            if (attempt <= 50)
              attempt++;

            if (attempt > 50)
              wait = 30;      // 30 seconds
            else if (attempt > 20)
              wait = 10;      // 10 seconds
            else if (attempt > 10)
              wait = 5;       // 5 seconds
            else
              wait = 1;       // 1 second

            for (int sleeps = 0; sleeps < wait && _keepAlive; sleeps++)
              Thread.Sleep(1000);
          }
        }

        #endregion Connect to server

        #region Wait for registered

        // Give up after 10 seconds ...
        attempt = 0;
        while (_keepAlive && !_registered && !reconnect)
        {
          if (++attempt >= 10)
            reconnect = true;
          else
            Thread.Sleep(1000);
        }

        #endregion Wait for registered

        #region Registered ...

        if (_keepAlive && _registered && !reconnect)
        {
          IrssLog.Info("Connected ({0})", _serverHost);

          _notifyIcon.Icon = Properties.Resources.Icon16;
          _notifyIcon.Text = "Tray Launcher";
        }

        #endregion Registered ...

        #region Ping the server repeatedly

        while (_keepAlive && _registered && !reconnect)
        {
          int pingID = random.Next();
          long pingTime = DateTime.Now.Ticks;

          try
          {
            PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Ping", BitConverter.GetBytes(pingID));
            PipeAccess.SendMessage(Common.ServerPipeName, _serverHost, message.ToString());
          }
          catch
          {
            // Failed to ping ... reconnect ...
            IrssLog.Warn("Failed to ping, attempting to reconnect ...");
            _registered = false;
            reconnect = true;
            break;
          }

          // Wait 10 seconds for a ping echo ...
          bool receivedEcho = false;
          while (_keepAlive && _registered && !reconnect &&
            !receivedEcho && DateTime.Now.Ticks - pingTime < 10 * 1000 * 10000)
          {
            if (_echoID == pingID)
            {
              receivedEcho = true;
            }
            else
            {
              Thread.Sleep(1000);
            }
          }

          if (receivedEcho) // Received ping echo ...
          {
            // Wait 60 seconds before re-pinging ...
            for (int sleeps = 0; sleeps < 60 && _keepAlive && _registered; sleeps++)
              Thread.Sleep(1000);
          }
          else // Didn't receive ping echo ...
          {
            IrssLog.Warn("No echo to ping, attempting to reconnect ...");

            // Break out of pinging cycle ...
            _registered = false;
            reconnect = true;
          }
        }

        #endregion Ping the server repeatedly

      }

    }

    void ReceivedMessage(string message)
    {
      PipeMessage received = PipeMessage.FromString(message);

      IrssLog.Debug("Received Message \"{0}\"", received.Name);

      try
      {
        switch (received.Name)
        {
          case "Keyboard Event":
          case "Mouse Event":
            break;

          case "Register Success":
            {
              IrssLog.Info("Registered to IR Server");
              _registered = true;
              //_transceiverInfo = TransceiverInfo.FromBytes(received.Data);
              break;
            }

          case "Register Failure":
            {
              IrssLog.Warn("IR Server refused to register");
              _registered = false;
              break;
            }

          case "Remote Event":
            {
              string keyCode = Encoding.ASCII.GetString(received.Data);
              RemoteHandlerCallback(keyCode);
              break;
            }

          case "Server Shutdown":
            {
              IrssLog.Warn("IR Server Shutdown - Tray Launcher disabled until IR Server returns");
              _registered = false;
              break;
            }

          case "Echo":
            {
              _echoID = BitConverter.ToInt32(received.Data, 0);
              break;
            }

          case "Error":
            {
              IrssLog.Info(Encoding.ASCII.GetString(received.Data));
              break;
            }

          default:
            {
              IrssLog.Info("Unknown message received: " + received.Name);
              break;
            }
        }

        // If another module of the program has registered to receive messages too ...
        if (_handleMessage != null)
          _handleMessage(message);
      }
      catch (Exception ex)
      {
        IrssLog.Error("ReceivedMessage - {0}", ex.Message);
      }
    }

    void RemoteHandlerCallback(string keyCode)
    {
      IrssLog.Info("Remote Event: {0}", keyCode);

      if (keyCode == _launchKeyCode)
        ClickLaunch(null, null);
    }

    bool Configure()
    {
      Setup setup = new Setup();

      setup.AutoRun       = _autoRun;
      setup.ServerHost    = _serverHost;
      setup.ProgramFile   = _programFile;
      setup.LaunchOnLoad  = _launchOnLoad;
      setup.LaunchKeyCode = _launchKeyCode;

      if (setup.ShowDialog() == DialogResult.OK)
      {
        _autoRun        = setup.AutoRun;
        _serverHost     = setup.ServerHost;
        _programFile    = setup.ProgramFile;
        _launchOnLoad   = setup.LaunchOnLoad;
        _launchKeyCode  = setup.LaunchKeyCode;

        SaveSettings();
        
        return true;
      }

      return false;
    }

    void ClickSetup(object sender, EventArgs e)
    {
      IrssLog.Info("Setup");

      if (Configure())
      {
        Stop();
        Thread.Sleep(500);
        Start();
      }
    }
    void ClickLaunch(object sender, EventArgs e)
    {
      IrssLog.Info("Launch");

      try
      {
        // Check for multiple instances
        foreach (Process process in Process.GetProcesses())
        {
          try
          {
            if (Path.GetFileName(process.MainModule.ModuleName).Equals(Path.GetFileName(_programFile), StringComparison.InvariantCultureIgnoreCase))
            {
              IrssLog.Info("Program already running");
              return;
            }
          }
          catch { }
        }

        // Launch program
        Process launch = new Process();
        launch.StartInfo.FileName = _programFile;
        launch.StartInfo.UseShellExecute = true;
        launch.Start();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.Message);
        MessageBox.Show(ex.Message, "Tray Launcher", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
    void ClickQuit(object sender, EventArgs e)
    {
      IrssLog.Info("Quit");

      Stop();

      Application.Exit();
    }

    #endregion Implementation

  }

}
