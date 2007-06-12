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

namespace WinLircReceiver
{

  public class WinLircReceiver : IIRServerPlugin
  {

    #region Constants

    static readonly string ConfigurationFile =
      Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
      "\\IR Server Suite\\IR Server\\WinLirc Receiver.xml";

    static readonly string[] Ports  = new string[] { "None" };
    static readonly string[] Speeds = new string[] { "None" };

    #endregion Constants

    #region Variables

    RemoteButtonHandler _remoteButtonHandler = null;
    WinLircServer _server;

    IPAddress _serverIP;
    int _serverPort;
    bool _startServer;
    string _serverPath;
    int _buttonReleaseTime;

    #endregion Variables

    #region IIRServerPlugin Members

    public string Name          { get { return "WinLirc"; } }
    public string Version       { get { return "1.0.3.2"; } }
    public string Author        { get { return "and-81, original code for MediaPortal by Sven"; } }
    public string Description   { get { return "Supports WinLirc as a reciever"; } }
    public bool   CanReceive    { get { return true; } }
    public bool   CanTransmit   { get { return false; } }
    public bool   CanLearn      { get { return false; } }
    public bool   CanConfigure  { get { return true; } }

    public RemoteButtonHandler RemoteButtonCallback
    {
      get { return _remoteButtonHandler; }
      set { _remoteButtonHandler = value; }
    }

    public string[] AvailablePorts  { get { return Ports; }   }
    public string[] AvailableSpeeds { get { return Speeds; }  }

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
    public bool Start()
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
    public void Suspend()   { }
    public void Resume()    { }
    public void Stop()
    {
      _server.CommandEvent -= new WinLircServer.CommandEventHandler(CommandHandler);
    }

    public bool Transmit(string file)
    {
      string password, remoteName, buttonName, repeats;

      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        password    = doc.DocumentElement.Attributes["Password"].Value;
        remoteName  = doc.DocumentElement.Attributes["RemoteName"].Value;
        buttonName  = doc.DocumentElement.Attributes["ButtonName"].Value;
        repeats     = doc.DocumentElement.Attributes["Repeats"].Value;

        string output = String.Format("{0} {1} {2} {3}\n", password, remoteName, buttonName, repeats);
        _server.Transmit(output);

        return true;
      }
      catch
      {
        return false;
      }
    }
    public LearnStatus Learn(string file)
    {
      return LearnStatus.Failure;
    }

    public bool SetPort(string port)    { return true; }
    public bool SetSpeed(string speed)  { return true; }

    #endregion IIRServerPlugin Members

    #region Implementation

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
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());

        _serverIP           = IPAddress.Parse("127.0.0.1");
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
        XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, System.Text.Encoding.UTF8);
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
        writer.Close();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
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
