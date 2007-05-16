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

      IrssLog.LogLevel = IrssLog.Level.Debug;
      IrssLog.Open(Common.FolderIrssLogs + "Tray Launcher.log");

      IrssLog.Debug("Platform is {0}", (IntPtr.Size == 4 ? "32-bit" : "64-bit"));

      Tray tray = new Tray();

      if (tray.Start())
        Application.Run();

      IrssLog.Close();
    }

  }

}
