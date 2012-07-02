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
using IrssUtils;

namespace IRServer.Plugin
{
  public partial class DirectInputReceiver : PluginBase, IConfigure, IRemoteReceiver, IMouseReceiver
  {
    #region PluginBase Members

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name
    {
      get { return "Direct Input"; }
    }

    /// <summary>
    /// IR Server plugin version.
    /// </summary>
    /// <value>The version.</value>
    public override string Version
    {
      get { return "1.5.0.0"; }
    }

    /// <summary>
    /// The IR Server plugin's author.
    /// </summary>
    /// <value>The author.</value>
    public override string Author
    {
      get { return "chefkoch, and-81, waeberd"; }
    }

    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    /// <value>The description.</value>
    public override string Description
    {
      get { return "Supports Direct Input game controllers"; }
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
      Shared.getStatus();
      if (Shared._serviceInstalled)
      {
        IrssLog.Warn("{0,15}: not available on \"service\" installation mode", Name);
        return DetectionResult.DeviceDisabled;
      }
      return DetectionResult.DevicePresent;
    }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    /// <returns>true if successful, otherwise false.</returns>
    public override void Start()
    {
      _config = new Config();
      ConfigManagement.LoadSettings(ref _config);

      RegisterDeviceNotification();
      StartListener();
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
      StopListener();
    }

    #endregion

    #region IConfigure Members

    /// <summary>
    /// Configure the IR Server plugin.
    /// </summary>
    public void Configure(IWin32Window owner)
    {
      _config = new Config();
      ConfigManagement.LoadSettings(ref _config);

      InitDeviceList();

      ConfigurationDialog configDialog = new ConfigurationDialog(_deviceList);
      configDialog.DeviceGuid = _config.DeviceGUID;
      configDialog.AxisLimit = _config.AxisLimit;

      if (configDialog.ShowDialog(owner) == DialogResult.OK)
      {
        if (!String.IsNullOrEmpty(configDialog.DeviceGuid))
        {
          _config.DeviceGUID = configDialog.DeviceGuid;
          _config.AxisLimit = configDialog.AxisLimit;

          ConfigManagement.SaveSettings(_config);
        }
      }
    }

    #endregion

    #region IRemoteReceiver Members

    /// <summary>
    /// Callback for remote button presses.
    /// </summary>
    public RemoteHandler RemoteCallback { get; set; }

    #endregion

    #region IMouseReceiver Members

    /// <summary>
    /// Callback for mouse events.
    /// </summary>
    public MouseHandler MouseCallback { get; set; }

    #endregion
  }
}