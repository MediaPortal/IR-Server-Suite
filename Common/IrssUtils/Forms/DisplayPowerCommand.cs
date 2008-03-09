using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace IrssUtils.Forms
{

  /// <summary>
  /// Display Power Command form.
  /// </summary>
  public partial class DisplayPowerCommand : Form
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
        return comboBoxState.SelectedItem as string;
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="DisplayPowerCommand"/> class.
    /// </summary>
    public DisplayPowerCommand()
    {
      InitializeComponent();

      comboBoxState.Items.Add("On");
      comboBoxState.Items.Add("Off");
      comboBoxState.Items.Add("Standby");
      
      comboBoxState.SelectedIndex = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DisplayPowerCommand"/> class.
    /// </summary>
    /// <param name="command">The command.</param>
    public DisplayPowerCommand(string command) : this()
    {
      comboBoxState.SelectedItem = command;
    }

    #endregion

    #region Buttons

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

    #endregion Buttons

  }

}
