#region Copyright (C) 2005-2012 Team MediaPortal

// Copyright (C) 2005-2012 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace IRServer.Plugin
{
  /// <summary>
  /// Device Details used to register for raw input.
  /// </summary>
  internal class DeviceDetails
  {
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the ID.
    /// </summary>
    /// <value>The ID.</value>
    public string ID { get; set; }

    /// <summary>
    /// Gets or sets the usage page.
    /// </summary>
    /// <value>The usage page.</value>
    public ushort UsagePage { get; set; }

    /// <summary>
    /// Gets or sets the usage.
    /// </summary>
    /// <value>The usage.</value>
    public ushort Usage { get; set; }
  }

  internal static class RawInput
  {
    #region Interop

    [DllImport("User32.dll")]
    internal static extern uint GetRawInputData(
      IntPtr hRawInput,
      RawInputCommand uiCommand,
      IntPtr pData,
      ref uint pcbSize,
      uint cbSizeHeader);

    [DllImport("User32.dll")]
    internal static extern bool RegisterRawInputDevices(
      RAWINPUTDEVICE[] pRawInputDevice,
      uint uiNumDevices,
      uint cbSize);

    [DllImport("User32.dll")]
    internal static extern uint GetRawInputDeviceList(
      IntPtr pRawInputDeviceList,
      ref uint uiNumDevices,
      uint cbSize);

    [DllImport("User32.dll")]
    internal static extern uint GetRawInputDeviceInfo(
      IntPtr hDevice,
      uint uiCommand,
      IntPtr pData,
      ref uint pcbSize);

    [DllImport("User32.dll")]
    internal static extern uint GetRawInputDeviceInfo(
      IntPtr deviceHandle,
      uint uiCommand,
      ref DeviceInfo data,
      ref uint dataSize);

    #endregion Interop

    #region Constants

    public const int KEYBOARD_OVERRUN_MAKE_CODE = 0x00FF;
    public const int RIDI_DEVICEINFO = 0x2000000B;
    public const int RIDI_DEVICENAME = 0x20000007;
    public const int RIDI_PREPARSEDDATA = 0x20000005;
    public const int WM_APPCOMMAND = 0x0319;
    public const int WM_INPUT = 0x00FF;
    public const int WM_KEYDOWN = 0x0100;
    public const int WM_KEYUP = 0x0101;
    public const int WM_SYSKEYDOWN = 0x0104;

    #endregion Constants

    #region Enumerations

    #region RawInputCommand enum

    public enum RawInputCommand
    {
      Input = 0x10000003,
      Header = 0x10000005,
    }

    #endregion

    #region RawInputDeviceFlags enum

    [Flags]
    public enum RawInputDeviceFlags
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

    #endregion

    #region RawInputType enum

    public enum RawInputType
    {
      Mouse = 0,
      Keyboard = 1,
      HID = 2
    }

    #endregion

    #region RawKeyboardFlags enum

    [Flags]
    public enum RawKeyboardFlags : ushort
    {
      KeyMake = 0x00,
      KeyBreak = 0x01,
      KeyE0 = 0x02,
      KeyE1 = 0x04,
      TerminalServerSetLED = 0x08,
      TerminalServerShadow = 0x10
    }

    #endregion

    #region RawMouseButtons enum

    [Flags]
    public enum RawMouseButtons : ushort
    {
      None = 0,
      LeftDown = 0x0001,
      LeftUp = 0x0002,
      RightDown = 0x0004,
      RightUp = 0x0008,
      MiddleDown = 0x0010,
      MiddleUp = 0x0020,
      Button4Down = 0x0040,
      Button4Up = 0x0080,
      Button5Down = 0x0100,
      Button5Up = 0x0200,
      MouseWheel = 0x0400
    }

    #endregion

    #region RawMouseFlags enum

    [Flags]
    public enum RawMouseFlags : ushort
    {
      MoveRelative = 0,
      MoveAbsolute = 1,
      VirtualDesktop = 2,
      AttributesChanged = 4
    }

    #endregion

    #endregion Enumerations

    #region Structures

    #region Nested type: BUTTONSSTR

    [StructLayout(LayoutKind.Sequential)]
    public struct BUTTONSSTR
    {
      [MarshalAs(UnmanagedType.U2)] public ushort usButtonFlags;
      [MarshalAs(UnmanagedType.U2)] public ushort usButtonData;
    }

    #endregion

    #region Nested type: DeviceInfo

    [StructLayout(LayoutKind.Explicit)]
    public struct DeviceInfo
    {
      [FieldOffset(0)] public int Size;
      [FieldOffset(4)] public RawInputType Type;

      [FieldOffset(8)] public DeviceInfoMouse MouseInfo;
      [FieldOffset(8)] public DeviceInfoKeyboard KeyboardInfo;
      [FieldOffset(8)] public DeviceInfoHID HIDInfo;
    }

    #endregion

    #region Nested type: DeviceInfoHID

    public struct DeviceInfoHID
    {
      public uint VendorID;
      public uint ProductID;
      public uint Revision;
      public ushort UsagePage;
      public ushort Usage;
      public uint VersionNumber;
    }

    #endregion

    #region Nested type: DeviceInfoKeyboard

    public struct DeviceInfoKeyboard
    {
      public uint KeyboardMode;
      public uint NumberOfFunctionKeys;
      public uint NumberOfIndicators;
      public uint NumberOfKeysTotal;
      public uint SubType;
      public uint Type;
    }

    #endregion

    #region Nested type: DeviceInfoMouse

    public struct DeviceInfoMouse
    {
      public uint ID;
      public uint NumberOfButtons;
      public uint SampleRate;
    }

    #endregion

    #region Nested type: RAWHID

    [StructLayout(LayoutKind.Sequential)]
    public struct RAWHID
    {
      [MarshalAs(UnmanagedType.U4)] public int dwSizeHid;
      [MarshalAs(UnmanagedType.U4)] public int dwCount;

      //public IntPtr bRawData;
    }

    #endregion

    #region Nested type: RAWINPUT

    [StructLayout(LayoutKind.Explicit)]
    public struct RAWINPUT
    {
      [FieldOffset(0)] public RAWINPUTHEADER header;
      [FieldOffset(16)] public RAWMOUSE mouse;
      [FieldOffset(16)] public RAWKEYBOARD keyboard;
      [FieldOffset(16)] public RAWHID hid;
    }

    #endregion

    #region Nested type: RAWINPUTDEVICE

    [StructLayout(LayoutKind.Sequential)]
    public struct RAWINPUTDEVICE
    {
      [MarshalAs(UnmanagedType.U2)] public ushort usUsagePage;
      [MarshalAs(UnmanagedType.U2)] public ushort usUsage;
      [MarshalAs(UnmanagedType.U4)] public RawInputDeviceFlags dwFlags;
      public IntPtr hwndTarget;
    }

    #endregion

    #region Nested type: RAWINPUTDEVICELIST

    [StructLayout(LayoutKind.Sequential)]
    public struct RAWINPUTDEVICELIST
    {
      public IntPtr hDevice;
      [MarshalAs(UnmanagedType.U4)] public RawInputType dwType;
    }

    #endregion

    #region Nested type: RAWINPUTHEADER

    [StructLayout(LayoutKind.Sequential)]
    public struct RAWINPUTHEADER
    {
      [MarshalAs(UnmanagedType.U4)] public RawInputType dwType;
      [MarshalAs(UnmanagedType.U4)] public int dwSize;
      public IntPtr hDevice;
      [MarshalAs(UnmanagedType.U4)] public int wParam;
    }

    #endregion

    #region Nested type: RAWKEYBOARD

    [StructLayout(LayoutKind.Sequential)]
    public struct RAWKEYBOARD
    {
      [MarshalAs(UnmanagedType.U2)] public ushort MakeCode;
      [MarshalAs(UnmanagedType.U2)] public RawKeyboardFlags Flags;
      [MarshalAs(UnmanagedType.U2)] public ushort Reserved;
      [MarshalAs(UnmanagedType.U2)] public ushort VKey;
      [MarshalAs(UnmanagedType.U4)] public uint Message;
      [MarshalAs(UnmanagedType.U4)] public uint ExtraInformation;
    }

    #endregion

    #region Nested type: RAWMOUSE

    [StructLayout(LayoutKind.Explicit)]
    public struct RAWMOUSE
    {
      [MarshalAs(UnmanagedType.U2)] [FieldOffset(0)] public ushort usFlags;
      [MarshalAs(UnmanagedType.U4)] [FieldOffset(4)] public uint ulButtons;
      [FieldOffset(4)] public BUTTONSSTR buttonsStr;
      [MarshalAs(UnmanagedType.U4)] [FieldOffset(8)] public uint ulRawButtons;
      [FieldOffset(12)] public int lLastX;
      [FieldOffset(16)] public int lLastY;
      [MarshalAs(UnmanagedType.U4)] [FieldOffset(20)] public uint ulExtraInformation;
    }

    #endregion

    #endregion Structures

    public static List<DeviceDetails> EnumerateDevices()
    {
      uint deviceCount = 0;
      int dwSize = Marshal.SizeOf(typeof (RAWINPUTDEVICELIST));

      // Get the number of raw input devices in the list,
      // then allocate sufficient memory and get the entire list
      if (GetRawInputDeviceList(IntPtr.Zero, ref deviceCount, (uint) dwSize) == 0)
      {
        IntPtr pRawInputDeviceList = Marshal.AllocHGlobal((int) (dwSize*deviceCount));
        GetRawInputDeviceList(pRawInputDeviceList, ref deviceCount, (uint) dwSize);

        List<DeviceDetails> devices = new List<DeviceDetails>((int) deviceCount);

        // Iterate through the list, discarding undesired items
        // and retrieving further information on keyboard devices
        for (int i = 0; i < deviceCount; i++)
        {
          uint pcbSize = 0;

          IntPtr location;
          int offset = dwSize*i;

          if (IntPtr.Size == 4)
            location = new IntPtr(pRawInputDeviceList.ToInt32() + offset);
          else
            location = new IntPtr(pRawInputDeviceList.ToInt64() + offset);

          RAWINPUTDEVICELIST rid = (RAWINPUTDEVICELIST) Marshal.PtrToStructure(location, typeof (RAWINPUTDEVICELIST));

          GetRawInputDeviceInfo(rid.hDevice, RIDI_DEVICENAME, IntPtr.Zero, ref pcbSize);

          if (pcbSize > 0)
          {
            IntPtr pData = Marshal.AllocHGlobal((int) pcbSize);
            GetRawInputDeviceInfo(rid.hDevice, RIDI_DEVICENAME, pData, ref pcbSize);
            string deviceName = Marshal.PtrToStringAnsi(pData);

            // Drop the "root" keyboard and mouse devices used for Terminal Services and the Remote Desktop
            if (deviceName.ToUpperInvariant().Contains("ROOT"))
              continue;

            // If the device is identified in the list as a keyboard or 
            // HID device, create a DeviceInfo object to store information 
            // about it

            // Get Detailed Info ...
            uint size = (uint) Marshal.SizeOf(typeof (DeviceInfo));
            DeviceInfo di = new DeviceInfo();
            di.Size = Marshal.SizeOf(typeof (DeviceInfo));
            GetRawInputDeviceInfo(rid.hDevice, RIDI_DEVICEINFO, ref di, ref size);

            di = new DeviceInfo();
            di.Size = Marshal.SizeOf(typeof (DeviceInfo));
            GetRawInputDeviceInfo(rid.hDevice, RIDI_DEVICEINFO, ref di, ref size);

            DeviceDetails details = new DeviceDetails();
            //details.Name = deviceName;
            details.ID = deviceName;

            switch (di.Type)
            {
              case RawInputType.HID:
                {
                  string vidAndPid = String.Format("Vid_{0:x4}&Pid_{1:x4}", di.HIDInfo.VendorID, di.HIDInfo.ProductID);
                  details.Name = String.Format("HID: {0}", GetFriendlyName(vidAndPid));
                  //details.ID = GetDeviceDesc(deviceName);

                  details.UsagePage = di.HIDInfo.UsagePage;
                  details.Usage = di.HIDInfo.Usage;

                  devices.Add(details);
                  break;
                }

              case RawInputType.Keyboard:
                {
                  details.Name = "HID Keyboard";
                  //details.ID = String.Format("{0}-{1}", di.KeyboardInfo.Type, di.KeyboardInfo.SubType);

                  details.UsagePage = 0x01;
                  details.Usage = 0x06;

                  devices.Add(details);
                  break;
                }

              case RawInputType.Mouse:
                {
                  details.Name = "HID Mouse";

                  details.UsagePage = 0x01;
                  details.Usage = 0x02;

                  devices.Add(details);
                  break;
                }
            }

            Marshal.FreeHGlobal(pData);
          }
        }

        Marshal.FreeHGlobal(pRawInputDeviceList);

        return devices;
      }
      else
      {
        throw new InvalidOperationException("An error occurred while retrieving the list of devices");
      }
    }

    private static string GetFriendlyName(string vidAndPid)
    {
      try
      {
        using (RegistryKey usbEnum = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Enum\\USB"))
        {
          foreach (string usbSubKey in usbEnum.GetSubKeyNames())
          {
            if (usbSubKey.IndexOf(vidAndPid, StringComparison.OrdinalIgnoreCase) == -1)
              continue;

            using (RegistryKey currentKey = usbEnum.OpenSubKey(usbSubKey))
            {
              string[] vidAndPidSubKeys = currentKey.GetSubKeyNames();

              foreach (string vidAndPidSubKey in vidAndPidSubKeys)
              {
                using (RegistryKey subKey = currentKey.OpenSubKey(vidAndPidSubKey))
                  return subKey.GetValue("LocationInformation", null) as string;
              }
            }
          }
        }
      }
      catch
      {
      }

      return null;
    }
  }
}