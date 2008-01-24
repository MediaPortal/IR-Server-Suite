using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Commands.General
{

  /// <summary>
  /// Popup Message Command form.
  /// </summary>
  public partial class EditPopup : Form
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
          textBoxHeading.Text.Trim(),
          textBoxText.Text.Trim(),
          numericUpDownTimeout.Value.ToString()
        };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EditPopup"/> class.
    /// </summary>
    public EditPopup()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EditPopup"/> class.
    /// </summary>
    /// <param name="commands">The command elements.</param>
    public EditPopup(string[] commands)
      : this()
    {
      textBoxHeading.Text = commands[0];
      textBoxText.Text = commands[1];
      numericUpDownTimeout.Value = Convert.ToDecimal(commands[2]);
    }

    #endregion Constructors

    #region Buttons

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

    #endregion Buttons

  }

}
