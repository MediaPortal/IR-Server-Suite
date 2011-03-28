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
using System.Text;
using System.Windows.Forms;
using System.Xml;
using IrssUtils;
using IrssUtils.Exceptions;
using IrssUtils.Forms;
using MediaPortal.GUI.Library;
using MPUtils.Forms;

namespace MediaPortal.Plugins
{
  internal partial class MacroEditor : Form
  {
    #region Constructor

    /// <summary>
    /// Creates a Macro Editor windows form.
    /// </summary>
    public MacroEditor()
    {
      InitializeComponent();

      textBoxName.Text = "New";
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

      textBoxName.Text = name;
      textBoxName.Enabled = false;

      string fileName = Path.Combine(MPControlPlugin.FolderMacros, name + IrssUtils.Common.FileExtensionMacro);
      ReadFromFile(fileName);
    }

    #endregion Constructor

    #region Implementation

    private void RefreshCommandList()
    {
      comboBoxCommands.Items.Clear();

      comboBoxCommands.Items.Add(IrssUtils.Common.UITextRun);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextPause);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextSerial);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextWindowMsg);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextTcpMsg);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextHttpMsg);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextKeys);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextMouse);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextEject);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextPopup);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextGotoScreen);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextMultiMap);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextMouseMode);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextInputLayer);
      //comboBoxCommands.Items.Add(Common.UITextWindowState);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextFocus);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextExit);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextSendMPAction);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextSendMPMsg);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextVirtualKB);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextSmsKB);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextBeep);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextSound);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextDisplayMode);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextStandby);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextHibernate);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextReboot);
      comboBoxCommands.Items.Add(IrssUtils.Common.UITextShutdown);

      string[] fileList = MPControlPlugin.GetFileList(true);
      if (fileList != null && fileList.Length > 0)
        comboBoxCommands.Items.AddRange(fileList);
    }

    /// <summary>
    /// Write the macro in the listBox to a macro name provided.
    /// </summary>
    /// <param name="fileName">Name of Macro to write (macro name, not file path).</param>
    private void WriteToFile(string fileName)
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
    private void ReadFromFile(string fileName)
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

        if (selected.Equals(IrssUtils.Common.UITextRun, StringComparison.OrdinalIgnoreCase))
        {
          ExternalProgram externalProgram = new ExternalProgram();
          if (externalProgram.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixRun + externalProgram.CommandString;
        }
        else if (selected.Equals(IrssUtils.Common.UITextPause, StringComparison.OrdinalIgnoreCase))
        {
          PauseTime pauseTime = new PauseTime();
          if (pauseTime.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixPause + pauseTime.Time;
        }
        else if (selected.Equals(IrssUtils.Common.UITextSerial, StringComparison.OrdinalIgnoreCase))
        {
          SerialCommand serialCommand = new SerialCommand();
          if (serialCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixSerial + serialCommand.CommandString;
        }
        else if (selected.Equals(IrssUtils.Common.UITextWindowMsg, StringComparison.OrdinalIgnoreCase))
        {
          MessageCommand messageCommand = new MessageCommand();
          if (messageCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixWindowMsg + messageCommand.CommandString;
        }
        else if (selected.Equals(IrssUtils.Common.UITextTcpMsg, StringComparison.OrdinalIgnoreCase))
        {
          TcpMessageCommand tcpMessageCommand = new TcpMessageCommand();
          if (tcpMessageCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixTcpMsg + tcpMessageCommand.CommandString;
        }
        else if (selected.Equals(IrssUtils.Common.UITextHttpMsg, StringComparison.OrdinalIgnoreCase))
        {
          HttpMessageCommand httpMessageCommand = new HttpMessageCommand();
          if (httpMessageCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixHttpMsg + httpMessageCommand.CommandString;
        }
        else if (selected.Equals(IrssUtils.Common.UITextKeys, StringComparison.OrdinalIgnoreCase))
        {
          KeysCommand keysCommand = new KeysCommand();
          if (keysCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixKeys + keysCommand.CommandString;
        }
        else if (selected.Equals(IrssUtils.Common.UITextMouse, StringComparison.OrdinalIgnoreCase))
        {
          MouseCommand mouseCommand = new MouseCommand();
          if (mouseCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixMouse + mouseCommand.CommandString;
        }
        else if (selected.Equals(IrssUtils.Common.UITextEject, StringComparison.OrdinalIgnoreCase))
        {
          EjectCommand ejectCommand = new EjectCommand();
          if (ejectCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixEject + ejectCommand.CommandString;
        }
        else if (selected.Equals(IrssUtils.Common.UITextPopup, StringComparison.OrdinalIgnoreCase))
        {
          PopupMessage popupMessage = new PopupMessage();
          if (popupMessage.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixPopup + popupMessage.CommandString;
        }
        else if (selected.Equals(IrssUtils.Common.UITextGotoScreen, StringComparison.OrdinalIgnoreCase))
        {
          GoToScreen goToScreen = new GoToScreen();
          if (goToScreen.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixGotoScreen + goToScreen.CommandString;
        }
        else if (selected.Equals(IrssUtils.Common.UITextMultiMap, StringComparison.OrdinalIgnoreCase))
        {
          MultiMapNameBox multiMapNameBox = new MultiMapNameBox("Toggle");

          if (multiMapNameBox.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixMultiMap + multiMapNameBox.MapName;
        }
        else if (selected.Equals(IrssUtils.Common.UITextMouseMode, StringComparison.OrdinalIgnoreCase))
        {
          newCommand = IrssUtils.Common.CmdPrefixMouseMode + "TOGGLE";
        }
        else if (selected.Equals(IrssUtils.Common.UITextInputLayer, StringComparison.OrdinalIgnoreCase))
        {
          newCommand = IrssUtils.Common.CmdPrefixInputLayer;
        }
        /*
      else if (selected.Equals(IrssUtils.Common.UITextWindowState, StringComparison.OrdinalIgnoreCase))
      {
        newCommand = IrssUtils.Common.CmdPrefixWindowState;
      }
      */
        else if (selected.Equals(IrssUtils.Common.UITextFocus, StringComparison.OrdinalIgnoreCase))
        {
          newCommand = IrssUtils.Common.CmdPrefixFocus;
        }
        else if (selected.Equals(IrssUtils.Common.UITextExit, StringComparison.OrdinalIgnoreCase))
        {
          newCommand = IrssUtils.Common.CmdPrefixExit;
        }
        else if (selected.Equals(IrssUtils.Common.UITextSendMPAction, StringComparison.OrdinalIgnoreCase))
        {
          MPAction edit = new MPAction();
          if (edit.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixSendMPAction + edit.CommandString;
        }
        else if (selected.Equals(IrssUtils.Common.UITextSendMPMsg, StringComparison.OrdinalIgnoreCase))
        {
          MPMessage edit = new MPMessage();
          if (edit.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixSendMPMsg + edit.CommandString;
        }
        else if (selected.Equals(IrssUtils.Common.UITextVirtualKB, StringComparison.OrdinalIgnoreCase))
        {
          newCommand = IrssUtils.Common.CmdPrefixVirtualKB;
        }
        else if (selected.Equals(IrssUtils.Common.UITextSmsKB, StringComparison.OrdinalIgnoreCase))
        {
          newCommand = IrssUtils.Common.CmdPrefixSmsKB;
        }
        else if (selected.Equals(IrssUtils.Common.UITextBeep, StringComparison.OrdinalIgnoreCase))
        {
          BeepCommand beepCommand = new BeepCommand();
          if (beepCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixBeep + beepCommand.CommandString;
        }
        else if (selected.Equals(IrssUtils.Common.UITextSound, StringComparison.OrdinalIgnoreCase))
        {
          OpenFileDialog openFileDialog = new OpenFileDialog();
          openFileDialog.Filter = "Wave Files|*.wav";
          openFileDialog.Multiselect = false;

          if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixSound + openFileDialog.FileName;
        }
        else if (selected.Equals(IrssUtils.Common.UITextDisplayMode, StringComparison.OrdinalIgnoreCase))
        {
          DisplayModeCommand displayModeCommand = new DisplayModeCommand();
          if (displayModeCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixDisplayMode + displayModeCommand.CommandString;
        }
        else if (selected.Equals(IrssUtils.Common.UITextStandby, StringComparison.OrdinalIgnoreCase))
        {
          newCommand = IrssUtils.Common.CmdPrefixStandby;
        }
        else if (selected.Equals(IrssUtils.Common.UITextHibernate, StringComparison.OrdinalIgnoreCase))
        {
          newCommand = IrssUtils.Common.CmdPrefixHibernate;
        }
        else if (selected.Equals(IrssUtils.Common.UITextReboot, StringComparison.OrdinalIgnoreCase))
        {
          newCommand = IrssUtils.Common.CmdPrefixReboot;
        }
        else if (selected.Equals(IrssUtils.Common.UITextShutdown, StringComparison.OrdinalIgnoreCase))
        {
          newCommand = IrssUtils.Common.CmdPrefixShutdown;
        }
        else if (selected.StartsWith(IrssUtils.Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
        {
          BlastCommand blastCommand = new BlastCommand(
            MPControlPlugin.BlastIR,
            IrssUtils.Common.FolderIRCommands,
            MPControlPlugin.TransceiverInformation.Ports,
            selected.Substring(IrssUtils.Common.CmdPrefixBlast.Length));

          if (blastCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixBlast + blastCommand.CommandString;
        }
        else if (selected.StartsWith(IrssUtils.Common.CmdPrefixMacro, StringComparison.OrdinalIgnoreCase))
        {
          newCommand = selected;
        }
        else
        {
          throw new CommandStructureException(String.Format("Unknown command in macro command list \"{0}\"", selected));
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
        MessageBox.Show(this, "You must supply a name for this Macro", "Name missing", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
        textBoxName.Focus();
        return;
      }

      if (!IrssUtils.Common.IsValidFileName(name))
      {
        MessageBox.Show(this, "You must supply a valid name for this Macro", "Invalid name", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
        textBoxName.Focus();
        return;
      }

      try
      {
        string fileName = Path.Combine(MPControlPlugin.FolderMacros, name + IrssUtils.Common.FileExtensionMacro);
        WriteToFile(fileName);

        MPControlPlugin.ProcessCommand(IrssUtils.Common.CmdPrefixMacro + name, false);
      }
      catch (Exception ex)
      {
        Log.Error(ex);
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

      if (!IrssUtils.Common.IsValidFileName(name))
      {
        MessageBox.Show(this, "You must supply a valid name for this Macro", "Invalid name", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
        textBoxName.Focus();
        return;
      }

      try
      {
        string fileName = Path.Combine(MPControlPlugin.FolderMacros, name + IrssUtils.Common.FileExtensionMacro);
        WriteToFile(fileName);
      }
      catch (Exception ex)
      {
        Log.Error(ex);
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
        string selected = listBoxMacro.SelectedItem as string;
        string newCommand = null;

        if (selected.StartsWith(IrssUtils.Common.CmdPrefixRun, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = IrssUtils.Common.SplitRunCommand(selected.Substring(IrssUtils.Common.CmdPrefixRun.Length));

          ExternalProgram executeProgram = new ExternalProgram(commands);
          if (executeProgram.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixRun + executeProgram.CommandString;
        }
        else if (selected.StartsWith(IrssUtils.Common.CmdPrefixPause, StringComparison.OrdinalIgnoreCase))
        {
          PauseTime pauseTime = new PauseTime(int.Parse(selected.Substring(IrssUtils.Common.CmdPrefixPause.Length)));
          if (pauseTime.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixPause + pauseTime.Time;
        }
        else if (selected.StartsWith(IrssUtils.Common.CmdPrefixSerial, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = IrssUtils.Common.SplitSerialCommand(selected.Substring(IrssUtils.Common.CmdPrefixSerial.Length));

          SerialCommand serialCommand = new SerialCommand(commands);
          if (serialCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixSerial + serialCommand.CommandString;
        }
        else if (selected.StartsWith(IrssUtils.Common.CmdPrefixWindowMsg, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = IrssUtils.Common.SplitWindowMessageCommand(selected.Substring(IrssUtils.Common.CmdPrefixWindowMsg.Length));

          MessageCommand messageCommand = new MessageCommand(commands);
          if (messageCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixWindowMsg + messageCommand.CommandString;
        }
        else if (selected.StartsWith(IrssUtils.Common.CmdPrefixTcpMsg, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = IrssUtils.Common.SplitTcpMessageCommand(selected.Substring(IrssUtils.Common.CmdPrefixTcpMsg.Length));

          TcpMessageCommand tcpMessageCommand = new TcpMessageCommand(commands);
          if (tcpMessageCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixTcpMsg + tcpMessageCommand.CommandString;
        }
        else if (selected.StartsWith(IrssUtils.Common.CmdPrefixHttpMsg, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = IrssUtils.Common.SplitHttpMessageCommand(selected.Substring(IrssUtils.Common.CmdPrefixHttpMsg.Length));

          HttpMessageCommand httpMessageCommand = new HttpMessageCommand(commands);
          if (httpMessageCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixHttpMsg + httpMessageCommand.CommandString;
        }
        else if (selected.StartsWith(IrssUtils.Common.CmdPrefixKeys, StringComparison.OrdinalIgnoreCase))
        {
          KeysCommand keysCommand = new KeysCommand(selected.Substring(IrssUtils.Common.CmdPrefixKeys.Length));
          if (keysCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixKeys + keysCommand.CommandString;
        }
        else if (selected.StartsWith(IrssUtils.Common.CmdPrefixMouse, StringComparison.OrdinalIgnoreCase))
        {
          MouseCommand mouseCommand = new MouseCommand(selected.Substring(IrssUtils.Common.CmdPrefixMouse.Length));
          if (mouseCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixMouse + mouseCommand.CommandString;
        }
        else if (selected.StartsWith(IrssUtils.Common.CmdPrefixEject, StringComparison.OrdinalIgnoreCase))
        {
          EjectCommand ejectCommand = new EjectCommand(selected.Substring(IrssUtils.Common.CmdPrefixEject.Length));
          if (ejectCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixEject + ejectCommand.CommandString;
        }
        else if (selected.StartsWith(IrssUtils.Common.CmdPrefixPopup, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = IrssUtils.Common.SplitPopupCommand(selected.Substring(IrssUtils.Common.CmdPrefixPopup.Length));

          PopupMessage popupMessage = new PopupMessage(commands);
          if (popupMessage.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixPopup + popupMessage.CommandString;
        }
        else if (selected.StartsWith(IrssUtils.Common.CmdPrefixGotoScreen, StringComparison.OrdinalIgnoreCase))
        {
          GoToScreen goToScreen = new GoToScreen(selected.Substring(IrssUtils.Common.CmdPrefixGotoScreen.Length));
          if (goToScreen.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixGotoScreen + goToScreen.CommandString;
        }
        else if (selected.StartsWith(IrssUtils.Common.CmdPrefixMultiMap, StringComparison.OrdinalIgnoreCase))
        {
          MultiMapNameBox multiMapNameBox = new MultiMapNameBox(selected.Substring(IrssUtils.Common.CmdPrefixMultiMap.Length));
          if (multiMapNameBox.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixMultiMap + multiMapNameBox.MapName;
        }
        else if (selected.StartsWith(IrssUtils.Common.CmdPrefixSendMPAction, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = IrssUtils.Common.SplitSendMPActionCommand(selected.Substring(IrssUtils.Common.CmdPrefixSendMPAction.Length));

          MPAction edit = new MPAction(commands);
          if (edit.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixSendMPAction + edit.CommandString;
        }
        else if (selected.StartsWith(IrssUtils.Common.CmdPrefixSendMPMsg, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = IrssUtils.Common.SplitSendMPMsgCommand(selected.Substring(IrssUtils.Common.CmdPrefixSendMPMsg.Length));

          MPMessage edit = new MPMessage(commands);
          if (edit.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixSendMPMsg + edit.CommandString;
        }
        else if (selected.StartsWith(IrssUtils.Common.CmdPrefixBeep, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = IrssUtils.Common.SplitBeepCommand(selected.Substring(IrssUtils.Common.CmdPrefixBeep.Length));

          BeepCommand beepCommand = new BeepCommand(commands);
          if (beepCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixBeep + beepCommand.CommandString;
        }
        else if (selected.StartsWith(IrssUtils.Common.CmdPrefixSound, StringComparison.OrdinalIgnoreCase))
        {
          OpenFileDialog openFileDialog = new OpenFileDialog();
          openFileDialog.Filter = "Wave Files|*.wav";
          openFileDialog.Multiselect = false;
          openFileDialog.FileName = selected.Substring(IrssUtils.Common.CmdPrefixSound.Length);

          if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixSound + openFileDialog.FileName;
        }
        else if (selected.StartsWith(IrssUtils.Common.CmdPrefixDisplayMode, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = IrssUtils.Common.SplitDisplayModeCommand(selected.Substring(IrssUtils.Common.CmdPrefixDisplayMode.Length));

          DisplayModeCommand displayModeCommand = new DisplayModeCommand(commands);
          if (displayModeCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixDisplayMode + displayModeCommand.CommandString;
        }
        else if (selected.StartsWith(IrssUtils.Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = IrssUtils.Common.SplitBlastCommand(selected.Substring(IrssUtils.Common.CmdPrefixBlast.Length));

          BlastCommand blastCommand = new BlastCommand(
            MPControlPlugin.BlastIR,
            IrssUtils.Common.FolderIRCommands,
            MPControlPlugin.TransceiverInformation.Ports,
            commands);

          if (blastCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = IrssUtils.Common.CmdPrefixBlast + blastCommand.CommandString;
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