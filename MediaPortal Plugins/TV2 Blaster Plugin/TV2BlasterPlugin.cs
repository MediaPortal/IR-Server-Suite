using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
using MediaPortal.Configuration;
using MediaPortal.Util;

using NamedPipes;
using IrssUtils;
using MPUtils;

namespace MediaPortal.Plugins
{

  public class TV2BlasterPlugin : IPlugin, ISetupForm
  {

    #region Constants

    internal const string PluginVersion = "TV2 Blaster Plugin 1.0.3.2 for IR Server";

    internal static readonly string MPConfigFile = Config.GetFolder(Config.Dir.Config) + "\\MediaPortal.xml";

    internal static readonly string FolderMacros = Common.FolderAppData + "TV2 Blaster Plugin\\Macro\\";

    internal static readonly string ExtCfgFolder = Common.FolderAppData + "TV2 Blaster Plugin\\";

    #endregion Constants

    #region Variables

    static string _serverHost;
    static string _localPipeName = String.Empty;
    static string _learnIRFilename = null;

    static bool _registered = false;
    static bool _keepAlive = true;
    static int _echoID = -1;
    static Thread _keepAliveThread;

    static string _blasterSpeed;
    static string _blasterPort;
    static bool _logVerbose;

    static ExternalChannelConfig[] _externalChannelConfigs;

    static Common.MessageHandler _handleMessage;

    static bool _inConfiguration = false;

    static TransceiverInfo _transceiverInfo = new TransceiverInfo();

    #endregion Variables

    #region Properties

    internal static bool IsRegistered
    {
      get { return _registered; }
    }

    internal static string ServerHost
    {
      get { return _serverHost; }
      set { _serverHost = value; }
    }
    internal static string BlastSpeed
    {
      get { return _blasterSpeed; }
      set { _blasterSpeed = value; }
    }
    internal static string BlastPort
    {
      get { return _blasterPort; }
      set { _blasterPort = value; }
    }
    internal static bool LogVerbose
    {
      get { return _logVerbose; }
      set { _logVerbose = value; }
    }

    internal static ExternalChannelConfig[] ExternalChannelConfigs
    {
      get { return _externalChannelConfigs; }
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

    internal static TransceiverInfo TransceiverInformation
    {
      get { return _transceiverInfo; }
    }

    #endregion Properties

    #region IPlugin methods

    public void Start()
    {
      Log.Info("TV2BlasterPlugin: Starting ({0})", PluginVersion);

      // Load basic settings
      LoadSettings();

      InConfiguration = false;

      LoadExternalConfigs();

      if (!StartComms())
        Log.Error("TV2BlasterPlugin: Failed to start local comms, IR blasting is disabled for this session");

      // Register with MediaPortal to receive GUI Messages ...
      GUIWindowManager.Receivers += new SendMessageHandler(OnMessage);

      if (LogVerbose)
        Log.Info("TV2BlasterPlugin: Started");
    }
    public void Stop()
    {
      GUIWindowManager.Receivers -= new SendMessageHandler(OnMessage);

      StopComms();

      if (LogVerbose)
        Log.Info("TV2BlasterPlugin: Stopped");
    }

    #endregion IPlugin methods

    #region ISetupForm methods

    public bool CanEnable()       { return true; }
    public bool HasSetup()        { return true; }
    public string PluginName()    { return "TV2 STB Blaster Plugin for IR Server"; }
    public bool DefaultEnabled()  { return true; }
    public int GetWindowId()      { return 0; }
    public string Author()        { return "and-81"; }
    public string Description()   { return "External Channel Changer for TV Engine 2 using IR Server"; }

    public void ShowPlugin()
    {
      try
      {
        LoadSettings();
        LoadExternalConfigs();

        InConfiguration = true;

        if (LogVerbose)
          Log.Info("TV2BlasterPlugin: ShowPlugin()");

        SetupForm setupForm = new SetupForm();
        if (setupForm.ShowDialog() == DialogResult.OK)
          SaveSettings();

        StopComms();

        if (LogVerbose)
          Log.Info("TV2BlasterPlugin: ShowPlugin() - End");
      }
      catch (Exception ex)
      {
        Log.Error(ex);
      }
    }

    public bool GetHome(out string strButtonText, out string strButtonImage, out string strButtonImageFocus, out string strPictureImage)
    {
      strButtonText = strButtonImage = strButtonImageFocus = strPictureImage = String.Empty;
      return false;
    }

    #endregion ISetupForm methods

    #region Implementation

    internal static bool StartComms()
    {
      try
      {
        if (OpenLocalPipe())
        {
          _keepAliveThread = new Thread(new ThreadStart(KeepAliveThread));
          _keepAliveThread.Start();

          return true;
        }
      }
      catch (Exception ex)
      {
        Log.Error(ex);
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

          PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Unregister", null);
          PipeAccess.SendMessage(Common.ServerPipeName, _serverHost, message.ToString());
        }
      }
      catch { }

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
          string localPipeTest = String.Format(Common.LocalPipeFormat, pipeNumber);

          if (PipeAccess.PipeExists(String.Format("\\\\.\\pipe\\{0}", localPipeTest)))
          {
            if (++pipeNumber <= Common.MaximumLocalClientCount)
              retry = true;
            else
              throw new Exception(String.Format("Maximum local client limit ({0}) reached", Common.MaximumLocalClientCount));
          }
          else
          {
            if (!PipeAccess.StartServer(localPipeTest, new PipeMessageHandler(ReceivedMessage)))
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
        Log.Error(ex);
        return false;
      }
    }

    static bool ConnectToServer()
    {
      try
      {
        PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Register", null);
        PipeAccess.SendMessage(Common.ServerPipeName, _serverHost, message.ToString());
        return true;
      }
      catch (AppModule.NamedPipes.NamedPipeIOException)
      {
        return false;
      }
      catch (Exception ex)
      {
        Log.Error(ex);
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

        Log.Debug("TV2BlasterPlugin: Connecting ({0}) ...", _serverHost);
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
          Log.Debug("TV2BlasterPlugin: Connected ({0})", _serverHost);

        #endregion Registered ...

        #region Ping the server repeatedly

        while (_keepAlive && _registered && !reconnect)
        {
          int pingID = random.Next();
          long pingTime = DateTime.Now.Ticks;

          try
          {
            PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Ping", BitConverter.GetBytes(pingID));
            PipeAccess.SendMessage(Common.ServerPipeName, _serverHost, message.ToString());
          }
          catch
          {
            // Failed to ping ... reconnect ...
            Log.Warn("TV2BlasterPlugin: Failed to ping, attempting to reconnect ...");
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
            Log.Warn("TV2BlasterPlugin: No echo to ping, attempting to reconnect ...");

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
        Log.Debug("TV2BlasterPlugin: Received Message \"{0}\"", received.Name);

      try
      {
        switch (received.Name)
        {
          case "Start Learn":
          case "Blast Success":
          case "Remote Button":
            break;

          case "Blast Failure":
            {
              Log.Error("TV2BlasterPlugin: Failed to blast IR command");
              break;
            }

          case "Register Success":
            {
              if (LogVerbose)
                Log.Info("TV2BlasterPlugin: Registered to IR Server");
              
              _registered = true;
              _transceiverInfo = TransceiverInfo.FromBytes(received.Data);
              break;
            }

          case "Register Failure":
            {
              Log.Warn("TV2BlasterPlugin: IR Server refused to register");
              _registered = false;
              break;
            }

          case "Learn Success":
            {
              if (LogVerbose)
                Log.Info("TV2BlasterPlugin: Learned IR Successfully \"{0}\"", _learnIRFilename);

              FileStream file = new FileStream(_learnIRFilename, FileMode.Create, FileAccess.Write, FileShare.None);
              file.Write(received.Data, 0, received.Data.Length);
              file.Flush();
              file.Close();

              _learnIRFilename = null;
              break;
            }

          case "Learn Failure":
            {
              Log.Error("TV2BlasterPlugin: Failed to learn IR command");

              _learnIRFilename = null;
              break;
            }

          case "Server Shutdown":
            {
              Log.Warn("TV2BlasterPlugin: IR Server Shutdown - Plugin disabled until IR Server returns");
              _registered = false;
              break;
            }

          case "Echo":
            {
              _echoID = BitConverter.ToInt32(received.Data, 0);
              break;
            }

          case "Error":
            {
              Log.Error("TV2BlasterPlugin: Received error: {0}", Encoding.ASCII.GetString(received.Data));
              break;
            }

          default:
            {
              Log.Debug("TV2BlasterPlugin: Unknown message received: {0}", received.Name);
              break;
            }
        }

        if (_handleMessage != null)
          _handleMessage(message);
      }
      catch (Exception ex)
      {
        Log.Error("TV2BlasterPlugin - ReveivedMessage(): {0}", ex.Message);
      }
    }

    /// <summary>
    /// OnMessage is used to receive requests to Tune External Channels and for event mapping.
    /// </summary>
    /// <param name="msg">Message</param>
    void OnMessage(GUIMessage msg)
    {
      if (msg.Message == GUIMessage.MessageType.GUI_MSG_TUNE_EXTERNAL_CHANNEL)
      {
        if (LogVerbose)
          Log.Info("TV2BlasterPlugin: Tune External Channel: {0}, Tuner card: {1}", msg.Label, msg.Label2);

        ProcessExternalChannel(msg.Label, msg.Label2);
      }
    }

    static void LoadExternalConfigs()
    {
      ArrayList cards = new ArrayList();
      MediaPortal.TV.Database.TVDatabase.GetCards(ref cards);

      int cardCount = cards.Count;
      if (cardCount == 0)
        cardCount = 1;

      _externalChannelConfigs = new ExternalChannelConfig[cardCount];

      string fileName;
      for (int index = 0; index < cardCount; index++)
      {
        fileName = String.Format("{0}ExternalChannelConfig{1}.xml", ExtCfgFolder, Convert.ToString(index + 1));
        try
        {
          ExternalChannelConfigs[index] = ExternalChannelConfig.Load(fileName);
        }
        catch (Exception ex)
        {
          ExternalChannelConfigs[index] = new ExternalChannelConfig(fileName);
          Log.Error(ex);
        }
        
        ExternalChannelConfigs[index].CardId = index;
      }
    }

    static void LoadSettings()
    {
      try
      {
        using (MediaPortal.Profile.Settings xmlreader = new MediaPortal.Profile.Settings(MPConfigFile))
        {
          ServerHost = xmlreader.GetValueAsString("TV2BlasterPlugin", "ServerHost", String.Empty);

          BlastSpeed = xmlreader.GetValueAsString("TV2BlasterPlugin", "BlastSpeed", "None");
          BlastPort = xmlreader.GetValueAsString("TV2BlasterPlugin", "BlastPort", "None");
          LogVerbose = xmlreader.GetValueAsBool("TV2BlasterPlugin", "LogVerbose", false);
        }
      }
      catch (Exception ex)
      {
        Log.Error("TV2BlasterPlugin: LoadSettings() {0}", ex.Message);
      }
    }
    static void SaveSettings()
    {
      try
      {
        using (MediaPortal.Profile.Settings xmlwriter = new MediaPortal.Profile.Settings(MPConfigFile))
        {
          xmlwriter.SetValue("TV2BlasterPlugin", "ServerHost", ServerHost);

          xmlwriter.SetValue("TV2BlasterPlugin", "BlastSpeed", BlastSpeed);
          xmlwriter.SetValue("TV2BlasterPlugin", "BlastPort", BlastPort);
          xmlwriter.SetValueAsBool("TV2BlasterPlugin", "LogVerbose", LogVerbose);
        }
      }
      catch (Exception ex)
      {
        Log.Error("TV2BlasterPlugin: SaveSettings() {0}", ex.Message);
      }
    }

    static void ProcessExternalChannel(string externalChannel, string tunerCard)
    {
      int card = int.Parse(tunerCard);

      // To work around a known bug in MediaPortal scheduled recording (Added: 25-Feb-2007)
      if (card < 0)
        card = 0;

      if (card >= ExternalChannelConfigs.Length)
        throw new ArgumentException("Card number is higher than last card in list, reconfigure plugin TV cards.");

      ExternalChannelConfig config = ExternalChannelConfigs[card];

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
              BlastIR(Common.FolderIRCommands + commands[0] + Common.FileExtensionIR, commands[1], commands[2]);
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

          case Common.XmlTagPopup:
            {
              string[] commands = Common.SplitPopupCommand(commandProperty);

              if (InConfiguration)
                MessageBox.Show(commands[1], commands[0], MessageBoxButtons.OK, MessageBoxIcon.Information);
              else
                MPCommands.ShowNotifyDialog(commands[0], commands[1], int.Parse(commands[2]));

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
    /// <param name="speed">Speed to blast at.</param>
    internal static void BlastIR(string fileName, string port, string speed)
    {
      if (!_registered)
        throw new Exception("Cannot Blast, not registered to an active IR Server");

      FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
      if (file.Length == 0)
        throw new Exception(String.Format("Cannot Blast, IR file \"{0}\" has no data, possible IR learn failure", fileName));

      byte[] outData = new byte[8 + port.Length + speed.Length + file.Length];

      BitConverter.GetBytes(port.Length).CopyTo(outData, 0);
      Encoding.ASCII.GetBytes(port).CopyTo(outData, 4);
      BitConverter.GetBytes(speed.Length).CopyTo(outData, 4 + port.Length);
      Encoding.ASCII.GetBytes(speed).CopyTo(outData, 8 + port.Length);

      file.Read(outData, 8 + port.Length + speed.Length, (int)file.Length);
      file.Close();

      PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Blast", outData);
      PipeAccess.SendMessage(Common.ServerPipeName, ServerHost, message.ToString());
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

        string fileName = Common.FolderIRCommands + commands[0] + Common.FileExtensionIR;
        BlastIR(fileName, commands[1], commands[2]);
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
    /// Learn an IR Command and put it in a file
    /// </summary>
    /// <param name="fileName">File to place learned IR command in.</param>
    /// <returns>Success.</returns>
    internal static bool LearnIRCommand(string fileName)
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

        PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Learn", null);
        PipeAccess.SendMessage(Common.ServerPipeName, ServerHost, message.ToString());

        return true;
      }
      catch (Exception ex)
      {
        _learnIRFilename = null;
        Log.Error("TV2BlasterPlugin - LearnIRCommand(): {0}", ex.Message);
        return false;
      }
    }

    /// <summary>
    /// Returns a list of Macros
    /// </summary>
    /// <returns>string[] of Macros</returns>
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
    /// Returns a combined list of IR Commands and Macros
    /// </summary>
    /// <returns>string[] of IR Commands and Macros</returns>
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
