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
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using InputService.Plugin.Properties;
using IrssUtils;

namespace InputService.Plugin
{
  /// <summary>
  /// IR Server Plugin for the IR507 IR receiver.
  /// </summary>
  [CLSCompliant(false)]
  public class IR507Receiver : PluginBase, IRemoteReceiver
  {
    #region Interop

    [DllImport("hid")]
    private static extern void HidD_GetHidGuid(
      ref Guid guid);

    [DllImport("setupapi", CharSet = CharSet.Auto)]
    private static extern IntPtr SetupDiGetClassDevs(
      ref Guid ClassGuid,
      [MarshalAs(UnmanagedType.LPTStr)] string Enumerator,
      IntPtr hwndParent,
      UInt32 Flags);

    [DllImport("setupapi", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetupDiEnumDeviceInfo(
      IntPtr handle,
      int Index,
      ref DeviceInfoData deviceInfoData);

    [DllImport("setupapi", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetupDiEnumDeviceInterfaces(
      IntPtr handle,
      ref DeviceInfoData deviceInfoData,
      ref Guid guidClass,
      int MemberIndex,
      ref DeviceInterfaceData deviceInterfaceData);

    [DllImport("setupapi", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetupDiGetDeviceInterfaceDetail(
      IntPtr handle,
      ref DeviceInterfaceData deviceInterfaceData,
      IntPtr unused1,
      int unused2,
      ref uint requiredSize,
      IntPtr unused3);

    [DllImport("setupapi", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetupDiGetDeviceInterfaceDetail(
      IntPtr handle,
      ref DeviceInterfaceData deviceInterfaceData,
      ref DeviceInterfaceDetailData deviceInterfaceDetailData,
      uint detailSize,
      IntPtr unused1,
      IntPtr unused2);

    [DllImport("setupapi")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetupDiDestroyDeviceInfoList(IntPtr handle);

    #region Nested type: DeviceInfoData

    [StructLayout(LayoutKind.Sequential)]
    private struct DeviceInfoData
    {
      public int Size;
      public Guid Class;
      public uint DevInst;
      public IntPtr Reserved;
    }

    #endregion

    #region Nested type: DeviceInterfaceData

    [StructLayout(LayoutKind.Sequential)]
    private struct DeviceInterfaceData
    {
      public int Size;
      public Guid Class;
      public uint Flags;
      public uint Reserved;
    }

    #endregion

    #region Nested type: DeviceInterfaceDetailData

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    private struct DeviceInterfaceDetailData
    {
      public int Size;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
      public string DevicePath;
    }

    #endregion

    #endregion Interop

    #region Constants

    //const string DeviceID = "vid_0e6a&pid_6002";  // Unknown
    private const string DeviceID = "vid_147a&pid_e02a";

    #endregion Constants

    #region Variables

    private RawInput.RAWINPUTDEVICE _device;

    private string _lastCode = String.Empty;
    private DateTime _lastCodeTime = DateTime.Now;
    private ReceiverWindow _receiverWindow;
    private RemoteHandler _remoteButtonHandler;

    #endregion Variables

    #region Implementation

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name
    {
      get { return "IR507"; }
    }

    /// <summary>
    /// IR Server plugin version.
    /// </summary>
    /// <value>The version.</value>
    public override string Version
    {
      get { return "1.4.2.0"; }
    }

    /// <summary>
    /// The IR Server plugin's author.
    /// </summary>
    /// <value>The author.</value>
    public override string Author
    {
      get { return "and-81"; }
    }

    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    /// <value>The description.</value>
    public override string Description
    {
      get { return "Support for the IR507 IR Receiver"; }
    }

    /// <summary>
    /// Gets a display icon for the plugin.
    /// </summary>
    /// <value>The icon.</value>
    public override Icon DeviceIcon
    {
      get { return Resources.Icon; }
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
    /// Detect the presence of this device.
    /// </summary>
    public override DetectionResult Detect()
    {
      try
      {
        Guid guid = new Guid();
        HidD_GetHidGuid(ref guid);

        string devicePath = FindDevice(guid);

        if (devicePath != null)
        {
          return DetectionResult.DevicePresent;
        }
      }
      catch (Win32Exception ex)
      {
        if (ex.NativeErrorCode != 2)
        {
          IrssLog.Error("{0} exception: {1}", Name, ex.NativeErrorCode);
          return DetectionResult.DeviceException;
        }
        IrssLog.Debug("{0} exception: {1}", Name, ex.NativeErrorCode);
      }
      catch (Exception ex)
      {
        IrssLog.Error("{0} exception: {1} type: {2}", Name, ex.Message, ex.GetType());
        return DetectionResult.DeviceException;
      }

      return DetectionResult.DeviceNotFound;
    }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    public override void Start()
    {
      _receiverWindow = new ReceiverWindow("IR507 Receiver");
      _receiverWindow.ProcMsg += ProcMessage;

      _device.usUsage = 1;
      _device.usUsagePage = 12;
      _device.dwFlags = RawInput.RawInputDeviceFlags.InputSink;
      _device.hwndTarget = _receiverWindow.Handle;

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

      _receiverWindow.ProcMsg -= ProcMessage;
      _receiverWindow.DestroyHandle();
      _receiverWindow = null;
    }

    private bool RegisterForRawInput(RawInput.RAWINPUTDEVICE device)
    {
      RawInput.RAWINPUTDEVICE[] devices = new RawInput.RAWINPUTDEVICE[1];
      devices[0] = device;

      return RegisterForRawInput(devices);
    }

    private bool RegisterForRawInput(RawInput.RAWINPUTDEVICE[] devices)
    {
      return RawInput.RegisterRawInputDevices(devices, (uint)devices.Length, (uint)Marshal.SizeOf(devices[0]));
    }

    private void ProcMessage(ref Message m)
    {
      if (m.Msg != RawInput.WM_INPUT)
        return;

      uint dwSize = 0;

      RawInput.GetRawInputData(m.LParam, RawInput.RawInputCommand.Input, IntPtr.Zero, ref dwSize,
                               (uint)Marshal.SizeOf(typeof(RawInput.RAWINPUTHEADER)));

      IntPtr buffer = Marshal.AllocHGlobal((int)dwSize);
      try
      {
        if (buffer == IntPtr.Zero)
          return;

        if (
          RawInput.GetRawInputData(m.LParam, RawInput.RawInputCommand.Input, buffer, ref dwSize,
                                   (uint)Marshal.SizeOf(typeof(RawInput.RAWINPUTHEADER))) != dwSize)
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
              _remoteButtonHandler(Name, code);

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

    private static string FindDevice(Guid classGuid)
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

        if (SetupDiGetDeviceInterfaceDetail(handle, ref deviceInterfaceData, IntPtr.Zero, 0, ref cbData, IntPtr.Zero) ==
            false && cbData == 0)
        {
          SetupDiDestroyDeviceInfoList(handle);
          throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        DeviceInterfaceDetailData deviceInterfaceDetailData = new DeviceInterfaceDetailData();
        deviceInterfaceDetailData.Size = 5;

        if (
          SetupDiGetDeviceInterfaceDetail(handle, ref deviceInterfaceData, ref deviceInterfaceDetailData, cbData,
                                          IntPtr.Zero, IntPtr.Zero) == false)
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

    // #define TEST_APPLICATION in the project properties when creating the console test app ...
#if TEST_APPLICATION

    static void xRemote(string deviceName, string code)
    {
      Console.WriteLine("Remote: {0}", code);
    }

    [STAThread]
    static void Main()
    {
      try
      {
        IR507Receiver device = new IR507Receiver();

        device.RemoteCallback += new RemoteHandler(xRemote);
        device.Start();

        Application.Run();

        device.Stop();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }

#endif
  }
}