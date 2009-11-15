using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using IRServer.Tray.Properties;
using IrssUtils;

namespace IRServer.Tray
{
  #region Enumerations

  /// <summary>
  /// Describes the operation mode of IR Server.
  /// </summary>
  internal enum IRServerMode
  {
    /// <summary>
    /// Acts as a standard Server (Default).
    /// </summary>
    ServerMode = 0,
    /// <summary>
    /// Relays button presses to another IR Server.
    /// </summary>
    RelayMode = 1,
    /// <summary>
    /// Acts as a repeater for another IR Server's blasting.
    /// </summary>
    RepeaterMode = 2,
  }

  /// <summary>
  /// Describes the actual status of IR Server
  /// </summary>
  internal enum IrsStatus
  {
    /// <summary>
    /// IR Server is not running.
    /// </summary>
    NotRunning,
    /// <summary>
    /// IR Server is running as Service.
    /// </summary>
    RunningService,
    /// <summary>
    /// IR Server is running as Application.
    /// </summary>
    RunningApplication
  }

  #endregion Enumerations

  static class Program
  {
    #region Constants

    internal const string ServerName = "IRServer";
    internal const string ServerWindowName = "IRSS - " + ServerName;
    internal const string ServerDisplayName = "IR Server";

    private static readonly string _configExe = Path.Combine(Common.FolderProgramFiles, @"IR Server Configuration.exe");
    private static readonly string _translatorExe = Path.Combine(Common.FolderProgramFiles, @"Translator.exe");
    private static readonly string _debugClientExe = Path.Combine(Common.FolderProgramFiles, @"DebugClient.exe");

    #endregion Constants

    #region Variables

    private static NotifyIcon _notifyIcon;
    private static Thread thread;

    internal static readonly Icon _iconGray = new Icon(Resources.iconGray, new Size(16, 16));
    internal static readonly Icon _iconGreen = new Icon(Resources.iconGreen, new Size(16, 16));

    private static ServiceController[] serviceControllers;
    private static IntPtr irsWindow;

    internal static IrsStatus _irsStatus;
    internal static bool _serviceInstalled;

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
      _notifyIcon.Text = ServerDisplayName;
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
        getStatus();
        _notifyIcon.Icon = getIcon();
        Thread.Sleep(1000);
      }
    }

    internal static void getStatus()
    {
      _irsStatus = IrsStatus.NotRunning;
      _serviceInstalled = false;
      serviceControllers = ServiceController.GetServices();
      foreach (ServiceController serviceController in serviceControllers)
      {
        if (serviceController.ServiceName == ServerName)
        {
          _serviceInstalled = true;
          if (serviceController.Status == ServiceControllerStatus.Running)
            _irsStatus = IrsStatus.RunningService;
        }
      }

      try
      {
        irsWindow = Win32.FindWindowByTitle(ServerWindowName);
        if (irsWindow != IntPtr.Zero)
          _irsStatus = IrsStatus.RunningApplication;
      }
      catch { }
    }

    private static Icon getIcon()
    {
      return (_irsStatus == IrsStatus.NotRunning) ? _iconGray : _iconGreen;
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
