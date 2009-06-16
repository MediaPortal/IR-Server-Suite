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
using System.IO;
using System.Windows.Forms;

namespace IrssUtils.Forms
{
  /// <summary>
  /// External Program Command form.
  /// </summary>
  public partial class ExternalProgram : Form
  {
    #region Variables

    private readonly string _parametersMessage = String.Empty;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Gets the command string.
    /// </summary>
    /// <value>The command string.</value>
    public string CommandString
    {
      get
      {
        return String.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}",
                             textBoxProgram.Text,
                             textBoxStartup.Text,
                             textBoxParameters.Text,
                             comboBoxWindowStyle.SelectedItem as string,
                             checkBoxNoWindow.Checked,
                             checkBoxShellExecute.Checked,
                             checkBoxWaitForExit.Checked,
                             checkBoxForceFocus.Checked);
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalProgram"/> class.
    /// </summary>
    public ExternalProgram() : this(null, String.Empty, true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalProgram"/> class.
    /// </summary>
    /// <param name="canWait">Enable the "Wait for program to finish" option.</param>
    public ExternalProgram(bool canWait) : this(null, String.Empty, canWait)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalProgram"/> class.
    /// </summary>
    /// <param name="parametersMessage">The optional parameters message.</param>
    public ExternalProgram(string parametersMessage) : this(null, parametersMessage, true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalProgram"/> class.
    /// </summary>
    /// <param name="commands">The command elements.</param>
    public ExternalProgram(string[] commands) : this(commands, String.Empty, true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalProgram"/> class.
    /// </summary>
    /// <param name="commands">The command elements.</param>
    /// <param name="canWait">Enable the "Wait for program to finish" option.</param>
    public ExternalProgram(string[] commands, bool canWait) : this(commands, String.Empty, canWait)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalProgram"/> class.
    /// </summary>
    /// <param name="commands">The command elements.</param>
    /// <param name="parametersMessage">The optional parameters message.</param>
    public ExternalProgram(string[] commands, string parametersMessage) : this(commands, parametersMessage, true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalProgram"/> class.
    /// </summary>
    /// <param name="commands">The command elements.</param>
    /// <param name="parametersMessage">The optional parameters message.</param>
    /// <param name="canWait">Enable the "Wait for program to finish" option.</param>
    public ExternalProgram(string[] commands, string parametersMessage, bool canWait)
    {
      InitializeComponent();

      if (canWait)
      {
        checkBoxWaitForExit.Visible = true;
        checkBoxWaitForExit.Enabled = true;
      }
      else
      {
        checkBoxWaitForExit.Visible = false;
        checkBoxWaitForExit.Enabled = false;
        checkBoxWaitForExit.Checked = false;
      }

      _parametersMessage = parametersMessage;

      comboBoxWindowStyle.Items.Clear();
      comboBoxWindowStyle.Items.AddRange(Enum.GetNames(typeof (ProcessWindowStyle)));

      if (commands != null)
      {
        textBoxProgram.Text = commands[0];
        textBoxStartup.Text = commands[1];
        textBoxParameters.Text = commands[2];

        checkBoxNoWindow.Checked = bool.Parse(commands[4]);
        checkBoxShellExecute.Checked = bool.Parse(commands[5]);
        checkBoxWaitForExit.Checked = bool.Parse(commands[6]);
        checkBoxForceFocus.Checked = bool.Parse(commands[7]);

        comboBoxWindowStyle.SelectedItem =
          ((ProcessWindowStyle) Enum.Parse(typeof (ProcessWindowStyle), commands[3], true)).ToString();
      }
      else
      {
        comboBoxWindowStyle.SelectedIndex = 0;
      }
    }

    #endregion Constructors

    private void ExternalProgram_Load(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(_parametersMessage) || _parametersMessage.Trim().Length == 0)
        buttonParamQuestion.Visible = false;
    }

    #region Buttons

    private void buttonProgam_Click(object sender, EventArgs e)
    {
      if (openFileDialog.ShowDialog(this) == DialogResult.OK)
      {
        textBoxProgram.Text = openFileDialog.FileName;

        if (textBoxStartup.Text.Trim().Length == 0)
        {
          textBoxStartup.Text = Path.GetDirectoryName(openFileDialog.FileName);
        }
      }
    }

    private void buttonStartup_Click(object sender, EventArgs e)
    {
      if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
      {
        textBoxProgram.Text = folderBrowserDialog.SelectedPath;
      }
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      if (textBoxProgram.Text.Trim().Length == 0)
      {
        MessageBox.Show(this, "You must specify a program to run", "Missing program path", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
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

    private void buttonParamQuestion_Click(object sender, EventArgs e)
    {
      MessageBox.Show(this, _parametersMessage, "Parameters", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void buttonTest_Click(object sender, EventArgs e)
    {
      if (textBoxProgram.Text.Trim().Length == 0)
      {
        MessageBox.Show(this, "You must specify a program to run", "Missing program path", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
        return;
      }

      try
      {
        string[] launchCommand = new string[]
                                   {
                                     textBoxProgram.Text,
                                     textBoxStartup.Text,
                                     textBoxParameters.Text,
                                     comboBoxWindowStyle.SelectedItem as string,
                                     checkBoxNoWindow.Checked.ToString(),
                                     checkBoxShellExecute.Checked.ToString(),
                                     false.ToString(),
                                     checkBoxForceFocus.Checked.ToString()
                                   };

        Common.ProcessRunCommand(launchCommand);
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Test Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #endregion Buttons
  }
}