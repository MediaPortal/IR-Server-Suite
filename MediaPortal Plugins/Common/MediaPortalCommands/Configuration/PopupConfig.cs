using System;

namespace IrssCommands.MediaPortal
{
  public partial class PopupConfig : BaseCommandConfig
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
            textBoxHeading.Text.Trim(),
            textBoxText.Text.Trim(),
            numericUpDownTimeout.Value.ToString()
          };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="PopupConfig"/> class.
    /// </summary>
    private PopupConfig()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PopupConfig"/> class.
    /// </summary>
    /// <param name="parameters">The command elements.</param>
    public PopupConfig(string[] parameters) : this()
    {
      textBoxHeading.Text = parameters[0];
      textBoxText.Text = parameters[1];

      if (!String.IsNullOrEmpty(parameters[2]))
        numericUpDownTimeout.Value = Convert.ToDecimal(parameters[2]);
    }

    #endregion Constructors
  }
}