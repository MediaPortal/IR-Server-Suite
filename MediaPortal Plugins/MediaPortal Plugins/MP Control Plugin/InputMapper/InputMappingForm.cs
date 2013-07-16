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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using IrssUtils;
using IrssUtils.Forms;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using MediaPortal.UserInterface.Controls;
using Action = MediaPortal.GUI.Library.Action;

namespace MediaPortal.Plugins
{
  ///<summary>
  /// 
  ///</summary>
  [CLSCompliant(false)]
  public partial class InputMappingForm : MPConfigForm
  {
    private readonly List<string> _actionList = new List<string>();
    private readonly List<string> _pluginList = new List<string>();

    private readonly string[] _fullScreenList = new string[] {"Fullscreen", "No Fullscreen"};
    private readonly string _inputClassName;
    private readonly string[] _layerList = new string[] {"all", "1", "2"};
    private readonly Array _nativeActionList = Enum.GetValues(typeof (Action.ActionType));

    private readonly string[] _nativePlayerList = new string[] {"TV", "DVD", "MEDIA", "MUSIC" };

    private readonly string[] _nativePowerList = new string[] {"EXIT", "REBOOT", "SHUTDOWN", "STANDBY", "HIBERNATE"};

    private readonly string[] _nativeProcessList = new string[] {"CLOSE", "KILL"};
    private readonly Array _nativeWindowsList = Enum.GetValues(typeof (GUIWindow.Window));
    private readonly string[] _playerList = new string[] {"TV is running", "DVD is playing", "Media is playing", "Music is playing" };

    private readonly string[] _powerList = new string[]
                                             {
                                               "Exit MediaPortal", "Reboot Windows", "Shutdown Windows",
                                               "Standby Windows"
                                               , "Hibernate Windows"
                                             };

    private readonly string[] _processList = new string[] {"Close Process", "Kill Process"};

    private readonly string[] _soundList = new string[] {"none", "back.wav", "click.wav", "cursor.wav"};
    private readonly ArrayList _windowsList = new ArrayList();
    private readonly ArrayList _windowsListFiltered = new ArrayList();

    private bool _changedSettings;


    /// <summary>
    /// Required designer variable.
    /// </summary>
    //private Container components;
    private string[] keyList = new string[]
                                 {
                                   "{BACKSPACE}", "{BREAK}", "{CAPSLOCK}", "{DELETE}", "{DOWN}", "{END}", "{ENTER}",
                                   "{ESC}",
                                   "{HELP}", "{HOME}", "{INSERT}", "{LEFT}", "{NUMLOCK}", "{PGDN}", "{PGUP}", "{PRTSC}",
                                   "{RIGHT}", "{SCROLLLOCK}", "{TAB}", "{UP}", "{F1}", "{F2}", "{F3}", "{F4}", "{F5}",
                                   "{F6}",
                                   "{F7}", "{F8}", "{F9}", "{F10}", "{F11}", "{F12}", "{F13}", "{F14}", "{F15}", "{F16}"
                                   ,
                                   "{ADD}", "{SUBTRACT}", "{MULTIPLY}", "{DIVIDE}"
                                 };

    ///<summary>
    ///</summary>
    ///<param name="name"></param>
    public InputMappingForm(string name)
    {
      InitializeComponent();

      foreach (GUIWindow.Window wnd in _nativeWindowsList)
      {
        if (wnd.ToString().IndexOf("DIALOG") == -1)
          switch ((int) Enum.Parse(typeof (GUIWindow.Window), wnd.ToString()))
          {
            case (int) GUIWindow.Window.WINDOW_ARTIST_INFO:
            case (int) GUIWindow.Window.WINDOW_DIALOG_DATETIME:
            case (int) GUIWindow.Window.WINDOW_DIALOG_EXIF:
            case (int) GUIWindow.Window.WINDOW_DIALOG_FILE:
            case (int) GUIWindow.Window.WINDOW_DIALOG_FILESTACKING:
            case (int) GUIWindow.Window.WINDOW_DIALOG_MENU:
            case (int) GUIWindow.Window.WINDOW_DIALOG_MENU_BOTTOM_RIGHT:
            case (int) GUIWindow.Window.WINDOW_DIALOG_NOTIFY:
            case (int) GUIWindow.Window.WINDOW_DIALOG_OK:
            case (int) GUIWindow.Window.WINDOW_DIALOG_PROGRESS:
            case (int) GUIWindow.Window.WINDOW_DIALOG_RATING:
            case (int) GUIWindow.Window.WINDOW_DIALOG_SELECT:
            case (int) GUIWindow.Window.WINDOW_DIALOG_SELECT2:
            case (int) GUIWindow.Window.WINDOW_DIALOG_TEXT:
            case (int) GUIWindow.Window.WINDOW_DIALOG_TVGUIDE:
            case (int) GUIWindow.Window.WINDOW_DIALOG_YES_NO:
            case (int) GUIWindow.Window.WINDOW_INVALID:
            case (int) GUIWindow.Window.WINDOW_MINI_GUIDE:
            case (int) GUIWindow.Window.WINDOW_TV_CROP_SETTINGS:
            case (int) GUIWindow.Window.WINDOW_MUSIC:
            case (int) GUIWindow.Window.WINDOW_MUSIC_COVERART_GRABBER_RESULTS:
            case (int) GUIWindow.Window.WINDOW_MUSIC_INFO:
            case (int) GUIWindow.Window.WINDOW_OSD:
            case (int) GUIWindow.Window.WINDOW_TOPBAR:
            case (int) GUIWindow.Window.WINDOW_TVOSD:
            case (int) GUIWindow.Window.WINDOW_TVZAPOSD:
            case (int) GUIWindow.Window.WINDOW_VIDEO_ARTIST_INFO:
            case (int) GUIWindow.Window.WINDOW_VIDEO_INFO:
            case (int) GUIWindow.Window.WINDOW_VIRTUAL_KEYBOARD:
              break;
            default:
              _windowsListFiltered.Add(GetFriendlyName(wnd.ToString()));
              break;
          }
        _windowsList.Add(GetFriendlyName(wnd.ToString()));
      }

      _actionList.Clear();
      foreach (Action.ActionType actn in _nativeActionList)
        _actionList.Add(GetFriendlyName(actn.ToString()));

      LoadPluginList();

      comboBoxSound.DataSource = _soundList;
      comboBoxLayer.DataSource = _layerList;
      _inputClassName = name;
      LoadMapping(_inputClassName + ".xml", false);
      headerLabel.Caption = _inputClassName;
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    //protected override void Dispose(bool disposing)
    //{
    //  if (disposing)
    //  {
    //    if (components != null)
    //    {
    //      components.Dispose();
    //    }
    //  }
    //  base.Dispose(disposing);
    //}
    private void CloseThread()
    {
      Thread.Sleep(200);
      Close();
    }

    private void LoadMapping(string xmlFile, bool defaults)
    {
      string pathDefault = Path.Combine(MPUtils.MPCommon.CustomInputDefault, xmlFile);
      string pathCustom = Path.Combine(MPUtils.MPCommon.CustomInputDevice, xmlFile);

      try
      {
        groupBoxLayer.Enabled = false;
        groupBoxCondition.Enabled = false;
        groupBoxAction.Enabled = false;
        treeMapping.Nodes.Clear();
        XmlDocument doc = new XmlDocument();
        string path = pathDefault;
        if (!defaults && File.Exists(pathCustom))
          path = pathCustom;
        if (!File.Exists(path))
        {
          MessageBox.Show(
            "Can't locate mapping file " + xmlFile + "\n\nMake sure it exists in /InputDeviceMappings/defaults",
            "Mapping file missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
          buttonUp.Enabled =
            buttonDown.Enabled =
            buttonNew.Enabled = buttonRemove.Enabled = buttonDefault.Enabled = buttonApply.Enabled = false;
          ShowInTaskbar = true;
          WindowState = FormWindowState.Minimized;
          Thread closeThread = new Thread(CloseThread);
          closeThread.Start();
          return;
        }
        doc.Load(path);
        XmlNodeList listRemotes = doc.DocumentElement.SelectNodes("/mappings/remote");

        foreach (XmlNode nodeRemote in listRemotes)
        {
          TreeNode remoteNode = new TreeNode(nodeRemote.Attributes["family"].Value);
          remoteNode.Tag = new Data("REMOTE", null, nodeRemote.Attributes["family"].Value);
          XmlNodeList listButtons = nodeRemote.SelectNodes("button");
          foreach (XmlNode nodeButton in listButtons)
          {
            TreeNode buttonNode = new TreeNode(nodeButton.Attributes["name"].Value);
            buttonNode.Tag = new Data("BUTTON", nodeButton.Attributes["name"].Value, nodeButton.Attributes["code"].Value);
            remoteNode.Nodes.Add(buttonNode);

            TreeNode layer1Node = new TreeNode("Layer 1");
            TreeNode layer2Node = new TreeNode("Layer 2");
            TreeNode layerAllNode = new TreeNode("All Layers");
            layer1Node.Tag = new Data("LAYER", null, "1");
            layer2Node.Tag = new Data("LAYER", null, "2");
            layerAllNode.Tag = new Data("LAYER", null, "0");
            layer1Node.ForeColor = Color.DimGray;
            layer2Node.ForeColor = Color.DimGray;
            layerAllNode.ForeColor = Color.DimGray;

            XmlNodeList listActions = nodeButton.SelectNodes("action");

            foreach (XmlNode nodeAction in listActions)
            {
              string conditionString = String.Empty;
              string commandString = String.Empty;

              string condition = nodeAction.Attributes["condition"].Value.ToUpper();
              string conProperty = nodeAction.Attributes["conproperty"].Value; // .ToUpper()
              string command = nodeAction.Attributes["command"].Value.ToUpper();
              string cmdProperty = nodeAction.Attributes["cmdproperty"].Value; // .ToUpper()
              string sound = String.Empty;
              XmlAttribute soundAttribute = nodeAction.Attributes["sound"];
              if (soundAttribute != null)
                sound = soundAttribute.Value;
              bool gainFocus = false;
              XmlAttribute focusAttribute = nodeAction.Attributes["focus"];
              if (focusAttribute != null)
                gainFocus = Convert.ToBoolean(focusAttribute.Value);
              int layer = Convert.ToInt32(nodeAction.Attributes["layer"].Value);

              #region Conditions

              switch (condition)
              {
                case "WINDOW":
                  {
                    conProperty = conProperty.ToUpper();
                    try
                    {
                      conditionString =
                        GetFriendlyName(Enum.GetName(typeof (GUIWindow.Window), Convert.ToInt32(conProperty)));
                    }
                    catch
                    {
                      conditionString = conProperty;
                    }
                    break;
                  }
                case "FULLSCREEN":
                  conProperty = conProperty.ToUpper();
                  if (conProperty == "TRUE")
                    conditionString = "Fullscreen";
                  else
                    conditionString = "No Fullscreen";
                  break;
                case "PLAYER":
                  conProperty = conProperty.ToUpper();
                  conditionString = _playerList[Array.IndexOf(_nativePlayerList, conProperty)];
                  break;
                case "PLUGIN":
                  conditionString = conProperty;
                  break;
                case "*":
                  conditionString = "No Condition";
                  break;
              }

              #endregion

              #region Commands

              switch (command)
              {
                case "ACTION":
                  commandString = "Action \"" +
                                  GetFriendlyName(Enum.GetName(typeof (Action.ActionType), Convert.ToInt32(cmdProperty))) +
                                  "\"";
                  break;
                case "KEY":
                  commandString = "Key \"" + cmdProperty + "\"";
                  break;
                case "WINDOW":
                  {
                    try
                    {
                      commandString = "Window \"" +
                                      GetFriendlyName(Enum.GetName(typeof (GUIWindow.Window),
                                                                   Convert.ToInt32(cmdProperty))) + "\"";
                    }
                    catch
                    {
                      commandString = "Window \"" + cmdProperty + "\"";
                    }
                    break;
                  }
                case "TOGGLE":
                  commandString = "Toggle Layer";
                  break;
                case "POWER":
                  commandString = _powerList[Array.IndexOf(_nativePowerList, cmdProperty)];
                  break;
                case "PROCESS":
                  commandString = _processList[Array.IndexOf(_nativeProcessList, cmdProperty)];
                  break;
                case "BLAST":
                  commandString = cmdProperty;
                  break;
              }

              #endregion

              TreeNode conditionNode = new TreeNode(conditionString);
              conditionNode.Tag = new Data("CONDITION", condition, conProperty);
              if (commandString == "Action \"Key Pressed\"")
              {
                string cmdKeyChar = nodeAction.Attributes["cmdkeychar"].Value;
                string cmdKeyCode = nodeAction.Attributes["cmdkeycode"].Value;
                TreeNode commandNode = new TreeNode(String.Format("Key Pressed: {0} [{1}]", cmdKeyChar, cmdKeyCode));

                Key key = new Key(Convert.ToInt32(cmdKeyChar), Convert.ToInt32(cmdKeyCode));

                commandNode.Tag = new Data("COMMAND", "KEY", key, gainFocus);
                commandNode.ForeColor = Color.DarkGreen;
                conditionNode.ForeColor = Color.Blue;
                conditionNode.Nodes.Add(commandNode);
              }
              else
              {
                TreeNode commandNode = new TreeNode(commandString);
                commandNode.Tag = new Data("COMMAND", command, cmdProperty, gainFocus);
                commandNode.ForeColor = Color.DarkGreen;
                conditionNode.ForeColor = Color.Blue;
                conditionNode.Nodes.Add(commandNode);
              }

              TreeNode soundNode = new TreeNode(sound);
              soundNode.Tag = new Data("SOUND", null, sound);
              if (String.IsNullOrEmpty(sound))
                soundNode.Text = "No Sound";
              soundNode.ForeColor = Color.DarkRed;
              conditionNode.Nodes.Add(soundNode);

              if (layer == 1) layer1Node.Nodes.Add(conditionNode);
              if (layer == 2) layer2Node.Nodes.Add(conditionNode);
              if (layer == 0) layerAllNode.Nodes.Add(conditionNode);
            }
            if (layer1Node.Nodes.Count > 0) buttonNode.Nodes.Add(layer1Node);
            if (layer2Node.Nodes.Count > 0) buttonNode.Nodes.Add(layer2Node);
            if (layerAllNode.Nodes.Count > 0) buttonNode.Nodes.Add(layerAllNode);
          }
          treeMapping.Nodes.Add(remoteNode);
          if (listRemotes.Count == 1)
            remoteNode.Expand();
        }
        _changedSettings = false;
      }
      catch (Exception ex)
      {
        Log.Error(ex);
        File.Delete(pathCustom);
        LoadMapping(xmlFile, true);
      }
    }

    private bool SaveMapping(string xmlFile)
    {
      string customDir = MPUtils.MPCommon.CustomInputDevice;
      string pathCustom = Path.Combine(customDir, xmlFile);

      try
      {
        Directory.CreateDirectory(customDir);
      }
      catch
      {
        Log.Info("MAP: Error accessing directory '{0}'", customDir);
      }

      try
      {
        using (
          XmlTextWriter writer = new XmlTextWriter(pathCustom, Encoding.UTF8)
          )
        {
          writer.Formatting = Formatting.Indented;
          writer.Indentation = 1;
          writer.IndentChar = (char) 9;
          writer.WriteStartDocument(true);
          writer.WriteStartElement("mappings"); // <mappings>
          writer.WriteAttributeString("version", "3");
          if (treeMapping.Nodes.Count > 0)
            foreach (TreeNode remoteNode in treeMapping.Nodes)
            {
              writer.WriteStartElement("remote"); // <remote>
              writer.WriteAttributeString("family", (string) ((Data) remoteNode.Tag).Value);
              if (remoteNode.Nodes.Count > 0)
                foreach (TreeNode buttonNode in remoteNode.Nodes)
                {
                  writer.WriteStartElement("button"); // <button>
                  writer.WriteAttributeString("name", (string) ((Data) buttonNode.Tag).Parameter);
                  writer.WriteAttributeString("code", (string) ((Data) buttonNode.Tag).Value);

                  if (buttonNode.Nodes.Count > 0)
                    foreach (TreeNode layerNode in buttonNode.Nodes)
                    {
                      foreach (TreeNode conditionNode in layerNode.Nodes)
                      {
                        string layer;
                        string condition;
                        string conProperty;
                        string command = String.Empty;
                        string cmdProperty = String.Empty;
                        string cmdKeyChar = String.Empty;
                        string cmdKeyCode = String.Empty;
                        string sound = String.Empty;
                        bool focus = false;
                        foreach (TreeNode commandNode in conditionNode.Nodes)
                        {
                          switch (((Data) commandNode.Tag).Type)
                          {
                            case "COMMAND":
                              {
                                command = (string) ((Data) commandNode.Tag).Parameter;
                                focus = ((Data) commandNode.Tag).Focus;
                                if (command != "KEY")
                                  cmdProperty = ((Data) commandNode.Tag).Value.ToString();
                                else
                                {
                                  command = "ACTION";
                                  Key key = (Key) ((Data) commandNode.Tag).Value;
                                  cmdProperty = "93";
                                  cmdKeyChar = key.KeyChar.ToString();
                                  cmdKeyCode = key.KeyCode.ToString();
                                }
                              }
                              break;
                            case "SOUND":
                              sound = (string) ((Data) commandNode.Tag).Value;
                              break;
                          }
                        }
                        condition = (string) ((Data) conditionNode.Tag).Parameter;
                        conProperty = ((Data) conditionNode.Tag).Value.ToString();
                        layer = Convert.ToString(((Data) layerNode.Tag).Value);
                        writer.WriteStartElement("action"); // <action>
                        writer.WriteAttributeString("layer", layer);
                        writer.WriteAttributeString("condition", condition);
                        writer.WriteAttributeString("conproperty", conProperty);
                        writer.WriteAttributeString("command", command);
                        writer.WriteAttributeString("cmdproperty", cmdProperty);
                        if (cmdProperty == Convert.ToInt32(Action.ActionType.ACTION_KEY_PRESSED).ToString())
                        {
                          if (!String.IsNullOrEmpty(cmdKeyChar))
                          {
                            writer.WriteAttributeString("cmdkeychar", cmdKeyChar);
                          }
                          else
                          {
                            writer.WriteAttributeString("cmdkeychar", "0");
                          }
                          if (!String.IsNullOrEmpty(cmdKeyCode))
                          {
                            writer.WriteAttributeString("cmdkeycode", cmdKeyCode);
                          }
                          else
                          {
                            writer.WriteAttributeString("cmdkeychar", "0");
                          }
                        }
                        if (!String.IsNullOrEmpty(sound))
                          writer.WriteAttributeString("sound", sound);
                        if (focus)
                          writer.WriteAttributeString("focus", focus.ToString());
                        writer.WriteEndElement(); // </action>
                      }
                    }
                  writer.WriteEndElement(); // </button>
                }
              writer.WriteEndElement(); // </remote>
            }
          writer.WriteEndElement(); // </mapping>
          writer.WriteEndDocument();
        }
        _changedSettings = false;
        return true;
      }
      catch (Exception ex)
      {
        Log.Error("MAP: Error saving mapping to XML file: {0}", ex.Message);
        return false;
      }
    }

    private void LoadPluginList()
    {
      _pluginList.Clear();

      string path = Config.GetFile(Config.Dir.Config, "MediaPortal.xml");

      if (!File.Exists(path))
      {
        _pluginList.Add("Music");
        return;
      }

      XmlDocument doc = new XmlDocument();
      doc.Load(path);


      //TreeNode remoteNode = new TreeNode(nodeRemote.Attributes["family"].Value);
      //remoteNode.Tag = new Data("REMOTE", null, nodeRemote.Attributes["family"].Value);
      //XmlNodeList listButtons = nodeRemote.SelectNodes("button");
      //foreach (XmlNode nodeButton in listButtons)
      //{
      //  TreeNode buttonNode = new TreeNode(nodeButton.Attributes["name"].Value);
      //  buttonNode.Tag = new Data("BUTTON", nodeButton.Attributes["name"].Value, nodeButton.Attributes["code"].Value);
      //  remoteNode.Nodes.Add(buttonNode);

      XmlNode plugins = null;
      XmlNodeList listSections = doc.DocumentElement.SelectNodes("/profile/section");
      foreach (XmlNode nodeSection in listSections)
        if (nodeSection.Attributes["name"].Value == "plugins")
        {
          plugins = nodeSection;
          break;
        }

      if (plugins == null)
      {
        _pluginList.Add("Music");
        return;
      }

      foreach (XmlNode nodePlugin in plugins.ChildNodes)
        _pluginList.Add(nodePlugin.Attributes["name"].Value);
    }

    private TreeNode getNode(string type)
    {
      TreeNode node = treeMapping.SelectedNode;
      Data data = (Data) node.Tag;
      if (data.Type == type)
        return node;

      #region Find Node

      switch (type)
      {
        case "COMMAND":
          if ((data.Type == "SOUND") || (data.Type == "KEY"))
          {
            node = node.Parent;
            foreach (TreeNode subNode in node.Nodes)
            {
              data = (Data) subNode.Tag;
              if (data.Type == type)
                return subNode;
            }
          }
          else if (data.Type == "CONDITION")
          {
            foreach (TreeNode subNode in node.Nodes)
            {
              data = (Data) subNode.Tag;
              if (data.Type == type)
                return subNode;
            }
          }
          break;
        case "SOUND":
          if ((data.Type == "COMMAND") || (data.Type == "KEY"))
          {
            node = node.Parent;
            foreach (TreeNode subNode in node.Nodes)
            {
              data = (Data) subNode.Tag;
              if (data.Type == type)
                return subNode;
            }
          }
          else if (data.Type == "CONDITION")
          {
            foreach (TreeNode subNode in node.Nodes)
            {
              data = (Data) subNode.Tag;
              if (data.Type == type)
                return subNode;
            }
          }
          break;
        case "CONDITION":
          if ((data.Type == "SOUND") || (data.Type == "COMMAND") || (data.Type == "KEY"))
            return node.Parent;
          break;
        case "LAYER":
          if ((data.Type == "SOUND") || (data.Type == "COMMAND") || (data.Type == "KEY"))
            return node.Parent.Parent;
          else if (data.Type == "CONDITION")
            return node.Parent;
          break;
        case "BUTTON":
          if ((data.Type == "SOUND") || (data.Type == "COMMAND") || (data.Type == "KEY"))
            return node.Parent.Parent.Parent;
          else if (data.Type == "CONDITION")
            return node.Parent.Parent;
          else if (data.Type == "LAYER")
            return node.Parent;
          break;
        case "REMOTE":
          if ((data.Type == "SOUND") || (data.Type == "COMMAND") || (data.Type == "KEY"))
            return node.Parent.Parent.Parent.Parent;
          else if (data.Type == "CONDITION")
            return node.Parent.Parent.Parent;
          else if (data.Type == "LAYER")
            return node.Parent.Parent;
          else if (data.Type == "BUTTON")
            return node.Parent;
          break;
      }

      #endregion

      return null;
    }

    private void treeMapping_AfterSelect(object sender, TreeViewEventArgs e)
    {
      if (e.Action == TreeViewAction.Unknown)
        return;

      TreeNode node = e.Node;
      Data data = (Data) node.Tag;
      switch (data.Type)
      {
        case "REMOTE":
        case "BUTTON":
          groupBoxLayer.Enabled = false;
          groupBoxCondition.Enabled = false;
          groupBoxAction.Enabled = false;
          comboBoxLayer.Text = "All Layers";
          comboBoxCondProperty.Text = "none";
          comboBoxCmdProperty.Text = "none";
          comboBoxSound.Text = "none";
          return;
        case "LAYER":
          groupBoxLayer.Enabled = true;
          groupBoxCondition.Enabled = false;
          groupBoxAction.Enabled = false;
          comboBoxCondProperty.Text = "none";
          comboBoxCmdProperty.Text = "none";
          comboBoxSound.Text = "none";
          comboBoxLayer.SelectedIndex = Convert.ToInt32(data.Value);
          return;
        case "COMMAND":
        case "SOUND":
        case "KEY":
        case "CONDITION":
          {
            groupBoxCondition.Enabled = true;
            groupBoxAction.Enabled = true;
            groupBoxLayer.Enabled = true;
            if ((data.Type == "COMMAND") || (data.Type == "SOUND"))
            {
              comboBoxLayer.SelectedIndex = Convert.ToInt32(((Data) node.Parent.Parent.Tag).Value);
              node = node.Parent;
              data = (Data) node.Tag;
            }
            else
              comboBoxLayer.SelectedIndex = Convert.ToInt32(((Data) node.Parent.Tag).Value);

            switch ((string) data.Parameter)
            {
              case "WINDOW":
                {
                  comboBoxCondProperty.DropDownStyle = ComboBoxStyle.DropDownList;

                  radioButtonWindow.Checked = true;
                  comboBoxCondProperty.Enabled = true;

                  string friendlyName;
                  try
                  {
                    friendlyName = GetFriendlyName(Enum.GetName(typeof (GUIWindow.Window), Convert.ToInt32(data.Value)));
                  }
                  catch
                  {
                    friendlyName = Convert.ToInt32(data.Value).ToString();
                  }
                  UpdateCombo(ref comboBoxCondProperty, _windowsList, friendlyName);
                  break;
                }
              case "FULLSCREEN":
                comboBoxCondProperty.DropDownStyle = ComboBoxStyle.DropDownList;

                radioButtonFullscreen.Checked = true;
                comboBoxCondProperty.Enabled = true;
                if (Convert.ToBoolean(data.Value))
                  UpdateCombo(ref comboBoxCondProperty, _fullScreenList, "Fullscreen");
                else
                  UpdateCombo(ref comboBoxCondProperty, _fullScreenList, "No Fullscreen");
                break;
              case "PLAYER":
                comboBoxCondProperty.DropDownStyle = ComboBoxStyle.DropDownList;

                radioButtonPlaying.Checked = true;
                comboBoxCondProperty.Enabled = true;
                UpdateCombo(ref comboBoxCondProperty, _playerList,
                            _playerList[Array.IndexOf(_nativePlayerList, (string) data.Value)]);
                break;
              case "PLUGIN":
                comboBoxCondProperty.DropDownStyle = ComboBoxStyle.DropDownList;

                radioButtonPlugin.Checked = true;
                comboBoxCondProperty.Enabled = true;
                UpdateCombo(ref comboBoxCondProperty, _pluginList.ToArray(), (string) data.Value);
                break;
              case "*":
                comboBoxCondProperty.DropDownStyle = ComboBoxStyle.DropDownList;

                comboBoxCondProperty.Text = "none";
                radioButtonNoCondition.Checked = true;
                comboBoxCondProperty.Enabled = false;
                comboBoxCondProperty.Items.Clear();
                break;
            }
            foreach (TreeNode typeNode in node.Nodes)
            {
              data = (Data) typeNode.Tag;
              switch (data.Type)
              {
                case "SOUND":
                  if (!String.IsNullOrEmpty(data.Value as string))
                    comboBoxSound.SelectedItem = data.Value;
                  else
                    comboBoxSound.SelectedItem = "none";
                  break;
                case "COMMAND":
                  checkBoxGainFocus.Checked = data.Focus;
                  switch ((string) data.Parameter)
                  {
                    case "ACTION":
                      comboBoxCmdProperty.DropDownStyle = ComboBoxStyle.DropDownList;
                      radioButtonAction.Checked = true;
                      comboBoxSound.Enabled = true;
                      comboBoxCmdProperty.Enabled = true;
                      textBoxKeyChar.Enabled = textBoxKeyCode.Enabled = false;
                      textBoxKeyChar.Text = textBoxKeyCode.Text = String.Empty;
                      UpdateCombo(ref comboBoxCmdProperty, _actionList.ToArray(),
                                  GetFriendlyName(Enum.GetName(typeof (Action.ActionType), Convert.ToInt32(data.Value))));
                      break;
                    case "KEY":
                      comboBoxCmdProperty.DropDownStyle = ComboBoxStyle.DropDownList;
                      radioButtonAction.Checked = true;
                      textBoxKeyChar.Enabled = textBoxKeyCode.Enabled = true;
                      textBoxKeyChar.Text = ((Key) data.Value).KeyChar.ToString();
                      textBoxKeyCode.Text = ((Key) data.Value).KeyCode.ToString();
                      comboBoxCmdProperty.Enabled = true;
                      UpdateCombo(ref comboBoxCmdProperty, _actionList.ToArray(), "Key Pressed");
                      break;
                    case "WINDOW":
                      {
                        comboBoxCmdProperty.DropDownStyle = ComboBoxStyle.DropDownList;
                        radioButtonActWindow.Checked = true;
                        comboBoxSound.Enabled = true;
                        comboBoxCmdProperty.Enabled = true;
                        textBoxKeyChar.Enabled = textBoxKeyCode.Enabled = false;
                        textBoxKeyChar.Text = textBoxKeyCode.Text = String.Empty;

                        string friendlyName;
                        try
                        {
                          friendlyName =
                            GetFriendlyName(Enum.GetName(typeof (GUIWindow.Window), Convert.ToInt32(data.Value)));
                        }
                        catch
                        {
                          friendlyName = Convert.ToInt32(data.Value).ToString();
                        }
                        UpdateCombo(ref comboBoxCmdProperty, _windowsListFiltered, friendlyName);
                        break;
                      }
                    case "TOGGLE":
                      radioButtonToggle.Checked = true;
                      comboBoxSound.Enabled = true;
                      comboBoxCmdProperty.Enabled = false;
                      textBoxKeyChar.Enabled = textBoxKeyCode.Enabled = false;
                      textBoxKeyChar.Text = textBoxKeyCode.Text = String.Empty;
                      comboBoxCmdProperty.Items.Clear();
                      comboBoxCmdProperty.Text = String.Empty;
                      break;
                    case "POWER":
                      comboBoxCmdProperty.DropDownStyle = ComboBoxStyle.DropDownList;
                      radioButtonPower.Checked = true;
                      comboBoxSound.Enabled = true;
                      comboBoxCmdProperty.Enabled = true;
                      textBoxKeyChar.Enabled = textBoxKeyCode.Enabled = false;
                      textBoxKeyChar.Text = textBoxKeyCode.Text = String.Empty;
                      UpdateCombo(ref comboBoxCmdProperty, _powerList,
                                  _powerList[Array.IndexOf(_nativePowerList, (string) data.Value)]);
                      break;
                    case "PROCESS":
                      comboBoxCmdProperty.DropDownStyle = ComboBoxStyle.DropDownList;
                      radioButtonProcess.Checked = true;
                      comboBoxSound.Enabled = true;
                      comboBoxCmdProperty.Enabled = true;
                      textBoxKeyChar.Enabled = textBoxKeyCode.Enabled = false;
                      textBoxKeyChar.Text = textBoxKeyCode.Text = String.Empty;
                      UpdateCombo(ref comboBoxCmdProperty, _processList,
                                  _processList[Array.IndexOf(_nativeProcessList, (string) data.Value)]);
                      break;
                    case "BLAST":
                      comboBoxCmdProperty.DropDownStyle = ComboBoxStyle.DropDownList;
                      radioButtonBlast.Checked = true;
                      comboBoxSound.Enabled = true;
                      comboBoxCmdProperty.Enabled = true;
                      textBoxKeyChar.Enabled = textBoxKeyCode.Enabled = false;
                      textBoxKeyChar.Text = textBoxKeyCode.Text = String.Empty;
                      UpdateCombo(ref comboBoxCmdProperty, MPControlPlugin.GetFileList(true), (string) data.Value);
                      break;
                  }
                  break;
              }
            }
          }
          break;
      }
    }

    private static void UpdateCombo(ref MPComboBox comboBox, Array list, string hilight)
    {
      comboBox.Items.Clear();
      foreach (object item in list)
        comboBox.Items.Add(item.ToString());
      comboBox.Text = hilight;
      comboBox.SelectedItem = hilight;
      comboBox.Enabled = true;
    }

    private static void UpdateCombo(ref MPComboBox comboBox, ArrayList list, string hilight)
    {
      UpdateCombo(ref comboBox, list.ToArray(), hilight);
    }

    private static string GetFriendlyName(string name)
    {
      if ((name.IndexOf("ACTION") != -1) || (name.IndexOf("WINDOW") != -1))
        name = name.Substring(7);

      bool upcase = true;
      string newName = String.Empty;

      foreach (char c in name)
      {
        if (c == '_')
        {
          newName += " ";
          upcase = true;
        }
        else if (upcase)
        {
          newName += c.ToString();
          upcase = false;
        }
        else
        {
          newName += c.ToString().ToLower();
        }
      }

      CleanAbbreviation(ref newName, "TV");
      CleanAbbreviation(ref newName, "DVD");
      CleanAbbreviation(ref newName, "UI");
      CleanAbbreviation(ref newName, "Guide");
      CleanAbbreviation(ref newName, "MSN");
      CleanAbbreviation(ref newName, "OSD");
      CleanAbbreviation(ref newName, "LCD");
      CleanAbbreviation(ref newName, "EPG");
      CleanAbbreviation(ref newName, "DVBC");
      CleanAbbreviation(ref newName, "DVBS");
      CleanAbbreviation(ref newName, "DVBT");

      return newName;
    }

    private static string GetWindowName(string friendlyName)
    {
      return "WINDOW_" + friendlyName.Replace(' ', '_').ToUpper();
    }

    private static string GetActionName(string friendlyName)
    {
      string actionName = String.Empty;

      try
      {
        if (Enum.Parse(typeof (Action.ActionType), "ACTION_" + friendlyName.Replace(' ', '_').ToUpper()) != null)
          actionName = "ACTION_" + friendlyName.Replace(' ', '_').ToUpper();
      }
      catch (ArgumentException)
      {
        try
        {
          if (Enum.Parse(typeof (Action.ActionType), friendlyName.Replace(' ', '_').ToUpper()) != null)
            actionName = friendlyName.Replace(' ', '_').ToUpper();
        }
        catch (ArgumentException)
        {
        }
      }

      return actionName;
    }


    private static void CleanAbbreviation(ref string name, string abbreviation)
    {
      int index = name.ToUpper().IndexOf(abbreviation.ToUpper());
      if (index != -1)
        name = name.Substring(0, index) + abbreviation + name.Substring(index + abbreviation.Length);
    }

    private void radioButtonWindow_CheckedChanged(object sender, EventArgs e)
    {
      if (!((Control) sender).Focused) return;

      comboBoxCondProperty.DropDownStyle = ComboBoxStyle.DropDownList;
      comboBoxCondProperty.Enabled = true;
      TreeNode node = getNode("CONDITION");
      node.Tag = new Data("CONDITION", "WINDOW", "0");
      UpdateCombo(ref comboBoxCondProperty, _windowsList, GetFriendlyName(Enum.GetName(typeof (GUIWindow.Window), 0)));
      node.Text = (string) comboBoxCondProperty.SelectedItem;
      _changedSettings = true;
    }

    private void radioButtonFullscreen_CheckedChanged(object sender, EventArgs e)
    {
      if (!((Control) sender).Focused) return;

      comboBoxCondProperty.DropDownStyle = ComboBoxStyle.DropDownList;
      comboBoxCondProperty.Enabled = true;
      TreeNode node = getNode("CONDITION");
      node.Tag = new Data("CONDITION", "FULLSCREEN", "true");
      UpdateCombo(ref comboBoxCondProperty, _fullScreenList, "Fullscreen");
      node.Text = (string) comboBoxCondProperty.SelectedItem;
      _changedSettings = true;
    }

    private void radioButtonPlaying_CheckedChanged(object sender, EventArgs e)
    {
      if (!((Control) sender).Focused) return;

      comboBoxCondProperty.DropDownStyle = ComboBoxStyle.DropDownList;
      comboBoxCondProperty.Enabled = true;
      TreeNode node = getNode("CONDITION");
      node.Tag = new Data("CONDITION", "PLAYER", "TV");
      node.Text = _playerList[0];
      UpdateCombo(ref comboBoxCondProperty, _playerList, _playerList[0]);
      _changedSettings = true;
    }

    private void radioButtonNoCondition_CheckedChanged(object sender, EventArgs e)
    {
      if (!((Control) sender).Focused) return;

      comboBoxCondProperty.DropDownStyle = ComboBoxStyle.DropDownList;
      comboBoxCondProperty.Enabled = false;
      comboBoxCondProperty.Items.Clear();
      comboBoxCondProperty.Text = "none";
      TreeNode node = getNode("CONDITION");
      node.Tag = new Data("CONDITION", "*", null);
      node.Text = "No Condition";
      _changedSettings = true;
    }

    private void radioButtonPlugin_CheckedChanged(object sender, EventArgs e)
    {
      if (!((Control) sender).Focused) return;

      comboBoxCondProperty.DropDownStyle = ComboBoxStyle.DropDownList;
      comboBoxCondProperty.Enabled = true;
      TreeNode node = getNode("CONDITION");
      node.Tag = new Data("CONDITION", "PLUGIN", _pluginList[0]);
      UpdateCombo(ref comboBoxCondProperty, _pluginList.ToArray(), _pluginList[0]);
      node.Text = comboBoxCondProperty.Text;
      _changedSettings = true;
    }

    private void radioButtonAction_Click(object sender, EventArgs e)
    {
      textBoxKeyChar.Enabled = textBoxKeyCode.Enabled = false;
      textBoxKeyChar.Text = textBoxKeyCode.Text = String.Empty;
      comboBoxCmdProperty.DropDownStyle = ComboBoxStyle.DropDownList;
      comboBoxSound.Enabled = true;
      comboBoxCmdProperty.Enabled = true;
      TreeNode node = getNode("COMMAND");
      Data data = new Data("COMMAND", "ACTION", "7");
      node.Tag = data;
      UpdateCombo(ref comboBoxCmdProperty, _actionList.ToArray(),
                  GetFriendlyName(Enum.GetName(typeof (Action.ActionType), Convert.ToInt32(data.Value))));
      node.Text = "Action \"" + (string) comboBoxCmdProperty.SelectedItem + "\"";
      ((Data) node.Tag).Focus = checkBoxGainFocus.Checked;
      _changedSettings = true;
    }

    private void radioButtonActWindow_Click(object sender, EventArgs e)
    {
      comboBoxCmdProperty.DropDownStyle = ComboBoxStyle.DropDownList;
      comboBoxSound.Enabled = true;
      comboBoxCmdProperty.Enabled = true;
      TreeNode node = getNode("COMMAND");
      Data data = new Data("COMMAND", "WINDOW", "0");
      node.Tag = data;

      string friendlyName;
      try
      {
        friendlyName = GetFriendlyName(Enum.GetName(typeof (GUIWindow.Window), Convert.ToInt32(data.Value)));
      }
      catch
      {
        friendlyName = Convert.ToInt32(data.Value).ToString();
      }
      UpdateCombo(ref comboBoxCmdProperty, _windowsListFiltered, friendlyName);

      node.Text = "Window \"" + comboBoxCmdProperty.Text + "\"";
      ((Data) node.Tag).Focus = checkBoxGainFocus.Checked;
      _changedSettings = true;
    }

    private void radioButtonToggle_Click(object sender, EventArgs e)
    {
      textBoxKeyChar.Enabled = textBoxKeyCode.Enabled = false;
      textBoxKeyChar.Text = textBoxKeyCode.Text = String.Empty;
      comboBoxSound.Enabled = true;
      comboBoxCmdProperty.Enabled = false;
      comboBoxCmdProperty.Items.Clear();
      comboBoxCmdProperty.Text = "none";
      TreeNode node = getNode("COMMAND");
      Data data = new Data("COMMAND", "TOGGLE", "-1");
      node.Tag = data;
      node.Text = "Toggle Layer";
      ((Data) node.Tag).Focus = checkBoxGainFocus.Checked;
      _changedSettings = true;
    }

    private void radioButtonPower_Click(object sender, EventArgs e)
    {
      textBoxKeyChar.Enabled = textBoxKeyCode.Enabled = false;
      textBoxKeyChar.Text = textBoxKeyCode.Text = String.Empty;
      comboBoxCmdProperty.DropDownStyle = ComboBoxStyle.DropDownList;
      comboBoxSound.Enabled = true;
      comboBoxCmdProperty.Enabled = true;
      TreeNode node = getNode("COMMAND");
      node.Tag = new Data("COMMAND", "POWER", "EXIT");
      node.Text = _powerList[0];
      UpdateCombo(ref comboBoxCmdProperty, _powerList, _powerList[0]);
      ((Data) node.Tag).Focus = checkBoxGainFocus.Checked;
      _changedSettings = true;
    }

    private void radioButtonProcess_Click(object sender, EventArgs e)
    {
      textBoxKeyChar.Enabled = textBoxKeyCode.Enabled = false;
      textBoxKeyChar.Text = textBoxKeyCode.Text = String.Empty;
      comboBoxCmdProperty.DropDownStyle = ComboBoxStyle.DropDownList;
      comboBoxSound.Enabled = true;
      comboBoxCmdProperty.Enabled = true;
      TreeNode node = getNode("COMMAND");
      node.Tag = new Data("COMMAND", "PROCESS", "CLOSE");
      node.Text = _processList[0];
      UpdateCombo(ref comboBoxCmdProperty, _processList, _processList[0]);
      ((Data) node.Tag).Focus = checkBoxGainFocus.Checked;
      _changedSettings = true;
    }

    private void buttonOk_Click(object sender, EventArgs e)
    {
      if (_changedSettings)
        SaveMapping(_inputClassName + ".xml");
      Close();
    }

    private void buttonApply_Click(object sender, EventArgs e)
    {
      if (_changedSettings)
        SaveMapping(_inputClassName + ".xml");
    }

    private void buttonUp_Click(object sender, EventArgs e)
    {
      bool expanded = false;
      TreeNode node = treeMapping.SelectedNode;
      if (((Data) node.Tag).Type != "BUTTON")
        expanded = node.IsExpanded;
      if ((((Data) node.Tag).Type == "COMMAND") || (((Data) node.Tag).Type == "SOUND"))
      {
        node = node.Parent;
        expanded = true;
      }
      if ((((Data) node.Tag).Type != "BUTTON") && (((Data) node.Tag).Type != "CONDITION"))
        return;
      if (node.Index > 0)
      {
        int index = node.Index - 1;
        TreeNode tmpNode = (TreeNode) node.Clone();
        TreeNode parentNode = node.Parent;
        node.Remove();
        if (expanded)
          tmpNode.Expand();
        parentNode.Nodes.Insert(index, tmpNode);
        treeMapping.SelectedNode = tmpNode;
      }
      _changedSettings = true;
    }

    private void buttonDown_Click(object sender, EventArgs e)
    {
      bool expanded = false;
      TreeNode node = treeMapping.SelectedNode;
      if (((Data) node.Tag).Type != "BUTTON")
        expanded = node.IsExpanded;
      if ((((Data) node.Tag).Type == "COMMAND") || (((Data) node.Tag).Type == "SOUND"))
      {
        node = node.Parent;
        expanded = true;
      }
      if ((((Data) node.Tag).Type != "BUTTON") && (((Data) node.Tag).Type != "CONDITION"))
        return;
      if (node.Index < node.Parent.Nodes.Count - 1)
      {
        int index = node.Index + 1;
        TreeNode tmpNode = (TreeNode) node.Clone();
        TreeNode parentNode = node.Parent;
        node.Remove();
        if (expanded)
          tmpNode.Expand();
        parentNode.Nodes.Insert(index, tmpNode);
        treeMapping.SelectedNode = tmpNode;
      }
      _changedSettings = true;
    }

    private void buttonRemove_Click(object sender, EventArgs e)
    {
      TreeNode node = treeMapping.SelectedNode;
      Data data = (Data) node.Tag;
      if ((data.Type == "COMMAND") || (data.Type == "SOUND") || (data.Type == "CONDITION"))
      {
        node = getNode("CONDITION");
        data = (Data) node.Tag;
      }
      DialogResult result = MessageBox.Show(this, "Are you sure you want to remove this " + data.Type.ToLower() + "?",
                                            "Remove " + data.Type.ToLower(),
                                            MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                            MessageBoxDefaultButton.Button2);
      if (result == DialogResult.Yes)
      {
        node.Remove();
        _changedSettings = true;
      }
    }


    private void buttonNew_Click(object sender, EventArgs e)
    {
      TreeNode node = treeMapping.SelectedNode;
      Data data = (Data) node.Tag;

      TreeNode newLayer = new TreeNode("All Layers");
      newLayer.Tag = new Data("LAYER", null, "0");
      newLayer.ForeColor = Color.DimGray;

      TreeNode newCondition = new TreeNode("No Condition");
      newCondition.Tag = new Data("CONDITION", "*", "-1");
      newCondition.ForeColor = Color.Blue;

      TreeNode newCommand = new TreeNode("Action \"Select Item\"");
      newCommand.Tag = new Data("COMMAND", "ACTION", "7");
      newCommand.ForeColor = Color.DarkGreen;

      TreeNode newSound = new TreeNode("No Sound");
      newSound.Tag = new Data("SOUND", String.Empty, String.Empty);
      newSound.ForeColor = Color.DarkRed;

      switch (data.Type)
      {
        case "LAYER":
          newCondition.Nodes.Add(newCommand);
          newCondition.Nodes.Add(newSound);
          newLayer.Nodes.Add(newCondition);
          node.Parent.Nodes.Add(newLayer);
          newLayer.Expand();
          treeMapping.SelectedNode = newLayer;
          break;
        case "CONDITION":
          newCondition.Nodes.Add(newCommand);
          newCondition.Nodes.Add(newSound);
          node.Parent.Nodes.Add(newCondition);
          newCondition.Expand();
          treeMapping.SelectedNode = newCondition;
          break;
        case "COMMAND":
        case "SOUND":
          newCondition.Nodes.Add(newCommand);
          newCondition.Nodes.Add(newSound);
          node.Parent.Parent.Nodes.Add(newCondition);
          newCondition.Expand();
          treeMapping.SelectedNode = newCondition;
          break;
        case "BUTTON":
          newCondition.Nodes.Add(newCommand);
          newCondition.Nodes.Add(newSound);
          newLayer.Nodes.Add(newCondition);
          node.Nodes.Add(newLayer);
          newLayer.Expand();
          treeMapping.SelectedNode = newLayer;
          break;
        default:
          //NewButtonForm newButtonForm = new NewButtonForm();
          //newButtonForm.ShowDialog();
          //if (newButtonForm.Accepted)
          //{
          //  Log.Info("Name: {0}", newButtonForm.ButtonName);
          //  Log.Info("Code: {0}", newButtonForm.ButtonCode);
          //}
          break;
      }
      _changedSettings = true;

      treeMapping_AfterSelect(this, new TreeViewEventArgs(treeMapping.SelectedNode, TreeViewAction.ByKeyboard));
    }

    private void buttonDefault_Click(object sender, EventArgs e)
    {
      string fileName = _inputClassName + ".xml";
      string filePath = Path.Combine(MPUtils.MPCommon.CustomInputDevice, fileName);

      if (File.Exists(filePath))
        File.Delete(filePath);

      LoadMapping(fileName, true);
    }

    private void textBoxKeyCode_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (!char.IsNumber(e.KeyChar) && e.KeyChar != 8)
      {
        e.Handled = true;
      }
    }

    private void textBoxKeyChar_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (!char.IsNumber(e.KeyChar) && e.KeyChar != 8)
      {
        e.Handled = true;
      }
    }

    private void comboBoxLayer_SelectionChangeCommitted(object sender, EventArgs e)
    {
      TreeNode node = getNode("LAYER");
      node.Tag = new Data("LAYER", null, comboBoxLayer.SelectedIndex);
      if (comboBoxLayer.SelectedIndex == 0)
        node.Text = "All Layers";
      else
        node.Text = "Layer " + comboBoxLayer.SelectedIndex;
      _changedSettings = true;
    }

    private void comboBoxCondProperty_TextChanged(object sender, EventArgs e)
    {
      if (!((Control) sender).Focused) return;

      ConditionPropChanged();
    }

    private void comboBoxCmdProperty_TextChanged(object sender, EventArgs e)
    {
      if (!((Control) sender).Focused) return;

      CommandPropChanged();
    }

    private void comboBoxCondProperty_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!comboBoxCondProperty.Focused) return;
      if (comboBoxCondProperty.DropDownStyle != ComboBoxStyle.DropDownList) return;

      ConditionPropChanged();
    }

    private void comboBoxCmdProperty_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!comboBoxCmdProperty.Focused) return;
      if (comboBoxCmdProperty.DropDownStyle != ComboBoxStyle.DropDownList) return;

      CommandPropChanged();
    }

    private void comboBoxSound_SelectionChangeCommitted(object sender, EventArgs e)
    {
      TreeNode node = getNode("SOUND");
      node.Text = (string) comboBoxSound.SelectedItem;
      if (node.Text == "none")
      {
        node.Tag = new Data("SOUND", null, String.Empty);
        node.Text = "No Sound";
      }
      else
        node.Tag = new Data("SOUND", null, comboBoxSound.SelectedItem);
      _changedSettings = true;
    }

    private void textBoxKeyChar_KeyUp(object sender, KeyEventArgs e)
    {
      string keyChar = textBoxKeyChar.Text;
      string keyCode = textBoxKeyCode.Text;
      TreeNode node = getNode("COMMAND");
      if (String.IsNullOrEmpty(keyChar))
        keyChar = "0";
      if (String.IsNullOrEmpty(keyCode))
        keyCode = "0";
      Key key = new Key(Convert.ToInt32(keyChar), Convert.ToInt32(keyCode));
      node.Tag = new Data("COMMAND", "KEY", key);
      node.Text = String.Format("Key Pressed: {0} [{1}]", keyChar, keyCode);
      ((Data) node.Tag).Focus = checkBoxGainFocus.Checked;
      _changedSettings = true;
    }

    private void textBoxKeyCode_KeyUp(object sender, KeyEventArgs e)
    {
      textBoxKeyChar_KeyUp(sender, e);
    }

    private void labelExpand_Click(object sender, EventArgs e)
    {
      if (treeMapping.SelectedNode == null)
        treeMapping.Select();
      treeMapping.SelectedNode.ExpandAll();
    }

    private void checkBoxGainFocus_CheckedChanged(object sender, EventArgs e)
    {
      TreeNode node = getNode("COMMAND");
      ((Data) node.Tag).Focus = checkBoxGainFocus.Checked;
      _changedSettings = true;
    }

    private void radioButtonBlast_Click(object sender, EventArgs e)
    {
      textBoxKeyChar.Enabled = textBoxKeyCode.Enabled = false;
      textBoxKeyChar.Text = textBoxKeyCode.Text = String.Empty;
      comboBoxSound.Enabled = true;
      comboBoxCmdProperty.Enabled = false;
      comboBoxCmdProperty.Items.Clear();
      comboBoxCmdProperty.Text = "none";
      TreeNode node = getNode("COMMAND");
      Data data = new Data("COMMAND", "BLAST", String.Empty);
      node.Tag = data;
      node.Text = "";
      ((Data) node.Tag).Focus = checkBoxGainFocus.Checked;
      UpdateCombo(ref comboBoxCmdProperty, MPControlPlugin.GetFileList(true), String.Empty);
      _changedSettings = true;
    }

    private void CommandPropChanged()
    {
      if ((string) comboBoxCmdProperty.SelectedItem == "Key Pressed")
        textBoxKeyChar.Enabled = textBoxKeyCode.Enabled = true;
      else
      {
        textBoxKeyChar.Enabled = textBoxKeyCode.Enabled = false;
        textBoxKeyChar.Text = textBoxKeyCode.Text = String.Empty;
      }

      TreeNode node = getNode("COMMAND");
      Data data = (Data) node.Tag;
      switch ((string) data.Parameter)
      {
        case "ACTION":
          if ((string) comboBoxCmdProperty.SelectedItem != "Key Pressed")
          {
            node.Tag = new Data("COMMAND", "ACTION",
                                (int)
                                Enum.Parse(typeof (Action.ActionType),
                                           GetActionName((string) comboBoxCmdProperty.SelectedItem)));
            node.Text = "Action \"" + (string) comboBoxCmdProperty.SelectedItem + "\"";
          }
          else
          {
            textBoxKeyChar.Text = "0";
            textBoxKeyCode.Text = "0";
            Key key = new Key(Convert.ToInt32(textBoxKeyChar.Text), Convert.ToInt32(textBoxKeyCode.Text));
            node.Tag = new Data("COMMAND", "KEY", key);
            node.Text = String.Format("Key Pressed: {0} [{1}]", textBoxKeyChar.Text, textBoxKeyCode.Text);
          }
          break;
        case "WINDOW":
          {
            int windowID;
            try
            {
              windowID = (int) Enum.Parse(typeof (GUIWindow.Window), GetWindowName(comboBoxCmdProperty.Text));
            }
            catch
            {
              windowID = Convert.ToInt32(comboBoxCmdProperty.Text);
            }

            node.Tag = new Data("COMMAND", "WINDOW", windowID);
            node.Text = "Window \"" + comboBoxCmdProperty.Text + "\"";
            break;
          }
        case "POWER":
          node.Tag = new Data("COMMAND", "POWER",
                              _nativePowerList[Array.IndexOf(_powerList, (string) comboBoxCmdProperty.SelectedItem)]);
          node.Text = (string) comboBoxCmdProperty.SelectedItem;
          break;
        case "PROCESS":
          node.Tag = new Data("COMMAND", "PROCESS",
                              _nativeProcessList[Array.IndexOf(_processList, (string) comboBoxCmdProperty.SelectedItem)]);
          node.Text = (string) comboBoxCmdProperty.SelectedItem;
          break;
        case "BLAST":
          {
            string text = (string) comboBoxCmdProperty.SelectedItem;
            if (text.StartsWith(IrssUtils.Common.CmdPrefixBlast, StringComparison.InvariantCultureIgnoreCase))
            {
              BlastCommand blastCommand = new BlastCommand(
                MPControlPlugin.BlastIR,
                IrssUtils.Common.FolderIRCommands,
                MPControlPlugin.TransceiverInformation.Ports,
                text.Substring(IrssUtils.Common.CmdPrefixBlast.Length));

              if (blastCommand.ShowDialog(this) == DialogResult.OK)
              {
                string command = IrssUtils.Common.CmdPrefixBlast + blastCommand.CommandString;
                node.Tag = new Data("COMMAND", "BLAST", command);
                node.Text = command;
              }
            }
            else if (text.StartsWith(IrssUtils.Common.CmdPrefixMacro, StringComparison.InvariantCultureIgnoreCase))
            {
              node.Tag = new Data("COMMAND", "BLAST", text);
              node.Text = text;
            }
            break;
          }
      }
      ((Data) node.Tag).Focus = checkBoxGainFocus.Checked;
      _changedSettings = true;
    }

    private void ConditionPropChanged()
    {
      TreeNode node = getNode("CONDITION");
      Data data = (Data) node.Tag;
      switch ((string) data.Parameter)
      {
        case "WINDOW":
          {
            int windowID;
            try
            {
              windowID = (int) Enum.Parse(typeof (GUIWindow.Window), GetWindowName(comboBoxCondProperty.Text));
            }
            catch
            {
              windowID = Convert.ToInt32(comboBoxCondProperty.Text);
            }

            node.Tag = new Data("CONDITION", "WINDOW", windowID);
            node.Text = comboBoxCondProperty.Text;
            break;
          }
        case "FULLSCREEN":
          if ((string) comboBoxCondProperty.SelectedItem == "Fullscreen")
            node.Tag = new Data("CONDITION", "FULLSCREEN", "true");
          else
            node.Tag = new Data("CONDITION", "FULLSCREEN", "false");
          node.Text = (string) comboBoxCondProperty.SelectedItem;
          break;
        case "PLAYER":
          {
            node.Tag = new Data("CONDITION", "PLAYER",
                                _nativePlayerList[Array.IndexOf(_playerList, (string) comboBoxCondProperty.SelectedItem)
                                  ]);
            node.Text = (string) comboBoxCondProperty.SelectedItem;
            break;
          }
        case "PLUGIN":
          {
            node.Tag = new Data("CONDITION", "PLUGIN", comboBoxCondProperty.Text);
            node.Text = comboBoxCondProperty.Text;
            break;
          }
        case "*":
          break;
      }
      _changedSettings = true;
    }

    #region Nested type: Data

    private class Data
    {
      private readonly string type;
      private object dataValue;
      private bool focus;
      private object parameter;

      public Data(object newType, object newParameter, object newValue)
      {
        if (newValue == null)
          newValue = String.Empty;
        if (newParameter == null)
          newParameter = String.Empty;
        type = (string) newType;
        dataValue = newValue;
        parameter = newParameter;
      }

      public Data(object newType, object newParameter, object newValue, bool newFocus)
      {
        if (newValue == null)
          newValue = String.Empty;
        if (newParameter == null)
          newParameter = String.Empty;
        type = (string) newType;
        dataValue = newValue;
        parameter = newParameter;
        focus = newFocus;
      }

      public string Type
      {
        get { return type; }
      }

      public object Value
      {
        get { return dataValue; }
        set { dataValue = value; }
      }

      public object Parameter
      {
        get { return parameter; }
        set { parameter = value; }
      }

      public bool Focus
      {
        get { return focus; }
        set { focus = value; }
      }
    }

    #endregion

    //    private TreeNode tn;
    //
    //    private void treeMapping_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
    //    {
    //      tn=e.Item as TreeNode;
    //      DoDragDrop(e.Item.ToString(), DragDropEffects.Move);
    //    }
    //
    //    private void treeMapping_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
    //    {
    //      Point pt = new Point(e.X,e.Y); 
    //      pt = treeMapping.PointToClient(pt); 
    //      TreeNode ParentNode = treeMapping.GetNodeAt(pt); 
    //      ParentNode.Nodes.Add(tn.Text); // this copies the node 
    //      tn.Remove(); // need to remove the original version of the node 
    //    }
    //
    //    private void treeMapping_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
    //    {
    //      e.Effect=DragDropEffects.Move;
    //    }
  }
}