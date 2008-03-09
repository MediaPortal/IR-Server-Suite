using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace IrssUtils.Forms
{

  /// <summary>
  /// Window or Class List form.
  /// </summary>
  public partial class WindowList : Form
  {

    #region Variables

    bool _listClasses;

    Win32.EnumWindowsProc _ewp;

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
        this.Text = "Class List";
      else
        this.Text = "Window List";

      _ewp = new Win32.EnumWindowsProc(AddWindow);

      PopulateList();
    }

    #endregion Constructor

    void PopulateList()
    {
      Win32.EnumerateWindows(_ewp, IntPtr.Zero);
    }

    bool AddWindow(IntPtr hWnd, IntPtr lParam)
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
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void listBoxWindows_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar == 27)
      {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
      }
    }

  }

}
