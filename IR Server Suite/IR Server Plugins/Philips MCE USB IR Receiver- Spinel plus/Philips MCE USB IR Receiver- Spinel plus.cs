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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using IRServer.Plugin.Properties;
using IrssUtils;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace IRServer.Plugin
{
  /// <summary>
  /// IR Server plugin to support Philips MCE USB IR Receiver- Spinel plus.
  /// </summary>
  public partial class PhilipsMceUsbIrReceiverSpinelPlus
  {
    #region Classes

    private class Device
    {
      public string Name;
      public List<DeviceDetails> deviceDetails;
    }

    private class DeviceDetails
    {
      public string hwID;
      public ushort UsagePage;
      public ushort Usage;
    }

    #endregion Classes

    #region Global Constants

    private static readonly string ConfigurationFile = Path.Combine(ConfigurationPath,
                                                                    "Philips MCE USB IR Receiver- Spinel plus.xml");

    private static readonly List<Device> supportedDevices = new List<Device>()
                                                              {
                                                                new Device()
                                                                  {
                                                                    Name = "Philips MCE USB IR Receiver- Spinel plus",
                                                                    deviceDetails = new List<DeviceDetails>()
                                                                                      {
                                                                                        new DeviceDetails()
                                                                                          {
                                                                                            hwID =
                                                                                              "Vid_0471&Pid_20cc&Col03",
                                                                                            UsagePage = 0x000c,
                                                                                            Usage = 0x0001
                                                                                          },
                                                                                        new DeviceDetails()
                                                                                          {
                                                                                            hwID =
                                                                                              "Vid_0471&Pid_20cc&Col04",
                                                                                            UsagePage = 0xffbc,
                                                                                            Usage = 0x0088
                                                                                          }
                                                                                      }
                                                                  }
                                                              };

    #endregion

    #region Global Variables

    private Config config;
    private List<RawInput.RAWINPUTDEVICE> _deviceList;
    private ReceiverWindow _receiverWindow;
    private RemoteHandler _remoteHandler;
    private bool _disposed;
    private System.Threading.Timer _RepeatTimer;
    private int _firstRepeatDelay;
    private int _heldRepeatDelay;

    #endregion Global Variables

    #region Destructor

    /// <summary>
    /// Releases unmanaged resources and performs other cleanup operations before the
    /// <see cref="PhilipsMceUsbIrReceiverSpinelPlus"/> is reclaimed by garbage collection.
    /// </summary>
    ~PhilipsMceUsbIrReceiverSpinelPlus()
    {
      // Call Dispose with false.  Since we're in the destructor call, the managed resources will be disposed of anyway.
      Dispose(false);
    }

    #endregion Destructor

    #region PluginBase Functional Implimentation

    /// <summary>
    /// Start the receiver.
    /// </summary>
    private void Start_Receiver()
    {
      Debug.Open("Philips MCE USB IR Receiver- Spinel plus.log");
      Debug.WriteLine("Start_Receiver()");
      config = new Config();
      ConfigManagement.LoadSettings(ref config);

      _firstRepeatDelay = config.firstRepeatDelay;
      _heldRepeatDelay = config.heldRepeatDelay;

      if (config.useSystemRatesDelay)
      {
        _firstRepeatDelay = 250 + (SystemInformation.KeyboardDelay*250);
        _heldRepeatDelay = (int) (1000.0/(2.5 + (SystemInformation.KeyboardSpeed*0.888)));
      }

      // create receiver Window
      _receiverWindow = new ReceiverWindow("Philips MCE USB IR Receiver- Spinel plus Receiver");
      _receiverWindow.ProcMsg += ProcMessage;

      // collect devices
      _deviceList = new List<RawInput.RAWINPUTDEVICE>();
      RawInput.RAWINPUTDEVICE _device;

      foreach (Device device in supportedDevices)
      {
        foreach (DeviceDetails details in device.deviceDetails)
        {
          _device = new RawInput.RAWINPUTDEVICE();
          _device.usUsage = details.Usage;
          _device.usUsagePage = details.UsagePage;
          _device.dwFlags = RawInput.RawInputDeviceFlags.InputSink;
          _device.hwndTarget = _receiverWindow.Handle;
          _deviceList.Add(_device);
        }
      }

      if (!RegisterForRawInput(_deviceList.ToArray()))
      {
        Debug.WriteLine("ERROR: Failed to register for HID Raw input");
        throw new InvalidOperationException("Failed to register for HID Raw input");
      }

      Debug.WriteLine("Start_Receiver(): completed");
    }

    /// <summary>
    /// Stop the receiver.
    /// </summary>
    private void Stop_Receiver()
    {
      Debug.WriteLine("Stop_Receiver()");
      RawInput.RAWINPUTDEVICE[] _deviceTreeArray = _deviceList.ToArray();
      if (_deviceTreeArray != null)
      {
        for (int i = 0; i < _deviceTreeArray.Length; i++)
        {
          _deviceTreeArray[i].dwFlags |= RawInput.RawInputDeviceFlags.Remove;
        }
        RegisterForRawInput(_deviceTreeArray);

        _receiverWindow.ProcMsg -= ProcMessage;
        _receiverWindow.DestroyHandle();
        _receiverWindow = null;
      }
    }

    /// <summary>
    /// Suspend the receiver.
    /// </summary>
    private void Suspend_Receiver()
    {
      Debug.WriteLine("Suspend_Receiver()");
    }

    /// <summary>
    /// Resume the receiver.
    /// </summary>
    private void Resume_Receiver()
    {
      Debug.WriteLine("Resume_Receiver()");
    }

    #endregion

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

    #endregion

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

    #region HID Device Specific Helper Functions

    private bool RegisterForRawInput(RawInput.RAWINPUTDEVICE device)
    {
      RawInput.RAWINPUTDEVICE[] devices = new RawInput.RAWINPUTDEVICE[1];
      devices[0] = device;

      return RegisterForRawInput(devices);
    }

    private bool RegisterForRawInput(RawInput.RAWINPUTDEVICE[] devices)
    {
      Debug.WriteLine("RegisterForRawInput(): Registering {0} device(s).", devices.Length);
      if (
        !RawInput.RegisterRawInputDevices(devices, (uint) devices.Length,
                                          (uint) Marshal.SizeOf(typeof (RawInput.RAWINPUTDEVICE))))
      {
        int dwError = Marshal.GetLastWin32Error();
        Debug.WriteLine("RegisterForRawInput(): error={0}", dwError);
        throw new Win32Exception(dwError, "PhilipsMceUsbIrReceiverSpinelPlus:RegisterForRawInput()");
      }
      Debug.WriteLine("RegisterForRawInput(): Done.");
      return true;
    }

    private void ProcMessage(ref Message m)
    {
      switch (m.Msg)
      {
        case RawInput.WM_INPUT:
          ProcessInputCommand(ref m);
          break;
      }
    }

    private void ProcessInputCommand(ref Message message)
    {
      uint dwSize = 0;

      RawInput.GetRawInputData(message.LParam, RawInput.RawInputCommand.Input, IntPtr.Zero, ref dwSize,
                               (uint) Marshal.SizeOf(typeof (RawInput.RAWINPUTHEADER)));

      IntPtr buffer = Marshal.AllocHGlobal((int) dwSize);
      try
      {
        if (buffer == IntPtr.Zero)
          return;

        if (
          RawInput.GetRawInputData(message.LParam, RawInput.RawInputCommand.Input, buffer, ref dwSize,
                                   (uint) Marshal.SizeOf(typeof (RawInput.RAWINPUTHEADER))) != dwSize)
          return;

        RawInput.RAWINPUT raw = (RawInput.RAWINPUT) Marshal.PtrToStructure(buffer, typeof (RawInput.RAWINPUT));

        // get the name of the device that generated the input message
        string deviceName = string.Empty;
        uint pcbSize = 0;
        RawInput.GetRawInputDeviceInfo(raw.header.hDevice, RawInput.RIDI_DEVICENAME, IntPtr.Zero, ref pcbSize);
        if (pcbSize > 0)
        {
          IntPtr pData = Marshal.AllocHGlobal((int) pcbSize);
          RawInput.GetRawInputDeviceInfo(raw.header.hDevice, RawInput.RIDI_DEVICENAME, pData, ref pcbSize);
          deviceName = Marshal.PtrToStringAnsi(pData);
          Marshal.FreeHGlobal(pData);
        }

        Debug.WriteLine("Received Input Command ({0})", Enum.GetName(typeof (RawInput.RawInputType), raw.header.dwType));
        Debug.WriteLine("RAW HID DEVICE: {0}", deviceName);

        switch (raw.header.dwType)
        {
          case RawInput.RawInputType.HID:
            {
              int offset = Marshal.SizeOf(typeof (RawInput.RAWINPUTHEADER)) + Marshal.SizeOf(typeof (RawInput.RAWHID));

              byte[] bRawData = new byte[offset + raw.hid.dwSizeHid];
              Marshal.Copy(buffer, bRawData, 0, bRawData.Length);

              byte[] newArray = new byte[raw.hid.dwSizeHid];
              Array.Copy(bRawData, offset, newArray, 0, newArray.Length);

              string RawCode = BitConverter.ToString(newArray);
              Debug.WriteLine("RAW HID DATA: {0}", RawCode);
              // process the remote button press
              string code = string.Empty;

              UInt64 keyCode = 0;
              for (int i = 1; i < newArray.Length; i++)
              {
                keyCode += (ulong) newArray[i] << (8*(i - 1));
              }
              if (keyCode != 0)
              {
                string codeString = string.Format("{0:X2}{1:X6}", newArray[0], keyCode);

                _remoteHandler(Name, codeString);

                if (config.doRepeats)
                {
                  _RepeatTimer = new System.Threading.Timer(new System.Threading.TimerCallback(repeatTimerCallback),
                                                            (object) codeString, _firstRepeatDelay, _heldRepeatDelay);
                }
              }
              else
              {
                if (_RepeatTimer != null)
                {
                  _RepeatTimer.Dispose();
                  _RepeatTimer = null;
                }
              }

              break;
            }
        }
      }
      finally
      {
        Marshal.FreeHGlobal(buffer);
      }
    }

    #endregion HID Device Specific Helper Functions

    #region Timer Callbacks

    private void repeatTimerCallback(object state)
    {
      string codeString = (string) state;
      _remoteHandler(Name, codeString);
    }

    #endregion Timer Callbacks
  }
}