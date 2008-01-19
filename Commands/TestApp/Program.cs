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

      string[] categories = new string[] { "General Commands", "MediaPortal Commands" };

      EditMacro edit1 = new EditMacro(
        commandProcessor, 
        @"C:\Documents and Settings\All Users.WINDOWS\Application Data\IR Server Suite\MP Blast Zone Plugin\Macro\",
        categories);

      edit1.ShowDialog();

      EditMacro edit2 = new EditMacro(
        commandProcessor,
        categories,
        @"C:\New");
        //@"C:\Documents and Settings\All Users.WINDOWS\Application Data\IR Server Suite\MP Blast Zone Plugin\Macro\Test");

      edit2.ShowDialog();
    }


    static void BlastIr(string fileName, string port)
    {
      MessageBox.Show(String.Format("File - {0}, Port - {1}", fileName, port), "Blast Command", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

  }

}
