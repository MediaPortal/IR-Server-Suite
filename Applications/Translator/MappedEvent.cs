using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Translator
{

  #region Enumerations

  /// <summary>
  /// A list of events that can be mapped in the Event Mapper
  /// </summary>
  public enum MappingEvent
  {
    None,
    //Time_Schedule,
    //Program_Launch,
    Translator_Start,
    Translator_Quit,
    PC_Shutdown,
    PC_Suspend,
    PC_Resume,
    PC_Logoff,
  }

  #region TODO
  /*
  public enum ScheduleRepeatDelay
  {
    // Repeat
    In_1_Minute,
    In_5_Minutes,
    In_10_Minutes,
    In_15_Minutes,
    In_30_Minutes,
    In_45_Minutes,
    In_1_Hour,
    In_2_Hours,
    In_6_Hours,
    In_12_Hours,
    In_24_Hours,
    In_48_Hours,
    In_7_Days,
    In_14_Days,
    In_21_Days,
    In_28_Days,
    In_60_Days,
    In_365_Days,
      
    // When
    At_Noon,
    At_Midnight,
    At_6_AM,
    At_6_PM,

    // Repeat?
    Weekdays,
    Weekends,
    Mondays,
    Tuesdays,
    Wednesdays,
    Thursdays,
    Fridays,
    Saturdays,
    Sundays,


  }
  */

  #endregion TODO

  #endregion Enumerations

  /// <summary>
  /// MappedEvent class is used to pass events into the Event Mapper feature
  /// </summary>
  public class MappedEvent
  {

    #region Variables

    MappingEvent _eventType;
    string _command;
    
    #endregion Variables

    #region Properties

    /// <summary>
    /// Type of event
    /// </summary>
    [XmlAttribute]
    public MappingEvent EventType
    {
      get { return _eventType; }
      set { _eventType = value; }
    }

    /// <summary>
    /// Command to execute when mapped event occurs
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
    /// Default constructor
    /// </summary>
    public MappedEvent() : this(MappingEvent.None, String.Empty) { }

    /// <summary>
    /// Used to run the Event Mapper
    /// </summary>
    /// <param name="eventType">Event to act on</param>
    /// <param name="command">Command to execute when event occurs</param>
    public MappedEvent(MappingEvent eventType, string command)
    {
      _eventType  = eventType;
      _command    = command;
    }

    #endregion Constructors

  }

}
