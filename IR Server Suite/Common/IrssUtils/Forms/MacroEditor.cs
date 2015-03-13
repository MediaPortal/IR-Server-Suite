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
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using IrssUtils;
using IrssUtils.Exceptions;
using IrssUtils.Forms;
using MSjogren.Samples.ShellLink;
using System.Reflection;

namespace IrssUtils.Forms
{
    public partial class MacroEditor : Form
    {

        // --------------------------------------------------------------------------------------------------
        #region attributes

        /// <summary>
        /// Return the name of the macro if successful
        /// </summary>
        public string MacroName
        {
            get {
                return textBoxName.Text;
            }
        }


        private string _name;
        private CommandManager commandList;
        private int _macroEditorClickedIndex = -1;
        private IRServerInfo _server;
        private BlastIrDelegate _blast;
        private LearnIrDelegate _learnIr;
        private ProcessCommandDelegate _ProcessCommand;
        private static List<string> _OpenInstances = new List<string>();
        private bool _editionEnabled;
        private bool _insertionEnabled;
        private bool _MacroNameValid;
        internal static readonly string FolderMacros = Path.Combine(Common.FolderAppData, "Translator\\Macro");

        #endregion attributes


        // --------------------------------------------------------------------------------------------------
        #region Constructor


        /// <summary>
        /// Creates a Macro Editor windows form.
        /// </summary>
        /// <param name="name">The name of an existing macro (default: new macro).</param>
        public MacroEditor(string name = "", IRServerInfo server = null, BlastIrDelegate blast = null, LearnIrDelegate learnIr=null, ProcessCommandDelegate processCommand=null, bool insertionEnabled = true)
        {
            if (name==null)  name = "";

            _insertionEnabled = insertionEnabled;
            _editionEnabled = !IsOpen(name);
            _OpenInstances.Add(name.ToLower());

            _name = name;
            _server = server;
            _blast = blast;
            _learnIr = learnIr;
            _ProcessCommand = processCommand;

            InitializeComponent();

            textBoxName.Text = name;
            buttonTest.Enabled = _ProcessCommand != null;
            buttonOK.Visible = _insertionEnabled || _editionEnabled;
            buttonOK.Enabled = _insertionEnabled;
            _MacroNameValid = name != "";
            buttonShortcut.Enabled = _MacroNameValid;
            if (_editionEnabled && !_insertionEnabled || _name == "")
            {  // Show save first
                buttonOK.Enabled = false;
                buttonOK.Text = "Save";
                this.buttonOK.Image = global::IrssUtils.Properties.Resources.Save;
            }
            else
            {
                buttonOK.Enabled = _insertionEnabled;
            }

            if (_editionEnabled)
            {
                InitializeCommandManager();
            } 
            else 
            {
                // Relayout for Read-only mode
                labelInvalid.Text = "Macro is already open for edition";
                labelInvalid.Show();
                textBoxName.Enabled = false;

                groupBoxCommandSequence.Controls.Remove(splitContainerMain);
                groupBoxCommandSequence.Controls.Add(panelActions);
                this.MinimumSize = new System.Drawing.Size(310, this.Height);
                this.Width = 350;
            }

            if (_name == "") return;

            try
            {
                string fileName = Path.Combine(FolderMacros, name + Common.FileExtensionMacro);
                string[] commands = IrssMacro.ReadFromFile(fileName);
                foreach (var cmd in commands)
                {
                    InsertCommand(cmd); 
                }
                
            }
            catch (Exception ex)
            {
                IrssLog.Error(ex);
                MessageBox.Show(this, ex.Message, "Failed to load macro: " + name + Common.FileExtensionMacro, MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxName.Text = "";
            }

            _editedMacro = false;
        }


        #endregion Constructor


        // --------------------------------------------------------------------------------------------------
        #region Edition

        /// <summary>
        /// Tell if an instance of macro-editor has open this macro.
        /// </summary>
        /// <param name="name">Name of the macro to check.</param>
        /// <returns>true if the macro is currently open in one of the macro-editor instances</returns>
        public bool IsOpen(string name)
        {
            if (name == "") return false;
            return _OpenInstances.Contains(name.ToLower());
        }

        private bool _editedMacro;
        private bool EditedMacro
        {
            get { return _editedMacro; }
            set
            {
                _editedMacro = value && _editionEnabled;
                if (buttonOK.Visible)
                {
                    if (_editedMacro)
                    {
                        buttonOK.Enabled = _MacroNameValid;
                        buttonOK.Text = "Save";
                        this.buttonOK.Image = global::IrssUtils.Properties.Resources.Save;
                        this.toolTipMacro.SetToolTip(this.buttonOK, "Save this macro (Ctrl + S)");
                        this.toolTipMacro.SetToolTip(this.buttonCancel, "Discard all changes and close editor (Esc)");
                    }
                    else if(_insertionEnabled)
                    {
                        buttonOK.Enabled = _MacroNameValid;
                        buttonOK.Text = "Insert";
                        this.buttonOK.Image = global::IrssUtils.Properties.Resources.ScrollLeft;
                        this.toolTipMacro.SetToolTip(this.buttonOK, "Insert this macro in the calling list and close editor (Enter)");
                        this.toolTipMacro.SetToolTip(this.buttonCancel, "Do not insert and close editor (Esc)");
                    }
                    else
                    {
                        buttonOK.Enabled = false;
                        buttonCancel.Text = "Close";
                        this.toolTipMacro.SetToolTip(this.buttonCancel, "Close macro-editor (Esc)");
                    }
                }
            }
        }

        #endregion Edition


        // --------------------------------------------------------------------------------------------------
        #region command formatting

        private const string _indent = "    ";

        private void InsertCommand(string command, int idx = -1)
        {
            if (command == null) return;
            EditedMacro = true;
            bool label = command.IndexOf(Common.CmdPrefixLabel) == 0;
            if (label) command = command.Substring(Common.CmdPrefixLabel.Length) + ":";
            else command = _indent + command;
            if(idx<0)  listBoxMacro.Items.Add(command);
            else listBoxMacro.Items.Insert(idx, command);
        }

        private string GetCommand(int idx)
        {
            if (idx < 0 || idx >= listBoxMacro.Items.Count) return null;
            string command = listBoxMacro.Items[idx].ToString();
            bool label = command.IndexOf(_indent) != 0;
            if (label) command = Common.CmdPrefixLabel + command.Remove(command.Length - 1);
            else  command = command.Substring(_indent.Length);
            return command;
        }

        #endregion command formatting


        // --------------------------------------------------------------------------------------------------
        #region Key mapping

        private void MacroEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift) return;
            if (e.Control)
            {
                if (e.KeyCode == Keys.S && EditedMacro)
                {
                    buttonOK_Click(null, null);
                    e.Handled = true;
                }
            }
            else if (e.Alt)
            {
                bool handled = true;
                switch (e.KeyCode)
                {
                    case Keys.Up: buttonMoveUp_Click(null, null); break;
                    case Keys.Down: buttonMoveDown_Click(null, null); break;
                    case Keys.Left: buttonAddCommand_Click(null, null); break;
                    case Keys.Right: buttonRemove_Click(null, null); break;
                    default: handled = false; break;
                }

                e.Handled = handled;
            }
            else if (e.KeyCode == Keys.F5)
            {
                buttonTest_Click(null, null);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter && !EditedMacro && _insertionEnabled)
            {
                buttonOK_Click(null, null);
                e.Handled = true;
            }
        }

        #endregion Key mapping


        // --------------------------------------------------------------------------------------------------
        #region commandManager callbacks

        /// <summary>
        /// Insert the CommandList control into the window
        /// </summary>
        private void InitializeCommandManager()
        {
            this.commandList = new CommandManager(_server, _blast, _learnIr, _ProcessCommand, true);
            this.commandList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commandList.Name = "commandList";
            this.commandList.CommandGenerated += commandList_CommandGenerated;
            this.commandList.treeViewCommandList.AfterSelect += commandList_AfterSelect;
            this.commandList.treeViewCommandList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.commandList_MouseDown);

            this.splitContainerMain.Panel2MinSize = 230; // Do it here to avoid bug in VS designer
            this.splitContainerMain.Panel2.Controls.Add(this.commandList);

        }

        private void commandList_CommandGenerated(object sender, CommandGeneratedEventArgs e)
        {

            if (e.test)
            {
                // Test the command
                // TODO
            }
            else
            {
                // Insert command in the list
                EditedMacro = true;
                if (listBoxMacro.SelectedIndex > -1)
                {
                    InsertCommand(e.command, listBoxMacro.SelectedIndex + 1);
                    listBoxMacro.SelectedIndex = listBoxMacro.SelectedIndex + 1;
                }
                else
                {
                    InsertCommand(e.command);
                    listBoxMacro.SelectedIndex = listBoxMacro.Items.Count - 1;
                }
            }
        }

        private void commandList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            buttonAddCommand.Enabled = !String.IsNullOrEmpty(commandList.SelectedCommand);
        }

        private void commandList_MouseDown(object sender, MouseEventArgs e)
        {
            if (_macroEditorClickedIndex >= 0)
            {
                listBoxMacro.DoDragDrop(listBoxMacro.Items[_macroEditorClickedIndex], DragDropEffects.None);
                _macroEditorClickedIndex = -1;
            }
        }

        #endregion commandManager callbacks


        // --------------------------------------------------------------------------------------------------
        #region macro Handling callbacks

        private void buttonAddCommand_Click(object sender, EventArgs e)
        {
            if (!_editionEnabled) return; 
            string newCommand = commandList.CommandFetch();
            if (String.IsNullOrEmpty(newCommand)) return;

            // Insert command in the list
            EditedMacro = true;
            if (listBoxMacro.SelectedIndex > -1)
            {
                InsertCommand(newCommand, listBoxMacro.SelectedIndex + 1);
                listBoxMacro.SelectedIndex = listBoxMacro.SelectedIndex + 1;
            }
            else
            {
                InsertCommand(newCommand);
                listBoxMacro.SelectedIndex = listBoxMacro.Items.Count - 1;
            }
        }

        private void buttonMoveUp_Click(object sender, EventArgs e)
        {
            if (!_editionEnabled) return;
            int selected = listBoxMacro.SelectedIndex;
            if (selected > 0)
            {
                EditedMacro = true;
                object item = listBoxMacro.Items[selected];
                listBoxMacro.Items.RemoveAt(selected);
                listBoxMacro.Items.Insert(selected - 1, item);
                listBoxMacro.SelectedIndex = selected - 1;
            }
        }

        private void buttonMoveDown_Click(object sender, EventArgs e)
        {
            if (!_editionEnabled) return;
            int selected = listBoxMacro.SelectedIndex;
            if (selected>=0 && selected < listBoxMacro.Items.Count - 1)
            {
                EditedMacro = true;
                object item = listBoxMacro.Items[selected];
                listBoxMacro.Items.RemoveAt(selected);
                listBoxMacro.Items.Insert(selected + 1, item);
                listBoxMacro.SelectedIndex = selected + 1;
            }
        }

        private void listBoxMacro_MouseDown(object sender, MouseEventArgs e)
        {
            if (!_editionEnabled) return;
            _macroEditorClickedIndex = listBoxMacro.IndexFromPoint(e.Location);

            if (_macroEditorClickedIndex < 0) 
                listBoxMacro.SelectedIndex = -1;
            else if (e.Button == MouseButtons.Right)
               listBoxMacro.SelectedIndex = _macroEditorClickedIndex;
        }

        private void listBoxMacro_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_editionEnabled) return;
            int idx = listBoxMacro.IndexFromPoint(e.Location);
            if ( idx != _macroEditorClickedIndex && _macroEditorClickedIndex >= 0 && idx >= 0
                 && (e.Button & MouseButtons.Left)==MouseButtons.Left )
            {
                // Start the drag-and-drop         
                DragDropEffects dropEffect = listBoxMacro.DoDragDrop(listBoxMacro.Items[idx], DragDropEffects.All);

            }
        }

        private void listBoxMacro_DragOver(object sender, DragEventArgs e)
        {
            if (!_editionEnabled) return;
            if (!e.Data.GetDataPresent(typeof(System.String)))
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            
            bool copy = (e.KeyState & 8) == 8;
            bool duplicate = copy && e.Effect != DragDropEffects.Copy;
            if (copy) e.Effect = DragDropEffects.Copy;
            else      e.Effect = DragDropEffects.Move;

            int idx = listBoxMacro.IndexFromPoint(listBoxMacro.PointToClient(new Point(e.X, e.Y)));
            if (idx != _macroEditorClickedIndex && _macroEditorClickedIndex >= 0 && idx >= 0)
            {
                EditedMacro = true;
                object item = listBoxMacro.Items[_macroEditorClickedIndex];
                if (duplicate)
                { // duplicate
                    idx++;
                }
                else
                { // move
                    listBoxMacro.Items.RemoveAt(_macroEditorClickedIndex);
                }
                listBoxMacro.Items.Insert(idx, item);
                listBoxMacro.SelectedIndex = idx;
                _macroEditorClickedIndex = idx;
            }

        }

        private void listBoxMacro_DragDrop(object sender, DragEventArgs e)
        {
            if (!_editionEnabled) return;
            listBoxMacro.DoDragDrop(listBoxMacro.Items[_macroEditorClickedIndex], DragDropEffects.None);
            _macroEditorClickedIndex = -1;
        }

        private void listBoxMacro_MouseUp(object sender, MouseEventArgs e)
        {
            _macroEditorClickedIndex = -1;
        }

        private void listBoxMacro_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx = listBoxMacro.SelectedIndex;
            bool selectedCommand = idx >= 0;
            buttonActionsTest.Enabled = _ProcessCommand != null && selectedCommand;
            buttonActionsEdit.Enabled = _editionEnabled && selectedCommand;
            buttonActionsRemove.Enabled = _editionEnabled && selectedCommand;
            buttonActionsCopy.Enabled = _editionEnabled && selectedCommand;
            buttonMoveUp.Enabled = _editionEnabled && idx > 0;
            buttonMoveDown.Enabled = _editionEnabled && selectedCommand && idx < listBoxMacro.Items.Count - 1;

            toolStripMenuTest.Enabled = _ProcessCommand != null && selectedCommand;
            toolStripMenuEdit.Enabled = _editionEnabled && selectedCommand;
            toolStripMenuCopy.Enabled = _editionEnabled && selectedCommand;
            toolStripMenuRemove.Enabled = _editionEnabled && selectedCommand;

        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            EditedMacro = true;
            _MacroNameValid = Common.IsValidFileName(textBoxName.Text);
            labelInvalid.Visible = textBoxName.Text.Length > 0 && !_MacroNameValid;
            buttonOK.Enabled = _MacroNameValid;
            buttonShortcut.Enabled = _MacroNameValid;
        }

        private void buttonTestCommand_Click(object sender, EventArgs e)
        {
            int selected = listBoxMacro.SelectedIndex;
            if (selected >= 0)
            {
                EditedMacro = true;
                string item = GetCommand(selected);
                if (_ProcessCommand != null) _ProcessCommand(item, false);
            }
        }
        private void toolStripMenuTest_Click(object sender, EventArgs e)
        {
            buttonTestCommand_Click(null, null);
        }

        private void buttonActionsEdit_Click(object sender, EventArgs e)
        {
            if (!_editionEnabled) return;
            string selected = GetCommand(listBoxMacro.SelectedIndex);
            if (selected == null) return;

            string newCommand = commandList.CommandEdit(selected);
            if (String.IsNullOrEmpty(newCommand)) return;

            // Replace command in the list
            EditedMacro = true;
            int index = listBoxMacro.SelectedIndex;
            listBoxMacro.Items.RemoveAt(index);
            InsertCommand(newCommand, index);
            listBoxMacro.SelectedIndex = index;
        }
        private void toolStripMenuEdit_Click(object sender, EventArgs e)
        {
            buttonActionsEdit_Click(null, null);
        }


        private void buttonCopyCommand_Click(object sender, EventArgs e)
        {
            if (!_editionEnabled) return;
            int selected = listBoxMacro.SelectedIndex;
            if (selected >= 0)
            {
                EditedMacro = true;
                string item = listBoxMacro.Items[selected].ToString();
                listBoxMacro.Items.Insert(selected + 1, item);
                listBoxMacro.SelectedIndex = selected + 1;
            }
        }
        private void toolStripMenuCopy_Click(object sender, EventArgs e)
        {
            buttonCopyCommand_Click(null, null);
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (!_editionEnabled) return;
            int selected = listBoxMacro.SelectedIndex;
            if (selected >= 0)
            {
                EditedMacro = true;
                listBoxMacro.Items.RemoveAt(selected);
                if (listBoxMacro.Items.Count > selected) listBoxMacro.SelectedIndex = selected;
                else if (listBoxMacro.Items.Count > 0) listBoxMacro.SelectedIndex = selected - 1;
            }
        }
        private void toolStripMenuRemove_Click(object sender, EventArgs e)
        {
            buttonRemove_Click(null, null);
        }

        #endregion macro Handling callbacks


        // --------------------------------------------------------------------------------------------------
        #region Form callbacks

        private void buttonTest_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text.Trim();

            try
            {
                string[] commands = new string[listBoxMacro.Items.Count];
                int index = 0;
                for(int idx=0; idx<listBoxMacro.Items.Count; idx++)
                    commands[index++] = GetCommand(idx);

                string fileName = Path.Combine(FolderMacros, Common.FileExtensionMacro);

                IrssMacro.WriteToFile(fileName, commands);
                this.Enabled = false;
                if (_ProcessCommand != null) _ProcessCommand(Common.CmdPrefixMacro, false);
                File.Delete(fileName);

            }
            catch (Exception ex)
            {
                IrssLog.Error(ex);
                MessageBox.Show(this, ex.Message, "Test failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Enabled = true;

        }

        private void MacroEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            _OpenInstances.Remove(_name.ToLower());
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (!_insertionEnabled && !_editionEnabled) return;

            if (!EditedMacro)
            {
                // Insert
                DialogResult = DialogResult.OK;
                Close();
                return;
            }
            
            // Save

            string name = textBoxName.Text.Trim();
            string fname = name + Common.FileExtensionMacro;

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

            string fileName = Path.Combine(FolderMacros, fname);
            if (name != _name && File.Exists(fileName))
            {
                DialogResult result = MessageBox.Show( this, "This macro already exists.\r\nDo you want to overwrite " + fname + " ?"
                                                     , "Overwrite file", MessageBoxButtons.YesNo
                                                     , MessageBoxIcon.Exclamation );

                if (result != System.Windows.Forms.DialogResult.Yes)
                {
                    textBoxName.Focus();
                    return;
                }
            }


            try
            {
                string[] commands = new string[listBoxMacro.Items.Count];
                int index = 0;
                for (int idx = 0; idx < listBoxMacro.Items.Count; idx++)
                    commands[index++] = GetCommand(idx);

                IrssMacro.WriteToFile(fileName, commands);
                EditedMacro = false;

                // Renamed macro: remove previous file
                if (name != _name && _name != "")
                {
                    string oldName = Path.Combine(FolderMacros, _name + Common.FileExtensionMacro);
                    File.Delete(oldName);
                }
            }
            catch (Exception ex)
            {
                IrssLog.Error(ex);
                MessageBox.Show(this, ex.Message, "Failed writing macro to file: " + fname, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void buttonShortcut_Click(object sender, EventArgs e)
        {
            if (!_MacroNameValid)  return;

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string macroName = MacroName;
            string shortcutPath = Path.Combine(desktopPath, String.Format("Macro - {0}.lnk", macroName));

            ShellShortcut shortcut = new ShellShortcut(shortcutPath);

            string translatorFolder = SystemRegistry.GetInstallFolder();
            if (translatorFolder==null)
                translatorFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            shortcut.Arguments = String.Format("-MACRO \"{0}\"", macroName);
            shortcut.Description = "Launch Macro: " + macroName;
            shortcut.Path = Path.Combine(translatorFolder, "Translator.exe");
            shortcut.WorkingDirectory = translatorFolder;
            //shortcut.WindowStyle = ProcessWindowStyle.Normal;

            shortcut.Save();

        }

        private void MacroEditor_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            IrssHelp.Open(this);
            hlpevent.Handled = true;
        }


        #endregion Form callbacks

    }

}