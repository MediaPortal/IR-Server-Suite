using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IrssUtils.Forms
{

  /// <summary>
  /// Beep Command form.
  /// </summary>
  public partial class BeepCommand : Form
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
        return String.Format("{0}|{1}",
          numericUpDownFreq.Value.ToString(),
          numericUpDownDuration.Value.ToString());
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="BeepCommand"/> class.
    /// </summary>
    public BeepCommand()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BeepCommand"/> class.
    /// </summary>
    /// <param name="commands">The command elements.</param>
    public BeepCommand(string[] commands)
    {
      InitializeComponent();

      numericUpDownFreq.Value = Decimal.Parse(commands[0]);
      numericUpDownDuration.Value = Decimal.Parse(commands[1]);
    }
    
    #endregion Constructors

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
