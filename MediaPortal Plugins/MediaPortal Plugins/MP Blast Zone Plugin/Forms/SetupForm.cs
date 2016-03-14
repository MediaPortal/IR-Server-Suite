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
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using IrssComms;
using IrssUtils;
using IrssUtils.Forms;
using MediaPortal.GUI.Library;
using MPUtils.Forms;

namespace MediaPortal.Plugins.IRSS.MPBlastZonePlugin.Forms
{
  internal partial class SetupForm : Form
  {
    #region Variables

    private LearnIR _learnIR;

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

      IPAddress serverIP = Network.GetIPFromName(MPBlastZonePlugin.ServerHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, Server.DefaultPort);

      if (!MPBlastZonePlugin.StartClient(endPoint))
        MessageBox.Show(this, "Failed to start local comms. IR functions temporarily disabled.",
                        "MP Blast Zone Plugin - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

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

      MPBlastZonePlugin.HandleMessage += ReceivedMessage;
    }

    private void SetupForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      MPBlastZonePlugin.HandleMessage -= ReceivedMessage;
    }

    #region Local Methods

    private void ReceivedMessage(IrssMessage received)
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

    private void RefreshIRList()
    {
      listViewIR.Items.Clear();

      string[] irList = IrssUtils.Common.GetIRList(false);
      if (irList != null && irList.Length > 0)
        foreach (string irFile in irList)
          listViewIR.Items.Add(irFile);
    }

    private void RefreshMacroList()
    {
      listViewMacro.Items.Clear();

      string[] macroList = MPBlastZonePlugin.GetMacroList(false);
      if (macroList != null && macroList.Length > 0)
        foreach (string macroFile in macroList)
          listViewMacro.Items.Add(macroFile);
    }

    private void RefreshCommandsCombo()
    {
      comboBoxCommands.Items.Clear();

      comboBoxCommands.Items.Add(IrssUtils.Common.UITextRun);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextSerial);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextWindowMsg);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextTcpMsg);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextEject);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextGotoScreen);
      //comboBoxCommands.Items.Add(IrssUtils.Common.UITextWindowState);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextExit);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextStandby);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextHibernate);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextReboot);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextShutdown);

      string[] fileList = MPBlastZonePlugin.GetFileList(true);

      if (fileList != null && fileList.Length > 0)
        comboBoxCommands.Items.AddRange(fileList);
    }

    private void EditIR()
    {
      if (listViewIR.SelectedItems.Count != 1)
        return;

      try
      {
        string command = listViewIR.SelectedItems[0].Text;
        string fileName = Path.Combine(IrssUtils.Common.FolderIRCommands, command + IrssUtils.Common.FileExtensionIR);

        if (File.Exists(fileName))
        {
          _learnIR = new LearnIR(
            MPBlastZonePlugin.LearnIR,
            MPBlastZonePlugin.BlastIR,
            MPBlastZonePlugin.TransceiverInformation.Ports,
            command);

          _learnIR.ShowDialog(this);

          _learnIR = null;
        }
        else
        {
          RefreshIRList();
          RefreshCommandsCombo();

          throw new FileNotFoundException("IR file missing", fileName);
        }
      }
      catch (Exception ex)
      {
        Log.Error(ex);
        MessageBox.Show(this, ex.Message, "Failed to edit IR file", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void EditMacro()
    {
      if (listViewMacro.SelectedItems.Count != 1)
        return;

      try
      {
        string command = listViewMacro.SelectedItems[0].Text;
        string fileName = MPBlastZonePlugin.FolderMacros + command + IrssUtils.Common.FileExtensionMacro;

        if (File.Exists(fileName))
        {
          MacroEditor macroEditor = new MacroEditor(command);
          macroEditor.ShowDialog(this);
        }
        else
        {
          RefreshMacroList();
          RefreshCommandsCombo();

          throw new FileNotFoundException("Macro file missing", fileName);
        }
      }
      catch (Exception ex)
      {
        Log.Error(ex);
        MessageBox.Show(this, ex.Message, "Failed to edit macro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
          if (
            MessageBox.Show(this, "Are you sure you want to remove this collection?", "Remove collection",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            treeViewMenu.Nodes.Remove(treeViewMenu.SelectedNode);
          break;

        case 1:
          if (
            MessageBox.Show(this, "Are you sure you want to remove this item?", "Remove item", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question) == DialogResult.Yes)
            treeViewMenu.SelectedNode.Parent.Nodes.Remove(treeViewMenu.SelectedNode);
          break;

        case 2:
          treeViewMenu.SelectedNode.Parent.Nodes.Remove(treeViewMenu.SelectedNode);
          break;
      }
    }

    private void buttonDeleteAll_Click(object sender, EventArgs e)
    {
      if (
        MessageBox.Show(this, "Are you sure you want to clear this entire setup?", "Clear setup",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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

    private void buttonHelp_Click(object sender, EventArgs e)
    {
      try
      {
        string file = Path.Combine(SystemRegistry.GetInstallFolder(), "IR Server Suite.chm");
        Help.ShowHelp(this, file, HelpNavigator.Topic, "Plugins\\MP Blast Zone Plugin\\index.html");
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
        MPBlastZonePlugin.LearnIR,
        MPBlastZonePlugin.BlastIR,
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
      string fileName = Path.Combine(IrssUtils.Common.FolderIRCommands, file + IrssUtils.Common.FileExtensionIR);
      if (File.Exists(fileName))
      {
        if (
          MessageBox.Show(this, String.Format("Are you sure you want to delete \"{0}\"?", file), "Confirm delete",
                          MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          File.Delete(fileName);
      }
      else
      {
        MessageBox.Show(this, "File not found: " + fileName, "IR file missing", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
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
      string fileName = MPBlastZonePlugin.FolderMacros + file + IrssUtils.Common.FileExtensionMacro;
      if (File.Exists(fileName))
      {
        if (
          MessageBox.Show(this, String.Format("Are you sure you want to delete \"{0}\"?", file), "Confirm delete",
                          MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          File.Delete(fileName);
      }
      else
      {
        MessageBox.Show(this, "File not found: " + fileName, "Macro file missing", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
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
        MPBlastZonePlugin.ProcessCommand(IrssUtils.Common.CmdPrefixMacro + listViewMacro.SelectedItems[0].Text, false);
      }
      catch (Exception ex)
      {
        Log.Error(ex);
        MessageBox.Show(this, ex.Message, "Test failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
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
      string newCommand = null;

      if (selected.Equals(IrssUtils.Common.UITextRun, StringComparison.OrdinalIgnoreCase))
      {
        ExternalProgram externalProgram = new ExternalProgram();
        if (externalProgram.ShowDialog(this) == DialogResult.OK)
          newCommand = IrssUtils.Common.CmdPrefixRun + externalProgram.CommandString;
      }
      else if (selected.Equals(IrssUtils.Common.UITextSerial, StringComparison.OrdinalIgnoreCase))
      {
        SerialCommand serialCommand = new SerialCommand();
        if (serialCommand.ShowDialog(this) == DialogResult.OK)
          newCommand = IrssUtils.Common.CmdPrefixSerial + serialCommand.CommandString;
      }
      else if (selected.Equals(IrssUtils.Common.UITextWindowMsg, StringComparison.OrdinalIgnoreCase))
      {
        MessageCommand messageCommand = new MessageCommand();
        if (messageCommand.ShowDialog(this) == DialogResult.OK)
          newCommand = IrssUtils.Common.CmdPrefixWindowMsg + messageCommand.CommandString;
      }
      else if (selected.Equals(IrssUtils.Common.UITextTcpMsg, StringComparison.OrdinalIgnoreCase))
      {
        TcpMessageCommand tcpMessageCommand = new TcpMessageCommand();
        if (tcpMessageCommand.ShowDialog(this) == DialogResult.OK)
          newCommand = IrssUtils.Common.CmdPrefixTcpMsg + tcpMessageCommand.CommandString;
      }
      else if (selected.Equals(IrssUtils.Common.UITextEject, StringComparison.OrdinalIgnoreCase))
      {
        EjectCommand ejectCommand = new EjectCommand();
        if (ejectCommand.ShowDialog(this) == DialogResult.OK)
          newCommand = IrssUtils.Common.CmdPrefixEject + ejectCommand.CommandString;
      }
      else if (selected.Equals(IrssUtils.Common.UITextGotoScreen, StringComparison.OrdinalIgnoreCase))
      {
        GoToScreen goToScreen = new GoToScreen();
        if (goToScreen.ShowDialog(this) == DialogResult.OK)
          newCommand = IrssUtils.Common.CmdPrefixGotoScreen + goToScreen.CommandString;
      }
      else if (selected.StartsWith(IrssUtils.Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
      {
        BlastCommand blastCommand = new BlastCommand(
          MPBlastZonePlugin.BlastIR,
          IrssUtils.Common.FolderIRCommands,
          MPBlastZonePlugin.TransceiverInformation.Ports,
          selected.Substring(IrssUtils.Common.CmdPrefixBlast.Length));

        if (blastCommand.ShowDialog(this) == DialogResult.OK)
          newCommand = IrssUtils.Common.CmdPrefixBlast + blastCommand.CommandString;
      }
      else
      {
        newCommand = selected;
      }

      parent.Nodes.Clear();

      TreeNode newNode = new TreeNode(newCommand);
      newNode.ForeColor = Color.Blue;

      parent.Nodes.Add(newNode);
      newNode.EnsureVisible();
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

    private void buttonOK_Click(object sender, EventArgs e)
    {
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
      MPBlastZonePlugin.StopClient();

      ServerAddress serverAddress = new ServerAddress(MPBlastZonePlugin.ServerHost);
      serverAddress.ShowDialog(this);

      MPBlastZonePlugin.ServerHost = serverAddress.ServerHost;

      IPAddress serverIP = Network.GetIPFromName(MPBlastZonePlugin.ServerHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, Server.DefaultPort);

      MPBlastZonePlugin.StartClient(endPoint);
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

      string oldFileName = Path.Combine(IrssUtils.Common.FolderIRCommands, originItem.Text + IrssUtils.Common.FileExtensionIR);
      if (!File.Exists(oldFileName))
      {
        MessageBox.Show("File not found: " + oldFileName, "Cannot rename, Original file not found", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
        e.CancelEdit = true;
        return;
      }

      string name = e.Label.Trim();

      if (!IrssUtils.Common.IsValidFileName(name))
      {
        MessageBox.Show("File name not valid: " + name, "Cannot rename, New file name not valid", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
        e.CancelEdit = true;
        return;
      }

      try
      {
        string newFileName = Path.Combine(IrssUtils.Common.FolderIRCommands, name + IrssUtils.Common.FileExtensionIR);

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

      string oldFileName = MPBlastZonePlugin.FolderMacros + originItem.Text + IrssUtils.Common.FileExtensionMacro;
      if (!File.Exists(oldFileName))
      {
        MessageBox.Show("File not found: " + oldFileName, "Cannot rename, Original file not found", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
        e.CancelEdit = true;
        return;
      }

      string name = e.Label.Trim();

      if (!IrssUtils.Common.IsValidFileName(name))
      {
        MessageBox.Show("File name not valid: " + name, "Cannot rename, New file name not valid", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
        e.CancelEdit = true;
        return;
      }

      try
      {
        string newFileName = MPBlastZonePlugin.FolderMacros + name + IrssUtils.Common.FileExtensionMacro;

        File.Move(oldFileName, newFileName);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(ex.Message, "Failed to rename file", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void treeViewMenu_DoubleClick(object sender, EventArgs e)
    {
      if (treeViewMenu.SelectedNode == null)
        return;

      if (treeViewMenu.SelectedNode.Level != 2)
        return;

      string command = treeViewMenu.SelectedNode.Text;
      string newCommand = null;

      if (command.StartsWith(IrssUtils.Common.CmdPrefixRun, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = IrssUtils.Common.SplitRunCommand(command.Substring(IrssUtils.Common.CmdPrefixRun.Length));
        ExternalProgram externalProgram = new ExternalProgram(commands);
        if (externalProgram.ShowDialog(this) == DialogResult.OK)
          newCommand = IrssUtils.Common.CmdPrefixRun + externalProgram.CommandString;
      }
      else if (command.StartsWith(IrssUtils.Common.CmdPrefixGotoScreen, StringComparison.OrdinalIgnoreCase))
      {
        GoToScreen goToScreen = new GoToScreen(command.Substring(IrssUtils.Common.CmdPrefixGotoScreen.Length));
        if (goToScreen.ShowDialog(this) == DialogResult.OK)
          newCommand = IrssUtils.Common.CmdPrefixGotoScreen + goToScreen.CommandString;
      }
      else if (command.StartsWith(IrssUtils.Common.CmdPrefixSerial, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = IrssUtils.Common.SplitSerialCommand(command.Substring(IrssUtils.Common.CmdPrefixSerial.Length));
        SerialCommand serialCommand = new SerialCommand(commands);
        if (serialCommand.ShowDialog(this) == DialogResult.OK)
          newCommand = IrssUtils.Common.CmdPrefixSerial + serialCommand.CommandString;
      }
      else if (command.StartsWith(IrssUtils.Common.CmdPrefixWindowMsg, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = IrssUtils.Common.SplitWindowMessageCommand(command.Substring(IrssUtils.Common.CmdPrefixWindowMsg.Length));
        MessageCommand messageCommand = new MessageCommand(commands);
        if (messageCommand.ShowDialog(this) == DialogResult.OK)
          newCommand = IrssUtils.Common.CmdPrefixWindowMsg + messageCommand.CommandString;
      }
      else if (command.StartsWith(IrssUtils.Common.CmdPrefixTcpMsg, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = IrssUtils.Common.SplitTcpMessageCommand(command.Substring(IrssUtils.Common.CmdPrefixTcpMsg.Length));
        TcpMessageCommand tcpMessageCommand = new TcpMessageCommand(commands);
        if (tcpMessageCommand.ShowDialog(this) == DialogResult.OK)
          newCommand = IrssUtils.Common.CmdPrefixTcpMsg + tcpMessageCommand.CommandString;
      }
      else if (command.StartsWith(IrssUtils.Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = IrssUtils.Common.SplitBlastCommand(command.Substring(IrssUtils.Common.CmdPrefixBlast.Length));

        BlastCommand blastCommand = new BlastCommand(
          MPBlastZonePlugin.BlastIR,
          IrssUtils.Common.FolderIRCommands,
          MPBlastZonePlugin.TransceiverInformation.Ports,
          commands);

        if (blastCommand.ShowDialog(this) == DialogResult.OK)
          newCommand = IrssUtils.Common.CmdPrefixBlast + blastCommand.CommandString;
      }

      if (!String.IsNullOrEmpty(newCommand))
        treeViewMenu.SelectedNode.Text = newCommand;
    }

    #endregion Other Controls
  }
}