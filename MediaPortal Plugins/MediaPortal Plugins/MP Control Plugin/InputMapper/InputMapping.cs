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
using System.Xml;
using System.Xml.Serialization;
using IrssCommands;
using IrssUtils;
using MediaPortal.Input;

namespace MediaPortal.Plugins.IRSS.MPControlPlugin
{
  [XmlRoot("mappings")]
  public class InputMapping
  {
    #region Properties

    [XmlAttribute("version")]
    public int Version
    {
      get { return 4; }
      set { return; }
    }

    [XmlElement("remote")]
    public List<Remote> Remotes { get; set; }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="InputMapping"/> class.
    /// </summary>
    public InputMapping()
    {
      Remotes = new List<Remote>();
    }

    #endregion Constructors

    #region Static Methods

    /// <summary>
    /// Save the supplied <see cref="InputMapping"/> to file.
    /// </summary>
    /// <param name="config"><see cref="InputMapping"/> to save.</param>
    /// <param name="fileName">File to save to.</param>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public static bool Save(InputMapping config, string fileName)
    {
      try
      {
        XmlSerializer writer = new XmlSerializer(typeof (InputMapping));
        using (StreamWriter file = new StreamWriter(fileName))
          writer.Serialize(file, config);

        return true;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        return false;
      }
    }

    /// <summary>
    /// Load <see cref="InputMapping"/> from a file.
    /// </summary>
    /// <param name="fileName">File to load from.</param>
    /// <returns>Loaded <see cref="InputMapping"/>.</returns>
    public static InputMapping Load(string fileName)
    {
      InputMapping mappings;

      try
      {
        UpdateMappingFileFormat(fileName);

        XmlSerializer reader = new XmlSerializer(typeof (InputMapping));
        using (StreamReader file = new StreamReader(fileName))
          mappings = (InputMapping) reader.Deserialize(file);
      }
      catch (FileNotFoundException)
      {
        IrssLog.Warn("No input mapping file found ({0})", fileName);
        mappings = new InputMapping();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        mappings = new InputMapping();
      }

      return mappings;
    }

    #endregion Static Methods

    #region Nested Classes

    public class Remote
    {
      [XmlAttribute("family")]
      public string Family { get; set; }

      [XmlElement("button")]
      public List<Button> Buttons { get; set; }

      public Remote()
      {
        Buttons = new List<Button>();
      }
    }

    public class Button
    {
      [XmlAttribute("name")]
      public string Name { get; set; }

      [XmlAttribute("code")]
      public string Code { get; set; }

      [XmlElement("action")]
      public List<Action> Actions { get; set; }

      public Button()
      {
        Actions = new List<Action>();
      }
    }
    //public class Mapping
    //{
    //  public Mapping(int newLayer, string newCondition, string newConProperty, string newCommand,
    //                 string newCmdProperty, int newCmdKeyChar, int newCmdKeyCode, string newSound, bool newFocus)
    //  {
    //    Layer = newLayer;
    //    Condition = newCondition;
    //    ConProperty = newConProperty;
    //    Command = newCommand;
    //    CmdProperty = newCmdProperty;
    //    CmdKeyChar = newCmdKeyChar;
    //    CmdKeyCode = newCmdKeyCode;
    //    Sound = newSound;
    //    Focus = newFocus;
    //  }

    //  public int Layer { get; private set; }
    //  public string Condition { get; private set; }
    //  public string ConProperty { get; private set; }
    //  public string Command { get; private set; }
    //  public string CmdProperty { get; private set; }
    //  public int CmdKeyChar { get; private set; }
    //  public int CmdKeyCode { get; private set; }
    //  public string Sound { get; private set; }
    //  public bool Focus { get; private set; }
    //}

    public class Action
    {
      private Condition _condition;
      private Command _command;

      [XmlAttribute("layer")]
      public int Layer { get; set; }

      [XmlAttribute("sound")]
      public string Sound { get; set; }

      /// <summary>
      /// Condition to validate
      /// </summary>
      [XmlIgnore]
      public Condition Condition
      {
        get
        {
          if (_condition == null && !string.IsNullOrEmpty(ConditionType))
          {
            try
            {
              _condition = Condition.CreateCondition(ConditionType, ConditionProperty);
            }
            catch (Exception ex)
            {
              IrssUtils.IrssLog.Error("Condition could not be created.", ex);
            }
          }

          return _condition;
        }
        set
        {
          _condition = value;

          if (ReferenceEquals(_condition, null))
          {
            ConditionType = string.Empty;
            ConditionProperty = null;
            return;
          }

          ConditionType = _condition.GetType().FullName;
          ConditionProperty = _condition.Property;
        }
      }

      [XmlAttribute]
      public string ConditionType { get; set; }

      [XmlAttribute]
      public string ConditionProperty { get; set; }

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

      #region Implementation

      [XmlIgnore]
      public bool IsCommandAvailable
      {
        get { return !ReferenceEquals(Command, null); }
      }

      [XmlIgnore]
      public DummyCommand DummyCommand
      {
        get { return new DummyCommand(CommandType,Parameters); }
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

      [XmlIgnore]
      public bool IsConditionAvailable
      {
        get { return !ReferenceEquals(Condition, null); }
      }

      [XmlIgnore]
      public DummyCondition DummyCondition
      {
        get { return new DummyCondition(ConditionType, ConditionProperty); }
      }

      public string GetConditionDisplayTextSafe()
      {
        if (IsConditionAvailable)
          return Condition.UserDisplayText;

        return DummyCondition.UserDisplayText;
      }

      public object GetConditionSafe()
      {
        if (IsConditionAvailable)
          return Condition;

        return DummyCondition;
      }

      #endregion Implementation
    }

    #endregion

    #region Backward compatibility

    private static void UpdateMappingFileFormat(string file)
    {
      if (!File.Exists(file)) return;

      // check for v3
      InputMapping inputMapping = LoadMappingV3(file);
      if (!ReferenceEquals(inputMapping, null))
      {
        string fileName = Path.GetFileNameWithoutExtension(file);

        // copy old format to a backup file
        string backupFileName = fileName + "_BACKUP_" + DateTime.Now.ToString("yyyymmdd_hhmm");
        File.Copy(file, MPUtils.MPCommon.GetCustomMappingFilePath(backupFileName));

        // save new mapping format file to custom location
        Save(inputMapping, MPUtils.MPCommon.GetCustomMappingFilePath(fileName));
      }
    }

    private static InputMapping LoadMappingV3(string file)
    {
      InputMapping mapping = new InputMapping();

      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNodeList listRemotes = doc.DocumentElement.SelectNodes("/mappings/remote");
        foreach (XmlNode nodeRemote in listRemotes)
        {
          Remote remote = new Remote();
          remote.Family = nodeRemote.Attributes["family"].Value;

          XmlNodeList listButtons = nodeRemote.SelectNodes("button");
          foreach (XmlNode nodeButton in listButtons)
          {
            Button button = new Button();
            button.Name = nodeButton.Attributes["name"].Value;
            button.Code = nodeButton.Attributes["code"].Value;

            XmlNodeList listActions = nodeButton.SelectNodes("action");
            foreach (XmlNode nodeAction in listActions)
            {
              Action action = new Action();

              #region Layer attribute

              action.Layer = Convert.ToInt32(nodeAction.Attributes["layer"].Value);

              #endregion

              #region Sound attribute

              XmlAttribute soundAttribute = nodeAction.Attributes["sound"];
              if (soundAttribute != null && !string.IsNullOrEmpty(soundAttribute.Value) &&
                  !soundAttribute.Value.Equals("No Sound"))
                action.Sound = soundAttribute.Value;

              #endregion

              #region Condition attributes

              string conProperty = nodeAction.Attributes["conproperty"].Value;
              switch (nodeAction.Attributes["condition"].Value.ToUpper())
              {
                case "WINDOW":
                  action.ConditionType = "MediaPortal.Input.WindowCondition";
                  action.ConditionProperty = conProperty;
                  break;

                case "FULLSCREEN":
                  action.ConditionType = "MediaPortal.Input.FullscreenCondition";
                  action.ConditionProperty = conProperty.ToUpper() == "TRUE" ? true.ToString() : false.ToString();
                  break;

                case "PLAYER":
                  action.ConditionType = "MediaPortal.Input.PlayerCondition";
                  action.ConditionProperty = conProperty;
                  break;

                case "PLUGIN":
                  action.ConditionType = "MediaPortal.Input.PluginEnabledCondition";
                  action.ConditionProperty = conProperty;
                  break;

                case "*":
                  action.ConditionType = "MediaPortal.Input.NoCondition";
                  break;
              }

              #endregion

              #region Commands

              // read focus attribute
              bool gainFocus = false;
              XmlAttribute focusAttribute = nodeAction.Attributes["focus"];
              if (focusAttribute != null)
                gainFocus = Convert.ToBoolean(focusAttribute.Value);
              // convert it into a 'IrssCommands.MediaPortal.CommandGetFocus' action
              if (gainFocus)
              {
                action.CommandType = "IrssCommands.MediaPortal.CommandGetFocus";
                // stop here as command has been set, no need to read command attributes
              }
              else
              {
                string commandString = String.Empty;
                string command = nodeAction.Attributes["command"].Value.ToUpper();
                string cmdProperty = nodeAction.Attributes["cmdproperty"].Value; // .ToUpper()

                switch (command)
                {
                  case "ACTION":
                    {
                      if (cmdProperty == "93")
                      {
                        action.CommandType = "IrssCommands.MediaPortal.CommandSendKey";
                        string cmdKeyChar = nodeAction.Attributes["cmdkeychar"].Value;
                        string cmdKeyCode = nodeAction.Attributes["cmdkeycode"].Value;
                        action.Parameters = new string[] {cmdKeyChar, cmdKeyCode};
                      }
                      else
                      {
                        action.CommandType = "IrssCommands.MediaPortal.CommandSendAction";
                        action.Parameters = new string[] {cmdProperty, "0", "0"};
                      }
                    }
                    break;

                  case "KEY":
                    action.CommandType = "IrssCommands.General.KeystrokesCommand";
                    action.Parameters = new string[] { cmdProperty };
                    break;

                  case "WINDOW":
                    action.CommandType = "IrssCommands.MediaPortal.CommandGotoScreen";
                    action.Parameters = new string[] {cmdProperty};
                    break;

                  case "TOGGLE":
                    action.CommandType = "IrssCommands.MediaPortal.CommandToggleLayer";
                    break;

                  case "POWER":
                    switch (cmdProperty.ToUpper())
                    {
                      case "EXIT":
                        action.CommandType = "IrssCommands.MediaPortal.CommandExit";
                        break;
                      case "REBOOT":
                        action.CommandType = "IrssCommands.MediaPortal.CommandReboot";
                        break;
                      case "SHUTDOWN":
                        action.CommandType = "IrssCommands.MediaPortal.CommandShutdown";
                        break;
                      case "STANDBY":
                        action.CommandType = "IrssCommands.MediaPortal.CommandStandBy";
                        break;
                      case "HIBERNATE":
                        action.CommandType = "IrssCommands.MediaPortal.CommandHibernate";
                        break;
                    }
                    break;

                  case "PROCESS":
                    action.CommandType = "IrssCommands.General.CloseProgramCommand";
                    action.Parameters = new string[] {"Application", cmdProperty};
                    break;

                  case "BLAST":
                    action.CommandType = "IrssCommands.CommandBlastIR";
                    action.Parameters = new string[] {cmdProperty, "0"};
                    break;
                }
              }

              #endregion

              button.Actions.Add(action);
            }

            remote.Buttons.Add(button);
          }

          mapping.Remotes.Add(remote);
        }

        return mapping;
      }
      catch (Exception ex)
      {
        return null;
      }
    }

    #endregion
  }
}