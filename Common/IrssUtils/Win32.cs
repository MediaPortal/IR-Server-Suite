using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security;

namespace IrssUtils
{
  
  /// <summary>
  /// Win32 native method class.
  /// </summary>
  public static class Win32
  {

    #region Constants

    /// <summary>
    /// Windows Message (USER).
    /// </summary>
    public const int WM_USER  = 0x0400;

    /// <summary>
    /// Windows Message (APP).
    /// </summary>
    public const int WM_APP   = 0x8000;

    #endregion Constants

    #region Interop

    [DllImport("user32")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32", SetLastError = false)]
    public static extern IntPtr SendMessage(IntPtr windowHandle, int msg, IntPtr wordParam, IntPtr longParam);

    [DllImport("user32", SetLastError = true)]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

    #region Net API

    [DllImport("netapi32", CharSet = CharSet.Auto, SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
    static extern int NetServerEnum(
      string ServerNane, // must be null
      int dwLevel,
      ref IntPtr pBuf,
      int dwPrefMaxLen,
      out int dwEntriesRead,
      out int dwTotalEntries,
      int dwServerType,
      string domain, // null for login domain
      out int dwResumeHandle
    );

    [DllImport("netapi32", SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
    static extern int NetApiBufferFree(IntPtr pBuf);

    [StructLayout(LayoutKind.Sequential)]
    struct _SERVER_INFO_100
    {
      public int sv100_platform_id;
      [MarshalAs(UnmanagedType.LPWStr)]
      public string sv100_name;
    }

    #endregion Net API

    #endregion Interop

    #region Methods

    /// <summary>
    /// Given a 32-bit integer this method returns the High Word (upper 16 bits).
    /// </summary>
    /// <param name="n">32-bit integer.</param>
    /// <returns>Upper 16 bits or source 32-bit integer.</returns>
    public static Int16 HighWord(Int32 n)
    {
      return (Int16)((n >> 16) & 0xffff);
    }

    /// <summary>
    /// Given a 32-bit integer this method returns the Low Word (lower 16 bits).
    /// </summary>
    /// <param name="n">32-bit integer.</param>
    /// <returns>Lower 16 bits or source 32-bit integer.</returns>
    public static Int16 LowWord(Int32 n)
    {
      return (Int16)(n & 0xffff);
    }

    /// <summary>
    /// Get a list of all computer names on the LAN.
    /// </summary>
    /// <returns>List of LAN computer names.</returns>
    public static ArrayList GetNetworkComputers()
    {
      try
      {
        ArrayList networkComputers = new ArrayList();

        const int MAX_PREFERRED_LENGTH = -1;

        int SV_TYPE_WORKSTATION = 1;
        int SV_TYPE_SERVER = 2;
        IntPtr buffer = IntPtr.Zero;
        IntPtr tmpBuffer = IntPtr.Zero;
        int entriesRead = 0;
        int totalEntries = 0;
        int resHandle = 0;
        int sizeofINFO = Marshal.SizeOf(typeof(_SERVER_INFO_100));

        int ret = NetServerEnum(
          null,
          100,
          ref buffer,
          MAX_PREFERRED_LENGTH,
          out entriesRead,
          out totalEntries,
          SV_TYPE_WORKSTATION | SV_TYPE_SERVER,
          null,
          out resHandle);

        if (ret == 0)
        {
          for (int i = 0; i < totalEntries; i++)
          {
            tmpBuffer = new IntPtr((int)buffer + (i * sizeofINFO));
            _SERVER_INFO_100 svrInfo = (_SERVER_INFO_100)Marshal.PtrToStructure(tmpBuffer, typeof(_SERVER_INFO_100));

            networkComputers.Add(svrInfo.sv100_name);
          }
        }

        NetApiBufferFree(buffer);

        return networkComputers;
      }
      catch
      {
        return null;
      }      
    }

    #endregion Methods

  }

}
