using System;
using System.Windows.Forms;

namespace Commands.General
{
  /// <summary>
  /// Show a popup message.
  /// </summary>
  public partial class PopupMessage : Form
  {
    #region Variables

    private int _timeout;

    #endregion Variables

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="PopupMessage"/> class.
    /// </summary>
    public PopupMessage(string header, string text, int timeout)
    {
      InitializeComponent();

      Text = header;
      textBoxMessage.Text = text;
      _timeout = timeout;

      buttonOK.Text = String.Format("OK ({0})", _timeout);
    }

    #endregion Constructor

    private void timerOK_Tick(object sender, EventArgs e)
    {
      _timeout--;

      if (_timeout <= 0)
      {
        DialogResult = DialogResult.Cancel;
        Close();
      }

      buttonOK.Text = String.Format("OK ({0})", _timeout);
    }

    private void ShowPopupMessage_Load(object sender, EventArgs e)
    {
      timerOK.Start();
    }

    private void ShowPopupMessage_FormClosing(object sender, FormClosingEventArgs e)
    {
      timerOK.Stop();
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.OK;
      Close();
    }
  }
}