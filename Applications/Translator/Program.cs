using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Win32;

using NamedPipes;
using IrssUtils;

namespace Translator
{

  static class Program
  {

    #region Enumerations

    /// <summary>
    /// A list of MCE remote buttons
    /// </summary>
    internal enum MceButton
    {
      Custom = -1,
      None = 0,
      TV_Power = 0x7b9a,
      Blue = 0x7ba1,
      Yellow = 0x7ba2,
      Green = 0x7ba3,
      Red = 0x7ba4,
      Teletext = 0x7ba5,
      Radio = 0x7baf,
      Print = 0x7bb1,
      Videos = 0x7bb5,
      Pictures = 0x7bb6,
      Recorded_TV = 0x7bb7,
      Music = 0x7bb8,
      TV = 0x7bb9,
      Guide = 0x7bd9,
      Live_TV = 0x7bda,
      DVD_Menu = 0x7bdb,
      Back = 0x7bdc,
      OK = 0x7bdd,
      Right = 0x7bde,
      Left = 0x7bdf,
      Down = 0x7be0,
      Up = 0x7be1,
      Star = 0x7be2,
      Hash = 0x7be3,
      Replay = 0x7be4,
      Skip = 0x7be5,
      Stop = 0x7be6,
      Pause = 0x7be7,
      Record = 0x7be8,
      Play = 0x7be9,
      Rewind = 0x7bea,
      Forward = 0x7beb,
      Channel_Down = 0x7bec,
      Channel_Up = 0x7bed,
      Volume_Down = 0x7bee,
      Volume_Up = 0x7bef,
      Info = 0x7bf0,
      Mute = 0x7bf1,
      Start = 0x7bf2,
      PC_Power = 0x7bf3,
      Enter = 0x7bf4,
      Escape = 0x7bf5,
      Number_9 = 0x7bf6,
      Number_8 = 0x7bf7,
      Number_7 = 0x7bf8,
      Number_6 = 0x7bf9,
      Number_5 = 0x7bfa,
      Number_4 = 0x7bfb,
      Number_3 = 0x7bfc,
      Number_2 = 0x7bfd,
      Number_1 = 0x7bfe,
      Number_0 = 0x7bff,
    }

    #endregion Enumerations

    #region Constants

    internal static readonly string ConfigFile = Common.FolderAppData + "Translator\\Translator.xml";

    internal static readonly string FolderMacros = Common.FolderAppData + "Translator\\Macro\\";

    #endregion Constants

    #region Variables

    static Configuration _config;

    static string _localPipeName = String.Empty;
    static string _learnIRFilename = null;

    static bool _registered = false;
    static bool _keepAlive = true;
    static int _echoID = -1;
    static Thread _keepAliveThread;

    static Common.MessageHandler _handleMessage;

    static bool _inConfiguration;

    static TransceiverInfo _transceiverInfo = new TransceiverInfo();

    #endregion Variables

    #region Properties

    internal static Configuration Config
    {
      get { return _config; }
      set { _config = value; }
    }

    internal static Common.MessageHandler HandleMessage
    {
      get { return _handleMessage; }
      set { _handleMessage = value; }
    }

    internal static TransceiverInfo TransceiverInformation
    {
      get { return _transceiverInfo; }
    }

    #endregion Properties

    #region Components

    static NotifyIcon _notifyIcon;
    static MainForm _mainForm;

    #endregion Components

    #region Main

    [STAThread]
    static void Main()
    {
      // Check for multiple instances.
      if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length != 1)
        return;

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      IrssLog.LogLevel = IrssLog.Level.Debug;
      IrssLog.Open(Common.FolderIrssLogs + "Translator.log");

      IrssLog.Debug("Platform is {0}", (IntPtr.Size == 4 ? "32-bit" : "64-bit"));

      _config = Configuration.Load(ConfigFile);
      if (_config == null)
        _config = new Configuration();

      if (String.IsNullOrEmpty(_config.ServerHost))
      {
        IrssUtils.Forms.ServerAddress serverAddress = new IrssUtils.Forms.ServerAddress(_config.ServerHost);

        if (serverAddress.ShowDialog() == DialogResult.OK)
        {
          _config.ServerHost = serverAddress.ServerHost;
          Configuration.Save(_config, ConfigFile);
        }
      }

      // Setup notify icon ...
      _notifyIcon = new NotifyIcon();
      _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
      _notifyIcon.Icon = Properties.Resources.Icon16Connecting;
      _notifyIcon.Text = "Translator - Connecting ...";
      _notifyIcon.DoubleClick += new EventHandler(ClickSetup);
      _notifyIcon.Visible = false;

      // Setup main form ...
      _mainForm = new MainForm();

      // Start server communications ...
      if (StartComms())
      {
        // Setup event notification ...
        SystemEvents.SessionEnding += new SessionEndingEventHandler(SystemEvents_SessionEnding);
        SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);

        Application.Run();

        SystemEvents.SessionEnding -= new SessionEndingEventHandler(SystemEvents_SessionEnding);
        SystemEvents.PowerModeChanged -= new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);
      }
      else
      {
        MessageBox.Show("Failed to start IR Server communications, refer to log file for more details.", "Translator - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      StopComms();

      IrssLog.Close();
    }

    #endregion Main

    #region Implementation

    static void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
    {
      switch (e.Mode)
      {
        case PowerModes.Resume:
          MapEvent(MappingEvent.PC_Resume);
          break;

        case PowerModes.Suspend:
          MapEvent(MappingEvent.PC_Suspend);
          break;
      }
    }
    static void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
    {
      switch (e.Reason)
      {
        case SessionEndReasons.Logoff:
          MapEvent(MappingEvent.PC_Logoff);
          break;

        case SessionEndReasons.SystemShutdown:
          MapEvent(MappingEvent.PC_Shutdown);
          break;
      }

    }

    internal static void UpdateNotifyMenu()
    {
      _notifyIcon.ContextMenuStrip.Items.Clear();

      if (Config.Programs.Count > 0)
      {
        foreach (ProgramSettings programSettings in Config.Programs)
          _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem(programSettings.Name, null, new EventHandler(ClickProgram)));

        _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
      }

      _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("&Setup", null, new EventHandler(ClickSetup)));
      _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("&Quit", null, new EventHandler(ClickQuit)));
    }

    static void ClickProgram(object sender, EventArgs e)
    {
      IrssLog.Info("Launch Program");

      ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
      if (menuItem == null)
        return;

      string program = menuItem.Text;
      foreach (ProgramSettings programSettings in Config.Programs)
      {
        if (programSettings.Name == program)
        {
          IrssLog.Info("Launching {0}", program);

          try
          {
            programSettings.Launch();
          }
          catch (Exception ex)
          {
            IrssLog.Error(ex.ToString());
          }

          return;
        }
      }

      IrssLog.Warn("Failed to launch (could not find program details): {0}", program);
    }
    static void ClickSetup(object sender, EventArgs e)
    {
      IrssLog.Info("Enter configuration");

      if (_inConfiguration)
      {
        IrssLog.Warn("Already in configuration");
        return;
      }

      _inConfiguration = true;

      _mainForm.ShowDialog();

      _inConfiguration = false;
    }
    static void ClickQuit(object sender, EventArgs e)
    {
      IrssLog.Info("User quit");

      if (_inConfiguration)
      {
        IrssLog.Warn("Can't quit while in configuration");
        return;
      }

      MapEvent(MappingEvent.Translator_Quit);

      Application.Exit();
    }

    internal static bool StartComms()
    {
      try
      {
        if (OpenLocalPipe())
        {
          _notifyIcon.Visible = true;

          _keepAliveThread = new Thread(new ThreadStart(KeepAliveThread));
          _keepAliveThread.Start();

          return true;
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
      }

      return false;
    }
    internal static void StopComms()
    {
      _notifyIcon.Visible = false;

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
          PipeAccess.SendMessage(Common.ServerPipeName, _config.ServerHost, message.ToString());
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
        IrssLog.Error(ex.ToString());
        return false;
      }
    }

    static bool ConnectToServer()
    {
      try
      {
        PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Register", null);
        PipeAccess.SendMessage(Common.ServerPipeName, _config.ServerHost, message.ToString());
        return true;
      }
      catch (AppModule.NamedPipes.NamedPipeIOException)
      {
        return false;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
        return false;
      }
    }

    static void KeepAliveThread()
    {
      bool firstConnect = true;

      Random random = new Random((int)DateTime.Now.Ticks);
      bool reconnect;
      int attempt;

      _keepAlive = true;
      while (_keepAlive)
      {
        reconnect = true;

        _notifyIcon.Icon = Properties.Resources.Icon16Connecting;
        _notifyIcon.Text = "Translator - Connecting ...";

        #region Connect to server

        IrssLog.Info("Connecting ({0}) ...", _config.ServerHost);
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
        {
          IrssLog.Info("Connected ({0})", _config.ServerHost);

          _notifyIcon.Icon = Properties.Resources.Icon16;
          _notifyIcon.Text = "Translator";

          if (firstConnect)
          {
            MapEvent(MappingEvent.Translator_Start);
            firstConnect = false;
          }
        }

        #endregion Registered ...

        #region Ping the server repeatedly

        while (_keepAlive && _registered && !reconnect)
        {
          IrssLog.Info("Ping ({0})", _config.ServerHost);

          int pingID = random.Next();
          long pingTime = DateTime.Now.Ticks;

          try
          {
            PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Ping", BitConverter.GetBytes(pingID));
            PipeAccess.SendMessage(Common.ServerPipeName, _config.ServerHost, message.ToString());
          }
          catch
          {
            // Failed to ping ... reconnect ...
            IrssLog.Warn("Failed to ping, attempting to reconnect ...");
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
              Thread.SpinWait(1000);
            }
          }

          if (receivedEcho) // Received ping echo ...
          {
            IrssLog.Info("Echo received");

            // Wait 60 seconds before re-pinging ...
            for (int sleeps = 0; sleeps < 60 && _keepAlive && _registered; sleeps++)
              Thread.Sleep(1000);
          }
          else // Didn't receive ping echo ...
          {
            IrssLog.Warn("No echo, attempting to reconnect ...");

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

      IrssLog.Debug("Received Message \"{0}\"", received.Name);

      try
      {
        switch (received.Name)
        {
          case "Start Learn":
          case "Blast Success":
            break;

          case "Remote Button":
            {
              string keyCode = Encoding.ASCII.GetString(received.Data);
              RemoteButtonPressed(keyCode);
              break;
            }

          case "Blast Failure":
            {
              IrssLog.Error("Failed to blast IR command");
              break;
            }

          case "Register Success":
            {
              IrssLog.Info("Registered to IR Server");
              _registered = true;
              _transceiverInfo = TransceiverInfo.FromBytes(received.Data);
              break;
            }

          case "Register Failure":
            {
              IrssLog.Warn("IR Server refused to register");
              _registered = false;
              break;
            }

          case "Learn Success":
            {
              IrssLog.Info("Learned IR Successfully");

              FileStream file = new FileStream(_learnIRFilename, FileMode.Create, FileAccess.Write, FileShare.None);
              file.Write(received.Data, 0, received.Data.Length);
              file.Flush();
              file.Close();

              _learnIRFilename = null;
              break;
            }

          case "Learn Failure":
            {
              IrssLog.Error("Failed to learn IR command");

              _learnIRFilename = null;
              break;
            }

          case "Server Shutdown":
            {
              IrssLog.Warn("IR Server Shutdown - Translator disabled until IR Server returns");
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
              _learnIRFilename = null;
              IrssLog.Error("Received error: {0}", Encoding.ASCII.GetString(received.Data));
              break;
            }

          default:
            {
              IrssLog.Warn("Unknown message received: {0}", received.Name);
              break;
            }
        }

        if (_handleMessage != null)
          _handleMessage(message);
      }
      catch (Exception ex)
      {
        _learnIRFilename = null;
        IrssLog.Error(ex.Message);
      }
    }

    static ProgramSettings ActiveProgram()
    {
      try
      {
        IntPtr active = Win32.GetForegroundWindow();

        if (active.Equals(IntPtr.Zero))
        {
          IrssLog.Debug("No active program (no foreground window)");
          return null;
        }

        int pid = -1;
        Win32.GetWindowThreadProcessId(active, out pid);

        string fileName = Path.GetFileName(Process.GetProcessById(pid).MainModule.FileName);

        foreach (ProgramSettings progSettings in Config.Programs)
        {
          if (fileName.Equals(Path.GetFileName(progSettings.Filename), StringComparison.InvariantCultureIgnoreCase))
          {
            return progSettings;
          }
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.Message);
      }

      IrssLog.Debug("Active program not found in Translator program list");
      return null;
    }

    static void RemoteButtonPressed(string keyCode)
    {
      ProgramSettings active = ActiveProgram();

      if (_inConfiguration)
        return;

      if (active == null)
      {
        // Try system wide button mappings ...
        foreach (ButtonMapping buttonMap in _config.SystemWideMappings)
        {
          if (buttonMap.KeyCode == keyCode)
          {
            IrssLog.Debug("KeyCode {0} mapped in System Wide mappings", keyCode);
            try
            {
              ProcessCommand(buttonMap.Command);
            }
            catch (Exception ex) { IrssLog.Error(ex.ToString()); }
            return;
          }
        }
      }
      else
      {
        // Try active program button mappings ...
        foreach (ButtonMapping buttonMap in active.ButtonMappings)
        {
          if (buttonMap.KeyCode == keyCode)
          {
            IrssLog.Debug("KeyCode {0} mapped in \"{1}\" mappings", keyCode, active.Name);
            try
            {
              ProcessCommand(buttonMap.Command);
            }
            catch (Exception ex) { IrssLog.Error(ex.ToString()); }
            return;
          }
        }

        if (!active.IgnoreSystemWide)
        {
          // Try system wide button mappings ...
          foreach (ButtonMapping buttonMap in _config.SystemWideMappings)
          {
            if (buttonMap.KeyCode == keyCode)
            {
              IrssLog.Debug("KeyCode {0} mapped in System Wide mappings", keyCode);
              try
              {
                ProcessCommand(buttonMap.Command);
              }
              catch (Exception ex) { IrssLog.Error(ex.ToString()); }
              return;
            }
          }
        }
      }

      IrssLog.Debug("No mapping found for KeyCode = {0}", keyCode);
    }

    static void MapEvent(MappingEvent theEvent)
    {
      if (_inConfiguration)
        return;

      string eventName = Enum.GetName(typeof(MappingEvent), theEvent);

      IrssLog.Debug("Mappable event: {0}", eventName);

      if (Config.Events.Count == 0)
      {
        IrssLog.Debug("No event mappings in current configuration");
        return;
      }

      foreach (MappedEvent mappedEvent in Config.Events)
      {
        if (mappedEvent.EventType == theEvent)
        {
          IrssLog.Info("Event mapped: {0}, {1}", eventName, mappedEvent.Command);
          try
          {
            ProcessCommand(mappedEvent.Command);
          }
          catch (Exception ex) { IrssLog.Error(ex.ToString()); }
        }
      }
    }

    /// <summary>
    /// Process the supplied Macro file.
    /// </summary>
    /// <param name="fileName">Macro file to process (absolute path).</param>
    internal static void ProcessMacro(string fileName)
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(fileName);

      if (doc.DocumentElement.InnerText.Contains(Common.XmlTagBlast) && !_registered)
        throw new Exception("Cannot process Macro with Blast commands when not registered to an active IR Server");

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

          case Common.XmlTagMessage:
            {
              string[] commands = Common.SplitMessageCommand(commandProperty);
              Common.ProcessMessageCommand(commands);
              break;
            }

          case Common.XmlTagKeys:
            {
              Common.ProcessKeyCommand(commandProperty);
              break;
            }

          case Common.XmlTagStandby:
            {
              Application.SetSuspendState(PowerState.Suspend, true, false);
              break;
            }

          case Common.XmlTagHibernate:
            {
              Application.SetSuspendState(PowerState.Hibernate, true, false);
              break;
            }
        }
      }
    }

    /// <summary>
    /// Learn an IR command.
    /// </summary>
    /// <param name="fileName">File to place learned IR command in.</param>
    /// <returns>Success.</returns>
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
          IrssLog.Warn("Not registered to an active IR Server");
          return false;
        }

        if (_learnIRFilename != null)
        {
          IrssLog.Warn("Already trying to learn an IR command");
          return false;
        }

        _learnIRFilename = fileName;

        PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Learn", null);
        PipeAccess.SendMessage(Common.ServerPipeName, _config.ServerHost, message.ToString());
      }
      catch (Exception ex)
      {
        _learnIRFilename = null;
        IrssLog.Error(ex.Message);
        return false;
      }

      return true;
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
      PipeAccess.SendMessage(Common.ServerPipeName, _config.ServerHost, message.ToString());
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
      else if (command.StartsWith(Common.CmdPrefixMessage)) // Message Command
      {
        string[] commands = Common.SplitMessageCommand(command.Substring(Common.CmdPrefixMessage.Length));
        Common.ProcessMessageCommand(commands);
      }
      else if (command.StartsWith(Common.CmdPrefixKeys)) // Keystroke Command
      {
        string keyCommand = command.Substring(Common.CmdPrefixKeys.Length);
        Common.ProcessKeyCommand(keyCommand);
      }
      else
      {
        throw new ArgumentException(String.Format("Cannot process unrecognized command \"{0}\"", command), "command");
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

    #endregion Implementation

  }

}
