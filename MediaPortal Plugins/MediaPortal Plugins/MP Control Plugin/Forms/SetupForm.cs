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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using IrssCommands.Forms;
using IrssComms;
using IrssUtils;
using IrssUtils.Forms;
using MediaPortal.GUI.Library;
using MediaPortal.Hardware;
using MPUtils;
using MediaPortal.Plugins.IRSS.MPControlPlugin.InputMapper;
using IrssCommands;

namespace MediaPortal.Plugins.IRSS.MPControlPlugin.Forms
{
  internal partial class SetupForm : Form
  {
    #region Variables

    private LearnIR _learnIR;

    internal MacroPanel _macroPanel;
    private IRCommandPanel _irCommandPanel;

    #endregion Variables

    #region Constructor

    public SetupForm()
    {
      InitializeComponent();
      StartCatchingExceptions();

      // add macro panel
      _macroPanel = new MacroPanel(MPControlPlugin.CommandProcessor, MPControlPlugin.FolderMacros, MPControlPlugin.MacroCategories);
      _macroPanel.Dock = DockStyle.Fill;
      tabPageMacros.Controls.Add(_macroPanel);

      // add macro panel
      _irCommandPanel = new IRCommandPanel(NewIRCommand, EditIRCommand);
      _irCommandPanel.Dock = DockStyle.Fill;
      tabPageIR.Controls.Add(_irCommandPanel);
    }

    ~SetupForm()
    {
      StopCatchingExceptions();
    }

    #endregion Constructor

    #region Implementation

    #region Form

    private void SetupForm_Load(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(MPControlPlugin.Config.ServerHost))
      {
        ServerAddress serverAddress = new ServerAddress();
        serverAddress.ShowDialog(this);

        MPControlPlugin.Config.ServerHost = serverAddress.ServerHost;
      }

      IPAddress serverIP = Network.GetIPFromName(MPControlPlugin.Config.ServerHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, Server.DefaultPort);

      if (!MPControlPlugin.StartClient(endPoint))
        MessageBox.Show(this, "Failed to start local comms. IR functions temporarily disabled.",
                        "MP Control Plugin - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

      checkBoxRequiresFocus.Checked = MPControlPlugin.Config.RequireFocus;
      checkBoxMultiMapping.Checked = MPControlPlugin.Config.MultiMappingEnabled;
      checkBoxEventMapper.Checked = MPControlPlugin.Config.EventMapperEnabled;
      checkBoxMouseMode.Checked = MPControlPlugin.Config.MouseModeEnabled;

      // macros tab
      _macroPanel.RefreshList();

      // ircommands tab
      _irCommandPanel.RefreshList();

      // Mouse Mode ...
      foreach (string button in Enum.GetNames(typeof(RemoteButton)))
        if (!button.Equals("None", StringComparison.OrdinalIgnoreCase))
          comboBoxMouseModeButton.Items.Add(button);

      comboBoxMouseModeButton.SelectedItem = Enum.GetName(typeof(RemoteButton), MPControlPlugin.Config.MouseModeButton);

      numericUpDownMouseStep.Value = new decimal(MPControlPlugin.Config.MouseModeStep);

      checkBoxMouseAcceleration.Checked = MPControlPlugin.Config.MouseModeAcceleration;

      // Multi-Mapping ...
      foreach (string button in Enum.GetNames(typeof(RemoteButton)))
        if (!button.Equals("None", StringComparison.OrdinalIgnoreCase))
          comboBoxMultiButton.Items.Add(button);

      comboBoxMultiButton.SelectedItem = Enum.GetName(typeof(RemoteButton), MPControlPlugin.Config.MultiMappingButton);
      RefeshMultiMappingList();

      // Event Mapper ...
      InitParametersList();
      PopulateCommandList();
      RefreshEventList();

      // Remote Control Presets ...
      comboBoxRemotePresets.Items.Clear();
      string[] presets = Directory.GetFiles(MPControlPlugin.RemotePresetsFolder, "*.xml", SearchOption.TopDirectoryOnly);
      foreach (string preset in presets)
        comboBoxRemotePresets.Items.Add(Path.GetFileNameWithoutExtension(preset));
      comboBoxRemotePresets.SelectedIndex = 0;

      // Load Remotes ...
      treeViewRemotes.Nodes.Clear();
      LoadRemotes(MPControlPlugin.RemotesFile);

      // Register for remote button presses
      _addNode = AddNode;

      MPControlPlugin.HandleMessage += ReceivedMessage;
    }

    private void SetupForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      MPControlPlugin.HandleMessage -= ReceivedMessage;
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      MPControlPlugin.Config.RequireFocus = checkBoxRequiresFocus.Checked;
      MPControlPlugin.Config.MultiMappingEnabled = checkBoxMultiMapping.Checked;
      MPControlPlugin.Config.EventMapperEnabled = checkBoxEventMapper.Checked;
      MPControlPlugin.Config.MouseModeEnabled = checkBoxMouseMode.Checked;

      MPControlPlugin.Config.MouseModeButton =
        (RemoteButton)Enum.Parse(typeof(RemoteButton), comboBoxMouseModeButton.SelectedItem as string, true);
      MPControlPlugin.Config.MouseModeStep = Decimal.ToInt32(numericUpDownMouseStep.Value);
      MPControlPlugin.Config.MouseModeAcceleration = checkBoxMouseAcceleration.Checked;

      MPControlPlugin.Config.MultiMappingButton =
        (RemoteButton)Enum.Parse(typeof(RemoteButton), comboBoxMultiButton.SelectedItem as string, true);

      CommitMultiMappings();
      CommitEvents();
      SaveRemotes(MPControlPlugin.RemotesFile);

      DialogResult = DialogResult.OK;
      Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    private void buttonChangeServer_Click(object sender, EventArgs e)
    {
      MPControlPlugin.StopClient();

      ServerAddress serverAddress = new ServerAddress(MPControlPlugin.Config.ServerHost);
      serverAddress.ShowDialog(this);

      MPControlPlugin.Config.ServerHost = serverAddress.ServerHost;

      IPAddress serverIP = Network.GetIPFromName(MPControlPlugin.Config.ServerHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, Server.DefaultPort);

      MPControlPlugin.StartClient(endPoint);
    }

    private void buttonHelp_Click(object sender, EventArgs e)
    {
      IrssHelp.Open(this.GetType().FullName + "_" + tabControl.SelectedTab.Name);
    }

    private void SetupForm_HelpRequested(object sender, HelpEventArgs hlpevent)
    {
      buttonHelp_Click(null, null);
      hlpevent.Handled = true;
    }

    #endregion Form

    #region Local Methods

    private DelegateAddNode _addNode;

    private void ReceivedMessage(IrssMessage received)
    {
      if (received.Type == MessageType.RemoteEvent)
      {
        //string deviceName = received.MessageData[IrssMessage.DEVICE_NAME] as string;
        string keyCode = received.MessageData[IrssMessage.KEY_CODE] as string;

        // TODO: Activate this code for 1.4.3
        //if (deviceName.Equals("Abstract", StringComparison.OrdinalIgnoreCase))
        Invoke(_addNode, new Object[] {keyCode});
        //else
        //  this.Invoke(_addNode, new Object[] { String.Format("{0} ({1})", deviceName, keyCode) });
      }
      else if (_learnIR != null && received.Type == MessageType.LearnIR)
      {
        if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
        {
          _learnIR.LearnStatus("Learned IR successfully", true);
        }
        else if ((received.Flags & MessageFlags.Timeout) == MessageFlags.Timeout)
        {
          _learnIR.LearnStatus("Learn IR timed out", false);
        }
        else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
        {
          _learnIR.LearnStatus("Learn IR failed", false);
        }
      }
    }

    private void LoadRemotes(string file)
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(file);

        XmlNodeList listRemotes = doc.DocumentElement.SelectNodes("remote");
        foreach (XmlNode nodeRemote in listRemotes)
        {
          TreeNode remoteNode = new TreeNode(nodeRemote.Attributes["name"].Value);
          treeViewRemotes.Nodes.Add(remoteNode);

          XmlNodeList listButtons = nodeRemote.SelectNodes("button");
          foreach (XmlNode nodeButton in listButtons)
          {
            TreeNode buttonNode = new TreeNode(nodeButton.Attributes["name"].Value);
            buttonNode.ForeColor = Color.Navy;
            remoteNode.Nodes.Add(buttonNode);

            XmlNodeList listIRCodes = nodeButton.SelectNodes("code");
            foreach (XmlNode nodeCode in listIRCodes)
            {
              TreeNode codeNode = new TreeNode(nodeCode.Attributes["value"].Value);
              codeNode.ForeColor = Color.Blue;
              buttonNode.Nodes.Add(codeNode);
            }
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error(ex);
      }
    }

    private void SaveRemotes(string file)
    {
      using (XmlTextWriter writer = new XmlTextWriter(file, Encoding.UTF8))
      {
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 1;
        writer.IndentChar = (char) 9;
        writer.WriteStartDocument(true);
        writer.WriteStartElement("remotes"); // <remotes>

        foreach (TreeNode remoteNode in treeViewRemotes.Nodes)
        {
          writer.WriteStartElement("remote"); // <remote>
          writer.WriteAttributeString("name", remoteNode.Text);

          foreach (TreeNode buttonNode in remoteNode.Nodes)
          {
            writer.WriteStartElement("button"); // <button>
            writer.WriteAttributeString("name", buttonNode.Text);

            foreach (TreeNode codeNode in buttonNode.Nodes)
            {
              writer.WriteStartElement("code"); // <code>
              writer.WriteAttributeString("value", codeNode.Text);
              writer.WriteEndElement(); // </code>
            }

            writer.WriteEndElement(); // </button>
          }

          writer.WriteEndElement(); // </remote>
        }

        writer.WriteEndElement(); // </remotes>
        writer.WriteEndDocument();
      }
    }

    private string KeyNodeExists(string keyCode)
    {
      foreach (TreeNode remote in treeViewRemotes.Nodes)
        foreach (TreeNode button in remote.Nodes)
          foreach (TreeNode code in button.Nodes)
            if (code.Text.Equals(keyCode, StringComparison.Ordinal))
              return button.Text;

      return null;
    }

    private void AddNode(string keyCode)
    {
      string exists = KeyNodeExists(keyCode);
      if (!String.IsNullOrEmpty(exists))
      {
        labelStatus.Text = String.Format("KeyCode {0} exists in node {1}", keyCode, exists);
        labelStatus.ForeColor = Color.Purple;
        return;
      }

      TreeNode selected = treeViewRemotes.SelectedNode;

      if (selected != null && selected.Level > 0)
      {
        TreeNode newNode = new TreeNode(keyCode);
        newNode.ForeColor = Color.Blue;

        if (selected.Level == 2)
        {
          selected.Parent.Nodes.Add(newNode);
          selected.Parent.ExpandAll();

          labelStatus.Text = String.Format("Mapped {0} to {1}", keyCode, selected.Parent.Text);
          labelStatus.ForeColor = Color.Green;
        }
        else if (selected.Level == 1)
        {
          selected.Nodes.Add(newNode);
          selected.ExpandAll();

          labelStatus.Text = String.Format("Mapped {0} to {1}", keyCode, selected.Text);
          labelStatus.ForeColor = Color.Green;
        }

        newNode.EnsureVisible();
      }
      else
      {
        labelStatus.Text = String.Format("Received: {0}", keyCode);
        labelStatus.ForeColor = Color.Blue;
      }
    }

    private delegate void DelegateAddNode(string keyCode);

    #endregion Local Methods

    #region Remotes tab

    private void buttonLoadPreset_Click(object sender, EventArgs e)
    {
      string fileName = Path.Combine(MPControlPlugin.RemotePresetsFolder,
                                     (comboBoxRemotePresets.SelectedItem as string) + ".xml");
      LoadRemotes(fileName);
    }

    private void buttonMapButtons_Click(object sender, EventArgs e)
    {
      InputMappingForm inputMappingForm = new InputMappingForm("MPControlPlugin");
      inputMappingForm.ShowDialog(this);
    }

    private void buttonClearAll_Click(object sender, EventArgs e)
    {
      treeViewRemotes.Nodes.Clear();

      labelStatus.Text = "Cleared All";
      labelStatus.ForeColor = Color.Purple;
    }

    private void treeViewRemotes_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.KeyCode)
      {
        case Keys.Delete:
          {
            if (treeViewRemotes.SelectedNode != null)
            {
              switch (treeViewRemotes.SelectedNode.Level)
              {
                case 0:
                  if (
                    MessageBox.Show(this, "Are you sure you want to remove this entire Remote?", "Remove remote",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    treeViewRemotes.Nodes.Remove(treeViewRemotes.SelectedNode);
                  break;

                case 1:
                  if (
                    MessageBox.Show(this, "Are you sure you want to remove this Button?", "Remove button",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    treeViewRemotes.SelectedNode.Parent.Nodes.Remove(treeViewRemotes.SelectedNode);
                  break;

                default:
                  treeViewRemotes.SelectedNode.Parent.Nodes.Remove(treeViewRemotes.SelectedNode);
                  break;
              }
            }
            break;
          }

        default:
          {
            e.SuppressKeyPress = false;
            break;
          }
      }
    }

    #endregion Remotes tab

    #region IRCommands tab

    private void NewIRCommand()
    {
      _learnIR = new LearnIR(
        MPControlPlugin.LearnIR,
        MPControlPlugin.BlastIR,
        MPControlPlugin.TransceiverInformation.Ports);

      _learnIR.ShowDialog(this);

      _learnIR = null;
    }

    private void EditIRCommand(string fileName)
    {
      string command = Path.GetFileNameWithoutExtension(fileName);

      _learnIR = new LearnIR(
        MPControlPlugin.LearnIR,
        MPControlPlugin.BlastIR,
        MPControlPlugin.TransceiverInformation.Ports,
        command);

      _learnIR.ShowDialog(this);

      _learnIR = null;
    }

    #endregion IRCommands tab

    #region MultiMapping tab

    private void RefeshMultiMappingList()
    {
      listBoxMappings.Items.Clear();

      foreach (string map in MPControlPlugin.MultiMappings)
        listBoxMappings.Items.Add(map);
    }

    private void CommitMultiMappings()
    {
      MPControlPlugin.MultiMappings.Clear();

      foreach (string item in listBoxMappings.Items)
      {
        MPControlPlugin.MultiMappings.Add(item);
      }
    }

    private void buttonNew_Click(object sender, EventArgs e)
    {
      MultiMapNameBox multiMapNameBox = new MultiMapNameBox();
      if (multiMapNameBox.ShowDialog(this) == DialogResult.Cancel)
        return;

      string mappingName = multiMapNameBox.MapName;

      string pathCustom = Path.Combine(MPCommon.InputDeviceMappings, "MPControlPlugin.xml");
      string pathDefault = Path.Combine(MPCommon.DefaultInputDeviceMappings, "MPControlPlugin.xml");

      string sourceFile;
      if (File.Exists(pathCustom))
      {
        sourceFile = pathCustom;
      }
      else if (File.Exists(pathDefault))
      {
        sourceFile = pathDefault;
      }
      else
      {
        Log.Error("MPControlPlugin: Default remote map \"MPControlPlugin.xml\" is missing");
        return;
      }

      string destinationFile = Path.Combine(MPCommon.InputDeviceMappings, mappingName + ".xml");

      File.Copy(sourceFile, destinationFile, true);

      listBoxMappings.Items.Add(mappingName);
    }

    private void buttonRemove_Click(object sender, EventArgs e)
    {
      int selected = listBoxMappings.SelectedIndex;
      if (selected != -1)
        listBoxMappings.Items.RemoveAt(selected);
    }

    private void buttonUp_Click(object sender, EventArgs e)
    {
      int selected = listBoxMappings.SelectedIndex;
      if (selected != -1)
      {
        object item = listBoxMappings.Items[selected];
        listBoxMappings.Items.RemoveAt(selected);
        listBoxMappings.Items.Insert(selected - 1, item);
        listBoxMappings.SelectedIndex = selected - 1;
      }
    }

    private void buttonDown_Click(object sender, EventArgs e)
    {
      int selected = listBoxMappings.SelectedIndex;
      if (selected < listBoxMappings.Items.Count - 1)
      {
        object item = listBoxMappings.Items[selected];
        listBoxMappings.Items.RemoveAt(selected);
        listBoxMappings.Items.Insert(selected + 1, item);
        listBoxMappings.SelectedIndex = selected + 1;
      }
    }

    private void buttonEdit_Click(object sender, EventArgs e)
    {
      if (listBoxMappings.SelectedIndex != -1)
      {
        InputMappingForm inputMappingForm = new InputMappingForm(listBoxMappings.SelectedItem as string);
        inputMappingForm.ShowDialog(this);
      }
    }

    private void listBoxMappings_DoubleClick(object sender, EventArgs e)
    {
      if (listBoxMappings.SelectedIndex != -1)
      {
        InputMappingForm inputMappingForm = new InputMappingForm(listBoxMappings.SelectedItem as string);
        inputMappingForm.ShowDialog(this);
      }
    }

    #endregion MultiMapping tab

    #region EventMapping tab

    private void InitParametersList()
    {
      comboBoxParameter.Items.Clear();
      comboBoxParameter.Items.Add("Ignore Parameters");
      comboBoxParameter.Items.Add("Label 1");
      comboBoxParameter.Items.Add("Label 2");
      comboBoxParameter.Items.Add("Label 3");
      comboBoxParameter.Items.Add("Label 4");
      comboBoxParameter.Items.Add("Parameter 1");
      comboBoxParameter.Items.Add("Parameter 2");
      comboBoxParameter.Items.Add("Parameter 3");
      comboBoxParameter.Items.Add("Parameter 4");
      comboBoxParameter.Items.Add("Send To Target Window");
      comboBoxParameter.Items.Add("Sender Control ID");
      comboBoxParameter.Items.Add("Target Control ID");
      comboBoxParameter.Items.Add("Target Window ID");
      comboBoxParameter.SelectedIndex = 0;
    }

    private void RefreshEventList()
    {
      listViewEventMap.Items.Clear();

      foreach (MappedEvent mappedEvent in MPControlPlugin.EventMappings.Events)
      {
        string[] subItems = new string[4];
        subItems[0] = Enum.GetName(typeof(MappedEvent.MappingEvent), mappedEvent.EventType);
        subItems[1] = string.IsNullOrEmpty(mappedEvent.Param) ? string.Empty : mappedEvent.Param;
        subItems[2] = string.IsNullOrEmpty(mappedEvent.ParamValue) ? string.Empty : mappedEvent.ParamValue;
        subItems[3] = mappedEvent.GetCommandDisplayText();

        ListViewItem item = new ListViewItem(subItems);
        if (mappedEvent.IsCommandAvailable)
        {
          item.Tag = mappedEvent.Command;
        }
        else
        {
          item.Tag = mappedEvent;
          item.ToolTipText = "Command was not found in CommandLibrary.";
        }
        listViewEventMap.Items.Add(item);
      }

      // refresh combobox
      comboBoxEvents.Items.Clear();
      foreach (string eventName in Enum.GetNames(typeof(MappedEvent.MappingEvent)))
        if (!eventName.Equals("None", StringComparison.OrdinalIgnoreCase))
          comboBoxEvents.Items.Add(eventName);

      // TODO: Add Enter/Exit screen events.
      //comboBoxEvents.Items.Add("Enter screen");
      //comboBoxEvents.Items.Add("Exit screen");

      comboBoxEvents.SelectedIndex = 0;
    }
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

    private void CommitEvents()
    {
      MPControlPlugin.EventMappings.Events.Clear();

      foreach (ListViewItem item in listViewEventMap.Items)
      {
        try
        {
          if (ReferenceEquals(item.Tag, null)) continue;

          // command is available, tag is a command
          Command command = item.Tag as Command;
          if (!ReferenceEquals(command, null))
          {
            MappedEvent.MappingEvent eventType =
              (MappedEvent.MappingEvent) Enum.Parse(typeof (MappedEvent.MappingEvent), item.SubItems[0].Text, true);
            string param = item.SubItems[1].Text;
            string paramValue = item.SubItems[2].Text;

            if (string.IsNullOrEmpty(param))
              MPControlPlugin.EventMappings.Events.Add(new MappedEvent(eventType, command));
            else
              MPControlPlugin.EventMappings.Events.Add(new MappedEvent(eventType, param, paramValue, command));
            continue;
          }

          // command is not available, tag is the preserved mapped event
          MappedEvent map = item.Tag as MappedEvent;
          if (!ReferenceEquals(map, null))
            MPControlPlugin.EventMappings.Events.Add(map);
        }
        catch (Exception ex)
        {
          IrssLog.Error("Bad item in event list: {0}, {1}\n{2}", item.SubItems[0].Text, item.SubItems[1].Text,
                        ex.Message);
        }
      }
    }

    private void buttonClearEventParams_Click(object sender, EventArgs e)
    {
      comboBoxParameter.SelectedIndex = 0;
      textBoxParamValue.Text = string.Empty;
    }

    private void AddEvent(object sender, EventArgs e)
    {
      string[] subItems = new string[4];
      subItems[0] = comboBoxEvents.SelectedItem as string;
      subItems[1] = comboBoxParameter.SelectedItem as string;
      subItems[2] = textBoxParamValue.Text;
      if (subItems[1].Equals("Ignore Parameters", StringComparison.OrdinalIgnoreCase))
      {
        subItems[1] = string.Empty;
        subItems[2] = string.Empty;
      }
      subItems[3] = string.Empty;

      ListViewItem item = new ListViewItem(subItems);
      listViewEventMap.SelectedIndices.Clear();
      listViewEventMap.Items.Add(item);
      item.Selected = true;
    }

    private void SetCommandToEvent(object sender, EventArgs e)
    {
      if (treeViewCommandList.SelectedNode == null || treeViewCommandList.SelectedNode.Level == 0) return;

      Type commandType = treeViewCommandList.SelectedNode.Tag as Type;
      if (ReferenceEquals(commandType, null)) return;

      Command command = (Command)Activator.CreateInstance(commandType);
      if (!MPControlPlugin.CommandProcessor.Edit(command, this)) return;

      foreach (ListViewItem item in listViewEventMap.SelectedItems)
      {
        item.Tag = command;
        item.SubItems[3].Text = command.UserDisplayText;
      }
    }

    private void listViewEventMap_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Delete)
        foreach (ListViewItem listViewItem in listViewEventMap.SelectedItems)
          listViewEventMap.Items.Remove(listViewItem);
    }

    private void listViewEventMap_DoubleClick(object sender, EventArgs e)
    {
      if (listViewEventMap.SelectedItems.Count != 1) return;

      ListViewItem item = listViewEventMap.SelectedItems[0];
      if (ReferenceEquals(item.Tag, null)) return;

      if (item.Tag is MappedEvent)
      {
        MessageBox.Show(this,
                        "The command is not available and can not be edited. Please check your commands directory in application folder or set a new command below.",
                        "Command unavailable", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
      }

      Command cmd = item.Tag as Command;
      if (ReferenceEquals(cmd, null)) return;

      if (!MPControlPlugin.CommandProcessor.Edit(cmd, this)) return;

      item.Tag = cmd;
      item.SubItems[3].Text = cmd.UserDisplayText;
    }

    #endregion EventMapping tab

    #region Exception Handling

    private void StartCatchingExceptions()
    {
      Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
      AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
    }

    private void StopCatchingExceptions()
    {
      AppDomain.CurrentDomain.UnhandledException -= new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
      Application.ThreadException -= new ThreadExceptionEventHandler(Application_ThreadException);
    }

    void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
    {
      MPControlPluginShowException ex = new MPControlPluginShowException(e.Exception);
      ex.ShowDialog(this);
    }

    void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
      MPControlPluginShowException ex = new MPControlPluginShowException(e.ExceptionObject as Exception);
      ex.ShowDialog(this);
    }

    #endregion Exception Handling

    #endregion Implementation
  }
}