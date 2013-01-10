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

namespace IRServer.Plugin
{

  #region Delegates

  /// <summary>
  /// IR Server callback for keyboard key press handling.
  /// </summary>
  /// <param name="deviceName">The device that detected the key event.</param>
  /// <param name="vKey">Virtual key code.</param>
  /// <param name="keyUp">Is this key coming up.</param>
  public delegate void KeyboardHandler(string deviceName, int vKey, bool keyUp);

  #endregion Delegates

  /// <summary>
  /// Plugins that implement this interface can receive keyboard button presses.
  /// </summary>
  public interface IKeyboardReceiver
  {
    /// <summary>
    /// Callback for keyboard presses.
    /// </summary>
    /// <value>The keyboard callback.</value>
    KeyboardHandler KeyboardCallback { get; set; }
  }
}