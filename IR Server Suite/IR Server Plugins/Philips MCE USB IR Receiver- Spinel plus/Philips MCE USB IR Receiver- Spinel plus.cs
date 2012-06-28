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

        #region Global Constants

        private static readonly string ConfigurationFile = Path.Combine(ConfigurationPath, "Philips MCE USB IR Receiver- Spinel plus.xml");

        #endregion

        #region Global Enumerations

        #endregion

        #region Device Support variables

        private class Device
        {
            public string Name;
            public List<string> hwIDs;
        }

        private static readonly List<Device> supportedDevices = new List<Device>()
            {
                new Device()
                {Name="Philips MCE USB IR Receiver- Spinel plus", hwIDs = new List<string>()
                {
                    "Vid_0471&Pid_20cc&Col02",
                    "Vid_0471&Pid_20cc&Col03",
                    "Vid_0471&Pid_20cc&Col04"
                }
                }
            };

        private List<RawInput.RAWINPUTDEVICE> _deviceTree;

        private ReceiverWindow _receiverWindowHID;

        #endregion Device Support variables

        #region Global Variables

        #region Configuration

        private bool _enableRemoteInput = true;
        private bool _usePadSwitch = true;
        private bool _useSystemRatesRemote;
        private int _remoteFirstRepeat = 400;
        private int _remoteHeldRepeats = 250;
        private bool _enableKeyboardInput;
        private bool _handleKeyboardLocally = true;
        private int _keyPadSensitivity = 7;
        private bool _useSystemRatesKeyboard = true;
        private int _keyboardFirstRepeat = 350;
        private int _keyboardHeldRepeats;
        private bool _enableMouseInput;
        private bool _handleMouseLocally = true;
        private double _mouseSensitivity = 1.0d;

        #endregion Configuration

        private bool _disposed;
        private DateTime _lastCodeTime = DateTime.Now;
        private DateTime _lastKeyboardKeyTime = DateTime.Now;
        private string _lastKeyCode = String.Empty;

        private uint _lastRemoteButtonKeyCode;
        private DateTime _lastRemoteButtonTime = DateTime.Now;
        private bool _remoteButtonRepeated;
        private RemoteHandler _remoteHandler;

        #endregion Global Variables


        #region DosDevice Specific Helper Functions

        private void ProcessInput(byte[] dataBytes)
        {
            DebugWrite("Data Received: ");
            DebugDump(dataBytes);
        }

        private void ReceiveThread()
        {
            DebugWriteLine("ReceiveThread()\n\n");
        }

        #endregion DosDevice Specific Helper Functions

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
        /// Start the IR Server plugin for HID device.
        /// </summary>
        private void Start_Receiver()
        {
            DebugOpen("Philips MCE USB IR Receiver- Spinel plus.log");
            DebugWriteLine("Start_Receiver()");
            LoadSettings();

            // create receiver Window
            _receiverWindowHID = new ReceiverWindow("Philips MCE USB IR Receiver- Spinel plus Receiver");
            _receiverWindowHID.ProcMsg += ProcMessage;

            // collect devices
            FindDevices();
                        
            if (_deviceTree != null)
            {
                if (_deviceTree.Count == 0)
                {
                    DebugWriteLine("ERROR: no devices found");
                    throw new InvalidOperationException("no devices found");
                }

                if (!RegisterForRawInput(_deviceTree.ToArray()))
                {
                    DebugWriteLine("ERROR: Failed to register for HID Raw input");
                    throw new InvalidOperationException("Failed to register for HID Raw input");
                }
            }
            DebugWriteLine("Start_Receiver(): completed");
        }

        /// <summary>
        /// Stop the IR Server plugin for HID devices.
        /// </summary>
        private void Stop_Receiver()
        {
            
            DebugWriteLine("Stop_Receiver()");
            RawInput.RAWINPUTDEVICE[] _deviceTreeArray = _deviceTree.ToArray();
            if (_deviceTreeArray != null)
            {
                for (int i = 0; i < _deviceTreeArray.Length; i++)
                {
                    _deviceTreeArray[i].dwFlags |= RawInput.RawInputDeviceFlags.Remove;
                }
                RegisterForRawInput(_deviceTreeArray);

                _receiverWindowHID.ProcMsg -= ProcMessage;
                _receiverWindowHID.DestroyHandle();
                _receiverWindowHID = null;
            }
        }

        /// <summary>
        /// Suspend the IR Server plugin for HID devices when computer enters standby.
        /// </summary>
        private void Suspend_Receiver()
        {
            DebugWriteLine("Suspend_Receiver()");
            Stop_Receiver();
        }

        /// <summary>
        /// Resume the IR Server plugin when the computer returns from standby.
        /// </summary>
        private void Resume_Receiver()
        {
            DebugWriteLine("Resume_Receiver()");
            Start_Receiver();
        }

        /// <summary>
        /// Detect the presence of Spinel Plus devices.  Devices that cannot be detected will always return false.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the device is present, otherwise <c>false</c>.
        /// </returns>
        private bool Detect_Device()
        {
            // create receiver Window
            _receiverWindowHID = new ReceiverWindow("Philips MCE USB IR Receiver- Spinel plus Receiver");
            _receiverWindowHID.ProcMsg += ProcMessage;

            FindDevices();

            _receiverWindowHID.ProcMsg -= ProcMessage;
            _receiverWindowHID.DestroyHandle();
            _receiverWindowHID = null;

            bool found = (_deviceTree.Count==0)?false:true;
            _deviceTree= null;

            return found;
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

        /// <summary>
        /// Loads the settings.
        /// </summary>
        private void LoadSettings()
        {
            DebugWriteLine("LoadSettings()");
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(ConfigurationFile);

                _enableRemoteInput = bool.Parse(doc.DocumentElement.Attributes["EnableRemoteInput"].Value);
                _usePadSwitch = bool.Parse(doc.DocumentElement.Attributes["UsePadSwitch"].Value);
                _useSystemRatesRemote = bool.Parse(doc.DocumentElement.Attributes["UseSystemRatesRemote"].Value);
                _remoteFirstRepeat = int.Parse(doc.DocumentElement.Attributes["RemoteFirstRepeat"].Value);
                _remoteHeldRepeats = int.Parse(doc.DocumentElement.Attributes["RemoteHeldRepeats"].Value);

                _enableKeyboardInput = bool.Parse(doc.DocumentElement.Attributes["EnableKeyboardInput"].Value);
                _useSystemRatesKeyboard = bool.Parse(doc.DocumentElement.Attributes["UseSystemRatesKeyboard"].Value);
                _keyboardFirstRepeat = int.Parse(doc.DocumentElement.Attributes["KeyboardFirstRepeat"].Value);
                _keyboardHeldRepeats = int.Parse(doc.DocumentElement.Attributes["KeyboardHeldRepeats"].Value);
                _handleKeyboardLocally = bool.Parse(doc.DocumentElement.Attributes["HandleKeyboardLocally"].Value);

                _enableMouseInput = bool.Parse(doc.DocumentElement.Attributes["EnableMouseInput"].Value);
                _handleMouseLocally = bool.Parse(doc.DocumentElement.Attributes["HandleMouseLocally"].Value);
                _mouseSensitivity = double.Parse(doc.DocumentElement.Attributes["MouseSensitivity"].Value);
                _keyPadSensitivity = int.Parse(doc.DocumentElement.Attributes["KeyPadSensitivity"].Value);
            }
            catch (Exception ex)
            {
                DebugWriteLine(ex.ToString());
                
                _enableRemoteInput = true;
                _useSystemRatesRemote = false;
                _remoteFirstRepeat = 400;
                _remoteHeldRepeats = 250;

                _enableKeyboardInput = true;
                _useSystemRatesKeyboard = true;
                _keyboardFirstRepeat = 350;
                _keyboardHeldRepeats = 0;
                _handleKeyboardLocally = true;

                _enableMouseInput = true;
                _handleMouseLocally = true;
                _mouseSensitivity = 1.0d;
            }
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        private void SaveSettings()
        {
            DebugWriteLine("SaveSettings()");
            try
            {
                XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, Encoding.UTF8);
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 1;
                writer.IndentChar = (char)9;
                writer.WriteStartDocument(true);
                writer.WriteStartElement("settings"); // <settings>

                writer.WriteAttributeString("EnableRemoteInput", _enableRemoteInput.ToString());
                writer.WriteAttributeString("UsePadSwitch", _usePadSwitch.ToString());
                writer.WriteAttributeString("UseSystemRatesRemote", _useSystemRatesRemote.ToString());
                writer.WriteAttributeString("RemoteFirstRepeat", _remoteFirstRepeat.ToString());
                writer.WriteAttributeString("RemoteHeldRepeats", _remoteHeldRepeats.ToString());

                writer.WriteAttributeString("EnableKeyboardInput", _enableKeyboardInput.ToString());
                writer.WriteAttributeString("UseSystemRatesKeyboard", _useSystemRatesKeyboard.ToString());
                writer.WriteAttributeString("KeyboardFirstRepeat", _keyboardFirstRepeat.ToString());
                writer.WriteAttributeString("KeyboardHeldRepeats", _keyboardHeldRepeats.ToString());
                writer.WriteAttributeString("HandleKeyboardLocally", _handleKeyboardLocally.ToString());

                writer.WriteAttributeString("EnableMouseInput", _enableMouseInput.ToString());
                writer.WriteAttributeString("HandleMouseLocally", _handleMouseLocally.ToString());
                writer.WriteAttributeString("MouseSensitivity", _mouseSensitivity.ToString());
                writer.WriteAttributeString("KeyPadSensitivity", _keyPadSensitivity.ToString());


                writer.WriteEndElement(); // </settings>
                writer.WriteEndDocument();
                writer.Close();
            }
            catch (Exception ex)
            {
                DebugWriteLine(ex.ToString());
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
            DebugWriteLine("RegisterForRawInput(): Registering {0} device(s).", devices.Length);
            if (
              !RawInput.RegisterRawInputDevices(devices, (uint)devices.Length,
                                                (uint)Marshal.SizeOf(typeof(RawInput.RAWINPUTDEVICE))))
            {
                int dwError = Marshal.GetLastWin32Error();
                DebugWriteLine("RegisterForRawInput(): error={0}", dwError);
                throw new Win32Exception(dwError, "PhilipsMceUsbIrReceiverSpinelPlus:RegisterForRawInput()");
            }
            DebugWriteLine("RegisterForRawInput(): Done.");
            return true;
        }

        /// <summary>
        /// finds the devices.
        /// </summary>
        private void FindDevices()
        {
            if (_deviceTree != null) return;
            _deviceTree = new List<RawInput.RAWINPUTDEVICE>();
            // configure the device tree
            DebugWriteLine("FindDevices(): starting...");

            // get the complete list of raw input devices and parse it for supported devices
            List<DeviceDetails> _rawInputDevices = new List<DeviceDetails>();

            try
            {
                _rawInputDevices = RawInput.EnumerateDevices();
                DebugWriteLine("FindDevices(): Enumerating Devices succeeded");
            }
            catch
            {
                DebugWriteLine("FindDevices(): Enumerating Devices failed");
                return;
            }
            RawInput.RAWINPUTDEVICE _device;

            if (_rawInputDevices.Count > 0)
            {
                foreach (Device supportedDevice in supportedDevices)
                {
                    DebugWriteLine("FindDevices(): searching for \"{0}\"", supportedDevice.Name);
                    foreach (string supportedDeviceHwID in supportedDevice.hwIDs)
                    {
                        DebugWriteLine("FindDevices(): probing \"{0}\"", supportedDeviceHwID);
                        foreach (DeviceDetails _rawInputDeviceDetails in _rawInputDevices)
                        {
                            if (_rawInputDeviceDetails.ID.ToLower().Contains(supportedDeviceHwID.ToLower()))
                            {
                                DebugWriteLine("FindDevices(): Found device \"{0}\"", _rawInputDeviceDetails.ID.Split('#')[1]);
                                DebugWriteLine("Remote Usage: {0}", _rawInputDeviceDetails.Usage);
                                DebugWriteLine("Remote UsagePage: {0}", _rawInputDeviceDetails.UsagePage);
                                DebugWriteLine("");

                                _device = new RawInput.RAWINPUTDEVICE();
                                _device.usUsage = _rawInputDeviceDetails.Usage;
                                _device.usUsagePage = _rawInputDeviceDetails.UsagePage;
                                _device.dwFlags = RawInput.RawInputDeviceFlags.InputSink;
                                _device.hwndTarget = _receiverWindowHID.Handle;
                                _deviceTree.Add(_device);
                            }
                        }
                    }
                }
            }
            else DebugWriteLine("FindDevices(): No input devices found");
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

                DebugWriteLine("Received Input Command ({0})", Enum.GetName(typeof(RawInput.RawInputType), raw.header.dwType));
                DebugWriteLine("RAW HID DEVICE: {0}", deviceName);

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
                            DebugWriteLine("RAW HID DATA: {0}", RawCode);
                            // process the remote button press
                            string code = string.Empty;

                            UInt64 keyCode = 0;
                            for (int i = 1; i < newArray.Length; i++)
                            {
                                keyCode += (ulong)newArray[i] << (8 * (i - 1));
                            }
                            if (keyCode != 0)
                                _remoteHandler(Name, string.Format("{0:X2}{1:X6}", newArray[0], keyCode));

                            switch (keyCode)
                            {
                                case 0x00:
                                    {
                                        break;
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

        #region IRemoteReceiver Functional Implimentation

        private void RemoteEvent(uint keyCode, bool firstPress)
        {
            DebugWriteLine("RemoteEvent: {0}, {1}", keyCode, firstPress);
            if (!_enableRemoteInput)
                return;

            if (!firstPress && _lastRemoteButtonKeyCode == keyCode)
            {
                TimeSpan timeBetween = DateTime.Now.Subtract(_lastRemoteButtonTime);

                int firstRepeat = _remoteFirstRepeat;
                int heldRepeats = _remoteHeldRepeats;
                if (_useSystemRatesRemote)
                {
                    firstRepeat = 250 + (SystemInformation.KeyboardDelay * 250);
                    heldRepeats = (int)(1000.0 / (2.5 + (SystemInformation.KeyboardSpeed * 0.888)));
                }

                if (!_remoteButtonRepeated && timeBetween.TotalMilliseconds < firstRepeat)
                {
                    DebugWriteLine("Skip, First Repeat\n");
                    return;
                }

                if (_remoteButtonRepeated && timeBetween.TotalMilliseconds < heldRepeats)
                {
                    DebugWriteLine("Skip, Held Repeat\n");
                    return;
                }

                if (_remoteButtonRepeated && timeBetween.TotalMilliseconds > firstRepeat)
                    _remoteButtonRepeated = false;
                else
                    _remoteButtonRepeated = true;
            }
            else
            {
                _lastRemoteButtonKeyCode = keyCode;
                _remoteButtonRepeated = false;
            }

            _lastRemoteButtonTime = DateTime.Now;

            if (_remoteHandler != null)
                _remoteHandler(Name, keyCode.ToString());
        }

        #endregion

        #region IMouseReceiver Functional Implimentation

        #endregion

        #region KeyCode Helper Functions

        // TODO: Convert this function to a lookup from an XML file, then provide multiple files and a way to fine-tune...
        private static Keyboard.VKey ConvertMceKeyCodeToVKey(uint keyCode)
        {
            switch (keyCode)
            {
                case 0x04:
                    return Keyboard.VKey.VK_A;
                case 0x05:
                    return Keyboard.VKey.VK_B;
                case 0x06:
                    return Keyboard.VKey.VK_C;
                case 0x07:
                    return Keyboard.VKey.VK_D;
                case 0x08:
                    return Keyboard.VKey.VK_E;
                case 0x09:
                    return Keyboard.VKey.VK_F;
                case 0x0A:
                    return Keyboard.VKey.VK_G;
                case 0x0B:
                    return Keyboard.VKey.VK_H;
                case 0x0C:
                    return Keyboard.VKey.VK_I;
                case 0x0D:
                    return Keyboard.VKey.VK_J;
                case 0x0E:
                    return Keyboard.VKey.VK_K;
                case 0x0F:
                    return Keyboard.VKey.VK_L;
                case 0x10:
                    return Keyboard.VKey.VK_M;
                case 0x11:
                    return Keyboard.VKey.VK_N;
                case 0x12:
                    return Keyboard.VKey.VK_O;
                case 0x13:
                    return Keyboard.VKey.VK_P;
                case 0x14:
                    return Keyboard.VKey.VK_Q;
                case 0x15:
                    return Keyboard.VKey.VK_R;
                case 0x16:
                    return Keyboard.VKey.VK_S;
                case 0x17:
                    return Keyboard.VKey.VK_T;
                case 0x18:
                    return Keyboard.VKey.VK_U;
                case 0x19:
                    return Keyboard.VKey.VK_V;
                case 0x1A:
                    return Keyboard.VKey.VK_W;
                case 0x1B:
                    return Keyboard.VKey.VK_X;
                case 0x1C:
                    return Keyboard.VKey.VK_Y;
                case 0x1D:
                    return Keyboard.VKey.VK_Z;
                case 0x1E:
                    return Keyboard.VKey.VK_1;
                case 0x1F:
                    return Keyboard.VKey.VK_2;
                case 0x20:
                    return Keyboard.VKey.VK_3;
                case 0x21:
                    return Keyboard.VKey.VK_4;
                case 0x22:
                    return Keyboard.VKey.VK_5;
                case 0x23:
                    return Keyboard.VKey.VK_6;
                case 0x24:
                    return Keyboard.VKey.VK_7;
                case 0x25:
                    return Keyboard.VKey.VK_8;
                case 0x26:
                    return Keyboard.VKey.VK_9;
                case 0x27:
                    return Keyboard.VKey.VK_0;
                case 0x28:
                    return Keyboard.VKey.VK_RETURN;
                case 0x29:
                    return Keyboard.VKey.VK_ESCAPE;
                case 0x2A:
                    return Keyboard.VKey.VK_BACK;
                case 0x2B:
                    return Keyboard.VKey.VK_TAB;
                case 0x2C:
                    return Keyboard.VKey.VK_SPACE;
                case 0x2D:
                    return Keyboard.VKey.VK_OEM_MINUS;
                case 0x2E:
                    return Keyboard.VKey.VK_OEM_PLUS;
                case 0x2F:
                    return Keyboard.VKey.VK_OEM_4;
                case 0x30:
                    return Keyboard.VKey.VK_OEM_6;
                case 0x31:
                    return Keyboard.VKey.VK_OEM_5;
                //case 0x32:return Keyboard.VKEY.VK_Non-US #;
                case 0x33:
                    return Keyboard.VKey.VK_OEM_1;
                case 0x34:
                    return Keyboard.VKey.VK_OEM_7;
                case 0x35:
                    return Keyboard.VKey.VK_OEM_3;
                case 0x36:
                    return Keyboard.VKey.VK_OEM_COMMA;
                case 0x37:
                    return Keyboard.VKey.VK_OEM_PERIOD;
                case 0x38:
                    return Keyboard.VKey.VK_OEM_2;
                case 0x39:
                    return Keyboard.VKey.VK_CAPITAL;
                case 0x3A:
                    return Keyboard.VKey.VK_F1;
                case 0x3B:
                    return Keyboard.VKey.VK_F2;
                case 0x3C:
                    return Keyboard.VKey.VK_F3;
                case 0x3D:
                    return Keyboard.VKey.VK_F4;
                case 0x3E:
                    return Keyboard.VKey.VK_F5;
                case 0x3F:
                    return Keyboard.VKey.VK_F6;
                case 0x40:
                    return Keyboard.VKey.VK_F7;
                case 0x41:
                    return Keyboard.VKey.VK_F8;
                case 0x42:
                    return Keyboard.VKey.VK_F9;
                case 0x43:
                    return Keyboard.VKey.VK_F10;
                case 0x44:
                    return Keyboard.VKey.VK_F11;
                case 0x45:
                    return Keyboard.VKey.VK_F12;
                case 0x46:
                    return Keyboard.VKey.VK_PRINT;
                case 0x47:
                    return Keyboard.VKey.VK_SCROLL;
                case 0x48:
                    return Keyboard.VKey.VK_PAUSE;
                case 0x49:
                    return Keyboard.VKey.VK_INSERT;
                case 0x4A:
                    return Keyboard.VKey.VK_HOME;
                case 0x4B:
                    return Keyboard.VKey.VK_PRIOR;
                case 0x4C:
                    return Keyboard.VKey.VK_DELETE;
                case 0x4D:
                    return Keyboard.VKey.VK_END;
                case 0x4E:
                    return Keyboard.VKey.VK_NEXT;
                case 0x4F:
                    return Keyboard.VKey.VK_RIGHT;
                case 0x50:
                    return Keyboard.VKey.VK_LEFT;
                case 0x51:
                    return Keyboard.VKey.VK_DOWN;
                case 0x52:
                    return Keyboard.VKey.VK_UP;
                case 0x64:
                    return Keyboard.VKey.VK_OEM_102;
                case 0x65:
                    return Keyboard.VKey.VK_APPS;

                default:
                    throw new ArgumentException(String.Format("Unknown Key Value {0}", keyCode), "keyCode");
            }
        }

        // TODO: Convert this function to a lookup from an XML file, then provide multiple files and a way to fine-tune...

        #endregion KeyCode Helper Functions

    }
}