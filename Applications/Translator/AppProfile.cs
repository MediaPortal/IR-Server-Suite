using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using IrssUtils;

namespace Translator
{
 
  /// <summary>
  /// Holds profile settings for a program that Translator can load.
  /// </summary>
  [XmlRoot]
  public class AppProfile
  {

    #region Enumerations

    /// <summary>
    /// Detection Method.
    /// </summary>
    public enum DetectionMethod
    {
      /// <summary>
      /// Match the exe name.
      /// </summary>
      Executable,
      /// <summary>
      /// Match the existence of a registry key.
      /// </summary>
      RegistryExist,
      /// <summary>
      /// Match a registry key value.
      /// </summary>
      RegistryValue,
      /// <summary>
      /// Match details of the file.
      /// </summary>
      FileDetails,
    }

    #endregion Enumerations

    #region Variables

    string _name;
    DetectionMethod _matchType;
    string _matchParameters;

    List<ButtonMapping> _buttonMappings;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Profile name.
    /// </summary>
    /// <value>The name.</value>
    [XmlElement]
    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }
    
    /// <summary>
    /// Program file name.
    /// </summary>
    [XmlElement]  
    public DetectionMethod MatchType
    {
      get { return _matchType; }
      set { _matchType = value; }
    }

    /// <summary>
    /// Program start folder.
    /// </summary>
    [XmlElement]
    public string MatchParameters
    {
      get { return _matchParameters; }
      set { _matchParameters = value; }
    }

    /// <summary>
    /// Gets a list of button mappings associated with this program.
    /// </summary>
    [XmlArray]      
    public List<ButtonMapping> ButtonMappings
    {
      get { return _buttonMappings; }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="AppProfile"/> class.
    /// </summary>
    public AppProfile()
    {
      _name             = String.Empty;
      _matchType        = DetectionMethod.Executable;
      _matchParameters  = String.Empty;
      _buttonMappings   = new List<ButtonMapping>();
    }
    
    #endregion Constructors

    #region Static Methods

    /// <summary>
    /// Save the supplied AppProfile to file.
    /// </summary>
    /// <param name="profile">AppProfile to save.</param>
    /// <param name="fileName">File to save to.</param>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    internal static bool Save(AppProfile profile, string fileName)
    {
      try
      {
        XmlSerializer writer = new XmlSerializer(typeof(AppProfile));
        using (StreamWriter file = new StreamWriter(fileName))
          writer.Serialize(file, profile);

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
    internal static AppProfile Load(string fileName)
    {
      try
      {
        XmlSerializer reader = new XmlSerializer(typeof(AppProfile));
        using (StreamReader file = new StreamReader(fileName))
          return (AppProfile)reader.Deserialize(file);
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
