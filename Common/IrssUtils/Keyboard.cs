using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace IrssUtils
{

  /// <summary>
  /// Win32 native method wrapper for Keyboard control functions.
  /// http://msdn2.microsoft.com/en-us/library/ms646304.aspx
  /// </summary>
  public static class Keyboard
  {

    #region Interop

    [DllImport("user32.dll")]
    static extern void keybd_event(
      byte bVk,
      byte bScan,
      uint dwFlags,
      IntPtr dwExtraInfo);

    [DllImport("user32.dll")]
    static extern short VkKeyScan(
      char ch);

    #endregion Interop

    #region Enumerations

    /// <summary>
    /// Virtual Key Codes
    /// </summary>
    public enum VKey
    {
      /// <summary>
      /// No Key.
      /// </summary>
      None = 0,
      /// <summary>
      /// 0
      /// </summary>
      VK_0 = 0x30,
      /// <summary>
      /// 1
      /// </summary>
      VK_1 = 0x31,
      /// <summary>
      /// 2
      /// </summary>
      VK_2 = 0x32,
      /// <summary>
      /// 3
      /// </summary>
      VK_3 = 0x33,
      /// <summary>
      /// 4
      /// </summary>
      VK_4 = 0x34,
      /// <summary>
      /// 5
      /// </summary>
      VK_5 = 0x35,
      /// <summary>
      /// 6
      /// </summary>
      VK_6 = 0x36,
      /// <summary>
      /// 7
      /// </summary>
      VK_7 = 0x37,
      /// <summary>
      /// 8
      /// </summary>
      VK_8 = 0x38,
      /// <summary>
      /// 9
      /// </summary>
      VK_9 = 0x39,
      /// <summary>
      /// A
      /// </summary>
      VK_A = 0x41,
      /// <summary>
      /// B
      /// </summary>
      VK_B = 0x42,
      /// <summary>
      /// C
      /// </summary>
      VK_C = 0x43,
      /// <summary>
      /// D
      /// </summary>
      VK_D = 0x44,
      /// <summary>
      /// E
      /// </summary>
      VK_E = 0x45,
      /// <summary>
      /// F
      /// </summary>
      VK_F = 0x46,
      /// <summary>
      /// G
      /// </summary>
      VK_G = 0x47,
      /// <summary>
      /// H
      /// </summary>
      VK_H = 0x48,
      /// <summary>
      /// I
      /// </summary>
      VK_I = 0x49,
      /// <summary>
      /// J
      /// </summary>
      VK_J = 0x4A,
      /// <summary>
      /// K
      /// </summary>
      VK_K = 0x4B,
      /// <summary>
      /// L
      /// </summary>
      VK_L = 0x4C,
      /// <summary>
      /// M
      /// </summary>
      VK_M = 0x4D,
      /// <summary>
      /// N
      /// </summary>
      VK_N = 0x4E,
      /// <summary>
      /// O
      /// </summary>
      VK_O = 0x4F,
      /// <summary>
      /// P
      /// </summary>
      VK_P = 0x50,
      /// <summary>
      /// Q
      /// </summary>
      VK_Q = 0x51,
      /// <summary>
      /// R
      /// </summary>
      VK_R = 0x52,
      /// <summary>
      /// S
      /// </summary>
      VK_S = 0x53,
      /// <summary>
      /// T
      /// </summary>
      VK_T = 0x54,
      /// <summary>
      /// U
      /// </summary>
      VK_U = 0x55,
      /// <summary>
      /// V
      /// </summary>
      VK_V = 0x56,
      /// <summary>
      /// W
      /// </summary>
      VK_W = 0x57,
      /// <summary>
      /// X
      /// </summary>
      VK_X = 0x58,
      /// <summary>
      /// Y
      /// </summary>
      VK_Y = 0x59,
      /// <summary>
      /// Z
      /// </summary>
      VK_Z = 0x5A,
      /// <summary>
      /// Add
      /// </summary>
      VK_ADD = 0x6B,
      /// <summary>
      /// Apps
      /// </summary>
      VK_APPS = 0x5D,
      /// <summary>
      /// Attn
      /// </summary>
      VK_ATTN = 0xF6,
      /// <summary>
      /// Backspace
      /// </summary>
      VK_BACK = 0x8,
      /// <summary>
      /// Cancel
      /// </summary>
      VK_CANCEL = 0x3,
      /// <summary>
      /// Captial
      /// </summary>
      VK_CAPITAL = 0x14,
      /// <summary>
      /// Clear
      /// </summary>
      VK_CLEAR = 0xC,
      /// <summary>
      /// Control
      /// </summary>
      VK_CONTROL = 0x11,
      /// <summary>
      /// CRSel
      /// </summary>
      VK_CRSEL = 0xF7,
      /// <summary>
      /// Decimal
      /// </summary>
      VK_DECIMAL = 0x6E,
      /// <summary>
      /// Delete
      /// </summary>
      VK_DELETE = 0x2E,
      /// <summary>
      /// Divide
      /// </summary>
      VK_DIVIDE = 0x6F,
      /// <summary>
      /// Down
      /// </summary>
      VK_DOWN = 0x28,
      /// <summary>
      /// End
      /// </summary>
      VK_END = 0x23,
      /// <summary>
      /// Ereof
      /// </summary>
      VK_EREOF = 0xF9,
      /// <summary>
      /// Escape
      /// </summary>
      VK_ESCAPE = 0x1B,
      /// <summary>
      /// Execute
      /// </summary>
      VK_EXECUTE = 0x2B,
      /// <summary>
      /// Exsel
      /// </summary>
      VK_EXSEL = 0xF8,
      /// <summary>
      /// F1
      /// </summary>
      VK_F1 = 0x70,
      /// <summary>
      /// F2
      /// </summary>
      VK_F2 = 0x71,
      /// <summary>
      /// F3
      /// </summary>
      VK_F3 = 0x72,
      /// <summary>
      /// F4
      /// </summary>
      VK_F4 = 0x73,
      /// <summary>
      /// F5
      /// </summary>
      VK_F5 = 0x74,
      /// <summary>
      /// F6
      /// </summary>
      VK_F6 = 0x75,
      /// <summary>
      /// F7
      /// </summary>
      VK_F7 = 0x76,
      /// <summary>
      /// F8
      /// </summary>
      VK_F8 = 0x77,
      /// <summary>
      /// F9
      /// </summary>
      VK_F9 = 0x78,
      /// <summary>
      /// F10
      /// </summary>
      VK_F10 = 0x79,
      /// <summary>
      /// F11
      /// </summary>
      VK_F11 = 0x7A,
      /// <summary>
      /// F12
      /// </summary>
      VK_F12 = 0x7B,
      /// <summary>
      /// F13
      /// </summary>
      VK_F13 = 0x7C,
      /// <summary>
      /// F14
      /// </summary>
      VK_F14 = 0x7D,
      /// <summary>
      /// F15
      /// </summary>
      VK_F15 = 0x7E,
      /// <summary>
      /// F16
      /// </summary>
      VK_F16 = 0x7F,
      /// <summary>
      /// F17
      /// </summary>
      VK_F17 = 0x80,
      /// <summary>
      /// F18
      /// </summary>
      VK_F18 = 0x81,
      /// <summary>
      /// F19
      /// </summary>
      VK_F19 = 0x82,
      /// <summary>
      /// F20
      /// </summary>
      VK_F20 = 0x83,
      /// <summary>
      /// F21
      /// </summary>
      VK_F21 = 0x84,
      /// <summary>
      /// F22
      /// </summary>
      VK_F22 = 0x85,
      /// <summary>
      /// F23
      /// </summary>
      VK_F23 = 0x86,
      /// <summary>
      /// F24
      /// </summary>
      VK_F24 = 0x87,
      /// <summary>
      /// Help
      /// </summary>
      VK_HELP = 0x2F,
      /// <summary>
      /// Home
      /// </summary>
      VK_HOME = 0x24,
      /// <summary>
      /// Insert
      /// </summary>
      VK_INSERT = 0x2D,
      /// <summary>
      /// Left Button
      /// </summary>
      VK_LBUTTON = 0x1,
      /// <summary>
      /// Left Control
      /// </summary>
      VK_LCONTROL = 0xA2,
      /// <summary>
      /// Left
      /// </summary>
      VK_LEFT = 0x25,
      /// <summary>
      /// Left Menu
      /// </summary>
      VK_LMENU = 0xA4,
      /// <summary>
      /// Left Shift
      /// </summary>
      VK_LSHIFT = 0xA0,
      /// <summary>
      /// Left Windows Key
      /// </summary>
      VK_LWIN = 0x5B,
      /// <summary>
      /// Middle Button
      /// </summary>
      VK_MBUTTON = 0x4,
      /// <summary>
      /// Menu
      /// </summary>
      VK_MENU = 0x12,
      /// <summary>
      /// Multiply
      /// </summary>
      VK_MULTIPLY = 0x6A,
      /// <summary>
      /// Next
      /// </summary>
      VK_NEXT = 0x22,
      /// <summary>
      /// No Name
      /// </summary>
      VK_NONAME = 0xFC,
      /// <summary>
      /// NumLock
      /// </summary>
      VK_NUMLOCK = 0x90,
      /// <summary>
      /// Numpad 0
      /// </summary>
      VK_NUMPAD0 = 0x60,
      /// <summary>
      /// Numpad 1
      /// </summary>
      VK_NUMPAD1 = 0x61,
      /// <summary>
      /// Numpad 2
      /// </summary>
      VK_NUMPAD2 = 0x62,
      /// <summary>
      /// Numpad 3
      /// </summary>
      VK_NUMPAD3 = 0x63,
      /// <summary>
      /// Numpad 4
      /// </summary>
      VK_NUMPAD4 = 0x64,
      /// <summary>
      /// Numpad 5
      /// </summary>
      VK_NUMPAD5 = 0x65,
      /// <summary>
      /// Numpad 6
      /// </summary>
      VK_NUMPAD6 = 0x66,
      /// <summary>
      /// Numpad 7
      /// </summary>
      VK_NUMPAD7 = 0x67,
      /// <summary>
      /// Numpad 8
      /// </summary>
      VK_NUMPAD8 = 0x68,
      /// <summary>
      /// Numpad 9
      /// </summary>
      VK_NUMPAD9 = 0x69,
      /// <summary>
      /// ;:
      /// </summary>
      VK_OEM_1 = 0xBA,
      /// <summary>
      /// /?
      /// </summary>
      VK_OEM_2 = 0xBF,
      /// <summary>
      /// "`~" for US
      /// </summary>
      VK_OEM_3 = 0xC0,
      /// <summary>
      /// "[{" for US
      /// </summary>
      VK_OEM_4 = 0xDB,
      /// <summary>
      /// "\|" for US
      /// </summary>
      VK_OEM_5 = 0xDC,
      /// <summary>
      /// "]}" for US
      /// </summary>
      VK_OEM_6 = 0xDD,
      /// <summary>
      /// "'"" for US
      /// </summary>
      VK_OEM_7 = 0xDE,
      /// <summary>
      /// OEM_102
      /// </summary>
      VK_OEM_102 = 0xE2,
      /// <summary>
      /// OEM Clear
      /// </summary>
      VK_OEM_CLEAR = 0xFE,
      /// <summary>
      /// OEM Comma
      /// </summary>
      VK_OEM_COMMA = 0xBC,
      /// <summary>
      /// OEM Minus
      /// </summary>
      VK_OEM_MINUS = 0xBD,
      /// <summary>
      /// OEM Period
      /// </summary>
      VK_OEM_PERIOD = 0xBE,
      /// <summary>
      /// "+="
      /// </summary>
      VK_OEM_PLUS = 0xBB,
      /// <summary>
      /// PA1
      /// </summary>
      VK_PA1 = 0xFD,
      /// <summary>
      /// Pause
      /// </summary>
      VK_PAUSE = 0x13,
      /// <summary>
      /// Play
      /// </summary>
      VK_PLAY = 0xFA,
      /// <summary>
      /// Print
      /// </summary>
      VK_PRINT = 0x2A,
      /// <summary>
      /// Prior
      /// </summary>
      VK_PRIOR = 0x21,
      /// <summary>
      /// Process Key
      /// </summary>
      VK_PROCESSKEY = 0xE5,
      /// <summary>
      /// Right Button
      /// </summary>
      VK_RBUTTON = 0x2,
      /// <summary>
      /// Right Control
      /// </summary>
      VK_RCONTROL = 0xA3,
      /// <summary>
      /// Return
      /// </summary>
      VK_RETURN = 0xD,
      /// <summary>
      /// Right
      /// </summary>
      VK_RIGHT = 0x27,
      /// <summary>
      /// Right Menu
      /// </summary>
      VK_RMENU = 0xA5,
      /// <summary>
      /// Right Shift
      /// </summary>
      VK_RSHIFT = 0xA1,
      /// <summary>
      /// Right Windows Key
      /// </summary>
      VK_RWIN = 0x5C,
      /// <summary>
      /// Scroll Lock
      /// </summary>
      VK_SCROLL = 0x91,
      /// <summary>
      /// Select
      /// </summary>
      VK_SELECT = 0x29,
      /// <summary>
      /// Separator
      /// </summary>
      VK_SEPARATOR = 0x6C,
      /// <summary>
      /// Shift
      /// </summary>
      VK_SHIFT = 0x10,
      /// <summary>
      /// Snapshot
      /// </summary>
      VK_SNAPSHOT = 0x2C,
      /// <summary>
      /// Space
      /// </summary>
      VK_SPACE = 0x20,
      /// <summary>
      /// Subtract
      /// </summary>
      VK_SUBTRACT = 0x6D,
      /// <summary>
      /// Tab
      /// </summary>
      VK_TAB = 0x9,
      /// <summary>
      /// Up
      /// </summary>
      VK_UP = 0x26,
      /// <summary>
      /// Zoom
      /// </summary>
      VK_ZOOM = 0xFB,
    }

    /// <summary>
    /// Key Event Types
    /// </summary>
    [Flags]
    public enum KeyEvents
    {
      /// <summary>
      /// Key is Down.
      /// </summary>
      KeyDown     = 0,
      /// <summary>
      /// Key is an Extended Key.
      /// </summary>
      ExtendedKey = 1,
      /// <summary>
      /// Key is Up.
      /// </summary>
      KeyUp       = 2,
      /// <summary>
      /// Key is Unicode.
      /// </summary>
      Unicode     = 4,
      /// <summary>
      /// Key is Scan Code.
      /// </summary>
      ScanCode    = 8,
    }

    #endregion Enumerations

    #region Public Methods

    /// <summary>
    /// Simulate a key being pressed down.
    /// </summary>
    /// <param name="vKey">Virtual key to press.</param>
    public static void KeyDown(VKey vKey)
    {
      keybd_event((byte)vKey, 0, (uint)KeyEvents.KeyDown, IntPtr.Zero);
    }

    /// <summary>
    /// Simulate a key being released.
    /// </summary>
    /// <param name="vKey">Virtual key to release.</param>
    public static void KeyUp(VKey vKey)
    {
      keybd_event((byte)vKey, 0, (uint)KeyEvents.KeyUp, IntPtr.Zero);
    }

    /// <summary>
    /// Simulate a Virtual Key event.
    /// </summary>
    /// <param name="vKey">Virtual Key.</param>
    /// <param name="scan">Scan code.</param>
    /// <param name="flags">Event type.</param>
    /// <param name="extraInfo">Pointer to additional information.</param>
    public static void Event(VKey vKey, byte scan, KeyEvents flags, IntPtr extraInfo)
    {
      keybd_event((byte)vKey, scan, (uint)flags, extraInfo);
    }

    /// <summary>
    /// Simulate a Virtual Key event.
    /// </summary>
    /// <param name="vKey">Virtual key code.</param>
    /// <param name="scan">Scan code.</param>
    /// <param name="flags">Event type.</param>
    /// <param name="extraInfo">Pointer to additional information.</param>
    public static void Event(byte vKey, byte scan, KeyEvents flags, IntPtr extraInfo)
    {
      keybd_event(vKey, scan, (uint)flags, extraInfo);
    }


    public static void Process(string keystrokes)
    {
      if (String.IsNullOrEmpty(keystrokes))
        throw new ArgumentNullException("keystrokes");



      for (int index = 0; index < keystrokes.Length; index++)
      {
        char curChar = keystrokes[index];

        short keyScan = VkKeyScan(curChar);

        byte keyCode = (byte)curChar;        
        byte scanCode = 0;
        
        bool isExtended = false;

        if (isExtended)
        {
          Event(keyCode, scanCode, KeyEvents.ExtendedKey | KeyEvents.KeyDown, IntPtr.Zero);
          Event(keyCode, scanCode, KeyEvents.ExtendedKey | KeyEvents.KeyUp, IntPtr.Zero);
        }
        else
        {
          Event(keyCode, scanCode, KeyEvents.KeyDown, IntPtr.Zero);
          Event(keyCode, scanCode, KeyEvents.KeyUp, IntPtr.Zero);
        }
      }


    }

    #endregion Public Methods

  }

}
