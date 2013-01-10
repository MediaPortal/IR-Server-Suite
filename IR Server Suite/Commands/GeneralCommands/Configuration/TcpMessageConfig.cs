using System;

namespace IrssCommands.General
{
  public partial class TcpMessageConfig : BaseCommandConfig
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
            textBoxIP.Text.Trim(),
            numericUpDownPort.Value.ToString(),
            textBoxText.Text.Trim()
          };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="TcpMessageConfig"/> class.
    /// </summary>
    private TcpMessageConfig()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TcpMessageConfig"/> class.
    /// </summary>
    /// <param name="parameters">The command elements.</param>
    public TcpMessageConfig(string[] parameters)
      : this()
    {
      textBoxIP.Text = parameters[0];
      if (!String.IsNullOrEmpty(parameters[1]))
        numericUpDownPort.Value = Convert.ToDecimal(parameters[1]);
      textBoxText.Text = parameters[2];
    }

    #endregion Constructors
  }
}