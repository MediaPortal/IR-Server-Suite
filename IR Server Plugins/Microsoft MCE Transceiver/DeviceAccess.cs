using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

using Microsoft.Win32.SafeHandles;

namespace MicrosoftMceTransceiver
{

  /// <summary>
  /// Provides access to the MCE driver.
  /// </summary>
  public static class DeviceAccess
  {

    #region Enumerations

    [Flags]
    enum Digcfs
    {
      None            = 0x00,
      Default         = 0x01,
      Present         = 0x02,
      AllClasses      = 0x04,
      Profile         = 0x08,
      DeviceInterface = 0x10,
    }

    [Flags]
    public enum FileShares
    {
       None   = 0x00,
       Read   = 0x01,
       Write  = 0x02,
       Delete = 0x04,
    }

    public enum CreationDisposition
    {
      None              = 0,
      New               = 1,
      CreateAlways      = 2,
      OpenExisting      = 3,
      OpenAlways        = 4,
      TruncateExisting  = 5,
    }

    [Flags]
    public enum FileAttributes : uint
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

    [Flags]
    public enum FileAccessTypes : uint
    {
      GenericRead     = 0x80000000,
      GenericWrite    = 0x40000000,
      GenericExecute  = 0x20000000,
      GenericAll      = 0x10000000,
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

    [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Auto)]
    static extern SafeFileHandle CreateFile(
      [MarshalAs(UnmanagedType.LPTStr)] string fileName,
      [MarshalAs(UnmanagedType.U4)] FileAccessTypes fileAccess,
      [MarshalAs(UnmanagedType.U4)] FileShares fileShare,
      IntPtr securityAttributes,
      [MarshalAs(UnmanagedType.U4)] CreationDisposition creationDisposition,
      [MarshalAs(UnmanagedType.U4)] FileAttributes flags,
      IntPtr templateFile);

    [DllImport("kernel32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool CancelIo(
      SafeFileHandle handle);

    [DllImport("setupapi", CharSet = CharSet.Auto)]
    static extern IntPtr SetupDiGetClassDevs(
      ref Guid classGuid,
      [MarshalAs(UnmanagedType.LPTStr)] string enumerator,
      IntPtr hwndParent,
      [MarshalAs(UnmanagedType.U4)] Digcfs flags);

    [DllImport("setupapi", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetupDiEnumDeviceInfo(
      IntPtr handle,
      int index,
      ref DeviceInfoData deviceInfoData);

    [DllImport("setupapi", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetupDiEnumDeviceInterfaces(
      IntPtr handle,
      ref DeviceInfoData deviceInfoData,
      ref Guid guidClass,
      int memberIndex,
      ref DeviceInterfaceData deviceInterfaceData);

    [DllImport("setupapi", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetupDiGetDeviceInterfaceDetail(
      IntPtr handle,
      ref DeviceInterfaceData deviceInterfaceData,
      IntPtr unused1,
      int unused2,
      ref uint requiredSize,
      IntPtr unused3);

    [DllImport("setupapi", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetupDiGetDeviceInterfaceDetail(
      IntPtr handle,
      ref DeviceInterfaceData deviceInterfaceData,
      ref DeviceInterfaceDetailData deviceInterfaceDetailData,
      uint detailSize,
      IntPtr unused1,
      IntPtr unused2);

    [DllImport("setupapi")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetupDiDestroyDeviceInfoList(
      IntPtr handle);

    #endregion Interop

    #region Methods

    /// <summary>
    /// Find the device path for the supplied Device Class Guid.
    /// </summary>
    /// <param name="classGuid">GUID to locate device with.</param>
    /// <returns>Device path.</returns>
    public static string FindDevice(Guid classGuid)
    {
      string devicePath = null;

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
          if (lastError != 0x0103 && lastError != 0x007E)
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

        if ((deviceInterfaceDetailData.DevicePath.IndexOf("#vid_03ee&pid_2501") != -1) ||   // Mitsumi MCE remote
            (deviceInterfaceDetailData.DevicePath.IndexOf("#vid_043e&pid_9803") != -1) ||   // LG
            (deviceInterfaceDetailData.DevicePath.IndexOf("#vid_045e&pid_00a0") != -1) ||   // Microsoft
            (deviceInterfaceDetailData.DevicePath.IndexOf("#vid_0471&pid_0815") != -1) ||   // Microsoft/Philips 2005
            (deviceInterfaceDetailData.DevicePath.IndexOf("#vid_0471&pid_060c") != -1) ||   // Philips (HP branded)
            (deviceInterfaceDetailData.DevicePath.IndexOf("#vid_045e&pid_006d") != -1) ||   // Microsoft/Philips 2004
            (deviceInterfaceDetailData.DevicePath.IndexOf("#vid_0609&pid_031d") != -1) ||   // SMK/Toshiba G83C0004D410 (Hauppauge)
            (deviceInterfaceDetailData.DevicePath.IndexOf("#vid_0609&pid_0322") != -1) ||   // SMK (Sony VAIO)
            (deviceInterfaceDetailData.DevicePath.IndexOf("#vid_107b&pid_3009") != -1) ||   // FIC Spectra/Mycom Mediacenter
            (deviceInterfaceDetailData.DevicePath.IndexOf("#vid_1308&pid_c001") != -1) ||   // Shuttle
            (deviceInterfaceDetailData.DevicePath.IndexOf("#vid_1460&pid_9150") != -1) ||   // HP
            (deviceInterfaceDetailData.DevicePath.IndexOf("#vid_1509&pid_9242") != -1) ||   // Fujitsu Scaleo-E
            (deviceInterfaceDetailData.DevicePath.IndexOf("#vid_1784&pid_0001") != -1) ||   // Topseed
            (deviceInterfaceDetailData.DevicePath.IndexOf("#vid_179d&pid_0010") != -1) ||   // Ricavision internal
            (deviceInterfaceDetailData.DevicePath.IndexOf("#vid_195d&pid_7002") != -1) ||   // Itron ione Libra Q-11
            (deviceInterfaceDetailData.DevicePath.StartsWith(@"\\?\hid#irdevice&col01#2"))) // Microsoft/Philips 2005 (Vista)
        {
          SetupDiDestroyDeviceInfoList(handle);
          devicePath = deviceInterfaceDetailData.DevicePath;
          break;
        }
      }

      return devicePath;
    }

    /// <summary>
    /// Open a handle to the device driver.
    /// </summary>
    /// <param name="devicePath">Device path.</param>
    /// <param name="access">Access type.</param>
    /// <param name="share">Share type.</param>
    /// <returns>Handle to device driver.</returns>
    public static SafeFileHandle OpenHandle(string devicePath, FileAccessTypes access, FileShares share)
    {
      return CreateFile(devicePath, access, share,
        IntPtr.Zero, CreationDisposition.OpenExisting, FileAttributes.Overlapped, IntPtr.Zero);
    }

    /// <summary>
    /// Cancel IO for device.
    /// </summary>
    /// <param name="deviceHandle">Handle to device.</param>
    /// <returns>Success.</returns>
    public static bool CancelDeviceIo(SafeFileHandle deviceHandle)
    {
      return CancelIo(deviceHandle);
    }

    #endregion Methods

  }

}
