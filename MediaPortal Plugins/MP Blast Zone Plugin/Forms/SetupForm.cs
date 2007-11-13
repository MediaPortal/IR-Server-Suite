using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using MediaPortal.GUI.Library;
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
      if (String.IsNullOrEmpty(MPBlastZonePlugin.ServerHost))
      {
        ServerAddress serverAddress = new ServerAddress();
        serverAddress.ShowDialog(this);

        MPBlastZonePlugin.ServerHost = serverAddress.ServerHost;
      }

      IPAddress serverIP = Client.GetIPFromName(MPBlastZonePlugin.ServerHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, IrssComms.Server.DefaultPort);

      if (!MPBlastZonePlugin.StartClient(endPoint))
        MessageBox.Show(this, "Failed to start local comms. IR functions temporarily disabled.", "MP Blast Zone Plugin - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

      checkBoxLogVerbose.Checked = MPBlastZonePlugin.LogVerbose;

      RefreshIRList();
      RefreshMacroList();
      RefreshCommandsCombo();

      // Populate the tree
      treeViewMenu.Nodes.Clear();
      foreach (string collection in MPBlastZonePlugin.Menu.GetAllItems())
      {
        TreeNode collectionNode = new TreeNode(collection);
        treeViewMenu.Nodes.Add(collectionNode);

        foreach (string command in MPBlastZonePlugin.Menu.GetItem(collection).GetAllItems())
        {
          TreeNode commandNode = new TreeNode(command);
          commandNode.ForeColor = Color.Navy;
          collectionNode.Nodes.Add(commandNode);

          TreeNode commandValueNode = new TreeNode(MPBlastZonePlugin.Menu.GetItem(collection).GetItem(command).Command);
          commandValueNode.ForeColor = Color.Blue;
          commandNode.Nodes.Add(commandValueNode);
        }
      }

      MPBlastZonePlugin.HandleMessage += new ClientMessageSink(ReceivedMessage);
    }

    private void SetupForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      MPBlastZonePlugin.HandleMessage -= new ClientMessageSink(ReceivedMessage);
    }

    #region Local Methods

    void ReceivedMessage(IrssMessage received)
    {
      if (_learnIR != null && received.Type == MessageType.LearnIR)
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

      string[] macroList = MPBlastZonePlugin.GetMacroList(false);
      if (macroList != null && macroList.Length > 0)
        foreach (string macroFile in macroList)
          listViewMacro.Items.Add(macroFile);
    }

    void RefreshCommandsCombo()
    {
      comboBoxCommands.Items.Clear();

      comboBoxCommands.Items.Add(Common.UITextRun);
      comboBoxCommands.Items.Add(Common.UITextSerial);
      comboBoxCommands.Items.Add(Common.UITextWindowMsg);
      comboBoxCommands.Items.Add(Common.UITextTcpMsg);
      comboBoxCommands.Items.Add(Common.UITextEject);
      comboBoxCommands.Items.Add(Common.UITextGoto);
      //comboBoxCommands.Items.Add(Common.UITextWindowState);
      comboBoxCommands.Items.Add(Common.UITextExit);
      comboBoxCommands.Items.Add(Common.UITextStandby);
      comboBoxCommands.Items.Add(Common.UITextHibernate);
      comboBoxCommands.Items.Add(Common.UITextReboot);
      comboBoxCommands.Items.Add(Common.UITextShutdown);

      string[] fileList = MPBlastZonePlugin.GetFileList(true);

      if (fileList != null && fileList.Length > 0)
        comboBoxCommands.Items.AddRange(fileList);
    }

    void EditIR()
    {
      if (listViewIR.SelectedItems.Count != 1)
        return;

      string command = listViewIR.SelectedItems[0].Text;
      string fileName = Common.FolderIRCommands + command + Common.FileExtensionIR;

      if (File.Exists(fileName))
      {
        _learnIR = new LearnIR(
          new LearnIrDelegate(MPBlastZonePlugin.LearnIR),
          new BlastIrDelegate(MPBlastZonePlugin.BlastIR),
          MPBlastZonePlugin.TransceiverInformation.Ports,
          command);

        _learnIR.ShowDialog(this);

        _learnIR = null;
      }
      else
      {
        MessageBox.Show(this, "File not found: " + fileName, "IR file missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        RefreshIRList();
        RefreshCommandsCombo();
      }
    }
    void EditMacro()
    {
      if (listViewMacro.SelectedItems.Count != 1)
        return;

      string command = listViewMacro.SelectedItems[0].Text;
      string fileName = MPBlastZonePlugin.FolderMacros + command + Common.FileExtensionMacro;

      if (File.Exists(fileName))
      {
        MacroEditor macroEditor = new MacroEditor(command);
        macroEditor.ShowDialog(this);
      }
      else
      {
        MessageBox.Show(this, "File not found: " + fileName, "Macro file missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        RefreshMacroList();
        RefreshCommandsCombo();
      }
    }

    #endregion Local Methods

    #region Buttons

    private void buttonAdd_Click(object sender, EventArgs e)
    {
      TreeNode newNode = new TreeNode("New Collection");
      newNode.ForeColor = Color.Black;

      treeViewMenu.Nodes.Add(newNode);
      newNode.EnsureVisible();
    }
    private void buttonDelete_Click(object sender, EventArgs e)
    {
      if (treeViewMenu.SelectedNode == null)
        return;

      switch (treeViewMenu.SelectedNode.Level)
      {
        case 0:
          if (MessageBox.Show(this, "Are you sure you want to remove this collection?", "Remove collection", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

          treeViewMenu.Nodes.Remove(treeViewMenu.SelectedNode);
          break;

        case 1:
          if (MessageBox.Show(this, "Are you sure you want to remove this item?", "Remove item", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

          treeViewMenu.SelectedNode.Parent.Nodes.Remove(treeViewMenu.SelectedNode);
          break;

        case 2:
          treeViewMenu.SelectedNode.Parent.Nodes.Remove(treeViewMenu.SelectedNode);
          break;
      }
    }
    private void buttonDeleteAll_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show(this, "Are you sure you want to clear this entire setup?", "Clear setup", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;

      treeViewMenu.Nodes.Clear();
    }
    private void buttonNewCommand_Click(object sender, EventArgs e)
    {
      if (treeViewMenu.SelectedNode == null)
        return;

      TreeNode parent = treeViewMenu.SelectedNode;
      switch (treeViewMenu.SelectedNode.Level)
      {
        case 0:
          parent = treeViewMenu.SelectedNode;
          break;

        case 1:
          parent = treeViewMenu.SelectedNode.Parent;
          break;

        case 2:
          parent = treeViewMenu.SelectedNode.Parent.Parent;
          break;
      }

      TreeNode newNode = new TreeNode("New Command");
      newNode.ForeColor = Color.Navy;

      parent.Nodes.Add(newNode);
      newNode.EnsureVisible();
    }
    private void buttonSetCommand_Click(object sender, EventArgs e)
    {
      if (treeViewMenu.SelectedNode == null)
        return;

      TreeNode parent = treeViewMenu.SelectedNode;
      switch (treeViewMenu.SelectedNode.Level)
      {
        case 0:
          return;

        case 1:
          parent = treeViewMenu.SelectedNode;
          break;

        case 2:
          parent = treeViewMenu.SelectedNode.Parent;
          break;
      }

      if (comboBoxCommands.SelectedIndex == -1)
        return;

      string selected = comboBoxCommands.SelectedItem as string;
      string command = String.Empty;

      if (selected.Equals(Common.UITextRun, StringComparison.OrdinalIgnoreCase))
      {
        ExternalProgram externalProgram = new ExternalProgram();
        if (externalProgram.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixRun + externalProgram.CommandString;
      }
      else if (selected.Equals(Common.UITextGoto, StringComparison.OrdinalIgnoreCase))
      {
        GoToScreen goToScreen = new GoToScreen();
        if (goToScreen.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixGoto + goToScreen.Screen;
      }
      else if (selected.Equals(Common.UITextSerial, StringComparison.OrdinalIgnoreCase))
      {
        SerialCommand serialCommand = new SerialCommand();
        if (serialCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixSerial + serialCommand.CommandString;
      }
      else if (selected.Equals(Common.UITextWindowMsg, StringComparison.OrdinalIgnoreCase))
      {
        MessageCommand messageCommand = new MessageCommand();
        if (messageCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixWindowMsg + messageCommand.CommandString;
      }
      else if (selected.StartsWith(Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
      {
        BlastCommand blastCommand = new BlastCommand(
          new BlastIrDelegate(MPBlastZonePlugin.BlastIR),
          Common.FolderIRCommands,
          MPBlastZonePlugin.TransceiverInformation.Ports,
          selected.Substring(Common.CmdPrefixBlast.Length));

        if (blastCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixBlast + blastCommand.CommandString;
      }
      else
      {
        command = selected;
      }

      parent.Nodes.Clear();

      TreeNode newNode = new TreeNode(command);
      newNode.ForeColor = Color.Blue;

      parent.Nodes.Add(newNode);
      newNode.EnsureVisible();
    }

    private void buttonNewIR_Click(object sender, EventArgs e)
    {
      _learnIR = new LearnIR(
        new LearnIrDelegate(MPBlastZonePlugin.LearnIR),
        new BlastIrDelegate(MPBlastZonePlugin.BlastIR),
        MPBlastZonePlugin.TransceiverInformation.Ports);

      _learnIR.ShowDialog(this);

      _learnIR = null;

      RefreshIRList();
      RefreshCommandsCombo();
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
      string fileName = Common.FolderIRCommands + file + Common.FileExtensionIR;
      if (File.Exists(fileName))
      {
        if (MessageBox.Show(this, "Are you sure you want to delete \"" + file + "\"?", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          File.Delete(fileName);
      }
      else
      {
        MessageBox.Show(this, "File not found: " + fileName, "IR file missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }

      RefreshIRList();
      RefreshCommandsCombo();
    }

    private void buttonNewMacro_Click(object sender, EventArgs e)
    {
      MacroEditor macroEditor = new MacroEditor();
      macroEditor.ShowDialog(this);

      RefreshMacroList();
      RefreshCommandsCombo();
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
      string fileName = MPBlastZonePlugin.FolderMacros + file + Common.FileExtensionMacro;
      if (File.Exists(fileName))
      {
        if (MessageBox.Show(this, "Are you sure you want to delete \"" + file + "\"?", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          File.Delete(fileName);
      }
      else
      {
        MessageBox.Show(this, "File not found: " + fileName, "Macro file missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }

      RefreshMacroList();
      RefreshCommandsCombo();
    }
    private void buttonTestMacro_Click(object sender, EventArgs e)
    {
      if (listViewMacro.SelectedItems.Count != 1)
        return;

      try
      {
        string fileName = MPBlastZonePlugin.FolderMacros + listViewMacro.SelectedItems[0].Text + Common.FileExtensionMacro;

        MPBlastZonePlugin.ProcessMacro(fileName);
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Test failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      MPBlastZonePlugin.LogVerbose = checkBoxLogVerbose.Checked;

      // Save menu ...
      MPBlastZonePlugin.Menu.Clear();
      foreach (TreeNode collectionNode in treeViewMenu.Nodes)
      {
        MenuFolder collection = new MenuFolder(collectionNode.Text);
        MPBlastZonePlugin.Menu.Add(collection);

        foreach (TreeNode commandNode in collectionNode.Nodes)
        {
          string commandValue = String.Empty;
          if (commandNode.Nodes.Count == 1)
            commandValue = commandNode.Nodes[0].Text;
          
          MenuCommand command = new MenuCommand(commandNode.Text, commandValue);
          collection.Add(command);
        }
      }
      MPBlastZonePlugin.Menu.Save(MPBlastZonePlugin.MenuFile);

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
      MPBlastZonePlugin.StopClient();

      ServerAddress serverAddress = new ServerAddress(MPBlastZonePlugin.ServerHost);
      serverAddress.ShowDialog(this);

      MPBlastZonePlugin.ServerHost = serverAddress.ServerHost;

      IPAddress serverIP = Client.GetIPFromName(MPBlastZonePlugin.ServerHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, IrssComms.Server.DefaultPort);

      MPBlastZonePlugin.StartClient(endPoint);
    }

    private void buttonHelp_Click(object sender, EventArgs e)
    {
      try
      {
        Help.ShowHelp(this, SystemRegistry.GetInstallFolder() + "\\IR Server Suite.chm", HelpNavigator.Topic, "Plugins\\MP Blast Zone Plugin\\index.html");
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Failed to load help", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void buttonTop_Click(object sender, EventArgs e)
    {
      if (treeViewMenu.SelectedNode == null)
        return;

      TreeNode selected = treeViewMenu.SelectedNode;

      switch (selected.Level)
      {
        case 0:
          treeViewMenu.Nodes.Remove(selected);
          treeViewMenu.Nodes.Insert(0, selected);
          break;

        case 1:
          TreeNode parent = selected.Parent;
          parent.Nodes.Remove(selected);
          parent.Nodes.Insert(0, selected);
          break;

        case 2:
          return;
      }

      selected.EnsureVisible();
    }
    private void buttonUp_Click(object sender, EventArgs e)
    {
      if (treeViewMenu.SelectedNode == null)
        return;

      TreeNode selected = treeViewMenu.SelectedNode;
      int index;

      switch (selected.Level)
      {
        case 0:
          index = treeViewMenu.Nodes.IndexOf(selected);
          if (index > 1)
          {
            treeViewMenu.Nodes.Remove(selected);
            treeViewMenu.Nodes.Insert(index - 1, selected);
          }
          break;

        case 1:
          TreeNode parent = selected.Parent;
          index = parent.Nodes.IndexOf(selected);
          if (index > 1)
          {
            parent.Nodes.Remove(selected);
            parent.Nodes.Insert(index - 1, selected);
          }
          break;

        case 2:
          return;
      }

      selected.EnsureVisible();
    }
    private void buttonDown_Click(object sender, EventArgs e)
    {
      if (treeViewMenu.SelectedNode == null)
        return;

      TreeNode selected = treeViewMenu.SelectedNode;
      int index;

      switch (selected.Level)
      {
        case 0:
          index = treeViewMenu.Nodes.IndexOf(selected);
          if (index < treeViewMenu.Nodes.Count - 1)
          {
            treeViewMenu.Nodes.Remove(selected);
            treeViewMenu.Nodes.Insert(index + 1, selected);
          }
          break;

        case 1:
          TreeNode parent = selected.Parent;
          index = parent.Nodes.IndexOf(selected);
          if (index < parent.Nodes.Count - 1)
          {
            parent.Nodes.Remove(selected);
            parent.Nodes.Insert(index + 1, selected);
          }
          break;

        case 2:
          return;
      }

      selected.EnsureVisible();
    }
    private void buttonBottom_Click(object sender, EventArgs e)
    {
      if (treeViewMenu.SelectedNode == null)
        return;

      TreeNode selected = treeViewMenu.SelectedNode;

      switch (selected.Level)
      {
        case 0:
          treeViewMenu.Nodes.Remove(selected);
          treeViewMenu.Nodes.Add(selected);
          break;

        case 1:
          TreeNode parent = selected.Parent;
          parent.Nodes.Remove(selected);
          parent.Nodes.Add(selected);
          break;

        case 2:
          return;
      }

      selected.EnsureVisible();
    }

    private void buttonEditTree_Click(object sender, EventArgs e)
    {
      if (treeViewMenu.SelectedNode == null)
        return;

      switch (treeViewMenu.SelectedNode.Level)
      {
        case 0:
        case 1:
          treeViewMenu.SelectedNode.BeginEdit();
          break;

        case 2:
          treeViewMenu_DoubleClick(null, null);
          break;
      }
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

      string oldFileName = Common.FolderIRCommands + originItem.Text + Common.FileExtensionIR;
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
        string newFileName = Common.FolderIRCommands + name + Common.FileExtensionIR;

        File.Move(oldFileName, newFileName);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
        MessageBox.Show(ex.ToString(), "Failed to rename file", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

      string oldFileName = MPBlastZonePlugin.FolderMacros + originItem.Text + Common.FileExtensionMacro;
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
        string newFileName = MPBlastZonePlugin.FolderMacros + name + Common.FileExtensionMacro;

        File.Move(oldFileName, newFileName);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
        MessageBox.Show(ex.ToString(), "Failed to rename file", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void treeViewMenu_DoubleClick(object sender, EventArgs e)
    {
      if (treeViewMenu.SelectedNode == null)
        return;

      if (treeViewMenu.SelectedNode.Level != 2)
        return;

      if (treeViewMenu.SelectedNode.Text.StartsWith(Common.CmdPrefixPause, StringComparison.OrdinalIgnoreCase))
      {
        PauseTime pauseTime = new PauseTime(int.Parse(treeViewMenu.SelectedNode.Text.Substring(Common.CmdPrefixPause.Length)));
        if (pauseTime.ShowDialog(this) == DialogResult.Cancel)
          return;

        treeViewMenu.SelectedNode.Text = Common.CmdPrefixPause + pauseTime.Time.ToString();
      }
      else if (treeViewMenu.SelectedNode.Text.StartsWith(Common.CmdPrefixRun, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitRunCommand(treeViewMenu.SelectedNode.Text.Substring(Common.CmdPrefixRun.Length));
        ExternalProgram executeProgram = new ExternalProgram(commands);
        if (executeProgram.ShowDialog(this) == DialogResult.Cancel)
          return;

        treeViewMenu.SelectedNode.Text = Common.CmdPrefixRun + executeProgram.CommandString;
      }
      else if (treeViewMenu.SelectedNode.Text.StartsWith(Common.CmdPrefixSerial, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitSerialCommand(treeViewMenu.SelectedNode.Text.Substring(Common.CmdPrefixSerial.Length));
        SerialCommand serialCommand = new SerialCommand(commands);
        if (serialCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        treeViewMenu.SelectedNode.Text = Common.CmdPrefixSerial + serialCommand.CommandString;
      }
      else if (treeViewMenu.SelectedNode.Text.StartsWith(Common.CmdPrefixWindowMsg, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitWindowMessageCommand(treeViewMenu.SelectedNode.Text.Substring(Common.CmdPrefixWindowMsg.Length));
        MessageCommand messageCommand = new MessageCommand(commands);
        if (messageCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        treeViewMenu.SelectedNode.Text = Common.CmdPrefixWindowMsg + messageCommand.CommandString;
      }
      else if (treeViewMenu.SelectedNode.Text.StartsWith(Common.CmdPrefixGoto, StringComparison.OrdinalIgnoreCase))
      {
        GoToScreen goToScreen = new GoToScreen(treeViewMenu.SelectedNode.Text.Substring(Common.CmdPrefixGoto.Length));
        if (goToScreen.ShowDialog(this) == DialogResult.Cancel)
          return;

        treeViewMenu.SelectedNode.Text = Common.CmdPrefixGoto + goToScreen.Screen;
      }
      else if (treeViewMenu.SelectedNode.Text.StartsWith(Common.CmdPrefixPopup, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitPopupCommand(treeViewMenu.SelectedNode.Text.Substring(Common.CmdPrefixPopup.Length));
        PopupMessage popupMessage = new PopupMessage(commands);
        if (popupMessage.ShowDialog(this) == DialogResult.Cancel)
          return;

        treeViewMenu.SelectedNode.Text = Common.CmdPrefixPopup + popupMessage.CommandString;
      }
      else if (treeViewMenu.SelectedNode.Text.StartsWith(Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitBlastCommand(treeViewMenu.SelectedNode.Text.Substring(Common.CmdPrefixBlast.Length));

        BlastCommand blastCommand = new BlastCommand(
          new BlastIrDelegate(MPBlastZonePlugin.BlastIR),
          Common.FolderIRCommands,
          MPBlastZonePlugin.TransceiverInformation.Ports,
          commands);

        if (blastCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        treeViewMenu.SelectedNode.Text = Common.CmdPrefixBlast + blastCommand.CommandString;
      }

    }

    #endregion Other Controls

  }

}
