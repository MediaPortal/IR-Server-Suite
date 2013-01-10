using System;
using System.IO.Ports;

namespace IrssCommands.General
{
  public partial class SerialConfig : BaseCommandConfig
  {
    #region Properties

    /// <summary>
    /// Gets the command parameters.
    /// </summary>
    /// <value>The command parameters.</value>
    public override string[] Parameters
    {
      get
      {
        return new[]
          {
            textBoxCommand.Text.Trim(),
            comboBoxPort.SelectedText,
            numericUpDownBaudRate.Value.ToString(),
            Enum.GetName(typeof (Parity), comboBoxParity.SelectedItem),
            numericUpDownDataBits.Value.ToString(),
            Enum.GetName(typeof (StopBits), comboBoxStopBits.SelectedItem),
            checkBoxWaitForResponse.Checked.ToString()
          };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SerialConfig"/> class.
    /// </summary>
    private SerialConfig()
    {
      InitializeComponent();

      comboBoxPort.Items.Clear();
      comboBoxPort.Items.AddRange(SerialPort.GetPortNames());
      if (comboBoxPort.Items.Count > 0)
        comboBoxPort.SelectedIndex = 0;

      comboBoxParity.Items.Clear();
      foreach (Parity p in Enum.GetValues(typeof (Parity)))
        comboBoxParity.Items.Add(p);

      comboBoxStopBits.Items.Clear();
      foreach (StopBits s in Enum.GetValues(typeof (StopBits)))
        comboBoxStopBits.Items.Add(s);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SerialConfig"/> class.
    /// </summary>
    /// <param name="parameters">The command elements.</param>
    public SerialConfig(string[] parameters)
      : this()
    {
      textBoxCommand.Text = parameters[0];
      comboBoxPort.SelectedItem = parameters[1];

      if (!String.IsNullOrEmpty(parameters[2]))
        numericUpDownBaudRate.Value = Convert.ToDecimal(parameters[2]);

      comboBoxParity.SelectedItem =
        (Parity) Enum.Parse(typeof (Parity), parameters[3], true);

      if (!String.IsNullOrEmpty(parameters[4]))
        numericUpDownDataBits.Value = Convert.ToDecimal(parameters[4]);

      comboBoxStopBits.SelectedItem =
        (StopBits) Enum.Parse(typeof (StopBits), parameters[5], true);
      checkBoxWaitForResponse.Checked = bool.Parse(parameters[6]);
    }

    #endregion Constructors

    #region Implementation

    private void buttonParamQuestion_Click(object sender, EventArgs e)
    {
#warning readd button code
    }

    #endregion Implementation
  }
}