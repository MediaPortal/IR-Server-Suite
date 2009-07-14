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
using System.Net;
using System.Windows.Forms;

namespace IRServer.Plugin
{
  internal partial class Configure : Form
  {
    #region Properties

    public IPAddress ServerIP
    {
      get { return IPAddress.Parse(textBoxServerAddress.Text); }
      set { textBoxServerAddress.Text = value.ToString(); }
    }

    public int ServerPort
    {
      get { return Decimal.ToInt32(numericUpDownServerPort.Value); }
      set { numericUpDownServerPort.Value = new Decimal(value); }
    }

    public bool StartServer
    {
      get { return checkBoxStartServer.Checked; }
      set { checkBoxStartServer.Checked = value; }
    }

    public string ServerPath
    {
      get { return textBoxServerPath.Text; }
      set { textBoxServerPath.Text = value; }
    }

    public int ButtonReleaseTime
    {
      get { return Decimal.ToInt32(numericUpDownButtonReleaseTime.Value); }
      set { numericUpDownButtonReleaseTime.Value = new Decimal(value); }
    }

    public int RepeatDelay
    {
      get { return Decimal.ToInt32(numericUpDownRepeatDelay.Value); }
      set { numericUpDownRepeatDelay.Value = new Decimal(value); }
    }

    #endregion Properties

    #region Constructor

    public Configure()
    {
      InitializeComponent();
    }

    #endregion Constructor

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

    private void buttonLocate_Click(object sender, EventArgs e)
    {
      if (openFileDialog.ShowDialog(this) == DialogResult.OK)
        textBoxServerPath.Text = openFileDialog.FileName;
    }

    private void buttonCreateIRFiles_Click(object sender, EventArgs e)
    {
      CreateIRFile createIRFile = new CreateIRFile();
      createIRFile.ShowDialog(this);
    }

    #endregion Buttons
  }
}