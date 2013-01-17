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
using System.Xml.Serialization;
using IrssCommands;
using IrssUtils;

namespace MediaPortal.Plugins.IRSS.MPBlastZonePlugin
{
  /// <summary>
  /// Represents the root element of the menu tree
  /// </summary>
  [XmlRoot("root")]
  public class MenuRoot
  {
    #region Properties

    [XmlAttribute("version")]
    public int Version
    {
      get { return 4; }
      set { return; }
    }
    
    [XmlElement("collection")]
    public List<MenuFolder> Items { get; set; }

    #endregion Properties

    #region Constructors

    public MenuRoot()
    {
      Items = new List<MenuFolder>();
    }

    #endregion Constructors

    #region Static Methods

    /// <summary>
    /// Save the supplied <see cref="MenuRoot"/> to file.
    /// </summary>
    /// <param name="menu"><see cref="MenuRoot"/> to save.</param>
    /// <param name="fileName">File to save to.</param>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public static bool Save(MenuRoot menu, string fileName)
    {
      try
      {
        XmlSerializer writer = new XmlSerializer(typeof(MenuRoot));
        using (StreamWriter file = new StreamWriter(fileName))
          writer.Serialize(file, menu);

        return true;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        return false;
      }
    }

    /// <summary>
    /// Load <see cref="MenuRoot"/> from a file.
    /// </summary>
    /// <param name="fileName">File to load from.</param>
    /// <returns>Loaded <see cref="MenuRoot"/>.</returns>
    public static MenuRoot Load(string fileName)
    {
      MenuRoot menu;

      try
      {
        //UpdateMappingFileFormat(fileName);

        XmlSerializer reader = new XmlSerializer(typeof(MenuRoot));
        using (StreamReader file = new StreamReader(fileName))
          menu = (MenuRoot)reader.Deserialize(file);
      }
      catch (FileNotFoundException)
      {
        IrssLog.Warn("No input mapping file found ({0})", fileName);
        menu = new MenuRoot();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        menu = new MenuRoot();
      }

      return menu;
    }

    #endregion Static Methods
  }

  /// <summary>
  /// Represents a folder of commands
  /// </summary>
  public class MenuFolder
  {
    #region Constructors

    public MenuFolder() 
    {
      Items = new List<MenuCommand>();
      Name = "New Collection";
    }

    public MenuFolder(string name)
      : this()
    {
      Name = name;
    }

    #endregion Constructors

    #region Properties

    [XmlAttribute("Name")]
    public string Name { get; set; }

    [XmlElement("MenuCommand")]
    public List<MenuCommand> Items { get; set; }

    #endregion Properties
  }

  /// <summary>
  /// Represents a menu item and it's command
  /// </summary>
  public class MenuCommand
  {
    #region Variables

    private Command _command;

    #endregion Variables

    //#region Constructors

    //public MenuCommand(string name, string command)
    //{
    //  Name = name;
    //  Command = command;
    //}

    //#endregion Constructors

    #region Properties

    [XmlAttribute("Name")]
    public string Name { get; set; }

    /// <summary>
    /// Command to execute
    /// </summary>
    [XmlIgnore]
    public Command Command
    {
      get
      {
        if (_command == null && !string.IsNullOrEmpty(CommandType))
        {
          try
          {
            _command = Processor.CreateCommand(CommandType, Parameters);
          }
          catch (Exception ex)
          {
            IrssUtils.IrssLog.Error(
              "Command could not be created. Please check your commands library ({0}) for correct command plugins or replace existing mapping with available commands.",
              ex, Processor.LibraryFolder);
          }
        }

        return _command;
      }
      set
      {
        _command = value;

        if (ReferenceEquals(_command, null))
        {
          CommandType = string.Empty;
          Parameters = null;
          return;
        }

        CommandType = _command.GetType().FullName;
        Parameters = _command.Parameters;
      }
    }

    [XmlAttribute]
    public string CommandType { get; set; }

    [XmlElement("parameter")]
    public string[] Parameters { get; set; }

    #endregion Properties

    #region Implementation

    [XmlIgnore]
    public bool IsCommandAvailable
    {
      get { return !ReferenceEquals(Command, null); }
    }

    [XmlIgnore]
    public DummyCommand DummyCommand
    {
      get { return new DummyCommand(CommandType, Parameters); }
    }

    public string GetCommandDisplayTextSafe()
    {
      if (IsCommandAvailable)
        return Command.UserDisplayText;

      return DummyCommand.UserDisplayText;
    }

    public object GetCommandSafe()
    {
      if (IsCommandAvailable)
        return Command;

      return DummyCommand;
    }

    #endregion Implementation
  }
}