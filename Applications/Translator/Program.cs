using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using IrssComms;
using IrssUtils;
using IrssUtils.Exceptions;
using IrssUtils.Forms;
using Microsoft.Win32;
using Translator.Properties;

namespace Translator
{
  internal static class Program
  {
    #region Constants

    private const string ProcessCommandThreadName = "ProcessCommand";
    private static readonly string DefaultConfigFile = Path.Combine(Common.FolderAppData, "Translator\\Translator.xml");

    internal static readonly string FolderDefaultSettings = Path.Combine(Common.FolderAppData,
                                                                         "Translator\\Default Settings");

    internal static readonly string FolderMacros = Path.Combine(Common.FolderAppData, "Translator\\Macro");

    #endregion Constants

    #region Components

    private static Client _client;
    private static Configuration _config;
    private static Container _container;
    private static CopyDataWM _copyDataWM;
    private static MainForm _mainForm;
    private static NotifyIcon _notifyIcon;

    #endregion Components

    #region Variables

    private static string _configFile;

    private static bool _firstConnection = true;

    private static ClientMessageSink _handleMessage;

    private static bool _inConfiguration;

    private static IRServerInfo _irServerInfo = new IRServerInfo();
    private static string _learnIRFilename;
    private static bool _menuFormVisible;
    private static bool _registered;

    private static VariableList _variables;

    #endregion Variables

    #region Properties

    internal static string ConfigFile
    {
      get { return _configFile; }
      set { _configFile = value; }
    }

    internal static Configuration Config
    {
      get { return _config; }
      set { _config = value; }
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

    internal static NotifyIcon TrayIcon
    {
      get { return _notifyIcon; }
    }

    #endregion Properties

    #region Main

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main(string[] args)
    {
      _configFile = DefaultConfigFile;

      if (args.Length > 0)
      {
        try
        {
          if (ProcessCommandLine(args))
            return;
        }
        catch (CommandExecutionException ex)
        {
          MessageBox.Show(ex.Message, "Translator", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (CommandStructureException ex)
        {
          MessageBox.Show(ex.Message, "Translator", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.ToString(), "Translator - Error processing command line", MessageBoxButtons.OK,
                          MessageBoxIcon.Error);
        }
      }

      // Check for multiple instances.
      if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length != 1)
      {
        CopyDataWM.SendCopyDataMessage(Common.CmdPrefixShowTrayIcon);
        return;
      }

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

#if DEBUG
      IrssLog.LogLevel = IrssLog.Level.Debug;
#else
      IrssLog.LogLevel = IrssLog.Level.Info;
#endif
      IrssLog.Open("Translator.log");

      Application.ThreadException += Application_ThreadException;

      // Initialize Variable List.
      _variables = new VariableList();

      // Load configuration ...
      _config = Configuration.Load(_configFile);
      if (_config == null)
      {
        IrssLog.Warn("Failed to load configuration file ({0}), creating new configuration", _configFile);
        _config = new Configuration();
      }

      // Adjust process priority ...
      AdjustPriority(_config.ProcessPriority);


      //foreach (ProgramSettings progSettings in _config.Programs)
      //{
      //  AppProfile profile = new AppProfile();

      //  profile.Name = progSettings.Name;
      //  profile.MatchType = AppProfile.DetectionMethod.Executable;
      //  profile.MatchParameters = progSettings.FileName;
      //  profile.ButtonMappings.AddRange(progSettings.ButtonMappings);

      //  AppProfile.Save(profile, "C:\\" + profile.Name + ".xml");
      //}


      // Setup notify icon ...
      _container = new Container();
      _notifyIcon = new NotifyIcon(_container);
      _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
      _notifyIcon.Icon = Resources.Icon16Connecting;
      _notifyIcon.Text = "Translator - Connecting ...";
      _notifyIcon.DoubleClick += ClickSetup;
      _notifyIcon.Visible = !_config.HideTrayIcon;

      // Setup the main form ...
      _mainForm = new MainForm();

      // Start server communications ...
      bool clientStarted = false;

      IPAddress serverIP = Client.GetIPFromName(_config.ServerHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, Server.DefaultPort);

      try
      {
        clientStarted = StartClient(endPoint);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        clientStarted = false;
      }

      if (clientStarted)
      {
        // Setup event notification ...
        SystemEvents.SessionEnding += SystemEvents_SessionEnding;
        SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;

        try
        {
          _copyDataWM = new CopyDataWM();
          _copyDataWM.Start();
        }
        catch (Win32Exception ex)
        {
          IrssLog.Error("Error enabling CopyData messages: {0}", ex.ToString());
        }

        Application.Run();

        if (_copyDataWM != null)
        {
          _copyDataWM.Dispose();
          _copyDataWM = null;
        }

        SystemEvents.SessionEnding -= SystemEvents_SessionEnding;
        SystemEvents.PowerModeChanged -= SystemEvents_PowerModeChanged;

        StopClient();
      }
      else
      {
        MessageBox.Show("Failed to start IR Server communications, refer to log file for more details.",
                        "Translator - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        _inConfiguration = true;

        _mainForm.ShowDialog();

        _inConfiguration = false;
      }

      // Dispose NotifyIcon ...
      _notifyIcon.Visible = false;
      _notifyIcon.Dispose();
      _notifyIcon = null;

      // Dispose Container ...
      _container.Dispose();
      _container = null;

      Application.ThreadException -= Application_ThreadException;

      IrssLog.Close();
    }

    #endregion Main

    #region Implementation

    /// <summary>
    /// Handles unhandled exceptions.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">Event args.</param>
    private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
    {
      IrssLog.Error(e.Exception);
    }

    private static bool ProcessCommandLine(string[] args)
    {
      bool dontRun = true;

      for (int index = 0; index < args.Length; index++)
      {
        string command = args[index].ToUpperInvariant();
        if (command.StartsWith("-") || command.StartsWith("/"))
          command = command.Substring(1);

        switch (command)
        {
          case "BLAST":
            if (args.Length > index + 2)
              CopyDataWM.SendCopyDataMessage(Common.CmdPrefixBlast + args[++index] + '|' + args[++index]);
            else
              throw new CommandStructureException("Blast command requires two parameters (IR file, Port)");
            break;

          case "MACRO":
            if (args.Length > index + 1)
              CopyDataWM.SendCopyDataMessage(Common.CmdPrefixMacro + args[++index]);
            else
              throw new CommandStructureException("Macro command requires a parameter (Macro file)");
            break;

          case "EJECT":
            if (args.Length > index + 1)
              CopyDataWM.SendCopyDataMessage(Common.CmdPrefixEject + args[++index]);
            else
              throw new CommandStructureException("Eject command requires a parameter (Drive)");
            break;

          case "SHUTDOWN":
            CopyDataWM.SendCopyDataMessage(Common.CmdPrefixShutdown);
            break;

          case "REBOOT":
            CopyDataWM.SendCopyDataMessage(Common.CmdPrefixReboot);
            break;

          case "STANDBY":
            CopyDataWM.SendCopyDataMessage(Common.CmdPrefixStandby);
            break;

          case "HIBERNATE":
            CopyDataWM.SendCopyDataMessage(Common.CmdPrefixHibernate);
            break;

          case "LOGOFF":
            CopyDataWM.SendCopyDataMessage(Common.CmdPrefixLogOff);
            break;

          case "OSD":
            CopyDataWM.SendCopyDataMessage(Common.CmdPrefixTranslator);
            break;

          case "CHANNEL":
            if (args.Length > index + 4)
            {
              string channel = args[++index];
              int padding = int.Parse(args[++index]);
              string port = args[++index];
              int delay = int.Parse(args[++index]);

              while (channel.Length < padding)
                channel = '0' + channel;

              foreach (char digit in channel)
              {
                CopyDataWM.SendCopyDataMessage(Common.CmdPrefixBlast + digit + '|' + port);
                if (delay > 0)
                  Thread.Sleep(delay);
              }
            }
            else
            {
              throw new CommandStructureException(
                "Channel command requires three parameters (Channel, Padding, Port, Delay)");
            }
            break;

          case "CONFIG":
            if (args.Length > index + 1)
              _configFile = args[++index];
            else
              throw new CommandStructureException("Config command requires a parameter (Config file path)");

            dontRun = false;
            break;


            // TODO: Add more command line options.


          default:
            throw new CommandStructureException(String.Format("Unrecognised command line parameter \"{0}\"", args[index]));
        }
      }

      return dontRun;
    }

    private static void ShowOSD()
    {
      IrssLog.Info("Show OSD");

      if (_menuFormVisible)
      {
        IrssLog.Info("OSD already visible");
      }
      else
      {
        ThreadStart threadStart = MenuThread;
        Thread thread = new Thread(threadStart);
        thread.Name = "Translator OSD";
        thread.IsBackground = true;
        thread.Start();
      }
    }

    private static void MenuThread()
    {
      try
      {
        _menuFormVisible = true;

        using (MenuForm menuForm = new MenuForm())
          menuForm.ShowDialog();

        _menuFormVisible = false;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
    }

    private static void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
    {
      switch (e.Mode)
      {
        case PowerModes.Resume:
          MapEvent(MappingEvent.PC_Resume, true);
          break;

        case PowerModes.Suspend:
          MapEvent(MappingEvent.PC_Suspend, false);
          break;
      }
    }

    private static void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
    {
      switch (e.Reason)
      {
        case SessionEndReasons.Logoff:
          MapEvent(MappingEvent.PC_Logoff, false);
          break;

        case SessionEndReasons.SystemShutdown:
          MapEvent(MappingEvent.PC_Shutdown, false);
          break;
      }
    }

    internal static void UpdateNotifyMenu()
    {
      _notifyIcon.ContextMenuStrip.Items.Clear();

      _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripLabel("Translator"));
      _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());

      if (Config.Programs.Count > 0)
      {
        ToolStripMenuItem launch = new ToolStripMenuItem("&Launch");

        foreach (ProgramSettings programSettings in Config.Programs)
          launch.DropDownItems.Add(programSettings.Name, null, ClickProgram);

        _notifyIcon.ContextMenuStrip.Items.Add(launch);
      }

      //string[] irList = Common.GetIRList(false);
      //if (irList.Length > 0)
      //{
      //  ToolStripMenuItem irCommands = new ToolStripMenuItem("&IR Commands");

      //  foreach (string irCommand in irList)
      //    irCommands.DropDownItems.Add(irCommand, null, new EventHandler(ClickIrCommand));

      //  _notifyIcon.ContextMenuStrip.Items.Add(irCommands);
      //}

      string[] macroList = IrssMacro.GetMacroList(FolderMacros, false);
      if (macroList.Length > 0)
      {
        ToolStripMenuItem macros = new ToolStripMenuItem("&Macros");

        foreach (string macro in macroList)
          macros.DropDownItems.Add(macro, null, ClickMacro);

        _notifyIcon.ContextMenuStrip.Items.Add(macros);
      }

      ToolStripMenuItem actions = new ToolStripMenuItem("&Actions");

      actions.DropDownItems.Add("System Standby", null, ClickAction);
      actions.DropDownItems.Add("System Hibernate", null, ClickAction);
      actions.DropDownItems.Add("System Reboot", null, ClickAction);
      actions.DropDownItems.Add("System LogOff", null, ClickAction);
      actions.DropDownItems.Add("System Shutdown", null, ClickAction);

      actions.DropDownItems.Add(new ToolStripSeparator());

      ToolStripMenuItem ejectMenu = new ToolStripMenuItem("Eject");
      DriveInfo[] drives = DriveInfo.GetDrives();
      foreach (DriveInfo drive in drives)
        if (drive.DriveType == DriveType.CDRom)
          ejectMenu.DropDownItems.Add(drive.Name, null, ClickEjectAction);
      actions.DropDownItems.Add(ejectMenu);

      actions.DropDownItems.Add(new ToolStripSeparator());

      actions.DropDownItems.Add("Volume Up", null, ClickAction);
      actions.DropDownItems.Add("Volume Down", null, ClickAction);
      actions.DropDownItems.Add("Volume Mute", null, ClickAction);

      _notifyIcon.ContextMenuStrip.Items.Add(actions);

      _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
      _notifyIcon.ContextMenuStrip.Items.Add("Show &OSD", null, ClickOSD);
      _notifyIcon.ContextMenuStrip.Items.Add("&Setup", null, ClickSetup);
      _notifyIcon.ContextMenuStrip.Items.Add("&Quit", null, ClickQuit);
    }

    private static void ClickProgram(object sender, EventArgs e)
    {
      IrssLog.Info("Click Launch Program");

      ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
      if (menuItem == null)
        return;

      string program = menuItem.Text;
      foreach (ProgramSettings programSettings in Config.Programs)
      {
        if (programSettings.Name.Equals(program, StringComparison.OrdinalIgnoreCase))
        {
          IrssLog.Info("Launching {0}", program);

          ProcessCommand(Common.CmdPrefixRun + programSettings.RunCommandString, true);
          return;
        }
      }

      IrssLog.Warn("Failed to launch (could not find program details): {0}", program);
    }

    private static void ClickMacro(object sender, EventArgs e)
    {
      IrssLog.Info("Click Macro");

      ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
      if (menuItem == null)
        return;

      try
      {
        ProcessCommand(Common.CmdPrefixMacro + menuItem.Text, true);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(ex.Message, "Macro failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private static void ClickAction(object sender, EventArgs e)
    {
      IrssLog.Info("Click Action");

      ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
      if (menuItem == null)
        return;

      try
      {
        switch (menuItem.Text)
        {
          case "System Standby":
            ProcessCommand(Common.CmdPrefixStandby, true);
            break;

          case "System Hibernate":
            ProcessCommand(Common.CmdPrefixHibernate, true);
            break;

          case "System Reboot":
            ProcessCommand(Common.CmdPrefixReboot, true);
            break;

          case "System LogOff":
            ProcessCommand(Common.CmdPrefixLogOff, true);
            break;

          case "System Shutdown":
            ProcessCommand(Common.CmdPrefixShutdown, true);
            break;


          case "Volume Up":
            // TODO: Replace with Volume Commands
            Win32.SendWindowsMessage(
              Win32.GetDesktopWindowHandle(),
              (int) Win32.WindowsMessage.WM_APPCOMMAND,
              0,
              65536*(int) Win32.AppCommand.APPCOMMAND_VOLUME_UP);
            break;

          case "Volume Down":
            Win32.SendWindowsMessage(
              Win32.GetDesktopWindowHandle(),
              (int) Win32.WindowsMessage.WM_APPCOMMAND,
              0,
              65536*(int) Win32.AppCommand.APPCOMMAND_VOLUME_DOWN);
            break;

          case "Volume Mute":
            Win32.SendWindowsMessage(
              Win32.GetDesktopWindowHandle(),
              (int) Win32.WindowsMessage.WM_APPCOMMAND,
              0,
              65536*(int) Win32.AppCommand.APPCOMMAND_VOLUME_MUTE);
            break;

          default:
            throw new ArgumentException(String.Format("Unknown action: {0}", menuItem.Text), "sender");
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(ex.Message, "Action failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private static void ClickEjectAction(object sender, EventArgs e)
    {
      IrssLog.Info("Click Eject Action");

      ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
      if (menuItem == null)
        return;

      ProcessCommand(Common.CmdPrefixEject + menuItem.Text, true);
    }

    private static void ClickOSD(object sender, EventArgs e)
    {
      ShowOSD();
    }

    private static void ClickSetup(object sender, EventArgs e)
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

    private static void ClickQuit(object sender, EventArgs e)
    {
      IrssLog.Info("User quit");

      if (_inConfiguration)
      {
        IrssLog.Warn("Can't quit while in configuration");
        return;
      }

      MapEvent(MappingEvent.Translator_Quit, false);

      Application.Exit();
    }

    private static void CommsFailure(object obj)
    {
      Exception ex = obj as Exception;

      if (ex != null)
        IrssLog.Error("Communications failure: {0}", ex.Message);
      else
        IrssLog.Error("Communications failure");

      _notifyIcon.Icon = Resources.Icon16Connecting;
      _notifyIcon.Text = "Translator - Serious Communications Failure";

      StopClient();

      MessageBox.Show("Please report this error.", "Translator - Communications failure", MessageBoxButtons.OK,
                      MessageBoxIcon.Error);
    }

    private static void Connected(object obj)
    {
      IrssLog.Info("Connected to server");

      IrssMessage message = new IrssMessage(MessageType.RegisterClient, MessageFlags.Request);
      _client.Send(message);

      _notifyIcon.Icon = Resources.Icon16;
      _notifyIcon.Text = "Translator";

      if (_firstConnection)
      {
        _firstConnection = false;
        MapEvent(MappingEvent.Translator_Start, true);
      }
    }

    private static void Disconnected(object obj)
    {
      IrssLog.Warn("Communications with server has been lost");

      _notifyIcon.Icon = Resources.Icon16Connecting;
      _notifyIcon.Text = "Translator - Reconnecting ...";

      Thread.Sleep(1000);
    }

    internal static bool StartClient(IPEndPoint endPoint)
    {
      if (_client != null)
        return false;

      _notifyIcon.Icon = Resources.Icon16Connecting;
      _notifyIcon.Text = "Translator - Connecting ...";

      ClientMessageSink sink = ReceivedMessage;

      _client = new Client(endPoint, sink);
      _client.CommsFailureCallback = CommsFailure;
      _client.ConnectCallback = Connected;
      _client.DisconnectCallback = Disconnected;

      if (_client.Start())
        return true;

      _client = null;
      return false;
    }

    internal static void StopClient()
    {
      if (_client == null)
        return;

      _client.Dispose();
      _client = null;

      _registered = false;
    }

    private static void ReceivedMessage(IrssMessage received)
    {
      IrssLog.Debug("Received Message \"{0}\"", received.Type);

      try
      {
        switch (received.Type)
        {
          case MessageType.RemoteEvent:
            byte[] data = received.GetDataAsBytes();
            int deviceNameSize = BitConverter.ToInt32(data, 0);
            string deviceName = Encoding.ASCII.GetString(data, 4, deviceNameSize);
            int keyCodeSize = BitConverter.ToInt32(data, 4 + deviceNameSize);
            string keyCode = Encoding.ASCII.GetString(data, 8 + deviceNameSize, keyCodeSize);

            RemoteHandlerCallback(deviceName, keyCode);
            break;

          case MessageType.KeyboardEvent:
            {
              byte[] dataBytes = received.GetDataAsBytes();

              int vKey = BitConverter.ToInt32(dataBytes, 0);
              bool keyUp = BitConverter.ToBoolean(dataBytes, 4);

              KeyboardHandlerCallback("TODO", vKey, keyUp);
              break;
            }

          case MessageType.MouseEvent:
            {
              byte[] dataBytes = received.GetDataAsBytes();

              int deltaX = BitConverter.ToInt32(dataBytes, 0);
              int deltaY = BitConverter.ToInt32(dataBytes, 4);
              int buttons = BitConverter.ToInt32(dataBytes, 8);

              MouseHandlerCallback("TODO", deltaX, deltaY, buttons);
              break;
            }

          case MessageType.BlastIR:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
              IrssLog.Info("Blast successful");
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
              IrssLog.Error("Failed to blast IR command");
            break;

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

          case MessageType.LearnIR:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
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
              IrssLog.Warn("Learn IR command timed-out");
            }

            _learnIRFilename = null;
            break;

          case MessageType.ServerShutdown:
            IrssLog.Warn("Input Service Shutdown - Translator disabled until Input Service returns");
            _registered = false;

            _notifyIcon.Icon = Resources.Icon16Connecting;
            _notifyIcon.Text = "Translator - Connecting ...";

            break;

          case MessageType.Error:
            _learnIRFilename = null;
            IrssLog.Error("Received error: {0}", received.GetDataAsString());
            break;
        }

        if (_handleMessage != null)
          _handleMessage(received);
      }
      catch (Exception ex)
      {
        _learnIRFilename = null;
        IrssLog.Error(ex);
      }
    }

    private static ProgramSettings ActiveProgram()
    {
      try
      {
        int pid = Win32.GetForegroundWindowPID();
        if (pid == -1)
        {
          IrssLog.Debug("Failed to retrieve foreground window process ID");
          return null;
        }

        Process process = Process.GetProcessById(pid);
        if (process == null)
        {
          IrssLog.Debug("Failed to locate process by process ID");
          return null;
        }

        string fileName = string.Empty;
        string processName = string.Empty;
        try
        {
          fileName = Path.GetFileName(process.MainModule.FileName);
        }catch
        {
          processName = Path.GetFileName(process.ProcessName);
        }
        foreach (ProgramSettings progSettings in Config.Programs)
        {
          if (fileName.Equals(Path.GetFileName(progSettings.FileName), StringComparison.OrdinalIgnoreCase))
          {
            return progSettings;
          }
          if (processName.Equals(Path.GetFileName(progSettings.Name), StringComparison.OrdinalIgnoreCase))
          {
            return progSettings;
          }
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }

      IrssLog.Debug("Active program not found in Translator program list");
      return null;
    }

    private static void RemoteHandlerCallback(string deviceName, string keyCode)
    {
      if (_inConfiguration)
        return;

      string button;

      // TODO: When Abstract Remote Model becomes on by default
      //if (deviceName.Equals("Abstract", StringComparison.OrdinalIgnoreCase)
      button = keyCode;
      //else
      //  button = String.Format("{0} ({1})", deviceName, keyCode);

      ProgramSettings active = ActiveProgram();
      if (active == null)
      {
        // Try system wide button mappings ...
        foreach (ButtonMapping buttonMap in _config.SystemWideMappings)
        {
          if (buttonMap.KeyCode.Equals(button, StringComparison.Ordinal))
          {
            IrssLog.Debug("KeyCode {0} mapped in System Wide mappings", button);
            try
            {
              ProcessCommand(buttonMap.Command, true);
            }
            catch (Exception ex)
            {
              IrssLog.Error(ex);
            }

            return;
          }
        }
      }
      else
      {
        // Try active program button mappings ...
        foreach (ButtonMapping buttonMap in active.ButtonMappings)
        {
          if (buttonMap.KeyCode.Equals(button, StringComparison.Ordinal))
          {
            IrssLog.Debug("KeyCode {0} mapped in \"{1}\" mappings", button, active.Name);
            try
            {
              ProcessCommand(buttonMap.Command, true);
            }
            catch (Exception ex)
            {
              IrssLog.Error(ex);
            }

            return;
          }
        }

        if (!active.IgnoreSystemWide)
        {
          // Try system wide button mappings ...
          foreach (ButtonMapping buttonMap in _config.SystemWideMappings)
          {
            if (buttonMap.KeyCode.Equals(button, StringComparison.Ordinal))
            {
              IrssLog.Debug("KeyCode {0} mapped in System Wide mappings", button);
              try
              {
                ProcessCommand(buttonMap.Command, true);
              }
              catch (Exception ex)
              {
                IrssLog.Error(ex);
              }

              return;
            }
          }
        }
      }

      IrssLog.Debug("No mapping found for KeyCode = {0}", button);
    }

    private static void KeyboardHandlerCallback(string deviceName, int vKey, bool keyUp)
    {
      if (keyUp)
        Keyboard.KeyUp((Keyboard.VKey) vKey);
      else
        Keyboard.KeyDown((Keyboard.VKey) vKey);
    }

    private static void MouseHandlerCallback(string deviceName, int deltaX, int deltaY, int buttons)
    {
      if (deltaX != 0 || deltaY != 0)
        Mouse.Move(deltaX, deltaY, false);

      if (buttons != (int) Mouse.MouseEvents.None)
        Mouse.Button((Mouse.MouseEvents) buttons);
    }

    private static void MapEvent(MappingEvent theEvent, bool async)
    {
      if (_inConfiguration)
        return;

      string eventName = Enum.GetName(typeof (MappingEvent), theEvent);

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
          if (String.IsNullOrEmpty(mappedEvent.Command))
          {
            IrssLog.Warn("Event found ({0}) with no command set", eventName);
          }
          else
          {
            try
            {
              IrssLog.Info("Event mapped: {0}, {1}", eventName, mappedEvent.Command);
              ProcessCommand(mappedEvent.Command, async);
            }
            catch (Exception ex)
            {
              IrssLog.Error(ex);
            }
          }
        }
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
      if (!_registered)
        throw new InvalidOperationException("Cannot Blast, not registered to an active Input Service");

      using (FileStream file = File.OpenRead(fileName))
      {
        if (file.Length == 0)
          throw new IOException(String.Format("Cannot Blast. IR file \"{0}\" has no data, possible IR learn failure",
                                              fileName));

        byte[] outData = new byte[4 + port.Length + file.Length];

        BitConverter.GetBytes(port.Length).CopyTo(outData, 0);
        Encoding.ASCII.GetBytes(port).CopyTo(outData, 4);

        file.Read(outData, 4 + port.Length, (int) file.Length);

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
          Thread newThread = new Thread(ProcCommand);
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
    private static void ProcCommand(object commandObj)
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
          string fileName = Path.Combine(FolderMacros,
                                         command.Substring(Common.CmdPrefixMacro.Length) + Common.FileExtensionMacro);
          IrssMacro.ExecuteMacro(fileName, _variables, ProcCommand);
        }
        else if (command.StartsWith(Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitBlastCommand(command.Substring(Common.CmdPrefixBlast.Length));
          BlastIR(Path.Combine(Common.FolderIRCommands, commands[0] + Common.FileExtensionIR), commands[1]);
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
        else if (command.StartsWith(Common.CmdPrefixMouse, StringComparison.OrdinalIgnoreCase))
        {
          string mouseCommand = command.Substring(Common.CmdPrefixMouse.Length);
          Common.ProcessMouseCommand(mouseCommand);
        }
        else if (command.StartsWith(Common.CmdPrefixEject, StringComparison.OrdinalIgnoreCase))
        {
          string ejectCommand = command.Substring(Common.CmdPrefixEject.Length);
          Common.ProcessEjectCommand(ejectCommand);
        }
        else if (command.StartsWith(Common.CmdPrefixPopup, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitPopupCommand(command.Substring(Common.CmdPrefixPopup.Length));

          ShowPopupMessage showPopupMessage = new ShowPopupMessage(commands[0], commands[1], int.Parse(commands[2]));
          showPopupMessage.ShowDialog();
        }
        else if (command.StartsWith(Common.CmdPrefixHibernate, StringComparison.OrdinalIgnoreCase))
        {
          if (_inConfiguration)
          {
            MessageBox.Show("Cannot Hibernate in configuration", Common.UITextHibernate, MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
          }
          else
          {
            if (!Common.Hibernate())
              IrssLog.Warn("Hibernate request was rejected by another application.");
          }
        }
        else if (command.StartsWith(Common.CmdPrefixLogOff, StringComparison.OrdinalIgnoreCase))
        {
          if (_inConfiguration)
          {
            MessageBox.Show("Cannot Log off in configuration", Common.UITextLogOff, MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
          }
          else
          {
            if (!Common.LogOff())
              IrssLog.Warn("LogOff request failed.");
          }
        }
        else if (command.StartsWith(Common.CmdPrefixReboot, StringComparison.OrdinalIgnoreCase))
        {
          if (_inConfiguration)
          {
            MessageBox.Show("Cannot Reboot in configuration", Common.UITextReboot, MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
          }
          else
          {
            if (!Common.Reboot())
              IrssLog.Warn("Reboot request failed.");
          }
        }
        else if (command.StartsWith(Common.CmdPrefixShutdown, StringComparison.OrdinalIgnoreCase))
        {
          if (_inConfiguration)
          {
            MessageBox.Show("Cannot ShutDown in configuration", Common.UITextShutdown, MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
          }
          else
          {
            if (!Common.ShutDown())
              IrssLog.Warn("ShutDown request failed.");
          }
        }
        else if (command.StartsWith(Common.CmdPrefixStandby, StringComparison.OrdinalIgnoreCase))
        {
          if (_inConfiguration)
          {
            MessageBox.Show("Cannot enter Standby in configuration", Common.UITextStandby, MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
          }
          else
          {
            if (!Common.Standby())
              IrssLog.Warn("Standby request was rejected by another application.");
          }
        }
        else if (command.StartsWith(Common.CmdPrefixTranslator, StringComparison.OrdinalIgnoreCase))
        {
          ShowOSD();
        }
        else if (command.StartsWith(Common.CmdPrefixVirtualKB, StringComparison.OrdinalIgnoreCase))
        {
          if (_inConfiguration)
          {
            MessageBox.Show("Cannot show Virtual Keyboard in configuration", Common.UITextVirtualKB,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
          }
          else
          {
            VirtualKeyboard vk = new VirtualKeyboard();
            if (vk.ShowDialog() == DialogResult.OK)
              Keyboard.ProcessCommand(vk.TextOutput);
          }
        }
        else if (command.StartsWith(Common.CmdPrefixSmsKB, StringComparison.OrdinalIgnoreCase))
        {
          if (_inConfiguration)
          {
            MessageBox.Show("Cannot show SMS Keyboard in configuration", Common.UITextSmsKB, MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
          }
          else
          {
            SmsKeyboard sms = new SmsKeyboard();
            if (sms.ShowDialog() == DialogResult.OK)
              Keyboard.ProcessCommand(sms.TextOutput);
          }
        }
        else if (command.StartsWith(Common.CmdPrefixShowTrayIcon, StringComparison.OrdinalIgnoreCase))
        {
          if (!_notifyIcon.Visible)
          {
            _notifyIcon.Visible = true;
            _notifyIcon.ShowBalloonTip(2000, "Translator", "Icon is now visible", ToolTipIcon.Info);
          }
        }
        else
        {
          throw new ArgumentException(String.Format("Cannot process unrecognized command \"{0}\"", command),
                                      "commandObj");
        }
      }
      catch (Exception ex)
      {
        if (!String.IsNullOrEmpty(Thread.CurrentThread.Name) &&
            Thread.CurrentThread.Name.Equals(ProcessCommandThreadName, StringComparison.OrdinalIgnoreCase))
          IrssLog.Error(ex);
        else
          throw;
      }
    }

    /// <summary>
    /// Adjusts the process priority.
    /// </summary>
    /// <param name="newPriority">The new priority.</param>
    internal static void AdjustPriority(string newPriority)
    {
      if (!newPriority.Equals("No Change", StringComparison.OrdinalIgnoreCase))
      {
        try
        {
          ProcessPriorityClass priority =
            (ProcessPriorityClass) Enum.Parse(typeof (ProcessPriorityClass), newPriority, true);
          Process.GetCurrentProcess().PriorityClass = priority;

          IrssLog.Info("Process priority set to: {0}", newPriority);
        }
        catch (Exception ex)
        {
          IrssLog.Error(ex);
        }
      }
    }

    #endregion Implementation
  }
}