using System;
using System.ComponentModel;
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

    private static readonly string _configExe = Path.Combine(Application.StartupPath, @"IR Server Configuration.exe");
    private static readonly string _translatorExe = Path.Combine(Application.StartupPath, @"Translator.exe");
    private static readonly string _debugClientExe = Path.Combine(Application.StartupPath, @"DebugClient.exe");

    private const int TimeToWaitForRestart = 10000;

    #endregion Constants

    #region Variables

    private static NotifyIcon _notifyIcon;
    private static Thread thread;

    internal static readonly Icon _iconGray = new Icon(Resources.iconGray, new Size(16, 16));
    internal static readonly Icon _iconGreen = new Icon(Resources.iconGreen, new Size(16, 16));

    private static HardwareMonitor _hardwareMonitor;
    private static DateTime _lastDeviceEvent = DateTime.MinValue;

    #endregion Variables

    /// <summary>
    /// Der Haupteinstiegspunkt für die Anwendung.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Thread.CurrentThread.Name = "Main Thread";
      IrssLog.Open("IR Server Tray.log");

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      if (ProcessHelper.IsProcessAlreadyRunning())
        return;

      _notifyIcon = new NotifyIcon();
      _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
      _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripLabel("IR Server Tray"));
      _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());

      if (File.Exists(_configExe))
      _notifyIcon.ContextMenuStrip.Items.Add("&Configuration", Win32.GetImageFromFile(_configExe), ClickConfiguration);

      if (File.Exists(_translatorExe))
        _notifyIcon.ContextMenuStrip.Items.Add("&Translator", Win32.GetImageFromFile(_translatorExe), ClickTranslator);
      
      if (File.Exists(_debugClientExe))
        _notifyIcon.ContextMenuStrip.Items.Add("&Debug Client", Win32.GetImageFromFile(_debugClientExe), ClickDebugClient);
      
      _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
      _notifyIcon.ContextMenuStrip.Items.Add("&Exit", null, ClickExit);
      //_notifyIcon.DoubleClick += new EventHandler(OpenConfiguration);
      _notifyIcon.Icon = new System.Drawing.Icon(Resources.iconGray, new System.Drawing.Size(16, 16));
      _notifyIcon.Text = Shared.ServerDisplayName;
      _notifyIcon.Visible = true;

      Settings.LoadSettings();
      if (Settings.RestartOnUSBChanges)
      {
        _hardwareMonitor = new HardwareMonitor();
        _hardwareMonitor.DeviceConnected += new HardwareMonitor.HardwareMonitorEvent(OnDeviceConnected);
        _hardwareMonitor.Start();
      }

      thread = new Thread(new ThreadStart(UpdateIcon));
      thread.IsBackground = true;
      thread.Start();

      Application.Run();
      thread.Abort();

      _hardwareMonitor.Stop();
      _hardwareMonitor = null;

      _notifyIcon.Visible = false;
      _notifyIcon = null;

      IrssLog.Close();
    }

    #region EventHandler

    private static void ClickConfiguration(object sender, EventArgs e)
    {
      try
      {
        Process.Start(_configExe);
      }
      catch (Win32Exception ex)
      {
        IrssLog.Error(ex);
      }
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

    #region Hardware Monitoring

    private static void OnDeviceConnected()
    {
      Thread lazyRestartThread = new Thread(LazyRestart);
      lazyRestartThread.Start();
    }

    private static void LazyRestart()
    {
      DateTime tempLastDeviceEvent = DateTime.Now;
      _lastDeviceEvent = tempLastDeviceEvent;

      // wait, if new decice events occur
      Thread.Sleep(TimeToWaitForRestart);

      // if new device event occured, stop here
      if (!tempLastDeviceEvent.Equals(_lastDeviceEvent)) return;

      // restart service
      IrssLog.Info("New device event. Restarting Input Service.");
      Shared.ApplicationStop();
      Shared.ApplicationStart();
    }

    #endregion
  }
}
