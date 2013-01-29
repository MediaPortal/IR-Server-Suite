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
    /// IR Server plugin to support iMon USB devices.
    /// </summary>
    public partial class iMonUSBReceivers
    {
        #region iMon DosDevice Constants

        private const int DosDeviceBufferSize = 8;
        private const string DosDevicePath = @"\\.\SGIMON";

        private const int ErrorIoPending = 997;
        private const uint IOCTL_IMON_FW_VER = 0x00222014; // function 0x805 - read FW version (4 bytes)
        private const uint IOCTL_IMON_RC_SET = 0x00222010; // function 0x804 - write RCset data (2 bytes) to device

        // IOCTL definitions 0x0022xxxx
        private const uint IOCTL_IMON_READ = 0x00222008; // function 0x802 - read data (64 bytes?) from device
        private const uint IOCTL_IMON_READ_RC = 0x00222030; // function 0x80C - read data (8 bytes) from device
        private const uint IOCTL_IMON_READ2 = 0x00222034; // function 0x80D - ??? read (8 bytes)
        private const uint IOCTL_IMON_WRITE = 0x00222018; // function 0x806 - write data (8 bytes) to device

        private static readonly byte[][] SetDosRemotePADold = new byte[][]
        {
            new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x00},
            new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x02},
            new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x04},
            new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x06},
            new byte[] {0x20, 0x20, 0x20, 0x20, 0x00, 0x00, 0x00, 0x08},
            new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x0A}     
        };
        private static readonly byte[][] SetDosRemoteMCEold = new byte[][]
        {
            new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x00},
            new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x02},
            new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x04},
            new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x06},
            new byte[] {0x20, 0x20, 0x20, 0x20, 0x01, 0x00, 0x00, 0x08},
            new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x0A}     
        };

        private static readonly byte[][] SetDosRemotePADnew = new byte[][]
        {
            new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02}
        };

        private static readonly byte[][] SetDosRemoteMCEnew = new byte[][]
        {
            new byte[] {0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02}
        };

        #endregion iMon DosDevice Constants

        #region Global Constants

        private const int DeviceBufferSize = 255;
        private const uint IMON_MCE_BUTTON = 2000;
        private const uint IMON_MCE_BUTTON_DOWN = 0x1F;
        private const uint IMON_MCE_BUTTON_LEFT = 0x20;
        private const uint IMON_MCE_BUTTON_RIGHT = 0x21;
        private const uint IMON_MCE_BUTTON_UP = 0x1E;
        private const uint IMON_PAD_BUTTON = 1000;
        private const uint IMON_PAD_BUTTON_DOWN = 0xF8;
        private const uint IMON_PAD_BUTTON_LCLICK = 0xE2;
        private const uint IMON_PAD_BUTTON_LEFT = 0xF6;
        private const uint IMON_PAD_BUTTON_MENUKEY = 0x3C;
        private const uint IMON_PAD_BUTTON_RCLICK = 0xE4;
        private const uint IMON_PAD_BUTTON_RIGHT = 0xF4;
        private const uint IMON_PAD_BUTTON_UP = 0xFA;
        private const uint IMON_PAD_BUTTON_WINKEY = 0xC2;

        private const uint IMON_PANEL_BUTTON = 3000;
        private const uint IMON_VOLUME_DOWN = 4002;
        private const uint IMON_VOLUME_UP = 4001;
        private const uint IMON_NAVIGATION_DOWN = 4012;
        private const uint IMON_NAVIGATION_UP = 4011;

        private static readonly string ConfigurationFile = Path.Combine(ConfigurationPath, "iMon USB Receivers.xml");

        #endregion Constants

        #region iMon DosDevice Enumerators

        #region Nested type: CreateFileAccessTypes

        [Flags]
        private enum CreateFileAccessTypes : uint
        {
            GenericRead = 0x80000000,
            GenericWrite = 0x40000000,
            GenericExecute = 0x20000000,
            GenericAll = 0x10000000,
        }

        #endregion

        #region Nested type: CreateFileAttributes

        [Flags]
        private enum CreateFileAttributes : uint
        {
            None = 0x00000000,
            Readonly = 0x00000001,
            Hidden = 0x00000002,
            System = 0x00000004,
            Directory = 0x00000010,
            Archive = 0x00000020,
            Device = 0x00000040,
            Normal = 0x00000080,
            Temporary = 0x00000100,
            SparseFile = 0x00000200,
            ReparsePoint = 0x00000400,
            Compressed = 0x00000800,
            Offline = 0x00001000,
            NotContentIndexed = 0x00002000,
            Encrypted = 0x00004000,
            Write_Through = 0x80000000,
            Overlapped = 0x40000000,
            NoBuffering = 0x20000000,
            RandomAccess = 0x10000000,
            SequentialScan = 0x08000000,
            DeleteOnClose = 0x04000000,
            BackupSemantics = 0x02000000,
            PosixSemantics = 0x01000000,
            OpenReparsePoint = 0x00200000,
            OpenNoRecall = 0x00100000,
            FirstPipeInstance = 0x00080000,
        }

        #endregion

        #region Nested type: CreateFileDisposition

        private enum CreateFileDisposition : uint
        {
            None = 0,
            New = 1,
            CreateAlways = 2,
            OpenExisting = 3,
            OpenAlways = 4,
            TruncateExisting = 5,
        }

        #endregion

        #region Nested type: CreateFileShares

        [Flags]
        private enum CreateFileShares : uint
        {
            None = 0x00,
            Read = 0x01,
            Write = 0x02,
            Delete = 0x04,
        }

        #endregion

        #endregion DosDevice Enumerations

        #region Global Enumerations

        private DeviceType _DriverMode = DeviceType.NotValid;
        private KeyModifierState ModifierState;

        #region Nested type: DeviceType

        private enum DeviceType
        {
            NotValid = -1,
            None = 0,
            DOS = 1,
            HID = 2,
        }

        #endregion

        #region Nested type: KeyModifiers

        [Flags]
        private enum KeyModifiers
        {
            None = 0x00,
            LeftControl = 0x01,
            LeftShift = 0x02,
            LeftAlt = 0x04,
            LeftWin = 0x08,
            RightControl = 0x10,
            RightShift = 0x20,
            RightAlt = 0x40,
            RightWin = 0x80,
        }

        #endregion

        #region Nested type: KeyModifierState

        private struct KeyModifierState
        {
            public bool AltOn;
            public bool CtrlOn;
            public bool LastKeydownWasAlt;
            public bool LastKeydownWasCtrl;
            public bool LastKeydownWasShift;
            public bool LastKeyupWasAlt;
            public bool LastKeyupWasCtrl;
            public bool LastKeyupWasShift;
            public bool ShiftOn;
        }

        #endregion

        #region Nested type: RemoteMode

        /// <summary>
        /// Hardware mode (either MCE or iMon).
        /// </summary>
        internal enum RemoteMode
        {
            /// <summary>
            /// Microsoft MCE Mode.
            /// </summary>
            MCE,
            /// <summary>
            /// Soundgraph iMON Mode.
            /// </summary>
            iMON,
        }

        #endregion

        #region Nested type: PadMode

        /// <summary>
        /// Remote mode (either MCE or iMon).
        /// </summary>
        internal enum PadMode
        {
            /// <summary>
            /// Mousestick emulates Mouse Mode.
            /// </summary>
            Mouse,
            /// <summary>
            /// Mousestick emulates direction keys. (Default Mode)
            /// </summary>
            Keyboard,
        }

        #endregion

        #endregion

        #region DosDevice Interop

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeviceIoControl(
          SafeFileHandle handle,
          uint ioControlCode,
          IntPtr inBuffer, int inBufferSize,
          IntPtr outBuffer, int outBufferSize,
          out int bytesReturned,
          IntPtr overlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetOverlappedResult(
          SafeFileHandle handle,
          IntPtr overlapped,
          out int bytesTransferred,
          [MarshalAs(UnmanagedType.Bool)] bool wait);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern SafeFileHandle CreateFile(
          [MarshalAs(UnmanagedType.LPTStr)] string fileName,
          [MarshalAs(UnmanagedType.U4)] CreateFileAccessTypes fileAccess,
          [MarshalAs(UnmanagedType.U4)] CreateFileShares fileShare,
          IntPtr securityAttributes,
          [MarshalAs(UnmanagedType.U4)] CreateFileDisposition creationDisposition,
          [MarshalAs(UnmanagedType.U4)] CreateFileAttributes flags,
          IntPtr templateFile);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CancelIo(
          SafeFileHandle handle);

        #endregion DosDevice Interop

        #region HID Device Support variables

        private const string HIDKeyboardSuffix = "MI_00&Col02#";
        private const string HIDMouseSuffix = "MI_00&Col01#";
        private const string HIDRemoteSuffix = "MI_01#";

        private static readonly byte[][] SetHidPadModeKeyboard = new byte[][]
        {
            new byte[] {0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80}
        };

        private static readonly byte[][] SetHidPadModeMouse = new byte[][]
        {
            new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80}
        };

        private static readonly byte[][] SetHidRemotePAD = new byte[][]
        {
            new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x86}
        };

        private static readonly byte[][] SetHidRemoteMCE = new byte[][]
        {
            new byte[] {0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x86}
        };

        private static readonly string[] SupportedDevices_HID = new string[]
        {
            "Vid_15c2&Pid_0044",
            "Vid_15c2&Pid_0043",
            "Vid_15c2&Pid_003c",
            "Vid_15c2&Pid_0038",
            "Vid_15c2&Pid_0036",
            "Vid_15c2&Pid_0035",
            "Vid_15c2&Pid_0039",
			"Vid_15c2&Pid_0041",
            "Vid_15c2&Pid_0045",

        };

        private RawInput.RAWINPUTDEVICE[] _deviceTree;

        private ReceiverWindow _receiverWindowHID;

        private int KeyboardDevice = -1;
        private string KeyboardDeviceName = string.Empty;
        private int MouseDevice = -1;
        private string MouseDeviceName = string.Empty;
        private int RemoteDevice = -1;
        private string RemoteDeviceName = string.Empty;

        #endregion HID Device Support variables

        #region Global Variables

        #region Configuration

        private bool _enableRemoteInput = true;
        private bool _usePadSwitch = true;
        private RemoteMode _remoteMode = RemoteMode.MCE;
        private PadMode _padMode = PadMode.Keyboard;
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
        private bool _killImonM;

        #endregion Configuration

        private bool _disposed;
        private KeyboardHandler _keyboardHandler;
        private bool _keyboardKeyRepeated;
        private DateTime _lastCodeTime = DateTime.Now;
        private uint _lastKeyboardKeyCode;
        private DateTime _lastKeyboardKeyTime = DateTime.Now;
        private uint _lastKeyboardModifiers;
        private string _lastKeyCode = String.Empty;

        private uint _lastRemoteButtonKeyCode;
        private DateTime _lastRemoteButtonTime = DateTime.Now;
        private Mouse.MouseEvents _mouseButtons = Mouse.MouseEvents.None;
        private MouseHandler _mouseHandler;
        private bool _remoteButtonRepeated;
        private RemoteHandler _remoteHandler;

        private byte _remoteToggle;
        private uint firmwareVersion = 0;

        #endregion Global Variables

        #region DosDevice Variables

        private SafeFileHandle _deviceHandle;
        private bool _processReceiveThread;
        private Thread _receiveThread;
        private IntPtr deviceBufferPtr;

        #endregion

        #region DosDevice Specific Helper Functions

        private void ProcessInput(byte[] dataBytes)
        {
            DebugWrite("Data Received: ");
            DebugDump(dataBytes);

            if ((dataBytes[0] & 0xFC) == 0x28)
            {
                DebugWriteLine("iMon PAD remote button");

                uint keyCode = IMON_PAD_BUTTON;
                keyCode += (uint)((dataBytes[0] & 0x03) << 6);
                keyCode += (uint)(dataBytes[1] & 0x30);
                keyCode += (uint)((dataBytes[1] & 0x06) << 1);
                keyCode += (uint)((dataBytes[2] & 0xC0) >> 6);

                if ((_usePadSwitch) & (_remoteMode == RemoteMode.iMON))
                {
                    if (keyCode == (IMON_PAD_BUTTON + 0x50))
                    {
                        // the mouse/keyboard button was released
                        _padMode = ((_padMode == PadMode.Mouse) ? PadMode.Keyboard : PadMode.Mouse);
                        //only change modes on the release of the button
                        DebugWriteLine("RAW IMON HID REMOTE - MOUSE/KEYBOARD MODE CHANGED - NEW MODE = {0}\n", Enum.GetName(typeof(PadMode), _padMode));
                        return;
                    }
                }
                if ((keyCode & 0x01) == 0 && (dataBytes[2] & 0x40) == 0)
                {
                    RemoteEvent(keyCode, _remoteToggle != 1);
                    _remoteToggle = 1;
                }
                else
                {
                    _remoteToggle = 0;
                }
            }
            else if ((dataBytes[0] & 0xFC) == 0x68)
            {
                DebugWriteLine("ProcessInput(): iMon PAD mouse report");
                ProcessiMonMouseReport(dataBytes[0], dataBytes[1], dataBytes[2], dataBytes[3]);
            }
            else if (dataBytes[7] == 0xAE)
            {
                DebugWriteLine("MCE remote button");
                uint keyCode = IMON_MCE_BUTTON + dataBytes[3];

                RemoteEvent(keyCode, _remoteToggle != dataBytes[2]);
                _remoteToggle = dataBytes[2];
            }
            else if (dataBytes[7] == 0xBE)
            {
                DebugWriteLine("MCE Keyboard key press");
                KeyboardEvent(dataBytes[2], dataBytes[3]);
            }
            else if (dataBytes[7] == 0xCE)
            {
                DebugWriteLine("MCE Keyboard mouse move/button");
                int xSign = (dataBytes[0] & 0x01) != 0 ? 1 : -1;
                int ySign = (dataBytes[0] & 0x02) == 0 ? 1 : -1;

                int xSize = ((dataBytes[2] & 0xF0) >> 4);
                int ySize = (dataBytes[2] & 0x0F);

                bool right = ((dataBytes[3] & 0x40) != 0);
                bool left = ((dataBytes[3] & 0x20) != 0);

                MouseEvent(xSign * xSize, ySign * ySize, right, left);
            }
            else if (dataBytes[7] == 0xEE)
            {
                DebugWriteLine("Front panel buttons/volume knob");
                if (dataBytes[3] > 0x01)
                {
                    uint keyCode = IMON_PANEL_BUTTON + dataBytes[3];
                    RemoteEvent(keyCode, _remoteToggle != dataBytes[3]);
                }
                _remoteToggle = dataBytes[3];

                if (dataBytes[0] == 0x01 && _remoteHandler != null)
                    _remoteHandler(Name, IMON_VOLUME_DOWN.ToString());

                if (dataBytes[1] == 0x01 && _remoteHandler != null)
                    _remoteHandler(Name, IMON_VOLUME_UP.ToString());
            }
        }

        private void ReceiveThread()
        {
            DebugWriteLine("ReceiveThread()\n\n");
            byte[] dataBytes;
            int col = 0;

            while (_processReceiveThread)
            {
                dataBytes = GetData();
                if (dataBytes.Length == DosDeviceBufferSize)
                {
                    try
                    {
                        if (dataBytes[7] != 0xF0)
                        {
                            string temp = "";
                            foreach (byte x in dataBytes)
                                temp += String.Format("{0:X2}", x);
                            if (col == 3)
                            {
                                col = 0;
                                Debug.Write(temp + "\n");
                            }
                            else
                            {
                                col++;
                                Debug.Write(temp + "  ");
                            }
                        }

                        // Rubbish data:
                        // FF, FF, FF, FF, FF, FF, 9F, FF, 
                        // 00, 00, 00, 00, 00, 00, 00, F0, 
                        if ((dataBytes[0] != 0xFF || dataBytes[1] != 0xFF || dataBytes[2] != 0xFF || dataBytes[3] != 0xFF ||
                             dataBytes[4] != 0xFF || dataBytes[5] != 0xFF) &&
                            (dataBytes[0] != 0x00 || dataBytes[1] != 0x00 || dataBytes[2] != 0x00 || dataBytes[3] != 0x00 ||
                             dataBytes[4] != 0x00 || dataBytes[5] != 0x00))
                        {
                            ProcessInput(dataBytes);
                        }
                        Thread.Sleep(5);
                    }
                    catch
                    {
                        DebugWriteLine("Error processing input command!");
                    }
                }
            }

            if (deviceBufferPtr != IntPtr.Zero)
                Marshal.FreeHGlobal(deviceBufferPtr);
        }

        private void FindFirmware()
        {
            do
            {
                byte[] dataBytes = GetData();
                if (dataBytes.Length == DosDeviceBufferSize)
                {
                    if ((dataBytes[0] == 0xFF) &&
                                    (dataBytes[1] == 0xFF) &&
                                    (dataBytes[2] == 0xFF) &&
                                    (dataBytes[3] == 0xFF) &&
                                    (dataBytes[4] == 0xFF) &&
                                    (dataBytes[5] == 0xFF))
                        firmwareVersion = (uint)dataBytes[6] + 2;
                }
            } while (firmwareVersion == 0);
        }

        private byte[] GetData()
        {
            try
            {
                int bytesRead;
                IoControl(IOCTL_IMON_READ_RC, IntPtr.Zero, 0, deviceBufferPtr, DosDeviceBufferSize, out bytesRead);
                if (bytesRead == DosDeviceBufferSize)
                {
                    byte[] dataBytes = new byte[bytesRead];
                    Marshal.Copy(deviceBufferPtr, dataBytes, 0, bytesRead);
                    return dataBytes;
                }
                else
                {
                    return new byte[0];
                }
            }
            catch (Exception ex)
            {
                DebugWriteLine(ex.ToString());
                if (_deviceHandle != null)
                    CancelIo(_deviceHandle);
                return new byte[0];
            }
        }

        private void IoControl(uint ioControlCode, IntPtr inBuffer, int inBufferSize, IntPtr outBuffer, int outBufferSize, out int bytesReturned)
        {
            try
            {
                DeviceIoControl(_deviceHandle, ioControlCode, inBuffer, inBufferSize, outBuffer, outBufferSize, out bytesReturned, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                DebugWriteLine(ex.ToString());
                if (_deviceHandle != null)
                    CancelIo(_deviceHandle);
                throw;
            }
        }

        private void SetDos(RemoteMode mode)
        {
            DebugWriteLine("SetRemoteMode({0})", Enum.GetName(typeof(RemoteMode), mode));
            byte[][] modeData = new byte[0][];

            switch (mode)
            {
                case RemoteMode.iMON:
                    modeData = (firmwareVersion < 0x98) ? SetDosRemotePADold : SetDosRemotePADnew;
                    break;

                case RemoteMode.MCE:
                    modeData = (firmwareVersion < 0x98) ? SetDosRemoteMCEold : SetDosRemoteMCEnew;
                    break;
            }
            SetDos(modeData);
        }

        private void SetDos(byte[][] modeData)
        {
            int bytesRead;
            IntPtr deviceBufferPtr = IntPtr.Zero;

            foreach (byte[] send in modeData)
            {
                try
                {
                    DebugWriteLine("SetMode (DOS): sending command ({0} bytes) to device", send.Length);
                    deviceBufferPtr = Marshal.AllocHGlobal(send.Length);
                    Marshal.Copy(send, 0, deviceBufferPtr, send.Length);
                    IoControl(IOCTL_IMON_WRITE, deviceBufferPtr, send.Length, IntPtr.Zero, 0, out bytesRead);
                    DebugWriteLine("SetMode (DOS): sent {0} bytes to device", bytesRead);
                    Marshal.FreeHGlobal(deviceBufferPtr);
                    deviceBufferPtr = IntPtr.Zero;
                }
                catch (Exception ex)
                {
                    DebugWriteLine(ex.ToString());
                    if (_deviceHandle != null)
                        CancelIo(_deviceHandle);

                    if (deviceBufferPtr != IntPtr.Zero)
                        Marshal.FreeHGlobal(deviceBufferPtr);
                }
            }
        }

        #endregion DosDevice Specific Helper Functions

        #region Destructor

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="iMonUSBReceivers"/> is reclaimed by garbage collection.
        /// </summary>
        ~iMonUSBReceivers()
        {
            // Call Dispose with false.  Since we're in the destructor call, the managed resources will be disposed of anyway.
            Dispose(false);
        }

        #endregion Destructor

        /// <summary>
        /// Determine the type of iMon device currently being used.
        /// </summary>
        /// <value>The DeviceType.</value>
        private DeviceType DeviceDriverMode
        {
            get
            {
                DebugWriteLine("DeviceDriverMode()");
                if (_DriverMode == DeviceType.NotValid)
                {
                    Detect();
                }
                return _DriverMode;
            }
        }

        #region PluginBase Functional Implimentation for iMon HID devices

        /// <summary>
        /// Start the IR Server plugin for HID device.
        /// </summary>
        private void Start_HID()
        {
            DebugOpen("iMon Receiver.log");
            DebugWriteLine("Start_HID()");
            InitializeKeyboardState();
            LoadSettings();

            // locate the iMon devices
            FindDevices_HID();
            if (_enableRemoteInput & (RemoteDevice > -1))
            {
                DebugWriteLine("Remote Usage: {0}", _deviceTree[RemoteDevice].usUsage);
                DebugWriteLine("Remote UsagePage: {0}", _deviceTree[RemoteDevice].usUsagePage);
                DebugWriteLine("Remote Repeat Delay: {0}", _remoteFirstRepeat);
                DebugWriteLine("Remote Held Delay: {0}", _remoteHeldRepeats);
                DebugWriteLine("");
            }
            if (_enableKeyboardInput & (KeyboardDevice > -1))
            {
                DebugWriteLine("Keyboard Usage: {0}", _deviceTree[KeyboardDevice].usUsage);
                DebugWriteLine("Keyboard UsagePage: {0}", _deviceTree[KeyboardDevice].usUsagePage);
                DebugWriteLine("Keyboard Repeat Delay: {0}", _keyboardFirstRepeat);
                DebugWriteLine("Keyboard Held Delay: {0}", _keyboardHeldRepeats);
                DebugWriteLine("");
            }
            if (_enableMouseInput & (MouseDevice > -1))
            {
                DebugWriteLine("Mouse Usage: {0}", _deviceTree[MouseDevice].usUsage);
                DebugWriteLine("Mouse UsagePage: {0}", _deviceTree[MouseDevice].usUsagePage);
                DebugWriteLine("Mouse Sensitivity: {0}", _mouseSensitivity);
                DebugWriteLine("");
            }

            DebugWriteLine("Configured Hardware Mode: {0}", _remoteMode);
            DebugWriteLine("Configured Remote MouseStick Mode: {0}", _padMode);
            DebugWriteLine("Using Remote MouseStick Mode: {0}\n", _padMode);

            // get a file handle for the HID Remote device

            HID_OpenDevice(ref _deviceHandle);
            int lastError = Marshal.GetLastWin32Error();

            if (_deviceHandle.IsInvalid)
            {
                DebugWriteLine("Start_HID(): Failed to get open device");
                // throw new Win32Exception(lastError, "Failed to get open device");
            }

            SetHid(_remoteMode);

            // make sure the iMon hardware is in the right state
            if (_remoteMode == RemoteMode.iMON)
            {
                SetHid(_padMode);
            }

            _receiverWindowHID = new ReceiverWindow("iMon HID Receiver");
            _receiverWindowHID.ProcMsg += ProcMessage;

            if (RemoteDevice > -1)
            {
                _deviceTree[RemoteDevice].dwFlags = RawInput.RawInputDeviceFlags.InputSink;
                _deviceTree[RemoteDevice].hwndTarget = _receiverWindowHID.Handle;
            }
            if (KeyboardDevice > -1)
            {
                _deviceTree[KeyboardDevice].dwFlags = RawInput.RawInputDeviceFlags.InputSink |
                                                      RawInput.RawInputDeviceFlags.NoLegacy;
                _deviceTree[KeyboardDevice].hwndTarget = _receiverWindowHID.Handle;
            }
            if (MouseDevice > -1)
            {
                _deviceTree[MouseDevice].dwFlags = RawInput.RawInputDeviceFlags.InputSink;
                _deviceTree[MouseDevice].hwndTarget = _receiverWindowHID.Handle;
            }

            if (!_enableRemoteInput & !_enableKeyboardInput & !_enableMouseInput)
            {
                DebugWriteLine("ERROR: no input devices enabled");
                throw new InvalidOperationException("no input devices enabled");
            }

            if (_deviceTree != null)
            {
                if (_deviceTree.Length == 0)
                {
                    DebugWriteLine("ERROR: no iMon devices found");
                    throw new InvalidOperationException("no iMon devices found");
                }

                if (!RegisterForRawInput(_deviceTree))
                {
                    DebugWriteLine("ERROR: Failed to register for HID Raw input");
                    throw new InvalidOperationException("Failed to register for HID Raw input");
                }
            }
            DebugWriteLine("Start_HID(): completed");
        }

        private void HID_OpenDevice(ref SafeFileHandle _deviceHandle)
        {
            System.Threading.Thread.Sleep(25);
            DebugWriteLine("HID_OpenDevice(): Trying to open device for writing config to it");
            DebugWriteLine("HID_OpenDevice(): RemoteDeviceName = {0}", RemoteDeviceName);
            try
            {
                _deviceHandle = FileIO.CreateFile(RemoteDeviceName, FileIO.GENERIC_WRITE, FileIO.FILE_SHARE_READ | FileIO.FILE_SHARE_WRITE, IntPtr.Zero, FileIO.OPEN_EXISTING, 0, 0);
            }
            catch { }
            DebugWriteLine("HID_OpenDevice(): returned _deviceHandle = {0}", _deviceHandle.IsInvalid.ToString());

            if (_deviceHandle.IsInvalid == true)
            {
                DebugWriteLine("HID_OpenDevice(): trying with RemoteDeviceNameLower");
                string RemoteDeviceNameLower = RemoteDeviceName.ToLower();
                DebugWriteLine("HID_OpenDevice(): RemoteDeviceNameLower = {0}", RemoteDeviceNameLower);
                try
                {
                    _deviceHandle = FileIO.CreateFile(RemoteDeviceNameLower, FileIO.GENERIC_WRITE, FileIO.FILE_SHARE_READ | FileIO.FILE_SHARE_WRITE, IntPtr.Zero, FileIO.OPEN_EXISTING, 0, 0);
                }
                catch { }
                DebugWriteLine("HID_OpenDevice(): returned _deviceHandle = {0}", _deviceHandle.IsInvalid.ToString());
            }
        }

        /// <summary>
        /// Stop the IR Server plugin for HID devices.
        /// </summary>
        private void Stop_HID()
        {
            DebugWriteLine("Stop_HID()");
            if (_deviceTree != null)
            {
                _deviceTree[0].dwFlags |= RawInput.RawInputDeviceFlags.Remove;
                _deviceTree[1].dwFlags |= RawInput.RawInputDeviceFlags.Remove;
                _deviceTree[2].dwFlags |= RawInput.RawInputDeviceFlags.Remove;
                RegisterForRawInput(_deviceTree);

                _receiverWindowHID.ProcMsg -= ProcMessage;
                _receiverWindowHID.DestroyHandle();
                _receiverWindowHID = null;
            }
        }

        /// <summary>
        /// Suspend the IR Server plugin for HID devices when computer enters standby.
        /// </summary>
        private void Suspend_HID()
        {
            DebugWriteLine("Suspend_HID()");
            Stop_HID();
        }

        /// <summary>
        /// Resume the IR Server plugin for HID devices when the computer returns from standby.
        /// </summary>
        private void Resume_HID()
        {
            DebugWriteLine("Resume_HID()");
            Start_HID();
        }

        /// <summary>
        /// Detect the presence of iMon HID devices.  Devices that cannot be detected will always return false.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the device is present, otherwise <c>false</c>.
        /// </returns>
        private bool Detect_HID()
        {
            DebugWriteLine("Detect_HID()");
            bool FoundiMon = false;
            try
            {
                // get the complete list of raw input devices and parse it for supported devices
                List<DeviceDetails> _devices = new List<DeviceDetails>();

                try
                {
                    _devices = RawInput.EnumerateDevices();
                }
                catch
                {
                    return false;
                }

                if (_devices.Count > 0)
                {
                    foreach (DeviceDetails details in _devices)
                    {
                        DebugWriteLine("Detect_HID(): checking device \"{0}\"", details.ID);
                        // check the details against the supported device list
                        foreach (string sDevice in SupportedDevices_HID)
                        {
                            if (details.ID.ToLower().Contains(sDevice.ToLower()))
                            {
                                DebugWriteLine("Detect_HID(): Found device \"{0}\"", details.ID.Split('#')[1]);
                                // check for remote device - MI_01 (HIDRemoteSuffix)
                                if (details.ID.Contains(HIDRemoteSuffix)) //if (details.ID.Contains("MI_01#"))
                                {
                                    DebugWriteLine("Detect_HID(): Found iMon Remote device\n");
                                    // found the remote device
                                    FoundiMon = true;
                                }
                                // check for keyboard device - MI_00&COL02 (HIDKeyboardSuffix)
                                if (details.ID.Contains(HIDKeyboardSuffix))
                                {
                                    DebugWriteLine("Detect_HID(): Found iMon Keyboard device\n");
                                    // found the keyboard device
                                    FoundiMon = true;
                                }
                                // check for remote device - MI_00&COL01 (HIDMouseSuffix)
                                if (details.ID.Contains(HIDMouseSuffix))
                                {
                                    DebugWriteLine("Detect_HID(): Found iMon Mouse device\n");
                                    // found the mouse device
                                    FoundiMon = true;
                                }
                            }
                        }
                    }
                    DebugWriteLine("Detect_HID(): Found iMon HID device = {0}", FoundiMon);
                    return FoundiMon;
                }
                else
                {
                    DebugWriteLine("Detect_HID(): Found iMon HID device = {0}", false);
                    return false;
                }
            }
            catch
            {
                DebugWriteLine("Detect_HID(): No HID devices attached to the system");
                return false;
            }
        }

        #endregion

        #region PluginBase Functional Implimentation for iMon DOS devices

        /// <summary>
        /// Start the IR Server plugin for iMon DOS devices.
        /// </summary>
        private void Start_DOS()
        {
            DebugOpen("iMon Receiver.log");
            DebugWriteLine("Start_DOS()");

            LoadSettings();

            _deviceHandle = CreateFile(DosDevicePath, CreateFileAccessTypes.GenericRead | CreateFileAccessTypes.GenericWrite,
                                       CreateFileShares.Read | CreateFileShares.Write, IntPtr.Zero,
                                       CreateFileDisposition.OpenExisting, CreateFileAttributes.Normal, IntPtr.Zero);
            int lastError = Marshal.GetLastWin32Error();

            if (_deviceHandle.IsInvalid)
            {
                DebugWriteLine("Start_DOS(): Failed to open device");
                throw new Win32Exception(lastError, "Failed to open device");
            }
            else
            {
                deviceBufferPtr = Marshal.AllocHGlobal(DosDeviceBufferSize);
                FindFirmware();
                SetDos(_remoteMode);

                if (_remoteMode == RemoteMode.iMON)
                {
                    DebugWriteLine("Configured Hardware Mode: {0}", _remoteMode);
                    DebugWriteLine("Configured Remote MouseStick Mode: {0}", _padMode);
                }
                else
                {
                    DebugWriteLine("Configured Hardware Mode: {0}\n", _remoteMode);
                }

                _processReceiveThread = true;
                _receiveThread = new Thread(ReceiveThread);
                _receiveThread.Name = "iMon Receive Thread";
                _receiveThread.IsBackground = true;
                _receiveThread.Start();
            }
        }

        /// <summary>
        /// Stop the IR Server plugin for iMon DosDevices.
        /// </summary>
        private void Stop_DOS()
        {
            DebugWriteLine("Stop_DOS()");
            if (_processReceiveThread)
            {
                _processReceiveThread = false;

                if (_deviceHandle != null && !_deviceHandle.IsClosed)
                    CancelIo(_deviceHandle);
            }

            if (_receiveThread != null && _receiveThread.IsAlive)
                _receiveThread.Abort();

            _receiveThread = null;

            if (_deviceHandle != null)
                _deviceHandle.Dispose();

            _deviceHandle = null;

            DebugClose();
        }

        /// <summary>
        /// Suspend the IR Server plugin when computer enters standby.
        /// </summary>
        private void Suspend_DOS()
        {
            DebugWriteLine("Suspend()");
            Stop_DOS();
        }

        /// <summary>
        /// Resume the IR Server plugin when the computer returns from standby.
        /// </summary>
        private void Resume_DOS()
        {
            DebugWriteLine("Resume()");
            Start_DOS();
        }

        /// <summary>
        /// Detect the presence of the iMon DOS device.  Devices that cannot be detected will always return false.
        /// This method should not throw exceptions.
        /// </summary>
        /// <returns><c>true</c> if the device is present, otherwise <c>false</c>.</returns>
        private bool Detect_DOS()
        {
            DebugWriteLine("Detect_DOS()");
            try
            {
                SafeFileHandle deviceHandle = CreateFile(DosDevicePath,
                                                         CreateFileAccessTypes.GenericRead | CreateFileAccessTypes.GenericWrite,
                                                         CreateFileShares.Read | CreateFileShares.Write, IntPtr.Zero,
                                                         CreateFileDisposition.OpenExisting, CreateFileAttributes.Normal,
                                                         IntPtr.Zero);
                int lastError = Marshal.GetLastWin32Error();

                if (deviceHandle.IsInvalid)
                    throw new Win32Exception(lastError, "Failed to open device");

                deviceHandle.Dispose();
                DebugWriteLine("Detect_DOS(): completed - found device.");
                return true;
            }
            catch (Exception ex)
            {
                DebugWriteLine(ex.Message);
                DebugWriteLine("Detect_DOS(): completed - device not found.");
                return false;
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

        private void InitializeKeyboardState()
        {
            // initialize the key modifier state structure
            ModifierState.ShiftOn = false;
            ModifierState.LastKeyupWasShift = false;
            ModifierState.LastKeydownWasShift = false;
            ModifierState.CtrlOn = false;
            ModifierState.LastKeyupWasCtrl = false;
            ModifierState.LastKeydownWasCtrl = false;
            ModifierState.AltOn = false;
            ModifierState.LastKeyupWasAlt = false;
            ModifierState.LastKeydownWasAlt = false;
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

                _remoteMode = (RemoteMode)Enum.Parse(typeof(RemoteMode), doc.DocumentElement.Attributes["RemoteMode"].Value);
                _padMode = (PadMode)Enum.Parse(typeof(PadMode), doc.DocumentElement.Attributes["PadMode"].Value);

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

                _killImonM = bool.Parse(doc.DocumentElement.Attributes["KillImonM"].Value);

            }
            catch (Exception ex)
            {
                DebugWriteLine(ex.ToString());
                _remoteMode = RemoteMode.iMON;
                _padMode = PadMode.Keyboard;

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

                _killImonM = true;
            }
        }

        /// <summary>
        /// Load Setting For Kill Imon Manager from Config file.
        /// </summary>
        private void LoadKillImonM_Setting()
        {
            DebugWriteLine("LoadKillImonM_Setting(): LoadSettings specially for Kill Imon Manager()");
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(ConfigurationFile);

                _killImonM = bool.Parse(doc.DocumentElement.Attributes["KillImonM"].Value);
            }
            catch (Exception ex)
            {
                // Possibility to add a link for load catch setting from LoadSetting()  
                // with LoadSetting() directly
                DebugWriteLine(ex.ToString());
                _killImonM = true;
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

                writer.WriteAttributeString("RemoteMode", Enum.GetName(typeof(RemoteMode), _remoteMode));
                writer.WriteAttributeString("PadMode", Enum.GetName(typeof(PadMode), _padMode));

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

                writer.WriteAttributeString("KillImonM", _killImonM.ToString());


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
                throw new Win32Exception(dwError, "Imon:RegisterForRawInput()");
            }
            DebugWriteLine("RegisterForRawInput(): Done.");
            return true;
        }

        /// <summary>
        /// finds the iMon HID devices.
        /// </summary>
        private void FindDevices_HID()
        {
            if (_deviceTree != null) return;
            // configure the device tree
            int numDevices = (_enableRemoteInput ? 1 : 0) + (_enableKeyboardInput ? 1 : 0) + (_enableMouseInput ? 1 : 0);
            DebugWriteLine("FindDevices_HID(): searching for {0} devices", numDevices);
            if (numDevices == 0) return;
            RawInput.RAWINPUTDEVICE rDevice = new RawInput.RAWINPUTDEVICE();
            RawInput.RAWINPUTDEVICE kDevice = new RawInput.RAWINPUTDEVICE();
            RawInput.RAWINPUTDEVICE mDevice = new RawInput.RAWINPUTDEVICE();
            // get the complete list of raw input devices and parse it for supported devices
            List<DeviceDetails> _devices = new List<DeviceDetails>();

            try
            {
                _devices = RawInput.EnumerateDevices();
            }
            catch
            {
                return;
            }

            if (_devices.Count > 0)
            {
                foreach (DeviceDetails details in _devices)
                {
                    DebugWriteLine("FindDevices_HID(): checking device \"{0}\"", details.ID);
                    // check the details against the supported device list
                    foreach (string sDevice in SupportedDevices_HID)
                    {
                        if (details.ID.ToLower().Contains(sDevice.ToLower()))
                        {
                            DebugWriteLine("FindDevices_HID(): Found device \"{0}\"", details.ID.Split('#')[1]);
                            // check for remote device - MI_01#
                            if (details.ID.Contains(HIDRemoteSuffix))
                            {
                                DebugWriteLine("FindDevices_HID(): Found iMon Remote device\n");
                                // found the remote device
                                rDevice = new RawInput.RAWINPUTDEVICE();
                                rDevice.usUsage = details.Usage;
                                rDevice.usUsagePage = details.UsagePage;
                                RemoteDeviceName = details.ID;

                                // on very rare systems RemoteDeviceName is reported wrong, no idea why. Lets change it!
                                if (RemoteDeviceName.StartsWith(@"\??\"))
                                {
                                    DebugWriteLine("FindDevices_HID(): Changing to right RemoteDeviceName...");
                                    DebugWriteLine("FindDevices_HID():    reported RemoteDeviceName: \"{0}\"", RemoteDeviceName);
                                    RemoteDeviceName = @"\\?\" + RemoteDeviceName.Substring(4);
                                    DebugWriteLine("FindDevices_HID():   corrected RemoteDeviceName: \"{0}\"", RemoteDeviceName);
                                }
                            }
                            // check for keyboard device - MI_00&Col02#
                            if (details.ID.Contains(HIDKeyboardSuffix))
                            {
                                DebugWriteLine("FindDevices_HID(): Found iMon Keyboard device\n");
                                // found the keyboard device
                                kDevice = new RawInput.RAWINPUTDEVICE();
                                kDevice.usUsage = details.Usage;
                                kDevice.usUsagePage = details.UsagePage;
                                KeyboardDeviceName = details.ID;
                            }
                            // check for remote device - MI_00&Col01#
                            if (details.ID.Contains(HIDMouseSuffix))
                            {
                                DebugWriteLine("FindDevices_HID(): Found iMon Mouse device\n");
                                // found the mouse device
                                mDevice = new RawInput.RAWINPUTDEVICE();
                                mDevice.usUsage = details.Usage;
                                mDevice.usUsagePage = details.UsagePage;
                                MouseDeviceName = details.ID;
                            }
                        }
                    }
                }
                numDevices = ((rDevice.usUsage > 0) ? 1 : 0) + ((kDevice.usUsage > 0) ? 1 : 0) + ((mDevice.usUsage > 0) ? 1 : 0);
                int DevIndex = 0;
                DebugWriteLine("FindDevices_HID(): Found {0} Devices", numDevices);
                _deviceTree = new RawInput.RAWINPUTDEVICE[numDevices];
                if (rDevice.usUsage > 0)
                {
                    RemoteDevice = DevIndex;
                    DevIndex++;
                    _deviceTree[RemoteDevice].usUsage = rDevice.usUsage;
                    _deviceTree[RemoteDevice].usUsagePage = rDevice.usUsagePage;
                    DebugWriteLine("FindDevices_HID(): Added iMon Remote device as deviceTree[{0}]", RemoteDevice);
                }

                if (kDevice.usUsage > 0)
                {
                    KeyboardDevice = DevIndex;
                    DevIndex++;
                    _deviceTree[KeyboardDevice].usUsage = kDevice.usUsage;
                    _deviceTree[KeyboardDevice].usUsagePage = kDevice.usUsagePage;
                    DebugWriteLine("FindDevices_HID(): Added iMon Keyboard device as deviceTree[{0}]", KeyboardDevice);
                }

                if (mDevice.usUsage > 0)
                {
                    MouseDevice = DevIndex;
                    _deviceTree[MouseDevice].usUsage = mDevice.usUsage;
                    _deviceTree[MouseDevice].usUsagePage = mDevice.usUsagePage;
                    DebugWriteLine("FindDevices_HID(): Added iMon Mouse device as deviceTree[{0}]", MouseDevice);
                }
            }
        }

        private void ProcMessage(ref Message m)
        {
            switch (m.Msg)
            {
                case RawInput.WM_INPUT:
                    ProcessInputCommand(ref m);
                    break;

                case RawInput.WM_KEYDOWN:
                    ProcessKeyDown(m.WParam.ToInt32());
                    break;

                case RawInput.WM_KEYUP:
                    ProcessKeyUp(m.WParam.ToInt32());
                    break;

                case RawInput.WM_APPCOMMAND:
                    ProcessAppCommand(m.LParam.ToInt32());
                    break;
            }
        }

        private void ProcessKeyDown(int param)
        {
            if (_keyboardHandler != null)
                _keyboardHandler(Name, param, false);
        }

        private void ProcessKeyUp(int param)
        {
            if (_keyboardHandler != null)
                _keyboardHandler(Name, param, true);
        }

        private void ProcessAppCommand(int param)
        {
            DebugWriteLine("Received AppCommand - Param: {0}", param);
        }

        private void ProcessiMonMouseReport(byte Report1, byte Report2, byte Report3, byte Report4)
        {
            // filter out invalid reports
            if ((Report1 & 0x01) == ((Report2 & 0x80) >> 7)) return; // invalid position data
            if ((Report2 & 0x04) == ((Report2 & 0x02) << 1)) return; // invalid right click
            if ((Report2 & 0x01) == ((Report3 & 0x80) >> 7)) return; // invalid left click
            if (((Report1 & 0xFC) != 0x68) | (Report4 != 0xB7)) return;

            int dx, dy;
            mouseDataToInt(Report1, Report2, Report3, out dx, out dy);

            bool rightButton = ((Report2 & 0x04) != 0);
            bool leftButton = ((Report2 & 0x01) != 0);
            uint ulButtons = (uint)((Report2 & 0x04) + (Report2 & 0x01));
            DebugWriteLine(
              "ProcessMouseReport():    (xSize = {0}, ySize = {1} - lButton = {2}, rButton = {3}",
              dx, dy, leftButton, rightButton);
            //MouseEvent(xSign * xSize, ySign * ySize, right, left);

            // X movement is horizontal (negative towards left), y movement is vertical (negative towards top)
            if (_padMode == PadMode.Keyboard)
            {
                // convert the mouse movement into direction keys
                uint KeyMode = ((_remoteMode == RemoteMode.iMON) ? IMON_PAD_BUTTON : IMON_MCE_BUTTON);
                uint KeyCode = 0;
                uint KeyCode1 = 0;
                KeyCode = (uint)TranslateMouseToKeypress(dx, dy);
                if (KeyCode != 0) RemoteEvent((KeyCode + KeyMode), false);

                KeyCode1 = 0;
                switch (ulButtons)
                {
                    case 1: // Left click down
                    case 2: // Left click up
                        KeyCode1 = IMON_PAD_BUTTON_LCLICK;
                        break;
                    case 4: // Right click down
                    case 8: // Right click up
                        KeyCode1 = IMON_PAD_BUTTON_RCLICK;
                        break;
                }
                if (KeyCode1 != 0)
                {
                    RemoteEvent((KeyCode1 + KeyMode), false);
                }
                if ((KeyCode == 0) & (KeyCode1 == 0))
                {
                    DebugWriteLine("ProcessMouseReport(): (Keyboard mode) IGNORING MOUSE REPORT");
                }
            }
            else
            {
                MouseEvent(dx, dy, rightButton, leftButton);
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

                #region device filtering

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
                // stop processing if the device that generated the event is NOT an iMon device
                if (deviceName.Equals(string.Empty) | deviceName.Equals(""))
                {
                    return;
                }
                else
                {
                    if ((RemoteDevice > 0) & (raw.header.dwType == RawInput.RawInputType.HID))
                        if (!deviceName.Equals(RemoteDeviceName)) return;
                    if ((KeyboardDevice > 0) & (raw.header.dwType == RawInput.RawInputType.Keyboard))
                        if (!deviceName.Equals(KeyboardDeviceName)) return;
                    if ((MouseDevice > 0) & (raw.header.dwType == RawInput.RawInputType.Mouse))
                        if (!deviceName.Equals(MouseDeviceName)) return;
                }

                #endregion

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
                            if (((newArray[1] & 0xFC) == 0x28) & (newArray[4] == 0xB7))
                            {
                                // iMon PAD remote
                                int val = (((newArray[1] & 0x03) << 6) | (newArray[2] & 0x30) | ((newArray[2] & 0x06) << 1) |
                                           ((newArray[3] & 0xC0) >> 6));
                                code = String.Format("{0:X2}", val);
                                uint keyCode = IMON_PAD_BUTTON + (uint)val;
                                if ((_usePadSwitch) & (_remoteMode == RemoteMode.iMON))
                                {
                                    if ((keyCode == (IMON_PAD_BUTTON + 0x50)) | (keyCode == (IMON_PAD_BUTTON + 0x51)))
                                    {
                                        // the keyboard/mouse button was pressed or released
                                        if (keyCode == (IMON_PAD_BUTTON + 0x51))
                                        {
                                            // the mouse/keyboard button was released
                                            keyCode = 0;
                                            _padMode = ((_padMode == PadMode.Mouse)
                                                                    ? PadMode.Keyboard
                                                                    : PadMode.Mouse);
                                            //only change modes on the release of the button
                                            DebugWriteLine("RAW IMON HID REMOTE - MOUSE/ KEYBOARD MODE CHANGED - NEW MODE = {0}\n",
                                                           Enum.GetName(typeof(PadMode), _padMode));
                                            SetHid(_padMode);
                                        }
                                        break;
                                    }
                                }
                                if ((keyCode & 0x01) == 0)
                                {
                                    if (keyCode > 0)
                                    {
                                        RemoteEvent(keyCode, _remoteToggle != 1);
                                        _remoteToggle = 1;
                                    }
                                }
                                else
                                {
                                    _remoteToggle = 0;
                                    DebugWriteLine("iMon HID RemoteEvent: {0}, {1}\n", keyCode, "RELEASED (CONSUMED)");
                                }
                            }
                            else if (newArray[8] == 0xAE) // MCE remote button
                            {
                                uint keyCode = IMON_MCE_BUTTON + newArray[4];

                                RemoteEvent(keyCode, _remoteToggle != newArray[3]);
                                _remoteToggle = newArray[3];
                            }
                            else if (newArray[8] == 0xBE) // MCE Keyboard key press
                            {
                                KeyboardEvent(newArray[3], newArray[4]);
                            }
                            else if (newArray[8] == 0xCE) // MCE Keyboard mouse move/button
                            {
                                int xSign = (newArray[3] & 0x20) == 0 ? 1 : -1;
                                int ySign = (newArray[2] & 0x10) == 0 ? 1 : -1;

                                int xSize = (newArray[4] & 0x0F);
                                int ySize = (newArray[3] & 0x0F);

                                bool right = (newArray[4] & 0x40) != 0;
                                bool left = (newArray[4] & 0x20) != 0;

                                MouseEvent(xSign * xSize, ySign * ySize, right, left);
                            }
                            else if (newArray[8] == 0xEE) // Front panel buttons/volume knob
                            {
                                if (newArray[5] != 0x00)
                                {
                                    uint keyCode = IMON_PANEL_BUTTON + newArray[5];
                                    RemoteEvent(keyCode, _remoteToggle != newArray[4]);
                                }
                                _remoteToggle = newArray[4];

                                if (newArray[1] == 0x01)
                                    RemoteEvent(IMON_VOLUME_DOWN, true);
                                if (newArray[2] == 0x01)
                                    RemoteEvent(IMON_VOLUME_UP, true);
                                if (newArray[4] == 0x01)
                                    RemoteEvent(IMON_NAVIGATION_UP, true);
                                if (newArray[3] == 0x01)
                                    RemoteEvent(IMON_NAVIGATION_DOWN, true);
                            }
                            else if ((newArray[1] & 0xFC) == 0x68)
                            {
                                ProcessiMonMouseReport(newArray[1], newArray[2], newArray[3], newArray[4]);
                            }
                            //if (_remoteHandler != null) _remoteHandler(this.Name, code);
                            break;
                        }

                    case RawInput.RawInputType.Mouse:
                        {
                            DebugWriteLine("RAW IMON HID MOUSE - lLastX: {0}  lLastY: {1}  Buttons: {2}", raw.mouse.lLastX,
                                           raw.mouse.lLastY, raw.mouse.ulButtons);
                            // X movement is horizontal (negative towards left), y movement is vertical (negative towards top)
                            if (_padMode == PadMode.Keyboard)
                            {
                                // convert the mouse movement into direction keys
                                uint KeyMode = ((_remoteMode == RemoteMode.iMON) ? IMON_PAD_BUTTON : IMON_MCE_BUTTON);
                                uint KeyCode = 0;
                                uint KeyCode1 = 0;

                                KeyCode = (uint)TranslateMouseToKeypress(raw.mouse.lLastX, raw.mouse.lLastY);
                                if (KeyCode != 0) RemoteEvent((KeyCode + KeyMode), false);

                                KeyCode1 = 0;
                                switch (raw.mouse.ulButtons)
                                {
                                    case 1: // Left click down
                                    case 2: // Left click up
                                        KeyCode1 = IMON_PAD_BUTTON_LCLICK;
                                        break;
                                    case 4: // Right click down
                                    case 8: // Right click up
                                        KeyCode1 = IMON_PAD_BUTTON_RCLICK;
                                        break;
                                }
                                if (KeyCode1 != 0)
                                {
                                    RemoteEvent((KeyCode1 + KeyMode), false);
                                }
                                if ((KeyCode == 0) & (KeyCode1 == 0))
                                {
                                    DebugWriteLine("RAW IMON HID MOUSE - Ignoring");
                                }
                            }
                            else
                            {
                                MouseEvent(raw.mouse.lLastX, raw.mouse.lLastY, ((int)(raw.mouse.ulButtons & 0x03) > 0),
                                           ((int)(raw.mouse.ulButtons & 0x0C) > 0));
                                //if (_mouseHandler != null)
                                //    _mouseHandler(this.Name, raw.mouse.lLastX, raw.mouse.lLastY, (int)raw.mouse.ulButtons);
                            }
                            break;
                        }

                    case RawInput.RawInputType.Keyboard:
                        {
                            if (_enableKeyboardInput)
                            {
                                DebugWriteLine("RAW IMON HID KEYBOARD- CODE: {0}  FLAGS: {1}  MESSAGE: {2}", raw.keyboard.VKey,
                                               raw.keyboard.Flags, raw.keyboard.Message);
                                bool ConsumeKeypress = false;
                                bool SendToKeyboard = false;
                                uint KeyCode = 0;

                                switch (raw.keyboard.Flags)
                                {
                                    case RawInput.RawKeyboardFlags.KeyE0:
                                        DebugWriteLine(String.Format("KEYBOARD FLAG E0: {0}", raw.keyboard.MakeCode));
                                        KeyCode = 0;
                                        uint KeyBase = ((_remoteMode == RemoteMode.iMON) ? IMON_PAD_BUTTON : IMON_MCE_BUTTON);
                                        if (_remoteMode == RemoteMode.iMON)
                                        {
                                            if (raw.keyboard.VKey == 92)
                                            {
                                                KeyCode = IMON_PAD_BUTTON_WINKEY;
                                            }
                                            if (raw.keyboard.VKey == 93)
                                            {
                                                KeyCode = IMON_PAD_BUTTON_MENUKEY;
                                            }
                                        }
                                        else
                                        {
                                            if (raw.keyboard.VKey == 37)
                                            {
                                                KeyCode = IMON_MCE_BUTTON_UP;
                                            }
                                            if (raw.keyboard.VKey == 38)
                                            {
                                                KeyCode = IMON_MCE_BUTTON_LEFT;
                                            }
                                            if (raw.keyboard.VKey == 39)
                                            {
                                                KeyCode = IMON_MCE_BUTTON_RIGHT;
                                            }
                                            if (raw.keyboard.VKey == 40)
                                            {
                                                KeyCode = IMON_MCE_BUTTON_DOWN;
                                            }
                                        }
                                        if (KeyCode != 0) RemoteEvent(KeyCode + KeyBase, false);
                                        //if (_keyboardHandler != null)
                                        //  _keyboardHandler(this.Name, 0xE000 | raw.keyboard.MakeCode, true);

                                        break;

                                    case RawInput.RawKeyboardFlags.KeyE1:
                                        DebugWriteLine(String.Format("KEYBOARD FLAG E1: {0}", raw.keyboard.MakeCode));
                                        //if (_keyboardHandler != null)
                                        //  _keyboardHandler(this.Name, 0xE100, true);
                                        break;

                                    case RawInput.RawKeyboardFlags.KeyMake:
                                        //#if DEBUG
                                        //                                  DebugWriteLine("RAW IMON HID KEYBOARD CODE: {0}  FLAGS: {1}  MESSAGE: {2}", raw.keyboard.VKey, raw.keyboard.Flags, raw.keyboard.Message);
                                        //                                  Console.WriteLine("RAW IMON HID KEYBOARD CODE: {0}  FLAGS: {1}  MESSAGE: {2}  EXTRA: {3}", raw.keyboard.VKey, raw.keyboard.Flags, raw.keyboard.Message, raw.keyboard.ExtraInformation);
                                        //#endif
                                        if (_keyboardHandler != null)
                                        {
                                            KeyCode = 0;
                                            // convert the keyboard code into an iMon code
                                            if ((raw.keyboard.VKey == 16) | (raw.keyboard.VKey == 17) | (raw.keyboard.VKey == 18))
                                            {
                                                ModifierState.LastKeydownWasShift = false;
                                                ModifierState.LastKeyupWasShift = false;
                                                ModifierState.LastKeydownWasCtrl = false;
                                                ModifierState.LastKeyupWasCtrl = false;
                                                ModifierState.LastKeydownWasAlt = false;
                                                ModifierState.LastKeyupWasAlt = false;
                                                if (raw.keyboard.VKey == 16)
                                                {
                                                    ModifierState.ShiftOn = true;
                                                    ModifierState.LastKeydownWasShift = true;
                                                }
                                                if (raw.keyboard.VKey == 17)
                                                {
                                                    ModifierState.CtrlOn = true;
                                                    ModifierState.LastKeydownWasCtrl = true;
                                                }
                                                if (raw.keyboard.VKey == 18)
                                                {
                                                    ModifierState.AltOn = true;
                                                    ModifierState.LastKeydownWasAlt = true;
                                                }
                                                ConsumeKeypress = true;
                                            }
                                            else
                                            {
                                                if (_remoteMode == RemoteMode.iMON)
                                                {
                                                    if (ConvertVKeyToiMonKeyCode((Keyboard.VKey)raw.keyboard.VKey, ModifierState) != 0)
                                                    {
                                                        KeyCode = IMON_PAD_BUTTON +
                                                                  ConvertVKeyToiMonKeyCode((Keyboard.VKey)raw.keyboard.VKey, ModifierState);
                                                    }
                                                    else
                                                    {
                                                        KeyCode = raw.keyboard.VKey;
                                                        SendToKeyboard = true;
                                                    }
                                                    ModifierState.LastKeydownWasShift = false;
                                                    ModifierState.LastKeydownWasCtrl = false;
                                                    ModifierState.LastKeydownWasAlt = false;
                                                }
                                                else
                                                {
                                                    KeyCode = IMON_MCE_BUTTON +
                                                              ConvertVKeyToiMonMceKeyCode((Keyboard.VKey)raw.keyboard.VKey, ModifierState);
                                                    ModifierState.LastKeydownWasShift = false;
                                                    ModifierState.LastKeydownWasCtrl = false;
                                                    ModifierState.LastKeydownWasAlt = false;
                                                }
                                            }
                                            if (!ConsumeKeypress & !SendToKeyboard)
                                            {
                                                RemoteEvent(KeyCode, false);
                                            }
                                            else if (!ConsumeKeypress)
                                            {
                                                _keyboardHandler(Name, (int)KeyCode, false);
                                            }
                                            else
                                            {
                                                DebugWriteLine("CONSUMED HID KEYBOARD - CODE: {0}  FLAGS: {1}  MESSAGE: {2}", raw.keyboard.VKey,
                                                               raw.keyboard.Flags, raw.keyboard.Message);
                                            }
                                            //_keyboardHandler(this.Name, (int)KeyCode, false);
                                        }
                                        break;

                                    case RawInput.RawKeyboardFlags.KeyBreak:
                                        KeyCode = 0;
                                        ConsumeKeypress = false;
                                        SendToKeyboard = false;
                                        //convert the keyboard code into an iMon code
                                        if ((raw.keyboard.VKey == 16) | (raw.keyboard.VKey == 17) | (raw.keyboard.VKey == 18))
                                        {
                                            ModifierState.LastKeydownWasShift = false;
                                            ModifierState.LastKeyupWasShift = false;
                                            ModifierState.LastKeydownWasCtrl = false;
                                            ModifierState.LastKeyupWasCtrl = false;
                                            ModifierState.LastKeydownWasAlt = false;
                                            ModifierState.LastKeyupWasAlt = false;
                                            if (raw.keyboard.VKey == 16)
                                            {
                                                ModifierState.ShiftOn = false;
                                                ModifierState.LastKeyupWasShift = true;
                                            }
                                            if (raw.keyboard.VKey == 17)
                                            {
                                                ModifierState.CtrlOn = false;
                                                ModifierState.LastKeyupWasAlt = true;
                                            }
                                            if (raw.keyboard.VKey == 18)
                                            {
                                                ModifierState.AltOn = false;
                                                ModifierState.LastKeyupWasAlt = true;
                                            }
                                            ConsumeKeypress = true;
                                        }
                                        else
                                        {
                                            if (_remoteMode == RemoteMode.iMON)
                                            {
                                                if (ConvertVKeyToiMonKeyCode((Keyboard.VKey)raw.keyboard.VKey, ModifierState) != 0)
                                                {
                                                    KeyCode = IMON_PAD_BUTTON +
                                                              ConvertVKeyToiMonKeyCode((Keyboard.VKey)raw.keyboard.VKey, ModifierState);
                                                }
                                                else
                                                {
                                                    KeyCode = raw.keyboard.VKey;
                                                    //_keyboardHandler(this.Name, (int)KeyCode, true);
                                                    SendToKeyboard = true;
                                                }
                                                ModifierState.LastKeyupWasShift = false;
                                                ModifierState.LastKeyupWasCtrl = false;
                                                ModifierState.LastKeyupWasAlt = false;
                                            }
                                            else
                                            {
                                                KeyCode = IMON_MCE_BUTTON +
                                                          ConvertVKeyToiMonMceKeyCode((Keyboard.VKey)raw.keyboard.VKey, ModifierState);
                                                ModifierState.LastKeyupWasShift = false;
                                                ModifierState.LastKeyupWasCtrl = false;
                                                ModifierState.LastKeyupWasAlt = false;
                                            }
                                        }
                                        if (!ConsumeKeypress & !SendToKeyboard)
                                        {
                                            RemoteEvent(KeyCode, false);
                                        }
                                        else if (!ConsumeKeypress)
                                        {
                                            _keyboardHandler(Name, (int)KeyCode, true);
                                        }
                                        else
                                        {
                                            DebugWriteLine("CONSUMED HID KEYBOARD - CODE: {0}  FLAGS: {1}  MESSAGE: {2}\n", raw.keyboard.VKey,
                                                           raw.keyboard.Flags, raw.keyboard.Message);
                                        }
                                        break;

                                    case RawInput.RawKeyboardFlags.TerminalServerSetLED:
                                        DebugWriteLine("RAW IMON HID KEYBOARD - TerminalServerSetLED - CODE: {0}  FLAGS: {1}  MESSAGE: {2}",
                                                       raw.keyboard.VKey, raw.keyboard.Flags, raw.keyboard.Message);
                                        break;

                                    case RawInput.RawKeyboardFlags.TerminalServerShadow:
                                        DebugWriteLine("RAW IMON HID KEYBOARD - TerminalServerShadow - CODE: {0}  FLAGS: {1}  MESSAGE: {2}",
                                                       raw.keyboard.VKey, raw.keyboard.Flags, raw.keyboard.Message);
                                        break;
                                }
                            }
                            else DebugWriteLine("RAW IMON HID KEYBOARD (ignoring) - CODE: {0}  FLAGS: {1}  MESSAGE: {2}",
                                                raw.keyboard.VKey, raw.keyboard.Flags, raw.keyboard.Message);
                            break;
                        }
                }
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        private void SetHid(RemoteMode mode)
        {
            DebugWriteLine("Force_HID_RemoteMode({0})", Enum.GetName(typeof(RemoteMode), mode));
            byte[][] modeData = new Byte[0][];

            switch (mode)
            {
                case RemoteMode.iMON:
                    modeData = SetHidRemotePAD;
                    break;
                case RemoteMode.MCE:
                    modeData = SetHidRemoteMCE;
                    break;
            }
            SetHid(modeData);
        }

        private void SetHid(PadMode mode)
        {
            DebugWriteLine("Force_HID_PadMode({0})", Enum.GetName(typeof(PadMode), mode));
            byte[][] modeData = new Byte[0][];

            switch (mode)
            {
                case PadMode.Keyboard:
                    modeData = SetHidPadModeKeyboard;
                    break;
                case PadMode.Mouse:
                    modeData = SetHidPadModeMouse;
                    break;
            }
            SetHid(modeData);
        }

        private void SetHid(byte[][] modeData)
        {
            try
            {
                foreach (byte[] send in modeData)
                {
                    bool writeDevice = false;

                    DebugWriteLine("SetMode (HID): sending command ({0} bytes) to device", send.Length);
                    int bytesWritten = 0;
                    for (int i = 0; i < 100; i++)
                    {
                        try
                        {
                            DebugWriteLine("SetMode (HID): trying to send...");
                            writeDevice = FileIO.WriteFile(_deviceHandle, send, send.Length, ref bytesWritten, IntPtr.Zero);
                            if (!writeDevice)
                            {
                                DebugWriteLine("SetMode (HID): no success, reopening handle...");
                                HID_OpenDevice(ref _deviceHandle);
                            }
                        }
                        catch
                        {
                            DebugWriteLine("SetMode (HID): exception appeared while writing to device! reopening handle...");
                            HID_OpenDevice(ref _deviceHandle);
                        }
                        if (writeDevice) break;
                    }
                    if (writeDevice)
                        DebugWriteLine("SetMode (HID): sent {0} bytes to device", bytesWritten);
                    else
                        DebugWriteLine("SetMode (HID): sending failed after 100 tries");
                }
            }
            catch (Exception ex)
            {
                DebugWriteLine(ex.ToString());
            }
        }

        #endregion HID Device Specific Helper Functions

        #region IRemoteReceiver Functional Implimentation

        private void RemoteEvent(uint keyCode, bool firstPress)
        {
            DebugWriteLine("iMon RemoteEvent: {0}, {1}", keyCode, firstPress);
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

        #region Keyboard Support Functions (MCE Keyboard support and iMON HID Keyboard Device)

        private void KeyboardEvent(uint keyCode, uint modifiers)
        {
            DebugWriteLine("iMon KeyboardEvent: {0}, {1}", keyCode, modifiers);
            if (!_enableKeyboardInput)
                return;

            if (keyCode != _lastKeyboardKeyCode && modifiers == _lastKeyboardModifiers)
            {
                if (_handleKeyboardLocally)
                {
                    KeyUp(_lastKeyboardKeyCode, 0);
                    KeyDown(keyCode, 0);
                }
                else
                {
                    KeyUpRemote(_lastKeyboardKeyCode, 0);
                    KeyDownRemote(keyCode, 0);
                }

                _keyboardKeyRepeated = false;
            }
            else if (keyCode == _lastKeyboardKeyCode && modifiers != _lastKeyboardModifiers)
            {
                uint turnOff = _lastKeyboardModifiers & ~modifiers;
                uint turnOn = modifiers & ~_lastKeyboardModifiers;

                if (_handleKeyboardLocally)
                {
                    KeyUp(0, turnOff);
                    KeyDown(0, turnOn);
                }
                else
                {
                    KeyUpRemote(0, turnOff);
                    KeyDownRemote(0, turnOn);
                }

                _keyboardKeyRepeated = false;
            }
            else if (keyCode != _lastKeyboardKeyCode && modifiers != _lastKeyboardModifiers)
            {
                uint turnOff = _lastKeyboardModifiers & ~modifiers;
                uint turnOn = modifiers & ~_lastKeyboardModifiers;

                if (_handleKeyboardLocally)
                {
                    KeyUp(_lastKeyboardKeyCode, turnOff);
                    KeyDown(keyCode, turnOn);
                }
                else
                {
                    KeyUpRemote(_lastKeyboardKeyCode, turnOff);
                    KeyDownRemote(keyCode, turnOn);
                }

                _keyboardKeyRepeated = false;
            }
            else if (keyCode == _lastKeyboardKeyCode && modifiers == _lastKeyboardModifiers)
            {
                // Repeats ...
                TimeSpan timeBetween = DateTime.Now.Subtract(_lastKeyboardKeyTime);

                int firstRepeat = _keyboardFirstRepeat;
                int heldRepeats = _keyboardHeldRepeats;
                if (_useSystemRatesRemote)
                {
                    firstRepeat = 250 + (SystemInformation.KeyboardDelay * 250);
                    heldRepeats = (int)(1000.0 / (2.5 + (SystemInformation.KeyboardSpeed * 0.888)));
                }

                if (!_keyboardKeyRepeated && timeBetween.TotalMilliseconds < firstRepeat)
                    return;

                if (_keyboardKeyRepeated && timeBetween.TotalMilliseconds < heldRepeats)
                    return;

                if (_keyboardKeyRepeated && timeBetween.TotalMilliseconds > firstRepeat)
                    _keyboardKeyRepeated = false;
                else
                    _keyboardKeyRepeated = true;

                if (_handleKeyboardLocally)
                    KeyDown(keyCode, modifiers);
                else
                    KeyDownRemote(keyCode, modifiers);
            }

            _lastKeyboardKeyCode = keyCode;
            _lastKeyboardModifiers = modifiers;

            _lastKeyboardKeyTime = DateTime.Now;
        }

        private static void KeyUp(uint keyCode, uint modifiers)
        {
            if (keyCode != 0)
            {
                Keyboard.VKey vKey = ConvertMceKeyCodeToVKey(keyCode);
                Keyboard.KeyUp(vKey);
            }

            if (modifiers != 0)
            {
                if ((modifiers & (uint)KeyModifiers.LeftAlt) != 0)
                    Keyboard.KeyUp(Keyboard.VKey.VK_LMENU);
                if ((modifiers & (uint)KeyModifiers.LeftControl) != 0)
                    Keyboard.KeyUp(Keyboard.VKey.VK_LCONTROL);
                if ((modifiers & (uint)KeyModifiers.LeftShift) != 0)
                    Keyboard.KeyUp(Keyboard.VKey.VK_LSHIFT);
                if ((modifiers & (uint)KeyModifiers.LeftWin) != 0)
                    Keyboard.KeyUp(Keyboard.VKey.VK_LWIN);

                if ((modifiers & (uint)KeyModifiers.RightAlt) != 0)
                    Keyboard.KeyUp(Keyboard.VKey.VK_RMENU);
                if ((modifiers & (uint)KeyModifiers.RightControl) != 0)
                    Keyboard.KeyUp(Keyboard.VKey.VK_RCONTROL);
                if ((modifiers & (uint)KeyModifiers.RightShift) != 0)
                    Keyboard.KeyUp(Keyboard.VKey.VK_RSHIFT);
                if ((modifiers & (uint)KeyModifiers.RightWin) != 0)
                    Keyboard.KeyUp(Keyboard.VKey.VK_RWIN);
            }
        }

        private void KeyUpRemote(uint keyCode, uint modifiers)
        {
            if (_keyboardHandler == null)
                return;

            if (keyCode != 0)
            {
                Keyboard.VKey vKey = ConvertMceKeyCodeToVKey(keyCode);
                _keyboardHandler(Name, (int)vKey, true);
            }

            if (modifiers != 0)
            {
                if ((modifiers & (uint)KeyModifiers.LeftAlt) != 0)
                    _keyboardHandler(Name, (int)Keyboard.VKey.VK_LMENU, true);
                if ((modifiers & (uint)KeyModifiers.LeftControl) != 0)
                    _keyboardHandler(Name, (int)Keyboard.VKey.VK_LCONTROL, true);
                if ((modifiers & (uint)KeyModifiers.LeftShift) != 0)
                    _keyboardHandler(Name, (int)Keyboard.VKey.VK_LSHIFT, true);
                if ((modifiers & (uint)KeyModifiers.LeftWin) != 0)
                    _keyboardHandler(Name, (int)Keyboard.VKey.VK_LWIN, true);

                if ((modifiers & (uint)KeyModifiers.RightAlt) != 0)
                    _keyboardHandler(Name, (int)Keyboard.VKey.VK_RMENU, true);
                if ((modifiers & (uint)KeyModifiers.RightControl) != 0)
                    _keyboardHandler(Name, (int)Keyboard.VKey.VK_RCONTROL, true);
                if ((modifiers & (uint)KeyModifiers.RightShift) != 0)
                    _keyboardHandler(Name, (int)Keyboard.VKey.VK_RSHIFT, true);
                if ((modifiers & (uint)KeyModifiers.RightWin) != 0)
                    _keyboardHandler(Name, (int)Keyboard.VKey.VK_RWIN, true);
            }
        }

        private static void KeyDown(uint keyCode, uint modifiers)
        {
            if (modifiers != 0)
            {
                if ((modifiers & (uint)KeyModifiers.LeftAlt) != 0)
                    Keyboard.KeyDown(Keyboard.VKey.VK_LMENU);
                if ((modifiers & (uint)KeyModifiers.LeftControl) != 0)
                    Keyboard.KeyDown(Keyboard.VKey.VK_LCONTROL);
                if ((modifiers & (uint)KeyModifiers.LeftShift) != 0)
                    Keyboard.KeyDown(Keyboard.VKey.VK_LSHIFT);
                if ((modifiers & (uint)KeyModifiers.LeftWin) != 0)
                    Keyboard.KeyDown(Keyboard.VKey.VK_LWIN);

                if ((modifiers & (uint)KeyModifiers.RightAlt) != 0)
                    Keyboard.KeyDown(Keyboard.VKey.VK_RMENU);
                if ((modifiers & (uint)KeyModifiers.RightControl) != 0)
                    Keyboard.KeyDown(Keyboard.VKey.VK_RCONTROL);
                if ((modifiers & (uint)KeyModifiers.RightShift) != 0)
                    Keyboard.KeyDown(Keyboard.VKey.VK_RSHIFT);
                if ((modifiers & (uint)KeyModifiers.RightWin) != 0)
                    Keyboard.KeyDown(Keyboard.VKey.VK_RWIN);
            }

            if (keyCode != 0)
            {
                Keyboard.VKey vKey = ConvertMceKeyCodeToVKey(keyCode);
                Keyboard.KeyDown(vKey);
            }
        }

        private void KeyDownRemote(uint keyCode, uint modifiers)
        {
            if (_keyboardHandler == null)
                return;

            if (modifiers != 0)
            {
                if ((modifiers & (uint)KeyModifiers.LeftAlt) != 0)
                    _keyboardHandler(Name, (int)Keyboard.VKey.VK_LMENU, false);
                if ((modifiers & (uint)KeyModifiers.LeftControl) != 0)
                    _keyboardHandler(Name, (int)Keyboard.VKey.VK_LCONTROL, false);
                if ((modifiers & (uint)KeyModifiers.LeftShift) != 0)
                    _keyboardHandler(Name, (int)Keyboard.VKey.VK_LSHIFT, false);
                if ((modifiers & (uint)KeyModifiers.LeftWin) != 0)
                    _keyboardHandler(Name, (int)Keyboard.VKey.VK_LWIN, false);

                if ((modifiers & (uint)KeyModifiers.RightAlt) != 0)
                    _keyboardHandler(Name, (int)Keyboard.VKey.VK_RMENU, false);
                if ((modifiers & (uint)KeyModifiers.RightControl) != 0)
                    _keyboardHandler(Name, (int)Keyboard.VKey.VK_RCONTROL, false);
                if ((modifiers & (uint)KeyModifiers.RightShift) != 0)
                    _keyboardHandler(Name, (int)Keyboard.VKey.VK_RSHIFT, false);
                if ((modifiers & (uint)KeyModifiers.RightWin) != 0)
                    _keyboardHandler(Name, (int)Keyboard.VKey.VK_RWIN, false);
            }

            if (keyCode != 0)
            {
                Keyboard.VKey vKey = ConvertMceKeyCodeToVKey(keyCode);
                _keyboardHandler(Name, (int)vKey, false);
            }
        }

        #endregion Keyboard Support Functions (MCE Keyboard support and iMON HID Keyboard Device)

        #region IMouseReceiver Functional Implimentation

        private void MouseEvent(int deltaX, int deltaY, bool right, bool left)
        {
            DebugWriteLine("iMon MouseEvent: DX {0}, DY {1}, Right: {2}, Left: {3}", deltaX, deltaY, right, left);
            if (!_enableMouseInput)
                return;

            Mouse.MouseEvents buttons = Mouse.MouseEvents.None;
            if ((_mouseButtons & Mouse.MouseEvents.RightDown) != 0)
            {
                if (!right)
                {
                    buttons |= Mouse.MouseEvents.RightUp;
                    _mouseButtons &= ~Mouse.MouseEvents.RightDown;
                }
            }
            else
            {
                if (right)
                {
                    buttons |= Mouse.MouseEvents.RightDown;
                    _mouseButtons |= Mouse.MouseEvents.RightDown;
                }
            }
            if ((_mouseButtons & Mouse.MouseEvents.LeftDown) != 0)
            {
                if (!left)
                {
                    buttons |= Mouse.MouseEvents.LeftUp;
                    _mouseButtons &= ~Mouse.MouseEvents.LeftDown;
                }
            }
            else
            {
                if (left)
                {
                    buttons |= Mouse.MouseEvents.LeftDown;
                    _mouseButtons |= Mouse.MouseEvents.LeftDown;
                }
            }

            if (buttons != Mouse.MouseEvents.None)
            {
                if (_handleMouseLocally)
                    Mouse.Button(buttons);
            }

            deltaX = (int)(deltaX * _mouseSensitivity);
            deltaY = (int)(deltaY * _mouseSensitivity);

            if (deltaX != 0 || deltaY != 0)
            {
                if (_handleMouseLocally)
                    Mouse.Move(deltaX, deltaY, false);
            }

            if (!_handleMouseLocally)
                _mouseHandler(Name, deltaX, deltaY, (int)buttons);
        }

        /// <summary>
        /// Translates the mouse data provided into a keypress.
        /// </summary>
        /// <param name="dx">The x delta.</param>
        /// <param name="dy">The y delta.</param>
        /// <returns>Remote key code.</returns>
        private int TranslateMouseToKeypress(int dx, int dy)
        {
            int dxAbs = Math.Abs(dx);
            int dyAbs = Math.Abs(dy);
            if (Math.Max(dxAbs, dyAbs) > _keyPadSensitivity)
            {
                switch (_remoteMode)
                {
                    case RemoteMode.iMON:
                        if (dxAbs >= dyAbs)       // horizontal movement is larger, so it has preference
                            return (dx == dxAbs) ? (int)IMON_PAD_BUTTON_RIGHT : (int)IMON_PAD_BUTTON_LEFT;
                        else if (dyAbs > dxAbs)   // vertical movement is larger, so it has preference
                            return (dy == dyAbs) ? (int)IMON_PAD_BUTTON_DOWN : (int)IMON_PAD_BUTTON_UP;
                        break;
                    case RemoteMode.MCE:
                        if (dxAbs >= dyAbs)       // horizontal movement is larger, so it has preference
                            return (dx == dxAbs) ? (int)IMON_MCE_BUTTON_RIGHT : (int)IMON_MCE_BUTTON_LEFT;
                        else if (dyAbs > dxAbs)   // vertical movement is larger, so it has preference
                            return (dy == dyAbs) ? (int)IMON_MCE_BUTTON_DOWN : (int)IMON_MCE_BUTTON_UP;
                        break;
                }
            }
            return 0;
        }

        private void mouseDataToInt(byte ReportSign, byte ReportX, byte ReportY, out int dx, out int dy)
        {
            dx = ((ReportX & 0x40) >> 6) | ((ReportX & 0x20) >> 4) | ((ReportX & 0x10) >> 2) | (ReportX & 0x08);
            dy = ((ReportY & 0x40) >> 6) | ((ReportY & 0x20) >> 4) | ((ReportY & 0x10) >> 2) | (ReportY & 0x08);
            dx = ((ReportSign & 0x02)) == 0 ? dx : dx - 16;
            dy = ((ReportSign & 0x01)) == 0 ? dy : dy - 16;
        }

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

        private static uint ConvertVKeyToiMonKeyCode(Keyboard.VKey vKey, KeyModifierState ModState)
        {
            switch (vKey)
            {
                case Keyboard.VKey.VK_BACK:
                    return 0x20;
                case Keyboard.VKey.VK_SPACE:
                    return 0x94;
                case Keyboard.VKey.VK_1:
                    return 0x3A;
                case Keyboard.VKey.VK_2:
                    return 0xF2;
                case Keyboard.VKey.VK_3:
                    if (ModState.ShiftOn | ModState.LastKeyupWasShift)
                        return 0x60;

                    return 0x32;
                case Keyboard.VKey.VK_4:
                    return 0x8A;
                case Keyboard.VKey.VK_5:
                    return 0x5A;
                case Keyboard.VKey.VK_6:
                    return 0xAA;
                case Keyboard.VKey.VK_7:
                    return 0xD6;
                case Keyboard.VKey.VK_8:
                    if (ModState.ShiftOn | ModState.LastKeyupWasShift)
                        return 0x38;

                    return 0x88;
                case Keyboard.VKey.VK_9:
                    return 0xA0;
                case Keyboard.VKey.VK_0:
                    return 0xEA;
                case Keyboard.VKey.VK_RETURN:
                    return 0x22;
                case Keyboard.VKey.VK_ESCAPE:
                    return 0xFC;

                default:
                    return 0;
                //throw new ArgumentException(String.Format("Unknown Key Value {0}", vKey), "vKey");
            }
        }

        // TODO: Convert this function to a lookup from an XML file, then provide multiple files and a way to fine-tune...
        private static uint ConvertVKeyToiMonMceKeyCode(Keyboard.VKey vKey, KeyModifierState ModState)
        {
            switch (vKey)
            {
                case Keyboard.VKey.VK_A:
                    return 0x04;
                case Keyboard.VKey.VK_B:
                    return 0x05;
                case Keyboard.VKey.VK_C:
                    return 0x06;
                case Keyboard.VKey.VK_D:
                    return 0x07;
                case Keyboard.VKey.VK_E:
                    return 0x08;
                case Keyboard.VKey.VK_F:
                    return 0x09;
                case Keyboard.VKey.VK_G:
                    return 0x0A;
                case Keyboard.VKey.VK_H:
                    return 0x0B;
                case Keyboard.VKey.VK_I:
                    return 0x0C;
                case Keyboard.VKey.VK_J:
                    return 0x0D;
                case Keyboard.VKey.VK_K:
                    return 0x0E;
                case Keyboard.VKey.VK_L:
                    return 0x0F;
                case Keyboard.VKey.VK_M:
                    return 0x10;
                case Keyboard.VKey.VK_N:
                    return 0x11;
                case Keyboard.VKey.VK_O:
                    return 0x12;
                case Keyboard.VKey.VK_P:
                    return 0x13;
                case Keyboard.VKey.VK_Q:
                    return 0x14;
                case Keyboard.VKey.VK_R:
                    return 0x15;
                case Keyboard.VKey.VK_S:
                    return 0x16;
                case Keyboard.VKey.VK_T:
                    return 0x17;
                case Keyboard.VKey.VK_U:
                    return 0x18;
                case Keyboard.VKey.VK_V:
                    return 0x19;
                case Keyboard.VKey.VK_W:
                    return 0x1A;
                case Keyboard.VKey.VK_X:
                    return 0x1B;
                case Keyboard.VKey.VK_Y:
                    return 0x1C;
                case Keyboard.VKey.VK_Z:
                    return 0x1D;

                case Keyboard.VKey.VK_1:
                    return 0x01;
                case Keyboard.VKey.VK_2:
                    return 0x02;
                case Keyboard.VKey.VK_3:
                    if (ModState.ShiftOn | ModState.LastKeyupWasShift)
                        return 0x1E;

                    return 0x03;
                case Keyboard.VKey.VK_4:
                    return 0x04;
                case Keyboard.VKey.VK_5:
                    return 0x05;
                case Keyboard.VKey.VK_6:
                    return 0x06;
                case Keyboard.VKey.VK_7:
                    return 0x07;
                case Keyboard.VKey.VK_8:
                    if (ModState.ShiftOn | ModState.LastKeyupWasShift)
                        return 0x1D;

                    return 0x08;
                case Keyboard.VKey.VK_9:
                    return 0x09;
                case Keyboard.VKey.VK_0:
                    return 0x00;

                case Keyboard.VKey.VK_ESCAPE:
                    return 0x29;
                case Keyboard.VKey.VK_RETURN:
                    return 0x28;

                case Keyboard.VKey.VK_BACK:
                    return 0x23;

                case Keyboard.VKey.VK_TAB:
                    return 0x2B;
                case Keyboard.VKey.VK_SPACE:
                    return 0x2C;
                case Keyboard.VKey.VK_OEM_MINUS:
                    return 0x2D;
                case Keyboard.VKey.VK_OEM_PLUS:
                    return 0x2E;
                case Keyboard.VKey.VK_OEM_4:
                    return 0x2F;
                case Keyboard.VKey.VK_OEM_6:
                    return 0x30;
                case Keyboard.VKey.VK_OEM_5:
                    return 0x31;
                //case Keyboard.VKEY.VK_Non-US #: return 0X32;
                case Keyboard.VKey.VK_OEM_1:
                    return 0x33;
                case Keyboard.VKey.VK_OEM_7:
                    return 0x34;
                case Keyboard.VKey.VK_OEM_3:
                    return 0x35;
                case Keyboard.VKey.VK_OEM_COMMA:
                    return 0x36;
                case Keyboard.VKey.VK_OEM_PERIOD:
                    return 0x37;
                case Keyboard.VKey.VK_OEM_2:
                    return 0x38;
                case Keyboard.VKey.VK_CAPITAL:
                    return 0x39;
                case Keyboard.VKey.VK_F1:
                    return 0x3A;
                case Keyboard.VKey.VK_F2:
                    return 0x3B;
                case Keyboard.VKey.VK_F3:
                    return 0x3C;
                case Keyboard.VKey.VK_F4:
                    return 0x3D;
                case Keyboard.VKey.VK_F5:
                    return 0x3E;
                case Keyboard.VKey.VK_F6:
                    return 0x3F;
                case Keyboard.VKey.VK_F7:
                    return 0x40;
                case Keyboard.VKey.VK_F8:
                    return 0x41;
                case Keyboard.VKey.VK_F9:
                    return 0x42;
                case Keyboard.VKey.VK_F10:
                    return 0x43;
                case Keyboard.VKey.VK_F11:
                    return 0x44;
                case Keyboard.VKey.VK_F12:
                    return 0x45;
                case Keyboard.VKey.VK_PRINT:
                    return 0x46;
                case Keyboard.VKey.VK_SCROLL:
                    return 0x47;
                case Keyboard.VKey.VK_PAUSE:
                    return 0x48;
                case Keyboard.VKey.VK_INSERT:
                    return 0x49;
                case Keyboard.VKey.VK_HOME:
                    return 0x4A;
                case Keyboard.VKey.VK_PRIOR:
                    return 0x4B;
                case Keyboard.VKey.VK_DELETE:
                    return 0x4C;
                case Keyboard.VKey.VK_END:
                    return 0x4D;
                case Keyboard.VKey.VK_NEXT:
                    return 0x4E;
                case Keyboard.VKey.VK_RIGHT:
                    return 0x4F;
                case Keyboard.VKey.VK_LEFT:
                    return 0x50;
                case Keyboard.VKey.VK_DOWN:
                    return 0x51;
                case Keyboard.VKey.VK_UP:
                    return 0x52;
                case Keyboard.VKey.VK_OEM_102:
                    return 0x64;
                case Keyboard.VKey.VK_APPS:
                    return 0x65;

                default:
                    throw new ArgumentException(String.Format("Unknown Key Value {0}", vKey), "vKey");
            }
        }

        #endregion KeyCode Helper Functions

        #region iMon Manager Control functions

        private const int WM_MOUSEMOVE = 0x0200;

        private string FindiMonPath()
        {
            RegistryKey rKey;
            string SoundGraphPath = string.Empty;

            rKey = Registry.CurrentUser.OpenSubKey("Software\\SOUNDGRAPH\\iMON", false);
            if (rKey != null)
            {
                // soundgraph registry key exists
                Registry.CurrentUser.Close();
                rKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\iMON.exe",
                                                        false);
                if (rKey != null)
                {
                    SoundGraphPath = (string)rKey.GetValue("Path", string.Empty);
                }
                Registry.LocalMachine.Close();
            }
            else
            {
                Registry.CurrentUser.Close();
            }
            return SoundGraphPath;
        }

        private void KilliMonManager()
        {
                    
            // Load Checkbox setting for Kill Imon Manager before Start Driver
            LoadKillImonM_Setting();

            if (_killImonM)              
            {
                DebugWriteLine("KilliMonManager(): Set for Kill Imon Manager");
                DebugWriteLine("KilliMonManager()");
                string iMonPath = FindiMonPath();
                string iMonEXE = iMonPath + @"\iMON.exe";
                if (iMonPath != string.Empty)
                {
                    DebugWriteLine("KilliMonManager(): Found iMon Manager - Version {0}",
                                   FileVersionInfo.GetVersionInfo(iMonEXE).FileVersion);
                    Console.WriteLine("KilliMonManager(): Found iMon Manager - Version {0}",
                                      FileVersionInfo.GetVersionInfo(iMonEXE).FileVersion);
                }
                bool hasExited = false;
                Process[] VFDproc = Process.GetProcessesByName("iMON");
                if (VFDproc.Length > 0)
                {
                    DebugWriteLine("KilliMonManager(): Killing iMon Manager process");
                    VFDproc[0].Kill();
                    hasExited = false;
                    while (!hasExited)
                    {
                        Thread.Sleep(100);
                        VFDproc[0].Dispose();
                        VFDproc = Process.GetProcessesByName("iMON");
                        if (VFDproc.Length == 0) hasExited = true;
                    }
                }
                else
                {
                 DebugWriteLine("KilliMonManager(): iMon Manager not running");
                }
                 DebugWriteLine("KilliMonManager(): completed");
            }
            else
            {
                DebugWriteLine("KilliMonManager(): Set for Not Kill Imon Manager !!! WARNING NOT RECOMMENDED !!!");
            }

        }
        #endregion


        
    }
}