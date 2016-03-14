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
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using IrssUtils.Exceptions;

namespace IrssUtils
{

    #region Delegates

    /// <summary>
    /// Provides a delegate to call to test a command.
    /// </summary>
    /// <param name="fileName">Full file path to the IR Command file.</param>
    /// <param name="port">IR Blaster port to transmit on.</param>
    public delegate void BlastIrDelegate(string fileName, string port);

    /// <summary>
    /// Provides a delegate to call to when learning of a new command is completed.
    /// </summary>
    /// <param name="status">description of the learning oucome</param>
    /// <param name="success">true if learning has completed successfully</param>
    public delegate void LearnStatusDelegate(string status, bool success);


    /// <summary>
    /// Provides a delegate to call to learn a new IR Command.
    /// </summary>
    /// <param name="fileName">Full file path to the IR Command file.</param>
    /// <returns>Successfully started IR learn process.</returns>
    public delegate bool LearnIrDelegate(string fileName, LearnStatusDelegate learnStatus=null);

    /// <summary>
    /// Provides a delegate to execute a IRSS command. 
    /// </summary>
    /// <param name="command">the IRSS command and its arguments</param>
    /// <param name="async">true is the execution of the command should be non-blocking</param>
    public delegate void ProcessCommandDelegate(string command, bool async);


    #endregion Delegates

    /// <summary>
    /// Common code class.
    /// </summary>
    public static class Common
    {
        #region Constants

        #region Folders

        /// <summary>
        /// IR Server Suite "Application Data" folder location (includes trailing '\')
        /// </summary>
        public static readonly string FolderAppData =
          Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "IR Server Suite");

        /// <summary>
        /// IR Server Suite "IR Commands" folder location (includes trailing '\')
        /// </summary>
        public static readonly string FolderIRCommands = Path.Combine(FolderAppData, "IR Commands");

        /// <summary>
        /// IR Server Suite "Logs" folder location (includes trailing '\')
        /// </summary>
        public static readonly string FolderIrssLogs = Path.Combine(FolderAppData, "Logs");

        /// <summary>
        /// IR Server Suite "Set Top Boxes" folder location (includes trailing '\')
        /// </summary>
        public static readonly string FolderSTB = Path.Combine(FolderAppData, "Set Top Boxes");

        #endregion Folders

        #region File Extensions

        /// <summary>
        /// File extension for IR Commands.
        /// </summary>
        public const string FileExtensionIR = ".IR";

        /// <summary>
        /// File extension for Macros.
        /// </summary>
        public const string FileExtensionMacro = ".Macro";

        /// <summary>
        /// File extension for stored Variable Lists.
        /// </summary>
        public const string FileExtensionVariableList = ".VariableList";

        #endregion File Extensions

        #region Strings

        /// <summary>
        /// Variables must be prefixed with this string.
        /// </summary>
        public const string VariablePrefix = "var_";

        #region Command Prefixes

        public const string CmdPrefixBeep = "Beep: ";
        public const string CmdPrefixBlast = "Blast: ";
        public const string CmdPrefixClearVars = "Clear Variables";
        public const string CmdPrefixCloseProgram = "Close Program: ";
        public const string CmdPrefixCommand = "Command: ";
        //  public const string CmdPrefixWindowState  = "Toggle Window State";

        public const string CmdPrefixDisplayMode = "Display Mode: ";
        public const string CmdPrefixDisplayPower = "Display Power: ";
        public const string CmdPrefixEject = "Eject: ";
        public const string CmdPrefixExit = "Exit MediaPortal";
        public const string CmdPrefixFocus = "Get Focus";

        // Macro Commands ...
        public const string CmdPrefixGotoLabel = "Goto Label: ";
        public const string CmdPrefixGotoScreen = "Goto: ";
        public const string CmdPrefixHibernate = "Hibernate";
        public const string CmdPrefixHttpMsg = "HTTP Message: ";
        public const string CmdPrefixIf = "If: ";
        public const string CmdPrefixInputLayer = "Toggle Input Layer";
        public const string CmdPrefixKeys = "Keys: ";
        public const string CmdPrefixLabel = "Label: ";
        public const string CmdPrefixLoadVars = "Load Variables: ";
        public const string CmdPrefixLogOff = "Log Off";
        public const string CmdPrefixMacro = "Macro: ";
        public const string CmdPrefixMouse = "Mouse: ";
        public const string CmdPrefixMouseMode = "Mouse Mode: ";

        // For MediaPortal ...
        public const string CmdPrefixMultiMap = "Multi-Mapping: ";
        public const string CmdPrefixPause = "Pause: ";
        public const string CmdPrefixPopup = "Popup: ";
        public const string CmdPrefixReboot = "Reboot";
        public const string CmdPrefixRun = "Run: ";
        public const string CmdPrefixSaveVars = "Save Variables: ";
        public const string CmdPrefixSendMPAction = "MediaPortal Action: ";
        public const string CmdPrefixSendMPMsg = "MediaPortal Message: ";
        public const string CmdPrefixSerial = "Serial: ";
        public const string CmdPrefixSetVar = "Set Variable: ";
        public const string CmdPrefixShowTrayIcon = "Show Tray Icon";
        public const string CmdPrefixShutdown = "Shutdown";
        public const string CmdPrefixSmsKB = "Show SMS Keyboard";
        public const string CmdPrefixSound = "Sound: ";
        public const string CmdPrefixStandby = "Standby";
        public const string CmdPrefixSTB = "STB: ";
        public const string CmdPrefixTcpMsg = "TCP Message: ";
        public const string CmdPrefixTranslator = "Show Translator Launcher";
        public const string CmdPrefixVirtualKB = "Show Virtual Keyboard";
        public const string CmdPrefixWindowMsg = "Window Message: ";

        #endregion Command Prefixes

        #region User Interface Text

        public const string UITextBeep = "Beep";
        public const string UITextClearVars = "Clear Variables";
        public const string UITextCloseProgram = "Close a Running Program";
        public const string UITextDisplayMode = "Display Mode";
        public const string UITextDisplayPower = "Display Power";
        public const string UITextEject = "Eject CD";
        public const string UITextExit = "Exit MediaPortal";
        public const string UITextFocus = "Get Focus";

        // Macro Commands ...
        public const string UITextGotoLabel = "Goto Label";
        public const string UITextGotoScreen = "Go To Screen";
        public const string UITextHibernate = "Hibernate";
        public const string UITextHttpMsg = "HTTP Message";
        public const string UITextIf = "If Statement";
        public const string UITextInputLayer = "Toggle Input Handler Layer";
        public const string UITextKeys = "Keystrokes Command";
        public const string UITextLabel = "Insert Label";
        public const string UITextLoadVars = "Load Variables";
        public const string UITextLogOff = "Log Off";
        public const string UITextMacro = "Macro";
        public const string UITextMouse = "Mouse Command";
        public const string UITextMouseMode = "Set Mouse Mode";

        // For MediaPortal ...
        public const string UITextMultiMap = "Set Multi-Mapping";
        public const string UITextPause = "Pause";
        public const string UITextPopup = "Popup Message";
        public const string UITextReboot = "Reboot";
        public const string UITextRun = "Run Program";
        public const string UITextSaveVars = "Save Variables";
        public const string UITextSendMPAction = "Send MediaPortal Action";
        public const string UITextSendMPMsg = "Send MediaPortal Message";
        public const string UITextSerial = "Serial Command";
        public const string UITextSetVar = "Set Variable";
        public const string UITextShutdown = "Shutdown";
        public const string UITextSmsKB = "Show SMS Keyboard";
        public const string UITextSound = "Play Sound";
        public const string UITextStandby = "Standby";
        public const string UITextTcpMsg = "TCP Message";
        public const string UITextTranslator = "Show Translator Launcher";
        public const string UITextVirtualKB = "Show Virtual Keyboard";
        public const string UITextWindowMsg = "Window Message";

        #endregion User Interface Text

        #region Mouse Commands

        public const string MouseClickLeft = "Click_Left";
        public const string MouseClickMiddle = "Click_Middle";
        public const string MouseClickRight = "Click_Right";

        public const string MouseDoubleClickLeft = "DoubleClick_Left";
        public const string MouseDoubleClickMiddle = "DoubleClick_Middle";
        public const string MouseDoubleClickRight = "DoubleClick_Right";
        public const string MouseMoveDown = "Move_Down ";
        public const string MouseMoveLeft = "Move_Left ";
        public const string MouseMoveRight = "Move_Right ";

        public const string MouseMoveToPos = "Move_To_Pos ";
        public const string MouseMoveUp = "Move_Up ";

        public const string MouseScrollDown = "Scroll_Down";
        public const string MouseScrollUp = "Scroll_Up";

        #endregion Mouse Commands

        #region Command Targets

        /// <summary>
        /// Target the active window.
        /// </summary>
        public const string TargetActive = "ACTIVE";

        /// <summary>
        /// Target an application.
        /// </summary>
        public const string TargetApplication = "APPLICATION";

        /// <summary>
        /// Target a class.
        /// </summary>
        public const string TargetClass = "CLASS";

        /// <summary>
        /// Target a window title.
        /// </summary>
        public const string TargetWindow = "WINDOW";

        #endregion Commad Targets

        #endregion Strings

        #region Command Segments

        /// <summary>
        /// Number of Segments in a Beep Command.
        /// </summary>
        private const int SegmentsBeepCommand = 2;

        /// <summary>
        /// Number of Segments in a Blast Command.
        /// </summary>
        private const int SegmentsBlastCommand = 2;

        /// <summary>
        /// Number of Segments in a Close Program Command.
        /// </summary>
        private const int SegmentsCloseProgramCommand = 2;

        /// <summary>
        /// Number of Segments in a Display Mode Command.
        /// </summary>
        private const int SegmentsDisplayModeCommand = 4;

        /// <summary>
        /// Number of Segments in a HTTP Message Command.
        /// </summary>
        private const int SegmentsHttpMessageCommand = 4;

        /// <summary>
        /// Number of Segments in an If Command.
        /// </summary>
        private const int SegmentsIfCommand = 5;

        /// <summary>
        /// Number of Segments in a Popup Command.
        /// </summary>
        private const int SegmentsPopupCommand = 3;

        /// <summary>
        /// Number of Segments in a Run Command.
        /// </summary>
        private const int SegmentsRunCommand = 8;

        /// <summary>
        /// Number of Segments in a Send MP Action Command.
        /// </summary>
        private const int SegmentsSendMPActionCommand = 3;

        /// <summary>
        /// Number of Segments in a Send MP Message Command.
        /// </summary>
        private const int SegmentsSendMPMessageCommand = 6;

        /// <summary>
        /// Number of Segments in a Serial Command.
        /// </summary>
        private const int SegmentsSerialCommand = 7;

        /// <summary>
        /// Number of Segments in a Set Variable Command.
        /// </summary>
        private const int SegmentsSetVarCommand = 2;

        /// <summary>
        /// Number of Segments in a TCP Message Command.
        /// </summary>
        private const int SegmentsTcpMessageCommand = 3;

        /// <summary>
        /// Number of Segments in a Windows Message Command.
        /// </summary>
        private const int SegmentsWindowMessageCommand = 5;

        #endregion Command Segments

        #endregion Constants

        #region stub

        /*
    /// <summary>
    /// Get a list of commands found in the Command Libraries.
    /// </summary>
    /// <returns>Available commands.</returns>
    public static Type[] GetLibraryCommands()
    {
      try
      {
        List<Type> commands = new List<Type>();

        string installFolder = SystemRegistry.GetInstallFolder();
        if (String.IsNullOrEmpty(installFolder))
          return null;

        string folder = Path.Combine(installFolder, "Commands");
        string[] files = Directory.GetFiles(folder, "*.dll", SearchOption.TopDirectoryOnly);

        foreach (string file in files)
        {
          try
          {
            Assembly assembly = Assembly.LoadFrom(file);
            Type[] types = assembly.GetExportedTypes();

            foreach (Type type in types)
              if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(Commands.Command)))
                commands.Add(type);
          }
          catch (BadImageFormatException)
          {
            // Ignore Bad Image Format Exceptions, just keep checking for IR Server Plugins
          }
          catch (TypeLoadException)
          {
            // Ignore Type Load Exceptions, just keep checking for IR Server Plugins
          }
          catch (Exception ex)
          {
            MessageBox.Show(ex.ToString(), "IR Server Command Error");
          }
        }

        return commands.ToArray();
      }
#if TRACE
      catch (Exception ex)
      {
        Trace.WriteLine(ex.ToString());
#else
      catch
      {
#endif
        return null;
      }
    }
    */
        #endregion stub



        #region Methods

        #region Command Splitters

        /// <summary>
        /// Splits an If Command into it's component parts.
        /// </summary>
        /// <param name="command">The command to be split.</param>
        /// <returns>Returns string[] of command elements.</returns>
        public static string[] SplitIfCommand(string command)
        {
            return SplitCommand(command, SegmentsIfCommand);
        }

        /// <summary>
        /// Splits a Set Variable Command into it's component parts.
        /// </summary>
        /// <param name="command">The command to be split.</param>
        /// <returns>Returns string[] of command elements.</returns>
        public static string[] SplitSetVarCommand(string command)
        {
            return SplitCommand(command, SegmentsSetVarCommand);
        }

        /// <summary>
        /// Splits a Blast command into it's component parts.
        /// </summary>
        /// <param name="command">The command to be split.</param>
        /// <returns>Returns string[] of command elements.</returns>
        public static string[] SplitBlastCommand(string command)
        {
            return SplitCommand(command, SegmentsBlastCommand);
        }

        /// <summary>
        /// Splits a Run command into it's component parts.
        /// </summary>
        /// <param name="command">The command to be split.</param>
        /// <returns>Returns string[] of command elements.</returns>
        public static string[] SplitRunCommand(string command)
        {
            return SplitCommand(command, SegmentsRunCommand);
        }

        /// <summary>
        /// Splits a Serial Command into it's component parts.
        /// </summary>
        /// <param name="command">The command to be split.</param>
        /// <returns>Returns string[] of command elements.</returns>
        public static string[] SplitSerialCommand(string command)
        {
            return SplitCommand(command, SegmentsSerialCommand);
        }

        /// <summary>
        /// Splits a Window Message Command into it's component parts.
        /// </summary>
        /// <param name="command">The command to be split.</param>
        /// <returns>Returns string[] of command elements.</returns>
        public static string[] SplitWindowMessageCommand(string command)
        {
            return SplitCommand(command, SegmentsWindowMessageCommand);
        }

        /// <summary>
        /// Splits a Popup Command into it's component parts.
        /// </summary>
        /// <param name="command">The command to be split.</param>
        /// <returns>Returns string[] of command elements.</returns>
        public static string[] SplitPopupCommand(string command)
        {
            return SplitCommand(command, SegmentsPopupCommand);
        }

        /// <summary>
        /// Splits a TCP Message Command into it's component parts.
        /// </summary>
        /// <param name="command">The command to be split.</param>
        /// <returns>Returns string[] of command elements.</returns>
        public static string[] SplitTcpMessageCommand(string command)
        {
            return SplitCommand(command, SegmentsTcpMessageCommand);
        }

        /// <summary>
        /// Splits an HTTP Message Command into it's component parts.
        /// </summary>
        /// <param name="command">The command to be split.</param>
        /// <returns>Returns string[] of command elements.</returns>
        public static string[] SplitHttpMessageCommand(string command)
        {
            return SplitCommand(command, SegmentsHttpMessageCommand);
        }

        /// <summary>
        /// Splits a Beep Command into it's component parts.
        /// </summary>
        /// <param name="command">The command to be split.</param>
        /// <returns>Returns string[] of command elements.</returns>
        public static string[] SplitBeepCommand(string command)
        {
            return SplitCommand(command, SegmentsBeepCommand);
        }

        /// <summary>
        /// Splits a Display Mode Command into it's component parts.
        /// </summary>
        /// <param name="command">The command to be split.</param>
        /// <returns>Returns string[] of command elements.</returns>
        public static string[] SplitDisplayModeCommand(string command)
        {
            return SplitCommand(command, SegmentsDisplayModeCommand);
        }

        /// <summary>
        /// Splits a Close Program Command into it's component parts.
        /// </summary>
        /// <param name="command">The command to be split.</param>
        /// <returns>Returns string[] of command elements.</returns>
        public static string[] SplitCloseProgramCommand(string command)
        {
            return SplitCommand(command, SegmentsCloseProgramCommand);
        }

        /// <summary>
        /// Splits a Send MP Action Command into it's component parts.
        /// </summary>
        /// <param name="command">The command to be split.</param>
        /// <returns>Returns string[] of command elements.</returns>
        public static string[] SplitSendMPActionCommand(string command)
        {
            return SplitCommand(command, SegmentsSendMPActionCommand);
        }

        /// <summary>
        /// Splits a Send MP Message Command into it's component parts.
        /// </summary>
        /// <param name="command">The command to be split.</param>
        /// <returns>Returns string[] of command elements.</returns>
        public static string[] SplitSendMPMsgCommand(string command)
        {
            return SplitCommand(command, SegmentsSendMPMessageCommand);
        }

        /// <summary>
        /// Splits the command (Gets called by all the specific command splitters).
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="elements">The number of element.</param>
        /// <returns>Returns string[] of command elements.</returns>
        private static string[] SplitCommand(string command, int elements)
        {
            if (String.IsNullOrEmpty(command))
                throw new ArgumentNullException("command");

            string[] commands = command.Split(new Char [] {'|'});

            if (elements == 0 || commands.Length != elements)
            {
                if (command != "")
                    throw new CommandStructureException(String.Format("Command structure does not split as expected: {0}", command));
                return null;
            }

            return commands;

            /** This alternative code allows to use the | character in the commands but is disabled for backward-compatibility **

            if (String.IsNullOrEmpty(command))
                throw new ArgumentNullException("command");

            if (elements == 0)
            {
                if (command != "") 
                    throw new CommandStructureException(String.Format("Command structure does not split as expected: {0}", command));
                return null;
            }

            string[] commands = new string[elements];

            int idx = 0;


            StringBuilder arg = new StringBuilder(command.Length);
            bool escaped = false;
            foreach (var c in command)
            {
                switch (c)
                {
                    case '\\':  // escape character
                        if (escaped) arg.Append(c); // got \\ escape
                        escaped = !escaped;
                        break;

                    case '|':
                        if (escaped)
                        { // got \| escape
                            arg.Append(c); 
                            escaped = !escaped;
                        }
                        else
                        { // end argument
                            commands[idx] = arg.ToString();
                            arg.Length = 0;
                            idx++;
                            if (idx > elements) 
                                throw new CommandStructureException(String.Format("Command structure does not split as expected: {0}", command));
                        }
                        break;

                    default:
                        if (escaped) arg.Append('\\'); // \ not escaped (kept as is)
                        arg.Append(c); 
                        escaped = false;
                        break;
                }
            }

            commands[idx] = arg.ToString();

            if (idx+1 != elements)
                throw new CommandStructureException(String.Format("Command structure does not split as expected: {0}", command));

            return commands;
            */
        }


        /// <summary>
        /// Compose the command (should be called by every edited commands)
        /// </summary>
        /// <param name="command">list of arguments of the command</param>
        /// <returns>Command arguments as one string</returns>
        public static string JoinCommand(string[] command)
        {
            if (command == null) return "";

            return String.Join("|", command);
            
            
            /** This alternative code allows to use the | character in the commands but is disabled for backward-compatibility **
            if (command == null) return "";


            // first estimated length, defined by rule of thumbs
            StringBuilder commands = new StringBuilder(256);

            // compose the | separated command, but insert \ escape-character for protecting | character
            bool started = false;
            bool escaped = false;
            foreach (var arg in command)
            {
                if (escaped) commands.Append(@"\");  // double the the last \ on previous command to protect the | separator
                if (started) commands.Append('|');
                escaped = false;
                foreach (var c in arg)
                {
                    switch (c)
                    {
                        case '\\':
                            if (escaped) commands.Append(@"\\\\"); // if \\ --> \\\\
                            escaped = !escaped;
                            break;

                        case '|':
                            if (escaped) commands.Append(@"\\"); // if \| --> \\\|
                            commands.Append(@"\|"); 
                            escaped = false;
                            break;
                        
                        default:
                            if (escaped) commands.Append(@"\"); // if \ alone --> \
                            commands.Append(c); 
                            escaped = false;
                            break;
                    }
                }
                if (escaped) commands.Append(@"\");  // don't forget the last \
                started = true;
            }
            return commands.ToString();

            */
        }

        #endregion Command Splitters

        #region Command Execution

        /// <summary>
        /// Given a split Run command this method will launch the process according to the details of the command structure.
        /// </summary>
        /// <param name="commands">An array of arguments for the method (the output of SplitRunCommand).</param>
        public static void ProcessRunCommand(string[] commands)
        {
            if (commands == null)
                throw new ArgumentNullException("commands");

            using (Process process = new Process())
            {
                process.StartInfo.FileName = commands[0];
                process.StartInfo.WorkingDirectory = commands[1];
                process.StartInfo.Arguments = commands[2];
                process.StartInfo.WindowStyle = (ProcessWindowStyle)Enum.Parse(typeof(ProcessWindowStyle), commands[3], true);
                process.StartInfo.CreateNoWindow = bool.Parse(commands[4]);
                process.StartInfo.UseShellExecute = bool.Parse(commands[5]);
                //process.PriorityClass               = ProcessPriorityClass.

                bool waitForExit = bool.Parse(commands[6]);
                bool forceFocus = bool.Parse(commands[7]);

                process.Start();

                // Give new process focus ...
                if (forceFocus && !process.StartInfo.CreateNoWindow &&
                    process.StartInfo.WindowStyle != ProcessWindowStyle.Hidden)
                {
                    FocusForcer forcer = new FocusForcer(process.Id);
                    //forcer.Start();
                    forcer.Force();
                }

                if (waitForExit)
                    process.WaitForExit();
            }
        }

        /// <summary>
        /// Given a split Serial Command this method will send the command over the serial port according to the command structure supplied.
        /// </summary>
        /// <param name="commands">An array of arguments for the method (the output of SplitSerialCommand).</param>
        public static void ProcessSerialCommand(string[] commands)
        {
            if (commands == null)
                throw new ArgumentNullException("commands");

            Byte[] command = ReplaceSpecial(commands[0]);

            string comPort = commands[1];
            int baudRate = int.Parse(commands[2]);
            Parity parity = (Parity)Enum.Parse(typeof(Parity), commands[3], true);
            int dataBits = int.Parse(commands[4]);
            StopBits stopBits = (StopBits)Enum.Parse(typeof(StopBits), commands[5], true);
            bool waitForResponse = bool.Parse(commands[6]);

            SerialPort serialPort = new SerialPort(comPort, baudRate, parity, dataBits, stopBits);
            serialPort.Open();

            try
            {
                serialPort.Write(command, 0, command.Length);

                if (waitForResponse)
                {
                    try
                    {
                        serialPort.ReadTimeout = 5000;
                        serialPort.ReadByte();
                    }
                    catch (Exception ex)
                    {
                        IrssLog.Debug("ProcessSerialCommand: {0}", ex.Message);
                    }
                }
            }
            finally
            {
                serialPort.Close();
            }

            serialPort.Dispose();
        }

        /// <summary>
        /// Given a split Window Message Command this method will send the windows message according to the command structure supplied.
        /// </summary>
        /// <param name="commands">An array of arguments for the method (the output of SplitWindowMessageCommand).</param>
        public static void ProcessWindowMessageCommand(string[] commands)
        {
            if (commands == null)
                throw new ArgumentNullException("commands");

            IntPtr windowHandle = IntPtr.Zero;

            string matchType = commands[0].ToUpperInvariant();
            switch (matchType)
            {
                case TargetActive:
                    windowHandle = Win32.ForegroundWindow();
                    break;

                case TargetApplication:
                    foreach (Process proc in Process.GetProcesses())
                    {
                        try
                        {
                            if (commands[1].Equals(proc.MainModule.FileName, StringComparison.OrdinalIgnoreCase))
                            {
                                windowHandle = proc.MainWindowHandle;
                                break;
                            }
                        }
                        catch
                        {
                        }
                    }
                    break;

                case TargetClass:
                    windowHandle = Win32.FindWindowByClass(commands[1]);
                    break;

                case TargetWindow:
                    windowHandle = Win32.FindWindowByTitle(commands[1]);
                    break;

                default:
                    throw new CommandStructureException(String.Format("Invalid message target type: {0}", commands[0]));
            }

            if (windowHandle == IntPtr.Zero)
                throw new CommandExecutionException(String.Format("Window Message target ({0}) not found", commands[1]));

            int msg = int.Parse(commands[2]);
            IntPtr wordParam = new IntPtr(int.Parse(commands[3]));
            IntPtr longParam = new IntPtr(int.Parse(commands[4]));

            Win32.SendWindowsMessage(windowHandle, msg, wordParam, longParam);
        }

        /// <summary>
        /// Process a Key Command.
        /// </summary>
        /// <param name="command">The keystrokes to send.</param>
        public static void ProcessKeyCommand(string command)
        {
            if (String.IsNullOrEmpty(command))
                throw new ArgumentNullException("command");

            try
            {
                Keyboard.ProcessCommand(command);
            }
            catch (Exception ex)
            {
                throw new CommandExecutionException("Error executing Keystrokes Command", ex);
            }
        }

        /// <summary>
        /// Given a split TCP Message Command this method will send the TCP message according to the command structure supplied.
        /// </summary>
        /// <param name="commands">An array of arguments for the method (the output of SplitTcpMessageCommand).</param>
        public static void ProcessTcpMessageCommand(string[] commands)
        {
            if (commands == null)
                throw new ArgumentNullException("commands");

            using (TcpClient tcpClient = new TcpClient())
            {
                tcpClient.Connect(commands[0], int.Parse(commands[1]));

                using (NetworkStream networkStream = tcpClient.GetStream())
                {
                    using (StreamWriter streamWriter = new StreamWriter(networkStream))
                    {
                        Byte[] toWrite = ReplaceSpecial(commands[2]);

                        streamWriter.Write(toWrite);
                        streamWriter.Flush();

                        Thread.Sleep(1000);
                    }
                }
            }
        }

        /// <summary>
        /// Given a Mouse Command this method will move, click or scroll the mouse according to the command issued.
        /// </summary>
        /// <param name="command">The Mouse Command string.</param>
        public static void ProcessMouseCommand(string command)
        {
            if (String.IsNullOrEmpty(command))
                throw new ArgumentNullException("command");

            switch (command)
            {
                case MouseClickLeft:
                    Mouse.Button(Mouse.MouseEvents.LeftDown);
                    Mouse.Button(Mouse.MouseEvents.LeftUp);
                    break;

                case MouseClickMiddle:
                    Mouse.Button(Mouse.MouseEvents.MiddleDown);
                    Mouse.Button(Mouse.MouseEvents.MiddleUp);
                    break;

                case MouseClickRight:
                    Mouse.Button(Mouse.MouseEvents.RightDown);
                    Mouse.Button(Mouse.MouseEvents.RightUp);
                    break;

                case MouseDoubleClickLeft:
                    Mouse.Button(Mouse.MouseEvents.LeftDown);
                    Mouse.Button(Mouse.MouseEvents.LeftUp);
                    Mouse.Button(Mouse.MouseEvents.LeftDown);
                    Mouse.Button(Mouse.MouseEvents.LeftUp);
                    break;

                case MouseDoubleClickMiddle:
                    Mouse.Button(Mouse.MouseEvents.MiddleDown);
                    Mouse.Button(Mouse.MouseEvents.MiddleUp);
                    Mouse.Button(Mouse.MouseEvents.MiddleDown);
                    Mouse.Button(Mouse.MouseEvents.MiddleUp);
                    break;

                case MouseDoubleClickRight:
                    Mouse.Button(Mouse.MouseEvents.RightDown);
                    Mouse.Button(Mouse.MouseEvents.RightUp);
                    Mouse.Button(Mouse.MouseEvents.RightDown);
                    Mouse.Button(Mouse.MouseEvents.RightUp);
                    break;

                case MouseScrollDown:
                    Mouse.Scroll(Mouse.ScrollDir.Down);
                    break;

                case MouseScrollUp:
                    Mouse.Scroll(Mouse.ScrollDir.Up);
                    break;

                default:
                    if (command.StartsWith(MouseMoveDown, StringComparison.OrdinalIgnoreCase))
                        Mouse.Move(0, int.Parse(command.Substring(MouseMoveDown.Length)), false);
                    else if (command.StartsWith(MouseMoveLeft, StringComparison.OrdinalIgnoreCase))
                        Mouse.Move(-int.Parse(command.Substring(MouseMoveLeft.Length)), 0, false);
                    else if (command.StartsWith(MouseMoveRight, StringComparison.OrdinalIgnoreCase))
                        Mouse.Move(int.Parse(command.Substring(MouseMoveRight.Length)), 0, false);
                    else if (command.StartsWith(MouseMoveUp, StringComparison.OrdinalIgnoreCase))
                        Mouse.Move(0, -int.Parse(command.Substring(MouseMoveUp.Length)), false);
                    else if (command.StartsWith(MouseMoveToPos, StringComparison.OrdinalIgnoreCase))
                    {
                        string subString = command.Substring(MouseMoveToPos.Length);

                        string[] coords = subString.Split(',');

                        int x = int.Parse(coords[0]);
                        int y = int.Parse(coords[1]);

                        Mouse.Move(x, y, true);
                    }
                    else
                    {
                        throw new CommandStructureException(String.Format("Invalid Mouse Command: {0}", command));
                    }
                    break;
            }
        }

        /// <summary>
        /// Given a CD-ROM drive letter this method will eject the CD tray.
        /// </summary>
        /// <param name="command">The drive letter of the CD-ROM drive to eject the tray on.</param>
        public static void ProcessEjectCommand(string command)
        {
            if (String.IsNullOrEmpty(command))
                throw new ArgumentNullException("command");

            if (CDRom.IsCDRom(command))
                CDRom.Open(command);
            else
                throw new CommandExecutionException(String.Format("Drive ({0}) is not a recognised optical drive", command));
        }

        /// <summary>
        /// Given a path to a wave file this method will play the sound.
        /// </summary>
        /// <param name="command">The path to an audio file.</param>
        public static void ProcessSoundCommand(string command)
        {
            if (String.IsNullOrEmpty(command))
                throw new ArgumentNullException("command");

            if (!Audio.PlayFile(command, false))
                throw new CommandExecutionException(String.Format("Sound Command ({0}) failed to play", command));
        }

        /// <summary>
        /// Given a split HTTP Message Command this method will send the HTTP message according to the command structure supplied.
        /// </summary>
        /// <param name="commands">An array of arguments for the method (the output of SplitHttpMessageCommand).</param>
        /// <returns>The response to the command.</returns>
        public static string ProcessHttpCommand(string[] commands)
        {
            if (commands == null)
                throw new ArgumentNullException("commands");

      Uri uri = new Uri(ReplaceSpecialVariables(commands[0]));

            WebRequest request = WebRequest.Create(uri);
            request.Timeout = int.Parse(commands[1]);

            if (!String.IsNullOrEmpty(commands[2]))
                request.Credentials = new NetworkCredential(commands[2], commands[3]);

            using (WebResponse response = request.GetResponse())
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader streamReader = new StreamReader(responseStream))
                return streamReader.ReadToEnd();
        }

        /// <summary>
        /// Given a split Beep Command this method will sound a Beep according to the command structure supplied.
        /// </summary>
        /// <param name="commands">An array of arguments for the method (the output of SplitBeepCommand).</param>
        public static void ProcessBeepCommand(string[] commands)
        {
            if (commands == null)
                throw new ArgumentNullException("commands");

            int beepFreq = int.Parse(commands[0]);
            int beepTime = int.Parse(commands[1]);

            Audio.PlayBeep(beepFreq, beepTime);
        }

        /// <summary>
        /// Given a split Display Mode Command this method will change the display mode according to the command structure supplied.
        /// </summary>
        /// <param name="commands">An array of arguments for the method (the output of SplitDisplayModeCommand).</param>
        public static void ProcessDisplayModeCommand(string[] commands)
        {
            if (commands == null)
                throw new ArgumentNullException("commands");

            int width = int.Parse(commands[0]);
            int height = int.Parse(commands[1]);
            short bpp = short.Parse(commands[2]);
            int refresh = int.Parse(commands[3]);

            Display.ChangeDisplayMode(width, height, bpp, refresh);
        }

        /// <summary>
        /// Processes the display power command.
        /// </summary>
        /// <param name="command">The command.</param>
        public static void ProcessDisplayPowerCommand(string command)
        {
            if (String.IsNullOrEmpty(command))
                throw new ArgumentNullException("command");

            int powerState = -1;
            if (int.TryParse(command, out powerState))
                Display.SetPowerState(powerState);
            else
                throw new CommandStructureException(String.Format("Display Power Command data is not a valid integer: {0}",
                                                                  command));
        }

        /// <summary>
        /// Given a split Close Program Command this method will attempt to close a program by command structure supplied.
        /// </summary>
        /// <param name="commands">An array of arguments for the method (the output of SplitCloseProgramCommand).</param>
        public static void ProcessCloseProgramCommand(string[] commands)
        {
            if (commands == null)
                throw new ArgumentNullException("commands");

            Process process;

            string matchType = commands[0].ToUpperInvariant();
            switch (matchType)
            {
                case TargetActive:
                    process = GetProcessByWindowHandle(Win32.ForegroundWindow());
                    break;

                case TargetApplication:
                    process = GetProcessByFilePath(commands[1]);
                    break;

                case TargetClass:
                    process = GetProcessByWindowHandle(Win32.FindWindowByClass(commands[1]));
                    break;

                case TargetWindow:
                    process = GetProcessByWindowTitle(commands[1]);
                    break;

                default:
                    throw new CommandStructureException(String.Format("Invalid close program target type: {0}", commands[0]));
            }

            if (process == null)
                throw new CommandExecutionException(String.Format("Close Program target ({0}) not found", commands[1]));

            EndProcess(process, 5000);

            process.Close();
        }

        #endregion Command Execution

       
        /// <summary>
        /// Returns a list of IR Commands.
        /// </summary>
        /// <returns>string[] of IR Commands.</returns>
        public static string[] GetIRList(bool commandPrefix)
    {
      if (!Directory.Exists(FolderIRCommands))
        Directory.CreateDirectory(FolderIRCommands);

      string[] files = Directory.GetFiles(FolderIRCommands, '*' + FileExtensionIR);
            string[] list = new string[files.Length];

            int i = 0;
            foreach (string file in files)
            {
                if (commandPrefix)
                    list[i++] = CmdPrefixBlast + Path.GetFileNameWithoutExtension(file);
                else
                    list[i++] = Path.GetFileNameWithoutExtension(file);
            }

            return list;
        }

        /// <summary>
        /// Determines the validity of a given filename.
        /// </summary>
        /// <param name="fileName">File name to validate.</param>
        /// <returns>true if the name is valid; otherwise, false.</returns>
        public static bool IsValidFileName(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                return false;

            char[] invalids = Path.GetInvalidFileNameChars();

            foreach (char c in fileName)
            {
                if (c < 32) return false;
                foreach (var v in invalids)
                {
                    if (c==v) return false;
                }
            }
            return true;
        }

    /// <summary>
    /// Replace all instances of environment variables and special values.
    /// </summary>
    /// <param name="input">The input to process.</param>
    /// <returns>Processed input string.</returns>
    public static string ReplaceSpecialVariables(string input)
    {
      if (String.IsNullOrEmpty(input))
      {
        return String.Empty;
      }


            // Process Special Codes ...
            if (input.Contains("%"))
            {
                foreach (Match match in Regex.Matches(input, @"%\w+%"))
                {
                    string varName = match.Value.Substring(1, match.Value.Length - 2);

                    string envVar = String.Empty;

                    switch (varName.ToUpperInvariant())
                    {
                        case "$CLIPBOARD_TEXT$":
                            if (Clipboard.ContainsText())
                                envVar = Clipboard.GetText();
                            break;

                        case "$TIME$":
                            envVar = DateTime.Now.ToShortTimeString();
                            break;

                        case "$HOUR$":
                            envVar = DateTime.Now.Hour.ToString();
                            break;

                        case "$MINUTE$":
                            envVar = DateTime.Now.Minute.ToString();
                            break;

                        case "$SECOND$":
                            envVar = DateTime.Now.Second.ToString();
                            break;

                        case "$DATE$":
                            envVar = DateTime.Now.ToShortDateString();
                            break;

                        case "$YEAR$":
                            envVar = DateTime.Now.Year.ToString();
                            break;

                        case "$MONTH$":
                            envVar = DateTime.Now.Month.ToString();
                            break;

                        case "$DAY$":
                            envVar = DateTime.Now.Day.ToString();
                            break;

                        case "$DAYOFWEEK$":
                            envVar = DateTime.Now.DayOfWeek.ToString();
                            break;

                        case "$DAYOFYEAR$":
                            envVar = DateTime.Now.DayOfYear.ToString();
                            break;

                        case "$USERNAME$":
                            envVar = WindowsIdentity.GetCurrent().Name;
                            break;

                        case "$MACHINENAME$":
                            envVar = Environment.MachineName;
                            break;

                        default:
                            envVar = Environment.GetEnvironmentVariable(varName);
                            break;
                    }

                    input = input.Replace(match.Value, envVar);
                }
      }

      return input;
    }

    /// <summary>
    /// Replace all instances of environment variables, special values and escape codes.
    /// </summary>
    /// <param name="input">The input to process.</param>
    /// <returns>Processed input string.</returns>
    public static Byte[] ReplaceSpecial(string input)
    {
      if (String.IsNullOrEmpty(input))
      {
        return new byte[0];
      }

      // Process Special Codes ...
      input = ReplaceSpecialVariables(input);

      // Process Escape Codes ...
            bool inEscapeCode = false;
            bool inHexCode = false;
            StringBuilder hexCode = new StringBuilder();
            List<Byte> output = new List<Byte>();

            foreach (char currentChar in input)
            {
                if (inEscapeCode)
                {
                    switch (currentChar)
                    {
                        case 'a':
                            output.Add((byte)7);
                            break;
                        case 'b':
                            output.Add((byte)Keys.Back);
                            break;
                        case 'f':
                            output.Add((byte)12);
                            break;
                        case 'n':
                            output.Add((byte)Keys.LineFeed);
                            break;
                        case 'r':
                            output.Add((byte)Keys.Return);
                            break;
                        case 't':
                            output.Add((byte)Keys.Tab);
                            break;
                        case 'v':
                            output.Add((byte)11);
                            break;
                        case 'x':
                            hexCode = new StringBuilder();
                            inHexCode = true;
                            break;
                        case '0': // I've got a bad feeling about this
                            output.Add((byte)0);
                            break;

                        default: // If it doesn't know it as an escape code, just use the char
                            output.Add((byte)currentChar);
                            break;
                    }

                    inEscapeCode = false;
                }
                else if (inHexCode)
                {
                    switch (currentChar)
                    {
                        case 'h': // 'h' terminates the hex code
                            byte hexParsed;
                            if (byte.TryParse(hexCode.ToString(), NumberStyles.HexNumber, null, out hexParsed))
                            {
                                output.Add(hexParsed);
                            }
                            else
                            {
                                throw new ArgumentException(String.Format("Bad Hex Code \"{0}\"", hexCode), "input");
                            }
                            inHexCode = false;
                            break;

                        default:
                            hexCode.Append(currentChar);
                            break;
                    }
                }
                else if (currentChar == '\\')
                {
                    inEscapeCode = true;
                }
                else
                {
                    output.Add((byte)currentChar);
                }
            }

            return output.ToArray();
        }

        /// <summary>
        /// Hibernate the PC.
        /// </summary>
        /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
        public static bool Hibernate()
        {
            return Application.SetSuspendState(PowerState.Hibernate, false, false);
        }

        /// <summary>
        /// Standby the PC.
        /// </summary>
        /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
        public static bool Standby()
        {
            return Application.SetSuspendState(PowerState.Suspend, false, false);
        }

        /// <summary>
        /// Reboot the PC.
        /// </summary>
        /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
        public static bool Reboot()
        {
            return Win32.WindowsExit(Win32.ExitWindows.Reboot, 0);
        }

        /// <summary>
        /// LogOff the current user.
        /// </summary>
        /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
        public static bool LogOff()
        {
            return Win32.WindowsExit(Win32.ExitWindows.LogOff, 0);
        }

        /// <summary>
        /// Shut Down the computer.
        /// </summary>
        /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
        public static bool ShutDown()
        {
            return Win32.WindowsExit(Win32.ExitWindows.ShutDown, 0);
        }

        /*
        public static string ConstructFilePath(string baseFolder, string fileName)
        {
          return Path.Combine(baseFolder, fileName);
        }
        public static string ConstructFilePath(string baseFolder, string fileName, string extension)
        {
          if (!extension.StartsWith("."))
            extension = "." + extension;

          if (fileName.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
            fileName = fileName.Substring(0, fileName.Length - extension.Length);

          fileName += extension;

          return Path.Combine(baseFolder, fileName);
        }
        public static string ConstructFilePath(string baseFolder, string subFolder, string fileName, string extension)
        {
          string folder = Path.Combine(baseFolder, subFolder);

          return ConstructFilePath(folder, fileName, extension);
        }
        */


        private static Process GetProcessByWindowHandle(IntPtr windowHandle)
        {
            foreach (Process process in Process.GetProcesses())
            {
                IntPtr procWindowHandle;
                try
                {
                    procWindowHandle = process.MainWindowHandle;
                }
                catch
                {
                    continue;
                }

                if (procWindowHandle == windowHandle)
                    return process;
            }

            return null;
        }

        private static Process GetProcessByWindowTitle(string windowTitle)
        {
            foreach (Process process in Process.GetProcesses())
            {
                string procWindowTitle;
                try
                {
                    procWindowTitle = process.MainWindowTitle;
                }
                catch
                {
                    continue;
                }

                if (procWindowTitle.Equals(windowTitle, StringComparison.OrdinalIgnoreCase))
                    return process;
            }

            return null;
        }

        private static Process GetProcessByFilePath(string filePath)
        {
            foreach (Process process in Process.GetProcesses())
            {
                string procFilePath;
                try
                {
                    procFilePath = process.MainModule.FileName;
                }
                catch
                {
                    continue;
                }

                if (procFilePath.Equals(filePath, StringComparison.OrdinalIgnoreCase))
                    return process;
            }

            return null;
        }

        /*
            private static Process GetProcessByFileName(string fileName)
            {
              foreach (Process process in Process.GetProcesses())
              {
                string procFileName;
                try
                {
                  procFileName = process.MainModule.ModuleName;
                }
                catch
                {
                  continue;
                }

                if (procFileName.Equals(fileName, StringComparison.OrdinalIgnoreCase))
                  return process;
              }

              return null;
            }
        */

        private static void EndProcess(Process process, int timeout)
        {
            if (process.CloseMainWindow())
                process.WaitForExit(timeout);

            if (!process.HasExited)
                process.Kill();
        }

        #endregion Methods
    }
}