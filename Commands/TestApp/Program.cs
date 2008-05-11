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

      CommandEditDialog dialog = new CommandEditDialog(new ControlLabel());
      dialog.ShowDialog();

      Processor commandProcessor = new Processor(new BlastIrDelegate(BlastIr), new string[] { "Default", "Port 1" });

      string[] categories = new string[]
      { 
        Processor.CategoryControl,
        Processor.CategoryVariable,
        Processor.CategoryStack,

        Processor.CategoryGeneral,
        Processor.CategoryMediaPortal,
        Processor.CategoryIRCommands,
        Processor.CategoryMacros,

        Processor.CategorySpecial
      };

      try
      {
        EditMacro edit1 = new EditMacro(
          commandProcessor,
          @"C:\Documents and Settings\All Users.WINDOWS\Application Data\IR Server Suite\MP Blast Zone Plugin\Macro\",
          categories);

        edit1.ShowDialog();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString());
      }

      try
      {
        EditMacro edit2 = new EditMacro(
          commandProcessor,
          @"C:\Documents and Settings\All Users.WINDOWS\Application Data\IR Server Suite\MP Blast Zone Plugin\Macro\",
          categories,
          @"C:\Documents and Settings\All Users.WINDOWS\Application Data\IR Server Suite\MP Blast Zone Plugin\Macro\Toggle Example.Macro");

        edit2.ShowDialog();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString());
      }
    }


    static void BlastIr(string fileName, string port)
    {
      MessageBox.Show(String.Format("File - {0}, Port - {1}", fileName, port), "Blast Command", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

  }

}
