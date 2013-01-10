using System;

namespace IrssCommands.General
{
  public partial class PauseConfig : BaseCommandConfig
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
            numericUpDownPause.Value.ToString()
          };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="PauseConfig"/> class.
    /// </summary>
    private PauseConfig()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PauseConfig"/> class.
    /// </summary>
    /// <param name="parameters">The command elements.</param>
    public PauseConfig(string[] parameters)
      : this()
    {
      if (!String.IsNullOrEmpty(parameters[0]))
        numericUpDownPause.Value = Convert.ToDecimal(parameters[0]);
    }

    #endregion Constructors
  }
}