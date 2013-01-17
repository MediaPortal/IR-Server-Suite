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
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using IrssCommands;
using IrssCommands.Forms;
using IrssComms;
using IrssUtils;
using IrssUtils.Forms;

namespace MediaPortal.Plugins.IRSS.MPBlastZonePlugin.Forms
{
  internal partial class SetupForm : Form
  {
    #region Constants

    private static Color ColorCategory = Color.Black;
    private static Color ColorCommandTitle = Color.Navy;
    private static Color ColorCommand = Color.Blue;

    #endregion Constants

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

      // setup images
      toolStripButtonTop.Image = IrssUtils.Properties.Resources.MoveTop;
      toolStripButtonUp.Image = IrssUtils.Properties.Resources.MoveUp;
      toolStripButtonDown.Image = IrssUtils.Properties.Resources.MoveDown;
      toolStripButtonBottom.Image = IrssUtils.Properties.Resources.MoveBottom;

      toolStripButtonAddCategory.Image = IrssUtils.Properties.Resources.AddFolder;
      toolStripButtonAddCommand.Image = IrssUtils.Properties.Resources.Plus;
      toolStripButtonEdit.Image = IrssUtils.Properties.Resources.Edit;
      toolStripButtonDelete.Image = IrssUtils.Properties.Resources.Delete;
      toolStripButtonDeleteAll.Image = IrssUtils.Properties.Resources.DeleteAll;

      toolStripButtonTop.DisplayStyle = ToolStripItemDisplayStyle.Image;
      toolStripButtonUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
      toolStripButtonDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
      toolStripButtonBottom.DisplayStyle = ToolStripItemDisplayStyle.Image;

      toolStripButtonAddCategory.DisplayStyle = ToolStripItemDisplayStyle.Image;
      toolStripButtonAddCommand.DisplayStyle = ToolStripItemDisplayStyle.Image;
      toolStripButtonEdit.DisplayStyle = ToolStripItemDisplayStyle.Image;
      toolStripButtonDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
      toolStripButtonDeleteAll.DisplayStyle = ToolStripItemDisplayStyle.Image;


      // add macro panel
      _macroPanel = new MacroPanel(MPBlastZonePlugin.CommandProcessor, MPBlastZonePlugin.FolderMacros, MPBlastZonePlugin.MacroCategories);
      _macroPanel.Dock = DockStyle.Fill;
      tabPageMacros.Controls.Add(_macroPanel);

      // add macro panel
      _irCommandPanel = new IRCommandPanel(NewIRCommand, EditIRCommand);
      _irCommandPanel.Dock = DockStyle.Fill;
      tabPageIR.Controls.Add(_irCommandPanel);
    }

    #endregion Constructor

    #region Implementation

    #region Form

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

      // macros tab
      _macroPanel.RefreshList();

      // ircommands tab
      _irCommandPanel.RefreshList();

      // menu tab
      PopulateCommandList();
      RefreshTreeView();

      MPBlastZonePlugin.HandleMessage += ReceivedMessage;
    }

    private void SetupForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      MPBlastZonePlugin.HandleMessage -= ReceivedMessage;
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

    private void buttonOK_Click(object sender, EventArgs e)
    {
      MPBlastZonePlugin.Menu.Items = GetFromTreeView().Items;
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

    #endregion Form

    #region Menu tab

    private void RefreshTreeView()
    {
      treeViewMenu.Nodes.Clear();
      foreach (MenuFolder folder in MPBlastZonePlugin.Menu.Items)
      {
        TreeNode collectionNode = new TreeNode(folder.Name);
        collectionNode.ForeColor = ColorCategory;
        collectionNode.Tag = folder;
        treeViewMenu.Nodes.Add(collectionNode);

        foreach (MenuCommand command in folder.Items)
        {
          TreeNode commandNameNode = new TreeNode(command.Name);
          commandNameNode.ForeColor = ColorCommandTitle;

          TreeNode commandNode = new TreeNode(command.GetCommandDisplayTextSafe());
          commandNode.Tag = command.GetCommandSafe();
          commandNode.ForeColor = ColorCommand;

          commandNameNode.Nodes.Add(commandNode);
          collectionNode.Nodes.Add(commandNameNode);
        }
      }

      treeViewMenu_AfterSelect(null, null);
    }

    private MenuRoot GetFromTreeView()
    {
      MenuRoot returnMenu = new MenuRoot();

      foreach (TreeNode collectionNode in treeViewMenu.Nodes)
      {
        MenuFolder collection = new MenuFolder(collectionNode.Text);

        foreach (TreeNode commandNode in collectionNode.Nodes)
        {
          if (commandNode.Nodes.Count != 1) continue;
          if (ReferenceEquals(commandNode.Nodes[0].Tag, null)) continue;

          MenuCommand mc = new MenuCommand();
          mc.Name = commandNode.Text;

          Command cmd = commandNode.Nodes[0].Tag as Command;
          DummyCommand dc = commandNode.Nodes[0].Tag as DummyCommand;
          if (!ReferenceEquals(cmd, null))
          {
            mc.Command = cmd;
          }
          else if (!ReferenceEquals(dc, null))
          {
            mc.CommandType = dc.CommandType;
            mc.Parameters = dc.Parameters;
          }

          if (!ReferenceEquals(mc.GetCommandSafe(),null))
            collection.Items.Add(mc);
        }

        if (collection.Items.Count > 0)
          returnMenu.Items.Add(collection);
      }

      return returnMenu;
    }

    private void treeViewMenu_AfterSelect(object sender, TreeViewEventArgs e)
    {
      // update controls if null
      TreeNode node = treeViewMenu.SelectedNode;
      if (ReferenceEquals(node, null))
      {
        splitContainer1.Panel2.Enabled = false;
        toolStripButtonTop.Enabled = false;
        toolStripButtonUp.Enabled = false;
        toolStripButtonDown.Enabled = false;
        toolStripButtonBottom.Enabled = false;

        toolStripButtonAddCommand.Enabled = false;
        toolStripButtonEdit.Enabled = false;
        toolStripButtonDelete.Enabled = false;
        return;
      }

      // update controls depending on level
      toolStripButtonAddCommand.Enabled = true;
      toolStripButtonEdit.Enabled = true;
      toolStripButtonDelete.Enabled = true;
      
      switch (node.Level)
      {
        case 0:
          splitContainer1.Panel2.Enabled = false;
          break;

        case 1:
          splitContainer1.Panel2.Enabled = true;
          break;

        case 2:
          splitContainer1.Panel2.Enabled = true;
          break;
      }

      int nodesCount = ReferenceEquals(node.Parent, null) ? treeViewMenu.Nodes.Count : node.Parent.Nodes.Count;
      // update controls depending on index
      if (node.Index == 0 || node.Index == nodesCount - 1)
      {
        // first
        toolStripButtonTop.Enabled = node.Index != 0;
        toolStripButtonUp.Enabled = node.Index != 0;
        // last
        toolStripButtonDown.Enabled = node.Index != nodesCount -1;
        toolStripButtonBottom.Enabled = node.Index != nodesCount -1;
      }
      else
      {
        toolStripButtonTop.Enabled = true;
        toolStripButtonUp.Enabled = true;
        toolStripButtonDown.Enabled = true;
        toolStripButtonBottom.Enabled = true;
      }
    }

    private void treeViewMenu_DoubleClick(object sender, EventArgs e)
    {
      TreeNode node = treeViewMenu.SelectedNode;
      if (ReferenceEquals(node, null)) return;
      if (node.Level != 2) return;

      Command command = node.Tag as Command;
      if (ReferenceEquals(command, null)) return;

      if (!MPBlastZonePlugin.CommandProcessor.Edit(command, this)) return;

      node.Text = command.UserDisplayText;
      node.Tag = command;



      //string command = treeViewMenu.SelectedNode.Text;
      //string newCommand = null;

      //if (command.StartsWith(IrssUtils.Common.CmdPrefixGotoScreen, StringComparison.OrdinalIgnoreCase))
      //{
      //  GoToScreen goToScreen = new GoToScreen(command.Substring(IrssUtils.Common.CmdPrefixGotoScreen.Length));
      //  if (goToScreen.ShowDialog(this) == DialogResult.OK)
      //    newCommand = IrssUtils.Common.CmdPrefixGotoScreen + goToScreen.CommandString;
      //}
      //else if (command.StartsWith(IrssUtils.Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
      //{
      //  string[] commands = IrssUtils.Common.SplitBlastCommand(command.Substring(IrssUtils.Common.CmdPrefixBlast.Length));

      //  BlastCommand blastCommand = new BlastCommand(
      //    MPBlastZonePlugin.BlastIR,
      //    IrssUtils.Common.FolderIRCommands,
      //    MPBlastZonePlugin.TransceiverInformation.Ports,
      //    commands);

      //  if (blastCommand.ShowDialog(this) == DialogResult.OK)
      //    newCommand = IrssUtils.Common.CmdPrefixBlast + blastCommand.CommandString;
      //}

      //if (!String.IsNullOrEmpty(newCommand))
      //  treeViewMenu.SelectedNode.Text = newCommand;
    }

    private void PopulateCommandList()
    {
      treeViewCommandList.Nodes.Clear();
      Dictionary<string, TreeNode> categoryNodes = new Dictionary<string, TreeNode>(MPBlastZonePlugin.CommandCategories.Length);

      // Create requested categories ...
      foreach (string category in MPBlastZonePlugin.CommandCategories)
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
      TreeNode node = treeViewCommandList.SelectedNode;
      if (ReferenceEquals(node, null)) return;
      if (node.Level == 0) return;
      TreeNode commandNameNode = treeViewMenu.SelectedNode;
      if (ReferenceEquals(commandNameNode, null)) return;

      Type commandType = treeViewCommandList.SelectedNode.Tag as Type;
      if (ReferenceEquals(commandType, null)) return;

      Command command = (Command)Activator.CreateInstance(commandType);
      if (!MPBlastZonePlugin.CommandProcessor.Edit(command, this)) return;

      if (commandNameNode.Level == 2)
        commandNameNode = commandNameNode.Parent;

      commandNameNode.Nodes.Clear();
      TreeNode commandNode = new TreeNode(command.UserDisplayText);
      commandNode.Tag = command;
      commandNode.ForeColor = ColorCommand;
      commandNameNode.Nodes.Add(commandNode);
      commandNode.EnsureVisible();
    }

    #region toolstrip buttons

    private void toolStripButtonAddCategory_Click(object sender, EventArgs e)
    {
      TreeNode node = treeViewMenu.SelectedNode;

      int index;
      if (ReferenceEquals(node, null))
      {
        // no node select, add at the end
        index = treeViewMenu.Nodes.Count;
      }
      else
      {
        // add after current selected root node
        switch (node.Level)
        {
          case 1:
            node = node.Parent;
            break;
          case 2:
            node = node.Parent.Parent;
            break;
        }

        index = node.Index + 1;
      }

      TreeNode newNode = new TreeNode("New Collection");
      newNode.ForeColor = ColorCategory;

      treeViewMenu.Nodes.Insert(index, newNode);
      newNode.EnsureVisible();
      treeViewMenu.SelectedNode = newNode;
      newNode.BeginEdit();
    }

    private void toolStripButtonAddCommand_Click(object sender, EventArgs e)
    {
      TreeNode node = treeViewMenu.SelectedNode;
      if (ReferenceEquals(node, null)) return;

      int index;
      switch (node.Level)
      {
        case 1:
          index = node.Index + 1;
          node = node.Parent;
          break;

        case 2:
          index = node.Parent.Index + 1;
          node = node.Parent.Parent;
          break;

        default:
          index = node.Nodes.Count;
          break;
      }

      TreeNode newNode = new TreeNode("New Command");
      newNode.ForeColor = ColorCommandTitle;

      node.Nodes.Insert(index, newNode);
      newNode.EnsureVisible();
      treeViewMenu.SelectedNode = newNode;
      newNode.BeginEdit();
    }

    private void toolStripButtonEdit_Click(object sender, EventArgs e)
    {
      TreeNode node = treeViewMenu.SelectedNode;
      if (ReferenceEquals(node, null)) return;

      switch (node.Level)
      {
        case 0:
        case 1:
          node.BeginEdit();
          break;

        case 2:
          treeViewMenu_DoubleClick(null, null);
          break;
      }
    }

    private void toolStripButtonDelete_Click(object sender, EventArgs e)
    {
      TreeNode node = treeViewMenu.SelectedNode;
      if (ReferenceEquals(node, null)) return;

      switch (node.Level)
      {
        case 0:
          if (
            MessageBox.Show(this, "Are you sure you want to remove this collection?", "Remove collection",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            treeViewMenu.Nodes.Remove(node);
          break;

        case 1:
          if (
            MessageBox.Show(this, "Are you sure you want to remove this item?", "Remove item", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question) == DialogResult.Yes)
            node.Parent.Nodes.Remove(node);
          break;

        case 2:
          node.Parent.Nodes.Remove(node);
          break;
      }
    }

    private void toolStripButtonDeleteAll_Click(object sender, EventArgs e)
    {
      if (
        MessageBox.Show(this, "Are you sure you want to clear this entire setup?", "Clear setup",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        treeViewMenu.Nodes.Clear();

      treeViewMenu_AfterSelect(null, null);
    }

    private void toolStripButtonTop_Click(object sender, EventArgs e)
    {
      TreeNode node = treeViewMenu.SelectedNode;
      if (ReferenceEquals(node, null)) return;

      switch (node.Level)
      {
        case 0:
          treeViewMenu.Nodes.Remove(node);
          treeViewMenu.Nodes.Insert(0, node);
          break;

        case 1:
          TreeNode parent = node.Parent;
          parent.Nodes.Remove(node);
          parent.Nodes.Insert(0, node);
          break;

        case 2:
          return;
      }

      node.EnsureVisible();
      treeViewMenu.SelectedNode = node;
    }

    private void toolStripButtonUp_Click(object sender, EventArgs e)
    {
      TreeNode node = treeViewMenu.SelectedNode;
      if (ReferenceEquals(node, null)) return;

      int index;
      switch (node.Level)
      {
        case 0:
          index = treeViewMenu.Nodes.IndexOf(node);
          if (index > 0)
          {
            treeViewMenu.Nodes.Remove(node);
            treeViewMenu.Nodes.Insert(index - 1, node);
          }
          break;

        case 1:
          TreeNode parent = node.Parent;
          index = parent.Nodes.IndexOf(node);
          if (index > 0)
          {
            parent.Nodes.Remove(node);
            parent.Nodes.Insert(index - 1, node);
          }
          break;

        case 2:
          return;
      }

      node.EnsureVisible();
      treeViewMenu.SelectedNode = node;
    }

    private void toolStripButtonDown_Click(object sender, EventArgs e)
    {
      TreeNode node = treeViewMenu.SelectedNode;
      if (ReferenceEquals(node, null)) return;

      int index;
      switch (node.Level)
      {
        case 0:
          index = treeViewMenu.Nodes.IndexOf(node);
          if (index < treeViewMenu.Nodes.Count - 1)
          {
            treeViewMenu.Nodes.Remove(node);
            treeViewMenu.Nodes.Insert(index + 1, node);
          }
          break;

        case 1:
          TreeNode parent = node.Parent;
          index = parent.Nodes.IndexOf(node);
          if (index < parent.Nodes.Count - 1)
          {
            parent.Nodes.Remove(node);
            parent.Nodes.Insert(index + 1, node);
          }
          break;

        case 2:
          return;
      }

      node.EnsureVisible();
      treeViewMenu.SelectedNode = node;
    }

    private void toolStripButtonBottom_Click(object sender, EventArgs e)
    {
      TreeNode node = treeViewMenu.SelectedNode;
      if (ReferenceEquals(node, null)) return;

      switch (node.Level)
      {
        case 0:
          treeViewMenu.Nodes.Remove(node);
          treeViewMenu.Nodes.Add(node);
          break;

        case 1:
          TreeNode parent = node.Parent;
          parent.Nodes.Remove(node);
          parent.Nodes.Add(node);
          break;

        case 2:
          return;
      }

      node.EnsureVisible();
      treeViewMenu.SelectedNode = node;
    }

    #endregion toolstrip buttons

    #endregion Menu tab

    #region IRCommands tab

    private void NewIRCommand()
    {
      _learnIR = new LearnIR(
        MPBlastZonePlugin.LearnIR,
        MPBlastZonePlugin.BlastIR,
        MPBlastZonePlugin.TransceiverInformation.Ports);

      _learnIR.ShowDialog(this);

      _learnIR = null;
    }

    private void EditIRCommand(string fileName)
    {
      string command = Path.GetFileNameWithoutExtension(fileName);

      _learnIR = new LearnIR(
        MPBlastZonePlugin.LearnIR,
        MPBlastZonePlugin.BlastIR,
        MPBlastZonePlugin.TransceiverInformation.Ports,
        command);

      _learnIR.ShowDialog(this);

      _learnIR = null;
    }

    #endregion IRCommands tab

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
      MPBlastZonePluginShowException ex = new MPBlastZonePluginShowException(e.Exception);
      ex.ShowDialog(this);
    }

    void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
      MPBlastZonePluginShowException ex = new MPBlastZonePluginShowException(e.ExceptionObject as Exception);
      ex.ShowDialog(this);
    }

    #endregion Exception Handling

    #endregion Implementation

    #region Buttons

    //private void buttonNewCommand_Click(object sender, EventArgs e)
    //{
    //  if (treeViewMenu.SelectedNode == null)
    //    return;

    //  TreeNode parent = treeViewMenu.SelectedNode;
    //  switch (treeViewMenu.SelectedNode.Level)
    //  {
    //    case 0:
    //      parent = treeViewMenu.SelectedNode;
    //      break;

    //    case 1:
    //      parent = treeViewMenu.SelectedNode.Parent;
    //      break;

    //    case 2:
    //      parent = treeViewMenu.SelectedNode.Parent.Parent;
    //      break;
    //  }

    //  TreeNode newNode = new TreeNode("New Command");
    //  newNode.ForeColor = Color.Navy;

    //  parent.Nodes.Add(newNode);
    //  newNode.EnsureVisible();
    //}

    //private void buttonNewIR_Click(object sender, EventArgs e)
    //{
    //  _learnIR = new LearnIR(
    //    MPBlastZonePlugin.LearnIR,
    //    MPBlastZonePlugin.BlastIR,
    //    MPBlastZonePlugin.TransceiverInformation.Ports);

    //  _learnIR.ShowDialog(this);

    //  _learnIR = null;

    //  RefreshIRList();
    //  RefreshCommandsCombo();
    //}

    //private void buttonEditIR_Click(object sender, EventArgs e)
    //{
    //  EditIR();
    //}

    //private void buttonDeleteIR_Click(object sender, EventArgs e)
    //{
    //  if (listViewIR.SelectedItems.Count != 1)
    //    return;

    //  string file = listViewIR.SelectedItems[0].Text;
    //  string fileName = Path.Combine(IrssUtils.Common.FolderIRCommands, file + IrssUtils.Common.FileExtensionIR);
    //  if (File.Exists(fileName))
    //  {
    //    if (
    //      MessageBox.Show(this, String.Format("Are you sure you want to delete \"{0}\"?", file), "Confirm delete",
    //                      MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
    //      File.Delete(fileName);
    //  }
    //  else
    //  {
    //    MessageBox.Show(this, "File not found: " + fileName, "IR file missing", MessageBoxButtons.OK,
    //                    MessageBoxIcon.Exclamation);
    //  }

    //  RefreshIRList();
    //  RefreshCommandsCombo();
    //}

    //private void buttonNewMacro_Click(object sender, EventArgs e)
    //{
    //  MacroEditor macroEditor = new MacroEditor();
    //  macroEditor.ShowDialog(this);

    //  RefreshMacroList();
    //  RefreshCommandsCombo();
    //}

    //private void buttonEditMacro_Click(object sender, EventArgs e)
    //{
    //  EditMacro();
    //}

    //private void buttonDeleteMacro_Click(object sender, EventArgs e)
    //{
    //  if (listViewMacro.SelectedItems.Count != 1)
    //    return;

    //  string file = listViewMacro.SelectedItems[0].Text;
    //  string fileName = MPBlastZonePlugin.FolderMacros + file + IrssUtils.Common.FileExtensionMacro;
    //  if (File.Exists(fileName))
    //  {
    //    if (
    //      MessageBox.Show(this, String.Format("Are you sure you want to delete \"{0}\"?", file), "Confirm delete",
    //                      MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
    //      File.Delete(fileName);
    //  }
    //  else
    //  {
    //    MessageBox.Show(this, "File not found: " + fileName, "Macro file missing", MessageBoxButtons.OK,
    //                    MessageBoxIcon.Exclamation);
    //  }

    //  RefreshMacroList();
    //  RefreshCommandsCombo();
    //}

    //private void buttonTestMacro_Click(object sender, EventArgs e)
    //{
    //  if (listViewMacro.SelectedItems.Count != 1)
    //    return;

    //  try
    //  {
    //    MPBlastZonePlugin.ProcessCommand(IrssUtils.Common.CmdPrefixMacro + listViewMacro.SelectedItems[0].Text, false);
    //  }
    //  catch (Exception ex)
    //  {
    //    Log.Error(ex);
    //    MessageBox.Show(this, ex.Message, "Test failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //  }
    //}

    //private void buttonSetCommand_Click(object sender, EventArgs e)
    //{
      //if (treeViewMenu.SelectedNode == null)
      //  return;

      //TreeNode parent = treeViewMenu.SelectedNode;
      //switch (treeViewMenu.SelectedNode.Level)
      //{
      //  case 0:
      //    return;

      //  case 1:
      //    parent = treeViewMenu.SelectedNode;
      //    break;

      //  case 2:
      //    parent = treeViewMenu.SelectedNode.Parent;
      //    break;
      //}

      //if (comboBoxCommands.SelectedIndex == -1)
      //  return;

      //string selected = comboBoxCommands.SelectedItem as string;
      //string newCommand = null;

      //if (selected.StartsWith(IrssUtils.Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
      //{
      //  BlastCommand blastCommand = new BlastCommand(
      //    MPBlastZonePlugin.BlastIR,
      //    IrssUtils.Common.FolderIRCommands,
      //    MPBlastZonePlugin.TransceiverInformation.Ports,
      //    selected.Substring(IrssUtils.Common.CmdPrefixBlast.Length));

      //  if (blastCommand.ShowDialog(this) == DialogResult.OK)
      //    newCommand = IrssUtils.Common.CmdPrefixBlast + blastCommand.CommandString;
      //}
      //else
      //{
      //  newCommand = selected;
      //}

      //parent.Nodes.Clear();

      //TreeNode newNode = new TreeNode(newCommand);
      //newNode.ForeColor = Color.Blue;

      //parent.Nodes.Add(newNode);
      //newNode.EnsureVisible();
    //}



    //private void buttonEditTree_Click(object sender, EventArgs e)
    //{
    //  if (treeViewMenu.SelectedNode == null)
    //    return;

    //  switch (treeViewMenu.SelectedNode.Level)
    //  {
    //    case 0:
    //    case 1:
    //      treeViewMenu.SelectedNode.BeginEdit();
    //      break;

    //    case 2:
    //      treeViewMenu_DoubleClick(null, null);
    //      break;
    //  }
    //}

    #endregion Buttons
  }
}