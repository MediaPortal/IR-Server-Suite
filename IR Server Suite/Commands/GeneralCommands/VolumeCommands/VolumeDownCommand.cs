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

namespace IrssCommands.General
{
  /// <summary>
  /// Command to decrease the volume.
  /// </summary>
  public class VolumeDownCommand : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="VolumeDownCommand"/> class.
    /// </summary>
    public VolumeDownCommand()
    {
      InitParameters(0);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VolumeDownCommand"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public VolumeDownCommand(string[] parameters)
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
      get { return "General Commands"; }
    }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <value>User interface text.</value>
    public override string UserInterfaceText
    {
      get { return "Volume Down"; }
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="variables">The variable list of the calling code.</param>
    public override void Execute(VariableList variables)
    {
      Win32.SendWindowsMessage(
        Win32.GetDesktopWindowHandle(),
        (int) Win32.WindowsMessage.WM_APPCOMMAND,
        0,
        65536*(int) Win32.AppCommand.APPCOMMAND_VOLUME_DOWN);
    }

    #endregion Implementation
  }
}