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
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;
using IrssCommands;
using IrssCommands.Forms;
using IrssComms;
using IrssUtils;
using IrssUtils.Forms;
using MSjogren.Samples.ShellLink;
using Translator.Forms;

namespace Translator
{
  internal partial class MainForm : Form
  {
    #region Constants

    private const string SystemWide = "System Wide";

    private string[] _eventCategories = new string[] {Processor.CategoryGeneral, Processor.CategorySpecial};

    private string[] _macroCategories = new string[]
        {
          Processor.CategoryGeneral, Processor.CategoryIRCommands, Processor.CategoryMacros,
          Processor.CategoryControl, Processor.CategoryMaths, Processor.CategoryStack, Processor.CategoryString, Processor.CategoryVariable
        };

    #endregion Constants

    #region Variables

    private LearnIR _learnIR;

    private int _selectedProgram;
    private readonly Dictionary<string, Type> _uiTextCategoryCache = new Dictionary<string, Type>();

    internal MacroPanel _macroPanel;
    internal IRCommandPanel _irPanel;

    #endregion Variables

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="MainForm"/> class.
    /// </summary>
    public MainForm()
    {
      InitializeComponent();

      // add macro panel
      _macroPanel = new MacroPanel(Program.CommandProcessor, Program.FolderMacros, _macroCategories);
      // todo: fix shortcut creation / launch
      //_macroPanel.DoCreateShortcutForMacro += CreateShortcutForMacro;
      _macroPanel.Dock = DockStyle.Fill;
      tabPageMacros.Controls.Add(_macroPanel);

      // add ir command panel
      _irPanel = new IRCommandPanel(NewIRCommand, EditIRCommand);
      _irPanel.Dock = DockStyle.Fill;
      tabPageIR.Controls.Add(_irPanel);

      SetImages();

      RefreshProgramList();
      programsListView.Items[0].Selected = true;

      RefreshMappingList();
      RefreshEventList();
      PopulateCommandList(_eventCategories);

      try
      {
        checkBoxAutoRun.Checked = SystemRegistry.GetAutoRun("Translator");
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);

        checkBoxAutoRun.Checked = false;
      }
    }

    #endregion Constructor

    #region Implementation

    #region Main Form

    private void SetImages()
    {
      // main menu
      this.newToolStripMenuItem.Image = IrssUtils.Properties.Resources.NewDocument;
      this.openToolStripMenuItem.Image = IrssUtils.Properties.Resources.OpenDocument;
      this.importToolStripMenuItem.Image = IrssUtils.Properties.Resources.ImportDocument;
      this.exportToolStripMenuItem.Image = IrssUtils.Properties.Resources.ExportDocument;

      this.serverToolStripMenuItem.Image = IrssUtils.Properties.Resources.ChangeServer;

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
      this.newMappingToolStripButton.Image = IrssUtils.Properties.Resources.Plus;
      this.editMappingToolStripButton.Image = IrssUtils.Properties.Resources.Edit;
      this.deleteMappingToolStripButton.Image = IrssUtils.Properties.Resources.Delete;
      this.clearMappingsToolStripButton.Image = IrssUtils.Properties.Resources.DeleteAll;
      this.remapToolStripButton.Image = IrssUtils.Properties.Resources.Remap;

      this.newMappingToolStripMenuItem.Image = IrssUtils.Properties.Resources.Plus;
      this.editMappingToolStripMenuItem.Image = IrssUtils.Properties.Resources.Edit;
      this.deleteMappingToolStripMenuItem.Image = IrssUtils.Properties.Resources.Delete;
      this.clearMappingsToolStripMenuItem.Image = IrssUtils.Properties.Resources.DeleteAll;
      this.remapToolStripMenuItem.Image = IrssUtils.Properties.Resources.Remap;
      this.copyMappingsFromToolStripMenuItem.Image = IrssUtils.Properties.Resources.MoveRight;

      // evens tab
      this.addEventToolStripMenuItem.Image = IrssUtils.Properties.Resources.Plus;
      this.removeEventToolStripMenuItem.Image = IrssUtils.Properties.Resources.Delete;
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      _macroPanel.RefreshList();
      _irPanel.RefreshList();

      Program.HandleMessage += ReceivedMessage;
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      Program.HandleMessage -= ReceivedMessage;

      CommitEvents();

      Configuration.Save(Program.Config, Program.ConfigFile);
    }

    private void MainForm_HelpRequested(object sender, HelpEventArgs hlpevent)
    {
      contentsToolStripMenuItem_Click(null, null);
      hlpevent.Handled = true;
    }


    private void buttonOK_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
    {
      switch (tabControl.SelectedTab.Name)
      {
        case "tabPageIRCodes":
          _irPanel.RefreshList();
          break;

        case "tabPageMacros":
          _macroPanel.RefreshList();
          break;

        case "tabPageEvents":
          RefreshEventList();
          break;

        case "tabPagePrograms":
          break;
      }
    }

    private void checkBoxAutoRun_CheckedChanged(object sender, EventArgs e)
    {
      if (checkBoxAutoRun.Checked)
        SystemRegistry.SetAutoRun("Translator", Application.ExecutablePath);
      else
        SystemRegistry.RemoveAutoRun("Translator");
    }


    #region Menus

    private void NewConfig(object sender, EventArgs e)
    {
      if (
        MessageBox.Show(this, "Are you sure you want to start a new configuration?", "New Configuration",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
        return;

      Program.Config = new Configuration();

      RefreshProgramList();
      RefreshMappingList();
      RefreshEventList();
    }

    private void OpenConfig(object sender, EventArgs e)
    {
      openFileDialog.Title = "Open settings file ...";

      if (openFileDialog.ShowDialog(this) == DialogResult.OK)
      {
        Configuration newConfig = Configuration.Load(openFileDialog.FileName);

        if (newConfig == null)
          return;

        Program.Config = newConfig;

        RefreshProgramList();
        RefreshMappingList();
        RefreshEventList();
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
        RefreshEventList();
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
      }
    }

    private void ShowAdvancedSettings(object sender, EventArgs e)
    {
      Advanced advanced = new Advanced();
      advanced.ProcessPriority = Program.Config.ProcessPriority;
      advanced.HideTrayIcon = Program.Config.HideTrayIcon;

      if (advanced.ShowDialog(this) == DialogResult.OK)
      {
        if (!advanced.ProcessPriority.Equals(Program.Config.ProcessPriority, StringComparison.OrdinalIgnoreCase))
        {
          Program.Config.ProcessPriority = advanced.ProcessPriority;
          Program.AdjustPriority(Program.Config.ProcessPriority);
        }

        Program.Config.HideTrayIcon = advanced.HideTrayIcon;
        Program.TrayIcon.Visible = !Program.Config.HideTrayIcon;
      }
    }


    private void CloseMainForm(object sender, EventArgs e)
    {
      this.Close();
    }

    private void ExitApplication(object sender, EventArgs e)
    {
      Application.Exit();
    }


    private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      IrssHelp.Open(this.GetType().FullName + "_" + tabControl.SelectedTab.Name);
    }

    private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      new AboutForm().ShowDialog();
    }

    private void removeEventToolStripMenuItem_Click(object sender, EventArgs e)
    {
      foreach (ListViewItem listViewItem in listViewEventMap.SelectedItems)
        listViewEventMap.Items.Remove(listViewItem);
    }

    #endregion Menus

    #endregion Main Form

    #region Programs Panel

    private void RefreshProgramList()
    {
      imageListPrograms.Images.Clear();
      imageListPrograms.Images.Add(IrssUtils.Properties.Resources.WinLogo);
      imageListPrograms.Images.Add(Win32.ExclamationMark);

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

      Program.UpdateNotifyMenu();
    }


    private void AddProgram()
    {
      ProgramSettings progSettings = new ProgramSettings();

      if (EditProgram(progSettings))
      {
        Program.Config.Programs.Add(progSettings);

        RefreshProgramList();

        // TODO: Detect and offer preconfigured settings ...
        /*
        string programFile = Path.GetFileName(progSettings.FileName);
        string settingsFile = Path.Combine(Program.FolderDefaultSettings, programFile + ".xml");
        if (File.Exists(settingsFile))
        {
          if (MessageBox.Show(this, String.Format("Do you want to use the default settings for {0} ({1})?", progSettings.Name, programFile), "Default settings available", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          {
            AppProfile appProfile = LoadDefaultSettings(settingsFile);

            if (appProfile != null)
            {
              progSettings.ButtonMappings.AddRange(appProfile.ButtonMappings);

              RefreshButtonList();
            }
          }
        }
        */
      }
    }

    private void AddProgram(object sender, EventArgs e)
    {
      AddProgram();
    }

    private bool EditProgram()
    {
      if (_selectedProgram == 0)
        return false;
      //if (programsListView.SelectedItems.Count == 0)
      //  return;

      string selectedItem = programsListView.Items[_selectedProgram].Text;
      //string selectedItem = programsListView.SelectedItems[0].Text;

      return EditProgram(selectedItem);
      //EditProgram(selectedItem);
    }

    private void EditProgram(object sender, EventArgs e)
    {
      EditProgram();
    }

    private bool EditProgram(string programName)
    {
      foreach (ProgramSettings progSettings in Program.Config.Programs)
      {
        if (progSettings.Name.Equals(programName))
        {
          if (EditProgram(progSettings))
          {
            RefreshProgramList();
            return true;
          }
          else
          {
            return false;
          }
        }
      }

      return false;
    }

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
      if (programsListView.SelectedItems.Count > 0)
      {
        _selectedProgram = programsListView.SelectedIndices[0];
        RefreshMappingList();
      }

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

    #region Mappings Panel

    private void RefreshMappingList()
    {
      mappingsListView.Items.Clear();

      List<ButtonMapping> currentMappings = GetCurrentButtonMappings();
      if (currentMappings == null)
        return;

      foreach (ButtonMapping map in currentMappings)
      {
        if (map.IsCommandAvailable)
          mappingsListView.Items.Add(
            new ListViewItem(new string[] { map.KeyCode, map.Description, map.GetCommandDisplayText() }));
        else
          mappingsListView.Items.Add(
            new ListViewItem(new string[] { map.KeyCode, map.Description, map.GetCommandDisplayText() })).ToolTipText = "Command was not found in CommandLibrary.";
      }

      mappingsListView_SelectedIndexChanged(null, null);
    }

    private void NewButtonMapping()
    {
      List<ButtonMapping> currentMappings = GetCurrentButtonMappings();
      if (currentMappings == null)
        return;

      GetKeyCodeForm getKeyCode = new GetKeyCodeForm();
      getKeyCode.ShowDialog(this);

      string keyCode = getKeyCode.KeyCode;
      string deviceName = getKeyCode.DeviceName;

      if (String.IsNullOrEmpty(keyCode))
        return;

      ButtonMappingForm map = null;
      ButtonMapping existing = null;

      foreach (ButtonMapping test in currentMappings)
      {
        if (keyCode.Equals(test.KeyCode, StringComparison.Ordinal))
        {
          existing = test;
          if (test.Command == null)
          {
            DialogResult dr = MessageBox.Show(this,
                                              "The command " + test.GetCommandDisplayText() + " was not found in CommandLibrary and can not be modified." +
                                              Environment.NewLine + Environment.NewLine +
                                              "Do you want to replace the command with a another one?",
                                              "Command not available", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                              MessageBoxDefaultButton.Button2);

            if (dr == DialogResult.Yes)
              map = new ButtonMappingForm(test.KeyCode, test.Description);
            else
              return;
          }
          else
            map = new ButtonMappingForm(test.KeyCode, test.Description, test.Command);
          break;
        }
      }

      if (map == null)
      {
        string description = String.Empty;

        // TODO: Implement abstract remote button descriptions.
        if (deviceName.Equals("Abstract", StringComparison.OrdinalIgnoreCase))
        {
          switch (keyCode.ToLowerInvariant())
          {
            case "Red":
              description = "Red teletext button";
              break;
          }
        }

        map = new ButtonMappingForm(keyCode, description);
      }

      if (map.ShowDialog(this) == DialogResult.OK)
      {
        if (existing == null) // Create new mapping
        {
          mappingsListView.Items.Add(
            new ListViewItem(new string[] {map.KeyCode, map.Description, map.CurrentCommand.UserDisplayText}));

          currentMappings.Add(new ButtonMapping(map.KeyCode, map.Description, map.CurrentCommand));
        }
        else // Replace existing mapping
        {
          for (int index = 0; index < mappingsListView.Items.Count; index++)
          {
            if (mappingsListView.Items[index].SubItems[0].Text.Equals(map.KeyCode, StringComparison.Ordinal))
            {
              mappingsListView.Items[index].SubItems[1].Text = map.Description;
              mappingsListView.Items[index].SubItems[2].Text = map.CurrentCommand.UserDisplayText;
            }
          }

          existing.Description = map.Description;
          existing.Command = map.CurrentCommand;
        }
      }
    }
    private void NewButtonMapping(object sender, EventArgs e)
    {
      NewButtonMapping();
    }

    private void DeleteButtonMapping()
    {
      if (mappingsListView.SelectedIndices.Count != 1) return;

      List<ButtonMapping> currentMappings = GetCurrentButtonMappings();
      if (currentMappings == null) return;

      int selectedIndex = mappingsListView.SelectedIndices[0];
      ListViewItem item = mappingsListView.SelectedItems[0];
      mappingsListView.Items.Remove(item);
      // reselect an item
      if (mappingsListView.Items.Count > 0)
        if (mappingsListView.Items.Count <= selectedIndex)
          mappingsListView.SelectedIndices.Add(mappingsListView.Items.Count - 1);
        else
          mappingsListView.SelectedIndices.Add(selectedIndex);

      ButtonMapping toRemove = null;
      foreach (ButtonMapping test in currentMappings)
      {
        if (test.KeyCode.Equals(item.SubItems[0].Text, StringComparison.Ordinal))
        {
          toRemove = test;
          break;
        }
      }

      if (toRemove != null)
        currentMappings.Remove(toRemove);
    }
    private void DeleteButtonMapping(object sender, EventArgs e)
    {
      DeleteButtonMapping();
    }

    private void EditButtonMapping()
    {
      if (mappingsListView.SelectedIndices.Count != 1)
        return;

      ListViewItem item = mappingsListView.SelectedItems[0];

      List<ButtonMapping> currentMappings = GetCurrentButtonMappings();
      if (currentMappings == null)
        return;

      foreach (ButtonMapping test in currentMappings)
      {
        if (item.SubItems[0].Text.Equals(test.KeyCode, StringComparison.Ordinal))
        {
          ButtonMappingForm map;

          if (!test.IsCommandAvailable)
          {
            DialogResult dr = MessageBox.Show(this,
                                              "The command " + test.GetCommandDisplayText() + " was not found in CommandLibrary and can not be modified." +
                                              Environment.NewLine + Environment.NewLine +
                                              "Do you want to replace the command with a another one?",
                                              "Command not available", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                              MessageBoxDefaultButton.Button2);

            if (dr == DialogResult.Yes)
              map = new ButtonMappingForm(test.KeyCode, test.Description);
            else
              return;
          }
          else
            map = new ButtonMappingForm(test.KeyCode, test.Description, test.Command);

          if (map.ShowDialog(this) == DialogResult.OK)
          {
            item.SubItems[1].Text = map.Description;
            item.SubItems[2].Text = map.CommandString;

            test.Description = map.Description;
            test.Command = map.CurrentCommand;
          }

          break;
        }
      }
    }
    private void EditButtonMapping(object sender, EventArgs e)
    {
      EditButtonMapping();
    }

    private void ClearButtonMappings()
    {
      List<ButtonMapping> currentMappings = GetCurrentButtonMappings();
      if (currentMappings == null)
        return;

      if (
        MessageBox.Show(this, "Are you sure you want to clear all remote button mappings?", "Warning",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        return;

      currentMappings.Clear();
      mappingsListView.Items.Clear();
    }
    private void ClearButtonMappings(object sender, EventArgs e)
    {
      ClearButtonMappings();
    }

    private void RemapButtonMapping()
    {
      if (mappingsListView.SelectedIndices.Count != 1)
        return;

      ListViewItem item = mappingsListView.SelectedItems[0];

      List<ButtonMapping> currentMappings = GetCurrentButtonMappings();
      if (currentMappings == null)
        return;

      ButtonMapping toModify = null;
      foreach (ButtonMapping test in currentMappings)
      {
        if (test.KeyCode.Equals(item.SubItems[0].Text, StringComparison.Ordinal))
        {
          toModify = test;
          break;
        }
      }

      if (toModify == null)
        return;

      GetKeyCodeForm getKeyCode = new GetKeyCodeForm();
      getKeyCode.ShowDialog(this);

      string keyCode = getKeyCode.KeyCode;
      if (String.IsNullOrEmpty(keyCode))
        return;

      foreach (ButtonMapping test in currentMappings)
      {
        if (test.KeyCode.Equals(keyCode, StringComparison.Ordinal))
        {
          MessageBox.Show(this,
                          String.Format("{0} is already mapped to {1} ({2})", keyCode, test.Description, test.GetCommandDisplayText()),
                          "Cannot remap", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          return;
        }
      }

      item.SubItems[0].Text = keyCode;

      toModify.KeyCode = keyCode;
    }
    private void RemapButtonMapping(object sender, EventArgs e)
    {
      RemapButtonMapping();
    }

    private void CopyMappingsFromOtherProgram(string programName)
    {
      if (programName.Equals(SystemWide))
      {
        ImportButtons(Program.Config.SystemWideMappings);
        return;
      }

      foreach (ProgramSettings programSettings in Program.Config.Programs)
      {
        if (programSettings.Name.Equals(programName, StringComparison.OrdinalIgnoreCase))
        {
          ImportButtons(programSettings.ButtonMappings);
          return;
        }
      }
    }
    private void CopyMappingsFromOtherProgram(object sender, EventArgs e)
    {
      ToolStripMenuItem origin = sender as ToolStripMenuItem;
      if (origin == null)
        return;

      CopyMappingsFromOtherProgram(origin.Text);
      RefreshMappingList();
    }


    private void mappingsContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
      copyMappingsFromToolStripMenuItem.DropDownItems.Clear();

      string selectedItem = programsListView.Items[_selectedProgram].Text;

      if (_selectedProgram > 0)
        copyMappingsFromToolStripMenuItem.DropDownItems.Add(SystemWide, IrssUtils.Properties.Resources.WinLogo,
                                                           CopyMappingsFromOtherProgram);

      foreach (ProgramSettings programSettings in Program.Config.Programs)
      {
        if (programSettings.Name.Equals(selectedItem))
          continue;

        if (String.IsNullOrEmpty(programSettings.FileName))
          continue;

        ToolStripItem item = new ToolStripMenuItem();
        item.Text = programSettings.Name;
        item.Image = Win32.GetImageFromFile(programSettings.FileName);
        item.Click += CopyMappingsFromOtherProgram;

        copyMappingsFromToolStripMenuItem.DropDownItems.Add(item);
      }

      copyMappingsFromToolStripMenuItem.Enabled = copyMappingsFromToolStripMenuItem.DropDownItems.Count > 0;
    }

    private void mappingsListView_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (mappingsListView.SelectedIndices.Count != 1)
      {
        editMappingToolStripButton.Enabled = false;
        editMappingToolStripMenuItem.Enabled = false;
        deleteMappingToolStripButton.Enabled = false;
        deleteMappingToolStripMenuItem.Enabled = false;
        remapToolStripButton.Enabled = false;
        remapToolStripMenuItem.Enabled = false;
      }
      else
      {
        editMappingToolStripButton.Enabled = true;
        editMappingToolStripMenuItem.Enabled = true;
        deleteMappingToolStripButton.Enabled = true;
        deleteMappingToolStripMenuItem.Enabled = true;
        remapToolStripButton.Enabled = true;
        remapToolStripMenuItem.Enabled = true;
      }
    }

    private void mappingsListView_KeyDown(object sender, KeyEventArgs e)
    {
      e.Handled = true;

      switch (e.KeyData)
      {
        case Keys.OemMinus:
        case Keys.Delete:
          DeleteButtonMapping();
          break;

        case Keys.F2:
        case Keys.Enter:
          EditButtonMapping();
          break;

        case Keys.Oemplus:
        case Keys.Insert:
          NewButtonMapping();
          break;

        default:
          e.Handled = false;
          break;
      }
    }

    #endregion Mappings Panel

    #region Events Tab

    private void RefreshEventList()
    {
      listViewEventMap.Items.Clear();

      foreach (MappedEvent mappedEvent in Program.Config.Events)
      {
        string[] subItems = new string[2];
        subItems[0] = Enum.GetName(typeof(MappingEvent), mappedEvent.EventType);
        subItems[1] = mappedEvent.GetCommandDisplayText();

        ListViewItem item = new ListViewItem(subItems);
        if (mappedEvent.IsCommandAvailable)
        {
          item.Tag = mappedEvent.Command;
        }
        else
        {
          item.Tag = mappedEvent;
          item.ToolTipText = "Command was not found in CommandLibrary.";
        }
        listViewEventMap.Items.Add(item);
      }

      // refresh combobox
      comboBoxEvents.Items.Clear();
      foreach (string eventName in Enum.GetNames(typeof(MappingEvent)))
        if (!eventName.Equals("None", StringComparison.OrdinalIgnoreCase))
          comboBoxEvents.Items.Add(eventName);

      comboBoxEvents.SelectedIndex = 0;

      // refresh context menu
      addEventToolStripMenuItem.DropDownItems.Clear();
      foreach (string eventName in Enum.GetNames(typeof(MappingEvent)))
        if (!eventName.Equals("None", StringComparison.OrdinalIgnoreCase))
          addEventToolStripMenuItem.DropDownItems.Add(
            eventName, null, AddEvent);
    }

    private void PopulateCommandList(string[] categories)
    {
      treeViewCommandList.Nodes.Clear();
      Dictionary<string, TreeNode> categoryNodes = new Dictionary<string, TreeNode>(categories.Length);

      // Create requested categories ...
      foreach (string category in categories)
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

    private void CommitEvents()
    {
      Program.Config.Events.Clear();

      foreach (ListViewItem item in listViewEventMap.Items)
      {
        try
        {
          if (ReferenceEquals(item.Tag, null)) continue;

          // command is available, tag is a command
          Command command = item.Tag as Command;
          if (!ReferenceEquals(command, null))
          {
            MappingEvent eventType = (MappingEvent) Enum.Parse(typeof (MappingEvent), item.SubItems[0].Text, true);

            Program.Config.Events.Add(new MappedEvent(eventType, command));
            continue;
          }

          // command is not available, tag is the preserved mapped event
          MappedEvent map = item.Tag as MappedEvent;
          if (!ReferenceEquals(map, null))
            Program.Config.Events.Add(map);
        }
        catch (Exception ex)
        {
          IrssLog.Error("Bad item in event list: {0}, {1}\n{2}", item.SubItems[0].Text, item.SubItems[1].Text,
                        ex.Message);
        }
      }
    }

    private void AddEvent(string mappingEvent)
    {
      string[] subItems = new string[2];
      subItems[0] = mappingEvent;
      subItems[1] = string.Empty;

      ListViewItem item = new ListViewItem(subItems);
      listViewEventMap.SelectedIndices.Clear();
      listViewEventMap.Items.Add(item);
      item.Selected = true;
    }
    private void AddEvent(object sender, EventArgs e)
    {
      if (Equals(sender, buttonAddEvent))
      {
        AddEvent(comboBoxEvents.SelectedItem as string);
      }
      else
      {
        AddEvent(sender.ToString());
      }
    }

    private void listViewEventMap_KeyDown(object sender, KeyEventArgs e)
    {

    }

    private void listViewEventMap_DoubleClick(object sender, EventArgs e)
    {
      if (listViewEventMap.SelectedItems.Count == 0) return;

      foreach (ListViewItem item in listViewEventMap.SelectedItems)
      {
        if (ReferenceEquals(item.Tag, null)) continue;

        Command command = item.Tag as Command;
        if (ReferenceEquals(command, null)) continue;

        if (!Program.CommandProcessor.Edit(command, this)) continue;

        item.Tag = command;
        item.SubItems[1].Text = command.UserDisplayText;
      }
    }

    private void listViewEventMap_SelectedIndexChanged(object sender, EventArgs e)
    {
      splitContainer1.Panel2.Enabled = listViewEventMap.SelectedItems.Count != 0;
    }

    private void treeViewCommandList_DoubleClick(object sender, EventArgs e)
    {
      if (treeViewCommandList.SelectedNode == null || treeViewCommandList.SelectedNode.Level == 0)
        return;

      if (listViewEventMap.SelectedItems.Count == 0) return;

      Type commandType = treeViewCommandList.SelectedNode.Tag as Type;
      Command command = (Command)Activator.CreateInstance(commandType);

      if (!Program.CommandProcessor.Edit(command, this)) return;

      foreach (ListViewItem item in listViewEventMap.SelectedItems)
      {
        item.Tag = command;
        item.SubItems[1].Text = command.UserDisplayText;
      }
    }

    #endregion

    #region Macros Tab

    private void CreateShortcutForMacro(string fileName)
    {
      string macroName = Path.GetFileNameWithoutExtension(fileName);

      string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
      string shortcutPath = Path.Combine(desktopPath, String.Format("Macro - {0}.lnk", macroName));

      ShellShortcut shortcut = new ShellShortcut(shortcutPath);

      string translatorExe = Assembly.GetEntryAssembly().Location;
      string translatorFolder = Path.GetDirectoryName(translatorExe);

      //shortcut.Arguments        = String.Format("-MACRO \"{0}\"", Path.Combine(Program.FolderMacros, macroName + Common.FileExtensionMacro));
      shortcut.Arguments = String.Format("-MACRO \"{0}\"", macroName);
      shortcut.Description = "Launch Macro: " + macroName;
      shortcut.Path = translatorExe;
      shortcut.WorkingDirectory = translatorFolder;
      shortcut.WindowStyle = ProcessWindowStyle.Normal;

      shortcut.Save();
    }

    #endregion

    #region IRCommands Tab

    private void NewIRCommand()
    {
      _learnIR = new LearnIR(
        Program.LearnIR,
        Program.BlastIR,
        Program.TransceiverInformation.Ports);

      _learnIR.ShowDialog(this);

      _learnIR = null;
    }

    private void EditIRCommand(string fileName)
    {
      string command = Path.GetFileNameWithoutExtension(fileName);

      _learnIR = new LearnIR(
        Program.LearnIR,
        Program.BlastIR,
        Program.TransceiverInformation.Ports,
        command);

      _learnIR.ShowDialog(this);

      _learnIR = null;
    }

    #endregion


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
        XmlSerializer reader = new XmlSerializer(typeof (AppProfile));
        using (StreamReader file = new StreamReader(settingsFile))
          return (AppProfile) reader.Deserialize(file);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }

      return null;
    }

    private void ImportButtons(List<ButtonMapping> buttonMappings)
    {
      List<ButtonMapping> currentMappings = GetCurrentButtonMappings();
      if (currentMappings == null)
        return;

      foreach (ButtonMapping newMapping in buttonMappings)
      {
        bool alreadyExists = false;

        foreach (ButtonMapping existingMapping in currentMappings)
        {
          if (existingMapping.KeyCode.Equals(newMapping.KeyCode, StringComparison.Ordinal))
          {
            // Change the existing mapping to the new one
            existingMapping.Description = newMapping.Description;
            existingMapping.Command = newMapping.Command;
            alreadyExists = true;
            break;
          }
        }

        if (!alreadyExists)
          currentMappings.Add(newMapping);
      }
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

    #endregion Implementation
  }
}