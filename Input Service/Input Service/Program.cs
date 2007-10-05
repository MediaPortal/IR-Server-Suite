using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Text;

using Microsoft.Win32;

using IRServerPluginInterface;
using IrssUtils;

namespace InputService
{

  static class Program
  {

    #region Constants

    public const string ServiceName         = "MPInputService";
    public const string ServiceDisplayName  = "MediaPortal Input Service";
    public const string ServiceDescription  = "Provides access to input devices";

    #endregion Constants

    static void Main(string[] args)
    {
      if (args.Length >= 1)
      {
        TransactedInstaller transactedInstaller = new TransactedInstaller();
        InputServiceInstaller inputServiceInstaller = new InputServiceInstaller();
        transactedInstaller.Installers.Add(inputServiceInstaller);

        String path = String.Format("/assemblypath={0}", Assembly.GetExecutingAssembly().Location);
        String[] cmdline = { path };

        InstallContext installContext = new InstallContext(String.Empty, cmdline);
        transactedInstaller.Context = installContext;

        if (args[0].Equals("/install", System.StringComparison.InvariantCultureIgnoreCase))
          transactedInstaller.Install(new Hashtable());
        else if (args[0].Equals("/uninstall", System.StringComparison.InvariantCultureIgnoreCase))
          transactedInstaller.Uninstall(null);

        return;
      }

      InputService inputService = new InputService();
      ServiceBase.Run(inputService);
    }

    /// <summary>
    /// Retreives a list of available IR Server plugins.
    /// </summary>
    /// <returns>Array of plugin instances.</returns>
    internal static IRServerPluginBase[] AvailablePlugins()
    {
      try
      {
        List<IRServerPluginBase> plugins = new List<IRServerPluginBase>();

        string installFolder = SystemRegistry.GetInstallFolder();
        if (String.IsNullOrEmpty(installFolder))
          return null;

        string[] files = Directory.GetFiles(installFolder + "\\IR Server Plugins\\", "*.dll", SearchOption.TopDirectoryOnly);

        foreach (string file in files)
        {
          try
          {
            Assembly assembly = Assembly.LoadFrom(file);
            Type[] types = assembly.GetExportedTypes();

            foreach (Type type in types)
            {
              if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(IRServerPluginBase)))
              {
                IRServerPluginBase plugin = (IRServerPluginBase)assembly.CreateInstance(type.FullName);

                if (plugin != null)
                  plugins.Add(plugin);
              }
            }
          }
          catch (BadImageFormatException)
          {
            // Ignore Bad Image Format Exceptions, just keep checking for IR Server Plugins
          }
          catch (TypeLoadException)
          {
            // Ignore Type Load Exceptions, just keep checking for IR Server Plugins
          }
        }

        return plugins.ToArray();
      }
#if TRACE
      catch (Exception ex)
      {
        Trace.WriteLine("IRServer: " + ex.ToString());
#else
      catch
      {
#endif
        return null;
      }
    }

    /// <summary>
    /// Retreives a plugin instance given the plugin name.
    /// </summary>
    /// <param name="pluginName">Name of plugin to instantiate.</param>
    /// <returns>Plugin instance.</returns>
    internal static IRServerPluginBase GetPlugin(string pluginName)
    {
      if (String.IsNullOrEmpty(pluginName))
        throw new ArgumentNullException("pluginName");

      IRServerPluginBase[] serverPlugins = AvailablePlugins();
      if (serverPlugins == null)
        throw new FileNotFoundException("No available plugins found");

      foreach (IRServerPluginBase plugin in serverPlugins)
        if (plugin.Name.Equals(pluginName, StringComparison.InvariantCultureIgnoreCase))
          return plugin;

      return null;
    }

    /// <summary>
    /// Retreives a list of detected Receiver plugins.
    /// </summary>
    /// <returns>String array of plugin names.</returns>
    internal static string[] DetectReceivers()
    {
      try
      {
        IRServerPluginBase[] plugins = AvailablePlugins();

        List<string> receivers = new List<string>();

        foreach (IRServerPluginBase plugin in plugins)
          if ((plugin is IRemoteReceiver || plugin is IKeyboardReceiver || plugin is IMouseReceiver) && plugin.Detect())
            receivers.Add(plugin.Name);

        if (receivers.Count > 0)
          return receivers.ToArray();
      }
#if TRACE
      catch (Exception ex)
      {
        Trace.WriteLine("IRServer: " + ex.ToString());
      }
#else
      catch
      {
      }
#endif

      return null;
    }

    /// <summary>
    /// Retreives a list of detected Blaster plugins.
    /// </summary>
    /// <returns>String array of plugin names.</returns>
    internal static string[] DetectBlasters()
    {
      try
      {
        IRServerPluginBase[] plugins = Program.AvailablePlugins();

        List<string> blasters = new List<string>();

        foreach (IRServerPluginBase plugin in plugins)
          if (plugin is ITransmitIR && plugin.Detect())
            blasters.Add(plugin.Name);

        if (blasters.Count > 0)
          return blasters.ToArray();
      }
#if TRACE
      catch (Exception ex)
      {
        Trace.WriteLine("IRServer: " + ex.ToString());
      }
#else
      catch
      {
      }
#endif

      return null;
    }

  }

}
