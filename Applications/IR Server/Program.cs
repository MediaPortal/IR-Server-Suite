using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using Microsoft.Win32;

using IRServerPluginInterface;
using IrssUtils;

namespace IRServer
{

  static class Program
  {

    [STAThread]
    static int Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      // Check for multiple instances
      try
      {
        if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length != 1)
          return 1;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return 1;
      }

      // Open log file
      try
      {
        // TODO: Change log level to info for release.
        IrssLog.LogLevel = IrssLog.Level.Debug;
        IrssLog.Open(Common.FolderIrssLogs + "IR Server.log");

        IrssLog.Debug("Platform is {0}", (IntPtr.Size == 4 ? "32-bit" : "64-bit"));
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return 1;
      }

      // Start Server
      IRServer irServer = new IRServer();
      
      if (irServer.Start())
      {
        Application.Run();

        IrssLog.Close();
        return 0;
      }
      else
      {
        MessageBox.Show("Failed to start IR Server, refer to log file for more details.", "IR Server", MessageBoxButtons.OK, MessageBoxIcon.Error);

        IrssLog.Close();
        return 1;
      }

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
            MessageBox.Show(ex.ToString(), "IR Server Unexpected Error");
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
