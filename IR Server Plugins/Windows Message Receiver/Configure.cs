using System;
using System.Windows.Forms;

namespace InputService.Plugin
{
  internal partial class Configure : Form
  {
    #region Properties

    public int MessageType
    {
      get { return Decimal.ToInt32(numericUpDownMessageType.Value); }
      set { numericUpDownMessageType.Value = new Decimal(value); }
    }

    public int WParam
    {
      get { return Decimal.ToInt32(numericUpDownWParam.Value); }
      set { numericUpDownWParam.Value = new Decimal(value); }
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

    private void Configure_Load(object sender, EventArgs e)
    {
      textBoxWindowTitle.Text = String.Format(
        "To send windows messages to this receiver target the following window title:\r\n\r\n{0}",
        WindowsMessageReceiver.WindowTitle);
    }
  }
}