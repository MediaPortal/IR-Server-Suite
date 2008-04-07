using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

using Microsoft.Win32;

using InputService.Plugin;
using IrssUtils;

namespace IRServer
{

  static class Program
  {

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      // Check for multiple instances ...
      try
      {
        if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length != 1)
        {
          MessageBox.Show("IR Server is already running!", "Cannot start", MessageBoxButtons.OK, MessageBoxIcon.Stop);
          return;
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString(), "Error detecting duplicate processes", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

#if DEBUG
      IrssLog.LogLevel = IrssLog.Level.Debug;
#else
      IrssLog.LogLevel = IrssLog.Level.Info;
#endif
      IrssLog.Open("IR Server.log");

      Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

      // Start Server
      using (IRServer irServer = new IRServer())
      {
        if (irServer.Start())
          Application.Run();
      }

      Application.ThreadException -= new ThreadExceptionEventHandler(Application_ThreadException);

      IrssLog.Close();
    }

    /// <summary>
    /// Handles unhandled exceptions.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">Event args.</param>
    public static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
    {
      IrssLog.Error(e.Exception);
    }

    /// <summary>
    /// Retreives a list of available IR Server plugins.
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
        catch (BadImageFormatException) { } // Ignore Bad Image Format Exceptions, just keep checking for IR Server Plugins
        catch (TypeLoadException) { }       // Ignore Type Load Exceptions, just keep checking for IR Server Plugins
        catch (FileNotFoundException) { }   // Ignore File Not Found Exceptions, just keep checking for IR Server Plugins
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
