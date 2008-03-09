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

using MediaPortal.GUI.Library;
using MediaPortal.Util;

using IrssUtils;
using IrssUtils.Forms;
using MPUtils;
using MPUtils.Forms;

namespace MediaPortal.Plugins
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
      : this()
    {
      if (String.IsNullOrEmpty(name))
        throw new ArgumentNullException("name");

      textBoxName.Text    = name;
      textBoxName.Enabled = false;

      string fileName = MPBlastZonePlugin.FolderMacros + name + Common.FileExtensionMacro;
      ReadFromFile(fileName);
    }

    #endregion Constructor

    #region Implementation

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
      comboBoxCommands.Items.Add(Common.UITextGotoScreen);
      comboBoxCommands.Items.Add(Common.UITextInputLayer);
      //comboBoxCommands.Items.Add(Common.UITextWindowState);
      comboBoxCommands.Items.Add(Common.UITextFocus);
      comboBoxCommands.Items.Add(Common.UITextExit);
      comboBoxCommands.Items.Add(Common.UITextSendMPAction);
      comboBoxCommands.Items.Add(Common.UITextSendMPMsg);
      comboBoxCommands.Items.Add(Common.UITextVirtualKB);
      comboBoxCommands.Items.Add(Common.UITextSmsKB);
      comboBoxCommands.Items.Add(Common.UITextBeep);
      comboBoxCommands.Items.Add(Common.UITextSound);
      comboBoxCommands.Items.Add(Common.UITextDisplayMode);
      comboBoxCommands.Items.Add(Common.UITextStandby);
      comboBoxCommands.Items.Add(Common.UITextHibernate);
      comboBoxCommands.Items.Add(Common.UITextReboot);
      comboBoxCommands.Items.Add(Common.UITextShutdown);

      string[] fileList = MPBlastZonePlugin.GetFileList(true);
      if (fileList != null && fileList.Length > 0)
        comboBoxCommands.Items.AddRange(fileList);
    }

    /// <summary>
    /// Write the macro in the listBox to a macro name provided.
    /// </summary>
    /// <param name="fileName">Name of Macro to write (macro name, not file path).</param>
    void WriteToFile(string fileName)
    {
      try
      {
        using (XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.UTF8))
        {
          writer.Formatting = Formatting.Indented;
          writer.WriteStartDocument(true);
          writer.WriteStartElement("macro");

          foreach (string item in listBoxMacro.Items)
          {
            writer.WriteStartElement("item");
            writer.WriteAttributeString("command", item);
            writer.WriteEndElement();
          }

          writer.WriteEndElement();
          writer.WriteEndDocument();
        }
      }
      catch (Exception ex)
      {
        Log.Error(ex);
      }
    }

    /// <summary>
    /// Read a macro into the listBox from the macro name provided.
    /// </summary>
    /// <param name="fileName">Name of Macro to read (macro name, not file path).</param>
    void ReadFromFile(string fileName)
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(fileName);

        XmlNodeList commandSequence = doc.DocumentElement.SelectNodes("item");

        listBoxMacro.Items.Clear();

        foreach (XmlNode item in commandSequence)
          listBoxMacro.Items.Add(item.Attributes["command"].Value);
      }
      catch (Exception ex)
      {
        Log.Error(ex);
      }
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

        if (selected.Equals(Common.UITextRun, StringComparison.OrdinalIgnoreCase))
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
        else if (selected.Equals(Common.UITextGotoScreen, StringComparison.OrdinalIgnoreCase))
        {
          GoToScreen goToScreen = new GoToScreen();
          if (goToScreen.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixGotoScreen + goToScreen.CommandString;
        }
        else if (selected.Equals(Common.UITextInputLayer, StringComparison.OrdinalIgnoreCase))
        {
          newCommand = Common.CmdPrefixInputLayer;
        }
        /*
        else if (selected.Equals(Common.UITextWindowState, StringComparison.OrdinalIgnoreCase))
        {
          newCommand = Common.CmdPrefixWindowState;
        }
        */
        else if (selected.Equals(Common.UITextFocus, StringComparison.OrdinalIgnoreCase))
        {
          newCommand = Common.CmdPrefixFocus;
        }
        else if (selected.Equals(Common.UITextExit, StringComparison.OrdinalIgnoreCase))
        {
          newCommand = Common.CmdPrefixExit;
        }
        else if (selected.Equals(Common.UITextSendMPAction, StringComparison.OrdinalIgnoreCase))
        {
          MPAction edit = new MPAction();
          if (edit.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixSendMPAction + edit.CommandString;
        }
        else if (selected.Equals(Common.UITextSendMPMsg, StringComparison.OrdinalIgnoreCase))
        {
          MPMessage edit = new MPMessage();
          if (edit.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixSendMPMsg + edit.CommandString;
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
        else if (selected.Equals(Common.UITextDisplayMode, StringComparison.OrdinalIgnoreCase))
        {
          DisplayModeCommand displayModeCommand = new DisplayModeCommand();
          if (displayModeCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixDisplayMode + displayModeCommand.CommandString;
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
            new BlastIrDelegate(MPBlastZonePlugin.BlastIR),
            Common.FolderIRCommands,
            MPBlastZonePlugin.TransceiverInformation.Ports,
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
          throw new IrssUtils.Exceptions.CommandStructureException(String.Format("Unknown command in macro command list \"{0}\"", selected));
        }

        if (!String.IsNullOrEmpty(newCommand))
          listBoxMacro.Items.Add(newCommand);
      }
      catch (Exception ex)
      {
        Log.Error(ex);
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
        WriteToFile(MPBlastZonePlugin.FolderMacros + name + Common.FileExtensionMacro);

        MPBlastZonePlugin.ProcessCommand(Common.CmdPrefixMacro + name, false);
      }
      catch (Exception ex)
      {
        Log.Error(ex);
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
        WriteToFile(MPBlastZonePlugin.FolderMacros + name + Common.FileExtensionMacro);
      }
      catch (Exception ex)
      {
        Log.Error(ex);
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

        if (selected.StartsWith(Common.CmdPrefixRun, StringComparison.OrdinalIgnoreCase))
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
        else if (selected.StartsWith(Common.CmdPrefixGotoScreen, StringComparison.OrdinalIgnoreCase))
        {
          GoToScreen goToScreen = new GoToScreen(selected.Substring(Common.CmdPrefixGotoScreen.Length));
          if (goToScreen.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixGotoScreen + goToScreen.CommandString;
        }
        else if (selected.StartsWith(Common.CmdPrefixSendMPAction, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitSendMPActionCommand(selected.Substring(Common.CmdPrefixSendMPAction.Length));

          MPAction edit = new MPAction(commands);
          if (edit.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixSendMPAction + edit.CommandString;
        }
        else if (selected.StartsWith(Common.CmdPrefixSendMPMsg, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitSendMPMsgCommand(selected.Substring(Common.CmdPrefixSendMPMsg.Length));

          MPMessage edit = new MPMessage(commands);
          if (edit.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixSendMPMsg + edit.CommandString;
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
        else if (selected.StartsWith(Common.CmdPrefixDisplayMode, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitDisplayModeCommand(selected.Substring(Common.CmdPrefixDisplayMode.Length));

          DisplayModeCommand displayModeCommand = new DisplayModeCommand(commands);
          if (displayModeCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixDisplayMode + displayModeCommand.CommandString;
        }
        else if (selected.StartsWith(Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitBlastCommand(selected.Substring(Common.CmdPrefixBlast.Length));

          BlastCommand blastCommand = new BlastCommand(
            new BlastIrDelegate(MPBlastZonePlugin.BlastIR),
            Common.FolderIRCommands,
            MPBlastZonePlugin.TransceiverInformation.Ports,
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
        Log.Error(ex);
        MessageBox.Show(this, ex.Message, "Failed to edit macro item", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #endregion Implementation

  }

}
