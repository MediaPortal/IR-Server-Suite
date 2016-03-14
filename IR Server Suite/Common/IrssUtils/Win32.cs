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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace IrssUtils
{
  /// <summary>
  /// Win32 native method class.
  /// </summary>
  [CLSCompliant(false)]
  public static class Win32
  {
    #region Constants

    private const int GCL_HICON = -14;
    private const int GCL_HICONSM = -34;


    private const int ICON_BIG = 1;
    private const int ICON_SMALL = 0;

    /// <summary>
    /// Maximum length of unmanaged Windows Path strings.
    /// </summary>
    private const int MAX_PATH = 260;

    /// <summary>
    /// Maximum length of unmanaged Typename.
    /// </summary>
    private const int MAX_TYPE = 80;

    private const int MINIMIZE_ALL = 419;
    private const int MINIMIZE_ALL_UNDO = 416;
    private const int WPF_RESTORETOMAXIMIZED = 2;


    /// <summary>Required to enable or disable the privileges in an access token.</summary>
    private const int TOKEN_ADJUST_PRIVILEGES = 0x20;
    /// <summary>Required to query an access token.</summary>
    private const int TOKEN_QUERY = 0x8;
    /// <summary>The privilege is enabled.</summary>
    private const int SE_PRIVILEGE_ENABLED = 0x2;
    /// <summary>Specifies that the function should search the system message-table resource(s) for the requested message.</summary>
    private const int FORMAT_MESSAGE_FROM_SYSTEM = 0x1000;
    /// <summary>Forces processes to terminate. When this flag is set, the system does not send the WM_QUERYENDSESSION and WM_ENDSESSION messages. This can cause the applications to lose data. Therefore, you should only use this flag in an emergency.</summary>
    private const int EWX_FORCE = 4;


    #endregion Constants

    #region Enumerations

    #region AppCommand enum

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

    #endregion

    #region ExitWindows enum

    /// <summary>
    /// Exit Windows method.
    /// </summary>
    [Flags]
    public enum ExitWindows
    {
      /// <summary>
      /// LogOff
      /// </summary>
      LogOff = 0,
      /// <summary>
      /// ShutDown
      /// </summary>
      ShutDown = 1,
      /// <summary>
      /// Reboot
      /// </summary>
      Reboot = 2,
      /// <summary>
      /// PowerOff
      /// </summary>
      PowerOff = 8,
    }

    #endregion

    #region GWL enum

    /// <summary>
    /// GWL.
    /// </summary>
    public enum GWL
    {
      /// <summary>
      /// WndProc.
      /// </summary>
      GWL_WNDPROC = (-4),
      /// <summary>
      /// HInstance.
      /// </summary>
      GWL_HINSTANCE = (-6),
      /// <summary>
      /// hWnd Parent.
      /// </summary>
      GWL_HWNDPARENT = (-8),
      /// <summary>
      /// Style.
      /// </summary>
      GWL_STYLE = (-16),
      /// <summary>
      /// Extended Style.
      /// </summary>
      GWL_EXSTYLE = (-20),
      /// <summary>
      /// User Data.
      /// </summary>
      GWL_USERDATA = (-21),
      /// <summary>
      /// ID.
      /// </summary>
      GWL_ID = (-12),
    }

    #endregion

    #region SendMessageTimeoutFlags enum

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

    #endregion

    #region ShutdownReasons enum

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

    #endregion

    #region SysCommand enum

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

    #endregion

    #region WindowExStyles enum

    /// <summary>
    /// Win32 Window Extended Styles.
    /// </summary>
    [Flags]
    public enum WindowExStyles
    {
      /// <summary>
      /// Specifies that a window created with this style accepts drag-drop files.
      /// </summary>
      WS_EX_ACCEPTFILES = 0x00000010,
      /// <summary>
      /// Forces a top-level window onto the taskbar when the window is visible.
      /// </summary>
      WS_EX_APPWINDOW = 0x00040000,
      /// <summary>
      /// Specifies that a window has a border with a sunken edge.
      /// </summary>
      WS_EX_CLIENTEDGE = 0x00000200,
      /// <summary>
      /// Windows XP: Paints all descendants of a window in bottom-to-top painting order using double-buffering. For more information, see Remarks. This cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC.
      /// </summary>
      WS_EX_COMPOSITED = 0x02000000,
      /// <summary>
      /// Includes a question mark in the title bar of the window. When the user clicks the question mark, the cursor changes to a question mark with a pointer. If the user then clicks a child window, the child receives a WM_HELP message. The child window should pass the message to the parent window procedure, which should call the WinHelp function using the HELP_WM_HELP command. The Help application displays a pop-up window that typically contains help for the child window.
      /// WS_EX_CONTEXTHELP cannot be used with the WS_MAXIMIZEBOX or WS_MINIMIZEBOX styles.
      /// </summary>
      WS_EX_CONTEXTHELP = 0x00000400,
      /// <summary>
      /// The window itself contains child windows that should take part in dialog box navigation. If this style is specified, the dialog manager recurses into children of this window when performing navigation operations such as handling the TAB key, an arrow key, or a keyboard mnemonic.
      /// </summary>
      WS_EX_CONTROLPARENT = 0x00010000,
      /// <summary>
      /// Creates a window that has a double border; the window can, optionally, be created with a title bar by specifying the WS_CAPTION style in the dwStyle parameter.
      /// </summary>
      WS_EX_DLGMODALFRAME = 0x00000001,
      /// <summary>
      /// Windows 2000/XP: Creates a layered window. Be aware that this cannot be used for child windows. Also, this cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC.
      /// </summary>
      WS_EX_LAYERED = 0x00080000,
      /// <summary>
      /// Arabic and Hebrew versions of Windows 98/Me, Windows 2000/XP: Creates a window whose horizontal origin is on the right edge. Increasing horizontal values advance to the left.
      /// </summary>
      WS_EX_LAYOUTRTL = 0x00400000,
      /// <summary>
      /// Creates a window that has generic left-aligned properties. This is the default.
      /// </summary>
      WS_EX_LEFT = 0x00000000,
      /// <summary>
      /// If the shell language is Hebrew, Arabic, or another language that supports reading order alignment, the vertical scroll bar (if present) is to the left of the client area. For other languages, the style is ignored.
      /// </summary>
      WS_EX_LEFTSCROLLBAR = 0x00004000,
      /// <summary>
      /// The window text is displayed using left-to-right reading-order properties. This is the default.
      /// </summary>
      WS_EX_LTRREADING = 0x00000000,
      /// <summary>
      /// Creates a multiple-document interface (MDI) child window.
      /// </summary>
      WS_EX_MDICHILD = 0x00000040,
      /// <summary>
      /// Windows 2000/XP: A top-level window created with this style does not become the foreground window when the user clicks it. The system does not bring this window to the foreground when the user minimizes or closes the foreground window.
      /// To activate the window, use the SetActiveWindow or SetForegroundWindow function.
      /// The window does not appear on the taskbar by default. To force the window to appear on the taskbar, use the WS_EX_APPWINDOW style.
      /// </summary>
      WS_EX_NOACTIVATE = 0x08000000,
      /// <summary>
      /// Windows 2000/XP: A window created with this style does not pass its window layout to its child windows.
      /// </summary>
      WS_EX_NOINHERITLAYOUT = 0x00100000,
      /// <summary>
      /// Specifies that a child window created with this style does not send the WM_PARENTNOTIFY message to its parent window when it is created or destroyed.
      /// </summary>
      WS_EX_NOPARENTNOTIFY = 0x00000004,
      /// <summary>
      /// Combines the WS_EX_CLIENTEDGE and WS_EX_WINDOWEDGE styles.
      /// </summary>
      WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE,
      /// <summary>
      /// Combines the WS_EX_WINDOWEDGE, WS_EX_TOOLWINDOW, and WS_EX_TOPMOST styles.
      /// </summary>
      WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST,
      /// <summary>
      /// The window has generic "right-aligned" properties. This depends on the window class. This style has an effect only if the shell language is Hebrew, Arabic, or another language that supports reading-order alignment; otherwise, the style is ignored.
      /// Using the WS_EX_RIGHT style for static or edit controls has the same effect as using the SS_RIGHT or ES_RIGHT style, respectively. Using this style with button controls has the same effect as using BS_RIGHT and BS_RIGHTBUTTON styles.
      /// </summary>
      WS_EX_RIGHT = 0x00001000,
      /// <summary>
      /// Vertical scroll bar (if present) is to the right of the client area. This is the default.
      /// </summary>
      WS_EX_RIGHTSCROLLBAR = 0x00000000,
      /// <summary>
      /// If the shell language is Hebrew, Arabic, or another language that supports reading-order alignment, the window text is displayed using right-to-left reading-order properties. For other languages, the style is ignored.
      /// </summary>
      WS_EX_RTLREADING = 0x00002000,
      /// <summary>
      /// Creates a window with a three-dimensional border style intended to be used for items that do not accept user input.
      /// </summary>
      WS_EX_STATICEDGE = 0x00020000,
      /// <summary>
      /// Creates a tool window; that is, a window intended to be used as a floating toolbar. A tool window has a title bar that is shorter than a normal title bar, and the window title is drawn using a smaller font. A tool window does not appear in the taskbar or in the dialog that appears when the user presses ALT+TAB. If a tool window has a system menu, its icon is not displayed on the title bar. However, you can display the system menu by right-clicking or by typing ALT+SPACE.
      /// </summary>
      WS_EX_TOOLWINDOW = 0x00000080,
      /// <summary>
      /// Specifies that a window created with this style should be placed above all non-topmost windows and should stay above them, even when the window is deactivated. To add or remove this style, use the SetWindowPos function.
      /// </summary>
      WS_EX_TOPMOST = 0x00000008,
      /// <summary>
      /// Specifies that a window created with this style should not be painted until siblings beneath the window (that were created by the same thread) have been painted. The window appears transparent because the bits of underlying sibling windows have already been painted.
      /// To achieve transparency without these restrictions, use the SetWindowRgn function.
      /// </summary>
      WS_EX_TRANSPARENT = 0x00000020,
      /// <summary>
      /// Specifies that a window has a border with a raised edge.
      /// </summary>
      WS_EX_WINDOWEDGE = 0x00000100
    }

    #endregion

    #region WindowsMessage enum

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

    #endregion

    #region WindowStyles enum

    /// <summary>
    /// Win32 Window Styles.
    /// </summary>
    [Flags]
    public enum WindowStyles : uint
    {
      /// <summary>
      /// Overlapped.
      /// </summary>
      WS_OVERLAPPED = 0x00000000,
      /// <summary>
      /// Popup.
      /// </summary>
      WS_POPUP = 0x80000000,
      /// <summary>
      /// Child.
      /// </summary>
      WS_CHILD = 0x40000000,
      /// <summary>
      /// Minimize.
      /// </summary>
      WS_MINIMIZE = 0x20000000,
      /// <summary>
      /// Visible.
      /// </summary>
      WS_VISIBLE = 0x10000000,
      /// <summary>
      /// Disabled.
      /// </summary>
      WS_DISABLED = 0x08000000,
      /// <summary>
      /// Clip Siblings.
      /// </summary>
      WS_CLIPSIBLINGS = 0x04000000,
      /// <summary>
      /// Clip Children.
      /// </summary>
      WS_CLIPCHILDREN = 0x02000000,
      /// <summary>
      /// Maximize.
      /// </summary>
      WS_MAXIMIZE = 0x01000000,
      /// <summary>
      /// Border.
      /// </summary>
      WS_BORDER = 0x00800000,
      /// <summary>
      /// Dialog Frame.
      /// </summary>
      WS_DLGFRAME = 0x00400000,
      /// <summary>
      /// Vertical Scroll.
      /// </summary>
      WS_VSCROLL = 0x00200000,
      /// <summary>
      /// Horizontal Scroll.
      /// </summary>
      WS_HSCROLL = 0x00100000,
      /// <summary>
      /// System Menu.
      /// </summary>
      WS_SYSMENU = 0x00080000,
      /// <summary>
      /// Thick Frame.
      /// </summary>
      WS_THICKFRAME = 0x00040000,
      /// <summary>
      /// Group.
      /// </summary>
      WS_GROUP = 0x00020000,
      /// <summary>
      /// Tab Stop.
      /// </summary>
      WS_TABSTOP = 0x00010000,

      /// <summary>
      /// Minimize Box.
      /// </summary>
      WS_MINIMIZEBOX = 0x00020000,
      /// <summary>
      /// Maximize Box.
      /// </summary>
      WS_MAXIMIZEBOX = 0x00010000,

      /// <summary>
      /// Caption (WS_BORDER | WS_DLGFRAME).
      /// </summary>
      WS_CAPTION = WS_BORDER | WS_DLGFRAME,
      /// <summary>
      /// Tiled (WS_OVERLAPPED).
      /// </summary>
      WS_TILED = WS_OVERLAPPED,
      /// <summary>
      /// Iconic (WS_MINIMIZE).
      /// </summary>
      WS_ICONIC = WS_MINIMIZE,
      /// <summary>
      /// Size Box (WS_THICKFRAME).
      /// </summary>
      WS_SIZEBOX = WS_THICKFRAME,
      /// <summary>
      /// Tiled Window (WS_OVERLAPPEDWINDOW).
      /// </summary>
      WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,

      /// <summary>
      /// Overlapped Window (WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX).
      /// </summary>
      WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
      /// <summary>
      /// Popup Window (WS_POPUP | WS_BORDER | WS_SYSMENU).
      /// </summary>
      WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
      /// <summary>
      /// Child Window (WS_CHILD).
      /// </summary>
      WS_CHILDWINDOW = WS_CHILD,
    }

    #endregion

    #region Nested type: SHGFI

    [Flags]
    private enum SHGFI
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

    #endregion

    #region Nested type: WindowShowStyle

    /// <summary>
    /// Enumeration of the different ways of showing a window using ShowWindow.
    /// </summary>
    private enum WindowShowStyle
    {
      /// <summary>Hides the window and activates another window.</summary>
      /// <remarks>See SW_HIDE</remarks>
      Hide = 0,
      /// <summary>Activates and displays a window. If the window is minimized
      /// or maximized, the system restores it to its original size and
      /// position. An application should specify this flag when displaying
      /// the window for the first time.</summary>
      /// <remarks>See SW_SHOWNORMAL</remarks>
      ShowNormal = 1,
      /// <summary>Activates the window and displays it as a minimized window.</summary>
      /// <remarks>See SW_SHOWMINIMIZED</remarks>
      ShowMinimized = 2,
      /// <summary>Activates the window and displays it as a maximized window.</summary>
      /// <remarks>See SW_SHOWMAXIMIZED</remarks>
      ShowMaximized = 3,
      /// <summary>Maximizes the specified window.</summary>
      /// <remarks>See SW_MAXIMIZE</remarks>
      Maximize = 3,
      /// <summary>Displays a window in its most recent size and position.
      /// This value is similar to "ShowNormal", except the window is not
      /// actived.</summary>
      /// <remarks>See SW_SHOWNOACTIVATE</remarks>
      ShowNormalNoActivate = 4,
      /// <summary>Activates the window and displays it in its current size
      /// and position.</summary>
      /// <remarks>See SW_SHOW</remarks>
      Show = 5,
      /// <summary>Minimizes the specified window and activates the next
      /// top-level window in the Z order.</summary>
      /// <remarks>See SW_MINIMIZE</remarks>
      Minimize = 6,
      /// <summary>Displays the window as a minimized window. This value is
      /// similar to "ShowMinimized", except the window is not activated.</summary>
      /// <remarks>See SW_SHOWMINNOACTIVE</remarks>
      ShowMinNoActivate = 7,
      /// <summary>Displays the window in its current size and position. This
      /// value is similar to "Show", except the window is not activated.</summary>
      /// <remarks>See SW_SHOWNA</remarks>
      ShowNoActivate = 8,
      /// <summary>Activates and displays the window. If the window is
      /// minimized or maximized, the system restores it to its original size
      /// and position. An application should specify this flag when restoring
      /// a minimized window.</summary>
      /// <remarks>See SW_RESTORE</remarks>
      Restore = 9,
      /// <summary>Sets the show state based on the SW_ value specified in the
      /// STARTUPINFO structure passed to the CreateProcess function by the
      /// program that started the application.</summary>
      /// <remarks>See SW_SHOWDEFAULT</remarks>
      ShowDefault = 10,
      /// <summary>Windows 2000/XP: Minimizes a window, even if the thread
      /// that owns the window is hung. This flag should only be used when
      /// minimizing windows from a different thread.</summary>
      /// <remarks>See SW_FORCEMINIMIZE</remarks>
      ForceMinimized = 11
    }

    #endregion

    #region Nested type: EFileAttributes

    [Flags]
    public enum EFileAttributes : uint
    {
      Readonly = 0x00000001,
      Hidden = 0x00000002,
      System = 0x00000004,
      Directory = 0x00000010,
      Archive = 0x00000020,
      Device = 0x00000040,
      Normal = 0x00000080,
      Temporary = 0x00000100,
      SparseFile = 0x00000200,
      ReparsePoint = 0x00000400,
      Compressed = 0x00000800,
      Offline = 0x00001000,
      NotContentIndexed = 0x00002000,
      Encrypted = 0x00004000,
      Write_Through = 0x80000000,
      Overlapped = 0x40000000,
      NoBuffering = 0x20000000,
      RandomAccess = 0x10000000,
      SequentialScan = 0x08000000,
      DeleteOnClose = 0x04000000,
      BackupSemantics = 0x02000000,
      PosixSemantics = 0x01000000,
      OpenReparsePoint = 0x00200000,
      OpenNoRecall = 0x00100000,
      FirstPipeInstance = 0x00080000,
    }

    #endregion


    #endregion Enumerations

    #region Structures

    #region Nested type: COPYDATASTRUCT

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
      public IntPtr lpData;
    }

    #endregion

    #region Nested type: POINTAPI

    [StructLayout(LayoutKind.Sequential)]
    private struct POINTAPI
    {
      public int x;
      public int y;
    }

    #endregion

    #region Nested type: RECT

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
      public int left;
      public int top;
      public int right;
      public int bottom;
    }

    #endregion

    #region Nested type: SHFILEINFO

    /// <summary>
    /// Data structure for retreiving information on files from the shell.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SHFILEINFO
    {
      /// <summary>
      /// hIcon;
      /// </summary>
      public IntPtr hIcon;

      /// <summary>
      /// iIcon.
      /// </summary>
      public IntPtr iIcon;

      /// <summary>
      /// Attributes.
      /// </summary>
      public int dwAttributes;

      /// <summary>
      /// Display Name.
      /// </summary>
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
      public string szDisplayName;

      /// <summary>
      /// Type Name.
      /// </summary>
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
      public string szTypeName;
    } ;

    #endregion

    #region Nested type: WINDOWPLACEMENT

    [StructLayout(LayoutKind.Sequential)]
    private struct WINDOWPLACEMENT
    {
      public int length;
      public int flags;
      public WindowShowStyle showCmd;
      public POINTAPI ptMinPosition;
      public POINTAPI ptMaxPosition;
      public RECT rcNormalPosition;
    }

    #endregion

    #region Nested type: TOKEN_PRIVILEGES
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TOKEN_PRIVILEGES
    {
      /// <summary>
      /// Specifies the number of entries in the Privileges array.
      /// </summary>
      public int PrivilegeCount;
      /// <summary>
      /// Specifies an array of LUID_AND_ATTRIBUTES structures. Each structure contains the LUID and attributes of a privilege.
      /// </summary>
      public LUID_AND_ATTRIBUTES Privileges;
    }
    #endregion

    #region Nested type: LUID
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct LUID
    {
      /// <summary>
      /// The low order part of the 64 bit value.
      /// </summary>
      public int LowPart;
      /// <summary>
      /// The high order part of the 64 bit value.
      /// </summary>
      public int HighPart;
    }
    #endregion

    #region Nested type: LUID_AND_ATTRIBUTES
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct LUID_AND_ATTRIBUTES
    {
      /// <summary>
      /// Specifies an LUID value.
      /// </summary>
      public LUID pLuid;
      /// <summary>
      /// Specifies attributes of the LUID. This value contains up to 32 one-bit flags. Its meaning is dependent on the definition and use of the LUID.
      /// </summary>
      public int Attributes;
    }
    #endregion

    #region Nested type: DeviceInfoData

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DeviceInfoData
    {
      public int Size;
      public Guid Class;
      public int DevInst;
      public IntPtr Reserved;
    }

    #endregion

    #region Nested type: DeviceInterfaceData

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DeviceInterfaceData
    {
      public int Size;
      public Guid Class;
      public int Flags;
      public IntPtr Reserved;
    }

    #endregion

    #region Nested type: DeviceInterfaceDetailData

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct DeviceInterfaceDetailData
    {
      public int Size;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
      public string DevicePath;
    }

    #endregion

    #endregion Structures

    #region Delegates

    /// <summary>
    /// Delegate for enumerating open Windows with EnumWindows method.
    /// </summary>
    /// <param name="hWnd">Window Handle.</param>
    /// <param name="lParam">lParam.</param>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    #endregion

    #region Interop

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetWindowPlacement(
      IntPtr hWnd,
      ref WINDOWPLACEMENT lpwndpl);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ShowWindow(
      IntPtr hWnd,
      WindowShowStyle style);

    [DllImport("user32.dll")]
    private static extern IntPtr GetDesktopWindow();

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

    [DllImport("user32.dll")]
    private static extern int DestroyIcon(
      IntPtr hIcon);

    [DllImport("user32.dll")]
    private static extern int EnumWindows(
      EnumWindowsProc ewp,
      IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", EntryPoint = "ExitWindowsEx", CharSet = CharSet.Ansi)]
    private static extern int ExitWindowsEx(
      int uFlags,
      int dwReserved);

    [DllImport("advapi32.dll", EntryPoint = "OpenProcessToken", CharSet = CharSet.Ansi)]
    private static extern int OpenProcessToken(
      IntPtr ProcessHandle,
      int DesiredAccess,
      ref IntPtr TokenHandle);

    [DllImport("advapi32.dll", EntryPoint = "LookupPrivilegeValueA", CharSet = CharSet.Ansi)]
    private static extern int LookupPrivilegeValue(
      string lpSystemName,
      string lpName,
      ref LUID lpLuid);

    [DllImport("advapi32.dll", EntryPoint = "AdjustTokenPrivileges", CharSet = CharSet.Ansi)]
    private static extern int AdjustTokenPrivileges(
      IntPtr TokenHandle,
      int DisableAllPrivileges,
      ref TOKEN_PRIVILEGES NewState,
      int BufferLength,
      ref TOKEN_PRIVILEGES PreviousState,
      ref int ReturnLength);

    [DllImport("kernel32.dll", EntryPoint = "LoadLibraryA", CharSet = CharSet.Ansi)]
    private static extern IntPtr LoadLibrary(string lpLibFileName);

    [DllImport("kernel32.dll", EntryPoint = "FreeLibrary", CharSet = CharSet.Ansi)]
    private static extern int FreeLibrary(IntPtr hLibModule);

    [DllImport("kernel32.dll", EntryPoint = "GetProcAddress", CharSet = CharSet.Ansi)]
    private static extern IntPtr GetProcAddress(
      IntPtr hModule,
      string lpProcName);

    [DllImport("user32.dll", EntryPoint = "FormatMessageA", CharSet = CharSet.Ansi)]
    private static extern int FormatMessage(
      int dwFlags,
      IntPtr lpSource,
      int dwMessageId,
      int dwLanguageId,
      StringBuilder lpBuffer,
      int nSize,
      int Arguments);

    [DllImport("user32.dll")]
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

    //[DllImport("user32.dll")]
    //static extern IntPtr SendMessage(IntPtr windowHandle, int msg, IntPtr wordParam, IntPtr longParam);

    //[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    //static extern int RegisterWindowMessage(string lpString);

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
    //static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool AttachThreadInput(
      int threadId,
      int threadIdTo,
      [MarshalAs(UnmanagedType.Bool)] bool attach);

    /*
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool IsIconic(IntPtr hWnd);
    */

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern int GetWindowText(
      IntPtr hWnd,
      StringBuilder lpString,
      int maxCount);

    [DllImport("user32.dll")]
    private static extern int GetWindowThreadProcessId(
      IntPtr hWnd,
      out int processId);

    [DllImport("kernel32.dll")]
    private static extern int GetCurrentThreadId();

    [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
    private static extern IntPtr GetWindowLongPtr32(
      IntPtr hWnd,
      GWL nIndex);

    [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
    private static extern IntPtr GetWindowLongPtr64(
      IntPtr hWnd,
      GWL nIndex);

    [DllImport("user32.dll", EntryPoint = "GetClassLong")]
    private static extern uint GetClassLongPtr32(
      IntPtr hWnd,
      int nIndex);

    [DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
    private static extern IntPtr GetClassLongPtr64(
      IntPtr hWnd,
      int nIndex);

    [DllImport("user32.dll")]
    private static extern int GetClassName(
      IntPtr hWnd,
      StringBuilder lpClassName,
      int nMaxCount);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool IsChild(
      IntPtr hWndParent,
      IntPtr hWndChild);

    [DllImport("user32.dll")]
    private static extern IntPtr GetParent(
      IntPtr hWnd);

    /*
    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    static extern bool PeekMessage(
      out System.Windows.Forms.Message msg,
      IntPtr hWnd,
      uint messageFilterMin,
      uint messageFilterMax,
      uint flags);
    */

    [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsWow64Process(
         [In] IntPtr hProcess,
         [Out] out bool lpSystemInfo);


    #region HID

    [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern SafeFileHandle CreateFile(
      String fileName,
      [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
      [MarshalAs(UnmanagedType.U4)] FileShare fileShare,
      IntPtr securityAttributes,
      [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
      [MarshalAs(UnmanagedType.U4)] EFileAttributes flags,
      IntPtr template);

    [DllImport("hid")]
    public static extern void HidD_GetHidGuid(
      ref Guid guid);

    [DllImport("setupapi", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr SetupDiGetClassDevs(
      ref Guid ClassGuid,
      int Enumerator,
      IntPtr hwndParent,
      int Flags);

    [DllImport("setupapi", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetupDiEnumDeviceInfo(
      IntPtr handle,
      int Index,
      ref DeviceInfoData deviceInfoData);

    [DllImport("setupapi", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetupDiEnumDeviceInterfaces(
      IntPtr handle,
      ref DeviceInfoData deviceInfoData,
      ref Guid guidClass,
      int MemberIndex,
      ref DeviceInterfaceData deviceInterfaceData);

    [DllImport("setupapi", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetupDiGetDeviceInterfaceDetail(
      IntPtr handle,
      ref DeviceInterfaceData deviceInterfaceData,
      IntPtr unused1,
      int unused2,
      ref uint requiredSize,
      IntPtr unused3);

    [DllImport("setupapi", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetupDiGetDeviceInterfaceDetail(
      IntPtr handle,
      ref DeviceInterfaceData deviceInterfaceData,
      ref DeviceInterfaceDetailData deviceInterfaceDetailData,
      uint detailSize,
      IntPtr unused1,
      IntPtr unused2);

    [DllImport("setupapi")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetupDiDestroyDeviceInfoList(IntPtr handle);

    #endregion

    #endregion Interop

    #region Methods

    /// <summary>
    /// Gets the parent window.
    /// </summary>
    /// <param name="child">The child.</param>
    /// <returns>Handle to parent window.</returns>
    public static IntPtr GetParentWindow(IntPtr child)
    {
      return GetParent(child);
    }

    /// <summary>
    /// Determines whether one window is a child of another.
    /// </summary>
    /// <param name="parent">The parent.</param>
    /// <param name="child">The child.</param>
    /// <returns><c>true</c> if the window is a child of the parent; otherwise, <c>false</c>.</returns>
    public static bool IsWindowChild(IntPtr parent, IntPtr child)
    {
      return IsChild(parent, child);
    }

    /// <summary>
    /// Gets the desktop window handle.
    /// </summary>
    /// <returns></returns>
    public static IntPtr GetDesktopWindowHandle()
    {
      return GetDesktopWindow();
    }

    /// <summary>
    /// Gets the icon for a supplied file.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    /// <returns>File icon.</returns>
    [Obsolete("Method has been replaces by GetIconFromFile() and will be removed soon.")]
    public static Icon GetIconFor(string fileName)
    {
      if (String.IsNullOrEmpty(fileName))
        throw new ArgumentNullException("fileName");

      SHFILEINFO shinfo = new SHFILEINFO();

      SHGetFileInfo(fileName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI.Icon | SHGFI.LargeIcon);

      Icon icon = null;

      if (shinfo.hIcon != IntPtr.Zero)
      {
        icon = Icon.FromHandle(shinfo.hIcon);
        //DestroyIcon(shinfo.hIcon);
      }

      return icon;
    }


    /// <summary>
    /// Gets the icon for a supplied file.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    /// <returns>File icon.</returns>
    public static Icon GetIconFromFile(string fileName)
    {
      if (String.IsNullOrEmpty(fileName))
        return null;

      if (!File.Exists(fileName))
        return null;

      Icon icon = Icon.ExtractAssociatedIcon(fileName);
      if (icon == null)
        return null;

      return icon;
    }

    /// <summary>
    /// Gets the icon for a supplied file as Image.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    /// <returns>File icon as Image.</returns>
    public static Image GetImageFromFile(string fileName)
    {
      Icon icon = GetIconFromFile(fileName);

      if (icon == null)
        icon = ExclamationMark;

      return icon.ToBitmap();
    }

    public static Icon ExclamationMark
    {
      get
      {
        if (_exclamationMark == null)
        {
          Icon large;
          Icon small;

          string folder = Environment.GetFolderPath(Environment.SpecialFolder.System);
          string file = Path.Combine(folder, "user32.dll");
          Win32.ExtractIcons(file, 1, out large, out small);

          _exclamationMark = large;
        }

        return _exclamationMark;
      }
    }

    private static Icon _exclamationMark;


    /// <summary>
    /// Gets the window icon.
    /// </summary>
    /// <param name="handle">The window handle to get the icon for.</param>
    /// <returns>Window icon.</returns>
    public static Icon GetWindowIcon(IntPtr handle)
    {
      IntPtr icon = IntPtr.Zero;

      SendMessageTimeout(handle, (int)WindowsMessage.WM_GETICON, new IntPtr(ICON_BIG), IntPtr.Zero,
                         SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 1000, out icon);

      if (icon == IntPtr.Zero)
        icon = GetClassLongPtr(handle, GCL_HICON);

      if (icon == IntPtr.Zero)
        SendMessageTimeout(handle, (int)WindowsMessage.WM_QUERYDRAGICON, IntPtr.Zero, IntPtr.Zero,
                           SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 1000, out icon);

      if (icon != IntPtr.Zero)
        return Icon.FromHandle(icon);

      return null;
    }

    /// <summary>
    /// Extracts the icons from a resource.
    /// </summary>
    /// <param name="fileName">Name of the file to extract icons from.</param>
    /// <param name="index">The index to the icon inside the file.</param>
    /// <param name="large">The large icon.</param>
    /// <param name="small">The small icon.</param>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
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

      return false;
    }

    /// <summary>
    /// Send a window message using the SendMessageTimeout method.
    /// </summary>
    /// <param name="hWnd">The window handle to send to.</param>
    /// <param name="msg">The message.</param>
    /// <param name="wParam">The wParam.</param>
    /// <param name="lParam">The lParam.</param>
    /// <returns>Result of message.</returns>
    public static IntPtr SendWindowsMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
    {
      IntPtr result = IntPtr.Zero;

      IntPtr returnValue = SendMessageTimeout(hWnd, msg, wParam, lParam, SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 1000,
                                              out result);
      int lastError = Marshal.GetLastWin32Error();

      if (returnValue == IntPtr.Zero && lastError != 0)
        throw new Win32Exception(lastError);

      return result;
    }

    /// <summary>
    /// Send a window message using the SendMessageTimeout method.
    /// </summary>
    /// <param name="hWnd">The window handle to send to.</param>
    /// <param name="msg">The message.</param>
    /// <param name="wParam">The wParam.</param>
    /// <param name="lParam">The lParam.</param>
    /// <returns>Result of message.</returns>
    public static IntPtr SendWindowsMessage(IntPtr hWnd, int msg, int wParam, int lParam)
    {
      return SendWindowsMessage(hWnd, msg, new IntPtr(wParam), new IntPtr(lParam));
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
    /// Gets the name of a class from a handle.
    /// </summary>
    /// <param name="hWnd">The handle to retreive the class name for.</param>
    /// <returns>The class name.</returns>
    public static string GetClassName(IntPtr hWnd)
    {
      StringBuilder name = new StringBuilder(255);
      GetClassName(hWnd, name, name.Capacity);

      return name.ToString();
    }

    /// <summary>
    /// Used to logoff, shutdown or reboot.
    /// </summary>
    /// <param name="flags">The type of exit to perform.</param>
    /// <param name="reasons">The reason for the exit.</param>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public static bool WindowsExit(ExitWindows flags, ShutdownReasons reasons)
    {
      EnableToken("SeShutdownPrivilege");
      return ExitWindowsEx((int)flags, (int)reasons) != 0;
    }

    /// <summary>
    /// Tries to enable the specified privilege.
    /// </summary>
    /// <param name="privilege">The privilege to enable.</param>
    /// <exception cref="PrivilegeException">There was an error while requesting a required privilege.</exception>
    /// <remarks>Thanks to Michael S. Muegel for notifying us about a bug in this code.</remarks>
    private static void EnableToken(string privilege)
    {
      if (Environment.OSVersion.Platform != PlatformID.Win32NT || !CheckEntryPoint("advapi32.dll", "AdjustTokenPrivileges"))
        return;
      IntPtr tokenHandle = IntPtr.Zero;
      LUID privilegeLUID = new LUID();
      TOKEN_PRIVILEGES newPrivileges = new TOKEN_PRIVILEGES();
      TOKEN_PRIVILEGES tokenPrivileges;
      if (OpenProcessToken(Process.GetCurrentProcess().Handle, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, ref tokenHandle) == 0)
        throw new PrivilegeException(FormatError(Marshal.GetLastWin32Error()));
      if (LookupPrivilegeValue("", privilege, ref privilegeLUID) == 0)
        throw new PrivilegeException(FormatError(Marshal.GetLastWin32Error()));
      tokenPrivileges.PrivilegeCount = 1;
      tokenPrivileges.Privileges.Attributes = SE_PRIVILEGE_ENABLED;
      tokenPrivileges.Privileges.pLuid = privilegeLUID;
      int size = 4;
      if (AdjustTokenPrivileges(tokenHandle, 0, ref tokenPrivileges, 4 + (12 * tokenPrivileges.PrivilegeCount), ref newPrivileges, ref size) == 0)
        throw new PrivilegeException(FormatError(Marshal.GetLastWin32Error()));
    }
    /// <summary>
    /// Checks whether a specified method exists on the local computer.
    /// </summary>
    /// <param name="library">The library that holds the method.</param>
    /// <param name="method">The entry point of the requested method.</param>
    /// <returns>True if the specified method is present, false otherwise.</returns>
    private static bool CheckEntryPoint(string library, string method)
    {
      IntPtr libPtr = LoadLibrary(library);
      if (!libPtr.Equals(IntPtr.Zero))
      {
        if (!GetProcAddress(libPtr, method).Equals(IntPtr.Zero))
        {
          FreeLibrary(libPtr);
          return true;
        }
        FreeLibrary(libPtr);
      }
      return false;
    }

    /// <summary>
    /// Formats an error number into an error message.
    /// </summary>
    /// <param name="number">The error number to convert.</param>
    /// <returns>A string representation of the specified error number.</returns>
    private static string FormatError(int number)
    {
      try
      {
        StringBuilder buffer = new StringBuilder(255);
        FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, IntPtr.Zero, number, 0, buffer, buffer.Capacity, 0);
        return buffer.ToString();
      }
      catch (Exception)
      {
        return "Unspecified error [" + number.ToString() + "]";
      }
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

      return FindWindow(className, null);
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

      return FindWindow(null, windowTitle);
    }

    /// <summary>
    /// Get the window title for a specified window handle.
    /// </summary>
    /// <param name="hWnd">Handle to a window.</param>
    /// <returns>Window title.</returns>
    public static string GetWindowTitle(IntPtr hWnd)
    {
      int length = GetWindowTextLength(hWnd);
      if (length == 0)
        return null;

      StringBuilder windowTitle = new StringBuilder(length + 1);

      GetWindowText(hWnd, windowTitle, windowTitle.Capacity);

      return windowTitle.ToString();
    }

    /// <summary>
    /// Takes a given window from whatever state it is in and makes it the foreground window.
    /// </summary>
    /// <param name="hWnd">Handle to window.</param>
    /// <param name="force">Force from a minimized or hidden state.</param>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public static bool SetForegroundWindow(IntPtr hWnd, bool force)
    {
#if TRACE
      Trace.WriteLine(String.Format("SetForegroundWindow({0}, {1})", hWnd, force));
#endif

      IntPtr fgWindow = GetForegroundWindow();

      if (hWnd == fgWindow)
      {
#if TRACE
        Trace.WriteLine("SetForegroundWindow: hWnd == fgWindow");
#endif
        return true;
      }

      bool setAttempt = SetForegroundWindow(hWnd);
#if TRACE
      Trace.WriteLine(String.Format("SetForegroundWindow: setAttempt == {0}", setAttempt));
#endif
      if (!force || setAttempt)
        return setAttempt;

      if (fgWindow == IntPtr.Zero)
      {
#if TRACE
        Trace.WriteLine("SetForegroundWindow: fgWindow == IntPtr.Zero");
#endif
        return false;
      }

      int processId;
      int fgWindowPID = GetWindowThreadProcessId(fgWindow, out processId);

      if (fgWindowPID == -1)
      {
#if TRACE
        Trace.WriteLine("SetForegroundWindow: fgWindowPID == -1");
#endif
        return false;
      }

      // If we don't attach successfully to the windows thread then we're out of options
      int curThreadID = GetCurrentThreadId();
      bool attached = AttachThreadInput(curThreadID, fgWindowPID, true);
      int lastError = Marshal.GetLastWin32Error();

      if (!attached)
      {
#if TRACE
        Trace.WriteLine(String.Format("SetForegroundWindow: !AttachThreadInput, LastError = {0}", lastError));
#endif
        return false;
      }

      SetForegroundWindow(hWnd);
      BringWindowToTop(hWnd);
      SetFocus(hWnd);

      // Detach
      AttachThreadInput(curThreadID, fgWindowPID, false);

#if TRACE
      Trace.WriteLine("SetForegroundWindow: Done");
#endif

      // We've done all that we can so base our return value on whether we have succeeded or not
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
    /// Gets the process ID of a given window.
    /// </summary>
    /// <param name="handle">The window handle.</param>
    /// <returns>Process ID.</returns>
    public static int GetWindowPID(IntPtr handle)
    {
      int pid = -1;
      GetWindowThreadProcessId(handle, out pid);

      return pid;
    }

    /// <summary>
    /// Activates the window by handle.
    /// </summary>
    /// <param name="hWnd">The handle to the window to activate.</param>
    public static void ActivateWindowByHandle(IntPtr hWnd)
    {
      WINDOWPLACEMENT windowPlacement = new WINDOWPLACEMENT();
      windowPlacement.length = Marshal.SizeOf(windowPlacement);
      GetWindowPlacement(hWnd, ref windowPlacement);

      switch (windowPlacement.showCmd)
      {
        case WindowShowStyle.Hide:
          ShowWindow(hWnd, WindowShowStyle.Restore);
          break;

        case WindowShowStyle.ShowMinimized:
          if (windowPlacement.flags == WPF_RESTORETOMAXIMIZED)
            ShowWindow(hWnd, WindowShowStyle.ShowMaximized);
          else
            ShowWindow(hWnd, WindowShowStyle.ShowNormal);
          break;

        default:
          SetForegroundWindow(hWnd, true);
          break;
      }
    }

    /// <summary>
    /// Gets a window long pointer.
    /// </summary>
    /// <param name="hWnd">The window handle.</param>
    /// <param name="nIndex">Index of the data to retreive.</param>
    /// <returns>IntPtr of retreived data.</returns>
    public static IntPtr GetWindowLongPtr(IntPtr hWnd, GWL nIndex)
    {
      if (IntPtr.Size == 8)
        return GetWindowLongPtr64(hWnd, nIndex);
      else
        return GetWindowLongPtr32(hWnd, nIndex);
    }

    /// <summary>
    /// Gets a class long pointer.
    /// </summary>
    /// <param name="hWnd">The window handle.</param>
    /// <param name="nIndex">Index of the data to retreive.</param>
    /// <returns>IntPtr of retreived data.</returns>
    public static IntPtr GetClassLongPtr(IntPtr hWnd, int nIndex)
    {
      if (IntPtr.Size == 8)
        return GetClassLongPtr64(hWnd, nIndex);
      else
        return new IntPtr(GetClassLongPtr32(hWnd, nIndex));
    }

    /// <summary>
    /// Show the desktop.
    /// </summary>
    public static void ShowDesktop()
    {
      IntPtr trayWnd = FindWindow("Shell_TrayWnd", null);

      if (trayWnd == IntPtr.Zero)
        return;

      IntPtr result;
      SendMessageTimeout(trayWnd, (int)WindowsMessage.WM_COMMAND, new IntPtr(MINIMIZE_ALL), IntPtr.Zero,
                         SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 1000, out result);
    }


    /// <summary>
    /// Given a 32-bit integer this method returns the High Word (upper 16 bits).
    /// </summary>
    /// <param name="dWord">32-bit integer.</param>
    /// <returns>Upper 16 bits or source 32-bit integer.</returns>
    public static Int16 HighWord(Int32 dWord)
    {
      return (Int16)((dWord >> 16) & 0xffff);
    }

    /// <summary>
    /// Given a 32-bit integer this method returns the Low Word (lower 16 bits).
    /// </summary>
    /// <param name="dWord">32-bit integer.</param>
    /// <returns>Lower 16 bits or source 32-bit integer.</returns>
    public static Int16 LowWord(Int32 dWord)
    {
      return (Int16)(dWord & 0xffff);
    }

    /// <summary>
    /// Given a 16-bit integer this method returns the High Byte (upper 8 bits).
    /// </summary>
    /// <param name="word">16-bit integer.</param>
    /// <returns>Upper 8 bits or source 16-bit integer.</returns>
    public static Byte HighByte(Int16 word)
    {
      return (Byte)((word >> 8) & 0xff);
    }

    /// <summary>
    /// Given a 16-bit integer this method returns the Low Byte (lower 8 bits).
    /// </summary>
    /// <param name="word">16-bit integer.</param>
    /// <returns>Lower 8 bits or source 16-bit integer.</returns>
    public static Byte LowByte(Int16 word)
    {
      return (Byte)(word & 0xff);
    }

    /// <summary>
    /// Check one value for the presence of a given bit-mask.
    /// </summary>
    /// <param name="check">Value to check.</param>
    /// <param name="mask">Bit-Mask to compare with.</param>
    /// <returns>true if the bit-mask is satisfied, otherwise false.</returns>
    public static bool CheckMask(byte check, byte mask)
    {
      return (check & mask) == mask ? true : false;
    }

    /// <summary>
    /// Check one value for the presence of a given bit-mask.
    /// </summary>
    /// <param name="check">Value to check.</param>
    /// <param name="mask">Bit-Mask to compare with.</param>
    /// <returns>true if the bit-mask is satisfied, otherwise false.</returns>
    public static bool CheckMask(int check, int mask)
    {
      return (check & mask) == mask ? true : false;
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

    public static bool Check64Bit()
    {
      //IsWow64Process is not supported under Windows2000 ( ver 5.0 )
      int osver = Environment.OSVersion.Version.Major * 10 + Environment.OSVersion.Version.Minor;
      if (osver <= 50) return false;

      Process p = Process.GetCurrentProcess();
      IntPtr handle = p.Handle;
      bool isWow64;
      bool success = IsWow64Process(handle, out isWow64);
      if (!success)
      {
        throw new Win32Exception();
      }
      return isWow64;
    }

    #endregion Methods

    #region SetThreadExecutionState

    [FlagsAttribute]
    public enum EXECUTION_STATE : uint
    {
      ES_AWAYMODE_REQUIRED = 0x00000040,
      ES_CONTINUOUS = 0x80000000,
      ES_DISPLAY_REQUIRED = 0x00000002,
      ES_SYSTEM_REQUIRED = 0x00000001
      // Legacy flag, should not be used.
      // ES_USER_PRESENT = 0x00000004
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

    #endregion SetThreadExecutionState
  }

  /// <summary>
  /// The exception that is thrown when an error occures when requesting a specific privilege.
  /// </summary>
  public class PrivilegeException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the PrivilegeException class.
    /// </summary>
    public PrivilegeException() : base() { }
    /// <summary>
    /// Initializes a new instance of the PrivilegeException class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public PrivilegeException(string message) : base(message) { }
  }
}