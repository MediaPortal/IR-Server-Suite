using System;

namespace IrssCommands.General
{
  public partial class HttpMessageConfig : BaseCommandConfig
  {
    #region Properties

    /// <summary>
    /// Gets the command parameters.
    /// </summary>
    /// <value>The command parameters.</value>
    public override string[] Parameters
    {
      get
      {
        return new[]
          {
            textBoxAddress.Text.Trim(),
            numericUpDownTimeout.Value.ToString(),
            textBoxUsername.Text.Trim(),
            textBoxPassword.Text.Trim()
          };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpMessageConfig"/> class.
    /// </summary>
    private HttpMessageConfig()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpMessageConfig"/> class.
    /// </summary>
    /// <param name="parameters">The command elements.</param>
    public HttpMessageConfig(string[] parameters)
      : this()
    {
      textBoxAddress.Text = parameters[0];
      if (!String.IsNullOrEmpty(parameters[1]))
        numericUpDownTimeout.Value = Convert.ToDecimal(parameters[1]);
      textBoxUsername.Text = parameters[2];
      textBoxPassword.Text = parameters[3];

      checkBoxUsernameAndPassword.Checked =
        !(string.IsNullOrEmpty(textBoxUsername.Text) && string.IsNullOrEmpty(textBoxPassword.Text));
      checkBoxUsernameAndPassword_CheckedChanged(null, null);
    }

    #endregion Constructors

    #region Implementation

    private void checkBoxUsernameAndPassword_CheckedChanged(object sender, EventArgs e)
    {
      labelUsername.Enabled = checkBoxUsernameAndPassword.Checked;
      textBoxUsername.Enabled = checkBoxUsernameAndPassword.Checked;
      labelPassword.Enabled = checkBoxUsernameAndPassword.Checked;
      textBoxPassword.Enabled = checkBoxUsernameAndPassword.Checked;

      if (!checkBoxUsernameAndPassword.Checked)
        textBoxUsername.Text = textBoxPassword.Text = string.Empty;
    }

    #endregion Implementation
  }
}