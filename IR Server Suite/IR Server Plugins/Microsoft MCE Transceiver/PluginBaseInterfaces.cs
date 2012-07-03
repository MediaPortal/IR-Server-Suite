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
        IrssLog.Error("{0,15}: exception {1}", Name, ex.Message);
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

      Guid deviceGuid;
      string devicePath;

      Driver newDriver = null;

      if (FindDevice(out deviceGuid, out devicePath))
      {
        if (deviceGuid == MicrosoftGuid)
        {
          if (Environment.OSVersion.Version.Major >= VistaVersionNumber)
            newDriver = new DriverVista(deviceGuid, devicePath, RemoteEvent, KeyboardEvent, MouseEvent);
          else
            newDriver = new DriverXP(deviceGuid, devicePath, RemoteEvent, KeyboardEvent, MouseEvent);
        }
        else
        {
          newDriver = new DriverReplacement(deviceGuid, devicePath, RemoteEvent, KeyboardEvent, MouseEvent);
        }
      }
      else
      {
        throw new InvalidOperationException("Device not found");
      }

      newDriver.Start();

      _driver = newDriver;
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

      if (_driver != null)
      {
        try
        {
          _driver.Stop();
        }
        finally
        {
          _driver = null;
        }
      }
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

      configDialog.LearnTimeout = _config._learnTimeout;
      configDialog.DisableMceServices = _config._disableMceServices;

      configDialog.EnableRemote = _config._enableRemoteInput;
      configDialog.UseSystemRatesForRemote = _config._useSystemRatesRemote;
      configDialog.RemoteRepeatDelay = _config._remoteFirstRepeat;
      configDialog.RemoteHeldDelay = _config._remoteHeldRepeats;
      configDialog.DisableAutomaticButtons = _config._disableAutomaticButtons;

      configDialog.EnableKeyboard = _config._enableKeyboardInput;
      configDialog.UseSystemRatesForKeyboard = _config._useSystemRatesKeyboard;
      configDialog.KeyboardRepeatDelay = _config._keyboardFirstRepeat;
      configDialog.KeyboardHeldDelay = _config._keyboardHeldRepeats;
      configDialog.HandleKeyboardLocal = _config._handleKeyboardLocally;
      configDialog.UseQwertzLayout = _config._useQwertzLayout;

      configDialog.EnableMouse = _config._enableMouseInput;
      configDialog.HandleMouseLocal = _config._handleMouseLocally;
      configDialog.MouseSensitivity = _config._mouseSensitivity;

      if (configDialog.ShowDialog(owner) == DialogResult.OK)
      {
        _config._learnTimeout = configDialog.LearnTimeout;
        _config._disableMceServices = configDialog.DisableMceServices;

        _config._enableRemoteInput = configDialog.EnableRemote;
        _config._useSystemRatesRemote = configDialog.UseSystemRatesForRemote;
        _config._remoteFirstRepeat = configDialog.RemoteRepeatDelay;
        _config._remoteHeldRepeats = configDialog.RemoteHeldDelay;
        _config._disableAutomaticButtons = configDialog.DisableAutomaticButtons;

        _config._enableKeyboardInput = configDialog.EnableKeyboard;
        _config._useSystemRatesKeyboard = configDialog.UseSystemRatesForKeyboard;
        _config._keyboardFirstRepeat = configDialog.KeyboardRepeatDelay;
        _config._keyboardHeldRepeats = configDialog.KeyboardHeldDelay;
        _config._handleKeyboardLocally = configDialog.HandleKeyboardLocal;

        _config._enableMouseInput = configDialog.EnableMouse;
        _config._handleMouseLocally = configDialog.HandleMouseLocal;
        _config._mouseSensitivity = configDialog.MouseSensitivity;

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

      LearnStatus status = _driver.Learn(_config._learnTimeout, out code);

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