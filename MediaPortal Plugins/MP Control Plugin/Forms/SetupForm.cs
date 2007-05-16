using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using MediaPortal.GUI.Library;
using MediaPortal.Hardware;
using MediaPortal.Util;

using IrssUtils;
using IrssUtils.Forms;

namespace MediaPortal.Plugins
{

  public partial class SetupForm : Form
  {
     
    #region Constructor

    public SetupForm()
    {
      InitializeComponent();
    }

    #endregion Constructor

    private void SetupForm_Load(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(MPControlPlugin.ServerHost))
      {
        IrssUtils.Forms.ServerAddress serverAddress = new IrssUtils.Forms.ServerAddress(Environment.MachineName);
        serverAddress.ShowDialog();
        MPControlPlugin.ServerHost = serverAddress.ServerHost;
      }

      if (!MPControlPlugin.StartComms())
        MessageBox.Show(this, "Failed to start local comms. IR functions temporarily disabled.", "MP Control Plugin - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

      checkBoxLogVerbose.Checked      = MPControlPlugin.LogVerbose;
      checkBoxRequiresFocus.Checked   = MPControlPlugin.RequireFocus;
      checkBoxMultiMapping.Checked    = MPControlPlugin.MultiMappingEnabled;
      checkBoxEventMapper.Checked     = MPControlPlugin.EventMapperEnabled;
      checkBoxMouseMode.Checked       = MPControlPlugin.MouseModeEnabled;

      RefreshIRList();
      RefreshMacroList();

      // Mouse Mode ...
      foreach (string button in Enum.GetNames(typeof(RemoteButton)))
        if (button != "None")
          comboBoxMouseModeButton.Items.Add(button);

      comboBoxMouseModeButton.SelectedItem = Enum.GetName(typeof(RemoteButton), MPControlPlugin.MouseModeButton);

      numericUpDownMouseStep.Value = new decimal(MPControlPlugin.MouseModeStep);

      checkBoxMouseAcceleration.Checked = MPControlPlugin.MouseModeAcceleration;

      // Multi-Mapping ...
      foreach (string button in Enum.GetNames(typeof(RemoteButton)))
        if (button != "None")
          comboBoxMultiButton.Items.Add(button);

      comboBoxMultiButton.SelectedItem = Enum.GetName(typeof(RemoteButton), MPControlPlugin.MultiMappingButton);

      foreach (string map in MPControlPlugin.MultiMaps)
        listBoxMappings.Items.Add(map);
      
      // Event Mapper ...
      comboBoxCommands.Items.Clear();
      comboBoxCommands.Items.Add(Common.UITextRun);
      comboBoxCommands.Items.Add(Common.UITextSerial);
      comboBoxCommands.Items.Add(Common.UITextMessage);
      comboBoxCommands.Items.Add(Common.UITextKeys);
      comboBoxCommands.Items.Add(Common.UITextGoto);
      comboBoxCommands.Items.AddRange(MPControlPlugin.GetFileList(true));

      comboBoxEvents.Items.Clear();
      foreach (string eventType in Enum.GetNames(typeof(MappedEvent.MappingEvent)))
        if (eventType != "None")
          comboBoxEvents.Items.Add(eventType);

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
      
      LoadEvents();

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
      _addNode = new DelegateAddNode(AddNode);
      MPControlPlugin.RemoteButtonListener += new RemoteButtonHandler(RemoteButtonPressed);
    }

    private void SetupForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      MPControlPlugin.RemoteButtonListener -= new RemoteButtonHandler(RemoteButtonPressed);
    }
    
    #region Local Methods

    void LoadRemotes(string file)
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
        Log.Error("MPControlPlugin: LoadRemotes() - {0}", ex.Message);
      }
    }
    void SaveRemotes(string file)
    {
      XmlTextWriter writer = new XmlTextWriter(file, System.Text.Encoding.UTF8);
      writer.Formatting = Formatting.Indented;
      writer.Indentation = 1;
      writer.IndentChar = (char)9;
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
      writer.Close();
    }

    delegate void DelegateAddNode(string keyCode);
    DelegateAddNode _addNode = null;

    void AddNode(string keyCode)
    {
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

    void RemoteButtonPressed(string keyCode)
    {
      this.Invoke(_addNode, new Object[] { keyCode });
    }

    void RefreshIRList()
    {
      listBoxIR.Items.Clear();
      listBoxIR.Items.AddRange(Common.GetIRList(false));
    }
    void RefreshMacroList()
    {
      listBoxMacro.Items.Clear();
      listBoxMacro.Items.AddRange(MPControlPlugin.GetMacroList(false));
    }

    void EditIR()
    {
      if (listBoxIR.SelectedIndex != -1)
      {
        string command = listBoxIR.SelectedItem as string;
        string fileName = Common.FolderIRCommands + command + Common.FileExtensionIR;

        if (File.Exists(fileName))
        {
          LearnIR learnIR = new LearnIR(false, command);
          learnIR.ShowDialog(this);
        }
        else
        {
          MessageBox.Show(this, "File not found: " + fileName, "File missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          RefreshIRList();
        }
      }
    }
    void EditMacro()
    {
      if (listBoxMacro.SelectedIndex != -1)
      {
        string command = listBoxMacro.SelectedItem as string;
        string fileName = MPControlPlugin.FolderMacros + command + Common.FileExtensionMacro;

        if (File.Exists(fileName))
        {
          MacroEditor macroEditor = new MacroEditor(false, command);
          macroEditor.ShowDialog(this);
        }
        else
        {
          MessageBox.Show(this, "File not found: " + fileName, "File missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          RefreshMacroList();
        }
      }
    }

    void LoadEvents()
    {
      if (!File.Exists(MPControlPlugin.EventMappingFile))
        return;

      XmlDocument doc = new XmlDocument();
      doc.Load(MPControlPlugin.EventMappingFile);

      XmlNodeList eventList = doc.DocumentElement.SelectNodes("mapping");

      string[] items = new string[2];
      foreach (XmlNode item in eventList)
      {
        items[0] = item.Attributes["event"].Value;
        items[1] = item.Attributes["command"].Value;
        listViewEventMap.Items.Add(new ListViewItem(items));
      }
    }
    void SaveEvents()
    {
      XmlTextWriter writer = new XmlTextWriter(MPControlPlugin.EventMappingFile, System.Text.Encoding.UTF8);
      writer.Formatting = Formatting.Indented;
      writer.Indentation = 1;
      writer.IndentChar = (char)9;
      writer.WriteStartDocument(true);
      writer.WriteStartElement("events"); // <events>

      foreach (ListViewItem item in listViewEventMap.Items)
      {
        writer.WriteStartElement("mapping"); // <mapping>

        writer.WriteAttributeString("event", item.SubItems[0].Text);
        writer.WriteAttributeString("command", item.SubItems[1].Text);

        writer.WriteEndElement(); // </mapping>
      }

      writer.WriteEndElement(); // </events>
      writer.WriteEndDocument();
      writer.Close();
    }

    void SaveMultiMappings()
    {
      XmlTextWriter writer = new XmlTextWriter(MPControlPlugin.MultiMappingFile, System.Text.Encoding.UTF8);
      writer.Formatting = Formatting.Indented;
      writer.Indentation = 1;
      writer.IndentChar = (char)9;
      writer.WriteStartDocument(true);
      writer.WriteStartElement("mappings"); // <mappings>

      foreach (string item in listBoxMappings.Items)
      {
        writer.WriteStartElement("map");
        writer.WriteAttributeString("name", item);
        writer.WriteEndElement();
      }

      writer.WriteEndElement(); // </mappings>
      writer.WriteEndDocument();
      writer.Close();
    }

    #endregion Local Methods

    #region Buttons

    private void buttonHelp_Click(object sender, EventArgs e)
    {
      try
      {
        Help.ShowHelp(this, SystemRegistry.GetInstallFolder() + "\\IR Server Suite.chm", HelpNavigator.Topic, "MP Control Plugin");
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Failed to load help", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void buttonNewIR_Click(object sender, EventArgs e)
    {
      LearnIR learnIR = new LearnIR(true, String.Empty);
      learnIR.ShowDialog(this);

      RefreshIRList();
    }
    private void buttonEditIR_Click(object sender, EventArgs e)
    {
      EditIR();
    }
    private void buttonDeleteIR_Click(object sender, EventArgs e)
    {
      if (listBoxIR.SelectedIndex != -1)
      {
        string file = listBoxIR.SelectedItem as string;
        string fileName = Common.FolderIRCommands + file + Common.FileExtensionIR;
        if (File.Exists(fileName))
        {
          if (MessageBox.Show(this, "Are you sure you want to delete \"" + file + "\"?", "Confirm delete",  MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            File.Delete(fileName);
        }
        else
        {
          MessageBox.Show(this, "File not found: " + fileName, "File missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        RefreshIRList();
      }
    }

    private void buttonNewMacro_Click(object sender, EventArgs e)
    {
      MacroEditor macroEditor = new MacroEditor(true, String.Empty);
      macroEditor.ShowDialog(this);

      RefreshMacroList();
    }
    private void buttonEditMacro_Click(object sender, EventArgs e)
    {
      EditMacro();
    }
    private void buttonDeleteMacro_Click(object sender, EventArgs e)
    {
      if (listBoxMacro.SelectedIndex != -1)
      {
        string file = listBoxMacro.SelectedItem as string;
        string fileName = MPControlPlugin.FolderMacros + file + Common.FileExtensionMacro;
        if (File.Exists(fileName))
        {
          if (MessageBox.Show(this, "Are you sure you want to delete \"" + file + "\"?", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            File.Delete(fileName);
        }
        else
        {
          MessageBox.Show(this, "File not found: " + fileName, "File missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        RefreshMacroList();
      }
    }
    private void buttonTestMacro_Click(object sender, EventArgs e)
    {
      if (listBoxMacro.SelectedIndex == -1)
        return;

      string fileName = MPControlPlugin.FolderMacros + listBoxMacro.SelectedItem as string + Common.FileExtensionMacro;

      try
      {
        MPControlPlugin.ProcessMacro(fileName);
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Test failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void buttonClearEventParams_Click(object sender, EventArgs e)
    {
      comboBoxParameter.SelectedIndex = 0;
      textBoxParamValue.Text = String.Empty;
    }
    private void buttonAddEvent_Click(object sender, EventArgs e)
    {
      string[] items = new string[2];

      if ((comboBoxParameter.SelectedItem as string) != "Ignore Parameters")
      {
        items[0] = comboBoxEvents.SelectedItem as string;
        items[0] += ",";
        items[0] += comboBoxParameter.SelectedItem as string;
        items[0] += "=";
        items[0] += textBoxParamValue.Text;
      }
      else
      {
        items[0] = comboBoxEvents.SelectedItem as string;
      }

      items[1] = String.Empty;

      listViewEventMap.Items.Add(new ListViewItem(items));
    }
    private void buttonSetCommand_Click(object sender, EventArgs e)
    {
      if (comboBoxCommands.SelectedIndex == -1)
        return;

      string selected = comboBoxCommands.SelectedItem as string;
      string command = String.Empty;

      if (selected == Common.UITextRun)
      {
        ExternalProgram externalProgram = new ExternalProgram();
        if (externalProgram.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixRun + externalProgram.CommandString;
      }
      else if (selected == Common.UITextGoto)
      {
        MPUtils.Forms.GoToScreen goToScreen = new MPUtils.Forms.GoToScreen();
        if (goToScreen.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixGoto + goToScreen.Screen;
      }
      else if (selected == Common.UITextSerial)
      {
        SerialCommand serialCommand = new SerialCommand();
        if (serialCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixSerial + serialCommand.CommandString;
      }
      else if (selected == Common.UITextMessage)
      {
        MessageCommand messageCommand = new MessageCommand();
        if (messageCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixMessage + messageCommand.CommandString;
      }
      else if (selected == Common.UITextKeys)
      {
        KeysCommand keysCommand = new KeysCommand();
        if (keysCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixKeys + keysCommand.CommandString;
      }
      else if (selected.StartsWith(Common.CmdPrefixBlast))
      {
        BlastCommand blastCommand = new BlastCommand(selected.Substring(Common.CmdPrefixBlast.Length));
        if (blastCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixBlast + blastCommand.CommandString;
      }
      else if (selected.StartsWith(Common.CmdPrefixMacro))
      {
        command = selected;
      }

      foreach (ListViewItem listViewItem in listViewEventMap.SelectedItems)
        listViewItem.SubItems[1].Text = command;
    }

    private void buttonNew_Click(object sender, EventArgs e)
    {
      MultiMapNameBox multiMapNameBox = new MultiMapNameBox();
      if (multiMapNameBox.ShowDialog(this) == DialogResult.Cancel)
        return;

      string mappingName = multiMapNameBox.MapName;

      string pathCustom = MPControlPlugin.CustomInputDevice + "MPControlPlugin.xml";
      string pathDefault = MPControlPlugin.CustomInputDefault + "MPControlPlugin.xml";

      string sourceFile;
      if (File.Exists(pathCustom))
        sourceFile = pathCustom;
      else if (File.Exists(pathDefault))
        sourceFile = pathDefault;
      else
      {
        Log.Error("MPControlPlugin: Default remote map \"MPControlPlugin.xml\" is missing");
        return;
      }

      string destinationFile = MPControlPlugin.CustomInputDevice + mappingName + ".xml";

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

    private void buttonOK_Click(object sender, EventArgs e)
    {
      MPControlPlugin.LogVerbose = checkBoxLogVerbose.Checked;
      MPControlPlugin.RequireFocus = checkBoxRequiresFocus.Checked;
      MPControlPlugin.MultiMappingEnabled = checkBoxMultiMapping.Checked;
      MPControlPlugin.EventMapperEnabled = checkBoxEventMapper.Checked;
      MPControlPlugin.MouseModeEnabled = checkBoxMouseMode.Checked;

      MPControlPlugin.MouseModeButton = (RemoteButton)Enum.Parse(typeof(RemoteButton), comboBoxMouseModeButton.SelectedItem as string);
      MPControlPlugin.MouseModeStep = Decimal.ToInt32(numericUpDownMouseStep.Value);
      MPControlPlugin.MouseModeAcceleration = checkBoxMouseAcceleration.Checked;

      MPControlPlugin.MultiMappingButton = (RemoteButton)Enum.Parse(typeof(RemoteButton), comboBoxMultiButton.SelectedItem as string);

      SaveMultiMappings();

      SaveRemotes(MPControlPlugin.RemotesFile);

      SaveEvents();

      this.DialogResult = DialogResult.OK;
      this.Close();
    }
    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void buttonChangeServer_Click(object sender, EventArgs e)
    {
      MPControlPlugin.StopComms();
      
      IrssUtils.Forms.ServerAddress serverAddress = new IrssUtils.Forms.ServerAddress(MPControlPlugin.ServerHost);
      serverAddress.ShowDialog();
      MPControlPlugin.ServerHost = serverAddress.ServerHost;

      MPControlPlugin.StartComms();
    }

    private void buttonLoadPreset_Click(object sender, EventArgs e)
    {
      LoadRemotes(MPControlPlugin.RemotePresetsFolder + comboBoxRemotePresets.SelectedItem as string + ".xml");
    }

    #endregion Buttons

    #region Other Controls

    private void listBoxIR_DoubleClick(object sender, EventArgs e)
    {
      EditIR();
    }
    private void listBoxMacro_DoubleClick(object sender, EventArgs e)
    {
      EditMacro();
    }

    private void listViewEventMap_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Delete)
        foreach (ListViewItem listViewItem in listViewEventMap.SelectedItems)
          listViewEventMap.Items.Remove(listViewItem);
    }
    private void listViewEventMap_DoubleClick(object sender, EventArgs e)
    {
      if (listViewEventMap.SelectedItems.Count != 1)
        return;

      string command = listViewEventMap.SelectedItems[0].SubItems[1].Text;

      if (command.StartsWith(Common.CmdPrefixRun))
      {
        string[] commands = Common.SplitRunCommand(command.Substring(Common.CmdPrefixRun.Length));
        ExternalProgram externalProgram = new ExternalProgram(commands);
        if (externalProgram.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixRun + externalProgram.CommandString;
      }
      else if (command.StartsWith(Common.CmdPrefixGoto))
      {
        MPUtils.Forms.GoToScreen goToScreen = new MPUtils.Forms.GoToScreen(command.Substring(Common.CmdPrefixGoto.Length));
        if (goToScreen.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixGoto + goToScreen.Screen;
      }
      else if (command.StartsWith(Common.CmdPrefixSerial))
      {
        string[] commands = Common.SplitSerialCommand(command.Substring(Common.CmdPrefixSerial.Length));
        SerialCommand serialCommand = new SerialCommand(commands);
        if (serialCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixSerial + serialCommand.CommandString;
      }
      else if (command.StartsWith(Common.CmdPrefixMessage))
      {
        string[] commands = Common.SplitMessageCommand(command.Substring(Common.CmdPrefixMessage.Length));
        MessageCommand messageCommand = new MessageCommand(commands);
        if (messageCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixMessage + messageCommand.CommandString;
      }
      else if (command.StartsWith(Common.CmdPrefixKeys))
      {
        KeysCommand keysCommand = new KeysCommand(command.Substring(Common.CmdPrefixKeys.Length));
        if (keysCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixKeys + keysCommand.CommandString;
      }
      else if (command.StartsWith(Common.CmdPrefixBlast))
      {
        BlastCommand blastCommand = new BlastCommand(command.Substring(Common.CmdPrefixBlast.Length));
        if (blastCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixBlast + blastCommand.CommandString;
      }
      else
      {
        return;
      }

      listViewEventMap.SelectedItems[0].SubItems[1].Text = command;
    }

    private void listBoxMappings_DoubleClick(object sender, EventArgs e)
    {
      if (listBoxMappings.SelectedIndex != -1)
      {
        InputMappingForm inputMappingForm = new InputMappingForm(listBoxMappings.SelectedItem as string);
        inputMappingForm.ShowDialog(this);
      }
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
                  if (MessageBox.Show(this, "Are you sure you want to remove this entire Remote?", "Remove remote", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    treeViewRemotes.Nodes.Remove(treeViewRemotes.SelectedNode);
                  break;

                case 1:
                  if (MessageBox.Show(this, "Are you sure you want to remove this Button?", "Remove button", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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

    #endregion Other Controls

  }

}
