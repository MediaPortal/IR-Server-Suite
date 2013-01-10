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

namespace IrssCommands
{
  /// <summary>
  /// A common windows form to configure the parameters for commands.
  /// </summary>
  public sealed partial class CommandConfigForm : Form
  {
    private readonly BaseCommandConfig _commandConfig;
    private readonly Command _command;

    #region Properties

    /// <summary>
    /// Gets the command parameters.
    /// </summary>
    /// <value>The command parameters.</value>
    public string[] Parameters
    {
      get { return _commandConfig.Parameters; }
    }

    #endregion Properties

    /// <summary>
    /// Initializes a new config form for the provided command.
    /// </summary>
    /// <param name="command">The command to configure.</param>
    public CommandConfigForm(Command command)
    {
      InitializeComponent();

      _command = command;
      _commandConfig = _command.GetEditControl();

      this.Text = _command.UserInterfaceText;
      this.buttonTest.Enabled = _command.IsTestAllowed;

      panel.Controls.Add(_commandConfig);
    }

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

    private void buttonTest_Click(object sender, EventArgs e)
    {
      _command.Parameters = Parameters;
      _command.Execute(new VariableList());
    }

    #endregion Controls
  }
}