using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace IrssUtils
{

  /// <summary>
  /// Win32 native method class.
  /// </summary>
  public static class Win32
  {

    #region Constants

    /// <summary>
    /// Maximum length of unmanaged Windows Path strings.
    /// </summary>
    private const int MAX_PATH = 260;

    /// <summary>
    /// Maximum length of unmanaged Typename.
    /// </summary>
    private const int MAX_TYPE = 80;

    #endregion Constants

    #region Enumerations

    [Flags]
    enum SHGFI : int
    {
      /// <summary>get icon</summary>
      Icon = 0x000000100,
      /// <summary>get display name</summary>
      DisplayName = 0x000000200,
      /// <summary>get type name</summary>
      TypeName = 0x000000400,
      /// <summary>get attributes</summary>
      Attributes = 0x000000800,
      /// <summary>get icon location</summary>
      IconLocation = 0x000001000,
      /// <summary>return exe type</summary>
      ExeType = 0x000002000,
      /// <summary>get system icon index</summary>
      SysIconIndex = 0x000004000,
      /// <summary>put a link overlay on icon</summary>
      LinkOverlay = 0x000008000,
      /// <summary>show icon in selected state</summary>
      Selected = 0x000010000,
      /// <summary>get only specified attributes</summary>
      Attr_Specified = 0x000020000,
      /// <summary>get large icon</summary>
      LargeIcon = 0x000000000,
      /// <summary>get small icon</summary>
      SmallIcon = 0x000000001,
      /// <summary>get open icon</summary>
      OpenIcon = 0x000000002,
      /// <summary>get shell size icon</summary>
      ShellIconSize = 0x000000004,
      /// <summary>pszPath is a pidl</summary>
      PIDL = 0x000000008,
      /// <summary>use passed dwFileAttribute</summary>
      UseFileAttributes = 0x000000010,
      /// <summary>apply the appropriate overlays</summary>
      AddOverlays = 0x000000020,
      /// <summary>Get the index of the overlay in the upper 8 bits of the iIcon</summary>
      OverlayIndex = 0x000000040,
    }

    /// <summary>
    /// Windows Message types.
    /// </summary>
    public enum WindowsMessage
    {
      /// <summary>
      /// WM_ACTIVATE
      /// </summary>
      WM_ACTIVATE = 0x6,
      /// <summary>
      /// WM_ACTIVATEAPP
      /// </summary>
      WM_ACTIVATEAPP = 0x1C,
      /// <summary>
      /// WM_AFXFIRST
      /// </summary>
      WM_AFXFIRST = 0x360,
      /// <summary>
      /// WM_AFXLAST
      /// </summary>
      WM_AFXLAST = 0x37F,
      /// <summary>
      /// WM_APP
      /// </summary>
      WM_APP = 0x8000,
      /// <summary>
      /// WM_APPCOMMAND
      /// </summary>
      WM_APPCOMMAND = 0x319,
      /// <summary>
      /// WM_ASKCBFORMATNAME
      /// </summary>
      WM_ASKCBFORMATNAME = 0x30C,
      /// <summary>
      /// WM_CANCELJOURNAL
      /// </summary>
      WM_CANCELJOURNAL = 0x4B,
      /// <summary>
      /// WM_CANCELMODE
      /// </summary>
      WM_CANCELMODE = 0x1F,
      /// <summary>
      /// WM_CAPTURECHANGED
      /// </summary>
      WM_CAPTURECHANGED = 0x215,
      /// <summary>
      /// WM_CHANGECBCHAIN
      /// </summary>
      WM_CHANGECBCHAIN = 0x30D,
      /// <summary>
      /// WM_CHAR
      /// </summary>
      WM_CHAR = 0x102,
      /// <summary>
      /// 
      /// </summary>
      WM_CHARTOITEM = 0x2F,
      /// <summary>
      /// 
      /// </summary>
      WM_CHILDACTIVATE = 0x22,
      /// <summary>
      /// 
      /// </summary>
      WM_CLEAR = 0x303,
      /// <summary>
      /// 
      /// </summary>
      WM_CLOSE = 0x10,
      /// <summary>
      /// 
      /// </summary>
      WM_COMMAND = 0x111,
      /// <summary>
      /// 
      /// </summary>
      WM_COMPACTING = 0x41,
      /// <summary>
      /// 
      /// </summary>
      WM_COMPAREITEM = 0x39,
      /// <summary>
      /// 
      /// </summary>
      WM_CONTEXTMENU = 0x7B,
      /// <summary>
      /// 
      /// </summary>
      WM_COPY = 0x301,
      /// <summary>
      /// 
      /// </summary>
      WM_COPYDATA = 0x4A,
      /// <summary>
      /// 
      /// </summary>
      WM_CREATE = 0x1,
      /// <summary>
      /// 
      /// </summary>
      WM_CTLCOLORBTN = 0x135,
      /// <summary>
      /// 
      /// </summary>
      WM_CTLCOLORDLG = 0x136,
      /// <summary>
      /// 
      /// </summary>
      WM_CTLCOLOREDIT = 0x133,
      /// <summary>
      /// 
      /// </summary>
      WM_CTLCOLORLISTBOX = 0x134,
      /// <summary>
      /// 
      /// </summary>
      WM_CTLCOLORMSGBOX = 0x132,
      /// <summary>
      /// 
      /// </summary>
      WM_CTLCOLORSCROLLBAR = 0x137,
      /// <summary>
      /// 
      /// </summary>
      WM_CTLCOLORSTATIC = 0x138,
      /// <summary>
      /// 
      /// </summary>
      WM_CUT = 0x300,
      /// <summary>
      /// 
      /// </summary>
      WM_DEADCHAR = 0x103,
      /// <summary>
      /// 
      /// </summary>
      WM_DELETEITEM = 0x2D,
      /// <summary>
      /// 
      /// </summary>
      WM_DESTROY = 0x2,
      /// <summary>
      /// 
      /// </summary>
      WM_DESTROYCLIPBOARD = 0x307,
      /// <summary>
      /// 
      /// </summary>
      WM_DEVICECHANGE = 0x219,
      /// <summary>
      /// 
      /// </summary>
      WM_DEVMODECHANGE = 0x1B,
      /// <summary>
      /// 
      /// </summary>
      WM_DISPLAYCHANGE = 0x7E,
      /// <summary>
      /// 
      /// </summary>
      WM_DRAWCLIPBOARD = 0x308,
      /// <summary>
      /// 
      /// </summary>
      WM_DRAWITEM = 0x2B,
      /// <summary>
      /// 
      /// </summary>
      WM_DROPFILES = 0x233,
      /// <summary>
      /// 
      /// </summary>
      WM_ENABLE = 0xA,
      /// <summary>
      /// 
      /// </summary>
      WM_ENDSESSION = 0x16,
      /// <summary>
      /// 
      /// </summary>
      WM_ENTERIDLE = 0x121,
      /// <summary>
      /// 
      /// </summary>
      WM_ENTERMENULOOP = 0x211,
      /// <summary>
      /// 
      /// </summary>
      WM_ENTERSIZEMOVE = 0x231,
      /// <summary>
      /// 
      /// </summary>
      WM_ERASEBKGND = 0x14,
      /// <summary>
      /// 
      /// </summary>
      WM_EXITMENULOOP = 0x212,
      /// <summary>
      /// 
      /// </summary>
      WM_EXITSIZEMOVE = 0x232,
      /// <summary>
      /// 
      /// </summary>
      WM_FONTCHANGE = 0x1D,
      /// <summary>
      /// 
      /// </summary>
      WM_GETDLGCODE = 0x87,
      /// <summary>
      /// 
      /// </summary>
      WM_GETFONT = 0x31,
      /// <summary>
      /// 
      /// </summary>
      WM_GETHOTKEY = 0x33,
      /// <summary>
      /// 
      /// </summary>
      WM_GETICON = 0x7F,
      /// <summary>
      /// 
      /// </summary>
      WM_GETMINMAXINFO = 0x24,
      /// <summary>
      /// 
      /// </summary>
      WM_GETOBJECT = 0x3D,
      /// <summary>
      /// 
      /// </summary>
      WM_GETSYSMENU = 0x313,
      /// <summary>
      /// 
      /// </summary>
      WM_GETTEXT = 0xD,
      /// <summary>
      /// 
      /// </summary>
      WM_GETTEXTLENGTH = 0xE,
      /// <summary>
      /// 
      /// </summary>
      WM_HANDHELDFIRST = 0x358,
      /// <summary>
      /// 
      /// </summary>
      WM_HANDHELDLAST = 0x35F,
      /// <summary>
      /// 
      /// </summary>
      WM_HELP = 0x53,
      /// <summary>
      /// 
      /// </summary>
      WM_HOTKEY = 0x312,
      /// <summary>
      /// 
      /// </summary>
      WM_HSCROLL = 0x114,
      /// <summary>
      /// 
      /// </summary>
      WM_HSCROLLCLIPBOARD = 0x30E,
      /// <summary>
      /// 
      /// </summary>
      WM_ICONERASEBKGND = 0x27,
      /// <summary>
      /// 
      /// </summary>
      WM_IME_CHAR = 0x286,
      /// <summary>
      /// 
      /// </summary>
      WM_IME_COMPOSITION = 0x10F,
      /// <summary>
      /// 
      /// </summary>
      WM_IME_COMPOSITIONFULL = 0x284,
      /// <summary>
      /// 
      /// </summary>
      WM_IME_CONTROL = 0x283,
      /// <summary>
      /// 
      /// </summary>
      WM_IME_ENDCOMPOSITION = 0x10E,
      /// <summary>
      /// 
      /// </summary>
      WM_IME_KEYDOWN = 0x290,
      /// <summary>
      /// 
      /// </summary>
      WM_IME_KEYLAST = 0x10F,
      /// <summary>
      /// 
      /// </summary>
      WM_IME_KEYUP = 0x291,
      /// <summary>
      /// 
      /// </summary>
      WM_IME_NOTIFY = 0x282,
      /// <summary>
      /// 
      /// </summary>
      WM_IME_REQUEST = 0x288,
      /// <summary>
      /// 
      /// </summary>
      WM_IME_SELECT = 0x285,
      /// <summary>
      /// 
      /// </summary>
      WM_IME_SETCONTEXT = 0x281,
      /// <summary>
      /// 
      /// </summary>
      WM_IME_STARTCOMPOSITION = 0x10D,
      /// <summary>
      /// 
      /// </summary>
      WM_INITDIALOG = 0x110,
      /// <summary>
      /// 
      /// </summary>
      WM_INITMENU = 0x116,
      /// <summary>
      /// 
      /// </summary>
      WM_INITMENUPOPUP = 0x117,
      /// <summary>
      /// 
      /// </summary>
      WM_INPUTLANGCHANGE = 0x51,
      /// <summary>
      /// 
      /// </summary>
      WM_INPUTLANGCHANGEREQUEST = 0x50,
      /// <summary>
      /// 
      /// </summary>
      WM_KEYDOWN = 0x100,
      /// <summary>
      /// 
      /// </summary>
      WM_KEYFIRST = 0x100,
      /// <summary>
      /// 
      /// </summary>
      WM_KEYLAST = 0x108,
      /// <summary>
      /// 
      /// </summary>
      WM_KEYUP = 0x101,
      /// <summary>
      /// 
      /// </summary>
      WM_KILLFOCUS = 0x8,
      /// <summary>
      /// 
      /// </summary>
      WM_LBUTTONDBLCLK = 0x203,
      /// <summary>
      /// 
      /// </summary>
      WM_LBUTTONDOWN = 0x201,
      /// <summary>
      /// 
      /// </summary>
      WM_LBUTTONUP = 0x202,
      /// <summary>
      /// 
      /// </summary>
      WM_MBUTTONDBLCLK = 0x209,
      /// <summary>
      /// 
      /// </summary>
      WM_MBUTTONDOWN = 0x207,
      /// <summary>
      /// 
      /// </summary>
      WM_MBUTTONUP = 0x208,
      /// <summary>
      /// 
      /// </summary>
      WM_MDIACTIVATE = 0x222,
      /// <summary>
      /// 
      /// </summary>
      WM_MDICASCADE = 0x227,
      /// <summary>
      /// 
      /// </summary>
      WM_MDICREATE = 0x220,
      /// <summary>
      /// 
      /// </summary>
      WM_MDIDESTROY = 0x221,
      /// <summary>
      /// 
      /// </summary>
      WM_MDIGETACTIVE = 0x229,
      /// <summary>
      /// 
      /// </summary>
      WM_MDIICONARRANGE = 0x228,
      /// <summary>
      /// 
      /// </summary>
      WM_MDIMAXIMIZE = 0x225,
      /// <summary>
      /// 
      /// </summary>
      WM_MDINEXT = 0x224,
      /// <summary>
      /// 
      /// </summary>
      WM_MDIREFRESHMENU = 0x234,
      /// <summary>
      /// 
      /// </summary>
      WM_MDIRESTORE = 0x223,
      /// <summary>
      /// 
      /// </summary>
      WM_MDISETMENU = 0x230,
      /// <summary>
      /// 
      /// </summary>
      WM_MDITILE = 0x226,
      /// <summary>
      /// 
      /// </summary>
      WM_MEASUREITEM = 0x2C,
      /// <summary>
      /// 
      /// </summary>
      WM_MENUCHAR = 0x120,
      /// <summary>
      /// 
      /// </summary>
      WM_MENUCOMMAND = 0x126,
      /// <summary>
      /// 
      /// </summary>
      WM_MENUDRAG = 0x123,
      /// <summary>
      /// 
      /// </summary>
      WM_MENUGETOBJECT = 0x124,
      /// <summary>
      /// 
      /// </summary>
      WM_MENURBUTTONUP = 0x122,
      /// <summary>
      /// 
      /// </summary>
      WM_MENUSELECT = 0x11F,
      /// <summary>
      /// 
      /// </summary>
      WM_MOUSEACTIVATE = 0x21,
      /// <summary>
      /// 
      /// </summary>
      WM_MOUSEFIRST = 0x200,
      /// <summary>
      /// 
      /// </summary>
      WM_MOUSEHOVER = 0x2A1,
      /// <summary>
      /// 
      /// </summary>
      WM_MOUSELAST = 0x20A,
      /// <summary>
      /// 
      /// </summary>
      WM_MOUSELEAVE = 0x2A3,
      /// <summary>
      /// 
      /// </summary>
      WM_MOUSEMOVE = 0x200,
      /// <summary>
      /// 
      /// </summary>
      WM_MOUSEWHEEL = 0x20A,
      /// <summary>
      /// 
      /// </summary>
      WM_MOVE = 0x3,
      /// <summary>
      /// 
      /// </summary>
      WM_MOVING = 0x216,
      /// <summary>
      /// 
      /// </summary>
      WM_NCACTIVATE = 0x86,
      /// <summary>
      /// 
      /// </summary>
      WM_NCCALCSIZE = 0x83,
      /// <summary>
      /// 
      /// </summary>
      WM_NCCREATE = 0x81,
      /// <summary>
      /// 
      /// </summary>
      WM_NCDESTROY = 0x82,
      /// <summary>
      /// 
      /// </summary>
      WM_NCHITTEST = 0x84,
      /// <summary>
      /// 
      /// </summary>
      WM_NCLBUTTONDBLCLK = 0xA3,
      /// <summary>
      /// 
      /// </summary>
      WM_NCLBUTTONDOWN = 0xA1,
      /// <summary>
      /// 
      /// </summary>
      WM_NCLBUTTONUP = 0xA2,
      /// <summary>
      /// 
      /// </summary>
      WM_NCMBUTTONDBLCLK = 0xA9,
      /// <summary>
      /// 
      /// </summary>
      WM_NCMBUTTONDOWN = 0xA7,
      /// <summary>
      /// 
      /// </summary>
      WM_NCMBUTTONUP = 0xA8,
      /// <summary>
      /// 
      /// </summary>
      WM_NCMOUSEHOVER = 0x2A0,
      /// <summary>
      /// 
      /// </summary>
      WM_NCMOUSELEAVE = 0x2A2,
      /// <summary>
      /// 
      /// </summary>
      WM_NCMOUSEMOVE = 0xA0,
      /// <summary>
      /// 
      /// </summary>
      WM_NCPAINT = 0x85,
      /// <summary>
      /// 
      /// </summary>
      WM_NCRBUTTONDBLCLK = 0xA6,
      /// <summary>
      /// 
      /// </summary>
      WM_NCRBUTTONDOWN = 0xA4,
      /// <summary>
      /// 
      /// </summary>
      WM_NCRBUTTONUP = 0xA5,
      /// <summary>
      /// 
      /// </summary>
      WM_NEXTDLGCTL = 0x28,
      /// <summary>
      /// 
      /// </summary>
      WM_NEXTMENU = 0x213,
      /// <summary>
      /// 
      /// </summary>
      WM_NOTIFY = 0x4E,
      /// <summary>
      /// 
      /// </summary>
      WM_NOTIFYFORMAT = 0x55,
      /// <summary>
      /// 
      /// </summary>
      WM_NULL = 0x0,
      /// <summary>
      /// 
      /// </summary>
      WM_PAINT = 0xF,
      /// <summary>
      /// 
      /// </summary>
      WM_PAINTCLIPBOARD = 0x309,
      /// <summary>
      /// 
      /// </summary>
      WM_PAINTICON = 0x26,
      /// <summary>
      /// 
      /// </summary>
      WM_PALETTECHANGED = 0x311,
      /// <summary>
      /// 
      /// </summary>
      WM_PALETTEISCHANGING = 0x310,
      /// <summary>
      /// 
      /// </summary>
      WM_PARENTNOTIFY = 0x210,
      /// <summary>
      /// 
      /// </summary>
      WM_PASTE = 0x302,
      /// <summary>
      /// 
      /// </summary>
      WM_PENWINFIRST = 0x380,
      /// <summary>
      /// 
      /// </summary>
      WM_PENWINLAST = 0x38F,
      /// <summary>
      /// 
      /// </summary>
      WM_POWER = 0x48,
      /// <summary>
      /// 
      /// </summary>
      WM_PRINT = 0x317,
      /// <summary>
      /// 
      /// </summary>
      WM_PRINTCLIENT = 0x318,
      /// <summary>
      /// 
      /// </summary>
      WM_QUERYDRAGICON = 0x37,
      /// <summary>
      /// 
      /// </summary>
      WM_QUERYENDSESSION = 0x11,
      /// <summary>
      /// 
      /// </summary>
      WM_QUERYNEWPALETTE = 0x30F,
      /// <summary>
      /// 
      /// </summary>
      WM_QUERYOPEN = 0x13,
      /// <summary>
      /// 
      /// </summary>
      WM_QUERYUISTATE = 0x129,
      /// <summary>
      /// 
      /// </summary>
      WM_QUEUESYNC = 0x23,
      /// <summary>
      /// 
      /// </summary>
      WM_QUIT = 0x12,
      /// <summary>
      /// 
      /// </summary>
      WM_RBUTTONDBLCLK = 0x206,
      /// <summary>
      /// 
      /// </summary>
      WM_RBUTTONDOWN = 0x204,
      /// <summary>
      /// 
      /// </summary>
      WM_RBUTTONUP = 0x205,
      /// <summary>
      /// 
      /// </summary>
      WM_RENDERALLFORMATS = 0x306,
      /// <summary>
      /// 
      /// </summary>
      WM_RENDERFORMAT = 0x305,
      /// <summary>
      /// 
      /// </summary>
      WM_SETCURSOR = 0x20,
      /// <summary>
      /// 
      /// </summary>
      WM_SETFOCUS = 0x7,
      /// <summary>
      /// 
      /// </summary>
      WM_SETFONT = 0x30,
      /// <summary>
      /// 
      /// </summary>
      WM_SETHOTKEY = 0x32,
      /// <summary>
      /// 
      /// </summary>
      WM_SETICON = 0x80,
      /// <summary>
      /// 
      /// </summary>
      WM_SETREDRAW = 0xB,
      /// <summary>
      /// 
      /// </summary>
      WM_SETTEXT = 0xC,
      /// <summary>
      /// 
      /// </summary>
      WM_SETTINGCHANGE = 0x1A,
      /// <summary>
      /// 
      /// </summary>
      WM_SHOWWINDOW = 0x18,
      /// <summary>
      /// 
      /// </summary>
      WM_SIZE = 0x5,
      /// <summary>
      /// 
      /// </summary>
      WM_SIZECLIPBOARD = 0x30B,
      /// <summary>
      /// 
      /// </summary>
      WM_SIZING = 0x214,
      /// <summary>
      /// 
      /// </summary>
      WM_SPOOLERSTATUS = 0x2A,
      /// <summary>
      /// 
      /// </summary>
      WM_STYLECHANGED = 0x7D,
      /// <summary>
      /// 
      /// </summary>
      WM_STYLECHANGING = 0x7C,
      /// <summary>
      /// 
      /// </summary>
      WM_SYNCPAINT = 0x88,
      /// <summary>
      /// 
      /// </summary>
      WM_SYSCHAR = 0x106,
      /// <summary>
      /// 
      /// </summary>
      WM_SYSCOLORCHANGE = 0x15,
      /// <summary>
      /// 
      /// </summary>
      WM_SYSCOMMAND = 0x112,
      /// <summary>
      /// 
      /// </summary>
      WM_SYSDEADCHAR = 0x107,
      /// <summary>
      /// 
      /// </summary>
      WM_SYSKEYDOWN = 0x104,
      /// <summary>
      /// 
      /// </summary>
      WM_SYSKEYUP = 0x105,
      /// <summary>
      /// WM_SYSTIMER, undocumented, see http://support.microsoft.com/?id=108938
      /// </summary>
      WM_SYSTIMER = 0x118,
      /// <summary>
      /// 
      /// </summary>
      WM_TCARD = 0x52,
      /// <summary>
      /// 
      /// </summary>
      WM_TIMECHANGE = 0x1E,
      /// <summary>
      /// WM_TIMER
      /// </summary>
      WM_TIMER = 0x113,
      /// <summary>
      /// WM_UNDO
      /// </summary>
      WM_UNDO = 0x304,
      /// <summary>
      /// WM_UNINITMENUPOPUP
      /// </summary>
      WM_UNINITMENUPOPUP = 0x125,
      /// <summary>
      /// WM_USER
      /// </summary>
      WM_USER = 0x400,
      /// <summary>
      /// WM_USERCHANGED
      /// </summary>
      WM_USERCHANGED = 0x54,
      /// <summary>
      /// WM_VKEYTOITEM
      /// </summary>
      WM_VKEYTOITEM = 0x2E,
      /// <summary>
      /// WM_VSCROLL
      /// </summary>
      WM_VSCROLL = 0x115,
      /// <summary>
      /// WM_VSCROLLCLIPBOARD
      /// </summary>
      WM_VSCROLLCLIPBOARD = 0x30A,
      /// <summary>
      /// WM_WINDOWPOSCHANGED
      /// </summary>
      WM_WINDOWPOSCHANGED = 0x47,
      /// <summary>
      /// WM_WINDOWPOSCHANGING
      /// </summary>
      WM_WINDOWPOSCHANGING = 0x46,
      /// <summary>
      /// WM_WININICHANGE
      /// </summary>
      WM_WININICHANGE = 0x1A,
      /// <summary>
      /// WM_XBUTTONDBLCLK
      /// </summary>
      WM_XBUTTONDBLCLK = 0x20D,
      /// <summary>
      /// WM_XBUTTONDOWN
      /// </summary>
      WM_XBUTTONDOWN = 0x20B,
      /// <summary>
      /// WM_XBUTTONUP
      /// </summary>
      WM_XBUTTONUP = 0x20C
    }

    /// <summary>
    /// Windows Message System Commands.
    /// </summary>
    public enum SysCommand
    {
      /// <summary>
      /// SC_SIZE
      /// </summary>
      SC_SIZE = 0xF000,
      /// <summary>
      /// SC_MOVE
      /// </summary>
      SC_MOVE = 0xF010,
      /// <summary>
      /// SC_MINIMIZE
      /// </summary>
      SC_MINIMIZE = 0xF020,
      /// <summary>
      /// SC_MAXIMIZE
      /// </summary>
      SC_MAXIMIZE = 0xF030,
      /// <summary>
      /// SC_NEXTWINDOW
      /// </summary>
      SC_NEXTWINDOW = 0xF040,
      /// <summary>
      /// SC_PREVWINDOW
      /// </summary>
      SC_PREVWINDOW = 0xF050,
      /// <summary>
      /// SC_CLOSE
      /// </summary>
      SC_CLOSE = 0xF060,
      /// <summary>
      /// SC_VSCROLL
      /// </summary>
      SC_VSCROLL = 0xF070,
      /// <summary>
      /// SC_HSCROLL
      /// </summary>
      SC_HSCROLL = 0xF080,
      /// <summary>
      /// SC_MOUSEMENU
      /// </summary>
      SC_MOUSEMENU = 0xF090,
      /// <summary>
      /// SC_KEYMENU
      /// </summary>
      SC_KEYMENU = 0xF100,
      /// <summary>
      /// SC_ARRANGE
      /// </summary>
      SC_ARRANGE = 0xF110,
      /// <summary>
      /// SC_RESTORE
      /// </summary>
      SC_RESTORE = 0xF120,
      /// <summary>
      /// SC_TASKLIST
      /// </summary>
      SC_TASKLIST = 0xF130,
      /// <summary>
      /// SC_SCREENSAVE
      /// </summary>
      SC_SCREENSAVE = 0xF140,
      /// <summary>
      /// SC_HOTKEY
      /// </summary>
      SC_HOTKEY = 0xF150,
      /// <summary>
      /// SC_DEFAULT
      /// </summary>
      SC_DEFAULT = 0xF160,
      /// <summary>
      /// SC_MONITORPOWER
      /// </summary>
      SC_MONITORPOWER = 0xF170,
      /// <summary>
      /// SC_CONTEXTHELP
      /// </summary>
      SC_CONTEXTHELP = 0xF180,
      /// <summary>
      /// SC_SEPARATOR
      /// </summary>
      SC_SEPARATOR = 0xF00F,

      /// <summary>
      /// SCF_ISSECURE
      /// </summary>
      SCF_ISSECURE = 0x00000001,

      /// <summary>
      /// SC_ICON
      /// </summary>
      SC_ICON = SC_MINIMIZE,
      /// <summary>
      /// SC_ZOOM
      /// </summary>
      SC_ZOOM = SC_MAXIMIZE,
    }

    /// <summary>
    /// Windows Message App Commands.
    /// </summary>
    public enum AppCommand
    {
      /// <summary>
      /// APPCOMMAND_BROWSER_BACKWARD
      /// </summary>
      APPCOMMAND_BROWSER_BACKWARD = 1,
      /// <summary>
      /// APPCOMMAND_BROWSER_FORWARD
      /// </summary>
      APPCOMMAND_BROWSER_FORWARD = 2,
      /// <summary>
      /// APPCOMMAND_BROWSER_REFRESH
      /// </summary>
      APPCOMMAND_BROWSER_REFRESH = 3,
      /// <summary>
      /// APPCOMMAND_BROWSER_STOP
      /// </summary>
      APPCOMMAND_BROWSER_STOP = 4,
      /// <summary>
      /// APPCOMMAND_BROWSER_SEARCH
      /// </summary>
      APPCOMMAND_BROWSER_SEARCH = 5,
      /// <summary>
      /// APPCOMMAND_BROWSER_FAVORITES
      /// </summary>
      APPCOMMAND_BROWSER_FAVORITES = 6,
      /// <summary>
      /// APPCOMMAND_BROWSER_HOME
      /// </summary>
      APPCOMMAND_BROWSER_HOME = 7,
      /// <summary>
      /// APPCOMMAND_VOLUME_MUTE
      /// </summary>
      APPCOMMAND_VOLUME_MUTE = 8,
      /// <summary>
      /// APPCOMMAND_VOLUME_DOWN
      /// </summary>
      APPCOMMAND_VOLUME_DOWN = 9,
      /// <summary>
      /// APPCOMMAND_VOLUME_UP
      /// </summary>
      APPCOMMAND_VOLUME_UP = 10,
      /// <summary>
      /// APPCOMMAND_MEDIA_NEXTTRACK
      /// </summary>
      APPCOMMAND_MEDIA_NEXTTRACK = 11,
      /// <summary>
      /// APPCOMMAND_MEDIA_PREVIOUSTRACK
      /// </summary>
      APPCOMMAND_MEDIA_PREVIOUSTRACK = 12,
      /// <summary>
      /// APPCOMMAND_MEDIA_STOP
      /// </summary>
      APPCOMMAND_MEDIA_STOP = 13,
      /// <summary>
      /// APPCOMMAND_MEDIA_PLAY_PAUSE
      /// </summary>
      APPCOMMAND_MEDIA_PLAY_PAUSE = 4143,
      /// <summary>
      /// APPCOMMAND_MEDIA_PLAY
      /// </summary>
      APPCOMMAND_MEDIA_PLAY = 4142,
      /// <summary>
      /// APPCOMMAND_MEDIA_PAUSE
      /// </summary>
      APPCOMMAND_MEDIA_PAUSE = 4143,
      /// <summary>
      /// APPCOMMAND_MEDIA_RECORD
      /// </summary>
      APPCOMMAND_MEDIA_RECORD = 4144,
      /// <summary>
      /// APPCOMMAND_MEDIA_FASTFORWARD
      /// </summary>
      APPCOMMAND_MEDIA_FASTFORWARD = 4145,
      /// <summary>
      /// APPCOMMAND_MEDIA_REWIND
      /// </summary>
      APPCOMMAND_MEDIA_REWIND = 4146,
      /// <summary>
      /// APPCOMMAND_MEDIA_CHANNEL_UP
      /// </summary>
      APPCOMMAND_MEDIA_CHANNEL_UP = 4147,
      /// <summary>
      /// APPCOMMAND_MEDIA_CHANNEL_DOWN
      /// </summary>
      APPCOMMAND_MEDIA_CHANNEL_DOWN = 4148,
    }

    /// <summary>
    /// Send Windows Message with Timeout Flags.
    /// </summary>
    [Flags]
    public enum SendMessageTimeoutFlags
    {
      /// <summary>
      /// Normal.
      /// </summary>
      SMTO_NORMAL = 0x0000,
      /// <summary>
      /// Block.
      /// </summary>
      SMTO_BLOCK = 0x0001,
      /// <summary>
      /// Abort if hung.
      /// </summary>
      SMTO_ABORTIFHUNG = 0x0002,
      /// <summary>
      /// To timeout if not hung.
      /// </summary>
      SMTO_NOTIMEOUTIFNOTHUNG = 0x0008,
    }

    /// <summary>
    /// Shutdown Reasons.
    /// </summary>
    [Flags]
    public enum ShutdownReasons
    {
      /// <summary>
      /// MajorApplication
      /// </summary>
      MajorApplication = 0x00040000,
      /// <summary>
      /// MajorHardware
      /// </summary>
      MajorHardware = 0x00010000,
      /// <summary>
      /// MajorLegacyApi
      /// </summary>
      MajorLegacyApi = 0x00070000,
      /// <summary>
      /// MajorOperatingSystem
      /// </summary>
      MajorOperatingSystem = 0x00020000,
      /// <summary>
      /// MajorOther
      /// </summary>
      MajorOther = 0x00000000,
      /// <summary>
      /// MajorPower
      /// </summary>
      MajorPower = 0x00060000,
      /// <summary>
      /// MajorSoftware
      /// </summary>
      MajorSoftware = 0x00030000,
      /// <summary>
      /// MajorSystem
      /// </summary>
      MajorSystem = 0x00050000,

      /// <summary>
      /// MinorBlueScreen
      /// </summary>
      MinorBlueScreen = 0x0000000F,
      /// <summary>
      /// MinorCordUnplugged
      /// </summary>
      MinorCordUnplugged = 0x0000000b,
      /// <summary>
      /// MinorDisk
      /// </summary>
      MinorDisk = 0x00000007,
      /// <summary>
      /// MinorEnvironment
      /// </summary>
      MinorEnvironment = 0x0000000c,
      /// <summary>
      /// MinorHardwareDriver
      /// </summary>
      MinorHardwareDriver = 0x0000000d,
      /// <summary>
      /// MinorHotfix
      /// </summary>
      MinorHotfix = 0x00000011,
      /// <summary>
      /// MinorHung
      /// </summary>
      MinorHung = 0x00000005,
      /// <summary>
      /// MinorInstallation
      /// </summary>
      MinorInstallation = 0x00000002,
      /// <summary>
      /// MinorMaintenance
      /// </summary>
      MinorMaintenance = 0x00000001,
      /// <summary>
      /// MinorMMC
      /// </summary>
      MinorMMC = 0x00000019,
      /// <summary>
      /// MinorNetworkConnectivity
      /// </summary>
      MinorNetworkConnectivity = 0x00000014,
      /// <summary>
      /// MinorNetworkCard
      /// </summary>
      MinorNetworkCard = 0x00000009,
      /// <summary>
      /// MinorOther
      /// </summary>
      MinorOther = 0x00000000,
      /// <summary>
      /// MinorOtherDriver
      /// </summary>
      MinorOtherDriver = 0x0000000e,
      /// <summary>
      /// MinorPowerSupply
      /// </summary>
      MinorPowerSupply = 0x0000000a,
      /// <summary>
      /// MinorProcessor
      /// </summary>
      MinorProcessor = 0x00000008,
      /// <summary>
      /// MinorReconfig
      /// </summary>
      MinorReconfig = 0x00000004,
      /// <summary>
      /// MinorSecurity
      /// </summary>
      MinorSecurity = 0x00000013,
      /// <summary>
      /// MinorSecurityFix
      /// </summary>
      MinorSecurityFix = 0x00000012,
      /// <summary>
      /// MinorSecurityFixUninstall
      /// </summary>
      MinorSecurityFixUninstall = 0x00000018,
      /// <summary>
      /// MinorServicePack
      /// </summary>
      MinorServicePack = 0x00000010,
      /// <summary>
      /// MinorServicePackUninstall
      /// </summary>
      MinorServicePackUninstall = 0x00000016,
      /// <summary>
      /// MinorTermSrv
      /// </summary>
      MinorTermSrv = 0x00000020,
      /// <summary>
      /// MinorUnstable
      /// </summary>
      MinorUnstable = 0x00000006,
      /// <summary>
      /// MinorUpgrade
      /// </summary>
      MinorUpgrade = 0x00000003,
      /// <summary>
      /// MinorWMI
      /// </summary>
      MinorWMI = 0x00000015,

      /// <summary>
      /// FlagUserDefined
      /// </summary>
      FlagUserDefined = 0x40000000,

      //FlagPlanned               = 0x80000000,
    }

    /// <summary>
    /// Exit Windows method.
    /// </summary>
    [Flags]
    public enum ExitWindows
    {
      /// <summary>
      /// LogOff
      /// </summary>
      LogOff = 0x00,
      /// <summary>
      /// ShutDown
      /// </summary>
      ShutDown = 0x01,
      /// <summary>
      /// Reboot
      /// </summary>
      Reboot = 0x02,
      /// <summary>
      /// PowerOff
      /// </summary>
      PowerOff = 0x08,
      /// <summary>
      /// RestartApps
      /// </summary>
      RestartApps = 0x40,

      /// <summary>
      /// Force
      /// </summary>
      Force = 0x04,
      /// <summary>
      /// ForceIfHung
      /// </summary>
      ForceIfHung = 0x10,
    }

    #endregion Enumerations

    #region Structures

    /// <summary>
    /// Data structure for sending data over a windows message.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct COPYDATASTRUCT
    {
      /// <summary>
      /// Data ID.
      /// </summary>
      public int dwData;
      /// <summary>
      /// Data size.
      /// </summary>
      public int cbData;
      /// <summary>
      /// Data.
      /// </summary>
      public int lpData;
    }

    /// <summary>
    /// Data structure for retreiving information on files from the shell.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SHFILEINFO
    {
      public IntPtr hIcon;
      public IntPtr iIcon;
      public int dwAttributes;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
      public string szDisplayName;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
      public string szTypeName;
    };

    #endregion Structures

    #region Delegates

    /// <summary>
    /// Delegate for enumerating open Windows with EnumWindows method.
    /// </summary>
    /// <param name="hWnd">Window Handle.</param>
    /// <param name="lParam">lParam.</param>
    /// <returns>true if successful, otherwise false.</returns>
    public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    #endregion Delegates

    #region Interop

    [DllImport("shell32.dll")]
    private static extern IntPtr SHGetFileInfo(
      string pszPath,
      uint dwFileAttributes,
      ref SHFILEINFO psfi,
      uint cbSizeFileInfo,
      SHGFI uFlags);

    [DllImport("shell32.dll", CharSet = CharSet.Auto)]
    private static extern int ExtractIconEx(
        string lpszFile,
        int nIconIndex,
        IntPtr[] phIconLarge,
        IntPtr[] phIconSmall,
        int nIcons);

    [DllImport("user32.dll", EntryPoint = "DestroyIcon", SetLastError = true)]
    private static extern int DestroyIcon(
      IntPtr hIcon);

    [DllImport("user32.dll")]
    private static extern int EnumWindows(
      EnumWindowsProc ewp,
      IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ExitWindowsEx(
      ExitWindows flags,
      ShutdownReasons reasons);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr FindWindow(
      string className,
      string windowName);


    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern IntPtr SendMessageTimeout(
      IntPtr hWnd,
      int msg,
      IntPtr wParam,
      IntPtr lParam,
      SendMessageTimeoutFlags flags,
      int timeout,
      out IntPtr result);

    //[DllImport("user32.dll", SetLastError = false)]
    //private static extern IntPtr SendMessage(IntPtr windowHandle, int msg, IntPtr wordParam, IntPtr longParam);

    //[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    //private static extern int RegisterWindowMessage(string lpString);

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
    private static extern bool AttachThreadInput(
      int threadId,
      int threadIdTo,
      [MarshalAs(UnmanagedType.Bool)]
      bool attach);

    //[DllImport("user32.dll")]
    //[return: MarshalAs(UnmanagedType.Bool)]
    //private static extern bool IsWindowVisible(IntPtr hWnd);

    //[DllImport("user32.dll")]
    //[return: MarshalAs(UnmanagedType.Bool)]
    //private static extern bool IsIconic(IntPtr hWnd);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern int GetWindowText(
      IntPtr hWnd,
      StringBuilder lpString,
      int maxCount);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowThreadProcessId(
      IntPtr hWnd,
      out int processId);

    [DllImport("kernel32.dll")]
    private static extern int GetCurrentThreadId();

    #region Net API

    [DllImport("netapi32.dll", CharSet = CharSet.Auto, SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
    static extern int NetServerEnum(
      string ServerName, // must be null
      int Level,
      ref IntPtr Buf,
      int PrefMaxLen,
      out int EntriesRead,
      out int TotalEntries,
      int ServerType,
      string Domain, // null for login domain
      out int ResumeHandle
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

    public static Icon GetIconFor(string fileName)
    {
      IntPtr ptr;
      SHFILEINFO shinfo = new SHFILEINFO();

      //Use this to get the large Icon
      ptr = SHGetFileInfo(
        fileName,
        0,
        ref shinfo,
        (uint)Marshal.SizeOf(shinfo),
        SHGFI.Icon | SHGFI.LargeIcon);

      if (shinfo.hIcon != IntPtr.Zero)
        return Icon.FromHandle(shinfo.hIcon);
      else
        return null;
    }

    /// <summary>
    /// Extracts the icons from a resource.
    /// </summary>
    /// <param name="fileName">Name of the file to extract icons from.</param>
    /// <param name="index">The index to the icon inside the file.</param>
    /// <param name="large">The large icon.</param>
    /// <param name="small">The small icon.</param>
    /// <returns>true if successful, otherwise false.</returns>
    public static bool ExtractIcons(string fileName, int index, out Icon large, out Icon small)
    {
      IntPtr[] hLarge = new IntPtr[1] { IntPtr.Zero };
      IntPtr[] hSmall = new IntPtr[1] { IntPtr.Zero };

      large = null;
      small = null;

      try
      {
        int iconCount = ExtractIconEx(fileName, index, hLarge, hSmall, 1);

        if (iconCount > 0)
        {
          large = (Icon)Icon.FromHandle(hLarge[0]).Clone();
          small = (Icon)Icon.FromHandle(hSmall[0]).Clone();
          return true;
        }
        else
          return false;
      }
      catch
      {
        return false;
      }
      finally
      {
        foreach (IntPtr ptr in hLarge)
          if (ptr != IntPtr.Zero)
            DestroyIcon(ptr);

        foreach (IntPtr ptr in hSmall)
          if (ptr != IntPtr.Zero)
            DestroyIcon(ptr);
      }
    }

    /// <summary>
    /// Send a window message using the SendMessageTimeout method.
    /// </summary>
    /// <param name="hWnd">The window handle to send to.</param>
    /// <param name="msg">The message.</param>
    /// <param name="wParam">The wParam.</param>
    /// <param name="lParam">The lParam.</param>
    public static void SendWindowsMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
    {
      IntPtr result = IntPtr.Zero;
      
      SendMessageTimeout(hWnd, msg, wParam, lParam, SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 1000, out result);
      int lastError = Marshal.GetLastWin32Error();
      
      if (result == IntPtr.Zero && lastError != 0)
        throw new Win32Exception(lastError);
    }

    /// <summary>
    /// Send a window message using the SendMessageTimeout method.
    /// </summary>
    /// <param name="hWnd">The window handle to send to.</param>
    /// <param name="msg">The message.</param>
    /// <param name="wParam">The wParam.</param>
    /// <param name="lParam">The lParam.</param>
    public static void SendWindowsMessage(IntPtr hWnd, int msg, int wParam, int lParam)
    {
      SendWindowsMessage(hWnd, msg, new IntPtr(wParam), new IntPtr(lParam));
    }

    /// <summary>
    /// Get a handle to the current foreground window.
    /// </summary>
    /// <returns>Handle to foreground window.</returns>
    public static IntPtr ForegroundWindow()
    {
      return GetForegroundWindow();
    }

    /// <summary>
    /// Enumerates all windows by calling the supplied delegate for each window.
    /// </summary>
    /// <param name="ewp">Delegate to call for each window.</param>
    /// <param name="lParam">Used to identify this enumeration session.</param>
    /// <returns>Number of windows.</returns>
    public static int EnumerateWindows(EnumWindowsProc ewp, IntPtr lParam)
    {
      return EnumWindows(ewp, lParam);
    }

    /// <summary>
    /// Used to logoff, shutdown or reboot.
    /// </summary>
    /// <param name="flags">The type of exit to perform.</param>
    /// <param name="reasons">The reason for the exit.</param>
    /// <returns>true if successful, otherwise false.</returns>
    public static bool WindowsExit(ExitWindows flags, ShutdownReasons reasons)
    {
      return ExitWindowsEx(flags, reasons);
    }

    /// <summary>
    /// Get the window handle for a specified window class.
    /// </summary>
    /// <param name="className">Window class name.</param>
    /// <returns>Handle to a window.</returns>
    public static IntPtr FindWindowByClass(string className)
    {
      if (String.IsNullOrEmpty(className))
        throw new ArgumentNullException("className");

      IntPtr window = FindWindow(className, null);
      int lastError = Marshal.GetLastWin32Error();

      if (window == IntPtr.Zero && lastError != 0)
        throw new Win32Exception(lastError);

      return window;
    }

    /// <summary>
    /// Get the window handle for a specified window title.
    /// </summary>
    /// <param name="windowTitle">The window title.</param>
    /// <returns>Handle to a window.</returns>
    public static IntPtr FindWindowByTitle(string windowTitle)
    {
      if (String.IsNullOrEmpty(windowTitle))
        throw new ArgumentNullException("windowTitle");

      IntPtr window = FindWindow(null, windowTitle);
      int lastError = Marshal.GetLastWin32Error();

      if (window == IntPtr.Zero && lastError != 0)
        throw new Win32Exception(lastError);

      return window;
    }

    /// <summary>
    /// Get the window title for a specified window handle.
    /// </summary>
    /// <param name="hWnd">Handle to a window.</param>
    /// <returns>Window title.</returns>
    public static string GetWindowTitle(IntPtr hWnd)
    {
      int length = GetWindowTextLength(hWnd);
      int lastError = Marshal.GetLastWin32Error();

      if (lastError != 0)
        throw new Win32Exception(lastError);
      
      StringBuilder windowTitle = new StringBuilder(length + 1);
      
      GetWindowText(hWnd, windowTitle, windowTitle.Capacity);
      lastError = Marshal.GetLastWin32Error();

      if (lastError != 0)
        throw new Win32Exception(lastError);

      return windowTitle.ToString();
    }

    /// <summary>
    /// Takes a given window from whatever state it is in and makes it the foreground window.
    /// </summary>
    /// <param name="hWnd">Handle to window.</param>
    /// <param name="force">Force from a minimized or hidden state.</param>
    /// <returns>true if successful, otherwise false.</returns>
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
    /// Get a list of all computer names on the LAN, except for the local host.
    /// </summary>
    /// <returns>List of LAN computer names.</returns>
    [EnvironmentPermission(SecurityAction.Demand, Read = "COMPUTERNAME")]
    public static string[] GetNetworkComputers(bool includeLocalMachine)
    {
      try
      {
        List<string> networkComputers = new List<string>();

        const int MAX_PREFERRED_LENGTH = -1;

        int SV_TYPE_WORKSTATION = 1;
        //int SV_TYPE_SERVER = 2;
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
          SV_TYPE_WORKSTATION, //  | SV_TYPE_SERVER
          null,
          out resHandle);

        if (ret == 0)
        {
          for (int i = 0; i < totalEntries; i++)
          {
            tmpBuffer = new IntPtr((int)buffer + (i * sizeofINFO));
            _SERVER_INFO_100 svrInfo = (_SERVER_INFO_100)Marshal.PtrToStructure(tmpBuffer, typeof(_SERVER_INFO_100));

            if (includeLocalMachine || !svrInfo.sv100_name.Equals(Environment.MachineName, StringComparison.InvariantCultureIgnoreCase))
              networkComputers.Add(svrInfo.sv100_name);
          }
        }

        NetApiBufferFree(buffer);

        if (networkComputers.Count > 0)
          return networkComputers.ToArray();
      }
      catch
      {
      }
      
      return null;
    }

    /// <summary>
    /// Get an IntPtr pointing to any object.
    /// </summary>
    /// <param name="obj">Object to get pointer for.</param>
    /// <returns>Pointer to object.</returns>
    public static IntPtr VarPtr(object obj)
    {
      GCHandle handle = GCHandle.Alloc(obj, GCHandleType.Pinned);
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
