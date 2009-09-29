using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using IrssUtils;
using TimeoutException=System.TimeoutException;

namespace IR_Server_AdminHelper
{
  class Program
  {
    #region Constants

    internal const string ServerName = "IRServer";
    internal const string ServerWindowName = "IRSS - " + ServerName;
    internal const string ServerDisplayName = "IR Server";

    //private static readonly string ConfigurationFile = Path.Combine(Common.FolderAppData, @"IR Server\IR Server.xml");
    internal static readonly string IRServerFile = Path.Combine(Common.FolderProgramFiles, @"IR Server.exe");

    private static readonly TimeSpan defaultServiceTime = new TimeSpan(0, 0, 30);

    #endregion Constants

    #region Variables

    //private static bool _abstractRemoteMode;
    //private static string _hostComputer;
    //private static IRServerMode _mode;
    //private static string[] _pluginNameReceive;
    //private static string _pluginNameTransmit;
    //private static string _processPriority;
    //private static NotifyIcon _notifyIcon;
    //private static bool _inConfiguration;
    //private static Thread thread;
    private static ServiceController serviceController;
    private static ServiceController[] serviceControllers;
    //private static IntPtr irsWindow;
    //private static int waitCount;
    //internal static IrsStatus _irsStatus;
    //internal static bool _serviceInstalled;

    #endregion Variables

    static void Main(string[] args)
    {
      try
      {
        IrssLog.LogLevel = IrssLog.Level.Debug;
        IrssLog.Open("IR Server AdminHelper.log");

        if (args.Length == 0)
        {
          throw new InvalidOperationException(String.Format("No command line parameter specified."));
        }
        else
        {
          foreach (string parameter in args)
          {
            switch (parameter.ToUpperInvariant().Replace("-", "/"))
            {
              case "/INSTALL":
                IrssLog.Info("Installing IR Server ...");
                ServiceInstall();
                break;

              case "/UNINSTALL":
                IrssLog.Info("Uninstalling IR Server ...");
                ServiceUninstall();
                break;

              case "/START":
                IrssLog.Info("Starting IR Server ...");
                ServiceStart();
                break;

              case "/STOP":
                IrssLog.Info("Stopping IR Server ...");
                ServiceStop();
                break;

              case "/RESTART":
                IrssLog.Info("Restarting IR Server ...");
                ServiceStop();
                ServiceStart();
                break;

              default:
                throw new InvalidOperationException(String.Format("Unknown command line parameter \"{0}\"", parameter));
            }
          }

          IrssLog.Info("Done.");
        }

        Environment.ExitCode = 0;
      }
      catch (System.Security.SecurityException secEx)
      {
        IrssLog.Error(secEx);
        Environment.ExitCode = 1;
      }
      catch (UnauthorizedAccessException authEx)
      {
        IrssLog.Error(authEx);
        Environment.ExitCode = 2;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        Environment.ExitCode = 3;
      }
      finally
      {
        IrssLog.Close();
      }
    }

    #region Service handler

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

    #endregion
  }
}
