using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;

namespace Translator
{
 
  /// <summary>
  /// Holds settings for a program that is mapped in Translator.
  /// </summary>
  public class ProgramSettings
  {

    #region Variables

    string _name;
    string _filename;
    string _folder;
    string _arguments;
    bool _useShellExecute;
    bool _ignoreSystemWide;
    ProcessWindowStyle _windowState;
    List<ButtonMapping> _buttonMappings;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Program name.
    /// </summary>
    [XmlAttribute]
    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }
    
    /// <summary>
    /// Program filename.
    /// </summary>
    [XmlAttribute]  
    public string Filename
    {
      get { return _filename; }
      set { _filename = value; }
    }

    /// <summary>
    /// Program start folder.
    /// </summary>
    [XmlAttribute]
    public string Folder
    {
      get { return _folder; }
      set { _folder = value; }
    }

    /// <summary>
    /// Program launch command line arguments.
    /// </summary>
    [XmlAttribute]
    public string Arguments
    {
      get { return _arguments; }
      set { _arguments = value; }
    }

    /// <summary>
    /// Launch using shell execute.
    /// </summary>
    [XmlAttribute]
    public bool UseShellExecute
    {
      get { return _useShellExecute; }
      set { _useShellExecute = value; }
    }

    /// <summary>
    /// Ignore system-wide Translator button mappings
    /// </summary>
    [XmlAttribute]
    public bool IgnoreSystemWide
    {
      get { return _ignoreSystemWide; }
      set { _ignoreSystemWide = value; }
    }

    /// <summary>
    /// Startup window state.
    /// </summary>
    [XmlAttribute]
    public ProcessWindowStyle WindowState
    {
      get { return _windowState; }
      set { _windowState = value; }
    }
    
    /// <summary>
    /// List of button mappings associated with this program.
    /// </summary>
    [XmlArray]      
    public List<ButtonMapping> ButtonMappings
    {
      get { return _buttonMappings; }
      set { _buttonMappings = value; }
    }

    #endregion Properties

    #region Constructors

    public ProgramSettings()
    {
      _name             = "New Program";
      _filename         = String.Empty;
      _folder           = String.Empty;
      _arguments        = String.Empty;
      _useShellExecute  = false;
      _ignoreSystemWide = false;
      _windowState      = ProcessWindowStyle.Normal;
      _buttonMappings   = new List<ButtonMapping>();
    }
    
    #endregion Constructors

    #region Members

    public void Launch()
    {
      Process process                     = new Process();
      process.StartInfo.FileName          = _filename;
      process.StartInfo.WorkingDirectory  = _folder;
      process.StartInfo.Arguments         = _arguments;
      process.StartInfo.WindowStyle       = _windowState;
      //process.StartInfo.CreateNoWindow  =
      process.StartInfo.UseShellExecute   = _useShellExecute;
      process.Start();
    }

    #endregion Members
  }

}
