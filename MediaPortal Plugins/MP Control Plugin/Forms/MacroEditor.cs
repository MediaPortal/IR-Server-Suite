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

      string fileName = MPControlPlugin.FolderMacros + name + Common.FileExtensionMacro;
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
      comboBoxCommands.Items.Add(Common.UITextKeys);
      comboBoxCommands.Items.Add(Common.UITextMouse);
      comboBoxCommands.Items.Add(Common.UITextEject);
      comboBoxCommands.Items.Add(Common.UITextGoto);
      comboBoxCommands.Items.Add(Common.UITextPopup);
      comboBoxCommands.Items.Add(Common.UITextMultiMap);
      comboBoxCommands.Items.Add(Common.UITextMouseMode);
      comboBoxCommands.Items.Add(Common.UITextInputLayer);
      //comboBoxCommands.Items.Add(Common.UITextWindowState);
      comboBoxCommands.Items.Add(Common.UITextFocus);
      comboBoxCommands.Items.Add(Common.UITextExit);
      comboBoxCommands.Items.Add(Common.UITextStandby);
      comboBoxCommands.Items.Add(Common.UITextHibernate);
      comboBoxCommands.Items.Add(Common.UITextReboot);
      comboBoxCommands.Items.Add(Common.UITextShutdown);

      string[] fileList = MPControlPlugin.GetFileList(true);
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
        using (XmlTextWriter writer = new XmlTextWriter(fileName, System.Text.Encoding.UTF8))
        {
          writer.Formatting = Formatting.Indented;
          writer.Indentation = 1;
          writer.IndentChar = (char)9;
          writer.WriteStartDocument(true);
          writer.WriteStartElement("macro"); // <macro>

          foreach (string item in listBoxMacro.Items)
          {
            writer.WriteStartElement("action");

            if (item.StartsWith(Common.CmdPrefixMacro, StringComparison.OrdinalIgnoreCase))
            {
              writer.WriteAttributeString("command", Common.XmlTagMacro);
              writer.WriteAttributeString("cmdproperty", item.Substring(Common.CmdPrefixMacro.Length));
            }
            else if (item.StartsWith(Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
            {
              writer.WriteAttributeString("command", Common.XmlTagBlast);
              writer.WriteAttributeString("cmdproperty", item.Substring(Common.CmdPrefixBlast.Length));
            }
            else if (item.StartsWith(Common.CmdPrefixPause, StringComparison.OrdinalIgnoreCase))
            {
              writer.WriteAttributeString("command", Common.XmlTagPause);
              writer.WriteAttributeString("cmdproperty", item.Substring(Common.CmdPrefixPause.Length));
            }
            else if (item.StartsWith(Common.CmdPrefixRun, StringComparison.OrdinalIgnoreCase))
            {
              writer.WriteAttributeString("command", Common.XmlTagRun);
              writer.WriteAttributeString("cmdproperty", item.Substring(Common.CmdPrefixRun.Length));
            }
            else if (item.StartsWith(Common.CmdPrefixGoto, StringComparison.OrdinalIgnoreCase))
            {
              writer.WriteAttributeString("command", Common.XmlTagGoto);
              writer.WriteAttributeString("cmdproperty", item.Substring(Common.CmdPrefixGoto.Length));
            }
            else if (item.StartsWith(Common.CmdPrefixSerial, StringComparison.OrdinalIgnoreCase))
            {
              writer.WriteAttributeString("command", Common.XmlTagSerial);
              writer.WriteAttributeString("cmdproperty", item.Substring(Common.CmdPrefixSerial.Length));
            }
            else if (item.StartsWith(Common.CmdPrefixWindowMsg, StringComparison.OrdinalIgnoreCase))
            {
              writer.WriteAttributeString("command", Common.XmlTagWindowMsg);
              writer.WriteAttributeString("cmdproperty", item.Substring(Common.CmdPrefixWindowMsg.Length));
            }
            else if (item.StartsWith(Common.CmdPrefixTcpMsg, StringComparison.OrdinalIgnoreCase))
            {
              writer.WriteAttributeString("command", Common.XmlTagTcpMsg);
              writer.WriteAttributeString("cmdproperty", item.Substring(Common.CmdPrefixTcpMsg.Length));
            }
            else if (item.StartsWith(Common.CmdPrefixKeys, StringComparison.OrdinalIgnoreCase))
            {
              writer.WriteAttributeString("command", Common.XmlTagKeys);
              writer.WriteAttributeString("cmdproperty", item.Substring(Common.CmdPrefixKeys.Length));
            }
            else if (item.StartsWith(Common.CmdPrefixMouse, StringComparison.OrdinalIgnoreCase))
            {
              writer.WriteAttributeString("command", Common.XmlTagMouse);
              writer.WriteAttributeString("cmdproperty", item.Substring(Common.CmdPrefixMouse.Length));
            }
            else if (item.StartsWith(Common.CmdPrefixEject, StringComparison.OrdinalIgnoreCase))
            {
              writer.WriteAttributeString("command", Common.XmlTagEject);
              writer.WriteAttributeString("cmdproperty", item.Substring(Common.CmdPrefixEject.Length));
            }
            else if (item.StartsWith(Common.CmdPrefixPopup, StringComparison.OrdinalIgnoreCase))
            {
              writer.WriteAttributeString("command", Common.XmlTagPopup);
              writer.WriteAttributeString("cmdproperty", item.Substring(Common.CmdPrefixPopup.Length));
            }
            else if (item.StartsWith(Common.CmdPrefixMultiMap, StringComparison.OrdinalIgnoreCase))
            {
              writer.WriteAttributeString("command", Common.XmlTagMultiMap);
              writer.WriteAttributeString("cmdproperty", item.Substring(Common.CmdPrefixMultiMap.Length));
            }
            else if (item.StartsWith(Common.CmdPrefixMouseMode, StringComparison.OrdinalIgnoreCase))
            {
              writer.WriteAttributeString("command", Common.XmlTagMouseMode);
              writer.WriteAttributeString("cmdproperty", item.Substring(Common.CmdPrefixMouseMode.Length));
            }
            else if (item.StartsWith(Common.CmdPrefixInputLayer, StringComparison.OrdinalIgnoreCase))
            {
              writer.WriteAttributeString("command", Common.XmlTagInputLayer);
              writer.WriteAttributeString("cmdproperty", String.Empty);
            }
            /*          
            else if (item.StartsWith(Common.CmdPrefixWindowState, StringComparison.OrdinalIgnoreCase))
            {
              writer.WriteAttributeString("command", Common.XmlTagWindowState);
              writer.WriteAttributeString("cmdproperty", String.Empty);
            }
            */
            else if (item.StartsWith(Common.CmdPrefixFocus, StringComparison.OrdinalIgnoreCase))
            {
              writer.WriteAttributeString("command", Common.XmlTagFocus);
              writer.WriteAttributeString("cmdproperty", String.Empty);
            }
            else if (item.StartsWith(Common.CmdPrefixExit, StringComparison.OrdinalIgnoreCase))
            {
              writer.WriteAttributeString("command", Common.XmlTagExit);
              writer.WriteAttributeString("cmdproperty", String.Empty);
            }
            else if (item.StartsWith(Common.CmdPrefixStandby, StringComparison.OrdinalIgnoreCase))
            {
              writer.WriteAttributeString("command", Common.XmlTagStandby);
              writer.WriteAttributeString("cmdproperty", String.Empty);
            }
            else if (item.StartsWith(Common.CmdPrefixHibernate, StringComparison.OrdinalIgnoreCase))
            {
              writer.WriteAttributeString("command", Common.XmlTagHibernate);
              writer.WriteAttributeString("cmdproperty", String.Empty);
            }
            else if (item.StartsWith(Common.CmdPrefixReboot, StringComparison.OrdinalIgnoreCase))
            {
              writer.WriteAttributeString("command", Common.XmlTagReboot);
              writer.WriteAttributeString("cmdproperty", String.Empty);
            }
            else if (item.StartsWith(Common.CmdPrefixShutdown, StringComparison.OrdinalIgnoreCase))
            {
              writer.WriteAttributeString("command", Common.XmlTagShutdown);
              writer.WriteAttributeString("cmdproperty", String.Empty);
            }
            else
            {
              Log.Error("MPControlPlugin: Cannot write unknown macro item ({0}) to file ({1}).", item, fileName);
            }

            writer.WriteEndElement();
          }

          writer.WriteEndElement(); // </macro>
          writer.WriteEndDocument();
        }
      }
      catch (Exception ex)
      {
        Log.Error("MPControlPlugin: {0}", ex.Message);
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

        listBoxMacro.Items.Clear();

        XmlNodeList commandSequence = doc.DocumentElement.SelectNodes("action");

        string commandProperty;
        foreach (XmlNode item in commandSequence)
        {
          commandProperty = item.Attributes["cmdproperty"].Value;

          switch (item.Attributes["command"].Value)
          {
            case Common.XmlTagMacro:
              listBoxMacro.Items.Add(Common.CmdPrefixMacro + commandProperty);
              break;

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

            case Common.XmlTagWindowMsg:
              listBoxMacro.Items.Add(Common.CmdPrefixWindowMsg + commandProperty);
              break;

            case Common.XmlTagTcpMsg:
              listBoxMacro.Items.Add(Common.CmdPrefixTcpMsg + commandProperty);
              break;

            case Common.XmlTagKeys:
              listBoxMacro.Items.Add(Common.CmdPrefixKeys + commandProperty);
              break;

            case Common.XmlTagMouse:
              listBoxMacro.Items.Add(Common.CmdPrefixMouse + commandProperty);
              break;

            case Common.XmlTagEject:
              listBoxMacro.Items.Add(Common.CmdPrefixEject + commandProperty);
              break;

            case Common.XmlTagGoto:
              listBoxMacro.Items.Add(Common.CmdPrefixGoto + commandProperty);
              break;

            case Common.XmlTagPopup:
              listBoxMacro.Items.Add(Common.CmdPrefixPopup + commandProperty);
              break;

            case Common.XmlTagMultiMap:
              listBoxMacro.Items.Add(Common.CmdPrefixMultiMap + commandProperty);
              break;

            case Common.XmlTagMouseMode:
              listBoxMacro.Items.Add(Common.CmdPrefixMouseMode + commandProperty);
              break;

            case Common.XmlTagInputLayer:
              listBoxMacro.Items.Add(Common.CmdPrefixInputLayer);
              break;
            /*
            case Common.XmlTagWindowState:
              listBoxMacro.Items.Add(Common.CmdPrefixWindowState);
              break;
            */
            case Common.XmlTagFocus:
              listBoxMacro.Items.Add(Common.CmdPrefixFocus);
              break;

            case Common.XmlTagExit:
              listBoxMacro.Items.Add(Common.CmdPrefixExit);
              break;

            case Common.XmlTagStandby:
              listBoxMacro.Items.Add(Common.CmdPrefixStandby);
              break;

            case Common.XmlTagHibernate:
              listBoxMacro.Items.Add(Common.CmdPrefixHibernate);
              break;

            case Common.XmlTagReboot:
              listBoxMacro.Items.Add(Common.CmdPrefixReboot);
              break;

            case Common.XmlTagShutdown:
              listBoxMacro.Items.Add(Common.CmdPrefixShutdown);
              break;
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("MPControlPlugin: {0}", ex.Message);
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

      if (selected.Equals(Common.UITextRun, StringComparison.OrdinalIgnoreCase))
      {
        ExternalProgram externalProgram = new ExternalProgram();
        if (externalProgram.ShowDialog(this) == DialogResult.OK)
          listBoxMacro.Items.Add(Common.CmdPrefixRun + externalProgram.CommandString);
      }
      else if (selected.Equals(Common.UITextPause, StringComparison.OrdinalIgnoreCase))
      {
        PauseTime pauseTime = new PauseTime();
        if (pauseTime.ShowDialog(this) == DialogResult.OK)
          listBoxMacro.Items.Add(Common.CmdPrefixPause + pauseTime.Time.ToString());
      }
      else if (selected.Equals(Common.UITextSerial, StringComparison.OrdinalIgnoreCase))
      {
        SerialCommand serialCommand = new SerialCommand();
        if (serialCommand.ShowDialog(this) == DialogResult.OK)
          listBoxMacro.Items.Add(Common.CmdPrefixSerial + serialCommand.CommandString);
      }
      else if (selected.Equals(Common.UITextWindowMsg, StringComparison.OrdinalIgnoreCase))
      {
        MessageCommand messageCommand = new MessageCommand();
        if (messageCommand.ShowDialog(this) == DialogResult.OK)
          listBoxMacro.Items.Add(Common.CmdPrefixWindowMsg + messageCommand.CommandString);
      }
      else if (selected.Equals(Common.UITextTcpMsg, StringComparison.OrdinalIgnoreCase))
      {
        TcpMessageCommand tcpMessageCommand = new TcpMessageCommand();
        if (tcpMessageCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        listBoxMacro.Items.Add(Common.CmdPrefixTcpMsg + tcpMessageCommand.CommandString);
      }
      else if (selected.Equals(Common.UITextKeys, StringComparison.OrdinalIgnoreCase))
      {
        KeysCommand keysCommand = new KeysCommand();
        if (keysCommand.ShowDialog(this) == DialogResult.OK)
          listBoxMacro.Items.Add(Common.CmdPrefixKeys + keysCommand.CommandString);
      }
      else if (selected.Equals(Common.UITextMouse, StringComparison.OrdinalIgnoreCase))
      {
        MouseCommand mouseCommand = new MouseCommand();
        if (mouseCommand.ShowDialog(this) == DialogResult.OK)
          listBoxMacro.Items.Add(Common.CmdPrefixMouse + mouseCommand.CommandString);
      }
      else if (selected.Equals(Common.UITextEject, StringComparison.OrdinalIgnoreCase))
      {
        EjectCommand ejectCommand = new EjectCommand();
        if (ejectCommand.ShowDialog(this) == DialogResult.OK)
          listBoxMacro.Items.Add(Common.CmdPrefixEject + ejectCommand.CommandString);
      }
      else if (selected.Equals(Common.UITextGoto, StringComparison.OrdinalIgnoreCase))
      {
        GoToScreen goToScreen = new GoToScreen();
        if (goToScreen.ShowDialog(this) == DialogResult.OK)
          listBoxMacro.Items.Add(Common.CmdPrefixGoto + goToScreen.Screen);
      }
      else if (selected.Equals(Common.UITextPopup, StringComparison.OrdinalIgnoreCase))
      {
        PopupMessage popupMessage = new PopupMessage();
        if (popupMessage.ShowDialog(this) == DialogResult.OK)
          listBoxMacro.Items.Add(Common.CmdPrefixPopup + popupMessage.CommandString);
      }
      else if (selected.Equals(Common.UITextMultiMap, StringComparison.OrdinalIgnoreCase))
      {
        listBoxMacro.Items.Add(Common.CmdPrefixMultiMap + "TOGGLE");
      }
      else if (selected.Equals(Common.UITextMouseMode, StringComparison.OrdinalIgnoreCase))
      {
        listBoxMacro.Items.Add(Common.CmdPrefixMouseMode + "TOGGLE");
      }
      else if (selected.Equals(Common.UITextInputLayer, StringComparison.OrdinalIgnoreCase))
      {
        listBoxMacro.Items.Add(Common.CmdPrefixInputLayer);
      }
      /*
      else if (selected.Equals(Common.UITextWindowState, StringComparison.OrdinalIgnoreCase))
      {
        listBoxMacro.Items.Add(Common.CmdPrefixWindowState);
      }
      */
      else if (selected.Equals(Common.UITextFocus, StringComparison.OrdinalIgnoreCase))
      {
        listBoxMacro.Items.Add(Common.CmdPrefixFocus);
      }
      else if (selected.Equals(Common.UITextExit, StringComparison.OrdinalIgnoreCase))
      {
        listBoxMacro.Items.Add(Common.CmdPrefixExit);
      }
      else if (selected.Equals(Common.UITextStandby, StringComparison.OrdinalIgnoreCase))
      {
        listBoxMacro.Items.Add(Common.CmdPrefixStandby);
      }
      else if (selected.Equals(Common.UITextHibernate, StringComparison.OrdinalIgnoreCase))
      {
        listBoxMacro.Items.Add(Common.CmdPrefixHibernate);
      }
      else if (selected.Equals(Common.UITextReboot, StringComparison.OrdinalIgnoreCase))
      {
        listBoxMacro.Items.Add(Common.CmdPrefixReboot);
      }
      else if (selected.Equals(Common.UITextShutdown, StringComparison.OrdinalIgnoreCase))
      {
        listBoxMacro.Items.Add(Common.CmdPrefixShutdown);
      }
      else if (selected.StartsWith(Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
      {
        BlastCommand blastCommand = new BlastCommand(
          new BlastIrDelegate(MPControlPlugin.BlastIR),
          Common.FolderIRCommands,
          MPControlPlugin.TransceiverInformation.Ports,
          selected.Substring(Common.CmdPrefixBlast.Length));

        if (blastCommand.ShowDialog(this) == DialogResult.OK)
          listBoxMacro.Items.Add(Common.CmdPrefixBlast + blastCommand.CommandString);
      }
      else if (selected.StartsWith(Common.CmdPrefixMacro, StringComparison.OrdinalIgnoreCase))
      {
        listBoxMacro.Items.Add(selected);
      }
      else
      {
        throw new ApplicationException(String.Format("Unknown command in macro command list \"{0}\"", selected));
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

      string fileName = MPControlPlugin.FolderMacros + name + Common.FileExtensionMacro;

      WriteToFile(fileName);

      try
      {
        MPControlPlugin.ProcessMacro(fileName);
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

      string fileName = MPControlPlugin.FolderMacros + name + Common.FileExtensionMacro;

      WriteToFile(fileName);

      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void listBoxCommandSequence_DoubleClick(object sender, EventArgs e)
    {
      if (listBoxMacro.SelectedIndex == -1)
        return;

      string selected = listBoxMacro.SelectedItem as string;

      if (selected.StartsWith(Common.CmdPrefixPause, StringComparison.OrdinalIgnoreCase))
      {
        PauseTime pauseTime = new PauseTime(int.Parse(selected.Substring(Common.CmdPrefixPause.Length)));
        if (pauseTime.ShowDialog(this) == DialogResult.Cancel)
          return;

        int index = listBoxMacro.SelectedIndex;
        listBoxMacro.Items.RemoveAt(index);
        listBoxMacro.Items.Insert(index, Common.CmdPrefixPause + pauseTime.Time.ToString());
        listBoxMacro.SelectedIndex = index;
      }
      else if (selected.StartsWith(Common.CmdPrefixRun, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitRunCommand(selected.Substring(Common.CmdPrefixRun.Length));

        ExternalProgram executeProgram = new ExternalProgram(commands);
        if (executeProgram.ShowDialog(this) == DialogResult.Cancel)
          return;

        int index = listBoxMacro.SelectedIndex;
        listBoxMacro.Items.RemoveAt(index);
        listBoxMacro.Items.Insert(index, Common.CmdPrefixRun + executeProgram.CommandString);
        listBoxMacro.SelectedIndex = index;
      }
      else if (selected.StartsWith(Common.CmdPrefixSerial, StringComparison.OrdinalIgnoreCase))
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
      else if (selected.StartsWith(Common.CmdPrefixWindowMsg, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitWindowMessageCommand(selected.Substring(Common.CmdPrefixWindowMsg.Length));

        MessageCommand messageCommand = new MessageCommand(commands);
        if (messageCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        int index = listBoxMacro.SelectedIndex;
        listBoxMacro.Items.RemoveAt(index);
        listBoxMacro.Items.Insert(index, Common.CmdPrefixWindowMsg + messageCommand.CommandString);
        listBoxMacro.SelectedIndex = index;
      }
      else if (selected.StartsWith(Common.CmdPrefixTcpMsg, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitTcpMessageCommand(selected.Substring(Common.CmdPrefixTcpMsg.Length));
        TcpMessageCommand tcpMessageCommand = new TcpMessageCommand(commands);
        if (tcpMessageCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        int index = listBoxMacro.SelectedIndex;
        listBoxMacro.Items.RemoveAt(index);
        listBoxMacro.Items.Insert(index, Common.CmdPrefixTcpMsg + tcpMessageCommand.CommandString);
        listBoxMacro.SelectedIndex = index;
      }
      else if (selected.StartsWith(Common.CmdPrefixKeys, StringComparison.OrdinalIgnoreCase))
      {
        KeysCommand keysCommand = new KeysCommand(selected.Substring(Common.CmdPrefixKeys.Length));
        if (keysCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        int index = listBoxMacro.SelectedIndex;
        listBoxMacro.Items.RemoveAt(index);
        listBoxMacro.Items.Insert(index, Common.CmdPrefixKeys + keysCommand.CommandString);
        listBoxMacro.SelectedIndex = index;
      }
      else if (selected.StartsWith(Common.CmdPrefixMouse, StringComparison.OrdinalIgnoreCase))
      {
        MouseCommand mouseCommand = new MouseCommand(selected.Substring(Common.CmdPrefixMouse.Length));
        if (mouseCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        int index = listBoxMacro.SelectedIndex;
        listBoxMacro.Items.RemoveAt(index);
        listBoxMacro.Items.Insert(index, Common.CmdPrefixMouse + mouseCommand.CommandString);
        listBoxMacro.SelectedIndex = index;
      }
      else if (selected.StartsWith(Common.CmdPrefixEject, StringComparison.OrdinalIgnoreCase))
      {
        EjectCommand ejectCommand = new EjectCommand(selected.Substring(Common.CmdPrefixEject.Length));
        if (ejectCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        int index = listBoxMacro.SelectedIndex;
        listBoxMacro.Items.RemoveAt(index);
        listBoxMacro.Items.Insert(index, Common.CmdPrefixEject + ejectCommand.CommandString);
        listBoxMacro.SelectedIndex = index;
      }
      else if (selected.StartsWith(Common.CmdPrefixGoto, StringComparison.OrdinalIgnoreCase))
      {
        GoToScreen goToScreen = new GoToScreen(selected.Substring(Common.CmdPrefixGoto.Length));
        if (goToScreen.ShowDialog(this) == DialogResult.Cancel)
          return;

        int index = listBoxMacro.SelectedIndex;
        listBoxMacro.Items.RemoveAt(index);
        listBoxMacro.Items.Insert(index, Common.CmdPrefixGoto + goToScreen.Screen);
        listBoxMacro.SelectedIndex = index;
      }
      else if (selected.StartsWith(Common.CmdPrefixPopup, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitPopupCommand(selected.Substring(Common.CmdPrefixPopup.Length));
        PopupMessage popupMessage = new PopupMessage(commands);
        if (popupMessage.ShowDialog(this) == DialogResult.Cancel)
          return;

        int index = listBoxMacro.SelectedIndex;
        listBoxMacro.Items.RemoveAt(index);
        listBoxMacro.Items.Insert(index, Common.CmdPrefixPopup + popupMessage.CommandString);
        listBoxMacro.SelectedIndex = index;
      }
      else if (selected.StartsWith(Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitBlastCommand(selected.Substring(Common.CmdPrefixBlast.Length));

        BlastCommand blastCommand = new BlastCommand(
          new BlastIrDelegate(MPControlPlugin.BlastIR),
          Common.FolderIRCommands,
          MPControlPlugin.TransceiverInformation.Ports,
          commands);

        if (blastCommand.ShowDialog(this) == DialogResult.Cancel)
          return;

        int index = listBoxMacro.SelectedIndex;
        listBoxMacro.Items.RemoveAt(index);
        listBoxMacro.Items.Insert(index, Common.CmdPrefixBlast + blastCommand.CommandString);
        listBoxMacro.SelectedIndex = index;
      }
    }

    #endregion Implementation

  }

}
