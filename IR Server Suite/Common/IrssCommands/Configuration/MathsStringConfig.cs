using System;

namespace IrssCommands
{
  public partial class MathsStringConfig : BaseCommandConfig
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
        if (labelInput2.Visible)
          return new string[] { textBoxInput1.Text.Trim(), textBoxInput2.Text.Trim(), textBoxOutputVar.Text.Trim() };
        else
          return new string[] { textBoxInput1.Text.Trim(), textBoxOutputVar.Text.Trim() };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="MathsStringConfig"/> class.
    /// </summary>
    private MathsStringConfig()
    {
      InitializeComponent();

      labelVarPrefix.Text = VariableList.VariablePrefix;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MathsStringConfig"/> class.
    /// </summary>
    /// <param name="parameters">The command elements.</param>
    public MathsStringConfig(string[] parameters)
      : this()
    {
      if (ReferenceEquals(parameters,null)) return;
      if (ReferenceEquals(parameters, new string[] { })) return;

      if (parameters.Length == 3)
      {
        textBoxInput1.Text = parameters[0];
        textBoxInput2.Text = parameters[1];
        textBoxOutputVar.Text = parameters[2];
      }
      else if (parameters.Length == 2)
      {
        textBoxInput1.Text = parameters[0];
        labelInput2.Visible = false;
        textBoxInput2.Visible = false;
        textBoxOutputVar.Text = parameters[1];
      }
      else
      {
        throw new ArgumentException("Parameter array size must be 2 or 3", "parameters");
      }
    }

    #endregion Constructors

    //verify
      //if (String.IsNullOrEmpty(textBoxInput1.Text.Trim()))
      //{
      //  MessageBox.Show(this, "You must include at least the first input", "Missing first input", MessageBoxButtons.OK,
      //                  MessageBoxIcon.Warning);
      //  return;
      //}

      //if (String.IsNullOrEmpty(textBoxOutputVar.Text.Trim()))
      //{
      //  MessageBox.Show(this, "You must include an output variable name", "Missing output variable name",
      //                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
      //  return;
      //}
  }
}