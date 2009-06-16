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
using System.Xml;

namespace IrssUtils
{
  /// <summary>
  /// XML access utils.
  /// </summary>
  public static class XML
  {
    /// <summary>
    /// Given a list of nodes, return a requested property or it's default value if not present.
    /// </summary>
    /// <param name="nodeList">List of xml nodes to search.</param>
    /// <param name="property">Propery to locate value for.</param>
    /// <param name="defaultValue">Default value if the property doesn't exist.</param>
    /// <returns>InnerText from node matching property name.</returns>
    public static string GetString(XmlNodeList nodeList, string property, string defaultValue)
    {
      if (nodeList == null || String.IsNullOrEmpty(property))
        return defaultValue;

      foreach (XmlNode node in nodeList)
        if (node.Name.Equals(property, StringComparison.OrdinalIgnoreCase))
          return node.InnerText;

      return defaultValue;
    }

    /// <summary>
    /// Given a list of nodes, return a requested property or it's default value if not present.
    /// </summary>
    /// <param name="nodeList">List of xml nodes to search.</param>
    /// <param name="property">Propery to locate value for.</param>
    /// <param name="defaultValue">Default value if the property doesn't exist.</param>
    /// <returns>InnerText from node matching property name.</returns>
    public static bool GetBool(XmlNodeList nodeList, string property, bool defaultValue)
    {
      if (nodeList == null || String.IsNullOrEmpty(property))
        return defaultValue;

      bool returnValue = defaultValue;
      foreach (XmlNode node in nodeList)
      {
        if (node.Name.Equals(property, StringComparison.OrdinalIgnoreCase))
        {
          bool.TryParse(node.InnerText, out returnValue);
          break;
        }
      }

      return returnValue;
    }

    /// <summary>
    /// Given a list of nodes, return a requested property or it's default value if not present.
    /// </summary>
    /// <param name="nodeList">List of xml nodes to search.</param>
    /// <param name="property">Propery to locate value for.</param>
    /// <param name="defaultValue">Default value if the property doesn't exist.</param>
    /// <returns>InnerText from node matching property name.</returns>
    public static int GetInt(XmlNodeList nodeList, string property, int defaultValue)
    {
      if (nodeList == null || String.IsNullOrEmpty(property))
        return defaultValue;

      int returnValue = defaultValue;
      foreach (XmlNode node in nodeList)
      {
        if (node.Name.Equals(property, StringComparison.OrdinalIgnoreCase))
        {
          int.TryParse(node.InnerText, out returnValue);
          break;
        }
      }

      return returnValue;
    }
  }
}