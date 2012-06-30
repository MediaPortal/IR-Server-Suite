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

namespace IRServer.Plugin.GamePad
{
  internal partial class DirectInputConverter
  {
    #region interface IButtonReader

    /// <summary>Reads the state of a button</summary>
    internal interface IButtonReader
    {
      /// <summary>Determines whether the specified button is pressed</summary>
      /// <param name="buttons">Array containing the state of all buttons</param>
      /// <returns>True if the specified button was pressed</returns>
      bool IsPressed(bool[] buttons);

      /// <summary>
      ///   Determines whether the state of the specified button has changed
      ///   between two states
      /// </summary>
      /// <param name="previous">Previous state of the buttons</param>
      /// <param name="current">Current state of the buttons</param>
      /// <returns>True if the state of the button has changed</returns>
      bool HasChanged(bool[] previous, bool[] current);
    }

    #endregion // interface IButtonReader

    #region class ButtonReader

    private class ButtonReader : IButtonReader
    {
      /// <summary>Initializes a new button reader for the specified button</summary>
      /// <param name="buttonIndex">
      ///   Index of the button the reader is initialized for
      /// </param>
      public ButtonReader(int buttonIndex)
      {
        this.buttonIndex = buttonIndex;
      }

      /// <summary>Determines whether the specified button is pressed</summary>
      /// <param name="buttons">Array containing the state of all buttons</param>
      /// <returns>True if the specified button was pressed</returns>
      public bool IsPressed(bool[] buttons)
      {
        return buttons[this.buttonIndex];
      }

      /// <summary>
      ///   Determines whether the state of the specified button has changed
      ///   between two states
      /// </summary>
      /// <param name="previous">Previous state of the buttons</param>
      /// <param name="current">Current state of the buttons</param>
      /// <returns>True if the state of the button has changed</returns>
      public bool HasChanged(bool[] previous, bool[] current)
      {
        return previous[this.buttonIndex] != current[this.buttonIndex];
      }

      /// <summary>Index of the button the reader is checking</summary>
      private int buttonIndex;
    }

    #endregion // class ButtonReader
  }
}