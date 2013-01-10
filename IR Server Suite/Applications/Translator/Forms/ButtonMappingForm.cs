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
using System.Windows.Forms;
using IrssCommands;
using IrssUtils;
using IrssUtils.Forms;

namespace Translator.Forms
{
  internal partial class ButtonMappingForm : Form
  {
    #region Variables

    private readonly string _keyCode;
    private string _description;

    private LearnIR _learnIR;

    private Command _currentCommand;
    private BaseCommandConfig _currentCommandConfig;
    private readonly Dictionary<string, Command> _commandStorage = new Dictionary<string, Command>();
    private readonly Dictionary<string, Type> _uiTextCategoryCache = new Dictionary<string, Type>();

    #endregion Variables

    #region Properties

    internal string KeyCode
    {
      get { return _keyCode; }
    }

    internal string Description
    {
      get { return _description; }
    }

    internal string CommandString
    {
      get
      {
        if (CurrentCommand == null)
          return string.Empty;

        return CurrentCommand.UserDisplayText;
      }
    }

    internal Command CurrentCommand
    {
      get
      {
        if (_currentCommand != null)
        {
          _currentCommand.Parameters = _currentCommandConfig.Parameters;
          return _currentCommand;
        }

        return null;
      }
    }

    #endregion Properties

    #region Constructors

    public ButtonMappingForm(string keyCode, string description)
    {
      InitializeComponent();

      _keyCode = keyCode;
      _description = description;

    }

    public ButtonMappingForm(string keyCode, string description, Command command)
      : this(keyCode, description)
    {
      if (ReferenceEquals(command, null)) return;

      _currentCommand = command;
    }

    #endregion Constructors

    private void ButtonMappingForm_Load(object sender, EventArgs e)
    {
      textBoxKeyCode.Text = _keyCode;
      textBoxButtonDesc.Text = _description;

      // setup command list
      string[] categoryList = new string[] { Processor.CategorySpecial, Processor.CategoryGeneral, "Translator Commands" };
      PopulateCommandList(categoryList);

      if (_currentCommand != null)
      {
        _commandStorage[_currentCommand.UserInterfaceText] = _currentCommand;

        if (ReferenceEquals(comboBoxCommands.SelectedItem, _currentCommand.UserInterfaceText))
          comboBoxCommands_SelectedValueChanged(null, null);
        else
          comboBoxCommands.SelectedItem = _currentCommand.UserInterfaceText;
      }
    }

    #region Controls

    private void buttonOK_Click(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(_keyCode))
      {
        MessageBox.Show(this, "You must provide a valid button key code to create a button mapping", "KeyCode Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
        textBoxKeyCode.Focus();
        return;
      }

      DialogResult = DialogResult.OK;
      Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    private void buttonTest_Click(object sender, EventArgs e)
    {
      try
      {
        CurrentCommand.Execute(new VariableList());
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.ToString(), "Test failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void textBoxButtonDesc_TextChanged(object sender, EventArgs e)
    {
      _description = textBoxButtonDesc.Text;
    }

    #endregion Controls

    private void comboBoxCommands_SelectedValueChanged(object sender, EventArgs e)
    {
      string uiText = comboBoxCommands.SelectedItem as string;
      SetCurrentCommand(uiText);
    }

    private void SetCurrentCommand(string uiText)
    {
      // save current values in temp storage
      if (_currentCommand != null && _currentCommandConfig != null)
      {
        _currentCommand.Parameters = _currentCommandConfig.Parameters;
        _commandStorage[_currentCommand.UserInterfaceText] = _currentCommand;
      }

      panel1.Controls.Clear();
      //load if command is already cached
      if (_commandStorage.ContainsKey(uiText))
      {
        _currentCommand = _commandStorage[uiText];
      }
      else
      {
        Type newType = _uiTextCategoryCache[uiText];
        Command newCommand = (Command) Activator.CreateInstance(newType);
        _currentCommand = newCommand;
      }

      _currentCommandConfig = _currentCommand.GetEditControl();
      _currentCommandConfig.Dock = DockStyle.Fill;
      panel1.Controls.Add(_currentCommandConfig);

      buttonTest.Enabled = _currentCommand.IsTestAllowed;
    }

    private void PopulateCommandList(IList<string> categories)
    {
      comboBoxCommands.Items.Clear();
      _uiTextCategoryCache.Clear();
      //Dictionary<string, TreeNode> categoryNodes = new Dictionary<string, TreeNode>(categories.Length);
      //Dictionary<string, TreeNode> categoryNodes = new Dictionary<string, TreeNode>(categories.Length);

      //// Create requested categories ...
      //foreach (string category in categories)
      //{
      //  TreeNode categoryNode = new TreeNode(category);
      //  //categoryNode.NodeFont = new Font(treeViewCommandList.Font, FontStyle.Underline);
      //  categoryNodes.Add(category, categoryNode);
      //}

      List<Type> allCommands = new List<Type>();

      Type[] specialCommands = Processor.GetBuiltInCommands();
      allCommands.AddRange(specialCommands);

      Type[] libCommands = Processor.GetLibraryCommands();
      if (libCommands != null)
        allCommands.AddRange(libCommands);

      foreach (Type type in allCommands)
      {
        Command command = (Command) Activator.CreateInstance(type);
        
        string commandCategory = command.Category;
        string commandTitle = command.UserInterfaceText;
        //string key = String.Format("{0}: {1}", commandCategory, commandTitel);

        //if (categoryNodes.ContainsKey(commandCategory))
        if (categories.Contains(commandCategory))
        {

          comboBoxCommands.Items.Add(commandTitle);
          _uiTextCategoryCache[commandTitle] = type;

          //TreeNode newNode = new TreeNode(command.GetUserInterfaceText());
          //newNode.Tag = type;

          //categoryNodes[commandCategory].Nodes.Add(newNode);
        }
      }

      //// Add list of existing IR Commands ...
      //if (categoryNodes.ContainsKey(Processor.CategoryIRCommands))
      //{
      //  string[] irFiles = Processor.GetListIR();
      //  if (irFiles != null)
      //  {
      //    foreach (string irFile in irFiles)
      //    {
      //      TreeNode newNode = new TreeNode(Path.GetFileNameWithoutExtension(irFile));
      //      newNode.Tag = irFile;

      //      categoryNodes[Processor.CategoryIRCommands].Nodes.Add(newNode);
      //    }
      //  }
      //}

      //// Add list of existing Macros ...
      //if (categoryNodes.ContainsKey(Processor.CategoryMacros))
      //{
      //  string macroFolder = _macroFolder;
      //  if (String.IsNullOrEmpty(_macroFolder))
      //    macroFolder = Path.GetDirectoryName(_fileName);
      //  string[] macros = Processor.GetListMacro(macroFolder);
      //  if (macros != null)
      //  {
      //    foreach (string macro in macros)
      //    {
      //      TreeNode newNode = new TreeNode(Path.GetFileNameWithoutExtension(macro));
      //      newNode.Tag = macro;

      //      categoryNodes[Processor.CategoryMacros].Nodes.Add(newNode);
      //    }
      //  }
      //}

      // Put all commands into tree view ...
      //foreach (TreeNode treeNode in categoryNodes.Values)
      //  if (treeNode.Nodes.Count > 0)
      //    treeViewCommandList.Nodes.Add(treeNode);

      //treeViewCommandList.SelectedNode = treeViewCommandList.Nodes[0];
      if (comboBoxCommands.Items.Count > 0 && _currentCommand == null)
        comboBoxCommands.SelectedIndex = 0;
      //treeViewCommandList.SelectedNode.Expand();
    }
  }
}