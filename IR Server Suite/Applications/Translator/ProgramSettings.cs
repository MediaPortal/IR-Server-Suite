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

    #region Variables

    private readonly List<ButtonMapping> _buttonMappings;
    private string _arguments;
    private string _fileName;
    private string _folder;
    private bool _forceWindowFocus;
    private bool _ignoreSystemWide;
    private string _name;
    private bool _useShellExecute;
    private ProcessWindowStyle _windowState;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Program name.
    /// </summary>
    [XmlAttribute]
    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    /// <summary>
    /// Program file name.
    /// </summary>
    [XmlAttribute]
    public string FileName
    {
      get { return _fileName; }
      set { _fileName = value; }
    }

    /// <summary>
    /// Program start folder.
    /// </summary>
    [XmlAttribute]
    public string Folder
    {
      get { return _folder; }
      set { _folder = value; }
    }

    /// <summary>
    /// Program launch command line arguments.
    /// </summary>
    [XmlAttribute]
    public string Arguments
    {
      get { return _arguments; }
      set { _arguments = value; }
    }

    /// <summary>
    /// Launch using shell execute.
    /// </summary>
    [XmlAttribute]
    public bool UseShellExecute
    {
      get { return _useShellExecute; }
      set { _useShellExecute = value; }
    }

    /// <summary>
    /// Force the new progam's window to be focused.
    /// </summary>
    [XmlAttribute]
    public bool ForceWindowFocus
    {
      get { return _forceWindowFocus; }
      set { _forceWindowFocus = value; }
    }

    /// <summary>
    /// Ignore system-wide Translator button mappings
    /// </summary>
    [XmlAttribute]
    public bool IgnoreSystemWide
    {
      get { return _ignoreSystemWide; }
      set { _ignoreSystemWide = value; }
    }

    /// <summary>
    /// Startup window state.
    /// </summary>
    [XmlAttribute]
    public ProcessWindowStyle WindowState
    {
      get { return _windowState; }
      set { _windowState = value; }
    }

    /// <summary>
    /// Gets a list of button mappings associated with this program.
    /// </summary>
    [XmlArray]
    public List<ButtonMapping> ButtonMappings
    {
      get { return _buttonMappings; }
    }


    /// <summary>
    /// Get the Command String to launch the program.
    /// </summary>
    /// <returns>Returns the Command String to launch the program.</returns>
    [XmlIgnore]
    internal string RunCommandString
    {
      get
      {
        return String.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}",
                             _fileName,
                             _folder,
                             _arguments,
                             _windowState,
                             false,
                             _useShellExecute,
                             false,
                             _forceWindowFocus);
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ProgramSettings"/> class.
    /// </summary>
    public ProgramSettings()
    {
      _name = NewProgramName;
      _fileName = String.Empty;
      _folder = String.Empty;
      _arguments = String.Empty;
      _useShellExecute = false;
      _forceWindowFocus = false;
      _ignoreSystemWide = false;
      _windowState = ProcessWindowStyle.Normal;
      _buttonMappings = new List<ButtonMapping>();
    }

    #endregion Constructors
  }
}