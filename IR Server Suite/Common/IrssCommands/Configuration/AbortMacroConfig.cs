using System;

namespace IrssCommands
{
  public partial class AbortMacroConfig : BaseCommandConfig
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
            textBoxMessage.Text.Trim()
          };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="AbortMacroConfig"/> class.
    /// </summary>
    private AbortMacroConfig()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AbortMacroConfig"/> class.
    /// </summary>
    /// <param name="parameters">The command elements.</param>
    public AbortMacroConfig(string[] parameters)
      : this()
    {
      if (ReferenceEquals(parameters, null)) return;
      if (ReferenceEquals(parameters, new string[] { })) return;

      if (!String.IsNullOrEmpty(parameters[0]))
        textBoxMessage.Text = parameters[0];
    }

    #endregion Constructors

    //verify
      //if (String.IsNullOrEmpty(textBoxLabel.Text.Trim()))
      //{
      //  MessageBox.Show(this, "You must include a label name", "Missing label name", MessageBoxButtons.OK,
      //                  MessageBoxIcon.Warning);
      //  return;
      //}
  }
}