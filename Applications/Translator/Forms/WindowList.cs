using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Translator
{

  public partial class WindowList : Form
  {

    #region Interop

    public delegate bool EnumWindowsProc(int hWnd, int lParam);

    [DllImport("user32.dll")]
    private static extern int GetWindowText(int hWnd, StringBuilder title, int size);
    
    [DllImport("user32.dll")]
    private static extern int GetWindowModuleFileName(int hWnd, StringBuilder title, int size);
    
    [DllImport("user32.dll")]
    private static extern int EnumWindows(EnumWindowsProc ewp, int lParam); 

    #endregion Interop

    #region Properties

    public string SelectedWindowTitle
    {
      get { return listBoxWindows.SelectedItem as string; }
    }

    #endregion Properties

    #region Constructor

    public WindowList()
    {
      InitializeComponent();

      PopulateList();
    }

    #endregion Constructor

    void PopulateList()
    {
      EnumWindowsProc ewp = new EnumWindowsProc(EvalWindow);

      EnumWindows(ewp, 0);
    }

    bool EvalWindow(int hWnd, int lParam)
    {
      StringBuilder title = new StringBuilder(256);
      GetWindowText(hWnd, title, 256);

      //StringBuilder module = new StringBuilder(256);
      //GetWindowModuleFileName(hWnd, module, 256);

      if (title.Length == 0)
        return true;

      listBoxWindows.Items.Add(title.ToString());

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
