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
using System.Windows.Forms;

namespace IrssUtils
{
  /// <summary>
  /// Win32 native method wrapper for Mouse control functions.
  /// </summary>
  public static class Mouse
  {
    #region Interop

    [DllImport("user32")]
    private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, IntPtr dwExtraInfo);

    #endregion Interop

    #region Enumerations

    #region MouseEvents enum

    /// <summary>
    /// Used to simulate mouse actions.
    /// </summary>
    [Flags]
    public enum MouseEvents
    {
      /// <summary>
      /// No Event.
      /// </summary>
      None = 0x0000,
      /// <summary>
      /// Move.
      /// </summary>
      Move = 0x0001,
      /// <summary>
      /// Left Button Down.
      /// </summary>
      LeftDown = 0x0002,
      /// <summary>
      /// Left Button Up.
      /// </summary>
      LeftUp = 0x0004,
      /// <summary>
      /// Right Button Down.
      /// </summary>
      RightDown = 0x0008,
      /// <summary>
      /// Right Button Up.
      /// </summary>
      RightUp = 0x0010,
      /// <summary>
      /// Middle Button Down.
      /// </summary>
      MiddleDown = 0x0020,
      /// <summary>
      /// Middle Button Up.
      /// </summary>
      MiddleUp = 0x0040,
      /// <summary>
      /// Scroll.
      /// </summary>
      Scroll = 0x0800,
      /// <summary>
      /// Position Absolute.
      /// </summary>
      Absolute = 0x8000
    }

    #endregion

    #region ScrollDir enum

    /// <summary>
    /// Used to simulate mouse wheel scrolling.
    /// </summary>
    public enum ScrollDir
    {
      /// <summary>
      /// No Scroll.
      /// </summary>
      None = 0,
      /// <summary>
      /// Scroll Up.
      /// </summary>
      Up = 120,
      /// <summary>
      /// Scroll Down.
      /// </summary>
      Down = -120
    }

    #endregion

    #endregion Enumerations

    #region Public Methods

    /// <summary>
    /// Simulates mouse button actions.
    /// </summary>
    /// <param name="flags">The button action to simulate.</param>
    public static void Button(MouseEvents flags)
    {
      mouse_event((int) flags, 0, 0, 0, IntPtr.Zero);
    }

    /// <summary>
    /// Simulate mouse movements.
    /// </summary>
    /// <param name="dx">Movement on the X axis.</param>
    /// <param name="dy">Movement on the Y axis.</param>
    /// <param name="absolute">If true, dx and dy are taken as absolute position.  If false, dx and dy are taken as relative to the current position.</param>
    public static void Move(int dx, int dy, bool absolute)
    {
      if (absolute)
      {
        int x = (int) (dx*(65536.0/Screen.PrimaryScreen.Bounds.Width));
        int y = (int) (dy*(65536.0/Screen.PrimaryScreen.Bounds.Height));

        mouse_event((int) (MouseEvents.Move | MouseEvents.Absolute), x, y, 0, IntPtr.Zero);
      }
      else
      {
        mouse_event((int) (MouseEvents.Move), dx, dy, 0, IntPtr.Zero);
      }
    }

    /// <summary>
    /// Simulates mouse wheel scrolling.
    /// </summary>
    /// <param name="direction">The direction to scroll.</param>
    public static void Scroll(ScrollDir direction)
    {
      mouse_event((int) MouseEvents.Scroll, 0, 0, (int) direction, IntPtr.Zero);
    }

    #endregion Public Methods
  }
}