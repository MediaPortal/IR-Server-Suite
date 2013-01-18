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

namespace IrssCommands
{
  /// <summary>
  /// This class can be used to represent an IR Server Suite commands, which is not available.
  /// i.e. because of a missing command plugin or a renamed class name etc.
  /// </summary>
  public class DummyCommand
  {
    #region Properties

    /// <summary>
    /// Gets or sets the command class fullname.
    /// </summary>
    /// <value>The command parameters.</value>
    public string CommandType { get; set; }

    /// <summary>
    /// Gets or sets the command parameters.
    /// </summary>
    /// <value>The command parameters.</value>
    public string[] Parameters { get; set; }

    /// <summary>
    /// Gets the user interface text.
    /// This method must be replaced in sub-classes.
    /// </summary>
    /// <value>The user interface text.</value>
    public string UserInterfaceText
    {
      get { return "!!! " + CommandType; }
    }

    /// <summary>
    /// Gets the user display text.
    /// </summary>
    /// <value>The user display text.</value>
    public string UserDisplayText
    {
      get
      {
        if (Parameters == null)
          return UserInterfaceText;
        else
          return String.Format("{0} ({1})", UserInterfaceText, String.Join(", ", Parameters));
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="DummyCommand"/> class.
    /// </summary>
    /// <param name="commandType">The command type (class fullname).</param>
    /// <param name="parameters">The command parameters.</param>
    public DummyCommand(string commandType, string[] parameters)
    {
      CommandType = commandType;
      Parameters = parameters;
    }

    #endregion Constructors
  }
}