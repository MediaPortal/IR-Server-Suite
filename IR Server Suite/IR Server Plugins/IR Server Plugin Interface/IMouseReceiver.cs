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

namespace InputService.Plugin
{

  #region Delegates

  /// <summary>
  /// IR Server callback for mouse event handling.
  /// </summary>
  /// <param name="deviceName">The device that detected the mouse event.</param>
  /// <param name="deltaX">Mouse movement on the X-axis.</param>
  /// <param name="deltaY">Mouse movement on the Y-axis.</param>
  /// <param name="buttons">Which (if any) buttons are pressed?</param>
  public delegate void MouseHandler(string deviceName, int deltaX, int deltaY, int buttons);

  #endregion Delegates

  /// <summary>
  /// Plugins that implement this interface can receive mouse movements and button presses.
  /// </summary>
  public interface IMouseReceiver
  {
    /// <summary>
    /// Callback for mouse events.
    /// </summary>
    /// <value>The mouse callback.</value>
    MouseHandler MouseCallback { get; set; }
  }
}