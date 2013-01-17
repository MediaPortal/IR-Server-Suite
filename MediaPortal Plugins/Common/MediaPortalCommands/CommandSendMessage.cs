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

namespace IrssCommands.MediaPortal
{
  /// <summary>
  /// Send Message MediaPortal command.
  /// </summary>
  public class CommandSendMessage : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandSendMessage"/> class.
    /// </summary>
    public CommandSendMessage()
    {
      InitParameters(6);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandSendMessage"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandSendMessage(string[] parameters)
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
      get { return "Send Message"; }
    }

    /// <summary>
    /// Gets the edit control to be used within a common edit form.
    /// </summary>
    /// <returns>The edit control.</returns>
    public override BaseCommandConfig GetEditControl()
    {
      return new SendMessageConfig(Parameters);
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

      GUIMessage.MessageType type = (GUIMessage.MessageType) Enum.Parse(typeof (GUIMessage.MessageType), processed[0]);
      int windowId = int.Parse(processed[1]);
      int senderId = int.Parse(processed[2]);
      int controlId = int.Parse(processed[3]);
      int param1 = int.Parse(processed[4]);
      int param2 = int.Parse(processed[5]);

      GUIMessage message = new GUIMessage(type, windowId, senderId, controlId, param1, param2, null);

      GUIGraphicsContext.ResetLastActivity();
      GUIWindowManager.SendThreadMessage(message);
    }

    #endregion Implementation
  }
}