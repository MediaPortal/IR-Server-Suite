using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Net;
using System.Windows.Forms;

namespace InputService.Plugin
{

  partial class Configure : Form
  {

    #region Properties
    
    public IPAddress ServerIP
    {
      get { return IPAddress.Parse(textBoxServerAddress.Text); }
      set { textBoxServerAddress.Text = value.ToString(); }
    }
    public int ServerPort
    {
      get { return Decimal.ToInt32(numericUpDownServerPort.Value); }
      set { numericUpDownServerPort.Value = new Decimal(value); }
    }
    public bool StartServer
    {
      get { return checkBoxStartServer.Checked; }
      set { checkBoxStartServer.Checked = value; }
    }
    public string ServerPath
    {
      get { return textBoxServerPath.Text; }
      set { textBoxServerPath.Text = value; }
    }
    public int ButtonReleaseTime
    {
      get { return Decimal.ToInt32(numericUpDownButtonReleaseTime.Value); }
      set { numericUpDownButtonReleaseTime.Value = new Decimal(value); }
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

    private void buttonLocate_Click(object sender, EventArgs e)
    {
      if (openFileDialog.ShowDialog(this) == DialogResult.OK)
        textBoxServerPath.Text = openFileDialog.FileName;
    }

    private void buttonCreateIRFiles_Click(object sender, EventArgs e)
    {
      CreateIRFile createIRFile = new CreateIRFile();
      createIRFile.ShowDialog(this);
    }

    #endregion Buttons

  }

}
