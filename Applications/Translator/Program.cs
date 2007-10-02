using System;
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

    #endregion Constants

    #region Variables

    static Client _client = null;

    static Configuration _config;

    static string _learnIRFilename = null;

    static bool _registered = false;

    static bool _firstConnection = true;

    static ClientMessageSink _handleMessage;

    static bool _inConfiguration;

    static IRServerInfo _irServerInfo = new IRServerInfo();

    //static Thread _focusWatcher;
    static IntPtr _currentForegroundWindow = IntPtr.Zero;

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
        ProcessCommandLine(args);
        return;
      }      
      
      // Check for multiple instances.
      if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length != 1)
        return;

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      // TODO: Change log level to info for release.
      IrssLog.LogLevel = IrssLog.Level.Debug;
      IrssLog.Open(Common.FolderIrssLogs + "Translator.log");

      Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

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

      // Setup main form ...
      _mainForm = new MainForm();

      // Start the window focus watcher thread
      /*
      _focusWatcher = new Thread(new ThreadStart(FocusWatcherThread));
      _focusWatcher.Name = "Translator - Focus watcher";
      _focusWatcher.IsBackground = true;
      _focusWatcher.Start();
      */

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

        Application.Run();

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

      //if (_focusWatcher.IsAlive)
        //_focusWatcher.Abort();

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
    public static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
    {
      IrssLog.Error(e.Exception.ToString());
    }

    /*static void FocusWatcherThread()
    {
      try
      {
        while (true)
        {
          UpdateForegroundWindow();
          Thread.Sleep(2000);
        }
      }
      catch
      {
      }
    }*/

    static void UpdateForegroundWindow()
    {
      try
      {
        IntPtr hWnd = Win32.GetForegroundWindow();

        if (hWnd == IntPtr.Zero)
          return;

        if (hWnd == _mainForm.Handle)
          return;

        if (hWnd == _notifyIcon.ContextMenuStrip.Handle)
          return;

        /*
                string windowTitle = Win32.GetWindowTitle(hWnd);
                if (windowTitle.StartsWith("Translator", StringComparison.InvariantCultureIgnoreCase))
                  return;

                int procID;
                Win32.GetWindowThreadProcessId(hWnd, out procID);
                Process proc = Process.GetProcessById(procID);
                if (proc.MainModule.ModuleName.Equals("Translator.exe", StringComparison.InvariantCultureIgnoreCase))
                  return;
                */

        _currentForegroundWindow = hWnd;
      }
      catch
      {
      }
    }

    static void ProcessCommandLine(string[] args)
    {
      for (int index = 0; index < args.Length; index++)
      {
        switch (args[index].ToLowerInvariant())
        {
          case "-macro":
            SendCopyDataMessage("Translator", Common.CmdPrefixMacro + args[++index]);
            continue;

          case "-eject":
            SendCopyDataMessage("Translator", Common.CmdPrefixEject + args[++index]);
            continue;

          case "-shutdown":
            SendCopyDataMessage("Translator", Common.CmdPrefixShutdown);
            continue;

          case "-reboot":
            SendCopyDataMessage("Translator", Common.CmdPrefixReboot);
            continue;

          case "-standby":
            SendCopyDataMessage("Translator", Common.CmdPrefixStandby);
            continue;

          case "-hibernate":
            SendCopyDataMessage("Translator", Common.CmdPrefixHibernate);
            continue;

          //TODO: Add more command line options.
        }
      }
    }

    static void ShowTranslatorMenu()
    {
      UpdateForegroundWindow();

      //Program._mainForm.Focus();

      _notifyIcon.ContextMenuStrip.Show(Screen.PrimaryScreen.Bounds.Width / 4, Screen.PrimaryScreen.Bounds.Height / 4);

      //_notifyIcon.ContextMenuStrip.Focus();
    }

    static void SendCopyDataMessage(string targetWindow, string data)
    {
      Win32.COPYDATASTRUCT copyData;

      copyData.dwData = 24;
      copyData.lpData = Win32.VarPtr(data).ToInt32();
      copyData.cbData = data.Length;

      IntPtr windowHandle = Win32.FindWindow(null, targetWindow);
      if (windowHandle != IntPtr.Zero)
      {
        IntPtr result;
        Win32.SendMessageTimeout(windowHandle, (int)Win32.WindowsMessage.WM_COPYDATA, IntPtr.Zero, Win32.VarPtr(copyData), Win32.SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 1000, out result);

        if (result == IntPtr.Zero)
        {
          int lastError = Marshal.GetLastWin32Error();
          throw new Win32Exception(lastError);
        }
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

      /**/
      ToolStripMenuItem actions = new ToolStripMenuItem("&Actions");
      actions.DropDownItems.Add("Next Window", null, new EventHandler(ClickAction));
      actions.DropDownItems.Add("Last Window", null, new EventHandler(ClickAction));
      actions.DropDownItems.Add("Close Window", null, new EventHandler(ClickAction));
      actions.DropDownItems.Add("Maximize Window", null, new EventHandler(ClickAction));
      actions.DropDownItems.Add("Minimize Window", null, new EventHandler(ClickAction));
      actions.DropDownItems.Add("Restore Window", null, new EventHandler(ClickAction));

      actions.DropDownItems.Add(new ToolStripSeparator());

      actions.DropDownItems.Add("System Standby", null, new EventHandler(ClickAction));
      actions.DropDownItems.Add("System Hibernate", null, new EventHandler(ClickAction));
      actions.DropDownItems.Add("System Reboot", null, new EventHandler(ClickAction));
      //actions.DropDownItems.Add("System Logoff", null, new EventHandler(ClickAction));
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
      /**/

      _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
      _notifyIcon.ContextMenuStrip.Items.Add("&Setup", null, new EventHandler(ClickSetup));
      _notifyIcon.ContextMenuStrip.Items.Add("&Quit", null, new EventHandler(ClickQuit));
    }

    /**/
    static bool SendMessageToWindow(IntPtr hWnd, Win32.WindowsMessage msg, int wParam, int lParam)
    {
      IntPtr result;
      Win32.SendMessageTimeout(hWnd, (int)msg, new IntPtr(wParam), new IntPtr(lParam), Win32.SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 1000, out result);

      if (result == IntPtr.Zero)
      {
        int lastError = Marshal.GetLastWin32Error();
        if (lastError != 0)
          throw new Win32Exception(lastError);
      }

      return true;
    }
    /**/

    static void ClickProgram(object sender, EventArgs e)
    {
      IrssLog.Info("Click Launch Program");

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
            string launchCommand = programSettings.LaunchCommand();
            string[] commands = Common.SplitRunCommand(launchCommand);
            
            Common.ProcessRunCommand(commands);
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

    /*
    static void ClickIrCommand(object sender, EventArgs e)
    {
      IrssLog.Info("Click IR Command");

      ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
      if (menuItem == null)
        return;

      string irCommand = menuItem.Text;
    }
    */

    static void ClickMacro(object sender, EventArgs e)
    {
      IrssLog.Info("Click Macro");

      ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
      if (menuItem == null)
        return;

      string fileName = Program.FolderMacros + menuItem.Text + Common.FileExtensionMacro;

      try
      {
        Program.ProcessMacro(fileName);
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
          case "Next Window":
            SendMessageToWindow(
              _currentForegroundWindow,
              Win32.WindowsMessage.WM_SYSCOMMAND,
              (int)Win32.SysCommand.SC_NEXTWINDOW,
              0);
            break;

          case "Last Window":
            SendMessageToWindow(
              _currentForegroundWindow,
              Win32.WindowsMessage.WM_SYSCOMMAND,
              (int)Win32.SysCommand.SC_PREVWINDOW,
              0);
            break;

          case "Close Window":
            SendMessageToWindow(
              _currentForegroundWindow,
              Win32.WindowsMessage.WM_SYSCOMMAND,
              (int)Win32.SysCommand.SC_CLOSE,
              0);
            break;

          case "Maximize Window":
            SendMessageToWindow(
              _currentForegroundWindow,
              Win32.WindowsMessage.WM_SYSCOMMAND,
              (int)Win32.SysCommand.SC_MAXIMIZE,
              0);
            break;

          case "Minimize Window":
            SendMessageToWindow(
              _currentForegroundWindow,
              Win32.WindowsMessage.WM_SYSCOMMAND,
              (int)Win32.SysCommand.SC_MINIMIZE,
              0);
            break;

          case "Restore Window":
            SendMessageToWindow(
              _currentForegroundWindow,
              Win32.WindowsMessage.WM_SYSCOMMAND,
              (int)Win32.SysCommand.SC_RESTORE,
              0);
            break;

          case "System Standby":
            Standby();
            break;

          case "System Hibernate":
            Hibernate();
            break;

          case "System Reboot":
            Reboot();
            break;

            /*
          case "System Logoff":
            LogOff();
            break;
            */

          case "System Shutdown":
            ShutDown();
            break;


          case "Volume Up":
            SendMessageToWindow(
              _currentForegroundWindow,
              Win32.WindowsMessage.WM_APPCOMMAND,
              (int)Win32.AppCommand.APPCOMMAND_VOLUME_UP,
              0);
            break;

          case "Volume Down":
            SendMessageToWindow(
              _currentForegroundWindow,
              Win32.WindowsMessage.WM_APPCOMMAND,
              (int)Win32.AppCommand.APPCOMMAND_VOLUME_DOWN,
              0);
            break;

          case "Volume Mute":
            SendMessageToWindow(
              _currentForegroundWindow,
              Win32.WindowsMessage.WM_APPCOMMAND,
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

      CDRom.Open(menuItem.Text);
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
              IrssLog.Debug("Blast successful");
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
          IrssLog.Debug("Error retreiving foreground window process ID");
          return null;
        }

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

    static void RemoteHandlerCallback(string keyCode)
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


    static void Hibernate()
    {
      IrssLog.Info("Hibernate");
      Application.SetSuspendState(PowerState.Hibernate, true, false);
    }
    static void Standby()
    {
      IrssLog.Info("Standby");
      Application.SetSuspendState(PowerState.Suspend, true, false);
    }
    static void Reboot()
    {
      IrssLog.Info("Reboot");
      Win32.ExitWindowsEx(Win32.ExitWindows.Reboot | Win32.ExitWindows.ForceIfHung, Win32.ShutdownReasons.FlagUserDefined);
    }
    /*static void LogOff()
    {
      IrssLog.Info("LogOff");
      Win32.ExitWindowsEx(Win32.ExitWindows.LogOff | Win32.ExitWindows.ForceIfHung, Win32.ShutdownReason.FlagUserDefined);
    }*/
    static void ShutDown()
    {
      IrssLog.Info("ShutDown");
      Win32.ExitWindowsEx(Win32.ExitWindows.ShutDown | Win32.ExitWindows.ForceIfHung, Win32.ShutdownReasons.FlagUserDefined);
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
          catch (Exception ex)
          {
            IrssLog.Error(ex.ToString());
          }
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
        throw new ApplicationException("Cannot process Macro with Blast commands when not registered to an active IR Server");

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

          case Common.XmlTagTcpMsg:
            {
              string[] commands = Common.SplitTcpMessageCommand(commandProperty);
              Common.ProcessTcpMessageCommand(commands);
              break;
            }

          case Common.XmlTagKeys:
            {
              Common.ProcessKeyCommand(commandProperty);
              break;
            }

          case Common.XmlTagEject:
            {
              Common.ProcessEjectCommand(commandProperty);
              break;
            }

          case Common.XmlTagStandby:
            {
              Standby();
              break;
            }

          case Common.XmlTagHibernate:
            {
              Hibernate();
              break;
            }

          case Common.XmlTagShutdown:
            {
              ShutDown();
              break;
            }

          case Common.XmlTagReboot:
            {
              Reboot();
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
      else if (command.StartsWith(Common.CmdPrefixWindowMsg)) // Message Command
      {
        string[] commands = Common.SplitWindowMessageCommand(command.Substring(Common.CmdPrefixWindowMsg.Length));
        Common.ProcessWindowMessageCommand(commands);
      }
      else if (command.StartsWith(Common.CmdPrefixKeys)) // Keystroke Command
      {
        string keyCommand = command.Substring(Common.CmdPrefixKeys.Length);
        Common.ProcessKeyCommand(keyCommand);
      }
      else if (command.StartsWith(Common.CmdPrefixEject)) // Eject Command
      {
        string ejectCommand = command.Substring(Common.CmdPrefixEject.Length);
        Common.ProcessEjectCommand(ejectCommand);
      }
      else if (command.StartsWith(Common.CmdPrefixMouse)) // Mouse Command
      {
        string mouseCommand = command.Substring(Common.CmdPrefixMouse.Length);
        Common.ProcessMouseCommand(mouseCommand);
      }
      else if (command.StartsWith(Common.CmdPrefixHibernate)) // Hibernate Command
      {
        Hibernate();
      }
      else if (command.StartsWith(Common.CmdPrefixReboot)) // Reboot Command
      {
        Reboot();
      }
      else if (command.StartsWith(Common.CmdPrefixShutdown)) // Shutdown Command
      {
        ShutDown();
      }
      else if (command.StartsWith(Common.CmdPrefixStandby)) // Standby Command
      {
        Standby();
      }
      else if (command.StartsWith(Common.CmdPrefixTranslator)) // Translator Command
      {
        ShowTranslatorMenu();
      }
      else
      {
        throw new ArgumentException(String.Format("Cannot process unrecognized command \"{0}\"", command), "command");
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

    #endregion Implementation

  }

}
