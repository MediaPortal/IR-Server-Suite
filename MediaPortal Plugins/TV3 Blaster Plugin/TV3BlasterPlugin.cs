using System;
using System.Collections;
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

  public class TV3BlasterPlugin : ITvServerPlugin
  {

    #region Constants

    internal const string PluginVersion = "TV3 Blaster Plugin 1.0.3.4 for IR Server";

    internal static readonly string FolderMacros = Common.FolderAppData + "TV3 Blaster Plugin\\Macro\\";

    internal static readonly string ExtCfgFolder = Common.FolderAppData + "TV3 Blaster Plugin\\";

    #endregion Constants

    #region Variables

    static Client _client = null;

    static string _serverHost;
    static string _learnIRFilename = null;

    static bool _registered = false;

    static bool _logVerbose;

    static ExternalChannelConfig[] _externalChannelConfigs;

    static ClientMessageSink _handleMessage;

    static bool _inConfiguration = false;

    TvServerEventHandler _eventHandler;

    static IRServerInfo _irServerInfo = new IRServerInfo();

    #endregion Variables

    #region Properties

    /// <summary>
    /// Returns the name of the plugin.
    /// </summary>
    public string Name { get { return "TV3 Blaster Plugin for IR Server"; } }
    /// <summary>
    /// Returns the version of the plugin.
    /// </summary>
    public string Version { get { return "1.0.3.4"; } }
    /// <summary>
    /// Returns the author of the plugin.
    /// </summary>
    public string Author { get { return "and-81"; } }
    /// <summary>
    /// Returns if the plugin should only run on the master server or also on slave servers.
    /// </summary>
    public bool MasterOnly { get { return false; } }

    internal static bool IsRegistered
    {
      get { return _registered; }
    }

    internal static string ServerHost
    {
      get { return _serverHost; }
      set { _serverHost = value; }
    }
    internal static bool LogVerbose
    {
      get { return _logVerbose; }
      set { _logVerbose = value; }
    }

    internal static ClientMessageSink HandleMessage
    {
      get { return _handleMessage; }
      set { _handleMessage = value; }
    }

    internal static bool InConfiguration
    {
      get { return _inConfiguration; }
      set { _inConfiguration = value; }
    }

    internal static IRServerInfo TransceiverInformation
    {
      get { return _irServerInfo; }
    }

    #endregion Properties

    #region ITvServerPlugin methods

    [CLSCompliant(false)]
    public void Start(IController controller)
    {
      Log.Info("TV3BlasterPlugin: Starting ({0})", PluginVersion);

      InConfiguration = false;

      TvBusinessLayer layer = new TvBusinessLayer();
      LogVerbose = Convert.ToBoolean(layer.GetSetting("TV3BlasterPlugin_LogVerbose", "False").Value);
      ServerHost = layer.GetSetting("TV3BlasterPlugin_ServerHost", Environment.MachineName).Value;

      LoadExternalConfigs();

      ITvServerEvent events = GlobalServiceProvider.Instance.Get<ITvServerEvent>();
      _eventHandler = new TvServerEventHandler(events_OnTvServerEvent);
      events.OnTvServerEvent += _eventHandler;

      IPAddress serverIP = Client.GetIPFromName(_serverHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, IrssComms.Server.DefaultPort);

      if (!StartClient(endPoint))
        Log.Error("TV3BlasterPlugin: Failed to start local comms, IR blasting is disabled for this session");

      if (LogVerbose)
        Log.Info("TV3BlasterPlugin: Started");
    }
    public void Stop()
    {
      ITvServerEvent events = GlobalServiceProvider.Instance.Get<ITvServerEvent>();
      events.OnTvServerEvent -= _eventHandler;

      StopClient();

      if (LogVerbose)
        Log.Info("TV3BlasterPlugin: Stopped");
    }

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
      if (_client == null)
        return;

      _client.Dispose();
      _client = null;
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

      // Process the channel and blast the relevant IR codes.
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
              ProcessCommand(command);

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
              ProcessCommand(command);

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
              ProcessCommand(command);

              if (config.DoubleChannelSelect)
              {
                if (config.PauseTime > 0)
                  Thread.Sleep(config.PauseTime);

                ProcessCommand(command);
              }
            }
          }
        }
      }
    }

    static void ProcessExternalChannelProgram(string runCommand, int currentChannelDigit, string fullChannelString)
    {
      string[] commands = Common.SplitRunCommand(runCommand);

      commands[2] = commands[2].Replace("%1", currentChannelDigit.ToString());
      commands[2] = commands[2].Replace("%2", fullChannelString);

      Common.ProcessRunCommand(commands);
    }

    static void ProcessExternalSerialCommand(string serialCommand, int currentChannelDigit, string fullChannelString)
    {
      string[] commands = Common.SplitSerialCommand(serialCommand);

      commands[0] = commands[0].Replace("%1", currentChannelDigit.ToString());
      commands[0] = commands[0].Replace("%2", fullChannelString);

      Common.ProcessSerialCommand(commands);
    }

    /// <summary>
    /// Process the supplied Macro file.
    /// </summary>
    /// <param name="fileName">Macro file to process (absolute path).</param>
    internal static void ProcessMacro(string fileName)
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(fileName);

      XmlNodeList commandSequence = doc.DocumentElement.SelectNodes("action");
      string commandProperty;
      string commandName;

      foreach (XmlNode item in commandSequence)
      {
        commandName = item.Attributes["command"].Value;
        commandProperty = item.Attributes["cmdproperty"].Value;

        switch (commandName)
        {
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

          case Common.XmlTagKeys:
            {
              if (InConfiguration)
                MessageBox.Show(commandProperty, "Keystroke Command", MessageBoxButtons.OK, MessageBoxIcon.Information);
              else
                Common.ProcessKeyCommand(commandProperty);
              break;
            }
        }
      }
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
    internal static void ProcessCommand(string command)
    {
      if (String.IsNullOrEmpty(command))
        throw new ArgumentNullException("command");

      if (command.StartsWith(Common.CmdPrefixMacro)) // Macro
      {
        string fileName = FolderMacros + command.Substring(Common.CmdPrefixMacro.Length) + Common.FileExtensionMacro;
        ProcessMacro(fileName);
      }
      else if (command.StartsWith(Common.CmdPrefixBlast))  // IR Code
      {
        string[] commands = Common.SplitBlastCommand(command.Substring(Common.CmdPrefixBlast.Length));
        BlastIR(Common.FolderIRCommands + commands[0] + Common.FileExtensionIR, commands[1]);
      }
      else if (command.StartsWith(Common.CmdPrefixSTB))  // STB IR Code
      {
        string[] commands = Common.SplitBlastCommand(command.Substring(Common.CmdPrefixSTB.Length));
        BlastIR(Common.FolderSTB + commands[0] + Common.FileExtensionIR, commands[1]);
      }
      else if (command.StartsWith(Common.CmdPrefixRun)) // External Program
      {
        string[] commands = Common.SplitRunCommand(command.Substring(Common.CmdPrefixRun.Length));
        Common.ProcessRunCommand(commands);
      }
      else if (command.StartsWith(Common.CmdPrefixSerial)) // Serial Port Command
      {
        string[] commands = Common.SplitSerialCommand(command.Substring(Common.CmdPrefixSerial.Length));
        Common.ProcessSerialCommand(commands);
      }
      else if (command.StartsWith(Common.CmdPrefixWindowMsg))  // Message Command
      {
        string[] commands = Common.SplitWindowMessageCommand(command.Substring(Common.CmdPrefixWindowMsg.Length));
        Common.ProcessWindowMessageCommand(commands);
      }
      else if (command.StartsWith(Common.CmdPrefixKeys))  // Keystroke Command
      {
        string keyCommand = command.Substring(Common.CmdPrefixKeys.Length);
        if (InConfiguration)
          MessageBox.Show(keyCommand, "Keystroke Command", MessageBoxButtons.OK, MessageBoxIcon.Information);
        else
          Common.ProcessKeyCommand(keyCommand);
      }
      else
      {
        throw new ArgumentException(String.Format("Cannot process unrecognized command \"{0}\"", command), "command");
      }
    }

    /// <summary>
    /// Learn an IR Command and put it in a file.
    /// </summary>
    /// <param name="fileName">File to place learned IR command in.</param>
    /// <returns>Success.</returns>
    internal static bool LearnIRCommand(string fileName)
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

        return true;
      }
      catch (Exception ex)
      {
        _learnIRFilename = null;
        Log.Error("TV3BlasterPlugin - LearnIRCommand(): {0}", ex.Message);
        return false;
      }
    }

    /// <summary>
    /// Returns a list of Macros.
    /// </summary>
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

    #endregion Implementation

  }

}
