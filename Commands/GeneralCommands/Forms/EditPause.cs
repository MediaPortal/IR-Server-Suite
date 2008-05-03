using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Commands.General
{

  /// <summary>
  /// Pause Time Command form.
  /// </summary>
  public partial class EditPause : Form
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
          numericUpDownPause.Value.ToString()
        };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EditPause"/> class.
    /// </summary>
    public EditPause()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EditPause"/> class.
    /// </summary>
    /// <param name="commands">The command elements.</param>
    public EditPause(string[] commands)
      : this()
    {
      if (!String.IsNullOrEmpty(commands[0]))
        numericUpDownPause.Value = Convert.ToDecimal(commands[0]);
    }

    #endregion Constructors

    private void buttonOK_Click(object sender, EventArgs e)
    {
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
