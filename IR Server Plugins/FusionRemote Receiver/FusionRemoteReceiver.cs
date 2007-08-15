using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Win32.SafeHandles;

using IRServerPluginInterface;

namespace FusionRemoteReceiver
{

  #region Delegates

  delegate void RemoteEventHandler(byte[] data);

  #endregion Delegates

  public class FusionRemoteReceiver : IIRServerPlugin
  {

    #region Constants

    static readonly string ConfigurationFile =
      Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
      "\\IR Server Suite\\IR Server\\FusionRemote Receiver.xml";

    static readonly string[] Ports  = new string[] { "None" };

    const string DeviceID = "VID_0FE9&PID_9010";

    const UInt32 DIGCF_ALLCLASSES       = 0x00000004;
    const UInt32 DIGCF_DEVICEINTERFACE  = 0x00000010;
    const UInt32 DIGCF_PRESENT          = 0x00000002;
    const UInt32 DIGCF_PROFILE          = 0x00000008;

    // File Access Types
    const uint GENERIC_READ     = 0x80000000;
    const uint GENERIC_WRITE    = 0x40000000;
    const uint GENERIC_EXECUTE  = 0x20000000;
    const uint GENERIC_ALL      = 0x10000000;

    #endregion Constants

    #region Interop

    [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Auto)]
    static extern SafeFileHandle CreateFile(
      [MarshalAs(UnmanagedType.LPTStr)] string fileName,
      uint fileAccess,
      [MarshalAs(UnmanagedType.U4)] EFileShares fileShare,
      //[In, Out, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(SecurityAttributesMarshaler))] SecurityAttributes lpSecurityAttributes,
      IntPtr sa,
      [MarshalAs(UnmanagedType.U4)] ECreationDisposition creationDisposition,
      [MarshalAs(UnmanagedType.U4)] EFileAttributes flags,
      IntPtr templateFile);

    [DllImport("kernel32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool CancelIo(SafeFileHandle handle);

    [Flags]
    public enum EFileShares
    {
       None   = 0x00000000,
       Read   = 0x00000001,
       Write  = 0x00000002,
       Delete = 0x00000004,
    }

    public enum ECreationDisposition
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

    #region Variables

    static RemoteHandler _remoteButtonHandler = null;

    static FileStream _deviceStream;
    static byte[] _deviceBuffer;
    static DateTime _lastCodeTime = DateTime.Now;

    #endregion Variables
   
    #region IIRServerPlugin Members

    public string Name          { get { return "FusionREMOTE"; } }
    public string Version       { get { return "1.0.3.3"; } }
    public string Author        { get { return "and-81"; } }
    public string Description   { get { return "DViCO FusionREMOTE Receiver"; } }
    public bool   CanReceive    { get { return true; } }
    public bool   CanTransmit   { get { return false; } }
    public bool   CanLearn      { get { return false; } }
    public bool   CanConfigure  { get { return false; } }

    public RemoteHandler RemoteCallback
    {
      get { return _remoteButtonHandler; }
      set { _remoteButtonHandler = value; }
    }

    public KeyboardHandler KeyboardCallback { get { return null; } set { } }

    public MouseHandler MouseCallback { get { return null; } set { } }

    public string[] AvailablePorts
    {
      get { return Ports; }
    }

    public void Configure() { }
    public bool Start()
    {
      try
      {
        OpenDevice();

        _deviceBuffer = new byte[128];

        _deviceStream.BeginRead(_deviceBuffer, 0, _deviceBuffer.Length, new AsyncCallback(OnReadComplete), null);

        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return false;
      }
    }
    public void Suspend() { }
    public void Resume() { }
    public void Stop()
    {
      try
      {
        _deviceStream.Close();
        _deviceStream = null;
      }
      catch (IOException)
      {
        // we are closing the stream so ignore this
      }
    }

    public bool Transmit(string file)
    {
      return false;
    }
    public LearnStatus Learn(out byte[] data)
    {
      data = null;
      return LearnStatus.Failure;
    }

    public bool SetPort(string port)
    {
      return true;
    }

    #endregion

    #region Implementation

    static void OpenDevice()
    {
      Guid hidGuid = new Guid();
      HidD_GetHidGuid(ref hidGuid);

      string devicePath = FindDevice(hidGuid, DeviceID);

      if (devicePath == null)
        throw new Exception("No device detected");

      SafeFileHandle deviceHandle = CreateFile(devicePath, GENERIC_READ, EFileShares.Read | EFileShares.Write, IntPtr.Zero, ECreationDisposition.OpenExisting, EFileAttributes.Overlapped, IntPtr.Zero);
      int lastError = Marshal.GetLastWin32Error();

      if (lastError != 0)
        throw new Win32Exception(lastError);

      _deviceStream = new FileStream(deviceHandle, FileAccess.Read, 128, true);
    }

    static string FindDevice(Guid classGuid, string deviceID)
    {
      string devicePath = null;

      IntPtr handle = SetupDiGetClassDevs(ref classGuid, null, IntPtr.Zero, DIGCF_DEVICEINTERFACE | DIGCF_PRESENT);

      //int lastError = Marshal.GetLastWin32Error();
      //if (lastError != 0)
        //throw new Win32Exception(lastError);

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

        if (deviceInterfaceDetailData.DevicePath.IndexOf(deviceID, StringComparison.InvariantCultureIgnoreCase) != -1)
        {
          SetupDiDestroyDeviceInfoList(handle);
          devicePath = deviceInterfaceDetailData.DevicePath;
          break;
        }
      }

      return devicePath;
    }

    static void OnReadComplete(IAsyncResult asyncResult)
    {
      try
      {
        if (_deviceStream == null)
          return;

        int bytesRead = _deviceStream.EndRead(asyncResult);
        if (bytesRead == 0)
        {
          _deviceStream.Close();
          _deviceStream = null;
          return;
        }

        byte keyCode = _deviceBuffer[3];

        if ((keyCode & 0x80) == 0x00)
        {
          _lastCodeTime = DateTime.Now;

          if (_remoteButtonHandler != null)
            _remoteButtonHandler(keyCode.ToString());
        }
        else
        {
          keyCode &= 0x7F;

          TimeSpan timeSpan = DateTime.Now - _lastCodeTime;
          if (timeSpan.Milliseconds >= 400)
          {
            _lastCodeTime = DateTime.Now;

            if (_remoteButtonHandler != null)
              _remoteButtonHandler(keyCode.ToString());
          }
        }

        // begin another asynchronous read from the device
        if (_deviceStream != null)
          _deviceStream.BeginRead(_deviceBuffer, 0, _deviceBuffer.Length, new AsyncCallback(OnReadComplete), null);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }

    #endregion Implementation

  }

}
