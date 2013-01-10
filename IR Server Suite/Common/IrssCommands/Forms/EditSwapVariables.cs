using System;
using System.Windows.Forms;

namespace IrssCommands
{
  /// <summary>
  /// Swap Variables Command form.
  /// </summary>
  internal partial class EditSwapVariables : Form
  {
    #region Properties

    /// <summary>
    /// Gets the command parameters.
    /// </summary>
    /// <value>The command parameters.</value>
    public string[] Parameters
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
    /// Initializes a new instance of the <see cref="EditSwapVariables"/> class.
    /// </summary>
    public EditSwapVariables()
    {
      InitializeComponent();
      labelVarPrefix1.Text = VariableList.VariablePrefix;
      labelVarPrefix2.Text = VariableList.VariablePrefix;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EditSwapVariables"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public EditSwapVariables(string[] parameters)
      : this()
    {
      textBoxVariable1.Text = parameters[0];
      textBoxVariable2.Text = parameters[1];
    }

    #endregion Constructors

    private void buttonOK_Click(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(textBoxVariable1.Text.Trim()) || String.IsNullOrEmpty(textBoxVariable2.Text.Trim()))
      {
        MessageBox.Show(this, "You must include variable names", "Missing variable name", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
        return;
      }

      DialogResult = DialogResult.OK;
      Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }
  }
}