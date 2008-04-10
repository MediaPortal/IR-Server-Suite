using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
#if TRACE
using System.Diagnostics;
#endif
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Text;

using Microsoft.Win32;

using InputService.Plugin;
using IrssUtils;

namespace InputService
{

  static class Program
  {

    #region Constants

    public const string ServiceName         = "InputService";
    public const string ServiceDisplayName  = "Input Service";
    public const string ServiceDescription  = "The main component of IR Server Suite, the Input Service provides access to your input devices";

    #endregion Constants

    /// <summary>
    /// The main entry point for the service.
    /// </summary>
    /// <param name="args">Command line parameters.</param>
    static void Main(string[] args)
    {
#if DEBUG
      IrssLog.LogLevel = IrssLog.Level.Debug;
#else
      IrssLog.LogLevel = IrssLog.Level.Info;
#endif

      try
      {
        if (args.Length == 0)
        {
          IrssLog.Open("Input Service.log");

          InputService inputService = new InputService();
          ServiceBase.Run(inputService);
        }
        else
        {
          IrssLog.Open("Input Service - Command Line.log");

          foreach (string parameter in args)
          {
            switch (parameter.ToUpperInvariant())
            {
              case "/INSTALL":
                IrssLog.Info("Installing Input Service ...");
                using (TransactedInstaller transactedInstaller = new TransactedInstaller())
                {
                  using (InputServiceInstaller inputServiceInstaller = new InputServiceInstaller())
                  {
                    transactedInstaller.Installers.Add(inputServiceInstaller);

                    string path = "/assemblypath=" + Assembly.GetExecutingAssembly().Location;
                    string[] cmdline = { path };

                    InstallContext installContext = new InstallContext(String.Empty, cmdline);
                    transactedInstaller.Context = installContext;

                    transactedInstaller.Install(new Hashtable());
                  }
                }
                break;

              case "/UNINSTALL":
                IrssLog.Info("Uninstalling Input Service ...");
                using (TransactedInstaller transactedInstaller = new TransactedInstaller())
                {
                  using (InputServiceInstaller inputServiceInstaller = new InputServiceInstaller())
                  {
                    transactedInstaller.Installers.Add(inputServiceInstaller);

                    string path = "/assemblypath=" + Assembly.GetExecutingAssembly().Location;
                    string[] cmdline = { path };

                    InstallContext installContext = new InstallContext(String.Empty, cmdline);
                    transactedInstaller.Context = installContext;

                    transactedInstaller.Uninstall(null);
                  }
                }
                break;

              case "/START":
                IrssLog.Info("Starting Input Service ...");
                using (ServiceController serviceController = new ServiceController(ServiceName))
                  if (serviceController.Status == ServiceControllerStatus.Stopped)
                    serviceController.Start();
                break;

              case "/STOP":
                IrssLog.Info("Stopping Input Service ...");
                using (ServiceController serviceController = new ServiceController(ServiceName))
                  if (serviceController.Status == ServiceControllerStatus.Running)
                    serviceController.Stop();
                break;

              case "/RESTART":
                IrssLog.Info("Restarting Input Service ...");
                using (ServiceController serviceController = new ServiceController(ServiceName))
                {
                  if (serviceController.Status == ServiceControllerStatus.Running)
                    serviceController.Stop();

                  serviceController.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 30));

                  if (serviceController.Status == ServiceControllerStatus.Stopped)
                    serviceController.Start();
                }
                break;

              default:
                throw new InvalidOperationException(String.Format("Unknown command line parameter \"{0}\"", parameter));
            }
          }

          IrssLog.Info("Done.");
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
      finally
      {
        IrssLog.Close();
      }
    }

    /// <summary>
    /// Retreives a list of available Input Service plugins.
    /// </summary>
    /// <returns>Array of plugin instances.</returns>
    internal static PluginBase[] AvailablePlugins()
    {
      List<PluginBase> plugins = new List<PluginBase>();

      string installFolder = SystemRegistry.GetInstallFolder();
      if (String.IsNullOrEmpty(installFolder))
        return null;

      string path = Path.Combine(installFolder, "IR Server Plugins");
      string[] files = Directory.GetFiles(path, "*.dll", SearchOption.TopDirectoryOnly);

      // TODO: Return a Type[], don't instantiate unless required

      foreach (string file in files)
      {
        try
        {
          Assembly assembly = Assembly.LoadFrom(file);
          Type[] types = assembly.GetExportedTypes();

          foreach (Type type in types)
          {
            if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(PluginBase)))
            {
              PluginBase plugin = (PluginBase)assembly.CreateInstance(type.FullName);

              if (plugin != null)
                plugins.Add(plugin);
            }
          }
        }
        catch (BadImageFormatException) { } // Ignore Bad Image Format Exceptions, just keep checking for Input Service Plugins
        catch (TypeLoadException) { }       // Ignore Type Load Exceptions, just keep checking for Input Service Plugins
        catch (FileNotFoundException) { }   // Ignore File Not Found Exceptions, just keep checking for Input Service Plugins
      }

      return plugins.ToArray();
    }

    /// <summary>
    /// Retreives a plugin instance given the plugin name.
    /// </summary>
    /// <param name="pluginName">Name of plugin to instantiate.</param>
    /// <returns>Plugin instance.</returns>
    internal static PluginBase GetPlugin(string pluginName)
    {
      if (String.IsNullOrEmpty(pluginName))
        throw new ArgumentNullException("pluginName");

      PluginBase[] serverPlugins = AvailablePlugins();
      if (serverPlugins == null)
        throw new FileNotFoundException("No available plugins found");

      foreach (PluginBase plugin in serverPlugins)
        if (plugin.Name.Equals(pluginName, StringComparison.OrdinalIgnoreCase))
          return plugin;

      throw new InvalidOperationException(String.Format("Plugin not found ({0})", pluginName));
    }

    /// <summary>
    /// Retreives a list of detected Receiver plugins.
    /// </summary>
    /// <returns>String array of plugin names.</returns>
    internal static string[] DetectReceivers()
    {
      IrssLog.Info("Detect Receivers ...");

      PluginBase[] plugins = AvailablePlugins();
      if (plugins == null || plugins.Length == 0)
        return null;

      List<string> receivers = new List<string>();

      foreach (PluginBase plugin in plugins)
      {
        try
        {
          if ((plugin is IRemoteReceiver || plugin is IKeyboardReceiver || plugin is IMouseReceiver) && plugin.Detect())
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

      PluginBase[] plugins = Program.AvailablePlugins();
      if (plugins == null || plugins.Length == 0)
        return null;

      List<string> blasters = new List<string>();

      foreach (PluginBase plugin in plugins)
      {
        try
        {
          if (plugin is ITransmitIR && plugin.Detect())
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
