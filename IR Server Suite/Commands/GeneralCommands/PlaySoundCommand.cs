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
using System.IO;
using System.Windows.Forms;
using IrssUtils;

namespace IrssCommands.General
{
  /// <summary>
  /// Eject drive command.
  /// </summary>
  public class PlaySoundCommand : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaySoundCommand"/> class.
    /// </summary>
    public PlaySoundCommand()
    {
      InitParameters(1);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaySoundCommand"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public PlaySoundCommand(string[] parameters)
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
      get { return "Play Sound"; }
    }

    /// <summary>
    /// Gets the edit control to be used within a common edit form.
    /// </summary>
    /// <returns>The edit control.</returns>
    public override BaseCommandConfig GetEditControl()
    {
      return new PlaySoundConfig(Parameters);
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
      // do not process the parameters to prevent paths getting corrupted
      //string[] processed = ProcessParameters(variables, Parameters);
      string[] processed = Parameters;

      string audioFile = processed[0];

      if (!File.Exists(audioFile))
        throw new FileNotFoundException(String.Format("Sound file ({0}) not found", audioFile));

      if (!Audio.PlayFile(audioFile, false))
        throw new CommandExecutionException(String.Format("Sound file ({0}) can not be played", audioFile));
    }

    #endregion Public Methods
  }
}