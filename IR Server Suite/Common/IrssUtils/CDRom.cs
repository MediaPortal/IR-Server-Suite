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

namespace IrssUtils
{
  /// <summary>
  /// Access to the CDRom door.
  /// </summary>
  public static class CDRom
  {
    #region Constants

    private const uint DRIVE_CDROM = 5;

    #endregion Constants

    #region Interop

    [DllImport("kernel32", SetLastError = true)]
    private static extern int GetDriveType(string driveLetter);

    [DllImport("winmm.dll", EntryPoint = "mciSendStringA")]
    private static extern void mciSendStringA(string command, string returnString, int returnLength, int callback);

    #endregion Interop

    #region Static Methods

    /// <summary>
    /// Given a drive letter this function returns true if it is a CD-Rom.
    /// </summary>
    /// <param name="driveLetter">Drive letter to test.</param>
    /// <returns>True if the drive is a CD-Rom, else false.</returns>
    public static bool IsCDRom(string driveLetter)
    {
      return (GetDriveType(driveLetter) == DRIVE_CDROM);
    }

    /// <summary>
    /// Open the tray on the given CD-Rom drive.
    /// </summary>
    /// <param name="driveLetter">Drive letter of CD-Rom to open.</param>
    public static void Open(string driveLetter)
    {
      string command = String.Format("set cdaudio!{0} door open", driveLetter);

      string returnString = String.Empty;
      mciSendStringA(command, returnString, 0, 0);
    }

    /// <summary>
    /// Close the tray on the given CD-Rom drive.
    /// </summary>
    /// <param name="driveLetter">Drive letter of CD-Rom to close.</param>
    public static void Close(string driveLetter)
    {
      string command = String.Format("set cdaudio!{0} door closed", driveLetter);

      string returnString = String.Empty;
      mciSendStringA(command, returnString, 0, 0);
    }

    #endregion Static Methods
  }
}