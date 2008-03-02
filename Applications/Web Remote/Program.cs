using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
#if TRACE
using System.Diagnostics;
#endif
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using System.Xml;

using IrssComms;
using IrssUtils;
using IrssUtils.Forms;

namespace WebRemote
{

  /// <summary>
  /// Web Remote.
  /// </summary>
  static class Program
  {

    #region Constants

    internal const string ButtonClickPrefix = "send?";

    const string DefaultSkin = "MCE";
    const int DefaultWebPort = 2481;

    static readonly string ConfigurationFile = Common.FolderAppData + "Virtual Remote\\Web Remote.xml";

    #endregion Constants

    #region Variables
    
    static NotifyIcon _notifyIcon;

    static bool _inConfiguration;

    static Client _client;

    static bool _registered;
    static string _serverHost;
    static string _installFolder;
    static string _remoteSkin;

    static string _device;

    static RemoteButton[] _buttons;

    static WebServer _webServer;
    static int _webPort;
    //static string _passwordHash;

    static string _imageFile;
    static string _imageMap;
    static string _webFile;

    #endregion Variables

    #region Properties

    internal static string InstallFolder
    {
      get { return _installFolder; }
    }

    internal static string RemoteSkin
    {
      get { return _remoteSkin; }
    }

    internal static string ImageFile
    {
      get { return _imageFile; }
    }

    internal static string ImageMap
    {
      get { return _imageMap; }
    }

    internal static string WebFile
    {
      get { return _webFile; }
    }

    #endregion Properties
    
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      // Check for multiple instances.
      if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length != 1)
        return;

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

#if DEBUG
      IrssLog.LogLevel = IrssLog.Level.Debug;
#else
      IrssLog.LogLevel = IrssLog.Level.Info;
#endif
      IrssLog.Open(Common.FolderIrssLogs + "Web Remote.log");

      Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

      LoadSettings();
      
      if (String.IsNullOrEmpty(_serverHost))
      {
        ServerAddress serverAddress = new ServerAddress();
        serverAddress.ShowDialog();

        _serverHost = serverAddress.ServerHost;
      }

      bool clientStarted = false;

      IPAddress serverIP = Client.GetIPFromName(_serverHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, IrssComms.Server.DefaultPort);

      try
      {
        clientStarted = StartClient(endPoint);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        clientStarted = false;
      }

      if (clientStarted)
      {
        ContextMenuStrip contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add(new ToolStripMenuItem("&Setup", null, new EventHandler(ClickSetup)));
        contextMenu.Items.Add(new ToolStripMenuItem("&Quit", null, new EventHandler(ClickQuit)));

        _notifyIcon = new NotifyIcon();
        _notifyIcon.ContextMenuStrip = contextMenu;
        _notifyIcon.DoubleClick += new EventHandler(ClickSetup);
        _notifyIcon.Icon = Properties.Resources.Icon;
        _notifyIcon.Text = "Web Remote";

        ChangeSkin();

        try
        {
          _webServer = new WebServer(_webPort);
          _webServer.Run();
        }
        catch (Exception ex)
        {
          IrssLog.Error(ex);
        }

        _notifyIcon.Visible = true;

        Application.Run();

        _notifyIcon.Visible = false;

        if (_webServer != null)
          _webServer.Stop();
      }

      SaveSettings();

      StopClient();

      Application.ThreadException -= new ThreadExceptionEventHandler(Application_ThreadException);

      IrssLog.Close();
    }

    /// <summary>
    /// Handles unhandled exceptions.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">Event args.</param>
    static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
    {
      IrssLog.Error(e.Exception);
    }

    static bool Configure()
    {
      Setup setup = new Setup();

      setup.ServerHost  = _serverHost;
      setup.RemoteSkin  = _remoteSkin;
      setup.WebPort     = _webPort;

      if (setup.ShowDialog() == DialogResult.OK)
      {
        _serverHost = setup.ServerHost;
        _remoteSkin = setup.RemoteSkin;
        _webPort    = setup.WebPort;

        //_passwordHash = setup.PasswordHash;

        SaveSettings();

        return true;
      }

      return false;
    }

    static void ClickSetup(object sender, EventArgs e)
    {
      IrssLog.Info("Setup");

      _inConfiguration = true;

      if (_webServer != null)
      {
        _webServer.Stop();
        _webServer = null;
      }

      if (Configure())
        ChangeSkin();

      try
      {
        _webServer = new WebServer(_webPort);
        _webServer.Run();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(ex.Message, "Web Remote - Setup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      _inConfiguration = false;
    }
    static void ClickQuit(object sender, EventArgs e)
    {
      IrssLog.Info("Quit");

      if (_inConfiguration)
      {
        IrssLog.Info("In Configuration");
        return;
      }

      Application.Exit();
    }

    internal static void ButtonPress(string keyCode)
    {
#if TRACE
      Trace.WriteLine("ButtonPress: " + keyCode);
#endif
      if (!_registered)
        return;

      byte[] deviceNameBytes = Encoding.ASCII.GetBytes(_device);
      byte[] keyCodeBytes = Encoding.ASCII.GetBytes(keyCode);

      byte[] bytes = new byte[8 + deviceNameBytes.Length + keyCodeBytes.Length];

      BitConverter.GetBytes(deviceNameBytes.Length).CopyTo(bytes, 0);
      deviceNameBytes.CopyTo(bytes, 4);
      BitConverter.GetBytes(keyCodeBytes.Length).CopyTo(bytes, 4 + deviceNameBytes.Length);
      keyCodeBytes.CopyTo(bytes, 8 + deviceNameBytes.Length);

      IrssMessage message = new IrssMessage(MessageType.ForwardRemoteEvent, MessageFlags.Notify, bytes);
      SendMessage(message);
    }

    static void ChangeSkin()
    {
      SetSkin(_remoteSkin);

      _imageMap = CreateImageMap();
      _webFile = String.Format("{0}\\Skins\\web.html", _installFolder);
    }

    static void SetSkin(string skin)
    {
      try
      {
        if (String.IsNullOrEmpty(skin))
          return;

        _imageFile = String.Format("{0}\\Skins\\{1}.png", _installFolder, skin);
        if (!File.Exists(_imageFile))
          throw new FileNotFoundException("Skin graphic file not found", _imageFile);

        // Try to load xml file of same name, failing that load using first word of skin name ...
        string xmlFile = String.Format("{0}\\Skins\\{1}.xml", _installFolder, skin);
        if (!File.Exists(xmlFile))
        {
          string firstWord = skin.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];

          xmlFile = String.Format("{0}\\Skins\\{1}.xml", _installFolder, firstWord);

          if (!File.Exists(xmlFile))
            throw new FileNotFoundException("Skin file not found", xmlFile);
        }

        _device = Path.GetFileNameWithoutExtension(xmlFile);
        LoadSkinXml(xmlFile);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Web Remote - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
    static void LoadSkinXml(string xmlFile)
    {
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
          temp.Shortcut = (Keys)Enum.Parse(typeof(Keys), key, true);
        }
        catch (ArgumentException)
        {
          IrssLog.Error("Invalid Key Shortcut \"{0}\" in skin \"{1}\"", key, xmlFile);
        }

        temp.Top = int.Parse(item.Attributes["top"].Value);
        temp.Left = int.Parse(item.Attributes["left"].Value);
        temp.Height = int.Parse(item.Attributes["height"].Value);
        temp.Width = int.Parse(item.Attributes["width"].Value);
        buttons.Add(temp);
      }

      _buttons = (RemoteButton[])buttons.ToArray(typeof(RemoteButton));
    }

    static string CreateImageMap()
    {
      StringBuilder imageMap = new StringBuilder();
      imageMap.AppendLine("<MAP NAME=\"REMOTE_MAP\">");

      foreach (RemoteButton button in _buttons)
      {
        string area = String.Format(
          "<AREA SHAPE=\"rect\" COORDS=\"{0},{1},{2},{3}\" TITLE=\"{4}\" HREF=\"#\" ONCLICK=\"SendMessage('{5}{6}');return false;\">",
          button.Left, button.Top, button.Left + button.Width, button.Top + button.Height,
          button.Name,
          ButtonClickPrefix, button.Code);

        imageMap.AppendLine(area);
      }

      imageMap.AppendLine("<AREA SHAPE=\"default\" NOHREF>");
      imageMap.AppendLine("</MAP>");

      return imageMap.ToString();
    }

    static void LoadSettings()
    {
      try
      {
        _installFolder = SystemRegistry.GetInstallFolder();
        if (String.IsNullOrEmpty(_installFolder))
          _installFolder = ".";
        else
          _installFolder += "\\Virtual Remote";
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);

        _installFolder = ".";
      }

      XmlDocument doc = new XmlDocument();

      try
      {
        doc.Load(ConfigurationFile);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);

        _serverHost = "localhost";
        _remoteSkin = DefaultSkin;
        _webPort = DefaultWebPort;
        //_passwordHash = null;

        return;
      }

      try { _serverHost = doc.DocumentElement.Attributes["ServerHost"].Value; } catch { _serverHost = "localhost"; }
      try { _remoteSkin = doc.DocumentElement.Attributes["RemoteSkin"].Value; } catch { _remoteSkin = DefaultSkin; }
      try { _webPort = int.Parse(doc.DocumentElement.Attributes["WebPort"].Value); } catch { _webPort = DefaultWebPort; }
      //try { _passwordHash = doc.DocumentElement.Attributes["PasswordHash"].Value; } catch { _passwordHash = null; }
    }
    static void SaveSettings()
    {
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
          writer.WriteAttributeString("RemoteSkin", _remoteSkin);
          writer.WriteAttributeString("WebPort", _webPort.ToString());
          //writer.WriteAttributeString("PasswordHash", _passwordHash);

          writer.WriteEndElement(); // </settings>
          writer.WriteEndDocument();
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
    }

    static void CommsFailure(object obj)
    {
      Exception ex = obj as Exception;
      
      if (ex != null)
        IrssLog.Error("Communications failure: {0}", ex.Message);
      else
        IrssLog.Error("Communications failure");

      StopClient();

      MessageBox.Show("Please report this error.", "Virtual Remote - Communications failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    static void Connected(object obj)
    {
      IrssLog.Info("Connected to server");

      IrssMessage message = new IrssMessage(MessageType.RegisterClient, MessageFlags.Request);
      _client.Send(message);
    }
    static void Disconnected(object obj)
    {
      IrssLog.Warn("Communications with server has been lost");

      Thread.Sleep(1000);
    }

    static bool StartClient(IPEndPoint endPoint)
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
    static void StopClient()
    {
      if (_client == null)
        return;

      _client.Dispose();
      _client = null;

      _registered = false;
    }

    static void SendMessage(IrssMessage message)
    {
      if (message == null)
        throw new ArgumentNullException("message");

      if (_client != null)
        _client.Send(message);
    }

    static void ReceivedMessage(IrssMessage received)
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

          case MessageType.ServerShutdown:
            IrssLog.Warn("Input Service Shutdown - Web Remote disabled until Input Service returns");
            _registered = false;
            break;

          case MessageType.Error:
            IrssLog.Error(received.GetDataAsString());
            break;
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
    }

  }

}
