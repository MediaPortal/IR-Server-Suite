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
using System.IO;
using System.Windows.Forms;

namespace IrssUtils.Forms
{
  public partial class MacroEditor : Form
  {
    private const string NEW_MACRO_NAME = "New";

    private readonly string _macroFolder;
    private readonly VariableList _variables;
    private readonly ProcessCommandCallback _procCommand;
    private readonly string _oldMacroName;

    #region Constructor

    /// <summary>
    /// Creates a Macro Editor windows form.
    /// </summary>
    public MacroEditor(string macroFolder, VariableList variables, ProcessCommandCallback procCommand, string macroName = "")
    {
      if (String.IsNullOrEmpty(macroFolder))
        throw new ArgumentNullException("macroFolder");
      if (variables == null)
        throw new ArgumentNullException("variables");
      if (procCommand == null)
        throw new ArgumentNullException("procCommand");

      InitializeComponent();

      _macroFolder = macroFolder;
      _variables = variables;
      _procCommand = procCommand;

      if (string.IsNullOrEmpty(macroName))
      {
        textBoxName.Text = NEW_MACRO_NAME;
        return;
      }

      _oldMacroName = macroName;
      textBoxName.Text = macroName;

      string fileName = Path.Combine(_macroFolder, macroName + Common.FileExtensionMacro);
      string[] commands = IrssMacro.ReadFromFile(fileName);

      foreach (string s in commands)
      {
        BaseCommand bc = CommandCollection.CreateCommandFromCommandText(s);
        if (bc != null)
          listBoxMacro.Items.Add(bc);
        else
          listBoxMacro.Items.Add(s);
      }
    }

    #endregion Constructor

    #region Implementation

    /// <summary>
    /// Refreshes the macro command list.
    /// </summary>
    private void RefreshCommandList()
    {
      comboBoxCommands.Items.Clear();
      comboBoxCommands.Items.AddRange(CommandCollection.GetCommandTexts().ToArray());

      comboBoxCommands.Items.Add(Common.UITextPause);
      comboBoxCommands.Items.Add(Common.UITextDisplayMode);
      comboBoxCommands.Items.Add(Common.UITextLabel);
      comboBoxCommands.Items.Add(Common.UITextGotoLabel);
      comboBoxCommands.Items.Add(Common.UITextIf);
      comboBoxCommands.Items.Add(Common.UITextSetVar);
      comboBoxCommands.Items.Add(Common.UITextClearVars);
      comboBoxCommands.Items.Add(Common.UITextLoadVars);
      comboBoxCommands.Items.Add(Common.UITextSaveVars);

      List<MacroCommand> mcList = IrssMacro.GetMacroList(_macroFolder);
      if (mcList.Count > 0)
        comboBoxCommands.Items.AddRange(mcList.ToArray());

      string[] irList = Common.GetIRList(true);
      if (irList != null && irList.Length > 0)
        comboBoxCommands.Items.AddRange(irList);
    }

    private void MacroEditor_Load(object sender, EventArgs e)
    {
      RefreshCommandList();
    }

    private void buttonAddCommand_Click(object sender, EventArgs e)
    {
      if (comboBoxCommands.SelectedIndex == -1)
        return;

      string selected = comboBoxCommands.SelectedItem as string;
      if (selected == null) return;

      try
      {
        BaseCommand bc = CommandCollection.CreateCommand(selected);
        if (bc != null)
        {
          // open config form for configurable commands, before adding them
          if (bc is IConfigurableCommand && !(bc is MacroCommand))
          {
            CommandConfigForm ccf = new CommandConfigForm(bc as IConfigurableCommand);
            if (ccf.ShowDialog(this) == DialogResult.Cancel)
              return;

            bc = ccf.Command;
          }

          // add non-configurable command directly or configured command
          listBoxMacro.Items.Add(bc);

          return;
        }

        string newCommand = null;
        if (selected.Equals(Common.UITextIf, StringComparison.OrdinalIgnoreCase))
        {
          IfCommand ifCommand = new IfCommand();
          if (ifCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixIf + ifCommand.CommandString;
        }
        else if (selected.Equals(Common.UITextLabel, StringComparison.OrdinalIgnoreCase))
        {
          LabelNameDialog labelDialog = new LabelNameDialog();
          if (labelDialog.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixLabel + labelDialog.LabelName;
        }
        else if (selected.Equals(Common.UITextGotoLabel, StringComparison.OrdinalIgnoreCase))
        {
          LabelNameDialog labelDialog = new LabelNameDialog();
          if (labelDialog.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixGotoLabel + labelDialog.LabelName;
        }
        else if (selected.Equals(Common.UITextSetVar, StringComparison.OrdinalIgnoreCase))
        {
          SetVariableCommand setVariableCommand = new SetVariableCommand();
          if (setVariableCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixSetVar + setVariableCommand.CommandString;
        }
        else if (selected.Equals(Common.UITextClearVars, StringComparison.OrdinalIgnoreCase))
        {
          newCommand = Common.CmdPrefixClearVars;
        }
        else if (selected.Equals(Common.UITextLoadVars, StringComparison.OrdinalIgnoreCase))
        {
          VariablesFileDialog varsFileDialog = new VariablesFileDialog();
          if (varsFileDialog.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixLoadVars + varsFileDialog.FileName;
        }
        else if (selected.Equals(Common.UITextSaveVars, StringComparison.OrdinalIgnoreCase))
        {
          VariablesFileDialog varsFileDialog = new VariablesFileDialog();
          if (varsFileDialog.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixSaveVars + varsFileDialog.FileName;
        }
        else if (selected.Equals(Common.UITextPause, StringComparison.OrdinalIgnoreCase))
        {
          PauseTime pauseTime = new PauseTime();
          if (pauseTime.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixPause + pauseTime.Time;
        }
        else if (selected.Equals(Common.UITextDisplayMode, StringComparison.OrdinalIgnoreCase))
        {
          DisplayModeCommand displayModeCommand = new DisplayModeCommand();
          if (displayModeCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixDisplayMode + displayModeCommand.CommandString;
        }
#warning fixme BLAST
        //else if (selected.StartsWith(Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
        //{
        //  BlastCommand blastCommand = new BlastCommand(
        //    Program.BlastIR,
        //    Common.FolderIRCommands,
        //    Program.TransceiverInformation.Ports,
        //    selected.Substring(Common.CmdPrefixBlast.Length));

        //  if (blastCommand.ShowDialog(this) == DialogResult.OK)
        //    newCommand = Common.CmdPrefixBlast + blastCommand.CommandString;
        //}
        else
        {
          throw new CommandStructureException(String.Format("Unknown macro command ({0})", selected));
        }

        if (!String.IsNullOrEmpty(newCommand))
          listBoxMacro.Items.Add(newCommand);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(this, ex.Message, "Failed to add macro command", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void buttonMoveUp_Click(object sender, EventArgs e)
    {
      int selected = listBoxMacro.SelectedIndex;
      if (selected > 0)
      {
        object item = listBoxMacro.Items[selected];
        listBoxMacro.Items.RemoveAt(selected);
        listBoxMacro.Items.Insert(selected - 1, item);
        listBoxMacro.SelectedIndex = selected - 1;
      }
    }

    private void buttonMoveDown_Click(object sender, EventArgs e)
    {
      int selected = listBoxMacro.SelectedIndex;
      if (selected < listBoxMacro.Items.Count - 1)
      {
        object item = listBoxMacro.Items[selected];
        listBoxMacro.Items.RemoveAt(selected);
        listBoxMacro.Items.Insert(selected + 1, item);
        listBoxMacro.SelectedIndex = selected + 1;
      }
    }

    private void buttonRemove_Click(object sender, EventArgs e)
    {
      if (listBoxMacro.SelectedIndex != -1)
        listBoxMacro.Items.RemoveAt(listBoxMacro.SelectedIndex);
    }

    private string[] GetCommandsAsText()
    {
      List<string> list = new List<string>();
      foreach (var item in listBoxMacro.Items)
        list.Add(item.ToString());

      return list.ToArray();
    }

    private void buttonTest_Click(object sender, EventArgs e)
    {
      string name = textBoxName.Text.Trim();

      if (name.Length == 0)
      {
        MessageBox.Show(this, "You must supply a name for this Macro", "Name missing", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
        textBoxName.Focus();
        return;
      }

      if (!Common.IsValidFileName(name))
      {
        MessageBox.Show(this, "You must supply a valid name for this Macro", "Invalid name", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
        textBoxName.Focus();
        return;
      }

      try
      {
        string[] commands = GetCommandsAsText();

        string fileName = Path.Combine(_macroFolder, name + Common.FileExtensionMacro);

        IrssMacro.WriteToFile(fileName, commands);

        IrssMacro.ExecuteMacro(commands, _variables, _procCommand);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(this, ex.Message, "Test failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      string name = textBoxName.Text.Trim();

      if (name.Length == 0)
      {
        MessageBox.Show(this, "You must supply a name for this Macro", "Name missing", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
        textBoxName.Focus();
        return;
      }

      if (!Common.IsValidFileName(name))
      {
        MessageBox.Show(this, "You must supply a valid name for this Macro", "Invalid name", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
        textBoxName.Focus();
        return;
      }

      // set new filename
      string fileName = Path.Combine(_macroFolder, name + Common.FileExtensionMacro);
      // set old filename, if macro already existed
      string oldFileName = _oldMacroName == null ? string.Empty : Path.Combine(_macroFolder, _oldMacroName + Common.FileExtensionMacro);
      if (File.Exists(fileName) && !oldFileName.Equals(fileName))
        if (MessageBox.Show(this, "Macro '" + name + "' already exists. Do you want to overwrite it with the new one?",
          "File exists", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
          MessageBoxDefaultButton.Button2) == DialogResult.No)
          return;

      // define dialog result
      DialogResult deleteOldFile = DialogResult.None;
      // if new and old file differ ask to delete old filename
      if (File.Exists(oldFileName) && !oldFileName.Equals(fileName))
        deleteOldFile = MessageBox.Show(this,
                               String.Format(
                                 "Macro '{0}' has been renamed to '{1}'.{2}Do you want to delete the old macro file '{0}'?",
                                 _oldMacroName, name, Environment.NewLine), "Rename Macro",
                               MessageBoxButtons.YesNoCancel,
                               MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

      if (deleteOldFile == DialogResult.Cancel) return;

      try
      {
        string[] commands = GetCommandsAsText();

        IrssMacro.WriteToFile(fileName, commands);
        if (deleteOldFile == DialogResult.Yes)
          File.Delete(oldFileName);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(this, ex.Message, "Failed writing macro to file", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      DialogResult = DialogResult.OK;
      Close();
    }

    private void listBoxCommandSequence_DoubleClick(object sender, EventArgs e)
    {
      if (listBoxMacro.SelectedIndex == -1)
        return;

      try
      {
        BaseCommand bc = listBoxMacro.SelectedItem as BaseCommand;
        if (bc != null)
        {
          // open config form for configurable commands, before adding them
          if (bc is IConfigurableCommand)
          {
            CommandConfigForm ccf = new CommandConfigForm(bc as IConfigurableCommand);
            if (ccf.ShowDialog(this) == DialogResult.Cancel)
              return;

            bc = ccf.Command;
          }

          // add non-configurable command directly or configured command
          int index = listBoxMacro.SelectedIndex;
          listBoxMacro.Items.RemoveAt(index);
          listBoxMacro.Items.Insert(index, bc);
          listBoxMacro.SelectedIndex = index;

          return;
        }

        string selected = listBoxMacro.SelectedItem as string;
        string newCommand = null;

        if (selected.StartsWith(Common.CmdPrefixIf, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitIfCommand(selected.Substring(Common.CmdPrefixIf.Length));

          IfCommand ifCommand = new IfCommand(commands);
          if (ifCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixIf + ifCommand.CommandString;
        }
        else if (selected.StartsWith(Common.CmdPrefixLabel, StringComparison.OrdinalIgnoreCase))
        {
          LabelNameDialog labelDialog = new LabelNameDialog(selected.Substring(Common.CmdPrefixLabel.Length));
          if (labelDialog.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixLabel + labelDialog.LabelName;
        }
        else if (selected.StartsWith(Common.CmdPrefixGotoLabel, StringComparison.OrdinalIgnoreCase))
        {
          LabelNameDialog labelDialog = new LabelNameDialog(selected.Substring(Common.CmdPrefixGotoLabel.Length));
          if (labelDialog.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixGotoLabel + labelDialog.LabelName;
        }
        else if (selected.StartsWith(Common.CmdPrefixSetVar, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitSetVarCommand(selected.Substring(Common.CmdPrefixSetVar.Length));

          SetVariableCommand setVariableCommand = new SetVariableCommand(commands);
          if (setVariableCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixSetVar + setVariableCommand.CommandString;
        }
        else if (selected.StartsWith(Common.CmdPrefixLoadVars, StringComparison.OrdinalIgnoreCase))
        {
          VariablesFileDialog varsFileDialog =
            new VariablesFileDialog(selected.Substring(Common.CmdPrefixLoadVars.Length));
          if (varsFileDialog.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixLoadVars + varsFileDialog.FileName;
        }
        else if (selected.StartsWith(Common.CmdPrefixSaveVars, StringComparison.OrdinalIgnoreCase))
        {
          VariablesFileDialog varsFileDialog =
            new VariablesFileDialog(selected.Substring(Common.CmdPrefixSaveVars.Length));
          if (varsFileDialog.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixSaveVars + varsFileDialog.FileName;
        }
        else if (selected.StartsWith(Common.CmdPrefixPause, StringComparison.OrdinalIgnoreCase))
        {
          PauseTime pauseTime = new PauseTime(int.Parse(selected.Substring(Common.CmdPrefixPause.Length)));
          if (pauseTime.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixPause + pauseTime.Time;
        }
        else if (selected.StartsWith(Common.CmdPrefixDisplayMode, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitDisplayModeCommand(selected.Substring(Common.CmdPrefixDisplayMode.Length));

          DisplayModeCommand displayModeCommand = new DisplayModeCommand(commands);
          if (displayModeCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixDisplayMode + displayModeCommand.CommandString;
        }
#warning fixme BLAST
        //else if (selected.StartsWith(Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
        //{
        //  string[] commands = Common.SplitBlastCommand(selected.Substring(Common.CmdPrefixBlast.Length));

        //  BlastCommand blastCommand = new BlastCommand(
        //    Program.BlastIR,
        //    Common.FolderIRCommands,
        //    Program.TransceiverInformation.Ports,
        //    commands);

        //  if (blastCommand.ShowDialog(this) == DialogResult.OK)
        //    newCommand = Common.CmdPrefixBlast + blastCommand.CommandString;
        //}

        if (!String.IsNullOrEmpty(newCommand))
        {
          int index = listBoxMacro.SelectedIndex;
          listBoxMacro.Items.RemoveAt(index);
          listBoxMacro.Items.Insert(index, newCommand);
          listBoxMacro.SelectedIndex = index;
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(this, ex.Message, "Failed to edit macro item", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #endregion Implementation
  }
}