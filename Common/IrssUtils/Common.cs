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
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

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
  /// Provides a delegate to call to learn a new IR Command.
  /// </summary>
  /// <param name="fileName">Full file path to the IR Command file.</param>
  /// <returns>Successfully started IR learn process.</returns>
  public delegate bool LearnIrDelegate(string fileName);

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
      Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
      "\\IR Server Suite\\";

    /// <summary>
    /// IR Server Suite "Logs" folder location (includes trailing '\')
    /// </summary>
    public static readonly string FolderIrssLogs    = FolderAppData + "Logs\\";

    /// <summary>
    /// IR Server Suite "IR Commands" folder location (includes trailing '\')
    /// </summary>
    public static readonly string FolderIRCommands  = FolderAppData + "IR Commands\\";

    /// <summary>
    /// IR Server Suite "Set Top Boxes" folder location (includes trailing '\')
    /// </summary>
    public static readonly string FolderSTB         = FolderAppData + "Set Top Boxes\\";

    #endregion Folders

    #region File Extensions

    /// <summary>
    /// File extension for IR Commands.
    /// </summary>
    public const string FileExtensionIR       = ".IR";

    /// <summary>
    /// File extension for Macros.
    /// </summary>
    public const string FileExtensionMacro    = ".Macro";

    #endregion File Extensions

    #region Strings

    #region Command Prefixes

    public const string CmdPrefixSTB          = "STB: ";
    public const string CmdPrefixMacro        = "Macro: ";
    public const string CmdPrefixBlast        = "Blast: ";
    public const string CmdPrefixPause        = "Pause: ";
    public const string CmdPrefixRun          = "Run: ";
    public const string CmdPrefixSerial       = "Serial: ";
    public const string CmdPrefixKeys         = "Keys: ";
    public const string CmdPrefixWindowMsg    = "Window Message: ";
    public const string CmdPrefixTcpMsg       = "TCP Message: ";
    public const string CmdPrefixHttpMsg      = "HTTP Message: ";
    public const string CmdPrefixGoto         = "Goto: ";
    public const string CmdPrefixPopup        = "Popup: ";
    public const string CmdPrefixMouseMode    = "Mouse Mode: ";
//  public const string CmdPrefixWindowState  = "Toggle Window State";
    public const string CmdPrefixStandby      = "Standby";
    public const string CmdPrefixHibernate    = "Hibernate";
    public const string CmdPrefixReboot       = "Reboot";
    public const string CmdPrefixShutdown     = "Shutdown";
    public const string CmdPrefixLogOff       = "Log Off";

    public const string CmdPrefixMouse        = "Mouse: ";
    public const string CmdPrefixEject        = "Eject: ";
    public const string CmdPrefixSound        = "Sound: ";

    public const string CmdPrefixTranslator   = "Show Translator OSD";

    // For MediaPortal ...
    public const string CmdPrefixMultiMap     = "Multi-Mapping: ";
    public const string CmdPrefixInputLayer   = "Toggle Input Layer";
    public const string CmdPrefixFocus        = "Get Focus";
    public const string CmdPrefixExit         = "Exit MediaPortal";

    #endregion Command Prefixes

    #region XML Tags

    public const string XmlTagMacro           = "MACRO";
    public const string XmlTagBlast           = "BLAST";
    public const string XmlTagPause           = "PAUSE";
    public const string XmlTagRun             = "RUN";
    public const string XmlTagSerial          = "SERIAL";
    public const string XmlTagKeys            = "KEYS";
    public const string XmlTagWindowMsg       = "WINDOW_MESSAGE";
    public const string XmlTagTcpMsg          = "TCP_MESSAGE";
    public const string XmlTagGoto            = "GOTO";
    public const string XmlTagPopup           = "POPUP";
    public const string XmlTagMultiMap        = "MULTI_MAPPING";
    public const string XmlTagMouseMode       = "MOUSE_MODE";
    public const string XmlTagInputLayer      = "INPUT_LAYER";
//  public const string XmlTagWindowState     = "WINDOW_STATE";
    public const string XmlTagFocus           = "GET_FOCUS";
    public const string XmlTagExit            = "EXIT";
    public const string XmlTagStandby         = "STANDBY";
    public const string XmlTagHibernate       = "HIBERNATE";
    public const string XmlTagReboot          = "REBOOT";
    public const string XmlTagShutdown        = "SHUTDOWN";
    public const string XmlTagLogOff          = "LOG_OFF";

    public const string XmlTagMouse           = "MOUSE";
    public const string XmlTagEject           = "EJECT";
    public const string XmlTagSound           = "SOUND";

    public const string XmlTagTranslator      = "TRANSLATOR";

    #endregion XML Tags

    #region User Interface Text

    public const string UITextRun             = "Run Program";
    public const string UITextPause           = "Pause";
    public const string UITextSerial          = "Serial Command";
    public const string UITextKeys            = "Keystrokes Command";
    public const string UITextWindowMsg       = "Window Message";
    public const string UITextTcpMsg          = "TCP Message";
    public const string UITextGoto            = "Go To Screen";
    public const string UITextPopup           = "Popup Message";
    public const string UITextMultiMap        = "Set Multi-Mapping";
    public const string UITextMouseMode       = "Set Mouse Mode";
    public const string UITextInputLayer      = "Toggle Input Handler Layer";
//  public const string UITextWindowState     = "Change Window State";
    public const string UITextFocus           = "Get Focus";
    public const string UITextExit            = "Exit MediaPortal";
    public const string UITextStandby         = "Standby";
    public const string UITextHibernate       = "Hibernate";
    public const string UITextReboot          = "Reboot";
    public const string UITextShutdown        = "Shutdown";
    public const string UITextLogOff          = "Log Off";

    public const string UITextMouse           = "Mouse Command";
    public const string UITextEject           = "Eject CD";
    public const string UITextSound           = "Play Sound";

    public const string UITextTranslator      = "Show Translator OSD";

    #endregion User Interface Text

    #region Mouse Commands

    public const string MouseMoveUp             = "Move_Up ";
    public const string MouseMoveDown           = "Move_Down ";
    public const string MouseMoveLeft           = "Move_Left ";
    public const string MouseMoveRight          = "Move_Right ";

    public const string MouseMoveToPos          = "Move_To_Pos ";

    public const string MouseClickLeft          = "Click_Left";
    public const string MouseClickRight         = "Click_Right";
    public const string MouseClickMiddle        = "Click_Middle";

    public const string MouseDoubleClickLeft    = "DoubleClick_Left";
    public const string MouseDoubleClickRight   = "DoubleClick_Right";
    public const string MouseDoubleClickMiddle  = "DoubleClick_Middle";

    public const string MouseScrollUp           = "Scroll_Up";
    public const string MouseScrollDown         = "Scroll_Down";

    #endregion Mouse Commands

    #region Windows Message Target

    public const string WMTargetActive      = "ACTIVE";
    public const string WMTargetApplication = "APPLICATION";
    public const string WMTargetClass       = "CLASS";
    public const string WMTargetWindow      = "WINDOW";

    #endregion Windows Message Target

    #endregion Strings

    #region Command Segments

    /// <summary>
    /// Number of Segments in a Blast Command.
    /// </summary>
    public const int SegmentsBlastCommand = 2;
    /// <summary>
    /// Number of Segments in a Run Command.
    /// </summary>
    public const int SegmentsRunCommand = 8;
    /// <summary>
    /// Number of Segments in a Serial Command.
    /// </summary>
    public const int SegmentsSerialCommand = 7;
    /// <summary>
    /// Number of Segments in a Windows Message Command.
    /// </summary>
    public const int SegmentsWindowMessageCommand = 5;
    /// <summary>
    /// Number of Segments in a Popup Command.
    /// </summary>
    public const int SegmentsPopupCommand = 3;
    /// <summary>
    /// Number of Segments in a TCP Message Command.
    /// </summary>
    public const int SegmentsTcpMessageCommand = 3;
    /// <summary>
    /// Number of Segments in a HTTP Message Command.
    /// </summary>
    public const int SegmentsHttpMessageCommand = 4;

    #endregion Command Segments

    #endregion Constants

    #region Methods

    #region Command Splitters

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

    static string[] SplitCommand(string command, int elements)
    {
      if (String.IsNullOrEmpty(command))
        throw new ArgumentNullException("command");

      string[] commands = command.Split(new char[] { '|' }, StringSplitOptions.None);

      if (commands.Length != elements)
        throw new ArgumentException(String.Format("Command structure is invalid: {0}", command), "command");

      return commands;
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

      Process process = new Process();
      process.StartInfo.FileName          = commands[0];
      process.StartInfo.WorkingDirectory  = commands[1];
      process.StartInfo.Arguments         = commands[2];
      process.StartInfo.WindowStyle       = (ProcessWindowStyle)Enum.Parse(typeof(ProcessWindowStyle), commands[3], true);
      process.StartInfo.CreateNoWindow    = bool.Parse(commands[4]);
      process.StartInfo.UseShellExecute   = bool.Parse(commands[5]);

      bool waitForExit                    = bool.Parse(commands[6]);
      bool forceFocus                     = bool.Parse(commands[7]);

      process.Start();

      // Give new process focus ...
      if (!process.StartInfo.CreateNoWindow &&
        process.StartInfo.WindowStyle != ProcessWindowStyle.Hidden &&
        forceFocus)
      {
        IntPtr processWindow = IntPtr.Zero;
        while (!process.HasExited)
        {
          processWindow = process.MainWindowHandle;
          if (processWindow != IntPtr.Zero)
          {
            Win32.SetForegroundWindow(processWindow, true);
            break;
          }

          Thread.Sleep(500);
        }
      }

      if (waitForExit)
        process.WaitForExit();
    }

    /// <summary>
    /// Given a split Serial Command this method will send the command over the serial port according to the command structure supplied.
    /// </summary>
    /// <param name="commands">An array of arguments for the method (the output of SplitSerialCommand).</param>
    public static void ProcessSerialCommand(string[] commands)
    {
      if (commands == null)
        throw new ArgumentNullException("commands");

      string command        = Common.ReplaceEscapeCodes(commands[0]);
      
      string comPort        = commands[1];
      int baudRate          = int.Parse(commands[2]);
      Parity parity         = (Parity)Enum.Parse(typeof(Parity), commands[3], true);
      int dataBits          = int.Parse(commands[4]);
      StopBits stopBits     = (StopBits)Enum.Parse(typeof(StopBits), commands[5], true);
      bool waitForResponse  = bool.Parse(commands[6]);

      SerialPort serialPort = new SerialPort(comPort, baudRate, parity, dataBits, stopBits);
      serialPort.Open();

      try
      {
        serialPort.Write(command);

        if (waitForResponse)
        {
          serialPort.ReadTimeout = 5000;
          serialPort.ReadByte();
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
        case WMTargetActive:
          windowHandle = Win32.ForegroundWindow();
          break;

        case WMTargetApplication:
          foreach (System.Diagnostics.Process proc in System.Diagnostics.Process.GetProcesses())
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

        case WMTargetClass:
          windowHandle = Win32.FindWindowByClass(commands[1]);
          break;

        case WMTargetWindow:
          windowHandle = Win32.FindWindowByTitle(commands[1]);
          break;

        default:
          throw new ArgumentOutOfRangeException("commands", commands[0], "Invalid message target");
      }

      if (windowHandle == IntPtr.Zero)
        throw new ApplicationException(String.Format("Window Message target ({0}) not found", commands[0]));

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

      Keyboard.ProcessCommand(command);
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
            streamWriter.Write(ReplaceEscapeCodes(commands[2]));
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
          if (command.StartsWith(MouseMoveDown))
            Mouse.Move(0, int.Parse(command.Substring(MouseMoveDown.Length)), false);
          else if (command.StartsWith(MouseMoveLeft))
            Mouse.Move(-int.Parse(command.Substring(MouseMoveLeft.Length)), 0, false);
          else if (command.StartsWith(MouseMoveRight))
            Mouse.Move(int.Parse(command.Substring(MouseMoveRight.Length)), 0, false);
          else if (command.StartsWith(MouseMoveUp))
            Mouse.Move(0, -int.Parse(command.Substring(MouseMoveUp.Length)), false);
          else if (command.StartsWith(MouseMoveToPos))
          {
            string subString = command.Substring(MouseMoveToPos.Length);

            string[] coords = subString.Split(new char[] { ',' });

            int x = int.Parse(coords[0]);
            int y = int.Parse(coords[1]);

            Mouse.Move(x, y, true);
          }
          else
            throw new ApplicationException("Invalid Mouse Command");
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
        throw new ApplicationException(String.Format("Drive ({0}) is not an optical drive", command));
    }
    
    /// <summary>
    /// Given a path to a wave file this method will play the sound.
    /// </summary>
    /// <param name="command">The path to an audio file.</param>
    public static void ProcessSoundCommand(string command)
    {
      if (String.IsNullOrEmpty(command))
        throw new ArgumentNullException("command");

      if (!Audio.Play(command, false))
        throw new ApplicationException(String.Format("Sound Command ({0}) failed to play", command));
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

      Uri uri = new Uri(ReplaceEscapeCodes(commands[0]));

      WebRequest request  = WebRequest.Create(uri);
      request.Timeout     = int.Parse(commands[1]);
      request.Credentials = new NetworkCredential(commands[2], commands[3]);

      using (WebResponse response = request.GetResponse())
        using (Stream responseStream = response.GetResponseStream())
          using (StreamReader streamReader = new StreamReader(responseStream))
            return streamReader.ReadToEnd();
    }

    #endregion Command Execution

    #region Misc

    /// <summary>
    /// Returns a list of IR Commands.
    /// </summary>
    /// <returns>string[] of IR Commands.</returns>
    public static string[] GetIRList(bool commandPrefix)
    {
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
    /// Replaces the escape codes in a supplied string with their character equivalents.
    /// </summary>
    /// <param name="input">String to process for escape codes.</param>
    /// <returns>Modified string with escape codes processed.</returns>
    public static string ReplaceEscapeCodes(string input)
    {
      if (String.IsNullOrEmpty(input))
        throw new ArgumentNullException("input");

      bool inEscapeCode = false;
      bool inHexCode = false;
      byte hexParsed;
      StringBuilder hexCode = new StringBuilder();
      StringBuilder output = new StringBuilder();

      foreach (char currentChar in input)
      {
        if (inEscapeCode)
        {
          switch (currentChar)
          {
            case 'a':
              output.Append((char)7);
              break;
            case 'b':
              output.Append((char)Keys.Back);
              break;
            case 'f':
              output.Append((char)12);
              break;
            case 'n':
              output.Append((char)Keys.LineFeed);
              break;
            case 'r':
              output.Append((char)Keys.Return);
              break;
            case 't':
              output.Append((char)Keys.Tab);
              break;
            case 'v':
              output.Append((char)11);
              break;
            case 'x':
              hexCode = new StringBuilder();
              inHexCode = true;
              inEscapeCode = false;
              break;
            case '0':   // I've got a bad feeling about this
              output.Append((char)0);
              break;

            default:    // If it doesn't know it as an escape code, just use the char
              output.Append(currentChar);
              break;
          }

          inEscapeCode = false;
        }
        else if (inHexCode)
        {
          switch (currentChar)
          {
            case 'h':   // 'h' terminates the hex code
              if (byte.TryParse(hexCode.ToString(), System.Globalization.NumberStyles.HexNumber, null, out hexParsed))
                output.Append((char)hexParsed);
              else
                throw new ArgumentException(String.Format("Bad Hex Code \"{0}\"", hexCode.ToString()), "input");

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
          output.Append(currentChar);
        }
      }

      return output.ToString();
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

      Regex validate = new Regex("^(?!^(PRN|AUX|CLOCK\\$|NUL|CON|COM\\d|LPT\\d|\\..*)(\\..+)?$)[^\\x00-\\x1f\\\\?*:\\\";|/]+$", RegexOptions.IgnoreCase);
      return validate.IsMatch(fileName);
    }

    /// <summary>
    /// Replace all instances of environment variables with their value.
    /// </summary>
    /// <param name="input">The input to process.</param>
    /// <returns>Modified input string.</returns>
    public static string ReplaceEnvironmentVariables(string input)
    {
      string output = input;

      if (input.Contains("%"))
      {
        foreach (Match match in Regex.Matches(input, @"%\w+%"))
        {
          string envVar = Environment.GetEnvironmentVariable(match.Value.Substring(1, match.Value.Length - 2));

          if (envVar != null)
            output = output.Replace(match.Value, envVar);
        }
      }

      return output;
    }

    #endregion Misc

    #endregion Methods

  }

}
