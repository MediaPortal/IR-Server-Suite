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
    #region interface ISliderReader

    /// <summary>Reads the state of a slider or compares states</summary>
    internal interface ISliderReader
    {
      /// <summary>Retrieves the current value of the slider</summary>
      /// <param name="state">Joystick state the slider is taken from</param>
      /// <returns>The value of the slider in the joystick state</returns>
      float GetValue(ref JoystickState state);
    }

    #endregion // interface ISliderReader

    #region class SliderReader

    /// <summary>Reads the state of a slider and normalized it</summary>
    private class SliderReader : ISliderReader
    {
      /// <summary>Initializes a new slider reader</summary>
      /// <param name="index">Index of the slider in the array</param>
      /// <param name="min">Negative range of the slider</param>
      /// <param name="max">Positive range of the slider</param>
      public SliderReader(int index, int min, int max)
      {
        this.Index = index;
        this.min = min;
        this.range = (float) (max - min);
      }

      /// <summary>Retrieves the current value of the axis</summary>
      /// <param name="state">Joystick state the axis is taken from</param>
      /// <returns>The value of the axis in the joystick state</returns>
      public float GetValue(ref JoystickState state)
      {
        int raw = Read(ref state);

        return (float) (raw - min)/this.range;
      }

      /// <summary>Reads the raw value from the joystick state</summary>
      /// <param name="state">Joystick state the value is read from</param>
      /// <returns>The raw value of the axis in the joystick state</returns>
      protected virtual int Read(ref JoystickState state)
      {
        return state.GetSliders()[this.Index];
      }

      /// <summary>Index of the slider in the array</summary>
      protected int Index;

      /// <summary>Minimum raw value of the slider</summary>
      private int min;

      /// <summary>Total value range of the slider</summary>
      private float range;
    }

    #endregion // class SliderReader

    #region class VelocitySliderRead

    /// <summary>Reads the value of a velocity slider</summary>
    private class VelocitySliderReader : SliderReader
    {
      public VelocitySliderReader(int index, int min, int max) : base(index, min, max)
      {
      }

      protected override int Read(ref JoystickState state)
      {
        return state.GetVelocitySliders()[base.Index];
      }
    }

    #endregion // class VelocitySliderReader

    #region class AccelerationSliderRead

    /// <summary>Reads the value of an acceleration slider</summary>
    private class AccelerationSliderReader : SliderReader
    {
      public AccelerationSliderReader(int index, int min, int max) : base(index, min, max)
      {
      }

      protected override int Read(ref JoystickState state)
      {
        return state.GetAccelerationSliders()[base.Index];
      }
    }

    #endregion // class AccelerationSliderReader

    #region class ForceSliderRead

    /// <summary>Reads the value of a force slider</summary>
    private class ForceSliderReader : SliderReader
    {
      public ForceSliderReader(int index, int min, int max) : base(index, min, max)
      {
      }

      protected override int Read(ref JoystickState state)
      {
        return state.GetForceSliders()[base.Index];
      }
    }

    #endregion // class ForceSliderReader
  }
}