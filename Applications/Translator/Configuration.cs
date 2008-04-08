using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using IrssUtils;

namespace Translator
{

  /// <summary>
  /// Translator configuration.
  /// </summary>
  [XmlRoot]
  public class Configuration
  {

    #region Variables

    string _serverHost;
    string _processPriority;
    bool _hideTrayIcon;

    List<ButtonMapping> _systemWideMappings;
    List<ProgramSettings> _programSettings;
    List<MappedEvent> _mappedEvents;

    #endregion Variables

    #region Properties

    /// <summary>
    /// IR Server host.
    /// </summary>
    /// <value>The server host.</value>
    [XmlElement]
    public string ServerHost
    {
      get { return _serverHost; }
      set { _serverHost = value; }
    }

    /// <summary>
    /// Gets or sets the process priority.
    /// </summary>
    /// <value>The process priority.</value>
    [XmlElement]
    public string ProcessPriority
    {
      get { return _processPriority; }
      set { _processPriority = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to hide the tray icon.
    /// </summary>
    /// <value><c>true</c> to hide the tray icon; otherwise, <c>false</c>.</value>
    [XmlElement]
    public bool HideTrayIcon
    {
      get { return _hideTrayIcon; }
      set { _hideTrayIcon = value; }
    }

    /// <summary>
    /// System wide button mappings.
    /// </summary>
    /// <value>The system wide mappings.</value>
    [XmlArray]
    public List<ButtonMapping> SystemWideMappings
    {
      get { return _systemWideMappings; }
    }

    /// <summary>
    /// Program settings.
    /// </summary>
    /// <value>The programs.</value>
    [XmlArray]
    public List<ProgramSettings> Programs
    {
      get { return _programSettings; }
    }

    /// <summary>
    /// Mapped events.
    /// </summary>
    /// <value>The events.</value>
    [XmlArray]
    public List<MappedEvent> Events
    {
      get { return _mappedEvents; }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Configuration"/> class.
    /// </summary>
    public Configuration()
    {
      _serverHost         = "localhost";
      _processPriority    = "No Change";
      _hideTrayIcon       = false;

      _systemWideMappings = new List<ButtonMapping>();
      _programSettings    = new List<ProgramSettings>();
      _mappedEvents       = new List<MappedEvent>();
    }

    #endregion Constructors

    #region Implementation

    /// <summary>
    /// Imports the specified configuration into this configuration.
    /// </summary>
    /// <param name="config">The configuration to merge in.</param>
    public void Import(Configuration config)
    {
      // TODO: Improve import logic ...

      _mappedEvents.AddRange(config.Events);
      _programSettings.AddRange(config.Programs);
      _systemWideMappings.AddRange(config.SystemWideMappings);
    }

    #endregion Implementation

    #region Static Methods

    /// <summary>
    /// Save the supplied configuration to file.
    /// </summary>
    /// <param name="config">Configuration to save.</param>
    /// <param name="fileName">File to save to.</param>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public static bool Save(Configuration config, string fileName)
    {
      try
      {
        XmlSerializer writer = new XmlSerializer(typeof(Configuration));
        using (StreamWriter file = new StreamWriter(fileName))
          writer.Serialize(file, config);
        
        return true;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        return false;
      }
    }

    /// <summary>
    /// Load a configuration file.
    /// </summary>
    /// <param name="fileName">File to load from.</param>
    /// <returns>Loaded Configuration.</returns>
    public static Configuration Load(string fileName)
    {
      try
      {
        XmlSerializer reader = new XmlSerializer(typeof(Configuration));
        using (StreamReader file = new StreamReader(fileName))
          return (Configuration)reader.Deserialize(file);
      }
      catch (FileNotFoundException)
      {
        IrssLog.Warn("No configuration file found ({0}), creating new configuration", fileName);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }

      return null;
    }

    #endregion Static Methods

  }

}
