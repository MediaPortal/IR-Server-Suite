using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace IrssUtils
{

  public static class CDRom
  {

    #region Constants

    const uint DRIVE_CDROM = 5;

    #endregion Constants

    #region Interop

    [DllImport("kernel32", SetLastError = true)]
    static extern int GetDriveType(string driveLetter);

    [DllImport("winmm.dll", EntryPoint = "mciSendStringA")]
    static extern void mciSendStringA(string lpstrCommand, string lpstrReturnString, long uReturnLength, long hwndCallback);

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
      string returnString = "";
      string command = String.Format("set cdaudio!{0} door open", driveLetter);
      mciSendStringA(command, returnString, 0, 0);
    }

    /// <summary>
    /// Close the tray on the given CD-Rom drive.
    /// </summary>
    /// <param name="driveLetter">Drive letter of CD-Rom to close.</param>
    public static void Close(string driveLetter)
    {
      string returnString = "";
      string command = String.Format("set cdaudio!{0} door closed", driveLetter);
      mciSendStringA(command, returnString, 0, 0);
    }

    #endregion Static Methods

  }

}
