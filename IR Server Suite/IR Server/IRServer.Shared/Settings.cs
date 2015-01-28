using System;
using System.IO;
using System.Text;
using System.Xml;
using IrssUtils;

namespace IRServer
{
  /// <summary>
  /// Handles settings used by <see cref="IRServer"/> or it's configuration.
  /// </summary>
  public static class Settings
  {
    /// <summary>
    /// Contains the full path to the XML-file, where the settings are saved.
    /// </summary>
    private static readonly string ConfigurationFile = Path.Combine(Common.FolderAppData, @"IR Server\IR Server.xml");

    public static bool AbstractRemoteMode { get; set; }
    public static IRServerMode Mode { get; set; }
    public static string HostComputer { get; set; }
    public static string ProcessPriority { get; set; }
    public static string LogVerbosity { get; set; }
    public static string[] PluginNameReceive { get; set; }
    public static string PluginNameTransmit { get; set; }

    /// <summary>
    /// Loads settings from XML-File to <see cref="Settings"/> class.
    /// </summary>
    public static void LoadSettings()
    {
      IrssLog.Info("Loading settings ...");

      AbstractRemoteMode = true;
      Mode = IRServerMode.ServerMode;
      HostComputer = String.Empty;
      ProcessPriority = "No Change";
      LogVerbosity = "Off";
      PluginNameReceive = null;
      PluginNameTransmit = String.Empty;

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
        AbstractRemoteMode = bool.Parse(doc.DocumentElement.Attributes["AbstractRemoteMode"].Value);
      }
      catch (Exception ex)
      {
        IrssLog.Warn(ex.ToString());
      }

      try
      {
        Mode =
          (IRServerMode)Enum.Parse(typeof(IRServerMode), doc.DocumentElement.Attributes["Mode"].Value, true);
      }
      catch (Exception ex)
      {
        IrssLog.Warn(ex.ToString());
      }

      try
      {
        HostComputer = doc.DocumentElement.Attributes["HostComputer"].Value;
      }
      catch (Exception ex)
      {
        IrssLog.Warn(ex.ToString());
      }

      try
      {
        ProcessPriority = doc.DocumentElement.Attributes["ProcessPriority"].Value;
      }
      catch (Exception ex)
      {
        IrssLog.Warn(ex.ToString());
      }

      try
      {
          LogVerbosity = doc.DocumentElement.Attributes["LogVerbosity"].Value;
      }
      catch (Exception ex)
      {
          IrssLog.Warn(ex.ToString());
      }

      try
      {
        PluginNameTransmit = doc.DocumentElement.Attributes["PluginTransmit"].Value;
      }
      catch (Exception ex)
      {
        IrssLog.Warn(ex.ToString());
      }

      try
      {
        string receivers = doc.DocumentElement.Attributes["PluginReceive"].Value;
        if (!String.IsNullOrEmpty(receivers))
          PluginNameReceive = receivers.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
      }
      catch (Exception ex)
      {
        IrssLog.Warn(ex.ToString());
      }
    }

    /// <summary>
    /// Saves settings from <see cref="Settings"/> class to XML-file.
    /// </summary>
    public static void SaveSettings()
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

          writer.WriteAttributeString("AbstractRemoteMode", AbstractRemoteMode.ToString());
          writer.WriteAttributeString("Mode", Enum.GetName(typeof(IRServerMode), Mode));
          writer.WriteAttributeString("HostComputer", HostComputer);
          writer.WriteAttributeString("ProcessPriority", ProcessPriority);
          writer.WriteAttributeString("LogVerbosity", LogVerbosity);
          writer.WriteAttributeString("PluginTransmit", PluginNameTransmit);

          if (PluginNameReceive != null)
          {
            StringBuilder receivers = new StringBuilder();
            for (int index = 0; index < PluginNameReceive.Length; index++)
            {
              receivers.Append(PluginNameReceive[index]);

              if (index < PluginNameReceive.Length - 1)
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

    /// <summary>
    /// Loads default values into settings class.
    /// </summary>
    private static void CreateDefaultSettings()
    {
      try
      {
        string[] blasters = Shared.DetectBlasters();
        if (blasters == null || blasters.Length == 0)
          PluginNameTransmit = String.Empty;
        else
          PluginNameTransmit = blasters[0];
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        PluginNameTransmit = String.Empty;
      }

      try
      {
        string[] receivers = Shared.DetectReceivers();
        if (receivers == null || receivers.Length == 0)
          PluginNameReceive = null;
        else
          PluginNameReceive = receivers;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        PluginNameReceive = null;
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
  }
}