using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Commands
{

  /// <summary>
  /// Edit String Operation Command form.
  /// </summary>
  partial class EditStringOperation : Form
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
        if (labelInput2.Visible)
          return new string[] { textBoxInput1.Text.Trim(), textBoxInput2.Text.Trim(), textBoxOutputVar.Text.Trim() };
        else
          return new string[] { textBoxInput1.Text.Trim(), textBoxOutputVar.Text.Trim() };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EditStringOperation"/> class.
    /// </summary>
    public EditStringOperation()
    {
      InitializeComponent();

      labelVarPrefix.Text = VariableList.VariablePrefix;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EditMathsOperation"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public EditStringOperation(string[] parameters)
      : this()
    {
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

    #region Buttons

    private void buttonOK_Click(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(textBoxInput1.Text.Trim()))
      {
        MessageBox.Show(this, "You must include at least the first input", "Missing first input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      if (String.IsNullOrEmpty(textBoxOutputVar.Text.Trim()))
      {
        MessageBox.Show(this, "You must include an output variable name", "Missing output variable name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    #endregion Buttons

  }

}
