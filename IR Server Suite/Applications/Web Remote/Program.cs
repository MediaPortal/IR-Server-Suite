#region Copyright (C) 2005-2009 Team MediaPortal

// Copyright (C) 2005-2009 Team MediaPortal
// http://www.team-mediaportal.com
// 
// This Program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2, or (at your option)
// any later version.
// 
// This Program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with GNU Make; see the file COPYING.  If not, write to
// the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
// http://www.gnu.org/copyleft/gpl.html

#endregion

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using IrssComms;
using IrssUtils;
using IrssUtils.Forms;
using WebRemote.Properties;

namespace WebRemote
{
  /// <summary>
  /// Web Remote.
  /// </summary>
  internal static class Program
  {
    #region Constants

    internal const string ButtonClickPrefix = "send?";

    private const string DefaultSkin = "MCE";
    private const int DefaultWebPort = 2481;

    private static readonly string ConfigurationFolder = Path.Combine(Common.FolderAppData,
                                                                    "Virtual Remote");
    private static readonly string ConfigurationFile = Path.Combine(ConfigurationFolder,
                                                                    "Web Remote.xml");

    #endregion Constants

    #region Variables

    private static RemoteButton[] _buttons;
    private static Client _client;
    private static Container _container;
    private static string _device;
    private static string _imageFile;
    private static string _imageMap;

    private static bool _inConfiguration;
    private static NotifyIcon _notifyIcon;

    private static bool _registered;
    private static string _remoteSkin;
    private static string _serverHost;
    private static string _skinsFolder;
    private static string _webFile;
    private static int _webPort;
    private static WebServer _webServer;

    #endregion Variables

    #region Properties

    internal static string SkinsFolder
    {
      get { return _skinsFolder; }
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
    private static void Main()
    {
      // Check for multiple instances.
      if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length != 1)
        return;

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      IrssLog.LogLevel = IrssLog.Level.Debug;
      IrssLog.Open("Web Remote.log");

      Application.ThreadException += Application_ThreadException;

      LoadSettings();

      if (String.IsNullOrEmpty(_serverHost))
      {
        ServerAddress serverAddress = new ServerAddress();
        serverAddress.ShowDialog();

        _serverHost = serverAddress.ServerHost;
      }

      bool clientStarted = false;

      IPAddress serverIP = Network.GetIPFromName(_serverHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, Server.DefaultPort);

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

        _container = new Container();
        _notifyIcon = new NotifyIcon(_container);
        _notifyIcon.ContextMenuStrip = contextMenu;
        _notifyIcon.DoubleClick += ClickSetup;
        _notifyIcon.Icon = Resources.Icon;
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

      Application.ThreadException -= Application_ThreadException;

      IrssLog.Close();
    }

    /// <summary>
    /// Handles unhandled exceptions.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">Event args.</param>
    private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
    {
      IrssLog.Error(e.Exception);
    }

    private static bool Configure()
    {
      Setup setup = new Setup();

      setup.ServerHost = _serverHost;
      setup.RemoteSkin = _remoteSkin;
      setup.WebPort = _webPort;

      if (setup.ShowDialog() == DialogResult.OK)
      {
        _serverHost = setup.ServerHost;
        _remoteSkin = setup.RemoteSkin;
        _webPort = setup.WebPort;

        //_passwordHash = setup.PasswordHash;

        SaveSettings();

        return true;
      }

      return false;
    }

    private static void ClickSetup(object sender, EventArgs e)
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

    private static void ClickQuit(object sender, EventArgs e)
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

      byte[] bytes = IrssMessage.EncodeRemoteEventData(_device, keyCode);

      IrssMessage message = new IrssMessage(MessageType.ForwardRemoteEvent, MessageFlags.Notify, bytes);
      SendMessage(message);
    }

    private static void ChangeSkin()
    {
      SetSkin(_remoteSkin);

      _imageMap = CreateImageMap();
      _webFile = Path.Combine(_skinsFolder, "web.html");
    }

    private static void SetSkin(string skin)
    {
      try
      {
        if (String.IsNullOrEmpty(skin))
          return;

        _imageFile = Path.Combine(_skinsFolder, skin + ".png");
        if (!File.Exists(_imageFile))
          throw new FileNotFoundException("Skin graphic file not found", _imageFile);

        // Try to load xml file of same name, failing that load using first word of skin name ...
        string xmlFile = Path.Combine(_skinsFolder, skin + ".xml");
        if (!File.Exists(xmlFile))
        {
          string firstWord = skin.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries)[0];

          xmlFile = Path.Combine(_skinsFolder, firstWord + ".xml");

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

    private static void LoadSkinXml(string xmlFile)
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
          temp.Shortcut = (Keys) Enum.Parse(typeof (Keys), key, true);
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

      _buttons = (RemoteButton[]) buttons.ToArray(typeof (RemoteButton));
    }

    private static string CreateImageMap()
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

    private static void LoadSettings()
    {
      try
      {
        _skinsFolder = SystemRegistry.GetInstallFolder();
        if (String.IsNullOrEmpty(_skinsFolder))
          _skinsFolder = ".\\Skins";
        else
          _skinsFolder = Path.Combine(_skinsFolder, "Virtual Remote\\Skins");
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);

        _skinsFolder = ".\\Skins";
      }

      XmlDocument doc = new XmlDocument();

      try
      {
        doc.Load(ConfigurationFile);
      }
      catch (DirectoryNotFoundException)
      {
        IrssLog.Warn("Configuration directory not found, using default settings");

        Directory.CreateDirectory(ConfigurationFolder);
        CreateDefaultSettings();
        return;
      }
      catch (FileNotFoundException)
      {
        IrssLog.Warn("Configuration file not found, using default settings");

        CreateDefaultSettings();
        return;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);

        CreateDefaultSettings();
        return;
      }

      try
      {
        _serverHost = doc.DocumentElement.Attributes["ServerHost"].Value;
      }
      catch
      {
        _serverHost = "localhost";
      }
      try
      {
        _remoteSkin = doc.DocumentElement.Attributes["RemoteSkin"].Value;
      }
      catch
      {
        _remoteSkin = DefaultSkin;
      }
      try
      {
        _webPort = int.Parse(doc.DocumentElement.Attributes["WebPort"].Value);
      }
      catch
      {
        _webPort = DefaultWebPort;
      }
      //try { _passwordHash = doc.DocumentElement.Attributes["PasswordHash"].Value; } catch { _passwordHash = null; }
    }

    private static void SaveSettings()
    {
      try
      {
        using (XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, Encoding.UTF8))
        {
          writer.Formatting = Formatting.Indented;
          writer.Indentation = 1;
          writer.IndentChar = (char) 9;
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

    private static void CreateDefaultSettings()
    {
      _serverHost = "localhost";
      _remoteSkin = DefaultSkin;
      _webPort = DefaultWebPort;
      //_passwordHash = null;

      SaveSettings();
    }

    private static void CommsFailure(object obj)
    {
      Exception ex = obj as Exception;

      if (ex != null)
        IrssLog.Error("Communications failure: {0}", ex.Message);
      else
        IrssLog.Error("Communications failure");

      StopClient();

      MessageBox.Show("Please report this error.", "Virtual Remote - Communications failure", MessageBoxButtons.OK,
                      MessageBoxIcon.Error);
    }

    private static void Connected(object obj)
    {
      IrssLog.Info("Connected to server");

      IrssMessage message = new IrssMessage(MessageType.RegisterClient, MessageFlags.Request);
      _client.Send(message);
    }

    private static void Disconnected(object obj)
    {
      IrssLog.Warn("Communications with server has been lost");

      Thread.Sleep(1000);
    }

    private static bool StartClient(IPEndPoint endPoint)
    {
      if (_client != null)
        return false;

      ClientMessageSink sink = ReceivedMessage;

      _client = new Client(endPoint, sink);
      _client.CommsFailureCallback = CommsFailure;
      _client.ConnectCallback = Connected;
      _client.DisconnectCallback = Disconnected;

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

    private static void StopClient()
    {
      if (_client == null)
        return;

      _client.Dispose();
      _client = null;

      _registered = false;
    }

    private static void SendMessage(IrssMessage message)
    {
      if (message == null)
        throw new ArgumentNullException("message");

      if (_client != null)
        _client.Send(message);
    }

    private static void ReceivedMessage(IrssMessage received)
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

              IrssLog.Info("Registered to IR Server");
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              _registered = false;
              IrssLog.Warn("IR Server refused to register");
            }
            break;

          case MessageType.ServerShutdown:
            IrssLog.Warn("IR Server Shutdown - Web Remote disabled until IR Server returns");
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