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

    internal static readonly string IRServerFile = Path.Combine(Common.FolderProgramFiles, @"IR Server.exe");

    private static readonly TimeSpan defaultServiceTime = new TimeSpan(0, 0, 30);

    #endregion Constants

    #region Variables

    private static bool _abstractRemoteMode;
    private static string _hostComputer;
    private static IRServerMode _mode;
    private static string[] _pluginNameReceive;
    private static string _pluginNameTransmit;
    private static string _processPriority;
    private static bool _inConfiguration;
    private static ServiceController serviceController;
    private static ServiceController[] serviceControllers;
    private static IntPtr irsWindow;
    private static int waitCount;
    internal static IrsStatus _irsStatus;
    internal static bool _serviceInstalled;

    #endregion Variables

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(true);

      if (ProcessHelper.IsProcessAlreadyRunning())
        return;

      IrssLog.LogLevel = IrssLog.Level.Debug;
      IrssLog.Open("IR Server Configuration.log");

      Application.Run(new Config());

      IrssLog.Close();
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

    private static ServiceController getServiceController()
    {
      serviceControllers = ServiceController.GetServices();
      foreach (ServiceController sc in serviceControllers)
      {
        if (sc.ServiceName == ServerName)
          return sc;
      }
      return null;
    }

    internal static void ServiceInstall()
    {
      try
      {
        IrssLog.Info("Installing IR Server service");
        Process IRServer = Process.Start(Program.IRServerFile, "/INSTALL");
        IRServer.WaitForExit((int)defaultServiceTime.TotalMilliseconds);
        IrssLog.Info("Installing IR Server service - done");
      }
      catch (Exception ex)
      {
        IrssLog.Error("Installing IR Server service - failed (see following...)");
        IrssLog.Error(ex);
      }
    }

    internal static void ServiceUninstall()
    {
      try
      {
        IrssLog.Info("Uninstalling IR Server service");
        Process IRServer = Process.Start(Program.IRServerFile, "/UNINSTALL");
        IRServer.WaitForExit((int)defaultServiceTime.TotalMilliseconds);
        IrssLog.Info("Uninstalling IR Server service - done");
      }
      catch (Exception ex)
      {
        IrssLog.Error("Uninstalling IR Server service - failed (see following...)");
        IrssLog.Error(ex);
      }
    }

    internal static void ServiceStart()
    {
      try
      {
        serviceController = getServiceController();
        if (serviceControllers != null)
        {
          IrssLog.Info("Starting IR Server (service)");
          serviceController.Start();
          serviceController.WaitForStatus(ServiceControllerStatus.Running, defaultServiceTime);
          IrssLog.Info("Starting IR Server (service) - done");
        }
      }
      catch (TimeoutException ex)
      {
        IrssLog.Error("Starting IR Server (service) - failed (timeout error)");
        IrssLog.Error(ex);
      }
      catch (Exception ex)
      {
        IrssLog.Error("Starting IR Server (service) - failed (see following...)");
        IrssLog.Error(ex);
      }
    }

    internal static void ServiceStop()
    {
      try
      {
        serviceController = getServiceController();
        if (serviceControllers != null)
        {
          IrssLog.Info("Stopping IR Server (service)");
          serviceController.Stop();
          serviceController.WaitForStatus(ServiceControllerStatus.Stopped, defaultServiceTime);
          IrssLog.Info("Stopping IR Server (service) - done");
        }
      }
      catch (TimeoutException ex)
      {
        IrssLog.Error("Stopping IR Server (service) - failed (timeout error)");
        IrssLog.Error(ex);
      }
      catch (Exception ex)
      {
        IrssLog.Error("Stopping IR Server (service) - failed (see following...)");
        IrssLog.Error(ex);
      }
    }

    internal static void ApplicationStart()
    {
      try
      {
        IrssLog.Info("Starting IR Server (application)");
        Process IRServer = Process.Start(IRServerFile);
        waitCount = 0;
        while (Win32.FindWindowByTitle(ServerWindowName) == IntPtr.Zero)
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
        IntPtr irssWindow = Win32.FindWindowByTitle(ServerWindowName);
        IntPtr result = Win32.SendWindowsMessage(irssWindow, 16, 0, 0);
        waitCount = 0;
        while (Win32.FindWindowByTitle(ServerWindowName) != IntPtr.Zero)
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

    internal static void RestartIRS()
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