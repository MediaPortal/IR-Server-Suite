using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MPUtils.Forms
{

  /// <summary>
  /// Go To Screen command input form.
  /// </summary>
  public partial class GoToScreen : Form
  {

    #region Properties

    /// <summary>
    /// MediaPortal screen identifier.
    /// </summary>
    public string CommandString
    {
      get { return comboBoxScreen.Text; }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Default Constructor.
    /// </summary>
    public GoToScreen() : this(String.Empty) { }

    /// <summary>
    /// Create the form with a preselected MediaPortal screen identifier.
    /// </summary>
    /// <param name="selected">MediaPortal screen identifier.</param>
    public GoToScreen(string selected)
    {
      InitializeComponent();

      SetupComboBox();

      if (String.IsNullOrEmpty(selected))
        comboBoxScreen.SelectedIndex = 0;
      else
        comboBoxScreen.Text = selected;
    }
    
    #endregion Constructors

    void SetupComboBox()
    {
      comboBoxScreen.Items.Clear();
      string[] items = Enum.GetNames(typeof(MediaPortal.GUI.Library.GUIWindow.Window));

      int index;
      for (index = 0; index < items.Length; index++)
          items[index] = items[index].Substring(7);

      Array.Sort(items);

      for (index = 0; index < items.Length; index++)
      {
        if (items[index].Equals("INVALID", StringComparison.OrdinalIgnoreCase) || items[index].Equals("SECOND_HOME", StringComparison.OrdinalIgnoreCase))
          continue;

        comboBoxScreen.Items.Add(items[index]);
      }
    }

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
