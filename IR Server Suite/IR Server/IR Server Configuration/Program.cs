#region Copyright (C) 2005-2009 Team MediaPortal

// Copyright (C) 2005-2009 Team MediaPortal
// http://www.team-mediaportal.com
// 
// This Program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2, or (at your option)
// any later version.
// 
// This Program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with GNU Make; see the file COPYING.  If not, write to
// the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
// http://www.gnu.org/copyleft/gpl.html

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using IRServer.Plugin;
using IrssUtils;
using TimeoutException = System.ServiceProcess.TimeoutException;
using IRServer.Configuration.Properties;
using System.Threading;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace IRServer.Configuration
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

  internal static class Program
  {
    #region Constants

    internal const string ServerName = "IRServer";
    internal const string ServerWindowName = "IRSS - " + ServerName;
    internal const string ServerDisplayName = "IR Server";

    private static readonly string ConfigurationFile = Path.Combine(Common.FolderAppData, @"IR Server\IR Server.xml");
    internal static readonly string IRServerFile = Path.Combine(Common.FolderProgramFiles, @"IR Server.exe");
    internal static readonly string AdminHelperFile = Path.Combine(Common.FolderProgramFiles, @"IR Server AdminHelper.exe");

    internal static readonly Icon _iconGray = new Icon(Resources.iconGray, new Size(16, 16));
    internal static readonly Icon _iconGreen = new Icon(Resources.iconGreen, new Size(16, 16));

    #endregion Constants

    #region Variables

    private static bool _abstractRemoteMode;
    private static string _hostComputer;
    private static IRServerMode _mode;
    private static string[] _pluginNameReceive;
    private static string _pluginNameTransmit;
    private static string _processPriority;
    private static NotifyIcon _notifyIcon;
    private static bool _inConfiguration;
    private static Thread thread;
    //private static ServiceController serviceController;
    private static ServiceController[] serviceControllers;
    private static IntPtr irsWindow;
    private static int waitCount;
    internal static IrsStatus _irsStatus;
    internal static bool _serviceInstalled;

    #endregion Variables

    #region Interops

    [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
    internal static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);
    [DllImport("user32.dll")]
    internal static extern int SendMessage(
          IntPtr hWnd,      // handle to destination window
          uint Msg,       // message
          long wParam,  // first message parameter
          long lParam   // second message parameter
          );

    #endregion Interops

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(true);

      // allow only one application instance
      if (IrssUtils.ProcessHelper.IsProcessAlreadyRunning())
        return;

      IrssLog.LogLevel = IrssLog.Level.Debug;
      IrssLog.Open("IR Server Configuration.log");

      _notifyIcon = new NotifyIcon();

      _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
      _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripLabel(ServerDisplayName));
      _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
      _notifyIcon.ContextMenuStrip.Items.Add("&Setup", null, OpenConfiguration);
      _notifyIcon.ContextMenuStrip.Items.Add("&Quit", null, ClickQuit);
      _notifyIcon.DoubleClick += new EventHandler(OpenConfiguration);
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
        irsWindow = FindWindowByCaption(IntPtr.Zero, ServerWindowName);
        if (irsWindow != IntPtr.Zero)
          _irsStatus = IrsStatus.RunningApplication;
      }
      catch { }
    }

    private static Icon getIcon()
    {
      return (_irsStatus == IrsStatus.NotRunning) ? _iconGray : _iconGreen;
    }

    private static void ClickQuit(object sender, EventArgs e)
    {
      Application.Exit();
    }

    private static void OpenConfiguration(object sender, EventArgs e)
    {
      if (_inConfiguration)
        return;

      IrssLog.Info("Setup");

      LoadSettings();

      Config config = new Config();

      config.AbstractRemoteMode = _abstractRemoteMode;
      config.Mode = _mode;
      config.HostComputer = _hostComputer;
      config.ProcessPriority = _processPriority;
      config.PluginReceive = _pluginNameReceive;
      config.PluginTransmit = _pluginNameTransmit;

      _inConfiguration = true;

      if (config.ShowDialog() == DialogResult.OK)
      {
        if ((_abstractRemoteMode != config.AbstractRemoteMode) ||
            (_mode != config.Mode) ||
            (_hostComputer != config.HostComputer) ||
            (_processPriority != config.ProcessPriority) ||
            (_pluginNameReceive != config.PluginReceive) ||
            (_pluginNameTransmit != config.PluginTransmit))
        {
          if (
            MessageBox.Show("IR Server will now be restarted for configuration changes to take effect",
                            "Restarting IR Server", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) ==
            DialogResult.OK)
          {
            // Change settings ...
            _abstractRemoteMode = config.AbstractRemoteMode;
            _mode = config.Mode;
            _hostComputer = config.HostComputer;
            _processPriority = config.ProcessPriority;
            _pluginNameReceive = config.PluginReceive;
            _pluginNameTransmit = config.PluginTransmit;

            SaveSettings();

            // Restart IR Server ...
            RestartIRS();
          }
          else
          {
            IrssLog.Info("Canceled settings changes");
          }
        }
      }

      _inConfiguration = false;

      IrssLog.Close();
    }

    private static void LoadSettings()
    {
      IrssLog.Info("Loading settings ...");

      _abstractRemoteMode = true;
      _mode = IRServerMode.ServerMode;
      _hostComputer = String.Empty;
      _processPriority = "No Change";
      _pluginNameReceive = null;
      _pluginNameTransmit = String.Empty;

      XmlDocument doc = new XmlDocument();

      try
      {
        doc.Load(ConfigurationFile);
      }
      catch (DirectoryNotFoundException)
      {
        IrssLog.Error("No configuration file found ({0}), folder not found! Creating default configuration file",
                      ConfigurationFile);

        Directory.CreateDirectory(Path.GetDirectoryName(ConfigurationFile));

        CreateDefaultSettings();
        return;
      }
      catch (FileNotFoundException)
      {
        IrssLog.Warn("No configuration file found ({0}), creating default configuration file", ConfigurationFile);

        CreateDefaultSettings();
        return;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        return;
      }

      try
      {
        _abstractRemoteMode = bool.Parse(doc.DocumentElement.Attributes["AbstractRemoteMode"].Value);
      }
      catch (Exception ex)
      {
        IrssLog.Warn(ex.ToString());
      }

      try
      {
        _mode =
          (IRServerMode)Enum.Parse(typeof(IRServerMode), doc.DocumentElement.Attributes["Mode"].Value, true);
      }
      catch (Exception ex)
      {
        IrssLog.Warn(ex.ToString());
      }

      try
      {
        _hostComputer = doc.DocumentElement.Attributes["HostComputer"].Value;
      }
      catch (Exception ex)
      {
        IrssLog.Warn(ex.ToString());
      }

      try
      {
        _processPriority = doc.DocumentElement.Attributes["ProcessPriority"].Value;
      }
      catch (Exception ex)
      {
        IrssLog.Warn(ex.ToString());
      }

      try
      {
        _pluginNameTransmit = doc.DocumentElement.Attributes["PluginTransmit"].Value;
      }
      catch (Exception ex)
      {
        IrssLog.Warn(ex.ToString());
      }

      try
      {
        string receivers = doc.DocumentElement.Attributes["PluginReceive"].Value;
        if (!String.IsNullOrEmpty(receivers))
          _pluginNameReceive = receivers.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
      }
      catch (Exception ex)
      {
        IrssLog.Warn(ex.ToString());
      }
    }

    private static void SaveSettings()
    {
      IrssLog.Info("Saving settings ...");

      try
      {
        using (XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, Encoding.UTF8))
        {
          writer.Formatting = Formatting.Indented;
          writer.Indentation = 1;
          writer.IndentChar = (char)9;
          writer.WriteStartDocument(true);
          writer.WriteStartElement("settings"); // <settings>

          writer.WriteAttributeString("AbstractRemoteMode", _abstractRemoteMode.ToString());
          writer.WriteAttributeString("Mode", Enum.GetName(typeof(IRServerMode), _mode));
          writer.WriteAttributeString("HostComputer", _hostComputer);
          writer.WriteAttributeString("ProcessPriority", _processPriority);
          writer.WriteAttributeString("PluginTransmit", _pluginNameTransmit);

          if (_pluginNameReceive != null)
          {
            StringBuilder receivers = new StringBuilder();
            for (int index = 0; index < _pluginNameReceive.Length; index++)
            {
              receivers.Append(_pluginNameReceive[index]);

              if (index < _pluginNameReceive.Length - 1)
                receivers.Append(',');
            }
            writer.WriteAttributeString("PluginReceive", receivers.ToString());
          }
          else
          {
            writer.WriteAttributeString("PluginReceive", String.Empty);
          }

          writer.WriteEndElement(); // </settings>
          writer.WriteEndDocument();
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
    }

    private static void CreateDefaultSettings()
    {
      try
      {
        string[] blasters = DetectBlasters();
        if (blasters == null)
          _pluginNameTransmit = String.Empty;
        else
          _pluginNameTransmit = blasters[0];
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        _pluginNameTransmit = String.Empty;
      }

      try
      {
        string[] receivers = DetectReceivers();
        if (receivers == null)
          _pluginNameReceive = null;
        else
          _pluginNameReceive = receivers;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        _pluginNameReceive = null;
      }

      try
      {
        SaveSettings();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
    }

    private static void RunAdminHelper(string arguments)
    {
      int exitCode = 0;

      Process process = new Process();
      process.StartInfo = new ProcessStartInfo();
      process.StartInfo.FileName = AdminHelperFile;
      process.StartInfo.Arguments = arguments;
      process.StartInfo.CreateNoWindow = true;
      process.StartInfo.UseShellExecute = true;

      // Must enable Exited event for both sync and async scenarios
      process.EnableRaisingEvents = true;

      try
      {
        process.Start();
        // Synchronously block until process is complete, then return exit code from process
        process.WaitForExit();
        exitCode = process.ExitCode;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        exitCode = 10;
      }


      if (exitCode != 0)
      {
        IrssLog.Error("RunAdminHelper exitcode = " + exitCode);
        MessageBox.Show("There occured an issue when trying to run AdminHelper." + Environment.NewLine +
          "Do you have administration rights?" + Environment.NewLine +
          "Did you accept the request for Administration rights?" + Environment.NewLine + Environment.NewLine +
          "If you think you did everything right, please report the issue to the devlopers.",
          "IR Server Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    internal static void ServiceInstall()
    {
      RunAdminHelper("/INSTALL");
    }

    internal static void ServiceUninstall()
    {
      RunAdminHelper("/UNINSTALL");
    }

    internal static void ServiceStart()
    {
      RunAdminHelper("/START");
    }

    internal static void ServiceStop()
    {
      RunAdminHelper("/STOP");
    }

    internal static void ApplicationStart()
    {
      try
      {
        IrssLog.Info("Starting IR Server (application)");
        Process IRServer = Process.Start(IRServerFile);
        waitCount = 0;
        while (FindWindowByCaption(IntPtr.Zero, ServerWindowName) == IntPtr.Zero)
        {
          waitCount++;
          if (waitCount > 150) throw new TimeoutException();
          Thread.Sleep(200);
        }
        IrssLog.Info("Starting IR Server (application) - done");
      }
      catch (TimeoutException ex)
      {
        IrssLog.Error("Starting IR Server (application) - failed (timeout error)");
        IrssLog.Error(ex);
      }
      catch (Exception ex)
      {
        IrssLog.Error("Starting IR Server (application) - failed (see following...)");
        IrssLog.Error(ex);
      }
    }

    internal static void ApplicationStop()
    {
      try
      {
        IrssLog.Info("Stopping IR Server (application)");
        IntPtr irssWindow = FindWindowByCaption(IntPtr.Zero, ServerWindowName);
        int result = SendMessage(irssWindow, 16, 0, 0);
        waitCount = 0;
        while (FindWindowByCaption(IntPtr.Zero, ServerWindowName) != IntPtr.Zero)
        {
          waitCount++;
          if (waitCount > 150) throw new TimeoutException();
          Thread.Sleep(200);
        }
        IrssLog.Info("Stopping IR Server (application) - done");
      }
      catch (TimeoutException ex)
      {
        IrssLog.Error("Stopping IR Server (application) - failed (timeout error)");
        IrssLog.Error(ex);
      }
      catch (Exception ex)
      {
        IrssLog.Error("Stopping IR Server (application) - failed (see following...)");
        IrssLog.Error(ex);
      }
    }

    private static void RestartIRS()
    {
      IrssLog.Info("Restarting IR Server");

      switch (_irsStatus)
      {
        case IrsStatus.RunningService:
          {
            ServiceStop();
            ServiceStart();
            break;
          }
        case IrsStatus.RunningApplication:
          {
            ApplicationStop();
            ApplicationStart();
            break;
          }
      }
      IrssLog.Info("Restarting IR Server - done");
    }

    /// <summary>
    /// Retreives a list of detected Receiver plugins.
    /// </summary>
    /// <returns>String array of plugin names.</returns>
    internal static string[] DetectReceivers()
    {
      IrssLog.Info("Detect Receivers ...");

      PluginBase[] plugins = BasicFunctions.AvailablePlugins();
      if (plugins == null || plugins.Length == 0)
        return null;

      List<string> receivers = new List<string>();

      foreach (PluginBase plugin in plugins)
      {
        try
        {
          if ((plugin is IRemoteReceiver || plugin is IKeyboardReceiver || plugin is IMouseReceiver) && plugin.Detect() == PluginBase.DetectionResult.DevicePresent)
            receivers.Add(plugin.Name);
        }
        catch (Exception ex)
        {
          IrssLog.Error(ex);
        }
      }

      if (receivers.Count > 0)
        return receivers.ToArray();

      return null;
    }

    /// <summary>
    /// Retreives a list of detected Blaster plugins.
    /// </summary>
    /// <returns>String array of plugin names.</returns>
    internal static string[] DetectBlasters()
    {
      IrssLog.Info("Detect Blasters ...");

      PluginBase[] plugins = BasicFunctions.AvailablePlugins();
      if (plugins == null || plugins.Length == 0)
        return null;

      List<string> blasters = new List<string>();

      foreach (PluginBase plugin in plugins)
      {
        try
        {
          if (plugin is ITransmitIR && plugin.Detect() == PluginBase.DetectionResult.DevicePresent)
            blasters.Add(plugin.Name);
        }
        catch (Exception ex)
        {
          IrssLog.Error(ex);
        }
      }

      if (blasters.Count > 0)
        return blasters.ToArray();

      return null;
    }
  }
}