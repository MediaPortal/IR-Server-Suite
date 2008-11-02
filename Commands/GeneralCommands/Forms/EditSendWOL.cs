using System;
using System.Windows.Forms;

namespace Commands.General
{
  /// <summary>
  /// Edit Send WOL Command form.
  /// </summary>
  internal partial class EditSendWOL : Form
  {
    #region Properties

    /// <summary>
    /// Gets the command parameters.
    /// </summary>
    /// <value>The command parameters.</value>
    public string[] Parameters
    {
      get
      {
        return new string[]
                 {
                   textBoxMac.Text.Trim(),
                   numericUpDownPort.Value.ToString(),
                   textBoxPassword.Text.Trim()
                 };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EditSendWOL"/> class.
    /// </summary>
    /// <param name="parameters">The command parameters.</param>
    public EditSendWOL(string[] parameters)
    {
      InitializeComponent();

      textBoxMac.Text = parameters[0];
      if (!String.IsNullOrEmpty(parameters[1]))
        numericUpDownPort.Value = Decimal.Parse(parameters[1]);
      textBoxPassword.Text = parameters[2];
    }

    #endregion

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