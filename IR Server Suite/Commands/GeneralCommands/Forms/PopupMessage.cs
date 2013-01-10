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
using System.Windows.Forms;

namespace IrssCommands.General
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