using System;
using System.Collections.Generic;
using System.Text;

using MediaPortal.GUI.Library;

namespace MediaPortal.Plugins
{

  /// <summary>
  /// MappedEvent class is used to pass events into the Event Mapper feature.
  /// </summary>
  public class MappedEvent
  {

    #region Enumerations

    /// <summary>
    /// A list of events that can be mapped in the Event Mapper.
    /// </summary>
    public enum MappingEvent
    {
      None,
      //Time_Schedule,
      MediaPortal_Start,
      MediaPortal_Stop,
      PC_Suspend,
      PC_Resume,
      CD_Inserted,
      CD_Ejected,
      File_Downloaded,
      File_Downloading,
      Focus_Lost,
      MSN_Close_Conversation,
      MSN_Message,
      MSN_Status_Message,
      Play_Audio_CD,
      Play_File,
      Play_Item,
      Play_Radio_Station,
      Playback_Ended,
      Playback_Started,
      Playback_Stopped,
      Record,
      About_To_Record,
      Recorder_Start,
      Recorder_Stop,
      Recorder_Stop_Radio,
      Recorder_Stop_Timeshift,
      Recorder_Stop_TV,
      Recorder_Stop_Viewing,
      Recorder_Tune_Radio,
      Recorder_View_Channel,
      Resume_TV,
      Slideshow_Start,
      File_Stop,
      Switch_Full_Windowed,
      Tune_External_Channel,
      Clicked,
      Item_Selected,
      Go_To_Window,

    }

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

    #endregion Enumerations

    #region Static Methods

    /// <summary>
    /// Translates a supplied GUIMessage.MessageType into an "Event".
    /// </summary>
    /// <param name="messageType">MediaPortal GUIMessage.MessageType.</param>
    /// <returns>MappingEvent equivalent of GUIMessage.MessageType.</returns>
    [CLSCompliant(false)]
    public static MappingEvent GetEventType(GUIMessage.MessageType messageType)
    {
      switch (messageType)
      {
        case GUIMessage.MessageType.GUI_MSG_CD_INSERTED:                          return MappingEvent.CD_Inserted;
        case GUIMessage.MessageType.GUI_MSG_CD_REMOVED:                           return MappingEvent.CD_Ejected;
        case GUIMessage.MessageType.GUI_MSG_FILE_DOWNLOADED:                      return MappingEvent.File_Downloaded;
        case GUIMessage.MessageType.GUI_MSG_FILE_DOWNLOADING:                     return MappingEvent.File_Downloading;
        case GUIMessage.MessageType.GUI_MSG_LOSTFOCUS:                            return MappingEvent.Focus_Lost;
        case GUIMessage.MessageType.GUI_MSG_MSN_CLOSECONVERSATION:                return MappingEvent.MSN_Close_Conversation;
        case GUIMessage.MessageType.GUI_MSG_MSN_MESSAGE:                          return MappingEvent.MSN_Message;
        case GUIMessage.MessageType.GUI_MSG_MSN_STATUS_MESSAGE:                   return MappingEvent.MSN_Status_Message;
        case GUIMessage.MessageType.GUI_MSG_PLAY_AUDIO_CD:                        return MappingEvent.Play_Audio_CD;
        case GUIMessage.MessageType.GUI_MSG_PLAY_FILE:                            return MappingEvent.Play_File;
        case GUIMessage.MessageType.GUI_MSG_PLAY_ITEM:                            return MappingEvent.Play_Item;
        case GUIMessage.MessageType.GUI_MSG_PLAY_RADIO_STATION:                   return MappingEvent.Play_Radio_Station;
        case GUIMessage.MessageType.GUI_MSG_PLAYBACK_ENDED:                       return MappingEvent.Playback_Ended;
        case GUIMessage.MessageType.GUI_MSG_PLAYBACK_STARTED:                     return MappingEvent.Playback_Started;
        case GUIMessage.MessageType.GUI_MSG_PLAYBACK_STOPPED:                     return MappingEvent.Playback_Stopped;
        case GUIMessage.MessageType.GUI_MSG_RECORD:                               return MappingEvent.Record;
        case GUIMessage.MessageType.GUI_MSG_RECORDER_ABOUT_TO_START_RECORDING:    return MappingEvent.About_To_Record;
        case GUIMessage.MessageType.GUI_MSG_RECORDER_START:                       return MappingEvent.Recorder_Start;
        case GUIMessage.MessageType.GUI_MSG_RECORDER_STOP:                        return MappingEvent.Recorder_Stop;
        case GUIMessage.MessageType.GUI_MSG_RECORDER_STOP_RADIO:                  return MappingEvent.Recorder_Stop_Radio;
        case GUIMessage.MessageType.GUI_MSG_RECORDER_STOP_TIMESHIFT:              return MappingEvent.Recorder_Stop_Timeshift;
        case GUIMessage.MessageType.GUI_MSG_RECORDER_STOP_TV:                     return MappingEvent.Recorder_Stop_TV;
        case GUIMessage.MessageType.GUI_MSG_RECORDER_STOP_VIEWING:                return MappingEvent.Recorder_Stop_Viewing;
        case GUIMessage.MessageType.GUI_MSG_RECORDER_TUNE_RADIO:                  return MappingEvent.Recorder_Tune_Radio;
        case GUIMessage.MessageType.GUI_MSG_RECORDER_VIEW_CHANNEL:                return MappingEvent.Recorder_View_Channel;
        case GUIMessage.MessageType.GUI_MSG_RESUME_TV:                            return MappingEvent.Resume_TV;
        case GUIMessage.MessageType.GUI_MSG_START_SLIDESHOW:                      return MappingEvent.Slideshow_Start;
        case GUIMessage.MessageType.GUI_MSG_STOP_FILE:                            return MappingEvent.File_Stop;
        case GUIMessage.MessageType.GUI_MSG_SWITCH_FULL_WINDOWED:                 return MappingEvent.Switch_Full_Windowed;
        case GUIMessage.MessageType.GUI_MSG_TUNE_EXTERNAL_CHANNEL:                return MappingEvent.Tune_External_Channel;
        case GUIMessage.MessageType.GUI_MSG_CLICKED:                              return MappingEvent.Clicked;
        case GUIMessage.MessageType.GUI_MSG_ITEM_SELECTED:                        return MappingEvent.Item_Selected;
        case GUIMessage.MessageType.GUI_MSG_GOTO_WINDOW:                          return MappingEvent.Go_To_Window;

        default:
          return MappingEvent.None;
      }
    }

    /// <summary>
    /// Creates a MappedEvent object from supplied eventString and commandString.
    /// </summary>
    /// <param name="eventString">This string has either the event name by itself or both the event name and a parameter to match.</param>
    /// <param name="commandString">This is the command to execute when the mapped event occurs.</param>
    /// <returns>Returns a MappedEvent object.</returns>
    public static MappedEvent FromStrings(string eventString, string commandString)
    {
      if (eventString == null || commandString == null)
        return null;

      string[] eventStringElements = eventString.Split(new char[] { ',', '=' }, StringSplitOptions.None);

      if (eventStringElements.Length == 1)
      {
        return new MappedEvent(
          (MappingEvent)Enum.Parse(typeof(MappingEvent), eventStringElements[0]),
          commandString);
      }
      else if (eventStringElements.Length == 3)
      {
        return new MappedEvent(
          (MappingEvent)Enum.Parse(typeof(MappingEvent), eventStringElements[0]),
          eventStringElements[1],
          eventStringElements[2],
          commandString);
      }
      else
      {
        return null;
      }
    }

    #endregion Static Methods

    #region Variables

    bool _matchParam;
    MappingEvent _eventType;
    string _param;
    string _paramValue;
    string _command;
    
    #endregion Variables

    #region Properties

    /// <summary>
    /// Is there a parameter to match for this event?
    /// </summary>
    public bool MatchParam
    {
      get { return _matchParam; }
    }

    /// <summary>
    /// Type of event.
    /// </summary>
    public MappingEvent EventType
    {
      get { return _eventType; }
    }

    /// <summary>
    /// Parameter to match.
    /// </summary>
    public string Param
    {
      get { return _param; }
    }

    /// <summary>
    /// Value of the parameter to match.
    /// </summary>
    public string ParamValue
    {
      get { return _paramValue; }
    }

    /// <summary>
    /// Command to execute when mapped event occurs.
    /// </summary>
    public string Command
    {
      get { return _command; }
    }
    
    #endregion Properties

    #region Constructors

    /// <summary>
    /// Used to run the Event Mapper.
    /// </summary>
    /// <param name="eventType">Event to act on.</param>
    /// <param name="command">Command to execute when event occurs.</param>
    public MappedEvent(MappingEvent eventType, string command)
    {
      _matchParam = false;
      _eventType  = eventType;
      _param      = String.Empty;
      _paramValue = String.Empty;
      _command    = command;
    }
    
    /// <summary>
    /// Used to run the Event Mapper.
    /// </summary>
    /// <param name="eventType">Event to act on.</param>
    /// <param name="param">Parameter to match.</param>
    /// <param name="paramValue">Value of parameter to match.</param>
    /// <param name="command">Command to execute when event occurs.</param>
    public MappedEvent(MappingEvent eventType, string param, string paramValue, string command)
    {
      _matchParam = true;
      _eventType  = eventType;
      _param      = param;
      _paramValue = paramValue;
      _command    = command;
    }

    #endregion Constructors

    /*
    public MappedEvent(string schedule, string command)
    {

    }
    */

  }

}
