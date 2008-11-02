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