using System;
using System.Windows.Forms;

namespace IrssUtils.Forms
{
  /// <summary>
  /// Variables file filename collection form.
  /// </summary>
  public partial class VariablesFileDialog : Form
  {
    #region Properties

    /// <summary>
    /// Gets the file name.
    /// </summary>
    /// <value>The file name.</value>
    public string FileName
    {
      get { return textBoxLabel.Text.Trim(); }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="VariablesFileDialog"/> class.
    /// </summary>
    public VariablesFileDialog()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VariablesFileDialog"/> class.
    /// </summary>
    /// <param name="name">The existing file name.</param>
    public VariablesFileDialog(string name) : this()
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
        MessageBox.Show(this, "You must include a variables file name", "Missing file name", MessageBoxButtons.OK,
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