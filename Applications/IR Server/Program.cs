using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

using Microsoft.Win32;

using IRServerPluginInterface;
using IrssUtils;

namespace IRServer
{

  static class Program
  {

    [STAThread]
    static void Main()
    {
      // Check for multiple instances ...
      try
      {
        if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length != 1)
          return;
      }
#if TRACE
      catch (Exception ex)
      {
        Trace.WriteLine(ex.ToString());
#else
      catch
      {
#endif
        return;
      }

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      // TODO: Change log level to info for release.
      IrssLog.LogLevel = IrssLog.Level.Debug;
      IrssLog.Open(Common.FolderIrssLogs + "IR Server.log");

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
      IrssLog.Error(e.Exception.ToString());
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
          catch (Exception ex)
          {
            MessageBox.Show(ex.ToString(), "IR Server Plugin Error");
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
        if (plugin.Name.Equals(pluginName, StringComparison.OrdinalIgnoreCase))
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
        {
          try
          {
            if ((plugin is IRemoteReceiver || plugin is IKeyboardReceiver || plugin is IMouseReceiver) && plugin.Detect())
              receivers.Add(plugin.Name);
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
        }

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
