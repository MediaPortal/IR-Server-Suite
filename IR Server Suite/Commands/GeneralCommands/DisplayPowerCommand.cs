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
using System.Diagnostics;
using System.Windows.Forms;
using IrssUtils;

namespace IrssCommands.General
{
  /// <summary>
  /// Command to close a running program.
  /// </summary>
  public class CloseProgramCommand : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CloseProgramCommand"/> class.
    /// </summary>
    public CloseProgramCommand()
    {
      InitParameters(2);

      // setting default values
      Parameters[0] = Enum.GetName(typeof(WindowTargetType), WindowTargetType.Active);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CloseProgramCommand"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CloseProgramCommand(string[] parameters)
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
      get { return "Close a Running Program"; }
    }

    /// <summary>
    /// Gets the edit control to be used within a common edit form.
    /// </summary>
    /// <returns>The edit control.</returns>
    public override BaseCommandConfig GetEditControl()
    {
      return new CloseProgramConfig(Parameters);
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="variables">The variable list of the calling code.</param>
    public override void Execute(VariableList variables)
    {
      string[] processed = ProcessParameters(variables, Parameters);

      // get parameters and error / exception handling
      WindowTargetType targetType =
        (WindowTargetType)Enum.Parse(typeof(WindowTargetType), processed[0], true);
      string target = processed[1];

      // do actual execute
      Process process;
      switch (targetType)
      {
        case WindowTargetType.Active:
          process = GetProcessByWindowHandle(Win32.ForegroundWindow());
          break;

        case WindowTargetType.Application:
          process = GetProcessByFilePath(target);
          break;

        case WindowTargetType.Class:
          process = GetProcessByWindowHandle(Win32.FindWindowByClass(target));
          break;

        case WindowTargetType.Window:
          process = GetProcessByWindowTitle(target);
          break;

        default:
          throw new CommandStructureException(String.Format("Invalid close program target type: {0}", targetType));
      }

      if (process == null)
        throw new CommandExecutionException(String.Format("Close Program target ({0}) not found", target));

      EndProcess(process, 5000);

      process.Close();
    }

    #endregion Public Methods


    //  if (String.IsNullOrEmpty(command))
    //    throw new ArgumentNullException("command");

    //  int powerState = -1;
    //  if (Int32.TryParse(command, out powerState))
    //    Display.SetPowerState(powerState);
    //  else
    //    throw new CommandStructureException(String.Format("Display Power Command data is not a valid integer: {0}",
    //                                                      command));
  }
}