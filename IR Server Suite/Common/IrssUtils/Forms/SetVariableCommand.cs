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
  /// Set Variable Command form.
  /// </summary>
  public partial class SetVariableCommand : Form
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
        return String.Format("{0}{1}|{2}",
                             Common.VariablePrefix,
                             textBoxVariable.Text,
                             textBoxValue.Text);
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SetVariableCommand"/> class.
    /// </summary>
    public SetVariableCommand()
    {
      InitializeComponent();
      labelVarPrefix.Text = Common.VariablePrefix;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SetVariableCommand"/> class.
    /// </summary>
    /// <param name="commands">The command elements.</param>
    public SetVariableCommand(string[] commands) : this()
    {
      string varName = commands[0];
      if (varName.StartsWith(Common.VariablePrefix, StringComparison.OrdinalIgnoreCase))
        varName = varName.Substring(Common.VariablePrefix.Length);

      textBoxVariable.Text = varName;
      textBoxValue.Text = commands[1];
    }

    #endregion Constructors

    private void buttonOK_Click(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(textBoxVariable.Text.Trim()))
      {
        MessageBox.Show(this, "You must include a variable name", "Missing variable name", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
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
  }
}