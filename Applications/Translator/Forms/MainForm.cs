using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

using IrssComms;
using IrssUtils;
using IrssUtils.Forms;

namespace Translator
{

  partial class MainForm : Form
  {

    #region Constants

    const string SystemWide = "System Wide";

    #endregion Constants

    #region Enumerations

    /// <summary>
    /// A list of MCE remote buttons.
    /// </summary>
    public enum MceButton
    {
      Custom        = -1,
      None          = 0,
      TV_Power      = 0x7b9a,
      Blue          = 0x7ba1,
      Yellow        = 0x7ba2,
      Green         = 0x7ba3,
      Red           = 0x7ba4,
      Teletext      = 0x7ba5,
      Radio         = 0x7baf,
      Print         = 0x7bb1,
      Videos        = 0x7bb5,
      Pictures      = 0x7bb6,
      Recorded_TV   = 0x7bb7,
      Music         = 0x7bb8,
      TV            = 0x7bb9,
      Guide         = 0x7bd9,
      Live_TV       = 0x7bda,
      DVD_Menu      = 0x7bdb,
      Back          = 0x7bdc,
      OK            = 0x7bdd,
      Right         = 0x7bde,
      Left          = 0x7bdf,
      Down          = 0x7be0,
      Up            = 0x7be1,
      Star          = 0x7be2,
      Hash          = 0x7be3,
      Replay        = 0x7be4,
      Skip          = 0x7be5,
      Stop          = 0x7be6,
      Pause         = 0x7be7,
      Record        = 0x7be8,
      Play          = 0x7be9,
      Rewind        = 0x7bea,
      Forward       = 0x7beb,
      Channel_Down  = 0x7bec,
      Channel_Up    = 0x7bed,
      Volume_Down   = 0x7bee,
      Volume_Up     = 0x7bef,
      Info          = 0x7bf0,
      Mute          = 0x7bf1,
      Start         = 0x7bf2,
      PC_Power      = 0x7bf3,
      Enter         = 0x7bf4,
      Escape        = 0x7bf5,
      Number_9      = 0x7bf6,
      Number_8      = 0x7bf7,
      Number_7      = 0x7bf8,
      Number_6      = 0x7bf9,
      Number_5      = 0x7bfa,
      Number_4      = 0x7bfb,
      Number_3      = 0x7bfc,
      Number_2      = 0x7bfd,
      Number_1      = 0x7bfe,
      Number_0      = 0x7bff,
    }

    #endregion Enumerations

    #region Variables

    LearnIR _learnIR;
    
    ToolStripMenuItem _addProgramToolStripMenuItem;
    ToolStripMenuItem _editProgramToolStripMenuItem;
    ToolStripMenuItem _removeProgramToolStripMenuItem;

    int _selectedProgram;

    #endregion Variables

    #region Constructor

    public MainForm()
    {
      InitializeComponent();

      RefreshProgramList();
      listViewPrograms.Items[0].Selected = true;

      RefreshButtonList();
      RefreshEventList();
      RefreshEventCommands();
      RefreshIRList();
      RefreshMacroList();

      SetupProgramsContextMenu();

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

    private void MainForm_Load(object sender, EventArgs e)
    {
      Program.HandleMessage += new ClientMessageSink(ReceivedMessage);
    }
    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      Program.HandleMessage -= new ClientMessageSink(ReceivedMessage);

      CommitEvents();

      Configuration.Save(Program.Config, Program.ConfigFile);
    }

    void RefreshProgramList()
    {
      imageListPrograms.Images.Clear();
      imageListPrograms.Images.Add(Properties.Resources.WinLogo);

      Icon large;
      Icon small;

      string folder = Environment.GetFolderPath(Environment.SpecialFolder.System);
      string file = Path.Combine(folder, "user32.dll");
      Win32.ExtractIcons(file, 1, out large, out small);
      imageListPrograms.Images.Add(large);


      //imageListPrograms.Images.Add(Properties.Resources.NoIcon);

      string wasSelected = string.Empty;
      if (listViewPrograms.Items.Count > 0)
        wasSelected = listViewPrograms.Items[_selectedProgram].Text;
      
      listViewPrograms.Items.Clear();
      _selectedProgram = 0;

      // Add System-Wide ...
      ListViewItem newItem = new ListViewItem(SystemWide, 0);
      newItem.ToolTipText = "Defines mappings that effect the whole computer";
      listViewPrograms.Items.Add(newItem);

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

        listViewPrograms.Items.Add(newItem);

        if (progSettings.Name.Equals(wasSelected))
          newItem.Selected = true;
      }

      if (wasSelected.Equals(SystemWide) || listViewPrograms.SelectedItems.Count == 0)
        listViewPrograms.Items[0].Selected = true;

      Program.UpdateNotifyMenu();
    }
    void RefreshButtonList()
    {
      listViewButtons.Items.Clear();

      List<ButtonMapping> currentMappings = GetCurrentButtonMappings();
      if (currentMappings == null)
        return;

      foreach (ButtonMapping map in currentMappings)
      {
        listViewButtons.Items.Add(
          new ListViewItem(
            new string[] { map.KeyCode, map.Description, map.Command }
          )
        );
      }
    }
    void RefreshEventList()
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

      comboBoxEvents.Items.Clear();
      foreach (string eventName in Enum.GetNames(typeof(MappingEvent)))
        if (!eventName.Equals("None", StringComparison.OrdinalIgnoreCase))
          comboBoxEvents.Items.Add(eventName);

      comboBoxEvents.SelectedIndex = 0;
    }
    void RefreshEventCommands()
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

      string[] macroList = IrssMacro.GetMacroList(Program.FolderMacros, false);
      if (macroList != null && macroList.Length > 0)
        foreach (string macroFile in macroList)
          listViewMacro.Items.Add(macroFile);

      Program.UpdateNotifyMenu();
    }

    List<ButtonMapping> GetCurrentButtonMappings()
    {
      if (_selectedProgram == 0)
      {
        return Program.Config.SystemWideMappings;
      }
      else
      {
        string selectedItem = listViewPrograms.Items[_selectedProgram].Text;

        foreach (ProgramSettings progSettings in Program.Config.Programs)
          if (progSettings.Name.Equals(selectedItem))
            return progSettings.ButtonMappings;        
      }

      return null;
    }

    void SetupProgramsContextMenu()
    {
      _addProgramToolStripMenuItem = new ToolStripMenuItem("&Add Program", Properties.Resources.Plus, new EventHandler(addProgramToolStripMenuItem_Click));
      _editProgramToolStripMenuItem = new ToolStripMenuItem("&Edit Program", Properties.Resources.Edit, new EventHandler(editProgramToolStripMenuItem_Click));
      _removeProgramToolStripMenuItem = new ToolStripMenuItem("&Remove Program", Properties.Resources.Delete, new EventHandler(removeProgramToolStripMenuItem_Click));

      contextMenuStripPrograms.Items.Add(_addProgramToolStripMenuItem);
      contextMenuStripPrograms.Items.Add(_editProgramToolStripMenuItem);
      contextMenuStripPrograms.Items.Add(_removeProgramToolStripMenuItem);
    }
    void RefreshProgramsContextMenu()
    {
      if (_selectedProgram == 0)
      {
        _editProgramToolStripMenuItem.Text      = "&Edit ...";
        _removeProgramToolStripMenuItem.Text    = "&Remove ...";

        _editProgramToolStripMenuItem.Enabled   = false;
        _removeProgramToolStripMenuItem.Enabled = false;
      }
      else
      {
        string program = listViewPrograms.Items[_selectedProgram].Text;

        _editProgramToolStripMenuItem.Text      = String.Format("&Edit \"{0}\"", program);
        _removeProgramToolStripMenuItem.Text    = String.Format("&Remove \"{0}\"", program);

        _editProgramToolStripMenuItem.Enabled   = true;
        _removeProgramToolStripMenuItem.Enabled = true;
      }
    }

    void AddProgram()
    {
      ProgramSettings progSettings = new ProgramSettings();

      if (EditProgram(progSettings))
      {
        Program.Config.Programs.Add(progSettings);

        RefreshProgramList();
        /*
        string programFile = Path.GetFileName(progSettings.FileName);
        string settingsFile = Path.Combine(Program.FolderDefaultSettings, programFile + ".xml");
        if (File.Exists(settingsFile))
        {
          if (MessageBox.Show(this, String.Format("Do you want to use the default settings for {0}", programFile), "Default settings found", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          {
            List<ButtonMapping> mappings = LoadDefaultSettings(settingsFile);

            progSettings.ButtonMappings.AddRange(mappings);

            RefreshButtonList();
          }
        }
        */
      }
    }

    List<ButtonMapping> LoadDefaultSettings(string settingsFile)
    {
      // TODO: Implement this :)
      return new List<ButtonMapping>();
    }

    bool EditCurrentProgram()
    {
      if (_selectedProgram == 0)
        return false;

      string selectedItem = listViewPrograms.Items[_selectedProgram].Text;
      
      return EditProgram(selectedItem);
    }
    bool EditProgram(string programName)
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
    bool EditProgram(ProgramSettings progSettings)
    {
      EditProgramForm editProg = new EditProgramForm(progSettings);

      if (editProg.ShowDialog(this) == DialogResult.OK)
      {
        progSettings.Name             = editProg.DisplayName;
        progSettings.FileName         = editProg.Filename;
        progSettings.Folder           = editProg.StartupFolder;
        progSettings.Arguments        = editProg.Parameters;
        progSettings.WindowState      = editProg.StartState;
        progSettings.UseShellExecute  = editProg.UseShellExecute;
        progSettings.IgnoreSystemWide = editProg.IgnoreSystemWide;

        Program.UpdateNotifyMenu();
        return true;
      }

      return false;
    }

    void EditIR()
    {
      if (listViewIR.SelectedItems.Count != 1)
        return;

      string command = listViewIR.SelectedItems[0].Text;
      string fileName = Path.Combine(Common.FolderIRCommands, command + Common.FileExtensionIR);

      if (File.Exists(fileName))
      {
        _learnIR = new LearnIR(
          new LearnIrDelegate(Program.LearnIR),
          new BlastIrDelegate(Program.BlastIR),
          Program.TransceiverInformation.Ports,
          command);

        _learnIR.ShowDialog(this);

        _learnIR = null;
      }
      else
      {
        MessageBox.Show(this, "File not found: " + fileName, "IR file missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        RefreshIRList();
      }
    }
    void EditMacro()
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
        MessageBox.Show(this, "File not found: " + fileName, "Macro file missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        RefreshMacroList();
      }
    }

    void CommitEvents()
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
          IrssLog.Error("Bad item in event list: {0}, {1}\n{2}", item.SubItems[0].Text, item.SubItems[1].Text, ex.Message);
        }
      }
    }

    void NewButtonMapping()
    {
      List<ButtonMapping> currentMappings = GetCurrentButtonMappings();
      if (currentMappings == null)
        return;

      GetKeyCodeForm getKeyCode = new GetKeyCodeForm();
      getKeyCode.ShowDialog(this);

      string keyCode = getKeyCode.KeyCode;

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

        // TODO: Get description from Abstract Remote Model ...

        map = new ButtonMappingForm(keyCode, description, String.Empty);
      }

      if (map.ShowDialog(this) == DialogResult.OK)
      {
        if (existing == null) // Create new mapping
        {
          listViewButtons.Items.Add(
            new ListViewItem(
            new string[] { map.KeyCode, map.Description, map.Command }
            ));

          currentMappings.Add(new ButtonMapping(map.KeyCode, map.Description, map.Command));
        }
        else // Replace existing mapping
        {
          for (int index = 0; index < listViewButtons.Items.Count; index++)
          {
            if (listViewButtons.Items[index].SubItems[0].Text.Equals(map.KeyCode, StringComparison.Ordinal))
            {
              listViewButtons.Items[index].SubItems[1].Text = map.Description;
              listViewButtons.Items[index].SubItems[2].Text = map.Command;
            }
          }

          existing.Description = map.Description;
          existing.Command = map.Command;
        }

      }
    }
    void DeleteButtonMapping()
    {
      if (listViewButtons.SelectedIndices.Count != 1)
        return;

      List<ButtonMapping> currentMappings = GetCurrentButtonMappings();
      if (currentMappings == null)
        return;

      ListViewItem item = listViewButtons.SelectedItems[0];
      listViewButtons.Items.Remove(item);

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
    void EditButtonMapping()
    {
      if (listViewButtons.SelectedIndices.Count != 1)
        return;

      ListViewItem item = listViewButtons.SelectedItems[0];

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
    void ClearButtonMappings()
    {
      List<ButtonMapping> currentMappings = GetCurrentButtonMappings();
      if (currentMappings == null)
        return;

      if (MessageBox.Show(this, "Are you sure you want to clear all remote button mappings?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        return;

      currentMappings.Clear();
      listViewButtons.Items.Clear();
    }
    void RemapButtonMapping()
    {
      if (listViewButtons.SelectedIndices.Count != 1)
        return;

      ListViewItem item = listViewButtons.SelectedItems[0];

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
          MessageBox.Show(this, String.Format("{0} is already mapped to {1} ({2})", keyCode, test.Description, test.Command), "Cannot remap", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          return;
        }
      }

      item.SubItems[0].Text = keyCode;

      toModify.KeyCode = keyCode;
    }

    void ClickCopyFrom(object sender, EventArgs e)
    {
      ToolStripMenuItem origin = sender as ToolStripMenuItem;
      if (origin == null)
        return;

      CopyButtonsFrom(origin.Text);
      RefreshButtonList();
    }

    void CopyButtonsFrom(string programName)
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

    void ImportButtons(List<ButtonMapping> buttonMappings)
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

    #endregion Implementation

    #region Controls

    private void listViewButtons_DoubleClick(object sender, EventArgs e)
    {
      EditButtonMapping();
    }
    private void listViewButtons_KeyDown(object sender, KeyEventArgs e)
    {
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
      }
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      this.Close();
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

      string oldFileName = Path.Combine(Common.FolderIRCommands, originItem.Text + Common.FileExtensionIR);
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
        string newFileName = Path.Combine(Common.FolderIRCommands, name + Common.FileExtensionIR);

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

      string oldFileName = Path.Combine(Program.FolderMacros, originItem.Text + Common.FileExtensionMacro);
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
        string newFileName = Path.Combine(Program.FolderMacros, name + Common.FileExtensionMacro);
        File.Move(oldFileName, newFileName);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(ex.Message, "Failed to rename file", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void checkBoxAutoRun_CheckedChanged(object sender, EventArgs e)
    {
      if (checkBoxAutoRun.Checked)
        SystemRegistry.SetAutoRun("Translator", Application.ExecutablePath);
      else
        SystemRegistry.RemoveAutoRun("Translator");
    }

    private void buttonAddEvent_Click(object sender, EventArgs e)
    {
      ListViewItem newItem = 
        new ListViewItem(new string[] { comboBoxEvents.SelectedItem as string, String.Empty });

      listViewEventMap.SelectedIndices.Clear();
      listViewEventMap.Items.Add(newItem);
      newItem.Selected = true;
    }
    private void buttonSetCommand_Click(object sender, EventArgs e)
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
          new BlastIrDelegate(Program.BlastIR),
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
          new BlastIrDelegate(Program.BlastIR),
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

    private void listViewPrograms_DoubleClick(object sender, EventArgs e)
    {
      EditCurrentProgram();
    }
    private void listViewPrograms_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (listViewPrograms.SelectedItems.Count == 0)
        return;

      _selectedProgram = listViewPrograms.SelectedIndices[0];
      RefreshButtonList();
    }

    #endregion Controls

    #region Menus

    private void newToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show(this, "Are you sure you want to start a new configuration?", "New Configuration", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
        return;

      Program.Config = new Configuration();

      RefreshProgramList();
      RefreshButtonList();
      RefreshEventList();
    }
    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
      openFileDialog.Title = "Open settings file ...";

      if (openFileDialog.ShowDialog(this) == DialogResult.OK)
      {
        Configuration newConfig = Configuration.Load(openFileDialog.FileName);

        if (newConfig == null)
          return;

        Program.Config = newConfig;
        
        RefreshProgramList();
        RefreshButtonList();
        RefreshEventList();
      }
    }
    private void importToolStripMenuItem_Click(object sender, EventArgs e)
    {
      openFileDialog.Title = "Import settings ...";

      if (openFileDialog.ShowDialog(this) == DialogResult.OK)
      {
        Configuration newConfig = Configuration.Load(openFileDialog.FileName);

        if (newConfig == null)
          return;

        // TODO: Improve import logic ...

        Program.Config.Events.AddRange(newConfig.Events);
        Program.Config.Programs.AddRange(newConfig.Programs);
        Program.Config.SystemWideMappings.AddRange(newConfig.SystemWideMappings);

        RefreshProgramList();
        RefreshButtonList();
        RefreshEventList();
      }
    }
    private void exportToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
      {
        if (!Configuration.Save(Program.Config, saveFileDialog.FileName))
          MessageBox.Show(this, "Failed to export settings to file", "Export failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
    private void serverToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ServerAddress serverAddress = new ServerAddress(Program.Config.ServerHost);
      if (serverAddress.ShowDialog(this) == DialogResult.OK)
      {
        Program.StopClient();

        Program.Config.ServerHost = serverAddress.ServerHost;

        IPAddress serverIP = Client.GetIPFromName(Program.Config.ServerHost);
        IPEndPoint endPoint = new IPEndPoint(serverIP, IrssComms.Server.DefaultPort);

        Program.StartClient(endPoint);
      }
    }
    private void quitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Application.Exit();
    }
    private void translatorHelpToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        string file = Path.Combine(SystemRegistry.GetInstallFolder(), "IR Server Suite.chm");
        Help.ShowHelp(this, file, HelpNavigator.Topic, "Translator\\index.html");
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(this, ex.Message, "Failed to load help", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
    private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      MessageBox.Show(this, "Translator\nVersion 1.0.4.2 for IR Server Suite\nBy Aaron Dinnage, 2007", "About Translator", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void addProgramToolStripMenuItem_Click(object sender, EventArgs e)
    {
      AddProgram();
    }
    private void editProgramToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (listViewPrograms.SelectedItems.Count == 0)
        return;

      string selectedItem = listViewPrograms.SelectedItems[0].Text;

      EditProgram(selectedItem);
    }
    private void removeProgramToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (listViewPrograms.SelectedItems.Count == 0)
        return;

      string selectedItem = listViewPrograms.SelectedItems[0].Text;

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
    private void contextMenuStripPrograms_Opening(object sender, CancelEventArgs e)
    {
      RefreshProgramsContextMenu();
    }

    private void removeEventToolStripMenuItem_Click(object sender, EventArgs e)
    {
      foreach (ListViewItem listViewItem in listViewEventMap.SelectedItems)
        listViewEventMap.Items.Remove(listViewItem);
    }

    private void contextMenuStripButtonMapping_Opening(object sender, CancelEventArgs e)
    {
      copyButtonsFromToolStripMenuItem.DropDownItems.Clear();

      string selectedItem = listViewPrograms.Items[_selectedProgram].Text;

      if (_selectedProgram > 0)
        copyButtonsFromToolStripMenuItem.DropDownItems.Add(SystemWide, Properties.Resources.WinLogo, new EventHandler(ClickCopyFrom));

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

        copyButtonsFromToolStripMenuItem.DropDownItems.Add(programSettings.Name, image, new EventHandler(ClickCopyFrom));
      }
    }

    private void newButtonToolStripMenuItem_Click(object sender, EventArgs e)
    {
      NewButtonMapping();
    }
    private void editButtonToolStripMenuItem_Click(object sender, EventArgs e)
    {
      EditButtonMapping();
    }
    private void deleteButtonToolStripMenuItem_Click(object sender, EventArgs e)
    {
      DeleteButtonMapping();
    }
    private void clearButtonsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ClearButtonMappings();
    }
    private void remapButtonToolStripMenuItem_Click(object sender, EventArgs e)
    {
      RemapButtonMapping();
    }

    #endregion Menus

    private void toolStripButtonNewMapping_Click(object sender, EventArgs e)
    {
      NewButtonMapping();
    }
    private void toolStripButtonEditMapping_Click(object sender, EventArgs e)
    {
      EditButtonMapping();
    }
    private void toolStripButtonDeleteMapping_Click(object sender, EventArgs e)
    {
      DeleteButtonMapping();
    }
    private void toolStripButtonDeleteAllMappings_Click(object sender, EventArgs e)
    {
      ClearButtonMappings();
    }
    private void toolStripButtonRemapMapping_Click(object sender, EventArgs e)
    {
      RemapButtonMapping();
    }

    private void toolStripButtonNewMacro_Click(object sender, EventArgs e)
    {
      MacroEditor macroEditor = new MacroEditor();
      macroEditor.ShowDialog(this);

      RefreshMacroList();
    }
    private void toolStripButtonEditMacro_Click(object sender, EventArgs e)
    {
      EditMacro();
    }
    private void toolStripButtonDeleteMacro_Click(object sender, EventArgs e)
    {
      if (listViewMacro.SelectedItems.Count != 1)
        return;

      string file = listViewMacro.SelectedItems[0].Text;
      string fileName = Path.Combine(Program.FolderMacros, file + Common.FileExtensionMacro);
      if (File.Exists(fileName))
      {
        if (MessageBox.Show(this, String.Format("Are you sure you want to delete \"{0}\"?", file), "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
          File.Delete(fileName);
          listViewMacro.Items.Remove(listViewMacro.SelectedItems[0]);
        }
      }
      else
      {
        MessageBox.Show(this, "File not found: " + fileName, "Macro file missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }
    private void toolStripButtonTestMacro_Click(object sender, EventArgs e)
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
    private void toolStripButtonCreateShortcutForMacro_Click(object sender, EventArgs e)
    {
      if (listViewMacro.SelectedItems.Count != 1)
        return;

      string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
      string macroName = listViewMacro.SelectedItems[0].Text;
      string shortcutPath = Path.Combine(desktopPath, String.Format("Macro - {0}.lnk", macroName));

      MSjogren.Samples.ShellLink.ShellShortcut shortcut = new MSjogren.Samples.ShellLink.ShellShortcut(shortcutPath);

      string translatorFolder = Path.Combine(SystemRegistry.GetInstallFolder(), "Translator");

      //shortcut.Arguments        = String.Format("-MACRO \"{0}\"", Path.Combine(Program.FolderMacros, macroName + Common.FileExtensionMacro));
      shortcut.Arguments        = String.Format("-MACRO \"{0}\"", macroName);
      shortcut.Description      = "Launch Macro: " + macroName;
      shortcut.Path             = Path.Combine(translatorFolder, "Translator.exe");
      shortcut.WorkingDirectory = translatorFolder;
      shortcut.WindowStyle      = ProcessWindowStyle.Normal;

      shortcut.Save();
    }

    private void toolStripButtonNewIR_Click(object sender, EventArgs e)
    {
      _learnIR = new LearnIR(
        new LearnIrDelegate(Program.LearnIR),
        new BlastIrDelegate(Program.BlastIR),
        Program.TransceiverInformation.Ports);

      _learnIR.ShowDialog(this);

      _learnIR = null;

      RefreshIRList();
    }
    private void toolStripButtonEditIR_Click(object sender, EventArgs e)
    {
      EditIR();
    }
    private void toolStripButtonDeleteIR_Click(object sender, EventArgs e)
    {
      if (listViewIR.SelectedItems.Count != 1)
        return;

      string file = listViewIR.SelectedItems[0].Text;
      string fileName = Path.Combine(Common.FolderIRCommands, file + Common.FileExtensionIR);
      if (File.Exists(fileName))
      {
        if (MessageBox.Show(this, String.Format("Are you sure you want to delete \"{0}\"?", file), "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
          File.Delete(fileName);
          listViewIR.Items.Remove(listViewIR.SelectedItems[0]);
        }
      }
      else
      {
        MessageBox.Show(this, "File not found: " + fileName, "IR file missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void labelProgramsAdd_Click(object sender, EventArgs e)
    {
      AddProgram();
    }
    private void labelProgramsEdit_Click(object sender, EventArgs e)
    {
      if (listViewPrograms.SelectedItems.Count == 0)
        return;

      string selectedItem = listViewPrograms.SelectedItems[0].Text;

      EditProgram(selectedItem);
    }
    private void labelProgramsDelete_Click(object sender, EventArgs e)
    {
      if (listViewPrograms.SelectedItems.Count == 0)
        return;

      string selectedItem = listViewPrograms.SelectedItems[0].Text;

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

  }

}
