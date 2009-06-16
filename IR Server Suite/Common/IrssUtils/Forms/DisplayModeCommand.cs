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
using System.Drawing;
using System.Windows.Forms;

namespace IrssUtils.Forms
{
  /// <summary>
  /// Display Mode Command form.
  /// </summary>
  public partial class DisplayModeCommand : Form
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
        return String.Format("{0}|{1}|{2}|{3}",
                             textBoxWidth.Text,
                             textBoxHeight.Text,
                             checkBoxBpp.Checked ? textBoxBpp.Text : "-1",
                             checkBoxRefresh.Checked ? textBoxRefresh.Text : "-1");
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="DisplayModeCommand"/> class.
    /// </summary>
    public DisplayModeCommand()
    {
      InitializeComponent();

      Size res = Display.GetResolution();

      textBoxWidth.Text = res.Width.ToString();
      textBoxHeight.Text = res.Height.ToString();

      textBoxBpp.Text = Display.GetBpp().ToString();
      textBoxRefresh.Text = Display.GetRefreshRate().ToString();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DisplayModeCommand"/> class.
    /// </summary>
    /// <param name="commands">The command elements.</param>
    public DisplayModeCommand(string[] commands)
    {
      InitializeComponent();

      textBoxWidth.Text = commands[0];
      textBoxHeight.Text = commands[1];
      textBoxBpp.Text = commands[2];
      textBoxRefresh.Text = commands[3];
    }

    #endregion

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