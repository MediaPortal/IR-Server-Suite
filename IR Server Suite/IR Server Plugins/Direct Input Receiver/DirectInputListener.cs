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
using System.Threading;
using IRServer.Plugin.GamePad;
using SlimDX.DirectInput;

namespace IRServer.Plugin
{
  /// <summary>
  /// Summary description for DirectInputListener.
  /// </summary>
  /// 
  internal class DirectInputListener
  {
    #region Variables

    #region Delegates

    public delegate void diStateChange(object sender, JoystickState state);

    #endregion

    private int delay = 150; // sleep time in milliseconds

    private Joystick device;
    private Thread inputListener;
    private volatile bool isListening;

    public event diStateChange OnStateChange = null;

    #endregion Variables

    #region Constructor / Deconstructor

    ~DirectInputListener()
    {
      StopListener();
      DeInitDevice();
    }

    #endregion Constructor / Deconstructor

    #region Properties

    public Joystick SelectedDevice
    {
      get { return device; }
    }

    public int Delay
    {
      get { return delay; }
      set { delay = value; }
    }

    public DirectInputConverter Converter
    {
      get
      {
        if (SelectedDevice != null)
          return new DirectInputConverter(SelectedDevice);

        return null;
      }
    }

    #endregion Properties

    public bool InitDevice(Guid guid)
    {
      device = new Joystick(new DirectInput(), guid);
      device.SetCooperativeLevel(new System.Windows.Forms.NativeWindow().Handle,
                                 CooperativeLevel.Background | CooperativeLevel.Nonexclusive);
      device.Properties.AxisMode = DeviceAxisMode.Absolute;

      // Enumerate axes
      foreach (DeviceObjectInstance doi in device.GetObjects())
      {
        if (doi.ObjectType.HasFlag(ObjectDeviceType.Axis))
        {
          // We found an axis, set the range to a max of 10,000
          device.Properties.SetRange(-5000, 5000);
        }
      }

      StopListener();
      StartListener();
      device.Acquire();

      return true;
    }

    public void DeInitDevice()
    {
      if (null != device)
      {
        try
        {
          device.Unacquire();
        }
        catch (NullReferenceException)
        {
        }
        device.Dispose();
        device = null;
      }
    }

    //public string GetCurrentButtonCombo()
    //{
    //  if (CheckDevice())
    //  {
    //    // Get the state of the device.
    //    try
    //    {
    //      JoystickState state = device.GetCurrentState();
    //      return ButtonComboAsString(state);
    //    }
    //      // Catch any exceptions. None will be handled here, 
    //      // any device re-aquisition will be handled above.  
    //    catch (DirectInputException)
    //    {
    //    }
    //  }

    //  return string.Empty;
    //}

    //private string ButtonComboAsString(JoystickState state)
    //{
    //  bool[] buttons = state.GetButtons();
    //  int button = 0;
    //  string res = "";

    //  // button combos
    //  string sep = "";
    //  foreach (bool b in buttons)
    //  {
    //    if (b)
    //    {
    //      res += sep + button.ToString("00");
    //      sep = ",";
    //    }
    //    button++;
    //  }
    //  return res;
    //}

    private void ThreadFunction()
    {
      while (isListening)
      {
        UpdateInputState();
        Thread.Sleep(delay);
      }
    }


    public bool CheckDevice()
    {
      if (device == null) return false;

      try
      {
        // Poll the device for info.
        device.Poll();
      }
      catch (DirectInputException inputex)
      {
        Debug.WriteLine(inputex);
        //if ((inputex is NotAcquiredException) || (inputex is InputLostException))
        //{
        // Check to see if either the app
        // needs to acquire the device, or
        // if the app lost the device to another
        // process.
        try
        {
          // Acquire the device.
          device.Acquire();
        }
        catch (DirectInputException ex)
        {
          // Failed to acquire the device.
          // This could be because the app
          // doesn't have focus.
          Debug.WriteLine(ex);
          return false;
        }
        //}
      }

      return (device != null);
    }

    private void UpdateInputState()
    {
      JoystickState state;

      if (CheckDevice())
      {
        // Get the state of the device.
        try
        {
          state = device.GetCurrentState();
        }
        catch (DirectInputException)
          // Catch any exceptions. None will be handled here, any device re-aquisition will be handled above.
        {
          return;
        }

        // send events here
        if (null != OnStateChange)
        {
          OnStateChange(this, state);
        }
      }
    }


    public void StopListener()
    {
      if (inputListener != null)
        return;

      isListening = false;

      Thread.Sleep(500);

      if (inputListener != null && inputListener.IsAlive)
        inputListener.Abort();

      inputListener = null;
    }

    private void StartListener()
    {
      isListening = true;

      inputListener = new Thread(ThreadFunction);
      inputListener.Name = "DirectInputListener";
      inputListener.IsBackground = true;
      inputListener.Start();
    }
  }
}