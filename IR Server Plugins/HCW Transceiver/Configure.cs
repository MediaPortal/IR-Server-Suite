using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HcwTransceiver
{

  /// <summary>
  /// Configure the HCW Transceiver plugin.
  /// </summary>
  public partial class Configure : Form
  {

    #region Properties

    /// <summary>
    /// Gets or sets the repeat delay.
    /// </summary>
    /// <value>The repeat delay.</value>
    public int RepeatDelay
    {
      get { return Decimal.ToInt32(numericUpDownButtonRepeatDelay.Value); }
      set { numericUpDownButtonRepeatDelay.Value = new Decimal(value); }
    }
    /// <summary>
    /// Gets or sets the blast repeats.
    /// </summary>
    /// <value>The blast repeats.</value>
    public int BlastRepeats
    {
      get { return Decimal.ToInt32(numericUpDownRepeatCount.Value); }
      set { numericUpDownRepeatCount.Value = new Decimal(value); }
    }
    /// <summary>
    /// Gets or sets the learn timeout.
    /// </summary>
    /// <value>The learn timeout.</value>
    public int LearnTimeout
    {
      get { return Decimal.ToInt32(numericUpDownLearnTimeout.Value); }
      set { numericUpDownLearnTimeout.Value = new Decimal(value); }
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
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    #endregion Buttons

  }

}
