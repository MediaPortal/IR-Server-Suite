using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace IrssUtils
{
  
  /// <summary>
  /// Win32 native method class.
  /// </summary>
  public static class Win32
  {

    #region Enumerations

    public enum WindowsMessage
    {
      WM_ACTIVATE = 0x6,
      WM_ACTIVATEAPP = 0x1C,
      WM_AFXFIRST = 0x360,
      WM_AFXLAST = 0x37F,
      WM_APP = 0x8000,
      WM_APPCOMMAND = 0x319,
      WM_ASKCBFORMATNAME = 0x30C,
      WM_CANCELJOURNAL = 0x4B,
      WM_CANCELMODE = 0x1F,
      WM_CAPTURECHANGED = 0x215,
      WM_CHANGECBCHAIN = 0x30D,
      WM_CHAR = 0x102,
      WM_CHARTOITEM = 0x2F,
      WM_CHILDACTIVATE = 0x22,
      WM_CLEAR = 0x303,
      WM_CLOSE = 0x10,
      WM_COMMAND = 0x111,
      WM_COMPACTING = 0x41,
      WM_COMPAREITEM = 0x39,
      WM_CONTEXTMENU = 0x7B,
      WM_COPY = 0x301,
      WM_COPYDATA = 0x4A,
      WM_CREATE = 0x1,
      WM_CTLCOLORBTN = 0x135,
      WM_CTLCOLORDLG = 0x136,
      WM_CTLCOLOREDIT = 0x133,
      WM_CTLCOLORLISTBOX = 0x134,
      WM_CTLCOLORMSGBOX = 0x132,
      WM_CTLCOLORSCROLLBAR = 0x137,
      WM_CTLCOLORSTATIC = 0x138,
      WM_CUT = 0x300,
      WM_DEADCHAR = 0x103,
      WM_DELETEITEM = 0x2D,
      WM_DESTROY = 0x2,
      WM_DESTROYCLIPBOARD = 0x307,
      WM_DEVICECHANGE = 0x219,
      WM_DEVMODECHANGE = 0x1B,
      WM_DISPLAYCHANGE = 0x7E,
      WM_DRAWCLIPBOARD = 0x308,
      WM_DRAWITEM = 0x2B,
      WM_DROPFILES = 0x233,
      WM_ENABLE = 0xA,
      WM_ENDSESSION = 0x16,
      WM_ENTERIDLE = 0x121,
      WM_ENTERMENULOOP = 0x211,
      WM_ENTERSIZEMOVE = 0x231,
      WM_ERASEBKGND = 0x14,
      WM_EXITMENULOOP = 0x212,
      WM_EXITSIZEMOVE = 0x232,
      WM_FONTCHANGE = 0x1D,
      WM_GETDLGCODE = 0x87,
      WM_GETFONT = 0x31,
      WM_GETHOTKEY = 0x33,
      WM_GETICON = 0x7F,
      WM_GETMINMAXINFO = 0x24,
      WM_GETOBJECT = 0x3D,
      WM_GETSYSMENU = 0x313,
      WM_GETTEXT = 0xD,
      WM_GETTEXTLENGTH = 0xE,
      WM_HANDHELDFIRST = 0x358,
      WM_HANDHELDLAST = 0x35F,
      WM_HELP = 0x53,
      WM_HOTKEY = 0x312,
      WM_HSCROLL = 0x114,
      WM_HSCROLLCLIPBOARD = 0x30E,
      WM_ICONERASEBKGND = 0x27,
      WM_IME_CHAR = 0x286,
      WM_IME_COMPOSITION = 0x10F,
      WM_IME_COMPOSITIONFULL = 0x284,
      WM_IME_CONTROL = 0x283,
      WM_IME_ENDCOMPOSITION = 0x10E,
      WM_IME_KEYDOWN = 0x290,
      WM_IME_KEYLAST = 0x10F,
      WM_IME_KEYUP = 0x291,
      WM_IME_NOTIFY = 0x282,
      WM_IME_REQUEST = 0x288,
      WM_IME_SELECT = 0x285,
      WM_IME_SETCONTEXT = 0x281,
      WM_IME_STARTCOMPOSITION = 0x10D,
      WM_INITDIALOG = 0x110,
      WM_INITMENU = 0x116,
      WM_INITMENUPOPUP = 0x117,
      WM_INPUTLANGCHANGE = 0x51,
      WM_INPUTLANGCHANGEREQUEST = 0x50,
      WM_KEYDOWN = 0x100,
      WM_KEYFIRST = 0x100,
      WM_KEYLAST = 0x108,
      WM_KEYUP = 0x101,
      WM_KILLFOCUS = 0x8,
      WM_LBUTTONDBLCLK = 0x203,
      WM_LBUTTONDOWN = 0x201,
      WM_LBUTTONUP = 0x202,
      WM_MBUTTONDBLCLK = 0x209,
      WM_MBUTTONDOWN = 0x207,
      WM_MBUTTONUP = 0x208,
      WM_MDIACTIVATE = 0x222,
      WM_MDICASCADE = 0x227,
      WM_MDICREATE = 0x220,
      WM_MDIDESTROY = 0x221,
      WM_MDIGETACTIVE = 0x229,
      WM_MDIICONARRANGE = 0x228,
      WM_MDIMAXIMIZE = 0x225,
      WM_MDINEXT = 0x224,
      WM_MDIREFRESHMENU = 0x234,
      WM_MDIRESTORE = 0x223,
      WM_MDISETMENU = 0x230,
      WM_MDITILE = 0x226,
      WM_MEASUREITEM = 0x2C,
      WM_MENUCHAR = 0x120,
      WM_MENUCOMMAND = 0x126,
      WM_MENUDRAG = 0x123,
      WM_MENUGETOBJECT = 0x124,
      WM_MENURBUTTONUP = 0x122,
      WM_MENUSELECT = 0x11F,
      WM_MOUSEACTIVATE = 0x21,
      WM_MOUSEFIRST = 0x200,
      WM_MOUSEHOVER = 0x2A1,
      WM_MOUSELAST = 0x20A,
      WM_MOUSELEAVE = 0x2A3,
      WM_MOUSEMOVE = 0x200,
      WM_MOUSEWHEEL = 0x20A,
      WM_MOVE = 0x3,
      WM_MOVING = 0x216,
      WM_NCACTIVATE = 0x86,
      WM_NCCALCSIZE = 0x83,
      WM_NCCREATE = 0x81,
      WM_NCDESTROY = 0x82,
      WM_NCHITTEST = 0x84,
      WM_NCLBUTTONDBLCLK = 0xA3,
      WM_NCLBUTTONDOWN = 0xA1,
      WM_NCLBUTTONUP = 0xA2,
      WM_NCMBUTTONDBLCLK = 0xA9,
      WM_NCMBUTTONDOWN = 0xA7,
      WM_NCMBUTTONUP = 0xA8,
      WM_NCMOUSEHOVER = 0x2A0,
      WM_NCMOUSELEAVE = 0x2A2,
      WM_NCMOUSEMOVE = 0xA0,
      WM_NCPAINT = 0x85,
      WM_NCRBUTTONDBLCLK = 0xA6,
      WM_NCRBUTTONDOWN = 0xA4,
      WM_NCRBUTTONUP = 0xA5,
      WM_NEXTDLGCTL = 0x28,
      WM_NEXTMENU = 0x213,
      WM_NOTIFY = 0x4E,
      WM_NOTIFYFORMAT = 0x55,
      WM_NULL = 0x0,
      WM_PAINT = 0xF,
      WM_PAINTCLIPBOARD = 0x309,
      WM_PAINTICON = 0x26,
      WM_PALETTECHANGED = 0x311,
      WM_PALETTEISCHANGING = 0x310,
      WM_PARENTNOTIFY = 0x210,
      WM_PASTE = 0x302,
      WM_PENWINFIRST = 0x380,
      WM_PENWINLAST = 0x38F,
      WM_POWER = 0x48,
      WM_PRINT = 0x317,
      WM_PRINTCLIENT = 0x318,
      WM_QUERYDRAGICON = 0x37,
      WM_QUERYENDSESSION = 0x11,
      WM_QUERYNEWPALETTE = 0x30F,
      WM_QUERYOPEN = 0x13,
      WM_QUERYUISTATE = 0x129,
      WM_QUEUESYNC = 0x23,
      WM_QUIT = 0x12,
      WM_RBUTTONDBLCLK = 0x206,
      WM_RBUTTONDOWN = 0x204,
      WM_RBUTTONUP = 0x205,
      WM_RENDERALLFORMATS = 0x306,
      WM_RENDERFORMAT = 0x305,
      WM_SETCURSOR = 0x20,
      WM_SETFOCUS = 0x7,
      WM_SETFONT = 0x30,
      WM_SETHOTKEY = 0x32,
      WM_SETICON = 0x80,
      WM_SETREDRAW = 0xB,
      WM_SETTEXT = 0xC,
      WM_SETTINGCHANGE = 0x1A,
      WM_SHOWWINDOW = 0x18,
      WM_SIZE = 0x5,
      WM_SIZECLIPBOARD = 0x30B,
      WM_SIZING = 0x214,
      WM_SPOOLERSTATUS = 0x2A,
      WM_STYLECHANGED = 0x7D,
      WM_STYLECHANGING = 0x7C,
      WM_SYNCPAINT = 0x88,
      WM_SYSCHAR = 0x106,
      WM_SYSCOLORCHANGE = 0x15,
      WM_SYSCOMMAND = 0x112,
      WM_SYSDEADCHAR = 0x107,
      WM_SYSKEYDOWN = 0x104,
      WM_SYSKEYUP = 0x105,
      WM_SYSTIMER = 0x118,  // undocumented, see http://support.microsoft.com/?id=108938
      WM_TCARD = 0x52,
      WM_TIMECHANGE = 0x1E,
      WM_TIMER = 0x113,
      WM_UNDO = 0x304,
      WM_UNINITMENUPOPUP = 0x125,
      WM_USER = 0x400,
      WM_USERCHANGED = 0x54,
      WM_VKEYTOITEM = 0x2E,
      WM_VSCROLL = 0x115,
      WM_VSCROLLCLIPBOARD = 0x30A,
      WM_WINDOWPOSCHANGED = 0x47,
      WM_WINDOWPOSCHANGING = 0x46,
      WM_WININICHANGE = 0x1A,
      WM_XBUTTONDBLCLK = 0x20D,
      WM_XBUTTONDOWN = 0x20B,
      WM_XBUTTONUP = 0x20C
    }

    public enum SysCommand
    {
      SC_SIZE         = 0xF000,
      SC_MOVE         = 0xF010,
      SC_MINIMIZE     = 0xF020,
      SC_MAXIMIZE     = 0xF030,
      SC_NEXTWINDOW   = 0xF040,
      SC_PREVWINDOW   = 0xF050,
      SC_CLOSE        = 0xF060,
      SC_VSCROLL      = 0xF070,
      SC_HSCROLL      = 0xF080,
      SC_MOUSEMENU    = 0xF090,
      SC_KEYMENU      = 0xF100,
      SC_ARRANGE      = 0xF110,
      SC_RESTORE      = 0xF120,
      SC_TASKLIST     = 0xF130,
      SC_SCREENSAVE   = 0xF140,
      SC_HOTKEY       = 0xF150,
      SC_DEFAULT      = 0xF160,
      SC_MONITORPOWER = 0xF170,
      SC_CONTEXTHELP  = 0xF180,
      SC_SEPARATOR    = 0xF00F,
      
      SCF_ISSECURE    = 0x00000001,
      
      SC_ICON         = SC_MINIMIZE,
      SC_ZOOM         = SC_MAXIMIZE,
    }

    public enum AppCommand
    {
      APPCOMMAND_BROWSER_BACKWARD     = 1,
      APPCOMMAND_BROWSER_FORWARD      = 2,
      APPCOMMAND_BROWSER_REFRESH      = 3,
      APPCOMMAND_BROWSER_STOP         = 4,
      APPCOMMAND_BROWSER_SEARCH       = 5,
      APPCOMMAND_BROWSER_FAVORITES    = 6,
      APPCOMMAND_BROWSER_HOME         = 7,
      APPCOMMAND_VOLUME_MUTE          = 8,
      APPCOMMAND_VOLUME_DOWN          = 9,
      APPCOMMAND_VOLUME_UP            = 10,
      APPCOMMAND_MEDIA_NEXTTRACK      = 11,
      APPCOMMAND_MEDIA_PREVIOUSTRACK  = 12,
      APPCOMMAND_MEDIA_STOP           = 13,
      APPCOMMAND_MEDIA_PLAY_PAUSE     = 4143,
      APPCOMMAND_MEDIA_PLAY           = 4142,
      APPCOMMAND_MEDIA_PAUSE          = 4143,
      APPCOMMAND_MEDIA_RECORD         = 4144,
      APPCOMMAND_MEDIA_FASTFORWARD    = 4145,
      APPCOMMAND_MEDIA_REWIND         = 4146,
      APPCOMMAND_MEDIA_CHANNEL_UP     = 4147,
      APPCOMMAND_MEDIA_CHANNEL_DOWN   = 4148,
    }

    [Flags]
    public enum SendMessageTimeoutFlags
    {
      SMTO_NORMAL             = 0x0000,
      SMTO_BLOCK              = 0x0001,
      SMTO_ABORTIFHUNG        = 0x0002,
      SMTO_NOTIMEOUTIFNOTHUNG = 0x0008,
    }

    [Flags]
    public enum ShutdownReasons
    {
      MajorApplication          = 0x00040000,
      MajorHardware             = 0x00010000,
      MajorLegacyApi            = 0x00070000,
      MajorOperatingSystem      = 0x00020000,
      MajorOther                = 0x00000000,
      MajorPower                = 0x00060000,
      MajorSoftware             = 0x00030000,
      MajorSystem               = 0x00050000,

      MinorBlueScreen           = 0x0000000F,
      MinorCordUnplugged        = 0x0000000b,
      MinorDisk                 = 0x00000007,
      MinorEnvironment          = 0x0000000c,
      MinorHardwareDriver       = 0x0000000d,
      MinorHotfix               = 0x00000011,
      MinorHung                 = 0x00000005,
      MinorInstallation         = 0x00000002,
      MinorMaintenance          = 0x00000001,
      MinorMMC                  = 0x00000019,
      MinorNetworkConnectivity  = 0x00000014,
      MinorNetworkCard          = 0x00000009,
      MinorOther                = 0x00000000,
      MinorOtherDriver          = 0x0000000e,
      MinorPowerSupply          = 0x0000000a,
      MinorProcessor            = 0x00000008,
      MinorReconfig             = 0x00000004,
      MinorSecurity             = 0x00000013,
      MinorSecurityFix          = 0x00000012,
      MinorSecurityFixUninstall = 0x00000018,
      MinorServicePack          = 0x00000010,
      MinorServicePackUninstall = 0x00000016,
      MinorTermSrv              = 0x00000020,
      MinorUnstable             = 0x00000006,
      MinorUpgrade              = 0x00000003,
      MinorWMI                  = 0x00000015,

      FlagUserDefined           = 0x40000000,
      //FlagPlanned               = 0x80000000,
    }

    [Flags]
    public enum ExitWindows
    {
      // ONE of the following five:
      LogOff      = 0x00,
      ShutDown    = 0x01,
      Reboot      = 0x02,
      PowerOff    = 0x08,
      RestartApps = 0x40,
      // Optionally include ONE of the following:
      Force       = 0x04,
      ForceIfHung = 0x10,
    }

    #endregion Enumerations

    #region Structures

    [StructLayout(LayoutKind.Sequential)]
    public struct COPYDATASTRUCT
    {
      public int dwData;
      public int cbData;
      public int lpData;
    }

    #endregion Structures

    #region Delegates

    public delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

    #endregion Delegates

    #region Interop

    [DllImport("user32.dll")]
    public static extern int EnumWindows(EnumWindowsProc ewp, int lParam); 

    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ExitWindowsEx(ExitWindows uFlags, ShutdownReasons dwReason);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessageTimeout(
      IntPtr hWnd,
      int msg,
      IntPtr wParam,
      IntPtr lParam,
      SendMessageTimeoutFlags flags,
      int timeout,
      out IntPtr result);

    //[DllImport("user32.dll", SetLastError = false)]
    //public static extern IntPtr SendMessage(IntPtr windowHandle, int msg, IntPtr wordParam, IntPtr longParam);

    //[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    //public static extern int RegisterWindowMessage(string lpString);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetFocus(IntPtr hWnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool BringWindowToTop(IntPtr hWnd);

    //[DllImport("user32.dll")]
    //[return: MarshalAs(UnmanagedType.Bool)]
    //private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool AttachThreadInput(int nThreadId, int nThreadIdTo, bool bAttach);

    //[DllImport("user32.dll")]
    //[return: MarshalAs(UnmanagedType.Bool)]
    //private static extern bool IsWindowVisible(IntPtr hWnd);

    //[DllImport("user32.dll")]
    //[return: MarshalAs(UnmanagedType.Bool)]
    //private static extern bool IsIconic(IntPtr hWnd);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

    [DllImport("kernel32.dll")]
    private static extern int GetCurrentThreadId();

    #region Net API

    [DllImport("netapi32.dll", CharSet = CharSet.Auto, SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
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

    [DllImport("netapi32.dll", SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
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
    /// Get the window title for a specified window handle.
    /// </summary>
    /// <param name="hWnd">Handle to a window.</param>
    /// <returns>Window title.</returns>
    public static string GetWindowTitle(IntPtr hWnd)
    {
      int length = GetWindowTextLength(hWnd);
      
      StringBuilder windowTitle = new StringBuilder(length + 1);
      
      GetWindowText(hWnd, windowTitle, windowTitle.Capacity);
      
      return windowTitle.ToString();
    }

    /// <summary>
    /// Takes a given window from whatever state it is in and makes it the foreground window.
    /// </summary>
    /// <param name="hWnd">Handle to window.</param>
    /// <param name="force">Force from a minimized or hidden state.</param>
    /// <returns>Success.</returns>
    public static bool SetForegroundWindow(IntPtr hWnd, bool force)
    {
      IntPtr fgWindow = GetForegroundWindow();

      if (hWnd == fgWindow || SetForegroundWindow(hWnd))
        return true;

      if (force == false)
        return false;

      if (fgWindow == IntPtr.Zero)
        return false;

      int fgWindowPID = -1;
      GetWindowThreadProcessId(fgWindow, out fgWindowPID);

      if (fgWindowPID == -1)
        return false;

      int curThreadID = GetCurrentThreadId();

      // if we don't attach successfully to the windows thread then we're out of options
      if (!AttachThreadInput(curThreadID, fgWindowPID, true))
        return false;

      SetForegroundWindow(hWnd);
      BringWindowToTop(hWnd);
      SetFocus(hWnd);

      AttachThreadInput(curThreadID, fgWindowPID, false);

      // we've done all that we can so base our return value on whether we have succeeded or not
      return (GetForegroundWindow() == hWnd);
    }

    /// <summary>
    /// Get the Process ID of the current foreground window.
    /// </summary>
    /// <returns>Process ID.</returns>
    public static int GetForegroundWindowPID()
    {
      int pid = -1;
      
      IntPtr active = GetForegroundWindow();

      if (active.Equals(IntPtr.Zero))
        return pid;

      GetWindowThreadProcessId(active, out pid);

      return pid;
    }

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

    /// <summary>
    /// Get an IntPtr pointing to any object.
    /// </summary>
    /// <param name="o">Object to get pointer for.</param>
    /// <returns>Pointer to object.</returns>
    public static IntPtr VarPtr(object o)
    {
      GCHandle handle = GCHandle.Alloc(o, GCHandleType.Pinned);
      IntPtr ptr = handle.AddrOfPinnedObject();
      handle.Free();
      return ptr;
    }

    /*
    public static void ActivateWindowByHandle(IntPtr hWnd)
    {
      WindowPlacement windowPlacement;
      GetWindowPlacement(hWnd, out windowPlacement);

      switch (windowPlacement.showCmd)
      {
        case SW_HIDE:           //Window is hidden
          ShowWindow(hWnd, SW_RESTORE);
          break;
        case SW_SHOWMINIMIZED:  //Window is minimized
          // if the window is minimized, then we need to restore it to its 
          // previous size. we also take into account whether it was 
          // previously maximized. 
          int showCmd = (windowPlacement.flags == WPF_RESTORETOMAXIMIZED) ? SW_SHOWMAXIMIZED : SW_SHOWNORMAL;
          ShowWindow(hWnd, showCmd);
          break;
        default:
          // if it's not minimized, then we just call SetForegroundWindow to 
          // bring it to the front. 
          SetForegroundWindow(hWnd);
          break;
      }
    }
    */

    #endregion Methods

  }

}
