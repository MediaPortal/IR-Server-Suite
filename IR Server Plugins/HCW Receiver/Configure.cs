using System;
using System.Windows.Forms;

namespace InputService.Plugin
{
  /// <summary>
  /// Configure the HCW Transceiver plugin.
  /// </summary>
  internal partial class Configure : Form
  {
    #region Properties

    /// <summary>
    /// Gets or sets the repeat delay.
    /// </summary>
    /// <value>The repeat delay.</value>
    public int RepeatDelay
    {
      get { return Decimal.ToInt32(numericUpDownButtonRepeatDelay.Value); }
      set { numericUpDownButtonRepeatDelay.Value = new Decimal(value); }
    }

    #endregion Properties

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Configure"/> class.
    /// </summary>
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