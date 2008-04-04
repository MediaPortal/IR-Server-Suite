using System;
using System.ComponentModel;
#if TRACE
using System.Diagnostics;
#endif
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace InputService.Plugin
{

  /// <summary>
  /// IR Server Plugin for the IR507 IR receiver.
  /// </summary>
  [CLSCompliant(false)]
  public class IR507Receiver : PluginBase, IRemoteReceiver
  {

    #region Debug
#if DEBUG

    [STAThread]
    static void Main()
    {
      try
      {
        IR507Receiver c = new IR507Receiver();

        c.RemoteCallback += new RemoteHandler(Remote);
        c.Start();

        Application.Run();

        c.Stop();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }

    static void Remote(string deviceName, string code)
    {
      Console.WriteLine("Remote: {0}", code);
    }

#endif
    #endregion Debug


    #region Interop

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

    //const string DeviceID = "vid_0e6a&pid_6002";  // Unknown
    const string DeviceID = "vid_147a&pid_e02a";

    #endregion Constants

    #region Variables

    RemoteHandler _remoteButtonHandler;

    ReceiverWindow _receiverWindow;
    RawInput.RAWINPUTDEVICE _device;

    string _lastCode = String.Empty;
    DateTime _lastCodeTime = DateTime.Now;

    #endregion Variables

    #region Implementation

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name         { get { return "IR507"; } }
    /// <summary>
    /// IR Server plugin version.
    /// </summary>
    /// <value>The version.</value>
    public override string Version      { get { return "1.4.2.0"; } }
    /// <summary>
    /// The IR Server plugin's author.
    /// </summary>
    /// <value>The author.</value>
    public override string Author       { get { return "and-81"; } }
    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    /// <value>The description.</value>
    public override string Description  { get { return "Support for the IR507 IR Receiver"; } }
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
      _receiverWindow = new ReceiverWindow("IR507 Receiver");
      _receiverWindow.ProcMsg += new ProcessMessage(ProcMessage);

      _device.usUsage     = 1;
      _device.usUsagePage = 12;
      _device.dwFlags     = RawInput.RawInputDeviceFlags.InputSink;
      _device.hwndTarget  = _receiverWindow.Handle;

      if (!RegisterForRawInput(_device))
        throw new InvalidOperationException("Failed to register for HID Raw input");
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
      _device.dwFlags |= RawInput.RawInputDeviceFlags.Remove;
      RegisterForRawInput(_device);

      _receiverWindow.ProcMsg -= new ProcessMessage(ProcMessage);
      _receiverWindow.DestroyHandle();
      _receiverWindow = null;
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

    bool RegisterForRawInput(RawInput.RAWINPUTDEVICE device)
    {
      RawInput.RAWINPUTDEVICE[] devices = new RawInput.RAWINPUTDEVICE[1];
      devices[0] = device;

      return RegisterForRawInput(devices);
    }
    bool RegisterForRawInput(RawInput.RAWINPUTDEVICE[] devices)
    {
      return RawInput.RegisterRawInputDevices(devices, (uint)devices.Length, (uint)Marshal.SizeOf(devices[0]));
    }

    void ProcMessage(ref Message m)
    {
      if (m.Msg != RawInput.WM_INPUT)
        return;

      uint dwSize = 0;

      RawInput.GetRawInputData(m.LParam, RawInput.RawInputCommand.Input, IntPtr.Zero, ref dwSize, (uint)Marshal.SizeOf(typeof(RawInput.RAWINPUTHEADER)));

      IntPtr buffer = Marshal.AllocHGlobal((int)dwSize);
      try
      {
        if (buffer == IntPtr.Zero)
          return;

        if (RawInput.GetRawInputData(m.LParam, RawInput.RawInputCommand.Input, buffer, ref dwSize, (uint)Marshal.SizeOf(typeof(RawInput.RAWINPUTHEADER))) != dwSize)
          return;

        RawInput.RAWINPUT raw = (RawInput.RAWINPUT)Marshal.PtrToStructure(buffer, typeof(RawInput.RAWINPUT));

        if (raw.header.dwType == RawInput.RawInputType.HID)
        {
          int offset = Marshal.SizeOf(typeof(RawInput.RAWINPUTHEADER)) + Marshal.SizeOf(typeof(RawInput.RAWHID));

          byte[] bRawData = new byte[offset + raw.hid.dwSizeHid];
          Marshal.Copy(buffer, bRawData, 0, bRawData.Length);

          byte[] newArray = new byte[raw.hid.dwSizeHid];
          Array.Copy(bRawData, offset, newArray, 0, newArray.Length);

          string code = BitConverter.ToString(newArray);

          TimeSpan timeSpan = DateTime.Now - _lastCodeTime;

          if (!code.Equals(_lastCode, StringComparison.Ordinal) || timeSpan.Milliseconds > 250)
          {
            if (_remoteButtonHandler != null)
              _remoteButtonHandler(this.Name, code);

            _lastCodeTime = DateTime.Now;
          }

          _lastCode = code;
        }
      }
      finally
      {
        Marshal.FreeHGlobal(buffer);
      }

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

    #endregion Implementation

  }

}
