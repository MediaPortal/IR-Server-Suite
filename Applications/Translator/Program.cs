using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    internal static Common.MessageHandler HandleMessage
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

      // Start the window focus watcher thread
      /*
      _focusWatcher = new Thread(new ThreadStart(FocusWatcherThread));
      _focusWatcher.Name = "Translator - Focus watcher";
      _focusWatcher.IsBackground = true;
      _focusWatcher.Start();
      */

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
            throw new Exception("Unknown action: " + menuItem.Text);
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
            IrssLog.Warn("No echo to ping, attempting to reconnect ...");

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
          case "Blast Success":
            break;

          case "Remote Event":
            {
              string keyCode = Encoding.ASCII.GetString(received.Data);
              RemoteHandlerCallback(keyCode);
              break;
            }

          case "Keyboard Event":
            {
              int vKey = BitConverter.ToInt32(received.Data, 0);
              bool keyUp = BitConverter.ToBoolean(received.Data, 4);
              KeyboardHandlerCallback(vKey, keyUp);
              break;
            }

          case "Mouse Event":
            {
              int deltaX = BitConverter.ToInt32(received.Data, 0);
              int deltaY = BitConverter.ToInt32(received.Data, 4);
              int buttons = BitConverter.ToInt32(received.Data, 8);
              MouseHandlerCallback(deltaX, deltaY, buttons);
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
              _irServerInfo = IRServerInfo.FromBytes(received.Data);
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
