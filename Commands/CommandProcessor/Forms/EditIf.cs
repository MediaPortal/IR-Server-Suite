using System;
using System.Windows.Forms;

namespace Commands
{
  /// <summary>
  /// Edit If Command form.
  /// </summary>
  internal partial class EditIf : Form
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
        return new string[]
                 {
                   textBoxParam1.Text.Trim(),
                   comboBoxComparer.SelectedItem as string,
                   textBoxParam2.Text.Trim(),
                   textBoxLabel1.Text.Trim(),
                   textBoxLabel2.Text.Trim()
                 };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EditIf"/> class.
    /// </summary>
    public EditIf()
    {
      InitializeComponent();

      comboBoxComparer.Items.Clear();
      comboBoxComparer.Items.Add(CommandIf.IfEquals);
      comboBoxComparer.Items.Add(CommandIf.IfNotEqual);
      comboBoxComparer.Items.Add(CommandIf.IfGreaterThan);
      comboBoxComparer.Items.Add(CommandIf.IfLessThan);
      comboBoxComparer.Items.Add(CommandIf.IfGreaterThanOrEqual);
      comboBoxComparer.Items.Add(CommandIf.IfLessThanOrEqual);
      comboBoxComparer.Items.Add(CommandIf.IfContains);
      comboBoxComparer.Items.Add(CommandIf.IfStartsWith);
      comboBoxComparer.Items.Add(CommandIf.IfEndsWith);

      comboBoxComparer.SelectedIndex = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EditIf"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public EditIf(string[] parameters) : this()
    {
      textBoxParam1.Text = parameters[0];
      comboBoxComparer.SelectedItem = parameters[1];
      textBoxParam2.Text = parameters[2];
      textBoxLabel1.Text = parameters[3];
      textBoxLabel2.Text = parameters[4];
    }

    #endregion Constructors

    #region Buttons

    private void buttonOK_Click(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(textBoxLabel1.Text.Trim()))
      {
        MessageBox.Show(this, "You must include at least the first label name", "Missing first label name",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
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