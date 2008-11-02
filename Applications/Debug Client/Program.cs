using System;
using System.Windows.Forms;

namespace DebugClient
{
  internal static class Program
  {
    /// <summary>
    /// Main method.
    /// </summary>
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm());
    }
  }
}