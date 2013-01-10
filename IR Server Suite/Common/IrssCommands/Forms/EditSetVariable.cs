using System;
using System.Windows.Forms;

namespace IrssCommands
{
  /// <summary>
  /// Set Variable Command form.
  /// </summary>
  internal partial class EditSetVariable : Form
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
                   textBoxVariable.Text.Trim(),
                   textBoxValue.Text
                 };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EditSetVariable"/> class.
    /// </summary>
    public EditSetVariable()
    {
      InitializeComponent();
      labelVarPrefix.Text = VariableList.VariablePrefix;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EditSetVariable"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public EditSetVariable(string[] parameters) : this()
    {
      textBoxVariable.Text = parameters[0];
      textBoxValue.Text = parameters[1];
    }

    #endregion Constructors

    private void buttonOK_Click(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(textBoxVariable.Text.Trim()))
      {
        MessageBox.Show(this, "You must include a variable name", "Missing variable name", MessageBoxButtons.OK,
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