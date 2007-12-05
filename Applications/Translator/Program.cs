using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Win32;

using IrssComms;
using IrssUtils;

namespace Translator
{

  static class Program
  {

    #region Constants

    internal static readonly string ConfigFile = Common.FolderAppData + "Translator\\Translator.xml";

    internal static readonly string FolderMacros = Common.FolderAppData + "Translator\\Macro\\";

    const string ProcessCommandThreadName = "ProcessCommand";

    #endregion Constants

    #region Variables

    static string _learnIRFilename;

    static bool _registered;

    static bool _firstConnection = true;

    static ClientMessageSink _handleMessage;

    static bool _inConfiguration;

    static IRServerInfo _irServerInfo = new IRServerInfo();

    static Hashtable _macroStacks;

    #endregion Variables

    #region Properties

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

    #endregion Properties

    #region Components

    static NotifyIcon _notifyIcon;
    static MainForm _mainForm;
    static MenuForm _menuForm;
    static Client _client;
    static Configuration _config;
    static CopyDataWM _copyDataWM;

    #endregion Components

    #region Main

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
      if (args.Length > 0)
      {
        try
        {
          ProcessCommandLine(args);
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.ToString(), "Translator - Error processing command line", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return;
      }

      // Check for multiple instances.
      if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length != 1)
        return;

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

#if DEBUG
      IrssLog.LogLevel = IrssLog.Level.Debug;
#else
      IrssLog.LogLevel = IrssLog.Level.Info;
#endif
      IrssLog.Open(Common.FolderIrssLogs + "Translator.log");

      Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

      // Hashtable for storing active macro stacks in.
      _macroStacks = new Hashtable();

      _config = Configuration.Load(ConfigFile);
      if (_config == null)
      {
        MessageBox.Show("Failed to load configuration, creating new configuration", "Translator - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        _config = new Configuration();
      }

      // Setup notify icon ...
      _notifyIcon = new NotifyIcon();
      _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
      _notifyIcon.Icon = Properties.Resources.Icon16Connecting;
      _notifyIcon.Text = "Translator - Connecting ...";
      _notifyIcon.DoubleClick += new EventHandler(ClickSetup);
      _notifyIcon.Visible = false;

      // Setup the Translator Menu ...
      _menuForm = new MenuForm();

      // Setup the main form ...
      _mainForm = new MainForm();

      // Start server communications ...
      bool clientStarted = false;

      IPAddress serverIP = Client.GetIPFromName(_config.ServerHost);
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
      {
        // Setup event notification ...
        SystemEvents.SessionEnding += new SessionEndingEventHandler(SystemEvents_SessionEnding);
        SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);

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

        SystemEvents.SessionEnding -= new SessionEndingEventHandler(SystemEvents_SessionEnding);
        SystemEvents.PowerModeChanged -= new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);

        StopClient();
      }
      else
      {
        MessageBox.Show("Failed to start IR Server communications, refer to log file for more details.", "Translator - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        _inConfiguration = true;

        _mainForm.ShowDialog();

        _inConfiguration = false;
      }
      
      _notifyIcon.Visible = false;

      Application.ThreadException -= new ThreadExceptionEventHandler(Application_ThreadException);

      IrssLog.Close();
    }

    #endregion Main

    #region Implementation

    /// <summary>
    /// Handles unhandled exceptions.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">Event args.</param>
    static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
    {
      IrssLog.Error(e.Exception.ToString());
    }

    static void ProcessCommandLine(string[] args)
    {
      for (int index = 0; index < args.Length; index++)
      {
        string command = args[index].ToUpperInvariant();

        switch (command)
        {
          case "-BLAST":
            if (args.Length > index + 2)
              CopyDataWM.SendCopyDataMessage(Common.CmdPrefixBlast + args[++index] + '|' + args[++index]);
            else
              Console.WriteLine("Blast command requires two parameters.");
            continue;

          case "-MACRO":
            if (args.Length > index + 1)
              CopyDataWM.SendCopyDataMessage(Common.CmdPrefixMacro + args[++index]);
            else
              Console.WriteLine("Macro command requires a parameter.");
            continue;

          case "-EJECT":
            if (args.Length > index + 1)
              CopyDataWM.SendCopyDataMessage(Common.CmdPrefixEject + args[++index]);
            else
              Console.WriteLine("Eject command requires a parameter.");
            continue;

          case "-SHUTDOWN":
            CopyDataWM.SendCopyDataMessage(Common.CmdPrefixShutdown);
            continue;

          case "-REBOOT":
            CopyDataWM.SendCopyDataMessage(Common.CmdPrefixReboot);
            continue;

          case "-STANDBY":
            CopyDataWM.SendCopyDataMessage(Common.CmdPrefixStandby);
            continue;

          case "-HIBERNATE":
            CopyDataWM.SendCopyDataMessage(Common.CmdPrefixHibernate);
            continue;

          case "-LOGOFF":
            CopyDataWM.SendCopyDataMessage(Common.CmdPrefixLogOff);
            continue;

          case "-OSD":
            CopyDataWM.SendCopyDataMessage(Common.CmdPrefixTranslator);
            continue;

          case "-CHANNEL":
            {
              if (args.Length > index + 3)
              {
                string channel = args[++index];
                int padding = int.Parse(args[++index]);
                string port = args[++index];

                while (channel.Length < padding)
                  channel = '0' + channel;

                foreach (char digit in channel)
                  CopyDataWM.SendCopyDataMessage(Common.CmdPrefixBlast + digit + '|' + port);
              }
              else
              {
                Console.WriteLine("Channel command requires three parameters.");
              }

              continue;
            }

          //TODO: Add more command line options.
        }
      }
    }

    static void ShowOSD()
    {
      IrssLog.Info("Show OSD");

      if (_menuForm.Visible)
      {
        IrssLog.Info("OSD already visible");
      }
      else
      {
        Thread thread = new Thread(new ThreadStart(MenuThread));
        thread.Name = "Translator OSD";
        thread.Start();
      }
    }
    static void MenuThread()
    {
      try
      {
        _menuForm.ShowDialog();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
      }
    }

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

      _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripLabel("Translator"));
      _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());

      if (Config.Programs.Count > 0)
      {
        ToolStripMenuItem launch = new ToolStripMenuItem("&Launch");

        foreach (ProgramSettings programSettings in Config.Programs)
          launch.DropDownItems.Add(programSettings.Name, null, new EventHandler(ClickProgram));

        _notifyIcon.ContextMenuStrip.Items.Add(launch);
      }
      
      /*
      string[] irList = Common.GetIRList(false);
      if (irList.Length > 0)
      {
        ToolStripMenuItem irCommands = new ToolStripMenuItem("&IR Commands");

        foreach (string irCommand in irList)
          irCommands.DropDownItems.Add(irCommand, null, new EventHandler(ClickIrCommand));

        _notifyIcon.ContextMenuStrip.Items.Add(irCommands);
      }
      */

      string[] macroList = GetMacroList(false);
      if (macroList.Length > 0)
      {
        ToolStripMenuItem macros = new ToolStripMenuItem("&Macros");

        foreach (string macro in macroList)
          macros.DropDownItems.Add(macro, null, new EventHandler(ClickMacro));

        _notifyIcon.ContextMenuStrip.Items.Add(macros);
      }

      ToolStripMenuItem actions = new ToolStripMenuItem("&Actions");

      actions.DropDownItems.Add("System Standby", null, new EventHandler(ClickAction));
      actions.DropDownItems.Add("System Hibernate", null, new EventHandler(ClickAction));
      actions.DropDownItems.Add("System Reboot", null, new EventHandler(ClickAction));
      actions.DropDownItems.Add("System LogOff", null, new EventHandler(ClickAction));
      actions.DropDownItems.Add("System Shutdown", null, new EventHandler(ClickAction));

      actions.DropDownItems.Add(new ToolStripSeparator());

      ToolStripMenuItem ejectMenu = new ToolStripMenuItem("Eject");
      DriveInfo[] drives = DriveInfo.GetDrives();
      foreach (DriveInfo drive in drives)
        if (drive.DriveType == DriveType.CDRom)
          ejectMenu.DropDownItems.Add(drive.Name, null, new EventHandler(ClickEjectAction));
      actions.DropDownItems.Add(ejectMenu);

      actions.DropDownItems.Add(new ToolStripSeparator());

      actions.DropDownItems.Add("Volume Up", null, new EventHandler(ClickAction));
      actions.DropDownItems.Add("Volume Down", null, new EventHandler(ClickAction));
      actions.DropDownItems.Add("Volume Mute", null, new EventHandler(ClickAction));

      _notifyIcon.ContextMenuStrip.Items.Add(actions);

      _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
      _notifyIcon.ContextMenuStrip.Items.Add("Show &OSD", null, new EventHandler(ClickOSD));
      _notifyIcon.ContextMenuStrip.Items.Add("&Setup", null, new EventHandler(ClickSetup));
      _notifyIcon.ContextMenuStrip.Items.Add("&Quit", null, new EventHandler(ClickQuit));
    }

    static void ClickProgram(object sender, EventArgs e)
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
    static void ClickMacro(object sender, EventArgs e)
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
        IrssLog.Error(ex.ToString());
        MessageBox.Show(ex.Message, "Macro failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
    static void ClickAction(object sender, EventArgs e)
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
              Win32.ForegroundWindow(),
              (int)Win32.WindowsMessage.WM_APPCOMMAND,
              (int)Win32.AppCommand.APPCOMMAND_VOLUME_UP,
              0);
            break;

          case "Volume Down":
            Win32.SendWindowsMessage(
              Win32.ForegroundWindow(),
              (int)Win32.WindowsMessage.WM_APPCOMMAND,
              (int)Win32.AppCommand.APPCOMMAND_VOLUME_DOWN,
              0);
            break;

          case "Volume Mute":
            Win32.SendWindowsMessage(
              Win32.ForegroundWindow(),
              (int)Win32.WindowsMessage.WM_APPCOMMAND,
              (int)Win32.AppCommand.APPCOMMAND_VOLUME_MUTE,
              0);
            break;

          default:
            throw new ArgumentException(String.Format("Unknown action: {0}", menuItem.Text), "sender");
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
        MessageBox.Show(ex.Message, "Action failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    static void ClickEjectAction(object sender, EventArgs e)
    {
      IrssLog.Info("Click Eject Action");

      ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
      if (menuItem == null)
        return;

      ProcessCommand(Common.CmdPrefixEject + menuItem.Text, true);
    }

    static void ClickOSD(object sender, EventArgs e)
    {
      ShowOSD();
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

    static void CommsFailure(object obj)
    {
      Exception ex = obj as Exception;
      
      if (ex != null)
        IrssLog.Error("Communications failure: {0}", ex.Message);
      else
        IrssLog.Error("Communications failure");

      _notifyIcon.Icon = Properties.Resources.Icon16Connecting;
      _notifyIcon.Text = "Translator - Serious Communications Failure";

      StopClient();

      MessageBox.Show("Please report this error.", "Translator - Communications failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    static void Connected(object obj)
    {
      IrssLog.Info("Connected to server");

      IrssMessage message = new IrssMessage(MessageType.RegisterClient, MessageFlags.Request);
      _client.Send(message);

      _notifyIcon.Icon = Properties.Resources.Icon16;
      _notifyIcon.Text = "Translator";

      if (_firstConnection)
      {
        _firstConnection = false;
        MapEvent(MappingEvent.Translator_Start);
      }
    }
    static void Disconnected(object obj)
    {
      IrssLog.Warn("Communications with server has been lost");

      _notifyIcon.Icon = Properties.Resources.Icon16Connecting;
      _notifyIcon.Text = "Translator - Reconnecting ...";

      Thread.Sleep(1000);
    }

    internal static bool StartClient(IPEndPoint endPoint)
    {
      if (_client != null)
        return false;

      _notifyIcon.Icon = Properties.Resources.Icon16Connecting;
      _notifyIcon.Text = "Translator - Connecting ...";
      _notifyIcon.Visible = true;

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
      IrssLog.Debug("Received Message \"{0}\"", received.Type);

      try
      {
        switch (received.Type)
        {
          case MessageType.RemoteEvent:
            RemoteHandlerCallback(received.GetDataAsString());
            break;

          case MessageType.KeyboardEvent:
          {
            byte[] dataBytes = received.GetDataAsBytes();

            int vKey    = BitConverter.ToInt32(dataBytes, 0);
            bool keyUp  = BitConverter.ToBoolean(dataBytes, 4);

            KeyboardHandlerCallback(vKey, keyUp);
            break;
          }

          case MessageType.MouseEvent:
          {
            byte[] dataBytes = received.GetDataAsBytes();

            int deltaX  = BitConverter.ToInt32(dataBytes, 0);
            int deltaY  = BitConverter.ToInt32(dataBytes, 4);
            int buttons = BitConverter.ToInt32(dataBytes, 8);

            MouseHandlerCallback(deltaX, deltaY, buttons);
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

              IrssLog.Info("Registered to IR Server");
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              _registered = false;
              IrssLog.Warn("IR Server refused to register");
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
            IrssLog.Warn("IR Server Shutdown - Translator disabled until IR Server returns");
            _registered = false;

            _notifyIcon.Icon = Properties.Resources.Icon16Connecting;
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
        IrssLog.Error(ex.ToString());
      }
    }

    static ProgramSettings ActiveProgram()
    {
      try
      {
        int pid = Win32.GetForegroundWindowPID();
        if (pid == -1)
        {
          IrssLog.Debug("Failed to retreive foreground window process ID");
          return null;
        }

        string fileName = Path.GetFileName(Process.GetProcessById(pid).MainModule.FileName);

        foreach (ProgramSettings progSettings in Config.Programs)
        {
          if (fileName.Equals(Path.GetFileName(progSettings.FileName), StringComparison.OrdinalIgnoreCase))
          {
            return progSettings;
          }
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
      }

      IrssLog.Debug("Active program not found in Translator program list");
      return null;
    }

    static void RemoteHandlerCallback(string keyCode)
    {
      if (_inConfiguration)
        return;

      ProgramSettings active = ActiveProgram();
      if (active == null)
      {
        // Try system wide button mappings ...
        foreach (ButtonMapping buttonMap in _config.SystemWideMappings)
        {
          if (buttonMap.KeyCode.Equals(keyCode, StringComparison.Ordinal))
          {
            IrssLog.Debug("KeyCode {0} mapped in System Wide mappings", keyCode);
            try
            {
              ProcessCommand(buttonMap.Command, true);
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
          if (buttonMap.KeyCode.Equals(keyCode, StringComparison.Ordinal))
          {
            IrssLog.Debug("KeyCode {0} mapped in \"{1}\" mappings", keyCode, active.Name);
            try
            {
              ProcessCommand(buttonMap.Command, true);
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
            if (buttonMap.KeyCode.Equals(keyCode, StringComparison.Ordinal))
            {
              IrssLog.Debug("KeyCode {0} mapped in System Wide mappings", keyCode);
              try
              {
                ProcessCommand(buttonMap.Command, true);
              }
              catch (Exception ex) { IrssLog.Error(ex.ToString()); }
              return;
            }
          }
        }
      }

      IrssLog.Debug("No mapping found for KeyCode = {0}", keyCode);
    }
    static void KeyboardHandlerCallback(int vKey, bool keyUp)
    {
      if (keyUp)
        Keyboard.KeyUp((Keyboard.VKey)vKey);
      else
        Keyboard.KeyDown((Keyboard.VKey)vKey);
    }
    static void MouseHandlerCallback(int deltaX, int deltaY, int buttons)
    {
      if (buttons != (int)Mouse.MouseEvents.None)
        Mouse.Button((Mouse.MouseEvents)buttons);

      if (deltaX != 0 || deltaY != 0)
        Mouse.Move(deltaX, deltaY, false);
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
          if (String.IsNullOrEmpty(mappedEvent.Command))
          {
            IrssLog.Warn("Event found ({0}) with no command set", eventName);
          }
          else
          {
            try
            {
              IrssLog.Info("Event mapped: {0}, {1}", eventName, mappedEvent.Command);
              ProcessCommand(mappedEvent.Command, true);
            }
            catch (Exception ex)
            {
              IrssLog.Error(ex.ToString());
            }
          }
        }
      }
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

        IrssMessage message = new IrssMessage(MessageType.LearnIR, MessageFlags.Request);
        _client.Send(message);
      }
      catch (Exception ex)
      {
        _learnIRFilename = null;
        IrssLog.Error(ex.ToString());
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
          newThread.Priority = ThreadPriority.BelowNormal;
          newThread.Start(command);
        }
        catch (Exception ex)
        {
          IrssLog.Error(ex.ToString());
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
        else if (command.StartsWith(Common.CmdPrefixHibernate, StringComparison.OrdinalIgnoreCase))
        {
          if (!Common.Hibernate())
            IrssLog.Warn("Hibernate request was rejected by another application.");
        }
        else if (command.StartsWith(Common.CmdPrefixLogOff, StringComparison.OrdinalIgnoreCase))
        {
          if (!Common.LogOff())
            IrssLog.Warn("LogOff request failed.");
        }
        else if (command.StartsWith(Common.CmdPrefixReboot, StringComparison.OrdinalIgnoreCase))
        {
          if (!Common.Reboot())
            IrssLog.Warn("Reboot request failed.");
        }
        else if (command.StartsWith(Common.CmdPrefixShutdown, StringComparison.OrdinalIgnoreCase))
        {
          if (!Common.ShutDown())
            IrssLog.Warn("ShutDown request failed.");
        }
        else if (command.StartsWith(Common.CmdPrefixStandby, StringComparison.OrdinalIgnoreCase))
        {
          if (!Common.Standby())
            IrssLog.Warn("Standby request was rejected by another application.");
        }
        else if (command.StartsWith(Common.CmdPrefixTranslator, StringComparison.OrdinalIgnoreCase))
        {
          ShowOSD();
        }
        else
        {
          throw new ArgumentException(String.Format("Cannot process unrecognized command \"{0}\"", command), "command");
        }
      }
      catch (Exception ex)
      {
        if (Thread.CurrentThread.Name.Equals(ProcessCommandThreadName, StringComparison.OrdinalIgnoreCase))
          IrssLog.Error(ex.ToString());
        else
          throw ex;
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
              
            case Common.XmlTagPopup:
              {
                string[] commands = Common.SplitPopupCommand(commandProperty);
                MessageBox.Show(commands[1], commands[0], MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (_inConfiguration)
                  MessageBox.Show(commandProperty, Common.UITextKeys, MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                  Common.ProcessKeyCommand(commandProperty);
                break;
              }

            case Common.XmlTagMouse:
              {
                Common.ProcessMouseCommand(commandProperty);
                break;
              }

            case Common.XmlTagEject:
              {
                Common.ProcessEjectCommand(commandProperty);
                break;
              }

            case Common.XmlTagStandby:
              {
                if (_inConfiguration)
                  MessageBox.Show("Cannot enter Standby in configuration", Common.UITextStandby, MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                  Common.Standby();
                break;
              }

            case Common.XmlTagHibernate:
              {
                if (_inConfiguration)
                  MessageBox.Show("Cannot Hibernate in configuration", Common.UITextHibernate, MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                  Common.Hibernate();
                break;
              }

            case Common.XmlTagShutdown:
              {
                if (_inConfiguration)
                  MessageBox.Show("Cannot ShutDown in configuration", Common.UITextShutdown, MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                  Common.ShutDown();
                break;
              }

            case Common.XmlTagReboot:
              {
                if (_inConfiguration)
                  MessageBox.Show("Cannot Reboot in configuration", Common.UITextReboot, MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                  Common.Reboot();
                break;
              }

            case Common.XmlTagLogOff:
              {
                if (_inConfiguration)
                  MessageBox.Show("Cannot Log off in configuration", Common.UITextLogOff, MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                  Common.LogOff();
                break;
              }

            case Common.XmlTagBeep:
              {
                string[] commands = Common.SplitBeepCommand(commandProperty);
                Common.ProcessBeepCommand(commands);
                break;
              }

            case Common.XmlTagDisplay:
              {
                string[] commands = Common.SplitDisplayModeCommand(commandProperty);
                Common.ProcessDisplayModeCommand(commands);
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

    #endregion Implementation

  }

}
