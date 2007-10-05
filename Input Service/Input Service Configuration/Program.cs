using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

using IRServerPluginInterface;
using IrssUtils;

namespace Configuration
{

  #region Enumerations

  /// <summary>
  /// Describes the operation mode of the IR Server.
  /// </summary>
  public enum IRServerMode
  {
    /// <summary>
    /// Acts as a standard IR Server (Default).
    /// </summary>
    ServerMode = 0,
    /// <summary>
    /// Relays button presses to another IR Server.
    /// </summary>
    RelayMode = 1,
    /// <summary>
    /// Acts as a repeater for another IR Server's IR blasting.
    /// </summary>
    RepeaterMode = 2,
  }

  #endregion Enumerations

  static class Program
  {

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new Config());
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
        Trace.WriteLine("Input Service Configuration: " + ex.ToString());
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
        Trace.WriteLine("Input Service Configuration: " + ex.ToString());
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
        Trace.WriteLine("Input Service Configuration: " + ex.ToString());
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
