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
using IrssUtils.Forms;
using MPUtils;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using MediaPortal.UserInterface.Controls;
using Action = MediaPortal.GUI.Library.Action;
using IrssCommands;
using MediaPortal.Input;

namespace MediaPortal.Plugins.IRSS.MPControlPlugin.InputMapper
{
  public partial class InputMappingForm : MPConfigForm
  {
    #region Constants

    private readonly List<string> _actionList = new List<string>();
    private readonly List<string> _pluginList = new List<string>();

    private readonly string _inputClassName;
    private readonly Array _nativeActionList = Enum.GetValues(typeof (Action.ActionType));

    private readonly Array _nativeWindowsList = Enum.GetValues(typeof (GUIWindow.Window));
    
    private const string NO_SOUND = "No Sound";
    private readonly string[] _soundList = new string[] {NO_SOUND, "back.wav", "click.wav", "cursor.wav"};

    private static readonly string[] _layerList = new string[] { "All Layers", "Layer 1", "Layer 2" };

    private Condition _currentCondition;
    private BaseConditionConfig _currentConditionConfig;
    private readonly Dictionary<string, Condition> _conditionStorage = new Dictionary<string, Condition>();
    private readonly Dictionary<string, Type> _uiTextCategoryCache = new Dictionary<string, Type>();

    #endregion Constants

    private InputMapping inputMapping;
    private bool _changedSettings;

    #region Constructor

    public InputMappingForm(string name)
    {
      InitializeComponent();

      _inputClassName = name;
      headerLabel.Caption = _inputClassName;

      // add images
      upToolStripButton.Image = IrssUtils.Properties.Resources.MoveUp;
      downToolStripButton.Image = IrssUtils.Properties.Resources.MoveDown;
      newToolStripButton.Image = IrssUtils.Properties.Resources.Plus;
      removeToolStripButton.Image = IrssUtils.Properties.Resources.Delete;

      upToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
      downToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
      newToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
      removeToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;

      #region comments

      //_actionList.Clear();
      //foreach (Action.ActionType actn in _nativeActionList)
      //  _actionList.Add(GetFriendlyName(actn.ToString()));

      #endregion

      // prepare combo boxes
      comboBoxLayer.DataSource = _layerList;
      comboBoxSound.DataSource = _soundList;
      PopulateCommandList();
      PopulateConditionList();

      string mappingsFile = MPCommon.GetCustomMappingFilePath(_inputClassName);
      if (!File.Exists(mappingsFile))
      {
        string defaultFile = MPCommon.GetDefaultMappingFilePath(_inputClassName);
        //if (!File.Exists(defaultFile))
        //add extraction from properties or error message

        File.Copy(defaultFile, mappingsFile, true);
      }

      inputMapping = InputMapping.Load(mappingsFile);
      RefreshTreeView();
    }

    #endregion Constructor

    private void CloseThread()
    {
      Thread.Sleep(200);
      Close();
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

    private TreeNode getNode(Type returnType)
    {
      TreeNode node = treeMapping.SelectedNode;
      if (ReferenceEquals(node, null)) return null;
      if (ReferenceEquals(node.Tag, null)) return null;

      // not working because of inheritation
      //if (returnType == node.Tag.GetType())
      //  return node;

      if (returnType == typeof (Sound))
      {
        // set node to condition
        if (node.Tag is Sound || node.Tag is Command || node.Tag is DummyCommand)
          node = node.Parent;

        // look for Sound in Condition's subnodes
        foreach (TreeNode subNode in node.Nodes)
          if (subNode.Tag is Sound)
            return subNode;
      }
      else if (returnType == typeof (Command))
      {
        if (node.Tag is Sound || node.Tag is Command || node.Tag is DummyCommand)
          node = node.Parent;

        // look for Command in Condition's subnodes
        foreach (TreeNode subNode in node.Nodes)
          if (subNode.Tag is Command || node.Tag is DummyCommand)
            return subNode;
      }
      else if (returnType == typeof (Condition))
      {
        if (node.Tag is Condition || node.Tag is DummyCondition)
          return node;

        if (node.Tag is Sound || node.Tag is Command || node.Tag is DummyCommand)
          return node.Parent;
      }
      else if (returnType == typeof (Layer))
      {
        if (node.Tag is Sound || node.Tag is Command || node.Tag is DummyCommand)
          return node.Parent.Parent;
        if (node.Tag is Condition || node.Tag is DummyCondition)
          return node.Parent;
      }
      else if (returnType == typeof (InputMapping.Button))
      {
        if (node.Tag is Sound || node.Tag is Command || node.Tag is DummyCommand)
          return node.Parent.Parent.Parent;
        if (node.Tag is Condition || node.Tag is DummyCondition)
          return node.Parent.Parent;
        if (node.Tag is Layer)
          return node.Parent;
      }
      else if (returnType == typeof (InputMapping.Remote))
      {
        if (node.Tag is Sound || node.Tag is Command || node.Tag is DummyCommand)
          return node.Parent.Parent.Parent.Parent;
        if (node.Tag is Condition || node.Tag is DummyCondition)
          return node.Parent.Parent.Parent;
        if (node.Tag is Layer)
          return node.Parent.Parent;
        if (node.Tag is InputMapping.Button)
          return node.Parent;
      }

      return null;
    }

    #region mapping treenode

    private void RefreshTreeView()
    {
      treeMapping.Nodes.Clear();
      if (ReferenceEquals(inputMapping, null)) return;

      foreach (InputMapping.Remote remote in inputMapping.Remotes)
      {
        TreeNode remoteNode = new TreeNode(remote.Family);
        remoteNode.Tag = remote;

        foreach (InputMapping.Button button in remote.Buttons)
        {
          TreeNode buttonNode = new TreeNode(button.Name);
          buttonNode.Tag = button;
          remoteNode.Nodes.Add(buttonNode);

          TreeNode layerAllNode = new TreeNode(_layerList[0]);
          layerAllNode.Tag = new Layer { Value = 0 };
          layerAllNode.ForeColor = Color.DimGray;
          TreeNode layer1Node = new TreeNode(_layerList[1]);
          layer1Node.Tag = new Layer { Value = 1 };
          layer1Node.ForeColor = Color.DimGray;
          TreeNode layer2Node = new TreeNode(_layerList[2]);
          layer2Node.Tag = new Layer { Value = 2 };
          layer2Node.ForeColor = Color.DimGray;

          foreach (InputMapping.Action action in button.Actions)
          {
            #region comments

            //switch (command)
            //{
            //  case "ACTION":
            //    commandString = "Action \"" +
            //                    GetFriendlyName(Enum.GetName(typeof (Action.ActionType), Convert.ToInt32(cmdProperty))) +
            //                    "\"";
            //    break;
            //  case "KEY":
            //    commandString = "Key \"" + cmdProperty + "\"";
            //    break;
            //  case "WINDOW":
            //    {
            //      try
            //      {
            //        commandString = "Window \"" +
            //                        GetFriendlyName(Enum.GetName(typeof (GUIWindow.Window),
            //                                                     Convert.ToInt32(cmdProperty))) + "\"";
            //      }
            //      catch
            //      {
            //        commandString = "Window \"" + cmdProperty + "\"";
            //      }
            //      break;
            //    }
            //  case "TOGGLE":
            //    commandString = "Toggle Layer";
            //    break;
            //  case "POWER":
            //    commandString = _powerList[Array.IndexOf(_nativePowerList, cmdProperty)];
            //    break;
            //  case "PROCESS":
            //    commandString = _processList[Array.IndexOf(_nativeProcessList, cmdProperty)];
            //    break;
            //  case "BLAST":
            //    commandString = cmdProperty;
            //    break;
            //}

            #endregion

            TreeNode conditionNode = new TreeNode(action.GetConditionDisplayTextSafe());
            conditionNode.Tag = action.GetConditionSafe();
            conditionNode.ForeColor = Color.Blue;

            TreeNode commandNode = new TreeNode(action.GetCommandDisplayTextSafe());
            commandNode.Tag = action.GetCommandSafe();
            commandNode.ForeColor = Color.DarkGreen;
            conditionNode.Nodes.Add(commandNode);

            Sound sound = new Sound { Value = action.Sound };
            TreeNode soundNode = new TreeNode(sound.UserDisplayText);
            soundNode.Tag = sound;
            soundNode.ForeColor = Color.DarkRed;
            conditionNode.Nodes.Add(soundNode);

            if (action.Layer == 0) layerAllNode.Nodes.Add(conditionNode);
            if (action.Layer == 1) layer1Node.Nodes.Add(conditionNode);
            if (action.Layer == 2) layer2Node.Nodes.Add(conditionNode);
          }
          if (layerAllNode.Nodes.Count > 0) buttonNode.Nodes.Add(layerAllNode);
          if (layer1Node.Nodes.Count > 0) buttonNode.Nodes.Add(layer1Node);
          if (layer2Node.Nodes.Count > 0) buttonNode.Nodes.Add(layer2Node);
        }
        treeMapping.Nodes.Add(remoteNode);
        if (inputMapping.Remotes.Count == 1)
          remoteNode.Expand();
      }
      _changedSettings = false;
    }

    // treeview layout
    //
    // -- Remote
    //   |-- Button
    //      |-- layer
    //         |-- condition
    //            |-- command
    //            |-- sound
    private void treeMapping_AfterSelect(object sender, TreeViewEventArgs e)
    {
      if (e.Action == TreeViewAction.Unknown) return;

      TreeNode node = e.Node;
      if (ReferenceEquals(node.Tag, null)) return;

      if (node.Tag is InputMapping.Remote || node.Tag is InputMapping.Button)
      {
        groupBoxLayer.Enabled = false;
        groupBoxCondition.Enabled = false;
        groupBoxCommand.Enabled = false;
        groupBoxSound.Enabled = false;
        newToolStripButton.Enabled = false;

        comboBoxLayer.SelectedIndex = 0;
        return;
      }

      Layer layer = node.Tag as Layer;
      if (layer != null)
      {
        groupBoxLayer.Enabled = true;
        groupBoxCondition.Enabled = false;
        groupBoxCommand.Enabled = false;
        groupBoxSound.Enabled = false;
        newToolStripButton.Enabled = true;

        comboBoxLayer.SelectedIndex = layer.Value;
        return;
      }

      Command command = node.Tag as Command;
      DummyCommand dummyCommand = node.Tag as DummyCommand;
      Condition condition = node.Tag as Condition;
      DummyCondition dummyCondition = node.Tag as DummyCondition;
      Sound sound = node.Tag as Sound;
      if (!ReferenceEquals(condition, null) || !ReferenceEquals(dummyCondition, null) || !ReferenceEquals(command, null) || !ReferenceEquals(dummyCommand, null) || !ReferenceEquals(sound, null))
      {
        // enable group boxes
        groupBoxLayer.Enabled = true;
        groupBoxCondition.Enabled = true;
        groupBoxCommand.Enabled = true;
        groupBoxSound.Enabled = true;
        newToolStripButton.Enabled = true;

        // set node to condition
        if (ReferenceEquals(condition, null) && ReferenceEquals(dummyCondition, null))
        {
          node = node.Parent;
          condition = node.Tag as Condition;
          dummyCondition = node.Tag as DummyCondition;
        }

        // read layer and set layer combo
        comboBoxLayer.SelectedIndex = ((Layer) node.Parent.Tag).Value;

        // set condition
        SetCurrentCondition(condition);

        foreach (TreeNode typeNode in node.Nodes)
        {
          // set sound
          sound = typeNode.Tag as Sound;
          if (!ReferenceEquals(sound, null))
            Visualize(sound);

          command = typeNode.Tag as Command;
          if (!ReferenceEquals(command, null))
            Visualize(command);
        }
      }
    }

    private void treeMapping_DoubleClick(object sender, EventArgs e)
    {
      TreeNode node = treeMapping.SelectedNode;
      if (ReferenceEquals(node, null)) return;
      if (ReferenceEquals(node.Tag, null)) return;

      Command command = node.Tag as Command;
      if (ReferenceEquals(command, null)) return;

      if (MPControlPlugin.CommandProcessor.Edit(command, this))
      {
        node.Tag = command;
        node.Text = command.UserDisplayText;
      }
    }

    #endregion mapping treenode

    private bool updateFromTreeView = false;

    #region Layer

    private void SetLayer(object sender, EventArgs e)
    {
      TreeNode node = getNode(typeof (Layer));
      Layer layer = new Layer {Value = comboBoxLayer.SelectedIndex};

      node.Tag = layer;
      node.Text = layer.UserDisplayText;

      _changedSettings = true;
    }

    #endregion Layer

    #region Condition

    private void PopulateConditionList()
    {
      _uiTextCategoryCache.Clear();
      conditionComboBox.Items.Clear();

      foreach (Type type in Condition.GetBuiltInConditions())
      {
        Condition condition = (Condition)Activator.CreateInstance(type);

        _uiTextCategoryCache.Add(condition.UserInterfaceText, type);
        conditionComboBox.Items.Add(condition.UserInterfaceText);
      }
    }

    private void conditionComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      // condition panel will be set within TreeViewAfterSelect
      if (updateFromTreeView) return;

      string uiText = conditionComboBox.SelectedItem as string;
      SetCurrentCondition(uiText);
    }

    private void SetCurrentCondition(string uiText)
    {
      // save current values in temp storage
      if (_currentCondition != null && _currentConditionConfig != null)
      {
        _currentCondition.Property = _currentConditionConfig.Property;
        _conditionStorage[_currentCondition.UserInterfaceText] = _currentCondition;
      }

      conditionPanel.Controls.Clear();
      //load if command is already cached
      if (_conditionStorage.ContainsKey(uiText))
      {
        _currentCondition = _conditionStorage[uiText];
      }
      else
      {
        Type newType = _uiTextCategoryCache[uiText];
        Condition newCondition = (Condition)Activator.CreateInstance(newType);
        _currentCondition = newCondition;
      }

      _currentConditionConfig = _currentCondition.GetEditControl();
      _currentConditionConfig.Dock = DockStyle.Fill;
      _currentConditionConfig.OnPropertyChanged += UpdateConditionInMapping;
      conditionPanel.Controls.Add(_currentConditionConfig);
      UpdateConditionInMapping();
    }

    /// <summary>
    /// Sets the current active condition.
    /// </summary>
    private void SetCurrentCondition(Condition condition)
    {
      if (ReferenceEquals(condition, null)) return;

      // set combobox value without calling SetCurrentCondition again
      updateFromTreeView = true;
      conditionComboBox.SelectedItem = condition.UserInterfaceText;
      updateFromTreeView = false;

      // clear cache, to prevent multiple usage of the same condition instances
      _conditionStorage.Clear();

      conditionPanel.Controls.Clear();
      _currentCondition = condition;
      _currentConditionConfig = _currentCondition.GetEditControl();
      _currentConditionConfig.Dock = DockStyle.Fill;
      _currentConditionConfig.OnPropertyChanged += UpdateConditionInMapping;
      conditionPanel.Controls.Add(_currentConditionConfig);

      //buttonTest.Enabled = _currentCommand.IsTestAllowed;

      #region comments

      //switch ((string)data.Parameter)
      //{
      //  case "WINDOW":
      //    {
      //      comboBoxCondProperty.DropDownStyle = ComboBoxStyle.DropDownList;

      //      radioButtonWindow.Checked = true;
      //      comboBoxCondProperty.Enabled = true;

      //      string friendlyName;
      //      try
      //      {
      //        friendlyName = GetFriendlyName(Enum.GetName(typeof(GUIWindow.Window), Convert.ToInt32(data.Value)));
      //      }
      //      catch
      //      {
      //        friendlyName = Convert.ToInt32(data.Value).ToString();
      //      }
      //      UpdateCombo(ref comboBoxCondProperty, _windowsList, friendlyName);
      //      break;
      //    }
      //  case "FULLSCREEN":
      //    comboBoxCondProperty.DropDownStyle = ComboBoxStyle.DropDownList;

      //    radioButtonFullscreen.Checked = true;
      //    comboBoxCondProperty.Enabled = true;
      //    if (Convert.ToBoolean(data.Value))
      //      UpdateCombo(ref comboBoxCondProperty, _fullScreenList, "Fullscreen");
      //    else
      //      UpdateCombo(ref comboBoxCondProperty, _fullScreenList, "No Fullscreen");
      //    break;
      //  case "PLAYER":
      //    comboBoxCondProperty.DropDownStyle = ComboBoxStyle.DropDownList;

      //    radioButtonPlaying.Checked = true;
      //    comboBoxCondProperty.Enabled = true;
      //    UpdateCombo(ref comboBoxCondProperty, _playerList,
      //                _playerList[Array.IndexOf(_nativePlayerList, (string)data.Value)]);
      //    break;
      //  case "PLUGIN":
      //    comboBoxCondProperty.DropDownStyle = ComboBoxStyle.DropDownList;

      //    radioButtonPlugin.Checked = true;
      //    comboBoxCondProperty.Enabled = true;
      //    UpdateCombo(ref comboBoxCondProperty, _pluginList.ToArray(), (string)data.Value);
      //    break;
      //  case "*":
      //    comboBoxCondProperty.DropDownStyle = ComboBoxStyle.DropDownList;

      //    comboBoxCondProperty.Text = "none";
      //    radioButtonNoCondition.Checked = true;
      //    comboBoxCondProperty.Enabled = false;
      //    comboBoxCondProperty.Items.Clear();
      //    break;
      //}

      #endregion
    }

    private void UpdateConditionInMapping()
    {
      TreeNode node = getNode(typeof(Condition));
      if (ReferenceEquals(node,null)) return;

      _currentCondition.Property = _currentConditionConfig.Property;
      node.Tag = _currentCondition;
      node.Text = _currentCondition.UserDisplayText;
    }

    #endregion Condition

    #region Command

    private void PopulateCommandList()
    {
      treeViewCommandList.Nodes.Clear();
      Dictionary<string, TreeNode> categoryNodes = new Dictionary<string, TreeNode>(MPControlPlugin.CommandCategories.Length);

      // Create requested categories ...
      foreach (string category in MPControlPlugin.CommandCategories)
      {
        TreeNode categoryNode = new TreeNode(category);
        //categoryNode.NodeFont = new Font(treeViewCommandList.Font, FontStyle.Underline);
        categoryNodes.Add(category, categoryNode);
      }

      List<Type> allCommands = new List<Type>();

      Type[] specialCommands = Processor.GetBuiltInCommands();
      allCommands.AddRange(specialCommands);

      Type[] libCommands = Processor.GetLibraryCommands();
      if (libCommands != null)
        allCommands.AddRange(libCommands);

      foreach (Type type in allCommands)
      {
        Command command = (Command)Activator.CreateInstance(type);

        string commandCategory = command.Category;

        if (categoryNodes.ContainsKey(commandCategory))
        {
          TreeNode newNode = new TreeNode(command.UserInterfaceText);
          newNode.Tag = type;

          categoryNodes[commandCategory].Nodes.Add(newNode);
        }
      }

      // Put all commands into tree view ...
      foreach (TreeNode treeNode in categoryNodes.Values)
        if (treeNode.Nodes.Count > 0)
          treeViewCommandList.Nodes.Add(treeNode);

      treeViewCommandList.SelectedNode = treeViewCommandList.Nodes[0];
      treeViewCommandList.SelectedNode.Expand();
    }

    private void treeViewCommandList_DoubleClick(object sender, EventArgs e)
    {
      Type commandType = treeViewCommandList.SelectedNode.Tag as Type;
      if (ReferenceEquals(commandType, null)) return;

      // get mapping node
      TreeNode mappingNode = getNode(typeof(Command));
      if (ReferenceEquals(mappingNode, null)) return;

      Command command = (Command)Activator.CreateInstance(commandType);
      if (MPControlPlugin.CommandProcessor.Edit(command, this))
      {
        mappingNode.Text = command.UserDisplayText;
        mappingNode.Tag = command;
      }
    }

    private void Visualize(Command command)
    {
      #region comments

      //                  checkBoxGainFocus.Checked = data.Focus;
      //                  switch ((string) data.Parameter)
      //                  {
      //                    case "ACTION":
      //                      comboBoxCmdProperty.DropDownStyle = ComboBoxStyle.DropDownList;
      //                      radioButtonAction.Checked = true;
      //                      comboBoxSound.Enabled = true;
      //                      comboBoxCmdProperty.Enabled = true;
      //                      textBoxKeyChar.Enabled = textBoxKeyCode.Enabled = false;
      //                      textBoxKeyChar.Text = textBoxKeyCode.Text = String.Empty;
      //                      UpdateCombo(ref comboBoxCmdProperty, _actionList.ToArray(),
      //                                  GetFriendlyName(Enum.GetName(typeof (Action.ActionType), Convert.ToInt32(data.Value))));
      //                      break;
      //                    case "KEY":
      //                      comboBoxCmdProperty.DropDownStyle = ComboBoxStyle.DropDownList;
      //                      radioButtonAction.Checked = true;
      //                      textBoxKeyChar.Enabled = textBoxKeyCode.Enabled = true;
      //                      textBoxKeyChar.Text = ((Key) data.Value).KeyChar.ToString();
      //                      textBoxKeyCode.Text = ((Key) data.Value).KeyCode.ToString();
      //                      comboBoxCmdProperty.Enabled = true;
      //                      UpdateCombo(ref comboBoxCmdProperty, _actionList.ToArray(), "Key Pressed");
      //                      break;
      //                    case "WINDOW":
      //                      {
      //                        comboBoxCmdProperty.DropDownStyle = ComboBoxStyle.DropDownList;
      //                        radioButtonActWindow.Checked = true;
      //                        comboBoxSound.Enabled = true;
      //                        comboBoxCmdProperty.Enabled = true;
      //                        textBoxKeyChar.Enabled = textBoxKeyCode.Enabled = false;
      //                        textBoxKeyChar.Text = textBoxKeyCode.Text = String.Empty;

      //                        string friendlyName;
      //                        try
      //                        {
      //                          friendlyName =
      //                            GetFriendlyName(Enum.GetName(typeof (GUIWindow.Window), Convert.ToInt32(data.Value)));
      //                        }
      //                        catch
      //                        {
      //                          friendlyName = Convert.ToInt32(data.Value).ToString();
      //                        }
      //                        UpdateCombo(ref comboBoxCmdProperty, _windowsListFiltered, friendlyName);
      //                        break;
      //                      }
      //                    case "TOGGLE":
      //                      radioButtonToggle.Checked = true;
      //                      comboBoxSound.Enabled = true;
      //                      comboBoxCmdProperty.Enabled = false;
      //                      textBoxKeyChar.Enabled = textBoxKeyCode.Enabled = false;
      //                      textBoxKeyChar.Text = textBoxKeyCode.Text = String.Empty;
      //                      comboBoxCmdProperty.Items.Clear();
      //                      comboBoxCmdProperty.Text = String.Empty;
      //                      break;
      //                    case "POWER":
      //                      comboBoxCmdProperty.DropDownStyle = ComboBoxStyle.DropDownList;
      //                      radioButtonPower.Checked = true;
      //                      comboBoxSound.Enabled = true;
      //                      comboBoxCmdProperty.Enabled = true;
      //                      textBoxKeyChar.Enabled = textBoxKeyCode.Enabled = false;
      //                      textBoxKeyChar.Text = textBoxKeyCode.Text = String.Empty;
      //                      UpdateCombo(ref comboBoxCmdProperty, _powerList,
      //                                  _powerList[Array.IndexOf(_nativePowerList, (string) data.Value)]);
      //                      break;
      //                    case "PROCESS":
      //                      comboBoxCmdProperty.DropDownStyle = ComboBoxStyle.DropDownList;
      //                      radioButtonProcess.Checked = true;
      //                      comboBoxSound.Enabled = true;
      //                      comboBoxCmdProperty.Enabled = true;
      //                      textBoxKeyChar.Enabled = textBoxKeyCode.Enabled = false;
      //                      textBoxKeyChar.Text = textBoxKeyCode.Text = String.Empty;
      //                      UpdateCombo(ref comboBoxCmdProperty, _processList,
      //                                  _processList[Array.IndexOf(_nativeProcessList, (string) data.Value)]);
      //                      break;
      //                    case "BLAST":
      //                      comboBoxCmdProperty.DropDownStyle = ComboBoxStyle.DropDownList;
      //                      radioButtonBlast.Checked = true;
      //                      comboBoxSound.Enabled = true;
      //                      comboBoxCmdProperty.Enabled = true;
      //                      textBoxKeyChar.Enabled = textBoxKeyCode.Enabled = false;
      //                      textBoxKeyChar.Text = textBoxKeyCode.Text = String.Empty;
      //#warning fixme FileList
      //                      //UpdateCombo(ref comboBoxCmdProperty, MPControlPlugin.GetFileList(true), (string) data.Value);
      //                      break;

      #endregion
    }

    #endregion Command

    #region Sound

    private void Visualize(Sound sound)
    {
      if (string.IsNullOrEmpty(sound.Value))
        comboBoxSound.SelectedItem = NO_SOUND;
      else
        comboBoxSound.SelectedItem = sound.Value;
    }

    private void SetSound(object sender, EventArgs e)
    {
      TreeNode node = getNode(typeof (Sound));
      Sound sound = new Sound();

      string strSound = comboBoxSound.SelectedItem as string;
      if (!strSound.Equals(NO_SOUND))
        sound.Value = strSound;

      node.Tag = sound;
      node.Text = sound.UserDisplayText;

      _changedSettings = true;
    }

    #endregion Sound

    #region comments

    //private static void UpdateCombo(ref ComboBox comboBox, Array list, string hilight)
    //{
    //  comboBox.Items.Clear();
    //  foreach (object item in list)
    //    comboBox.Items.Add(item.ToString());
    //  comboBox.Text = hilight;
    //  comboBox.SelectedItem = hilight;
    //  comboBox.Enabled = true;
    //}

    //private static void UpdateCombo(ref ComboBox comboBox, ArrayList list, string hilight)
    //{
    //  UpdateCombo(ref comboBox, list.ToArray(), hilight);
    //}

    //private static string GetFriendlyName(string name)
    //{
    //  if ((name.IndexOf("ACTION") != -1) || (name.IndexOf("WINDOW") != -1))
    //    name = name.Substring(7);

    //  bool upcase = true;
    //  string newName = String.Empty;

    //  foreach (char c in name)
    //  {
    //    if (c == '_')
    //    {
    //      newName += " ";
    //      upcase = true;
    //    }
    //    else if (upcase)
    //    {
    //      newName += c.ToString();
    //      upcase = false;
    //    }
    //    else
    //    {
    //      newName += c.ToString().ToLower();
    //    }
    //  }

    //  CleanAbbreviation(ref newName, "TV");
    //  CleanAbbreviation(ref newName, "DVD");
    //  CleanAbbreviation(ref newName, "UI");
    //  CleanAbbreviation(ref newName, "Guide");
    //  CleanAbbreviation(ref newName, "MSN");
    //  CleanAbbreviation(ref newName, "OSD");
    //  CleanAbbreviation(ref newName, "LCD");
    //  CleanAbbreviation(ref newName, "EPG");
    //  CleanAbbreviation(ref newName, "DVBC");
    //  CleanAbbreviation(ref newName, "DVBS");
    //  CleanAbbreviation(ref newName, "DVBT");

    //  return newName;
    //}

    //private static string GetWindowName(string friendlyName)
    //{
    //  return "WINDOW_" + friendlyName.Replace(' ', '_').ToUpper();
    //}

    //private static string GetActionName(string friendlyName)
    //{
    //  string actionName = String.Empty;

    //  try
    //  {
    //    if (Enum.Parse(typeof (Action.ActionType), "ACTION_" + friendlyName.Replace(' ', '_').ToUpper()) != null)
    //      actionName = "ACTION_" + friendlyName.Replace(' ', '_').ToUpper();
    //  }
    //  catch (ArgumentException)
    //  {
    //    try
    //    {
    //      if (Enum.Parse(typeof (Action.ActionType), friendlyName.Replace(' ', '_').ToUpper()) != null)
    //        actionName = friendlyName.Replace(' ', '_').ToUpper();
    //    }
    //    catch (ArgumentException)
    //    {
    //    }
    //  }

    //  return actionName;
    //}


    //private static void CleanAbbreviation(ref string name, string abbreviation)
    //{
    //  int index = name.ToUpper().IndexOf(abbreviation.ToUpper());
    //  if (index != -1)
    //    name = name.Substring(0, index) + abbreviation + name.Substring(index + abbreviation.Length);
    //}

    //private void radioButtonAction_Click(object sender, EventArgs e)
    //{
    //  textBoxKeyChar.Enabled = textBoxKeyCode.Enabled = false;
    //  textBoxKeyChar.Text = textBoxKeyCode.Text = String.Empty;
    //  comboBoxCmdProperty.DropDownStyle = ComboBoxStyle.DropDownList;
    //  comboBoxSound.Enabled = true;
    //  comboBoxCmdProperty.Enabled = true;
    //  TreeNode node = getNode("COMMAND");
    //  Data data = new Data("COMMAND", "ACTION", "7");
    //  node.Tag = data;
    //  UpdateCombo(ref comboBoxCmdProperty, _actionList.ToArray(),
    //              GetFriendlyName(Enum.GetName(typeof (Action.ActionType), Convert.ToInt32(data.Value))));
    //  node.Text = "Action \"" + (string) comboBoxCmdProperty.SelectedItem + "\"";
    //  ((Data) node.Tag).Focus = checkBoxGainFocus.Checked;
    //  _changedSettings = true;
    //}

    //private void radioButtonActWindow_Click(object sender, EventArgs e)
    //{
    //  comboBoxCmdProperty.DropDownStyle = ComboBoxStyle.DropDownList;
    //  comboBoxSound.Enabled = true;
    //  comboBoxCmdProperty.Enabled = true;
    //  TreeNode node = getNode("COMMAND");
    //  Data data = new Data("COMMAND", "WINDOW", "0");
    //  node.Tag = data;

    //  string friendlyName;
    //  try
    //  {
    //    friendlyName = GetFriendlyName(Enum.GetName(typeof (GUIWindow.Window), Convert.ToInt32(data.Value)));
    //  }
    //  catch
    //  {
    //    friendlyName = Convert.ToInt32(data.Value).ToString();
    //  }
    //  UpdateCombo(ref comboBoxCmdProperty, _windowsListFiltered, friendlyName);

    //  node.Text = "Window \"" + comboBoxCmdProperty.Text + "\"";
    //  ((Data) node.Tag).Focus = checkBoxGainFocus.Checked;
    //  _changedSettings = true;
    //}

    //private void radioButtonToggle_Click(object sender, EventArgs e)
    //{
    //  textBoxKeyChar.Enabled = textBoxKeyCode.Enabled = false;
    //  textBoxKeyChar.Text = textBoxKeyCode.Text = String.Empty;
    //  comboBoxSound.Enabled = true;
    //  comboBoxCmdProperty.Enabled = false;
    //  comboBoxCmdProperty.Items.Clear();
    //  comboBoxCmdProperty.Text = "none";
    //  TreeNode node = getNode("COMMAND");
    //  Data data = new Data("COMMAND", "TOGGLE", "-1");
    //  node.Tag = data;
    //  node.Text = "Toggle Layer";
    //  ((Data) node.Tag).Focus = checkBoxGainFocus.Checked;
    //  _changedSettings = true;
    //}

    //private void radioButtonPower_Click(object sender, EventArgs e)
    //{
    //  textBoxKeyChar.Enabled = textBoxKeyCode.Enabled = false;
    //  textBoxKeyChar.Text = textBoxKeyCode.Text = String.Empty;
    //  comboBoxCmdProperty.DropDownStyle = ComboBoxStyle.DropDownList;
    //  comboBoxSound.Enabled = true;
    //  comboBoxCmdProperty.Enabled = true;
    //  TreeNode node = getNode("COMMAND");
    //  node.Tag = new Data("COMMAND", "POWER", "EXIT");
    //  node.Text = _powerList[0];
    //  UpdateCombo(ref comboBoxCmdProperty, _powerList, _powerList[0]);
    //  ((Data) node.Tag).Focus = checkBoxGainFocus.Checked;
    //  _changedSettings = true;
    //}

    //private void radioButtonProcess_Click(object sender, EventArgs e)
    //{
    //  textBoxKeyChar.Enabled = textBoxKeyCode.Enabled = false;
    //  textBoxKeyChar.Text = textBoxKeyCode.Text = String.Empty;
    //  comboBoxCmdProperty.DropDownStyle = ComboBoxStyle.DropDownList;
    //  comboBoxSound.Enabled = true;
    //  comboBoxCmdProperty.Enabled = true;
    //  TreeNode node = getNode("COMMAND");
    //  node.Tag = new Data("COMMAND", "PROCESS", "CLOSE");
    //  node.Text = _processList[0];
    //  UpdateCombo(ref comboBoxCmdProperty, _processList, _processList[0]);
    //  ((Data) node.Tag).Focus = checkBoxGainFocus.Checked;
    //  _changedSettings = true;
    //}

    #endregion

    #region window buttons

    private void buttonOk_Click(object sender, EventArgs e)
    {
      string customFile = MPUtils.MPCommon.GetCustomMappingFilePath(_inputClassName);
      InputMapping.Save(inputMapping, customFile);

      Close();
    }

    private void buttonApply_Click(object sender, EventArgs e)
    {
      string customFile = MPUtils.MPCommon.GetCustomMappingFilePath(_inputClassName);
      InputMapping.Save(inputMapping, customFile);
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void buttonDefault_Click(object sender, EventArgs e)
    {
      string defaultFile = MPUtils.MPCommon.GetDefaultMappingFilePath(_inputClassName);
      inputMapping = InputMapping.Load(defaultFile);

      RefreshTreeView();
    }

    private void buttonHelp_Click(object sender, EventArgs e)
    {
      IrssUtils.IrssHelp.Open(this.GetType().FullName);
    }

    private void InputMappingForm_HelpRequested(object sender, HelpEventArgs hlpevent)
    {
      buttonHelp_Click(null, null);
      hlpevent.Handled = true;
    }

    #endregion window buttons

    #region toolstrip buttons

    private void buttonUp_Click(object sender, EventArgs e)
    {
      bool expanded = false;
      TreeNode node = treeMapping.SelectedNode;

      if (!(node.Tag is InputMapping.Button))
        expanded = node.IsExpanded;

      if (node.Tag is Command || node.Tag is Sound)
      {
        node = node.Parent;
        expanded = true;
      }

      if (!(node.Tag is Button) && !(node.Tag is Condition)) return;
      if (node.Index <= 0) return;

      int index = node.Index - 1;
      TreeNode tmpNode = (TreeNode) node.Clone();
      TreeNode parentNode = node.Parent;
      node.Remove();
      if (expanded)
        tmpNode.Expand();
      parentNode.Nodes.Insert(index, tmpNode);
      treeMapping.SelectedNode = tmpNode;

      _changedSettings = true;
    }

    private void buttonDown_Click(object sender, EventArgs e)
    {
      bool expanded = false;
      TreeNode node = treeMapping.SelectedNode;

      if (!(node.Tag is InputMapping.Button))
        expanded = node.IsExpanded;

      if (node.Tag is Command || node.Tag is Sound)
      {
        node = node.Parent;
        expanded = true;
      }

      if (!(node.Tag is Button) && !(node.Tag is Condition)) return;
      if (node.Index >= node.Parent.Nodes.Count - 1) return;

      int index = node.Index + 1;
      TreeNode tmpNode = (TreeNode) node.Clone();
      TreeNode parentNode = node.Parent;
      node.Remove();
      if (expanded)
        tmpNode.Expand();
      parentNode.Nodes.Insert(index, tmpNode);
      treeMapping.SelectedNode = tmpNode;

      _changedSettings = true;
    }

    private void buttonRemove_Click(object sender, EventArgs e)
    {
      TreeNode node = treeMapping.SelectedNode;
      if (node.Tag is Sound || node.Tag is Command)
        node = getNode(typeof (Condition));

      string strObject = node.Tag.GetType().Name.ToLower();
      DialogResult result = MessageBox.Show(this, "Are you sure you want to remove this " + strObject + "?",
                                            "Remove " + strObject,
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
      Layer layer = new Layer {Value = 0};
      TreeNode newLayer = new TreeNode(layer.UserDisplayText);
      newLayer.Tag = layer;
      newLayer.ForeColor = Color.DimGray;

      Condition condition = new NoCondition();
      TreeNode newCondition = new TreeNode(condition.UserDisplayText);
      newCondition.Tag = condition;
      newCondition.ForeColor = Color.Blue;

      Command command = Processor.CreateCommand("IrssCommands.MediaPortal.CommandGetFocus");
      TreeNode newCommand = new TreeNode(command.UserDisplayText);
      newCommand.Tag = command;
      newCommand.ForeColor = Color.DarkGreen;

      Sound sound = new Sound();
      TreeNode newSound = new TreeNode(sound.UserDisplayText);
      newSound.Tag = sound;
      newSound.ForeColor = Color.DarkRed;


      TreeNode node = treeMapping.SelectedNode;
      if (node.Tag is Layer)
      {
        newCondition.Nodes.Add(newCommand);
        newCondition.Nodes.Add(newSound);
        newLayer.Nodes.Add(newCondition);
        node.Parent.Nodes.Add(newLayer);
        newLayer.Expand();
        treeMapping.SelectedNode = newLayer;
      }
      else if (node.Tag is Condition)
      {
        newCondition.Nodes.Add(newCommand);
        newCondition.Nodes.Add(newSound);
        node.Parent.Nodes.Add(newCondition);
        newCondition.Expand();
        treeMapping.SelectedNode = newCondition;
      }
      else if (node.Tag is Command || node.Tag is Sound)
      {
        newCondition.Nodes.Add(newCommand);
        newCondition.Nodes.Add(newSound);
        node.Parent.Parent.Nodes.Add(newCondition);
        newCondition.Expand();
        treeMapping.SelectedNode = newCondition;
      }

      #region comments

      //case "BUTTON":
      //newCondition.Nodes.Add(newCommand);
      //newCondition.Nodes.Add(newSound);
      //newLayer.Nodes.Add(newCondition);
      //node.Nodes.Add(newLayer);
      //newLayer.Expand();
      //treeMapping.SelectedNode = newLayer;

      //default:
      ////NewButtonForm newButtonForm = new NewButtonForm();
      ////newButtonForm.ShowDialog();
      ////if (newButtonForm.Accepted)
      ////{
      ////  Log.Info("Name: {0}", newButtonForm.ButtonName);
      ////  Log.Info("Code: {0}", newButtonForm.ButtonCode);
      ////}

      #endregion

      _changedSettings = true;
      treeMapping_AfterSelect(this, new TreeViewEventArgs(treeMapping.SelectedNode, TreeViewAction.ByKeyboard));
    }

    private void Expand(object sender, EventArgs e)
    {
      if (treeMapping.SelectedNode == null)
        treeMapping.Select();
      treeMapping.SelectedNode.ExpandAll();
    }

    private void Collapse(object sender, EventArgs e)
    {
      if (treeMapping.SelectedNode == null)
        treeMapping.Select();
      treeMapping.SelectedNode.Collapse(false);
    }

    #endregion toolstrip buttons


    #region comments

    //private void textBoxKeyCode_KeyPress(object sender, KeyPressEventArgs e)
    //{
    //  if (!char.IsNumber(e.KeyChar) && e.KeyChar != 8)
    //  {
    //    e.Handled = true;
    //  }
    //}

    //private void textBoxKeyChar_KeyPress(object sender, KeyPressEventArgs e)
    //{
    //  if (!char.IsNumber(e.KeyChar) && e.KeyChar != 8)
    //  {
    //    e.Handled = true;
    //  }
    //}

    //private void comboBoxCondProperty_TextChanged(object sender, EventArgs e)
    //{
    //  if (!((Control) sender).Focused) return;

    //  ConditionPropChanged();
    //}

    //private void comboBoxCmdProperty_TextChanged(object sender, EventArgs e)
    //{
    //  if (!((Control) sender).Focused) return;

    //  CommandPropChanged();
    //}

    //private void comboBoxCondProperty_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  if (!comboBoxCondProperty.Focused) return;
    //  if (comboBoxCondProperty.DropDownStyle != ComboBoxStyle.DropDownList) return;

    //  ConditionPropChanged();
    //}

    //private void comboBoxCmdProperty_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  if (!comboBoxCmdProperty.Focused) return;
    //  if (comboBoxCmdProperty.DropDownStyle != ComboBoxStyle.DropDownList) return;

    //  CommandPropChanged();
    //}

    //private void textBoxKeyChar_KeyUp(object sender, KeyEventArgs e)
    //{
    //  string keyChar = textBoxKeyChar.Text;
    //  string keyCode = textBoxKeyCode.Text;
    //  TreeNode node = getNode("COMMAND");
    //  if (String.IsNullOrEmpty(keyChar))
    //    keyChar = "0";
    //  if (String.IsNullOrEmpty(keyCode))
    //    keyCode = "0";
    //  Key key = new Key(Convert.ToInt32(keyChar), Convert.ToInt32(keyCode));
    //  node.Tag = new Data("COMMAND", "KEY", key);
    //  node.Text = String.Format("Key Pressed: {0} [{1}]", keyChar, keyCode);
    //  ((Data) node.Tag).Focus = checkBoxGainFocus.Checked;
    //  _changedSettings = true;
    //}

    //private void textBoxKeyCode_KeyUp(object sender, KeyEventArgs e)
    //{
    //  textBoxKeyChar_KeyUp(sender, e);
    //}

    //private void checkBoxGainFocus_CheckedChanged(object sender, EventArgs e)
    //{
    //  TreeNode node = getNode("COMMAND");
    //  ((Data) node.Tag).Focus = checkBoxGainFocus.Checked;
    //  _changedSettings = true;
    //}

    //private void radioButtonBlast_Click(object sender, EventArgs e)
    //{
    //      textBoxKeyChar.Enabled = textBoxKeyCode.Enabled = false;
    //      textBoxKeyChar.Text = textBoxKeyCode.Text = String.Empty;
    //      comboBoxSound.Enabled = true;
    //      comboBoxCmdProperty.Enabled = false;
    //      comboBoxCmdProperty.Items.Clear();
    //      comboBoxCmdProperty.Text = "none";
    //      TreeNode node = getNode("COMMAND");
    //      Data data = new Data("COMMAND", "BLAST", String.Empty);
    //      node.Tag = data;
    //      node.Text = "";
    //      ((Data) node.Tag).Focus = checkBoxGainFocus.Checked;
    //#warning fixme FileList
    //      //UpdateCombo(ref comboBoxCmdProperty, MPControlPlugin.GetFileList(true), String.Empty);
    //      _changedSettings = true;
    //}

    //    private void CommandPropChanged()
    //    {
    //      if ((string) comboBoxCmdProperty.SelectedItem == "Key Pressed")
    //        textBoxKeyChar.Enabled = textBoxKeyCode.Enabled = true;
    //      else
    //      {
    //        textBoxKeyChar.Enabled = textBoxKeyCode.Enabled = false;
    //        textBoxKeyChar.Text = textBoxKeyCode.Text = String.Empty;
    //      }

    //      TreeNode node = getNode("COMMAND");
    //      Data data = (Data) node.Tag;
    //      switch ((string) data.Parameter)
    //      {
    //        case "ACTION":
    //          if ((string) comboBoxCmdProperty.SelectedItem != "Key Pressed")
    //          {
    //            node.Tag = new Data("COMMAND", "ACTION",
    //                                (int)
    //                                Enum.Parse(typeof (Action.ActionType),
    //                                           GetActionName((string) comboBoxCmdProperty.SelectedItem)));
    //            node.Text = "Action \"" + (string) comboBoxCmdProperty.SelectedItem + "\"";
    //          }
    //          else
    //          {
    //            textBoxKeyChar.Text = "0";
    //            textBoxKeyCode.Text = "0";
    //            Key key = new Key(Convert.ToInt32(textBoxKeyChar.Text), Convert.ToInt32(textBoxKeyCode.Text));
    //            node.Tag = new Data("COMMAND", "KEY", key);
    //            node.Text = String.Format("Key Pressed: {0} [{1}]", textBoxKeyChar.Text, textBoxKeyCode.Text);
    //          }
    //          break;
    //        case "WINDOW":
    //          {
    //            int windowID;
    //            try
    //            {
    //              windowID = (int) Enum.Parse(typeof (GUIWindow.Window), GetWindowName(comboBoxCmdProperty.Text));
    //            }
    //            catch
    //            {
    //              windowID = Convert.ToInt32(comboBoxCmdProperty.Text);
    //            }

    //            node.Tag = new Data("COMMAND", "WINDOW", windowID);
    //            node.Text = "Window \"" + comboBoxCmdProperty.Text + "\"";
    //            break;
    //          }
    //        case "POWER":
    //          node.Tag = new Data("COMMAND", "POWER",
    //                              _nativePowerList[Array.IndexOf(_powerList, (string) comboBoxCmdProperty.SelectedItem)]);
    //          node.Text = (string) comboBoxCmdProperty.SelectedItem;
    //          break;
    //        case "PROCESS":
    //          node.Tag = new Data("COMMAND", "PROCESS",
    //                              _nativeProcessList[Array.IndexOf(_processList, (string) comboBoxCmdProperty.SelectedItem)]);
    //          node.Text = (string) comboBoxCmdProperty.SelectedItem;
    //          break;
    //        case "BLAST":
    //          {
    //            string text = (string) comboBoxCmdProperty.SelectedItem;
    //            if (text.StartsWith(IrssUtils.Common.CmdPrefixBlast, StringComparison.InvariantCultureIgnoreCase))
    //            {
    //              BlastCommand blastCommand = new BlastCommand(
    //                MPControlPlugin.BlastIR,
    //                IrssUtils.Common.FolderIRCommands,
    //                MPControlPlugin.TransceiverInformation.Ports,
    //                text.Substring(IrssUtils.Common.CmdPrefixBlast.Length));

    //              if (blastCommand.ShowDialog(this) == DialogResult.OK)
    //              {
    //                string command = IrssUtils.Common.CmdPrefixBlast + blastCommand.CommandString;
    //                node.Tag = new Data("COMMAND", "BLAST", command);
    //                node.Text = command;
    //              }
    //            }
    //#warning fixme cmd Macro
    //            //else if (text.StartsWith(IrssUtils.Common.CmdPrefixMacro, StringComparison.InvariantCultureIgnoreCase))
    //            //{
    //            //  node.Tag = new Data("COMMAND", "BLAST", text);
    //            //  node.Text = text;
    //            //}
    //            break;
    //          }
    //      }
    //      ((Data) node.Tag).Focus = checkBoxGainFocus.Checked;
    //      _changedSettings = true;
    //    }

    //private void ConditionPropChanged()
    //{
    //  TreeNode node = getNode("CONDITION");
    //  Data data = (Data) node.Tag;
    //  switch ((string) data.Parameter)
    //  {
    //    case "WINDOW":
    //      {
    //        int windowID;
    //        try
    //        {
    //          windowID = (int) Enum.Parse(typeof (GUIWindow.Window), GetWindowName(comboBoxCondProperty.Text));
    //        }
    //        catch
    //        {
    //          windowID = Convert.ToInt32(comboBoxCondProperty.Text);
    //        }

    //        node.Tag = new Data("CONDITION", "WINDOW", windowID);
    //        node.Text = comboBoxCondProperty.Text;
    //        break;
    //      }
    //    case "FULLSCREEN":
    //      if ((string) comboBoxCondProperty.SelectedItem == "Fullscreen")
    //        node.Tag = new Data("CONDITION", "FULLSCREEN", "true");
    //      else
    //        node.Tag = new Data("CONDITION", "FULLSCREEN", "false");
    //      node.Text = (string) comboBoxCondProperty.SelectedItem;
    //      break;
    //    case "PLAYER":
    //      {
    //        node.Tag = new Data("CONDITION", "PLAYER",
    //                            _nativePlayerList[Array.IndexOf(_playerList, (string) comboBoxCondProperty.SelectedItem)
    //                              ]);
    //        node.Text = (string) comboBoxCondProperty.SelectedItem;
    //        break;
    //      }
    //    case "PLUGIN":
    //      {
    //        node.Tag = new Data("CONDITION", "PLUGIN", comboBoxCondProperty.Text);
    //        node.Text = comboBoxCondProperty.Text;
    //        break;
    //      }
    //    case "*":
    //      break;
    //  }
    //  _changedSettings = true;
    //}

    #endregion

    internal class Layer
    {
      public int Value { get; set; }

      public string UserDisplayText
      {
        get { return _layerList[Value]; }
      }
    }

    internal class Sound
    {
      public string Value { get; set; }

      public string UserDisplayText
      {
        get { return string.IsNullOrEmpty(Value) ? NO_SOUND : Value; }
      }
    }

    private class ConditionComboBoxItem
    {
      public readonly Type _conditionType;

      public ConditionComboBoxItem(Type type)
      {
        _conditionType = type;
      }

      public override string ToString()
      {
        Condition condition = (Condition)Activator.CreateInstance(_conditionType);

        return condition.UserInterfaceText;
      }
    }

    #region comments

    //#region Nested type: Data

    //private class Data
    //{
    //  private readonly string type;
    //  private object dataValue;
    //  private bool focus;
    //  private object parameter;

    //  public Data(object newType, object newParameter, object newValue)
    //  {
    //    if (newValue == null)
    //      newValue = String.Empty;
    //    if (newParameter == null)
    //      newParameter = String.Empty;
    //    type = (string) newType;
    //    dataValue = newValue;
    //    parameter = newParameter;
    //  }

    //  public Data(object newType, object newParameter, object newValue, bool newFocus)
    //  {
    //    if (newValue == null)
    //      newValue = String.Empty;
    //    if (newParameter == null)
    //      newParameter = String.Empty;
    //    type = (string) newType;
    //    dataValue = newValue;
    //    parameter = newParameter;
    //    focus = newFocus;
    //  }

    //  public string Type
    //  {
    //    get { return type; }
    //  }

    //  public object Value
    //  {
    //    get { return dataValue; }
    //    set { dataValue = value; }
    //  }

    //  public object Parameter
    //  {
    //    get { return parameter; }
    //    set { parameter = value; }
    //  }

    //  public bool Focus
    //  {
    //    get { return focus; }
    //    set { focus = value; }
    //  }
    //}

    //#endregion

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

    #endregion
  }
}