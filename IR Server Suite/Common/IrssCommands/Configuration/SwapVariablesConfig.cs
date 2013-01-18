using System;

namespace IrssCommands
{
  public partial class SwapVariablesConfig : BaseCommandConfig
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
        return new string[]
          {
            textBoxVariable1.Text.Trim(),
            textBoxVariable2.Text
          };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SwapVariablesConfig"/> class.
    /// </summary>
    private SwapVariablesConfig()
    {
      InitializeComponent();
      labelVarPrefix1.Text = VariableList.VariablePrefix;
      labelVarPrefix2.Text = VariableList.VariablePrefix;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SwapVariablesConfig"/> class.
    /// </summary>
    /// <param name="parameters">The command elements.</param>
    public SwapVariablesConfig(string[] parameters)
      : this()
    {
      textBoxVariable1.Text = parameters[0];
      textBoxVariable2.Text = parameters[1];
    }

    #endregion Constructors

    private void SetVariableConfig_Load(object sender, EventArgs e)
    {

    }

    //verify
    
      //if (String.IsNullOrEmpty(textBoxVariable1.Text.Trim()) || String.IsNullOrEmpty(textBoxVariable2.Text.Trim()))
      //{
      //  MessageBox.Show(this, "You must include variable names", "Missing variable name", MessageBoxButtons.OK,
      //                  MessageBoxIcon.Warning);
      //  return;
      //}
  }
}