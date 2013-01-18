using System;

namespace IrssCommands
{
  public partial class SetVariableConfig : BaseCommandConfig
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
            textBoxVariable.Text.Trim(),
            textBoxValue.Text
          };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SetVariableConfig"/> class.
    /// </summary>
    private SetVariableConfig()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SetVariableConfig"/> class.
    /// </summary>
    /// <param name="parameters">The command elements.</param>
    public SetVariableConfig(string[] parameters)
      : this()
    {
      textBoxVariable.Text = parameters[0];
      textBoxValue.Text = parameters[1];
    }

    #endregion Constructors

    //verify
    
      //if (String.IsNullOrEmpty(textBoxVariable.Text.Trim()))
      //{
      //  MessageBox.Show(this, "You must include a variable name", "Missing variable name", MessageBoxButtons.OK,
      //                  MessageBoxIcon.Warning);
      //  return;
      //}
  }
}