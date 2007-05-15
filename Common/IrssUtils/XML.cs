using System;
using System.Collections.Generic;
using System.Text;
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
        if (node.Name == property)
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
        if (node.Name == property)
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
        if (node.Name == property)
        {
          int.TryParse(node.InnerText, out returnValue);
          break;
        }
      }

      return returnValue;
    }

  }

}
