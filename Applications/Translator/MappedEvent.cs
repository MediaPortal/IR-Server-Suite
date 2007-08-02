using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Translator
{

  #region Enumerations

  /// <summary>
  /// A list of events that can be mapped in the Event Mapper.
  /// </summary>
  public enum MappingEvent
  {
    None,
    Translator_Start,
    Translator_Quit,
    PC_Shutdown,
    PC_Suspend,
    PC_Resume,
    PC_Logoff,
    Scheduled_Event,
  }

  #endregion Enumerations

  /// <summary>
  /// MappedEvent class is used to pass events into the Event Mapper feature.
  /// </summary>
  public class MappedEvent
  {

    #region Variables

    MappingEvent _eventType;
    string _command;

    //EventSchedule _schedule;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Type of event.
    /// </summary>
    [XmlAttribute]
    public MappingEvent EventType
    {
      get { return _eventType; }
      set { _eventType = value; }
    }

    /// <summary>
    /// Command to execute when mapped event occurs.
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
    /// Default constructor.
    /// </summary>
    public MappedEvent() : this(MappingEvent.None, String.Empty) { }

    /// <summary>
    /// Used to run the Event Mapper.
    /// </summary>
    /// <param name="eventType">Event to act on.</param>
    /// <param name="command">Command to execute when event occurs.</param>
    public MappedEvent(MappingEvent eventType, string command)
    {
      _eventType  = eventType;
      _command    = command;
    }

    #endregion Constructors

  }

}
