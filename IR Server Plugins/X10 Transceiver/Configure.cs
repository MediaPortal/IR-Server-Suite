using System;
using System.Windows.Forms;

namespace InputService.Plugin
{
  /// <summary>
  /// Configure the HCW Transceiver plugin.
  /// </summary>
  internal partial class Configure : Form
  {
    #region variables

    private X10Transceiver x10Transceiver;
    private int count;

    #endregion variables

    #region Properties

    /// <summary>
    /// Gets or sets if channel control is used
    /// </summary>
    /// <value>Use channel control</value>
    public bool UseChannelControl
    {
      get { return checkBoxUseChannelControl.Checked; }
      set
      {
        checkBoxUseChannelControl.Checked = value;
        numericUpDownButtonChannelNumber.Enabled = checkBoxUseChannelControl.Checked;
        buttonGetChannelNumber.Enabled = checkBoxUseChannelControl.Checked;
      }
    }

    /// <summary>
    /// Gets or sets the channel number
    /// </summary>
    /// <value>The channel number</value>
    public int ChannelNumber
    {
      get { return Decimal.ToInt32(numericUpDownButtonChannelNumber.Value); }
      set { numericUpDownButtonChannelNumber.Value = new Decimal(value); }
    }

    /// <summary>
    /// Sets the corresponding X10Transceiver
    /// </summary>
    public X10Transceiver X10Transceiver
    {
      set { x10Transceiver = value; }
    }

    #endregion Properties

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Configure"/> class.
    /// </summary>
    public Configure()
    {
      InitializeComponent();
    }

    #endregion Constructor

    #region Buttons

    private void buttonOK_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.OK;
      Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    private void checkBoxUseChannelControl_CheckedChanged(object sender, EventArgs e)
    {
      numericUpDownButtonChannelNumber.Enabled = checkBoxUseChannelControl.Checked;
      buttonGetChannelNumber.Enabled = checkBoxUseChannelControl.Checked;
    }

    #endregion Buttons

    private void buttonGetChannelNumber_Click(object sender, EventArgs e)
    {
      try
      {
        x10Transceiver.StartGetChannelNumber();
        buttonOK.Enabled = false;
        buttonCancel.Enabled = false;
        buttonGetChannelNumber.Enabled = false;
        numericUpDownButtonChannelNumber.Enabled = false;
        checkBoxUseChannelControl.Enabled = false;
        count = 0;
        timer1.Start();
      }
      catch
      {
        MessageBox.Show("Error while starting X10 device", "X10 Configuration", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
      }
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      int number = x10Transceiver.GetChannelNumber();
      if (number == -1)
      {
        count++;
        if (count == 50)
        {
          timer1.Stop();
          try
          {
            x10Transceiver.StopGetChannelNumber();
          }
          finally
          {
            buttonOK.Enabled = true;
            buttonCancel.Enabled = true;
            buttonGetChannelNumber.Enabled = true;
            numericUpDownButtonChannelNumber.Enabled = true;
            checkBoxUseChannelControl.Enabled = true;
          }
        }
      }
      else
      {
        timer1.Stop();
        try
        {
          x10Transceiver.StopGetChannelNumber();
        }
        finally
        {
          checkBoxUseChannelControl.Enabled = true;
          numericUpDownButtonChannelNumber.Enabled = true;
          numericUpDownButtonChannelNumber.Value = number;
          buttonOK.Enabled = true;
          buttonCancel.Enabled = true;
          buttonGetChannelNumber.Enabled = true;
        }
      }
    }
  }
}