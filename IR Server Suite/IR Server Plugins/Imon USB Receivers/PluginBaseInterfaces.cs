using System;
using System.Collections.Generic;
using System.Text;
using IRServer.Plugin.Properties;
using System.Windows.Forms;
using System.Drawing;

namespace IRServer.Plugin
{
    public partial class iMonUSBReceivers : PluginBase, IConfigure, IRemoteReceiver, IKeyboardReceiver, IMouseReceiver, IDisposable
    {
        #region IR Server Suite PluginBase Interface Implimentaion

        /// <summary>
        /// Name of the IR Server plugin.
        /// </summary>
        /// <value>The name.</value>
        public override string Name
        {
            get { return "iMon USB"; }
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
            get { return "and-81 and CybrMage"; }
        }

        /// <summary>
        /// A description of the IR Server plugin.
        /// </summary>
        /// <value>The description.</value>
        public override string Description
        {
            get { return "Supports iMon USB Legacy and HID IR devices"; }
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
        /// Detect the presence of iMon devices.
        /// </summary>
        public override DetectionResult Detect()
        {
            DebugWriteLine("Detect()");
            bool HasDOS = Detect_DOS();
            if (HasDOS)
            {
                _DriverMode = DeviceType.DOS;
                DebugWriteLine("Detect(): completed - found DOS device");
                return DetectionResult.DevicePresent;
            }
            bool HasHID = Detect_HID();
            if (HasHID)
            {
                _DriverMode = DeviceType.HID;
                DebugWriteLine("Detect(): completed - found HID device");
                return DetectionResult.DevicePresent;
            }
            _DriverMode = DeviceType.None;
            DebugWriteLine("Detect(): completed - device not found");
            return DetectionResult.DeviceNotFound;
        }

        /// <summary>
        /// Start the IR Server plugin for HID device.
        /// </summary>
        public override void Start()
        {
            DeviceType DevType = DeviceDriverMode;
            DebugOpen("iMonHidReceiver.log");
            DebugWriteLine("Start()");
            DebugWriteLine("Start(): DeviceDriverMode = {0}", Enum.GetName(typeof(DeviceType), DevType));
            // ensure that iMon manager will not interfere with us
            KilliMonManager();

            if (DevType == DeviceType.DOS)
            {
                DebugWriteLine("Start(): Starting DOS device");
                Start_DOS();
            }
            else if (DevType == DeviceType.HID)
            {
                DebugWriteLine("Start(): Starting HID device");
                Start_HID();
            }
            else
            {
                DebugWriteLine("Start(): No iMon devices found!");
                throw new ApplicationException("No iMon devices found!");
            }
        }

        /// <summary>
        /// Suspend the IR Server plugin for HID devices when computer enters standby.
        /// </summary>
        public override void Suspend()
        {
            DebugWriteLine("Suspend()");
            if (_DriverMode == DeviceType.HID)
                Stop_HID();
            else
                Stop_DOS();
        }

        /// <summary>
        /// Resume the IR Server plugin for HID devices when the computer returns from standby.
        /// </summary>
        public override void Resume()
        {
            DebugWriteLine("Resume()");
            if (_DriverMode == DeviceType.HID)
                Start_HID();
            else
                Start_DOS();
        }

        /// <summary>
        /// Stop the IR Server plugin.
        /// </summary>
        public override void Stop()
        {
            DebugWriteLine("Stop()");
            if (_DriverMode == DeviceType.HID)
            {
                Stop_HID();
            }
            else
            {
                Stop_DOS();
            }
        }

        #endregion

        #region IR Server Suite IConfigure Interface Implimentaion

        /// <summary>
        /// Configure the IR Server plugin.
        /// </summary>
        public void Configure(IWin32Window owner)
        {
            DebugWriteLine("Configure()");
            LoadSettings();

            Configuration config = new Configuration();

            config.RemoteMode = _remoteMode;
            config.PadMode = _padMode;

            config.EnableRemote = _enableRemoteInput;
            config.UsePadSwitch = _usePadSwitch; 
            config.UseSystemRatesForRemote = _useSystemRatesRemote;
            config.RemoteRepeatDelay = _remoteFirstRepeat;
            config.RemoteHeldDelay = _remoteHeldRepeats;

            config.EnableKeyboard = _enableKeyboardInput;
            config.KeyPadSensitivity = _keyPadSensitivity;
            config.UseSystemRatesForKeyboard = _useSystemRatesKeyboard;
            config.KeyboardRepeatDelay = _keyboardFirstRepeat;
            config.KeyboardHeldDelay = _keyboardHeldRepeats;
            config.HandleKeyboardLocal = _handleKeyboardLocally;

            config.EnableMouse = _enableMouseInput;
            config.HandleMouseLocal = _handleMouseLocally;
            config.MouseSensitivity = _mouseSensitivity;
            
            if (config.ShowDialog(owner) == DialogResult.OK)
            {
                _remoteMode = config.RemoteMode;
                if ((_remoteMode != RemoteMode.iMON) & (_remoteMode != RemoteMode.MCE)) _remoteMode = RemoteMode.iMON;

                _padMode = config.PadMode;
                if ((_padMode != PadMode.Keyboard) & (_padMode != PadMode.Mouse)) _padMode = PadMode.Keyboard;

                _enableRemoteInput = config.EnableRemote;
                _usePadSwitch = config.UsePadSwitch;
                _useSystemRatesRemote = config.UseSystemRatesForRemote;
                _remoteFirstRepeat = config.RemoteRepeatDelay;
                _remoteHeldRepeats = config.RemoteHeldDelay;

                _enableKeyboardInput = config.EnableKeyboard;
                _keyPadSensitivity = config.KeyPadSensitivity;
                _useSystemRatesKeyboard = config.UseSystemRatesForKeyboard;
                _keyboardFirstRepeat = config.KeyboardRepeatDelay;
                _keyboardHeldRepeats = config.KeyboardHeldDelay;
                _handleKeyboardLocally = config.HandleKeyboardLocal;

                _enableMouseInput = config.EnableMouse;
                _handleMouseLocally = config.HandleMouseLocal;
                _mouseSensitivity = config.MouseSensitivity;
                
                SaveSettings();
                if (_DriverMode == DeviceType.DOS)
                {
                    SetDos(_remoteMode);
                }
                else if (_DriverMode == DeviceType.HID)
                {
                    SetHid(_remoteMode);
                    SetHid(_padMode);
                }
            }
            DebugWriteLine("Configure(): Completed");
        }

        #endregion

        #region IR Server Suite IRemoteReceiver Interface Implimentation

        /// <summary>
        /// Callback for remote button presses.
        /// </summary>
        public RemoteHandler RemoteCallback
        {
            get { return _remoteHandler; }
            set { _remoteHandler = value; }
        }

        #endregion

        #region IR Server Suite IKeyboardReceiver Interface Implimentation

        /// <summary>
        /// Callback for keyboard presses.
        /// </summary>
        public KeyboardHandler KeyboardCallback
        {
            get { return _keyboardHandler; }
            set { _keyboardHandler = value; }
        }

        #endregion

        #region IR Server Suite IMouseReceiver Interface Implimentation

        /// <summary>
        /// Callback for mouse events.
        /// </summary>
        public MouseHandler MouseCallback
        {
            get { return _mouseHandler; }
            set { _mouseHandler = value; }
        }

        #endregion
    }
}
