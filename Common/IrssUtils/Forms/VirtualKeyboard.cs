using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace IrssUtils.Forms
{

  /// <summary>
  /// Virtual Keyboard.
  /// </summary>
  public partial class VirtualKeyboard : Form
  {

    #region Variables

    bool _capsLock;
    bool _shift;

    string _text = String.Empty;
    int _pos = 0;

    string _cursor = "|";

    Timer _timer;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Gets or sets the text output.
    /// </summary>
    /// <value>The text output.</value>
    public string TextOutput
    {
      get { return _text; }
      set
      {
        _text = value;
        _pos = _text.Length;
      }
    }

    #endregion Properties

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="VirtualKeyboard"/> class.
    /// </summary>
    public VirtualKeyboard()
    {
      InitializeComponent();

      LayoutNormal();

      TextUpdate();

      _timer = new Timer();
      _timer.Interval = 1000;
      _timer.Tick += new EventHandler(Flash);
      _timer.Enabled = true;
      _timer.Start();
    }

    #endregion Constructor

    void Flash(object sender, EventArgs e)
    {
      if (_cursor == "|")
        _cursor = " ";
      else
        _cursor = "|";

      TextUpdate();
    }

    void LayoutNormal()
    {
      buttonSymbols.Text = "Symbols";

      if (_shift)     ToggleShift();
      if (_capsLock)  ToggleCapsLock();

      buttonCapsLock.Enabled  = true;
      buttonShift.Enabled     = true;

      button1.Text = "1";
      button2.Text = "2";
      button3.Text = "3";
      button4.Text = "4";
      button5.Text = "5";
      button6.Text = "6";
      button7.Text = "7";
      button8.Text = "8";
      button9.Text = "9";
      button0.Text = "0";

      buttonA.Text = "a";
      buttonB.Text = "b";
      buttonC.Text = "c";
      buttonD.Text = "d";
      buttonE.Text = "e";
      buttonF.Text = "f";
      buttonG.Text = "g";
      buttonH.Text = "h";
      buttonI.Text = "i";
      buttonJ.Text = "j";
      buttonK.Text = "k";
      buttonL.Text = "l";
      buttonM.Text = "m";
      buttonN.Text = "n";
      buttonO.Text = "o";
      buttonP.Text = "p";
      buttonQ.Text = "q";
      buttonR.Text = "r";
      buttonS.Text = "s";
      buttonT.Text = "t";
      buttonU.Text = "u";
      buttonV.Text = "v";
      buttonW.Text = "w";
      buttonX.Text = "x";
      buttonY.Text = "y";
      buttonZ.Text = "z";
    }
    void LayoutSymbols()
    {
      buttonSymbols.Text = "Normal";

      if (_shift)     ToggleShift();
      if (_capsLock)  ToggleCapsLock();

      buttonCapsLock.Enabled  = false;
      buttonShift.Enabled     = false;

      button1.Text = "!";
      button2.Text = "@";
      button3.Text = "#";
      button4.Text = "$";
      button5.Text = "%";
      button6.Text = "^";
      button7.Text = "&";
      button8.Text = "*";
      button9.Text = "(";
      button0.Text = ")";

      buttonA.Text = "`";
      buttonB.Text = "~";
      buttonC.Text = "-";
      buttonD.Text = "_";
      buttonE.Text = "=";
      buttonF.Text = "+";
      buttonG.Text = "[";
      buttonH.Text = "{";
      buttonI.Text = "]";
      buttonJ.Text = "}";
      buttonK.Text = "|";
      buttonL.Text = "\\";
      buttonM.Text = ";";
      buttonN.Text = ":";
      buttonO.Text = "'";
      buttonP.Text = "\"";
      buttonQ.Text = ",";
      buttonR.Text = "<";
      buttonS.Text = ".";
      buttonT.Text = ">";
      buttonU.Text = "/";
      buttonV.Text = "?";
      buttonW.Text = "/";
      buttonX.Text = " ";
      buttonY.Text = " ";
      buttonZ.Text = " ";
    }

    void TextAdd(string str)
    {
      if (_pos == _text.Length)
        _text += str;
      else
        _text = _text.Insert(_pos, str);
      
      _pos += str.Length;

      TextUpdate();
    }
    void TextRemove()
    {
      TextRemove(1);
    }
    void TextRemove(int count)
    {
      int remPos = _pos - count;
      if (remPos == -1 || (remPos == 0 && _text.Length == 0))
        return;

      _text = _text.Remove(remPos, count);
      _pos = remPos;

      TextUpdate();
    }
    void TextUpdate()
    {
      string mod = _text.Clone() as string;

      if (_pos == mod.Length)
        mod += _cursor;
      else
        mod = mod.Insert(_pos, _cursor);

      textBoxKeys.Text = mod;
    }

    void NumberButton_Click(object sender, EventArgs e)
    {
      Button origin = sender as Button;

      TextAdd(origin.Text);
    }
    void SpecialButton_Click(object sender, EventArgs e)
    {
      Button origin = sender as Button;

      switch (origin.Text.ToUpperInvariant())
      {
        case "BACKSPACE":
          TextRemove();
          break;

        case "SHIFT":
          ToggleShift();
          break;

        case "CAPS LOCK":
          ToggleCapsLock();
          break;

        case "SYMBOLS":
          LayoutSymbols();
          break;

        case "NORMAL":
          LayoutNormal();
          break;

        case "SPACE":
          TextAdd(" ");
          break;

        case "HOME":
          _pos = 0;
          TextUpdate();
          break;

        case "END":
          _pos = _text.Length;
          TextUpdate();
          break;

        case "<-":
          {
            int newPos = _pos - 1;
            if (newPos == -1)
              return;
            
            _pos = newPos;
            TextUpdate();
            break;
          }

        case "->":
          {
            int newPos = _pos + 1;
            if (newPos > _text.Length)
              return;

            _pos = newPos;
            TextUpdate();
            break;
          }
      }
    }
    void LetterButton_Click(object sender, EventArgs e)
    {
      Button origin = sender as Button;

      if (_capsLock || _shift)
      {
        TextAdd(origin.Text.ToUpper(CultureInfo.CurrentCulture));

        if (_shift)
          ToggleShift();
      }
      else
      {
        TextAdd(origin.Text.ToLower(CultureInfo.CurrentCulture));
      }
    }

    void ButtonsCaseUpper()
    {
      buttonA.Text = buttonA.Text.ToUpper(CultureInfo.CurrentCulture);
      buttonB.Text = buttonB.Text.ToUpper(CultureInfo.CurrentCulture);
      buttonC.Text = buttonC.Text.ToUpper(CultureInfo.CurrentCulture);
      buttonD.Text = buttonD.Text.ToUpper(CultureInfo.CurrentCulture);
      buttonE.Text = buttonE.Text.ToUpper(CultureInfo.CurrentCulture);
      buttonF.Text = buttonF.Text.ToUpper(CultureInfo.CurrentCulture);
      buttonG.Text = buttonG.Text.ToUpper(CultureInfo.CurrentCulture);
      buttonH.Text = buttonH.Text.ToUpper(CultureInfo.CurrentCulture);
      buttonI.Text = buttonI.Text.ToUpper(CultureInfo.CurrentCulture);
      buttonJ.Text = buttonJ.Text.ToUpper(CultureInfo.CurrentCulture);
      buttonK.Text = buttonK.Text.ToUpper(CultureInfo.CurrentCulture);
      buttonL.Text = buttonL.Text.ToUpper(CultureInfo.CurrentCulture);
      buttonM.Text = buttonM.Text.ToUpper(CultureInfo.CurrentCulture);
      buttonN.Text = buttonN.Text.ToUpper(CultureInfo.CurrentCulture);
      buttonO.Text = buttonO.Text.ToUpper(CultureInfo.CurrentCulture);
      buttonP.Text = buttonP.Text.ToUpper(CultureInfo.CurrentCulture);
      buttonQ.Text = buttonQ.Text.ToUpper(CultureInfo.CurrentCulture);
      buttonR.Text = buttonR.Text.ToUpper(CultureInfo.CurrentCulture);
      buttonS.Text = buttonS.Text.ToUpper(CultureInfo.CurrentCulture);
      buttonT.Text = buttonT.Text.ToUpper(CultureInfo.CurrentCulture);
      buttonU.Text = buttonU.Text.ToUpper(CultureInfo.CurrentCulture);
      buttonV.Text = buttonV.Text.ToUpper(CultureInfo.CurrentCulture);
      buttonW.Text = buttonW.Text.ToUpper(CultureInfo.CurrentCulture);
      buttonX.Text = buttonX.Text.ToUpper(CultureInfo.CurrentCulture);
      buttonY.Text = buttonY.Text.ToUpper(CultureInfo.CurrentCulture);
      buttonZ.Text = buttonZ.Text.ToUpper(CultureInfo.CurrentCulture);
    }
    void ButtonsCaseLower()
    {
      buttonA.Text = buttonA.Text.ToLower(CultureInfo.CurrentCulture);
      buttonB.Text = buttonB.Text.ToLower(CultureInfo.CurrentCulture);
      buttonC.Text = buttonC.Text.ToLower(CultureInfo.CurrentCulture);
      buttonD.Text = buttonD.Text.ToLower(CultureInfo.CurrentCulture);
      buttonE.Text = buttonE.Text.ToLower(CultureInfo.CurrentCulture);
      buttonF.Text = buttonF.Text.ToLower(CultureInfo.CurrentCulture);
      buttonG.Text = buttonG.Text.ToLower(CultureInfo.CurrentCulture);
      buttonH.Text = buttonH.Text.ToLower(CultureInfo.CurrentCulture);
      buttonI.Text = buttonI.Text.ToLower(CultureInfo.CurrentCulture);
      buttonJ.Text = buttonJ.Text.ToLower(CultureInfo.CurrentCulture);
      buttonK.Text = buttonK.Text.ToLower(CultureInfo.CurrentCulture);
      buttonL.Text = buttonL.Text.ToLower(CultureInfo.CurrentCulture);
      buttonM.Text = buttonM.Text.ToLower(CultureInfo.CurrentCulture);
      buttonN.Text = buttonN.Text.ToLower(CultureInfo.CurrentCulture);
      buttonO.Text = buttonO.Text.ToLower(CultureInfo.CurrentCulture);
      buttonP.Text = buttonP.Text.ToLower(CultureInfo.CurrentCulture);
      buttonQ.Text = buttonQ.Text.ToLower(CultureInfo.CurrentCulture);
      buttonR.Text = buttonR.Text.ToLower(CultureInfo.CurrentCulture);
      buttonS.Text = buttonS.Text.ToLower(CultureInfo.CurrentCulture);
      buttonT.Text = buttonT.Text.ToLower(CultureInfo.CurrentCulture);
      buttonU.Text = buttonU.Text.ToLower(CultureInfo.CurrentCulture);
      buttonV.Text = buttonV.Text.ToLower(CultureInfo.CurrentCulture);
      buttonW.Text = buttonW.Text.ToLower(CultureInfo.CurrentCulture);
      buttonX.Text = buttonX.Text.ToLower(CultureInfo.CurrentCulture);
      buttonY.Text = buttonY.Text.ToLower(CultureInfo.CurrentCulture);
      buttonZ.Text = buttonZ.Text.ToLower(CultureInfo.CurrentCulture);
    }

    void ToggleShift()
    {
      _shift = !_shift;
      if (_capsLock)
      {
        _capsLock = false;
        buttonCapsLock.BackColor = Color.Transparent;
      }

      if (_shift)
      {
        buttonShift.BackColor = Color.Green;
        ButtonsCaseUpper();
      }
      else
      {
        buttonShift.BackColor = Color.Transparent;
        ButtonsCaseLower();
      }      
    }
    void ToggleCapsLock()
    {
      _capsLock = !_capsLock;
      if (_shift)
      {
        _shift = false;
        buttonShift.BackColor = Color.Transparent;
      }

      if (_capsLock)
      {
        buttonCapsLock.BackColor = Color.Green;
        ButtonsCaseUpper();
      }
      else
      {
        buttonCapsLock.BackColor = Color.Transparent;
        ButtonsCaseLower();
      }
    }

    void Button_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
    {
      e.IsInputKey = true;
    }
    void Button_KeyDown(object sender, KeyEventArgs e)
    {
      Button origin = sender as Button;

      int col     = tableLayoutPanelKeys.GetColumn(origin);
      int row     = tableLayoutPanelKeys.GetRow(origin);
      int colSpan = tableLayoutPanelKeys.GetColumnSpan(origin);
      int rowSpan = tableLayoutPanelKeys.GetRowSpan(origin);

      bool proc = true;
      switch (e.KeyCode)
      {
        case Keys.Down:   row += rowSpan;   break;
        case Keys.Up:     row--;            break;
        case Keys.Right:  col += colSpan;   break;
        case Keys.Left:   col--;            break;

        case Keys.Escape:
          {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            break;
          }

        case Keys.Enter:
          {
            this.DialogResult = DialogResult.OK;
            this.Close();
            break;
          }

        default:          proc = false;     break;
      }

      if (proc)
      {
        if (col == tableLayoutPanelKeys.ColumnCount)  col = 0;
        if (col == -1)                                col = tableLayoutPanelKeys.ColumnCount - 1;
        if (row == tableLayoutPanelKeys.RowCount)     row = 0;
        if (row == -1)                                row = tableLayoutPanelKeys.RowCount - 1;

        Control select = tableLayoutPanelKeys.GetControlFromPosition(col, row);
        if (select.Enabled)
          select.Select();
      }
    }

    private void buttonDone_Click(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(_text))
        this.DialogResult = DialogResult.Cancel;

      this.Close();
    }
    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void VirtualKeyboard_FormClosed(object sender, FormClosedEventArgs e)
    {
      _timer.Stop();
    }

  }

}
