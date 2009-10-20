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
using System.Text;
using System.Windows.Forms;
using System.Xml;
using IrssUtils;
using IrssUtils.Exceptions;
using IrssUtils.Forms;
using TvLibrary.Log;

namespace TvEngine.Forms
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

      string fileName = TV3BlasterPlugin.
        PathCombine(name);
      ReadFromFile(fileName);
    }

    #endregion Constructor

    #region Implementation

    private void RefreshCommandList()
    {
      comboBoxCommands.Items.Clear();

      comboBoxCommands.Items.Add(Common.UITextRun);
      comboBoxCommands.Items.Add(Common.UITextPause);
      comboBoxCommands.Items.Add(Common.UITextSerial);
      comboBoxCommands.Items.Add(Common.UITextWindowMsg);
      comboBoxCommands.Items.Add(Common.UITextTcpMsg);
      comboBoxCommands.Items.Add(Common.UITextHttpMsg);
      comboBoxCommands.Items.Add(Common.UITextKeys);
      comboBoxCommands.Items.Add(Common.UITextEject);
      comboBoxCommands.Items.Add(Common.UITextStandby);
      comboBoxCommands.Items.Add(Common.UITextHibernate);
      comboBoxCommands.Items.Add(Common.UITextReboot);
      comboBoxCommands.Items.Add(Common.UITextShutdown);

      string[] fileList = TV3BlasterPlugin.GetFileList(true);
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
        Log.Error(ex.ToString());
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
        Log.Error(ex.ToString());
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
            newCommand = Common.CmdPrefixPause + pauseTime.Time;
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
        else if (selected.Equals(Common.UITextEject, StringComparison.OrdinalIgnoreCase))
        {
          EjectCommand ejectCommand = new EjectCommand();
          if (ejectCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixEject + ejectCommand.CommandString;
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
            TV3BlasterPlugin.BlastIR,
            Common.FolderIRCommands,
            TV3BlasterPlugin.TransceiverInformation.Ports,
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
          throw new CommandStructureException(String.Format("Unknown command in macro command list \"{0}\"", selected));
        }

        if (!String.IsNullOrEmpty(newCommand))
          listBoxMacro.Items.Add(newCommand);
      }
      catch (Exception ex)
      {
        Log.Error(ex.ToString());
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

      if (!Common.IsValidFileName(name))
      {
        MessageBox.Show(this, "You must supply a valid name for this Macro", "Invalid name", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
        textBoxName.Focus();
        return;
      }

      try
      {
        WriteToFile(TV3BlasterPlugin.PathCombine(name));

        TV3BlasterPlugin.ProcessCommand(Common.CmdPrefixMacro + name, false);
      }
      catch (Exception ex)
      {
        Log.Error(ex.ToString());
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

      if (!Common.IsValidFileName(name))
      {
        MessageBox.Show(this, "You must supply a valid name for this Macro", "Invalid name", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
        textBoxName.Focus();
        return;
      }

      try
      {
        WriteToFile(TV3BlasterPlugin.PathCombine(name));
      }
      catch (Exception ex)
      {
        Log.Error(ex.ToString());
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
            newCommand = Common.CmdPrefixPause + pauseTime.Time;
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
        else if (selected.StartsWith(Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitBlastCommand(selected.Substring(Common.CmdPrefixBlast.Length));

          BlastCommand blastCommand = new BlastCommand(
            TV3BlasterPlugin.BlastIR,
            Common.FolderIRCommands,
            TV3BlasterPlugin.TransceiverInformation.Ports,
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
        Log.Error(ex.ToString());
        MessageBox.Show(this, ex.Message, "Failed to edit macro item", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #endregion Implementation
  }
}