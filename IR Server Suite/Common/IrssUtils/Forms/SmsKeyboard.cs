#region Copyright (C) 2005-2009 Team MediaPortal

// Copyright (C) 2005-2009 Team MediaPortal
// http://www.team-mediaportal.com
// 
// This Program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2, or (at your option)
// any later version.
// 
// This Program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with GNU Make; see the file COPYING.  If not, write to
// the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
// http://www.gnu.org/copyleft/gpl.html

#endregion

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;

namespace IrssUtils.Forms
{
  /// <summary>
  /// SMS style virtual keyboard.
  /// </summary>
  public partial class SmsKeyboard : Form
  {
    #region Constants

    private const string NumPad1Keys = "!@#$%^&*()_+-=`~[]{}\\|,.<>/?;:'\"/";
    private const string NumPad2Keys = "ABC";
    private const string NumPad3Keys = "DEF";
    private const string NumPad4Keys = "GHI";
    private const string NumPad5Keys = "JKL";
    private const string NumPad6Keys = "MNO";
    private const string NumPad7Keys = "PQRS";
    private const string NumPad8Keys = "TUV";
    private const string NumPad9Keys = "WXYZ";

    #endregion Constants

    #region Variables

    private readonly Timer _timer;

    private Keys _lastKey = Keys.None;
    private int _repeated;
    private bool _shift;

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
      _timer.Tick += Timeout;
      _timer.Enabled = true;
    }

    #endregion Constructor

    private void Timeout(object sender, EventArgs e)
    {
      if (textBoxKeys.SelectionLength == 1)
      {
        textBoxKeys.SelectionLength = 0;
        textBoxKeys.SelectionStart++;
      }

      _timer.Stop();
      _lastKey = Keys.None;
    }

    private bool HandleKeyPress(Keys key)
    {
      Trace.WriteLine("Key: " + key);

      if (_lastKey == key)
      {
        _repeated++;
      }
      else
      {
        _repeated = 0;
        _lastKey = key;
      }

      switch (key)
      {
        case Keys.NumPad1:
        case Keys.D1:
          GetChar(NumPad1Keys);
          return true;

        case Keys.NumPad2:
        case Keys.D2:
          GetChar(NumPad2Keys);
          return true;

        case Keys.NumPad3:
        case Keys.D3:
          GetChar(NumPad3Keys);
          return true;

        case Keys.NumPad4:
        case Keys.D4:
          GetChar(NumPad4Keys);
          return true;

        case Keys.NumPad5:
        case Keys.D5:
          GetChar(NumPad5Keys);
          return true;

        case Keys.NumPad6:
        case Keys.D6:
          GetChar(NumPad6Keys);
          return true;

        case Keys.NumPad7:
        case Keys.D7:
          GetChar(NumPad7Keys);
          return true;

        case Keys.NumPad8:
        case Keys.D8:
          GetChar(NumPad8Keys);
          return true;

        case Keys.NumPad9:
        case Keys.D9:
          GetChar(NumPad9Keys);
          return true;

        case Keys.NumPad0:
        case Keys.D0:
          PutChar(" ");
          return true;

        case Keys.Multiply:
          _shift = !_shift;
          return true;

        case Keys.Enter:
          if (String.IsNullOrEmpty(textBoxKeys.Text))
            DialogResult = DialogResult.Cancel;
          else
            DialogResult = DialogResult.OK;

          Close();
          return true;

        case Keys.Escape:
          DialogResult = DialogResult.Cancel;
          Close();
          return true;

        case Keys.Left:
        case Keys.Right:
        case Keys.Up:
        case Keys.Down:
          _timer.Stop();
          break;
      }

      return false;
    }

    private void GetChar(string keys)
    {
      _timer.Stop();

      if (textBoxKeys.SelectionLength == 1 && _repeated == 0)
      {
        textBoxKeys.SelectionLength = 0;
        textBoxKeys.SelectionStart++;
      }
      else if (textBoxKeys.SelectionLength != 1)
      {
        _repeated = 0;
      }

      int chrIdx = _repeated % keys.Length;

      string chr = keys[chrIdx].ToString();

      PutChar(chr);
    }

    private void PutChar(string chr)
    {
      if (_shift)
        chr = chr.ToUpper(CultureInfo.CurrentCulture);
      else
        chr = chr.ToLower(CultureInfo.CurrentCulture);

      int curPos = textBoxKeys.SelectionStart;

      textBoxKeys.Paste(chr);
      textBoxKeys.SelectionStart = curPos;
      textBoxKeys.SelectionLength = 1;

      _timer.Start();
    }

    private void SmsKeyboard_FormClosed(object sender, FormClosedEventArgs e)
    {
      _timer.Stop();
    }

    private void textBoxKeys_KeyDown(object sender, KeyEventArgs e)
    {
      e.SuppressKeyPress = HandleKeyPress(e.KeyCode);
    }

    private void button1_Click(object sender, EventArgs e)
    {
      HandleKeyPress(Keys.NumPad1);
      textBoxKeys.Focus();
    }

    private void button2_Click(object sender, EventArgs e)
    {
      HandleKeyPress(Keys.NumPad2);
      textBoxKeys.Focus();
    }

    private void button3_Click(object sender, EventArgs e)
    {
      HandleKeyPress(Keys.NumPad3);
      textBoxKeys.Focus();
    }

    private void button4_Click(object sender, EventArgs e)
    {
      HandleKeyPress(Keys.NumPad4);
      textBoxKeys.Focus();
    }

    private void button5_Click(object sender, EventArgs e)
    {
      HandleKeyPress(Keys.NumPad5);
      textBoxKeys.Focus();
    }

    private void button6_Click(object sender, EventArgs e)
    {
      HandleKeyPress(Keys.NumPad6);
      textBoxKeys.Focus();
    }

    private void button7_Click(object sender, EventArgs e)
    {
      HandleKeyPress(Keys.NumPad7);
      textBoxKeys.Focus();
    }

    private void button8_Click(object sender, EventArgs e)
    {
      HandleKeyPress(Keys.NumPad8);
      textBoxKeys.Focus();
    }

    private void button9_Click(object sender, EventArgs e)
    {
      HandleKeyPress(Keys.NumPad9);
      textBoxKeys.Focus();
    }

    private void button0_Click(object sender, EventArgs e)
    {
      HandleKeyPress(Keys.NumPad0);
      textBoxKeys.Focus();
    }

    private void buttonStar_Click(object sender, EventArgs e)
    {
      HandleKeyPress(Keys.Multiply);
      textBoxKeys.Focus();
    }

    private void buttonHash_Click(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(textBoxKeys.Text))
        DialogResult = DialogResult.Cancel;
      else
        DialogResult = DialogResult.OK;

      Close();
    }
  }
}