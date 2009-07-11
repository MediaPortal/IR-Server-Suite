#region Copyright (C) 2005-2009 Team MediaPortal

// Copyright (C) 2005-2009 Team MediaPortal
// http://www.team-mediaportal.com
// 
// This Program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2, or (at your option)
// any later version.
// 
// This Program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with GNU Make; see the file COPYING.  If not, write to
// the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
// http://www.gnu.org/copyleft/gpl.html

#endregion

using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using InputService.Plugin.Properties;
using IrssUtils;
using Microsoft.DirectX.DirectInput;

namespace InputService.Plugin
{
  /// <summary>
  /// IR Server Plugin for Direct Input game controllers.
  /// </summary>
  public class DirectInputReceiver : PluginBase, IRemoteReceiver, IMouseReceiver, IConfigure
  {
    #region Debug

    private static void Remote(string deviceName, string code)
    {
      Console.WriteLine("Remote: {0}", code);
    }

    private static void Mouse(string deviceName, int x, int y, int buttons)
    {
      Console.WriteLine("Mouse: ({0}, {1}) - {2}", x, y, buttons);
    }

    [STAThread]
    private static void Main()
    {
      DirectInputReceiver c = new DirectInputReceiver();

      c.Configure(null);

      c.RemoteCallback += Remote;
      c.MouseCallback += Mouse;

      c.Start();

      Application.Run();

      c.Stop();
      c = null;
    }

    #endregion Debug

    #region Constants

    private const int AxisLimit = 4200;
    private static readonly string ConfigurationFile = Path.Combine(ConfigurationPath, "Direct Input Receiver.xml");

    #endregion Constants

    #region Enumerations

    private enum joyButton
    {
      axisXUp = 3000,
      axisXDown = 3001,
      axisYUp = 3002,
      axisYDown = 3003,
      axisZUp = 3004,
      axisZDown = 3005,

      rotationXUp = 3010,
      rotationXDown = 3011,
      rotationYUp = 3012,
      rotationYDown = 3013,
      rotationZUp = 3014,
      rotationZDown = 3015,

      povN = 3020,
      povNE = 3021,
      povE = 3022,
      povSE = 3023,
      povS = 3024,
      povSW = 3025,
      povW = 3026,
      povNW = 3027,

      button1 = 3030,
      button2 = 3031,
      button3 = 3032,
      button4 = 3033,
      button5 = 3034,
      button6 = 3035,
      button7 = 3036,
      button8 = 3037,
      button9 = 3038,
      button10 = 3039,
      button11 = 3040,
      button12 = 3041,
      button13 = 3042,
      button14 = 3043,
      button15 = 3044,
      button16 = 3045,
      button17 = 3046,
      button18 = 3047,
      button19 = 3048,
      button20 = 3049,
    }

    #endregion Enumerations

    #region Variables

    private DeviceList _deviceList;
    private DirectInputListener _diListener;
    private MouseHandler _mouseHandler;
    private RemoteHandler _remoteHandler;

    private string _selectedDeviceGUID;

    #endregion Variables

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    public override string Name
    {
      get { return "Direct Input"; }
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
      get { return "and-81, with original MediaPortal code by waeberd"; }
    }

    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
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

    #region IConfigure Members

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

    #endregion

    #region IMouseReceiver Members

    /// <summary>
    /// Callback for mouse events.
    /// </summary>
    public MouseHandler MouseCallback
    {
      get { return _mouseHandler; }
      set { _mouseHandler = value; }
    }

    #endregion

    #region IRemoteReceiver Members

    /// <summary>
    /// Callback for remote button presses.
    /// </summary>
    public RemoteHandler RemoteCallback
    {
      get { return _remoteHandler; }
      set { _remoteHandler = value; }
    }

    #endregion

    /// <summary>
    /// Detect the presence of this device.
    /// </summary>
    public override DetectionResult Detect()
    {
      try
      {
        InitDeviceList();

        if (_deviceList.Count != 0)
        {
          return DetectionResult.DevicePresent;
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error("{0} exception: {1}", Name, ex.Message);
        return DetectionResult.DeviceException;
      }

      return DetectionResult.DeviceNotFound;
    }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    /// <returns>true if successful, otherwise false.</returns>
    public override void Start()
    {
      LoadSettings();

      InitDeviceList();

      if (_deviceList.Count == 0)
        throw new InvalidOperationException("No Direct Input devices connected");

      if (String.IsNullOrEmpty(_selectedDeviceGUID))
      {
#if TRACE
        Trace.WriteLine("No direct input device selected in plugin configuration, using first found");
#endif
        _deviceList.Reset(); // Move to the position before the first in the device list.
        _deviceList.MoveNext(); // Move to the first position in the device list.
        DeviceInstance di = (DeviceInstance) _deviceList.Current; // Retreive the first position in the device list.
        _selectedDeviceGUID = di.InstanceGuid.ToString();
      }

      _diListener = new DirectInputListener();
      _diListener.Delay = 150;
      _diListener.OnStateChange += diListener_OnStateChange;

      if (!AcquireDevice())
        throw new InvalidOperationException("Failed to acquire device");
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

        _diListener.OnStateChange -= diListener_OnStateChange;
        _diListener = null;
      }

      _deviceList = null;
    }


    private void LoadSettings()
    {
      XmlDocument doc = new XmlDocument();

      try
      {
        doc.Load(ConfigurationFile);
      }
      catch
      {
        return;
      }

      try
      {
        _selectedDeviceGUID = doc.DocumentElement.Attributes["DeviceGUID"].Value;
      }
      catch
      {
      }
    }

    private void SaveSettings()
    {
      try
      {
        using (XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, Encoding.UTF8))
        {
          writer.Formatting = Formatting.Indented;
          writer.Indentation = 1;
          writer.IndentChar = (char) 9;
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


    private void InitDeviceList()
    {
      _deviceList = Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly);
    }

    private void diListener_OnStateChange(object sender, JoystickState state)
    {
      SendActions(state);
    }

    private bool AcquireDevice()
    {
      if (_deviceList == null)
        return false;

      _deviceList.Reset();
      foreach (DeviceInstance di in _deviceList)
        if (_selectedDeviceGUID.Equals(di.InstanceGuid.ToString(), StringComparison.OrdinalIgnoreCase))
          return _diListener.InitDevice(di.InstanceGuid);

      return false;
    }

    private void SendActions(JoystickState state)
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
          case 0:
            actionCode = (int) joyButton.povN;
            break;
          case 4500:
            actionCode = (int) joyButton.povNE;
            break;
          case 9000:
            actionCode = (int) joyButton.povE;
            break;
          case 13500:
            actionCode = (int) joyButton.povSE;
            break;
          case 18000:
            actionCode = (int) joyButton.povS;
            break;
          case 22500:
            actionCode = (int) joyButton.povSW;
            break;
          case 27000:
            actionCode = (int) joyButton.povW;
            break;
          case 31500:
            actionCode = (int) joyButton.povNW;
            break;
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
            actionCode = (int) joyButton.axisXUp; // right
          }
          else
          {
            actionCode = (int) joyButton.axisXDown; // left
          }
        }
        else if (Math.Abs(state.Y) > AxisLimit)
        {
          //curAxisValue = state.Y;
          if (state.Y > 0)
          {
            // down
            actionCode = (int) joyButton.axisYUp;
          }
          else
          {
            // up
            actionCode = (int) joyButton.axisYDown;
          }
        }
        else if (Math.Abs(state.Z) > AxisLimit)
        {
          //curAxisValue = state.Z;
          if (state.Z > 0)
          {
            actionCode = (int) joyButton.axisZUp;
          }
          else
          {
            actionCode = (int) joyButton.axisZDown;
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
            actionCode = (int) joyButton.rotationXUp;
          }
          else
          {
            actionCode = (int) joyButton.rotationXDown;
          }
        }
        else if (Math.Abs(state.Ry) > AxisLimit)
        {
          //curAxisValue = state.Ry;
          if (state.Ry > 0)
          {
            actionCode = (int) joyButton.rotationYUp;
          }
          else
          {
            actionCode = (int) joyButton.rotationYDown;
          }
        }
        else if (Math.Abs(state.Rz) > AxisLimit)
        {
          //curAxisValue = state.Rz;
          if (state.Rz > 0)
          {
            actionCode = (int) joyButton.rotationZUp;
          }
          else
          {
            actionCode = (int) joyButton.rotationZDown;
          }
        }
      }

      if (actionCode != -1 && _remoteHandler != null)
      {
        string keyCode = TranslateActionCode(actionCode);

        _remoteHandler(Name, keyCode);
      }
    }

    private string TranslateActionCode(int actionCode)
    {
      joyButton j = (joyButton) actionCode;

      return Enum.GetName(typeof (joyButton), j);
    }
  }
}