using System;
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
        return String.Format("{0}|{1}|{2}",
                             textBoxIP.Text,
                             numericUpDownPort.Value,
                             textBoxText.Text);
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

      textBoxIP.Text = commands[0];
      numericUpDownPort.Value = Convert.ToDecimal(commands[1]);
      textBoxText.Text = commands[2];
    }

    #endregion Constructors

    #region Buttons

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

    #endregion Buttons
  }
}