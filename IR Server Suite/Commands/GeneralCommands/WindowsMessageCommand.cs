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
  /// HTTP Message command.
  /// </summary>
  public class WindowsMessageCommand : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowsMessageCommand"/> class.
    /// </summary>
    public WindowsMessageCommand()
    {
      InitParameters(5);

      // setting default values
      Parameters[0] = Enum.GetName(typeof (WindowTargetType), WindowTargetType.Active);
      Parameters[1] = String.Empty;
      Parameters[2] = ((int) Win32.WindowsMessage.WM_USER).ToString();
      Parameters[3] = 0.ToString();
      Parameters[4] = 0.ToString();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowsMessageCommand"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public WindowsMessageCommand(string[] parameters)
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
      get { return "Window Message"; }
    }

    /// <summary>
    /// Gets the edit control to be used within a common edit form.
    /// </summary>
    /// <returns>The edit control.</returns>
    public override BaseCommandConfig GetEditControl()
    {
      return new WindowsMessageConfig(Parameters);
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="variables">The variable list of the calling code.</param>
    public override void Execute(VariableList variables)
    {
      string[] processed = ProcessParameters(variables, Parameters);

      // get parameters and error / exception handling
      WindowTargetType targetType = (WindowTargetType) Enum.Parse(typeof (WindowTargetType), processed[0], true);
      string target = processed[1];
      int message = int.Parse(processed[2]);
      IntPtr wordParam = new IntPtr(int.Parse(processed[3]));
      IntPtr longParam = new IntPtr(int.Parse(processed[4]));

      // do actual execute
      IntPtr windowHandle = IntPtr.Zero;

      switch (targetType)
      {
        case WindowTargetType.Active:
          windowHandle = Win32.ForegroundWindow();
          break;

        case WindowTargetType.Application:
          foreach (Process proc in Process.GetProcesses())
          {
            try
            {
              if (target.Equals(proc.MainModule.FileName, StringComparison.OrdinalIgnoreCase))
              {
                windowHandle = proc.MainWindowHandle;
                break;
              }
            }
            catch
            {
            }
          }
          break;

        case WindowTargetType.Class:
          windowHandle = Win32.FindWindowByClass(target);
          break;

        case WindowTargetType.Window:
          windowHandle = Win32.FindWindowByTitle(target);
          break;

        default:
          throw new CommandStructureException(String.Format("Invalid message target type: {0}", targetType));
      }

      if (windowHandle == IntPtr.Zero)
        throw new CommandExecutionException(String.Format("Window Message target ({0}) not found", target));

      Win32.SendWindowsMessage(windowHandle, message, wordParam, longParam);

#warning check if result needs to be publsihed (log or msg box)
    }

    #endregion Public Methods
  }
}