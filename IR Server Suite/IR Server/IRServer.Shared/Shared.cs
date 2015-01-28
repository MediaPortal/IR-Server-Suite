using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using IRServer.Plugin;
using IrssUtils;
using TimeoutException = System.ServiceProcess.TimeoutException;

namespace IRServer
{
  #region Enumerations

  /// <summary>
  /// Describes the operation mode of IR Server.
  /// </summary>
  public enum IRServerMode
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
  public enum IrsStatus
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

  public static class Shared
  {
    #region Constants

    public static readonly string IRServerFile = Path.Combine(
      Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase), @"IR Server.exe");

    public const string ServerName = "IRServer";
    public const string ServerWindowName = "IRSS - " + ServerName;
    public const string ServerDisplayName = "IR Server";

    private static readonly TimeSpan defaultServiceTime = new TimeSpan(0, 0, 30);

    #endregion

    #region Variables

    private static ServiceController serviceController;
    private static int waitCount;

    public static ServiceController[] serviceControllers;
    private static IntPtr irsWindow;

    public static IrsStatus irsStatus;
    public static bool serviceInstalled;

    #endregion

    #region Start / Stop   App / Service

    private static ServiceController getServiceController()
    {
      Shared.serviceControllers = ServiceController.GetServices();
      foreach (ServiceController sc in Shared.serviceControllers)
      {
        if (sc.ServiceName == Shared.ServerName)
          return sc;
      }
      return null;
    }

    public static void ServiceInstall()
    {
      try
      {
        IrssLog.Info("Installing IR Server service");
        Process IRServer = Process.Start(Shared.IRServerFile, "/INSTALL");
        IRServer.WaitForExit((int)defaultServiceTime.TotalMilliseconds);
        IrssLog.Info("Installing IR Server service - done");
      }
      catch (Exception ex)
      {
        IrssLog.Error("Installing IR Server service - failed (see following...)");
        IrssLog.Error(ex);
      }
    }

    public static void ServiceUninstall()
    {
      try
      {
        IrssLog.Info("Uninstalling IR Server service");
        Process IRServer = Process.Start(Shared.IRServerFile, "/UNINSTALL");
        IRServer.WaitForExit((int)defaultServiceTime.TotalMilliseconds);
        IrssLog.Info("Uninstalling IR Server service - done");
      }
      catch (Exception ex)
      {
        IrssLog.Error("Uninstalling IR Server service - failed (see following...)");
        IrssLog.Error(ex);
      }
    }

    public static void ServiceStart()
    {
      try
      {
        serviceController = getServiceController();
        if (Shared.serviceControllers != null)
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

    public static void ServiceStop()
    {
      try
      {
        serviceController = getServiceController();
        if (Shared.serviceControllers != null)
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

    public static void RestartIRS()
    {
      IrssLog.Info("Restarting IR Server");

      switch (Shared.irsStatus)
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

    public static void ApplicationStart()
    {
      try
      {
        IrssLog.Info("Starting IR Server (application)");
        Process IRServer = Process.Start(IRServerFile);
        waitCount = 0;
        while (Win32.FindWindowByTitle(Shared.ServerWindowName) == IntPtr.Zero)
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

    public static void ApplicationStop()
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

    #endregion

    #region detect hardware

    /// <summary>
    /// Retreives a list of detected Receiver plugins.
    /// </summary>
    /// <returns>String array of plugin names.</returns>
    public static string[] DetectReceivers()
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
    public static string[] DetectBlasters()
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

    #endregion

    #region getstatus

    public static void getStatus()
    {
      irsStatus = IrsStatus.NotRunning;
      serviceInstalled = false;
      serviceControllers = ServiceController.GetServices();
      foreach (ServiceController serviceController in serviceControllers)
      {
        if (serviceController.ServiceName == ServerName)
        {
          serviceInstalled = true;
          if (serviceController.Status == ServiceControllerStatus.Running)
            irsStatus = IrsStatus.RunningService;
        }
      }

      try
      {
        irsWindow = Win32.FindWindowByTitle(ServerWindowName);
        if (irsWindow != IntPtr.Zero)
          irsStatus = IrsStatus.RunningApplication;
      }
      catch { }
    }

    #endregion
  }
}
