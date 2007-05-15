using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

namespace IrssUtils.Forms
{

  public partial class SerialCommand : Form
  {

    #region Properties

    public string CommandString
    {
      get
      {
        return String.Format("{0}|{1}|{2}|{3}|{4}|{5}",
          textBoxCommand.Text,
          comboBoxPort.SelectedItem as string,
          numericUpDownBaudRate.Value.ToString(),
          comboBoxParity.SelectedItem as string,
          numericUpDownDataBits.Value.ToString(),
          comboBoxStopBits.SelectedItem as string);
      }
    }

    #endregion Properties

    #region Variables

    string _parametersMessage = 
@"\a = Alert (ascii 7)
\b = Backspace (ascii 8)
\f = Form Feed (ascii 12)
\n = Line Feed (ascii 10)
\r = Carriage Return (ascii 13)
\t = Tab (ascii 9)
\v = Vertical Tab (ascii 11)
\x = Hex Value (\x0Fh = ascii char 15, \x8h = ascii char 8)
\0 = Null (ascii 0)

";

    #endregion Variables

    #region Constructors

    public SerialCommand() : this(null, String.Empty) { }
    public SerialCommand(string parametersMessage) : this(null, parametersMessage) { }
    public SerialCommand(string[] commands) : this(commands, String.Empty) { }
    public SerialCommand(string[] commands, string parametersMessage)
    {
      InitializeComponent();

      Setup();

      if (!String.IsNullOrEmpty(parametersMessage))
        _parametersMessage += parametersMessage;

      if (commands != null)
      {
        textBoxCommand.Text           = commands[0];
        comboBoxPort.SelectedItem     = commands[1];
        numericUpDownBaudRate.Value   = new Decimal(int.Parse(commands[2]));
        comboBoxParity.SelectedItem   = commands[3];
        numericUpDownDataBits.Value   = new Decimal(int.Parse(commands[4]));
        comboBoxStopBits.SelectedItem = commands[5];
      }
    }

    #endregion Constructors

    void Setup()
    {
      comboBoxPort.Items.Clear();
      comboBoxPort.Items.AddRange(SerialPort.GetPortNames());
      if (comboBoxPort.Items.Count > 0)
        comboBoxPort.SelectedIndex = 0;

      comboBoxParity.Items.Clear();
      comboBoxParity.Items.AddRange(Enum.GetNames(typeof(Parity)));
      comboBoxParity.SelectedIndex = 0;

      comboBoxStopBits.Items.Clear();
      comboBoxStopBits.Items.AddRange(Enum.GetNames(typeof(StopBits)));
      comboBoxStopBits.SelectedIndex = 0;
    }

    private void buttonParamQuestion_Click(object sender, EventArgs e)
    {
      MessageBox.Show(this, _parametersMessage, "Parameters", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(textBoxCommand.Text.Trim()))
      {
        MessageBox.Show(this, "You must specify a command", "Missing command", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
      }

      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void buttonTest_Click(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(textBoxCommand.Text.Trim()))
      {
        MessageBox.Show(this, "You must specify a command", "Missing command", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
      }

      try
      {
        string[] commands = Common.SplitSerialCommand(this.CommandString);
        Common.ProcessSerialCommand(commands);
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Test failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

  }

}
