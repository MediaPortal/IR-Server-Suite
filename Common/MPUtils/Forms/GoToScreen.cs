using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MPUtils.Forms
{

  public partial class GoToScreen : Form
  {

    #region Properties

    public string Screen
    {
      get { return comboBoxScreen.SelectedText; }
    }

    #endregion Properties

    #region Constructors

    public GoToScreen() : this(String.Empty) { }
    public GoToScreen(string selected)
    {
      InitializeComponent();

      SetupComboBox();

      if (String.IsNullOrEmpty(selected))
        comboBoxScreen.SelectedIndex = 0;
      else
        comboBoxScreen.SelectedText = selected;
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
        if (items[index] != "INVALID" && items[index] != "SECOND_HOME")
          comboBoxScreen.Items.Add(items[index]);
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
