#region CPL License

/*
Nuclex Framework
Copyright (C) 2002-2011 Nuclex Development Labs

This library is free software; you can redistribute it and/or
modify it under the terms of the IBM Common Public License as
published by the IBM Corporation; either version 1.0 of the
License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
IBM Common Public License for more details.

You should have received a copy of the IBM Common Public
License along with this library
*/

#endregion

using System;
using IRServer.Plugin.GamePad.Enums;

namespace IRServer.Plugin.GamePad
{
  /// <summary>Game pad state with additional buttons and axes</summary>
  public partial struct GamePadState
  {
    /// <summary>Retrieves the state of the specified button</summary>
    /// <param name="buttonIndex">
    ///   Index of the button whose state will be retrieved
    /// </param>
    /// <returns>The state of the queried button</returns>
    public ButtonState GetButton(int buttonIndex)
    {
      return IsButtonDown(buttonIndex) ? ButtonState.Pressed : ButtonState.Released;
    }

    /// <summary>Determines whether the specified button is pressed down</summary>
    /// <param name="buttonIndex">Button which will be checked</param>
    /// <returns>True if the specified button is pressed down</returns>
    public bool IsButtonDown(int buttonIndex)
    {
      if (buttonIndex >= 128)
      {
        throw new ArgumentOutOfRangeException("buttonIndex", "Button index out of range");
      }
      else if (buttonIndex >= 64)
      {
        return (this.buttonState2 & (1UL << (buttonIndex - 64))) != 0;
      }
      else if (buttonIndex >= 0)
      {
        return (this.buttonState1 & (1UL << buttonIndex)) != 0;
      }
      else
      {
        throw new ArgumentOutOfRangeException("buttonIndex", "Button index out of range");
      }
    }

    /// <summary>Determines whether the specified button is up</summary>
    /// <param name="buttonIndex">Button which will be checked</param>
    /// <returns>True if the specified button is up</returns>
    public bool IsButtonUp(int buttonIndex)
    {
      if (buttonIndex >= 128)
      {
        throw new ArgumentOutOfRangeException("buttonIndex", "Button index out of range");
      }
      else if (buttonIndex >= 64)
      {
        return (this.buttonState2 & (1UL << (buttonIndex - 64))) == 0;
      }
      else if (buttonIndex >= 0)
      {
        return (this.buttonState1 & (1UL << buttonIndex)) == 0;
      }
      else
      {
        throw new ArgumentOutOfRangeException("buttonIndex", "Button index out of range");
      }
    }

    /// <summary>Number of available axes in this state</summary>
    public int AxisCount
    {
      get { return countBits((uint) this.AvailableAxes); }
    }

    /// <summary>Retrieves the state of the specified axis</summary>
    /// <param name="axis">Axis whose state will be retrieved</param>
    /// <returns>The state of the specified axis</returns>
    public float GetAxis(Axes axis)
    {
      switch (axis)
      {
        case Axes.X:
          {
            return this.X;
          }
        case Axes.Y:
          {
            return this.Y;
          }
        case Axes.Z:
          {
            return this.Z;
          }
        case Axes.VelocityX:
          {
            return this.VelocityX;
          }
        case Axes.VelocityY:
          {
            return this.VelocityY;
          }
        case Axes.VelocityZ:
          {
            return this.VelocityZ;
          }
        case Axes.AccelerationX:
          {
            return this.AccelerationX;
          }
        case Axes.AccelerationY:
          {
            return this.AccelerationY;
          }
        case Axes.AccelerationZ:
          {
            return this.AccelerationZ;
          }
        case Axes.ForceX:
          {
            return this.ForceX;
          }
        case Axes.ForceY:
          {
            return this.ForceY;
          }
        case Axes.ForceZ:
          {
            return this.ForceZ;
          }
        case Axes.RotationX:
          {
            return this.RotationX;
          }
        case Axes.RotationY:
          {
            return this.RotationY;
          }
        case Axes.RotationZ:
          {
            return this.RotationZ;
          }
        case Axes.AngularVelocityX:
          {
            return this.AngularVelocityX;
          }
        case Axes.AngularVelocityY:
          {
            return this.AngularVelocityY;
          }
        case Axes.AngularVelocityZ:
          {
            return this.AngularVelocityZ;
          }
        case Axes.AngularAccelerationX:
          {
            return this.AngularAccelerationX;
          }
        case Axes.AngularAccelerationY:
          {
            return this.AngularAccelerationY;
          }
        case Axes.AngularAccelerationZ:
          {
            return this.AngularAccelerationZ;
          }
        case Axes.TorqueX:
          {
            return this.TorqueX;
          }
        case Axes.TorqueY:
          {
            return this.TorqueY;
          }
        case Axes.TorqueZ:
          {
            return this.TorqueZ;
          }
        default:
          {
            throw new ArgumentOutOfRangeException("axis", "Invalid axis");
          }
      }
    }

    /// <summary>Number of available sliders in this state</summary>
    public int SliderCount
    {
      get { return countBits((uint) this.AvailableSliders); }
    }

    /// <summary>Retrieves the state of the specified slider</summary>
    /// <param name="slider">Slider whose state will be retrieved</param>
    /// <returns>The state of the specified slider</returns>
    public float GetSlider(Sliders slider)
    {
      switch (slider)
      {
        case Sliders.Slider1:
          {
            return this.Slider1;
          }
        case Sliders.Slider2:
          {
            return this.Slider2;
          }
        case Sliders.Velocity1:
          {
            return this.VelocitySlider1;
          }
        case Sliders.Velocity2:
          {
            return this.VelocitySlider2;
          }
        case Sliders.Acceleration1:
          {
            return this.AccelerationSlider1;
          }
        case Sliders.Acceleration2:
          {
            return this.AccelerationSlider2;
          }
        case Sliders.Force1:
          {
            return this.ForceSlider1;
          }
        case Sliders.Force2:
          {
            return this.ForceSlider2;
          }
        default:
          {
            throw new ArgumentOutOfRangeException("slider", "Invalid slider");
          }
      }
    }

    /// <summary>Retrieves the PoV controller of the specified index</summary>
    /// <param name="index">Index of the PoV controller that will be retrieved</param>
    /// <returns>The state of the PoV controller with the specified index</returns>
    public int GetPov(int index)
    {
      switch (index)
      {
        case 0:
          {
            return this.Pov1;
          }
        case 1:
          {
            return this.Pov2;
          }
        case 2:
          {
            return this.Pov3;
          }
        case 3:
          {
            return this.Pov4;
          }
        default:
          {
            throw new ArgumentOutOfRangeException("index", "PoV index out of range");
          }
      }
    }

    /// <summary>Internal helper method that retrieves the raw button states</summary>
    /// <param name="buttons1">State of the first 64 buttons</param>
    /// <param name="buttons2">State of the second 64 buttons</param>
    internal void InternalGetButtons(out ulong buttons1, out ulong buttons2)
    {
      buttons1 = this.buttonState1;
      buttons2 = this.buttonState2;
    }

    /// <summary>Returns the number of bits set in an unsigned integer</summary>
    /// <param name="value">Value whose bits will be counted</param>
    /// <returns>The number of bits set in the unsigned integer</returns>
    /// <remarks>
    ///   Based on a trick revealed here:
    ///   http://stackoverflow.com/questions/109023
    /// </remarks>
    private static int countBits(uint value)
    {
      value = value - ((value >> 1) & 0x55555555);
      value = (value & 0x33333333) + ((value >> 2) & 0x33333333);

      return (int) unchecked(
                     ((value + (value >> 4) & 0xF0F0F0F)*0x1010101) >> 24
                     );
    }

    /// <summary>Axes for which this state provides values</summary>
    public readonly Axes AvailableAxes;

    /// <summary>State of the device's X axis</summary>
    public readonly float X;

    /// <summary>State of the device's Y axis</summary>
    public readonly float Y;

    /// <summary>State of the device's Z axis</summary>
    public readonly float Z;

    /// <summary>State of the device's X velocity axis</summary>
    public readonly float VelocityX;

    /// <summary>State of the device's Y velocity axis</summary>
    public readonly float VelocityY;

    /// <summary>State of the device's Z velocity axis</summary>
    public readonly float VelocityZ;

    /// <summary>State of the device's X acceleration axis</summary>
    public readonly float AccelerationX;

    /// <summary>State of the device's Y acceleration axis</summary>
    public readonly float AccelerationY;

    /// <summary>State of the device's Z acceleration axis</summary>
    public readonly float AccelerationZ;

    /// <summary>State of the device's X force axis</summary>
    public readonly float ForceX;

    /// <summary>State of the device's Y force axis</summary>
    public readonly float ForceY;

    /// <summary>State of the device's Z force axis</summary>
    public readonly float ForceZ;

    /// <summary>State of the device's X rotation axis</summary>
    public readonly float RotationX;

    /// <summary>State of the device's Y rotation axis</summary>
    public readonly float RotationY;

    /// <summary>State of the device's Z rotation axis</summary>
    public readonly float RotationZ;

    /// <summary>State of the device's X angular velocity axis</summary>
    public readonly float AngularVelocityX;

    /// <summary>State of the device's Y angular velocity axis</summary>
    public readonly float AngularVelocityY;

    /// <summary>State of the device's Z angular velocity axis</summary>
    public readonly float AngularVelocityZ;

    /// <summary>State of the device's X angular acceleration axis</summary>
    public readonly float AngularAccelerationX;

    /// <summary>State of the device's Y angular acceleration axis</summary>
    public readonly float AngularAccelerationY;

    /// <summary>State of the device's Z angular acceleration axis</summary>
    public readonly float AngularAccelerationZ;

    /// <summary>State of the device's X torque axis</summary>
    public readonly float TorqueX;

    /// <summary>State of the device's Y torque axis</summary>
    public readonly float TorqueY;

    /// <summary>State of the device's Z torque axis</summary>
    public readonly float TorqueZ;

    /// <summary>Number of buttons provided by the state</summary>
    public readonly int ButtonCount;

    /// <summary>Sliders for which this state provides values</summary>
    public readonly Sliders AvailableSliders;

    /// <summary>First slider, formerly the U-axis</summary>
    public readonly float Slider1;

    /// <summary>Second slider, formerly the V-axis</summary>
    public readonly float Slider2;

    /// <summary>First velocity slider</summary>
    public readonly float VelocitySlider1;

    /// <summary>second velocity slider</summary>
    public readonly float VelocitySlider2;

    /// <summary>First acceleration slider</summary>
    public readonly float AccelerationSlider1;

    /// <summary>Second acceleration slider</summary>
    public readonly float AccelerationSlider2;

    /// <summary>First force slider</summary>
    public readonly float ForceSlider1;

    /// <summary>Second force slider</summary>
    public readonly float ForceSlider2;

    /// <summary>Number of point-of-view controllers in this state</summary>
    public readonly int PovCount;

    /// <summary>Position of the first point-of-view controller</summary>
    public readonly int Pov1;

    /// <summary>Position of the second point-of-view controller</summary>
    public readonly int Pov2;

    /// <summary>Position of the third point-of-view controller</summary>
    public readonly int Pov3;

    /// <summary>Position of the fourth point-of-view controller</summary>
    public readonly int Pov4;

    /// <summary>Bitfield containing the first 64 buttons</summary>
    private ulong buttonState1;

    /// <summary>Bitfield containing the last 64 buttons</summary>
    private ulong buttonState2;
  }
}

// namespace Nuclex.Input.Devices