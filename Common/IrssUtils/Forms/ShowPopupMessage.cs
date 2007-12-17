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
  /// Show a popup message.
  /// </summary>
  public partial class ShowPopupMessage : Form
  {

    #region Variables

    int _timeout;

    #endregion Variables
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ShowPopupMessage"/> class.
    /// </summary>
    public ShowPopupMessage(string header, string text, int timeout)
    {
      InitializeComponent();

      this.Text         = header;
      labelMessage.Text = text;
      _timeout          = timeout;
    }


    private void timerOK_Tick(object sender, EventArgs e)
    {
      _timeout--;

      if (_timeout <= 0)
        this.Close();

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
      this.Close();
    }

  }

}
