using System;
using System.Text;
using System.Xml.Serialization;

namespace Translator
{

  /// <summary>
  /// Stores the relevant information for mapping a remote button to a command.
  /// </summary>
  public class ButtonMapping
  {

    #region Variables

    string _keyCode;
    string _description;
    string _command;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Remote button key code.
    /// </summary>
    [XmlAttribute]
    public string KeyCode
    {
      get { return _keyCode; }
      set { _keyCode = value; }
    }

    /// <summary>
    /// Remote button description.
    /// </summary>
    [XmlAttribute]
    public string Description
    {
      get { return _description; }
      set { _description = value; }
    }

    /// <summary>
    /// Command to execute for the remote button.
    /// </summary>
    [XmlAttribute]
    public string Command
    {
      get { return _command; }
      set { _command = value; }
    }

    #endregion Properties

    #region Constructors

    public ButtonMapping() { }    
    public ButtonMapping(string keyCode, string description, string command)
    {
      _keyCode      = keyCode;
      _description  = description;
      _command      = command;
    }

    #endregion Constructors

  }

}
