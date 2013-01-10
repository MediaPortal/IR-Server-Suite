using System;

namespace IrssCommands.General
{
  public partial class SendWOLConfig : BaseCommandConfig
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
            textBoxMac.Text.Trim(),
            numericUpDownPort.Value.ToString(),
            textBoxPassword.Text.Trim()
          };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SendWOLConfig"/> class.
    /// </summary>
    private SendWOLConfig()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SendWOLConfig"/> class.
    /// </summary>
    /// <param name="parameters">The command elements.</param>
    public SendWOLConfig(string[] parameters)
      : this()
    {
      textBoxMac.Text = parameters[0];

      if (!String.IsNullOrEmpty(parameters[1]))
        numericUpDownPort.Value = Decimal.Parse(parameters[1]);

      textBoxPassword.Text = parameters[2];
    }

    #endregion Constructors
  }
}