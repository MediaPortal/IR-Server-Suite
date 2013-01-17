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
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using IrssUtils;

namespace MediaPortal.Plugins.IRSS.MPControlPlugin
{
  [XmlRoot("mappings")]
  public class EventMappings
  {
    #region Properties

    [XmlAttribute("version")]
    public int Version
    {
      get { return 4; }
      set { return; }
    }

    [XmlElement("event")]
    public List<MappedEvent> Events { get; set; }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EventMappings"/> class.
    /// </summary>
    public EventMappings()
    {
      Events = new List<MappedEvent>();
    }

    #endregion Constructors

    #region Static Methods

    /// <summary>
    /// Save the supplied <see cref="EventMappings"/> to file.
    /// </summary>
    /// <param name="config"><see cref="EventMappings"/> to save.</param>
    /// <param name="fileName">File to save to.</param>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public static bool Save(EventMappings config, string fileName)
    {
      try
      {
        XmlSerializer writer = new XmlSerializer(typeof(EventMappings));
        using (StreamWriter file = new StreamWriter(fileName))
          writer.Serialize(file, config);

        return true;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        return false;
      }
    }

    /// <summary>
    /// Load <see cref="EventMappings"/> from a file.
    /// </summary>
    /// <param name="fileName">File to load from.</param>
    /// <returns>Loaded <see cref="EventMappings"/>.</returns>
    public static EventMappings Load(string fileName)
    {
      EventMappings mappings;
      try
      {
        XmlSerializer reader = new XmlSerializer(typeof(EventMappings));
        using (StreamReader file = new StreamReader(fileName))
          mappings = (EventMappings)reader.Deserialize(file);
      }
      catch (FileNotFoundException)
      {
        IrssLog.Warn("No event mapping file found ({0}), creating new EventMappings", fileName);
        mappings = new EventMappings();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        IrssLog.Warn("Failed to load event mapping file ({0}), creating new EventMappings", fileName);
        mappings = new EventMappings();
      }

      return mappings;
    }

    #endregion Static Methods
  }
}