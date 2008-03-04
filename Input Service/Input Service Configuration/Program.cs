using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using InputService.Plugin;
using IrssUtils;

namespace InputService.Configuration
{

  #region Enumerations

  /// <summary>
  /// Describes the operation mode of the Input Service.
  /// </summary>
  enum InputServiceMode
  {
    /// <summary>
    /// Acts as a standard Server (Default).
    /// </summary>
    ServerMode    = 0,
    /// <summary>
    /// Relays button presses to another Input Service.
    /// </summary>
    RelayMode     = 1,
    /// <summary>
    /// Acts as a repeater for another Input Service's blasting.
    /// </summary>
    RepeaterMode  = 2,
  }

  #endregion Enumerations

  static class Program
  {

    #region Constants

    static readonly string ConfigurationFile = Path.Combine(Common.FolderAppData, "Input Service\\Input Service.xml");

    #endregion Constants

    #region Variables

    static bool _abstractRemoteMode;
    static InputServiceMode _mode;
    static string _hostComputer;
    static string[] _pluginNameReceive;
    static string _pluginNameTransmit;

    #endregion Variables

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
          MessageBox.Show("Input Service Configuration is already running!", "Cannot start", MessageBoxButtons.OK, MessageBoxIcon.Stop);
          return;
        }
      }
      catch (Exception ex)
      {
#if TRACE
        Trace.WriteLine(ex.ToString());
#endif
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

      config.Mode           = _mode;
      config.HostComputer   = _hostComputer;
      config.PluginReceive  = _pluginNameReceive;
      config.PluginTransmit = _pluginNameTransmit;

      if (config.ShowDialog() == DialogResult.OK)
      {
        if ((_mode != config.Mode) ||
            (_hostComputer != config.HostComputer) ||
            (_pluginNameReceive != config.PluginReceive) ||
            (_pluginNameTransmit != config.PluginTransmit))
        {

          if (MessageBox.Show("Input Service will now be restarted for configuration changes to take effect", "Restarting Input Service", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
          {
            // Change settings ...
            _mode               = config.Mode;
            _hostComputer       = config.HostComputer;
            _pluginNameReceive  = config.PluginReceive;
            _pluginNameTransmit = config.PluginTransmit;

            SaveSettings();

            // Restart Input Service ...
            RestartService("InputService");
          }
          else
          {
            IrssLog.Info("Canceled settings changes");
          }
        }
      }

      IrssLog.Close();
    }

    static void LoadSettings()
    {
      IrssLog.Info("Loading settings ...");

      _abstractRemoteMode = false;
      _mode               = InputServiceMode.ServerMode;
      _hostComputer       = String.Empty;
      _pluginNameReceive  = null;
      _pluginNameTransmit = String.Empty;

      XmlDocument doc = new XmlDocument();

      try
      {
        doc.Load(ConfigurationFile);
      }
      catch (FileNotFoundException)
      {
        IrssLog.Warn("No configuration file found ({0}), creating default configuration file", ConfigurationFile);

        string[] blasters = DetectBlasters();
        if (blasters == null)
          _pluginNameTransmit = String.Empty;
        else
          _pluginNameTransmit = blasters[0];

        string[] receivers = DetectReceivers();
        if (receivers == null)
          _pluginNameReceive = null;
        else
          _pluginNameReceive = receivers;

        SaveSettings();
        return;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        return;
      }

      try { _abstractRemoteMode = bool.Parse(doc.DocumentElement.Attributes["AbstractRemoteMode"].Value); }
      catch (Exception ex) { IrssLog.Warn(ex.ToString()); }

      try { _mode = (InputServiceMode)Enum.Parse(typeof(InputServiceMode), doc.DocumentElement.Attributes["Mode"].Value, true); }
      catch (Exception ex) { IrssLog.Warn(ex.ToString()); }

      try { _hostComputer = doc.DocumentElement.Attributes["HostComputer"].Value; }
      catch (Exception ex) { IrssLog.Warn(ex.ToString()); }

      try { _pluginNameTransmit = doc.DocumentElement.Attributes["PluginTransmit"].Value; }
      catch (Exception ex) { IrssLog.Warn(ex.ToString()); }

      try
      {
        string receivers = doc.DocumentElement.Attributes["PluginReceive"].Value;
        if (!String.IsNullOrEmpty(receivers))
          _pluginNameReceive = receivers.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
      }
      catch (Exception ex)
      {
        IrssLog.Warn(ex.ToString());
      }
    }
    static void SaveSettings()
    {
      IrssLog.Info("Saving settings ...");

      try
      {
        using (XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, Encoding.UTF8))
        {
          writer.Formatting = Formatting.Indented;
          writer.Indentation = 1;
          writer.IndentChar = (char)9;
          writer.WriteStartDocument(true);
          writer.WriteStartElement("settings"); // <settings>

          writer.WriteAttributeString("AbstractRemoteMode", _abstractRemoteMode.ToString());
          writer.WriteAttributeString("Mode", Enum.GetName(typeof(InputServiceMode), _mode));
          writer.WriteAttributeString("HostComputer", _hostComputer);
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

    static void RestartService(string serviceName)
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
      catch (System.ComponentModel.Win32Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(ex.Message, "Error restarting service", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      catch (System.ServiceProcess.TimeoutException ex)
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
      try
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
                PluginBase plugin = (PluginBase)assembly.CreateInstance(type.FullName);

                if (plugin != null)
                  plugins.Add(plugin);
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

      return null;
    }

    /// <summary>
    /// Retreives a list of detected Receiver plugins.
    /// </summary>
    /// <returns>String array of plugin names.</returns>
    internal static string[] DetectReceivers()
    {
      IrssLog.Info("Detect Receivers ...");

      try
      {
        PluginBase[] plugins = AvailablePlugins();

        List<string> receivers = new List<string>();

        foreach (PluginBase plugin in plugins)
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
      IrssLog.Info("Detect Blasters ...");

      try
      {
        PluginBase[] plugins = Program.AvailablePlugins();

        List<string> blasters = new List<string>();

        foreach (PluginBase plugin in plugins)
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
