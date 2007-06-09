using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using IrssUtils;
using IrssUtils.Forms;

namespace Translator
{

  public partial class MacroEditor : Form
  {

    #region Constructor

    public MacroEditor(bool newMacro, string name)
    {
      InitializeComponent();

      textBoxName.Text = name;
      textBoxName.Enabled = newMacro;

      if (!newMacro)
      {
        string fileName = Program.FolderMacros + name + Common.FileExtensionMacro;
        ReadFromFile(fileName);
      }
    }

    #endregion Constructor

    void RefreshCommandList()
    {
      comboBoxCommands.Items.Clear();

      comboBoxCommands.Items.Add(Common.UITextRun);
      comboBoxCommands.Items.Add(Common.UITextPause);
      comboBoxCommands.Items.Add(Common.UITextSerial);
      comboBoxCommands.Items.Add(Common.UITextMessage);
      comboBoxCommands.Items.Add(Common.UITextKeys);
      //TODO: Add Shutdown and Reboot
      comboBoxCommands.Items.Add(Common.UITextStandby);
      comboBoxCommands.Items.Add(Common.UITextHibernate);
      
      string[] irList = Common.GetIRList(true);
      if (irList != null && irList.Length > 0)
        comboBoxCommands.Items.AddRange(irList);
    }

    void WriteToFile(string fileName)
    {
      try
      {
        XmlTextWriter writer = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 1;
        writer.IndentChar = (char)9;
        writer.WriteStartDocument(true);
        writer.WriteStartElement("macro"); // <macro>

        foreach (string item in listBoxMacro.Items)
        {
          writer.WriteStartElement("action");

          if (item.StartsWith(Common.CmdPrefixBlast))
          {
            writer.WriteAttributeString("command", Common.XmlTagBlast);
            writer.WriteAttributeString("cmdproperty", item.Substring(Common.CmdPrefixBlast.Length));
          }
          else if (item.StartsWith(Common.CmdPrefixPause))
          {
            writer.WriteAttributeString("command", Common.XmlTagPause);
            writer.WriteAttributeString("cmdproperty", item.Substring(Common.CmdPrefixPause.Length));
          }
          else if (item.StartsWith(Common.CmdPrefixRun))
          {
            writer.WriteAttributeString("command", Common.XmlTagRun);
            writer.WriteAttributeString("cmdproperty", item.Substring(Common.CmdPrefixRun.Length));
          }
          else if (item.StartsWith(Common.CmdPrefixSerial))
          {
            writer.WriteAttributeString("command", Common.XmlTagSerial);
            writer.WriteAttributeString("cmdproperty", item.Substring(Common.CmdPrefixSerial.Length));
          }
          else if (item.StartsWith(Common.CmdPrefixMessage))
          {
            writer.WriteAttributeString("command", Common.XmlTagMessage);
            writer.WriteAttributeString("cmdproperty", item.Substring(Common.CmdPrefixMessage.Length));
          }
          else if (item.StartsWith(Common.CmdPrefixKeys))
          {
            writer.WriteAttributeString("command", Common.XmlTagKeys);
            writer.WriteAttributeString("cmdproperty", item.Substring(Common.CmdPrefixKeys.Length));
          }
          else if (item.StartsWith(Common.CmdPrefixStandby))
          {
            writer.WriteAttributeString("command", Common.XmlTagStandby);
            writer.WriteAttributeString("cmdproperty", String.Empty);
          }
          else if (item.StartsWith(Common.CmdPrefixHibernate))
          {
            writer.WriteAttributeString("command", Common.XmlTagHibernate);
            writer.WriteAttributeString("cmdproperty", String.Empty);
          }

          writer.WriteEndElement();
        }

        writer.WriteEndElement(); // </macro>
        writer.WriteEndDocument();
        writer.Close();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
      }
    }
    void ReadFromFile(string fileName)
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(fileName);

        XmlNodeList commandSequence = doc.DocumentElement.SelectNodes("action");

        string commandProperty;
        foreach (XmlNode item in commandSequence)
        {
          commandProperty = item.Attributes["cmdproperty"].Value;

          switch (item.Attributes["command"].Value)
          {
            case Common.XmlTagBlast:
              listBoxMacro.Items.Add(Common.CmdPrefixBlast + commandProperty);
              break;

            case Common.XmlTagPause:
              listBoxMacro.Items.Add(Common.CmdPrefixPause + commandProperty);
              break;

            case Common.XmlTagRun:
              listBoxMacro.Items.Add(Common.CmdPrefixRun + commandProperty);
              break;

            case Common.XmlTagSerial:
              listBoxMacro.Items.Add(Common.CmdPrefixSerial + commandProperty);
              break;

            case Common.XmlTagMessage:
              listBoxMacro.Items.Add(Common.CmdPrefixMessage + commandProperty);
              break;

            case Common.XmlTagKeys:
              listBoxMacro.Items.Add(Common.CmdPrefixKeys + commandProperty);
              break;

            case Common.XmlTagStandby:
              listBoxMacro.Items.Add(Common.CmdPrefixStandby);
              break;
            
            case Common.XmlTagHibernate:
              listBoxMacro.Items.Add(Common.CmdPrefixHibernate);
              break;
          }
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.Message);
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

      string selected = comboBoxCommands.SelectedItem as string;

      if (selected == Common.UITextRun)
      {
        ExternalProgram externalProgram = new ExternalProgram(false);

        if (externalProgram.ShowDialog(this) == DialogResult.Cancel)
          return;

        listBoxMacro.Items.Add(Common.CmdPrefixRun + externalProgram.CommandString);
      }
      else if (selected == Common.UITextPause)
      {
        IrssUtils.Forms.PauseTime pauseTime = new IrssUtils.Forms.PauseTime();

        if (pauseTime.ShowDialog(this) == DialogResult.Cancel)
          return;

        listBoxMacro.Items.Add(Common.CmdPrefixPause + pauseTime.Time.ToString());
      }
      else if (selected == Common.UITextSerial)
      {
        SerialCommand serialCommand = new SerialCommand();
        if (serialCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        listBoxMacro.Items.Add(Common.CmdPrefixSerial + serialCommand.CommandString);
      }
      else if (selected == Common.UITextMessage)
      {
        MessageCommand messageCommand = new MessageCommand();
        if (messageCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        listBoxMacro.Items.Add(Common.CmdPrefixMessage + messageCommand.CommandString);
      }
      else if (selected == Common.UITextKeys)
      {
        KeysCommand keysCommand = new KeysCommand();
        if (keysCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        listBoxMacro.Items.Add(Common.CmdPrefixKeys + keysCommand.CommandString);
      }
      else if (selected == Common.UITextStandby)
      {
        listBoxMacro.Items.Add(Common.CmdPrefixStandby);
      }
      else if (selected == Common.UITextHibernate)
      {
        listBoxMacro.Items.Add(Common.CmdPrefixHibernate);
      }
      else
      {
        BlastCommand blastCommand = new BlastCommand(selected.Substring(Common.CmdPrefixBlast.Length));
        if (blastCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        listBoxMacro.Items.Add(Common.CmdPrefixBlast + blastCommand.CommandString);
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
      string fileName = textBoxName.Text.Trim();

      if (fileName.Length == 0)
      {
        MessageBox.Show(this, "You must supply a name for this macro", "Name missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
      }

      fileName = Program.FolderMacros + fileName + Common.FileExtensionMacro;

      WriteToFile(fileName);

      try
      {
        Program.ProcessMacro(fileName);
      }
      catch (Exception ex)
      {
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
      string fileName = textBoxName.Text.Trim();

      if (fileName.Length == 0)
      {
        MessageBox.Show(this, "You must supply a name for this macro", "Name missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
      }

      fileName = Program.FolderMacros + fileName + Common.FileExtensionMacro;

      WriteToFile(fileName);

      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void listBoxCommandSequence_DoubleClick(object sender, EventArgs e)
    {
      if (listBoxMacro.SelectedIndex != -1)
      {
        string selected = listBoxMacro.SelectedItem as string;

        if (selected.StartsWith(Common.CmdPrefixPause))
        {
          IrssUtils.Forms.PauseTime pauseTime = new IrssUtils.Forms.PauseTime(int.Parse(selected.Substring(Common.CmdPrefixPause.Length)));

          if (pauseTime.ShowDialog(this) == DialogResult.Cancel)
            return;

          int index = listBoxMacro.SelectedIndex;
          listBoxMacro.Items.RemoveAt(index);
          listBoxMacro.Items.Insert(index, Common.CmdPrefixPause + pauseTime.Time.ToString());
          listBoxMacro.SelectedIndex = index;
        }
        else if (selected.StartsWith(Common.CmdPrefixRun))
        {
          string[] commands = Common.SplitRunCommand(selected.Substring(Common.CmdPrefixRun.Length));
          ExternalProgram executeProgram = new ExternalProgram(commands, false);
          if (executeProgram.ShowDialog(this) == DialogResult.Cancel)
            return;

          int index = listBoxMacro.SelectedIndex;
          listBoxMacro.Items.RemoveAt(index);
          listBoxMacro.Items.Insert(index, Common.CmdPrefixRun + executeProgram.CommandString);
          listBoxMacro.SelectedIndex = index;
        }
        else if (selected.StartsWith(Common.CmdPrefixSerial))
        {
          string[] commands = Common.SplitSerialCommand(selected.Substring(Common.CmdPrefixSerial.Length));
          SerialCommand serialCommand = new SerialCommand(commands);
          if (serialCommand.ShowDialog(this) == DialogResult.Cancel)
            return;

          int index = listBoxMacro.SelectedIndex;
          listBoxMacro.Items.RemoveAt(index);
          listBoxMacro.Items.Insert(index, Common.CmdPrefixSerial + serialCommand.CommandString);
          listBoxMacro.SelectedIndex = index;
        }
        else if (selected.StartsWith(Common.CmdPrefixMessage))
        {
          string[] commands = Common.SplitMessageCommand(selected.Substring(Common.CmdPrefixMessage.Length));
          MessageCommand messageCommand = new MessageCommand(commands);
          if (messageCommand.ShowDialog(this) == DialogResult.Cancel)
            return;

          int index = listBoxMacro.SelectedIndex;
          listBoxMacro.Items.RemoveAt(index);
          listBoxMacro.Items.Insert(index, Common.CmdPrefixMessage + messageCommand.CommandString);
          listBoxMacro.SelectedIndex = index;
        }
        else if (selected.StartsWith(Common.CmdPrefixKeys))
        {
          KeysCommand keysCommand = new KeysCommand(selected.Substring(Common.CmdPrefixKeys.Length));
          if (keysCommand.ShowDialog(this) == DialogResult.Cancel)
            return;

          int index = listBoxMacro.SelectedIndex;
          listBoxMacro.Items.RemoveAt(index);
          listBoxMacro.Items.Insert(index, Common.CmdPrefixKeys + keysCommand.CommandString);
          listBoxMacro.SelectedIndex = index;
        }
        else if (selected.StartsWith(Common.CmdPrefixBlast))
        {
          string[] commands = Common.SplitBlastCommand(selected.Substring(Common.CmdPrefixBlast.Length));
          BlastCommand blastCommand = new BlastCommand(commands);
          if (blastCommand.ShowDialog(this) == DialogResult.Cancel)
            return;

          int index = listBoxMacro.SelectedIndex;
          listBoxMacro.Items.RemoveAt(index);
          listBoxMacro.Items.Insert(index, Common.CmdPrefixBlast + blastCommand.CommandString);
          listBoxMacro.SelectedIndex = index;
        }

      }
    }

  }

}
