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
  /// Go To Screen command input form.
  /// </summary>
  public partial class EditGotoScreen : Form
  {
    #region Properties

    /// <summary>
    /// MediaPortal screen identifier.
    /// </summary>
    public string ScreenID
    {
      get { return comboBoxScreen.Text; }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Default Constructor.
    /// </summary>
    public EditGotoScreen() : this(String.Empty)
    {
    }

    /// <summary>
    /// Create the form with a preselected MediaPortal screen identifier.
    /// </summary>
    /// <param name="selected">MediaPortal screen identifier.</param>
    public EditGotoScreen(string selected)
    {
      InitializeComponent();

      SetupComboBox();

      if (String.IsNullOrEmpty(selected))
        comboBoxScreen.SelectedIndex = 0;
      else
        comboBoxScreen.Text = selected;
    }

    #endregion Constructors

    private void SetupComboBox()
    {
      comboBoxScreen.Items.Clear();
      string[] items = Enum.GetNames(typeof (GUIWindow.Window));

      int index;
      for (index = 0; index < items.Length; index++)
        items[index] = items[index].Substring(7);

      Array.Sort(items);

      for (index = 0; index < items.Length; index++)
      {
        if (items[index].Equals("INVALID", StringComparison.OrdinalIgnoreCase))
          continue;

        comboBoxScreen.Items.Add(items[index]);
      }
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