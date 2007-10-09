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
  /// TCP Message Command form.
  /// </summary>
  public partial class TcpMessageCommand : Form
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
        return
          textBoxIP.Text + "|" +
          numericUpDownPort.Value.ToString() + "|" +
          textBoxText.Text;
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="TcpMessageCommand"/> class.
    /// </summary>
    public TcpMessageCommand()
    {
      InitializeComponent();
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="TcpMessageCommand"/> class.
    /// </summary>
    /// <param name="commands">The command elements.</param>
    public TcpMessageCommand(string[] commands)
      : this()
    {
      if (commands == null)
        throw new ArgumentNullException("commands");

      textBoxIP.Text          = commands[0];
      numericUpDownPort.Value = Convert.ToDecimal(commands[1]);
      textBoxText.Text        = commands[2];
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
