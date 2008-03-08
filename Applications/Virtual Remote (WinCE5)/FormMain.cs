using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace VirtualRemote
{

  /// <summary>
  /// Main Form for Virtual Remote.
  /// </summary>
  partial class FormMain : Form
  {

    #region Constants

    const string ConfigurationFile = "VirtualRemote.xml";

    const int ServerPort = 24000;

    #endregion Constants

    #region Variables

    Client _client;

    string _serverHost = "192.168.0.3";

    bool _registered;

    #endregion Variables

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="FormMain"/> class.
    /// </summary>
    public FormMain()
    {
      InitializeComponent();

      _Notification = new DelegateNotifcation(Notification);

      LoadSettings();
      textBoxComputer.Text = _serverHost;
    }

    #endregion Constructor

    delegate void DelegateNotifcation(string text);
    DelegateNotifcation _Notification;

    void Notification(string text)
    {
      textBoxStatus.Text = text;
    }

    private void FormMain_Load(object sender, EventArgs e)
    {
      IPAddress serverIP = IPAddress.Parse(_serverHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, ServerPort);

      StartClient(endPoint);
    }

    void ReceivedMessage(IrssMessage received)
    {
      switch (received.Type)
      {
        case MessageType.RegisterClient:
          if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
          {
            _registered = true;
            this.Invoke(_Notification, "Connected");
          }
          else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
          {
            _registered = false;
            this.Invoke(_Notification, "Failed to negotiate connection");
          }
          return;

        case MessageType.ServerShutdown:
          _registered = false;
          this.Invoke(_Notification, "Server Shutdown");
          return;

        case MessageType.Error:
          this.Invoke(_Notification, received.GetDataAsString());
          return;
      }
    }

    void CommsFailure(object obj)
    {
      StopClient();

      Exception ex = obj as Exception;

      if (ex != null)
        this.Invoke(_Notification, ex.Message);
      else
        this.Invoke(_Notification, "Communications failure");
    }
    void Connected(object obj)
    {
      IrssMessage message = new IrssMessage(MessageType.RegisterClient, MessageFlags.Request);
      _client.Send(message);
    }
    void Disconnected(object obj)
    {
      this.Invoke(_Notification, "Communications with server has been lost");

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

    void ButtonPress(string keyCode)
    {
      if (!_registered)
        return;

      byte[] deviceNameBytes = Encoding.ASCII.GetBytes("Abstract");
      byte[] keyCodeBytes = Encoding.ASCII.GetBytes(keyCode);

      byte[] bytes = new byte[8 + deviceNameBytes.Length + keyCodeBytes.Length];

      BitConverter.GetBytes(deviceNameBytes.Length).CopyTo(bytes, 0);
      deviceNameBytes.CopyTo(bytes, 4);
      BitConverter.GetBytes(keyCodeBytes.Length).CopyTo(bytes, 4 + deviceNameBytes.Length);
      keyCodeBytes.CopyTo(bytes, 8 + deviceNameBytes.Length);

      IrssMessage message = new IrssMessage(MessageType.ForwardRemoteEvent, MessageFlags.Notify, bytes);
      _client.Send(message);
    }

    void Quit()
    {
      StopClient();
      Application.Exit();
    }

    void LoadSettings()
    {
      XmlDocument doc = new XmlDocument();

      try
      {
        doc.Load(ConfigurationFile);
      }
      catch
      {
        _serverHost = "192.168.0.1";
        return;
      }

      try { _serverHost = doc.DocumentElement.Attributes["ServerHost"].Value; }
      catch { _serverHost = "192.168.0.1"; }
    }
    void SaveSettings()
    {
      using (XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, Encoding.UTF8))
      {
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 1;
        writer.IndentChar = (char)9;
        writer.WriteStartDocument(true);
        writer.WriteStartElement("settings"); // <settings>

        writer.WriteAttributeString("ServerHost", _serverHost);

        writer.WriteEndElement(); // </settings>
        writer.WriteEndDocument();
      }
    }

    private void button_Click(object sender, EventArgs e)
    {
      Button origin = sender as Button;

      if (origin != null)
        ButtonPress(origin.Tag as string);
    }

    private void FormMain_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.KeyCode)
      {
        case Keys.Up:     ButtonPress("Up");    break;
        case Keys.Down:   ButtonPress("Down");  break;
        case Keys.Left:   ButtonPress("Left");  break;
        case Keys.Right:  ButtonPress("Right"); break;
        case Keys.Enter:  ButtonPress("OK");    break;
      }
    }

    private void buttonDisconnect_Click(object sender, EventArgs e)
    {
      StopClient();

      this.Invoke(_Notification, "Disconnected");
    }

    private void buttonConnect_Click(object sender, EventArgs e)
    {
      _serverHost = textBoxComputer.Text;

      this.Invoke(_Notification, String.Format("Connecting to {0} ...", _serverHost));

      SaveSettings();

      IPAddress serverIP = IPAddress.Parse(_serverHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, ServerPort);

      StartClient(endPoint);
    }

    private void buttonQuit_Click(object sender, EventArgs e)
    {
      Quit();
    }

  }

}
