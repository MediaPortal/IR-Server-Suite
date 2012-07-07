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
using System.Collections.Generic;
using System.IO;
using System.Management;
using IRServer.Plugin.GamePad;
using IRServer.Plugin.GamePad.Enums;
using IrssUtils;
using SlimDX.DirectInput;
using System.Windows.Forms;

namespace IRServer.Plugin
{
  /// <summary>
  /// IR Server Plugin for Direct Input game controllers.
  /// </summary>
  public partial class DirectInputReceiver
  {
#if DEBUG

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

#endif

    #region Constants

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

    private IList<DeviceInstance> _deviceList;
    private DirectInputListener _diListener;

    private Config _config;

    #endregion Variables

    private void RegisterDeviceNotification()
    {
      IrssLog.Debug("DirectInput: Registering device notifications...");
      WqlEventQuery _q = new WqlEventQuery("__InstanceOperationEvent", "TargetInstance ISA 'Win32_USBControllerDevice' ");
      _q.WithinInterval = TimeSpan.FromSeconds(1);
      ManagementEventWatcher _w = new ManagementEventWatcher(_q);
      _w.EventArrived += new EventArrivedEventHandler(onEventArrived);
      _w.Start();
    }

    private void onEventArrived(object sender, EventArrivedEventArgs e)
    {
      ManagementBaseObject _o = e.NewEvent["TargetInstance"] as ManagementBaseObject;
      if (_o == null) return;

      ManagementObject mo = new ManagementObject(_o["Dependent"].ToString());
      if (mo == null) return;

      IList<DeviceInstance> newDeviceList = new DirectInput().GetDevices(DeviceClass.GameController,
                                                                         DeviceEnumerationFlags.AttachedOnly);
      if (_deviceList != null && _deviceList.Count == newDeviceList.Count) return;

      try
      {
        if (mo.GetPropertyValue("DeviceID").ToString() != string.Empty)
        {
          //connected
          IrssLog.Debug("DirectInput: An USB device has been connected");
          RestartListener();
        }
      }
      catch (ManagementException ex)
      {
        //disconnected
        IrssLog.Debug("DirectInput: An USB device has been disconnected");
        if (newDeviceList.Count == 0)
          StopListener();
        else
          RestartListener();
      }
    }

    private void RestartListener()
    {
      StopListener();
      StartListener();
    }

    private void StartListener()
    {
      InitDeviceList();
      if (_deviceList.Count == 0) return;

      IrssLog.Debug("DirectInput: Start listening...");


      if (String.IsNullOrEmpty(_config.DeviceGUID))
      {
        IrssLog.Info("No direct input device selected in plugin configuration, using first found");
        // Retreive the first position in the device list.
        _config.DeviceGUID = _deviceList[0].InstanceGuid.ToString();
      }

      _diListener = new DirectInputListener();
      _diListener.Delay = 150;
      _diListener.OnStateChange += diListener_OnStateChange;

      if (!AcquireDevice())
        IrssLog.Warn("Failed to acquire device");
    }

    private void StopListener()
    {
      IrssLog.Debug("DirectInput: Stop listening...");

      if (_diListener != null)
      {
        _diListener.DeInitDevice();
        _diListener.StopListener();

        _diListener.OnStateChange -= diListener_OnStateChange;
        _diListener = null;
      }

      _deviceList = null;
    }


    private void InitDeviceList()
    {
      DirectInput di = new DirectInput();
      _deviceList = di.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly);
    }

    private void diListener_OnStateChange(object sender, JoystickState state)
    {
      SendActions(state);
    }

    private bool AcquireDevice()
    {
      if (_deviceList == null)
        return false;

      foreach (DeviceInstance di in _deviceList)
        if (_config.DeviceGUID.Equals(di.InstanceGuid.ToString(), StringComparison.OrdinalIgnoreCase))
          return _diListener.InitDevice(di.InstanceGuid);

      return false;
    }

    private void SendActions(JoystickState state)
    {
      int actionCode = -1;
      GamePadState betterState = new GamePadState(_diListener.Converter, ref state);

      // buttons
      if (actionCode == -1)
      {
        int button = -1;
        for (int i = 0; i < betterState.ButtonCount; i++)
        {
          if (betterState.IsButtonDown(i))
          {
            button = i;
            break;
          }
        }

        if (button != -1)
        {
          if ((button >= 0) && (button <= 19))
          {
            // don't need no stinkin' enum-constants here....
            actionCode = 3030 + button;
          }
        }
      }

      // pov
      if (actionCode == -1)
      {
        for (int i = 0; i < betterState.PovCount; i++)
        {
          switch (betterState.GetPov(i))
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

          if (actionCode != -1) break;
        }
      }

      // axes
      if (actionCode == -1)
      {
        if (betterState.AvailableAxes.HasFlag(Axes.X) &&
            Math.Abs(betterState.GetAxis(Axes.X)) > _config.AxisLimit)
        {
          if (betterState.GetAxis(Axes.X) > 0)
            actionCode = (int) joyButton.axisXUp; // right
          else
            actionCode = (int) joyButton.axisXDown; // left
        }
        else if (betterState.AvailableAxes.HasFlag(Axes.Y) &&
                 Math.Abs(betterState.GetAxis(Axes.Y)) > _config.AxisLimit)
        {
          if (betterState.GetAxis(Axes.Y) > 0)
            actionCode = (int) joyButton.axisYUp; // down
          else
            actionCode = (int) joyButton.axisYDown; // up
        }
        else if (betterState.AvailableAxes.HasFlag(Axes.Z) &&
                 Math.Abs(betterState.GetAxis(Axes.Z)) > _config.AxisLimit)
        {
          if (betterState.GetAxis(Axes.Z) > 0)
            actionCode = (int) joyButton.axisZUp;
          else
            actionCode = (int) joyButton.axisZDown;
        }
        else if (betterState.AvailableAxes.HasFlag(Axes.RotationX) &&
                 Math.Abs(betterState.GetAxis(Axes.RotationX)) > _config.AxisLimit)
        {
          if (betterState.GetAxis(Axes.RotationX) > 0)
            actionCode = (int) joyButton.rotationXUp;
          else
            actionCode = (int) joyButton.rotationXDown;
        }
        else if (betterState.AvailableAxes.HasFlag(Axes.RotationY) &&
                 Math.Abs(betterState.GetAxis(Axes.RotationY)) > _config.AxisLimit)
        {
          if (betterState.GetAxis(Axes.RotationY) > 0)
            actionCode = (int) joyButton.rotationYUp;
          else
            actionCode = (int) joyButton.rotationYDown;
        }
        else if (betterState.AvailableAxes.HasFlag(Axes.RotationZ) &&
                 Math.Abs(betterState.GetAxis(Axes.RotationZ)) > _config.AxisLimit)
        {
          if (betterState.GetAxis(Axes.RotationZ) > 0)
            actionCode = (int) joyButton.rotationZUp;
          else
            actionCode = (int) joyButton.rotationZDown;
        }
      }

      if (actionCode != -1 && RemoteCallback != null)
      {
        string keyCode = TranslateActionCode(actionCode);
        IrssLog.Debug("DirectInput action mapped actionCode={0} to keyCode={1}", actionCode, keyCode);

        RemoteCallback(Name, keyCode);
      }
    }

    private string TranslateActionCode(int actionCode)
    {
      joyButton j = (joyButton) actionCode;

      return Enum.GetName(typeof (joyButton), j);
    }
  }
}