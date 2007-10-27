using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
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

    #endregion Variables

    #region Constructor

    public MainForm()
    {
      InitializeComponent();

      RefreshProgramList();
      RefreshButtonList();
      RefreshEventList();
      RefreshEventCommands();
      RefreshIRList();
      RefreshMacroList();

      try
      {
        checkBoxAutoRun.Checked = SystemRegistry.GetAutoRun("Translator");
      }
      catch { }
    }

    #endregion Constructor

    void RefreshProgramList()
    {
      comboBoxProgram.Items.Clear();
      comboBoxProgram.Items.Add("System wide");

      foreach (ProgramSettings progSettings in Program.Config.Programs)
        comboBoxProgram.Items.Add(progSettings.Name);

      comboBoxProgram.SelectedIndex = 0;

      Program.UpdateNotifyMenu();
    }
    void RefreshButtonList()
    {
      listViewButtons.Items.Clear();

      List<ButtonMapping> current = GetCurrentSettings();
      if (current == null)
        return;

      foreach (ButtonMapping map in current)
      {
        listViewButtons.Items.Add(
          new ListViewItem(
            new string[] { map.KeyCode.ToString(), map.Description, map.Command }
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
        if (eventName != "None")
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

      list = Program.GetMacroList(true);
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

      string[] macroList = Program.GetMacroList(false);
      if (macroList != null && macroList.Length > 0)
        foreach (string macroFile in macroList)
          listViewMacro.Items.Add(macroFile);

      Program.UpdateNotifyMenu();
    }

    List<ButtonMapping> GetCurrentSettings()
    {
      string selectedItem = comboBoxProgram.SelectedItem as string;

      if (selectedItem == null)
      {
        return null;
      }
      else if (selectedItem == "System wide")
      {
        return Program.Config.SystemWideMappings;
      }
      else
      {
        foreach (ProgramSettings progSettings in Program.Config.Programs)
          if (progSettings.Name == selectedItem)
            return progSettings.ButtonMappings;        
      }

      return null;
    }

    bool EditProgram(ProgramSettings progSettings)
    {
      EditProgramForm editProg = new EditProgramForm(progSettings);

      if (editProg.ShowDialog(this) == DialogResult.OK)
      {
        progSettings.Name = editProg.DisplayName;
        progSettings.Filename = editProg.Filename;
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

    void EditIR()
    {
      if (listViewIR.SelectedItems.Count != 1)
        return;

      string command = listViewIR.SelectedItems[0].Text;
      string fileName = Common.FolderIRCommands + command + Common.FileExtensionIR;

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
      string fileName = Program.FolderMacros + command + Common.FileExtensionMacro;

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

    #region Controls

    private void listViewButtons_DoubleClick(object sender, EventArgs e)
    {
      buttonModify_Click(null, null);
    }
    private void listViewButtons_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.KeyData)
      {
        case Keys.OemMinus:
        case Keys.Delete:
          buttonDelete_Click(null, null);
          break;

        case Keys.F2:
        case Keys.Enter:
          buttonModify_Click(null, null);
          break;

        case Keys.Oemplus:
        case Keys.Insert:
          buttonNew_Click(null, null);
          break;
      }
    }

    private void comboBoxProgram_SelectedIndexChanged(object sender, EventArgs e)
    {
      if ((comboBoxProgram.SelectedItem as string) == "System wide")
      {
        buttonRemoveProgram.Enabled = false;
        buttonEditProgram.Enabled = false;
      }
      else
      {
        buttonRemoveProgram.Enabled = true;
        buttonEditProgram.Enabled = true;
      }

      RefreshButtonList();
    }

    private void buttonAddProgram_Click(object sender, EventArgs e)
    {
      ProgramSettings progSettings = new ProgramSettings();

      if (EditProgram(progSettings))
      {
        // TODO: check for duplicates in Program.Config.Programs ...

        comboBoxProgram.Items.Add(progSettings.Name);
        Program.Config.Programs.Add(progSettings);
        comboBoxProgram.SelectedIndex = comboBoxProgram.Items.Count - 1;
      }
    }
    private void buttonRemoveProgram_Click(object sender, EventArgs e)
    {
      ProgramSettings progSettings = null;

      foreach (ProgramSettings settings in Program.Config.Programs)
      {
        if (settings.Name == comboBoxProgram.SelectedItem as string)
        {
          progSettings = settings;
        }
      }

      if (progSettings != null)
      {
        if (MessageBox.Show(this, String.Format("Are you sure you want to remove all mappings for {0}?", progSettings.Name), String.Format("Remove {0}?", progSettings.Name), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
          Program.Config.Programs.Remove(progSettings);
          comboBoxProgram.Items.Remove(comboBoxProgram.SelectedItem);
          comboBoxProgram.SelectedIndex = 0;

          Program.UpdateNotifyMenu();
        }
      }
    }
    private void buttonEditProgram_Click(object sender, EventArgs e)
    {
      foreach (ProgramSettings progSettings in Program.Config.Programs)
        if (progSettings.Name == comboBoxProgram.SelectedItem as string)
          if (EditProgram(progSettings))
          {
            comboBoxProgram.Items.Remove(comboBoxProgram.SelectedItem);
            comboBoxProgram.Items.Add(progSettings.Name);
            comboBoxProgram.SelectedItem = progSettings.Name;
          }
    }

    private void buttonNew_Click(object sender, EventArgs e)
    {
      GetKeyCodeForm getKeyCode = new GetKeyCodeForm();
      getKeyCode.ShowDialog(this);

      string keyCode = getKeyCode.KeyCode;

      if (String.IsNullOrEmpty(keyCode))
        return;

      List<ButtonMapping> currentMapping = GetCurrentSettings();
      ButtonMappingForm map = null;
      ButtonMapping existing = null;

      foreach (ButtonMapping test in currentMapping)
      {
        if (keyCode == test.KeyCode)
        {
          existing = test;
          map = new ButtonMappingForm(test.KeyCode, test.Description, test.Command);
          break;
        }
      }

      if (map == null)
      {
        string description = String.Empty;
        try
        {
          MceButton temp = (MceButton)Enum.Parse(typeof(MceButton), keyCode, true);
          description = Enum.GetName(typeof(MceButton), temp);
        }
        catch
        {
          // keyCode did not fall within MceButton enum
        }

        map = new ButtonMappingForm(keyCode, description, String.Empty);
      }

      if (map.ShowDialog(this) == DialogResult.OK)
      {
        if (existing == null)
        {
          listViewButtons.Items.Add(
            new ListViewItem(
            new string[] { map.KeyCode.ToString(), map.Description, map.Command }
            ));

          currentMapping.Add(new ButtonMapping(map.KeyCode, map.Description, map.Command));
        }
        else
        {
          for (int index = 0; index < listViewButtons.Items.Count; index++)
          {
            if (listViewButtons.Items[index].SubItems[0].Text == map.KeyCode.ToString())
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
    private void buttonDelete_Click(object sender, EventArgs e)
    {
      if (listViewButtons.SelectedIndices.Count != 1)
        return;

      ListViewItem item = listViewButtons.SelectedItems[0];

      listViewButtons.Items.Remove(item);

      ButtonMapping toRemove = null;
      foreach (ButtonMapping test in GetCurrentSettings())
      {
        if (test.KeyCode.ToString() == item.SubItems[0].Text)
        {
          toRemove = test;
          break;
        }
      }
      
      GetCurrentSettings().Remove(toRemove);
    }
    private void buttonModify_Click(object sender, EventArgs e)
    {
      if (listViewButtons.SelectedIndices.Count != 1)
        return;

      ListViewItem item = listViewButtons.SelectedItems[0];

      List<ButtonMapping> currentMapping = GetCurrentSettings();

      foreach (ButtonMapping test in currentMapping)
      {
        if (item.SubItems[0].Text == test.KeyCode.ToString())
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
    private void buttonClear_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show(this, "Are you sure you want to clear all remote button mappings?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        return;

      listViewButtons.Items.Clear();
      GetCurrentSettings().Clear();
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

    private void buttonNewIR_Click(object sender, EventArgs e)
    {
      _learnIR = new LearnIR(
        new LearnIrDelegate(Program.LearnIR),
        new BlastIrDelegate(Program.BlastIR),
        Program.TransceiverInformation.Ports);

      _learnIR.ShowDialog(this);

      _learnIR = null;
      
      RefreshIRList();
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
      string fileName = Common.FolderIRCommands + file + Common.FileExtensionIR;
      if (File.Exists(fileName))
      {
        if (MessageBox.Show(this, "Are you sure you want to delete \"" + file + "\"?", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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

    private void buttonNewMacro_Click(object sender, EventArgs e)
    {
      MacroEditor macroEditor = new MacroEditor();
      macroEditor.ShowDialog(this);

      RefreshMacroList();
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
      string fileName = Program.FolderMacros + file + Common.FileExtensionMacro;
      if (File.Exists(fileName))
      {
        if (MessageBox.Show(this, "Are you sure you want to delete \"" + file + "\"?", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
    private void buttonTestMacro_Click(object sender, EventArgs e)
    {
      if (listViewMacro.SelectedItems.Count != 1)
        return;

      string fileName = Program.FolderMacros + listViewMacro.SelectedItems[0].Text + Common.FileExtensionMacro;

      try
      {
        Program.ProcessMacro(fileName);
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Test failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

      string oldFileName = Common.FolderIRCommands + originItem.Text + Common.FileExtensionIR;
      if (!File.Exists(oldFileName))
      {
        MessageBox.Show("File not found: " + oldFileName, "Cannot rename, Original file not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
        e.CancelEdit = true;
        return;
      }

      if (!Common.IsValidFileName(e.Label))
      {
        MessageBox.Show("File name not valid: " + e.Label, "Cannot rename, New file name not valid", MessageBoxButtons.OK, MessageBoxIcon.Error);
        e.CancelEdit = true;
        return;
      }

      try
      {
        string newFileName = Common.FolderIRCommands + e.Label + Common.FileExtensionIR;

        File.Move(oldFileName, newFileName);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
        MessageBox.Show(ex.ToString(), "Failed to rename file", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

      string oldFileName = Program.FolderMacros + originItem.Text + Common.FileExtensionMacro;
      if (!File.Exists(oldFileName))
      {
        MessageBox.Show("File not found: " + oldFileName, "Cannot rename, Original file not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
        e.CancelEdit = true;
        return;
      }

      if (!Common.IsValidFileName(e.Label))
      {
        MessageBox.Show("File name not valid: " + e.Label, "Cannot rename, New file name not valid", MessageBoxButtons.OK, MessageBoxIcon.Error);
        e.CancelEdit = true;
        return;
      }

      try
      {
        string newFileName = Program.FolderMacros + e.Label + Common.FileExtensionMacro;

        File.Move(oldFileName, newFileName);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
        MessageBox.Show(ex.ToString(), "Failed to rename file", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
      listViewEventMap.Items.Add(
        new ListViewItem(
          new string[] { comboBoxEvents.SelectedItem as string, String.Empty }
        )
      );
    }
    private void buttonSetCommand_Click(object sender, EventArgs e)
    {
      if (comboBoxCommands.SelectedIndex == -1 || listViewEventMap.SelectedItems.Count == 0)
        return;

      string selected = comboBoxCommands.SelectedItem as string;
      string command = String.Empty;

      if (selected == Common.UITextRun)
      {
        ExternalProgram externalProgram = new ExternalProgram(false);

        if (externalProgram.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixRun + externalProgram.CommandString;
      }
      else if (selected == Common.UITextSerial)
      {
        SerialCommand serialCommand = new SerialCommand();
        if (serialCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixSerial + serialCommand.CommandString;
      }
      else if (selected == Common.UITextWindowMsg)
      {
        MessageCommand messageCommand = new MessageCommand();
        if (messageCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixWindowMsg + messageCommand.CommandString;
      }
      else if (selected == Common.UITextKeys)
      {
        KeysCommand keysCommand = new KeysCommand();
        if (keysCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixKeys + keysCommand.CommandString;
      }
      else if (selected.StartsWith(Common.CmdPrefixBlast))
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
      else if (selected.StartsWith(Common.CmdPrefixMacro))
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

      if (command.StartsWith(Common.CmdPrefixRun))
      {
        string[] commands = Common.SplitRunCommand(command.Substring(Common.CmdPrefixRun.Length));
        ExternalProgram externalProgram = new ExternalProgram(commands, false);
        if (externalProgram.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixRun + externalProgram.CommandString;
      }
      else if (command.StartsWith(Common.CmdPrefixSerial))
      {
        string[] commands = Common.SplitSerialCommand(command.Substring(Common.CmdPrefixSerial.Length));
        SerialCommand serialCommand = new SerialCommand(commands);
        if (serialCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixSerial + serialCommand.CommandString;
      }
      else if (command.StartsWith(Common.CmdPrefixWindowMsg))
      {
        string[] commands = Common.SplitWindowMessageCommand(command.Substring(Common.CmdPrefixWindowMsg.Length));
        MessageCommand messageCommand = new MessageCommand(commands);
        if (messageCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixWindowMsg + messageCommand.CommandString;
      }
      else if (command.StartsWith(Common.CmdPrefixKeys))
      {
        KeysCommand keysCommand = new KeysCommand(command.Substring(Common.CmdPrefixKeys.Length));
        if (keysCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        command = Common.CmdPrefixKeys + keysCommand.CommandString;
      }
      else if (command.StartsWith(Common.CmdPrefixBlast))
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

    #endregion Controls

    #region Menu

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
        Help.ShowHelp(this, SystemRegistry.GetInstallFolder() + "\\IR Server Suite.chm", HelpNavigator.Topic, "Translator\\index.html");
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Failed to load help", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
    private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      MessageBox.Show(this, "Translator\nVersion 1.0.3.5 for IR Server Suite\nBy Aaron Dinnage, 2007", "About Translator", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    #endregion Menu

  }

}
