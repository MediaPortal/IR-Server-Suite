using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
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

  partial class StbSetup : UserControl
  {

    #region Constants

    const string parameterInfo =
@"%1 = Current channel number digit (-1 for Select/Pre-Change)
%2 = Full channel number string
%3 = Blaster port (0 = Both, 1 = Port 1, 2 = Port 2)";

    #endregion Constants

    #region Variables

    int _cardId;

    #endregion Variables

    #region Properties

    public int PauseTime
    {
      get { return Decimal.ToInt32(numericUpDownPauseTime.Value); }
    }
    public bool SendSelect
    {
      get { return checkBoxSendSelect.Checked; }
    }
    public bool DoubleChannelSelect
    {
      get { return checkBoxDoubleSelect.Checked; }
    }
    public int RepeatChannelCommands
    {
      get { return Decimal.ToInt32(numericUpDownRepeat.Value); }
    }
    public int ChannelDigits
    {
      get
      {
        int chDigits = comboBoxChDigits.SelectedIndex;
        if (chDigits > 0)
          chDigits++;
        return chDigits;
      }
    }
    public int RepeatPauseTime
    {
      get { return Decimal.ToInt32(numericUpDownRepeatDelay.Value); }
    }
    public bool UsePreChangeCommand
    {
      get { return checkBoxUsePreChange.Checked; }
    }

    public string[] Digits
    {
      get
      {
        string[] _digits = new string[10];
        for (int i = 0; i < 10; i++)
          _digits[i] = listViewExternalCommands.Items[i].SubItems[1].Text;
        return _digits; 
      }
    }
    public string SelectCommand
    {
      get { return listViewExternalCommands.Items[10].SubItems[1].Text; }
    }
    public string PreChangeCommand
    {
      get { return listViewExternalCommands.Items[11].SubItems[1].Text; }
    }

    #endregion Properties

    #region Constructor

    public StbSetup(int cardId)
    {
      InitializeComponent();

      _cardId = cardId;

      // Setup commands combo box
      comboBoxCommands.Items.Add(Common.UITextRun);
      comboBoxCommands.Items.Add(Common.UITextSerial);
      comboBoxCommands.Items.Add(Common.UITextWindowMsg);
      comboBoxCommands.Items.Add(Common.UITextTcpMsg);
      comboBoxCommands.Items.Add(Common.UITextHttpMsg);
      comboBoxCommands.Items.Add(Common.UITextKeys);
      comboBoxCommands.Items.Add(Common.UITextPopup);

      string[] fileList = TV2BlasterPlugin.GetFileList(true);
      if (fileList != null)
        comboBoxCommands.Items.AddRange(fileList);

      comboBoxCommands.SelectedIndex = 0;

      // Setup command list
      ListViewItem item;
      string[] subItems = new string[2];
      for (int i = 0; i < 10; i++)
      {
        subItems[0] = "Digit " + i.ToString();
        subItems[1] = String.Empty;
        item = new ListViewItem(subItems);
        listViewExternalCommands.Items.Add(item);
      }

      subItems[0] = "Select";
      subItems[1] = String.Empty;
      item = new ListViewItem(subItems);
      listViewExternalCommands.Items.Add(item);

      subItems[0] = "PreChange";
      subItems[1] = String.Empty;
      item = new ListViewItem(subItems);
      listViewExternalCommands.Items.Add(item);

      SetToCard(_cardId);
    }

    #endregion Constructor

    #region Public Methods

    public void SetToCard(int cardId)
    {
      ExternalChannelConfig config = TV2BlasterPlugin.GetExternalChannelConfig(cardId);

      if (config == null)
        return;

      // Setup command list.
      for (int i = 0; i < 10; i++)
        listViewExternalCommands.Items[i].SubItems[1].Text  = config.Digits[i];

      listViewExternalCommands.Items[10].SubItems[1].Text   = config.SelectCommand;
      listViewExternalCommands.Items[11].SubItems[1].Text   = config.PreChangeCommand;

      // Setup options.
      numericUpDownPauseTime.Value  = config.PauseTime;
      checkBoxSendSelect.Checked    = config.SendSelect;
      checkBoxDoubleSelect.Checked  = config.DoubleChannelSelect;
      numericUpDownRepeat.Value     = config.RepeatChannelCommands;

      checkBoxDoubleSelect.Enabled  = checkBoxSendSelect.Checked;

      int channelDigitsSelect = config.ChannelDigits;
      if (channelDigitsSelect > 0)
        channelDigitsSelect--;
      comboBoxChDigits.SelectedIndex  = channelDigitsSelect;

      checkBoxUsePreChange.Checked    = config.UsePreChangeCommand;
      numericUpDownRepeatDelay.Value  = new Decimal(config.RepeatPauseTime);
    }

    public void SetToConfig(int cardId)
    {
      ExternalChannelConfig config = TV2BlasterPlugin.GetExternalChannelConfig(cardId);

      if (config == null)
        return;

      config.CardId = cardId;

      config.PauseTime = Decimal.ToInt32(numericUpDownPauseTime.Value);
      config.SendSelect = checkBoxSendSelect.Checked;
      config.DoubleChannelSelect = checkBoxDoubleSelect.Checked;
      config.RepeatChannelCommands = Decimal.ToInt32(numericUpDownRepeat.Value);

      int chDigits = comboBoxChDigits.SelectedIndex;
      if (chDigits > 0)
        chDigits++;
      config.ChannelDigits = chDigits;

      config.RepeatPauseTime = Decimal.ToInt32(numericUpDownRepeatDelay.Value);
      config.UsePreChangeCommand = checkBoxUsePreChange.Checked;

      config.SelectCommand = listViewExternalCommands.Items[10].SubItems[1].Text;
      config.PreChangeCommand = listViewExternalCommands.Items[11].SubItems[1].Text;

      for (int i = 0; i < 10; i++)
        config.Digits[i] = listViewExternalCommands.Items[i].SubItems[1].Text;
    }

    public void SetToXml(string xmlFile)
    {
      if (xmlFile.Equals("Clear all", StringComparison.OrdinalIgnoreCase))
      {
        foreach (ListViewItem item in listViewExternalCommands.Items)
          item.SubItems[1].Text = String.Empty;

        return;
      }

      string fileName = Common.FolderSTB + xmlFile + ".xml";

      XmlDocument doc = new XmlDocument();
      doc.Load(fileName);

      XmlNodeList nodeList = doc.DocumentElement.ChildNodes;

      string command;
      BlastCommand blastCommand;

      bool useForAllBlastCommands = false;
      string useForAllBlasterPort = String.Empty;

      int blastCommandCount = 0;
      for (int i = 0; i < 12; i++)
      {
        if (i == 10)
          command = IrssUtils.XML.GetString(nodeList, "SelectCommand", String.Empty);
        else if (i == 11)
          command = IrssUtils.XML.GetString(nodeList, "PreChangeCommand", String.Empty);
        else
          command = IrssUtils.XML.GetString(nodeList, String.Format("Digit{0}", i), String.Empty);

        if (command.StartsWith(Common.CmdPrefixSTB, StringComparison.OrdinalIgnoreCase))
          blastCommandCount++;
      }

      for (int i = 0; i < 12; i++)
      {
        if (i == 10)
          command = IrssUtils.XML.GetString(nodeList, "SelectCommand", String.Empty);
        else if (i == 11)
          command = IrssUtils.XML.GetString(nodeList, "PreChangeCommand", String.Empty);
        else
          command = IrssUtils.XML.GetString(nodeList, String.Format("Digit{0}", i), String.Empty);

        if (command.StartsWith(Common.CmdPrefixSTB, StringComparison.OrdinalIgnoreCase))
        {
          blastCommand = new BlastCommand(
            new BlastIrDelegate(TV2BlasterPlugin.BlastIR),
            Common.FolderSTB,
            TV2BlasterPlugin.TransceiverInformation.Ports,
            command.Substring(Common.CmdPrefixSTB.Length),
            blastCommandCount--);

          if (useForAllBlastCommands)
          {
            blastCommand.BlasterPort = useForAllBlasterPort;
            listViewExternalCommands.Items[i].SubItems[1].Text = Common.CmdPrefixSTB + blastCommand.CommandString;
          }
          else
          {
            if (blastCommand.ShowDialog(this) == DialogResult.OK)
            {
              if (blastCommand.UseForAll)
              {
                useForAllBlastCommands = true;
                useForAllBlasterPort = blastCommand.BlasterPort;
              }
              listViewExternalCommands.Items[i].SubItems[1].Text = Common.CmdPrefixSTB + blastCommand.CommandString;
            }
            else
            {
              blastCommand = new BlastCommand(
                new BlastIrDelegate(TV2BlasterPlugin.BlastIR),
                Common.FolderSTB,
                TV2BlasterPlugin.TransceiverInformation.Ports,
                command.Substring(Common.CmdPrefixSTB.Length));

              listViewExternalCommands.Items[i].SubItems[1].Text = Common.CmdPrefixSTB + blastCommand.CommandString;
            }
          }
        }
        else
        {
          listViewExternalCommands.Items[i].SubItems[1].Text = command;
        }
      }

      numericUpDownPauseTime.Value = new Decimal(IrssUtils.XML.GetInt(nodeList, "PauseTime", Decimal.ToInt32(numericUpDownPauseTime.Value)));
      checkBoxUsePreChange.Checked = IrssUtils.XML.GetBool(nodeList, "UsePreChangeCommand", checkBoxUsePreChange.Checked);
      checkBoxSendSelect.Checked = IrssUtils.XML.GetBool(nodeList, "SendSelect", checkBoxSendSelect.Checked);
      checkBoxDoubleSelect.Checked = IrssUtils.XML.GetBool(nodeList, "DoubleChannelSelect", checkBoxDoubleSelect.Checked);
      numericUpDownRepeat.Value = new Decimal(IrssUtils.XML.GetInt(nodeList, "RepeatChannelCommands", Decimal.ToInt32(numericUpDownRepeat.Value)));
      numericUpDownRepeatDelay.Value = new Decimal(IrssUtils.XML.GetInt(nodeList, "RepeatDelay", Decimal.ToInt32(numericUpDownRepeatDelay.Value)));

      int digitsWas = comboBoxChDigits.SelectedIndex;
      if (digitsWas > 0)
        digitsWas--;
      int digits = IrssUtils.XML.GetInt(nodeList, "ChannelDigits", digitsWas);
      if (digits > 0)
        digits++;
      comboBoxChDigits.SelectedIndex = digits;
    }

    public void Save()
    {
      SetToConfig(_cardId);
    }

    #endregion Public Methods

    #region Private Methods

    private void listViewExternalCommands_KeyDown(object sender, KeyEventArgs e)
    {
      if (listViewExternalCommands.SelectedIndices.Count > 0 && e.KeyCode == Keys.Delete)
        foreach (ListViewItem listViewItem in listViewExternalCommands.SelectedItems)
          listViewItem.SubItems[1].Text = String.Empty;
    }

    private void checkBoxSendSelect_CheckedChanged(object sender, EventArgs e)
    {
      checkBoxDoubleSelect.Enabled = checkBoxSendSelect.Checked;
    }

    private void listViewExternalCommands_DoubleClick(object sender, EventArgs e)
    {
      if (listViewExternalCommands.SelectedIndices.Count != 1)
        return;

      try
      {
        string selected = listViewExternalCommands.SelectedItems[0].SubItems[1].Text;
        string newCommand = null;

        if (selected.StartsWith(Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitBlastCommand(selected.Substring(Common.CmdPrefixBlast.Length));

          BlastCommand blastCommand = new BlastCommand(
            new BlastIrDelegate(TV2BlasterPlugin.BlastIR),
            Common.FolderIRCommands,
            TV2BlasterPlugin.TransceiverInformation.Ports,
            commands);

          if (blastCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixBlast + blastCommand.CommandString;
        }
        else if (selected.StartsWith(Common.CmdPrefixSTB, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitBlastCommand(selected.Substring(Common.CmdPrefixSTB.Length));

          BlastCommand blastCommand = new BlastCommand(
            new BlastIrDelegate(TV2BlasterPlugin.BlastIR),
            Common.FolderSTB,
            TV2BlasterPlugin.TransceiverInformation.Ports,
            commands);

          if (blastCommand.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixSTB + blastCommand.CommandString;
        }
        else if (selected.StartsWith(Common.CmdPrefixRun, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitRunCommand(selected.Substring(Common.CmdPrefixRun.Length));
          
          ExternalProgram executeProgram = new ExternalProgram(commands, parameterInfo);
          if (executeProgram.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixRun + executeProgram.CommandString;
        }
        else if (selected.StartsWith(Common.CmdPrefixSerial, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitSerialCommand(selected.Substring(Common.CmdPrefixSerial.Length));
          
          SerialCommand serialCommand = new SerialCommand(commands, parameterInfo);
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
        else if (selected.StartsWith(Common.CmdPrefixPopup, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitPopupCommand(selected.Substring(Common.CmdPrefixPopup.Length));

          PopupMessage popupMessage = new PopupMessage(commands);
          if (popupMessage.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixPopup + popupMessage.CommandString;
        }

        if (!String.IsNullOrEmpty(newCommand))
          listViewExternalCommands.SelectedItems[0].SubItems[1].Text = newCommand;
      }
      catch (Exception ex)
      {
        Log.Error("TV2BlasterPlugin: {0}", ex.ToString());
        MessageBox.Show(this, ex.Message, "Failed to edit command", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void buttonSet_Click(object sender, EventArgs e)
    {
      if (listViewExternalCommands.SelectedIndices.Count == 0 || comboBoxCommands.SelectedIndex == -1)
        return;

      try
      {
        string selected = comboBoxCommands.SelectedItem as string;
        string newCommand = null;

        if (selected.Equals(Common.UITextRun, StringComparison.OrdinalIgnoreCase))
        {
          ExternalProgram externalProgram = new ExternalProgram(parameterInfo);
          if (externalProgram.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixRun + externalProgram.CommandString;
        }
        else if (selected.Equals(Common.UITextSerial, StringComparison.OrdinalIgnoreCase))
        {
          SerialCommand serialCommand = new SerialCommand(parameterInfo);
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
        else if (selected.Equals(Common.UITextPopup, StringComparison.OrdinalIgnoreCase))
        {
          PopupMessage popupMessage = new PopupMessage();
          if (popupMessage.ShowDialog(this) == DialogResult.OK)
            newCommand = Common.CmdPrefixPopup + popupMessage.CommandString;
        }
        else if (selected.StartsWith(Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
        {
          BlastCommand blastCommand = new BlastCommand(
            new BlastIrDelegate(TV2BlasterPlugin.BlastIR),
            Common.FolderIRCommands,
            TV2BlasterPlugin.TransceiverInformation.Ports,
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
          throw new ApplicationException(String.Format("Invalid command in STB Setup: {0}", selected));
        }

        if (!String.IsNullOrEmpty(newCommand))
          foreach (ListViewItem item in listViewExternalCommands.SelectedItems)
            item.SubItems[1].Text = newCommand;
      }
      catch (Exception ex)
      {
        Log.Error("TV2BlasterPlugin: {0}", ex.ToString());
        MessageBox.Show(this, ex.Message, "Failed to set command", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #endregion Private Methods

  }

}
