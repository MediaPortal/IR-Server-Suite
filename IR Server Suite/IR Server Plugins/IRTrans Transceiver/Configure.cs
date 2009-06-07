using System;
using System.Windows.Forms;

namespace InputService.Plugin
{
  internal partial class Configure : Form
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
      DialogResult = DialogResult.OK;
      Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    #endregion Buttons
  }
}