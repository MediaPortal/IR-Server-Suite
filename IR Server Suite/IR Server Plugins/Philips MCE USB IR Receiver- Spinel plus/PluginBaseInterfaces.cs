using System;
using System.Collections.Generic;
using System.Text;
using IRServer.Plugin.Properties;
using System.Windows.Forms;
using System.Drawing;

namespace IRServer.Plugin
{
    public partial class PhilipsMceUsbIrReceiverSpinelPlus : PluginBase, IConfigure, IRemoteReceiver, IDisposable
    {
        #region IR Server Suite PluginBase Interface Implimentaion

        /// <summary>
        /// Name of the IR Server plugin.
        /// </summary>
        /// <value>The name.</value>
        public override string Name
        {
          get { return "Philips MCE USB IR Receiver- Spinel plus"; }
        }

        /// <summary>
        /// IR Server plugin version.
        /// </summary>
        /// <value>The version.</value>
        public override string Version
        {
            get { return "0.1.0.0"; }
        }

        /// <summary>
        /// The IR Server plugin's author.
        /// </summary>
        /// <value>The author.</value>
        public override string Author
        {
            get { return "belcom"; }
        }

        /// <summary>
        /// A description of the IR Server plugin.
        /// </summary>
        /// <value>The description.</value>
        public override string Description
        {
          get { return "Supports Philips MCE USB IR Receiver- Spinel plus"; }
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
        /// Detect the presence of "Philips MCE USB IR Receiver- Spinel plus" devices.
        /// </summary>
        public override DetectionResult Detect()
        {
            DebugWriteLine("Detect()");
            bool detected = Detect_Device();
            if (detected)
            {
              DebugWriteLine("Detect(): completed - found Philips MCE USB IR Receiver- Spinel plus device");
                return DetectionResult.DevicePresent;
            }
            DebugWriteLine("Detect(): completed - device not found");
            return DetectionResult.DeviceNotFound;
        }

        /// <summary>
        /// Start the IR Server plugin for HID device.
        /// </summary>
        public override void Start()
        {
            DebugOpen("Philips MCE USB IR Receiver- Spinel plus.log");
            DebugWriteLine("Start()");
            //if (Detect() == DetectionResult.DevicePresent)
            //{
            DebugWriteLine("Start(): Starting \"Philips MCE USB IR Receiver- Spinel plus\" device");
            Start_Receiver();
            //}            
        }

        /// <summary>
        /// Suspend the IR Server plugin for HID devices when computer enters standby.
        /// </summary>
        public override void Suspend()
        {
            DebugWriteLine("Suspend()");
            Stop_Receiver();
        }

        /// <summary>
        /// Resume the IR Server plugin for HID devices when the computer returns from standby.
        /// </summary>
        public override void Resume()
        {
            DebugWriteLine("Resume()");
            Start_Receiver();            
        }

        /// <summary>
        /// Stop the IR Server plugin.
        /// </summary>
        public override void Stop()
        {
            DebugWriteLine("Stop()");
            Stop_Receiver();
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

            config.EnableRemote = _enableRemoteInput;
            config.UseSystemRatesForRemote = _useSystemRatesRemote;
            config.RemoteRepeatDelay = _remoteFirstRepeat;
            config.RemoteHeldDelay = _remoteHeldRepeats;
           
            if (config.ShowDialog(owner) == DialogResult.OK)
            {
                _enableRemoteInput = config.EnableRemote;
                _useSystemRatesRemote = config.UseSystemRatesForRemote;
                _remoteFirstRepeat = config.RemoteRepeatDelay;
                _remoteHeldRepeats = config.RemoteHeldDelay;

                SaveSettings();                
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

    }
}
