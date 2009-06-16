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
  /// Window or Class List form.
  /// </summary>
  public partial class WindowList : Form
  {
    #region Variables

    private readonly Win32.EnumWindowsProc _ewp;
    private readonly bool _listClasses;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Gets the selected window title or class name.
    /// </summary>
    /// <value>The selected window title or class name.</value>
    public string SelectedItem
    {
      get { return listBoxItems.SelectedItem as string; }
    }

    #endregion Properties

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowList"/> class.
    /// </summary>
    /// <param name="listClasses">if set to <c>true</c> [list classes].</param>
    public WindowList(bool listClasses)
    {
      _listClasses = listClasses;

      InitializeComponent();

      if (listClasses)
        Text = "Class List";
      else
        Text = "Window List";

      _ewp = AddWindow;

      PopulateList();
    }

    #endregion Constructor

    private void PopulateList()
    {
      Win32.EnumerateWindows(_ewp, IntPtr.Zero);
    }

    private bool AddWindow(IntPtr hWnd, IntPtr lParam)
    {
      string windowTitle = Win32.GetWindowTitle(hWnd);

      if (_listClasses && String.IsNullOrEmpty(windowTitle))
      {
        string className = Win32.GetClassName(hWnd);
        listBoxItems.Items.Add(className);
      }
      else if (!_listClasses && !String.IsNullOrEmpty(windowTitle))
      {
        listBoxItems.Items.Add(windowTitle);
      }

      return true;
    }

    private void listBoxWindows_DoubleClick(object sender, EventArgs e)
    {
      DialogResult = DialogResult.OK;
      Close();
    }

    private void listBoxWindows_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar == 27)
      {
        DialogResult = DialogResult.Cancel;
        Close();
      }
    }
  }
}