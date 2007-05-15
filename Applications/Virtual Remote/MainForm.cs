using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Win32;

using NamedPipes;
using IrssUtils;

namespace VirtualRemote
{

  public partial class MainForm : Form
  {

    #region Enumerations

    /// <summary>
    /// A list of MCE remote buttons
    /// </summary>
    public enum MceButton
    {
      Custom        = -1,
      None          = 0,
      TV_Power      = 0x7b9a,
      Blue          = 0x7ba1,
      Yellow        = 0x7ba2,
      Green         = 0x7ba3,
      Red           = 0x7ba4,
      Teletext      = 0x7ba5,
      Radio         = 0x7baf,
      Print         = 0x7bb1,
      Videos        = 0x7bb5,
      Pictures      = 0x7bb6,
      Recorded_TV   = 0x7bb7,
      Music         = 0x7bb8,
      TV            = 0x7bb9,
      Guide         = 0x7bd9,
      Live_TV       = 0x7bda,
      DVD_Menu      = 0x7bdb,
      Back          = 0x7bdc,
      OK            = 0x7bdd,
      Right         = 0x7bde,
      Left          = 0x7bdf,
      Down          = 0x7be0,
      Up            = 0x7be1,
      Star          = 0x7be2,
      Hash          = 0x7be3,
      Replay        = 0x7be4,
      Skip          = 0x7be5,
      Stop          = 0x7be6,
      Pause         = 0x7be7,
      Record        = 0x7be8,
      Play          = 0x7be9,
      Rewind        = 0x7bea,
      Forward       = 0x7beb,
      Channel_Down  = 0x7bec,
      Channel_Up    = 0x7bed,
      Volume_Down   = 0x7bee,
      Volume_Up     = 0x7bef,
      Info          = 0x7bf0,
      Mute          = 0x7bf1,
      Start         = 0x7bf2,
      PC_Power      = 0x7bf3,
      Enter         = 0x7bf4,
      Escape        = 0x7bf5,
      Number_9      = 0x7bf6,
      Number_8      = 0x7bf7,
      Number_7      = 0x7bf8,
      Number_6      = 0x7bf9,
      Number_5      = 0x7bfa,
      Number_4      = 0x7bfb,
      Number_3      = 0x7bfc,
      Number_2      = 0x7bfd,
      Number_1      = 0x7bfe,
      Number_0      = 0x7bff,
    }

    #endregion Enumerations

    #region Constants

    const string DefaultSkin          = "MCE";
    const string DefaultIntallFolder  = ".";

    #endregion Constants

    #region Variables

    bool _registered = false;
    bool _keepAlive = true;
    int _echoID = -1;
    Thread _keepAliveThread;

    string _serverHost;
    string _localPipeName;
    string _lastKeyCode = String.Empty;
    string _remoteSkin;

    string _installFolder;

    RemoteButton[] _buttons = null;

    #endregion Variables

    #region Constructor

    public MainForm()
    {
      InitializeComponent();

      _setLabel = new DelegateSetLabel(SetLabel);
    }

    #endregion Constructor

    private void MainForm_Load(object sender, EventArgs e)
    {
      IrssLog.LogLevel = IrssLog.Level.Debug;
      IrssLog.Open(Common.FolderIrssLogs + "Virtual Remote.log");

      IrssLog.Debug("Platform is {0}", (IntPtr.Size == 4 ? "32-bit" : "64-bit"));

      LoadSettings();

      IrssUtils.Forms.ServerAddress serverAddress;

      if (String.IsNullOrEmpty(_serverHost))
      {
        serverAddress = new IrssUtils.Forms.ServerAddress(Environment.MachineName);
        serverAddress.ShowDialog(this);
        _serverHost = serverAddress.ServerHost;
      }

      if (StartComms())
      {
        SetSkinList();
        SetSkin(_remoteSkin);
      }
      else
      {
        Application.Exit();
      }
    }
    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      SaveSettings();

      StopComms();

      IrssLog.Close();
    }
    private void MainForm_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Left)
        return;

      if (_buttons == null)
        return;

      foreach (RemoteButton button in _buttons)
      {
        if (e.Y >= button.Top &&
            e.Y < button.Top + button.Height &&
            e.X >= button.Left &&
            e.X <= button.Left + button.Width)
        {
          ButtonPress(button.Code);
          break;
        }
      }
    }
    private void MainForm_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.None)
        return;

      foreach (RemoteButton button in _buttons)
      {
        if (button.Shortcut == e.KeyCode)
        {
          ButtonPress(button.Code);
          break;
        }
      }
    }

    void LoadSettings()
    {
      try
      {
        RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\and-81\VirtualRemote");
        _remoteSkin = (string)key.GetValue("Skin", DefaultSkin);
        _serverHost = (string)key.GetValue("ServerHost", String.Empty);
        key.Close();

        key = Registry.LocalMachine.OpenSubKey("Software\\IR Server Suite\\");
        _installFolder = (string)key.GetValue("Install_Dir", DefaultIntallFolder);
        key.Close();

        if (_installFolder != DefaultIntallFolder)
          _installFolder += "\\Virtual Remote";
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
        _remoteSkin     = DefaultSkin;
        _serverHost     = String.Empty;
        _installFolder  = DefaultIntallFolder;
      }
    }
    void SaveSettings()
    {
      try
      {
        RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\and-81\VirtualRemote");
        key.SetValue("Skin", _remoteSkin);
        key.SetValue("ServerHost", _serverHost);
        key.Close();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
      }
    }

    bool StartComms()
    {
      try
      {
        if (OpenLocalPipe())
        {
          _keepAliveThread = new Thread(new ThreadStart(KeepAliveThread));
          _keepAliveThread.Start();
          return true;
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
      }

      return false;
    }

    void StopComms()
    {
      _keepAlive = false;

      try
      {
        if (_keepAliveThread != null)
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

    bool LoadSkinXml(string xmlFile)
    {
      if (!File.Exists(xmlFile))
        return false;

      XmlDocument doc = new XmlDocument();
      doc.Load(xmlFile);

      ArrayList buttons = new ArrayList();
      RemoteButton temp;
      string key = String.Empty;

      XmlNodeList commandSequence = doc.DocumentElement.SelectNodes("button");
      foreach (XmlNode item in commandSequence)
      {
        temp = new RemoteButton();
        temp.Name = item.Attributes["name"].Value;
        temp.Code = item.Attributes["code"].Value;

        try
        {
          key = item.Attributes["shortcut"].Value;
          temp.Shortcut = (Keys)Enum.Parse(typeof(Keys), key);
        }
        catch (ArgumentException)
        {
          IrssLog.Error("Invalid Key Shortcut \"{0}\" in skin \"{1}\"", key, xmlFile);
        }

        temp.Top    = int.Parse(item.Attributes["top"].Value);
        temp.Left   = int.Parse(item.Attributes["left"].Value);
        temp.Height = int.Parse(item.Attributes["height"].Value);
        temp.Width  = int.Parse(item.Attributes["width"].Value);
        buttons.Add(temp);
      }

      _buttons = (RemoteButton[])buttons.ToArray(typeof(RemoteButton));

      return true;
    }

    void SetSkinList()
    {
      try
      {
        string[] skins = Directory.GetFiles(_installFolder + "\\Skins\\", "*.png", SearchOption.TopDirectoryOnly);
        for (int index = 0; index < skins.Length; index++)
          skins[index] = Path.GetFileNameWithoutExtension(skins[index]);

        toolStripComboBoxSkin.Items.Clear();
        toolStripComboBoxSkin.Items.AddRange(skins);

        toolStripComboBoxSkin.SelectedItem = _remoteSkin;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.Message);
      }
    }

    void SetSkin(string skin)
    {
      try
      {
        if (String.IsNullOrEmpty(skin))
          return;

        string skinFile = String.Format("{0}\\Skins\\{1}.png", _installFolder, skin);
        if (!File.Exists(skinFile))
          throw new FileNotFoundException("Skin graphic file not found", skin);

        // Try to load xml file of same name, failing that load using first word of skin name ...
        string xmlFile = String.Format("{0}\\Skins\\{1}.xml", _installFolder, skin);
        if (!File.Exists(xmlFile))
        {
          string firstWord = skin.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];

          xmlFile = String.Format("{0}\\Skins\\{1}.xml", _installFolder, firstWord);

          if (!File.Exists(xmlFile))
            throw new FileNotFoundException("Skin file not found", xmlFile);
        }

        if (LoadSkinXml(xmlFile))
        {
          this.BackgroundImage = new Bitmap(skinFile);
          this.ClientSize = new System.Drawing.Size(this.BackgroundImage.Width, this.BackgroundImage.Height);
          _remoteSkin = skin;
        }
        else
          throw new Exception(String.Format("Failed to load skin file not found \"{0}\"", xmlFile));
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Virtual Remote - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    void ButtonPress(string keyCode)
    {
      if (!PipeAccess.ServerRunning)
        return;

      PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Forward Remote Button", Encoding.ASCII.GetBytes(keyCode));
      PipeAccess.SendMessage(Common.ServerPipeName, _serverHost, message.ToString());
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

      _registered = false;
      _keepAlive = true;
      while (_keepAlive)
      {
        reconnect = true;

        this.Invoke(_setLabel, new Object[] { true });

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

          this.Invoke(_setLabel, new Object[] { false });
        }

        #endregion Registered ...

        #region Ping the server repeatedly

        while (_keepAlive && _registered && !reconnect)
        {
          IrssLog.Info("Ping ({0})", _serverHost);

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
              Thread.SpinWait(1000);
            }
          }

          if (receivedEcho) // Received ping echo ...
          {
            IrssLog.Info("Echo received");

            // Wait 60 seconds before re-pinging ...
            for (int sleeps = 0; sleeps < 60 && _keepAlive && _registered; sleeps++)
              Thread.Sleep(1000);
          }
          else // Didn't receive ping echo ...
          {
            IrssLog.Warn("No echo, attempting to reconnect ...");

            // Break out of pinging cycle ...
            _registered = false;
            reconnect = true;
          }
        }

        #endregion Ping the server repeatedly

      }

    }

    delegate void DelegateSetLabel(bool visible);
    DelegateSetLabel _setLabel = null;

    void SetLabel(bool visible)
    {
      labelDisabled.Visible = visible;
    }

    void ReceivedMessage(string message)
    {
      PipeMessage received = PipeMessage.FromString(message);

      IrssLog.Debug("Received Message \"{0}\"", received.Name);

      try
      {
        switch (received.Name)
        {
          case "Remote Button":
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
              IrssLog.Error(Encoding.ASCII.GetString(received.Data));
              break;
            }

          default:
            {
              IrssLog.Warn("Unknown message received from server: " + received.Name);
              break;
            }
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
      }
    }

    private void toolStripMenuItemQuit_Click(object sender, EventArgs e)
    {
      IrssLog.Info("User quit");

      this.Close();
    }

    private void toolStripComboBoxSkin_SelectedIndexChanged(object sender, EventArgs e)
    {
      SetSkin(toolStripComboBoxSkin.SelectedItem as string);
      contextMenuStrip.Hide();
    }

    private void changeServerHostToolStripMenuItem_Click(object sender, EventArgs e)
    {
      StopComms();

      IrssUtils.Forms.ServerAddress serverAddress = new IrssUtils.Forms.ServerAddress(_serverHost);
      serverAddress.ShowDialog(this);
      _serverHost = serverAddress.ServerHost;

      StartComms();
    }

  }

}
