using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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

    #region Debug

    static void Remote(string deviceName, string code)
    {
      Console.WriteLine("Remote: {0}", code);
    }
    static void Keyboard(string deviceName, int button, bool up)
    {
      Console.WriteLine("Keyboard: {0}, {1}", button, up);
    }
    static void Mouse(string deviceName, int x, int y, int buttons)
    {
      Console.WriteLine("Mouse: ({0}, {1}) - {2}", x, y, buttons);
    }

    [STAThread]
    static void Main()
    {
      DirectInputReceiver c = new DirectInputReceiver();

      c.Configure(null);

      c.RemoteCallback += new RemoteHandler(Remote);
      //c.KeyboardCallback += new KeyboardHandler(Keyboard);
      c.MouseCallback += new MouseHandler(Mouse);

      c.Start();

      Application.Run();

      c.Stop();
      c = null;
    }

    #endregion Debug


    #region Constants

    static readonly string ConfigurationFile = Path.Combine(ConfigurationPath, "Direct Input Receiver.xml");

    const int AxisLimit = 4200;

    #endregion Constants

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
    }

    #endregion Enumerations

    #region Variables

    RemoteHandler _remoteHandler;
    MouseHandler _mouseHandler;

    DirectInputListener _diListener;

    string _selectedDeviceGUID;

    DeviceList _deviceList;

    #endregion Variables

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
    /// Gets a display icon for the plugin.
    /// </summary>
    /// <value>The icon.</value>
    public override Icon DeviceIcon     { get { return Properties.Resources.Icon; } }

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

      if (String.IsNullOrEmpty(_selectedDeviceGUID))
        throw new ApplicationException("Direct Input requires configuration");

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
        _diListener.DeInitDevice();
        _diListener.StopListener();
        
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
      LoadSettings();

      InitDeviceList();

      Configure config = new Configure(_deviceList);
      config.DeviceGuid = _selectedDeviceGUID;

      if (config.ShowDialog(owner) == DialogResult.OK)
      {
        if (!String.IsNullOrEmpty(config.DeviceGuid))
        {
          _selectedDeviceGUID = config.DeviceGuid;
          SaveSettings();
        }
      }
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


    void InitDeviceList()
    {
      _deviceList = Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly);
    }

    void diListener_OnStateChange(object sender, JoystickState state)
    {
      SendActions(state);
    }

    bool AcquireDevice()
    {
      if (_deviceList == null)
        return false;

      bool res = false;

      foreach (DeviceInstance di in _deviceList)
        if (_selectedDeviceGUID.Equals(di.InstanceGuid.ToString(), StringComparison.OrdinalIgnoreCase))
          res = _diListener.InitDevice(di.InstanceGuid);

      return res;
    }

    void SendActions(JoystickState state)
    {
      int actionCode = -1;

      //int curAxisValue = 0;
      // todo: timer stuff!!

      // buttons first!
      byte[] buttons = state.GetButtons();
      int button = 0;

      // button combos
      /*
      string sep = "";
      string pressedButtons = "";
      foreach (byte b in buttons)
      {
        if ((b & 0x80) != 0)
        {
          pressedButtons += sep + button.ToString("00");
          sep = ",";
        }
        button++;
      }
      */

      // single buttons
      if (actionCode == -1)
      {
        button = 0;
        bool foundButton = false;
        foreach (byte b in buttons)
        {
          if (0 != (b & 0x80))
          {
            foundButton = true;
            break;
          }
          button++;
        }
        if (foundButton)
        {
          if ((button >= 0) && (button <= 19))
          {
            // don't need no stinkin' enum-constants here....
            actionCode = 3030 + button;
          }
        }
      }

      // pov next
      if (actionCode == -1)
      {
        int[] pov = state.GetPointOfView();
        switch (pov[0])
        {
          case 0:     actionCode = (int)joyButton.povN;   break;
          case 4500:  actionCode = (int)joyButton.povNE;  break;
          case 9000:  actionCode = (int)joyButton.povE;   break;
          case 13500: actionCode = (int)joyButton.povSE;  break;
          case 18000: actionCode = (int)joyButton.povS;   break;
          case 22500: actionCode = (int)joyButton.povSW;  break;
          case 27000: actionCode = (int)joyButton.povW;   break;
          case 31500: actionCode = (int)joyButton.povNW;  break;
        }
      }

      if (actionCode == -1)
      {
        // axes next
        if (Math.Abs(state.X) > AxisLimit)
        {
          //curAxisValue = state.X;
          if (state.X > 0)
          {
            actionCode = (int)joyButton.axisXUp; // right
          }
          else
          {
            actionCode = (int)joyButton.axisXDown; // left
          }
        }
        else if (Math.Abs(state.Y) > AxisLimit)
        {
          //curAxisValue = state.Y;
          if (state.Y > 0)
          {
            // down
            actionCode = (int)joyButton.axisYUp;
          }
          else
          {
            // up
            actionCode = (int)joyButton.axisYDown;
          }
        }
        else if (Math.Abs(state.Z) > AxisLimit)
        {
          //curAxisValue = state.Z;
          if (state.Z > 0)
          {
            actionCode = (int)joyButton.axisZUp;
          }
          else
          {
            actionCode = (int)joyButton.axisZDown;
          }
        }
      }

      if (actionCode == -1)
      {
        // rotation
        if (Math.Abs(state.Rx) > AxisLimit)
        {
          //curAxisValue = state.Rx;
          if (state.Rx > 0)
          {
            actionCode = (int)joyButton.rotationXUp;
          }
          else
          {
            actionCode = (int)joyButton.rotationXDown;
          }
        }
        else if (Math.Abs(state.Ry) > AxisLimit)
        {
          //curAxisValue = state.Ry;
          if (state.Ry > 0)
          {
            actionCode = (int)joyButton.rotationYUp;
          }
          else
          {
            actionCode = (int)joyButton.rotationYDown;
          }
        }
        else if (Math.Abs(state.Rz) > AxisLimit)
        {
          //curAxisValue = state.Rz;
          if (state.Rz > 0)
          {
            actionCode = (int)joyButton.rotationZUp;
          }
          else
          {
            actionCode = (int)joyButton.rotationZDown;
          }
        }
      }

      if (actionCode != -1 && _remoteHandler != null)
      {
        string keyCode = TranslateActionCode(actionCode);

        _remoteHandler(this.Name, keyCode);
      }
    }

    string TranslateActionCode(int actionCode)
    {
      joyButton j = (joyButton)actionCode;

      return Enum.GetName(typeof(joyButton), j);
    }

  }

}
