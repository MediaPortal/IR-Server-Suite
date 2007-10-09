using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IrssUtils.Forms
{

  /// <summary>
  /// Keystrokes Command form.
  /// </summary>
  public partial class KeysCommand : Form
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
        return textBoxKeystrokes.Text;
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="KeysCommand"/> class.
    /// </summary>
    public KeysCommand()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="KeysCommand"/> class.
    /// </summary>
    /// <param name="command">The command.</param>
    public KeysCommand(string command)
      : this()
    {
      if (!String.IsNullOrEmpty(command))
        textBoxKeystrokes.Text = command;
    }

    #endregion

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
