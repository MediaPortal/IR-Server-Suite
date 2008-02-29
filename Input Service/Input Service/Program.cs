using System;
using System.Collections;
using System.Collections.Generic;
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
      if (args.Length == 1)
      {
        TransactedInstaller transactedInstaller = new TransactedInstaller();
        InputServiceInstaller inputServiceInstaller = new InputServiceInstaller();
        transactedInstaller.Installers.Add(inputServiceInstaller);

        string path = "/assemblypath=" + Assembly.GetExecutingAssembly().Location;
        string[] cmdline = { path };

        InstallContext installContext = new InstallContext(String.Empty, cmdline);
        transactedInstaller.Context = installContext;

        if (args[0].Equals("/install", StringComparison.OrdinalIgnoreCase))
        {
          transactedInstaller.Install(new Hashtable());
        }
        else if (args[0].Equals("/uninstall", StringComparison.OrdinalIgnoreCase))
        {
          transactedInstaller.Uninstall(null);
        }
        else if (args[0].Equals("/start", StringComparison.OrdinalIgnoreCase))
        {
          using (ServiceController serviceController = new ServiceController(ServiceName))
            if (serviceController.Status == ServiceControllerStatus.Stopped)
              serviceController.Start();
        }
        else if (args[0].Equals("/stop", StringComparison.OrdinalIgnoreCase))
        {
          using (ServiceController serviceController = new ServiceController(ServiceName))
            if (serviceController.Status == ServiceControllerStatus.Running)
              serviceController.Stop();
        }
        else if (args[0].Equals("/restart", StringComparison.OrdinalIgnoreCase))
        {
          using (ServiceController serviceController = new ServiceController(ServiceName))
          {
            if (serviceController.Status == ServiceControllerStatus.Running)
              serviceController.Stop();

            serviceController.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 30));

            if (serviceController.Status == ServiceControllerStatus.Stopped)
              serviceController.Start();
          }
        }

        return;
      }

      InputService inputService = new InputService();
      ServiceBase.Run(inputService);
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
              try
              {
                PluginBase plugin = (PluginBase)assembly.CreateInstance(type.FullName);

                if (plugin != null)
                  plugins.Add(plugin);
              }
              catch
              {
                // Ignore this plugin ...
              }
            }
          }
        }
        catch (BadImageFormatException)
        {
          // Ignore Bad Image Format Exceptions, just keep checking for Input Service Plugins
        }
        catch (TypeLoadException)
        {
          // Ignore Type Load Exceptions, just keep checking for Input Service Plugins
        }
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

      throw new ApplicationException(String.Format("Plugin not found ({0})", pluginName));
    }

    /// <summary>
    /// Retreives a list of detected Receiver plugins.
    /// </summary>
    /// <returns>String array of plugin names.</returns>
    internal static string[] DetectReceivers()
    {
      PluginBase[] plugins = AvailablePlugins();

      List<string> receivers = new List<string>();

      foreach (PluginBase plugin in plugins)
        if ((plugin is IRemoteReceiver || plugin is IKeyboardReceiver || plugin is IMouseReceiver) && plugin.Detect())
          receivers.Add(plugin.Name);

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
      PluginBase[] plugins = Program.AvailablePlugins();

      List<string> blasters = new List<string>();

      foreach (PluginBase plugin in plugins)
        if (plugin is ITransmitIR && plugin.Detect())
          blasters.Add(plugin.Name);

      if (blasters.Count > 0)
        return blasters.ToArray();

      return null;
    }

  }

}
