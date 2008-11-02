using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace VirtualRemote
{
  /// <summary>
  /// Main Form for Virtual Remote.
  /// </summary>
  internal partial class FormMain : Form
  {
    #region Constants

    private const string ConfigurationFile = "VirtualRemote.xml";

    private const int ServerPort = 24000;

    #endregion Constants

    #region Variables

    private Client _client;

    private bool _registered;
    private string _serverHost = "192.168.0.3";

    #endregion Variables

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="FormMain"/> class.
    /// </summary>
    public FormMain()
    {
      InitializeComponent();

      _Notification = Notification;

      LoadSettings();
      textBoxComputer.Text = _serverHost;
    }

    #endregion Constructor

    private readonly DelegateNotifcation _Notification;

    private void Notification(string text)
    {
      textBoxStatus.Text = text;
    }

    private void FormMain_Load(object sender, EventArgs e)
    {
      IPAddress serverIP = IPAddress.Parse(_serverHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, ServerPort);

      StartClient(endPoint);
    }

    private void ReceivedMessage(IrssMessage received)
    {
      switch (received.Type)
      {
        case MessageType.RegisterClient:
          if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
          {
            _registered = true;
            Invoke(_Notification, "Connected");
          }
          else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
          {
            _registered = false;
            Invoke(_Notification, "Failed to negotiate connection");
          }
          return;

        case MessageType.ServerShutdown:
          _registered = false;
          Invoke(_Notification, "Server Shutdown");
          return;

        case MessageType.Error:
          Invoke(_Notification, received.GetDataAsString());
          return;
      }
    }

    private void CommsFailure(object obj)
    {
      StopClient();

      Exception ex = obj as Exception;

      if (ex != null)
        Invoke(_Notification, ex.Message);
      else
        Invoke(_Notification, "Communications failure");
    }

    private void Connected(object obj)
    {
      IrssMessage message = new IrssMessage(MessageType.RegisterClient, MessageFlags.Request);
      _client.Send(message);
    }

    private void Disconnected(object obj)
    {
      Invoke(_Notification, "Communications with server has been lost");

      Thread.Sleep(1000);
    }

    private bool StartClient(IPEndPoint endPoint)
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

    private void StopClient()
    {
      if (_client == null)
        return;

      _client.Dispose();
      _client = null;

      _registered = false;
    }

    private void ButtonPress(string keyCode)
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

    private void Quit()
    {
      StopClient();
      Application.Exit();
    }

    private void LoadSettings()
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

      try
      {
        _serverHost = doc.DocumentElement.Attributes["ServerHost"].Value;
      }
      catch
      {
        _serverHost = "192.168.0.1";
      }
    }

    private void SaveSettings()
    {
      using (XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, Encoding.UTF8))
      {
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 1;
        writer.IndentChar = (char) 9;
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
        case Keys.Up:
          ButtonPress("Up");
          break;
        case Keys.Down:
          ButtonPress("Down");
          break;
        case Keys.Left:
          ButtonPress("Left");
          break;
        case Keys.Right:
          ButtonPress("Right");
          break;
        case Keys.Enter:
          ButtonPress("OK");
          break;
      }
    }

    private void buttonDisconnect_Click(object sender, EventArgs e)
    {
      StopClient();

      Invoke(_Notification, "Disconnected");
    }

    private void buttonConnect_Click(object sender, EventArgs e)
    {
      _serverHost = textBoxComputer.Text;

      Invoke(_Notification, String.Format("Connecting to {0} ...", _serverHost));

      SaveSettings();

      IPAddress serverIP = IPAddress.Parse(_serverHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, ServerPort);

      StartClient(endPoint);
    }

    private void buttonQuit_Click(object sender, EventArgs e)
    {
      Quit();
    }

    #region Nested type: DelegateNotifcation

    private delegate void DelegateNotifcation(string text);

    #endregion
  }
}