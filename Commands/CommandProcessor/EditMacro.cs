using System;
using System.Collections.Generic;
using System.ComponentModel;
#if TRACE
using System.Diagnostics;
#endif
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using IrssUtils;

namespace Commands
{

  /// <summary>
  /// Edit Macro form.
  /// </summary>
  public partial class EditMacro : Form
  {

    #region Special Categories

    const string CategoryIRCommands = "IR Commands";
    const string CategoryMacros     = "Macros";

    #endregion Special Categories

    #region Variables

    Processor _commandProcessor;

    string _macroFolder;
    string _fileName;

    Dictionary<string, Type> _commands;

    #endregion Variables

    #region Constructor

    /// <summary>
    /// Creates a Macro Editor windows form.
    /// </summary>
    /// <param name="commandProcessor">The command processor.</param>
    /// <param name="macroFolder">The macro folder.</param>
    /// <param name="categories">The command categories to include.</param>
    public EditMacro(Processor commandProcessor, string macroFolder, string[] categories)
    {
      if (commandProcessor == null)
        throw new ArgumentNullException("commandProcessor");

      if (String.IsNullOrEmpty(macroFolder))
        throw new ArgumentNullException("macroFolder");

      if (categories == null)
        throw new ArgumentNullException("categories");

      InitializeComponent();

      _commandProcessor   = commandProcessor;
      _macroFolder        = macroFolder;

      textBoxName.Text    = "New";
      textBoxName.Enabled = true;

      PopulateCommandList(categories);
    }

    /// <summary>
    /// Creates a Macro Editor windows form.
    /// </summary>
    /// <param name="commandProcessor">The command processor.</param>
    /// <param name="categories">The command categories to include.</param>
    /// <param name="fileName">Name of the macro file.</param>
    public EditMacro(Processor commandProcessor, string[] categories, string fileName)
    {
      if (commandProcessor == null)
        throw new ArgumentNullException("commandProcessor");

      if (categories == null)
        throw new ArgumentNullException("categories");

      if (String.IsNullOrEmpty(fileName))
        throw new ArgumentNullException("fileName");

      InitializeComponent();

      _commandProcessor   = commandProcessor;
      _fileName           = fileName;

      string macroName    = fileName;
      if (macroName.StartsWith(Common.FolderAppData, StringComparison.OrdinalIgnoreCase))
        macroName = fileName.Substring(Common.FolderAppData.Length);

      _macroFolder = fileName.Substring(0, fileName.Length - macroName.Length);

      textBoxName.Text    = macroName;
      textBoxName.Enabled = false;

      Macro macro = new Macro(fileName + Processor.FileExtensionMacro);
      foreach (Command command in macro.Commands)
      {
        ListViewItem item = new ListViewItem(command.GetUserDisplayText());
        item.Tag = command.ToString();
        listViewMacro.Items.Add(item);
      }

      PopulateCommandList(categories);
    }

    #endregion Constructor

    #region Implementation

    void PopulateCommandList(string[] categories)
    {
      _commands = new Dictionary<string, Type>();
      treeViewCommandList.Nodes.Clear();
      Dictionary<string, TreeNode> treeNodes = new Dictionary<string,TreeNode>(categories.Length);

      foreach (string category in categories)
      {
        TreeNode categoryNode = new TreeNode(category);
        //categoryNode.NodeFont = new Font(treeViewCommandList.Font, FontStyle.Underline);
        treeNodes.Add(category, categoryNode);
      }

      Type[] allCommands = Common.GetLibraryCommands();
      if (allCommands != null)
      {
        foreach (Type type in allCommands)
        {
          Command command = (Command)Activator.CreateInstance(type);

          string commandCategory = command.GetCategory();

          if (treeNodes.ContainsKey(commandCategory))
          {
            string uiText = command.GetUserInterfaceText();
            treeNodes[commandCategory].Nodes.Add(uiText);
            _commands.Add(uiText, type);
          }
        }
      }

      // Add built-in macro commands ...
      TreeNode macroCommands = new TreeNode(Processor.CategoryMacro);
      //macroCommands.NodeFont = new Font(treeViewCommandList.Font, FontStyle.Underline);

      Type[] specialCommands = Processor.GetSpecialCommands();
      foreach (Type type in specialCommands)
      {
        Command command = (Command)Activator.CreateInstance(type);

        //if (!command.GetCategory().Equals(Processor.CategoryHidden, StringComparison.OrdinalIgnoreCase))
          macroCommands.Nodes.Add(command.GetUserInterfaceText());

        _commands.Add(command.GetUserInterfaceText(), type);
      }
      treeNodes.Add(Processor.CategoryMacro, macroCommands);

      // Add list of existing IR Commands ...
      string[] irFiles = Processor.GetListIR();
      if (irFiles != null)
      {
        TreeNode irCommands = new TreeNode(CategoryIRCommands);
        //irCommands.NodeFont = new Font(treeViewCommandList.Font, FontStyle.Underline);

        foreach (string irFile in irFiles)
        {
          TreeNode newNode = new TreeNode(irFile);
          newNode.Tag = Common.FolderIRCommands + irFile;
          irCommands.Nodes.Add(newNode);
        }

        treeNodes.Add(irCommands.Text, irCommands);
      }

      // Add list of existing Macros ...
      string macroFolder = _macroFolder;
      if (String.IsNullOrEmpty(_macroFolder))
        macroFolder = Path.GetDirectoryName(_fileName);
      string[] macros = Processor.GetListMacro(macroFolder);
      if (macros != null)
      {
        TreeNode otherMacros = new TreeNode(CategoryMacros);
        //otherMacros.NodeFont = new Font(treeViewCommandList.Font, FontStyle.Underline);

        foreach (string macro in macros)
          otherMacros.Nodes.Add(macro);

        treeNodes.Add(otherMacros.Text, otherMacros);
      }

      // Put all commands into tree view ...
      foreach (TreeNode treeNode in treeNodes.Values)
        if (treeNode.Nodes.Count > 0)
          treeViewCommandList.Nodes.Add(treeNode);

      treeViewCommandList.SelectedNode = treeViewCommandList.Nodes[0];
      treeViewCommandList.SelectedNode.Expand();
    }

    private void treeViewCommandList_DoubleClick(object sender, EventArgs e)
    {
      if (treeViewCommandList.SelectedNode == null || treeViewCommandList.SelectedNode.Level == 0)
        return;

      try
      {
        string selected = treeViewCommandList.SelectedNode.Text as string;

        ListViewItem newCommand = new ListViewItem();
        Command command;

        if (treeViewCommandList.SelectedNode.Parent.Text.Equals(CategoryIRCommands, StringComparison.OrdinalIgnoreCase))
        {
          string selectedTag = treeViewCommandList.SelectedNode.Tag as string;

          command = new CommandBlastIR(new string[] { selectedTag, _commandProcessor.BlastIrPorts[0] });

          if (_commandProcessor.Edit(command, this))
          {
            newCommand.Text = command.GetUserDisplayText();
            newCommand.Tag = command.ToString();
            listViewMacro.Items.Add(newCommand);
          }
        }
        else if (treeViewCommandList.SelectedNode.Parent.Text.Equals(CategoryMacros, StringComparison.OrdinalIgnoreCase))
        {
          command = new CommandCallMacro(new string[] { selected });

          newCommand.Text = command.GetUserDisplayText();
          newCommand.Tag = command.ToString();
          listViewMacro.Items.Add(newCommand);
        }
        else if (_commands.ContainsKey(selected))
        {
          command = (Command)Activator.CreateInstance(_commands[selected]);

          if (_commandProcessor.Edit(command, this))
          {
            newCommand.Text = command.GetUserDisplayText();
            newCommand.Tag = command.ToString();
            listViewMacro.Items.Add(newCommand);
          }
        }
        else
        {
          throw new ApplicationException(String.Format("Unknown macro command ({0})", selected));
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(this, ex.Message, "Failed to add macro command", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void buttonMoveUp_Click(object sender, EventArgs e)
    {
      if (listViewMacro.SelectedItems.Count != 1)
        return;
      
      int selected = listViewMacro.SelectedIndices[0];
      if (selected > 0)
      {
        ListViewItem item = listViewMacro.Items[selected];
        listViewMacro.Items.RemoveAt(selected);
        listViewMacro.Items.Insert(selected - 1, item);
        item.Selected = true;
      }
    }
    private void buttonMoveDown_Click(object sender, EventArgs e)
    {
      if (listViewMacro.SelectedItems.Count != 1)
        return;

      int selected = listViewMacro.SelectedIndices[0];
      if (selected < listViewMacro.Items.Count - 1)
      {
        ListViewItem item = listViewMacro.Items[selected];
        listViewMacro.Items.RemoveAt(selected);
        listViewMacro.Items.Insert(selected + 1, item);
        item.Selected = true;
      }
    }

    private void buttonRemove_Click(object sender, EventArgs e)
    {
      if (listViewMacro.SelectedItems.Count == 1)
        listViewMacro.Items.RemoveAt(listViewMacro.SelectedIndices[0]);
    }

    private void buttonTest_Click(object sender, EventArgs e)
    {
      string name = textBoxName.Text.Trim();

      if (name.Length == 0)
      {
        MessageBox.Show(this, "You must supply a name for this Macro", "Name missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        textBoxName.Focus();
        return;
      }

      if (textBoxName.Enabled && !Common.IsValidFileName(name))
      {
        MessageBox.Show(this, "You must supply a valid name for this Macro", "Invalid name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        textBoxName.Focus();
        return;
      }

      try
      {
        string[] commands = new string[listViewMacro.Items.Count];
        
        Macro newMacro = new Macro();
        foreach (ListViewItem item in listViewMacro.Items)
        {
          string itemTag = item.Tag as string;
          Command command = Processor.CreateCommand(itemTag);
          newMacro.Commands.Add(command);
        }

        newMacro.Execute(_commandProcessor);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(this, ex.Message, "Test failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      string name = textBoxName.Text.Trim();

      if (name.Length == 0)
      {
        MessageBox.Show(this, "You must supply a name for this Macro", "Name missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        textBoxName.Focus();
        return;
      }

      if (textBoxName.Enabled && !Common.IsValidFileName(name))
      {
        MessageBox.Show(this, "You must supply a valid name for this Macro", "Invalid name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        textBoxName.Focus();
        return;
      }

      try
      {
        string[] commands = new string[listViewMacro.Items.Count];

        Macro newMacro = new Macro();
        foreach (ListViewItem item in listViewMacro.Items)
        {
          string itemTag = item.Tag as string;
          Command command = Processor.CreateCommand(itemTag);
          newMacro.Commands.Add(command);
        }

        newMacro.Save(_macroFolder + name + Processor.FileExtensionMacro);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(this, ex.Message, "Failed writing macro to file", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    #endregion Implementation

    private void listViewMacro_DoubleClick(object sender, EventArgs e)
    {
      try
      {
        ListViewItem selected = listViewMacro.SelectedItems[0];

        string selectedTag = selected.Tag as string;
        Command command = Processor.CreateCommand(selectedTag);

        if (_commandProcessor.Edit(command, this))
        {
          selected.Text = command.GetUserDisplayText();
          selected.Tag = command.ToString();
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(this, ex.Message, "Failed to edit macro command", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

  }

}
