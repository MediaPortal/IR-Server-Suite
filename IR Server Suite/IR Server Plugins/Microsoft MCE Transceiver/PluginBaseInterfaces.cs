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

namespace IRServer.Plugin
{
  public partial class MicrosoftMceTransceiver : PluginBase, IConfigure, ITransmitIR, ILearnIR, IRemoteReceiver,
                                         IKeyboardReceiver, IMouseReceiver
  {
    #region PluginBase Members

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    public override string Name
    {
      get { return "Microsoft MCE"; }
    }

    /// <summary>
    /// IR Server plugin version.
    /// </summary>
    public override string Version
    {
      get { return "1.4.2.0"; }
    }

    /// <summary>
    /// The IR Server plugin's author.
    /// </summary>
    public override string Author
    {
      get { return "and-81, inspired by Bruno Fleurette"; }
    }

    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    public override string Description
    {
      get { return "Microsoft MCE Infrared Transceiver"; }
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
      _config = new Config();
      ConfigManagement.LoadSettings(ref _config);
      if (_config.RestartTransceiverOnUSBEvent)
        return DetectionResult.DeviceIsPlugAndPlay;

      try
      {
        Guid deviceGuid;
        string devicePath;

        if (FindDevice(out deviceGuid, out devicePath))
        {
          return DetectionResult.DevicePresent;
        }
      }
      catch (Exception ex)
      {
        Logger.Error("{0,15}: exception {1}", Name, ex.Message);
        return DetectionResult.DeviceException;
      }

      return DetectionResult.DeviceNotFound;
    }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public override void Start()
    {
#if TRACE
      Trace.WriteLine("Start MicrosoftMceTransceiver");
#endif

      if (_driver != null)
        throw new InvalidOperationException("MicrosoftMceTransceiver already started");

      _config = new Config();
      ConfigManagement.LoadSettings(ref _config);

      // Put this in a try...catch so that if the registry keys don't exist we don't throw an ugly exception.
      try
      {
        _ignoreAutomaticButtons = CheckAutomaticButtons();
      }
      catch
      {
        _ignoreAutomaticButtons = false;
      }

      if (_config._disableMceServices)
        DisableMceServices();

      if (_config.RestartTransceiverOnUSBEvent)
        RegisterDeviceNotification();
      StartDriver();
    }

    /// <summary>
    /// Suspend the IR Server plugin when computer enters standby.
    /// </summary>
    public override void Suspend()
    {
#if TRACE
      Trace.WriteLine("Suspend MicrosoftMceTransceiver");
#endif

      if (_driver != null)
        _driver.Suspend();
    }

    /// <summary>
    /// Resume the IR Server plugin when the computer returns from standby.
    /// </summary>
    public override void Resume()
    {
#if TRACE
      Trace.WriteLine("Resume MicrosoftMceTransceiver");
#endif

      if (_driver != null)
        _driver.Resume();
    }

    /// <summary>
    /// Stop the IR Server plugin.
    /// </summary>
    public override void Stop()
    {
#if TRACE
      Trace.WriteLine("Stop MicrosoftMceTransceiver");
#endif

      StopDriver();
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

      ConfigurationDialog configDialog = new ConfigurationDialog();

      configDialog.LearnTimeout = _config.LearnTimeout;
      configDialog.DisableMceServices = _config._disableMceServices;

      configDialog.EnableRemote = _config.EnableRemoteInput;
      configDialog.UseSystemRatesForRemote = _config.UseSystemRatesRemote;
      configDialog.RemoteRepeatDelay = _config.RemoteFirstRepeat;
      configDialog.RemoteHeldDelay = _config.RemoteHeldRepeats;
      configDialog.DisableAutomaticButtons = _config._disableAutomaticButtons;

      configDialog.EnableKeyboard = _config.EnableKeyboardInput;
      configDialog.UseSystemRatesForKeyboard = _config.UseSystemRatesKeyboard;
      configDialog.KeyboardRepeatDelay = _config.KeyboardFirstRepeat;
      configDialog.KeyboardHeldDelay = _config.KeyboardHeldRepeats;
      configDialog.HandleKeyboardLocal = _config.HandleKeyboardLocally;
      configDialog.UseQwertzLayout = _config.UseQwertzLayout;

      configDialog.EnableMouse = _config.EnableMouseInput;
      configDialog.HandleMouseLocal = _config.HandleMouseLocally;
      configDialog.MouseSensitivity = _config.MouseSensitivity;

      if (configDialog.ShowDialog(owner) == DialogResult.OK)
      {
        _config.LearnTimeout = configDialog.LearnTimeout;
        _config._disableMceServices = configDialog.DisableMceServices;

        _config.EnableRemoteInput = configDialog.EnableRemote;
        _config.UseSystemRatesRemote = configDialog.UseSystemRatesForRemote;
        _config.RemoteFirstRepeat = configDialog.RemoteRepeatDelay;
        _config.RemoteHeldRepeats = configDialog.RemoteHeldDelay;
        _config._disableAutomaticButtons = configDialog.DisableAutomaticButtons;

        _config.EnableKeyboardInput = configDialog.EnableKeyboard;
        _config.UseSystemRatesKeyboard = configDialog.UseSystemRatesForKeyboard;
        _config.KeyboardFirstRepeat = configDialog.KeyboardRepeatDelay;
        _config.KeyboardHeldRepeats = configDialog.KeyboardHeldDelay;
        _config.HandleKeyboardLocally = configDialog.HandleKeyboardLocal;

        _config.EnableMouseInput = configDialog.EnableMouse;
        _config.HandleMouseLocally = configDialog.HandleMouseLocal;
        _config.MouseSensitivity = configDialog.MouseSensitivity;

        ConfigManagement.SaveSettings(_config);
      }
    }

    #endregion

    #region IRemoteReceiver Members

    /// <summary>
    /// Callback for remote button presses.
    /// </summary>
    public RemoteHandler RemoteCallback { get; set; }

    #endregion

    #region IKeyboardReceiver Members

    /// <summary>
    /// Callback for keyboard presses.
    /// </summary>
    public KeyboardHandler KeyboardCallback { get; set; }

    #endregion

    #region IMouseReceiver Members

    /// <summary>
    /// Callback for mouse events.
    /// </summary>
    public MouseHandler MouseCallback { get; set; }

    #endregion

    #region ILearnIR Members

    /// <summary>
    /// Learn an infrared command.
    /// </summary>
    /// <param name="data">New infrared command.</param>
    /// <returns>
    /// Tells the calling code if the learn was Successful, Failed or Timed Out.
    /// </returns>
    public LearnStatus Learn(out byte[] data)
    {
      IrCode code;

      LearnStatus status = _driver.Learn(_config.LearnTimeout, out code);

      if (code != null)
        data = code.ToByteArray();
      else
        data = null;

      return status;
    }

    #endregion

    #region ITransmitIR Members

    /// <summary>
    /// Lists the available blaster ports.
    /// </summary>
    public string[] AvailablePorts
    {
      get { return Enum.GetNames(typeof(BlasterPort)); }
    }

    /// <summary>
    /// Transmit an infrared command.
    /// </summary>
    /// <param name="port">Port to transmit on.</param>
    /// <param name="data">Data to transmit.</param>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public bool Transmit(string port, byte[] data)
    {
      BlasterPort blasterPort = BlasterPort.Both;
      try
      {
        blasterPort = (BlasterPort)Enum.Parse(typeof(BlasterPort), port, true);
      }
      catch
      {
#if TRACE
        Trace.WriteLine(String.Format("Invalid Blaster Port ({0}), using default ({1})", port, blasterPort));
#endif
      }

      IrCode code = IrCode.FromByteArray(data);

      if (code == null)
        throw new ArgumentException("Invalid IR Command data", "data");

      _driver.Send(code, (int)blasterPort);

      return true;
    }

    #endregion
  }
}