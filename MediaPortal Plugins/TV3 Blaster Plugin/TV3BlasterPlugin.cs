using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using TvLibrary.Log;
using TvControl;
using SetupTv;
using TvEngine.Events;
using TvLibrary.Interfaces;
using TvLibrary.Implementations;
using TvDatabase;

using IrssComms;
using IrssUtils;
using MPUtils;

namespace TvEngine
{

  /// <summary>
  /// MediaPortal TV3 Blaster Plugin for IR Server.
  /// </summary>
  public class TV3BlasterPlugin : ITvServerPlugin
  {

    #region Constants

    /// <summary>
    /// The plugin version string.
    /// </summary>
    internal const string PluginVersion = "TV3 Blaster Plugin 1.0.3.5 for IR Server";

    internal static readonly string FolderMacros = Common.FolderAppData + "TV3 Blaster Plugin\\Macro\\";

    internal static readonly string ExtCfgFolder = Common.FolderAppData + "TV3 Blaster Plugin\\";

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

    static IRServerInfo _irServerInfo = new IRServerInfo();

    static Hashtable _macroStacks;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Returns the name of the plugin.
    /// </summary>
    public string Name { get { return "TV3 Blaster Plugin for IR Server"; } }
    /// <summary>
    /// Returns the version of the plugin.
    /// </summary>
    public string Version { get { return "1.0.3.5"; } }
    /// <summary>
    /// Returns the author of the plugin.
    /// </summary>
    public string Author { get { return "and-81"; } }
    /// <summary>
    /// Returns if the plugin should only run on the master server or also on slave servers.
    /// </summary>
    public bool MasterOnly { get { return false; } }

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

    #region ITvServerPlugin methods

    /// <summary>
    /// Starts this instance.
    /// </summary>
    [CLSCompliant(false)]
    public void Start(IController controller)
    {
      InConfiguration = false;

      Log.Info("TV3BlasterPlugin: Starting ({0})", PluginVersion);

      // Hashtable for storing active macro stacks in.
      _macroStacks = new Hashtable();

      // Load basic settings
      LoadSettings();

      LoadExternalConfigs();

      ITvServerEvent events = GlobalServiceProvider.Instance.Get<ITvServerEvent>();
      events.OnTvServerEvent += new TvServerEventHandler(events_OnTvServerEvent);

      IPAddress serverIP = Client.GetIPFromName(_serverHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, IrssComms.Server.DefaultPort);

      if (!StartClient(endPoint))
        Log.Error("TV3BlasterPlugin: Failed to start local comms, IR blasting is disabled for this session");

      if (LogVerbose)
        Log.Info("TV3BlasterPlugin: Started");
    }
    /// <summary>
    /// Stops this instance.
    /// </summary>
    public void Stop()
    {
      ITvServerEvent events = GlobalServiceProvider.Instance.Get<ITvServerEvent>();
      events.OnTvServerEvent -= new TvServerEventHandler(events_OnTvServerEvent);

      StopClient();

      _macroStacks = null;

      if (LogVerbose)
        Log.Info("TV3BlasterPlugin: Stopped");
    }

    /// <summary>
    /// Gets the setup form control.
    /// </summary>
    /// <value>The setup form control.</value>
    [CLSCompliant(false)]
    public SetupTv.SectionSettings Setup
    {
      get
      {
        return new SetupTv.Sections.PluginSetup();
      }
    }

    #endregion ITvServerPlugin methods

    #region Implementation

    static void CommsFailure(object obj)
    {
      Exception ex = obj as Exception;
      
      if (ex != null)
        Log.Error("TV3BlasterPlugin: Communications failure: {0}", ex.Message);
      else
        Log.Error("TV3BlasterPlugin: Communications failure");

      StopClient();

      Log.Info("TV3BlasterPlugin: Attempting communications restart ...");

      IPAddress serverIP = Client.GetIPFromName(_serverHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, IrssComms.Server.DefaultPort);

      StartClient(endPoint);
    }
    static void Connected(object obj)
    {
      Log.Info("TV3BlasterPlugin: Connected to server");

      IrssMessage message = new IrssMessage(MessageType.RegisterClient, MessageFlags.Request);
      _client.Send(message);
    }
    static void Disconnected(object obj)
    {
      Log.Info("TV3BlasterPlugin: Communications with server has been lost");

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
        Log.Debug("TV3BlasterPlugin: Received Message \"{0}\"", received.Type);

      try
      {
        switch (received.Type)
        {
          case MessageType.BlastIR:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              if (LogVerbose)
                Log.Info("TV3BlasterPlugin: Blast successful");
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              Log.Error("TV3BlasterPlugin: Failed to blast IR command");
            }
            break;

          case MessageType.RegisterClient:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              _irServerInfo = IRServerInfo.FromBytes(received.GetDataAsBytes());
              _registered = true;

              if (LogVerbose)
                Log.Info("TV3BlasterPlugin: Registered to IR Server");
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              _registered = false;
              Log.Error("TV3BlasterPlugin: IR Server refused to register");
            }
            break;

          case MessageType.LearnIR:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              if (LogVerbose)
                Log.Info("TV3BlasterPlugin: Learned IR Successfully");

              byte[] dataBytes = received.GetDataAsBytes();

              using (FileStream file = File.Create(_learnIRFilename))
                file.Write(dataBytes, 0, dataBytes.Length);
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              Log.Error("TV3BlasterPlugin: Failed to learn IR command");
            }
            else if ((received.Flags & MessageFlags.Timeout) == MessageFlags.Timeout)
            {
              Log.Error("TV3BlasterPlugin: Learn IR command timed-out");
            }

            _learnIRFilename = null;
            break;

          case MessageType.ServerShutdown:
            Log.Info("TV3BlasterPlugin: IR Server Shutdown - Plugin disabled until IR Server returns");
            _registered = false;
            break;

          case MessageType.Error:
            _learnIRFilename = null;
            Log.Error("TV3BlasterPlugin: Received error: {0}", received.GetDataAsString());
            break;
        }

        if (_handleMessage != null)
          _handleMessage(received);
      }
      catch (Exception ex)
      {
        Log.Error("TV3BlasterPlugin - ReveivedMessage(): {0}", ex.Message);
      }
    }

    /// <summary>
    /// events_OnTvServerEvent is used to receive requests to Tune External Channels.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="eventArgs">Event arguments.</param>
    void events_OnTvServerEvent(object sender, EventArgs eventArgs)
    {
      TvServerEventArgs tvEvent = (TvServerEventArgs)eventArgs;
      AnalogChannel analogChannel = tvEvent.channel as AnalogChannel;

      if (analogChannel == null)
        return;

      if (tvEvent.EventType == TvServerEventType.StartZapChannel)
      {
        if (LogVerbose)
          Log.Info("TV3BlasterPlugin: Card: {0}, Channel: {1}, {2}", tvEvent.Card.Id, analogChannel.ChannelNumber, analogChannel.Name);

        ProcessExternalChannel(analogChannel.ChannelNumber.ToString(), tvEvent.Card.Id);
      }
    }

    /// <summary>
    /// Load external channel configurations.
    /// </summary>
    internal static void LoadExternalConfigs()
    {
      IList cards = TvDatabase.Card.ListAll();

      if (cards.Count == 0)
        return;

      _externalChannelConfigs = new ExternalChannelConfig[cards.Count];

      int index = 0;
      string fileName;
      foreach (TvDatabase.Card card in cards)
      {
        fileName = String.Format("{0}ExternalChannelConfig{1}.xml", ExtCfgFolder, card.IdCard);
        try
        {
          _externalChannelConfigs[index] = ExternalChannelConfig.Load(fileName);
        }
        catch (Exception ex)
        {
          _externalChannelConfigs[index] = new ExternalChannelConfig(fileName);
          Log.Error(ex.ToString());
        }

        _externalChannelConfigs[index].CardId = card.IdCard;
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
    /// <param name="externalChannel">The external channel.</param>
    /// <param name="cardId">The tuner card ID.</param>
    static void ProcessExternalChannel(string externalChannel, int cardId)
    {
      ExternalChannelConfig config = GetExternalChannelConfig(cardId);

      // Clean up the "externalChannel" string into "channel".
      StringBuilder channel = new StringBuilder();
      foreach (char digit in externalChannel)
        if (char.IsDigit(digit))
          channel.Append(digit);

      // Pad the channel number with leading 0's to meet ChannelDigits length.
      while (channel.Length < config.ChannelDigits)
        channel.Insert(0, '0');

      // Process the channel and blast the relevant IR Commands.
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
            if (command.StartsWith(Common.CmdPrefixRun))
              ProcessExternalChannelProgram(command.Substring(Common.CmdPrefixRun.Length), -1, channel.ToString());
            else if (command.StartsWith(Common.CmdPrefixSerial))
              ProcessExternalSerialCommand(command.Substring(Common.CmdPrefixSerial.Length), -1, channel.ToString());
            else
              ProcessCommand(command, false);

            if (config.PauseTime > 0)
              Thread.Sleep(config.PauseTime);
          }
        }

        foreach (char digit in channel.ToString())
        {
          charVal = digit - 48;

          command = config.Digits[charVal];
          if (!String.IsNullOrEmpty(command))
          {
            if (command.StartsWith(Common.CmdPrefixRun))
              ProcessExternalChannelProgram(command.Substring(Common.CmdPrefixRun.Length), charVal, channel.ToString());
            else if (command.StartsWith(Common.CmdPrefixSerial))
              ProcessExternalSerialCommand(command.Substring(Common.CmdPrefixSerial.Length), charVal, channel.ToString());
            else
              ProcessCommand(command, false);

            if (config.PauseTime > 0)
              Thread.Sleep(config.PauseTime);
          }
        }

        if (config.SendSelect)
        {
          command = config.SelectCommand;
          if (!String.IsNullOrEmpty(command))
          {
            if (command.StartsWith(Common.CmdPrefixRun))
            {
              ProcessExternalChannelProgram(command.Substring(Common.CmdPrefixRun.Length), -1, channel.ToString());

              if (config.DoubleChannelSelect)
              {
                if (config.PauseTime > 0)
                  Thread.Sleep(config.PauseTime);

                ProcessExternalChannelProgram(command.Substring(Common.CmdPrefixRun.Length), -1, channel.ToString());
              }
            }
            else if (command.StartsWith(Common.CmdPrefixSerial))
            {
              ProcessExternalSerialCommand(command.Substring(Common.CmdPrefixSerial.Length), -1, channel.ToString());

              if (config.DoubleChannelSelect)
              {
                if (config.PauseTime > 0)
                  Thread.Sleep(config.PauseTime);

                ProcessExternalSerialCommand(command.Substring(Common.CmdPrefixSerial.Length), -1, channel.ToString());
              }
            }
            else
            {
              ProcessCommand(command, false);

              if (config.DoubleChannelSelect)
              {
                if (config.PauseTime > 0)
                  Thread.Sleep(config.PauseTime);

                ProcessCommand(command, false);
              }
            }
          }
        }
      }
    }

    /// <summary>
    /// Processes the external channel program.
    /// </summary>
    /// <param name="runCommand">The run command.</param>
    /// <param name="currentChannelDigit">The current channel digit.</param>
    /// <param name="fullChannelString">The full channel string.</param>
    static void ProcessExternalChannelProgram(string runCommand, int currentChannelDigit, string fullChannelString)
    {
      string[] commands = Common.SplitRunCommand(runCommand);

      commands[2] = commands[2].Replace("%1", currentChannelDigit.ToString());
      commands[2] = commands[2].Replace("%2", fullChannelString);

      Common.ProcessRunCommand(commands);
    }

    /// <summary>
    /// Processes the external serial command.
    /// </summary>
    /// <param name="serialCommand">The serial command.</param>
    /// <param name="currentChannelDigit">The current channel digit.</param>
    /// <param name="fullChannelString">The full channel string.</param>
    static void ProcessExternalSerialCommand(string serialCommand, int currentChannelDigit, string fullChannelString)
    {
      string[] commands = Common.SplitSerialCommand(serialCommand);

      commands[0] = commands[0].Replace("%1", currentChannelDigit.ToString());
      commands[0] = commands[0].Replace("%2", fullChannelString);

      Common.ProcessSerialCommand(commands);
    }

    /// <summary>
    /// Learn an IR command.
    /// </summary>
    /// <param name="fileName">File to place learned IR command in (absolute path).</param>
    /// <returns>true if successful, otherwise false.</returns>
    internal static bool LearnIR(string fileName)
    {
      try
      {
        if (String.IsNullOrEmpty(fileName))
        {
          Log.Error("TV3BlasterPlugin: Null or Empty file name for LearnIR()");
          return false;
        }

        if (!_registered)
        {
          Log.Error("TV3BlasterPlugin: Not registered to an active IR Server");
          return false;
        }

        if (_learnIRFilename != null)
        {
          Log.Error("TV3BlasterPlugin: Already trying to learn an IR command");
          return false;
        }

        _learnIRFilename = fileName;

        IrssMessage message = new IrssMessage(MessageType.LearnIR, MessageFlags.Request);
        _client.Send(message);
      }
      catch (Exception ex)
      {
        _learnIRFilename = null;
        Log.Error("TV3BlasterPlugin - LearnIR(): {0}", ex.Message);
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
    /// </summary>
    /// <param name="command">Command to process.</param>
    /// <param name="async">Process command asynchronously?</param>
    internal static void ProcessCommand(string command, bool async)
    {
      if (String.IsNullOrEmpty(command))
        throw new ArgumentNullException("command");

      if (async)
      {
        Thread newThread = new Thread(new ParameterizedThreadStart(ProcCommand));
        newThread.Name = "ProcessCommand";
        newThread.Priority = ThreadPriority.BelowNormal;
        newThread.Start(command);
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
      string command = commandObj as string;

      if (command.StartsWith(Common.CmdPrefixMacro, StringComparison.OrdinalIgnoreCase)) // Macro
      {
        string fileName = FolderMacros + command.Substring(Common.CmdPrefixMacro.Length) + Common.FileExtensionMacro;
        ProcMacro(fileName);
      }
      else if (command.StartsWith(Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))  // IR Code
      {
        string[] commands = Common.SplitBlastCommand(command.Substring(Common.CmdPrefixBlast.Length));
        BlastIR(Common.FolderIRCommands + commands[0] + Common.FileExtensionIR, commands[1]);
      }
      else if (command.StartsWith(Common.CmdPrefixSTB, StringComparison.OrdinalIgnoreCase))  // STB IR Code
      {
        string[] commands = Common.SplitBlastCommand(command.Substring(Common.CmdPrefixSTB.Length));
        BlastIR(Common.FolderSTB + commands[0] + Common.FileExtensionIR, commands[1]);
      }
      else if (command.StartsWith(Common.CmdPrefixRun, StringComparison.OrdinalIgnoreCase)) // External Program
      {
        string[] commands = Common.SplitRunCommand(command.Substring(Common.CmdPrefixRun.Length));
        Common.ProcessRunCommand(commands);
      }
      else if (command.StartsWith(Common.CmdPrefixSerial, StringComparison.OrdinalIgnoreCase)) // Serial Port Command
      {
        string[] commands = Common.SplitSerialCommand(command.Substring(Common.CmdPrefixSerial.Length));
        Common.ProcessSerialCommand(commands);
      }
      else if (command.StartsWith(Common.CmdPrefixWindowMsg, StringComparison.OrdinalIgnoreCase))  // Message Command
      {
        string[] commands = Common.SplitWindowMessageCommand(command.Substring(Common.CmdPrefixWindowMsg.Length));
        Common.ProcessWindowMessageCommand(commands);
      }
      else if (command.StartsWith(Common.CmdPrefixKeys, StringComparison.OrdinalIgnoreCase))  // Keystroke Command
      {
        string keyCommand = command.Substring(Common.CmdPrefixKeys.Length);
        if (InConfiguration)
          MessageBox.Show(keyCommand, Common.UITextKeys, MessageBoxButtons.OK, MessageBoxIcon.Information);
        else
          Common.ProcessKeyCommand(keyCommand);
      }
      else
      {
        throw new ArgumentException(String.Format("Cannot process unrecognized command \"{0}\"", command), "command");
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

        if (doc.DocumentElement.InnerText.Contains(Common.XmlTagBlast) && !_registered)
          throw new ApplicationException("Cannot process Macro with Blast commands when not registered to an active IR Server");

        XmlNodeList commandSequence = doc.DocumentElement.SelectNodes("action");
        string commandProperty;

        foreach (XmlNode item in commandSequence)
        {
          commandProperty = item.Attributes["cmdproperty"].Value;

          switch (item.Attributes["command"].Value)
          {
            case Common.XmlTagMacro:
              {
                ProcMacro(FolderMacros + commandProperty + Common.FileExtensionMacro);
                break;
              }

            case Common.XmlTagBlast:
              {
                string[] commands = Common.SplitBlastCommand(commandProperty);
                BlastIR(Common.FolderIRCommands + commands[0] + Common.FileExtensionIR, commands[1]);
                break;
              }

            case Common.XmlTagPause:
              {
                int sleep = int.Parse(commandProperty);
                Thread.Sleep(sleep);
                break;
              }

            case Common.XmlTagRun:
              {
                string[] commands = Common.SplitRunCommand(commandProperty);
                Common.ProcessRunCommand(commands);
                break;
              }

            case Common.XmlTagSerial:
              {
                string[] commands = Common.SplitSerialCommand(commandProperty);
                Common.ProcessSerialCommand(commands);
                break;
              }

            case Common.XmlTagWindowMsg:
              {
                string[] commands = Common.SplitWindowMessageCommand(commandProperty);
                Common.ProcessWindowMessageCommand(commands);
                break;
              }

            case Common.XmlTagTcpMsg:
              {
                string[] commands = Common.SplitTcpMessageCommand(commandProperty);
                Common.ProcessTcpMessageCommand(commands);
                break;
              }

            case Common.XmlTagKeys:
              {
                if (InConfiguration)
                  MessageBox.Show(commandProperty, Common.UITextKeys, MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                  Common.ProcessKeyCommand(commandProperty);
                break;
              }
          }
        }
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
      TvBusinessLayer layer = new TvBusinessLayer();
      ServerHost = layer.GetSetting("TV3BlasterPlugin_ServerHost", "localhost").Value;
      LogVerbose = Convert.ToBoolean(layer.GetSetting("TV3BlasterPlugin_LogVerbose", "False").Value);
    }

    #endregion Implementation

  }

}
