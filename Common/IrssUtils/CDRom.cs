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

    public static bool IsCDRom(string driveLetter)
    {
      return (GetDriveType(driveLetter) == DRIVE_CDROM);
    }

    public static void Open(string driveLetter)
    {
      string returnString = "";
      string command = String.Format("set cdaudio!{0} door open", driveLetter);
      mciSendStringA(command, returnString, 0, 0);
    }

    public static void Close(string driveLetter)
    {
      string returnString = "";
      string command = String.Format("set cdaudio!{0} door closed", driveLetter);
      mciSendStringA(command, returnString, 0, 0);
    }

    #endregion Static Methods

  }

}
