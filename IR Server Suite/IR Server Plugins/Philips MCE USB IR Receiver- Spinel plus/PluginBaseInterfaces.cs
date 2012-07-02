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

      configDialog.DoRepeats = config.DoRepeats;
      configDialog.UseSystemRatesDelay = config.UseSystemRatesDelay;
      configDialog.FirstRepeatDelay = config.FirstRepeatDelay;
      configDialog.HeldRepeatDelay = config.HeldRepeatDelay;

      if (configDialog.ShowDialog(owner) == DialogResult.OK)
      {
        config.DoRepeats = configDialog.DoRepeats;
        config.UseSystemRatesDelay = configDialog.UseSystemRatesDelay;
        config.FirstRepeatDelay = configDialog.FirstRepeatDelay;
        config.HeldRepeatDelay = configDialog.HeldRepeatDelay;

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