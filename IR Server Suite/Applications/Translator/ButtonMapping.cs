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
using System.Xml.Serialization;
using IrssCommands;

namespace Translator
{
  /// <summary>
  /// Stores the relevant information for mapping a remote button to a command.
  /// </summary>
  public class ButtonMapping
  {
    #region Variables

    private Command _command;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Remote button key code.
    /// </summary>
    [XmlAttribute]
    public string KeyCode { get; set; }

    /// <summary>
    /// Remote button description.
    /// </summary>
    [XmlAttribute]
    public string Description { get; set; }

    /// <summary>
    /// Command to execute
    /// </summary>
    [XmlIgnore]
    public Command Command
    {
      get
      {
        if (_command == null && !string.IsNullOrEmpty(CommandType))
        {
          try
          {
            _command = Processor.CreateCommand(CommandType, Parameters);
          }
          catch (Exception ex)
          {
            IrssUtils.IrssLog.Error("Command could not be created. Please check your commands library ({0}) for correct command plugins or replace existing mapping with available commands.",ex, Processor.LibraryFolder);
          }
        }

        return _command;
      }
      set
      {
        _command = value;

        if (ReferenceEquals(_command, null))
        {
          CommandType = string.Empty;
          Parameters = null;
          return;
        }

        CommandType = _command.GetType().FullName;
        Parameters = _command.Parameters;
      }
    }

    [XmlAttribute]
    public string CommandType { get; set; }

    [XmlElement("parameter")]
    public string[] Parameters { get; set; }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ButtonMapping"/> class.
    /// </summary>
    public ButtonMapping()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ButtonMapping"/> class.
    /// </summary>
    /// <param name="keyCode">The remote key code.</param>
    /// <param name="description">The description.</param>
    /// <param name="command">The command to execute for this remote button.</param>
    public ButtonMapping(string keyCode, string description, Command command)
    {
      KeyCode = keyCode;
      Description = description;
      Command = command;
    }

    #endregion Constructors

    #region Implementation

    public string GetCommandDisplayText()
    {
      if (!ReferenceEquals(Command, null))
        return Command.UserDisplayText;

      if (!string.IsNullOrEmpty(CommandType))
        return "!!!" + CommandType;

      return string.Empty;
    }

    [XmlIgnore]
    public bool IsCommandAvailable
    {
      get { return !ReferenceEquals(Command, null); }
    }

    #endregion Implementation
  }
}