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
using System.Runtime.InteropServices;

namespace IRServer.Plugin
{
  /// <summary>
  /// Win32 native method wrapper for Keyboard control functions.
  /// </summary>
  internal static class Keyboard
  {
    #region Interop

    [DllImport("user32.dll")]
    private static extern void keybd_event(
      byte bVk,
      byte bScan,
      uint dwFlags,
      IntPtr dwExtraInfo);

    #endregion Interop

    #region Enumerations

    #region KeyEvents enum

    /// <summary>
    /// Key Event Types.
    /// </summary>
    [Flags]
    public enum KeyEvents
    {
      KeyDown = 0x00,
      ExtendedKey = 0x01,
      KeyUp = 0x02,
      Unicode = 0x04,
      ScanCode = 0x08,
    }

    #endregion

    #region VKey enum

    /// <summary>
    /// Virtual Key Codes.
    /// </summary>
    public enum VKey
    {
      None = 0,
      VK_0 = 0x30,
      VK_1 = 0x31,
      VK_2 = 0x32,
      VK_3 = 0x33,
      VK_4 = 0x34,
      VK_5 = 0x35,
      VK_6 = 0x36,
      VK_7 = 0x37,
      VK_8 = 0x38,
      VK_9 = 0x39,
      VK_A = 0x41,
      VK_B = 0x42,
      VK_C = 0x43,
      VK_D = 0x44,
      VK_E = 0x45,
      VK_F = 0x46,
      VK_G = 0x47,
      VK_H = 0x48,
      VK_I = 0x49,
      VK_J = 0x4A,
      VK_K = 0x4B,
      VK_L = 0x4C,
      VK_M = 0x4D,
      VK_N = 0x4E,
      VK_O = 0x4F,
      VK_P = 0x50,
      VK_Q = 0x51,
      VK_R = 0x52,
      VK_S = 0x53,
      VK_T = 0x54,
      VK_U = 0x55,
      VK_V = 0x56,
      VK_W = 0x57,
      VK_X = 0x58,
      VK_Y = 0x59,
      VK_Z = 0x5A,
      VK_ADD = 0x6B,
      VK_APPS = 0x5D,
      VK_ATTN = 0xF6,
      VK_BACK = 0x8,
      VK_CANCEL = 0x3,
      VK_CAPITAL = 0x14,
      VK_CLEAR = 0xC,
      VK_CONTROL = 0x11,
      VK_CRSEL = 0xF7,
      VK_DECIMAL = 0x6E,
      VK_DELETE = 0x2E,
      VK_DIVIDE = 0x6F,
      VK_DOWN = 0x28,
      VK_END = 0x23,
      VK_EREOF = 0xF9,
      VK_ESCAPE = 0x1B,
      VK_EXECUTE = 0x2B,
      VK_EXSEL = 0xF8,
      VK_F1 = 0x70,
      VK_F2 = 0x71,
      VK_F3 = 0x72,
      VK_F4 = 0x73,
      VK_F5 = 0x74,
      VK_F6 = 0x75,
      VK_F7 = 0x76,
      VK_F8 = 0x77,
      VK_F9 = 0x78,
      VK_F10 = 0x79,
      VK_F11 = 0x7A,
      VK_F12 = 0x7B,
      VK_F13 = 0x7C,
      VK_F14 = 0x7D,
      VK_F15 = 0x7E,
      VK_F16 = 0x7F,
      VK_F17 = 0x80,
      VK_F18 = 0x81,
      VK_F19 = 0x82,
      VK_F20 = 0x83,
      VK_F21 = 0x84,
      VK_F22 = 0x85,
      VK_F23 = 0x86,
      VK_F24 = 0x87,
      VK_HELP = 0x2F,
      VK_HOME = 0x24,
      VK_INSERT = 0x2D,
      VK_LBUTTON = 0x1,
      VK_LCONTROL = 0xA2,
      VK_LEFT = 0x25,
      VK_LMENU = 0xA4,
      VK_LSHIFT = 0xA0,
      VK_LWIN = 0x5B,
      VK_MBUTTON = 0x4,
      VK_MENU = 0x12,
      VK_MULTIPLY = 0x6A,
      VK_NEXT = 0x22,
      VK_NONAME = 0xFC,
      VK_NUMLOCK = 0x90,
      VK_NUMPAD0 = 0x60,
      VK_NUMPAD1 = 0x61,
      VK_NUMPAD2 = 0x62,
      VK_NUMPAD3 = 0x63,
      VK_NUMPAD4 = 0x64,
      VK_NUMPAD5 = 0x65,
      VK_NUMPAD6 = 0x66,
      VK_NUMPAD7 = 0x67,
      VK_NUMPAD8 = 0x68,
      VK_NUMPAD9 = 0x69,
      VK_OEM_1 = 0xBA, // ";:"
      VK_OEM_2 = 0xBF, // "/?"
      VK_OEM_3 = 0xC0, // "`~" for US
      VK_OEM_4 = 0xDB, //	"[{" for US
      VK_OEM_5 = 0xDC, //	"\|" for US
      VK_OEM_6 = 0xDD, //	"]}" for US
      VK_OEM_7 = 0xDE, //	"'"" for US
      VK_OEM_102 = 0xE2,
      VK_OEM_CLEAR = 0xFE,
      VK_OEM_COMMA = 0xBC,
      VK_OEM_MINUS = 0xBD,
      VK_OEM_PERIOD = 0xBE,
      VK_OEM_PLUS = 0xBB, // "+="
      VK_PA1 = 0xFD,
      VK_PAUSE = 0x13,
      VK_PLAY = 0xFA,
      VK_PRINT = 0x2A,
      VK_PRIOR = 0x21,
      VK_PROCESSKEY = 0xE5,
      VK_RBUTTON = 0x2,
      VK_RCONTROL = 0xA3,
      VK_RETURN = 0xD,
      VK_RIGHT = 0x27,
      VK_RMENU = 0xA5,
      VK_RSHIFT = 0xA1,
      VK_RWIN = 0x5C,
      VK_SCROLL = 0x91,
      VK_SELECT = 0x29,
      VK_SEPARATOR = 0x6C,
      VK_SHIFT = 0x10,
      VK_SNAPSHOT = 0x2C,
      VK_SPACE = 0x20,
      VK_SUBTRACT = 0x6D,
      VK_TAB = 0x9,
      VK_UP = 0x26,
      VK_ZOOM = 0xFB,
    }

    #endregion

    #endregion Enumerations

    #region Public Methods

    /// <summary>
    /// Simulate a key being pressed down.
    /// </summary>
    /// <param name="vKey">Virtual key to press.</param>
    public static void KeyDown(VKey vKey)
    {
      keybd_event((byte) vKey, 0, (uint) KeyEvents.KeyDown, IntPtr.Zero);
    }

    /// <summary>
    /// Simulate a key being released.
    /// </summary>
    /// <param name="vKey">Virtual key to release.</param>
    public static void KeyUp(VKey vKey)
    {
      keybd_event((byte) vKey, 0, (uint) KeyEvents.KeyUp, IntPtr.Zero);
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
      keybd_event((byte) vKey, scan, (uint) flags, extraInfo);
    }

    #endregion Public Methods
  }
}