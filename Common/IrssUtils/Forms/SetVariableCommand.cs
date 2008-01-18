using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IrssUtils.Forms
{

  /// <summary>
  /// Set Variable Command form.
  /// </summary>
  public partial class SetVariableCommand : Form
  {

    #region Properties

    /// <summary>
    /// Gets the command string.
    /// </summary>
    /// <value>The command string.</value>
    public string CommandString
    {
      get
      {
        return String.Format("{0}{1}|{2}",
          Common.VariablePrefix,
          textBoxVariable.Text,
          textBoxValue.Text);
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SetVariableCommand"/> class.
    /// </summary>
    public SetVariableCommand()
    {
      InitializeComponent();
      labelVarPrefix.Text = Common.VariablePrefix;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SetVariableCommand"/> class.
    /// </summary>
    /// <param name="commands">The command elements.</param>
    public SetVariableCommand(string[] commands) : this()
    {
      string varName = commands[0];
      if (varName.StartsWith(Common.VariablePrefix, StringComparison.OrdinalIgnoreCase))
        varName = varName.Substring(Common.VariablePrefix.Length);

      textBoxVariable.Text  = varName;
      textBoxValue.Text     = commands[1];
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
