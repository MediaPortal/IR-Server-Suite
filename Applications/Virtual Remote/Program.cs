using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using IrssUtils.Forms;

namespace VirtualRemote
{

  static class Program
  {

    #region Constants

    const string DefaultSkin = "MCE";

    static readonly string ConfigurationFile = Common.FolderAppData + "Virtual Remote\\Virtual Remote.xml";

    #endregion Constants

    #region Variables

    static Client _client;

    static bool _registered;

    static string _serverHost;

    static string _installFolder;

    static string _remoteSkin;

    #endregion Variables

    #region Properties

    internal static bool Registered
    {
      get { return _registered; }
    }

    internal static string ServerHost
    {
      get { return _serverHost; }
      set { _serverHost = value; }
    }

    internal static string InstallFolder
    {
      get { return _installFolder; }
    }

    internal static string RemoteSkin
    {
      get { return _remoteSkin; }
      set { _remoteSkin = value; }
    }

    #endregion Properties

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      // TODO: Change log level to info for release.
      IrssLog.LogLevel = IrssLog.Level.Debug;
      IrssLog.Open(Common.FolderIrssLogs + "Virtual Remote.log");

      Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

      LoadSettings();

      if (args.Length > 0) // Command Line Start ...
      {
        List<String> virtualButtons = new List<string>();
        
        try
        {
          for (int index = 0; index < args.Length; index++)
          {
            if (args[index].Equals("-host", StringComparison.OrdinalIgnoreCase))
            {
              _serverHost = args[++index];
              continue;
            }
            else
            {
              virtualButtons.Add(args[index]);
            }
          }
        }
        catch (Exception ex)
        {
          IrssLog.Error("Error processing command line parameters: {0}", ex.ToString());
        }

        IPAddress serverIP = Client.GetIPFromName(_serverHost);
        IPEndPoint endPoint = new IPEndPoint(serverIP, IrssComms.Server.DefaultPort);

        if (virtualButtons.Count != 0 && StartClient(endPoint))
        {
          Thread.Sleep(250);

          // Wait for registered ... Give up after 10 seconds ...
          int attempt = 0;
          while (!_registered)
          {
            if (++attempt >= 10)
              break;
            else
              Thread.Sleep(1000);
          }

          if (_registered)
          {
            foreach (String button in virtualButtons)
            {
              if (button.StartsWith("~"))
              {
                Thread.Sleep(button.Length * 500);
              }
              else
              {
                IrssMessage message = new IrssMessage(MessageType.ForwardRemoteEvent, MessageFlags.Notify, button);
                _client.Send(message);
              }
            }

            Thread.Sleep(500);
          }
          else
          {
            IrssLog.Warn("Failed to register with server host \"{0}\", custom message(s) not sent", Program.ServerHost);
          }
        }

      }
      else // GUI Start ...
      {
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
          IrssLog.Error(ex.ToString());
          clientStarted = false;
        }

        if (clientStarted)
          Application.Run(new MainForm());

        SaveSettings();
      }

      StopClient();

      Application.ThreadException -= new ThreadExceptionEventHandler(Application_ThreadException);

      IrssLog.Close();
    }

    /// <summary>
    /// Handles unhandled exceptions.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">Event args.</param>
    public static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
    {
      IrssLog.Error(e.Exception.ToString());
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
        IrssLog.Error(ex.ToString());

        _installFolder = ".";
      }

      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationFile);

        _serverHost = doc.DocumentElement.Attributes["ServerHost"].Value;
        _remoteSkin = doc.DocumentElement.Attributes["RemoteSkin"].Value;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());

        _serverHost = "localhost";
        _remoteSkin = DefaultSkin;
      }
    }
    static void SaveSettings()
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

          writer.WriteAttributeString("ServerHost", _serverHost);
          writer.WriteAttributeString("RemoteSkin", _remoteSkin);

          writer.WriteEndElement(); // </settings>
          writer.WriteEndDocument();
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
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

    internal static bool StartClient(IPEndPoint endPoint)
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
    internal static void StopClient()
    {
      if (_client == null)
        return;

      _client.Dispose();
      _client = null;
    }

    internal static void SendMessage(IrssMessage message)
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

              IrssLog.Info("Registered to IR Server");
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              _registered = false;
              IrssLog.Warn("IR Server refused to register");
            }
            break;

          case MessageType.ServerShutdown:
            IrssLog.Warn("IR Server Shutdown - Virtual Remote disabled until IR Server returns");
            _registered = false;
            break;

          case MessageType.Error:
            IrssLog.Error(received.GetDataAsString());
            break;
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
      }
    }

  }

}
