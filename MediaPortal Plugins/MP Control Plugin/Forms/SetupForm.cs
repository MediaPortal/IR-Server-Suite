using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using MediaPortal.GUI.Library;
using MediaPortal.Hardware;
using MediaPortal.Util;

using IrssComms;
using IrssUtils;
using IrssUtils.Forms;
using MPUtils.Forms;

namespace MediaPortal.Plugins
{

  partial class SetupForm : Form
  {

    #region Variables

    LearnIR _learnIR;

    #endregion Variables

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
        ServerAddress serverAddress = new ServerAddress();
        serverAddress.ShowDialog(this);

        MPControlPlugin.ServerHost = serverAddress.ServerHost;
      }

      IPAddress serverIP = Client.GetIPFromName(MPControlPlugin.ServerHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, IrssComms.Server.DefaultPort);

      if (!MPControlPlugin.StartClient(endPoint))
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
        if (!button.Equals("None", StringComparison.OrdinalIgnoreCase))
          comboBoxMouseModeButton.Items.Add(button);

      comboBoxMouseModeButton.SelectedItem = Enum.GetName(typeof(RemoteButton), MPControlPlugin.MouseModeButton);

      numericUpDownMouseStep.Value = new decimal(MPControlPlugin.MouseModeStep);

      checkBoxMouseAcceleration.Checked = MPControlPlugin.MouseModeAcceleration;

      // Multi-Mapping ...
      foreach (string button in Enum.GetNames(typeof(RemoteButton)))
        if (!button.Equals("None", StringComparison.OrdinalIgnoreCase))
          comboBoxMultiButton.Items.Add(button);

      comboBoxMultiButton.SelectedItem = Enum.GetName(typeof(RemoteButton), MPControlPlugin.MultiMappingButton);

      foreach (string map in MPControlPlugin.MultiMaps)
        listBoxMappings.Items.Add(map);
      
      // Event Mapper ...
      RefreshEventMapperCommands();

      comboBoxEvents.Items.Clear();
      foreach (string eventType in Enum.GetNames(typeof(MappedEvent.MappingEvent)))
        if (!eventType.Equals("None", StringComparison.OrdinalIgnoreCase))
          comboBoxEvents.Items.Add(eventType);

      // TODO: Add Enter/Exit screen events.
      //comboBoxEvents.Items.Add("Enter screen");
      //comboBoxEvents.Items.Add("Exit screen");

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

      MPControlPlugin.HandleMessage += new ClientMessageSink(ReceivedMessage);
    }

    private void SetupForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      MPControlPlugin.HandleMessage -= new ClientMessageSink(ReceivedMessage);
    }

    #region Local Methods

    void ReceivedMessage(IrssMessage received)
    {
      if (received.Type == MessageType.RemoteEvent)
      {
        byte[] data = received.GetDataAsBytes();
        int deviceNameSize = BitConverter.ToInt32(data, 0);
        string deviceName = Encoding.ASCII.GetString(data, 4, deviceNameSize);
        int keyCodeSize = BitConverter.ToInt32(data, 4 + deviceNameSize);
        string keyCode = Encoding.ASCII.GetString(data, 8 + deviceNameSize, keyCodeSize);

        // TODO: Activate this code for 1.4.3
        //if (deviceName.Equals("Abstract", StringComparison.OrdinalIgnoreCase))
          this.Invoke(_addNode, new Object[] { keyCode });
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
        Log.Error(ex);
      }
    }
    void SaveRemotes(string file)
    {
      using (XmlTextWriter writer = new XmlTextWriter(file, Encoding.UTF8))
      {
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
      }
    }

    string KeyNodeExists(string keyCode)
    {
      foreach (TreeNode remote in treeViewRemotes.Nodes)
        foreach (TreeNode button in remote.Nodes)
          foreach (TreeNode code in button.Nodes)
            if (code.Text.Equals(keyCode, StringComparison.Ordinal))
              return button.Text;

      return null;
    }

    delegate void DelegateAddNode(string keyCode);
    DelegateAddNode _addNode;

    void AddNode(string keyCode)
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

    void RefreshIRList()
    {
      listViewIR.Items.Clear();

      string[] irList = Common.GetIRList(false);
      if (irList != null && irList.Length > 0)
        foreach (string irFile in irList)
          listViewIR.Items.Add(irFile);
    }
    void RefreshMacroList()
    {
      listViewMacro.Items.Clear();

      string[] macroList = MPControlPlugin.GetMacroList(false);
      if (macroList != null && macroList.Length > 0)
        foreach (string macroFile in macroList)
          listViewMacro.Items.Add(macroFile);
    }

    void RefreshEventMapperCommands()
    {
      comboBoxCommands.Items.Clear();

      comboBoxCommands.Items.Add(Common.UITextRun);
      comboBoxCommands.Items.Add(Common.UITextSerial);
      comboBoxCommands.Items.Add(Common.UITextWindowMsg);
      comboBoxCommands.Items.Add(Common.UITextTcpMsg);
      comboBoxCommands.Items.Add(Common.UITextKeys);
      comboBoxCommands.Items.Add(Common.UITextEject);
      comboBoxCommands.Items.Add(Common.UITextGotoScreen);
      //comboBoxCommands.Items.Add(Common.UITextWindowState);
      comboBoxCommands.Items.Add(Common.UITextExit);
      comboBoxCommands.Items.Add(Common.UITextStandby);
      comboBoxCommands.Items.Add(Common.UITextHibernate);
      comboBoxCommands.Items.Add(Common.UITextReboot);
      comboBoxCommands.Items.Add(Common.UITextShutdown);

      string[] fileList = MPControlPlugin.GetFileList(true);

      if (fileList != null && fileList.Length > 0)
        comboBoxCommands.Items.AddRange(fileList);
    }

    void EditIR()
    {
      if (listViewIR.SelectedItems.Count != 1)
        return;

      try
      {
        string command = listViewIR.SelectedItems[0].Text;
        string fileName = Path.Combine(Common.FolderIRCommands, command + Common.FileExtensionIR);

        if (File.Exists(fileName))
        {
          _learnIR = new LearnIR(
            new LearnIrDelegate(MPControlPlugin.LearnIR),
            new BlastIrDelegate(MPControlPlugin.BlastIR),
            MPControlPlugin.TransceiverInformation.Ports,
            command);

          _learnIR.ShowDialog(this);

          _learnIR = null;
        }
        else
        {
          RefreshIRList();
          RefreshEventMapperCommands();

          throw new FileNotFoundException("IR file missing", fileName);
        }
      }
      catch (Exception ex)
      {
        Log.Error(ex);
        MessageBox.Show(this, ex.Message, "Failed to edit IR file", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
    void EditMacro()
    {
      if (listViewMacro.SelectedItems.Count != 1)
        return;

      try
      {
        string command = listViewMacro.SelectedItems[0].Text;
        string fileName = Path.Combine(MPControlPlugin.FolderMacros, command + Common.FileExtensionMacro);

        if (File.Exists(fileName))
        {
          MacroEditor macroEditor = new MacroEditor(command);
          macroEditor.ShowDialog(this);
        }
        else
        {
          RefreshMacroList();
          RefreshEventMapperCommands();

          throw new FileNotFoundException("Macro file missing", fileName);
        }
      }
      catch (Exception ex)
      {
        Log.Error(ex);
        MessageBox.Show(this, ex.Message, "Failed to edit macro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
      using (XmlTextWriter writer = new XmlTextWriter(MPControlPlugin.EventMappingFile, Encoding.UTF8))
      {
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
      }
    }

    void SaveMultiMappings()
    {
      using (XmlTextWriter writer = new XmlTextWriter(MPControlPlugin.MultiMappingFile, Encoding.UTF8))
      {
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
      }
    }

    #endregion Local Methods

    #region Buttons

    private void buttonHelp_Click(object sender, EventArgs e)
    {
      try
      {
        string file = Path.Combine(SystemRegistry.GetInstallFolder(), "IR Server Suite.chm");
        Help.ShowHelp(this, file, HelpNavigator.Topic, "Plugins\\MP Control Plugin\\index.html");
      }
      catch (Exception ex)
      {
        Log.Error(ex);
        MessageBox.Show(this, ex.Message, "Failed to load help", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void buttonNewIR_Click(object sender, EventArgs e)
    {
      _learnIR = new LearnIR(
        new LearnIrDelegate(MPControlPlugin.LearnIR),
        new BlastIrDelegate(MPControlPlugin.BlastIR),
        MPControlPlugin.TransceiverInformation.Ports);

      _learnIR.ShowDialog(this);

      _learnIR = null;

      RefreshIRList();
      RefreshEventMapperCommands();
    }
    private void buttonEditIR_Click(object sender, EventArgs e)
    {
      EditIR();
    }
    private void buttonDeleteIR_Click(object sender, EventArgs e)
    {
      if (listViewIR.SelectedItems.Count != 1)
        return;

      string file = listViewIR.SelectedItems[0].Text;
      string fileName = Path.Combine(Common.FolderIRCommands, file + Common.FileExtensionIR);
      if (File.Exists(fileName))
      {
        if (MessageBox.Show(this, String.Format("Are you sure you want to delete \"{0}\"?", file), "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          File.Delete(fileName);
      }
      else
      {
        MessageBox.Show(this, "File not found: " + fileName, "IR file missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }

      RefreshIRList();
      RefreshEventMapperCommands();
    }

    private void buttonNewMacro_Click(object sender, EventArgs e)
    {
      MacroEditor macroEditor = new MacroEditor();
      macroEditor.ShowDialog(this);

      RefreshMacroList();
      RefreshEventMapperCommands();
    }
    private void buttonEditMacro_Click(object sender, EventArgs e)
    {
      EditMacro();
    }
    private void buttonDeleteMacro_Click(object sender, EventArgs e)
    {
      if (listViewMacro.SelectedItems.Count != 1)
        return;

      string file = listViewMacro.SelectedItems[0].Text;
      string fileName = Path.Combine(MPControlPlugin.FolderMacros, file + Common.FileExtensionMacro);
      if (File.Exists(fileName))
      {
        if (MessageBox.Show(this, String.Format("Are you sure you want to delete \"{0}\"?", file), "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          File.Delete(fileName);
      }
      else
      {
        MessageBox.Show(this, "File not found: " + fileName, "Macro file missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }

      RefreshMacroList();
      RefreshEventMapperCommands();
    }
    private void buttonTestMacro_Click(object sender, EventArgs e)
    {
      if (listViewMacro.SelectedItems.Count != 1)
        return;

      try
      {
        MPControlPlugin.ProcessCommand(Common.CmdPrefixMacro + listViewMacro.SelectedItems[0].Text, false);
      }
      catch (Exception ex)
      {
        Log.Error(ex);
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

      if ((comboBoxParameter.SelectedItem as string).Equals("Ignore Parameters", StringComparison.OrdinalIgnoreCase))
      {
        items[0] = comboBoxEvents.SelectedItem as string;
      }
      else
      {
        items[0] = comboBoxEvents.SelectedItem as string;
        items[0] += ",";
        items[0] += comboBoxParameter.SelectedItem as string;
        items[0] += "=";
        items[0] += textBoxParamValue.Text;
      }

      items[1] = String.Empty;

      listViewEventMap.Items.Add(new ListViewItem(items));
    }
    private void buttonSetCommand_Click(object sender, EventArgs e)
    {
      if (comboBoxCommands.SelectedIndex == -1)
        return;

      string selected = comboBoxCommands.SelectedItem as string;
      string newCommand = null;

      if (selected.Equals(Common.UITextRun, StringComparison.OrdinalIgnoreCase))
      {
        ExternalProgram externalProgram = new ExternalProgram();
        if (externalProgram.ShowDialog(this) == DialogResult.OK)
          newCommand = Common.CmdPrefixRun + externalProgram.CommandString;
      }
      else if (selected.Equals(Common.UITextSerial, StringComparison.OrdinalIgnoreCase))
      {
        SerialCommand serialCommand = new SerialCommand();
        if (serialCommand.ShowDialog(this) == DialogResult.OK)
          newCommand = Common.CmdPrefixSerial + serialCommand.CommandString;
      }
      else if (selected.Equals(Common.UITextWindowMsg, StringComparison.OrdinalIgnoreCase))
      {
        MessageCommand messageCommand = new MessageCommand();
        if (messageCommand.ShowDialog(this) == DialogResult.OK)
          newCommand = Common.CmdPrefixWindowMsg + messageCommand.CommandString;
      }
      else if (selected.Equals(Common.UITextTcpMsg, StringComparison.OrdinalIgnoreCase))
      {
        TcpMessageCommand tcpMessageCommand = new TcpMessageCommand();
        if (tcpMessageCommand.ShowDialog(this) == DialogResult.OK)
          newCommand = Common.CmdPrefixTcpMsg + tcpMessageCommand.CommandString;
      }
      else if (selected.Equals(Common.UITextKeys, StringComparison.OrdinalIgnoreCase))
      {
        KeysCommand keysCommand = new KeysCommand();
        if (keysCommand.ShowDialog(this) == DialogResult.OK)
          newCommand = Common.CmdPrefixKeys + keysCommand.CommandString;
      }
      else if (selected.Equals(Common.UITextEject, StringComparison.OrdinalIgnoreCase))
      {
        EjectCommand ejectCommand = new EjectCommand();
        if (ejectCommand.ShowDialog(this) == DialogResult.OK)
          newCommand = Common.CmdPrefixEject + ejectCommand.CommandString;
      }
      else if (selected.Equals(Common.UITextGotoScreen, StringComparison.OrdinalIgnoreCase))
      {
        GoToScreen goToScreen = new GoToScreen();
        if (goToScreen.ShowDialog(this) == DialogResult.OK)
          newCommand = Common.CmdPrefixGotoScreen + goToScreen.CommandString;
      }
      else if (selected.StartsWith(Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
      {
        BlastCommand blastCommand = new BlastCommand(
          new BlastIrDelegate(MPControlPlugin.BlastIR),
          Common.FolderIRCommands,
          MPControlPlugin.TransceiverInformation.Ports,
          selected.Substring(Common.CmdPrefixBlast.Length));

        if (blastCommand.ShowDialog(this) == DialogResult.OK)
          newCommand = Common.CmdPrefixBlast + blastCommand.CommandString;
      }
      else
      {
        newCommand = selected;
      }

      foreach (ListViewItem listViewItem in listViewEventMap.SelectedItems)
        listViewItem.SubItems[1].Text = newCommand;
    }

    private void buttonNew_Click(object sender, EventArgs e)
    {
      MultiMapNameBox multiMapNameBox = new MultiMapNameBox();
      if (multiMapNameBox.ShowDialog(this) == DialogResult.Cancel)
        return;

      string mappingName = multiMapNameBox.MapName;

      string pathCustom = Path.Combine(MPUtils.MPCommon.CustomInputDevice, "MPControlPlugin.xml");
      string pathDefault = Path.Combine(MPUtils.MPCommon.CustomInputDefault, "MPControlPlugin.xml");

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

      string destinationFile = Path.Combine(MPUtils.MPCommon.CustomInputDevice, mappingName + ".xml");

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

      MPControlPlugin.MouseModeButton = (RemoteButton)Enum.Parse(typeof(RemoteButton), comboBoxMouseModeButton.SelectedItem as string, true);
      MPControlPlugin.MouseModeStep = Decimal.ToInt32(numericUpDownMouseStep.Value);
      MPControlPlugin.MouseModeAcceleration = checkBoxMouseAcceleration.Checked;

      MPControlPlugin.MultiMappingButton = (RemoteButton)Enum.Parse(typeof(RemoteButton), comboBoxMultiButton.SelectedItem as string, true);

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
      MPControlPlugin.StopClient();

      ServerAddress serverAddress = new ServerAddress(MPControlPlugin.ServerHost);
      serverAddress.ShowDialog(this);

      MPControlPlugin.ServerHost = serverAddress.ServerHost;

      IPAddress serverIP = Client.GetIPFromName(MPControlPlugin.ServerHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, IrssComms.Server.DefaultPort);

      MPControlPlugin.StartClient(endPoint);
    }

    private void buttonLoadPreset_Click(object sender, EventArgs e)
    {
      string fileName= Path.Combine(MPControlPlugin.RemotePresetsFolder, comboBoxRemotePresets.SelectedItem as string + ".xml");
      LoadRemotes(fileName);
    }

    #endregion Buttons

    #region Other Controls

    private void listViewIR_DoubleClick(object sender, EventArgs e)
    {
      EditIR();
    }
    private void listViewMacro_DoubleClick(object sender, EventArgs e)
    {
      EditMacro();
    }

    private void listViewIR_AfterLabelEdit(object sender, LabelEditEventArgs e)
    {
      ListView origin = sender as ListView;
      if (origin == null)
      {
        e.CancelEdit = true;
        return;
      }

      if (String.IsNullOrEmpty(e.Label))
      {
        e.CancelEdit = true;
        return;
      }

      ListViewItem originItem = origin.Items[e.Item];

      string oldFileName = Path.Combine(Common.FolderIRCommands, originItem.Text + Common.FileExtensionIR);
      if (!File.Exists(oldFileName))
      {
        MessageBox.Show("File not found: " + oldFileName, "Cannot rename, Original file not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
        e.CancelEdit = true;
        return;
      }

      string name = e.Label.Trim();

      if (!Common.IsValidFileName(name))
      {
        MessageBox.Show("File name not valid: " + name, "Cannot rename, New file name not valid", MessageBoxButtons.OK, MessageBoxIcon.Error);
        e.CancelEdit = true;
        return;
      }

      try
      {
        string newFileName = Path.Combine(Common.FolderIRCommands, name + Common.FileExtensionIR);

        File.Move(oldFileName, newFileName);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(ex.Message, "Failed to rename file", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
    private void listViewMacro_AfterLabelEdit(object sender, LabelEditEventArgs e)
    {
      ListView origin = sender as ListView;
      if (origin == null)
      {
        e.CancelEdit = true;
        return;
      }

      if (String.IsNullOrEmpty(e.Label))
      {
        e.CancelEdit = true;
        return;
      }

      ListViewItem originItem = origin.Items[e.Item];

      string oldFileName = Path.Combine(MPControlPlugin.FolderMacros, originItem.Text + Common.FileExtensionMacro);
      if (!File.Exists(oldFileName))
      {
        MessageBox.Show("File not found: " + oldFileName, "Cannot rename, Original file not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
        e.CancelEdit = true;
        return;
      }

      string name = e.Label.Trim();

      if (!Common.IsValidFileName(name))
      {
        MessageBox.Show("File name not valid: " + name, "Cannot rename, New file name not valid", MessageBoxButtons.OK, MessageBoxIcon.Error);
        e.CancelEdit = true;
        return;
      }

      try
      {
        string newFileName = Path.Combine(MPControlPlugin.FolderMacros, name + Common.FileExtensionMacro);

        File.Move(oldFileName, newFileName);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(ex.Message, "Failed to rename file", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
      if (listViewEventMap.SelectedItems.Count != 1)
        return;

      string command = listViewEventMap.SelectedItems[0].SubItems[1].Text;
      string newCommand = null;

      if (command.StartsWith(Common.CmdPrefixRun, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitRunCommand(command.Substring(Common.CmdPrefixRun.Length));
        ExternalProgram externalProgram = new ExternalProgram(commands);
        if (externalProgram.ShowDialog(this) == DialogResult.OK)
          newCommand = Common.CmdPrefixRun + externalProgram.CommandString;
      }
      else if (command.StartsWith(Common.CmdPrefixGotoScreen, StringComparison.OrdinalIgnoreCase))
      {
        GoToScreen goToScreen = new GoToScreen(command.Substring(Common.CmdPrefixGotoScreen.Length));
        if (goToScreen.ShowDialog(this) == DialogResult.OK)
          newCommand = Common.CmdPrefixGotoScreen + goToScreen.CommandString;
      }
      else if (command.StartsWith(Common.CmdPrefixSerial, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitSerialCommand(command.Substring(Common.CmdPrefixSerial.Length));
        SerialCommand serialCommand = new SerialCommand(commands);
        if (serialCommand.ShowDialog(this) == DialogResult.OK)
          newCommand = Common.CmdPrefixSerial + serialCommand.CommandString;
      }
      else if (command.StartsWith(Common.CmdPrefixWindowMsg, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitWindowMessageCommand(command.Substring(Common.CmdPrefixWindowMsg.Length));
        MessageCommand messageCommand = new MessageCommand(commands);
        if (messageCommand.ShowDialog(this) == DialogResult.OK)
          newCommand = Common.CmdPrefixWindowMsg + messageCommand.CommandString;
      }
      else if (command.StartsWith(Common.CmdPrefixKeys, StringComparison.OrdinalIgnoreCase))
      {
        KeysCommand keysCommand = new KeysCommand(command.Substring(Common.CmdPrefixKeys.Length));
        if (keysCommand.ShowDialog(this) == DialogResult.OK)
          newCommand = Common.CmdPrefixKeys + keysCommand.CommandString;
      }
      else if (command.StartsWith(Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitBlastCommand(command.Substring(Common.CmdPrefixBlast.Length));

        BlastCommand blastCommand = new BlastCommand(
          new BlastIrDelegate(MPControlPlugin.BlastIR),
          Common.FolderIRCommands,
          MPControlPlugin.TransceiverInformation.Ports,
          commands);

        if (blastCommand.ShowDialog(this) == DialogResult.OK)
          newCommand = Common.CmdPrefixBlast + blastCommand.CommandString;
      }

      if (!String.IsNullOrEmpty(newCommand))
        listViewEventMap.SelectedItems[0].SubItems[1].Text = newCommand;
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
