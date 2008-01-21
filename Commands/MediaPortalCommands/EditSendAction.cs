using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MediaPortal.GUI.Library;

namespace Commands.MediaPortal
{

  /// <summary>
  /// Send MediaPortal Action command form.
  /// </summary>
  public partial class EditSendAction : Form
  {

    #region Properties

    /// <summary>
    /// Gets the command string.
    /// </summary>
    /// <value>The command string.</value>
    public string[] Parameters
    {
      get
      {
        return new string[] { comboBoxActionType.Text, textBoxFloat1.Text, textBoxFloat2.Text };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EditSendAction"/> class.
    /// </summary>
    public EditSendAction()
    {
      InitializeComponent();

      SetupComboBox();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EditSendAction"/> class.
    /// </summary>
    /// <param name="parameters">The command parameters.</param>
    public EditSendAction(string[] parameters)
      : this()
    {
      comboBoxActionType.Text = parameters[0];
      textBoxFloat1.Text      = parameters[1];
      textBoxFloat2.Text      = parameters[2];
    }

    #endregion Constructors

    void SetupComboBox()
    {
      comboBoxActionType.Items.Clear();

      string[] items = Enum.GetNames(typeof(Action.ActionType));
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
