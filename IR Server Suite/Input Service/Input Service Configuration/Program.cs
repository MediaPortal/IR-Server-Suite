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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using InputService.Plugin;
using IrssUtils;
using TimeoutException=System.ServiceProcess.TimeoutException;

namespace InputService.Configuration
{

  #region Enumerations

  /// <summary>
  /// Describes the operation mode of the Input Service.
  /// </summary>
  internal enum InputServiceMode
  {
    /// <summary>
    /// Acts as a standard Server (Default).
    /// </summary>
    ServerMode = 0,
    /// <summary>
    /// Relays button presses to another Input Service.
    /// </summary>
    RelayMode = 1,
    /// <summary>
    /// Acts as a repeater for another Input Service's blasting.
    /// </summary>
    RepeaterMode = 2,
  }

  #endregion Enumerations

  internal static class Program
  {
    #region Constants

    internal const string ServiceName = "InputService";

    private static readonly string ConfigurationFile = Path.Combine(Common.FolderAppData,
                                                                    "Input Service\\Input Service.xml");

    #endregion Constants

    #region Variables

    private static bool _abstractRemoteMode;
    private static string _hostComputer;
    private static InputServiceMode _mode;
    private static string[] _pluginNameReceive;
    private static string _pluginNameTransmit;
    private static string _processPriority;

    #endregion Variables

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
          MessageBox.Show("Input Service Configuration is already running!", "Cannot start", MessageBoxButtons.OK,
                          MessageBoxIcon.Stop);
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
      IrssLog.Open("Input Service Configuration.log");

      LoadSettings();

      Config config = new Config();

      config.AbstractRemoteMode = _abstractRemoteMode;
      config.Mode = _mode;
      config.HostComputer = _hostComputer;
      config.ProcessPriority = _processPriority;
      config.PluginReceive = _pluginNameReceive;
      config.PluginTransmit = _pluginNameTransmit;

      if (config.ShowDialog() == DialogResult.OK)
      {
        if ((_abstractRemoteMode != config.AbstractRemoteMode) ||
            (_mode != config.Mode) ||
            (_hostComputer != config.HostComputer) ||
            (_processPriority != config.ProcessPriority) ||
            (_pluginNameReceive != config.PluginReceive) ||
            (_pluginNameTransmit != config.PluginTransmit))
        {
          if (
            MessageBox.Show("Input Service will now be restarted for configuration changes to take effect",
                            "Restarting Input Service", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) ==
            DialogResult.OK)
          {
            // Change settings ...
            _abstractRemoteMode = config.AbstractRemoteMode;
            _mode = config.Mode;
            _hostComputer = config.HostComputer;
            _processPriority = config.ProcessPriority;
            _pluginNameReceive = config.PluginReceive;
            _pluginNameTransmit = config.PluginTransmit;

            SaveSettings();

            // Restart Input Service ...
            RestartService(ServiceName);
          }
          else
          {
            IrssLog.Info("Canceled settings changes");
          }
        }
      }

      IrssLog.Close();
    }

    private static void LoadSettings()
    {
      IrssLog.Info("Loading settings ...");

      _abstractRemoteMode = true;
      _mode = InputServiceMode.ServerMode;
      _hostComputer = String.Empty;
      _processPriority = "No Change";
      _pluginNameReceive = null;
      _pluginNameTransmit = String.Empty;

      XmlDocument doc = new XmlDocument();

      try
      {
        doc.Load(ConfigurationFile);
      }
      catch (DirectoryNotFoundException)
      {
        IrssLog.Error("No configuration file found ({0}), folder not found! Creating default configuration file",
                      ConfigurationFile);

        Directory.CreateDirectory(Path.GetDirectoryName(ConfigurationFile));

        CreateDefaultSettings();
        return;
      }
      catch (FileNotFoundException)
      {
        IrssLog.Warn("No configuration file found ({0}), creating default configuration file", ConfigurationFile);

        CreateDefaultSettings();
        return;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        return;
      }

      try
      {
        _abstractRemoteMode = bool.Parse(doc.DocumentElement.Attributes["AbstractRemoteMode"].Value);
      }
      catch (Exception ex)
      {
        IrssLog.Warn(ex.ToString());
      }

      try
      {
        _mode =
          (InputServiceMode) Enum.Parse(typeof (InputServiceMode), doc.DocumentElement.Attributes["Mode"].Value, true);
      }
      catch (Exception ex)
      {
        IrssLog.Warn(ex.ToString());
      }

      try
      {
        _hostComputer = doc.DocumentElement.Attributes["HostComputer"].Value;
      }
      catch (Exception ex)
      {
        IrssLog.Warn(ex.ToString());
      }

      try
      {
        _processPriority = doc.DocumentElement.Attributes["ProcessPriority"].Value;
      }
      catch (Exception ex)
      {
        IrssLog.Warn(ex.ToString());
      }

      try
      {
        _pluginNameTransmit = doc.DocumentElement.Attributes["PluginTransmit"].Value;
      }
      catch (Exception ex)
      {
        IrssLog.Warn(ex.ToString());
      }

      try
      {
        string receivers = doc.DocumentElement.Attributes["PluginReceive"].Value;
        if (!String.IsNullOrEmpty(receivers))
          _pluginNameReceive = receivers.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
      }
      catch (Exception ex)
      {
        IrssLog.Warn(ex.ToString());
      }
    }

    private static void SaveSettings()
    {
      IrssLog.Info("Saving settings ...");

      try
      {
        using (XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, Encoding.UTF8))
        {
          writer.Formatting = Formatting.Indented;
          writer.Indentation = 1;
          writer.IndentChar = (char) 9;
          writer.WriteStartDocument(true);
          writer.WriteStartElement("settings"); // <settings>

          writer.WriteAttributeString("AbstractRemoteMode", _abstractRemoteMode.ToString());
          writer.WriteAttributeString("Mode", Enum.GetName(typeof (InputServiceMode), _mode));
          writer.WriteAttributeString("HostComputer", _hostComputer);
          writer.WriteAttributeString("ProcessPriority", _processPriority);
          writer.WriteAttributeString("PluginTransmit", _pluginNameTransmit);

          if (_pluginNameReceive != null)
          {
            StringBuilder receivers = new StringBuilder();
            for (int index = 0; index < _pluginNameReceive.Length; index++)
            {
              receivers.Append(_pluginNameReceive[index]);

              if (index < _pluginNameReceive.Length - 1)
                receivers.Append(',');
            }
            writer.WriteAttributeString("PluginReceive", receivers.ToString());
          }
          else
          {
            writer.WriteAttributeString("PluginReceive", String.Empty);
          }

          writer.WriteEndElement(); // </settings>
          writer.WriteEndDocument();
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
    }

    private static void CreateDefaultSettings()
    {
      try
      {
        string[] blasters = DetectBlasters();
        if (blasters == null)
          _pluginNameTransmit = String.Empty;
        else
          _pluginNameTransmit = blasters[0];
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        _pluginNameTransmit = String.Empty;
      }

      try
      {
        string[] receivers = DetectReceivers();
        if (receivers == null)
          _pluginNameReceive = null;
        else
          _pluginNameReceive = receivers;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        _pluginNameReceive = null;
      }

      try
      {
        SaveSettings();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
    }

    private static void RestartService(string serviceName)
    {
      IrssLog.Info("Restarting service ({0})", serviceName);

      try
      {
        ServiceController[] services = ServiceController.GetServices();
        foreach (ServiceController service in services)
        {
          if (service.ServiceName.Equals(serviceName, StringComparison.OrdinalIgnoreCase))
          {
            if (service.Status != ServiceControllerStatus.Stopped)
            {
              service.Stop();
              service.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 30));
            }

            service.Start();
          }
        }
      }
      catch (Win32Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(ex.Message, "Error restarting service", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      catch (TimeoutException ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(ex.Message, "Error stopping service", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        } // Ignore Bad Image Format Exceptions, just keep checking for Input Service Plugins
        catch (TypeLoadException)
        {
        } // Ignore Type Load Exceptions, just keep checking for Input Service Plugins
        catch (FileNotFoundException)
        {
        } // Ignore File Not Found Exceptions, just keep checking for Input Service Plugins
      }

      return plugins.ToArray();
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

      PluginBase[] plugins = AvailablePlugins();
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