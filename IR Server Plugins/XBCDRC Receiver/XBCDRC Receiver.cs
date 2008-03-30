using System;
using System.ComponentModel;
#if TRACE
using System.Diagnostics;
#endif
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

using Microsoft.Win32.SafeHandles;

namespace InputService.Plugin
{

  /// <summary>
  /// IR Server Plugin for the XBox 1 IR Receiver with XBCDRC.
  /// </summary>
  public class XBCDRCReceiver : PluginBase, IRemoteReceiver, IDisposable
  {

    // #define TEST_APPLICATION in the project properties when creating the console test app ...
#if TEST_APPLICATION

    static void Remote(string deviceName, string code)
    {
      Console.WriteLine("Remote: {0}", code);
    }

    [STAThread]
    static void Main()
    {
      XBCDRCReceiver c;

      try
      {
        c = new XBCDRCReceiver();

        //c.Configure(null);

        c.RemoteCallback += new RemoteHandler(Remote);

        c.Start();

        System.Windows.Forms.Application.Run();

        c.Stop();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
      finally
      {
        c = null;
      }

      Console.ReadKey();
    }

#endif


    #region Constants

    const int DeviceBufferSize = 7;
    const string DevicePathVidPid = "vid_045e&pid_0284";
    // const string DevicePathVidPid = "vid_1130&pid_cc00"; Asus DH Remote

    #endregion Constants

    #region Variables

    RemoteHandler _remoteButtonHandler;
    FileStream _deviceStream;
    byte[] _deviceBuffer;

    int _lastPacketID;
    string _lastCode        = String.Empty;
    DateTime _lastCodeTime  = DateTime.Now;

    bool _disposed;

    #endregion Variables

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

    [DllImport("hid")]
    static extern void HidD_GetHidGuid(
      ref Guid guid);

    [DllImport("setupapi", CharSet = CharSet.Auto, SetLastError = true)]
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

    #region Destructor

    /// <summary>
    /// Releases unmanaged resources and performs other cleanup operations before the
    /// <see cref="XBCDRCReceiver"/> is reclaimed by garbage collection.
    /// </summary>
    ~XBCDRCReceiver()
    {
      // Call Dispose with false.  Since we're in the destructor call, the managed resources will be disposed of anyway.
      Dispose(false);
    }

    #endregion Destructor

    #region IDisposable Members

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources
    /// </summary>
    public void Dispose()
    {
      // Dispose of the managed and unmanaged resources
      Dispose(true);

      // Tell the GC that the Finalize process no longer needs to be run for this object.
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      // process only if mananged and unmanaged resources have
      // not been disposed of.
      if (!_disposed)
      {
        if (disposing)
        {
          // dispose managed resources
          Stop();
        }

        // dispose unmanaged resources
        _disposed = true;
      }
    }


    #endregion

    #region Implementation

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name         { get { return "XBCDRC"; } }
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
    public override string Description  { get { return "Supports the XBox 1 IR receiver with XBCDRC"; } }
    /// <summary>
    /// Gets a display icon for the plugin.
    /// </summary>
    /// <value>The icon.</value>
    public override Icon DeviceIcon     { get { return Properties.Resources.Icon; } }

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
        throw new InvalidOperationException("Device not found");

      SafeFileHandle deviceHandle = CreateFile(devicePath, FileAccess.Read, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, EFileAttributes.Overlapped, IntPtr.Zero);
      int lastError = Marshal.GetLastWin32Error();

      if (deviceHandle.IsInvalid)
        throw new Win32Exception(lastError, "Failed to open device");

      // TODO: Add device removal notification.
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

    /// <summary>
    /// Finds the device.
    /// </summary>
    /// <param name="classGuid">The class GUID.</param>
    /// <returns>Device path.</returns>
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

        if (!SetupDiEnumDeviceInfo(handle, deviceIndex, ref deviceInfoData))
        {
          // out of devices or do we have an error?
          lastError = Marshal.GetLastWin32Error();
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

        if (deviceInterfaceDetailData.DevicePath.IndexOf(DevicePathVidPid, StringComparison.OrdinalIgnoreCase) != -1)
        {
          SetupDiDestroyDeviceInfoList(handle);
          devicePath = deviceInterfaceDetailData.DevicePath;
          break;
        }
      }

      return devicePath;
    }

    /// <summary>
    /// Called when a device read is completed.
    /// </summary>
    /// <param name="asyncResult">The async result.</param>
    void OnReadComplete(IAsyncResult asyncResult)
    {
      try
      {
        if (_deviceStream.EndRead(asyncResult) == DeviceBufferSize && _deviceBuffer[1] != 1 && _remoteButtonHandler != null)
        {
          int packetID = BitConverter.ToInt32(_deviceBuffer, 3);

          if (packetID != _lastPacketID)
          {
            _lastPacketID = packetID;

            TimeSpan timeSpan = DateTime.Now - _lastCodeTime;

            string keyCode = ((int)_deviceBuffer[3]).ToString();

            if (keyCode != _lastCode || timeSpan.Milliseconds >= 250)
            {
              _remoteButtonHandler(this.Name, keyCode);

              _lastCodeTime = DateTime.Now;
            }

            _lastCode = keyCode;
          }
        }

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
