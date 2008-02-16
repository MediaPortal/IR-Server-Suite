using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using Microsoft.DirectX.DirectInput;

namespace InputService.Plugin
{

  /// <summary>
  /// IR Server Plugin for Direct Input game controllers.
  /// </summary>
  public class DirectInputReceiver : PluginBase, IRemoteReceiver, IMouseReceiver, IConfigure
  {

    #region Constants

    static readonly string ConfigurationFile =
      Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
      "\\IR Server Suite\\Input Service\\Direct Input Receiver.xml";

    #endregion Constants

    #region Variables

    RemoteHandler _remoteHandler;
    MouseHandler _mouseHandler;

    DirectInputListener _diListener;

    string _selectedDeviceGUID;

    DeviceList _deviceList;

    #endregion Variables

    #region Enumerations

    enum joyButton
    {
      axisXUp   = 3000,
      axisXDown = 3001,
      axisYUp   = 3002,
      axisYDown = 3003,
      axisZUp   = 3004,
      axisZDown = 3005,

      rotationXUp   = 3010,
      rotationXDown = 3011,
      rotationYUp   = 3012,
      rotationYDown = 3013,
      rotationZUp   = 3014,
      rotationZDown = 3015,

      povN  = 3020,
      povNE = 3021,
      povE  = 3022,
      povSE = 3023,
      povS  = 3024,
      povSW = 3025,
      povW  = 3026,
      povNW = 3027,

      button1   = 3030,
      button2   = 3031,
      button3   = 3032,
      button4   = 3033,
      button5   = 3034,
      button6   = 3035,
      button7   = 3036,
      button8   = 3037,
      button9   = 3038,
      button10  = 3039,
      button11  = 3040,
      button12  = 3041,
      button13  = 3042,
      button14  = 3043,
      button15  = 3044,
      button16  = 3045,
      button17  = 3046,
      button18  = 3047,
      button19  = 3048,
      button20  = 3049,

      comboKillProcess  = 4000,
      comboCloseProcess = 4001,
    }

    #endregion Enumerations


    void InitDeviceList()
    {
      _deviceList = Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly);
    }
    
    void diListener_OnStateChange(object sender, JoystickState state)
    {
      //SendActions(state);
    }

    void UnacquireDevice()
    {
      _diListener.DeInitDevice();
    }

    bool AcquireDevice()
    {
      if (_deviceList == null)
        return false;

      bool res = false;
      
      foreach (DeviceInstance di in _deviceList)
        if (di.InstanceGuid.ToString() == _selectedDeviceGUID)
          res = _diListener.InitDevice(di.InstanceGuid);

      return res;
    }



    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    public override string Name         { get { return "Direct Input"; } }
    /// <summary>
    /// IR Server plugin version.
    /// </summary>
    public override string Version      { get { return "1.0.4.2"; } }
    /// <summary>
    /// The IR Server plugin's author.
    /// </summary>
    public override string Author       { get { return "and-81, with original MediaPortal code by waeberd"; } }
    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    public override string Description  { get { return "Supports Direct Input game controllers"; } }

    /// <summary>
    /// Detect the presence of this device.  Devices that cannot be detected will always return false.
    /// </summary>
    /// <returns>
    /// true if the device is present, otherwise false.
    /// </returns>
    public override bool Detect()
    {
      // TODO: Add detection code.
      return false;
    }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    /// <returns>true if successful, otherwise false.</returns>
    public override void Start()
    {
      LoadSettings();

      InitDeviceList();

      _diListener = new DirectInputListener();
      _diListener.Delay = 150;
      _diListener.OnStateChange += new DirectInputListener.diStateChange(diListener_OnStateChange);

      if (!AcquireDevice())
        throw new ApplicationException("Failed to acquire device");
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
      if (_diListener != null)
      {
        UnacquireDevice();

        _diListener.OnStateChange -= new DirectInputListener.diStateChange(diListener_OnStateChange);
        _diListener = null;
      }

      _deviceList = null;
    }

    /// <summary>
    /// Configure the IR Server plugin.
    /// </summary>
    public void Configure(IWin32Window owner)
    {
      /*
      LoadSettings();

      InitDeviceList();

      Configure config = new Configure();

      config.MouseSensitivity     = _mouseSensitivity;

      if (config.ShowDialog(owner) == DialogResult.OK)
      {

        _mouseSensitivity       = config.MouseSensitivity;

        SaveSettings();
      }
      */
    }

    /// <summary>
    /// Callback for remote button presses.
    /// </summary>
    public RemoteHandler RemoteCallback
    {
      get { return _remoteHandler; }
      set { _remoteHandler = value; }
    }
    /// <summary>
    /// Callback for mouse events.
    /// </summary>
    public MouseHandler MouseCallback
    {
      get { return _mouseHandler; }
      set { _mouseHandler = value; }
    }



    void LoadSettings()
    {
      XmlDocument doc = new XmlDocument();

      try   { doc.Load(ConfigurationFile); }
      catch { return; }

      try   { _selectedDeviceGUID = doc.DocumentElement.Attributes["DeviceGUID"].Value; } catch {}
    }
    void SaveSettings()
    {
      try
      {
        using (XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, Encoding.UTF8))
        {
          writer.Formatting = Formatting.Indented;
          writer.Indentation = 1;
          writer.IndentChar = (char)9;
          writer.WriteStartDocument(true);
          writer.WriteStartElement("settings"); // <settings>

          writer.WriteAttributeString("DeviceGUID", _selectedDeviceGUID);

          writer.WriteEndElement(); // </settings>
          writer.WriteEndDocument();
        }
      }
#if TRACE
      catch (Exception ex)
      {
        Trace.WriteLine(ex.ToString());
      }
#else
      catch
      {
      }
#endif
    }



  }

}
