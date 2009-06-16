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
  /// IR Server callback for remote button press handling.
  /// </summary>
  /// <param name="deviceName">The device that detected the remote button.</param>
  /// <param name="keyCode">Remote button code.</param>
  public delegate void RemoteHandler(string deviceName, string keyCode);

  #endregion Delegates

  /// <summary>
  /// Plugins that implement this interface can receive remote control button presses.
  /// </summary>
  public interface IRemoteReceiver
  {
    /// <summary>
    /// Callback for remote button presses.
    /// </summary>
    /// <value>The remote callback.</value>
    RemoteHandler RemoteCallback { get; set; }
  }
}