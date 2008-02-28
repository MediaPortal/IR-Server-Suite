using System;
using System.ComponentModel;
#if TRACE
using System.Diagnostics;
#endif
using System.IO;
using System.Runtime.InteropServices;

using Microsoft.Win32.SafeHandles;

namespace InputService.Plugin
{

  /// <summary>
  /// IR Server Plugin for the IR501 IR receiver.
  /// </summary>
  [CLSCompliant(false)]
  public class IR501Receiver : PluginBase, IRemoteReceiver
  {

    #region Interop

    [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Auto)]
    static extern SafeFileHandle CreateFile(
        String fileName,
        [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
        [MarshalAs(UnmanagedType.U4)] FileShare fileShare,
        IntPtr securityAttributes,
        [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
        [MarshalAs(UnmanagedType.U4)] EFileAttributes flags,
        IntPtr template);

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
      FirstPipeInstance = 0x00080000,
    }

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

    [DllImport("hid")]
    static extern void HidD_GetHidGuid(
      ref Guid guid);

    [DllImport("setupapi", CharSet = CharSet.Auto)]
    static extern IntPtr SetupDiGetClassDevs(
      ref Guid ClassGuid,
      [MarshalAs(UnmanagedType.LPTStr)] string Enumerator,
      IntPtr hwndParent,
      UInt32 Flags);

    [DllImport("setupapi", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetupDiEnumDeviceInfo(
      IntPtr handle,
      int Index,
      ref DeviceInfoData deviceInfoData);

    [DllImport("setupapi", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetupDiEnumDeviceInterfaces(
      IntPtr handle,
      ref DeviceInfoData deviceInfoData,
      ref Guid guidClass,
      int MemberIndex,
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
    static extern bool SetupDiDestroyDeviceInfoList(IntPtr handle);

    #endregion Interop

    #region Constants

    const int DeviceBufferSize = 255;

    const string DeviceID = "vid_147a&pid_e001";

    #endregion Constants

    #region Variables

    RemoteHandler _remoteButtonHandler;

    FileStream _deviceStream;
    byte[] _deviceBuffer;

    int _lastCode = -1;
    DateTime _lastCodeTime = DateTime.Now;

    #endregion Variables

    #region Implementation

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name         { get { return "IR501"; } }
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
    public override string Description  { get { return "Support for the IR501 IR Receiver"; } }

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
        Guid guid = new Guid();
        HidD_GetHidGuid(ref guid);

        string devicePath = FindDevice(guid);

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
      Guid guid = new Guid();
      HidD_GetHidGuid(ref guid);

      string devicePath = FindDevice(guid);
      if (String.IsNullOrEmpty(devicePath))
        throw new ApplicationException("Device not found");

      SafeFileHandle deviceHandle = CreateFile(devicePath, FileAccess.Read, FileShare.Read, IntPtr.Zero, FileMode.Open, EFileAttributes.Overlapped, IntPtr.Zero);
      int lastError = Marshal.GetLastWin32Error();

      if (deviceHandle.IsInvalid)
        throw new Win32Exception(lastError, "Failed to open device");

      //_deviceWatcher.RegisterDeviceRemoval(deviceHandle);

      _deviceBuffer = new byte[DeviceBufferSize];
      _deviceStream = new FileStream(deviceHandle, FileAccess.Read, _deviceBuffer.Length, true);
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
      get { return _remoteButtonHandler; }
      set { _remoteButtonHandler = value; }
    }

    static string FindDevice(Guid classGuid)
    {
      int lastError;

      // 0x12 = DIGCF_PRESENT | DIGCF_DEVICEINTERFACE
      IntPtr handle = SetupDiGetClassDevs(ref classGuid, "", IntPtr.Zero, 0x12);
      lastError = Marshal.GetLastWin32Error();

      if (handle.ToInt32() == -1)
        throw new Win32Exception(lastError);

      string devicePath = null;

      for (int deviceIndex = 0; ; deviceIndex++)
      {
        DeviceInfoData deviceInfoData = new DeviceInfoData();
        deviceInfoData.Size = Marshal.SizeOf(deviceInfoData);

        if (SetupDiEnumDeviceInfo(handle, deviceIndex, ref deviceInfoData) == false)
        {
          // out of devices or do we have an error?
          lastError = Marshal.GetLastWin32Error();
          if (lastError != 0x0103 && lastError != 0x007E)
          {
            SetupDiDestroyDeviceInfoList(handle);
            throw new Win32Exception(Marshal.GetLastWin32Error());
          }

          SetupDiDestroyDeviceInfoList(handle);
          break;
        }

        DeviceInterfaceData deviceInterfaceData = new DeviceInterfaceData();
        deviceInterfaceData.Size = Marshal.SizeOf(deviceInterfaceData);

        if (SetupDiEnumDeviceInterfaces(handle, ref deviceInfoData, ref classGuid, 0, ref deviceInterfaceData) == false)
        {
          SetupDiDestroyDeviceInfoList(handle);
          throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        uint cbData = 0;

        if (SetupDiGetDeviceInterfaceDetail(handle, ref deviceInterfaceData, IntPtr.Zero, 0, ref cbData, IntPtr.Zero) == false && cbData == 0)
        {
          SetupDiDestroyDeviceInfoList(handle);
          throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        DeviceInterfaceDetailData deviceInterfaceDetailData = new DeviceInterfaceDetailData();
        deviceInterfaceDetailData.Size = 5;

        if (SetupDiGetDeviceInterfaceDetail(handle, ref deviceInterfaceData, ref deviceInterfaceDetailData, cbData, IntPtr.Zero, IntPtr.Zero) == false)
        {
          SetupDiDestroyDeviceInfoList(handle);
          throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        if (deviceInterfaceDetailData.DevicePath.IndexOf(DeviceID, StringComparison.InvariantCultureIgnoreCase) != -1)
        {
          SetupDiDestroyDeviceInfoList(handle);
          devicePath = deviceInterfaceDetailData.DevicePath;
          break;
        }
      }

      return devicePath;
    }

    void OnReadComplete(IAsyncResult asyncResult)
    {
      try
      {
        if (_deviceStream.EndRead(asyncResult) == 4 && _deviceBuffer[1] == 0)
        {
          TimeSpan timeSpan = DateTime.Now - _lastCodeTime;

          int keyCode = (int)_deviceBuffer[2];

          if (keyCode != _lastCode || timeSpan.Milliseconds > 250)
          {
            if (_remoteButtonHandler != null)
              _remoteButtonHandler(this.Name, keyCode.ToString());

            _lastCodeTime = DateTime.Now;
          }

          _lastCode = keyCode;
        }

        _deviceStream.BeginRead(_deviceBuffer, 0, _deviceBuffer.Length, new AsyncCallback(OnReadComplete), null);
      }
      catch (Exception)
      {
      }
    }

    #endregion Implementation

  }

}
