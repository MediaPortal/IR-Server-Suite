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
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using InputService.Plugin.Properties;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace InputService.Plugin
{
  /// <summary>
  /// IR Server plugin to support iMon USB devices.
  /// </summary>
  public class iMonUSBReceivers : PluginBase, IConfigure, IRemoteReceiver, IKeyboardReceiver, IMouseReceiver,
                                  IDisposable
  {
    #region Code to impliment the test application version of the plugin

    // #define TEST_APPLICATION in the project properties when creating the console test app ...
#if TEST_APPLICATION

    #region Test Application - Enumeration for KeyCode Translation
    /// <summary>
    /// Remote Key Mapping (for displaying the remote button names).
    /// </summary>
    internal enum iMonRemoteKeyMapping
    {
      // iMon PAD mappings
      IMON_PAD_BUTTON_APPEXIT = 1002,
      IMON_PAD_BUTTON_POWER = 1016,
      IMON_PAD_BUTTON_RECORD = 1064,
      IMON_PAD_BUTTON_PLAY = 1128,
      IMON_PAD_BUTTON_EJECT = 1114,
      IMON_PAD_BUTTON_REWIND = 1130,
      IMON_PAD_BUTTON_PAUSE = 1144,
      IMON_PAD_BUTTON_FORWARD = 1192,
      IMON_PAD_BUTTON_REPLAY = 1208,
      IMON_PAD_BUTTON_STOP = 1220,
      IMON_PAD_BUTTON_SKIP = 1066,

      IMON_PAD_BUTTON_BACKSPACE = 1032,
      IMON_PAD_BUTTON_MOUSE_KEYBD = 1080,
      IMON_PAD_BUTTON_SELECT_SPACE = 1148,
      IMON_PAD_BUTTON_WINKEY = 1194,
      IMON_PAD_BUTTON_MENUKEY = 1060,

      IMON_PAD_BUTTON_LEFTCLICK = 1226,
      IMON_PAD_BUTTON_RIGHTCLICK = 1228,

      IMON_PAD_BUTTON_ENTER = 1034,
      IMON_PAD_BUTTON_ESC = 1252,
      IMON_PAD_BUTTON_EJECT2 = 1086,
      IMON_PAD_BUTTON_APPLAUNCH = 1124,
      IMON_PAD_BUTTON_GREENBUTTON = 1178,
      IMON_PAD_BUTTON_TASKSWITCH = 1150,

      IMON_PAD_BUTTON_MUTE = 1218,
      IMON_PAD_BUTTON_VOLUME_UP = 1038,
      IMON_PAD_BUTTON_CHANNEL_UP = 1022,
      IMON_PAD_BUTTON_TIMER = 1198,
      IMON_PAD_BUTTON_VOLUME_DOWN = 1042,
      IMON_PAD_BUTTON_CHANNEL_DOWN = 1014,

      IMON_PAD_BUTTON_NUMPAD_1 = 1058,
      IMON_PAD_BUTTON_NUMPAD_2 = 1242,
      IMON_PAD_BUTTON_NUMPAD_3 = 1050,
      IMON_PAD_BUTTON_NUMPAD_4 = 1138,
      IMON_PAD_BUTTON_NUMPAD_5 = 1090,
      IMON_PAD_BUTTON_NUMPAD_6 = 1170,
      IMON_PAD_BUTTON_NUMPAD_7 = 1214,
      IMON_PAD_BUTTON_NUMPAD_8 = 1136,
      IMON_PAD_BUTTON_NUMPAD_9 = 1160,
      IMON_PAD_BUTTON_NUMPAD_STAR = 1056,
      IMON_PAD_BUTTON_NUMPAD_0 = 1234,
      IMON_PAD_BUTTON_NUMPAD_HASH = 1096,

      IMON_PAD_BUTTON_MY_MOVIE = 1200,
      IMON_PAD_BUTTON_MY_MUSIC = 1082,
      IMON_PAD_BUTTON_MY_PHOTO = 1224,
      IMON_PAD_BUTTON_MY_TV = 1040,
      IMON_PAD_BUTTON_BOOKMARK = 1008,
      IMON_PAD_BUTTON_THUMBNAIL = 1188,
      IMON_PAD_BUTTON_ASPECT_RATIO = 1106,
      IMON_PAD_BUTTON_FULLSCREEN = 1166,
      IMON_PAD_BUTTON_MY_DVD = 1102,
      IMON_PAD_BUTTON_MENU = 1230,
      IMON_PAD_BUTTON_CAPTION = 1074,
      IMON_PAD_BUTTON_LANGUAGE = 1202,


      IMON_PAD_BUTTON_RIGHT = 1244,
      IMON_PAD_BUTTON_LEFT = 1246,
      IMON_PAD_BUTTON_DOWN = 1248,
      IMON_PAD_BUTTON_UP = 1250,

      IMON_MCE_BUTTON_POWER_TV = 2101,
      IMON_MCE_BUTTON_RECORD = 2023,
      IMON_MCE_BUTTON_STOP = 2025,
      IMON_MCE_BUTTON_PAUSE = 2024,
      IMON_MCE_BUTTON_REWIND = 2021,
      IMON_MCE_BUTTON_PLAY = 2022,
      IMON_MCE_BUTTON_FORWARD = 2020,
      IMON_MCE_BUTTON_REPLAY = 2027,
      IMON_MCE_BUTTON_SKIP = 2026,
      IMON_MCE_BUTTON_BACK = 2035,
      IMON_MCE_BUTTON_UP = 2030,
      IMON_MCE_BUTTON_DOWN = 2031,
      IMON_MCE_BUTTON_LEFT = 2032,
      IMON_MCE_BUTTON_RIGHT = 2033,
      IMON_MCE_BUTTON_OK = 2034,
      IMON_MCE_BUTTON_INFO = 2015,
      IMON_MCE_BUTTON_VOLUME_UP = 2016,
      IMON_MCE_BUTTON_VOLUME_DOWN = 2017,
      IMON_MCE_BUTTON_START = 2013,
      IMON_MCE_BUTTON_CHANNEL_UP = 2018,
      IMON_MCE_BUTTON_CHANNEL_DOWN = 2019,
      IMON_MCE_BUTTON_MUTE = 2014,
      IMON_MCE_BUTTON_RECORDED_TV = 2072,
      IMON_MCE_BUTTON_GUIDE = 2038,
      IMON_MCE_BUTTON_LIVE_TV = 2037,
      IMON_MCE_BUTTON_DVD_MENU = 2036,
      IMON_MCE_BUTTON_NUMPAD_1 = 2001,
      IMON_MCE_BUTTON_NUMPAD_2 = 2002,
      IMON_MCE_BUTTON_NUMPAD_3 = 2003,
      IMON_MCE_BUTTON_NUMPAD_4 = 2004,
      IMON_MCE_BUTTON_NUMPAD_5 = 2005,
      IMON_MCE_BUTTON_NUMPAD_6 = 2006,
      IMON_MCE_BUTTON_NUMPAD_7 = 2007,
      IMON_MCE_BUTTON_NUMPAD_8 = 2008,
      IMON_MCE_BUTTON_NUMPAD_9 = 2009,
      IMON_MCE_BUTTON_NUMPAD_0 = 2000,
      IMON_MCE_BUTTON_NUMPAD_STAR = 2029,
      IMON_MCE_BUTTON_NUMPAD_HASH = 2028,
      IMON_MCE_BUTTON_CLEAR = 2010,
      IMON_MCE_BUTTON_ENTER = 2011,
      IMON_MCE_BUTTON_TELETEXT = 2090,
      IMON_MCE_BUTTON_RED = 2091,
      IMON_MCE_BUTTON_GREEN = 2092,
      IMON_MCE_BUTTON_YELLOW = 2093,
      IMON_MCE_BUTTON_BLUE = 2094,
      IMON_MCE_BUTTON_MY_TV = 2070,
      IMON_MCE_BUTTON_MY_MUSIC = 2071,
      IMON_MCE_BUTTON_MY_PICTURES = 2073,
      IMON_MCE_BUTTON_MY_VIDEOS = 2074,
      IMON_MCE_BUTTON_MY_RADIO = 2080,
      IMON_MCE_BUTTON_MESSENGER = 2105,
      IMON_MCE_BUTTON_ASPECT_RATIO = 2012,
      IMON_MCE_BUTTON_PRINT = 2078,

      IMON_PANEL_BUTTON = 3000,

      IMON_PANEL_BUTTON_VOLUME_KNOB = 3001,
      IMON_PANEL_BUTTON_MCE = 3015,
      IMON_PANEL_BUTTON_APPEXIT = 3043,
      IMON_PANEL_BUTTON_BACK = 3023,
      IMON_PANEL_BUTTON_UP = 3018,
      IMON_PANEL_BUTTON_ENTER = 3022,
      IMON_PANEL_BUTTON_START = 3044,
      IMON_PANEL_BUTTON_MENU = 3045,
      IMON_PANEL_BUTTON_LEFT = 3020,
      IMON_PANEL_BUTTON_DOWN = 3019,
      IMON_PANEL_BUTTON_RIGHT = 3021,

      IMON_VOLUME_UP = 4001,
      IMON_VOLUME_DOWN = 4002,
    }
    #endregion

    static void xRemote_HID(string deviceName, string code)
    {
      Console.WriteLine("iMon HID Remote: {0}     (button = {1})\n", code, Enum.GetName(typeof(iMonRemoteKeyMapping), Enum.Parse(typeof(iMonRemoteKeyMapping), code)));
#if DEBUG
      DebugWriteLine("iMon HID Remote: {0}     (button = {1})\n", code, Enum.GetName(typeof(iMonRemoteKeyMapping), Enum.Parse(typeof(iMonRemoteKeyMapping), code)));
#endif
    }

    static void xRemote_DOS(string deviceName, string code)
    {
      Console.WriteLine("iMon DOS Remote: {0}     (button = {1})\n", code, Enum.GetName(typeof(iMonRemoteKeyMapping), Enum.Parse(typeof(iMonRemoteKeyMapping), code)));
#if DEBUG
      DebugWriteLine("iMon DOS Remote: {0}     (button = {1})\n", code, Enum.GetName(typeof(iMonRemoteKeyMapping), Enum.Parse(typeof(iMonRemoteKeyMapping), code)));
#endif
    }

    static void xKeyboard_HID(string deviceName, int button, bool up)
    {
      Console.WriteLine("iMon HID Keyboard: {0}, {1}     (key = {2})\n", button, (up ? "Released" : "Pressed"), Enum.GetName(typeof(iMonRemoteKeyMapping), Enum.Parse(typeof(iMonRemoteKeyMapping), button.ToString())));
#if DEBUG
      DebugWriteLine("iMon HID Keyboard: {0}, {1}     (key = {2})\n", button, (up ? "Released" : "Pressed"), Enum.GetName(typeof(iMonRemoteKeyMapping), Enum.Parse(typeof(iMonRemoteKeyMapping), button.ToString())));
#endif
    }

    static void xKeyboard_DOS(string deviceName, int button, bool up)
    {
      Console.WriteLine("iMon DOS Keyboard: {0}, {1}     (key = {2})\n", button, (up ? "Released" : "Pressed"), Enum.GetName(typeof(iMonRemoteKeyMapping), Enum.Parse(typeof(iMonRemoteKeyMapping), button.ToString())));
#if DEBUG
      DebugWriteLine("iMon DOS Keyboard: {0}, {1}     (key = {2})\n", button, (up ? "Released" : "Pressed"), Enum.GetName(typeof(iMonRemoteKeyMapping), Enum.Parse(typeof(iMonRemoteKeyMapping), button.ToString())));
#endif
    }

    static void xMouse_HID(string deviceName, int x, int y, int buttons)
    {
      Console.WriteLine("iMon HID Mouse: ({0}, {1}) - {2}\n", x, y, buttons);
#if DEBUG
      DebugWriteLine("iMon HID Mouse: ({0}, {1}) - {2}\n", x, y, buttons);
#endif
    }

    static void xMouse_DOS(string deviceName, int x, int y, int buttons)
    {
      Console.WriteLine("iMon DOS Mouse: ({0}, {1}) - {2}\n", x, y, buttons);
#if DEBUG
      DebugWriteLine("iMon DOS Mouse: ({0}, {1}) - {2}\n", x, y, buttons);
#endif
    }

    [STAThread]
    static void Main()
    {
#if DEBUG
      DebugOpen("iMonTestApp.log");
      DebugWriteLine("Main()");
#endif

      DeviceType DevType;

      iMonUSBReceivers device = new iMonUSBReceiver();

      try
      {
        device.Configure(null);

        DevType = device.DeviceDriverMode;

#if DEBUG
        DebugWriteLine("Main(): Detected device type = {0}", Enum.GetName(typeof(DeviceType), DevType));
#endif

        if (DevType == DeviceType.DOS)
        {
          Console.WriteLine("Found an iMon DOS Device\n");
#if DEBUG
          DebugWriteLine("Found an iMon DOS Device\n");
#endif
          device.RemoteCallback += new RemoteHandler(xRemote_DOS);
          device.KeyboardCallback += new KeyboardHandler(xKeyboard_DOS);
          device.MouseCallback += new MouseHandler(xMouse_DOS);
        }
        else if (DevType == DeviceType.HID)
        {
          Console.WriteLine("Found an iMon HID Device\n");
#if DEBUG
          DebugWriteLine("Found an iMon HID Device\n");
#endif
          device.RemoteCallback += new RemoteHandler(xRemote_HID);
          device.KeyboardCallback += new KeyboardHandler(xKeyboard_HID);
          device.MouseCallback += new MouseHandler(xMouse_HID);
        }

        if ((DevType == DeviceType.DOS) | (DevType == DeviceType.HID))
        {
          device.Start();

          Application.Run();

          device.Stop();
        }
        else
        {
#if DEBUG
          DebugWriteLine("Main(): NO SUPPORTED iMon DEVICE FOUND\n");
#endif
          throw new Exception("NO SUPPORTED iMon DEVICE FOUND");
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
      finally
      {
        device = null;
      }
#if DEBUG
      DebugWriteLine("Main(): completed");
#endif

      Console.ReadKey();
    }

#endif

    #endregion

    #region iMon DosDevice Constants

    private const int DosDeviceBufferSize = 8;
    private const string DosDevicePath = @"\\.\SGIMON";

    private const int ErrorIoPending = 997;
    private const uint IOCTL_IMON_FW_VER = 0x00222014; // function 0x805 - read FW version (4 bytes)
    private const uint IOCTL_IMON_RC_SET = 0x00222010; // function 0x804 - write RCset data (2 bytes) to device

    // IOCTL definitions 0x0022xxxx
    private const uint IOCTL_IMON_READ = 0x00222008; // function 0x802 - read data (64 bytes?) from device
    private const uint IOCTL_IMON_READ_RC = 0x00222030; // function 0x80C - read data (8 bytes) from device
    private const uint IOCTL_IMON_READ2 = 0x00222034; // function 0x80D - ??? read (8 bytes)
    private const uint IOCTL_IMON_WRITE = 0x00222018; // function 0x806 - write data (8 bytes) to device

    private static readonly byte[][] SetModeiMon = new byte[][]
                                                     {
                                                       new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x00},
                                                       new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x02},
                                                       new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x04},
                                                       new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x06},
                                                       new byte[] {0x20, 0x20, 0x20, 0x20, 0x00, 0x00, 0x00, 0x08},
                                                       new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x0A}
                                                     };

    private static readonly byte[][] SetModeMCE = new byte[][]
                                                    {
                                                      new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x00},
                                                      new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x02},
                                                      new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x04},
                                                      new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x06},
                                                      new byte[] {0x20, 0x20, 0x20, 0x20, 0x01, 0x00, 0x00, 0x08},
                                                      new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x0A}
                                                    };

    #endregion iMon DosDevice Constants

    #region Global Constants

    private const int DeviceBufferSize = 255;
    private const uint IMON_MCE_BUTTON = 2000;
    private const uint IMON_MCE_BUTTON_DOWN = 0x1F;
    private const uint IMON_MCE_BUTTON_LEFT = 0x20;
    private const uint IMON_MCE_BUTTON_RIGHT = 0x21;
    private const uint IMON_MCE_BUTTON_UP = 0x1E;
    private const uint IMON_PAD_BUTTON = 1000;
    private const uint IMON_PAD_BUTTON_DOWN = 0xF8;
    private const uint IMON_PAD_BUTTON_LCLICK = 0xE2;
    private const uint IMON_PAD_BUTTON_LEFT = 0xF6;
    private const uint IMON_PAD_BUTTON_MENUKEY = 0x3C;
    private const uint IMON_PAD_BUTTON_RCLICK = 0xE4;
    private const uint IMON_PAD_BUTTON_RIGHT = 0xF4;
    private const uint IMON_PAD_BUTTON_UP = 0xFA;
    private const uint IMON_PAD_BUTTON_WINKEY = 0xC2;

    private const uint IMON_PANEL_BUTTON = 3000;
    private const uint IMON_VOLUME_DOWN = 4002;
    private const uint IMON_VOLUME_UP = 4001;

#if TEST_APPLICATION
    static readonly string ConfigurationFile = @"./iMon USB Receivers.xml";
#else
    private static readonly string ConfigurationFile = Path.Combine(ConfigurationPath, "iMon USB Receivers.xml");
#endif

    #endregion Constants

    #region iMon DosDevice Enumerators

    #region Nested type: CreateFileAccessTypes

    [Flags]
    private enum CreateFileAccessTypes : uint
    {
      GenericRead = 0x80000000,
      GenericWrite = 0x40000000,
      GenericExecute = 0x20000000,
      GenericAll = 0x10000000,
    }

    #endregion

    #region Nested type: CreateFileAttributes

    [Flags]
    private enum CreateFileAttributes : uint
    {
      None = 0x00000000,
      Readonly = 0x00000001,
      Hidden = 0x00000002,
      System = 0x00000004,
      Directory = 0x00000010,
      Archive = 0x00000020,
      Device = 0x00000040,
      Normal = 0x00000080,
      Temporary = 0x00000100,
      SparseFile = 0x00000200,
      ReparsePoint = 0x00000400,
      Compressed = 0x00000800,
      Offline = 0x00001000,
      NotContentIndexed = 0x00002000,
      Encrypted = 0x00004000,
      Write_Through = 0x80000000,
      Overlapped = 0x40000000,
      NoBuffering = 0x20000000,
      RandomAccess = 0x10000000,
      SequentialScan = 0x08000000,
      DeleteOnClose = 0x04000000,
      BackupSemantics = 0x02000000,
      PosixSemantics = 0x01000000,
      OpenReparsePoint = 0x00200000,
      OpenNoRecall = 0x00100000,
      FirstPipeInstance = 0x00080000,
    }

    #endregion

    #region Nested type: CreateFileDisposition

    private enum CreateFileDisposition : uint
    {
      None = 0,
      New = 1,
      CreateAlways = 2,
      OpenExisting = 3,
      OpenAlways = 4,
      TruncateExisting = 5,
    }

    #endregion

    #region Nested type: CreateFileShares

    [Flags]
    private enum CreateFileShares : uint
    {
      None = 0x00,
      Read = 0x01,
      Write = 0x02,
      Delete = 0x04,
    }

    #endregion

    #endregion DosDevice Enumerations

    #region Global Enumerations

    private DeviceType _DriverMode = DeviceType.NotValid;
    private KeyModifierState ModifierState;

    #region Nested type: DeviceType

    private enum DeviceType
    {
      NotValid = -1,
      None = 0,
      DOS = 1,
      HID = 2,
    }

    #endregion

    #region Nested type: KeyModifiers

    [Flags]
    private enum KeyModifiers
    {
      None = 0x00,
      LeftControl = 0x01,
      LeftShift = 0x02,
      LeftAlt = 0x04,
      LeftWin = 0x08,
      RightControl = 0x10,
      RightShift = 0x20,
      RightAlt = 0x40,
      RightWin = 0x80,
    }

    #endregion

    #region Nested type: KeyModifierState

    private struct KeyModifierState
    {
      public bool AltOn;
      public bool CtrlOn;
      public bool LastKeydownWasAlt;
      public bool LastKeydownWasCtrl;
      public bool LastKeydownWasShift;
      public bool LastKeyupWasAlt;
      public bool LastKeyupWasCtrl;
      public bool LastKeyupWasShift;
      public bool ShiftOn;
    }

    #endregion

    #region Nested type: RcMode

    /// <summary>
    /// Hardware mode (either MCE or iMon).
    /// </summary>
    internal enum RcMode
    {
      /// <summary>
      /// Microsoft MCE Mode.
      /// </summary>
      Mce,
      /// <summary>
      /// Soundgraph iMon Mode.
      /// </summary>
      iMon,
    }

    #endregion

    #region Nested type: RemoteMode

    /// <summary>
    /// Remote mode (either MCE or iMon).
    /// </summary>
    internal enum RemoteMode
    {
      /// <summary>
      /// Mousestick emulates Mouse Mode.
      /// </summary>
      Mouse,
      /// <summary>
      /// Mousestick emulates direction keys. (Default Mode)
      /// </summary>
      Keyboard,
      /// <summary>
      /// Mousestick switches mode when the mouse/keyboard button is pressed (iMon Mode ONLY).
      /// </summary>
      SelectWithButton,
    }

    #endregion

    #endregion

    #region DosDevice Interop

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool DeviceIoControl(
      SafeFileHandle handle,
      uint ioControlCode,
      IntPtr inBuffer, int inBufferSize,
      IntPtr outBuffer, int outBufferSize,
      out int bytesReturned,
      IntPtr overlapped);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetOverlappedResult(
      SafeFileHandle handle,
      IntPtr overlapped,
      out int bytesTransferred,
      [MarshalAs(UnmanagedType.Bool)] bool wait);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern SafeFileHandle CreateFile(
      [MarshalAs(UnmanagedType.LPTStr)] string fileName,
      [MarshalAs(UnmanagedType.U4)] CreateFileAccessTypes fileAccess,
      [MarshalAs(UnmanagedType.U4)] CreateFileShares fileShare,
      IntPtr securityAttributes,
      [MarshalAs(UnmanagedType.U4)] CreateFileDisposition creationDisposition,
      [MarshalAs(UnmanagedType.U4)] CreateFileAttributes flags,
      IntPtr templateFile);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CancelIo(
      SafeFileHandle handle);

    #endregion DosDevice Interop

    #region HID Device Support variables

    private const string HIDKeyboardSuffix = "MI_00&Col02#";
    private const string HIDMouseSuffix = "MI_00&Col01#";
    private const string HIDRemoteSuffix = "MI_01#";

    private static readonly byte[][] SetHIDModeKeyboard_X36 = new byte[][]
                                                                {
                                                                  new byte[]
                                                                    {0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80}
                                                                };

    private static readonly byte[][] SetHIDModeKeyboard_X38 = new byte[][]
                                                                {
                                                                  new byte[]
                                                                    {0x00, 0x00, 0x00, 0x00, 0x70, 0x5B, 0x27, 0x03}
                                                                };

    private static readonly byte[][] SetHIDModeMouse_X36 = new byte[][]
                                                             {
                                                               new byte[]
                                                                 {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80}
                                                             };

    private static readonly byte[][] SetHIDModeMouse_X38 = new byte[][]
                                                             {
                                                               new byte[]
                                                                 {0x00, 0x00, 0x00, 0x00, 0x00, 0x6C, 0x27, 0x80}
                                                             };

    private static readonly string[] SupportedDevices_HID = new string[]
                                                              {
#if DEBUG_FAKE_HID
          "Vid_046d&Pid_c221",
#endif
                                                                "Vid_15c2&Pid_003c",
                                                                "Vid_15c2&Pid_0038",
                                                                "Vid_15c2&Pid_0036"
                                                              };

    private RawInput.RAWINPUTDEVICE[] _deviceTree;

    private ReceiverWindow _receiverWindowHID;

    private int KeyboardDevice = -1;
    private string KeyboardDeviceName = string.Empty;
    private int MouseDevice = -1;
    private string MouseDeviceName = string.Empty;
    private int RemoteDevice = -1;
    private string RemoteDeviceName = string.Empty;

    #endregion HID Device Support variables

    #region Global Variables

    #region Configuration

    private RemoteMode _CurrentRemoteMode = RemoteMode.Keyboard;

    private bool _enableKeyboardInput;

    private bool _enableMouseInput;
    private bool _enableRemoteInput = true;
    private bool _handleKeyboardLocally = true;
    private bool _handleMouseLocally = true;
    private RcMode _hardwareMode = RcMode.Mce;
    private int _keyboardFirstRepeat = 350;
    private int _keyboardHeldRepeats;
    private double _mouseSensitivity = 1.0d;
    private int _remoteFirstRepeat = 400;
    private int _remoteHeldRepeats = 250;
    private RemoteMode _RemoteMode = RemoteMode.Keyboard;
    private bool _useSystemRatesKeyboard = true;
    private bool _useSystemRatesRemote;

    #endregion Configuration

    private bool _disposed;
    private KeyboardHandler _keyboardHandler;
    private bool _keyboardKeyRepeated;
    private DateTime _lastCodeTime = DateTime.Now;
    private uint _lastKeyboardKeyCode;
    private DateTime _lastKeyboardKeyTime = DateTime.Now;
    private uint _lastKeyboardModifiers;
    private string _lastKeyCode = String.Empty;

    private uint _lastRemoteButtonKeyCode;
    private DateTime _lastRemoteButtonTime = DateTime.Now;
    private Mouse.MouseEvents _mouseButtons = Mouse.MouseEvents.None;
    private MouseHandler _mouseHandler;
    private bool _remoteButtonRepeated;
    private RemoteHandler _remoteHandler;

    private byte _remoteToggle;

    #endregion Global Variables

    #region DosDevice Variables

    private SafeFileHandle _deviceHandle;

    private bool _processReceiveThread;
    private Thread _receiveThread;

    #endregion

    #region DosDevice Specific Helper Functions

    private void ProcessInput(byte[] dataBytes)
    {
#if DEBUG
      DebugWrite("Data Received: ");
      DebugDump(dataBytes);
#endif

      if ((dataBytes[0] & 0xFC) == 0x28)
      {
#if DEBUG
        DebugWriteLine("iMon PAD remote button");
#endif
        uint keyCode = IMON_PAD_BUTTON;
        keyCode += (uint) ((dataBytes[0] & 0x03) << 6);
        keyCode += (uint) (dataBytes[1] & 0x30);
        keyCode += (uint) ((dataBytes[1] & 0x06) << 1);
        keyCode += (uint) ((dataBytes[2] & 0xC0) >> 6);

        if ((_RemoteMode == RemoteMode.SelectWithButton) & (_hardwareMode == RcMode.iMon))
        {
          if ((keyCode == (IMON_PAD_BUTTON + 0x50)) | (keyCode == (IMON_PAD_BUTTON + 0x51)))
          {
            // the keyboard/mouse button was pressed or released
            if (keyCode == (IMON_PAD_BUTTON + 0x51))
            {
              // the mouse/keyboard button was released
              _CurrentRemoteMode = ((_CurrentRemoteMode == RemoteMode.Mouse) ? RemoteMode.Keyboard : RemoteMode.Mouse);
              //only change modes on the release of the button
#if DEBUG
#if TEST_APPLICATION
              Console.WriteLine("RAW IMON HID REMOTE - MOUSE/ KEYBOARD MODE CHANGED - NEW MODE = {0}\n", Enum.GetName(typeof(RemoteMode), _CurrentRemoteMode));
#endif
              DebugWriteLine("RAW IMON HID REMOTE - MOUSE/ KEYBOARD MODE CHANGED - NEW MODE = {0}\n",
                             Enum.GetName(typeof (RemoteMode), _CurrentRemoteMode));
#endif
            }
            return;
          }
        }
        if ((keyCode & 0x01) == 0 && (dataBytes[2] & 0x40) == 0)
        {
          RemoteEvent(keyCode, _remoteToggle != 1);
          _remoteToggle = 1;
        }
        else
        {
          _remoteToggle = 0;
        }
      }
      else if ((dataBytes[0] & 0xFC) == 0x68)
      {
#if DEBUG
        DebugWriteLine("ProcessInput(): iMon PAD mouse report");
#endif
        ProcessiMonMouseReport(dataBytes[0], dataBytes[1], dataBytes[2], dataBytes[3]);
        /*
        int xSign = (((dataBytes[0] & 0x02) != 0) ? 1 : -1);
        int ySign = (((dataBytes[0] & 0x01) != 0) ? 1 : -1);
        int xSize = ((dataBytes[1] & 0x78) >> 3);
        int ySize = ((dataBytes[2] & 0x78) >> 3);

        bool right = ((dataBytes[1] & 0x04) != 0);
        bool left = ((dataBytes[1] & 0x01) != 0);

        if (_CurrentRemoteMode == RemoteMode.Keyboard)
        {
            // convert the mouse movement into direction keys
            uint KeyMode = ((_hardwareMode == RcMode.iMon) ? IMON_PAD_BUTTON : IMON_MCE_BUTTON);
            uint KeyCode = 0;
            uint KeyCode1 = 0;
            if ((xSize != 0) && (ySize != 0))
            {
                KeyCode = (uint)TranslateMouseToKeypress((xSign * xSize), (ySign * ySize));
                if (KeyCode != 0) RemoteEvent((KeyCode + KeyMode), false);
            }
            KeyCode1 = 0;
            switch (ulButtons)
            {
                case 1:   // Left click down
                case 2:   // Left click up
                    KeyCode1 = IMON_PAD_BUTTON_LCLICK;
                    break;
                case 4:   // Right click down
                case 8:   // Right click up
                    KeyCode1 = IMON_PAD_BUTTON_RCLICK;
                    break;
            }
            if (KeyCode1 != 0)
            {
                RemoteEvent((KeyCode1 + KeyMode), false);
            }
#if DEBUG
            if ((KeyCode == 0) & (KeyCode1 == 0))
            {
                DebugWriteLine("RAW IMON HID (MOUSE REPORT) - Ignoring");
#if TEST_APPLICATION
                Console.WriteLine("RAW IMON HID (MOUSE REPORT) - Ignoring");
#endif
         }
#endif
        }
        else
        {
            MouseEvent(xSign * xSize, ySign * ySize, rightButton, leftButton);
            //if (_mouseHandler != null) _mouseHandler(this.Name, raw.mouse.lLastX, raw.mouse.lLastY, (int)raw.mouse.ulButtons);
        }
         */
        //MouseEvent(xSign * xSize, ySign * ySize, right, left);
      }
      else if (dataBytes[7] == 0xAE)
      {
#if DEBUG
        DebugWriteLine("MCE remote button");
#endif
        uint keyCode = IMON_MCE_BUTTON + dataBytes[3];

        RemoteEvent(keyCode, _remoteToggle != dataBytes[2]);
        _remoteToggle = dataBytes[2];
      }
      else if (dataBytes[7] == 0xBE)
      {
#if DEBUG
        DebugWriteLine("MCE Keyboard key press");
#endif
        KeyboardEvent(dataBytes[2], dataBytes[3]);
      }
      else if (dataBytes[7] == 0xCE)
      {
#if DEBUG
        DebugWriteLine("MCE Keyboard mouse move/button");
#endif
        //int xSign = (dataBytes[2] & 0x20) == 0 ? 1 : -1;
        //int ySign = (dataBytes[1] & 0x10) == 0 ? 1 : -1;

        //int xSize = (dataBytes[3] & 0x0F);
        //int ySize = (dataBytes[2] & 0x0F);

        //bool right = (dataBytes[3] & 0x40) != 0;
        //bool left = (dataBytes[3] & 0x20) != 0;

        //
        int xSign = (dataBytes[0] & 0x01) != 0 ? 1 : -1;
        int ySign = (dataBytes[0] & 0x02) == 0 ? 1 : -1;

        int xSize = ((dataBytes[2] & 0xF0) >> 4);
        int ySize = (dataBytes[2] & 0x0F);

        bool right = ((dataBytes[3] & 0x40) != 0);
        bool left = ((dataBytes[3] & 0x20) != 0);

        MouseEvent(xSign * xSize, ySign * ySize, right, left);
      }
      else if (dataBytes[7] == 0xEE)
      {
#if DEBUG
        DebugWriteLine("Front panel buttons/volume knob");
#endif
        if (dataBytes[3] > 0x01)
        {
          uint keyCode = IMON_PANEL_BUTTON + dataBytes[3];
          RemoteEvent(keyCode, _remoteToggle != dataBytes[3]);
        }
        _remoteToggle = dataBytes[3];

        if (dataBytes[0] == 0x01 && _remoteHandler != null)
          _remoteHandler(Name, IMON_VOLUME_DOWN.ToString());

        if (dataBytes[1] == 0x01 && _remoteHandler != null)
          _remoteHandler(Name, IMON_VOLUME_UP.ToString());
      }
    }

    private void ReceiveThread()
    {
#if DEBUG
      DebugWriteLine("ReceiveThread()\n\n");
#endif

      IntPtr deviceBufferPtr = IntPtr.Zero;

      try
      {
        deviceBufferPtr = Marshal.AllocHGlobal(DosDeviceBufferSize);

        while (_processReceiveThread)
        {
          int bytesRead;
          IoControl(IOCTL_IMON_READ_RC, IntPtr.Zero, 0, deviceBufferPtr, DosDeviceBufferSize, out bytesRead);
          if (bytesRead == DosDeviceBufferSize)
          {
            byte[] dataBytes = new byte[bytesRead];
            Marshal.Copy(deviceBufferPtr, dataBytes, 0, bytesRead);

            // Rubbish data:
            // FF, FF, FF, FF, FF, FF, 9F, FF, 
            // 00, 00, 00, 00, 00, 00, 00, F0, 
            if ((dataBytes[0] != 0xFF || dataBytes[1] != 0xFF || dataBytes[2] != 0xFF || dataBytes[3] != 0xFF ||
                 dataBytes[4] != 0xFF || dataBytes[5] != 0xFF) &&
                (dataBytes[0] != 0x00 || dataBytes[1] != 0x00 || dataBytes[2] != 0x00 || dataBytes[3] != 0x00 ||
                 dataBytes[4] != 0x00 || dataBytes[5] != 0x00))
            {
              ProcessInput(dataBytes);
            }
            Thread.Sleep(5);
          }
        }
      }
#if DEBUG
      catch (Exception ex)
      {
#if TEST_APPLICATION
        Console.WriteLine(ex.ToString());
#endif
        DebugWriteLine(ex.ToString());
#else
      catch
      {
#endif
        if (_deviceHandle != null)
          CancelIo(_deviceHandle);
      }
      finally
      {
        if (deviceBufferPtr != IntPtr.Zero)
          Marshal.FreeHGlobal(deviceBufferPtr);
      }
    }

    private void IoControl(uint ioControlCode, IntPtr inBuffer, int inBufferSize, IntPtr outBuffer, int outBufferSize,
                           out int bytesReturned)
    {
      try
      {
        DeviceIoControl(_deviceHandle, ioControlCode, inBuffer, inBufferSize, outBuffer, outBufferSize,
                        out bytesReturned, IntPtr.Zero);
      }
#if DEBUG
      catch (Exception ex)
      {
#if TEST_APPLICATION
        Console.WriteLine(ex.ToString());
#endif
        DebugWriteLine(ex.ToString());
#else
          catch
          {
#endif
        if (_deviceHandle != null)
          CancelIo(_deviceHandle);

        throw;
      }
    }

    private void SetHardwareMode(RcMode mode)
    {
#if DEBUG
      DebugWriteLine("SetHardwareMode({0})", Enum.GetName(typeof (RcMode), mode));
#endif

      int bytesRead;

      IntPtr deviceBufferPtr = IntPtr.Zero;

      switch (mode)
      {
          #region iMon

        case RcMode.iMon:
          {
            foreach (byte[] send in SetModeiMon)
            {
              try
              {
                deviceBufferPtr = Marshal.AllocHGlobal(send.Length);

                Marshal.Copy(send, 0, deviceBufferPtr, send.Length);
                IoControl(IOCTL_IMON_WRITE, deviceBufferPtr, send.Length, IntPtr.Zero, 0, out bytesRead);

                Marshal.FreeHGlobal(deviceBufferPtr);
                deviceBufferPtr = IntPtr.Zero;
              }
#if DEBUG
              catch (Exception ex)
              {
                DebugWriteLine(ex.ToString());
#else
            catch
            {
#endif
                if (_deviceHandle != null)
                  CancelIo(_deviceHandle);

                if (deviceBufferPtr != IntPtr.Zero)
                  Marshal.FreeHGlobal(deviceBufferPtr);
              }
            }
            break;
          }

          #endregion iMon

          #region Mce

        case RcMode.Mce:
          {
            foreach (byte[] send in SetModeMCE)
            {
              try
              {
                deviceBufferPtr = Marshal.AllocHGlobal(send.Length);

                Marshal.Copy(send, 0, deviceBufferPtr, send.Length);
                IoControl(IOCTL_IMON_WRITE, deviceBufferPtr, send.Length, IntPtr.Zero, 0, out bytesRead);

                Marshal.FreeHGlobal(deviceBufferPtr);
                deviceBufferPtr = IntPtr.Zero;
              }
#if DEBUG
              catch (Exception ex)
              {
                DebugWriteLine(ex.ToString());
#else
            catch
            {
#endif
                if (_deviceHandle != null)
                  CancelIo(_deviceHandle);

                if (deviceBufferPtr != IntPtr.Zero)
                  Marshal.FreeHGlobal(deviceBufferPtr);
              }
            }
            break;
          }

          #endregion Mce
      }
    }

    #endregion DosDevice Specific Helper Functions

    #region Destructor

    /// <summary>
    /// Releases unmanaged resources and performs other cleanup operations before the
    /// <see cref="iMonUSBReceivers"/> is reclaimed by garbage collection.
    /// </summary>
    ~iMonUSBReceivers()
    {
      // Call Dispose with false.  Since we're in the destructor call, the managed resources will be disposed of anyway.
      Dispose(false);
    }

    #endregion Destructor

    /// <summary>
    /// Determine the type of iMon device currently being used.
    /// </summary>
    /// <value>The DeviceType.</value>
    private DeviceType DeviceDriverMode
    {
      get
      {
#if DEBUG
        DebugWriteLine("DeviceDriverMode()");
#endif
        //if ((_DriverMode != DeviceType.DOS) && (_DriverMode != DeviceType.HID))
        if (_DriverMode == DeviceType.NotValid)
        {
          Detect();
        }
        return _DriverMode;
      }
    }

    #region PluginBase Functional Implimentation for iMon HID devices

    /// <summary>
    /// Start the IR Server plugin for HID device.
    /// </summary>
    private void Start_HID()
    {
#if DEBUG
      DebugOpen("iMon Receiver.log");
      DebugWriteLine("Start_HID()");
#endif
      InitializeKeyboardState();
      LoadSettings();

      // locate the iMon devices
      FindDevices_HID();

#if DEBUG
      if (_enableRemoteInput & (RemoteDevice > -1))
      {
#if TEST_APPLICATION
        Console.WriteLine("Remote Usage: {0}", _deviceTree[RemoteDevice].usUsage);
        Console.WriteLine("Remote UsagePage: {0}", _deviceTree[RemoteDevice].usUsagePage);
        Console.WriteLine("Remote Repeat Delay: {0}", _remoteFirstRepeat);
        Console.WriteLine("Remote Held Delay: {0}", _remoteHeldRepeats);
        Console.WriteLine();
#endif
        DebugWriteLine("Remote Usage: {0}", _deviceTree[RemoteDevice].usUsage);
        DebugWriteLine("Remote UsagePage: {0}", _deviceTree[RemoteDevice].usUsagePage);
        DebugWriteLine("Remote Repeat Delay: {0}", _remoteFirstRepeat);
        DebugWriteLine("Remote Held Delay: {0}", _remoteHeldRepeats);
        DebugWriteLine("");
      }
      if (_enableKeyboardInput & (KeyboardDevice > -1))
      {
#if TEST_APPLICATION
        Console.WriteLine("Keyboard Usage: {0}", _deviceTree[KeyboardDevice].usUsage);
        Console.WriteLine("Keyboard UsagePage: {0}", _deviceTree[KeyboardDevice].usUsagePage);
        Console.WriteLine("Keyboard Repeat Delay: {0}", _keyboardFirstRepeat);
        Console.WriteLine("Keyboard Held Delay: {0}", _keyboardHeldRepeats);
        Console.WriteLine();
#endif
        DebugWriteLine("Keyboard Usage: {0}", _deviceTree[KeyboardDevice].usUsage);
        DebugWriteLine("Keyboard UsagePage: {0}", _deviceTree[KeyboardDevice].usUsagePage);
        DebugWriteLine("Keyboard Repeat Delay: {0}", _keyboardFirstRepeat);
        DebugWriteLine("Keyboard Held Delay: {0}", _keyboardHeldRepeats);
        DebugWriteLine("");
      }
      if (_enableMouseInput & (MouseDevice > -1))
      {
#if TEST_APPLICATION
        Console.WriteLine("Mouse Usage: {0}", _deviceTree[MouseDevice].usUsage);
        Console.WriteLine("Mouse UsagePage: {0}", _deviceTree[MouseDevice].usUsagePage);
        Console.WriteLine("Mouse Sensitivity: {0}", _mouseSensitivity);
        Console.WriteLine();
#endif
        DebugWriteLine("Mouse Usage: {0}", _deviceTree[MouseDevice].usUsage);
        DebugWriteLine("Mouse UsagePage: {0}", _deviceTree[MouseDevice].usUsagePage);
        DebugWriteLine("Mouse Sensitivity: {0}", _mouseSensitivity);
        DebugWriteLine("");
      }
#endif

#if DEBUG
#if TEST_APPLICATION
      Console.WriteLine("");
      Console.WriteLine("Configured Hardware Mode: {0}", _hardwareMode);
      Console.WriteLine("Configured Remote MouseStick Mode: {0}", _RemoteMode);
      Console.WriteLine("Using Remote MouseStick Mode: {0}\n", _CurrentRemoteMode);
#endif
      DebugWriteLine("Configured Hardware Mode: {0}", _hardwareMode);
      DebugWriteLine("Configured Remote MouseStick Mode: {0}", _RemoteMode);
      DebugWriteLine("Using Remote MouseStick Mode: {0}\n", _CurrentRemoteMode);
#endif

      // get a file handle for the HID Remote device
#if USE_USB_HID_FUNCTIONS
          HID_OpenDevice(ref _deviceHandle);
          int lastError = Marshal.GetLastWin32Error();

          if (_deviceHandle.IsInvalid)
          {
#if DEBUG
              DebugWriteLine("Start_HID(): Failed to get open device");
#if TEST_APPLICATION
              Console.WriteLine("Start_HID(): Failed to get open device");
#endif
#endif
              throw new Win32Exception(lastError, "Failed to get open device");
          }
#endif


      // make sure the iMon hardware is in the right state
      if (_hardwareMode == RcMode.iMon)
      {
        HID_SetMode(_CurrentRemoteMode);
      }

      _receiverWindowHID = new ReceiverWindow("iMon HID Receiver");
      _receiverWindowHID.ProcMsg += ProcMessage;

      if (RemoteDevice > -1)
      {
        _deviceTree[RemoteDevice].dwFlags = RawInput.RawInputDeviceFlags.InputSink;
        _deviceTree[RemoteDevice].hwndTarget = _receiverWindowHID.Handle;
      }
      if (KeyboardDevice > -1)
      {
        _deviceTree[KeyboardDevice].dwFlags = RawInput.RawInputDeviceFlags.InputSink |
                                              RawInput.RawInputDeviceFlags.NoLegacy;
        _deviceTree[KeyboardDevice].hwndTarget = _receiverWindowHID.Handle;
      }
      if (MouseDevice > -1)
      {
        _deviceTree[MouseDevice].dwFlags = RawInput.RawInputDeviceFlags.InputSink;
        _deviceTree[MouseDevice].hwndTarget = _receiverWindowHID.Handle;
      }

      if (!_enableRemoteInput & !_enableKeyboardInput & !_enableMouseInput)
      {
#if DEBUG
        DebugWriteLine("ERROR: no input devices enabled");
#endif
        throw new InvalidOperationException("no input devices enabled");
      }

      if (_deviceTree != null)
      {
        if (_deviceTree.Length == 0)
        {
#if DEBUG
          DebugWriteLine("ERROR: no iMon devices found");
#endif
          throw new InvalidOperationException("no iMon devices found");
        }

        if (!RegisterForRawInput(_deviceTree))
        {
#if DEBUG
          DebugWriteLine("ERROR: Failed to register for HID Raw input");
#endif
          throw new InvalidOperationException("Failed to register for HID Raw input");
        }
      }
#if DEBUG
      DebugWriteLine("Start_HID(): completed");
#endif
    }

    /// <summary>
    /// Stop the IR Server plugin for HID devices.
    /// </summary>
    private void Stop_HID()
    {
#if DEBUG
      DebugWriteLine("Stop_HID()");
#endif
      if (_deviceTree != null)
      {
        _deviceTree[0].dwFlags |= RawInput.RawInputDeviceFlags.Remove;
        _deviceTree[1].dwFlags |= RawInput.RawInputDeviceFlags.Remove;
        _deviceTree[2].dwFlags |= RawInput.RawInputDeviceFlags.Remove;
        RegisterForRawInput(_deviceTree);

        _receiverWindowHID.ProcMsg -= ProcMessage;
        _receiverWindowHID.DestroyHandle();
        _receiverWindowHID = null;
      }
    }

    /// <summary>
    /// Suspend the IR Server plugin for HID devices when computer enters standby.
    /// </summary>
    private void Suspend_HID()
    {
#if DEBUG
      DebugWriteLine("Suspend_HID()");
#endif
      Stop_HID();
    }

    /// <summary>
    /// Resume the IR Server plugin for HID devices when the computer returns from standby.
    /// </summary>
    private void Resume_HID()
    {
#if DEBUG
      DebugWriteLine("Resume_HID()");
#endif
      Start_HID();
    }

    /// <summary>
    /// Detect the presence of iMon HID devices.  Devices that cannot be detected will always return false.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the device is present, otherwise <c>false</c>.
    /// </returns>
    private bool Detect_HID()
    {
#if DEBUG
      DebugWriteLine("Detect_HID()");
#endif
      bool FoundiMon = false;
      try
      {
        // get the complete list of raw input devices and parse it for supported devices
        List<DeviceDetails> _devices = new List<DeviceDetails>();

        try
        {
          _devices = RawInput.EnumerateDevices();
        }
        catch
        {
          return false;
        }

        if (_devices.Count > 0)
        {
          foreach (DeviceDetails details in _devices)
          {
#if DEBUG
#if TEST_APPLICATION
            Console.Write("Detect_HID(): checking device \"{0}\"\n", details.ID);
#endif
            DebugWriteLine("Detect_HID(): checking device \"{0}\"", details.ID);
#endif
            // check the details against the supported device list
            foreach (string sDevice in SupportedDevices_HID)
            {
              if (details.ID.ToLower().Contains(sDevice.ToLower()))
              {
#if DEBUG
#if TEST_APPLICATION
                Console.Write("Detect_HID(): Found device \"{0}\"", details.ID.Split('#')[1]);
#endif
                DebugWriteLine("Detect_HID(): Found device \"{0}\"", details.ID.Split('#')[1]);
#endif
                // check for remote device - MI_01 (HIDRemoteSuffix)
                if (details.ID.Contains(HIDRemoteSuffix)) //if (details.ID.Contains("MI_01#"))
                {
#if DEBUG
#if TEST_APPLICATION
                  Console.Write("Detect_HID(): Found iMon Remote device\n");
#endif
                  DebugWriteLine("Detect_HID(): Found iMon Remote device\n");
#endif
                  // found the remote device
                  FoundiMon = true;
                }
                // check for keyboard device - MI_00&COL02 (HIDKeyboardSuffix)
                if (details.ID.Contains(HIDKeyboardSuffix))
                {
#if DEBUG
#if TEST_APPLICATION
                  Console.Write("Detect_HID(): Found iMon Keyboard device\n");
#endif
                  DebugWriteLine("Detect_HID(): Found iMon Keyboard device\n");
#endif
                  // found the keyboard device
                  FoundiMon = true;
                }
                // check for remote device - MI_00&COL01 (HIDMouseSuffix)
                if (details.ID.Contains(HIDMouseSuffix))
                {
#if DEBUG
#if TEST_APPLICATION
                  Console.Write("Detect_HID(): Found iMon Mouse device\n");
#endif
                  DebugWriteLine("Detect_HID(): Found iMon Mouse device\n");
#endif
                  // found the mouse device
                  FoundiMon = true;
                }
              }
            }
          }
#if DEBUG
#if DEBUG_FAKE_KEYBOARD
                  FoundiMon = true; ;
#endif
#if DEBUG_FAKE_MOUSE
                  FoundiMon = true; ;
#endif
          DebugWriteLine("Detect_HID(): Found iMon HID device = {0}", FoundiMon);
#endif
          return FoundiMon;
        }
        else
        {
#if DEBUG
          DebugWriteLine("Detect_HID(): Found iMon HID device = {0}", false);
#endif
          return false;
        }
      }
      catch
      {
#if DEBUG
        DebugWriteLine("Detect_HID(): No HID devices attached to the system");
#endif
        return false;
      }
    }

    #endregion

    #region PluginBase Functional Implimentation for iMon DOS devices

    /// <summary>
    /// Start the IR Server plugin for iMon DOS devices.
    /// </summary>
    private void Start_DOS()
    {
#if DEBUG
      DebugOpen("iMon Receiver.log");
      DebugWriteLine("Start_DOS()");
#if TEST_APPLICATION
      Console.WriteLine("Start_DOS()");
#endif
#endif

      LoadSettings();

      _deviceHandle = CreateFile(DosDevicePath, CreateFileAccessTypes.GenericRead | CreateFileAccessTypes.GenericWrite,
                                 CreateFileShares.Read | CreateFileShares.Write, IntPtr.Zero,
                                 CreateFileDisposition.OpenExisting, CreateFileAttributes.Normal, IntPtr.Zero);
      int lastError = Marshal.GetLastWin32Error();

      if (_deviceHandle.IsInvalid)
      {
#if DEBUG
        DebugWriteLine("Start_DOS(): Failed to open device");
#if TEST_APPLICATION
        Console.WriteLine("Start_DOS(): Failed to open device");
#endif
#endif
        throw new Win32Exception(lastError, "Failed to open device");
      }
      else
      {
#if DEBUG
#if TEST_APPLICATION
        Console.WriteLine("");
        Console.WriteLine("Configured Hardware Mode: {0}", _hardwareMode);
        if (_hardwareMode == RcMode.iMon)
        {
          Console.WriteLine("Configured Remote MouseStick Mode: {0}", _RemoteMode);
          Console.WriteLine("Using Remote MouseStick Mode: {0}\n", _CurrentRemoteMode);
        }
        Console.WriteLine("");
#endif
        if (_hardwareMode == RcMode.iMon)
        {
          DebugWriteLine("Configured Hardware Mode: {0}", _hardwareMode);
          DebugWriteLine("Configured Remote MouseStick Mode: {0}", _RemoteMode);
          DebugWriteLine("Using Remote MouseStick Mode: {0}\n", _CurrentRemoteMode);
        }
        else
        {
          DebugWriteLine("Configured Hardware Mode: {0}\n", _hardwareMode);
        }
#endif

        SetHardwareMode(_hardwareMode);

        _processReceiveThread = true;
        _receiveThread = new Thread(ReceiveThread);
        _receiveThread.Name = "iMon Receive Thread";
        _receiveThread.IsBackground = true;
        _receiveThread.Start();
      }
    }

    /// <summary>
    /// Stop the IR Server plugin for iMon DosDevices.
    /// </summary>
    private void Stop_DOS()
    {
#if DEBUG
      DebugWriteLine("Stop_DOS()");
#endif

      if (_processReceiveThread)
      {
        _processReceiveThread = false;

        if (_deviceHandle != null && !_deviceHandle.IsClosed)
          CancelIo(_deviceHandle);
      }

      if (_receiveThread != null && _receiveThread.IsAlive)
        _receiveThread.Abort();

      _receiveThread = null;

      if (_deviceHandle != null)
        _deviceHandle.Dispose();

      _deviceHandle = null;

#if DEBUG
      DebugClose();
#endif
    }

    /// <summary>
    /// Suspend the IR Server plugin when computer enters standby.
    /// </summary>
    private void Suspend_DOS()
    {
#if DEBUG
      DebugWriteLine("Suspend()");
#endif

      Stop_DOS();
    }

    /// <summary>
    /// Resume the IR Server plugin when the computer returns from standby.
    /// </summary>
    private void Resume_DOS()
    {
#if DEBUG
      DebugWriteLine("Resume()");
#endif

      Start_DOS();
    }

    /// <summary>
    /// Detect the presence of the iMon DOS device.  Devices that cannot be detected will always return false.
    /// This method should not throw exceptions.
    /// </summary>
    /// <returns><c>true</c> if the device is present, otherwise <c>false</c>.</returns>
    private bool Detect_DOS()
    {
#if DEBUG
      DebugWriteLine("Detect_DOS()");
#endif
      try
      {
        SafeFileHandle deviceHandle = CreateFile(DosDevicePath,
                                                 CreateFileAccessTypes.GenericRead | CreateFileAccessTypes.GenericWrite,
                                                 CreateFileShares.Read | CreateFileShares.Write, IntPtr.Zero,
                                                 CreateFileDisposition.OpenExisting, CreateFileAttributes.Normal,
                                                 IntPtr.Zero);
        int lastError = Marshal.GetLastWin32Error();

        if (deviceHandle.IsInvalid)
          throw new Win32Exception(lastError, "Failed to open device");

        deviceHandle.Dispose();
#if DEBUG
        DebugWriteLine("Detect_DOS(): completed - found device.");
#endif
        return true;
      }
#if DEBUG
      catch (Exception ex)
      {
        DebugWriteLine(ex.Message);
        DebugWriteLine("Detect_DOS(): completed - device not found.");
#else
          catch
          {
#endif
        return false;
      }
    }

    #endregion

    #region IDisposable Members

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources
    /// </summary>
    public void Dispose()
    {
      // Dispose of the managed and unmanaged resources
      Dispose(true);

      // Tell the GC that the Finalize process no longer needs to be run for this object.
      GC.SuppressFinalize(this);
    }

    #endregion

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      // process only if mananged and unmanaged resources have
      // not been disposed of.
      if (!_disposed)
      {
        if (disposing)
        {
          // dispose managed resources
          Stop();
        }

        // dispose unmanaged resources
        _disposed = true;
      }
    }

    private void InitializeKeyboardState()
    {
      // initialize the key modifier state structure
      ModifierState.ShiftOn = false;
      ModifierState.LastKeyupWasShift = false;
      ModifierState.LastKeydownWasShift = false;
      ModifierState.CtrlOn = false;
      ModifierState.LastKeyupWasCtrl = false;
      ModifierState.LastKeydownWasCtrl = false;
      ModifierState.AltOn = false;
      ModifierState.LastKeyupWasAlt = false;
      ModifierState.LastKeydownWasAlt = false;
    }

    /// <summary>
    /// Loads the settings.
    /// </summary>
    private void LoadSettings()
    {
#if DEBUG
      DebugWriteLine("LoadSettings()");
#endif
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationFile);

        _hardwareMode = (RcMode) Enum.Parse(typeof (RcMode), doc.DocumentElement.Attributes["HardwareMode"].Value);
        _RemoteMode = (RemoteMode) Enum.Parse(typeof (RemoteMode), doc.DocumentElement.Attributes["RemoteMode"].Value);
        _CurrentRemoteMode = _RemoteMode;
        if (_RemoteMode == RemoteMode.SelectWithButton)
        {
          if (_hardwareMode == RcMode.Mce)
          {
            // SwitchWithButton mode is not supported with an MCE keyboard - always use mouse mode
            _CurrentRemoteMode = RemoteMode.Mouse;
          }
          else
          {
            _CurrentRemoteMode = RemoteMode.Keyboard;
          }
        }

        _enableRemoteInput = bool.Parse(doc.DocumentElement.Attributes["EnableRemoteInput"].Value);
        _useSystemRatesRemote = bool.Parse(doc.DocumentElement.Attributes["UseSystemRatesRemote"].Value);
        _remoteFirstRepeat = int.Parse(doc.DocumentElement.Attributes["RemoteFirstRepeat"].Value);
        _remoteHeldRepeats = int.Parse(doc.DocumentElement.Attributes["RemoteHeldRepeats"].Value);

        _enableKeyboardInput = bool.Parse(doc.DocumentElement.Attributes["EnableKeyboardInput"].Value);
        _useSystemRatesKeyboard = bool.Parse(doc.DocumentElement.Attributes["UseSystemRatesKeyboard"].Value);
        _keyboardFirstRepeat = int.Parse(doc.DocumentElement.Attributes["KeyboardFirstRepeat"].Value);
        _keyboardHeldRepeats = int.Parse(doc.DocumentElement.Attributes["KeyboardHeldRepeats"].Value);
        _handleKeyboardLocally = bool.Parse(doc.DocumentElement.Attributes["HandleKeyboardLocally"].Value);

        _enableMouseInput = bool.Parse(doc.DocumentElement.Attributes["EnableMouseInput"].Value);
        _handleMouseLocally = bool.Parse(doc.DocumentElement.Attributes["HandleMouseLocally"].Value);
        _mouseSensitivity = double.Parse(doc.DocumentElement.Attributes["MouseSensitivity"].Value);
      }
#if DEBUG
      catch (Exception ex)
      {
        DebugWriteLine(ex.ToString());
#else
          catch
          {
#endif
        _hardwareMode = RcMode.iMon;
        _RemoteMode = RemoteMode.Keyboard;
        _CurrentRemoteMode = _RemoteMode;

        _enableRemoteInput = true;
        _useSystemRatesRemote = false;
        _remoteFirstRepeat = 400;
        _remoteHeldRepeats = 250;

        _enableKeyboardInput = true;
        _useSystemRatesKeyboard = true;
        _keyboardFirstRepeat = 350;
        _keyboardHeldRepeats = 0;
        _handleKeyboardLocally = true;

        _enableMouseInput = true;
        _handleMouseLocally = true;
        _mouseSensitivity = 1.0d;
      }
    }

    /// <summary>
    /// Saves the settings.
    /// </summary>
    private void SaveSettings()
    {
#if DEBUG
      DebugWriteLine("SaveSettings()");
#endif
      try
      {
        XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, Encoding.UTF8);
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 1;
        writer.IndentChar = (char) 9;
        writer.WriteStartDocument(true);
        writer.WriteStartElement("settings"); // <settings>

        writer.WriteAttributeString("HardwareMode", Enum.GetName(typeof (RcMode), _hardwareMode));
        writer.WriteAttributeString("RemoteMode", Enum.GetName(typeof (RemoteMode), _RemoteMode));

        writer.WriteAttributeString("EnableRemoteInput", _enableRemoteInput.ToString());
        writer.WriteAttributeString("UseSystemRatesRemote", _useSystemRatesRemote.ToString());
        writer.WriteAttributeString("RemoteFirstRepeat", _remoteFirstRepeat.ToString());
        writer.WriteAttributeString("RemoteHeldRepeats", _remoteHeldRepeats.ToString());

        writer.WriteAttributeString("EnableKeyboardInput", _enableKeyboardInput.ToString());
        writer.WriteAttributeString("UseSystemRatesKeyboard", _useSystemRatesKeyboard.ToString());
        writer.WriteAttributeString("KeyboardFirstRepeat", _keyboardFirstRepeat.ToString());
        writer.WriteAttributeString("KeyboardHeldRepeats", _keyboardHeldRepeats.ToString());
        writer.WriteAttributeString("HandleKeyboardLocally", _handleKeyboardLocally.ToString());

        writer.WriteAttributeString("EnableMouseInput", _enableMouseInput.ToString());
        writer.WriteAttributeString("HandleMouseLocally", _handleMouseLocally.ToString());
        writer.WriteAttributeString("MouseSensitivity", _mouseSensitivity.ToString());


        writer.WriteEndElement(); // </settings>
        writer.WriteEndDocument();
        writer.Close();
      }
#if DEBUG
      catch (Exception ex)
      {
        DebugWriteLine(ex.ToString());
      }
#else
          catch
          {
          }
#endif
    }

    #region HID Device Specific Helper Functions

    private bool RegisterForRawInput(RawInput.RAWINPUTDEVICE device)
    {
      RawInput.RAWINPUTDEVICE[] devices = new RawInput.RAWINPUTDEVICE[1];
      devices[0] = device;

      return RegisterForRawInput(devices);
    }

    private bool RegisterForRawInput(RawInput.RAWINPUTDEVICE[] devices)
    {
#if DEBUG
      DebugWriteLine("RegisterForRawInput(): Registering {0} device(s).", devices.Length);
#endif
      if (
        !RawInput.RegisterRawInputDevices(devices, (uint) devices.Length,
                                          (uint) Marshal.SizeOf(typeof (RawInput.RAWINPUTDEVICE))))
      {
        int dwError = Marshal.GetLastWin32Error();
#if DEBUG
        DebugWriteLine("RegisterForRawInput(): error={0}", dwError);
#endif
        throw new Win32Exception(dwError, "Imon:RegisterForRawInput()");
      }
#if DEBUG
      DebugWriteLine("RegisterForRawInput(): Done.");
#endif
      return true;
    }

    /// <summary>
    /// finds the iMon HID devices.
    /// </summary>
    private void FindDevices_HID()
    {
      if (_deviceTree != null) return;
      // configure the device tree
      int numDevices = (_enableRemoteInput ? 1 : 0) + (_enableKeyboardInput ? 1 : 0) + (_enableMouseInput ? 1 : 0);
#if DEBUG
#if TEST_APPLICATION
      Console.Write("FindDevices_HID(): searching for {0} devices\n", numDevices);
#endif
      DebugWriteLine("FindDevices_HID(): searching for {0} devices", numDevices);
#endif
      if (numDevices == 0) return;
      RawInput.RAWINPUTDEVICE rDevice = new RawInput.RAWINPUTDEVICE();
      RawInput.RAWINPUTDEVICE kDevice = new RawInput.RAWINPUTDEVICE();
      RawInput.RAWINPUTDEVICE mDevice = new RawInput.RAWINPUTDEVICE();
      // get the complete list of raw input devices and parse it for supported devices
      List<DeviceDetails> _devices = new List<DeviceDetails>();

      try
      {
        _devices = RawInput.EnumerateDevices();
      }
      catch
      {
        return;
      }

      if (_devices.Count > 0)
      {
        foreach (DeviceDetails details in _devices)
        {
#if DEBUG
#if TEST_APPLICATION
          Console.Write("FindDevices_HID(): checking device \"{0}\"\n", details.ID);
#endif
          DebugWriteLine("FindDevices_HID(): checking device \"{0}\"", details.ID);
#endif
          // check the details against the supported device list
          foreach (string sDevice in SupportedDevices_HID)
          {
            if (details.ID.ToLower().Contains(sDevice.ToLower()))
            {
#if DEBUG
              DebugWriteLine("FindDevices_HID(): Found device \"{0}\"", details.ID.Split('#')[1]);
#endif
              // check for remote device - MI_01#
              if (details.ID.Contains(HIDRemoteSuffix))
              {
#if DEBUG
#if TEST_APPLICATION
                Console.Write("FindDevices_HID(): Found iMon Remote device\n");
#endif
                DebugWriteLine("FindDevices_HID(): Found iMon Remote device\n");
#endif
                // found the remote device
                rDevice = new RawInput.RAWINPUTDEVICE();
                rDevice.usUsage = details.Usage;
                rDevice.usUsagePage = details.UsagePage;
                RemoteDeviceName = details.ID;
              }
              // check for keyboard device - MI_00&Col02#
              if (details.ID.Contains(HIDKeyboardSuffix))
              {
#if DEBUG
#if TEST_APPLICATION
                Console.Write("FindDevices_HID(): Found iMon Keyboard device\n");
#endif
                DebugWriteLine("FindDevices_HID(): Found iMon Keyboard device\n");
#endif
                // found the keyboard device
                kDevice = new RawInput.RAWINPUTDEVICE();
                kDevice.usUsage = details.Usage;
                kDevice.usUsagePage = details.UsagePage;
                KeyboardDeviceName = details.ID;
              }
              // check for remote device - MI_00&Col01#
              if (details.ID.Contains(HIDMouseSuffix))
              {
#if DEBUG
#if TEST_APPLICATION
                Console.Write("FindDevices_HID(): Found iMon Mouse device\n");
#endif
                DebugWriteLine("FindDevices_HID(): Found iMon Mouse device\n");
#endif
                // found the mouse device
                mDevice = new RawInput.RAWINPUTDEVICE();
                mDevice.usUsage = details.Usage;
                mDevice.usUsagePage = details.UsagePage;
                MouseDeviceName = details.ID;
              }
            }
          }
        }
        numDevices = ((rDevice.usUsage > 0) ? 1 : 0) + ((kDevice.usUsage > 0) ? 1 : 0) + ((mDevice.usUsage > 0) ? 1 : 0);
        int DevIndex = 0;
#if DEBUG
#if DEBUG_FAKE_KEYBOARD
              if (kDevice.usUsage == 0) numDevices++;
#endif
#if DEBUG_FAKE_MOUSE
              if (mDevice.usUsage == 0) numDevices++;
#endif
#if TEST_APPLICATION
        Console.Write("FindDevices_HID(): Found {0} Devices\n", numDevices);
#endif
        DebugWriteLine("FindDevices_HID(): Found {0} Devices", numDevices);
#endif
        _deviceTree = new RawInput.RAWINPUTDEVICE[numDevices];
        if (rDevice.usUsage > 0)
        {
          RemoteDevice = DevIndex;
          DevIndex++;
          _deviceTree[RemoteDevice].usUsage = rDevice.usUsage;
          _deviceTree[RemoteDevice].usUsagePage = rDevice.usUsagePage;
#if DEBUG
#if TEST_APPLICATION
          Console.Write("FindDevices_HID(): Added iMon Remote device as deviceTree[{0}]\n", RemoteDevice);
#endif
          DebugWriteLine("FindDevices_HID(): Added iMon Remote device as deviceTree[{0}]", RemoteDevice);
#endif
        }

        if (kDevice.usUsage > 0)
        {
          KeyboardDevice = DevIndex;
          DevIndex++;
          _deviceTree[KeyboardDevice].usUsage = kDevice.usUsage;
          _deviceTree[KeyboardDevice].usUsagePage = kDevice.usUsagePage;
#if DEBUG
#if TEST_APPLICATION
          Console.Write("FindDevices_HID(): Added iMon Keyboard device as deviceTree[{0}]\n", KeyboardDevice);
#endif
          DebugWriteLine("FindDevices_HID(): Added iMon Keyboard device as deviceTree[{0}]", KeyboardDevice);
#endif
        }
#if DEBUG
#if DEBUG_FAKE_KEYBOARD
              else
              {
                  KeyboardDevice = DevIndex;
                  DevIndex++;
                  _deviceTree[KeyboardDevice].usUsage = 6;
                  _deviceTree[KeyboardDevice].usUsagePage = 1;
#if TEST_APPLICATION
                  Console.Write("FindDevices_HID(): Added Fake Keyboard device as deviceTree[{0}]\n", KeyboardDevice);
#endif
                  DebugWriteLine("FindDevices_HID(): Added Fake Keyboard device as deviceTree[{0}]\n", KeyboardDevice);
              }
#endif
#endif

        if (mDevice.usUsage > 0)
        {
          MouseDevice = DevIndex;
          _deviceTree[MouseDevice].usUsage = mDevice.usUsage;
          _deviceTree[MouseDevice].usUsagePage = mDevice.usUsagePage;
#if DEBUG
#if TEST_APPLICATION
          Console.Write("FindDevices_HID(): Added iMon Mouse device as deviceTree[{0}]\n", MouseDevice);
#endif
          DebugWriteLine("FindDevices_HID(): Added iMon Mouse device as deviceTree[{0}]", MouseDevice);
#endif
        }
#if DEBUG
#if DEBUG_FAKE_MOUSE
              else
              {
                  MouseDevice = DevIndex;
                  _deviceTree[MouseDevice].usUsage = 2;
                  _deviceTree[MouseDevice].usUsagePage = 1;
#if DEBUG_FAKE_MOUSE
                  Console.Write("FindDevices_HID(): Added Fake Mouse device as deviceTree[{0}]\n", MouseDevice);
#endif
                  DebugWriteLine("FindDevices_HID(): Added Fake Mouse device as deviceTree[{0}]\n", MouseDevice);
              }
#endif
#endif
      }
    }

    private void ProcMessage(ref Message m)
    {
      switch (m.Msg)
      {
        case RawInput.WM_INPUT:
          ProcessInputCommand(ref m);
          break;

        case RawInput.WM_KEYDOWN:
          ProcessKeyDown(m.WParam.ToInt32());
          break;

        case RawInput.WM_KEYUP:
          ProcessKeyUp(m.WParam.ToInt32());
          break;

        case RawInput.WM_APPCOMMAND:
          ProcessAppCommand(m.LParam.ToInt32());
          break;
      }
    }

    private void ProcessKeyDown(int param)
    {
#if TRACE
      Trace.WriteLine(String.Format("KeyDown - Param: {0}", param));
#endif
      if (_keyboardHandler != null)
        _keyboardHandler(Name, param, false);
    }

    private void ProcessKeyUp(int param)
    {
#if TRACE
      Trace.WriteLine(String.Format("KeyUp - Param: {0}", param));
#endif

      if (_keyboardHandler != null)
        _keyboardHandler(Name, param, true);
    }

    private void ProcessAppCommand(int param)
    {
#if TRACE
      Trace.WriteLine(String.Format("AppCommand - Param: {0}", param));
#endif
#if DEBUG
#if TEST_APPLICATION
      Console.WriteLine("Received AppCommand - Param: {0}", param);
#endif
      DebugWriteLine("Received AppCommand - Param: {0}", param);
#endif
    }

    private void ProcessiMonMouseReport(byte Report1, byte Report2, byte Report3, byte Report4)
    {
      // filter out invalid reports
      if ((Report1 & 0x01) == ((Report2 & 0x80) >> 7)) return; // invalid position data
      if ((Report2 & 0x04) == ((Report2 & 0x02) << 1)) return; // invalid right click
      if ((Report2 & 0x01) == ((Report3 & 0x80) >> 7)) return; // invalid left click
      if (((Report1 & 0xFC) != 0x68) | (Report4 != 0xB7)) return;

      int xSign = (((Report1 & 0x02) != 0) ? -1 : 1);
      int ySign = (((Report1 & 0x01) != 0) ? -1 : 1);
      int xSize = ((Report2 & 0x78) >> 3);
      int ySize = ((Report3 & 0x78) >> 3);

      bool rightButton = ((Report2 & 0x04) != 0);
      bool leftButton = ((Report2 & 0x01) != 0);
      uint ulButtons = (uint) ((Report2 & 0x04) + (Report2 & 0x01));
#if DEBUG
#if TEST_APPLICATION
      Console.WriteLine("ProcessMouseReport():    (xSign = {0}, xSize = {1}, ySign = {2}, ySize = {3} - lButton = {4}, rButton = {5}", xSign, xSize, ySign, ySize, leftButton, rightButton);
#endif
      DebugWriteLine(
        "ProcessMouseReport():    (xSign = {0}, xSize = {1}, ySign = {2}, ySize = {3} - lButton = {4}, rButton = {5}",
        xSign, xSize, ySign, ySize, leftButton, rightButton);
#endif
      //MouseEvent(xSign * xSize, ySign * ySize, right, left);

      // X movement is horizontal (negative towards left), y movement is vertical (negative towards top)
      if (_CurrentRemoteMode == RemoteMode.Keyboard)
      {
        // convert the mouse movement into direction keys
        uint KeyMode = ((_hardwareMode == RcMode.iMon) ? IMON_PAD_BUTTON : IMON_MCE_BUTTON);
        uint KeyCode = 0;
        uint KeyCode1 = 0;
        if ((xSize != 0) || (ySize != 0))
        {
          KeyCode = (uint) TranslateMouseToKeypress(xSign, xSize, ySign, ySize);
          if (KeyCode != 0) RemoteEvent((KeyCode + KeyMode), false);
        }
        KeyCode1 = 0;
        switch (ulButtons)
        {
          case 1: // Left click down
          case 2: // Left click up
            KeyCode1 = IMON_PAD_BUTTON_LCLICK;
            break;
          case 4: // Right click down
          case 8: // Right click up
            KeyCode1 = IMON_PAD_BUTTON_RCLICK;
            break;
        }
        if (KeyCode1 != 0)
        {
          RemoteEvent((KeyCode1 + KeyMode), false);
        }
#if DEBUG
        if ((KeyCode == 0) & (KeyCode1 == 0))
        {
          DebugWriteLine("ProcessMouseReport(): (Keyboard mode) IGNORING MOUSE REPORT");
#if TEST_APPLICATION
          Console.WriteLine("ProcessMouseReport(): (Keyboard mode) IGNORING MOUSE REPORT");
#endif
        }
#endif
      }
      else
      {
        MouseEvent(xSign * xSize, ySign * ySize, rightButton, leftButton);
        //if (_mouseHandler != null) _mouseHandler(this.Name, raw.mouse.lLastX, raw.mouse.lLastY, (int)raw.mouse.ulButtons);
      }
    }

    private void ProcessInputCommand(ref Message message)
    {
      uint dwSize = 0;

      RawInput.GetRawInputData(message.LParam, RawInput.RawInputCommand.Input, IntPtr.Zero, ref dwSize,
                               (uint) Marshal.SizeOf(typeof (RawInput.RAWINPUTHEADER)));

      IntPtr buffer = Marshal.AllocHGlobal((int) dwSize);
      try
      {
        if (buffer == IntPtr.Zero)
          return;

        if (
          RawInput.GetRawInputData(message.LParam, RawInput.RawInputCommand.Input, buffer, ref dwSize,
                                   (uint) Marshal.SizeOf(typeof (RawInput.RAWINPUTHEADER))) != dwSize)
          return;

        RawInput.RAWINPUT raw = (RawInput.RAWINPUT) Marshal.PtrToStructure(buffer, typeof (RawInput.RAWINPUT));

#if !DISABLE_DEVICE_FILTER

        #region device filtering

        // get the name of the device that generated the input message
        string deviceName = string.Empty;
        uint pcbSize = 0;
        RawInput.GetRawInputDeviceInfo(raw.header.hDevice, RawInput.RIDI_DEVICENAME, IntPtr.Zero, ref pcbSize);
        if (pcbSize > 0)
        {
          IntPtr pData = Marshal.AllocHGlobal((int) pcbSize);
          RawInput.GetRawInputDeviceInfo(raw.header.hDevice, RawInput.RIDI_DEVICENAME, pData, ref pcbSize);
          deviceName = Marshal.PtrToStringAnsi(pData);
          Marshal.FreeHGlobal(pData);
        }
        // stop processing if the device that generated the event is NOT an iMon device
        if (deviceName.Equals(string.Empty) | deviceName.Equals(""))
        {
          return;
        }
        else
        {
          if ((RemoteDevice > 0) & (raw.header.dwType == RawInput.RawInputType.HID))
            if (!deviceName.Equals(RemoteDeviceName)) return;
          if ((KeyboardDevice > 0) & (raw.header.dwType == RawInput.RawInputType.Keyboard))
            if (!deviceName.Equals(KeyboardDeviceName)) return;
          if ((MouseDevice > 0) & (raw.header.dwType == RawInput.RawInputType.Mouse))
            if (!deviceName.Equals(MouseDeviceName)) return;
        }
#if DEBUG
        DebugWriteLine("Received Input Command ({0})", Enum.GetName(typeof (RawInput.RawInputType), raw.header.dwType));
#if TEST_APPLICATION
        Console.Write("RAW HID DEVICE: {0}\n", deviceName);
#endif
        DebugWriteLine("RAW HID DEVICE: {0}", deviceName);
#endif

        #endregion

#endif

        switch (raw.header.dwType)
        {
          case RawInput.RawInputType.HID:
            {
              int offset = Marshal.SizeOf(typeof (RawInput.RAWINPUTHEADER)) + Marshal.SizeOf(typeof (RawInput.RAWHID));

              byte[] bRawData = new byte[offset + raw.hid.dwSizeHid];
              Marshal.Copy(buffer, bRawData, 0, bRawData.Length);

              byte[] newArray = new byte[raw.hid.dwSizeHid];
              Array.Copy(bRawData, offset, newArray, 0, newArray.Length);

#if DEBUG
              string RawCode = BitConverter.ToString(newArray);
#if TEST_APPLICATION
              Console.Write("RAW HID DATA: {0}", RawCode);
#endif
              DebugWriteLine("RAW HID DATA: {0}", RawCode);
#endif
              // process the remote button press
              string code = string.Empty;
              if (((newArray[1] & 0xFC) == 0x28) & (newArray[4] == 0xB7))
              {
                // iMon PAD remote
                int val = (((newArray[1] & 0x03) << 6) | (newArray[2] & 0x30) | ((newArray[2] & 0x06) << 1) |
                           ((newArray[3] & 0xC0) >> 6));
                code = String.Format("{0:X2}", val);
                uint keyCode = IMON_PAD_BUTTON + (uint) val;
                if ((_RemoteMode == RemoteMode.SelectWithButton) & (_hardwareMode == RcMode.iMon))
                {
                  if ((keyCode == (IMON_PAD_BUTTON + 0x50)) | (keyCode == (IMON_PAD_BUTTON + 0x51)))
                  {
                    // the keyboard/mouse button was pressed or released
                    if (keyCode == (IMON_PAD_BUTTON + 0x51))
                    {
                      // the mouse/keyboard button was released
                      keyCode = 0;
                      _CurrentRemoteMode = ((_CurrentRemoteMode == RemoteMode.Mouse)
                                              ? RemoteMode.Keyboard
                                              : RemoteMode.Mouse);
                      //only change modes on the release of the button
#if DEBUG
#if TEST_APPLICATION
                      Console.WriteLine("RAW IMON HID REMOTE - MOUSE/ KEYBOARD MODE CHANGED - NEW MODE = {0}\n", Enum.GetName(typeof(RemoteMode), _CurrentRemoteMode));
#endif
                      DebugWriteLine("RAW IMON HID REMOTE - MOUSE/ KEYBOARD MODE CHANGED - NEW MODE = {0}\n",
                                     Enum.GetName(typeof (RemoteMode), _CurrentRemoteMode));
#endif
                    }
                    break;
                  }
                }
                if ((keyCode & 0x01) == 0)
                {
                  if (keyCode > 0)
                  {
                    RemoteEvent(keyCode, _remoteToggle != 1);
                    _remoteToggle = 1;
                  }
                }
                else
                {
                  _remoteToggle = 0;
#if DEBUG
#if TEST_APPLICATION
                  Console.WriteLine("iMon HID RemoteEvent: {0}, {1}\n", keyCode, "RELEASED (CONSUMED)");
#endif
                  DebugWriteLine("iMon HID RemoteEvent: {0}, {1}\n", keyCode, "RELEASED (CONSUMED)");
#endif
                }
              }
              else if (newArray[8] == 0xAE) // MCE remote button
              {
                uint keyCode = IMON_MCE_BUTTON + newArray[4];

                RemoteEvent(keyCode, _remoteToggle != newArray[3]);
                _remoteToggle = newArray[3];
              }
              else if (newArray[8] == 0xBE) // MCE Keyboard key press
              {
                KeyboardEvent(newArray[3], newArray[4]);
              }
              else if (newArray[8] == 0xCE) // MCE Keyboard mouse move/button
              {
                int xSign = (newArray[3] & 0x20) == 0 ? 1 : -1;
                int ySign = (newArray[2] & 0x10) == 0 ? 1 : -1;

                int xSize = (newArray[4] & 0x0F);
                int ySize = (newArray[3] & 0x0F);

                bool right = (newArray[4] & 0x40) != 0;
                bool left = (newArray[4] & 0x20) != 0;

                MouseEvent(xSign * xSize, ySign * ySize, right, left);
              }
              else if (newArray[8] == 0xEE) // Front panel buttons/volume knob
              {
                if (newArray[4] != 0x00)
                {
                  uint keyCode = IMON_PANEL_BUTTON + newArray[4];
                  RemoteEvent(keyCode, _remoteToggle != newArray[4]);
                }
                _remoteToggle = newArray[4];

                if (newArray[1] == 0x01)
                  RemoteEvent(IMON_VOLUME_DOWN, true);
                if (newArray[2] == 0x01)
                  RemoteEvent(IMON_VOLUME_UP, true);
              }
              else if ((newArray[1] & 0xFC) == 0x68)
              {
                ProcessiMonMouseReport(newArray[1], newArray[2], newArray[3], newArray[4]);

                /*
                int xSign = (((newArray[1] & 0x02) != 0) ? 1 : -1);
                int ySign = (((newArray[1] & 0x01) != 0) ? 1 : -1);
                int xSize = ((newArray[2] & 0x78) >> 3);
                int ySize = ((newArray[3] & 0x78) >> 3);

                bool rightButton = ((newArray[2] & 0x04) != 0);
                bool leftButton = ((newArray[2] & 0x01) != 0);
                uint ulButtons = (uint)((newArray[2] & 0x04) + (newArray[2] & 0x01));
#if DEBUG
                DebugWriteLine("iMon PAD mouse move/button:    (xSign = {0}, xSize = {1}, ySign = {2}, ySize = {3} - lButton = {4}, rButton = {5}", xSign, xSize, ySign, ySize, leftButton, rightButton);
#endif
                //MouseEvent(xSign * xSize, ySign * ySize, right, left);

                // X movement is horizontal (negative towards left), y movement is vertical (negative towards top)
                if (_CurrentRemoteMode == RemoteMode.Keyboard)
                {
                    // convert the mouse movement into direction keys
                    uint KeyMode = ((_hardwareMode == RcMode.iMon) ? IMON_PAD_BUTTON : IMON_MCE_BUTTON);
                    uint KeyCode = 0;
                    uint KeyCode1 = 0;
                    if ((xSize!= 0) && (ySize != 0))
                    {
                        KeyCode = (uint)TranslateMouseToKeypress(xSign, xSize, ySign, ySize);
                        if (KeyCode != 0) RemoteEvent((KeyCode + KeyMode), false);
                    }
                    KeyCode1 = 0;
                    switch (ulButtons)
                    {
                        case 1:   // Left click down
                        case 2:   // Left click up
                            KeyCode1 = IMON_PAD_BUTTON_LCLICK;
                            break;
                        case 4:   // Right click down
                        case 8:   // Right click up
                            KeyCode1 = IMON_PAD_BUTTON_RCLICK;
                            break;
                    }
                    if (KeyCode1 != 0)
                    {
                        RemoteEvent((KeyCode1 + KeyMode), false);
                    }
#if DEBUG
                    if ((KeyCode == 0) & (KeyCode1 == 0))
                    {
                        DebugWriteLine("RAW IMON HID (MOUSE REPORT) - Ignoring");
#if TEST_APPLICATION
                        Console.WriteLine("RAW IMON HID (MOUSE REPORT) - Ignoring");
#endif
                    }
#endif
                }
                else
                {
                    MouseEvent(xSign * xSize, ySign * ySize, rightButton, leftButton);
                    //if (_mouseHandler != null) _mouseHandler(this.Name, raw.mouse.lLastX, raw.mouse.lLastY, (int)raw.mouse.ulButtons);
                }
            */
              }
#if TRACE
              Trace.WriteLine(code);
#endif
              //if (_remoteHandler != null) _remoteHandler(this.Name, code);
              break;
            }

          case RawInput.RawInputType.Mouse:
            {
#if TRACE
              Trace.WriteLine(String.Format("Mouse Event"));
              Trace.WriteLine(String.Format("Buttons: {0}", raw.mouse.ulButtons));
              Trace.WriteLine(String.Format("Raw Buttons: {0}", raw.mouse.ulRawButtons));
              Trace.WriteLine(String.Format("Flags: {0}", raw.mouse.usFlags));
              Trace.WriteLine(String.Format("Extra: {0}", raw.mouse.ulExtraInformation));
              Trace.WriteLine(String.Format("Button Data: {0}", raw.mouse.buttonsStr.usButtonData));
              Trace.WriteLine(String.Format("Button Flags: {0}", raw.mouse.buttonsStr.usButtonFlags));
              Trace.WriteLine(String.Format("Last X: {0}", raw.mouse.lLastX));
              Trace.WriteLine(String.Format("Last Y: {0}", raw.mouse.lLastY));
#endif
#if DEBUG
#if TEST_APPLICATION
              Console.WriteLine("RAW IMON HID MOUSE - lLastX: {0}  lLastY: {1}  Buttons: {2}", raw.mouse.lLastX, raw.mouse.lLastY, raw.mouse.ulButtons);
#endif
              DebugWriteLine("RAW IMON HID MOUSE - lLastX: {0}  lLastY: {1}  Buttons: {2}", raw.mouse.lLastX,
                             raw.mouse.lLastY, raw.mouse.ulButtons);
#endif
              // X movement is horizontal (negative towards left), y movement is vertical (negative towards top)
              if (_CurrentRemoteMode == RemoteMode.Keyboard)
              {
                // convert the mouse movement into direction keys
                uint KeyMode = ((_hardwareMode == RcMode.iMon) ? IMON_PAD_BUTTON : IMON_MCE_BUTTON);
                uint KeyCode = 0;
                uint KeyCode1 = 0;
                if ((raw.mouse.lLastX != 0) || (raw.mouse.lLastY != 0))
                {
                  int xDir = ((raw.mouse.lLastX < 0) ? -1 : 1);
                  int yDir = ((raw.mouse.lLastY < 0) ? -1 : 1);
                  KeyCode = (uint) TranslateMouseToKeypress(xDir, raw.mouse.lLastX, yDir, raw.mouse.lLastY);
                  if (KeyCode != 0) RemoteEvent((KeyCode + KeyMode), false);
                }
                KeyCode1 = 0;
                switch (raw.mouse.ulButtons)
                {
                  case 1: // Left click down
                  case 2: // Left click up
                    KeyCode1 = IMON_PAD_BUTTON_LCLICK;
                    break;
                  case 4: // Right click down
                  case 8: // Right click up
                    KeyCode1 = IMON_PAD_BUTTON_RCLICK;
                    break;
                }
                if (KeyCode1 != 0)
                {
                  RemoteEvent((KeyCode1 + KeyMode), false);
                }
#if DEBUG
                if ((KeyCode == 0) & (KeyCode1 == 0))
                {
#if TEST_APPLICATION
                  Console.WriteLine("RAW IMON HID MOUSE - Ignoring");
#endif
                  DebugWriteLine("RAW IMON HID MOUSE - Ignoring");
                }
#endif
              }
              else
              {
                MouseEvent(raw.mouse.lLastX, raw.mouse.lLastY, ((int) (raw.mouse.ulButtons & 0x03) > 0),
                           ((int) (raw.mouse.ulButtons & 0x0C) > 0));
                //if (_mouseHandler != null)
                //    _mouseHandler(this.Name, raw.mouse.lLastX, raw.mouse.lLastY, (int)raw.mouse.ulButtons);
              }
              break;
            }

          case RawInput.RawInputType.Keyboard:
            {
#if TRACE
              Trace.WriteLine("Keyboard Event");
#endif
#if DEBUG
              DebugWriteLine("RAW IMON HID KEYBOARD- CODE: {0}  FLAGS: {1}  MESSAGE: {2}", raw.keyboard.VKey,
                             raw.keyboard.Flags, raw.keyboard.Message);
#if TEST_APPLICATION
              Console.WriteLine("RAW IMON HID KEYBOARD- CODE: {0}  FLAGS: {1}  MESSAGE: {2}  EXTRA: {3}", raw.keyboard.VKey, raw.keyboard.Flags, raw.keyboard.Message, raw.keyboard.ExtraInformation);

              Console.WriteLine("RAW IMON HID KEYBOARD- Shift State: {0}  LastUp: {1}  LastDown: {2}", ModifierState.ShiftOn, ModifierState.LastKeyupWasShift, ModifierState.LastKeydownWasShift);
              Console.WriteLine("RAW IMON HID KEYBOARD- CTRL  State: {0}  LastUp: {1}  LastDown: {2}", ModifierState.CtrlOn, ModifierState.LastKeyupWasCtrl, ModifierState.LastKeyupWasAlt);
              Console.WriteLine("RAW IMON HID KEYBOARD- ALT   State: {0}  LastUp: {1}  LastDown: {2}", ModifierState.AltOn, ModifierState.LastKeyupWasAlt, ModifierState.LastKeydownWasAlt);
#endif
#endif
              bool ConsumeKeypress = false;
              bool SendToKeyboard = false;
              uint KeyCode = 0;

              switch (raw.keyboard.Flags)
              {
                case RawInput.RawKeyboardFlags.KeyE0:
#if TRACE
                  Trace.WriteLine(String.Format("E0: {0}", raw.keyboard.MakeCode));
#endif
#if DEBUG
                  DebugWriteLine(String.Format("KEYBOARD FLAG E0: {0}", raw.keyboard.MakeCode));
#if TEST_APPLICATION
                  Console.WriteLine("KEYBOARD FLAG E0 - CODE: {0}  FLAGS: {1}  MESSAGE: {2}", raw.keyboard.VKey, raw.keyboard.Flags, raw.keyboard.Message);
#endif
#endif
                  KeyCode = 0;
                  uint KeyBase = ((_hardwareMode == RcMode.iMon) ? IMON_PAD_BUTTON : IMON_MCE_BUTTON);
                  if (_hardwareMode == RcMode.iMon)
                  {
                    if (raw.keyboard.VKey == 92)
                    {
                      KeyCode = IMON_PAD_BUTTON_WINKEY;
                    }
                    if (raw.keyboard.VKey == 93)
                    {
                      KeyCode = IMON_PAD_BUTTON_MENUKEY;
                    }
                  }
                  else
                  {
                    if (raw.keyboard.VKey == 37)
                    {
                      KeyCode = IMON_MCE_BUTTON_UP;
                    }
                    if (raw.keyboard.VKey == 38)
                    {
                      KeyCode = IMON_MCE_BUTTON_LEFT;
                    }
                    if (raw.keyboard.VKey == 39)
                    {
                      KeyCode = IMON_MCE_BUTTON_RIGHT;
                    }
                    if (raw.keyboard.VKey == 40)
                    {
                      KeyCode = IMON_MCE_BUTTON_DOWN;
                    }
                  }
                  if (KeyCode != 0) RemoteEvent(KeyCode + KeyBase, false);
                  //if (_keyboardHandler != null)
                  //  _keyboardHandler(this.Name, 0xE000 | raw.keyboard.MakeCode, true);

                  break;

                case RawInput.RawKeyboardFlags.KeyE1:
#if TRACE
                  Trace.WriteLine("E1");
#endif
#if DEBUG
                  DebugWriteLine(String.Format("KEYBOARD FLAG E1: {0}", raw.keyboard.MakeCode));
#if TEST_APPLICATION
                  Console.WriteLine("KEYBOARD FLAG E1 - CODE: {0}  FLAGS: {1}  MESSAGE: {2}", raw.keyboard.VKey, raw.keyboard.Flags, raw.keyboard.Message);
#endif
#endif
                  //if (_keyboardHandler != null)
                  //  _keyboardHandler(this.Name, 0xE100, true);
                  break;

                case RawInput.RawKeyboardFlags.KeyMake:
#if TRACE
                  Trace.WriteLine(String.Format("Make: {0}", raw.keyboard.VKey));
#endif
                  //#if DEBUG
                  //                                  DebugWriteLine("RAW IMON HID KEYBOARD CODE: {0}  FLAGS: {1}  MESSAGE: {2}", raw.keyboard.VKey, raw.keyboard.Flags, raw.keyboard.Message);
                  //                                  Console.WriteLine("RAW IMON HID KEYBOARD CODE: {0}  FLAGS: {1}  MESSAGE: {2}  EXTRA: {3}", raw.keyboard.VKey, raw.keyboard.Flags, raw.keyboard.Message, raw.keyboard.ExtraInformation);
                  //#endif
                  if (_keyboardHandler != null)
                  {
                    KeyCode = 0;
                    // convert the keyboard code into an iMon code
                    if ((raw.keyboard.VKey == 16) | (raw.keyboard.VKey == 17) | (raw.keyboard.VKey == 18))
                    {
                      ModifierState.LastKeydownWasShift = false;
                      ModifierState.LastKeyupWasShift = false;
                      ModifierState.LastKeydownWasCtrl = false;
                      ModifierState.LastKeyupWasCtrl = false;
                      ModifierState.LastKeydownWasAlt = false;
                      ModifierState.LastKeyupWasAlt = false;
                      if (raw.keyboard.VKey == 16)
                      {
                        ModifierState.ShiftOn = true;
                        ModifierState.LastKeydownWasShift = true;
                      }
                      if (raw.keyboard.VKey == 17)
                      {
                        ModifierState.CtrlOn = true;
                        ModifierState.LastKeydownWasCtrl = true;
                      }
                      if (raw.keyboard.VKey == 18)
                      {
                        ModifierState.AltOn = true;
                        ModifierState.LastKeydownWasAlt = true;
                      }
                      ConsumeKeypress = true;
                    }
                    else
                    {
                      if (_hardwareMode == RcMode.iMon)
                      {
                        if (ConvertVKeyToiMonKeyCode((Keyboard.VKey) raw.keyboard.VKey, ModifierState) != 0)
                        {
                          KeyCode = IMON_PAD_BUTTON +
                                    ConvertVKeyToiMonKeyCode((Keyboard.VKey) raw.keyboard.VKey, ModifierState);
                        }
                        else
                        {
                          KeyCode = raw.keyboard.VKey;
                          SendToKeyboard = true;
                        }
                        ModifierState.LastKeydownWasShift = false;
                        ModifierState.LastKeydownWasCtrl = false;
                        ModifierState.LastKeydownWasAlt = false;
                      }
                      else
                      {
                        KeyCode = IMON_MCE_BUTTON +
                                  ConvertVKeyToiMonMceKeyCode((Keyboard.VKey) raw.keyboard.VKey, ModifierState);
                        ModifierState.LastKeydownWasShift = false;
                        ModifierState.LastKeydownWasCtrl = false;
                        ModifierState.LastKeydownWasAlt = false;
                      }
                    }
                    if (!ConsumeKeypress & !SendToKeyboard)
                    {
                      RemoteEvent(KeyCode, false);
                    }
                    else if (!ConsumeKeypress)
                    {
                      _keyboardHandler(Name, (int) KeyCode, false);
                    }
#if DEBUG
                    else
                    {
                      DebugWriteLine("CONSUMED HID KEYBOARD - CODE: {0}  FLAGS: {1}  MESSAGE: {2}", raw.keyboard.VKey,
                                     raw.keyboard.Flags, raw.keyboard.Message);
#if TEST_APPLICATION
                      Console.WriteLine("CONSUMED HID KEYBOARD - CODE: {0}  FLAGS: {1}  MESSAGE: {2}  EXTRA: {3}", raw.keyboard.VKey, raw.keyboard.Flags, raw.keyboard.Message, raw.keyboard.ExtraInformation);
#endif
                    }
#endif
                    //_keyboardHandler(this.Name, (int)KeyCode, false);
                  }
                  break;

                case RawInput.RawKeyboardFlags.KeyBreak:
#if TRACE
                  Trace.WriteLine(String.Format("Break: {0}", raw.keyboard.VKey));
#endif
                  KeyCode = 0;
                  ConsumeKeypress = false;
                  SendToKeyboard = false;
                  //convert the keyboard code into an iMon code
                  if ((raw.keyboard.VKey == 16) | (raw.keyboard.VKey == 17) | (raw.keyboard.VKey == 18))
                  {
                    ModifierState.LastKeydownWasShift = false;
                    ModifierState.LastKeyupWasShift = false;
                    ModifierState.LastKeydownWasCtrl = false;
                    ModifierState.LastKeyupWasCtrl = false;
                    ModifierState.LastKeydownWasAlt = false;
                    ModifierState.LastKeyupWasAlt = false;
                    if (raw.keyboard.VKey == 16)
                    {
                      ModifierState.ShiftOn = false;
                      ModifierState.LastKeyupWasShift = true;
                    }
                    if (raw.keyboard.VKey == 17)
                    {
                      ModifierState.CtrlOn = false;
                      ModifierState.LastKeyupWasAlt = true;
                    }
                    if (raw.keyboard.VKey == 18)
                    {
                      ModifierState.AltOn = false;
                      ModifierState.LastKeyupWasAlt = true;
                    }
                    ConsumeKeypress = true;
                  }
                  else
                  {
                    if (_hardwareMode == RcMode.iMon)
                    {
                      if (ConvertVKeyToiMonKeyCode((Keyboard.VKey) raw.keyboard.VKey, ModifierState) != 0)
                      {
                        KeyCode = IMON_PAD_BUTTON +
                                  ConvertVKeyToiMonKeyCode((Keyboard.VKey) raw.keyboard.VKey, ModifierState);
                      }
                      else
                      {
                        KeyCode = raw.keyboard.VKey;
                        //_keyboardHandler(this.Name, (int)KeyCode, true);
                        SendToKeyboard = true;
                      }
                      ModifierState.LastKeyupWasShift = false;
                      ModifierState.LastKeyupWasCtrl = false;
                      ModifierState.LastKeyupWasAlt = false;
                    }
                    else
                    {
                      KeyCode = IMON_MCE_BUTTON +
                                ConvertVKeyToiMonMceKeyCode((Keyboard.VKey) raw.keyboard.VKey, ModifierState);
                      ModifierState.LastKeyupWasShift = false;
                      ModifierState.LastKeyupWasCtrl = false;
                      ModifierState.LastKeyupWasAlt = false;
                    }
                  }
                  if (!ConsumeKeypress & !SendToKeyboard)
                  {
                    RemoteEvent(KeyCode, false);
                  }
                  else if (!ConsumeKeypress)
                  {
                    _keyboardHandler(Name, (int) KeyCode, true);
                  }
#if DEBUG
                  else
                  {
                    DebugWriteLine("CONSUMED HID KEYBOARD - CODE: {0}  FLAGS: {1}  MESSAGE: {2}\n", raw.keyboard.VKey,
                                   raw.keyboard.Flags, raw.keyboard.Message);
#if TEST_APPLICATION
                    Console.WriteLine("CONSUMED HID KEYBOARD - CODE: {0}  FLAGS: {1}  MESSAGE: {2}", raw.keyboard.VKey, raw.keyboard.Flags, raw.keyboard.Message);
#endif
                  }
#endif
                  break;

                case RawInput.RawKeyboardFlags.TerminalServerSetLED:
#if TRACE
                  Trace.WriteLine("TerminalServerSetLED");
#endif
#if DEBUG
                  DebugWriteLine("RAW IMON HID KEYBOARD - TerminalServerSetLED - CODE: {0}  FLAGS: {1}  MESSAGE: {2}",
                                 raw.keyboard.VKey, raw.keyboard.Flags, raw.keyboard.Message);
#if TEST_APPLICATION
                  Console.WriteLine("RAW IMON HID KEYBOARD - TerminalServerSetLED - CODE: {0}  FLAGS: {1}  MESSAGE: {2}  EXTRA: {3}", raw.keyboard.VKey, raw.keyboard.Flags, raw.keyboard.Message, raw.keyboard.ExtraInformation);
#endif
#endif
                  break;

                case RawInput.RawKeyboardFlags.TerminalServerShadow:
#if TRACE
                  Trace.WriteLine("TerminalServerShadow");
#endif
#if DEBUG
                  DebugWriteLine("RAW IMON HID KEYBOARD - TerminalServerShadow - CODE: {0}  FLAGS: {1}  MESSAGE: {2}",
                                 raw.keyboard.VKey, raw.keyboard.Flags, raw.keyboard.Message);
#if TEST_APPLICATION
                  Console.WriteLine("RAW IMON HID KEYBOARD - TerminalServerShadow - CODE: {0}  FLAGS: {1}  MESSAGE: {2}  EXTRA: {3}", raw.keyboard.VKey, raw.keyboard.Flags, raw.keyboard.Message, raw.keyboard.ExtraInformation);
#endif
#endif
                  break;
              }
#if DEBUG
#if TEST_APPLICATION
              Console.WriteLine("RAW IMON HID KBD - NEW Shift State: {0}  LastUp: {1}  LastDown: {2}", ModifierState.ShiftOn, ModifierState.LastKeyupWasShift, ModifierState.LastKeydownWasShift);
              Console.WriteLine("RAW IMON HID KBD - NEW CTRL  State: {0}  LastUp: {1}  LastDown: {2}", ModifierState.CtrlOn, ModifierState.LastKeyupWasCtrl, ModifierState.LastKeyupWasAlt);
              Console.WriteLine("RAW IMON HID KBD - NEW ALT   State: {0}  LastUp: {1}  LastDown: {2}\n", ModifierState.AltOn, ModifierState.LastKeyupWasAlt, ModifierState.LastKeydownWasAlt);
#endif
#endif
              break;
            }
        }
      }
      finally
      {
        Marshal.FreeHGlobal(buffer);
      }
    }

    #endregion HID Device Specific Helper Functions

    #region IRemoteReceiver Functional Implimentation

    private void RemoteEvent(uint keyCode, bool firstPress)
    {
#if DEBUG
      DebugWriteLine("iMon RemoteEvent: {0}, {1}", keyCode, firstPress);
#if TEST_APPLICATION
      Console.WriteLine("iMon RemoteEvent: {0}, {1}    (button = {2})", keyCode, firstPress, Enum.GetName(typeof(iMonRemoteKeyMapping), Enum.Parse(typeof(iMonRemoteKeyMapping), keyCode.ToString())));
#endif
#endif

      if (!_enableRemoteInput)
        return;

      if (!firstPress && _lastRemoteButtonKeyCode == keyCode)
      {
        TimeSpan timeBetween = DateTime.Now.Subtract(_lastRemoteButtonTime);

        int firstRepeat = _remoteFirstRepeat;
        int heldRepeats = _remoteHeldRepeats;
        if (_useSystemRatesRemote)
        {
          firstRepeat = 250 + (SystemInformation.KeyboardDelay * 250);
          heldRepeats = (int) (1000.0 / (2.5 + (SystemInformation.KeyboardSpeed * 0.888)));
        }

        if (!_remoteButtonRepeated && timeBetween.TotalMilliseconds < firstRepeat)
        {
#if DEBUG
          DebugWriteLine("Skip, First Repeat\n");
#if TEST_APPLICATION
          Console.WriteLine("Skip, First Repeat\n");
#endif
#endif
          return;
        }

        if (_remoteButtonRepeated && timeBetween.TotalMilliseconds < heldRepeats)
        {
#if DEBUG
          DebugWriteLine("Skip, Held Repeat\n");
#if TEST_APPLICATION
          Console.WriteLine("Skip, Held Repeat\n");
#endif
#endif
          return;
        }

        if (_remoteButtonRepeated && timeBetween.TotalMilliseconds > firstRepeat)
          _remoteButtonRepeated = false;
        else
          _remoteButtonRepeated = true;
      }
      else
      {
        _lastRemoteButtonKeyCode = keyCode;
        _remoteButtonRepeated = false;
      }

      _lastRemoteButtonTime = DateTime.Now;

      if (_remoteHandler != null)
        _remoteHandler(Name, keyCode.ToString());
    }

    #endregion

    #region Keyboard Support Functions (MCE Keyboard support and iMON HID Keyboard Device)

    private void KeyboardEvent(uint keyCode, uint modifiers)
    {
#if DEBUG
      DebugWriteLine("iMon KeyboardEvent: {0}, {1}", keyCode, modifiers);
#if TEST_APPLICATION
      Console.WriteLine("iMon KeyboardEvent: {0}, {1}", keyCode, modifiers);
#endif
#endif

      if (!_enableKeyboardInput)
        return;

      if (keyCode != _lastKeyboardKeyCode && modifiers == _lastKeyboardModifiers)
      {
        if (_handleKeyboardLocally)
        {
          KeyUp(_lastKeyboardKeyCode, 0);
          KeyDown(keyCode, 0);
        }
        else
        {
          KeyUpRemote(_lastKeyboardKeyCode, 0);
          KeyDownRemote(keyCode, 0);
        }

        _keyboardKeyRepeated = false;
      }
      else if (keyCode == _lastKeyboardKeyCode && modifiers != _lastKeyboardModifiers)
      {
        uint turnOff = _lastKeyboardModifiers & ~modifiers;
        uint turnOn = modifiers & ~_lastKeyboardModifiers;

        if (_handleKeyboardLocally)
        {
          KeyUp(0, turnOff);
          KeyDown(0, turnOn);
        }
        else
        {
          KeyUpRemote(0, turnOff);
          KeyDownRemote(0, turnOn);
        }

        _keyboardKeyRepeated = false;
      }
      else if (keyCode != _lastKeyboardKeyCode && modifiers != _lastKeyboardModifiers)
      {
        uint turnOff = _lastKeyboardModifiers & ~modifiers;
        uint turnOn = modifiers & ~_lastKeyboardModifiers;

        if (_handleKeyboardLocally)
        {
          KeyUp(_lastKeyboardKeyCode, turnOff);
          KeyDown(keyCode, turnOn);
        }
        else
        {
          KeyUpRemote(_lastKeyboardKeyCode, turnOff);
          KeyDownRemote(keyCode, turnOn);
        }

        _keyboardKeyRepeated = false;
      }
      else if (keyCode == _lastKeyboardKeyCode && modifiers == _lastKeyboardModifiers)
      {
        // Repeats ...
        TimeSpan timeBetween = DateTime.Now.Subtract(_lastKeyboardKeyTime);

        int firstRepeat = _keyboardFirstRepeat;
        int heldRepeats = _keyboardHeldRepeats;
        if (_useSystemRatesRemote)
        {
          firstRepeat = 250 + (SystemInformation.KeyboardDelay * 250);
          heldRepeats = (int) (1000.0 / (2.5 + (SystemInformation.KeyboardSpeed * 0.888)));
        }

        if (!_keyboardKeyRepeated && timeBetween.TotalMilliseconds < firstRepeat)
          return;

        if (_keyboardKeyRepeated && timeBetween.TotalMilliseconds < heldRepeats)
          return;

        if (_keyboardKeyRepeated && timeBetween.TotalMilliseconds > firstRepeat)
          _keyboardKeyRepeated = false;
        else
          _keyboardKeyRepeated = true;

        if (_handleKeyboardLocally)
          KeyDown(keyCode, modifiers);
        else
          KeyDownRemote(keyCode, modifiers);
      }

      _lastKeyboardKeyCode = keyCode;
      _lastKeyboardModifiers = modifiers;

      _lastKeyboardKeyTime = DateTime.Now;
    }

    private static void KeyUp(uint keyCode, uint modifiers)
    {
      if (keyCode != 0)
      {
        Keyboard.VKey vKey = ConvertMceKeyCodeToVKey(keyCode);
        Keyboard.KeyUp(vKey);
      }

      if (modifiers != 0)
      {
        if ((modifiers & (uint) KeyModifiers.LeftAlt) != 0)
          Keyboard.KeyUp(Keyboard.VKey.VK_LMENU);
        if ((modifiers & (uint) KeyModifiers.LeftControl) != 0)
          Keyboard.KeyUp(Keyboard.VKey.VK_LCONTROL);
        if ((modifiers & (uint) KeyModifiers.LeftShift) != 0)
          Keyboard.KeyUp(Keyboard.VKey.VK_LSHIFT);
        if ((modifiers & (uint) KeyModifiers.LeftWin) != 0)
          Keyboard.KeyUp(Keyboard.VKey.VK_LWIN);

        if ((modifiers & (uint) KeyModifiers.RightAlt) != 0)
          Keyboard.KeyUp(Keyboard.VKey.VK_RMENU);
        if ((modifiers & (uint) KeyModifiers.RightControl) != 0)
          Keyboard.KeyUp(Keyboard.VKey.VK_RCONTROL);
        if ((modifiers & (uint) KeyModifiers.RightShift) != 0)
          Keyboard.KeyUp(Keyboard.VKey.VK_RSHIFT);
        if ((modifiers & (uint) KeyModifiers.RightWin) != 0)
          Keyboard.KeyUp(Keyboard.VKey.VK_RWIN);
      }
    }

    private void KeyUpRemote(uint keyCode, uint modifiers)
    {
      if (_keyboardHandler == null)
        return;

      if (keyCode != 0)
      {
        Keyboard.VKey vKey = ConvertMceKeyCodeToVKey(keyCode);
        _keyboardHandler(Name, (int) vKey, true);
      }

      if (modifiers != 0)
      {
        if ((modifiers & (uint) KeyModifiers.LeftAlt) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_LMENU, true);
        if ((modifiers & (uint) KeyModifiers.LeftControl) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_LCONTROL, true);
        if ((modifiers & (uint) KeyModifiers.LeftShift) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_LSHIFT, true);
        if ((modifiers & (uint) KeyModifiers.LeftWin) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_LWIN, true);

        if ((modifiers & (uint) KeyModifiers.RightAlt) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_RMENU, true);
        if ((modifiers & (uint) KeyModifiers.RightControl) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_RCONTROL, true);
        if ((modifiers & (uint) KeyModifiers.RightShift) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_RSHIFT, true);
        if ((modifiers & (uint) KeyModifiers.RightWin) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_RWIN, true);
      }
    }

    private static void KeyDown(uint keyCode, uint modifiers)
    {
      if (modifiers != 0)
      {
        if ((modifiers & (uint) KeyModifiers.LeftAlt) != 0)
          Keyboard.KeyDown(Keyboard.VKey.VK_LMENU);
        if ((modifiers & (uint) KeyModifiers.LeftControl) != 0)
          Keyboard.KeyDown(Keyboard.VKey.VK_LCONTROL);
        if ((modifiers & (uint) KeyModifiers.LeftShift) != 0)
          Keyboard.KeyDown(Keyboard.VKey.VK_LSHIFT);
        if ((modifiers & (uint) KeyModifiers.LeftWin) != 0)
          Keyboard.KeyDown(Keyboard.VKey.VK_LWIN);

        if ((modifiers & (uint) KeyModifiers.RightAlt) != 0)
          Keyboard.KeyDown(Keyboard.VKey.VK_RMENU);
        if ((modifiers & (uint) KeyModifiers.RightControl) != 0)
          Keyboard.KeyDown(Keyboard.VKey.VK_RCONTROL);
        if ((modifiers & (uint) KeyModifiers.RightShift) != 0)
          Keyboard.KeyDown(Keyboard.VKey.VK_RSHIFT);
        if ((modifiers & (uint) KeyModifiers.RightWin) != 0)
          Keyboard.KeyDown(Keyboard.VKey.VK_RWIN);
      }

      if (keyCode != 0)
      {
        Keyboard.VKey vKey = ConvertMceKeyCodeToVKey(keyCode);
        Keyboard.KeyDown(vKey);
      }
    }

    private void KeyDownRemote(uint keyCode, uint modifiers)
    {
      if (_keyboardHandler == null)
        return;

      if (modifiers != 0)
      {
        if ((modifiers & (uint) KeyModifiers.LeftAlt) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_LMENU, false);
        if ((modifiers & (uint) KeyModifiers.LeftControl) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_LCONTROL, false);
        if ((modifiers & (uint) KeyModifiers.LeftShift) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_LSHIFT, false);
        if ((modifiers & (uint) KeyModifiers.LeftWin) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_LWIN, false);

        if ((modifiers & (uint) KeyModifiers.RightAlt) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_RMENU, false);
        if ((modifiers & (uint) KeyModifiers.RightControl) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_RCONTROL, false);
        if ((modifiers & (uint) KeyModifiers.RightShift) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_RSHIFT, false);
        if ((modifiers & (uint) KeyModifiers.RightWin) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_RWIN, false);
      }

      if (keyCode != 0)
      {
        Keyboard.VKey vKey = ConvertMceKeyCodeToVKey(keyCode);
        _keyboardHandler(Name, (int) vKey, false);
      }
    }

    #endregion Keyboard Support Functions (MCE Keyboard support and iMON HID Keyboard Device)

    #region IMouseReceiver Functional Implimentation

    private void MouseEvent(int deltaX, int deltaY, bool right, bool left)
    {
#if DEBUG
      DebugWriteLine("iMon MouseEvent: DX {0}, DY {1}, Right: {2}, Left: {3}", deltaX, deltaY, right, left);
#if TEST_APPLICATION
      Console.WriteLine("iMon MouseEvent: DX {0}, DY {1}, Right: {2}, Left: {3}", deltaX, deltaY, right, left);
#endif
#endif

      if (!_enableMouseInput)
        return;

      #region Buttons

      Mouse.MouseEvents buttons = Mouse.MouseEvents.None;
      if ((_mouseButtons & Mouse.MouseEvents.RightDown) != 0)
      {
        if (!right)
        {
          buttons |= Mouse.MouseEvents.RightUp;
          _mouseButtons &= ~Mouse.MouseEvents.RightDown;
        }
      }
      else
      {
        if (right)
        {
          buttons |= Mouse.MouseEvents.RightDown;
          _mouseButtons |= Mouse.MouseEvents.RightDown;
        }
      }
      if ((_mouseButtons & Mouse.MouseEvents.LeftDown) != 0)
      {
        if (!left)
        {
          buttons |= Mouse.MouseEvents.LeftUp;
          _mouseButtons &= ~Mouse.MouseEvents.LeftDown;
        }
      }
      else
      {
        if (left)
        {
          buttons |= Mouse.MouseEvents.LeftDown;
          _mouseButtons |= Mouse.MouseEvents.LeftDown;
        }
      }

      if (buttons != Mouse.MouseEvents.None)
      {
        if (_handleMouseLocally)
          Mouse.Button(buttons);
      }

      #endregion Buttons

      #region Movement Delta

      deltaX = (int) (deltaX * _mouseSensitivity);
      deltaY = (int) (deltaY * _mouseSensitivity);

      if (deltaX != 0 || deltaY != 0)
      {
        if (_handleMouseLocally)
          Mouse.Move(deltaX, deltaY, false);
      }

      #endregion Movement Delta

      if (!_handleMouseLocally)
        _mouseHandler(Name, deltaX, deltaY, (int) buttons);
    }

    /// <summary>
    /// Translates the mouse data provided into a keypress.
    /// </summary>
    /// <param name="xDir">The x direction.</param>
    /// <param name="xDelta">The x delta.</param>
    /// <param name="yDir">The y direction.</param>
    /// <param name="yDelta">The y delta.</param>
    /// <returns>Remote key code.</returns>
    public int TranslateMouseToKeypress(int xDir, int xDelta, int yDir, int yDelta)
    {
      /*
      int xDelta = Math.Abs(xVal);
      int yDelta = Math.Abs(yVal);
      int xDir = ((xVal == 0) ? 0 : ((xVal > 0) ? 1 : -1));
      int yDir = ((yVal == 0) ? 0 : ((yVal > 0) ? 1 : -1));
      */

      // make sure that we don't get false readings do to mouse acceleration
      /*
      if (xDelta < 6) xDelta = 0;
      if (yDelta < 6) yDelta = 0;
      if ((xDelta > 5) && (yDelta > 5)) return 0;
      */

      if (xDelta > yDelta)
      {
        // horizontal movement is larger, so it has preference
        if (xDir == 1)
        {
          // cursor right
          if (_hardwareMode == RcMode.iMon)
          {
            return (int) IMON_PAD_BUTTON_RIGHT;
          }
          else
          {
            return (int) IMON_MCE_BUTTON_RIGHT;
          }
        }
        else
        {
          // cursor left
          if (_hardwareMode == RcMode.iMon)
          {
            return (int) IMON_PAD_BUTTON_LEFT;
          }
          else
          {
            return (int) IMON_MCE_BUTTON_LEFT;
          }
        }
      }
      else if (yDelta > xDelta)
      {
        // vertical movement is larger, so it has preference
        if (yDir == 1)
        {
          // cursor down
          if (_hardwareMode == RcMode.iMon)
          {
            return (int) IMON_PAD_BUTTON_DOWN;
          }
          else
          {
            return (int) IMON_MCE_BUTTON_DOWN;
          }
        }
        else
        {
          // cursor up
          if (_hardwareMode == RcMode.iMon)
          {
            return (int) IMON_PAD_BUTTON_UP;
          }
          else
          {
            return (int) IMON_MCE_BUTTON_UP;
          }
        }
      }
      return 0;
    }

    #endregion

    #region KeyCode Helper Functions

    // TODO: Convert this function to a lookup from an XML file, then provide multiple files and a way to fine-tune...
    private static Keyboard.VKey ConvertMceKeyCodeToVKey(uint keyCode)
    {
      switch (keyCode)
      {
        case 0x04:
          return Keyboard.VKey.VK_A;
        case 0x05:
          return Keyboard.VKey.VK_B;
        case 0x06:
          return Keyboard.VKey.VK_C;
        case 0x07:
          return Keyboard.VKey.VK_D;
        case 0x08:
          return Keyboard.VKey.VK_E;
        case 0x09:
          return Keyboard.VKey.VK_F;
        case 0x0A:
          return Keyboard.VKey.VK_G;
        case 0x0B:
          return Keyboard.VKey.VK_H;
        case 0x0C:
          return Keyboard.VKey.VK_I;
        case 0x0D:
          return Keyboard.VKey.VK_J;
        case 0x0E:
          return Keyboard.VKey.VK_K;
        case 0x0F:
          return Keyboard.VKey.VK_L;
        case 0x10:
          return Keyboard.VKey.VK_M;
        case 0x11:
          return Keyboard.VKey.VK_N;
        case 0x12:
          return Keyboard.VKey.VK_O;
        case 0x13:
          return Keyboard.VKey.VK_P;
        case 0x14:
          return Keyboard.VKey.VK_Q;
        case 0x15:
          return Keyboard.VKey.VK_R;
        case 0x16:
          return Keyboard.VKey.VK_S;
        case 0x17:
          return Keyboard.VKey.VK_T;
        case 0x18:
          return Keyboard.VKey.VK_U;
        case 0x19:
          return Keyboard.VKey.VK_V;
        case 0x1A:
          return Keyboard.VKey.VK_W;
        case 0x1B:
          return Keyboard.VKey.VK_X;
        case 0x1C:
          return Keyboard.VKey.VK_Y;
        case 0x1D:
          return Keyboard.VKey.VK_Z;
        case 0x1E:
          return Keyboard.VKey.VK_1;
        case 0x1F:
          return Keyboard.VKey.VK_2;
        case 0x20:
          return Keyboard.VKey.VK_3;
        case 0x21:
          return Keyboard.VKey.VK_4;
        case 0x22:
          return Keyboard.VKey.VK_5;
        case 0x23:
          return Keyboard.VKey.VK_6;
        case 0x24:
          return Keyboard.VKey.VK_7;
        case 0x25:
          return Keyboard.VKey.VK_8;
        case 0x26:
          return Keyboard.VKey.VK_9;
        case 0x27:
          return Keyboard.VKey.VK_0;
        case 0x28:
          return Keyboard.VKey.VK_RETURN;
        case 0x29:
          return Keyboard.VKey.VK_ESCAPE;
        case 0x2A:
          return Keyboard.VKey.VK_BACK;
        case 0x2B:
          return Keyboard.VKey.VK_TAB;
        case 0x2C:
          return Keyboard.VKey.VK_SPACE;
        case 0x2D:
          return Keyboard.VKey.VK_OEM_MINUS;
        case 0x2E:
          return Keyboard.VKey.VK_OEM_PLUS;
        case 0x2F:
          return Keyboard.VKey.VK_OEM_4;
        case 0x30:
          return Keyboard.VKey.VK_OEM_6;
        case 0x31:
          return Keyboard.VKey.VK_OEM_5;
          //case 0x32:return Keyboard.VKEY.VK_Non-US #;
        case 0x33:
          return Keyboard.VKey.VK_OEM_1;
        case 0x34:
          return Keyboard.VKey.VK_OEM_7;
        case 0x35:
          return Keyboard.VKey.VK_OEM_3;
        case 0x36:
          return Keyboard.VKey.VK_OEM_COMMA;
        case 0x37:
          return Keyboard.VKey.VK_OEM_PERIOD;
        case 0x38:
          return Keyboard.VKey.VK_OEM_2;
        case 0x39:
          return Keyboard.VKey.VK_CAPITAL;
        case 0x3A:
          return Keyboard.VKey.VK_F1;
        case 0x3B:
          return Keyboard.VKey.VK_F2;
        case 0x3C:
          return Keyboard.VKey.VK_F3;
        case 0x3D:
          return Keyboard.VKey.VK_F4;
        case 0x3E:
          return Keyboard.VKey.VK_F5;
        case 0x3F:
          return Keyboard.VKey.VK_F6;
        case 0x40:
          return Keyboard.VKey.VK_F7;
        case 0x41:
          return Keyboard.VKey.VK_F8;
        case 0x42:
          return Keyboard.VKey.VK_F9;
        case 0x43:
          return Keyboard.VKey.VK_F10;
        case 0x44:
          return Keyboard.VKey.VK_F11;
        case 0x45:
          return Keyboard.VKey.VK_F12;
        case 0x46:
          return Keyboard.VKey.VK_PRINT;
        case 0x47:
          return Keyboard.VKey.VK_SCROLL;
        case 0x48:
          return Keyboard.VKey.VK_PAUSE;
        case 0x49:
          return Keyboard.VKey.VK_INSERT;
        case 0x4A:
          return Keyboard.VKey.VK_HOME;
        case 0x4B:
          return Keyboard.VKey.VK_PRIOR;
        case 0x4C:
          return Keyboard.VKey.VK_DELETE;
        case 0x4D:
          return Keyboard.VKey.VK_END;
        case 0x4E:
          return Keyboard.VKey.VK_NEXT;
        case 0x4F:
          return Keyboard.VKey.VK_RIGHT;
        case 0x50:
          return Keyboard.VKey.VK_LEFT;
        case 0x51:
          return Keyboard.VKey.VK_DOWN;
        case 0x52:
          return Keyboard.VKey.VK_UP;
        case 0x64:
          return Keyboard.VKey.VK_OEM_102;
        case 0x65:
          return Keyboard.VKey.VK_APPS;

        default:
          throw new ArgumentException(String.Format("Unknown Key Value {0}", keyCode), "keyCode");
      }
    }

    private static uint ConvertVKeyToiMonKeyCode(Keyboard.VKey vKey, KeyModifierState ModState)
    {
      switch (vKey)
      {
        case Keyboard.VKey.VK_BACK:
          return 0x20;
        case Keyboard.VKey.VK_SPACE:
          return 0x94;
        case Keyboard.VKey.VK_1:
          return 0x3A;
        case Keyboard.VKey.VK_2:
          return 0xF2;
        case Keyboard.VKey.VK_3:
          if (ModState.ShiftOn | ModState.LastKeyupWasShift)
            return 0x60;

          return 0x32;
        case Keyboard.VKey.VK_4:
          return 0x8A;
        case Keyboard.VKey.VK_5:
          return 0x5A;
        case Keyboard.VKey.VK_6:
          return 0xAA;
        case Keyboard.VKey.VK_7:
          return 0xD6;
        case Keyboard.VKey.VK_8:
          if (ModState.ShiftOn | ModState.LastKeyupWasShift)
            return 0x38;

          return 0x88;
        case Keyboard.VKey.VK_9:
          return 0xA0;
        case Keyboard.VKey.VK_0:
          return 0xEA;
        case Keyboard.VKey.VK_RETURN:
          return 0x22;
        case Keyboard.VKey.VK_ESCAPE:
          return 0xFC;

        default:
          return 0;
          //throw new ArgumentException(String.Format("Unknown Key Value {0}", vKey), "vKey");
      }
    }

    // TODO: Convert this function to a lookup from an XML file, then provide multiple files and a way to fine-tune...
    private static uint ConvertVKeyToiMonMceKeyCode(Keyboard.VKey vKey, KeyModifierState ModState)
    {
      switch (vKey)
      {
        case Keyboard.VKey.VK_A:
          return 0x04;
        case Keyboard.VKey.VK_B:
          return 0x05;
        case Keyboard.VKey.VK_C:
          return 0x06;
        case Keyboard.VKey.VK_D:
          return 0x07;
        case Keyboard.VKey.VK_E:
          return 0x08;
        case Keyboard.VKey.VK_F:
          return 0x09;
        case Keyboard.VKey.VK_G:
          return 0x0A;
        case Keyboard.VKey.VK_H:
          return 0x0B;
        case Keyboard.VKey.VK_I:
          return 0x0C;
        case Keyboard.VKey.VK_J:
          return 0x0D;
        case Keyboard.VKey.VK_K:
          return 0x0E;
        case Keyboard.VKey.VK_L:
          return 0x0F;
        case Keyboard.VKey.VK_M:
          return 0x10;
        case Keyboard.VKey.VK_N:
          return 0x11;
        case Keyboard.VKey.VK_O:
          return 0x12;
        case Keyboard.VKey.VK_P:
          return 0x13;
        case Keyboard.VKey.VK_Q:
          return 0x14;
        case Keyboard.VKey.VK_R:
          return 0x15;
        case Keyboard.VKey.VK_S:
          return 0x16;
        case Keyboard.VKey.VK_T:
          return 0x17;
        case Keyboard.VKey.VK_U:
          return 0x18;
        case Keyboard.VKey.VK_V:
          return 0x19;
        case Keyboard.VKey.VK_W:
          return 0x1A;
        case Keyboard.VKey.VK_X:
          return 0x1B;
        case Keyboard.VKey.VK_Y:
          return 0x1C;
        case Keyboard.VKey.VK_Z:
          return 0x1D;

        case Keyboard.VKey.VK_1:
          return 0x01;
        case Keyboard.VKey.VK_2:
          return 0x02;
        case Keyboard.VKey.VK_3:
          if (ModState.ShiftOn | ModState.LastKeyupWasShift)
            return 0x1E;

          return 0x03;
        case Keyboard.VKey.VK_4:
          return 0x04;
        case Keyboard.VKey.VK_5:
          return 0x05;
        case Keyboard.VKey.VK_6:
          return 0x06;
        case Keyboard.VKey.VK_7:
          return 0x07;
        case Keyboard.VKey.VK_8:
          if (ModState.ShiftOn | ModState.LastKeyupWasShift)
            return 0x1D;

          return 0x08;
        case Keyboard.VKey.VK_9:
          return 0x09;
        case Keyboard.VKey.VK_0:
          return 0x00;

        case Keyboard.VKey.VK_ESCAPE:
          return 0x29;
        case Keyboard.VKey.VK_RETURN:
          return 0x28;

        case Keyboard.VKey.VK_BACK:
          return 0x23;

        case Keyboard.VKey.VK_TAB:
          return 0x2B;
        case Keyboard.VKey.VK_SPACE:
          return 0x2C;
        case Keyboard.VKey.VK_OEM_MINUS:
          return 0x2D;
        case Keyboard.VKey.VK_OEM_PLUS:
          return 0x2E;
        case Keyboard.VKey.VK_OEM_4:
          return 0x2F;
        case Keyboard.VKey.VK_OEM_6:
          return 0x30;
        case Keyboard.VKey.VK_OEM_5:
          return 0x31;
          //case Keyboard.VKEY.VK_Non-US #: return 0X32;
        case Keyboard.VKey.VK_OEM_1:
          return 0x33;
        case Keyboard.VKey.VK_OEM_7:
          return 0x34;
        case Keyboard.VKey.VK_OEM_3:
          return 0x35;
        case Keyboard.VKey.VK_OEM_COMMA:
          return 0x36;
        case Keyboard.VKey.VK_OEM_PERIOD:
          return 0x37;
        case Keyboard.VKey.VK_OEM_2:
          return 0x38;
        case Keyboard.VKey.VK_CAPITAL:
          return 0x39;
        case Keyboard.VKey.VK_F1:
          return 0x3A;
        case Keyboard.VKey.VK_F2:
          return 0x3B;
        case Keyboard.VKey.VK_F3:
          return 0x3C;
        case Keyboard.VKey.VK_F4:
          return 0x3D;
        case Keyboard.VKey.VK_F5:
          return 0x3E;
        case Keyboard.VKey.VK_F6:
          return 0x3F;
        case Keyboard.VKey.VK_F7:
          return 0x40;
        case Keyboard.VKey.VK_F8:
          return 0x41;
        case Keyboard.VKey.VK_F9:
          return 0x42;
        case Keyboard.VKey.VK_F10:
          return 0x43;
        case Keyboard.VKey.VK_F11:
          return 0x44;
        case Keyboard.VKey.VK_F12:
          return 0x45;
        case Keyboard.VKey.VK_PRINT:
          return 0x46;
        case Keyboard.VKey.VK_SCROLL:
          return 0x47;
        case Keyboard.VKey.VK_PAUSE:
          return 0x48;
        case Keyboard.VKey.VK_INSERT:
          return 0x49;
        case Keyboard.VKey.VK_HOME:
          return 0x4A;
        case Keyboard.VKey.VK_PRIOR:
          return 0x4B;
        case Keyboard.VKey.VK_DELETE:
          return 0x4C;
        case Keyboard.VKey.VK_END:
          return 0x4D;
        case Keyboard.VKey.VK_NEXT:
          return 0x4E;
        case Keyboard.VKey.VK_RIGHT:
          return 0x4F;
        case Keyboard.VKey.VK_LEFT:
          return 0x50;
        case Keyboard.VKey.VK_DOWN:
          return 0x51;
        case Keyboard.VKey.VK_UP:
          return 0x52;
        case Keyboard.VKey.VK_OEM_102:
          return 0x64;
        case Keyboard.VKey.VK_APPS:
          return 0x65;

        default:
          throw new ArgumentException(String.Format("Unknown Key Value {0}", vKey), "vKey");
      }
    }

    #endregion KeyCode Helper Functions

    #region iMon Manager Control functions

    private const int WM_MOUSEMOVE = 0x0200;

    private string FindiMonPath()
    {
      RegistryKey rKey;
      string SoundGraphPath = string.Empty;

      rKey = Registry.CurrentUser.OpenSubKey("Software\\SOUNDGRAPH\\iMON", false);
      if (rKey != null)
      {
        // soundgraph registry key exists
        Registry.CurrentUser.Close();
        rKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\iMON.exe",
                                                false);
        if (rKey != null)
        {
          SoundGraphPath = (string) rKey.GetValue("Path", string.Empty);
        }
        Registry.LocalMachine.Close();
      }
      else
      {
        Registry.CurrentUser.Close();
      }
      return SoundGraphPath;
    }

    private void KilliMonManager()
    {
#if DEBUG
      DebugWriteLine("KilliMonManager()");
#if TEST_APPLICATION
      Console.WriteLine("KilliMonManager()");
#endif
      string iMonPath = FindiMonPath();
      string iMonEXE = iMonPath + @"\iMON.exe";
      if (iMonPath != string.Empty)
      {
        DebugWriteLine("KilliMonManager(): Found iMon Manager - Version {0}",
                       FileVersionInfo.GetVersionInfo(iMonEXE).FileVersion);
        Console.WriteLine("KilliMonManager(): Found iMon Manager - Version {0}",
                          FileVersionInfo.GetVersionInfo(iMonEXE).FileVersion);
      }
#endif
      bool hasExited = false;
      Process[] VFDproc = Process.GetProcessesByName("iMON");
      if (VFDproc.Length > 0)
      {
#if DEBUG
        DebugWriteLine("KilliMonManager(): Killing iMon Manager process");
#if TEST_APPLICATION
        Console.WriteLine("KilliMonManager(): Killing iMon Manager process");
#endif
#endif
        VFDproc[0].Kill();
        hasExited = false;
        while (!hasExited)
        {
          Thread.Sleep(100);
          VFDproc[0].Dispose();
          VFDproc = Process.GetProcessesByName("iMON");
          if (VFDproc.Length == 0) hasExited = true;
        }
        // refresh the notification area
        RedrawNotificationArea();
      }
      else
      {
#if DEBUG
        DebugWriteLine("KilliMonManager(): iMon Manager not running");
#if TEST_APPLICATION
        Console.WriteLine("KilliMonManager(): iMon Manager not running");
#endif
#endif
      }
#if DEBUG
      DebugWriteLine("KilliMonManager(): completed");
#if TEST_APPLICATION
      Console.WriteLine("KilliMonManager(): completed");
#endif
#endif
    }

#if USE_USB_HID_FUNCTIONS
      void HID_SetMode_test(RemoteMode mode)
      {
#if DEBUG
          DebugWriteLine("Force_HID_Mode({0})", Enum.GetName(typeof(RemoteMode), mode));
#endif

          //int bytesRead;
          IntPtr deviceBufferPtr = IntPtr.Zero;
          byte[][] ModeData;

          try
          {
              switch (mode)
              {
                  case RemoteMode.Keyboard:
                  case RemoteMode.SelectWithButton:
                      if (RemoteDeviceName.ToLower().Contains("pid_0036"))
                      {
                          ModeData = SetHIDModeKeyboard_X36;
                      } else {
                          ModeData = SetHIDModeKeyboard_X38;
                      }
                      foreach (byte[] send in ModeData)
                      {
                          deviceBufferPtr = Marshal.AllocHGlobal(send.Length);

                          Marshal.Copy(send, 0, deviceBufferPtr, send.Length);

#if DEBUG
                          DebugWriteLine("HID_SetMode({0}): sending command ({1} bytes) to HID device", Enum.GetName(typeof(RemoteMode), mode), send.Length);
#endif
                          int bytesWritten = 0;
                          //IoControl(IOCTL_IMON_WRITE, deviceBufferPtr, send.Length, IntPtr.Zero, 0, out bytesRead);
                          bool writeDevice = WriteFile(_deviceHandle, send, send.Length, out bytesWritten, IntPtr.Zero);
#if DEBUG
                          DebugWriteLine("HID_SetMode({0}): sent {1} bytes to HID device", Enum.GetName(typeof(RemoteMode), mode),bytesWritten);
#endif
 
                          Marshal.FreeHGlobal(deviceBufferPtr);
                      }
                      break;

                  case RemoteMode.Mouse:
                      if (RemoteDeviceName.ToLower().Contains("pid_0036"))
                      {
                          ModeData = SetHIDModeMouse_X36;
                      } else {
                          ModeData = SetHIDModeMouse_X38;
                      }
                      foreach (byte[] send in ModeData)
                      {
#if DEBUG
                          DebugWriteLine("Force_HID_Mode({0}): sending command to HID device", Enum.GetName(typeof(RemoteMode), mode));
#endif
                          deviceBufferPtr = Marshal.AllocHGlobal(send.Length);

                          Marshal.Copy(send, 0, deviceBufferPtr, send.Length);
                          int bytesWritten = 0;
                          //IoControl(IOCTL_IMON_WRITE, deviceBufferPtr, send.Length, IntPtr.Zero, 0, out bytesRead);
                          bool writeDevice = WriteFile(_deviceHandle, send, send.Length, out bytesWritten, IntPtr.Zero);
#if DEBUG
                          DebugWriteLine("Force_HID_Mode({0}): sent {1} bytes to HID device", Enum.GetName(typeof(RemoteMode), mode), bytesWritten);
#endif

                          Marshal.FreeHGlobal(deviceBufferPtr);
                      }
                      break;
              }
          }
#if DEBUG
          catch (Exception ex)
          {
              DebugWriteLine(ex.ToString());
#else
      catch
      {
#endif
              if (_deviceHandle != null)
                  CancelIo(_deviceHandle);
          }
          finally
          {
              if (deviceBufferPtr != IntPtr.Zero)
                  Marshal.FreeHGlobal(deviceBufferPtr);
          }
      }
#endif

    private void HID_SetMode(RemoteMode mode)
    {
#if DEBUG
      DebugWriteLine("HID_SetMode({0})", Enum.GetName(typeof (RemoteMode), mode));
#if TEST_APPLICATION
      Console.WriteLine("HID_SetMode({0})", Enum.GetName(typeof(RemoteMode), mode));
#endif
#endif
      // check for Antec registry entries first
      Process VFDnew;
      string HelperPath = FindiMonPath();

      if (HelperPath == string.Empty)
      {
#if DEBUG
        DebugWriteLine("HID_SetMode({0}): Can't find iMon Manager... unable to set mode",
                       Enum.GetName(typeof (RemoteMode), mode));
#if TEST_APPLICATION
        Console.WriteLine("HID_SetMode({0}): Can't find iMon Manager... unable to set mode", Enum.GetName(typeof(RemoteMode), mode));
#endif
#endif
        return;
      }

      // then check for SoundGraph registry entries
      RegistryKey rKey = Registry.CurrentUser.OpenSubKey("Software\\SOUNDGRAPH\\iMON", true);
      if (rKey != null)
      {
        // set the correct options
        rKey.SetValue("RCPlugin", 1, RegistryValueKind.DWord);
        rKey.SetValue("RunFront", 0, RegistryValueKind.DWord);
        rKey.SetValue("MouseMode", 0, RegistryValueKind.DWord);
        Registry.CurrentUser.Close();
        Thread.Sleep(100);
#if DEBUG
        DebugWriteLine("HID_SetMode({0}): starting iMon Manager to change device Mode",
                       Enum.GetName(typeof (RemoteMode), mode));
#if TEST_APPLICATION
        Console.WriteLine("HID_SetMode({0}): starting iMon Manager to change device Mode", Enum.GetName(typeof(RemoteMode), mode));
#endif
#endif
        // start the iMON.exe process
        VFDnew = new Process();
        //VFDnew.StartInfo.WorkingDirectory = "c:\\Program Files\\SOUNDGRAPH\\iMON\\";
        VFDnew.StartInfo.WorkingDirectory = HelperPath;
        VFDnew.StartInfo.FileName = "iMON.exe";
        Process.Start(VFDnew.StartInfo);
        Thread.Sleep(1500);
#if DEBUG
        DebugWriteLine("HID_SetMode({0}): waiting for iMon Manager termination",
                       Enum.GetName(typeof (RemoteMode), mode));
#if TEST_APPLICATION
        Console.WriteLine("HID_SetMode({0}): waiting for iMon Manager termination", Enum.GetName(typeof (RemoteMode), mode));
#endif
#endif
        KilliMonManager();
      }
    }

    private static void RedrawNotificationArea()
    {
      IntPtr hNotificationArea = GetNotificationAreaHandle();
      RECT r;
      GetClientRect(hNotificationArea, out r);
      for (int x = 0; x < r.Right; x += 5)
        for (int y = 0; y < r.Bottom; y += 5)
          SendMessage(hNotificationArea, WM_MOUSEMOVE, 0, ((y << 16) + x));
    }

    private static IntPtr GetNotificationAreaHandle()
    {
      IntPtr hwnd = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Shell_TrayWnd", null);
      hwnd = FindWindowEx(hwnd, IntPtr.Zero, "TrayNotifyWnd", null);
      hwnd = FindWindowEx(hwnd, IntPtr.Zero, "SysPager", null);
      if (hwnd != IntPtr.Zero) hwnd = FindWindowEx(hwnd, IntPtr.Zero, null, "Notification Area");
      return hwnd;
    }

    [DllImport("user32.dll")]
    private static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass,
                                              string lpszWindow);

    [DllImport("user32.dll")]
    private static extern bool SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

    [Serializable, StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
      public readonly int Left;
      public readonly int Top;
      public readonly int Right;
      public readonly int Bottom;

      public RECT(int left_, int top_, int right_, int bottom_)
      {
        Left = left_;
        Top = top_;
        Right = right_;
        Bottom = bottom_;
      }

      public int Height
      {
        get { return Bottom - Top; }
      }

      public int Width
      {
        get { return Right - Left; }
      }

      public Size Size
      {
        get { return new Size(Width, Height); }
      }

      public Point Location
      {
        get { return new Point(Left, Top); }
      }

      // Handy method for converting to a System.Drawing.Rectangle
      public Rectangle ToRectangle()
      {
        return Rectangle.FromLTRB(Left, Top, Right, Bottom);
      }

      public static RECT FromRectangle(Rectangle rectangle)
      {
        return new RECT(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
      }

      public override int GetHashCode()
      {
        return Left ^ ((Top << 13) | (Top >> 0x13))
               ^ ((Width << 0x1a) | (Width >> 6))
               ^ ((Height << 7) | (Height >> 0x19));
      }

      #region Operator overloads

      public static implicit operator Rectangle(RECT rect)
      {
        return rect.ToRectangle();
      }

      public static implicit operator RECT(Rectangle rect)
      {
        return FromRectangle(rect);
      }

      #endregion
    }

    #endregion

    #region Debug

#if DEBUG

    private static StreamWriter _debugFile;

    /// <summary>
    /// Opens a debug output file.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    private static void DebugOpen(string fileName)
    {
      if (_debugFile != null) return;
      try
      {
#if TEST_APPLICATION
        string path = fileName;
#else
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                                   String.Format("IR Server Suite\\Logs\\{0}", fileName));
#endif
        _debugFile = new StreamWriter(path, false);
        _debugFile.AutoFlush = true;
      }
#if TRACE
      catch (Exception ex)
      {
        Trace.WriteLine(ex.ToString());
#else
      catch
      {
#endif
        _debugFile = null;
      }
    }

    /// <summary>
    /// Closes the debug output file.
    /// </summary>
    private static void DebugClose()
    {
      if (_debugFile != null)
      {
        _debugFile.Close();
        _debugFile.Dispose();
        _debugFile = null;
      }
    }

    /// <summary>
    /// Writes a line to the debug output file.
    /// </summary>
    /// <param name="line">The line.</param>
    /// <param name="args">Formatting arguments.</param>
    private static void DebugWriteLine(string line, params object[] args)
    {
      if (_debugFile != null)
      {
        _debugFile.Write("{0:yyyy-MM-dd HH:mm:ss.ffffff} - ", DateTime.Now);
        _debugFile.WriteLine(line, args);
      }
#if TRACE
      else
      {
        Trace.WriteLine(String.Format(line, args));
      }
#endif
    }

    /// <summary>
    /// Writes a string to the debug output file.
    /// </summary>
    /// <param name="text">The string to write.</param>
    /// <param name="args">Formatting arguments.</param>
    private static void DebugWrite(string text, params object[] args)
    {
      if (_debugFile != null)
      {
        _debugFile.Write(text, args);
      }
#if TRACE
      else
      {
        Trace.Write(String.Format(text, args));
      }
#endif
    }

    /// <summary>
    /// Writes a new line to the debug output file.
    /// </summary>
    private static void DebugWriteNewLine()
    {
      if (_debugFile != null)
      {
        _debugFile.WriteLine();
      }
#if TRACE
      else
      {
        Trace.WriteLine(String.Empty);
      }
#endif
    }

    /// <summary>
    /// Dumps an Array to the debug output file.
    /// </summary>
    /// <param name="array">The array.</param>
    private static void DebugDump(Array array)
    {
      foreach (object item in array)
      {
        if (item is byte)
        {
          DebugWrite("{0:X2}", (byte) item);
        }
        else if (item is ushort)
        {
          DebugWrite("{0:X4}", (ushort) item);
        }
        else if (item is int)
        {
          DebugWrite("{1}{0}", (int) item, (int) item > 0 ? "+" : String.Empty);
        }
        else
        {
          DebugWrite("{0}", item);
        }

        DebugWrite(", ");
      }

      DebugWriteNewLine();
    }

#endif

    #endregion Debug

    #region IR Server Suite PluginBase Interface Implimentaion

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name
    {
      get { return "iMon USB"; }
    }

    /// <summary>
    /// IR Server plugin version.
    /// </summary>
    /// <value>The version.</value>
    public override string Version
    {
      get { return "1.4.2.0"; }
    }

    /// <summary>
    /// The IR Server plugin's author.
    /// </summary>
    /// <value>The author.</value>
    public override string Author
    {
      get { return "and-81 and CybrMage"; }
    }

    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    /// <value>The description.</value>
    public override string Description
    {
      get { return "Supports iMon USB Legacy and HID IR devices"; }
    }

    /// <summary>
    /// Gets a display icon for the plugin.
    /// </summary>
    /// <value>The icon.</value>
    public override Icon DeviceIcon
    {
      get { return Resources.Icon; }
    }

    /// <summary>
    /// Detect the presence of iMon devices.  Devices that cannot be detected will always return false.
    /// </summary>
    /// <returns>
    /// <c>true</c> if a DOS or HID device is present, otherwise <c>false</c>.
    /// </returns>
    public override bool Detect()
    {
#if DEBUG
      DebugWriteLine("Detect()");
#endif
      bool HasDOS = Detect_DOS();
      if (HasDOS)
      {
        _DriverMode = DeviceType.DOS;
#if DEBUG
        DebugWriteLine("Detect(): completed - found DOS device");
#endif
        return true;
      }
      bool HasHID = Detect_HID();
      if (HasHID)
      {
        _DriverMode = DeviceType.HID;
#if DEBUG
        DebugWriteLine("Detect(): completed - found HID device");
#endif
        return true;
      }
      _DriverMode = DeviceType.None;
#if DEBUG
      DebugWriteLine("Detect(): completed - device not found");
#endif
      return false;
    }

    /// <summary>
    /// Start the IR Server plugin for HID device.
    /// </summary>
    public override void Start()
    {
      DeviceType DevType = DeviceDriverMode;
#if DEBUG
      DebugOpen("iMonHidReceiver.log");
      DebugWriteLine("Start()");
      DebugWriteLine("Start(): DeviceDriverMode = {0}", Enum.GetName(typeof (DeviceType), DevType));
#endif
      // ensure that iMon manager will not interfere with us
      KilliMonManager();

      if (DevType == DeviceType.DOS)
      {
#if DEBUG
        DebugWriteLine("Start(): Starting DOS device");
#endif
        Start_DOS();
      }
      else if (DevType == DeviceType.HID)
      {
#if DEBUG
        DebugWriteLine("Start(): Starting HID device");
#endif
        Start_HID();
      }
      else
      {
#if DEBUG
        DebugWriteLine("Start(): No iMon devices found!");
#endif
        throw new ApplicationException("No iMon devices found!");
      }
    }

    /// <summary>
    /// Suspend the IR Server plugin for HID devices when computer enters standby.
    /// </summary>
    public override void Suspend()
    {
#if DEBUG
      DebugWriteLine("Suspend()");
#endif
      if (_DriverMode == DeviceType.HID)
        Stop_HID();
      else
        Stop_DOS();
    }

    /// <summary>
    /// Resume the IR Server plugin for HID devices when the computer returns from standby.
    /// </summary>
    public override void Resume()
    {
#if DEBUG
      DebugWriteLine("Resume()");
#endif
      if (_DriverMode == DeviceType.HID)
        Start_HID();
      else
        Start_DOS();
    }

    /// <summary>
    /// Stop the IR Server plugin.
    /// </summary>
    public override void Stop()
    {
#if DEBUG
      DebugWriteLine("Stop()");
#endif
      if (_DriverMode == DeviceType.HID)
      {
        Stop_HID();
      }
      else
      {
        Stop_DOS();
      }
    }

    #endregion

    #region IR Server Suite IConfigure Interface Implimentaion

    /// <summary>
    /// Configure the IR Server plugin.
    /// </summary>
    public void Configure(IWin32Window owner)
    {
#if DEBUG
      DebugWriteLine("Configure()");
#endif
      LoadSettings();

      Configuration config = new Configuration();

      config.HardwareMode = _hardwareMode;
      config.RemoteMode = _RemoteMode;

      config.EnableRemote = _enableRemoteInput;
      config.UseSystemRatesForRemote = _useSystemRatesRemote;
      config.RemoteRepeatDelay = _remoteFirstRepeat;
      config.RemoteHeldDelay = _remoteHeldRepeats;

      config.EnableKeyboard = _enableKeyboardInput;
      config.UseSystemRatesForKeyboard = _useSystemRatesKeyboard;
      config.KeyboardRepeatDelay = _keyboardFirstRepeat;
      config.KeyboardHeldDelay = _keyboardHeldRepeats;
      config.HandleKeyboardLocal = _handleKeyboardLocally;

      config.EnableMouse = _enableMouseInput;
      config.HandleMouseLocal = _handleMouseLocally;
      config.MouseSensitivity = _mouseSensitivity;

      if (config.ShowDialog(owner) == DialogResult.OK)
      {
        _hardwareMode = config.HardwareMode;
        if ((_hardwareMode != RcMode.iMon) & (_hardwareMode != RcMode.Mce)) _hardwareMode = RcMode.iMon;

        _RemoteMode = config.RemoteMode;
        if ((_RemoteMode != RemoteMode.Keyboard) & (_RemoteMode != RemoteMode.Mouse) &
            (_RemoteMode != RemoteMode.SelectWithButton)) _RemoteMode = RemoteMode.Keyboard;

        _enableRemoteInput = config.EnableRemote;
        _useSystemRatesRemote = config.UseSystemRatesForRemote;
        _remoteFirstRepeat = config.RemoteRepeatDelay;
        _remoteHeldRepeats = config.RemoteHeldDelay;

        _enableKeyboardInput = config.EnableKeyboard;
        _useSystemRatesKeyboard = config.UseSystemRatesForKeyboard;
        _keyboardFirstRepeat = config.KeyboardRepeatDelay;
        _keyboardHeldRepeats = config.KeyboardHeldDelay;
        _handleKeyboardLocally = config.HandleKeyboardLocal;

        _enableMouseInput = config.EnableMouse;
        _handleMouseLocally = config.HandleMouseLocal;
        _mouseSensitivity = config.MouseSensitivity;

        SaveSettings();
      }
#if DEBUG
      DebugWriteLine("Configure(): Completed");
#endif
    }

    #endregion

    #region IR Server Suite IRemoteReceiver Interface Implimentation

    /// <summary>
    /// Callback for remote button presses.
    /// </summary>
    public RemoteHandler RemoteCallback
    {
      get { return _remoteHandler; }
      set { _remoteHandler = value; }
    }

    #endregion

    #region IR Server Suite IKeyboardReceiver Interface Implimentation

    /// <summary>
    /// Callback for keyboard presses.
    /// </summary>
    public KeyboardHandler KeyboardCallback
    {
      get { return _keyboardHandler; }
      set { _keyboardHandler = value; }
    }

    #endregion

    #region IR Server Suite IMouseReceiver Interface Implimentation

    /// <summary>
    /// Callback for mouse events.
    /// </summary>
    public MouseHandler MouseCallback
    {
      get { return _mouseHandler; }
      set { _mouseHandler = value; }
    }

    #endregion
  }
}