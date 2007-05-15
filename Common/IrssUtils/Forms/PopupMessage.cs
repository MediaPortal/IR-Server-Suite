using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IrssUtils.Forms
{

  public partial class PopupMessage : Form
  {

    #region Properties

    public string CommandString
    {
      get
      {
        return
          textBoxHeading.Text + "|" +
          textBoxText.Text + "|" +
          numericUpDownTimeout.Value.ToString();
      }
    }

    #endregion Properties

    #region Constructors

    public PopupMessage() : this(null) { }
    public PopupMessage(string[] commands)
    {
      InitializeComponent();

      if (commands != null)
      {
        textBoxHeading.Text = commands[0];
        textBoxText.Text = commands[1];
        numericUpDownTimeout.Value = Convert.ToDecimal(commands[2]);
      }
    }

    #endregion Constructors

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
