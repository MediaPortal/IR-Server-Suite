using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Commands
{

  /// <summary>
  /// Set Variable Command form.
  /// </summary>
  public partial class EditSetVariable : Form
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
        return new string[] {
          VariableList.VariablePrefix + textBoxVariable.Text.Trim(),
          textBoxValue.Text };
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
      string varName = parameters[0];
      if (varName.StartsWith(VariableList.VariablePrefix, StringComparison.OrdinalIgnoreCase))
        varName = varName.Substring(VariableList.VariablePrefix.Length);

      textBoxVariable.Text  = varName;
      textBoxValue.Text     = parameters[1];
    }
    
    #endregion Constructors

    private void buttonOK_Click(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(textBoxVariable.Text.Trim()))
      {
        MessageBox.Show(this, "You must include a variable name", "Missing variable name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

  }

}
