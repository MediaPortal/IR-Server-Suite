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

using Microsoft.MediaCenter.Samples.MediaState;

using IrssComms;
using IrssUtils;

namespace MediaCenterBlaster
{

  /// <summary>
  /// Media Center Blaster main class.
  /// </summary>
  class Tray
  {

    #region Constants

    static readonly string ConfigurationFile = Path.Combine(Common.FolderAppData, "Media Center Blaster\\Media Center Blaster.xml");

    internal static readonly string FolderMacros = Path.Combine(Common.FolderAppData, "Media Center Blaster\\Macro");

    internal static readonly string ExtCfgFolder = Path.Combine(Common.FolderAppData, "Media Center Blaster");

    const string ProcessCommandThreadName = "ProcessCommand";

    #endregion Constants

    #region Variables

    static ClientMessageSink _handleMessage;

    static Client _client;

    static bool _registered;

    static string _serverHost;
    static bool _autoRun;
    static bool _logVerbose;

    static ExternalChannelConfig _externalChannelConfig;

    static bool _inConfiguration;
    static string _learnIRFilename;

    static IRServerInfo _irServerInfo = new IRServerInfo();

    static Container _container;
    static NotifyIcon _notifyIcon;
    static MediaState _mediaState;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Gets or sets the server host.
    /// </summary>
    /// <value>The server host.</value>
    internal static string ServerHost
    {
      get { return _serverHost; }
      set { _serverHost = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to log verbosely.
    /// </summary>
    /// <value><c>true</c> if logging is set to verbose; otherwise, <c>false</c>.</value>
    internal static bool LogVerbose
    {
      get { return _logVerbose; }
      set { _logVerbose = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether in configuration.
    /// </summary>
    /// <value><c>true</c> if in configuration; otherwise, <c>false</c>.</value>
    internal static bool InConfiguration
    {
      get { return _inConfiguration; }
      set { _inConfiguration = value; }
    }

    /// <summary>
    /// Gets or sets the message handler delegate.
    /// </summary>
    /// <value>The message handler delegate.</value>
    internal static ClientMessageSink HandleMessage
    {
      get { return _handleMessage; }
      set { _handleMessage = value; }
    }

    /// <summary>
    /// Gets the transceiver information.
    /// </summary>
    /// <value>The transceiver information.</value>
    internal static IRServerInfo TransceiverInformation
    {
      get { return _irServerInfo; }
    }

    internal static bool Registered
    {
      get { return _registered; }
    }

    internal static ExternalChannelConfig ExtChannelConfig
    {
      get { return _externalChannelConfig; }
      set { _externalChannelConfig = value; }
    }

    #endregion Properties

    #region Constructor

    public Tray()
    {
      ContextMenuStrip contextMenu = new ContextMenuStrip();
      contextMenu.Items.Add(new ToolStripLabel("Media Center Blaster"));
      contextMenu.Items.Add(new ToolStripSeparator());
      contextMenu.Items.Add(new ToolStripMenuItem("&Setup", null, new EventHandler(ClickSetup)));
      contextMenu.Items.Add(new ToolStripMenuItem("&Quit", null, new EventHandler(ClickQuit)));

      _container = new Container();
      _notifyIcon = new NotifyIcon(_container);
      _notifyIcon.ContextMenuStrip = contextMenu;
      _notifyIcon.DoubleClick += new EventHandler(ClickSetup);

      UpdateTrayIcon("Media Center Blaster - Connecting ...", Resources.Icon16Connecting);
    }

    #endregion Constructor

    #region Implementation

    void OnMSASEvent(object state, MediaStatusEventArgs args)
    {
      //MediaState typedState = (MediaState)state;
      IrssLog.Info("OnMSASEvent: {0} {1} {2} {3}", args.Session, args.SessionID, args.Tag, args.Value);
    }

    void TV_MediaChanged(object sender, EventArgs e)
    {
      IrssLog.Info("TV_MediaChanged");

      MediaStatusEventArgs mediaStatusEventArgs = (MediaStatusEventArgs)e;

      // MSPROPTAG_TrackNumber

      IrssLog.Info("Channel: {0}", mediaStatusEventArgs.Value);
      
      /*
      if (_externalChannelConfig == null)
        throw new ApplicationException("Cannot process tune request, no STB settings are loaded");

      try
      {
        Thread newThread = new Thread(new ParameterizedThreadStart(ProcessExternalChannel));
        newThread.Name = "ProcessExternalChannel";
        newThread.IsBackground = true;
        newThread.Start(new string[] { trackNumberProperty });
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
      */
    }


    static void UpdateTrayIcon(string text, Icon icon)
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

        LoadExternalConfig();

        if (String.IsNullOrEmpty(_serverHost))
        {
          if (!Configure())
            return false;
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
          MessageBox.Show("Failed to start IR Server communications, refer to log file for more details.", "Media Center Blaster - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
          clientStarted = false;
        }

        if (clientStarted)
        {
          _notifyIcon.Visible = true;

          _mediaState = new MediaState();
          _mediaState.OnMSASEvent += new MediaState.MSASEventHandler(OnMSASEvent);
          _mediaState.TV.MediaChanged += new EventHandler(TV_MediaChanged);

          _mediaState.Connect();

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

        _mediaState = null;
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

      if (_mediaState != null)
      {
        _mediaState.Dispose();
        _mediaState = null;
      }
    }

    void LoadSettings()
    {
      try
      {
        _autoRun = SystemRegistry.GetAutoRun("Media Center Blaster");
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

        _serverHost = doc.DocumentElement.Attributes["ServerHost"].Value;
        _logVerbose = bool.Parse(doc.DocumentElement.Attributes["LogVerbose"].Value);
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
          SystemRegistry.SetAutoRun("Media Center Blaster", Application.ExecutablePath);
        else
          SystemRegistry.RemoveAutoRun("Media Center Blaster");
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
          writer.WriteAttributeString("LogVerbose", _logVerbose.ToString());

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
      _serverHost = "localhost";
      _logVerbose = true;

      SaveSettings();
    }

    /// <summary>
    /// Load external channel configuration.
    /// </summary>
    static void LoadExternalConfig()
    {
      string fileName = Path.Combine(ExtCfgFolder, "ExternalChannelConfig.xml");

      try
      {
        _externalChannelConfig = ExternalChannelConfig.Load(fileName);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);

        _externalChannelConfig = new ExternalChannelConfig(fileName);
      }

      _externalChannelConfig.CardId = 0;
    }

    /// <summary>
    /// Processes the external channel.
    /// </summary>
    /// <param name="args">String array of parameters.</param>
    static void ProcessExternalChannel(object args)
    {
      try
      {
        string[] data = args as string[];
        if (data == null)
          throw new ArgumentException("Parameter is not of type string[]", "args");

        ExternalChannelConfig config = _externalChannelConfig;

        // Clean up the "data[0]" string into "channel".
        StringBuilder channel = new StringBuilder();
        foreach (char digit in data[0])
          if (char.IsDigit(digit))
            channel.Append(digit);

        // Pad the channel number with leading 0's to meet ChannelDigits length.
        while (channel.Length < config.ChannelDigits)
          channel.Insert(0, '0');

        // Process the channel and blast the relevant IR Commands.
        string channelString = channel.ToString();
        string command;
        int charVal;

        for (int repeatCount = 0; repeatCount <= config.RepeatChannelCommands; repeatCount++)
        {
          if (repeatCount > 0 && config.RepeatPauseTime > 0)
            Thread.Sleep(config.RepeatPauseTime);

          if (config.UsePreChangeCommand)
          {
            command = config.PreChangeCommand;
            if (!String.IsNullOrEmpty(command))
            {
              ProcessExternalCommand(command, -1, channelString);

              if (config.PauseTime > 0)
                Thread.Sleep(config.PauseTime);
            }
          }

          foreach (char digit in channelString)
          {
            charVal = digit - 48;

            command = config.Digits[charVal];
            if (!String.IsNullOrEmpty(command))
            {
              ProcessExternalCommand(command, charVal, channelString);

              if (config.PauseTime > 0)
                Thread.Sleep(config.PauseTime);
            }
          }

          if (config.SendSelect)
          {
            command = config.SelectCommand;
            if (!String.IsNullOrEmpty(command))
            {
              ProcessExternalCommand(command, -1, channelString);

              if (config.DoubleChannelSelect)
              {
                if (config.PauseTime > 0)
                  Thread.Sleep(config.PauseTime);

                ProcessExternalCommand(command, -1, channelString);
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
    }

    /// <summary>
    /// Processes the external channel change command.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="channelDigit">The current channel digit.</param>
    /// <param name="channelFull">The channel full ID.</param>
    internal static void ProcessExternalCommand(string command, int channelDigit, string channelFull)
    {
#if DEBUG
      IrssLog.Debug("ProcessExternalCommand(\"{0}\", {1}, {2})", command, channelDigit, channelFull);
#endif

      if (command.StartsWith(Common.CmdPrefixRun, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitRunCommand(command.Substring(Common.CmdPrefixRun.Length));

        commands[2] = commands[2].Replace("%1", channelDigit.ToString());
        commands[2] = commands[2].Replace("%2", channelFull);

        Common.ProcessRunCommand(commands);
      }
      else if (command.StartsWith(Common.CmdPrefixSerial, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitSerialCommand(command.Substring(Common.CmdPrefixSerial.Length));

        commands[0] = commands[0].Replace("%1", channelDigit.ToString());
        commands[0] = commands[0].Replace("%2", channelFull);

        Common.ProcessSerialCommand(commands);
      }
      else if (command.StartsWith(Common.CmdPrefixWindowMsg, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitWindowMessageCommand(command.Substring(Common.CmdPrefixWindowMsg.Length));

        commands[3] = commands[3].Replace("%1", channelDigit.ToString());
        commands[3] = commands[3].Replace("%2", channelFull);

        commands[4] = commands[4].Replace("%1", channelDigit.ToString());
        commands[4] = commands[4].Replace("%2", channelFull);

        Common.ProcessWindowMessageCommand(commands);
      }
      else if (command.StartsWith(Common.CmdPrefixTcpMsg, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitTcpMessageCommand(command.Substring(Common.CmdPrefixTcpMsg.Length));

        commands[0] = commands[0].Replace("%1", channelDigit.ToString());
        commands[0] = commands[0].Replace("%2", channelFull);

        commands[2] = commands[2].Replace("%1", channelDigit.ToString());
        commands[2] = commands[2].Replace("%2", channelFull);

        Common.ProcessTcpMessageCommand(commands);
      }
      else if (command.StartsWith(Common.CmdPrefixHttpMsg, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitHttpMessageCommand(command.Substring(Common.CmdPrefixHttpMsg.Length));

        commands[0] = commands[0].Replace("%1", channelDigit.ToString());
        commands[0] = commands[0].Replace("%2", channelFull);

        Common.ProcessHttpCommand(commands);
      }
      else if (command.StartsWith(Common.CmdPrefixKeys, StringComparison.OrdinalIgnoreCase))
      {
        string keysCommand = command.Substring(Common.CmdPrefixKeys.Length);

        keysCommand = keysCommand.Replace("%1", channelDigit.ToString());
        keysCommand = keysCommand.Replace("%2", channelFull);

        if (_inConfiguration)
          MessageBox.Show(keysCommand, Common.UITextKeys, MessageBoxButtons.OK, MessageBoxIcon.Information);
        else
          Common.ProcessKeyCommand(keysCommand);
      }
      else if (command.StartsWith(Common.CmdPrefixPopup, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitPopupCommand(command.Substring(Common.CmdPrefixPopup.Length));

        commands[0] = commands[0].Replace("%1", channelDigit.ToString());
        commands[0] = commands[0].Replace("%2", channelFull);

        commands[1] = commands[1].Replace("%1", channelDigit.ToString());
        commands[1] = commands[1].Replace("%2", channelFull);

        IrssUtils.Forms.ShowPopupMessage showPopupMessage = new IrssUtils.Forms.ShowPopupMessage(commands[0], commands[1], int.Parse(commands[2]));
        showPopupMessage.ShowDialog();
      }
      else
      {
        ProcessCommand(command, false);
      }
    }

    /// <summary>
    /// Learn an IR command.
    /// </summary>
    /// <param name="fileName">File to place learned IR command in (absolute path).</param>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    internal static bool LearnIR(string fileName)
    {
      try
      {
        if (String.IsNullOrEmpty(fileName))
        {
          IrssLog.Error("Null or Empty file name for LearnIR()");
          return false;
        }

        if (!_registered)
        {
          IrssLog.Warn("Not registered to an active Input Service");
          return false;
        }

        if (_learnIRFilename != null)
        {
          IrssLog.Warn("Already trying to learn an IR command");
          return false;
        }

        _learnIRFilename = fileName;

        IrssMessage message = new IrssMessage(MessageType.LearnIR, MessageFlags.Request);
        _client.Send(message);
      }
      catch (Exception ex)
      {
        _learnIRFilename = null;
        IrssLog.Error(ex);
        return false;
      }

      return true;
    }

    /// <summary>
    /// Blast an IR command.
    /// </summary>
    /// <param name="fileName">File to blast (absolute path).</param>
    /// <param name="port">Port to blast to.</param>
    internal static void BlastIR(string fileName, string port)
    {
      if (LogVerbose)
        IrssLog.Debug("BlastIR(): {0}, {1}", fileName, port);

      if (!_registered)
        throw new ApplicationException("Cannot Blast, not registered to an active Input Service");

      using (FileStream file = File.OpenRead(fileName))
      {
        if (file.Length == 0)
          throw new IOException(String.Format("Cannot Blast. IR file \"{0}\" has no data, possible IR learn failure", fileName));

        byte[] outData = new byte[4 + port.Length + file.Length];

        BitConverter.GetBytes(port.Length).CopyTo(outData, 0);
        Encoding.ASCII.GetBytes(port).CopyTo(outData, 4);

        file.Read(outData, 4 + port.Length, (int)file.Length);

        IrssMessage message = new IrssMessage(MessageType.BlastIR, MessageFlags.Request, outData);
        _client.Send(message);
      }
    }

    /// <summary>
    /// Given a command this method processes the request accordingly.
    /// Throws exceptions only if run synchronously, if async exceptions are logged instead.
    /// </summary>
    /// <param name="command">Command to process.</param>
    /// <param name="async">Process command asynchronously?</param>
    internal static void ProcessCommand(string command, bool async)
    {
      if (async)
      {
        try
        {
          Thread newThread = new Thread(new ParameterizedThreadStart(ProcCommand));
          newThread.Name = ProcessCommandThreadName;
          newThread.IsBackground = true;
          newThread.Start(command);
        }
        catch (Exception ex)
        {
          IrssLog.Error(ex);
        }
      }
      else
      {
        ProcCommand(command);
      }
    }

    /// <summary>
    /// Used by ProcessCommand to actually handle the command.
    /// Can be called Synchronously or as a Parameterized Thread.
    /// </summary>
    /// <param name="commandObj">Command string to process.</param>
    static void ProcCommand(object commandObj)
    {
      try
      {
        if (commandObj == null)
          throw new ArgumentNullException("commandObj");

        string command = commandObj as string;

        if (String.IsNullOrEmpty(command))
          throw new ArgumentException("commandObj translates to empty or null string", "commandObj");

        if (command.StartsWith(Common.CmdPrefixMacro, StringComparison.OrdinalIgnoreCase))
        {
          string fileName = FolderMacros + command.Substring(Common.CmdPrefixMacro.Length) + Common.FileExtensionMacro;
          ProcMacro(fileName);
        }
        else if (command.StartsWith(Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitBlastCommand(command.Substring(Common.CmdPrefixBlast.Length));
          BlastIR(Common.FolderIRCommands + commands[0] + Common.FileExtensionIR, commands[1]);
        }
        else if (command.StartsWith(Common.CmdPrefixSTB, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitBlastCommand(command.Substring(Common.CmdPrefixSTB.Length));
          BlastIR(Common.FolderSTB + commands[0] + Common.FileExtensionIR, commands[1]);
        }
        else if (command.StartsWith(Common.CmdPrefixPause, StringComparison.OrdinalIgnoreCase))
        {
          int pauseTime = int.Parse(command.Substring(Common.CmdPrefixPause.Length));
          Thread.Sleep(pauseTime);
        }
        else if (command.StartsWith(Common.CmdPrefixRun, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitRunCommand(command.Substring(Common.CmdPrefixRun.Length));
          Common.ProcessRunCommand(commands);
        }
        else if (command.StartsWith(Common.CmdPrefixSerial, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitSerialCommand(command.Substring(Common.CmdPrefixSerial.Length));
          Common.ProcessSerialCommand(commands);
        }
        else if (command.StartsWith(Common.CmdPrefixWindowMsg, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitWindowMessageCommand(command.Substring(Common.CmdPrefixWindowMsg.Length));
          Common.ProcessWindowMessageCommand(commands);
        }
        else if (command.StartsWith(Common.CmdPrefixTcpMsg, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitTcpMessageCommand(command.Substring(Common.CmdPrefixTcpMsg.Length));
          Common.ProcessTcpMessageCommand(commands);
        }
        else if (command.StartsWith(Common.CmdPrefixHttpMsg, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitHttpMessageCommand(command.Substring(Common.CmdPrefixHttpMsg.Length));
          Common.ProcessHttpCommand(commands);
        }
        else if (command.StartsWith(Common.CmdPrefixKeys, StringComparison.OrdinalIgnoreCase))
        {
          string keyCommand = command.Substring(Common.CmdPrefixKeys.Length);
          if (_inConfiguration)
            MessageBox.Show(keyCommand, Common.UITextKeys, MessageBoxButtons.OK, MessageBoxIcon.Information);
          else
            Common.ProcessKeyCommand(keyCommand);
        }
        else if (command.StartsWith(Common.CmdPrefixEject, StringComparison.OrdinalIgnoreCase))
        {
          string ejectCommand = command.Substring(Common.CmdPrefixEject.Length);
          Common.ProcessEjectCommand(ejectCommand);
        }
        else if (command.StartsWith(Common.CmdPrefixPopup, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitPopupCommand(command.Substring(Common.CmdPrefixPopup.Length));
          IrssUtils.Forms.ShowPopupMessage showPopupMessage = new IrssUtils.Forms.ShowPopupMessage(commands[0], commands[1], int.Parse(commands[2]));
          showPopupMessage.ShowDialog();
        }
        else
        {
          throw new ArgumentException(String.Format("Cannot process unrecognized command \"{0}\"", command), "command");
        }
      }
      catch (Exception ex)
      {
        if (!String.IsNullOrEmpty(Thread.CurrentThread.Name) && Thread.CurrentThread.Name.Equals(ProcessCommandThreadName, StringComparison.OrdinalIgnoreCase))
          IrssLog.Error(ex);
        else
          throw;
      }
    }

    /// <summary>
    /// Called by ProcCommand to process the supplied Macro file.
    /// </summary>
    /// <param name="fileName">Macro file to process (absolute path).</param>
    static void ProcMacro(string fileName)
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(fileName);

      XmlNodeList commandSequence = doc.DocumentElement.SelectNodes("item");

      foreach (XmlNode item in commandSequence)
        ProcCommand(item.Attributes["command"].Value);
    }

    /// <summary>
    /// Returns a list of Macros.
    /// </summary>
    /// <param name="commandPrefix">Add the command prefix to each list item.</param>
    /// <returns>string[] of Macros.</returns>
    internal static string[] GetMacroList(bool commandPrefix)
    {
      string[] files = Directory.GetFiles(FolderMacros, '*' + Common.FileExtensionMacro);
      string[] list = new string[files.Length];

      int i = 0;
      foreach (string file in files)
      {
        if (commandPrefix)
          list[i++] = Common.CmdPrefixMacro + Path.GetFileNameWithoutExtension(file);
        else
          list[i++] = Path.GetFileNameWithoutExtension(file);
      }

      return list;
    }

    /// <summary>
    /// Returns a combined list of IR Commands and Macros.
    /// </summary>
    /// <param name="commandPrefix">Add the command prefix to each list item.</param>
    /// <returns>string[] of IR Commands and Macros.</returns>
    internal static string[] GetFileList(bool commandPrefix)
    {
      string[] MacroFiles = Directory.GetFiles(FolderMacros, '*' + Common.FileExtensionMacro);
      string[] IRFiles = Directory.GetFiles(Common.FolderIRCommands, '*' + Common.FileExtensionIR);
      string[] list = new string[MacroFiles.Length + IRFiles.Length];

      int i = 0;
      foreach (string file in MacroFiles)
      {
        if (commandPrefix)
          list[i++] = Common.CmdPrefixMacro + Path.GetFileNameWithoutExtension(file);
        else
          list[i++] = Path.GetFileNameWithoutExtension(file);
      }

      foreach (string file in IRFiles)
      {
        if (commandPrefix)
          list[i++] = Common.CmdPrefixBlast + Path.GetFileNameWithoutExtension(file);
        else
          list[i++] = Path.GetFileNameWithoutExtension(file);
      }

      return list;
    }





    static void CommsFailure(object obj)
    {
      Exception ex = obj as Exception;

      if (ex != null)
        IrssLog.Error("Communications failure: {0}", ex.Message);
      else
        IrssLog.Error("Communications failure");

      StopClient();

      MessageBox.Show("Please report this error.", "Media Center Blaster - Communications failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    static void Connected(object obj)
    {
      IrssLog.Info("Connected to server");

      UpdateTrayIcon("Media Center Blaster", Resources.Icon16);

      IrssMessage message = new IrssMessage(MessageType.RegisterClient, MessageFlags.Request);
      _client.Send(message);
    }
    static void Disconnected(object obj)
    {
      IrssLog.Warn("Communications with server has been lost");

      UpdateTrayIcon("Media Center Blaster - Re-Connecting ...", Resources.Icon16Connecting);

      Thread.Sleep(1000);
    }

    static internal bool StartClient(IPEndPoint endPoint)
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
    static internal void StopClient()
    {
      if (_client == null)
        return;

      _client.Dispose();
      _client = null;

      _registered = false;
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
              _irServerInfo = IRServerInfo.FromBytes(received.GetDataAsBytes());
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

          case MessageType.BlastIR:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              if (LogVerbose)
                IrssLog.Info("Blast successful");
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              IrssLog.Error("Failed to blast IR command");
            }
            break;

          case MessageType.LearnIR:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              if (LogVerbose)
                IrssLog.Info("Learned IR Successfully");

              byte[] dataBytes = received.GetDataAsBytes();

              using (FileStream file = File.Create(_learnIRFilename))
                file.Write(dataBytes, 0, dataBytes.Length);
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              IrssLog.Error("Failed to learn IR command");
            }
            else if ((received.Flags & MessageFlags.Timeout) == MessageFlags.Timeout)
            {
              IrssLog.Error("Learn IR command timed-out");
            }

            _learnIRFilename = null;
            break;

          case MessageType.ServerShutdown:
            IrssLog.Warn("Input Service Shutdown - Media Center Blaster disabled until Input Service returns");
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

    static void RemoteHandlerCallback(string deviceName, string keyCode)
    {
      IrssLog.Info("Remote Event: {0}", keyCode);
    }

    bool Configure()
    {
      SetupForm setup = new SetupForm();

      if (setup.ShowDialog() == DialogResult.OK)
      {
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
