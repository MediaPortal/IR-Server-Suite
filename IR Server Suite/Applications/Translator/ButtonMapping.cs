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

using System.Xml.Serialization;

namespace Translator
{
  /// <summary>
  /// Stores the relevant information for mapping a remote button to a command.
  /// </summary>
  public class ButtonMapping
  {
    #region Variables

    private string _command;
    private string _description;
    private string _keyCode;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Remote button key code.
    /// </summary>
    [XmlAttribute]
    public string KeyCode
    {
      get { return _keyCode; }
      set { _keyCode = value; }
    }

    /// <summary>
    /// Remote button description.
    /// </summary>
    [XmlAttribute]
    public string Description
    {
      get { return _description; }
      set { _description = value; }
    }

    /// <summary>
    /// Command to execute for the remote button.
    /// </summary>
    [XmlAttribute]
    public string Command
    {
      get { return _command; }
      set { _command = value; }
    }

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
    public ButtonMapping(string keyCode, string description, string command)
    {
      _keyCode = keyCode;
      _description = description;
      _command = command;
    }

    #endregion Constructors
  }
}