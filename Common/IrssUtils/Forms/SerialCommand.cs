using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace IrssUtils.Forms
{
  /// <summary>
  /// Serial Command form.
  /// </summary>
  public partial class SerialCommand : Form
  {
    #region Properties

    /// <summary>
    /// Gets the command string.
    /// </summary>
    /// <value>The command string.</value>
    public string CommandString
    {
      get
      {
        return String.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}",
                             textBoxCommand.Text,
                             comboBoxPort.SelectedItem as string,
                             numericUpDownBaudRate.Value,
                             comboBoxParity.SelectedItem as string,
                             numericUpDownDataBits.Value,
                             comboBoxStopBits.SelectedItem as string,
                             checkBoxWaitForResponse.Checked);
      }
    }

    #endregion Properties

    #region Variables

    private readonly string _parametersMessage =
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

    /// <summary>
    /// Initializes a new instance of the <see cref="SerialCommand"/> class.
    /// </summary>
    public SerialCommand() : this(null, String.Empty)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SerialCommand"/> class.
    /// </summary>
    /// <param name="parametersMessage">The optional parameters message.</param>
    public SerialCommand(string parametersMessage) : this(null, parametersMessage)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SerialCommand"/> class.
    /// </summary>
    /// <param name="commands">The command elements.</param>
    public SerialCommand(string[] commands) : this(commands, String.Empty)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SerialCommand"/> class.
    /// </summary>
    /// <param name="commands">The command elements.</param>
    /// <param name="parametersMessage">The optional parameters message.</param>
    public SerialCommand(string[] commands, string parametersMessage)
    {
      InitializeComponent();

      Setup();

      if (!String.IsNullOrEmpty(parametersMessage))
        _parametersMessage += parametersMessage;

      if (commands != null)
      {
        textBoxCommand.Text = commands[0];
        comboBoxPort.SelectedItem = commands[1];
        numericUpDownBaudRate.Value = new Decimal(int.Parse(commands[2]));
        comboBoxParity.SelectedItem = commands[3];
        numericUpDownDataBits.Value = new Decimal(int.Parse(commands[4]));
        comboBoxStopBits.SelectedItem = commands[5];
        checkBoxWaitForResponse.Checked = bool.Parse(commands[6]);
      }
    }

    #endregion Constructors

    private void Setup()
    {
      comboBoxPort.Items.Clear();
      comboBoxPort.Items.AddRange(SerialPort.GetPortNames());
      if (comboBoxPort.Items.Count > 0)
        comboBoxPort.SelectedIndex = 0;

      comboBoxParity.Items.Clear();
      comboBoxParity.Items.AddRange(Enum.GetNames(typeof (Parity)));
      comboBoxParity.SelectedIndex = 0;

      comboBoxStopBits.Items.Clear();
      comboBoxStopBits.Items.AddRange(Enum.GetNames(typeof (StopBits)));
      comboBoxStopBits.SelectedIndex = 1;
    }

    private void buttonParamQuestion_Click(object sender, EventArgs e)
    {
      MessageBox.Show(this, _parametersMessage, "Parameters", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(textBoxCommand.Text.Trim()))
      {
        MessageBox.Show(this, "You must specify a command", "Missing command", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
        return;
      }

      DialogResult = DialogResult.OK;
      Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    private void buttonTest_Click(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(textBoxCommand.Text.Trim()))
      {
        MessageBox.Show(this, "You must specify a command", "Missing command", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
        return;
      }

      try
      {
        string[] commands = Common.SplitSerialCommand(CommandString);
        Common.ProcessSerialCommand(commands);
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Test failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
  }
}