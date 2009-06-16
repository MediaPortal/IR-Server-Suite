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
using MediaPortal.GUI.Library;

namespace Commands.MediaPortal
{
  /// <summary>
  /// Send MediaPortal Action command form.
  /// </summary>
  public partial class EditSendAction : Form
  {
    #region Properties

    /// <summary>
    /// Gets the command string.
    /// </summary>
    /// <value>The command string.</value>
    public string[] Parameters
    {
      get { return new string[] {comboBoxActionType.Text, textBoxFloat1.Text, textBoxFloat2.Text}; }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EditSendAction"/> class.
    /// </summary>
    public EditSendAction()
    {
      InitializeComponent();

      SetupComboBox();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EditSendAction"/> class.
    /// </summary>
    /// <param name="parameters">The command parameters.</param>
    public EditSendAction(string[] parameters)
      : this()
    {
      comboBoxActionType.Text = parameters[0];
      textBoxFloat1.Text = parameters[1];
      textBoxFloat2.Text = parameters[2];
    }

    #endregion Constructors

    private void SetupComboBox()
    {
      comboBoxActionType.Items.Clear();

      string[] items = Enum.GetNames(typeof (Action.ActionType));
      Array.Sort(items);

      comboBoxActionType.Items.AddRange(items);
    }

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