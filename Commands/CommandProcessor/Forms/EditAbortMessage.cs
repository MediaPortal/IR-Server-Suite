using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Commands
{

  /// <summary>
  /// Edit Abort Message form.
  /// </summary>
  partial class EditAbortMessage : Form
  {

    #region Properties

    /// <summary>
    /// Gets the abort message.
    /// </summary>
    /// <value>The abort message.</value>
    public string AbortMessage
    {
      get
      {
        return textBoxMessage.Text.Trim();
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EditAbortMessage"/> class.
    /// </summary>
    public EditAbortMessage()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EditAbortMessage"/> class.
    /// </summary>
    /// <param name="name">The existing label name.</param>
    public EditAbortMessage(string name)
      : this()
    {
      if (!String.IsNullOrEmpty(name))
        textBoxMessage.Text = name;
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
