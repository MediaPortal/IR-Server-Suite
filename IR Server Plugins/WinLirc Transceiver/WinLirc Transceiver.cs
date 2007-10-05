using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Xml;

using IRServerPluginInterface;

namespace WinLircTransceiver
{

  public class WinLircTransceiver : IRServerPluginBase, IConfigure, IRemoteReceiver, ITransmitIR
  {

    #region Constants

    static readonly string ConfigurationFile =
      Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
      "\\IR Server Suite\\IR Server\\WinLirc Receiver.xml";

    #endregion Constants

    #region Variables

    RemoteHandler _remoteButtonHandler = null;
    WinLircServer _server;

    IPAddress _serverIP;
    int _serverPort;
    bool _startServer;
    string _serverPath;
    int _buttonReleaseTime;

    #endregion Variables

    #region Implementation

    public override string Name         { get { return "WinLirc"; } }
    public override string Version      { get { return "1.0.3.4"; } }
    public override string Author       { get { return "and-81, original code for MediaPortal by Sven"; } }
    public override string Description  { get { return "Supports WinLirc as a Transciever"; } }

    public override bool Detect()
    {
      try
      {
        return WinLircServer.IsServerRunning();
      }
      catch
      {
        return false;
      }
    }

    public override bool Start()
    {
      LoadSettings();

      if (_startServer)
      {
        if (!WinLircServer.StartServer(_serverPath))
          return false;
      }

      _server = new WinLircServer(_serverIP, _serverPort, TimeSpan.FromMilliseconds(_buttonReleaseTime));
      _server.CommandEvent += new WinLircServer.CommandEventHandler(CommandHandler);

      return true;
    }
    public override void Suspend()
    {
      Stop();
    }
    public override void Resume()
    {
      Start();
    }
    public override void Stop()
    {
      if (_server != null)
      {
        _server.CommandEvent -= new WinLircServer.CommandEventHandler(CommandHandler);
        _server = null;
      }
    }

    public RemoteHandler RemoteCallback
    {
      get { return _remoteButtonHandler; }
      set { _remoteButtonHandler = value; }
    }

    public void Configure()
    {
      LoadSettings();

      Configure config = new Configure();

      config.ServerIP           = _serverIP;
      config.ServerPort         = _serverPort;
      config.StartServer        = _startServer;
      config.ServerPath         = _serverPath;
      config.ButtonReleaseTime  = _buttonReleaseTime;

      if (config.ShowDialog() == DialogResult.OK)
      {
        _serverIP           = config.ServerIP;
        _serverPort         = config.ServerPort;
        _startServer        = config.StartServer;
        _serverPath         = config.ServerPath;
        _buttonReleaseTime  = config.ButtonReleaseTime;

        SaveSettings();
      }
    }

    public string[] AvailablePorts { get { return new string[] { "Default" }; } }

    public bool Transmit(string port, byte[] data)
    {
      string password, remoteName, buttonName, repeats;

      using (MemoryStream memoryStream = new MemoryStream(data))
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(memoryStream);

        password = doc.DocumentElement.Attributes["Password"].Value;
        remoteName = doc.DocumentElement.Attributes["RemoteName"].Value;
        buttonName = doc.DocumentElement.Attributes["ButtonName"].Value;
        repeats = doc.DocumentElement.Attributes["Repeats"].Value;

        string output = String.Format("{0} {1} {2} {3}\n", password, remoteName, buttonName, repeats);
        _server.Transmit(output);
      }

      return true;
    }

    void LoadSettings()
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationFile);

        _serverIP           = IPAddress.Parse(doc.DocumentElement.Attributes["ServerIP"].Value);
        _serverPort         = int.Parse(doc.DocumentElement.Attributes["ServerPort"].Value);
        _startServer        = bool.Parse(doc.DocumentElement.Attributes["StartServer"].Value);
        _serverPath         = doc.DocumentElement.Attributes["ServerPath"].Value;
        _buttonReleaseTime  = int.Parse(doc.DocumentElement.Attributes["ButtonReleaseTime"].Value);
      }
#if TRACE
      catch (Exception ex)
      {
        Trace.WriteLine(ex.ToString());
#else
      catch
      {
#endif

        _serverIP           = IPAddress.Loopback;
        _serverPort         = 8765;
        _startServer        = false;
        _serverPath         = "winlirc.exe";
        _buttonReleaseTime  = 200;
      }
    }
    void SaveSettings()
    {
      try
      {
        using (XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, System.Text.Encoding.UTF8))
        {
          writer.Formatting = Formatting.Indented;
          writer.Indentation = 1;
          writer.IndentChar = (char)9;
          writer.WriteStartDocument(true);
          writer.WriteStartElement("settings"); // <settings>

          writer.WriteAttributeString("ServerIP", _serverIP.ToString());
          writer.WriteAttributeString("ServerPort", _serverPort.ToString());
          writer.WriteAttributeString("StartServer", _startServer.ToString());
          writer.WriteAttributeString("ServerPath", _serverPath);
          writer.WriteAttributeString("ButtonReleaseTime", _buttonReleaseTime.ToString());

          writer.WriteEndElement(); // </settings>
          writer.WriteEndDocument();
        }
      }
#if TRACE
      catch (Exception ex)
      {
        Trace.WriteLine(ex.ToString());
      }
#else
      catch
      {
      }
#endif
    }

    void CommandHandler(WinLircServer.Command cmd)
    {
      if (_remoteButtonHandler == null)
        return;

      string buttonCode = cmd.Remote + ": " + cmd.Button;

      _remoteButtonHandler(buttonCode);
    }
    
    #endregion Implementation

  }

}
