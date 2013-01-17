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

namespace IrssCommands.MediaPortal
{
  /// <summary>
  /// Command to toggle layer in input mapper
  /// </summary>
  public class CommandToggleLayer : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandToggleLayer"/> class.
    /// </summary>
    public CommandToggleLayer()
    {
      InitParameters(0);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandToggleLayer"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandToggleLayer(string[] parameters)
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
      get { return "Toggle Layer"; }
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
    /// Actually this command does nothing, but it is catched in <see cref="InputHandler"/> specifically to toggle the active layer.
    /// </summary>
    /// <param name="variables">The variable list of the calling code.</param>
    public override void Execute(VariableList variables)
    {
    }

    #endregion Implementation
  }
}