using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IrssUtils.Forms
{

  public partial class KeysCommand : Form
  {

    #region Properties

    public string CommandString
    {
      get
      {
        return textBoxKeystrokes.Text;
      }
    }

    #endregion Properties

    #region Constructors

    public KeysCommand() : this(null) { }
    public KeysCommand(string command)
    {
      InitializeComponent();

      if (command != null)
        textBoxKeystrokes.Text = command;
    }

    #endregion

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
