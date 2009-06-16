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

namespace InputService.Plugin
{
  /// <summary>
  /// Setup form for Wii Remote Receiver IR Server Plugin.
  /// </summary>
  public partial class Setup : Form
  {
    #region Properties

    /// <summary>
    /// Gets or sets a value indicating whether to handle mouse commands locally or to pass them on to the IR Server.
    /// </summary>
    /// <value><c>true</c> to handle the mouse commands locally; otherwise, <c>false</c>.</value>
    public bool HandleMouseLocal
    {
      get { return checkBoxHandleMouseLocal.Checked; }
      set { checkBoxHandleMouseLocal.Checked = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to use the nunchuk as a mouse control.
    /// </summary>
    /// <value><c>true</c> if the nunchuk is used as a mouse; otherwise, <c>false</c>.</value>
    public bool UseNunchukAsMouse
    {
      get { return checkBoxNunchukMouse.Checked; }
      set { checkBoxNunchukMouse.Checked = value; }
    }

    /// <summary>
    /// Gets or sets the mouse sensitivity.
    /// </summary>
    /// <value>The mouse sensitivity.</value>
    public double MouseSensitivity
    {
      get { return Decimal.ToInt32(numericUpDownMouseSensitivity.Value); }
      set { numericUpDownMouseSensitivity.Value = new Decimal(value); }
    }

    /// <summary>
    /// Gets or sets a value indicating LED state.
    /// </summary>
    /// <value><c>true</c> if LED1 is on; otherwise, <c>false</c>.</value>
    public bool LED1
    {
      get { return checkBoxLED1.Checked; }
      set { checkBoxLED1.Checked = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating LED state.
    /// </summary>
    /// <value><c>true</c> if LED2 is on; otherwise, <c>false</c>.</value>
    public bool LED2
    {
      get { return checkBoxLED2.Checked; }
      set { checkBoxLED2.Checked = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating LED state.
    /// </summary>
    /// <value><c>true</c> if LED1 is on; otherwise, <c>false</c>.</value>
    public bool LED3
    {
      get { return checkBoxLED3.Checked; }
      set { checkBoxLED3.Checked = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating LED state.
    /// </summary>
    /// <value><c>true</c> if LED4 is on; otherwise, <c>false</c>.</value>
    public bool LED4
    {
      get { return checkBoxLED4.Checked; }
      set { checkBoxLED4.Checked = value; }
    }

    #endregion Properties

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Setup"/> class.
    /// </summary>
    public Setup()
    {
      InitializeComponent();
    }

    #endregion Constructor

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
  }
}