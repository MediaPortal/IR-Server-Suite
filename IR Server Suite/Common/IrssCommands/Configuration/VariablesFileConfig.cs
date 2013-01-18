using System;

namespace IrssCommands
{
  public partial class VariablesFileConfig : BaseCommandConfig
  {
    #region Properties

    /// <summary>
    /// Gets the command parameters.
    /// </summary>
    /// <value>The command parameters.</value>
    public override string[] Parameters
    {
      get { return new string[] {textBoxLabel.Text.Trim()}; }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="VariablesFileConfig"/> class.
    /// </summary>
    private VariablesFileConfig()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VariablesFileConfig"/> class.
    /// </summary>
    /// <param name="parameters">The command elements.</param>
    public VariablesFileConfig(string[] parameters)
      : this()
    {
      if (!String.IsNullOrEmpty(parameters[0]))
        textBoxLabel.Text = parameters[0];
    }

    #endregion Constructors

    //verify
      //
      //if (String.IsNullOrEmpty(textBoxLabel.Text.Trim()))
      //{
      //  MessageBox.Show(this, "You must include a variables file name", "Missing file name", MessageBoxButtons.OK,
      //                  MessageBoxIcon.Warning);
      //  return;
      //}
  }
}