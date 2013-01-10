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
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Translator
{
  /// <summary>
  /// Holds settings for a program that is mapped in Translator.
  /// </summary>
  public class ProgramSettings
  {
    #region Constants

    /// <summary>
    /// The default name given to a new program settings object.
    /// </summary>
    internal const string NewProgramName = "New Program";

    #endregion Constants

    #region Properties

    /// <summary>
    /// Program name.
    /// </summary>
    [XmlAttribute]
    public string Name { get; set; }

    /// <summary>
    /// Program file name.
    /// </summary>
    [XmlAttribute]
    public string FileName { get; set; }

    /// <summary>
    /// Program start folder.
    /// </summary>
    [XmlAttribute]
    public string Folder { get; set; }

    /// <summary>
    /// Program launch command line arguments.
    /// </summary>
    [XmlAttribute]
    public string Arguments { get; set; }

    /// <summary>
    /// Launch using shell execute.
    /// </summary>
    [XmlAttribute]
    public bool UseShellExecute { get; set; }

    /// <summary>
    /// Force the new progam's window to be focused.
    /// </summary>
    [XmlAttribute]
    public bool ForceWindowFocus { get; set; }

    /// <summary>
    /// Ignore system-wide Translator button mappings
    /// </summary>
    [XmlAttribute]
    public bool IgnoreSystemWide { get; set; }

    /// <summary>
    /// Startup window state.
    /// </summary>
    [XmlAttribute]
    public ProcessWindowStyle WindowState { get; set; }

    /// <summary>
    /// Gets a list of button mappings associated with this program.
    /// </summary>
    [XmlArray]
    public List<ButtonMapping> ButtonMappings { get; private set; }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ProgramSettings"/> class.
    /// </summary>
    public ProgramSettings()
    {
      Name = NewProgramName;
      FileName = String.Empty;
      Folder = String.Empty;
      Arguments = String.Empty;
      UseShellExecute = false;
      ForceWindowFocus = false;
      IgnoreSystemWide = false;
      WindowState = ProcessWindowStyle.Normal;
      ButtonMappings = new List<ButtonMapping>();
    }

    #endregion Constructors

    internal string[] GetRunCommandParameters()
    {
      return new[]
        {
          FileName.Trim(),
          Folder.Trim(),
          Arguments.Trim(),
          Enum.GetName(typeof (ProcessWindowStyle), WindowState),
          false.ToString(),
          UseShellExecute.ToString(),
          false.ToString(),
          ForceWindowFocus.ToString()
        };
    }
  }
}