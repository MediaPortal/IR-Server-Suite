using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

using Microsoft.Win32.SafeHandles;

using IRServerPluginInterface;

namespace MicrosoftMceTransceiver
{

  /// <summary>
  /// Base class for the different MCE device driver access classes.
  /// </summary>
  public abstract class Driver
  {

    #region Enumerations

    [Flags]
    enum Digcfs : uint
    {
      None            = 0x00,
      Default         = 0x01,
      Present         = 0x02,
      AllClasses      = 0x04,
      Profile         = 0x08,
      DeviceInterface = 0x10,
    }

    [Flags]
    protected enum CreateFileAccessTypes : uint
    {
      GenericRead     = 0x80000000,
      GenericWrite    = 0x40000000,
      GenericExecute  = 0x20000000,
      GenericAll      = 0x10000000,
    }

    [Flags]
    protected enum CreateFileShares : uint
    {
       None   = 0x00,
       Read   = 0x01,
       Write  = 0x02,
       Delete = 0x04,
    }

    protected enum CreateFileDisposition : uint
    {
      None              = 0,
      New               = 1,
      CreateAlways      = 2,
      OpenExisting      = 3,
      OpenAlways        = 4,
      TruncateExisting  = 5,
    }

    [Flags]
    protected enum CreateFileAttributes : uint
    {
      Readonly          = 0x00000001,
      Hidden            = 0x00000002,
      System            = 0x00000004,
      Directory         = 0x00000010,
      Archive           = 0x00000020,
      Device            = 0x00000040,
      Normal            = 0x00000080,
      Temporary         = 0x00000100,
      SparseFile        = 0x00000200,
      ReparsePoint      = 0x00000400,
      Compressed        = 0x00000800,
      Offline           = 0x00001000,
      NotContentIndexed = 0x00002000,
      Encrypted         = 0x00004000,
      Write_Through     = 0x80000000,
      Overlapped        = 0x40000000,
      NoBuffering       = 0x20000000,
      RandomAccess      = 0x10000000,
      SequentialScan    = 0x08000000,
      DeleteOnClose     = 0x04000000,
      BackupSemantics   = 0x02000000,
      PosixSemantics    = 0x01000000,
      OpenReparsePoint  = 0x00200000,
      OpenNoRecall      = 0x00100000,
      FirstPipeInstance = 0x00080000,
    }

    #endregion Enumerations

    #region Structures

    [StructLayout(LayoutKind.Sequential)]
    struct DeviceInfoData
    {
      public int Size;
      public Guid Class;
      public uint DevInst;
      public IntPtr Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct DeviceInterfaceData
    {
      public int Size;
      public Guid Class;
      public uint Flags;
      public uint Reserved;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct DeviceInterfaceDetailData
    {
      public int Size;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
      public string DevicePath;
    }

    #endregion Structures

    #region Interop

    [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
    static extern IntPtr SetupDiGetClassDevs(
      ref Guid classGuid,
      [MarshalAs(UnmanagedType.LPTStr)] string enumerator,
      IntPtr hwndParent,
      [MarshalAs(UnmanagedType.U4)] Digcfs flags);

    [DllImport("setupapi.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetupDiEnumDeviceInfo(
      IntPtr handle,
      int index,
      ref DeviceInfoData deviceInfoData);

    [DllImport("setupapi.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetupDiEnumDeviceInterfaces(
      IntPtr handle,
      ref DeviceInfoData deviceInfoData,
      ref Guid guidClass,
      int memberIndex,
      ref DeviceInterfaceData deviceInterfaceData);

    [DllImport("setupapi.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetupDiGetDeviceInterfaceDetail(
      IntPtr handle,
      ref DeviceInterfaceData deviceInterfaceData,
      IntPtr unused1,
      int unused2,
      ref uint requiredSize,
      IntPtr unused3);

    [DllImport("setupapi.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetupDiGetDeviceInterfaceDetail(
      IntPtr handle,
      ref DeviceInterfaceData deviceInterfaceData,
      ref DeviceInterfaceDetailData deviceInterfaceDetailData,
      uint detailSize,
      IntPtr unused1,
      IntPtr unused2);

    [DllImport("setupapi.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetupDiDestroyDeviceInfoList(
      IntPtr handle);

    #endregion Interop

    #region Variables

    protected Guid _deviceGuid;
    protected string _devicePath;

    protected RemoteCallback _remoteCallback      = null;
    protected KeyboardCallback _keyboardCallback  = null;
    protected MouseCallback _mouseCallback        = null;

    #endregion Variables

    #region Constructors

    protected Driver() { }
    protected Driver(Guid deviceGuid, string devicePath, RemoteCallback remoteCallback, KeyboardCallback keyboardCallback, MouseCallback mouseCallback)
    {
      if (String.IsNullOrEmpty(devicePath))
        throw new ArgumentException("Null or Empty device path supplied", "devicePath");

      _deviceGuid = deviceGuid;
      _devicePath = devicePath;

      _remoteCallback   = remoteCallback;
      _keyboardCallback = keyboardCallback;
      _mouseCallback    = mouseCallback;
    }

    #endregion Constructors

    #region Abstract Methods

    /// <summary>
    /// Start using the device.
    /// </summary>
    public abstract void Start();

    /// <summary>
    /// Stop access to the device.
    /// </summary>
    public abstract void Stop();

    /// <summary>
    /// Learn an IR Command.
    /// </summary>
    /// <param name="learnTimeout">How long to wait before aborting learn.</param>
    /// <param name="learned">Newly learned IR Command.</param>
    /// <returns>Learn status.</returns>
    public abstract LearnStatus Learn(int learnTimeout, out IrCode learned);

    /// <summary>
    /// Send an IR Command.
    /// </summary>
    /// <param name="code">IR code data to send.</param>
    /// <param name="port">IR port to send to.</param>
    public abstract void Send(IrCode code, uint port);

    #endregion Abstract Methods

    #region Static Methods

    /// <summary>
    /// Find the device path for the supplied Device Class Guid.
    /// </summary>
    /// <param name="classGuid">GUID to locate device with.</param>
    /// <returns>Device path.</returns>
    public static string Find(Guid classGuid)
    {
      IntPtr handle = SetupDiGetClassDevs(ref classGuid, null, IntPtr.Zero, Digcfs.DeviceInterface | Digcfs.Present);

      if (handle.ToInt32() == -1)
        return null;

      for (int deviceIndex = 0; ; deviceIndex++)
      {
        DeviceInfoData deviceInfoData = new DeviceInfoData();
        deviceInfoData.Size = Marshal.SizeOf(deviceInfoData);

        if (!SetupDiEnumDeviceInfo(handle, deviceIndex, ref deviceInfoData))
        {
          int lastError = Marshal.GetLastWin32Error();

          // out of devices or do we have an error?
          if (lastError != Win32ErrorCodes.ERROR_NO_MORE_ITEMS && lastError != Win32ErrorCodes.ERROR_MOD_NOT_FOUND)
          {
            SetupDiDestroyDeviceInfoList(handle);
            throw new Win32Exception(lastError);
          }

          SetupDiDestroyDeviceInfoList(handle);
          break;
        }

        DeviceInterfaceData deviceInterfaceData = new DeviceInterfaceData();
        deviceInterfaceData.Size = Marshal.SizeOf(deviceInterfaceData);

        if (!SetupDiEnumDeviceInterfaces(handle, ref deviceInfoData, ref classGuid, 0, ref deviceInterfaceData))
        {
          SetupDiDestroyDeviceInfoList(handle);
          throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        uint cbData = 0;

        if (!SetupDiGetDeviceInterfaceDetail(handle, ref deviceInterfaceData, IntPtr.Zero, 0, ref cbData, IntPtr.Zero) && cbData == 0)
        {
          SetupDiDestroyDeviceInfoList(handle);
          throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        DeviceInterfaceDetailData deviceInterfaceDetailData = new DeviceInterfaceDetailData();
        deviceInterfaceDetailData.Size = 5;

        if (!SetupDiGetDeviceInterfaceDetail(handle, ref deviceInterfaceData, ref deviceInterfaceDetailData, cbData, IntPtr.Zero, IntPtr.Zero))
        {
          SetupDiDestroyDeviceInfoList(handle);
          throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        if (!String.IsNullOrEmpty(deviceInterfaceDetailData.DevicePath))
        {
          SetupDiDestroyDeviceInfoList(handle);
          return deviceInterfaceDetailData.DevicePath;
        }
      }

      return null;
    }

    #endregion Static Methods

  }

}
