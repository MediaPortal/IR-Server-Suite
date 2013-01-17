using System;

namespace IrssCommands.MediaPortal
{
  public partial class SendKeyConfig : BaseCommandConfig
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
            numericUpDownChar.Value.ToString(),
            numericUpDownCode.Value.ToString()
          };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SendKeyConfig"/> class.
    /// </summary>
    private SendKeyConfig()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SendKeyConfig"/> class.
    /// </summary>
    /// <param name="parameters">The command elements.</param>
    public SendKeyConfig(string[] parameters)
      : this()
    {
      if (!String.IsNullOrEmpty(parameters[0]))
        numericUpDownChar.Value = Convert.ToDecimal(parameters[0]);
      if (!String.IsNullOrEmpty(parameters[1]))
        numericUpDownCode.Value = Convert.ToDecimal(parameters[1]);
    }

    #endregion Constructors
  }
}