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
      Debug.Open("Philips MCE USB IR Receiver- Spinel plus.log");
      Debug.WriteLine("Detect(): Called, Retrieving Plug&Play");
      return DetectionResult.DeviceIsPlugAndPlay;
    }

    /// <summary>
    /// Start the IR Server plugin for HID device.
    /// </summary>
    public override void Start()
    {
      Debug.Open("Philips MCE USB IR Receiver- Spinel plus.log");
      Debug.WriteLine("Start(): Starting \"Philips MCE USB IR Receiver- Spinel plus\" device");
      Start_Receiver();
    }

    /// <summary>
    /// Suspend the IR Server plugin for HID devices when computer enters standby.
    /// </summary>
    public override void Suspend()
    {
      Debug.WriteLine("Suspend()");
      //Stop_Receiver();
    }

    /// <summary>
    /// Resume the IR Server plugin for HID devices when the computer returns from standby.
    /// </summary>
    public override void Resume()
    {
      Debug.WriteLine("Resume()");
      //Start_Receiver();            
    }

    /// <summary>
    /// Stop the IR Server plugin.
    /// </summary>
    public override void Stop()
    {
      Debug.WriteLine("Stop()");
      Stop_Receiver();
    }

    #endregion

    #region IR Server Suite IConfigure Interface Implimentaion

    /// <summary>
    /// Configure the IR Server plugin.
    /// </summary>
    public void Configure(IWin32Window owner)
    {
      Debug.WriteLine("Configure()");
      config = new Config();
      ConfigManagement.LoadSettings(ref config);

      ConfigurationDialog configDialog = new ConfigurationDialog();

      configDialog.DoRepeats = config.doRepeats;
      configDialog.UseSystemRatesDelay = config.useSystemRatesDelay;
      configDialog.FirstRepeatDelay = config.firstRepeatDelay;
      configDialog.HeldRepeatDelay = config.heldRepeatDelay;

      if (configDialog.ShowDialog(owner) == DialogResult.OK)
      {
        config.doRepeats = configDialog.DoRepeats;
        config.useSystemRatesDelay = configDialog.UseSystemRatesDelay;
        config.firstRepeatDelay = configDialog.FirstRepeatDelay;
        config.heldRepeatDelay = configDialog.HeldRepeatDelay;

        ConfigManagement.SaveSettings(config);
      }
      Debug.WriteLine("Configure(): Completed");
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