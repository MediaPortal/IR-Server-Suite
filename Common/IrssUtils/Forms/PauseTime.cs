using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IrssUtils.Forms
{

  public partial class PauseTime : Form
  {

    #region Properties

    public int Time
    {
      get { return Decimal.ToInt32(numericUpDownPause.Value); }
    }

    #endregion Properties

    #region Constructors

    public PauseTime() : this(-1) { }
    public PauseTime(int time)
    {
      InitializeComponent();
      
      if (time != -1)
        numericUpDownPause.Value = new Decimal(time);
    }

    #endregion Constructors

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

  }

}
