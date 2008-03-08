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
using IrssUtils.Forms;

namespace Translator
{

  partial class MacroEditor : Form
  {

    #region Constructor

    /// <summary>
    /// Creates a Macro Editor windows form.
    /// </summary>
    public MacroEditor()
    {
      InitializeComponent();

      textBoxName.Text    = "New";
      textBoxName.Enabled = true;
    }

    /// <summary>
    /// Creates a Macro Editor windows form.
    /// </summary>
    /// <param name="name">The name of an existing macro.</param>
    public MacroEditor(string name)
    {
      if (String.IsNullOrEmpty(name))
        throw new ArgumentNullException("name");

      InitializeComponent();

      textBoxName.Text    = name;
      textBoxName.Enabled = false;

      string fileName = Path.Combine(Program.FolderMacros, name + Common.FileExtensionMacro);
      string[] commands = IrssMacro.ReadFromFile(fileName);

      listBoxMacro.Items.AddRange(commands);
    }

    #endregion Constructor

    #region Implementation

    /// <summary>
    /// Refreshes the macro command list.
    /// </summary>
    void RefreshCommandList()
    {
      comboBoxCommands.Items.Clear();

      comboBoxCommands.Items.Add(Common.UITextRun);
      comboBoxCommands.Items.Add(Common.UITextPause);
      comboBoxCommands.Items.Add(Common.UITextSerial);
      comboBoxCommands.Items.Add(Common.UITextWindowMsg);
      comboBoxCommands.Items.Add(Common.UITextTcpMsg);
      comboBoxCommands.Items.Add(Common.UITextHttpMsg);
      comboBoxCommands.Items.Add(Common.UITextKeys);
      comboBoxCommands.Items.Add(Common.UITextMouse);
      comboBoxCommands.Items.Add(Common.UITextEject);
      comboBoxCommands.Items.Add(Common.UITextPopup);
      comboBoxCommands.Items.Add(Common.UITextVirtualKB);
      comboBoxCommands.Items.Add(Common.UITextSmsKB);
      comboBoxCommands.Items.Add(Common.UITextBeep);
      comboBoxCommands.Items.Add(Common.UITextSound);
      comboBoxCommands.Items.Add(Common.UITextDisplay);
      comboBoxCommands.Items.Add(Common.UITextStandby);
      comboBoxCommands.Items.Add(Common.UITextHibernate);
      comboBoxCommands.Items.Add(Common.UITextReboot);
      comboBoxCommands.Items.Add(Common.UITextShutdown);
      comboBoxCommands.Items.Add(Common.UITextLabel);
      comboBoxCommands.Items.Add(Common.UITextGotoLabel);
      comboBoxCommands.Items.Add(Common.UITextIf);
      comboBoxCommands.Items.Add(Common.UITextSetVar);
      comboBoxCommands.Items.Add(Common.UITextClearVars);
      comboBoxCommands.Items.Add(Common.UITextLoadVars);
      comboBoxCommands.Items.Add(Common.UITextSaveVars);

      string[] macroList = IrssMacro.GetMacroList(Program.FolderMacros, true);
      if (macroList != null && macroList.Length > 0)
        comboBoxCommands.Items.AddRange(macroList);

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

      try
      {
        string selected = comboBoxCommands.SelectedItem as string;
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
        else if (selected.Equals(Common.UITextRun, StringComparison.OrdinalIgnoreCase))
        {
          ExternalProgram externalProgram = new ExternalProgram();
          if (externalProgram.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixRun + externalProgram.CommandString;
        }
        else if (selected.Equals(Common.UITextPause, StringComparison.OrdinalIgnoreCase))
        {
          PauseTime pauseTime = new PauseTime();
          if (pauseTime.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixPause + pauseTime.Time.ToString();
        }
        else if (selected.Equals(Common.UITextSerial, StringComparison.OrdinalIgnoreCase))
        {
          SerialCommand serialCommand = new SerialCommand();
          if (serialCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixSerial + serialCommand.CommandString;
        }
        else if (selected.Equals(Common.UITextWindowMsg, StringComparison.OrdinalIgnoreCase))
        {
          MessageCommand messageCommand = new MessageCommand();
          if (messageCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixWindowMsg + messageCommand.CommandString;
        }
        else if (selected.Equals(Common.UITextTcpMsg, StringComparison.OrdinalIgnoreCase))
        {
          TcpMessageCommand tcpMessageCommand = new TcpMessageCommand();
          if (tcpMessageCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixTcpMsg + tcpMessageCommand.CommandString;
        }
        else if (selected.Equals(Common.UITextHttpMsg, StringComparison.OrdinalIgnoreCase))
        {
          HttpMessageCommand httpMessageCommand = new HttpMessageCommand();
          if (httpMessageCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixHttpMsg + httpMessageCommand.CommandString;
        }
        else if (selected.Equals(Common.UITextKeys, StringComparison.OrdinalIgnoreCase))
        {
          KeysCommand keysCommand = new KeysCommand();
          if (keysCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixKeys + keysCommand.CommandString;
        }
        else if (selected.Equals(Common.UITextMouse, StringComparison.OrdinalIgnoreCase))
        {
          MouseCommand mouseCommand = new MouseCommand();
          if (mouseCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixMouse + mouseCommand.CommandString;
        }
        else if (selected.Equals(Common.UITextEject, StringComparison.OrdinalIgnoreCase))
        {
          EjectCommand ejectCommand = new EjectCommand();
          if (ejectCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixEject + ejectCommand.CommandString;
        }
        else if (selected.Equals(Common.UITextPopup, StringComparison.OrdinalIgnoreCase))
        {
          PopupMessage popupMessage = new PopupMessage();
          if (popupMessage.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixPopup + popupMessage.CommandString;
        }
        else if (selected.Equals(Common.UITextVirtualKB, StringComparison.OrdinalIgnoreCase))
        {
          newCommand = Common.CmdPrefixVirtualKB;
        }
        else if (selected.Equals(Common.UITextSmsKB, StringComparison.OrdinalIgnoreCase))
        {
          newCommand = Common.CmdPrefixSmsKB;
        }
        else if (selected.Equals(Common.UITextBeep, StringComparison.OrdinalIgnoreCase))
        {
          BeepCommand beepCommand = new BeepCommand();
          if (beepCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixBeep + beepCommand.CommandString;
        }
        else if (selected.Equals(Common.UITextSound, StringComparison.OrdinalIgnoreCase))
        {
          OpenFileDialog openFileDialog = new OpenFileDialog();
          openFileDialog.Filter = "Wave Files|*.wav";
          openFileDialog.Multiselect = false;

          if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixSound + openFileDialog.FileName;
        }
        else if (selected.Equals(Common.UITextDisplay, StringComparison.OrdinalIgnoreCase))
        {
          DisplayModeCommand displayModeCommand = new DisplayModeCommand();
          if (displayModeCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixDisplay + displayModeCommand.CommandString;
        }
        else if (selected.Equals(Common.UITextStandby, StringComparison.OrdinalIgnoreCase))
        {
          newCommand = Common.CmdPrefixStandby;
        }
        else if (selected.Equals(Common.UITextHibernate, StringComparison.OrdinalIgnoreCase))
        {
          newCommand = Common.CmdPrefixHibernate;
        }
        else if (selected.Equals(Common.UITextReboot, StringComparison.OrdinalIgnoreCase))
        {
          newCommand = Common.CmdPrefixReboot;
        }
        else if (selected.Equals(Common.UITextShutdown, StringComparison.OrdinalIgnoreCase))
        {
          newCommand = Common.CmdPrefixShutdown;
        }
        else if (selected.StartsWith(Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
        {
          BlastCommand blastCommand = new BlastCommand(
            new BlastIrDelegate(Program.BlastIR),
            Common.FolderIRCommands,
            Program.TransceiverInformation.Ports,
            selected.Substring(Common.CmdPrefixBlast.Length));

          if (blastCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixBlast + blastCommand.CommandString;
        }
        else if (selected.StartsWith(Common.CmdPrefixMacro, StringComparison.OrdinalIgnoreCase))
        {
          newCommand = selected;
        }
        else
        {
          throw new IrssUtils.Exceptions.CommandStructureException(String.Format("Unknown macro command ({0})", selected));
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

    private void buttonTest_Click(object sender, EventArgs e)
    {
      string name = textBoxName.Text.Trim();

      if (name.Length == 0)
      {
        MessageBox.Show(this, "You must supply a name for this Macro", "Name missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        textBoxName.Focus();
        return;
      }

      if (!Common.IsValidFileName(name))
      {
        MessageBox.Show(this, "You must supply a valid name for this Macro", "Invalid name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        textBoxName.Focus();
        return;
      }

      try
      {
        string[] commands = new string[listBoxMacro.Items.Count];
        int index = 0;
        foreach (string item in listBoxMacro.Items)
          commands[index++] = item;

        string fileName = Path.Combine(Program.FolderMacros, name + Common.FileExtensionMacro);

        IrssMacro.WriteToFile(fileName, commands);

        Program.ProcessCommand(Common.CmdPrefixMacro + name, false);
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

      if (!Common.IsValidFileName(name))
      {
        MessageBox.Show(this, "You must supply a valid name for this Macro", "Invalid name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        textBoxName.Focus();
        return;
      }

      try
      {
        string[] commands = new string[listBoxMacro.Items.Count];
        int index = 0;
        foreach (string item in listBoxMacro.Items)
          commands[index++] = item;

        string fileName = Path.Combine(Program.FolderMacros, name + Common.FileExtensionMacro);

        IrssMacro.WriteToFile(fileName, commands);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(this, ex.Message, "Failed writing macro to file", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void listBoxCommandSequence_DoubleClick(object sender, EventArgs e)
    {
      if (listBoxMacro.SelectedIndex == -1)
        return;

      try
      {
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
          VariablesFileDialog varsFileDialog = new VariablesFileDialog(selected.Substring(Common.CmdPrefixLoadVars.Length));
          if (varsFileDialog.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixLoadVars + varsFileDialog.FileName;
        }
        else if (selected.StartsWith(Common.CmdPrefixSaveVars, StringComparison.OrdinalIgnoreCase))
        {
          VariablesFileDialog varsFileDialog = new VariablesFileDialog(selected.Substring(Common.CmdPrefixSaveVars.Length));
          if (varsFileDialog.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixSaveVars + varsFileDialog.FileName;
        }
        else if (selected.StartsWith(Common.CmdPrefixRun, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitRunCommand(selected.Substring(Common.CmdPrefixRun.Length));

          ExternalProgram executeProgram = new ExternalProgram(commands);
          if (executeProgram.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixRun + executeProgram.CommandString;
        }
        else if (selected.StartsWith(Common.CmdPrefixPause, StringComparison.OrdinalIgnoreCase))
        {
          PauseTime pauseTime = new PauseTime(int.Parse(selected.Substring(Common.CmdPrefixPause.Length)));
          if (pauseTime.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixPause + pauseTime.Time.ToString();
        }
        else if (selected.StartsWith(Common.CmdPrefixSerial, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitSerialCommand(selected.Substring(Common.CmdPrefixSerial.Length));

          SerialCommand serialCommand = new SerialCommand(commands);
          if (serialCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixSerial + serialCommand.CommandString;
        }
        else if (selected.StartsWith(Common.CmdPrefixWindowMsg, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitWindowMessageCommand(selected.Substring(Common.CmdPrefixWindowMsg.Length));

          MessageCommand messageCommand = new MessageCommand(commands);
          if (messageCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixWindowMsg + messageCommand.CommandString;
        }
        else if (selected.StartsWith(Common.CmdPrefixTcpMsg, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitTcpMessageCommand(selected.Substring(Common.CmdPrefixTcpMsg.Length));

          TcpMessageCommand tcpMessageCommand = new TcpMessageCommand(commands);
          if (tcpMessageCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixTcpMsg + tcpMessageCommand.CommandString;
        }
        else if (selected.StartsWith(Common.CmdPrefixHttpMsg, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitHttpMessageCommand(selected.Substring(Common.CmdPrefixHttpMsg.Length));
        
          HttpMessageCommand httpMessageCommand = new HttpMessageCommand(commands);
          if (httpMessageCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixHttpMsg + httpMessageCommand.CommandString;
        }
        else if (selected.StartsWith(Common.CmdPrefixKeys, StringComparison.OrdinalIgnoreCase))
        {
          KeysCommand keysCommand = new KeysCommand(selected.Substring(Common.CmdPrefixKeys.Length));
          if (keysCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixKeys + keysCommand.CommandString;
        }
        else if (selected.StartsWith(Common.CmdPrefixMouse, StringComparison.OrdinalIgnoreCase))
        {
          MouseCommand mouseCommand = new MouseCommand(selected.Substring(Common.CmdPrefixMouse.Length));
          if (mouseCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixMouse + mouseCommand.CommandString;
        }
        else if (selected.StartsWith(Common.CmdPrefixEject, StringComparison.OrdinalIgnoreCase))
        {
          EjectCommand ejectCommand = new EjectCommand(selected.Substring(Common.CmdPrefixEject.Length));
          if (ejectCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixEject + ejectCommand.CommandString;
        }
        else if (selected.StartsWith(Common.CmdPrefixPopup, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitPopupCommand(selected.Substring(Common.CmdPrefixPopup.Length));

          PopupMessage popupMessage = new PopupMessage(commands);
          if (popupMessage.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixPopup + popupMessage.CommandString;
        }
        else if (selected.StartsWith(Common.CmdPrefixBeep, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitBeepCommand(selected.Substring(Common.CmdPrefixBeep.Length));

          BeepCommand beepCommand = new BeepCommand(commands);
          if (beepCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixBeep + beepCommand.CommandString;
        }
        else if (selected.StartsWith(Common.CmdPrefixSound, StringComparison.OrdinalIgnoreCase))
        {
          OpenFileDialog openFileDialog = new OpenFileDialog();
          openFileDialog.Filter = "Wave Files|*.wav";
          openFileDialog.Multiselect = false;
          openFileDialog.FileName = selected.Substring(Common.CmdPrefixSound.Length);

          if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixSound + openFileDialog.FileName;
        }
        else if (selected.StartsWith(Common.CmdPrefixDisplay, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitDisplayModeCommand(selected.Substring(Common.CmdPrefixDisplay.Length));

          DisplayModeCommand displayModeCommand = new DisplayModeCommand(commands);
          if (displayModeCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixDisplay + displayModeCommand.CommandString;
        }
        else if (selected.StartsWith(Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitBlastCommand(selected.Substring(Common.CmdPrefixBlast.Length));

          BlastCommand blastCommand = new BlastCommand(
            new BlastIrDelegate(Program.BlastIR),
            Common.FolderIRCommands,
            Program.TransceiverInformation.Ports,
            commands);

          if (blastCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixBlast + blastCommand.CommandString;
        }

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
