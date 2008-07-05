using System;
using System.Windows.Forms;

namespace DebugClient
{

  static class Program
  {

    /// <summary>
    /// Main method.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm());
    }

  }

}
