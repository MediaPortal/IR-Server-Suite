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

namespace MPUtils.Forms
{
  /// <summary>
  /// Send MediaPortal Message command form.
  /// </summary>
  public partial class MPMessage : Form
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
        return String.Format("{0}|{1}|{2}|{3}|{4}|{5}",
                             comboBoxMessageType.Text,
                             textBoxWindowId.Text.Trim(),
                             textBoxSenderId.Text.Trim(),
                             textBoxControlId.Text.Trim(),
                             textBoxParam1.Text.Trim(),
                             textBoxParam2.Text.Trim());
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Default Constructor.
    /// </summary>
    public MPMessage()
    {
      InitializeComponent();

      SetupComboBox();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MPMessage"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public MPMessage(string[] parameters)
      : this()
    {
      comboBoxMessageType.SelectedItem = parameters[0];
      textBoxWindowId.Text = parameters[1];
      textBoxSenderId.Text = parameters[2];
      textBoxControlId.Text = parameters[3];
      textBoxParam1.Text = parameters[4];
      textBoxParam2.Text = parameters[5];
    }

    #endregion Constructors

    private void SetupComboBox()
    {
      comboBoxMessageType.Items.Clear();

      string[] items = Enum.GetNames(typeof (GUIMessage.MessageType));
      Array.Sort(items);

      comboBoxMessageType.Items.AddRange(items);
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