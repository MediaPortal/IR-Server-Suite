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

namespace IrssUtils.Forms
{
  /// <summary>
  /// If Command form.
  /// </summary>
  public partial class IfCommand : Form
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
        return String.Format("{0}|{1}|{2}|{3}|{4}",
                             textBoxParam1.Text.Trim(),
                             comboBoxComparer.SelectedItem as string,
                             textBoxParam2.Text.Trim(),
                             textBoxLabel1.Text.Trim(),
                             textBoxLabel2.Text.Trim());
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="IfCommand"/> class.
    /// </summary>
    public IfCommand()
    {
      InitializeComponent();

      comboBoxComparer.Items.Clear();
      comboBoxComparer.Items.Add(IrssMacro.IfEquals);
      comboBoxComparer.Items.Add(IrssMacro.IfNotEqual);
      comboBoxComparer.Items.Add(IrssMacro.IfGreaterThan);
      comboBoxComparer.Items.Add(IrssMacro.IfLessThan);
      comboBoxComparer.Items.Add(IrssMacro.IfGreaterThanOrEqual);
      comboBoxComparer.Items.Add(IrssMacro.IfLessThanOrEqual);
      comboBoxComparer.Items.Add(IrssMacro.IfContains);
      comboBoxComparer.Items.Add(IrssMacro.IfStartsWith);
      comboBoxComparer.Items.Add(IrssMacro.IfEndsWith);

      comboBoxComparer.SelectedIndex = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IfCommand"/> class.
    /// </summary>
    /// <param name="commands">The command elements.</param>
    public IfCommand(string[] commands) : this()
    {
      textBoxParam1.Text = commands[0];
      comboBoxComparer.SelectedItem = commands[1];
      textBoxParam2.Text = commands[2];
      textBoxLabel1.Text = commands[3];
      textBoxLabel2.Text = commands[4];
    }

    #endregion Constructors

    #region Buttons

    private void buttonOK_Click(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(textBoxLabel1.Text.Trim()))
      {
        MessageBox.Show(this, "You must include at least the first label name", "Missing first label name",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

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