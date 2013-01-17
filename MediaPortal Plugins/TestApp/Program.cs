using System;
using System.Windows.Forms;
using MediaPortal.Plugins.IRSS.MPBlastZonePlugin;
using MediaPortal.Plugins.IRSS.MPControlPlugin;

namespace TestApp
{
  static class Program
  {
    /// <summary>
    /// Der Haupteinstiegspunkt für die Anwendung.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      MPBlastZonePlugin plugin = new MPBlastZonePlugin();

      plugin.ShowPlugin();
    }
  }
}
