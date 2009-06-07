using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace InputService.Plugin
{

  partial class Configure : Form
  {

    #region Properties

    public int RepeatDelay
    {
      get { return Decimal.ToInt32(numericUpDownButtonRepeatDelay.Value); }
      set { numericUpDownButtonRepeatDelay.Value = new Decimal(value); }
    }
    public int BlastRepeats
    {
      get { return Decimal.ToInt32(numericUpDownRepeatCount.Value); }
      set { numericUpDownRepeatCount.Value = new Decimal(value); }
    }
    public int LearnTimeout
    {
      get { return Decimal.ToInt32(numericUpDownLearnTimeout.Value); }
      set { numericUpDownLearnTimeout.Value = new Decimal(value); }
    }

    #endregion Properties

    #region Constructor

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
