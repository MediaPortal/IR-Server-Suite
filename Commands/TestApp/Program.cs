using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Commands
{

  static class Program
  {

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      Processor commandProcessor = new Processor(new BlastIrDelegate(BlastIr), new string[] { "Default", "Port 1" });

      EditMacro edit = new EditMacro(commandProcessor, "C:\\", new string[] { "General Commands", "MediaPortal Commands" });

      edit.ShowDialog();
    }


    static void BlastIr(string fileName, string port)
    {
      MessageBox.Show(String.Format("File - {0}, Port - {1}", fileName, port), "Blast Command", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

  }

}
