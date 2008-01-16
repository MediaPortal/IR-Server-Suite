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
    
    #endregion Constructors

    void SetupComboBox()
    {
      comboBoxActionType.Items.Clear();

      string[] items = Enum.GetNames(typeof(MediaPortal.GUI.Library.Action.ActionType));
      Array.Sort(items);
      
      comboBoxActionType.Items.AddRange(items);
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
