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
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      // Check for multiple instances
      try
      {
        if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length != 1)
          return;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return;
      }

      // TODO: Change log level to info for release.
      IrssLog.LogLevel = IrssLog.Level.Debug;
      IrssLog.Open(Common.FolderIrssLogs + "IR Server.log");

      Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

      // Start Server
      IRServer irServer = new IRServer();
      
      if (irServer.Start())
        Application.Run();

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
    internal static IIRServerPlugin[] AvailablePlugins()
    {
      try
      {
        List<IIRServerPlugin> plugins = new List<IIRServerPlugin>();

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
              if (type.IsClass && !type.IsAbstract && type.GetInterface(typeof(IIRServerPlugin).Name) == typeof(IIRServerPlugin))
              {
                IIRServerPlugin plugin = (IIRServerPlugin)Activator.CreateInstance(type);
                if (plugin == null)
                  continue;

                plugins.Add(plugin);
              }
            }
          }
          catch (BadImageFormatException)
          {
            // Ignore Bad Image Format Exceptions, just keep checking for IR Server Plugins
          }
          catch (Exception ex)
          {
            MessageBox.Show(ex.ToString(), "IR Server Plugin Error");
          }
        }

        return plugins.ToArray();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        return null;
      }
    }

    /// <summary>
    /// Retreives a plugin instance given the plugin name.
    /// </summary>
    /// <param name="pluginName">Name of plugin to instantiate.</param>
    /// <returns>Plugin instance.</returns>
    internal static IIRServerPlugin GetPlugin(string pluginName)
    {
      if (String.IsNullOrEmpty(pluginName))
        return null;

      IIRServerPlugin[] serverPlugins = AvailablePlugins();
      if (serverPlugins == null)
        return null;

      foreach (IIRServerPlugin plugin in serverPlugins)
        if (plugin.Name.Equals(pluginName, StringComparison.InvariantCultureIgnoreCase))
          return plugin;

      return null;
    }

  }

}
