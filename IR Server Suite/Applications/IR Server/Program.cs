#region Copyright (C) 2005-2009 Team MediaPortal

// Copyright (C) 2005-2009 Team MediaPortal
// http://www.team-mediaportal.com
// 
// This Program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2, or (at your option)
// any later version.
// 
// This Program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with GNU Make; see the file COPYING.  If not, write to
// the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
// http://www.gnu.org/copyleft/gpl.html

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using InputService.Plugin;
using IrssUtils;

namespace IRServer
{
  internal static class Program
  {
    private static PluginBase[] _plugins;

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
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

      IrssLog.LogLevel = IrssLog.Level.Debug;
      IrssLog.Open("IR Server.log");

      Application.ThreadException += Application_ThreadException;

      // Start Server
      using (IRServer irServer = new IRServer())
      {
        if (irServer.Start())
        {
          Application.Run();
          irServer.Stop();
        }
      }

      Application.ThreadException -= Application_ThreadException;

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
      if (_plugins == null)
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
              if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof (PluginBase)))
              {
                PluginBase plugin = (PluginBase) assembly.CreateInstance(type.FullName);

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
          catch (FileNotFoundException)
          {
            // Ignore File Not Found Exceptions, just keep checking for IR Server Plugins 
          }
        }

        _plugins = plugins.ToArray();
      }

      return _plugins;
//      return plugins.ToArray();
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
    internal static string[] DetectBlasters()
    {
      IrssLog.Info("Detect Blasters ...");

      PluginBase[] plugins = AvailablePlugins();
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
  }
}