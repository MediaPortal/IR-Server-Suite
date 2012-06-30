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


#if UNITTEST

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Input;

using NUnit.Framework;
using NMock;
using Microsoft.Xna.Framework;

namespace Nuclex.Input.Devices {

  /// <summary>Unit tests for the extended game pad state container</summary>
  [TestFixture]
  public class ExtendedGamePadStateTest {

    /// <summary>
    ///   Verifies that the default constructor of the game state class works
    /// </summary>
    [Test]
    public void DefaultConstructorCanBeUsed() {
      var state = new ExtendedGamePadState();
      Assert.IsNotNull(state); // nonsense, avoids compiler warning
    }

    /// <summary>
    ///   Verifies that the axis mappings in the full constructor are working
    /// </summary>
    [Test]
    public void ConstructorTakesOverAxisStates() {
      var axes = new float[24];
      var sliders = new float[8];
      var buttons = new bool[128];
      var povs = new int[4];

      int axisIndex = 0;
      foreach (ExtendedAxes axis in Enum.GetValues(typeof(ExtendedAxes))) {
        Array.Clear(axes, 0, 24);
        axes[axisIndex] = 12.34f;

        ExtendedGamePadState state = new ExtendedGamePadState(
          axis, axes,
          0, sliders,
          0, buttons,
          0, povs
        );

        // Only the provided axis should be reported as available by the state
        Assert.AreEqual(state.AvailableAxes, axis);

        // Ensure only the provided axis has the value assigned
        foreach (ExtendedAxes axis2 in Enum.GetValues(typeof(ExtendedAxes))) {
          if (axis2 == axis) {
            Assert.AreEqual(12.34f, state.GetAxis(axis2));
          } else {
            Assert.AreEqual(0.0f, state.GetAxis(axis2));
          }
        }

        ++axisIndex;
      }
    }

    /// <summary>
    ///   Verifies that the number of axes is taken over by the constructor
    /// </summary>
    [Test]
    public void ConstructorTakesOverAxisCount() {
      var axes = new float[24];
      var sliders = new float[8];
      var buttons = new bool[128];
      var povs = new int[4];

      for (int count = 0; count < 24; ++count) {
        int axisBits = 0;
        for (int axisIndex = 0; axisIndex < count; ++axisIndex) {
          axisBits |= (1 << axisIndex);
        }

        ExtendedGamePadState state = new ExtendedGamePadState(
          (ExtendedAxes)axisBits, axes,
          0, sliders,
          0, buttons,
          0, povs
        );

        // Only the provided axes should be reported as available by the state
        Assert.AreEqual(state.AvailableAxes, (ExtendedAxes)axisBits);
        Assert.AreEqual(count, state.AxisCount);
      }
    }

    /// <summary>
    ///   Verifies that the slider mappings in the full constructor are working
    /// </summary>
    [Test]
    public void ConstructorTakesOverSliderStates() {
      var axes = new float[24];
      var sliders = new float[8];
      var buttons = new bool[128];
      var povs = new int[4];

      int sliderIndex = 0;
      foreach (ExtendedSliders slider in Enum.GetValues(typeof(ExtendedSliders))) {
        Array.Clear(sliders, 0, 8);
        sliders[sliderIndex] = 12.34f;

        ExtendedGamePadState state = new ExtendedGamePadState(
          0, axes,
          slider, sliders,
          0, buttons,
          0, povs
        );

        // Only the provided slider should be reported as available by the state
        Assert.AreEqual(state.AvailableSliders, slider);

        // Ensure only the provided slider has the value assigned
        foreach (ExtendedSliders slider2 in Enum.GetValues(typeof(ExtendedSliders))) {
          if (slider2 == slider) {
            Assert.AreEqual(12.34f, state.GetSlider(slider2));
          } else {
            Assert.AreEqual(0.0f, state.GetSlider(slider2));
          }
        }

        ++sliderIndex;
      }
    }

    /// <summary>
    ///   Verifies that the number of sliders is taken over by the constructor
    /// </summary>
    [Test]
    public void ConstructorTakesOverSliderCount() {
      var axes = new float[24];
      var sliders = new float[8];
      var buttons = new bool[128];
      var povs = new int[4];

      for (int count = 0; count < 24; ++count) {
        int sliderBits = 0;
        for (int sliderIndex = 0; sliderIndex < count; ++sliderIndex) {
          sliderBits |= (1 << sliderIndex);
        }

        ExtendedGamePadState state = new ExtendedGamePadState(
          0, axes,
          (ExtendedSliders)sliderBits, sliders,
          0, buttons,
          0, povs
        );

        // Only the provided axes should be reported as available by the state
        Assert.AreEqual(state.AvailableSliders, (ExtendedSliders)sliderBits);
        Assert.AreEqual(count, state.SliderCount);
      }
    }

    /// <summary>
    ///   Verifies that the button mappings in the full constructor are working
    /// </summary>
    [Test]
    public void ConstuctorTakesOverButtonStates() {
      var axes = new float[24];
      var sliders = new float[8];
      var buttons = new bool[128];
      var povs = new int[4];

      for (int buttonIndex = 0; buttonIndex < 128; ++buttonIndex) {
        Array.Clear(buttons, 0, 128);
        buttons[buttonIndex] = true;

        ExtendedGamePadState state = new ExtendedGamePadState(
          0, axes,
          0, sliders,
          buttonIndex + 1, buttons,
          0, povs
        );

        // The specified number of buttons should be available
        Assert.AreEqual(state.ButtonCount, buttonIndex + 1);

        // Ensure only the provided button is reported as pressed
        for (int buttonIndex2 = 0; buttonIndex2 < 128; ++buttonIndex2) {
          if (buttonIndex2 == buttonIndex) {
            Assert.AreEqual(ButtonState.Pressed, state.GetButton(buttonIndex2));
            Assert.IsTrue(state.IsButtonDown(buttonIndex2));
            Assert.IsFalse(state.IsButtonUp(buttonIndex2));
          } else {
            Assert.AreEqual(ButtonState.Released, state.GetButton(buttonIndex2));
            Assert.IsFalse(state.IsButtonDown(buttonIndex2));
            Assert.IsTrue(state.IsButtonUp(buttonIndex2));
          }
        } // for
      } // for
    }

    /// <summary>
    ///   Verifies that the PoV controller mappings in the full constructor are working
    /// </summary>
    [Test]
    public void ConstructorTakesOverPovControllerStates() {
      var axes = new float[24];
      var sliders = new float[8];
      var buttons = new bool[128];
      var povs = new int[4];

      for (int povIndex = 0; povIndex < 4; ++povIndex) {
        Array.Clear(povs, 0, 4);
        povs[povIndex] = 12345;

        var state = new ExtendedGamePadState(
          0, axes,
          0, sliders,
          0, buttons,
          povIndex + 1, povs
        );

        // The specified number of PoV controllers should be available
        Assert.AreEqual(state.PovCount, povIndex + 1);

        // Ensure only the provided pov has the value assigned
        for (int povIndex2 = 0; povIndex2 < 4; ++povIndex2) {
          if (povIndex2 == povIndex) {
            Assert.AreEqual(12345, state.GetPov(povIndex2));
          } else {
            Assert.AreEqual(0, state.GetPov(povIndex2));
          }
        } // for
      } // for
    }

    /// <summary>
    ///   Verifies that an exception is thrown if an invalid axis is accessed
    /// </summary>
    [Test]
    public void AccessingInvalidAxisCausesException() {
      var state = new ExtendedGamePadState();
      Assert.Throws<ArgumentOutOfRangeException>(
        delegate() {
          state.GetAxis(ExtendedAxes.X | ExtendedAxes.Y);
        }
      );
    }

    /// <summary>
    ///   Verifies that an exception is thrown if an invalid slider is accessed
    /// </summary>
    [Test]
    public void AccessingInvalidSliderCausesException() {
      var state = new ExtendedGamePadState();
      Assert.Throws<ArgumentOutOfRangeException>(
        delegate() {
          state.GetSlider(ExtendedSliders.Slider1 | ExtendedSliders.Slider2);
        }
      );
    }

    /// <summary>
    ///   Verifies that an exception is thrown if an invalid button is accessed
    /// </summary>
    [
      Test, TestCase(-1), TestCase(128)
    ]
    public void AccessingInvalidButtonsCausesException(int buttonIndex) {
      var state = new ExtendedGamePadState();
      Assert.Throws<ArgumentOutOfRangeException>(
        delegate() { state.IsButtonDown(buttonIndex); }
      );
      Assert.Throws<ArgumentOutOfRangeException>(
        delegate() { state.IsButtonUp(buttonIndex); }
      );
    }

    /// <summary>
    ///   Verifies that an exception is thrown if an invalid PoV controller is accessed
    /// </summary>
    [
      Test, TestCase(-1), TestCase(4)
    ]
    public void InvalidPovIndexCausesException(int povIndex) {
      var state = new ExtendedGamePadState();
      Assert.Throws<ArgumentOutOfRangeException>(
        delegate() { state.GetPov(povIndex); }
      );
    }

    /// <summary>
    ///   Verifies that an extended game pad state can be constructed from a normal
    ///   XNA game pad state
    /// </summary>
    /// <param name="button">Button to test on the normal game pad state</param>
    [
      Test,
      TestCase(Buttons.DPadUp),
      TestCase(Buttons.DPadDown),
      TestCase(Buttons.DPadLeft),
      TestCase(Buttons.DPadRight),
      TestCase(Buttons.Start),
      TestCase(Buttons.Back),
      TestCase(Buttons.LeftStick),
      TestCase(Buttons.RightStick),
      TestCase(Buttons.LeftShoulder),
      TestCase(Buttons.RightShoulder),
      TestCase(Buttons.BigButton),
      TestCase(Buttons.A),
      TestCase(Buttons.B),
      TestCase(Buttons.X),
      TestCase(Buttons.Y)
    ]
    public void ExtendedGamePadStateCanBeConstructedFromGamePadState(Buttons button) {
      var state = new GamePadState(
        new Vector2(0.12f, 0.34f),
        new Vector2(0.56f, 0.78f),
        0.1234f, 0.5678f,
        button
      );
      var extendedState = new ExtendedGamePadState(ref state);

      Assert.AreEqual(0.12f, extendedState.GetAxis(ExtendedAxes.X));
      Assert.AreEqual(0.34f, extendedState.GetAxis(ExtendedAxes.Y));
      Assert.AreEqual(0.56f, extendedState.GetAxis(ExtendedAxes.RotationX));
      Assert.AreEqual(0.78f, extendedState.GetAxis(ExtendedAxes.RotationY));
      Assert.AreEqual(0.1234f, extendedState.GetSlider(ExtendedSliders.Slider1));
      Assert.AreEqual(0.5678f, extendedState.GetSlider(ExtendedSliders.Slider2));

      switch (button) {
        case Buttons.DPadUp: {
          Assert.AreEqual(0, extendedState.GetPov(0));
          break;
        }
        case Buttons.DPadRight: {
          Assert.AreEqual(9000, extendedState.GetPov(0));
          break;
        }
        case Buttons.DPadDown: {
          Assert.AreEqual(18000, extendedState.GetPov(0));
          break;
        }
        case Buttons.DPadLeft: {
          Assert.AreEqual(27000, extendedState.GetPov(0));
          break;
        }
        default: {
          Assert.IsTrue(
            extendedState.IsButtonDown(
              Array.IndexOf(ExtendedGamePadState.ButtonOrder, button)
            )
          );
          break;
        }
      }
    }

    /// <summary>
    ///   Verifies that directional pad states can be converted into PoV controller states
    ///   and vice versa
    /// </summary>
    /// <param name="pov">PoV controll state</param>
    /// <param name="up">Whether up on the directional pad is being pressed</param>
    /// <param name="down">Whether down on the directional pad is being pressed</param>
    /// <param name="left">Whether left on the directional pad is being pressed</param>
    /// <param name="right">Whether right on the directional pad is being pressed</param>
    [
      Test,
      TestCase(-1, false, false, false, false),
      TestCase(0, true, false, false, false),
      TestCase(4500, true, false, false, true),
      TestCase(9000, false, false, false, true),
      TestCase(13500, false, true, false, true),
      TestCase(18000, false, true, false, false),
      TestCase(22500, false, true, true, false),
      TestCase(27000, false, false, true, false),
      TestCase(31500, true, false, true, false),
    ]
    public void CanConvertBetweeenDirectionalPadAndPov(
      int pov, bool up, bool down, bool left, bool right
    ) {
      GamePadDPad dpad = new GamePadDPad(
        up ? ButtonState.Pressed : ButtonState.Released,
        down ? ButtonState.Pressed : ButtonState.Released,
        left ? ButtonState.Pressed : ButtonState.Released,
        right ? ButtonState.Pressed : ButtonState.Released
      );

      Assert.AreEqual(pov, ExtendedGamePadState.PovFromDpad(dpad));
      Assert.AreEqual(dpad, ExtendedGamePadState.DpadFromPov(pov));
    }

  }

} // namespace Nuclex.Input.Devices

#endif
// UNITTEST