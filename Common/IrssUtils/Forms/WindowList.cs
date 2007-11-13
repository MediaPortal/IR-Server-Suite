using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace IrssUtils.Forms
{

  /// <summary>
  /// Window List form.
  /// </summary>
  public partial class WindowList : Form
  {

    #region Properties

    /// <summary>
    /// Gets the selected window title.
    /// </summary>
    /// <value>The selected window title.</value>
    public string SelectedWindowTitle
    {
      get { return listBoxWindows.SelectedItem as string; }
    }

    #endregion Properties

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowList"/> class.
    /// </summary>
    public WindowList()
    {
      InitializeComponent();

      PopulateList();
    }

    #endregion Constructor

    void PopulateList()
    {
      Win32.EnumWindowsProc ewp = new Win32.EnumWindowsProc(AddWindow);

      Win32.EnumerateWindows(ewp, IntPtr.Zero);
    }

    bool AddWindow(IntPtr hWnd, IntPtr lParam)
    {
      string title = Win32.GetWindowTitle(hWnd);

      if (!String.IsNullOrEmpty(title))
        listBoxWindows.Items.Add(title);

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
