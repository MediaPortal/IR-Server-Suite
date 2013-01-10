using System;
using System.Windows.Forms;

namespace Commands
{
  /// <summary>
  /// Edit Label form.
  /// </summary>
  internal partial class EditLabel : Form
  {
    #region Properties

    /// <summary>
    /// Gets the label name.
    /// </summary>
    /// <value>The label name.</value>
    public string LabelName
    {
      get { return textBoxLabel.Text.Trim(); }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EditLabel"/> class.
    /// </summary>
    public EditLabel()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EditLabel"/> class.
    /// </summary>
    /// <param name="name">The existing label name.</param>
    public EditLabel(string name) : this()
    {
      if (!String.IsNullOrEmpty(name))
        textBoxLabel.Text = name;
    }

    #endregion

    #region Buttons

    private void buttonOK_Click(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(textBoxLabel.Text.Trim()))
      {
        MessageBox.Show(this, "You must include a label name", "Missing label name", MessageBoxButtons.OK,
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

    #endregion Buttons
  }
}