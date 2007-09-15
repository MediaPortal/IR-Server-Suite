using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
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

using NamedPipes;
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

    static MessageQueue _messageQueue = new MessageQueue(new MessageQueueSink(ReceivedMessage));

    static string _serverHost;
    static string _localPipeName = String.Empty;
    static string _learnIRFilename = null;

    static bool _registered = false;
    static bool _keepAlive = true;
    static int _echoID = -1;
    static Thread _keepAliveThread;

    static bool _logVerbose;

    static ExternalChannelConfig[] _externalChannelConfigs;

    static Common.MessageHandler _handleMessage;

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

    internal static bool LogVerbose
    {
      get { return _logVerbose; }
      set { _logVerbose = value; }
    }

    internal static string ServerHost
    {
      get { return _serverHost; }
      set { _serverHost = value; }
    }

    internal static string LocalPipeName
    {
      get { return _localPipeName; }
      set { _localPipeName = value; }
    }

    internal static Common.MessageHandler HandleMessage
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
      Log.Info("TV3BlasterPlugin: Version {0}", PluginVersion);

      InConfiguration = false;

      TvBusinessLayer layer = new TvBusinessLayer();
      LogVerbose = Convert.ToBoolean(layer.GetSetting("TV3BlasterPlugin_LogVerbose", "False").Value);
      ServerHost = layer.GetSetting("TV3BlasterPlugin_ServerHost", Environment.MachineName).Value;

      LoadExternalConfigs();

      ITvServerEvent events = GlobalServiceProvider.Instance.Get<ITvServerEvent>();
      _eventHandler = new TvServerEventHandler(events_OnTvServerEvent);
      events.OnTvServerEvent += _eventHandler;

      if (!StartComms())
        Log.Error("TV3BlasterPlugin: Failed to start local comms, IR blasting is disabled for this session");

      if (LogVerbose)
        Log.Info("TV3BlasterPlugin: Started");
    }

    public void Stop()
    {
      ITvServerEvent events = GlobalServiceProvider.Instance.Get<ITvServerEvent>();
      events.OnTvServerEvent -= _eventHandler;

      StopComms();

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

    internal static bool StartComms()
    {
      try
      {
        if (OpenLocalPipe())
        {
          _messageQueue.Start();

          _keepAliveThread = new Thread(new ThreadStart(KeepAliveThread));
          _keepAliveThread.Start();

          return true;
        }
      }
      catch (Exception ex)
      {
        Log.Error(ex.ToString());
      }

      return false;
    }
    internal static void StopComms()
    {
      _keepAlive = false;

      try
      {
        if (_keepAliveThread != null && _keepAliveThread.IsAlive)
          _keepAliveThread.Abort();
      }
      catch { }

      try
      {
        if (_registered)
        {
          _registered = false;

          PipeMessage message = new PipeMessage(Environment.MachineName, _localPipeName, PipeMessageType.UnregisterClient, PipeMessageFlags.Request);
          PipeAccess.SendMessage(Common.ServerPipeName, _serverHost, message);
        }
      }
      catch { }

      _messageQueue.Stop();

      try
      {
        if (PipeAccess.ServerRunning)
          PipeAccess.StopServer();
      }
      catch { }
    }

    static bool OpenLocalPipe()
    {
      try
      {
        int pipeNumber = 1;
        bool retry = false;

        do
        {
          string localPipeTest = String.Format("irserver\\mptv3-{0:00}", pipeNumber);

          if (PipeAccess.PipeExists(Common.LocalPipePrefix + localPipeTest))
          {
            if (++pipeNumber <= Common.MaximumLocalClientCount)
              retry = true;
            else
              throw new Exception(String.Format("Maximum local client limit ({0}) reached", Common.MaximumLocalClientCount));
          }
          else
          {
            if (!PipeAccess.StartServer(localPipeTest, new PipeMessageHandler(_messageQueue.Enqueue)))
              throw new Exception(String.Format("Failed to start local pipe server \"{0}\"", localPipeTest));

            _localPipeName = localPipeTest;
            retry = false;
          }
        }
        while (retry);

        return true;
      }
      catch (Exception ex)
      {
        Log.Error(ex.ToString());
        return false;
      }
    }

    static bool ConnectToServer()
    {
      try
      {
        PipeMessage message = new PipeMessage(Environment.MachineName, _localPipeName, PipeMessageType.RegisterClient, PipeMessageFlags.Request);
        PipeAccess.SendMessage(Common.ServerPipeName, _serverHost, message);
        return true;
      }
      catch (AppModule.NamedPipes.NamedPipeIOException)
      {
        return false;
      }
      catch (Exception ex)
      {
        Log.Error(ex.ToString());
        return false;
      }
    }

    static void KeepAliveThread()
    {
      Random random = new Random((int)DateTime.Now.Ticks);
      bool reconnect;
      int attempt;

      _keepAlive = true;
      while (_keepAlive)
      {
        reconnect = true;

        #region Connect to server

        Log.Debug("TV3BlasterPlugin: Connecting ({0}) ...", _serverHost);
        attempt = 0;
        while (_keepAlive && reconnect)
        {
          if (ConnectToServer())
          {
            reconnect = false;
          }
          else
          {
            int wait;

            if (attempt <= 50)
              attempt++;

            if (attempt > 50)
              wait = 30;      // 30 seconds
            else if (attempt > 20)
              wait = 10;      // 10 seconds
            else if (attempt > 10)
              wait = 5;       // 5 seconds
            else
              wait = 1;       // 1 second

            for (int sleeps = 0; sleeps < wait && _keepAlive; sleeps++)
              Thread.Sleep(1000);
          }
        }

        #endregion Connect to server

        #region Wait for registered

        // Give up after 10 seconds ...
        attempt = 0;
        while (_keepAlive && !_registered && !reconnect)
        {
          if (++attempt >= 10)
            reconnect = true;
          else
            Thread.Sleep(1000);
        }

        #endregion Wait for registered

        #region Registered ...

        if (_keepAlive && _registered && !reconnect)
          Log.Debug("TV3BlasterPlugin: Connected ({0})", _serverHost);

        #endregion Registered ...

        #region Ping the server repeatedly

        while (_keepAlive && _registered && !reconnect)
        {
          int pingID = random.Next();
          long pingTime = DateTime.Now.Ticks;

          try
          {
            PipeMessage message = new PipeMessage(Environment.MachineName, _localPipeName, PipeMessageType.Ping, PipeMessageFlags.Request, BitConverter.GetBytes(pingID));
            PipeAccess.SendMessage(Common.ServerPipeName, _serverHost, message);
          }
          catch
          {
            // Failed to ping ... reconnect ...
            Log.Error("TV3BlasterPlugin: Failed to ping, attempting to reconnect ...");
            _registered = false;
            reconnect = true;
            break;
          }

          // Wait 10 seconds for a ping echo ...
          bool receivedEcho = false;
          while (_keepAlive && _registered && !reconnect &&
            !receivedEcho && DateTime.Now.Ticks - pingTime < 10 * 1000 * 10000)
          {
            if (_echoID == pingID)
            {
              receivedEcho = true;
            }
            else
            {
              Thread.Sleep(1000);
            }
          }

          if (receivedEcho) // Received ping echo ...
          {
            // Wait 60 seconds before re-pinging ...
            for (int sleeps = 0; sleeps < 60 && _keepAlive && _registered; sleeps++)
              Thread.Sleep(1000);
          }
          else // Didn't receive ping echo ...
          {
            Log.Error("TV3BlasterPlugin: No echo to ping, attempting to reconnect ...");

            // Break out of pinging cycle ...
            _registered = false;
            reconnect = true;
          }
        }

        #endregion Ping the server repeatedly

      }

    }

    static void ReceivedMessage(string message)
    {
      PipeMessage received = PipeMessage.FromString(message);

      if (LogVerbose)
        Log.Debug("TV3BlasterPlugin: Received Message \"{0}\"", received.Type);

      try
      {
        switch (received.Type)
        {
          case PipeMessageType.BlastIR:
            if ((received.Flags & PipeMessageFlags.Success) == PipeMessageFlags.Success)
            {
              if (LogVerbose)
                Log.Info("TV3BlasterPlugin: Blast successful");
            }
            else if ((received.Flags & PipeMessageFlags.Failure) == PipeMessageFlags.Failure)
            {
              Log.Error("TV3BlasterPlugin: Failed to blast IR command");
            }
            break;

          case PipeMessageType.RegisterClient:
            if ((received.Flags & PipeMessageFlags.Success) == PipeMessageFlags.Success)
            {
              _irServerInfo = IRServerInfo.FromBytes(received.DataAsBytes);
              _registered = true;

              if (LogVerbose)
                Log.Info("TV3BlasterPlugin: Registered to IR Server");
            }
            else if ((received.Flags & PipeMessageFlags.Failure) == PipeMessageFlags.Failure)
            {
              _registered = false;
              Log.Error("TV3BlasterPlugin: IR Server refused to register");
            }
            break;

          case PipeMessageType.LearnIR:
            if ((received.Flags & PipeMessageFlags.Success) == PipeMessageFlags.Success)
            {
              if (LogVerbose)
                Log.Info("TV3BlasterPlugin: Learned IR Successfully");

              byte[] dataBytes = received.DataAsBytes;

              FileStream file = new FileStream(_learnIRFilename, FileMode.Create);
              file.Write(dataBytes, 0, dataBytes.Length);
              file.Close();
            }
            else if ((received.Flags & PipeMessageFlags.Failure) == PipeMessageFlags.Failure)
            {
              Log.Error("TV3BlasterPlugin: Failed to learn IR command");
            }
            else if ((received.Flags & PipeMessageFlags.Timeout) == PipeMessageFlags.Timeout)
            {
              Log.Error("TV3BlasterPlugin: Learn IR command timed-out");
            }
            
            _learnIRFilename = null;
            break;

          case PipeMessageType.ServerShutdown:
            Log.Info("TV3BlasterPlugin: IR Server Shutdown - Plugin disabled until IR Server returns");
            _registered = false;
            break;

          case PipeMessageType.Echo:
            _echoID = BitConverter.ToInt32(received.DataAsBytes, 0);
            break;

          case PipeMessageType.Error:
            _learnIRFilename = null;
            Log.Error("TV3BlasterPlugin: Received error: {0}", received.DataAsString);
            break;
        }

        if (_handleMessage != null)
          _handleMessage(message);
      }
      catch (Exception ex)
      {
        Log.Error("TV3BlasterPlugin - ReveivedMessage(): {0}", ex.Message);
      }
    }

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
        throw new Exception("Cannot Blast, not registered to an active IR Server");

      FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
      if (file.Length == 0)
        throw new Exception(String.Format("Cannot Blast, IR file \"{0}\" has no data, possible IR learn failure", fileName));

      byte[] outData = new byte[4 + port.Length + file.Length];

      BitConverter.GetBytes(port.Length).CopyTo(outData, 0);
      Encoding.ASCII.GetBytes(port).CopyTo(outData, 4);

      file.Read(outData, 4 + port.Length, (int)file.Length);
      file.Close();

      PipeMessage message = new PipeMessage(Environment.MachineName, _localPipeName, PipeMessageType.BlastIR, PipeMessageFlags.Request, outData);
      PipeAccess.SendMessage(Common.ServerPipeName, ServerHost, message);
    }

    /// <summary>
    /// Given a command this method processes the request accordingly.
    /// </summary>
    /// <param name="command">Command to process.</param>
    internal static void ProcessCommand(string command)
    {
      if (String.IsNullOrEmpty(command))
        throw new ArgumentException("Null or empty argument", "command");

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
        BlastIR(Common.FolderSTB + commands[0], commands[1]);
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

        PipeMessage message = new PipeMessage(Environment.MachineName, _localPipeName, PipeMessageType.LearnIR, PipeMessageFlags.Request);
        PipeAccess.SendMessage(Common.ServerPipeName, ServerHost, message);

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
