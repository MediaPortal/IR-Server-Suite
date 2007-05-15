using System;
using System.Text;
using System.Xml.Serialization;

namespace Translator
{

  public class ButtonMapping
  {

    #region Variables

    string _keyCode;
    string _description;
    string _command;

    #endregion Variables

    #region Properties

    [XmlAttribute]
    public string KeyCode
    {
      get { return _keyCode; }
      set { _keyCode = value; }
    }

    [XmlAttribute]
    public string Description
    {
      get { return _description; }
      set { _description = value; }
    }

    [XmlAttribute]
    public string Command
    {
      get { return _command; }
      set { _command = value; }
    }

    #endregion Properties

    #region Constructors

    public ButtonMapping() : this(String.Empty, String.Empty, String.Empty) { }    
    public ButtonMapping(string keyCode, string description, string command)
    {
      _keyCode      = keyCode;
      _description  = description;
      _command      = command;
    }

    #endregion Constructors

  }

}
