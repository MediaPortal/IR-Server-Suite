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

namespace WiimoteLib
{
  /// <summary>
  /// Event to handle a state change on the Wiimote
  /// </summary>
  /// <param name="sender">Object sending the event</param>
  /// <param name="args">Current Wiimote state</param>
  public delegate void WiimoteChangedEventHandler(object sender, WiimoteChangedEventArgs args);

  /// <summary>
  /// Event to handle insertion/removal of an extension (Nunchuk/Classic Controller)
  /// </summary>
  /// <param name="sender">Object sending the event</param>
  /// <param name="args">Current extension status</param>
  public delegate void WiimoteExtensionChangedEventHandler(object sender, WiimoteExtensionChangedEventArgs args);

  /// <summary>
  /// Argument sent through the WiimoteExtensionChangedEvent
  /// </summary>
  public class WiimoteExtensionChangedEventArgs : EventArgs
  {
    /// <summary>
    /// The extenstion type inserted or removed
    /// </summary>
    public ExtensionType ExtensionType;

    /// <summary>
    /// Whether the extension was inserted or removed
    /// </summary>
    public bool Inserted;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="type">The extension type inserted or removed</param>
    /// <param name="inserted">Whether the extension was inserted or removed</param>
    public WiimoteExtensionChangedEventArgs(ExtensionType type, bool inserted)
    {
      ExtensionType = type;
      Inserted = inserted;
    }
  }

  /// <summary>
  /// Argument sent through the WiimoteChangedEvent
  /// </summary>
  public class WiimoteChangedEventArgs : EventArgs
  {
    /// <summary>
    /// The current state of the Wiimote and extension controllers
    /// </summary>
    public WiimoteState WiimoteState;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="ws">Wiimote state</param>
    public WiimoteChangedEventArgs(WiimoteState ws)
    {
      WiimoteState = ws;
    }
  }
}