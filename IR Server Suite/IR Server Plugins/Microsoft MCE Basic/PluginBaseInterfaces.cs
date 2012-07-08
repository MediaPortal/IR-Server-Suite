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
using System.Diagnostics;
using IRServer.Plugin.Properties;
using System.Windows.Forms;
using System.Drawing;
using IrssUtils;

namespace IRServer.Plugin
{
  public partial class MCEBasic : PluginBase, IConfigure, IRemoteReceiver, IDisposable
  {
    #region PluginBase Members

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    public override string Name
    {
      get { return "Microsoft MCE Basic"; }
    }

    /// <summary>
    /// IR Server plugin version.
    /// </summary>
    public override string Version
    {
      get { return "1.5.0.0"; }
    }

    /// <summary>
    /// The IR Server plugin's author.
    /// </summary>
    public override string Author
    {
      get { return "chefkoch"; }
    }

    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    public override string Description
    {
      get { return "Microsoft MCE Basic Receiver, based on MPcode"; }
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
    /// Detect the presence of this device.
    /// </summary>
    public override DetectionResult Detect()
    {
      return DetectionResult.DeviceIsPlugAndPlay;
    }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public override void Start()
    {
      Start_Receiver();
    }


    /// <summary>
    /// Suspend the IR Server plugin when computer enters standby.
    /// </summary>
    public override void Suspend()
    {
    }

    /// <summary>
    /// Resume the IR Server plugin when the computer returns from standby.
    /// </summary>
    public override void Resume()
    {
    }

    /// <summary>
    /// Stop the IR Server plugin.
    /// </summary>
    public override void Stop()
    {
      Stop_Receiver();
    }

    #endregion

    #region IConfigure Members

    /// <summary>
    /// Configure the IR Server plugin.
    /// </summary>
    public void Configure(IWin32Window owner)
    {
      //_config = new Config();
      //ConfigManagement.LoadSettings(ref _config);

      //ConfigurationDialog configDialog = new ConfigurationDialog();

      //configDialog.LearnTimeout = _config.LearnTimeout;
      //configDialog.DisableMceServices = _config._disableMceServices;

      //configDialog.EnableRemote = _config.EnableRemoteInput;
      //configDialog.UseSystemRatesForRemote = _config.UseSystemRatesRemote;
      //configDialog.RemoteRepeatDelay = _config.RemoteFirstRepeat;
      //configDialog.RemoteHeldDelay = _config.RemoteHeldRepeats;
      //configDialog.DisableAutomaticButtons = _config._disableAutomaticButtons;

      //configDialog.EnableKeyboard = _config.EnableKeyboardInput;
      //configDialog.UseSystemRatesForKeyboard = _config.UseSystemRatesKeyboard;
      //configDialog.KeyboardRepeatDelay = _config.KeyboardFirstRepeat;
      //configDialog.KeyboardHeldDelay = _config.KeyboardHeldRepeats;
      //configDialog.HandleKeyboardLocal = _config.HandleKeyboardLocally;
      //configDialog.UseQwertzLayout = _config.UseQwertzLayout;

      //configDialog.EnableMouse = _config.EnableMouseInput;
      //configDialog.HandleMouseLocal = _config.HandleMouseLocally;
      //configDialog.MouseSensitivity = _config.MouseSensitivity;

      //if (configDialog.ShowDialog(owner) == DialogResult.OK)
      //{
      //  _config.LearnTimeout = configDialog.LearnTimeout;
      //  _config._disableMceServices = configDialog.DisableMceServices;

      //  _config.EnableRemoteInput = configDialog.EnableRemote;
      //  _config.UseSystemRatesRemote = configDialog.UseSystemRatesForRemote;
      //  _config.RemoteFirstRepeat = configDialog.RemoteRepeatDelay;
      //  _config.RemoteHeldRepeats = configDialog.RemoteHeldDelay;
      //  _config._disableAutomaticButtons = configDialog.DisableAutomaticButtons;

      //  _config.EnableKeyboardInput = configDialog.EnableKeyboard;
      //  _config.UseSystemRatesKeyboard = configDialog.UseSystemRatesForKeyboard;
      //  _config.KeyboardFirstRepeat = configDialog.KeyboardRepeatDelay;
      //  _config.KeyboardHeldRepeats = configDialog.KeyboardHeldDelay;
      //  _config.HandleKeyboardLocally = configDialog.HandleKeyboardLocal;

      //  _config.EnableMouseInput = configDialog.EnableMouse;
      //  _config.HandleMouseLocally = configDialog.HandleMouseLocal;
      //  _config.MouseSensitivity = configDialog.MouseSensitivity;

      //  ConfigManagement.SaveSettings(_config);
      //}
    }

    #endregion

    #region IRemoteReceiver Members

    /// <summary>
    /// Callback for remote button presses.
    /// </summary>
    public RemoteHandler RemoteCallback { get; set; }

    #endregion
  }
}