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

namespace MediaPortal.Input
{
  /// <summary>
  /// This class can be used to represent an IR Server Suite commands, which is not available.
  /// i.e. because of a missing command plugin or a renamed class name etc.
  /// </summary>
  public class DummyCondition
  {
    #region Properties

    /// <summary>
    /// Gets or sets the condition class fullname.
    /// </summary>
    /// <value>The condition parameters.</value>
    public string ConditionType { get; set; }

    /// <summary>
    /// Gets or sets the condition property.
    /// </summary>
    /// <value>The condition property.</value>
    public string Property { get; set; }

    /// <summary>
    /// Gets the user interface text.
    /// This method must be replaced in sub-classes.
    /// </summary>
    /// <value>The user interface text.</value>
    public string UserInterfaceText
    {
      get { return "!!! " + ConditionType; }
    }

    /// <summary>
    /// Gets the user display text.
    /// </summary>
    /// <value>The user display text.</value>
    public string UserDisplayText
    {
      get
      {
        if (Property == null)
          return UserInterfaceText;
        else
          return String.Format("{0} ({1})", UserInterfaceText, Property);
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="DummyCondition"/> class.
    /// </summary>
    /// <param name="conditionType">The condition type (class fullname).</param>
    /// <param name="property">The condition property.</param>
    public DummyCondition(string conditionType, string property)
    {
      ConditionType = conditionType;
      Property = property;
    }

    #endregion Constructors
  }
}