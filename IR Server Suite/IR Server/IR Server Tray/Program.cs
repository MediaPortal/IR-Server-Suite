using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using IRServer.Tray.Properties;
using IrssUtils;

namespace IRServer.Tray
{
  static class Program
  {
    #region Constants

    private static readonly string _configExe = Path.Combine(Common.FolderProgramFiles, @"IR Server Configuration.exe");
    private static readonly string _translatorExe = Path.Combine(Common.FolderProgramFiles, @"Translator.exe");
    private static readonly string _debugClientExe = Path.Combine(Common.FolderProgramFiles, @"DebugClient.exe");

    #endregion Constants

    #region Variables

    private static NotifyIcon _notifyIcon;
    private static Thread thread;

    internal static readonly Icon _iconGray = new Icon(Resources.iconGray, new Size(16, 16));
    internal static readonly Icon _iconGreen = new Icon(Resources.iconGreen, new Size(16, 16));

    #endregion Variables

    /// <summary>
    /// Der Haupteinstiegspunkt für die Anwendung.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      if (ProcessHelper.IsProcessAlreadyRunning())
        return;

      _notifyIcon = new NotifyIcon();
      _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
      _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripLabel("IR Server Tray"));
      _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
      
      _notifyIcon.ContextMenuStrip.Items.Add("&Configuration", GetExeIconAsImage(_configExe), ClickConfiguration);
      
      if (File.Exists(_translatorExe))
        _notifyIcon.ContextMenuStrip.Items.Add("&Translator", GetExeIconAsImage(_translatorExe), ClickTranslator);
      
      if (File.Exists(_debugClientExe))
        _notifyIcon.ContextMenuStrip.Items.Add("&Debug Client", GetExeIconAsImage(_debugClientExe), ClickDebugClient);
      
      _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
      _notifyIcon.ContextMenuStrip.Items.Add("&Exit", null, ClickExit);
      //_notifyIcon.DoubleClick += new EventHandler(OpenConfiguration);
      _notifyIcon.Icon = new System.Drawing.Icon(Resources.iconGray, new System.Drawing.Size(16, 16));
      _notifyIcon.Text = Shared.ServerDisplayName;
      _notifyIcon.Visible = true;

      thread = new Thread(new ThreadStart(UpdateIcon));
      thread.IsBackground = true;
      thread.Start();

      Application.Run();
      thread.Abort();
      _notifyIcon.Visible = false;
      _notifyIcon = null;
    }

    #region EventHandler

    private static void ClickConfiguration(object sender, EventArgs e)
    {
      Process.Start(_configExe);
    }

    private static void ClickTranslator(object sender, EventArgs e)
    {
      Process.Start(_translatorExe);
    }

    private static void ClickDebugClient(object sender, EventArgs e)
    {
      Process.Start(_debugClientExe);
    }

    private static void ClickExit(object sender, EventArgs e)
    {
      Application.Exit();
    }

    #endregion

    #region Update IR Server Status

    private static void UpdateIcon()
    {
      while (thread != null && thread.IsAlive)
      {
        Shared.getStatus();
        _notifyIcon.Icon = getIcon();
        Thread.Sleep(1000);
      }
    }

    private static Icon getIcon()
    {
      return (Shared._irsStatus == IrsStatus.NotRunning) ? _iconGray : _iconGreen;
    }

    #endregion

    private static Image GetExeIconAsImage(string filepath)
    {
      if (filepath == null) return null;

      Icon icon = Icon.ExtractAssociatedIcon(filepath);
      if (icon == null) return null;

      return icon.ToBitmap();
    }
  }
}
