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
using System.Windows.Forms;

namespace Translator
{
  /// <summary>
  /// Advanced Configuration Form.
  /// </summary>
  internal partial class Advanced : Form
  {
    #region Properties

    /// <summary>
    /// Gets or sets the process priority.
    /// </summary>
    /// <value>The process priority.</value>
    public string ProcessPriority
    {
      get { return comboBoxPriority.SelectedItem as string; }
      set { comboBoxPriority.SelectedItem = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to hide the tray icon.
    /// </summary>
    /// <value><c>true</c> to hide tray icon; otherwise, <c>false</c>.</value>
    public bool HideTrayIcon
    {
      get { return checkBoxHideTrayIcon.Checked; }
      set { checkBoxHideTrayIcon.Checked = value; }
    }

    #endregion Properties

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Advanced"/> class.
    /// </summary>
    public Advanced()
    {
      InitializeComponent();

      comboBoxPriority.Items.Add("No Change");
      comboBoxPriority.Items.AddRange(Enum.GetNames(typeof (ProcessPriorityClass)));
      comboBoxPriority.SelectedIndex = 0;
    }

    #endregion Constructor

    #region Controls

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

    #endregion Controls
  }
}