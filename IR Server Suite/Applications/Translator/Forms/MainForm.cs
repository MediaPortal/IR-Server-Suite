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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml.Serialization;
using IrssComms;
using IrssUtils;
using IrssUtils.Forms;
using Translator.Properties;

namespace Translator
{
    internal partial class MainForm : Form
    {

        // --------------------------------------------------------------------------------------------------
        #region Attributes

        // Constant
        private const string SystemWide = "System Wide";
        private const string _listBoxHistoryEventsInitialMsg = "  Press a remote button to show its event...";
        private const string _mappingsGridViewCellInitialMsg = "This property should be assigned";
        private const string _mappingsGridViewCellDuplicateMsg = "Code mapped {0} times";

        // Variable
        private int _selectedProgram;
        private CommandManager commandManager;
        private bool _programsListViewRefreshAppend = false;
        private bool _mappingRowMode = true;
        private bool _mappingBypassSelectionChanged = false;

        // Properties
        private bool _edited;
        private bool Edited
        {
            get { return _edited; }
            set
            {
                _edited = value;
                this.saveToolStripMenuItem.Enabled = _edited;
                this.SaveToolStripButton.Enabled = _edited;
            }
        }

        private bool _connected;
        public bool Connected
        {
            get { return _connected; }
            set
            {
                _connected = value;
                if (_connected)
                {
                    this.SafeInvoke(() =>
                    {
                        this.labelConnect.Image = IrssUtils.Properties.Resources.Connect;
                        this.toolTip.SetToolTip(this.labelConnect, "connected to IR server");
                    });
                } 
                else
                {
                    this.SafeInvoke(() =>
                    {
                        this.labelConnect.Image = IrssUtils.Properties.Resources.Disconnect;
                        this.toolTip.SetToolTip(this.labelConnect, "disconnected");
                    });
                }
            }
        }


        #endregion Attributes

        // --------------------------------------------------------------------------------------------------
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            InitializeCommandManager();
            SetImages();

            RefreshProgramList();
            programsListView.Items[0].Selected = true;

            // refresh Event tab
            listBoxHistoryEvents.Items.Add(_listBoxHistoryEventsInitialMsg);
            listBoxTranslatorEvents.Items.Clear();
            foreach (string eventName in Enum.GetNames(typeof(MappingEvent)))
                if (!eventName.Equals("None", StringComparison.OrdinalIgnoreCase))
                    listBoxTranslatorEvents.Items.Add(eventName);

            listBoxTranslatorEvents.ClearSelected();

            // Refresh Mapping list
            RefreshMappingList();

            mappingsGridView.CurrentCell = null;
            mappingsGridView.ClearSelection();

            Edited = false;
            Connected = false;

        }

        private void SetImages()
        {
            // main menu
            this.newToolStripMenuItem.Image = IrssUtils.Properties.Resources.NewDocument;
            this.saveToolStripMenuItem.Image = IrssUtils.Properties.Resources.Save;
            this.openToolStripMenuItem.Image = IrssUtils.Properties.Resources.OpenDocument;
            this.importToolStripMenuItem.Image = IrssUtils.Properties.Resources.ImportDocument;
            this.exportToolStripMenuItem.Image = IrssUtils.Properties.Resources.ExportDocument;

            this.serverToolStripMenuItem.Image = IrssUtils.Properties.Resources.ChangeServer;
            this.advancedToolStripMenuItem.Image = IrssUtils.Properties.Resources.Advanced;

            this.contentsToolStripMenuItem.Image = IrssUtils.Properties.Resources.Help;
            this.aboutToolStripMenuItem.Image = IrssUtils.Properties.Resources.Info;

            // programs panel
            this.addProgramToolStripButton.Image = IrssUtils.Properties.Resources.Plus;
            this.addProgramToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.editProgramToolStripButton.Image = IrssUtils.Properties.Resources.Edit;
            this.editProgramToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.removeProgramToolStripButton.Image = IrssUtils.Properties.Resources.Delete;
            this.removeProgramToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;

            this.addProgramToolStripMenuItem.Image = IrssUtils.Properties.Resources.Plus;
            this.editProgramToolStripMenuItem.Image = IrssUtils.Properties.Resources.Edit;
            this.removeProgramToolStripMenuItem.Image = IrssUtils.Properties.Resources.Delete;

            // mappings panel
            this.buttonAddCommand.Image = IrssUtils.Properties.Resources.ScrollLeft;
            this.SaveToolStripButton.Image = IrssUtils.Properties.Resources.Save;

            this.newMappingToolStripButton.Image = IrssUtils.Properties.Resources.Plus;
            this.testMappingToolStripButton.Image = IrssUtils.Properties.Resources.Run;
            this.editMappingToolStripButton.Image = IrssUtils.Properties.Resources.Edit;
            this.deleteMappingToolStripButton.Image = IrssUtils.Properties.Resources.Delete;
            this.copyMappingDropDownButton.Image = IrssUtils.Properties.Resources.CopyDocument;
            
            this.newMappingToolStripMenuItem.Image = IrssUtils.Properties.Resources.Plus;
            this.testMappingToolStripMenuItem.Image = IrssUtils.Properties.Resources.Run;
            this.editMappingToolStripMenuItem.Image = IrssUtils.Properties.Resources.Edit;
            this.deleteMappingToolStripMenuItem.Image = IrssUtils.Properties.Resources.Delete;
            this.copyMappingsToolStripMenuItem.Image = IrssUtils.Properties.Resources.CopyDocument;

            // Events panel
            this.clearAllToolStripMenuItem.Image = IrssUtils.Properties.Resources.DeleteAll;

        }

        #endregion Constructor


        // --------------------------------------------------------------------------------------------------
        #region Main Form

        private void buttonAddCommand_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabPageEvents)
            {
                InsertEvent();
            }
            else if (tabControl.SelectedTab == tabPageCommands)
            {
                InsertCommand();
            }
        }

        private void SaveConfig()
        {
            Configuration.Save(Program.Config, Program.ConfigFile);
            Edited = false;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Edited == true)
            {
                var result = MessageBox.Show(this, "Configuration has changed.\n\rDo you want to save it before you quit ?", "Translator",
                                  MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes) SaveConfig();
                else if (result == DialogResult.Cancel || result == DialogResult.Abort) e.Cancel = true;
            }
        }

        private void MainForm_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            IrssHelp.Open(sender);
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl.SelectedTab.Name)
            {
                case "tabPageEvents":
                    break;

                case "tabPageCommands":
                    commandManager.IRServer = Program.TransceiverInformation;
                    break;
            }
        }

        #endregion Main Form


        // --------------------------------------------------------------------------------------------------
        #region Menus

        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

        private void NewConfig(object sender, EventArgs e)
        {
            if (
              MessageBox.Show(this, "Are you sure you want to start a new configuration?", "New Configuration",
                              MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            Program.Config = new Configuration();

            RefreshProgramList();
            RefreshMappingList();

            Edited = true;
        }

        private void OpenConfig(object sender, EventArgs e)
        {
            if (Edited == true)
            {
                var result = MessageBox.Show(this, "Configuration has changed.\n\rDo you want to save it before opening another one ?", "Translator",
                                  MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes) SaveConfig();
                else if (result == DialogResult.Cancel || result == DialogResult.Abort) return;
            }

            openFileDialog.Title = "Open settings file ...";

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                Configuration newConfig = Configuration.Load(openFileDialog.FileName);

                if (newConfig == null)
                    return;

                Program.Config = newConfig;

                RefreshProgramList();
                RefreshMappingList();

                Edited = false;

            }
        }

        private void ImportConfig(object sender, EventArgs e)
        {
            openFileDialog.Title = "Import settings ...";

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                Configuration newConfig = Configuration.Load(openFileDialog.FileName);
                if (newConfig == null)
                    return;

                Program.Config.Import(newConfig);

                RefreshProgramList();
                RefreshMappingList();

                Edited = true;
            }
        }

        private void ExportConfig(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                if (!Configuration.Save(Program.Config, saveFileDialog.FileName))
                    MessageBox.Show(this, "Failed to export settings to file", "Export failed", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
            }
        }

        private void SetServer(object sender, EventArgs e)
        {
            ServerAddress serverAddress = new ServerAddress(Program.Config.ServerHost);
            if (serverAddress.ShowDialog(this) == DialogResult.OK)
            {
                Program.StopClient();

                Program.Config.ServerHost = serverAddress.ServerHost;

                IPAddress serverIP = Network.GetIPFromName(Program.Config.ServerHost);
                IPEndPoint endPoint = new IPEndPoint(serverIP, Server.DefaultPort);

                Program.StartClient(endPoint);
                commandManager.IRServer = Program.TransceiverInformation;

                Edited = true;
            }
        }

        private void ShowAdvancedSettings(object sender, EventArgs e)
        {
            Advanced advanced = new Advanced();
            advanced.ProcessPriority = Program.Config.ProcessPriority;
            advanced.LogVerbosity = Program.Config.LogVerbosity;
            advanced.HideTrayIcon = Program.Config.HideTrayIcon;
            try
            {
                advanced.checkBoxAutoRun.Checked = SystemRegistry.GetAutoRun("Translator");
            }
            catch (Exception ex)
            {
                IrssLog.Error(ex);
                advanced.checkBoxAutoRun.Checked = false;
            }

            if (advanced.ShowDialog(this) == DialogResult.OK)
            {
                Edited = true;

                IrssLog.Info("Log Verbosity Level: " + advanced.LogVerbosity.ToString());
                Program.Config.LogVerbosity = advanced.LogVerbosity;
                IrssLog.LogLevel = (IrssLog.Level)Enum.Parse(typeof(IrssLog.Level), advanced.LogVerbosity);

                if (!advanced.ProcessPriority.Equals(Program.Config.ProcessPriority, StringComparison.OrdinalIgnoreCase))
                {
                    Program.Config.ProcessPriority = advanced.ProcessPriority;
                    Program.AdjustPriority(Program.Config.ProcessPriority);
                }

                Program.Config.HideTrayIcon = advanced.HideTrayIcon;
                Program.TrayIcon.Visible = !Program.Config.HideTrayIcon;

                if (advanced.checkBoxAutoRun.Checked)
                    SystemRegistry.SetAutoRun("Translator", Application.ExecutablePath);
                else
                    SystemRegistry.RemoveAutoRun("Translator");

            }
        }

        private void ExitApplication(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IrssHelp.Open(this);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private void tabControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift || e.Alt || e.Control) return;
            if (e.KeyCode == Keys.Enter)
            {
                buttonAddCommand_Click(null, null);
                e.Handled = true;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion Menus


        // --------------------------------------------------------------------------------------------------
        #region Programs Panel

        private void RefreshProgramList()
        {
            imageListPrograms.Images.Clear();
            imageListPrograms.Images.Add(IrssUtils.Properties.Resources.WinLogo);

            Icon large;
            Icon small;

            string folder = Environment.GetFolderPath(Environment.SpecialFolder.System);
            string file = Path.Combine(folder, "user32.dll");
            Win32.ExtractIcons(file, 1, out large, out small);
            imageListPrograms.Images.Add(large);


            //imageListPrograms.Images.Add(Properties.Resources.NoIcon);

            string wasSelected = string.Empty;
            if (programsListView.Items.Count > 0)
                wasSelected = programsListView.Items[_selectedProgram].Text;

            programsListView.Items.Clear();
            _selectedProgram = 0;

            // Add System-Wide ...
            ListViewItem newItem = new ListViewItem(SystemWide, 0);
            newItem.ToolTipText = "Defines mappings that effect the whole computer";
            programsListView.Items.Add(newItem);

            // Add other programs ...
            int imageIndex = 2;
            foreach (ProgramSettings progSettings in Program.Config.Programs)
            {
                Icon icon = null;

                if (!String.IsNullOrEmpty(progSettings.FileName))
                    icon = Win32.GetIconFromFile(progSettings.FileName);

                if (icon != null)
                {
                    imageListPrograms.Images.Add(icon);
                    newItem = new ListViewItem(progSettings.Name, imageIndex++);
                    newItem.ToolTipText = progSettings.FileName;
                }
                else
                {
                    newItem = new ListViewItem(progSettings.Name, 1);
                    newItem.ToolTipText = "Please check program file path";
                }

                programsListView.Items.Add(newItem);

                if (progSettings.Name.Equals(wasSelected))
                    newItem.Selected = true;
            }

            if (wasSelected.Equals(SystemWide) || programsListView.SelectedItems.Count == 0)
                programsListView.Items[0].Selected = true;


            // Update the "Copy to" items of the mappings
            copyMappingsToolStripMenuItem.DropDownItems.Clear();
            copyMappingDropDownButton.DropDownItems.Clear();

            copyMappingsToolStripMenuItem.DropDownItems.Add(SystemWide, IrssUtils.Properties.Resources.WinLogo,
                                                                CopyButtonMapping);
            copyMappingDropDownButton.DropDownItems.Add(SystemWide, IrssUtils.Properties.Resources.WinLogo,
                                                                CopyButtonMapping);

            foreach (ProgramSettings programSettings in Program.Config.Programs)
            {
                if (String.IsNullOrEmpty(programSettings.FileName)) continue;
                Icon icon = Win32.GetIconFromFile(programSettings.FileName);
                Image image = null;
                if (icon != null) image = icon.ToBitmap();

                copyMappingsToolStripMenuItem.DropDownItems.Add(programSettings.Name, image, CopyButtonMapping);
                copyMappingDropDownButton.DropDownItems.Add(programSettings.Name, image, CopyButtonMapping);

            }

            Program.UpdateNotifyMenu();
        }

        private void AddProgram(object sender, EventArgs e)
        {
            ProgramSettings progSettings = new ProgramSettings();

            if (EditProgram(progSettings))
            {
                Program.Config.Programs.Add(progSettings);

                RefreshProgramList();
            }
        }

        private void EditProgram(object sender, EventArgs e)
        {
            if (_selectedProgram == 0) return;
            string programName = programsListView.Items[_selectedProgram].Text;

            foreach (ProgramSettings progSettings in Program.Config.Programs)
            {
                if (progSettings.Name.Equals(programName))
                {
                    if (EditProgram(progSettings))
                    {
                        RefreshProgramList();
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
            }

        }

        private void DeleteProgram(object sender, EventArgs e)
        {
            if (programsListView.SelectedItems.Count == 0)
                return;

            string selectedItem = programsListView.SelectedItems[0].Text;

            string message = String.Format("Are you sure you want to remove all mappings for {0}?", selectedItem);
            string caption = String.Format("Remove {0}?", selectedItem);

            if (MessageBox.Show(this, message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (ProgramSettings progSettings in Program.Config.Programs)
                {
                    if (progSettings.Name.Equals(selectedItem))
                    {
                        Program.Config.Programs.Remove(progSettings);
                        break;
                    }
                }

                RefreshProgramList();
            }
        }

        private void programsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            // only update '_selectedProgram' related things if there is a new item selected
            // do nothing, if nothing is selected
            if (programsListView.SelectedItems.Count > 0 )
            {
                _selectedProgram = programsListView.SelectedIndices[0];
                RefreshMappingList(_programsListViewRefreshAppend);
                _programsListViewRefreshAppend = false;

            }
            
            listBoxTranslatorEvents.Enabled = programsListView.SelectedIndices.Count == 1 && _selectedProgram == 0;

            if (programsListView.SelectedIndices.Count != 1 || _selectedProgram == 0)
            {
                editProgramToolStripMenuItem.Text = "&Edit ...";
                removeProgramToolStripMenuItem.Text = "&Remove ...";

                editProgramToolStripButton.Enabled = false;
                editProgramToolStripMenuItem.Enabled = false;
                removeProgramToolStripButton.Enabled = false;
                removeProgramToolStripMenuItem.Enabled = false;
            }
            else
            {
                string program = programsListView.Items[_selectedProgram].Text;

                editProgramToolStripMenuItem.Text = String.Format("&Edit \"{0}\"", program);
                removeProgramToolStripMenuItem.Text = String.Format("&Remove \"{0}\"", program);

                editProgramToolStripButton.Enabled = true;
                editProgramToolStripMenuItem.Enabled = true;
                removeProgramToolStripButton.Enabled = true;
                removeProgramToolStripMenuItem.Enabled = true;
            }
        }

        #endregion Programs Panel


        // --------------------------------------------------------------------------------------------------
        #region Mappings Panel

        private void RefreshMappingList(bool append = false)
        {
            if (!append)
            {
                //mappingsGridView.Rows.Clear(); // doesn't work... don't know why...
                for (int i = mappingsGridView.Rows.Count; i > 0; i--) 
                    mappingsGridView.Rows.Remove(mappingsGridView.Rows[0]);
            }

            int idx = 0;

            // Populate the System/Program events
            List<ButtonMapping> currentMappings = GetCurrentButtonMappings();
            if (currentMappings == null) return;
            foreach (ButtonMapping map in currentMappings)
            {
                var item = new string[] { map.KeyCode, map.Command, map.Description };
                if (append) mappingsGridView.Rows.Insert(idx++, item); 
                else mappingsGridView.Rows.Add(item); 
            }

            // Populate the Translator events
            if (_selectedProgram == 0)
            {
                foreach (MappedEvent mappedEvent in Program.Config.Events)
                {
                    var item = new string[] { Enum.GetName(typeof(MappingEvent), mappedEvent.EventType), mappedEvent.Command, mappedEvent.Description };
                    if (append) mappingsGridView.Rows.Insert(idx++, item);
                    else mappingsGridView.Rows.Add(item);
                }
            }

            mappingsGridView_SelectedIndexChanged(null, null);
            SpotInvalidCellsInMappingList();
        }

        private void CommitMappingList()
        {
            List<ButtonMapping> currentMappings = GetCurrentButtonMappings();
            currentMappings.Clear();

            if (_selectedProgram == 0)
                Program.Config.Events.Clear();

            foreach (DataGridViewRow item in mappingsGridView.Rows)
            {
                string button = item.Cells[0].Value.ToString();
                string command = item.Cells[1].Value.ToString();
                string description = item.Cells[2].Value.ToString();

                if (Enum.IsDefined(typeof(MappingEvent), button)) {
                    // Translator events
                    try
                    {
                        MappingEvent eventType = (MappingEvent)Enum.Parse(typeof(MappingEvent), button, true);
                        if (_selectedProgram == 0) // safeguard
                            Program.Config.Events.Add(new MappedEvent(eventType, command, description));
                    }
                    catch { }
                }
                else 
                {
                    // System/program event
                    currentMappings.Add(new ButtonMapping(button, description, command));
                }
            }

            SpotInvalidCellsInMappingList();
            mappingsGridView_SelectedIndexChanged(null, null);
        }

        private void SpotInvalidCellsInMappingList()
        {
            foreach (DataGridViewRow row in mappingsGridView.Rows)
            {
                string code = row.Cells[0].Value.ToString();
                string command = row.Cells[1].Value.ToString();

                // Keycode
                if (String.IsNullOrEmpty(code))
                {
                    row.Cells[0].ErrorText = _mappingsGridViewCellInitialMsg;
                }
                else
                {
                    int count = 0;
                    foreach (DataGridViewRow srow in mappingsGridView.Rows)
                    {
                        count += srow.Cells[0].Value.ToString() == code ? 1 : 0;
                    }

                    if (count > 1)
                        row.Cells[0].ErrorText = String.Format(_mappingsGridViewCellDuplicateMsg, count);
                    else
                        row.Cells[0].ErrorText = "";
                }

                // Command
                if (String.IsNullOrEmpty(command))
                {
                    row.Cells[1].ErrorText = _mappingsGridViewCellInitialMsg;
                }
                else if (command.IndexOf(Common.CmdPrefixMacro) == 0)
                {
                    string[] macroList = IrssMacro.GetMacroList(Program.FolderMacros, true);
                    bool found = false;
                    if (macroList != null && macroList.Length > 0)
                    {
                        foreach (string macro in macroList)
                        {
                            if (macro == command)
                            {
                                found = true;
                                break;
                            }
                        }
                    }

                    if (found) row.Cells[1].ErrorText = ""; 
                    else row.Cells[1].ErrorText = "Macro not found";
                }
                else
                {
                    row.Cells[1].ErrorText = "";
                }
            }
        }

        private void MappingSetModeRow(bool rowMode)
        {
            _mappingRowMode = rowMode;
            if (rowMode)
            {
                if (mappingsGridView.SelectedCells.Count > 0)
                {
                    List<DataGridViewRow> selectedRows = new List<DataGridViewRow>();
                    foreach (DataGridViewCell cell in mappingsGridView.SelectedCells)
                    {
                        DataGridViewRow row = cell.OwningRow;
                        if (!selectedRows.Contains(row)) selectedRows.Add(row);
                    }

                    _mappingBypassSelectionChanged = true;
                    for (int i = selectedRows.Count-1; i >= 0; i-- )
                    {
                        _mappingBypassSelectionChanged = i > 0;
                        foreach (DataGridViewCell cell in selectedRows[i].Cells) cell.Selected = true;
                    }
                    _mappingBypassSelectionChanged = false;

                }
            } 
            else 
            {
                foreach ( DataGridViewCell cell in mappingsGridView.SelectedCells)
                    if (cell != mappingsGridView.CurrentCell)
                        cell.Selected = false;
            }
        }


        /// <summary>
        /// Insert a command to the selected mapping item.
        /// </summary>
        /// <param name="cmd">Command to insert. Default: fetch the selected command from the form.</param>
        private void InsertCommand(string cmd = null)
        {
            if (mappingsGridView.CurrentCell == null) return;
            int row = mappingsGridView.CurrentCell.RowIndex;
            if (row < 0) return;

            if (cmd == null) 
            {
                cmd = commandManager.CommandFetch();
                if (String.IsNullOrEmpty(cmd)) return;
            }

            mappingsGridView.Rows[row].Cells[1].Value = cmd;
            mappingsGridView.Focus();
            buttonAddCommand.Enabled = false;

            Edited = true;
            CommitMappingList();
        }

        /// <summary>
        /// Insert an event to the selected mapping item.
        /// </summary>
        /// <param name="cmd">Event to insert. Default: fetch the selected event from the form.</param>
        private void InsertEvent(string cmd = null)
        {
            if (mappingsGridView.CurrentCell == null) return;
            int row = mappingsGridView.CurrentCell.RowIndex;
            if (row < 0) return;

            if (cmd == null)
            {
                cmd = EventGet();
                if (String.IsNullOrEmpty(cmd)) return;
            }

            foreach (DataGridViewRow crow in mappingsGridView.Rows)
            {
                if (crow.Cells[0].Value.ToString() == cmd)
                {
                    // already exists
                    return;
                }
            }

            // avoid problem by deselecting events
            listBoxTranslatorEvents.SelectedIndex = -1;
            listBoxHistoryEvents.SelectedIndex = -1;

            mappingsGridView.Rows[row].Cells[0].Value = cmd;
            MappingSetModeRow(true);
            mappingsGridView.Focus();
            buttonAddCommand.Enabled = false;

            Edited = true;
            CommitMappingList();
        }

        private void NewButtonMapping(object sender, EventArgs e)
        {
            int row = mappingsGridView.Rows.Count;
            mappingsGridView.Rows.Add(new string[] { "", "", "" });
            mappingsGridView.CurrentCell = mappingsGridView.Rows[row].Cells[0];
            mappingsGridView.Rows[row].Cells[0].ErrorText = _mappingsGridViewCellInitialMsg;
            mappingsGridView.Rows[row].Cells[1].ErrorText = _mappingsGridViewCellInitialMsg;
            MappingSetModeRow(true);
            mappingsGridView_SelectedIndexChanged(null, null);
        }

        private void DeleteButtonMapping(object sender, EventArgs e)
        {
            if (!_mappingRowMode || mappingsGridView.SelectedCells.Count == 0) return;

            List<DataGridViewRow> selectedRows = new List<DataGridViewRow>();
            foreach (DataGridViewCell cell in mappingsGridView.SelectedCells)
            {
                DataGridViewRow row = cell.OwningRow;
                if (!selectedRows.Contains(row)) selectedRows.Add(row);
            }

            foreach (DataGridViewRow item in selectedRows)
            {
                mappingsGridView.Rows.Remove(item);
            }

            Edited = true;
            MappingSetModeRow(true);
            CommitMappingList();
        }

        private void TestButtonMapping(object sender, EventArgs e)
        {
            if (mappingsGridView.CurrentCell == null) return;
            int row = mappingsGridView.CurrentCell.RowIndex;
            mappingsGridView.CurrentCell = mappingsGridView.Rows[row].Cells[1];
            string cmd = mappingsGridView.Rows[row].Cells[1].Value.ToString();
            if (cmd == "") return;
            splitContainerMain.Enabled = false;
            Program.ProcessCommand(cmd, false);
            splitContainerMain.Enabled = true;
        }

        private void EditButtonMapping(object sender, EventArgs e)
        {
            if (mappingsGridView.CurrentCell == null) return;
            int row = mappingsGridView.CurrentCell.RowIndex;
            mappingsGridView.CurrentCell = mappingsGridView.Rows[row].Cells[1];
            mappingsGridView_DoubleClick(null, null);
        }

        private void CopyButtonMapping(object sender, EventArgs e)
        {
            ToolStripMenuItem destination = sender as ToolStripMenuItem;
            if (destination == null)  return;
            string programName = destination.Text;

            if (!_mappingRowMode || mappingsGridView.SelectedCells.Count == 0) return;

            // Find the destination program
            int idx = -1;
            for (int i = 0; i < programsListView.Items.Count; i++)
            {
                if (programsListView.Items[i].Text == programName)
                {
                    idx = i;
                    break;
                }
            }
            if (idx < 0) return;

            _programsListViewRefreshAppend = true;

            // Keep only selected rows in the mapping list
            for (int i = 0; i < mappingsGridView.Rows.Count; )
            {
                DataGridViewRow item = mappingsGridView.Rows[i];
                if (item.Cells[0].Selected) 
                {
                    i++;
                    // Erase Translator events
                    if (Enum.IsDefined(typeof(MappingEvent), item.Cells[0].Value.ToString()) && idx>0)
                        item.Cells[0].Value = "";
                }
                else
                {
                    mappingsGridView.Rows.Remove(item);
                }
            }

            // Switch to destination program and keep current mapping list
            if (idx == _selectedProgram) programsListView_SelectedIndexChanged(null, null);
            else programsListView.Items[idx].Selected = true;
            _programsListViewRefreshAppend = false;

            // Finalize edition
            Edited = true;
            CommitMappingList();
        }
        
        private void mappingsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_mappingBypassSelectionChanged) return;

            if (mappingsGridView.SelectedCells.Count == 0 || !_mappingRowMode)
            {
                // No row selected, Cell mode

                copyMappingsToolStripMenuItem.Enabled = copyMappingDropDownButton.Enabled = false;

                bool cmdSelected = false;
                if (mappingsGridView.CurrentCell == null )
                {
                    listBoxTranslatorEvents.Enabled = false;
                    commandManager.Enabled = false;
                    buttonAddCommand.Enabled = false;
                }
                else
                {
                    cmdSelected =  mappingsGridView.SelectedCells[0].ColumnIndex == 1 
                                && mappingsGridView.SelectedCells[0].Value.ToString() != "" ;
                }

                testMappingToolStripButton.Enabled = cmdSelected && _connected;
                testMappingToolStripMenuItem.Enabled = cmdSelected && _connected;
                editMappingToolStripButton.Enabled = cmdSelected;
                editMappingToolStripMenuItem.Enabled = cmdSelected;

                deleteMappingToolStripButton.Enabled = false;
                deleteMappingToolStripMenuItem.Enabled = false;

            }
            else
            {
                // In Row mode

                // Enforce row selection
                MappingSetModeRow(true);

                bool cmdCol =  mappingsGridView.SelectedCells.Count==3 
                            && mappingsGridView.SelectedCells[0].OwningRow.Cells[1].Value.ToString() != "";

                testMappingToolStripButton.Enabled = cmdCol && _connected;
                testMappingToolStripMenuItem.Enabled = cmdCol && _connected;
                editMappingToolStripButton.Enabled = cmdCol;
                editMappingToolStripMenuItem.Enabled = cmdCol;

                deleteMappingToolStripButton.Enabled = true;
                deleteMappingToolStripMenuItem.Enabled = true;

                copyMappingDropDownButton.Enabled = copyMappingsToolStripMenuItem.Enabled = true;

                listBoxTranslatorEvents.Enabled = true;
                commandManager.Enabled = true;
            }
        }

        private void mappingsGridView_DoubleClick(object sender, EventArgs e)
        {
            int column = mappingsGridView.CurrentCell.ColumnIndex;
            if (column < 2) 
            { 
                tabControl.SelectedIndex = column;
                if (column == 1)
                {
                    string cmd = commandManager.CommandEdit(mappingsGridView.CurrentCell.Value.ToString());
                    if (cmd != null)
                    {
                        mappingsGridView.CurrentCell.Value = cmd;
                        Edited = true;
                    }
                }
            }

            MappingSetModeRow(true);
            mappingsGridView.Focus();

            if (column == 2) mappingsGridView.BeginEdit(true);
        }

        private void mappingsGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            int row = e.RowIndex;
            int col = e.ColumnIndex;
            if (row >= 0)
            {
                DataGridViewCell clicked = mappingsGridView.Rows[row].Cells[col];
                if (!mappingsGridView.SelectedCells.Contains(clicked))
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Left && Control.ModifierKeys == Keys.Control)
                    {
                        if (!clicked.Selected) 
                            clicked.Selected = true;
                    }
                    else if (e.Button == System.Windows.Forms.MouseButtons.Left && Control.ModifierKeys == Keys.Shift)
                    {
                        // Define Range selection

                        int minRow = mappingsGridView.Rows.Count;
                        int maxRow = -1;
                        foreach (DataGridViewCell cell in mappingsGridView.SelectedCells)
                        {
                            int idx = cell.RowIndex;
                            if (idx < minRow) minRow = idx;
                            if (idx > maxRow) maxRow = idx;
                        }

                        int rangeStart;
                        int rangeEnd;
                        if (row < minRow)
                        {
                            rangeStart = row;
                            rangeEnd = minRow;
                        }
                        else if (row > maxRow)
                        {
                            rangeStart = maxRow;
                            rangeEnd = row;
                        }
                        else
                        {
                            rangeStart = minRow;
                            rangeEnd = row;
                        }

                        // Deselelect previous cells
                        _mappingBypassSelectionChanged = true;
                        DataGridViewSelectedCellCollection selectedCells = mappingsGridView.SelectedCells;
                        foreach (DataGridViewCell cell in selectedCells)
                        {
                            int idx = cell.RowIndex;
                            if (idx < rangeStart || idx > rangeEnd) cell.Selected = false;
                        }
                        _mappingBypassSelectionChanged = false;

                        // Select Cells in Range
                        for (int i = rangeStart; i <= rangeEnd; i++ )
                            mappingsGridView.Rows[i].Cells[0].Selected = true;
                    } 
                    else
                    {
                        mappingsGridView.CurrentCell = clicked;
                    }
                }
            }
            MappingSetModeRow(true);
        }

        private void mappingsGridView_Click(object sender, EventArgs e)
        {
            MappingSetModeRow(true);
        }

        private void mappingsGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            if (mappingsGridView.Rows[row].Cells[0].Value.ToString() != "" && mappingsGridView.Rows[row].Cells[1].Value.ToString() != "")
            {
                Edited = true;
                CommitMappingList();
            }

            Edited = true;
            CommitMappingList();
        }

        #endregion Mappings Panel


        // --------------------------------------------------------------------------------------------------
        #region Events Tab

        /// <summary>
        /// Add an event item to the Remote Events log.
        /// </summary>
        /// <param name="item">the event description, in a form of "sender : key"</param>
        public void EventAdd(string item)
        {
            if (item == null) return;
            this.SafeInvoke(() => {

                if (listBoxHistoryEvents.Items.Count == 1 && listBoxHistoryEvents.Items[0].ToString() == _listBoxHistoryEventsInitialMsg)
                {
                    listBoxHistoryEvents.Items.Clear();
                }

                listBoxHistoryEvents.TopIndex = listBoxHistoryEvents.Items.Add(item);
                listBoxHistoryEvents.Enabled = true;
            });
        }

        /// <summary>
        /// Add an event by its device-name and key-code to the Remote Events log.
        /// </summary>
        /// <param name="deviceName">REmote device name</param>
        /// <param name="keyCode">Key-code</param>
        public void EventAdd(string deviceName, string keyCode)
        {
            EventAdd(String.Format("{0} : {1}", deviceName, keyCode));
        }

        /// <summary>
        /// Get the selected event and returns its keycode.
        /// </summary>
        /// <returns>the keycode of the selected event, or null if no selection.</returns>
        public string EventGet()
        {
            string cmd = null;
            if (listBoxHistoryEvents.SelectedIndex >= 0)
            {
                string str = listBoxHistoryEvents.SelectedItem.ToString();
                int idx = str.IndexOf(" : ");
                if (idx>0) cmd = str.Substring(idx + 3);
            } 
            else if (listBoxTranslatorEvents.SelectedIndex >= 0)
            {
                cmd = listBoxTranslatorEvents.SelectedItem.ToString();
            }

            return cmd;
        }

        private void listBoxEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool selected = false;

            if (sender == listBoxHistoryEvents)
            {
                selected = listBoxHistoryEvents.SelectedIndex >= 0;
                if (selected)  listBoxTranslatorEvents.SelectedIndex = -1;
            }
            else if (sender == listBoxTranslatorEvents)
            {
                selected = listBoxTranslatorEvents.SelectedIndex >= 0;
                if (selected)  listBoxHistoryEvents.SelectedIndex = -1;
            }

            // Retrieve the associated mapping (if any) in the mapping panel
            DataGridViewRow exists = null;
            if (selected)
            {
                string keycode = EventGet();
                foreach (DataGridViewRow crow in mappingsGridView.Rows)
                {
                    if (crow.Cells[0].Value.ToString() == keycode)
                    {
                        exists = crow;
                        break;
                    }
                }
            }

            buttonAddCommand.Enabled = false;

            if (exists != null)
            {
                // Select the matching row in Mapping
                mappingsGridView.CurrentCell = exists.Cells[0];
                MappingSetModeRow(true);
            }
            else
            {
                // Select the current "Command" column in Mapping
                if (mappingsGridView.CurrentCell == null) return;
                int row = mappingsGridView.CurrentCell.RowIndex;
                if (row < 0) return;
                this.mappingsGridView.CurrentCell = mappingsGridView.Rows[row].Cells[0];

                if (selected)
                {
                    buttonAddCommand.Enabled = true;
                    this.toolTip.SetToolTip(this.buttonAddCommand, "Map this event (Enter)");
                    MappingSetModeRow(false);
                }
            }
        }

        private void listBoxEvents_DoubleClick(object sender, EventArgs e)
        {
            InsertEvent();
        }

        private void listBoxEvents_Enter(object sender, EventArgs e)
        {
            listBoxEvents_SelectedIndexChanged(sender, null);
        }

        private void listBoxEvents_Leave(object sender, EventArgs e)
        {
            if (!buttonAddCommand.Focused)
            {
                listBoxHistoryEvents.ClearSelected();
                listBoxTranslatorEvents.ClearSelected();
                MappingSetModeRow(true);
            }
        }

        private void listBoxEventsClearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBoxHistoryEvents.Items.Clear();
            listBoxHistoryEvents.Items.Add(_listBoxHistoryEventsInitialMsg);
            listBoxHistoryEvents.Enabled = false;
        }

        #endregion Events Tab


        // --------------------------------------------------------------------------------------------------
        #region Commands Tab

        /// <summary>
        /// Insert the CommandList control into the window
        /// </summary>
        private void InitializeCommandManager()
        {
            this.commandManager = new CommandManager(Program.TransceiverInformation, Program.BlastIR, Program.LearnIR, Program.ProcessCommand);
            this.commandManager.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.commandManager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commandManager.Name = "commandManager";
            this.commandManager.CommandGenerated += commandManager_CommandGenerated;
            this.commandManager.treeViewCommandList.AfterSelect += commandManager_AfterSelect;
            this.commandManager.treeViewCommandList.Enter += commandManager_Enter;
            this.commandManager.treeViewCommandList.Leave += commandManager_Leave;

            this.splitContainerMain.Panel2MinSize = 250; // Do it here to avoid bug in VS designer
            this.tabPageCommands.Controls.Add(this.commandManager);
        }

        private void commandManager_AfterSelect(object sender, TreeViewEventArgs e)
        {
            buttonAddCommand.Enabled = false;

            // Select "Command" column in Mapping
            int row = mappingsGridView.CurrentCell.RowIndex;
            if (row < 0) return;
            this.mappingsGridView.CurrentCell = mappingsGridView.Rows[row].Cells[1];

            if (String.IsNullOrEmpty(commandManager.SelectedCommand))
                MappingSetModeRow(true);
            else
            {
                buttonAddCommand.Enabled = true;
                this.toolTip.SetToolTip(this.buttonAddCommand, "Map this command (Enter)");
                MappingSetModeRow(false);
            }
            mappingsGridView_SelectedIndexChanged(null, null);
        }

        private void commandManager_Enter(object sender, EventArgs e)
        {
            commandManager_AfterSelect(null, null);
        }

        private void commandManager_Leave(object sender, EventArgs e)
        {
            MappingSetModeRow(true);
        }

        private void commandManager_CommandGenerated(object sender, CommandGeneratedEventArgs e)
        {

            if (e.test)
            {
                // Test the command
                // TODO
            }
            else
            {
                // Insert command in the list
                InsertCommand(e.command);
            }
        }

        #endregion Commands Tab


        // --------------------------------------------------------------------------------------------------
        #region Implementation

        private bool EditProgram(ProgramSettings progSettings)
        {
            EditProgramForm editProg = new EditProgramForm(progSettings);

            if (editProg.ShowDialog(this) == DialogResult.OK)
            {
                progSettings.Name = editProg.DisplayName;
                progSettings.FileName = editProg.Filename;
                progSettings.Folder = editProg.StartupFolder;
                progSettings.Arguments = editProg.Parameters;
                progSettings.WindowState = editProg.StartState;
                progSettings.UseShellExecute = editProg.UseShellExecute;
                progSettings.IgnoreSystemWide = editProg.IgnoreSystemWide;

                Program.UpdateNotifyMenu();
                return true;
            }

            return false;
        }

        private List<ButtonMapping> GetCurrentButtonMappings()
        {
            if (_selectedProgram == 0)
            {
                return Program.Config.SystemWideMappings;
            }
            else
            {
                string selectedItem = programsListView.Items[_selectedProgram].Text;

                foreach (ProgramSettings progSettings in Program.Config.Programs)
                    if (progSettings.Name.Equals(selectedItem))
                        return progSettings.ButtonMappings;
            }

            return null;
        }

        private AppProfile LoadDefaultSettings(string settingsFile)
        {
            try
            {
                XmlSerializer reader = new XmlSerializer(typeof(AppProfile));
                using (StreamReader file = new StreamReader(settingsFile))
                    return (AppProfile)reader.Deserialize(file);
            }
            catch (Exception ex)
            {
                IrssLog.Error(ex);
            }

            return null;
        }

        #endregion Implementation


    }
}