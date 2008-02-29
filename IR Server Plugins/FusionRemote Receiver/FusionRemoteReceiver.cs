using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
#if TRACE
using System.Diagnostics;
#endif
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Win32.SafeHandles;

namespace InputService.Plugin
{

  /// <summary>
  /// IR Server Plugin for the DViCO FusionREMOTE USB Receiver device.
  /// </summary>
  public class FusionRemoteReceiver : PluginBase, IRemoteReceiver
  {

    #region Constants

    static readonly string ConfigurationFile = Path.Combine(ConfigurationPath, "FusionRemote Receiver.xml");

    const string DeviceID = "VID_0FE9&PID_9010";

    const int DeviceBufferSize = 4;
    const uint ToggleBit = 0x80000000;

    const uint DIGCF_ALLCLASSES       = 0x04;
    const uint DIGCF_DEVICEINTERFACE  = 0x10;
    const uint DIGCF_PRESENT          = 0x02;
    const uint DIGCF_PROFILE          = 0x08;

    // File Access Types
    const uint GENERIC_READ     = 0x80000000;
    const uint GENERIC_WRITE    = 0x40000000;
    const uint GENERIC_EXECUTE  = 0x20000000;
    const uint GENERIC_ALL      = 0x10000000;

    #endregion Constants

    #region Interop

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    static extern SafeFileHandle CreateFile(
      [MarshalAs(UnmanagedType.LPTStr)] string fileName,
      uint fileAccess,
      [MarshalAs(UnmanagedType.U4)] EFileShares fileShare,
      //[In, Out, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(SecurityAttributesMarshaler))] SecurityAttributes lpSecurityAttributes,
      IntPtr sa,
      [MarshalAs(UnmanagedType.U4)] ECreationDisposition creationDisposition,
      [MarshalAs(UnmanagedType.U4)] EFileAttributes flags,
      IntPtr templateFile);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool CancelIo(SafeFileHandle handle);

    [Flags]
    enum EFileShares
    {
       None   = 0x00000000,
       Read   = 0x00000001,
       Write  = 0x00000002,
       Delete = 0x00000004,
    }

    enum ECreationDisposition
    {
       New              = 1,
       CreateAlways     = 2,
       OpenExisting     = 3,
       OpenAlways       = 4,
       TruncateExisting = 5,
    }

    [Flags]
    enum EFileAttributes : uint
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
      FirstPipeInstance = 0x00080000
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct DeviceInfoData
    {
      public int Size;
      public Guid Class;
      public int DevInst;
      public IntPtr Reserved;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct DeviceInterfaceData
    {
      public int Size;
      public Guid Class;
      public int Flags;
      public IntPtr Reserved;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    struct DeviceInterfaceDetailData
    {
      public int Size;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
      public string DevicePath;
    }

    [DllImport("hid.dll")]
    static extern void HidD_GetHidGuid(
      ref Guid guid);

    [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
    static extern IntPtr SetupDiGetClassDevs(
      ref Guid ClassGuid,
      [MarshalAs(UnmanagedType.LPTStr)] string Enumerator,
      IntPtr hwndParent,
      UInt32 Flags);

    [DllImport("setupapi.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetupDiEnumDeviceInfo(
      IntPtr handle,
      int Index,
      ref DeviceInfoData deviceInfoData);

    [DllImport("setupapi.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetupDiEnumDeviceInterfaces(
      IntPtr handle,
      ref DeviceInfoData deviceInfoData,
      ref Guid guidClass,
      int MemberIndex,
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
    static extern bool SetupDiDestroyDeviceInfoList(IntPtr handle);

    #endregion Interop

    #region Variables

    RemoteHandler _remoteHandler;

    FileStream _deviceStream;
    byte[] _deviceBuffer;
    DateTime _lastCodeTime = DateTime.Now;

    #endregion Variables

    #region Implementation

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name         { get { return "FusionREMOTE"; } }
    /// <summary>
    /// IR Server plugin version.
    /// </summary>
    /// <value>The version.</value>
    public override string Version      { get { return "1.0.4.2"; } }
    /// <summary>
    /// The IR Server plugin's author.
    /// </summary>
    /// <value>The author.</value>
    public override string Author       { get { return "and-81"; } }
    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    /// <value>The description.</value>
    public override string Description  { get { return "DViCO FusionREMOTE Receiver"; } }

    /// <summary>
    /// Detect the presence of this device.  Devices that cannot be detected will always return false.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the device is present, otherwise <c>false</c>.
    /// </returns>
    public override bool Detect()
    {
      try
      {
        Guid hidGuid = new Guid();
        HidD_GetHidGuid(ref hidGuid);

        string devicePath = FindDevice(hidGuid, DeviceID);

        return (devicePath != null);
      }
      catch
      {
        return false;
      }
    }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    public override void Start()
    {
      OpenDevice();

      _deviceStream.BeginRead(_deviceBuffer, 0, _deviceBuffer.Length, new AsyncCallback(OnReadComplete), null);
    }
    /// <summary>
    /// Suspend the IR Server plugin when computer enters standby.
    /// </summary>
    public override void Suspend()
    {
      Stop();
    }
    /// <summary>
    /// Resume the IR Server plugin when the computer returns from standby.
    /// </summary>
    public override void Resume()
    {
      Start();
    }
    /// <summary>
    /// Stop the IR Server plugin.
    /// </summary>
    public override void Stop()
    {
      if (_deviceStream == null)
        return;

      try
      {
        _deviceStream.Dispose();
      }
      catch (IOException)
      {
        // we are closing the stream so ignore this
      }
      finally
      {
        _deviceStream = null;
      }
    }

    /// <summary>
    /// Callback for remote button presses.
    /// </summary>
    /// <value>The remote callback.</value>
    public RemoteHandler RemoteCallback
    {
      get { return _remoteHandler; }
      set { _remoteHandler = value; }
    }

    /// <summary>
    /// Opens the device.
    /// </summary>
    void OpenDevice()
    {
      Guid hidGuid = new Guid();
      HidD_GetHidGuid(ref hidGuid);

      string devicePath = FindDevice(hidGuid, DeviceID);

      if (devicePath == null)
        throw new ApplicationException("No device detected");

      SafeFileHandle deviceHandle = CreateFile(devicePath, GENERIC_READ, EFileShares.Read | EFileShares.Write, IntPtr.Zero, ECreationDisposition.OpenExisting, EFileAttributes.Overlapped, IntPtr.Zero);
      int lastError = Marshal.GetLastWin32Error();

      if (lastError != 0)
        throw new Win32Exception(lastError);

      _deviceBuffer = new byte[DeviceBufferSize];
      _deviceStream = new FileStream(deviceHandle, FileAccess.Read, _deviceBuffer.Length, true);
    }

    /// <summary>
    /// Finds the device.
    /// </summary>
    /// <param name="classGuid">The class GUID.</param>
    /// <param name="deviceID">The device ID.</param>
    /// <returns>Device path.</returns>
    static string FindDevice(Guid classGuid, string deviceID)
    {
      string devicePath = null;

      IntPtr handle = SetupDiGetClassDevs(ref classGuid, null, IntPtr.Zero, DIGCF_DEVICEINTERFACE | DIGCF_PRESENT);

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
        if (IntPtr.Size == 8)
          deviceInterfaceDetailData.Size = 8;
        else
          deviceInterfaceDetailData.Size = 5;

        if (!SetupDiGetDeviceInterfaceDetail(handle, ref deviceInterfaceData, ref deviceInterfaceDetailData, cbData, IntPtr.Zero, IntPtr.Zero))
        {
          SetupDiDestroyDeviceInfoList(handle);
          throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        if (deviceInterfaceDetailData.DevicePath.IndexOf(deviceID, StringComparison.OrdinalIgnoreCase) != -1)
        {
          SetupDiDestroyDeviceInfoList(handle);
          devicePath = deviceInterfaceDetailData.DevicePath;
          break;
        }
      }

      return devicePath;
    }

    /// <summary>
    /// Called when a device read completes.
    /// </summary>
    /// <param name="asyncResult">The async result.</param>
    void OnReadComplete(IAsyncResult asyncResult)
    {
      try
      {
        if (_deviceStream == null)
          return;

        int bytesRead = _deviceStream.EndRead(asyncResult);
        if (bytesRead == 0)
        {
          _deviceStream.Dispose();
          _deviceStream = null;
          return;
        }

        uint keyCode = (uint)BitConverter.ToInt32(_deviceBuffer, 0);

        if ((keyCode & ToggleBit) == 0x00)
        {
          _lastCodeTime = DateTime.Now;

          if (_remoteHandler != null)
            _remoteHandler(this.Name, String.Format("{0:X8}", keyCode));
        }
        else
        {
          keyCode &= ~ToggleBit;

          TimeSpan timeSpan = DateTime.Now - _lastCodeTime;
          if (timeSpan.Milliseconds >= 400)
          {
            _lastCodeTime = DateTime.Now;

            if (_remoteHandler != null)
              _remoteHandler(this.Name, String.Format("{0:X8}", keyCode));
          }
        }

        // begin another asynchronous read from the device
        if (_deviceStream != null)
          _deviceStream.BeginRead(_deviceBuffer, 0, _deviceBuffer.Length, new AsyncCallback(OnReadComplete), null);
      }
#if TRACE
      catch (Exception ex)
      {
        Trace.WriteLine(ex.ToString());
      }
#else
      catch
      {
      }
#endif
    }

    #endregion Implementation

  }

}
