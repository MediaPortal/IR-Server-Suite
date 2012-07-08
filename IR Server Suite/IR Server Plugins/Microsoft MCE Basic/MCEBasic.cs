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
using System.Globalization;
using System.Linq;
using System.IO;
//using System.ServiceProcess;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;
//using IRServer.Plugin.Hardware;
using IRServer.Plugin.Properties;
using IrssUtils;
using Microsoft.Win32;
using SlimDX.Multimedia;
using SlimDX.RawInput;

namespace IRServer.Plugin
{

  /// <summary>
  /// IR Server Plugin for Microsoft MCE Transceiver device.
  /// </summary>
  public partial class MCEBasic
  {

    private class Device
    {
      public string Name;
      public string DeviceID;
      public string VendorID;
      public string ProductID;
      public ushort Usage;
      public ushort UsagePage;
    }

    #region Constants

    private const string AutomaticButtonsRegKey =
      @"SYSTEM\CurrentControlSet\Services\HidIr\Remotes\745a17a0-74d3-11d0-b6fe-00a0c90f57da";

    private const int VistaVersionNumber = 6;

    internal static readonly string ConfigurationFile = Path.Combine(ConfigurationPath, "Microsoft MCE Basic.xml");

    internal static readonly string eHomeTransceiverList = Path.Combine(ConfigurationPath, "Microsoft MCE Basic_eHomeTransceiverList.xml");

    private static readonly Guid MicrosoftGuid = new Guid(0x7951772d, 0xcd50, 0x49b7, 0xb1, 0x03, 0x2b, 0xaa, 0xc4, 0x94,
                                                          0xfc, 0x57);

    private static readonly Guid ReplacementGuid = new Guid(0x00873fdf, 0x61a8, 0x11d1, 0xaa, 0x5e, 0x00, 0xc0, 0x4f,
                                                            0xb1, 0x72, 0x8b);

    #endregion Constants

    #region Variables

    private Config _config;

    //private Driver _driver;
    private bool _ignoreAutomaticButtons;

    private bool _keyboardKeyRepeated;

    private uint _lastKeyboardKeyCode;
    private DateTime _lastKeyboardKeyTime = DateTime.Now;
    private uint _lastKeyboardModifiers;
    //private IrProtocol _lastRemoteButtonCodeType = IrProtocol.None;
    private uint _lastRemoteButtonKeyCode;
    private DateTime _lastRemoteButtonTime = DateTime.Now;

    private Mouse.MouseEvents _mouseButtons = Mouse.MouseEvents.None;
    
    private List<RawInput.RAWINPUTDEVICE> _deviceList;
    private ReceiverWindow _receiverWindow;
    private bool _disposed;
    private System.Threading.Timer _repeatTimer;
    private int _firstRepeatDelay;
    private int _heldRepeatDelay;

    private bool _remoteButtonRepeated;
    private List<string> _hidDevices;
    private List<Device> _supportedDevices;

    #endregion Variables

    #region Implementation


    private void Start_Receiver()
    {
      Debug.Open("Microsoft MCE Basic.log");
      Debug.WriteLine("Start_Receiver()");
      _config = new Config();
      ConfigManagement.LoadSettings(ref _config);

      _firstRepeatDelay = _config.FirstRepeatDelay;
      _heldRepeatDelay = _config.HeldRepeatDelay;

      if (_config.UseSystemRatesDelay)
      {
        _firstRepeatDelay = 250 + (SystemInformation.KeyboardDelay * 250);
        _heldRepeatDelay = (int)(1000.0 / (2.5 + (SystemInformation.KeyboardSpeed * 0.888)));
      }

      // create receiver Window
      _receiverWindow = new ReceiverWindow("Microsoft MCE Basic");
      _receiverWindow.ProcMsg += ProcMessage;
      Debug.WriteLine((new Guid(0x7951772d, 0xcd50, 0x49b7, 0xb1, 0x03, 0x2b, 0xaa, 0xc4, 0x94,
                                                          0xfc, 0x57)).ToString());
      Debug.WriteLine("=================");
      Debug.WriteLine("slimdx getdevices all");
      Debug.WriteLine("=================");
      var devList = SlimDX.RawInput.Device.GetDevices().OrderBy(di => di.DeviceName);
      foreach (var deviceInfo in devList)
      {
        Debug.WriteLine("name: {0}, type: {1}", deviceInfo.DeviceName, deviceInfo.DeviceType);
      }

      Debug.WriteLine("=================");
      Debug.WriteLine("slimdx getdevices hid");
      Debug.WriteLine("=================");
      _hidDevices = new List<string>();
      foreach (var deviceInfo in devList.OrderBy(di => di.DeviceName))
      {
        SlimDX.RawInput.HidInfo hidInfo = deviceInfo as HidInfo;
        if (hidInfo == null) continue;

        Debug.WriteLine("name: {0}, type: {1}", hidInfo.DeviceName, hidInfo.DeviceType);
        Debug.WriteLine("VendorId: {0}, ProductId: {1}, VersionNumber: {2}", hidInfo.VendorId, hidInfo.ProductId, hidInfo.VersionNumber);
        _hidDevices.Add(hidInfo.DeviceName);
      }

      Debug.WriteLine("=================");
      Debug.WriteLine("rawinput enumerate");
      Debug.WriteLine("=================");
      foreach (var dev in RawInput.EnumerateDevices().OrderBy(rid => rid.ID))
      {
        Debug.WriteLine("ID: {0}, Name: {1}", dev.ID, dev.Name);
        Debug.WriteLine("Usage: {0}, UsagePage: {1}", dev.Usage, dev.UsagePage);
      }

      //LoadDeviceFromXml();
      LoadDevicesFromSystem();

      // collect devices
      _deviceList = new List<RawInput.RAWINPUTDEVICE>();

      foreach (Device device in _supportedDevices)
      {
          RawInput.RAWINPUTDEVICE rid = new RawInput.RAWINPUTDEVICE();
          rid.usUsage = device.Usage;
          rid.usUsagePage = device.UsagePage;
          rid.dwFlags = RawInput.RawInputDeviceFlags.InputSink;
          rid.hwndTarget = _receiverWindow.Handle;
          _deviceList.Add(rid);
      }

      if (!RegisterForRawInput(_deviceList.ToArray()))
      {
        Debug.WriteLine("ERROR: Failed to register for HID Raw input");
        throw new InvalidOperationException("Failed to register for HID Raw input");
      }

      Debug.WriteLine("Start_Receiver(): completed");
    }

    protected void LoadDevicesFromSystem()
    {
      _supportedDevices = new List<Device>();
      foreach (var details in RawInput.EnumerateDevices())
      {
        if (!_hidDevices.Contains(details.ID)) continue;

        _supportedDevices.Add(new Device()
                                {
                                  Usage = details.Usage,
                                  UsagePage = details.UsagePage
                                });
      }
    }

    protected void LoadDeviceFromXml()
    {

      //if (!File.Exists(MCEBasic.eHomeTransceiverList))
        File.WriteAllText(MCEBasic.eHomeTransceiverList, Resources.eHomeTransceiverList, Encoding.UTF8);


      _supportedDevices = new List<Device>();

      try
      {
        XmlDocument source = new XmlDocument();
        source.Load(MCEBasic.eHomeTransceiverList);
        XmlNodeList transceiverNodes = source.SelectNodes("/ehomelist/transceiver");

        foreach (XmlNode transceiverNode in transceiverNodes)
        {
          XmlAttribute name = transceiverNode.Attributes["name"];
          XmlAttribute deviceid = transceiverNode.Attributes["deviceid"];
          XmlAttribute vendorid = transceiverNode.Attributes["vendorid"];
          XmlAttribute productid = transceiverNode.Attributes["productid"];
          XmlAttribute usage = transceiverNode.Attributes["usage"];
          XmlAttribute usagePage = transceiverNode.Attributes["usagePage"];
          Device d = new Device()
                       {
                         Name = name.Value,
                         DeviceID = deviceid.Value,
                         VendorID = vendorid.Value,
                         ProductID = productid.Value,
                         Usage = ushort.Parse(usage.Value),
                         UsagePage = ushort.Parse(usage.Value)
                       };
          _supportedDevices.Add(d);
        }
      }
      catch (XmlException)
      {
        MCEBasic.Debug.WriteLine("MCE: Error in XML file " + MCEBasic.eHomeTransceiverList, "error");
        _supportedDevices = null;
        return;
      }
    }


    /// <summary>
    /// Initialize MCE Remote
    /// </summary>
    //private void Init2()
    //{
    //  Debug.Open("Microsoft MCE Basic.log");
    //  Debug.WriteLine("MCE: Initializing MCE remote");
    //  _config = new Config();
    //  ConfigManagement.LoadSettings(ref _config);

    //  try
    //  {
    //    Remote.LogVerbose = true;
    //    // Register Device
    //    Remote.Click = null;
    //    Remote.Click += new RemoteEventHandler(OnRemoteClick);
    //    Remote.DeviceRemoval += new DeviceEventHandler(OnDeviceRemoval);
    //  }
    //  catch (Exception ex)
    //  {
    //    //controlEnabled = false;
    //    Debug.WriteLine("MCE: {0} - support disabled until MP restart", ex.InnerException.Message);
    //    return;
    //  }

    //  //// Kill ehtray.exe since that program catches the MCE remote keys and would start MCE 2005
    //  //Process[] myProcesses;
    //  //myProcesses = Process.GetProcesses();
    //  //foreach (Process myProcess in myProcesses)
    //  //{
    //  //  if (myProcess.ProcessName.ToLower().Equals("ehtray"))
    //  //  {
    //  //    try
    //  //    {
    //  //      Debug.WriteLine("MCE: Stopping Microsoft ehtray");
    //  //      myProcess.Kill();
    //  //    }
    //  //    catch (Exception)
    //  //    {
    //  //      Debug.WriteLine("MCE: Cannot stop Microsoft ehtray");
    //  //      DeInit();
    //  //      return;
    //  //    }
    //  //  }
    //  //}

    //  //_inputHandler = new InputHandler("Microsoft MCE");
    //  //if (!_inputHandler.IsLoaded)
    //  //{
    //  //  Debug.WriteLine("MCE: Error loading default mapping file - please reinstall MediaPortal");
    //  //  DeInit();
    //  //  return;
    //  //}
    //  //else
    //  //{
    //  //  Debug.WriteLine("MCE: MCE remote enabled");
    //  //}
    //}

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
    /// Remove all device handling
    /// </summary>
    //public void DeInit2()
    //{
    //  //if (controlEnabled)
    //  //{
    //    //if (logVerbose)
    //    //{
    //  Debug.WriteLine("MCE: Stopping MCE remote");
    //    //}
    //    Remote.Click -= new RemoteEventHandler(OnRemoteClick);
    //    Remote.DeviceRemoval -= new DeviceEventHandler(OnDeviceRemoval);
    //    Remote.DeviceArrival -= new DeviceEventHandler(OnDeviceArrival);
    //    //_inputHandler = null;
    //    //controlEnabled = false;
    //  //}
    //}

    //private void OnDeviceRemoval(object sender, EventArgs e)
    //{
    //  Remote.DeviceRemoval -= new DeviceEventHandler(OnDeviceRemoval);
    //  Remote.DeviceArrival += new DeviceEventHandler(OnDeviceArrival);
    //  Debug.WriteLine("MCE: MCE receiver has been unplugged");
    //}

    //private void OnDeviceArrival(object sender, EventArgs e)
    //{
    //  Remote.DeviceArrival -= new DeviceEventHandler(OnDeviceArrival);
    //  Remote.Click -= new RemoteEventHandler(OnRemoteClick);
    //  Debug.WriteLine("MCE: MCE receiver detected");
    //  Init();
    //}

    /// <summary>
    /// Let everybody know that this HID message may not be handled by anyone else
    /// </summary>
    /// <param name="msg">System.Windows.Forms.Message</param>
    /// <returns>Command handled</returns>
    //public bool WndProc(Message msg)
    //{
    //  if (msg.Msg != 0x0319) return false;

    //  //int command = (msg.LParam.ToInt32() >> 16) & ~0xF000;
    //  //InputDevices.LastHidRequest = (AppCommands)command;

    //  //RemoteButton button = RemoteButton.None;

    //  //if ((AppCommands)command == AppCommands.VolumeUp)
    //  //{
    //  //  button = RemoteButton.VolumeUp;
    //  //}

    //  //if ((AppCommands)command == AppCommands.VolumeDown)
    //  //{
    //  //  button = RemoteButton.VolumeDown;
    //  //}

    //  //if (button != RemoteButton.None)
    //  //{
    //  //  // Get & execute Mapping
    //  //  if (_inputHandler.MapAction((int)button))
    //  //  {
    //  //    if (logVerbose)
    //  //    {
    //  //      Debug.WriteLine("MCE: Command \"{0}\" mapped", button);
    //  //    }
    //  //  }
    //  //  else if (logVerbose)
    //  //  {
    //  //    Debug.WriteLine("MCE: Command \"{0}\" not mapped", button);
    //  //  }
    //  //}

    //  return true;
    //}


    /// <summary>
    /// Evaluate button press from remote
    /// </summary>
    /// <param name="button">Remote Button</param>
    //private void OnRemoteClick(object sender, RemoteEventArgs e)
    //{
    //  Debug.WriteLine("OnRemoteClick :  {0}", e.Button);
    //  //if (RemoteCallback != null)
    //  //  RemoteCallback(Name, keyCode.ToString());
    //  //RemoteButton button = e.Button;
    //  //if (logVerbose)
    //  //{
    //  //  Log.Info("MCE: Incoming button command: {0}", button);
    //  //}

    //  //// Set LastHidRequest, otherwise the HID handler (if enabled) would react on some remote buttons (double execution of command)
    //  //switch (button)
    //  //{
    //  //  case RemoteButton.Record:
    //  //    InputDevices.LastHidRequest = AppCommands.MediaRecord;
    //  //    break;
    //  //  case RemoteButton.Stop:
    //  //    InputDevices.LastHidRequest = AppCommands.MediaStop;
    //  //    break;
    //  //  case RemoteButton.Pause:
    //  //    InputDevices.LastHidRequest = AppCommands.MediaPause;
    //  //    break;
    //  //  case RemoteButton.Rewind:
    //  //    InputDevices.LastHidRequest = AppCommands.MediaRewind;
    //  //    break;
    //  //  case RemoteButton.Play:
    //  //    InputDevices.LastHidRequest = AppCommands.MediaPlay;
    //  //    break;
    //  //  case RemoteButton.Forward:
    //  //    InputDevices.LastHidRequest = AppCommands.MediaFastForward;
    //  //    break;
    //  //  case RemoteButton.Replay:
    //  //    InputDevices.LastHidRequest = AppCommands.MediaPreviousTrack;
    //  //    break;
    //  //  case RemoteButton.Skip:
    //  //    InputDevices.LastHidRequest = AppCommands.MediaNextTrack;
    //  //    break;
    //  //  case RemoteButton.Back:
    //  //    InputDevices.LastHidRequest = AppCommands.BrowserBackward;
    //  //    break;
    //  //  case RemoteButton.ChannelUp:
    //  //    InputDevices.LastHidRequest = AppCommands.MediaChannelUp;
    //  //    break;
    //  //  case RemoteButton.ChannelDown:
    //  //    InputDevices.LastHidRequest = AppCommands.MediaChannelDown;
    //  //    break;
    //  //  case RemoteButton.Mute:
    //  //    InputDevices.LastHidRequest = AppCommands.VolumeMute;
    //  //    break;
    //  //  case RemoteButton.VolumeUp:
    //  //    return; // Don't handle this command, benefit from OS' repeat handling instead
    //  //  case RemoteButton.VolumeDown:
    //  //    return; // Don't handle this command, benefit from OS' repeat handling instead
    //  //}

    //  //// Get & execute Mapping
    //  //if (_inputHandler.MapAction((int)button))
    //  //{
    //  //  if (logVerbose)
    //  //  {
    //  //    Log.Info("MCE: Command \"{0}\" mapped", button);
    //  //  }
    //  //}
    //  //else if (logVerbose)
    //  //{
    //  //  Log.Info("MCE: Command \"{0}\" not mapped", button);
    //  //}
    //}








    internal static bool CheckAutomaticButtons()
    {
      using (RegistryKey key = Registry.LocalMachine.OpenSubKey(AutomaticButtonsRegKey, false))
      {
        return (key.GetValue("CodeSetNum0", null) != null);
      }
    }

    internal static void EnableAutomaticButtons()
    {
      using (RegistryKey key = Registry.LocalMachine.OpenSubKey(AutomaticButtonsRegKey, true))
      {
        key.SetValue("CodeSetNum0", 1, RegistryValueKind.DWord);
        key.SetValue("CodeSetNum1", 2, RegistryValueKind.DWord);
        key.SetValue("CodeSetNum2", 3, RegistryValueKind.DWord);
        key.SetValue("CodeSetNum3", 4, RegistryValueKind.DWord);
      }
    }

    internal static void DisableAutomaticButtons()
    {
      using (RegistryKey key = Registry.LocalMachine.OpenSubKey(AutomaticButtonsRegKey, true))
      {
        key.DeleteValue("CodeSetNum0", false);
        key.DeleteValue("CodeSetNum1", false);
        key.DeleteValue("CodeSetNum2", false);
        key.DeleteValue("CodeSetNum3", false);
      }
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
        !RawInput.RegisterRawInputDevices(devices, (uint)devices.Length,
                                          (uint)Marshal.SizeOf(typeof(RawInput.RAWINPUTDEVICE))))
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
                               (uint)Marshal.SizeOf(typeof(RawInput.RAWINPUTHEADER)));

      IntPtr buffer = Marshal.AllocHGlobal((int)dwSize);
      try
      {
        if (buffer == IntPtr.Zero)
          return;

        if (
          RawInput.GetRawInputData(message.LParam, RawInput.RawInputCommand.Input, buffer, ref dwSize,
                                   (uint)Marshal.SizeOf(typeof(RawInput.RAWINPUTHEADER))) != dwSize)
          return;

        RawInput.RAWINPUT raw = (RawInput.RAWINPUT)Marshal.PtrToStructure(buffer, typeof(RawInput.RAWINPUT));

        // get the name of the device that generated the input message
        string deviceName = string.Empty;
        uint pcbSize = 0;
        RawInput.GetRawInputDeviceInfo(raw.header.hDevice, RawInput.RIDI_DEVICENAME, IntPtr.Zero, ref pcbSize);
        if (pcbSize > 0)
        {
          IntPtr pData = Marshal.AllocHGlobal((int)pcbSize);
          RawInput.GetRawInputDeviceInfo(raw.header.hDevice, RawInput.RIDI_DEVICENAME, pData, ref pcbSize);
          deviceName = Marshal.PtrToStringAnsi(pData);
          Marshal.FreeHGlobal(pData);
        }

        Debug.WriteLine("Received Input Command ({0})", Enum.GetName(typeof(RawInput.RawInputType), raw.header.dwType));
        Debug.WriteLine("RAW HID DEVICE: {0}", deviceName);

        switch (raw.header.dwType)
        {
          case RawInput.RawInputType.HID:
            {
              int offset = Marshal.SizeOf(typeof(RawInput.RAWINPUTHEADER)) + Marshal.SizeOf(typeof(RawInput.RAWHID));

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
                keyCode += (ulong)newArray[i] << (8 * (i - 1));
              }
              Debug.WriteLine("RAW KEY CODE: {0}", keyCode);
              if (keyCode != 0)
              {
                string codeString = string.Format("{0:X2}{1:X6}", newArray[0], keyCode);

                RemoteCallback(Name, codeString);

                if (_config.DoRepeats)
                {
                  _repeatTimer = new System.Threading.Timer(new System.Threading.TimerCallback(repeatTimerCallback),
                                                            (object)codeString, _firstRepeatDelay, _heldRepeatDelay);
                }
              }
              else
              {
                if (_repeatTimer != null)
                {
                  _repeatTimer.Dispose();
                  _repeatTimer = null;
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
      string codeString = (string)state;
      RemoteCallback(Name, codeString);
    }

    #endregion Timer Callbacks
  }
}