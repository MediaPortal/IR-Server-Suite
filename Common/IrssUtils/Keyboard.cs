using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace IrssUtils
{

  /// <summary>
  /// Win32 native method wrapper for Keyboard control functions.
  /// http://msdn2.microsoft.com/en-us/library/ms646304.aspx
  /// </summary>
  public static class Keyboard
  {

    #region Constants

    public const string CommandPause      = "PAUSE=";
    public const string CommandBeep       = "BEEP=";
    public const string CommandBraceOpen  = "OPENBRACE";
    public const string CommandBraceClose = "CLOSEBRACE";

    public const char ModifierAlt     = '%';
    public const char ModifierControl = '^';
    public const char ModifierShift   = '+';
    public const char ModifierWinKey  = '@';

    public const char BracketOpen     = '(';
    public const char BracketClose    = ')';

    public const char BraceOpen       = '{';
    public const char BraceClose      = '}';

    public const char EnterShortcut   = '~';

    const string PrefixVK = "VK_";

    #endregion Constants

    #region Interop

    [DllImport("user32.dll")]
    static extern void keybd_event(
      byte vk,
      byte scan,
      uint flags,
      IntPtr extraInfo);

    [DllImport("user32.dll")]
    static extern short VkKeyScan(
      char ch);

    [DllImport("user32.dll")]
    static extern uint MapVirtualKey(
      uint code,
      uint mapType);

    [DllImport("Kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool Beep(
      uint frequency,
      uint duration);

    #endregion Interop

    #region Enumerations

    /// <summary>
    /// Virtual key modifiers.
    /// </summary>
    [Flags]
    public enum KeyModifiers
    {
      /// <summary>
      /// No modifier.
      /// </summary>
      None      = 0x00,
      /// <summary>
      /// Shift.
      /// </summary>
      Shift     = 0x01,
      /// <summary>
      /// Control.
      /// </summary>
      Ctrl      = 0x02,
      /// <summary>
      /// Alt
      /// </summary>
      Alt       = 0x04,
      
      //Hankaku   = 0x08,
      //Reserved1 = 0x10,
      //Reserved2 = 0x20,
    }

    /// <summary>
    /// Virtual Key Codes.
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
      /// Alt (VK_MENU)
      /// </summary>
      VK_ALT = VK_MENU,
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
      VK_BACK = 0x08,
      /// <summary>
      /// Backspace (VK_BACK)
      /// </summary>
      VK_BACKSPACE = VK_BACK,
      /// <summary>
      /// Bell
      /// </summary>
      VK_BELL = 0x07,
      /// <summary>
      /// Backspace (VK_BACK)
      /// </summary>
      VK_BKSP = VK_BACK,
      /// <summary>
      /// Break
      /// </summary>
      VK_BREAK = 0x03,
      /// <summary>
      /// Backspace (VK_BACK)
      /// </summary>
      VK_BS = VK_BACK,
      /// <summary>
      /// Cancel
      /// </summary>
      VK_CANCEL = 0x03,
      /// <summary>
      /// Captial
      /// </summary>
      VK_CAPITAL = 0x14,
      /// <summary>
      /// Clear
      /// </summary>
      VK_CLEAR = 0x0C,
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
      /// Delete (VK_DELETE)
      /// </summary>
      VK_DEL = VK_DELETE,
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
      /// Enter (VK_RETURN)
      /// </summary>
      VK_ENTER = VK_RETURN,
      /// <summary>
      /// Ereof
      /// </summary>
      VK_EREOF = 0xF9,
      /// <summary>
      /// Escape (VK_ESCAPE)
      /// </summary>
      VK_ESC = VK_ESCAPE,
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
      /// Insert (VK_INSERT)
      /// </summary>
      VK_INS = VK_INSERT,
      /// <summary>
      /// Left Button
      /// </summary>
      VK_LBUTTON = 0x01,
      /// <summary>
      /// Left Control
      /// </summary>
      VK_LCONTROL = 0xA2,
      /// <summary>
      /// Left
      /// </summary>
      VK_LEFT = 0x25,
      /// <summary>
      /// Linefeed
      /// </summary>
      VK_LINEFEED = 0x0A,
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
      VK_MBUTTON = 0x04,
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
      /// Num Lock
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
      /// Print Screen (VK_SNAPSHOT)
      /// </summary>
      VK_PRTSC = VK_SNAPSHOT,
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
      VK_RETURN = 0x0D,
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
      /// Scroll Lock (VK_SCROLL)
      /// </summary>
      VK_SCROLLLOCK = VK_SCROLL,
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
      /// Windows Key (VK_LWIN)
      /// </summary>
      VK_WIN = VK_LWIN,
      /// <summary>
      /// Zoom
      /// </summary>
      VK_ZOOM = 0xFB,
    }

    /// <summary>
    /// Key Event Types.
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
    /// <param name="vKey">Virtual key to simulate.</param>
    public static void KeyDown(VKey vKey)
    {
      bool isExtended = IsExtendedKey(vKey);

      uint scanCode = MapVirtualKey((uint)vKey, 0);

      if (isExtended)
        keybd_event((byte)vKey, (byte)scanCode, (uint)KeyEvents.KeyDown | (uint)KeyEvents.ExtendedKey, IntPtr.Zero);
      else
        keybd_event((byte)vKey, (byte)scanCode, (uint)KeyEvents.KeyDown, IntPtr.Zero);
    }

    /// <summary>
    /// Simulate a key being released.
    /// </summary>
    /// <param name="vKey">Virtual key to simulate.</param>
    public static void KeyUp(VKey vKey)
    {
      bool isExtended = IsExtendedKey(vKey);

      uint scanCode = MapVirtualKey((uint)vKey, 0);

      if (isExtended)
        keybd_event((byte)vKey, (byte)scanCode, (uint)KeyEvents.KeyUp | (uint)KeyEvents.ExtendedKey, IntPtr.Zero);
      else
        keybd_event((byte)vKey, (byte)scanCode, (uint)KeyEvents.KeyUp, IntPtr.Zero);
    }

    /// <summary>
    /// Simulate a key being pressed down and immediately released.
    /// </summary>
    /// <param name="vKey">Virtual key to simulate.</param>
    public static void KeyPress(VKey vKey)
    {
      bool isExtended = IsExtendedKey(vKey);

      uint scanCode = MapVirtualKey((uint)vKey, 0);

      if (isExtended)
      {
        keybd_event((byte)vKey, (byte)scanCode, (uint)KeyEvents.KeyDown | (uint)KeyEvents.ExtendedKey, IntPtr.Zero);
        keybd_event((byte)vKey, (byte)scanCode, (uint)KeyEvents.KeyUp | (uint)KeyEvents.ExtendedKey, IntPtr.Zero);
      }
      else
      {
        keybd_event((byte)vKey, (byte)scanCode, (uint)KeyEvents.KeyDown, IntPtr.Zero);
        keybd_event((byte)vKey, (byte)scanCode, (uint)KeyEvents.KeyUp, IntPtr.Zero);
      }
    }
    
    /// <summary>
    /// Simulate a key being pressed down and immediately released.
    /// </summary>
    /// <param name="ch">Key character to simulate.</param>
    public static void KeyPress(char ch)
    {
      short virtualKeyCode = VkKeyScan(ch);

      byte modifiers = Win32.HighByte(virtualKeyCode);

      VKey vKey = (VKey)Win32.LowByte(virtualKeyCode);

      if (modifiers == 0)
        KeyPress(vKey);
      else
        KeyPress(vKey, modifiers);
    }

    /// <summary>
    /// Simulate a key being pressed down and immediately released.
    /// </summary>
    /// <param name="vKey">Virtual key to simulate.</param>
    /// <param name="modifiers">Key modifiers in effect.</param>
    public static void KeyPress(VKey vKey, byte modifiers)
    {
      bool alt    = Win32.CheckMask(modifiers, (byte)KeyModifiers.Alt);
      bool ctrl   = Win32.CheckMask(modifiers, (byte)KeyModifiers.Ctrl);
      bool shift  = Win32.CheckMask(modifiers, (byte)KeyModifiers.Shift);

      KeyPress(vKey, alt, ctrl, shift, false);
    }

    /// <summary>
    /// Simulate a key being pressed down and immediately released.
    /// </summary>
    /// <param name="vKey">Virtual key to simulate.</param>
    /// <param name="alt">Simulate alt key.</param>
    /// <param name="ctrl">Simulate control key.</param>
    /// <param name="shift">Simulate shift key.</param>
    /// <param name="winKey">Simulate windows key.</param>
    public static void KeyPress(VKey vKey, bool alt, bool ctrl, bool shift, bool winKey)
    {
      if (alt)        KeyDown(VKey.VK_MENU);
      if (ctrl)       KeyDown(VKey.VK_CONTROL);
      if (shift)      KeyDown(VKey.VK_SHIFT);
      if (winKey)     KeyDown(VKey.VK_LWIN);

      KeyPress(vKey);

      if (winKey)     KeyUp(VKey.VK_LWIN);
      if (shift)      KeyUp(VKey.VK_SHIFT);
      if (ctrl)       KeyUp(VKey.VK_CONTROL);
      if (alt)        KeyUp(VKey.VK_MENU);
    }

    /// <summary>
    /// Determines if a given virtual key is an extended key.
    /// </summary>
    /// <param name="vKey">Virtual key in question.</param>
    /// <returns>true if the virtual key is an extended key, otherwise false.</returns>
    public static bool IsExtendedKey(VKey vKey)
    {
      switch (vKey)
      {
        case VKey.VK_UP:
        case VKey.VK_DOWN:
        case VKey.VK_LEFT:
        case VKey.VK_RIGHT:
        case VKey.VK_HOME:
        case VKey.VK_END:
        case VKey.VK_PRIOR:
        case VKey.VK_NEXT:
        case VKey.VK_INSERT:
        case VKey.VK_DELETE:
          return true;

        default:
          return false;
      }
    }

    /// <summary>
    /// Process a keystroke command sequence.
    /// </summary>
    /// <param name="keystrokes">The keystroke command sequence.</param>
    public static void ProcessCommand(string keystrokes)
    {
      if (String.IsNullOrEmpty(keystrokes))
        throw new ArgumentNullException("keystrokes");

      bool inBrackets = false;

      // TODO: Keep track of opening and closing brackets ...

      //List<char> bracketModStack = new List<char>();

      
      bool Shift      = false;
      bool Alt        = false;
      bool Ctrl       = false;
      bool WinKey     = false;

      for (int index = 0; index < keystrokes.Length; index++)
      {
        char ch = keystrokes[index];

        switch (ch)
        {
          case BraceOpen:
            {
              int endBrace = keystrokes.IndexOf(BraceClose, index);

              if (endBrace == -1)
                throw new ArgumentException("Missing closing brace \"}\" after position " + index.ToString(), "keystrokes");

              index++;

              int length = endBrace - index;
              if (length < 1)
                throw new ArgumentException("Invalid braced command \"{}\"", "keystrokes");

              string special = keystrokes.Substring(index, length);

              if (special.Equals(ModifierAlt.ToString(), StringComparison.Ordinal))               KeyPress(ModifierAlt);
              else if (special.Equals(ModifierControl.ToString(), StringComparison.Ordinal))      KeyPress(ModifierControl);
              else if (special.Equals(ModifierShift.ToString(), StringComparison.Ordinal))        KeyPress(ModifierShift);
              else if (special.Equals(ModifierWinKey.ToString(), StringComparison.Ordinal))       KeyPress(ModifierWinKey);
              else if (special.Equals(BracketOpen.ToString(), StringComparison.Ordinal))          KeyPress(BracketOpen);
              else if (special.Equals(BracketClose.ToString(), StringComparison.Ordinal))         KeyPress(BracketClose);
              else if (special.Equals(EnterShortcut.ToString(), StringComparison.Ordinal))        KeyPress(EnterShortcut);
              else if (special.Equals(CommandBraceOpen, StringComparison.OrdinalIgnoreCase))      KeyPress(BraceOpen);
              else if (special.Equals(CommandBraceClose, StringComparison.OrdinalIgnoreCase))     KeyPress(BraceClose);
              else if (special.StartsWith(CommandPause, StringComparison.OrdinalIgnoreCase))
              {
                string pauseString = special.Substring(CommandPause.Length);
                if (String.IsNullOrEmpty(pauseString))
                  throw new ArgumentException("Invalid pause command: " + special, "keystrokes");

                int time = int.Parse(pauseString);

                Thread.Sleep(time);
              }
              else if (special.StartsWith(CommandBeep, StringComparison.OrdinalIgnoreCase))
              {
                string beepString = special.Substring(CommandPause.Length);
                if (String.IsNullOrEmpty(beepString))
                  throw new ArgumentException("Invalid beep command: " + special, "keystrokes");

                string[] parameters = beepString.Split(new char[] { ',' });
                if (parameters.Length != 2)
                  throw new ArgumentException("Invalid beep command structure: " + special, "keystrokes");
                
                uint beepFreq = uint.Parse(parameters[0]);
                uint beepTime = uint.Parse(parameters[1]);

                Beep(beepFreq, beepTime);
              }
              else
              {
                int count = 1;

                int space = special.IndexOf(' ');
                if (space != -1)
                {
                  string countString = special.Substring(space);
                  if (!String.IsNullOrEmpty(countString))
                    count = int.Parse(countString);
                }

                VKey vKey = VKey.None;
                try
                {
                  if (special.StartsWith(PrefixVK, StringComparison.OrdinalIgnoreCase))
                    vKey = (VKey)Enum.Parse(typeof(VKey), special, true);
                  else
                    vKey = (VKey)Enum.Parse(typeof(VKey), PrefixVK + special, true);
                }
                catch (Exception ex)
                {
                  throw new ArgumentException("Invalid virtual key code \"" + special + "\"", "keystrokes", ex);
                }

                for (int repeat = 0; repeat < count; repeat++)
                  KeyPress(vKey);

                if (!inBrackets)
                {
                  UndoModifiers(Alt, Ctrl, Shift, WinKey);
                  Alt = false;
                  Ctrl = false;
                  Shift = false;
                  WinKey = false;
                }
              }

              index = endBrace;
              break;
            }

          case BracketOpen:
            {
              inBrackets = true;
              break;
            }

          case BracketClose:
            {
              inBrackets = false;

              if (!inBrackets)
              {
                UndoModifiers(Alt, Ctrl, Shift, WinKey);
                Alt     = false;
                Ctrl    = false;
                Shift   = false;
                WinKey  = false;
              }
              break;
            }

          case ModifierShift:
            {
              if (!Shift)
              {
                KeyDown(VKey.VK_SHIFT);
                Shift = true;
              }
              break;
            }

          case ModifierControl:
            {
              if (!Ctrl)
              {
                KeyDown(VKey.VK_CONTROL);
                Ctrl = true;
              }
              break;
            }

          case ModifierAlt:
            {
              if (!Alt)
              {
                KeyDown(VKey.VK_MENU);
                Alt = true;
              }
              break;
            }

          case ModifierWinKey:
            {
              if (!WinKey)
              {
                KeyDown(VKey.VK_LWIN);
                WinKey = true;
              }
              break;
            }

          case EnterShortcut:
            {
              KeyPress(VKey.VK_ENTER);

              if (!inBrackets)
              {
                UndoModifiers(Alt, Ctrl, Shift, WinKey);
                Alt     = false;
                Ctrl    = false;
                Shift   = false;
                WinKey  = false;
              }
              break;
            }

          default:
            {
              KeyPress(ch);

              if (!inBrackets)
              {
                UndoModifiers(Alt, Ctrl, Shift, WinKey);
                Alt     = false;
                Ctrl    = false;
                Shift   = false;
                WinKey  = false;
              }
              break;
            }
        }

      }

      UndoModifiers(Alt, Ctrl, Shift, WinKey);
    }

    /// <summary>
    /// Performs a "Key Up" action on any currently held modifiers.
    /// </summary>
    /// <param name="alt">Alt key is pressed.</param>
    /// <param name="ctrl">Control key is pressed.</param>
    /// <param name="shift">Shift key is pressed.</param>
    /// <param name="winKey">Windows key is pressed.</param>
    static void UndoModifiers(bool alt, bool ctrl, bool shift, bool winKey)
    {
      if (alt)    KeyUp(VKey.VK_MENU);
      if (ctrl)   KeyUp(VKey.VK_CONTROL);
      if (shift)  KeyUp(VKey.VK_SHIFT);
      if (winKey) KeyUp(VKey.VK_LWIN);
    }

    #endregion Public Methods

  }

}
