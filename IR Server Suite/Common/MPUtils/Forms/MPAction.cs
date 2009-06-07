using System;
using System.Windows.Forms;
using MediaPortal.GUI.Library;

namespace MPUtils.Forms
{
  /// <summary>
  /// Send MediaPortal Action command form.
  /// </summary>
  public partial class MPAction : Form
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
        return String.Format("{0}|{1}|{2}",
                             comboBoxActionType.Text,
                             textBoxFloat1.Text,
                             textBoxFloat2.Text);
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Default Constructor.
    /// </summary>
    public MPAction()
    {
      InitializeComponent();

      SetupComboBox();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MPAction"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public MPAction(string[] parameters) : this()
    {
      comboBoxActionType.SelectedItem = parameters[0];
      textBoxFloat1.Text = parameters[1];
      textBoxFloat2.Text = parameters[2];
    }

    #endregion Constructors

    private void SetupComboBox()
    {
      comboBoxActionType.Items.Clear();

      string[] items = Enum.GetNames(typeof (Action.ActionType));
      Array.Sort(items);

      comboBoxActionType.Items.AddRange(items);
    }

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
  }
}