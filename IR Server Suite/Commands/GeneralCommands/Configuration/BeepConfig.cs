using System;

namespace IrssCommands.General
{
  public partial class BeepConfig : BaseCommandConfig
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
            numericUpDownFreq.Value.ToString(),
            numericUpDownDuration.Value.ToString()
          };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="BeepConfig"/> class.
    /// </summary>
    private BeepConfig()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BeepConfig"/> class.
    /// </summary>
    /// <param name="parameters">The command elements.</param>
    public BeepConfig(string[] parameters) : this()
    {
      if (!String.IsNullOrEmpty(parameters[0]))
        numericUpDownFreq.Value = Convert.ToDecimal(parameters[0]);
      if (!String.IsNullOrEmpty(parameters[1]))
        numericUpDownDuration.Value = Convert.ToDecimal(parameters[1]);
    }

    #endregion Constructors
  }
}