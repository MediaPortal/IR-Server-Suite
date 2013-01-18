#region Copyright (C) 2005-2009 Team MediaPortal

// Copyright (C) 2005-2009 Team MediaPortal
// http://www.team-mediaportal.com
// 
// This Program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2, or (at your option)
// any later version.
// 
// This Program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with GNU Make; see the file COPYING.  If not, write to
// the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
// http://www.gnu.org/copyleft/gpl.html

#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using IrssCommands;
using IrssComms;
using IrssUtils;
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

    private static bool _connected;

    private static int _connectionFailures;
    #endregion Variables

    #region Properties

    internal static string ConfigFile
    {
      get { return _configFile; }
      set { _configFile = value; }
    }

    internal static Configuration Config { get; set; }

    internal static ClientMessageSink HandleMessage
    {
      get { return _handleMessage; }
      set { _handleMessage = value; }
    }

    internal static IRServerInfo TransceiverInformation
    {
      get { return _irServerInfo; }
    }

    internal static Processor CommandProcessor { get; private set; }

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
      _connectionFailures = 0;
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
        CopyDataWM.SendCopyDataMessage(Common.WM_ShowTrayIcon);
        return;
      }

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      IrssLog.LogLevel = IrssLog.Level.Debug;
      IrssLog.Open("Translator.log");

      Application.ThreadException += Application_ThreadException;

      // Initialize Variable List.
      _variables = new VariableList();

      // Load configuration ...
      Config = Configuration.Load(_configFile);
      if (Config == null)
      {
        IrssLog.Warn("Failed to load configuration file ({0}), creating new configuration", _configFile);
        Config = new Configuration();
      }

      // Adjust process priority ...
      AdjustPriority(Config.ProcessPriority);

      // Set directory for command plugin
      string appPath = Path.GetDirectoryName(Application.ExecutablePath);
      Processor.LibraryFolder = Path.Combine(appPath, "Commands");
      Processor.MacroFolder = FolderMacros;
      CommandProcessor = new Processor(BlastIR, TransceiverInformation.Ports);


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
      _notifyIcon.Visible = !Config.HideTrayIcon;

      // Setup the main form ...
      _mainForm = new MainForm();

      // Start server communications ...
      bool clientStarted = false;

      IPAddress serverIP = Network.GetIPFromName(Config.ServerHost);
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

        Command cmdToSend;
        switch (command)
        {
          case "BLAST":
            if (args.Length > index + 2)
            {
              string param1 = args[++index];
              string param2 = args[++index];
              cmdToSend = CreateCommandSafe(Common.CLASS_BlastIRCommand, new string[] {param1, param2});
              if (!ReferenceEquals(cmdToSend, null))
                CopyDataWM.SendCopyDataMessage(cmdToSend.ToString());
            }
            else
              throw new CommandStructureException("Blast command requires two parameters (IR file, Port)");
            break;

          case "MACRO":
            if (args.Length > index + 1)
            {
              string macroFile = args[++index];
              cmdToSend = CreateCommandSafe(Common.CLASS_CallMacroCommand, new string[] { macroFile });
              if (!ReferenceEquals(cmdToSend, null))
                CopyDataWM.SendCopyDataMessage(cmdToSend.ToString());
            }
            else
              throw new CommandStructureException("Macro command requires a parameter (Macro file)");
            break;

          case "EJECT":
            if (args.Length > index + 1)
            {
              string drive = args[++index];
              cmdToSend = CreateCommandSafe(Common.CLASS_EjectCommand, new string[] {drive});
              if (!ReferenceEquals(cmdToSend, null))
                CopyDataWM.SendCopyDataMessage(cmdToSend.ToString());
            }
            else
              throw new CommandStructureException("Eject command requires a parameter (Drive)");
            break;

          case "SHUTDOWN":
            cmdToSend = CreateCommandSafe(Common.CLASS_ShutdownCommand, null);
            if (!ReferenceEquals(cmdToSend, null))
              CopyDataWM.SendCopyDataMessage(cmdToSend.ToString());
            break;

          case "REBOOT":
            cmdToSend = CreateCommandSafe(Common.CLASS_RebootCommand, null);
            if (!ReferenceEquals(cmdToSend, null))
              CopyDataWM.SendCopyDataMessage(cmdToSend.ToString());
            break;

          case "STANDBY":
            cmdToSend = CreateCommandSafe(Common.CLASS_StandByCommand, null);
            if (!ReferenceEquals(cmdToSend, null))
              CopyDataWM.SendCopyDataMessage(cmdToSend.ToString());
            break;

          case "HIBERNATE":
            cmdToSend = CreateCommandSafe(Common.CLASS_HibernateCommand, null);
            if (!ReferenceEquals(cmdToSend, null))
              CopyDataWM.SendCopyDataMessage(cmdToSend.ToString());
            break;

          case "LOGOFF":
            cmdToSend = CreateCommandSafe(Common.CLASS_LogOffCommand, null);
            if (!ReferenceEquals(cmdToSend, null))
              CopyDataWM.SendCopyDataMessage(cmdToSend.ToString());
            break;

          case "OSD":
            CopyDataWM.SendCopyDataMessage(Common.WM_ShowTranslatorOSD);
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
                cmdToSend = CreateCommandSafe(Common.CLASS_EjectCommand, new string[] { digit.ToString(), port });
                if (!ReferenceEquals(cmdToSend, null))
                  CopyDataWM.SendCopyDataMessage(cmdToSend.ToString());

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

    internal static void ShowOSD()
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
        {
          ToolStripItem item = new ToolStripMenuItem();
          item.Text = programSettings.Name;
          item.Image = Win32.GetImageFromFile(programSettings.FileName);

          Command command = CreateCommandSafe(Common.CLASS_RunCommand, programSettings.GetRunCommandParameters());
          item.Tag = command;
          item.Click += ClickAction;
          item.Enabled = File.Exists(programSettings.FileName) && command != null;

          if (command == null)
            item.ToolTipText =
              "Was not able to create the RunCommand. Please check your command plugins and see log for more infos.";

          launch.DropDownItems.Add(item);
        }

        _notifyIcon.ContextMenuStrip.Items.Add(launch);
      }

      ToolStripMenuItem macros = new ToolStripMenuItem("&Macros");
      string[] macroList = Processor.GetListMacro(FolderMacros);
      foreach (string s in macroList)
      {
        string name = Path.GetFileNameWithoutExtension(s);
        Macro macro = CreateMacroSafe(s);
        if (macro == null) continue;

        macros.DropDownItems.Add(name, null, ClickAction).Tag = macro;
      }
      if (macros.DropDownItems.Count > 0)
        _notifyIcon.ContextMenuStrip.Items.Add(macros);


      ToolStripMenuItem actions = new ToolStripMenuItem("&Actions");

      actions.DropDownItems.Add("System Standby", null, ClickAction).Tag = CreateCommandSafe(Common.CLASS_StandByCommand, null);
      actions.DropDownItems.Add("System Hibernate", null, ClickAction).Tag = CreateCommandSafe(Common.CLASS_HibernateCommand, null);
      actions.DropDownItems.Add("System Reboot", null, ClickAction).Tag = CreateCommandSafe(Common.CLASS_RebootCommand, null);
      actions.DropDownItems.Add("System LogOff", null, ClickAction).Tag = CreateCommandSafe(Common.CLASS_LogOffCommand, null);
      actions.DropDownItems.Add("System Shutdown", null, ClickAction).Tag = CreateCommandSafe(Common.CLASS_ShutdownCommand, null);

      actions.DropDownItems.Add(new ToolStripSeparator());

      ToolStripMenuItem ejectMenu = new ToolStripMenuItem("Eject");
      DriveInfo[] drives = DriveInfo.GetDrives();
      foreach (DriveInfo drive in drives)
        if (drive.DriveType == DriveType.CDRom)
        {
          ejectMenu.DropDownItems.Add(drive.Name, null, ClickAction).Tag = CreateCommandSafe(Common.CLASS_EjectCommand,
                                                                                             new string[] {drive.Name});
        }
      if (ejectMenu.DropDownItems.Count > 0)
        actions.DropDownItems.Add(ejectMenu);

      //actions.DropDownItems.Add(new ToolStripSeparator());

      //actions.DropDownItems.Add("Volume Up", null, ClickAction).Tag = new VolumeUpCommand();
      //actions.DropDownItems.Add("Volume Down", null, ClickAction).Tag = new VolumeDownCommand();
      //actions.DropDownItems.Add("Volume Mute", null, ClickAction).Tag = new VolumeMuteCommand();

      _notifyIcon.ContextMenuStrip.Items.Add(actions);

      _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
      _notifyIcon.ContextMenuStrip.Items.Add("Show &OSD", null, ClickOSD);
      _notifyIcon.ContextMenuStrip.Items.Add("&Setup", null, ClickSetup);
      _notifyIcon.ContextMenuStrip.Items.Add("&Quit", null, ClickQuit);
    }

    private static void ClickAction(object sender, EventArgs e)
    {
      IrssLog.Info("Click Action");

      ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
      if (menuItem == null) return;
      if (menuItem.Tag == null) return;

      if (menuItem.Tag is Command)
      {
        Command command = (Command) menuItem.Tag;
        try
        {
          CommandProcessor.Execute(command, true);
        }
        catch (Exception ex)
        {
          IrssLog.Error(ex);
          MessageBox.Show(ex.Message, "Action failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      else if (menuItem.Tag is Macro)
      {
        Macro macro = (Macro) menuItem.Tag;
        try
        {
          macro.Execute(CommandProcessor);
        }
        catch (Exception ex)
        {
          IrssLog.Error(ex);
          MessageBox.Show(ex.Message, "Action failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
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
        _mainForm.BringToFront();
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
      _connectionFailures ++;
      Exception ex = obj as Exception;

      if (ex != null)
        IrssLog.Error("Communications failure: {0}", ex.Message);
      else
        IrssLog.Error("Communications failure");

      _notifyIcon.Icon = Resources.Icon16Connecting;
      _notifyIcon.Text = "Translator - Serious Communications Failure";

      StopClient();

      if (!_connected && _connectionFailures < 30)
      {
        Thread.Sleep(1000);

        IPAddress serverIP = Network.GetIPFromName(Config.ServerHost);
        IPEndPoint endPoint = new IPEndPoint(serverIP, Server.DefaultPort);

        try
        {
          StartClient(endPoint);
        }
        catch (Exception ex2)
        {
          IrssLog.Error(ex2);
        }
      } else
      {
        MessageBox.Show("Please report this error.", "Translator - Communications failure", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
      }
    }

    private static void Connected(object obj)
    {
      _connectionFailures = 0;
      IrssLog.Info("Connected to server");
      _connected = true;
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
      _connected = false;
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
            {
              string deviceName = received.MessageData[IrssMessage.DEVICE_NAME] as string;
              string keyCode = received.MessageData[IrssMessage.KEY_CODE] as string;

              RemoteHandlerCallback(deviceName, keyCode);
            }
            break;

          case MessageType.KeyboardEvent:
            {
              int vKey = (int)received.MessageData[IrssMessage.V_KEY];
              bool keyUp = (bool)received.MessageData[IrssMessage.KEY_UP];

              KeyboardHandlerCallback("TODO", vKey, keyUp);
              break;
            }

          case MessageType.MouseEvent:
            {
              int deltaX = (int)received.MessageData[IrssMessage.DELTA_X];
              int deltaY = (int)received.MessageData[IrssMessage.DELTA_Y];
              int buttons = (int)received.MessageData[IrssMessage.BUTTONS];

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
              IrssLog.Info("Registered to IR Server");
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              _registered = false;
              IrssLog.Warn("IR Server refused to register");
            }
            UpdateCommandProcessor();
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

            _notifyIcon.Icon = Resources.Icon16Connecting;
            _notifyIcon.Text = "Translator - Connecting ...";

            UpdateCommandProcessor();
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
        }
        catch
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
        foreach (ButtonMapping buttonMap in Config.SystemWideMappings)
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
          foreach (ButtonMapping buttonMap in Config.SystemWideMappings)
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
      if (_inConfiguration) return;

      string eventName = Enum.GetName(typeof (MappingEvent), theEvent);

      IrssLog.Debug("Mappable event: {0}", eventName);

      if (Config.Events.Count == 0)
      {
        IrssLog.Debug("No event mappings in current configuration");
        return;
      }

      foreach (MappedEvent mappedEvent in Config.Events)
      {
        if (mappedEvent.EventType != theEvent) continue;

        if (ReferenceEquals(mappedEvent.Command, null))
        {
          IrssLog.Warn("Event found ({0}) with no command set", eventName);
          continue;
        }

        try
        {
          IrssLog.Info("Event mapped: {0}, {1}", eventName, mappedEvent.Command);
          CommandProcessor.Execute(mappedEvent.Command, async);
        }
        catch (Exception ex)
        {
          IrssLog.Error(ex);
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
        throw new InvalidOperationException("Cannot Blast, not registered to an active IR Server");

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

    private static void UpdateCommandProcessor()
    {
      // chefkoch, cp should not set be null
      //CommandProcessor = null;
      //if (!_registered) return;

      CommandProcessor = new Processor(BlastIR, TransceiverInformation.Ports);
      
      if (!ReferenceEquals(_mainForm, null) && !ReferenceEquals(_mainForm._macroPanel, null))
        _mainForm._macroPanel.SetCommandProcessor(CommandProcessor);
    }

    /// <summary>
    /// Given a command this method processes the request accordingly.
    /// Throws exceptions only if run synchronously, if async exceptions are logged instead.
    /// </summary>
    /// <param name="command">Command to process.</param>
    /// <param name="async">Process command asynchronously?</param>
    internal static void ProcessCommand(Command command, bool async = false)
    {
      CommandProcessor.Execute(command, async);
    }

    /// <summary>
    /// Used by ProcessCommand to actually handle the command, if it is a string, otherwise it will call the other overload ProcCommand using the object as type Command.
    /// Can be called Synchronously or as a Parameterized Thread.
    /// </summary>
    /// <param name="commandObj">Command object to process. This could be a string or Command.</param>
    internal static void ProcessCommand(object commandObj)
    {
      try
      {
        if (commandObj == null)
          throw new ArgumentNullException("commandObj");

        Command command = commandObj as Command;
        if (command != null)
        {
          ProcessCommand(command);
          return;
        }

        string strCommand = commandObj as string;
        // is obj is not a command nor a string, stop here
        if (strCommand == null) return;


        // check for serialized Command
        try
        {
          command = Processor.CreateCommand(strCommand);
        }
        catch (Exception)
        {
          // catch all exception, as the provided text might not be a serialized command
        }
        if (command != null)
        {
          ProcessCommand(command);
          return;
        }


        // check for different strings which might have arrived through WindowMessages
        if (strCommand.StartsWith(Common.WM_ShowTranslatorOSD))
          ShowOSD();
        else if (strCommand.StartsWith(Common.WM_ShowTrayIcon, StringComparison.OrdinalIgnoreCase))
        {
          if (!_notifyIcon.Visible)
          {
            _notifyIcon.Visible = true;
            _notifyIcon.ShowBalloonTip(2000, "Translator", "Icon is now visible", ToolTipIcon.Info);
          }
        }

        //        if (String.IsNullOrEmpty(command))
        //          throw new ArgumentException("commandObj translates to empty or null string", "commandObj");

        //        else if (command.StartsWith(MacroSelectCommand.PREFIX))
        //        {
        //          bc = new MacroCommand(FolderMacros).CreateCommandFromString(command);
        //          ProcCommand(bc);
        //        }
        //#warning fixme BLAST
        //        //else if (command.StartsWith(Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
        //        //{
        //        //  string[] commands = Common.SplitBlastCommand(command.Substring(Common.CmdPrefixBlast.Length));
        //        //  BlastIR(Path.Combine(Common.FolderIRCommands, commands[0] + Common.FileExtensionIR), commands[1]);
        //        //}
        //        else
        //        {
        //          throw new ArgumentException(String.Format("Cannot process unrecognized command \"{0}\"", command),
        //                                      "commandObj");
        //        }
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

    internal static Command CreateCommandSafe(string commandType, string[] parameters)
    {
      try
      {
        return Processor.CreateCommand(commandType, parameters);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        return null;
      }
    }

    internal static Macro CreateMacroSafe(string filename)
    {
      try
      {
        return new Macro(filename);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        return null;
      }
    }

    #endregion Implementation
  }
}