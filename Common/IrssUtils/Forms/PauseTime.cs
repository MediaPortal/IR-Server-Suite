using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IrssUtils.Forms
{

  /// <summary>
  /// Pause Time Command form.
  /// </summary>
  public partial class PauseTime : Form
  {

    #region Properties

    /// <summary>
    /// Gets the time.
    /// </summary>
    /// <value>The time.</value>
    public int Time
    {
      get { return Decimal.ToInt32(numericUpDownPause.Value); }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="PauseTime"/> class.
    /// </summary>
    public PauseTime()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PauseTime"/> class.
    /// </summary>
    /// <param name="time">The intial time.</param>
    public PauseTime(int time)
      : this()
    {      
      if (time > -1)
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
