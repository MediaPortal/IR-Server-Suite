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
using MediaPortal.GUI.Library;
using Action = MediaPortal.GUI.Library.Action;

namespace IrssCommands.MediaPortal
{
  /// <summary>
  /// Send Action MediaPortal command.
  /// </summary>
  public class CommandSendAction : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandSendAction"/> class.
    /// </summary>
    public CommandSendAction()
    {
      InitParameters(3);

      Parameters[0] = Enum.GetName(typeof(Action.ActionType), Action.ActionType.ACTION_SELECT_ITEM);
      Parameters[1] = "0";
      Parameters[2] = "0";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandSendAction"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandSendAction(string[] parameters)
      : base(parameters)
    {
    }

    #endregion Constructors

    #region Implementation

    /// <summary>
    /// Gets the category of this command.
    /// </summary>
    /// <value>The category of this command.</value>
    public override string Category
    {
      get { return "MediaPortal Commands"; }
    }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <value>User interface text.</value>
    public override string UserInterfaceText
    {
      get { return "Send Action"; }
    }

    /// <summary>
    /// Gets the user display text.
    /// </summary>
    /// <value>The user display text.</value>
    public override string UserDisplayText
    {
      get
      {
        if (Parameters == null)
          return UserInterfaceText;

        return String.Format("{0} ({1})", UserInterfaceText, MPUtils.MPCommon.GetFriendlyActionName(Parameters[0]));
      }
    }

    /// <summary>
    /// Gets the edit control to be used within a common edit form.
    /// </summary>
    /// <returns>The edit control.</returns>
    public override BaseCommandConfig GetEditControl()
    {
      return new SendActionConfig(Parameters);
    }

    /// <summary>
    /// Gets the value, wether this command can be tested.
    /// </summary>
    /// <value>Whether the command can be tested.</value>
    public override bool IsTestAllowed
    {
      get { return false; }
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="variables">The variable list of the calling code.</param>
    public override void Execute(VariableList variables)
    {
      Action.ActionType type = (Action.ActionType) Enum.Parse(typeof (Action.ActionType), Parameters[0], true);

      float f1;
      float.TryParse(Parameters[1], out f1);
      float f2;
      float.TryParse(Parameters[2], out f2);

      Action action = new Action(type, f1, f2);
      GUIGraphicsContext.OnAction(action);
    }

    #endregion Implementation
  }
}