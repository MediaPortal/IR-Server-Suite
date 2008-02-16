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

    public string ServerAddress
    {
      get { return textBoxServerAddress.Text; }
      set { textBoxServerAddress.Text = value; }
    }
    public int ServerPort
    {
      get { return decimal.ToInt32(numericUpDownServerPort.Value); }
      set { numericUpDownServerPort.Value = new decimal(value); }
    }
    public string RemoteModel
    {
      get { return textBoxRemoteModel.Text; }
      set { textBoxRemoteModel.Text = value; }
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
