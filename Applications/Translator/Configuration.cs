using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using IrssUtils;

namespace Translator
{
  
  [XmlRoot]
  public class Configuration
  {

    #region Variables

    List<ButtonMapping> _systemWideMappings;
    List<ProgramSettings> _programSettings;
    List<MappedEvent> _mappedEvents;

    #endregion Variables

    #region Properties

    [XmlArray]
    public List<ButtonMapping> SystemWideMappings
    {
      get { return _systemWideMappings; }
      set { _systemWideMappings = value; }
    }

    [XmlArray]
    public List<ProgramSettings> Programs
    {
      get { return _programSettings; }
      set { _programSettings = value; }
    }

    [XmlArray]
    public List<MappedEvent> Events
    {
      get { return _mappedEvents; }
      set { _mappedEvents = value; }
    }

    #endregion Properties

    #region Constructors

    public Configuration()
    {
      _systemWideMappings = new List<ButtonMapping>();
      _programSettings = new List<ProgramSettings>();
      _mappedEvents = new List<MappedEvent>();
    }

    #endregion Constructors

    #region Methods

    /// <summary>
    /// Save the supplied configuration to file.
    /// </summary>
    /// <param name="config">Configuration to save.</param>
    /// <param name="fileName">File to save to.</param>
    /// <returns>Success.</returns>
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
        IrssLog.Error(ex.Message);
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
      catch (Exception ex)
      {
        IrssLog.Error(ex.Message);
        return null;
      }
    }

    #endregion Methods

  }

}
