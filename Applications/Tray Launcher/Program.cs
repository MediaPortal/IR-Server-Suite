using System;
using System.Diagnostics;
using System.Windows.Forms;

using IrssUtils;

namespace TrayLauncher
{

  static class Program
  {
    
    [STAThread]
    static void Main()
    {
      // Check for multiple instances.
      if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length != 1)
        return;

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      // TODO: Change log level to info for release.
      IrssLog.LogLevel = IrssLog.Level.Debug;
      IrssLog.Open(Common.FolderIrssLogs + "Tray Launcher.log");

      Tray tray = new Tray();

      if (tray.Start())
        Application.Run();

      IrssLog.Close();
    }

  }

}
