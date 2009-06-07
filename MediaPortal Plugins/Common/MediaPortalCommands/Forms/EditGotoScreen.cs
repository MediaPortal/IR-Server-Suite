using System;
using System.Windows.Forms;
using MediaPortal.GUI.Library;

namespace Commands.MediaPortal
{
  /// <summary>
  /// Go To Screen command input form.
  /// </summary>
  public partial class EditGotoScreen : Form
  {
    #region Properties

    /// <summary>
    /// MediaPortal screen identifier.
    /// </summary>
    public string ScreenID
    {
      get { return comboBoxScreen.Text; }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Default Constructor.
    /// </summary>
    public EditGotoScreen() : this(String.Empty)
    {
    }

    /// <summary>
    /// Create the form with a preselected MediaPortal screen identifier.
    /// </summary>
    /// <param name="selected">MediaPortal screen identifier.</param>
    public EditGotoScreen(string selected)
    {
      InitializeComponent();

      SetupComboBox();

      if (String.IsNullOrEmpty(selected))
        comboBoxScreen.SelectedIndex = 0;
      else
        comboBoxScreen.Text = selected;
    }

    #endregion Constructors

    private void SetupComboBox()
    {
      comboBoxScreen.Items.Clear();
      string[] items = Enum.GetNames(typeof (GUIWindow.Window));

      int index;
      for (index = 0; index < items.Length; index++)
        items[index] = items[index].Substring(7);

      Array.Sort(items);

      for (index = 0; index < items.Length; index++)
      {
        if (items[index].Equals("INVALID", StringComparison.OrdinalIgnoreCase))
          continue;

        comboBoxScreen.Items.Add(items[index]);
      }
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