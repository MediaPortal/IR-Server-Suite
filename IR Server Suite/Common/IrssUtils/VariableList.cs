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
using System.IO;
using System.Text;
using System.Xml;

namespace IrssUtils
{
  /// <summary>
  /// List of Variables for use in macros and commands.
  /// </summary>
  public class VariableList
  {
    #region Variables

    private readonly Dictionary<string, string> _variables;

    #endregion Variables

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="VariableList"/> class.
    /// </summary>
    public VariableList()
    {
      _variables = new Dictionary<string, string>();
    }

    #endregion Constructor

    #region Implementation

    /// <summary>
    /// Sets a string variable.
    /// </summary>
    /// <param name="variable">The variable.</param>
    /// <param name="value">The value.</param>
    public void SetVariable(string variable, string value)
    {
      if (_variables.ContainsKey(variable))
        _variables[variable] = value;
      else
        _variables.Add(variable, value);
    }

    /// <summary>
    /// Gets a string variable.
    /// </summary>
    /// <param name="variable">The variable.</param>
    /// <returns>The variable value.</returns>
    public string GetVariable(string variable)
    {
      if (_variables.ContainsKey(variable))
        return _variables[variable];
      else
        return String.Empty;
    }

    /// <summary>
    /// Clears all the variables from this instance of the Variable List.
    /// </summary>
    public void Clear()
    {
      _variables.Clear();
    }

    /// <summary>
    /// Saves the Variable List to a file.
    /// </summary>
    /// <param name="fileName">Name of the file to save to.</param>
    public void Save(string fileName)
    {
      if (String.IsNullOrEmpty(fileName))
        throw new ArgumentNullException("fileName");

      string path = Path.Combine(Common.FolderAppData, fileName + Common.FileExtensionVariableList);

      using (XmlTextWriter writer = new XmlTextWriter(path, Encoding.UTF8))
      {
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 1;
        writer.IndentChar = (char) 9;
        writer.WriteStartDocument(true);
        writer.WriteStartElement("variables"); // <variables>

        foreach (string key in _variables.Keys)
          writer.WriteElementString(key, _variables[key]);

        writer.WriteEndElement(); // </variables>
      }
    }

    /// <summary>
    /// Loads the variables from a given file into the current Variable List.
    /// </summary>
    /// <param name="fileName">Name of the file to load.</param>
    public void Load(string fileName)
    {
      if (String.IsNullOrEmpty(fileName))
        throw new ArgumentNullException("fileName");

      string path = Path.Combine(Common.FolderAppData, fileName + Common.FileExtensionVariableList);

      XmlDocument doc = new XmlDocument();
      doc.Load(path);

      XmlNodeList variableNodes = doc.DocumentElement.SelectNodes("VariableList");

      foreach (XmlNode node in variableNodes)
        SetVariable(node.Name, node.Value);
    }

    #endregion Implementation
  }
}