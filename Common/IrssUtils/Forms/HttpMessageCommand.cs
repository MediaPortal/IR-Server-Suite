using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IrssUtils.Forms
{

  /// <summary>
  /// HTTP Message Command form.
  /// </summary>
  public partial class HttpMessageCommand : Form
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
        return String.Format("{0}|{1}|{2}|{3}",
          textBoxAddress.Text,
          numericUpDownTimeout.Value.ToString(),
          checkBoxUsernameAndPassword.Checked ? textBoxUsername.Text : String.Empty,
          checkBoxUsernameAndPassword.Checked ? textBoxPassword.Text : String.Empty);
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="TcpMessageCommand"/> class.
    /// </summary>
    public HttpMessageCommand()
    {
      InitializeComponent();
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="TcpMessageCommand"/> class.
    /// </summary>
    /// <param name="commands">The command elements.</param>
    public HttpMessageCommand(string[] commands)
      : this()
    {
      if (commands == null)
        throw new ArgumentNullException("commands");

      textBoxAddress.Text         = commands[0];
      numericUpDownTimeout.Value  = Convert.ToDecimal(commands[1]);
      textBoxUsername.Text        = commands[2];
      textBoxPassword.Text        = commands[3];
    }

    #endregion Constructors

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
