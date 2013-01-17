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
using MediaPortal.GUI.Library;

namespace MediaPortal.Input
{
  /// <summary>
  /// Condition to check for the current active window.
  /// </summary>
  public class WindowCondition : Condition
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowCondition"/> class.
    /// </summary>
    public WindowCondition()
    {
      Property = "0";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowCondition"/> class.
    /// </summary>
    /// <param name="property">The condition property.</param>
    public WindowCondition(string property)
      : base(property)
    {
    }

    #endregion Constructors

    #region Implementation

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <value>User interface text.</value>
    public override string UserInterfaceText
    {
      get { return "Window Condition"; }
    }

    /// <summary>
    /// Gets the user display text.
    /// </summary>
    /// <value>The user display text.</value>
    public override string UserDisplayText
    {
      get
      {
        // no property, use interface text only
        if (Property == null)
          return UserInterfaceText;

        return String.Format("{0} ({1})", UserInterfaceText, MPUtils.MPCommon.GetFriendlyWindowName(int.Parse(Property)));
      }
    }

    /// <summary>
    /// Gets the edit control to be used within a common edit form.
    /// </summary>
    /// <returns>The edit control.</returns>
    public override BaseConditionConfig GetEditControl()
    {
      return new WindowConditionConfig(Property);
    }

    /// <summary>
    /// Validate the condition.
    /// </summary>
    public override bool Validate()
    {
      int windowID = int.Parse(Property);

      return (windowID == GUIWindowManager.ActiveWindowEx && !GUIWindowManager.IsOsdVisible) || 
             (windowID == (int) GUIWindowManager.VisibleOsd);
    }

    #endregion Implementation
  }
}