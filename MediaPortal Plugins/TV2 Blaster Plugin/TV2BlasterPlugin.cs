using System;
using System.Collections;
using System.Collections.Generic;
#if TRACE
using System.Diagnostics;
#endif
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
using MediaPortal.Configuration;
using MediaPortal.Util;

using IrssComms;
using IrssUtils;
using MPUtils;

namespace MediaPortal.Plugins
{

  /// <summary>
  /// MediaPortal TV2 Blaster Plugin for IR Server.
  /// </summary>
  public class TV2BlasterPlugin : IPlugin, ISetupForm
  {

    #region Constants

    /// <summary>
    /// The plugin version string.
    /// </summary>
    internal const string PluginVersion = "TV2 Blaster Plugin 1.0.4.1 for IR Server";

    internal static readonly string FolderMacros = Common.FolderAppData + "TV2 Blaster Plugin\\Macro\\";

    internal static readonly string ExtCfgFolder = Common.FolderAppData + "TV2 Blaster Plugin\\";

    const string ProcessCommandThreadName = "ProcessCommand";

    #endregion Constants

    #region Variables

    static Client _client;

    static string _serverHost;
    static string _learnIRFilename;

    static bool _registered;

    static bool _logVerbose;

    static ExternalChannelConfig[] _externalChannelConfigs;

    static ClientMessageSink _handleMessage;

    static bool _inConfiguration;

    static bool _mpBasicHome;

    static IRServerInfo _irServerInfo = new IRServerInfo();

    static Hashtable _macroStacks;

    #endregion Variables

    #region Properties

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

    internal static ClientMessageSink HandleMessage
    {
      get { return _handleMessage; }
      set { _handleMessage = value; }
    }

    internal static IRServerInfo TransceiverInformation
    {
      get { return _irServerInfo; }
    }

    #endregion Properties

    #region IPlugin methods

    /// <summary>
    /// Starts this instance.
    /// </summary>
    public void Start()
    {
      _inConfiguration = false;

      Log.Info("TV2BlasterPlugin: Starting ({0})", PluginVersion);

      // Hashtable for storing active macro stacks in.
      _macroStacks = new Hashtable();

      // Load basic settings
      LoadSettings();

      LoadExternalConfigs();

      IPAddress serverIP = Client.GetIPFromName(_serverHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, IrssComms.Server.DefaultPort);

      if (!StartClient(endPoint))
        Log.Error("TV2BlasterPlugin: Failed to start local comms, IR blasting is disabled for this session");

      // Register with MediaPortal to receive GUI Messages ...
      GUIWindowManager.Receivers += new SendMessageHandler(OnMessage);

      if (LogVerbose)
        Log.Info("TV2BlasterPlugin: Started");
    }
    /// <summary>
    /// Stops this instance.
    /// </summary>
    public void Stop()
    {
      GUIWindowManager.Receivers -= new SendMessageHandler(OnMessage);

      StopClient();

      _macroStacks = null;

      if (LogVerbose)
        Log.Info("TV2BlasterPlugin: Stopped");
    }

    #endregion IPlugin methods

    #region ISetupForm methods

    /// <summary>
    /// Determines whether this plugin can be enabled.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this plugin can be enabled; otherwise, <c>false</c>.
    /// </returns>
    public bool CanEnable()       { return true; }
    /// <summary>
    /// Determines whether this plugin has setup.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this plugin has setup; otherwise, <c>false</c>.
    /// </returns>
    public bool HasSetup()        { return true; }
    /// <summary>
    /// Gets the plugin name.
    /// </summary>
    /// <returns>The plugin name.</returns>
    public string PluginName()    { return "TV2 Blaster Plugin for IR Server"; }
    /// <summary>
    /// Defaults enabled.
    /// </summary>
    /// <returns>true if this plugin is enabled by default, otherwise false.</returns>
    public bool DefaultEnabled()  { return true; }
    /// <summary>
    /// Gets the window id.
    /// </summary>
    /// <returns>The window id.</returns>
    public int GetWindowId()      { return 0; }
    /// <summary>
    /// Gets the plugin author.
    /// </summary>
    /// <returns>The plugin author.</returns>
    public string Author()        { return "and-81"; }
    /// <summary>
    /// Gets the description of the plugin.
    /// </summary>
    /// <returns>The plugin description.</returns>
    public string Description()   { return "External Channel Changer for TV Engine 2 using IR Server"; }

    /// <summary>
    /// Shows the plugin configuration.
    /// </summary>
    public void ShowPlugin()
    {
      try
      {
        LoadSettings();
        LoadExternalConfigs();

        _inConfiguration = true;

        if (LogVerbose)
          Log.Info("TV2BlasterPlugin: ShowPlugin()");

        SetupForm setupForm = new SetupForm();
        if (setupForm.ShowDialog() == DialogResult.OK)
          SaveSettings();

        StopClient();
      }
      catch (Exception ex)
      {
        Log.Error(ex);
      }

      if (LogVerbose)
        Log.Info("TV2BlasterPlugin: ShowPlugin() - End");
    }

    /// <summary>
    /// Gets the home screen details for the plugin.
    /// </summary>
    /// <param name="strButtonText">The button text.</param>
    /// <param name="strButtonImage">The button image.</param>
    /// <param name="strButtonImageFocus">The button image focus.</param>
    /// <param name="strPictureImage">The picture image.</param>
    /// <returns>true if the plugin can be seen, otherwise false.</returns>
    public bool GetHome(out string strButtonText, out string strButtonImage, out string strButtonImageFocus, out string strPictureImage)
    {
      strButtonText = strButtonImage = strButtonImageFocus = strPictureImage = String.Empty;
      return false;
    }

    #endregion ISetupForm methods

    #region Implementation

    static void CommsFailure(object obj)
    {
      Exception ex = obj as Exception;
      
      if (ex != null)
        Log.Error("TV2BlasterPlugin: Communications failure: {0}", ex.ToString());
      else
        Log.Error("TV2BlasterPlugin: Communications failure");

      StopClient();

      Log.Warn("TV2BlasterPlugin: Attempting communications restart ...");

      IPAddress serverIP = Client.GetIPFromName(_serverHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, IrssComms.Server.DefaultPort);

      StartClient(endPoint);
    }
    static void Connected(object obj)
    {
      Log.Info("TV2BlasterPlugin: Connected to server");

      IrssMessage message = new IrssMessage(MessageType.RegisterClient, MessageFlags.Request);
      _client.Send(message);
    }
    static void Disconnected(object obj)
    {
      Log.Warn("TV2BlasterPlugin: Communications with server has been lost");

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
      if (_client != null)
      {
        _client.Dispose();
        _client = null;
      }
    }

    static void ReceivedMessage(IrssMessage received)
    {
      if (LogVerbose)
        Log.Debug("TV2BlasterPlugin: Received Message \"{0}\"", received.Type);

      try
      {
        switch (received.Type)
        {
          case MessageType.BlastIR:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              if (LogVerbose)
                Log.Info("TV2BlasterPlugin: Blast successful");
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              Log.Error("TV2BlasterPlugin: Failed to blast IR command");
            }
            break;

          case MessageType.RegisterClient:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              _irServerInfo = IRServerInfo.FromBytes(received.GetDataAsBytes());
              _registered = true;

              if (LogVerbose)
                Log.Info("TV2BlasterPlugin: Registered to IR Server");
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              _registered = false;
              Log.Error("TV2BlasterPlugin: IR Server refused to register");
            }
            break;

          case MessageType.LearnIR:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              if (LogVerbose)
                Log.Info("TV2BlasterPlugin: Learned IR Successfully");

              byte[] dataBytes = received.GetDataAsBytes();

              using (FileStream file = File.Create(_learnIRFilename))
                file.Write(dataBytes, 0, dataBytes.Length);
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              Log.Error("TV2BlasterPlugin: Failed to learn IR command");
            }
            else if ((received.Flags & MessageFlags.Timeout) == MessageFlags.Timeout)
            {
              Log.Error("TV2BlasterPlugin: Learn IR command timed-out");
            }

            _learnIRFilename = null;
            break;

          case MessageType.ServerShutdown:
            Log.Info("TV2BlasterPlugin: IR Server Shutdown - Plugin disabled until IR Server returns");
            _registered = false;
            break;

          case MessageType.Error:
            _learnIRFilename = null;
            Log.Error("TV2BlasterPlugin: Received error: {0}", received.GetDataAsString());
            break;
        }

        if (_handleMessage != null)
          _handleMessage(received);
      }
      catch (Exception ex)
      {
        _learnIRFilename = null;
        Log.Error(ex);
      }
    }

    /// <summary>
    /// OnMessage is used to receive requests to Tune External Channels and for event mapping.
    /// </summary>
    /// <param name="msg">Message.</param>
    void OnMessage(GUIMessage msg)
    {
      if (msg.Message != GUIMessage.MessageType.GUI_MSG_TUNE_EXTERNAL_CHANNEL)
        return;

      Log.Info("TV2BlasterPlugin: Tune request - Card: {0}, Channel: {1}", msg.Label2, msg.Label);

      try
      {
        Thread newThread = new Thread(new ParameterizedThreadStart(ProcessExternalChannel));
        newThread.Name = "ProcessExternalChannel";
        newThread.IsBackground = true;
        newThread.Start(new string[] { msg.Label, msg.Label2 });
      }
      catch (Exception ex)
      {
        Log.Error(ex);
      }
    }

    /// <summary>
    /// Load external channel configurations.
    /// </summary>
    static void LoadExternalConfigs()
    {
      ArrayList cards = new ArrayList();
      MediaPortal.TV.Database.TVDatabase.GetCards(ref cards);

      if (cards.Count == 0)
      {
        Log.Warn("Cannot load external channel configurations, there are no TV cards registered");

        cards.Add(0);
      }

      _externalChannelConfigs = new ExternalChannelConfig[cards.Count];

      int index = 0;
      foreach (int cardId in cards)
      {
        string fileName = String.Format("{0}ExternalChannelConfig{1}.xml", ExtCfgFolder, cardId);

        try
        {
          _externalChannelConfigs[index] = ExternalChannelConfig.Load(fileName);
        }
        catch (Exception ex)
        {
          Log.Error(ex);

          _externalChannelConfigs[index] = new ExternalChannelConfig(fileName);
        }

        _externalChannelConfigs[index].CardId = cardId;
        index++;
      }
    }

    /// <summary>
    /// Given a card ID returns the configuration for that card.
    /// </summary>
    /// <param name="cardId">ID of card to retreive configuration for.</param>
    /// <returns>Card configuration, null if it doesn't exist.</returns>
    internal static ExternalChannelConfig GetExternalChannelConfig(int cardId)
    {
      if (_externalChannelConfigs == null)
        return null;

      foreach (ExternalChannelConfig config in _externalChannelConfigs)
        if (config.CardId == cardId)
          return config;

      return null;
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

        int card = int.Parse(data[1]);

        // To work around a known bug in MediaPortal scheduled recording (Added: 25-Feb-2007)
        if (card < 0)
          card = 0;

        ExternalChannelConfig config = GetExternalChannelConfig(card);

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
        Log.Error(ex);
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
      Log.Debug("TV2BlasterPlugin: ProcessExternalCommand(\"{0}\", {1}, {2})", command, channelDigit, channelFull);
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

        if (_inConfiguration)
          MessageBox.Show(commands[1], commands[0], MessageBoxButtons.OK, MessageBoxIcon.Information);
        else
          MPCommon.ShowNotifyDialog(commands[0], commands[1], int.Parse(commands[2]));
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
          Log.Error("TV2BlasterPlugin: Null or Empty file name for LearnIR()");
          return false;
        }

        if (!_registered)
        {
          Log.Warn("TV2BlasterPlugin: Not registered to an active IR Server");
          return false;
        }

        if (_learnIRFilename != null)
        {
          Log.Warn("TV2BlasterPlugin: Already trying to learn an IR command");
          return false;
        }

        _learnIRFilename = fileName;

        IrssMessage message = new IrssMessage(MessageType.LearnIR, MessageFlags.Request);
        _client.Send(message);
      }
      catch (Exception ex)
      {
        _learnIRFilename = null;
        Log.Error(ex);
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
        Log.Debug("TV2BlasterPlugin - BlastIR(): {0}, {1}", fileName, port);

      if (!_registered)
        throw new ApplicationException("Cannot Blast, not registered to an active IR Server");

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
          Log.Error(ex);
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
          if (_inConfiguration)
            MessageBox.Show(commands[1], commands[0], MessageBoxButtons.OK, MessageBoxIcon.Information);
          else
            MPCommon.ShowNotifyDialog(commands[0], commands[1], int.Parse(commands[2]));
        }
        else if (command.StartsWith(Common.CmdPrefixGoto, StringComparison.OrdinalIgnoreCase))
        {
          string screenID = command.Substring(Common.CmdPrefixGoto.Length);

          if (_inConfiguration)
            MessageBox.Show(screenID, Common.UITextGoto, MessageBoxButtons.OK, MessageBoxIcon.Information);
          else
            MPCommon.ProcessGoTo(screenID, _mpBasicHome);
        }
        else
        {
          throw new ArgumentException(String.Format("Cannot process unrecognized command \"{0}\"", command), "command");
        }
      }
      catch (Exception ex)
      {
        if (Thread.CurrentThread.Name.Equals(ProcessCommandThreadName, StringComparison.OrdinalIgnoreCase))
          Log.Error(ex);
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
      MacroStackAdd(Thread.CurrentThread.ManagedThreadId, fileName);

      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(fileName);

        if (doc.DocumentElement.InnerText.Contains(Common.CmdPrefixBlast) && !_registered)
          throw new ApplicationException("Cannot process Macro with Blast commands when not registered to an active IR Server");

        XmlNodeList commandSequence = doc.DocumentElement.SelectNodes("item");

        foreach (XmlNode item in commandSequence)
          ProcCommand(item.Attributes["command"].Value);
      }
      finally
      {
        MacroStackRemove(Thread.CurrentThread.ManagedThreadId, fileName);
      }
    }

    /// <summary>
    /// Retreives the required Macro Stack from the Hashtable.
    /// </summary>
    /// <param name="hash">Hash table lookup value.</param>
    /// <returns>Macro Stack.</returns>
    static List<string> GetMacroStack(int hash)
    {
      if (_macroStacks.ContainsKey(hash))
      {
        return (List<string>)_macroStacks[hash];
      }
      else
      {
        List<string> newStack = new List<string>();
        _macroStacks.Add(hash, newStack);
        return newStack;
      }
    }

    /// <summary>
    /// Adds to the Macro Stack.
    /// </summary>
    /// <param name="hash">Hash table lookup value.</param>
    /// <param name="fileName">Name of the macro file.</param>
    static void MacroStackAdd(int hash, string fileName)
    {
      List<string> stack = GetMacroStack(hash);

      string upperCasedFileName = fileName.ToUpperInvariant();

      if (stack.Contains(upperCasedFileName))
      {
        StringBuilder macroStackTrace = new StringBuilder();
        macroStackTrace.AppendLine("Macro infinite loop detected!");
        macroStackTrace.AppendLine();
        macroStackTrace.AppendLine("Stack trace:");

        foreach (string macro in stack)
        {
          if (macro.Equals(upperCasedFileName))
            macroStackTrace.AppendLine(String.Format("--> {0}", macro));
          else
            macroStackTrace.AppendLine(macro);
        }

        macroStackTrace.AppendLine(String.Format("--> {0}", upperCasedFileName));

        throw new ApplicationException(macroStackTrace.ToString());
      }

      stack.Add(upperCasedFileName);
    }

    /// <summary>
    /// Removes from the Macro Stack.
    /// </summary>
    /// <param name="hash">Hash table lookup value.</param>
    /// <param name="fileName">Name of the macro file.</param>
    static void MacroStackRemove(int hash, string fileName)
    {
      List<string> stack = GetMacroStack(hash);

      string upperCasedFileName = fileName.ToUpperInvariant();

      if (stack.Contains(upperCasedFileName))
        stack.Remove(upperCasedFileName);

      if (stack.Count == 0)
        _macroStacks.Remove(hash);
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

    /// <summary>
    /// Loads the settings.
    /// </summary>
    static void LoadSettings()
    {
      try
      {
        using (MediaPortal.Profile.Settings xmlreader = new MediaPortal.Profile.Settings(MPCommon.MPConfigFile))
        {
          ServerHost = xmlreader.GetValueAsString("TV2BlasterPlugin", "ServerHost", "localhost");
          LogVerbose = xmlreader.GetValueAsBool("TV2BlasterPlugin", "LogVerbose", false);

          // MediaPortal settings ...
          _mpBasicHome = xmlreader.GetValueAsBool("general", "startbasichome", false);
        }
      }
      catch (Exception ex)
      {
        Log.Error("TV2BlasterPlugin - LoadSettings(): {0}", ex.ToString());
      }
    }
    /// <summary>
    /// Saves the settings.
    /// </summary>
    static void SaveSettings()
    {
      try
      {
        using (MediaPortal.Profile.Settings xmlwriter = new MediaPortal.Profile.Settings(MPCommon.MPConfigFile))
        {
          xmlwriter.SetValue("TV2BlasterPlugin", "ServerHost", ServerHost);
          xmlwriter.SetValueAsBool("TV2BlasterPlugin", "LogVerbose", LogVerbose);
        }
      }
      catch (Exception ex)
      {
        Log.Error("TV2BlasterPlugin - SaveSettings(): {0}", ex.ToString());
      }
    }

    #endregion Implementation

  }

}
