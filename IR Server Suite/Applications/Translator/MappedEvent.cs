#region Copyright (C) 2005-2009 Team MediaPortal

// Copyright (C) 2005-2009 Team MediaPortal
// http://www.team-mediaportal.com
// 
// This Program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2, or (at your option)
// any later version.
// 
// This Program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with GNU Make; see the file COPYING.  If not, write to
// the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
// http://www.gnu.org/copyleft/gpl.html

#endregion

using System;
using System.Xml.Serialization;

namespace Translator
{

    #region Enumerations

    /// <summary>
    /// A list of events that can be mapped in the Event Mapper.
    /// </summary>
    public enum MappingEvent
    {
        /// <summary>
        /// No event.
        /// </summary>
        None,
        /// <summary>
        /// Translator started.
        /// </summary>
        Translator_Start,
        /// <summary>
        /// Translator quit.
        /// </summary>
        Translator_Quit,
        /// <summary>
        /// The PC is shutting down.
        /// </summary>
        PC_Shutdown,
        /// <summary>
        /// The PC is suspending.
        /// </summary>
        PC_Suspend,
        /// <summary>
        /// The PC has returned from suspend.
        /// </summary>
        PC_Resume,
        /// <summary>
        /// The user is logging off.
        /// </summary>
        PC_Logoff,

        //ScreenSaver_Start,
        //ScreenSaver_Stop,

        //Scheduled_Event,
    }

    #endregion Enumerations

    /// <summary>
    /// MappedEvent class is used to pass events into the Event Mapper feature.
    /// </summary>
    public class MappedEvent
    {
        #region Variables

        private string _command;
        private string _description;
        private MappingEvent _eventType;

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
        /// Event description
        /// </summary>
        [XmlAttribute]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
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
        public MappedEvent()
            : this(MappingEvent.None, String.Empty)
        {
        }

        /// <summary>
        /// Used to run the Event Mapper.
        /// </summary>
        /// <param name="eventType">Event to act on.</param>
        /// <param name="command">Command to execute when event occurs.</param>
        /// <param name="description">Description for this event/Command association.</param>
        public MappedEvent(MappingEvent eventType, string command, string description = "")
        {
            _eventType = eventType;
            _description = description;
            _command = command;
        }

        #endregion Constructors
    }
}