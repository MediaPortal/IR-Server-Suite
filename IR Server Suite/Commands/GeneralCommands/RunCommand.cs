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
  /// Run command.
  /// </summary>
  public class RunCommand : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="RunCommand"/> class.
    /// </summary>
    public RunCommand()
    {
      InitParameters(8);

      // setting default values
      Parameters[3] = Enum.GetName(typeof (ProcessWindowStyle), ProcessWindowStyle.Normal);
      Parameters[4] = false.ToString();
      Parameters[5] = false.ToString();
      Parameters[6] = false.ToString();
      Parameters[7] = false.ToString();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RunCommand"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public RunCommand(string[] parameters) : base(parameters)
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
      get { return "Run Program"; }
    }

    /// <summary>
    /// Gets the edit control to be used within a common edit form.
    /// </summary>
    /// <returns>The edit control.</returns>
    public override BaseCommandConfig GetEditControl()
    {
      return new RunConfig(Parameters);
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

      // get parameters and error / exception handling
      string fileName = processed[0];
      string workingDirectory = processed[1];
      string arguments = processed[2];
      ProcessWindowStyle processWindowStyle =
        (ProcessWindowStyle) Enum.Parse(typeof (ProcessWindowStyle), processed[3], true);
      bool createNoWindow = bool.Parse(processed[4]);
      bool useShellExecute = bool.Parse(processed[5]);
      bool waitForExit = bool.Parse(processed[6]);
      bool forceFocus = bool.Parse(processed[7]);

      // do actual execute
      using (Process process = new Process())
      {
        process.StartInfo.FileName = fileName;
        process.StartInfo.WorkingDirectory = workingDirectory;
        process.StartInfo.Arguments = arguments;
        process.StartInfo.WindowStyle = processWindowStyle;
        process.StartInfo.CreateNoWindow = createNoWindow;
        process.StartInfo.UseShellExecute = useShellExecute;
        //process.PriorityClass               = ProcessPriorityClass.

        process.Start();

        // Give new process focus ...
        if (forceFocus && !process.StartInfo.CreateNoWindow &&
            process.StartInfo.WindowStyle != ProcessWindowStyle.Hidden)
        {
          FocusForcer forcer = new FocusForcer(process.Id);
          //forcer.Start();
          forcer.Force();
        }

        if (waitForExit)
          process.WaitForExit();
      }
    }

    #endregion Public Methods
  }
}