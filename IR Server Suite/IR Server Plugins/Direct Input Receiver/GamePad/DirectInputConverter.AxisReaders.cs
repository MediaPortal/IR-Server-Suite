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

using SlimDX.DirectInput;

namespace IRServer.Plugin.GamePad
{
  internal partial class DirectInputConverter
  {
    #region interface IAxisReader

    /// <summary>Reads the state of an axis or compares states</summary>
    internal interface IAxisReader
    {
      /// <summary>Retrieves the current value of the axis</summary>
      /// <param name="state">Joystick state the axis is taken from</param>
      /// <returns>The value of the axis in the joystick state</returns>
      float GetValue(ref JoystickState state);
    }

    #endregion // interface IAxisReader

    #region class AxisReader

    /// <summary>Reads the state of an axis and normalizes it</summary>
    private abstract class AxisReader : IAxisReader
    {
      /// <summary>Initializes a new axis reader</summary>
      /// <param name="min">Negative range of the axis</param>
      /// <param name="max">Positive range of the axis</param>
      public AxisReader(int min, int max)
      {
        this.center = (min + max)/2;
        this.min = (float) (min - this.center);
        this.max = (float) (max - this.center);
      }

      /// <summary>Retrieves the current value of the axis</summary>
      /// <param name="state">Joystick state the axis is taken from</param>
      /// <returns>The value of the axis in the joystick state</returns>
      public float GetValue(ref JoystickState state)
      {
        int raw = Read(ref state);

        if (raw < this.center)
        {
          return (float) (this.center - raw)/this.min;
        }
        else
        {
          return (float) (raw - this.center)/this.max;
        }
      }

      /// <summary>Reads the raw value from the joystick state</summary>
      /// <param name="state">Joystick state the value is read from</param>
      /// <returns>The raw value of the axis in the joystick state</returns>
      protected abstract int Read(ref JoystickState state);

      /// <summary>The centered position of the axis</summary>
      private int center;

      /// <summary>Positive range of the axis</summary>
      private float min;

      /// <summary>Negative range of the axis</summary>
      private float max;
    }

    #endregion // class AxisReader

    #region Position axis readers

    private class XAxisReader : AxisReader
    {
      public XAxisReader(int min, int max) : base(min, max)
      {
      }

      protected override int Read(ref JoystickState state)
      {
        return state.X;
      }
    }

    private class YAxisReader : AxisReader
    {
      public YAxisReader(int min, int max) : base(min, max)
      {
      }

      protected override int Read(ref JoystickState state)
      {
        return state.Y;
      }
    }

    private class ZAxisReader : AxisReader
    {
      public ZAxisReader(int min, int max) : base(min, max)
      {
      }

      protected override int Read(ref JoystickState state)
      {
        return state.Z;
      }
    }

    #endregion // Position axis readers

    #region Velocity axis readers

    private class VelocityXAxisReader : AxisReader
    {
      public VelocityXAxisReader(int min, int max) : base(min, max)
      {
      }

      protected override int Read(ref JoystickState state)
      {
        return state.VelocityX;
      }
    }

    private class VelocityYAxisReader : AxisReader
    {
      public VelocityYAxisReader(int min, int max) : base(min, max)
      {
      }

      protected override int Read(ref JoystickState state)
      {
        return state.VelocityY;
      }
    }

    private class VelocityZAxisReader : AxisReader
    {
      public VelocityZAxisReader(int min, int max) : base(min, max)
      {
      }

      protected override int Read(ref JoystickState state)
      {
        return state.VelocityZ;
      }
    }

    #endregion // Velocity axis readers

    #region Acceleration axis readers

    private class AccelerationXAxisReader : AxisReader
    {
      public AccelerationXAxisReader(int min, int max) : base(min, max)
      {
      }

      protected override int Read(ref JoystickState state)
      {
        return state.AccelerationX;
      }
    }

    private class AccelerationYAxisReader : AxisReader
    {
      public AccelerationYAxisReader(int min, int max) : base(min, max)
      {
      }

      protected override int Read(ref JoystickState state)
      {
        return state.AccelerationY;
      }
    }

    private class AccelerationZAxisReader : AxisReader
    {
      public AccelerationZAxisReader(int min, int max) : base(min, max)
      {
      }

      protected override int Read(ref JoystickState state)
      {
        return state.AccelerationZ;
      }
    }

    #endregion // Acceleration axis readers

    #region Force axis readers

    private class ForceXAxisReader : AxisReader
    {
      public ForceXAxisReader(int min, int max) : base(min, max)
      {
      }

      protected override int Read(ref JoystickState state)
      {
        return state.ForceX;
      }
    }

    private class ForceYAxisReader : AxisReader
    {
      public ForceYAxisReader(int min, int max) : base(min, max)
      {
      }

      protected override int Read(ref JoystickState state)
      {
        return state.ForceY;
      }
    }

    private class ForceZAxisReader : AxisReader
    {
      public ForceZAxisReader(int min, int max) : base(min, max)
      {
      }

      protected override int Read(ref JoystickState state)
      {
        return state.ForceZ;
      }
    }

    #endregion // Force axis readers

    #region Rotation axis readers

    private class RotationXAxisReader : AxisReader
    {
      public RotationXAxisReader(int min, int max) : base(min, max)
      {
      }

      protected override int Read(ref JoystickState state)
      {
        return state.RotationX;
      }
    }

    private class RotationYAxisReader : AxisReader
    {
      public RotationYAxisReader(int min, int max) : base(min, max)
      {
      }

      protected override int Read(ref JoystickState state)
      {
        return state.RotationY;
      }
    }

    private class RotationZAxisReader : AxisReader
    {
      public RotationZAxisReader(int min, int max) : base(min, max)
      {
      }

      protected override int Read(ref JoystickState state)
      {
        return state.RotationZ;
      }
    }

    #endregion // Rotation axis readers

    #region Angular velocity axis readers

    private class AngularVelocityXAxisReader : AxisReader
    {
      public AngularVelocityXAxisReader(int min, int max) : base(min, max)
      {
      }

      protected override int Read(ref JoystickState state)
      {
        return state.AngularVelocityX;
      }
    }

    private class AngularVelocityYAxisReader : AxisReader
    {
      public AngularVelocityYAxisReader(int min, int max) : base(min, max)
      {
      }

      protected override int Read(ref JoystickState state)
      {
        return state.AngularVelocityY;
      }
    }

    private class AngularVelocityZAxisReader : AxisReader
    {
      public AngularVelocityZAxisReader(int min, int max) : base(min, max)
      {
      }

      protected override int Read(ref JoystickState state)
      {
        return state.AngularVelocityZ;
      }
    }

    #endregion // Angular velocity axis readers

    #region Angular acceleration axis readers

    private class AngularAccelerationXAxisReader : AxisReader
    {
      public AngularAccelerationXAxisReader(int min, int max) : base(min, max)
      {
      }

      protected override int Read(ref JoystickState state)
      {
        return state.AngularAccelerationX;
      }
    }

    private class AngularAccelerationYAxisReader : AxisReader
    {
      public AngularAccelerationYAxisReader(int min, int max) : base(min, max)
      {
      }

      protected override int Read(ref JoystickState state)
      {
        return state.AngularAccelerationY;
      }
    }

    private class AngularAccelerationZAxisReader : AxisReader
    {
      public AngularAccelerationZAxisReader(int min, int max) : base(min, max)
      {
      }

      protected override int Read(ref JoystickState state)
      {
        return state.AngularAccelerationZ;
      }
    }

    #endregion // Angular acceleration axis readers

    #region Torque axis readers

    private class TorqueXAxisReader : AxisReader
    {
      public TorqueXAxisReader(int min, int max) : base(min, max)
      {
      }

      protected override int Read(ref JoystickState state)
      {
        return state.TorqueX;
      }
    }

    private class TorqueYAxisReader : AxisReader
    {
      public TorqueYAxisReader(int min, int max) : base(min, max)
      {
      }

      protected override int Read(ref JoystickState state)
      {
        return state.TorqueY;
      }
    }

    private class TorqueZAxisReader : AxisReader
    {
      public TorqueZAxisReader(int min, int max) : base(min, max)
      {
      }

      protected override int Read(ref JoystickState state)
      {
        return state.TorqueZ;
      }
    }

    #endregion // Torque axis readers
  }
}