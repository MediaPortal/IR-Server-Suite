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
using MediaPortal.Hardware;

namespace MediaPortal.Plugins.IRSS.MPControlPlugin
{
  /// <summary>
  /// Translator configuration.
  /// </summary>
  [XmlRoot]
  public class Configuration
  {
    #region Properties

    /// <summary>
    /// IR Server host.
    /// </summary>
    /// <value>The server host.</value>
    [XmlElement]
    public string ServerHost { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether MediaPortal will require focus to handle input.
    /// </summary>
    /// <value><c>true</c> if requires focus; otherwise, <c>false</c>.</value>
    [XmlElement]
    public bool RequireFocus { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether multi mapping is enabled.
    /// </summary>
    /// <value><c>true</c> if multi mapping is enabled; otherwise, <c>false</c>.</value>
    [XmlElement]
    public bool MultiMappingEnabled { get; set; }

    /// <summary>
    /// Gets or sets the multi mapping button.
    /// </summary>
    /// <value>The multi mapping button.</value>
    [XmlElement]
    public RemoteButton MultiMappingButton { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the event mapper is enabled.
    /// </summary>
    /// <value><c>true</c> if the event mapper is enabled; otherwise, <c>false</c>.</value>
    [XmlElement]
    public bool EventMapperEnabled { get; set; }

    /// <summary>
    /// Gets or sets the mouse mode button.
    /// </summary>
    /// <value>The mouse mode button.</value>
    [XmlElement]
    public RemoteButton MouseModeButton { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether mouse mode is enabled.
    /// </summary>
    /// <value><c>true</c> if mouse mode is enabled; otherwise, <c>false</c>.</value>
    [XmlElement]
    public bool MouseModeEnabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether mouse mode is active.
    /// </summary>
    /// <value><c>true</c> if mouse mode is active; otherwise, <c>false</c>.</value>
    [XmlElement]
    public bool MouseModeActive { get; set; }

    /// <summary>
    /// Gets or sets the mouse mode step distance.
    /// </summary>
    /// <value>The mouse mode step distance.</value>
    [XmlElement]
    public int MouseModeStep { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether mouse mode acceleration is enabled.
    /// </summary>
    /// <value>
    /// <c>true</c> if mouse mode acceleration is enabled; otherwise, <c>false</c>.
    /// </value>
    [XmlElement]
    public bool MouseModeAcceleration { get; set; }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Configuration"/> class.
    /// </summary>
    public Configuration()
    {
      ServerHost = "localhost";
      RequireFocus = true;

      MultiMappingEnabled = false;
      MultiMappingButton = RemoteButton.Start;

      EventMapperEnabled = false;
      
      MouseModeEnabled = false;
      MouseModeButton = RemoteButton.Teletext;
      MouseModeStep = 10;
      MouseModeAcceleration = true;
    }

    #endregion Constructors

    #region Static Methods

    /// <summary>
    /// Save the supplied configuration to file.
    /// </summary>
    /// <param name="config">Configuration to save.</param>
    /// <param name="fileName">File to save to.</param>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public static bool Save(Configuration config, string fileName)
    {
      try
      {
        XmlSerializer writer = new XmlSerializer(typeof(Configuration));
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
    /// Load a configuration file.
    /// </summary>
    /// <param name="fileName">File to load from.</param>
    /// <returns>Loaded Configuration.</returns>
    public static Configuration Load(string fileName)
    {
      Configuration config;
      try
      {
        XmlSerializer reader = new XmlSerializer(typeof(Configuration));
        using (StreamReader file = new StreamReader(fileName))
          config = (Configuration)reader.Deserialize(file);
      }
      catch (FileNotFoundException)
      {
        IrssLog.Warn("No configuration file found ({0}), creating new configuration", fileName);
        config = new Configuration();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        IrssLog.Warn("Failed to load configuration file ({0}), creating new configuration", fileName);
        config = new Configuration();
      }

      return config;
    }

    #endregion Static Methods
  }
}