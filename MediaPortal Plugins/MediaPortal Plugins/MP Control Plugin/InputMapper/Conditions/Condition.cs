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
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MediaPortal.Input
{
  /// <summary>
  /// Base class for all MediaPortal conditions.
  /// </summary>
  public abstract class Condition
  {
    #region Properties

    /// <summary>
    /// Gets the value, wether this command can be tested.
    /// </summary>
    /// <value>Whether the command can be tested.</value>
    public virtual bool IsTestAllowed
    {
      get { return true; }
    }

    /// <summary>
    /// Gets or sets the command parameters.
    /// </summary>
    /// <value>The command parameters.</value>
    public virtual string Property { get; set; }

    /// <summary>
    /// Gets the user interface text.
    /// This method must be replaced in sub-classes.
    /// </summary>
    /// <value>The user interface text.</value>
    public abstract string UserInterfaceText { get; }

    /// <summary>
    /// Gets the user display text.
    /// </summary>
    /// <value>The user display text.</value>
    public virtual string UserDisplayText
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
    /// Initializes a new instance of the <see cref="Condition"/> class.
    /// </summary>
    protected Condition()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Condition"/> class.
    /// </summary>
    /// <param name="property">The condition property.</param>
    protected Condition(string property)
    {
      Property = property;
    }

    #endregion Constructors

    #region Implementation

    /// <summary>
    /// Validate the condition.
    /// </summary>
    public virtual bool Validate()
    {
      return false;
    }

    /// <summary>
    /// Edit this command.
    /// </summary>
    /// <param name="parent">The parent window.</param>
    /// <returns><c>true</c> if the command was modified; otherwise <c>false</c>.</returns>
    public virtual bool Edit(IWin32Window parent)
    {
      return true;
    }

    /// <summary>
    /// Gets the edit control to be used within a common edit form.
    /// </summary>
    /// <returns>The edit control.</returns>
    public virtual BaseConditionConfig GetEditControl()
    {
      return new NoConditionConfig();
    }

    /// <summary>
    /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
    /// </summary>
    /// <returns>A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.</returns>
    public override string ToString()
    {
      StringBuilder xml = new StringBuilder();
      using (StringWriter stringWriter = new StringWriter(xml))
      {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof (string[]));
        xmlSerializer.Serialize(stringWriter, Property);
      }

      return String.Format("{0}, {1}", GetType().FullName, xml);
    }

    #endregion Implementation

    #region Static Methods

    /// <summary>
    /// Gets the condition.
    /// </summary>
    /// <returns>List of condition types.</returns>
    public static Type[] GetBuiltInConditions()
    {
      List<Type> conditions = new List<Type>();

      conditions.Add(typeof(NoCondition));
      conditions.Add(typeof(FullscreenCondition));
      conditions.Add(typeof(PlayerCondition));
      conditions.Add(typeof(PluginIsEnabledCondition));
      conditions.Add(typeof(WindowCondition));

      return conditions.ToArray();
    }

    public static Condition CreateCondition(string conditionType, string conditionProperty)
    {
      object[] args = new object[] {conditionProperty};
      //foreach (Type type in allCommands)
      //  if (type.FullName.Equals(commandType))
      if (ReferenceEquals(conditionProperty, null))
        return (Condition) Activator.CreateInstance(Type.GetType(conditionType));
      else
        return (Condition) Activator.CreateInstance(Type.GetType(conditionType), args);
    }

    #endregion Static Methods
  }
}