using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace MediaPortal.Plugins
{

  /// <summary>
  /// Represents the root element of the menu tree
  /// </summary>
  class MenuRoot
  {

    #region Variables

    List<MenuFolder> _items;

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
          MenuCommand newCommand = new MenuCommand(nodeCommand.Attributes["name"].Value, nodeCommand.Attributes["value"].Value);
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
        writer.IndentChar = (char)9;
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
  class MenuFolder
  {

    #region Variables

    string _name;
    List<MenuCommand> _items;

    #endregion Variables

    #region Constructors

    public MenuFolder() : this("New Collection") { }
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
  class MenuCommand
  {

    #region Variables

    string _name;
    string _command;

    #endregion Variables

    #region Constructors

    public MenuCommand() : this("New Command", String.Empty) { }
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
