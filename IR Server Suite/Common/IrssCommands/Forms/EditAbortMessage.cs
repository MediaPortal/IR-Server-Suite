using System;
using System.Windows.Forms;

namespace IrssCommands
{
  /// <summary>
  /// Edit Abort Message form.
  /// </summary>
  internal partial class EditAbortMessage : Form
  {
    #region Properties

    /// <summary>
    /// Gets the abort message.
    /// </summary>
    /// <value>The abort message.</value>
    public string AbortMessage
    {
      get { return textBoxMessage.Text.Trim(); }
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
      DialogResult = DialogResult.OK;
      Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    #endregion Buttons
  }
}