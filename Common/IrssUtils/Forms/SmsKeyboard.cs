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
  /// SMS style virtual keyboard.
  /// </summary>
  public partial class SmsKeyboard : Form
  {

    #region Variables

    bool _capsLock;
    bool _shift;

    Timer _timer;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Gets or sets the text output.
    /// </summary>
    /// <value>The text output.</value>
    public string TextOutput
    {
      get { return textBoxKeys.Text; }
      set { textBoxKeys.Text = value; }
    }

    #endregion Properties

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="VirtualKeyboard"/> class.
    /// </summary>
    public SmsKeyboard()
    {
      InitializeComponent();

      _timer = new Timer();
      _timer.Interval = 2000;
      _timer.Tick += new EventHandler(Timeout);
      _timer.Enabled = true;
    }

    #endregion Constructor

    void Timeout(object sender, EventArgs e)
    {
      //textBoxKeys.SelectionLength = 0;
      //textBoxKeys.SelectionStart++;


      _timer.Stop();
    }

    void TextAdd(string str)
    {
      string toAdd = str.Clone() as string;

      if (_shift || _capsLock)
        toAdd = toAdd.ToUpper();
      else
        toAdd = toAdd.ToLower();

      textBoxKeys.Paste(toAdd);

      if (_shift)
        _shift = false;

      _timer.Start();
    }
    void TextBackspace()
    {
    }

    void ToggleShift()
    {
      _shift = !_shift;
      if (_capsLock)
        _capsLock = false;
    }
    void ToggleCapsLock()
    {
      _capsLock = !_capsLock;
      if (_shift)
        _shift = false;
    }

    private void buttonDone_Click(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(textBoxKeys.Text))
        this.DialogResult = DialogResult.Cancel;

      this.Close();
    }
    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void SmsKeyboard_FormClosed(object sender, FormClosedEventArgs e)
    {
      _timer.Stop();
    }

  }

}
