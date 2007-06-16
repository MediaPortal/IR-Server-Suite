using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IrssUtils.Forms
{

  public partial class TcpMessageCommand : Form
  {

    #region Properties

    public string CommandString
    {
      get
      {
        return
          textBoxIP.Text + "|" +
          numericUpDownPort.Value.ToString() + "|" +
          textBoxText.Text;
      }
    }

    #endregion Properties

    #region Constructors

    public TcpMessageCommand() : this(null) { }
    public TcpMessageCommand(string[] commands)
    {
      InitializeComponent();

      if (commands != null)
      {
        textBoxIP.Text = commands[0];
        numericUpDownPort.Value = Convert.ToDecimal(commands[1]);
        textBoxText.Text = commands[2];
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
