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

    List<ButtonMapping> _systemWideMappings;
    List<ProgramSettings> _programSettings;
    List<MappedEvent> _mappedEvents;

    #endregion Variables

    #region Properties

    /// <summary>
    /// IR Server host.
    /// </summary>
    [XmlElement]
    public string ServerHost
    {
      get { return _serverHost; }
      set { _serverHost = value; }
    }

    /// <summary>
    /// System wide button mappings.
    /// </summary>
    [XmlArray]
    public List<ButtonMapping> SystemWideMappings
    {
      get { return _systemWideMappings; }
    }

    /// <summary>
    /// Program settings.
    /// </summary>
    [XmlArray]
    public List<ProgramSettings> Programs
    {
      get { return _programSettings; }
    }

    /// <summary>
    /// Mapped events.
    /// </summary>
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
      _serverHost = "localhost";

      _systemWideMappings = new List<ButtonMapping>();
      _programSettings = new List<ProgramSettings>();
      _mappedEvents = new List<MappedEvent>();
    }

    #endregion Constructors

    #region Static Methods

    /// <summary>
    /// Save the supplied configuration to file.
    /// </summary>
    /// <param name="config">Configuration to save.</param>
    /// <param name="fileName">File to save to.</param>
    /// <returns>true if successful, otherwise false.</returns>
    internal static bool Save(Configuration config, string fileName)
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
        IrssLog.Error(ex.ToString());
        return false;
      }
    }

    /// <summary>
    /// Load a configuration file.
    /// </summary>
    /// <param name="fileName">File to load from.</param>
    /// <returns>Loaded Configuration.</returns>
    internal static Configuration Load(string fileName)
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
        IrssLog.Error(ex.ToString());
      }

      return null;
    }

    #endregion Static Methods

  }

}
