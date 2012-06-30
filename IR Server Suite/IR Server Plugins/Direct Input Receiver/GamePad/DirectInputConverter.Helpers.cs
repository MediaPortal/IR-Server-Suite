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
using SlimDX.DirectInput;

namespace IRServer.Plugin.GamePad
{
  internal partial class DirectInputConverter
  {
    /// <summary>Returns the slider index from a value in the slider enumeration</summary>
    /// <param name="slider">Slider enumeration values whose index will be returned</param>
    /// <returns>The index of the specified slider enumeration value</returns>
    private static int indexFromSlider(Sliders slider)
    {
      switch (slider)
      {
        case Sliders.Slider1:
          {
            return 0;
          }
        case Sliders.Slider2:
          {
            return 1;
          }
        case Sliders.Velocity1:
          {
            return 2;
          }
        case Sliders.Velocity2:
          {
            return 3;
          }
        case Sliders.Acceleration1:
          {
            return 4;
          }
        case Sliders.Acceleration2:
          {
            return 5;
          }
        case Sliders.Force1:
          {
            return 6;
          }
        case Sliders.Force2:
          {
            return 7;
          }
        default:
          {
            return -1;
          }
      }
    }

    /// <summary>Returns the axis index from a value in the axis enumeration</summary>
    /// <param name="axis">Axis enumeration values whose index will be returned</param>
    /// <returns>The index of the specified axis enumeration value</returns>
    private static int indexFromAxis(Axes axis)
    {
      switch (axis)
      {
        case Axes.X:
          {
            return 0;
          }
        case Axes.Y:
          {
            return 1;
          }
        case Axes.Z:
          {
            return 2;
          }
        case Axes.VelocityX:
          {
            return 3;
          }
        case Axes.VelocityY:
          {
            return 4;
          }
        case Axes.VelocityZ:
          {
            return 5;
          }
        case Axes.AccelerationX:
          {
            return 6;
          }
        case Axes.AccelerationY:
          {
            return 7;
          }
        case Axes.AccelerationZ:
          {
            return 8;
          }
        case Axes.ForceX:
          {
            return 9;
          }
        case Axes.ForceY:
          {
            return 10;
          }
        case Axes.ForceZ:
          {
            return 11;
          }
        case Axes.RotationX:
          {
            return 12;
          }
        case Axes.RotationY:
          {
            return 13;
          }
        case Axes.RotationZ:
          {
            return 14;
          }
        case Axes.AngularVelocityX:
          {
            return 15;
          }
        case Axes.AngularVelocityY:
          {
            return 16;
          }
        case Axes.AngularVelocityZ:
          {
            return 17;
          }
        case Axes.AngularAccelerationX:
          {
            return 18;
          }
        case Axes.AngularAccelerationY:
          {
            return 19;
          }
        case Axes.AngularAccelerationZ:
          {
            return 20;
          }
        case Axes.TorqueX:
          {
            return 21;
          }
        case Axes.TorqueY:
          {
            return 22;
          }
        case Axes.TorqueZ:
          {
            return 23;
          }
        default:
          {
            return -1;
          }
      }
    }

    /// <summary>Creates an axis reader for the specified object</summary>
    /// <param name="joystick">Joystick providing the control object</param>
    /// <param name="axis">Axis a reader will be created for</param>
    /// <param name="control">Control description for the axis</param>
    /// <returns>A new axis reader for the specified axis</returns>
    private static IAxisReader createAxisReader(
      Joystick joystick, Axes axis, DeviceObjectInstance control
      )
    {
      int id = (int) control.ObjectType;
      ObjectProperties properties = joystick.GetObjectPropertiesById(id);

      int min = properties.LowerRange;
      int max = properties.UpperRange;

      switch (axis)
      {
        case Axes.X:
          {
            return new XAxisReader(min, max);
          }
        case Axes.Y:
          {
            return new YAxisReader(min, max);
          }
        case Axes.Z:
          {
            return new ZAxisReader(min, max);
          }
        case Axes.VelocityX:
          {
            return new VelocityXAxisReader(min, max);
          }
        case Axes.VelocityY:
          {
            return new VelocityYAxisReader(min, max);
          }
        case Axes.VelocityZ:
          {
            return new VelocityZAxisReader(min, max);
          }
        case Axes.AccelerationX:
          {
            return new AccelerationXAxisReader(min, max);
          }
        case Axes.AccelerationY:
          {
            return new AccelerationYAxisReader(min, max);
          }
        case Axes.AccelerationZ:
          {
            return new AccelerationZAxisReader(min, max);
          }
        case Axes.ForceX:
          {
            return new ForceXAxisReader(min, max);
          }
        case Axes.ForceY:
          {
            return new ForceYAxisReader(min, max);
          }
        case Axes.ForceZ:
          {
            return new ForceZAxisReader(min, max);
          }
        case Axes.RotationX:
          {
            return new RotationXAxisReader(min, max);
          }
        case Axes.RotationY:
          {
            return new RotationYAxisReader(min, max);
          }
        case Axes.RotationZ:
          {
            return new RotationZAxisReader(min, max);
          }
        case Axes.AngularVelocityX:
          {
            return new AngularVelocityXAxisReader(min, max);
          }
        case Axes.AngularVelocityY:
          {
            return new AngularVelocityYAxisReader(min, max);
          }
        case Axes.AngularVelocityZ:
          {
            return new AngularVelocityZAxisReader(min, max);
          }
        case Axes.AngularAccelerationX:
          {
            return new AngularAccelerationXAxisReader(min, max);
          }
        case Axes.AngularAccelerationY:
          {
            return new AngularAccelerationYAxisReader(min, max);
          }
        case Axes.AngularAccelerationZ:
          {
            return new AngularAccelerationZAxisReader(min, max);
          }
        case Axes.TorqueX:
          {
            return new TorqueXAxisReader(min, max);
          }
        case Axes.TorqueY:
          {
            return new TorqueYAxisReader(min, max);
          }
        case Axes.TorqueZ:
          {
            return new TorqueZAxisReader(min, max);
          }
        default:
          {
            return null;
          }
      }
    }

    /// <summary>Creates a slider reader for the specified object</summary>
    /// <param name="joystick">Joystick providing the control object</param>
    /// <param name="slider">Slider a reader will be created for</param>
    /// <param name="control">Control description for the axis</param>
    /// <returns>A new slider reader for the specified axis</returns>
    private static ISliderReader createSliderReader(
      Joystick joystick, Sliders slider, DeviceObjectInstance control
      )
    {
      int id = (int) control.ObjectType;
      ObjectProperties properties = joystick.GetObjectPropertiesById(id);

      int min = properties.LowerRange;
      int max = properties.UpperRange;

      switch (slider)
      {
        case Sliders.Slider1:
          {
            return new SliderReader(0, min, max);
          }
        case Sliders.Slider2:
          {
            return new SliderReader(1, min, max);
          }
        case Sliders.Velocity1:
          {
            return new VelocitySliderReader(0, min, max);
          }
        case Sliders.Velocity2:
          {
            return new VelocitySliderReader(1, min, max);
          }
        case Sliders.Acceleration1:
          {
            return new AccelerationSliderReader(0, min, max);
          }
        case Sliders.Acceleration2:
          {
            return new AccelerationSliderReader(1, min, max);
          }
        case Sliders.Force1:
          {
            return new ForceSliderReader(0, min, max);
          }
        case Sliders.Force2:
          {
            return new ForceSliderReader(1, min, max);
          }
        default:
          {
            return null;
          }
      }
    }

    /// <summary>Identifies the specified axis in the ExtendedAxes enumeration</summary>
    /// <param name="aspect">Aspect describing the order of the control</param>
    /// <param name="typeGuid">GUID describing the type of control</param>
    /// <returns>The equivalent entry in the ExtendedAxes enumeration or 0</returns>
    private static Axes identifyAxis(ObjectAspect aspect, Guid typeGuid)
    {
      Axes axis;

      if (typeGuid == ObjectGuid.XAxis)
      {
        axis = Axes.X;
      }
      else if (typeGuid == ObjectGuid.YAxis)
      {
        axis = Axes.Y;
      }
      else if (typeGuid == ObjectGuid.ZAxis)
      {
        axis = Axes.Z;
      }
      else if (typeGuid == ObjectGuid.RotationalXAxis)
      {
        axis = Axes.RotationX;
      }
      else if (typeGuid == ObjectGuid.RotationalYAxis)
      {
        axis = Axes.RotationY;
      }
      else if (typeGuid == ObjectGuid.RotationalZAxis)
      {
        axis = Axes.RotationZ;
      }
      else
      {
        return 0;
      }

      if ((aspect & ObjectAspect.Acceleration) == ObjectAspect.Acceleration)
      {
        if (axis == Axes.X)
        {
          return Axes.AccelerationX;
        }
        if (axis == Axes.Y)
        {
          return Axes.AccelerationY;
        }
        if (axis == Axes.Z)
        {
          return Axes.AccelerationZ;
        }
        if (axis == Axes.RotationX)
        {
          return Axes.AngularAccelerationX;
        }
        if (axis == Axes.RotationY)
        {
          return Axes.AngularAccelerationY;
        }
        if (axis == Axes.RotationZ)
        {
          return Axes.AngularAccelerationZ;
        }
      }
      else if ((aspect & ObjectAspect.Velocity) == ObjectAspect.Velocity)
      {
        if (axis == Axes.X)
        {
          return Axes.VelocityX;
        }
        if (axis == Axes.Y)
        {
          return Axes.VelocityY;
        }
        if (axis == Axes.Z)
        {
          return Axes.VelocityZ;
        }
        if (axis == Axes.RotationX)
        {
          return Axes.AngularVelocityX;
        }
        if (axis == Axes.RotationY)
        {
          return Axes.AngularVelocityY;
        }
        if (axis == Axes.RotationZ)
        {
          return Axes.AngularVelocityZ;
        }
      }
      else if ((aspect & ObjectAspect.Force) == ObjectAspect.Force)
      {
        if (axis == Axes.X)
        {
          return Axes.ForceX;
        }
        if (axis == Axes.Y)
        {
          return Axes.ForceY;
        }
        if (axis == Axes.Z)
        {
          return Axes.ForceZ;
        }
        if (axis == Axes.RotationX)
        {
          return Axes.TorqueX;
        }
        if (axis == Axes.RotationY)
        {
          return Axes.TorqueY;
        }
        if (axis == Axes.RotationZ)
        {
          return Axes.TorqueZ;
        }
      }
      else if ((aspect & ObjectAspect.Position) == ObjectAspect.Position)
      {
        return axis;
      }

      return 0;
    }

    /// <summary>Identifies the specified slider in the ExtendedSliders enumeration</summary>
    /// <param name="aspect">Aspect describing the order of the control</param>
    /// <param name="typeGuid">GUID describing the type of control</param>
    /// <returns>The equivalent entry in the ExtendedSliders enumeration or 0</returns>
    private static Sliders identifySlider(ObjectAspect aspect, Guid typeGuid)
    {
      if (typeGuid == ObjectGuid.Slider)
      {
        if ((aspect & ObjectAspect.Acceleration) == ObjectAspect.Acceleration)
        {
          return Sliders.Acceleration1;
        }
        else if ((aspect & ObjectAspect.Velocity) == ObjectAspect.Velocity)
        {
          return Sliders.Velocity1;
        }
        else if ((aspect & ObjectAspect.Force) == ObjectAspect.Force)
        {
          return Sliders.Force1;
        }
        else if ((aspect & ObjectAspect.Position) == ObjectAspect.Position)
        {
          return Sliders.Slider1;
        }
      }

      return 0;
    }
  }
}