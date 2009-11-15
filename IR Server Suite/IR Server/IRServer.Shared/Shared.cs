using System;
using System.Collections.Generic;
using System.ServiceProcess;
using IRServer.Plugin;
using IrssUtils;

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

    public const string ServerName = "IRServer";
    public const string ServerWindowName = "IRSS - " + ServerName;
    public const string ServerDisplayName = "IR Server";

    #endregion

    #region Variables

    public static ServiceController[] serviceControllers;
    private static IntPtr irsWindow;

    public static IrsStatus _irsStatus;
    public static bool _serviceInstalled;

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

    #region

    public static void getStatus()
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

    #endregion
  }
}
