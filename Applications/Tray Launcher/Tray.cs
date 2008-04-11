using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using IrssComms;
using IrssUtils;

namespace TrayLauncher
{

  /// <summary>
  /// Tray launcher main class.
  /// </summary>
  class Tray
  {

    #region Constants

    static readonly string ConfigurationFile = Path.Combine(Common.FolderAppData, "Tray Launcher\\Tray Launcher.xml");

    #endregion Constants

    #region Variables

    static ClientMessageSink _handleMessage;

    Client _client;

    static bool _registered;

    string _serverHost;
    string _programFile;
    bool _autoRun;
    bool _launchOnLoad;
    string _launchKeyCode;

    Container _container;
    NotifyIcon _notifyIcon;

    bool _inConfiguration;

    #endregion Variables

    #region Properties

    internal static ClientMessageSink HandleMessage
    {
      get { return _handleMessage; }
      set { _handleMessage = value; }
    }

    internal static bool Registered
    {
      get { return _registered; }
    }

    #endregion Properties

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Tray"/> class.
    /// </summary>
    public Tray()
    {
      ContextMenuStrip contextMenu = new ContextMenuStrip();
      contextMenu.Items.Add(new ToolStripLabel("Tray Launcher"));
      contextMenu.Items.Add(new ToolStripSeparator());
      contextMenu.Items.Add(new ToolStripMenuItem("&Launch", null, new EventHandler(ClickLaunch)));
      contextMenu.Items.Add(new ToolStripMenuItem("&Setup", null, new EventHandler(ClickSetup)));
      contextMenu.Items.Add(new ToolStripMenuItem("&Quit", null, new EventHandler(ClickQuit)));

      _container = new Container();

      _notifyIcon = new NotifyIcon(_container);
      _notifyIcon.ContextMenuStrip = contextMenu;
      _notifyIcon.DoubleClick += new EventHandler(ClickSetup);

      UpdateTrayIcon("Tray Launcher - Connecting ...", Properties.Resources.Icon16Connecting);
    }

    #endregion Constructor

    #region Implementation

    void UpdateTrayIcon(string text, Icon icon)
    {
      if (String.IsNullOrEmpty(text))
        throw new ArgumentNullException("text");

      if (icon == null)
        throw new ArgumentNullException("icon");

      _notifyIcon.Text = text;
      _notifyIcon.Icon = icon;
    }

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

        bool clientStarted = false;

        try
        {
          IPAddress serverIP = Client.GetIPFromName(_serverHost);
          IPEndPoint endPoint = new IPEndPoint(serverIP, IrssComms.Server.DefaultPort);

          clientStarted = StartClient(endPoint);
        }
        catch (Exception ex)
        {
          IrssLog.Error(ex);
          MessageBox.Show("Failed to start IR Server communications, refer to log file for more details.", "Tray Launcher - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
          clientStarted = false;
        }

        if (clientStarted)
        {
          _notifyIcon.Visible = true;

          if (!didSetup && _launchOnLoad)
            ClickLaunch(null, null);

          return true;
        }
        else
        {
          Configure();
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }

      return false;
    }

    void Stop()
    {
      _notifyIcon.Visible = false;

      try
      {
        if (_registered)
        {
          _registered = false;

          IrssMessage message = new IrssMessage(MessageType.UnregisterClient, MessageFlags.Request);
          _client.Send(message);
        }
      }
      catch { }
      
      StopClient();
    }

    void LoadSettings()
    {
      try
      {
        _autoRun = SystemRegistry.GetAutoRun("Tray Launcher");
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);

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
      catch (FileNotFoundException)
      {
        IrssLog.Warn("Configuration file not found, using defaults");

        CreateDefaultSettings();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);

        CreateDefaultSettings();
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
        IrssLog.Error(ex);
      }

      try
      {
        using (XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, Encoding.UTF8))
        {
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
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
    }
    void CreateDefaultSettings()
    {
      _serverHost     = "localhost";
      _programFile    = String.Empty;
      _launchOnLoad   = false;
      _launchKeyCode  = "Start";

      SaveSettings();
    }

    void CommsFailure(object obj)
    {
      Exception ex = obj as Exception;

      if (ex != null)
        IrssLog.Error("Communications failure: {0}", ex.Message);
      else
        IrssLog.Error("Communications failure");

      StopClient();

      MessageBox.Show("Please report this error.", "Tray Launcher - Communications failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    void Connected(object obj)
    {
      IrssLog.Info("Connected to server");

      UpdateTrayIcon("Tray Launcher", Properties.Resources.Icon16);

      IrssMessage message = new IrssMessage(MessageType.RegisterClient, MessageFlags.Request);
      _client.Send(message);
    }
    void Disconnected(object obj)
    {
      IrssLog.Warn("Communications with server has been lost");

      UpdateTrayIcon("Tray Launcher - Re-Connecting ...", Properties.Resources.Icon16Connecting);

      Thread.Sleep(1000);
    }

    bool StartClient(IPEndPoint endPoint)
    {
      if (_client != null)
        return false;

      ClientMessageSink sink = new ClientMessageSink(ReceivedMessage);

      _client = new Client(endPoint, sink);
      _client.CommsFailureCallback  = new WaitCallback(CommsFailure);
      _client.ConnectCallback       = new WaitCallback(Connected);
      _client.DisconnectCallback    = new WaitCallback(Disconnected);
      
      if (_client.Start())
      {
        return true;
      }
      else
      {
        _client = null;
        return false;
      }
    }
    void StopClient()
    {
      if (_client == null)
        return;

      _client.Dispose();
      _client = null;

      _registered = false;
    }

    void ReceivedMessage(IrssMessage received)
    {
      IrssLog.Debug("Received Message \"{0}\"", received.Type);

      try
      {
        switch (received.Type)
        {
          case MessageType.RegisterClient:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              //_irServerInfo = IRServerInfo.FromBytes(received.DataAsBytes);
              _registered = true;

              IrssLog.Info("Registered to Input Service");
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              _registered = false;
              IrssLog.Warn("Input Service refused to register");
            }
            break;

          case MessageType.RemoteEvent:
            byte[] data = received.GetDataAsBytes();
            int deviceNameSize = BitConverter.ToInt32(data, 0);
            string deviceName = Encoding.ASCII.GetString(data, 4, deviceNameSize);
            int keyCodeSize = BitConverter.ToInt32(data, 4 + deviceNameSize);
            string keyCode = Encoding.ASCII.GetString(data, 8 + deviceNameSize, keyCodeSize);

            RemoteHandlerCallback(deviceName, keyCode);
            break;

          case MessageType.ServerShutdown:
            IrssLog.Warn("Input Service Shutdown - Tray Launcher disabled until Input Service returns");
            _registered = false;
            break;

          case MessageType.Error:
            IrssLog.Error("Received error: {0}", received.GetDataAsString());
            break;
        }

        if (_handleMessage != null)
          _handleMessage(received);
      }
      catch (Exception ex)
      {
        IrssLog.Error("ReceivedMessage(): {0}", ex.ToString());
      }
    }

    void RemoteHandlerCallback(string deviceName, string keyCode)
    {
      IrssLog.Info("Remote Event: {0}", keyCode);

      if (keyCode.Equals(_launchKeyCode, StringComparison.Ordinal))
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

      _inConfiguration = true;

      if (Configure())
      {
        Stop();
        Thread.Sleep(500);
        Start();
      }
      
      _inConfiguration = false;
    }
    void ClickLaunch(object sender, EventArgs e)
    {
      IrssLog.Info("Launch");

      if (_inConfiguration)
      {
        IrssLog.Info("In Configuration");
        return;
      }

      try
      {
        // Check for multiple instances
        foreach (Process process in Process.GetProcesses())
        {
          try
          {
            if (Path.GetFileName(process.MainModule.ModuleName).Equals(Path.GetFileName(_programFile), StringComparison.OrdinalIgnoreCase))
            {
              IrssLog.Info("Program already running.");
              return;
            }
          }
          catch (Win32Exception ex)
          {
            if (ex.ErrorCode != -2147467259) // Ignore "Unable to enumerate the process modules" errors.
              IrssLog.Error(ex);
          }
          catch (Exception ex)
          {
            IrssLog.Error(ex);
          }
        }

        string[] launchCommand = new string[] {
          _programFile,
          Path.GetDirectoryName(_programFile),
          String.Empty,
          Enum.GetName(typeof(ProcessWindowStyle), ProcessWindowStyle.Normal),
          false.ToString(),
          true.ToString(),
          false.ToString(),
          true.ToString()        
        };

        Common.ProcessRunCommand(launchCommand);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(ex.Message, "Tray Launcher", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
    void ClickQuit(object sender, EventArgs e)
    {
      IrssLog.Info("Quit");

      if (_inConfiguration)
      {
        IrssLog.Info("In Configuration");
        return;
      }

      Stop();

      Application.Exit();
    }

    #endregion Implementation

  }

}
