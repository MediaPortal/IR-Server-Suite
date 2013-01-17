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
using IrssCommands.MediaPortal.Configuration;
using MediaPortal.GUI.Library;

namespace IrssCommands.MediaPortal
{
  /// <summary>
  /// Goto Screen MediaPortal command.
  /// </summary>
  public class CommandGotoScreen : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandGotoScreen"/> class.
    /// </summary>
    public CommandGotoScreen()
    {
      InitParameters(1);

      Parameters[0] = "0";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandGotoScreen"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandGotoScreen(string[] parameters)
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
      get { return "Goto Screen"; }
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

        return String.Format("{0} ({1})", UserInterfaceText, MPUtils.MPCommon.GetFriendlyWindowName(int.Parse(Parameters[0])));
      }
    }

    /// <summary>
    /// Gets the edit control to be used within a common edit form.
    /// </summary>
    /// <returns>The edit control.</returns>
    public override BaseCommandConfig GetEditControl()
    {
      return new GotoScreenConfig(Parameters);
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
      string[] processed = ProcessParameters(variables, Parameters);

      string windowID = processed[0];

      int window = (int) GUIWindow.Window.WINDOW_INVALID;
      try
      {
        window = (int) Enum.Parse(typeof (GUIWindow.Window), "WINDOW_" + windowID, true);
      }
      catch (ArgumentException)
      {
        // Parsing the window id as a GUIWindow.Window failed, so parse it as an int
      }

      if (window == (int) GUIWindow.Window.WINDOW_INVALID)
        int.TryParse(windowID, out window);

      if (window == (int) GUIWindow.Window.WINDOW_INVALID)
        throw new CommandStructureException(String.Format("Failed to parse Goto screen command window id \"{0}\"",
                                                          windowID));

#warning fixME use basic home
      //if (window == (int)GUIWindow.Window.WINDOW_HOME && useBasicHome)
      //  window = (int)GUIWindow.Window.WINDOW_SECOND_HOME;

      GUIGraphicsContext.ResetLastActivity();
      GUIWindowManager.SendThreadMessage(new GUIMessage(GUIMessage.MessageType.GUI_MSG_GOTO_WINDOW, 0, 0, 0, window, 0,
                                                        null));
    }

    #endregion Implementation
  }
}