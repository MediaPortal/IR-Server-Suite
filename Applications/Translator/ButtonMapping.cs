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

    /// <summary>
    /// Initializes a new instance of the <see cref="ButtonMapping"/> class.
    /// </summary>
    public ButtonMapping() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="ButtonMapping"/> class.
    /// </summary>
    /// <param name="keyCode">The remote key code.</param>
    /// <param name="description">The description.</param>
    /// <param name="command">The command to execute for this remote button.</param>
    public ButtonMapping(string keyCode, string description, string command)
    {
      _keyCode      = keyCode;
      _description  = description;
      _command      = command;
    }

    #endregion Constructors

  }

}
