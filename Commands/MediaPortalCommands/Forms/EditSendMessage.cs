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
  /// Send MediaPortal Message command form.
  /// </summary>
  public partial class EditSendMessage : Form
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
        return new string[] {
          comboBoxMessageType.Text.Trim(),
          textBoxWindowId.Text.Trim(),
          textBoxSenderId.Text.Trim(),
          textBoxControlId.Text.Trim(),
          textBoxParam1.Text.Trim(),
          textBoxParam2.Text.Trim()
        };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EditSendMessage"/> class.
    /// </summary>
    public EditSendMessage()
    {
      InitializeComponent();

      SetupComboBox();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EditSendMessage"/> class.
    /// </summary>
    /// <param name="parameters">The command parameters.</param>
    public EditSendMessage(string[] parameters)
      : this()
    {
      comboBoxMessageType.Text = parameters[0];
      textBoxWindowId.Text = parameters[1];
      textBoxSenderId.Text = parameters[2];
      textBoxControlId.Text = parameters[3];
      textBoxParam1.Text = parameters[4];
      textBoxParam2.Text = parameters[5];
    }

    #endregion Constructors

    void SetupComboBox()
    {
      comboBoxMessageType.Items.Clear();

      string[] items = Enum.GetNames(typeof(GUIMessage.MessageType));
      Array.Sort(items);

      comboBoxMessageType.Items.AddRange(items);
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
