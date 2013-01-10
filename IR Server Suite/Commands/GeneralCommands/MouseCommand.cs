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

using System.Windows.Forms;
using IrssUtils;

namespace IrssCommands.General
{
  /// <summary>
  /// HTTP Message command.
  /// </summary>
  public class MouseCommand : Command
  {
    #region Constants

    public const string MouseClickLeft = "Click_Left";
    public const string MouseClickMiddle = "Click_Middle";
    public const string MouseClickRight = "Click_Right";

    public const string MouseDoubleClickLeft = "DoubleClick_Left";
    public const string MouseDoubleClickMiddle = "DoubleClick_Middle";
    public const string MouseDoubleClickRight = "DoubleClick_Right";

    public const string MouseMoveUp = "Move_Up ";
    public const string MouseMoveDown = "Move_Down ";
    public const string MouseMoveLeft = "Move_Left ";
    public const string MouseMoveRight = "Move_Right ";

    public const string MouseMoveToPos = "Move_To_Pos ";

    public const string MouseScrollDown = "Scroll_Down";
    public const string MouseScrollUp = "Scroll_Up";

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="MouseCommand"/> class.
    /// </summary>
    public MouseCommand()
    {
      InitParameters(3);

      // setting default values
      Parameters[0] = "none";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MouseCommand"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public MouseCommand(string[] parameters)
      : base(parameters)
    {
    }

    #endregion Constructors

    #region Public Methods

    /// <summary>
    /// Gets the category of this command.
    /// </summary>
    /// <value>The category of this command.</value>
    public override string Category
    {
      get { return "General Commands"; }
    }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <value>User interface text.</value>
    public override string UserInterfaceText
    {
      get { return "Mouse Command"; }
    }

    /// <summary>
    /// Gets the edit control to be used within a common edit form.
    /// </summary>
    /// <returns>The edit control.</returns>
    public override BaseCommandConfig GetEditControl()
    {
      return new MouseConfig(Parameters);
    }

    /// <summary>
    /// Edit this command.
    /// </summary>
    /// <param name="parent">The parent window.</param>
    /// <returns><c>true</c> if the command was modified; otherwise <c>false</c>.</returns>
    public override bool Edit(IWin32Window parent)
    {
      CommandConfigForm edit = new CommandConfigForm(this);
      if (edit.ShowDialog(parent) == DialogResult.OK)
      {
        Parameters = edit.Parameters;
        return true;
      }

      return false;
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="variables">The variable list of the calling code.</param>
    public override void Execute(VariableList variables)
    {
      string[] processed = ProcessParameters(variables, Parameters);

      // get parameters and error / exception handling
      string cmd = processed[0];
      int x;
      int y;
      int move;

      // do actual execute
      switch (cmd)
      {
        case MouseClickLeft:
          Mouse.Button(Mouse.MouseEvents.LeftDown);
          Mouse.Button(Mouse.MouseEvents.LeftUp);
          break;
        case MouseClickMiddle:
          Mouse.Button(Mouse.MouseEvents.MiddleDown);
          Mouse.Button(Mouse.MouseEvents.MiddleUp);
          break;
        case MouseClickRight:
          Mouse.Button(Mouse.MouseEvents.RightDown);
          Mouse.Button(Mouse.MouseEvents.RightUp);
          break;

        case MouseDoubleClickLeft:
          Mouse.Button(Mouse.MouseEvents.LeftDown);
          Mouse.Button(Mouse.MouseEvents.LeftUp);
          Mouse.Button(Mouse.MouseEvents.LeftDown);
          Mouse.Button(Mouse.MouseEvents.LeftUp);
          break;
        case MouseDoubleClickMiddle:
          Mouse.Button(Mouse.MouseEvents.MiddleDown);
          Mouse.Button(Mouse.MouseEvents.MiddleUp);
          Mouse.Button(Mouse.MouseEvents.MiddleDown);
          Mouse.Button(Mouse.MouseEvents.MiddleUp);
          break;
        case MouseDoubleClickRight:
          Mouse.Button(Mouse.MouseEvents.RightDown);
          Mouse.Button(Mouse.MouseEvents.RightUp);
          Mouse.Button(Mouse.MouseEvents.RightDown);
          Mouse.Button(Mouse.MouseEvents.RightUp);
          break;

        case MouseScrollUp:
          Mouse.Scroll(Mouse.ScrollDir.Down);
          break;
        case MouseScrollDown:
          Mouse.Scroll(Mouse.ScrollDir.Up);
          break;

        case MouseMoveUp:
          move = int.Parse(processed[1]);
          Mouse.Move(0, -move, false);
          break;
        case MouseMoveDown:
          move = int.Parse(processed[1]);
          Mouse.Move(0, move, false);
          break;
        case MouseMoveLeft:
          move = int.Parse(processed[1]);
          Mouse.Move(-move, 0, false);
          break;
        case MouseMoveRight:
          move = int.Parse(processed[1]);
          Mouse.Move(move, 0, false);
          break;

        case MouseMoveToPos:
          x = int.Parse(processed[1]);
          y = int.Parse(processed[2]);
          Mouse.Move(x, y, true);
          break;
      }
    }

    #endregion Public Methods
  }
}