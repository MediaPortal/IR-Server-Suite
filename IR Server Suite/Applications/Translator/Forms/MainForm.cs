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
using MSjogren.Samples.ShellLink;
using Translator.Properties;

namespace Translator
{
  internal partial class MainForm : Form
  {
    #region Constants

    private const string SystemWide = "System Wide";

    #endregion Constants

    #region Variables

    private LearnIR _learnIR;

    private int _selectedProgram;

    #endregion Variables

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="MainForm"/> class.
    /// </summary>
    public MainForm()
    {
      InitializeComponent();
      SetImages();

      RefreshProgramList();
      programsListView.Items[0].Selected = true;

      RefreshMappingList();
      RefreshEventList();
      RefreshEventCommands();
      RefreshIRList();
      RefreshMacroList();

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

      // macros tab
      this.newMacroToolStripButton.Image = IrssUtils.Properties.Resources.Plus;
      this.editMacroToolStripButton.Image = IrssUtils.Properties.Resources.Edit;
      this.deleteMacroToolStripButton.Image = IrssUtils.Properties.Resources.Delete;
      this.createShortcutForMacroToolStripButton.Image = IrssUtils.Properties.Resources.Shortcut;
      this.testMacroToolStripButton.Image = IrssUtils.Properties.Resources.MoveRight;

      this.addMacroToolStripMenuItem.Image = IrssUtils.Properties.Resources.Plus;
      this.editMacroToolStripMenuItem.Image = IrssUtils.Properties.Resources.Edit;
      this.deleteMacroToolStripMenuItem.Image = IrssUtils.Properties.Resources.Delete;
      this.createShortcutForMacroToolStripMenuItem.Image = IrssUtils.Properties.Resources.Shortcut;
      this.testMacroToolStripMenuItem.Image = IrssUtils.Properties.Resources.MoveRight;

      // ir commands tab
      this.newIRToolStripButton.Image = IrssUtils.Properties.Resources.Plus;
      this.editIRToolStripButton.Image = IrssUtils.Properties.Resources.Edit;
      this.deleteIRToolStripButton.Image = IrssUtils.Properties.Resources.Delete;

      this.addIRToolStripMenuItem.Image = IrssUtils.Properties.Resources.Plus;
      this.editIRToolStripMenuItem.Image = IrssUtils.Properties.Resources.Edit;
      this.deleteIRToolStripMenuItem.Image = IrssUtils.Properties.Resources.Delete;
    }

    #region Implementation

    #region Main Form

    private void MainForm_Load(object sender, EventArgs e)
    {
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
      IrssHelp.Open(sender);
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
          RefreshIRList();
          break;

        case "tabPageMacro":
          RefreshMacroList();
          break;

        case "tabPageEvents":
          RefreshEventCommands();
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
      IrssHelp.Open(this);
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
          icon = Win32.GetIconFor(progSettings.FileName);

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
        mappingsListView.Items.Add(
          new ListViewItem(
            new string[] {map.KeyCode, map.Description, map.Command}
            )
          );
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

        map = new ButtonMappingForm(keyCode, description, String.Empty);
      }

      if (map.ShowDialog(this) == DialogResult.OK)
      {
        if (existing == null) // Create new mapping
        {
          mappingsListView.Items.Add(
            new ListViewItem(
              new string[] { map.KeyCode, map.Description, map.Command }
              ));

          currentMappings.Add(new ButtonMapping(map.KeyCode, map.Description, map.Command));
        }
        else // Replace existing mapping
        {
          for (int index = 0; index < mappingsListView.Items.Count; index++)
          {
            if (mappingsListView.Items[index].SubItems[0].Text.Equals(map.KeyCode, StringComparison.Ordinal))
            {
              mappingsListView.Items[index].SubItems[1].Text = map.Description;
              mappingsListView.Items[index].SubItems[2].Text = map.Command;
            }
          }

          existing.Description = map.Description;
          existing.Command = map.Command;
        }
      }
    }
    private void NewButtonMapping(object sender, EventArgs e)
    {
      NewButtonMapping();
    }

    private void DeleteButtonMapping()
    {
      if (mappingsListView.SelectedIndices.Count != 1)
        return;

      List<ButtonMapping> currentMappings = GetCurrentButtonMappings();
      if (currentMappings == null)
        return;

      ListViewItem item = mappingsListView.SelectedItems[0];
      mappingsListView.Items.Remove(item);

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
          ButtonMappingForm map = new ButtonMappingForm(test.KeyCode, test.Description, test.Command);

          if (map.ShowDialog(this) == DialogResult.OK)
          {
            item.SubItems[1].Text = map.Description;
            item.SubItems[2].Text = map.Command;

            test.Description = map.Description;
            test.Command = map.Command;
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
                          String.Format("{0} is already mapped to {1} ({2})", keyCode, test.Description, test.Command),
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

        Icon icon = Win32.GetIconFor(programSettings.FileName);

        Image image = null;
        if (icon != null)
          image = icon.ToBitmap();

        copyMappingsFromToolStripMenuItem.DropDownItems.Add(programSettings.Name, image, CopyMappingsFromOtherProgram);
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
        listViewEventMap.Items.Add(
          new ListViewItem(
            new string[] { Enum.GetName(typeof(MappingEvent), mappedEvent.EventType), mappedEvent.Command }
            )
          );
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

    private void RefreshEventCommands()
    {
      object wasSelected = null;
      if (comboBoxCommands.SelectedIndex != -1)
        wasSelected = comboBoxCommands.SelectedItem;

      comboBoxCommands.Items.Clear();

      comboBoxCommands.Items.Add(Common.UITextRun);
      comboBoxCommands.Items.Add(Common.UITextSerial);
      comboBoxCommands.Items.Add(Common.UITextWindowMsg);
      comboBoxCommands.Items.Add(Common.UITextKeys);

      string[] list = Common.GetIRList(true);
      if (list != null && list.Length > 0)
        comboBoxCommands.Items.AddRange(list);

      list = IrssMacro.GetMacroList(Program.FolderMacros, true);
      if (list != null && list.Length > 0)
        comboBoxCommands.Items.AddRange(list);

      if (wasSelected != null && comboBoxCommands.Items.Contains(wasSelected))
        comboBoxCommands.SelectedItem = wasSelected;
      else
        comboBoxCommands.SelectedIndex = 0;
    }


    private void CommitEvents()
    {
      Program.Config.Events.Clear();
      MappingEvent eventType;
      string command;

      foreach (ListViewItem item in listViewEventMap.Items)
      {
        try
        {
          eventType = (MappingEvent)Enum.Parse(typeof(MappingEvent), item.SubItems[0].Text, true);
          command = item.SubItems[1].Text;

          Program.Config.Events.Add(new MappedEvent(eventType, command));
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
      ListViewItem newItem =
        new ListViewItem(new string[] { mappingEvent, String.Empty });

      listViewEventMap.SelectedIndices.Clear();
      listViewEventMap.Items.Add(newItem);
      newItem.Selected = true;
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

    private void SetCommandToEvent(object sender, EventArgs e)
    {
      if (comboBoxCommands.SelectedIndex == -1 || listViewEventMap.SelectedItems.Count == 0)
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
      else if (selected.Equals(Common.UITextKeys, StringComparison.OrdinalIgnoreCase))
      {
        KeysCommand keysCommand = new KeysCommand();
        if (keysCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixKeys + keysCommand.CommandString;
      }
      else if (selected.StartsWith(Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
      {
        BlastCommand blastCommand = new BlastCommand(
          Program.BlastIR,
          Common.FolderIRCommands,
          Program.TransceiverInformation.Ports,
          selected.Substring(Common.CmdPrefixBlast.Length));

        if (blastCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixBlast + blastCommand.CommandString;
      }
      else if (selected.StartsWith(Common.CmdPrefixMacro, StringComparison.OrdinalIgnoreCase))
      {
        command = selected;
      }
      else
      {
        IrssLog.Error("Invalid command set in Events: {0}", selected);
        return;
      }

      foreach (ListViewItem listViewItem in listViewEventMap.SelectedItems)
        listViewItem.SubItems[1].Text = command;
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

      if (command.StartsWith(Common.CmdPrefixRun, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitRunCommand(command.Substring(Common.CmdPrefixRun.Length));
        ExternalProgram externalProgram = new ExternalProgram(commands);
        if (externalProgram.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixRun + externalProgram.CommandString;
      }
      else if (command.StartsWith(Common.CmdPrefixSerial, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitSerialCommand(command.Substring(Common.CmdPrefixSerial.Length));
        SerialCommand serialCommand = new SerialCommand(commands);
        if (serialCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixSerial + serialCommand.CommandString;
      }
      else if (command.StartsWith(Common.CmdPrefixWindowMsg, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitWindowMessageCommand(command.Substring(Common.CmdPrefixWindowMsg.Length));
        MessageCommand messageCommand = new MessageCommand(commands);
        if (messageCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixWindowMsg + messageCommand.CommandString;
      }
      else if (command.StartsWith(Common.CmdPrefixKeys, StringComparison.OrdinalIgnoreCase))
      {
        KeysCommand keysCommand = new KeysCommand(command.Substring(Common.CmdPrefixKeys.Length));
        if (keysCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixKeys + keysCommand.CommandString;
      }
      else if (command.StartsWith(Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitBlastCommand(command.Substring(Common.CmdPrefixBlast.Length));

        BlastCommand blastCommand = new BlastCommand(
          Program.BlastIR,
          Common.FolderIRCommands,
          Program.TransceiverInformation.Ports,
          commands);

        if (blastCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixBlast + blastCommand.CommandString;
      }
      else
      {
        return;
      }

      listViewEventMap.SelectedItems[0].SubItems[1].Text = command;
    }

    #endregion

    #region Macros Tab

    private void RefreshMacroList()
    {
      listViewMacro.Items.Clear();

      string[] macroList = IrssMacro.GetMacroList(Program.FolderMacros, false);
      if (macroList != null && macroList.Length > 0)
        foreach (string macroFile in macroList)
          listViewMacro.Items.Add(macroFile);

      Program.UpdateNotifyMenu();

      listViewMacro_SelectedIndexChanged(null, null);
    }


    private void NewMacro(object sender, EventArgs e)
    {
      MacroEditor macroEditor = new MacroEditor();
      macroEditor.ShowDialog(this);

      RefreshMacroList();
    }

    private void EditMacro(object sender, EventArgs e)
    {
      if (listViewMacro.SelectedItems.Count != 1)
        return;

      string command = listViewMacro.SelectedItems[0].Text;
      string fileName = Path.Combine(Program.FolderMacros, command + Common.FileExtensionMacro);

      if (File.Exists(fileName))
      {
        MacroEditor macroEditor = new MacroEditor(command);
        macroEditor.ShowDialog(this);
      }
      else
      {
        MessageBox.Show(this, "File not found: " + fileName, "Macro file missing", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
        RefreshMacroList();
      }
    }

    private void DeleteMacro(object sender, EventArgs e)
    {
      if (listViewMacro.SelectedItems.Count != 1)
        return;

      string file = listViewMacro.SelectedItems[0].Text;
      string fileName = Path.Combine(Program.FolderMacros, file + Common.FileExtensionMacro);
      if (File.Exists(fileName))
      {
        if (
          MessageBox.Show(this, String.Format("Are you sure you want to delete \"{0}\"?", file), "Confirm delete",
                          MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
          File.Delete(fileName);
          listViewMacro.Items.Remove(listViewMacro.SelectedItems[0]);
        }
      }
      else
      {
        MessageBox.Show(this, "File not found: " + fileName, "Macro file missing", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
      }
    }

    private void TestMacro(object sender, EventArgs e)
    {
      if (listViewMacro.SelectedItems.Count != 1)
        return;

      try
      {
        Program.ProcessCommand(Common.CmdPrefixMacro + listViewMacro.SelectedItems[0].Text, false);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(this, ex.Message, "Test failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void CreateShortcutForMacro(object sender, EventArgs e)
    {
      if (listViewMacro.SelectedItems.Count != 1)
        return;

      string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
      string macroName = listViewMacro.SelectedItems[0].Text;
      string shortcutPath = Path.Combine(desktopPath, String.Format("Macro - {0}.lnk", macroName));

      ShellShortcut shortcut = new ShellShortcut(shortcutPath);

      string translatorFolder = Path.Combine(SystemRegistry.GetInstallFolder(), "Translator");

      //shortcut.Arguments        = String.Format("-MACRO \"{0}\"", Path.Combine(Program.FolderMacros, macroName + Common.FileExtensionMacro));
      shortcut.Arguments = String.Format("-MACRO \"{0}\"", macroName);
      shortcut.Description = "Launch Macro: " + macroName;
      shortcut.Path = Path.Combine(translatorFolder, "Translator.exe");
      shortcut.WorkingDirectory = translatorFolder;
      shortcut.WindowStyle = ProcessWindowStyle.Normal;

      shortcut.Save();
    }


    private void listViewMacro_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (listViewMacro.SelectedIndices.Count != 1)
      {
        editMacroToolStripButton.Enabled = false;
        editMacroToolStripMenuItem.Enabled = false;
        deleteMacroToolStripButton.Enabled = false;
        deleteMacroToolStripMenuItem.Enabled = false;
        testMacroToolStripButton.Enabled = false;
        testMacroToolStripMenuItem.Enabled = false;
        createShortcutForMacroToolStripButton.Enabled = false;
        createShortcutForMacroToolStripMenuItem.Enabled = false;
      }
      else
      {
        editMacroToolStripButton.Enabled = true;
        editMacroToolStripMenuItem.Enabled = true;
        deleteMacroToolStripButton.Enabled = true;
        deleteMacroToolStripMenuItem.Enabled = true;
        testMacroToolStripButton.Enabled = true;
        testMacroToolStripMenuItem.Enabled = true;
        createShortcutForMacroToolStripButton.Enabled = true;
        createShortcutForMacroToolStripMenuItem.Enabled = true;
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

      string oldFileName = Path.Combine(Program.FolderMacros, originItem.Text + Common.FileExtensionMacro);
      if (!File.Exists(oldFileName))
      {
        MessageBox.Show("File not found: " + oldFileName, "Cannot rename, Original file not found", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
        e.CancelEdit = true;
        return;
      }

      string name = e.Label.Trim();

      if (!Common.IsValidFileName(name))
      {
        MessageBox.Show("File name not valid: " + name, "Cannot rename, New file name not valid", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
        e.CancelEdit = true;
        return;
      }

      try
      {
        string newFileName = Path.Combine(Program.FolderMacros, name + Common.FileExtensionMacro);
        File.Move(oldFileName, newFileName);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(ex.Message, "Failed to rename file", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #endregion

    #region IRCommands Tab

    private void RefreshIRList()
    {
      listViewIR.Items.Clear();

      string[] irList = Common.GetIRList(false);
      if (irList != null && irList.Length > 0)
        foreach (string irFile in irList)
          listViewIR.Items.Add(irFile);

      listViewIR_SelectedIndexChanged(null, null);
    }


    private void NewIRCommand(object sender, EventArgs e)
    {
      _learnIR = new LearnIR(
        Program.LearnIR,
        Program.BlastIR,
        Program.TransceiverInformation.Ports);

      _learnIR.ShowDialog(this);

      _learnIR = null;

      RefreshIRList();
    }

    private void EditIRCommand(object sender, EventArgs e)
    {
      if (listViewIR.SelectedItems.Count != 1)
        return;

      string command = listViewIR.SelectedItems[0].Text;
      string fileName = Path.Combine(Common.FolderIRCommands, command + Common.FileExtensionIR);

      if (File.Exists(fileName))
      {
        _learnIR = new LearnIR(
          Program.LearnIR,
          Program.BlastIR,
          Program.TransceiverInformation.Ports,
          command);

        _learnIR.ShowDialog(this);

        _learnIR = null;
      }
      else
      {
        MessageBox.Show(this, "File not found: " + fileName, "IR file missing", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
        RefreshIRList();
      }
    }

    private void DeleteIRCommand(object sender, EventArgs e)
    {
      if (listViewIR.SelectedItems.Count != 1)
        return;

      string file = listViewIR.SelectedItems[0].Text;
      string fileName = Path.Combine(Common.FolderIRCommands, file + Common.FileExtensionIR);
      if (File.Exists(fileName))
      {
        if (
          MessageBox.Show(this, String.Format("Are you sure you want to delete \"{0}\"?", file), "Confirm delete",
                          MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
          File.Delete(fileName);
          listViewIR.Items.Remove(listViewIR.SelectedItems[0]);
        }
      }
      else
      {
        MessageBox.Show(this, "File not found: " + fileName, "IR file missing", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
      }
    }


    private void listViewIR_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (listViewIR.SelectedIndices.Count != 1)
      {
        editIRToolStripButton.Enabled = false;
        editIRToolStripMenuItem.Enabled = false;
        deleteIRToolStripButton.Enabled = false;
        deleteIRToolStripMenuItem.Enabled = false;
      }
      else
      {
        editIRToolStripButton.Enabled = true;
        editIRToolStripMenuItem.Enabled = true;
        deleteIRToolStripButton.Enabled = true;
        deleteIRToolStripMenuItem.Enabled = true;
      }
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
        MessageBox.Show("File not found: " + oldFileName, "Cannot rename, Original file not found", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
        e.CancelEdit = true;
        return;
      }

      string name = e.Label.Trim();

      if (!Common.IsValidFileName(name))
      {
        MessageBox.Show("File name not valid: " + name, "Cannot rename, New file name not valid", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
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