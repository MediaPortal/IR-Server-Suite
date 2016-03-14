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
using System.Text;
using System.Xml;

namespace MediaPortal.Plugins.IRSS.MPBlastZonePlugin
{
  /// <summary>
  /// Represents the root element of the menu tree
  /// </summary>
  internal class MenuRoot
  {
    #region Variables

    private readonly List<MenuFolder> _items;

    #endregion Variables

    #region Constructors

    public MenuRoot()
    {
      _items = new List<MenuFolder>();
    }

    public MenuRoot(string fileName) : this()
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(fileName);

      XmlNodeList listCollections = doc.DocumentElement.SelectNodes("collection");

      foreach (XmlNode nodeCollection in listCollections)
      {
        MenuFolder newCollection = new MenuFolder(nodeCollection.Attributes["name"].Value);
        _items.Add(newCollection);

        foreach (XmlNode nodeCommand in nodeCollection.SelectNodes("command"))
        {
          MenuCommand newCommand = new MenuCommand(nodeCommand.Attributes["name"].Value,
                                                   nodeCommand.Attributes["value"].Value);
          newCollection.Add(newCommand);
        }
      }
    }

    #endregion Constructors

    #region Methods

    public void Save(string fileName)
    {
      using (XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.UTF8))
      {
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 1;
        writer.IndentChar = (char) 9;
        writer.WriteStartDocument(true);
        writer.WriteStartElement("menu"); // <menu>

        foreach (MenuFolder collection in _items)
        {
          writer.WriteStartElement("collection"); // <collection>
          writer.WriteAttributeString("name", collection.Name);

          foreach (string command in collection.GetAllItems())
          {
            writer.WriteStartElement("command"); // <command>
            writer.WriteAttributeString("name", collection.GetItem(command).Name);
            writer.WriteAttributeString("value", collection.GetItem(command).Command);

            writer.WriteEndElement(); // </command>
          }

          writer.WriteEndElement(); // </collection>
        }

        writer.WriteEndElement(); // </menu>
        writer.WriteEndDocument();
      }
    }

    public MenuFolder GetItem(string name)
    {
      foreach (MenuFolder item in _items)
        if (item.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
          return item;

      return null;
    }

    public void Add(MenuFolder item)
    {
      _items.Add(item);
    }

    public void Remove(MenuFolder item)
    {
      _items.Remove(item);
    }

    public void Clear()
    {
      _items.Clear();
    }

    public string[] GetAllItems()
    {
      string[] items = new string[_items.Count];
      int index = 0;
      foreach (MenuFolder item in _items)
        items[index++] = item.Name;

      return items;
    }

    #endregion Methods
  }

  /// <summary>
  /// Represents a folder of commands
  /// </summary>
  internal class MenuFolder
  {
    #region Variables

    private readonly List<MenuCommand> _items;
    private readonly string _name;

    #endregion Variables

    #region Constructors

    public MenuFolder() : this("New Collection")
    {
    }

    public MenuFolder(string name)
    {
      _items = new List<MenuCommand>();

      _name = name;
    }

    #endregion Constructors

    #region Properties

    public string Name
    {
      get { return _name; }
    }

    #endregion Properties

    #region Methods

    public MenuCommand GetItem(string name)
    {
      foreach (MenuCommand item in _items)
        if (item.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
          return item;

      return null;
    }

    public void Add(MenuCommand item)
    {
      _items.Add(item);
    }

    public void Remove(MenuCommand item)
    {
      _items.Remove(item);
    }

    public void Clear()
    {
      _items.Clear();
    }

    public string[] GetAllItems()
    {
      string[] items = new string[_items.Count];
      int index = 0;
      foreach (MenuCommand item in _items)
        items[index++] = item.Name;

      return items;
    }

    #endregion Methods
  }

  /// <summary>
  /// Represents a menu item and it's command
  /// </summary>
  internal class MenuCommand
  {
    #region Variables

    private string _command;
    private string _name;

    #endregion Variables

    #region Constructors

    public MenuCommand() : this("New Command", String.Empty)
    {
    }

    public MenuCommand(string name, string command)
    {
      _name = name;
      _command = command;
    }

    #endregion Constructors

    #region Properties

    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    public string Command
    {
      get { return _command; }
      set { _command = value; }
    }

    #endregion Properties
  }
}