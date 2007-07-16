using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

using Microsoft.Win32.SafeHandles;

namespace CustomHIDReceiver
{

  internal static class NativeMethods
  {

    #region Constants

    public const int WM_KEYDOWN     = 0x0100;
    public const int WM_APPCOMMAND  = 0x0319;
    public const int WM_INPUT       = 0x00FF;
    public const int WM_SYSKEYDOWN  = 0x0104;

    public const int RIDI_PREPARSEDDATA = 0x20000005;
    public const int RIDI_DEVICENAME    = 0x20000007;
    public const int RIDI_DEVICEINFO    = 0x2000000B;
    
    public const int KEYBOARD_OVERRUN_MAKE_CODE = 0x00FF;

    #endregion Constants

    #region Enumerations

    internal enum RawInputType
    {
      Mouse     = 0,
      Keyboard  = 1,
      HID       = 2
    }

    [Flags]
    internal enum RawMouseFlags : ushort
    {
      MoveRelative      = 0,
      MoveAbsolute      = 1,
      VirtualDesktop    = 2,
      AttributesChanged = 4
    }

    [Flags]
    internal enum RawMouseButtons : ushort
    {
      None        = 0,
      LeftDown    = 0x0001,
      LeftUp      = 0x0002,
      RightDown   = 0x0004,
      RightUp     = 0x0008,
      MiddleDown  = 0x0010,
      MiddleUp    = 0x0020,
      Button4Down = 0x0040,
      Button4Up   = 0x0080,
      Button5Down = 0x0100,
      Button5Up   = 0x0200,
      MouseWheel  = 0x0400
    }

    [Flags]
    internal enum RawKeyboardFlags : ushort
    {
      KeyMake               = 0x00,
      KeyBreak              = 0x01,
      KeyE0                 = 0x02,
      KeyE1                 = 0x04,
      TerminalServerSetLED  = 0x08,
      TerminalServerShadow  = 0x10
    }

    internal enum RawInputCommand
    {
      Input   = 0x10000003,
      Header  = 0x10000005
    }

    [Flags]
    internal enum RawInputDeviceFlags
    {
      /// <summary>No flags.</summary>
      None = 0,
      /// <summary>If set, this removes the top level collection from the inclusion list. This tells the operating system to stop reading from a device which matches the top level collection.</summary>
      Remove = 0x00000001,
      /// <summary>If set, this specifies the top level collections to exclude when reading a complete usage page. This flag only affects a TLC whose usage page is already specified with PageOnly.</summary>
      Exclude = 0x00000010,
      /// <summary>If set, this specifies all devices whose top level collection is from the specified usUsagePage. Note that Usage must be zero. To exclude a particular top level collection, use Exclude.</summary>
      PageOnly = 0x00000020,
      /// <summary>If set, this prevents any devices specified by UsagePage or Usage from generating legacy messages. This is only for the mouse and keyboard.</summary>
      NoLegacy = 0x00000030,
      /// <summary>If set, this enables the caller to receive the input even when the caller is not in the foreground. Note that WindowHandle must be specified.</summary>
      InputSink = 0x00000100,
      /// <summary>If set, the mouse button click does not activate the other window.</summary>
      CaptureMouse = 0x00000200,
      /// <summary>If set, the application-defined keyboard device hotkeys are not handled. However, the system hotkeys; for example, ALT+TAB and CTRL+ALT+DEL, are still handled. By default, all keyboard hotkeys are handled. NoHotKeys can be specified even if NoLegacy is not specified and WindowHandle is NULL.</summary>
      NoHotKeys = 0x00000200,
      /// <summary>If set, application keys are handled.  NoLegacy must be specified.  Keyboard only.</summary>
      AppKeys = 0x00000400
    }

    [Flags]
    internal enum EFileAttributes : uint
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
      FirstPipeInstance = 0x00080000
    }

    #endregion Enumerations

    #region Structures

    [StructLayout(LayoutKind.Sequential)]
    internal struct DeviceInfoData
    {
      public int Size;
      public Guid Class;
      public uint DevInst;
      public IntPtr Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct DeviceInterfaceData
    {
      public int Size;
      public Guid Class;
      public uint Flags;
      public uint Reserved;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct DeviceInterfaceDetailData
    {
      public int Size;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
      public string DevicePath;
    }


    [StructLayout(LayoutKind.Explicit)]
    internal struct DeviceInfo
    {
      [FieldOffset(0)]
      public int Size;
      [FieldOffset(4)]
      public RawInputType Type;

      [FieldOffset(8)]
      public DeviceInfoMouse MouseInfo;
      [FieldOffset(8)]
      public DeviceInfoKeyboard KeyboardInfo;
      [FieldOffset(8)]
      public DeviceInfoHID HIDInfo;
    }

    internal struct DeviceInfoMouse
    {
      public uint ID;
      public uint NumberOfButtons;
      public uint SampleRate;
    }

    internal struct DeviceInfoKeyboard
    {
      public uint Type;
      public uint SubType;
      public uint KeyboardMode;
      public uint NumberOfFunctionKeys;
      public uint NumberOfIndicators;
      public uint NumberOfKeysTotal;
    }

    internal struct DeviceInfoHID
    {
      public uint VendorID;
      public uint ProductID;
      public uint VersionNumber;
      public ushort UsagePage;
      public ushort Usage;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct RAWINPUTDEVICELIST
    {
      public IntPtr hDevice;
      [MarshalAs(UnmanagedType.U4)]
      public RawInputType dwType;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct RAWINPUT
    {
      [FieldOffset(0)]
      public RAWINPUTHEADER header;
      [FieldOffset(16)]
      public RAWMOUSE mouse;
      [FieldOffset(16)]
      public RAWKEYBOARD keyboard;
      [FieldOffset(16)]
      public RAWHID hid;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RAWINPUTHEADER
    {
      [MarshalAs(UnmanagedType.U4)]
      public RawInputType dwType;
      [MarshalAs(UnmanagedType.U4)]
      public int dwSize;
      public IntPtr hDevice;
      [MarshalAs(UnmanagedType.U4)]
      public int wParam;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RAWHID
    {
      [MarshalAs(UnmanagedType.U4)]
      public int dwSizeHid;
      [MarshalAs(UnmanagedType.U4)]
      public int dwCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct BUTTONSSTR
    {
      [MarshalAs(UnmanagedType.U2)]
      public ushort usButtonFlags;
      [MarshalAs(UnmanagedType.U2)]
      public ushort usButtonData;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct RAWMOUSE
    {
      [MarshalAs(UnmanagedType.U2)]
      [FieldOffset(0)]
      public ushort usFlags;
      [MarshalAs(UnmanagedType.U4)]
      [FieldOffset(4)]
      public uint ulButtons;
      [FieldOffset(4)]
      public BUTTONSSTR buttonsStr;
      [MarshalAs(UnmanagedType.U4)]
      [FieldOffset(8)]
      public uint ulRawButtons;
      [FieldOffset(12)]
      public int lLastX;
      [FieldOffset(16)]
      public int lLastY;
      [MarshalAs(UnmanagedType.U4)]
      [FieldOffset(20)]
      public uint ulExtraInformation;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RAWKEYBOARD
    {
      [MarshalAs(UnmanagedType.U2)]
      public ushort MakeCode;
      [MarshalAs(UnmanagedType.U2)]
      public RawKeyboardFlags Flags;
      [MarshalAs(UnmanagedType.U2)]
      public ushort Reserved;
      [MarshalAs(UnmanagedType.U2)]
      public ushort VKey;
      [MarshalAs(UnmanagedType.U4)]
      public uint Message;
      [MarshalAs(UnmanagedType.U4)]
      public uint ExtraInformation;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RAWINPUTDEVICE
    {
      [MarshalAs(UnmanagedType.U2)]
      public ushort usUsagePage;
      [MarshalAs(UnmanagedType.U2)]
      public ushort usUsage;
      [MarshalAs(UnmanagedType.U4)]
      public RawInputDeviceFlags dwFlags;
      public IntPtr hwndTarget;
    }
    
    #endregion Structures

    #region Interop

    [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Auto)]
    internal static extern SafeFileHandle CreateFile(
        String fileName,
        [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
        [MarshalAs(UnmanagedType.U4)] FileShare fileShare,
        IntPtr securityAttributes,
        [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
        [MarshalAs(UnmanagedType.U4)] EFileAttributes flags,
        IntPtr template);

    [DllImport("hid")]
    internal static extern void HidD_GetHidGuid(ref Guid guid);

    [DllImport("setupapi", CharSet = CharSet.Auto)]
    internal static extern IntPtr SetupDiGetClassDevs(
      ref Guid ClassGuid,
      [MarshalAs(UnmanagedType.LPTStr)] string Enumerator,
      IntPtr hwndParent,
      UInt32 Flags);

    [DllImport("setupapi", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool SetupDiEnumDeviceInfo(
      IntPtr handle,
      int Index,
      ref DeviceInfoData deviceInfoData);

    [DllImport("setupapi", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool SetupDiEnumDeviceInterfaces(
      IntPtr handle,
      ref DeviceInfoData deviceInfoData,
      ref Guid guidClass,
      int MemberIndex,
      ref DeviceInterfaceData deviceInterfaceData);

    [DllImport("setupapi", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool SetupDiGetDeviceInterfaceDetail(
      IntPtr handle,
      ref DeviceInterfaceData deviceInterfaceData,
      IntPtr unused1,
      int unused2,
      ref uint requiredSize,
      IntPtr unused3);

    [DllImport("setupapi", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool SetupDiGetDeviceInterfaceDetail(
      IntPtr handle,
      ref DeviceInterfaceData deviceInterfaceData,
      ref DeviceInterfaceDetailData deviceInterfaceDetailData,
      uint detailSize,
      IntPtr unused1,
      IntPtr unused2);

    [DllImport("setupapi")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool SetupDiDestroyDeviceInfoList(
      IntPtr handle);

    [DllImport("User32.dll")]
    public static extern uint GetRawInputData(
      IntPtr hRawInput,
      RawInputCommand uiCommand,
      IntPtr pData,
      ref uint pcbSize,
      uint cbSizeHeader);

    [DllImport("User32.dll")]
    public static extern bool RegisterRawInputDevices(
      RAWINPUTDEVICE[] pRawInputDevice,
      uint uiNumDevices,
      uint cbSize);

    [DllImport("User32.dll")]
    public static extern uint GetRawInputDeviceList(
      IntPtr pRawInputDeviceList,
      ref uint uiNumDevices,
      uint cbSize);

    [DllImport("User32.dll")]
    public static extern uint GetRawInputDeviceInfo(
      IntPtr hDevice,
      uint uiCommand,
      IntPtr pData,
      ref uint pcbSize);

    [DllImport("User32.dll")]
    public static extern uint GetRawInputDeviceInfo(
      IntPtr deviceHandle,
      uint uiCommand,
      ref DeviceInfo data,
      ref uint dataSize);


    #endregion Interop

  }

}
