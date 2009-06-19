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

namespace Translator
{
  /// <summary>
  /// Translator configuration.
  /// </summary>
  [XmlRoot]
  public class Configuration
  {
    #region Variables

    private readonly List<MappedEvent> _mappedEvents;
    private readonly List<ProgramSettings> _programSettings;
    private readonly List<ButtonMapping> _systemWideMappings;
    private bool _hideTrayIcon;
    private string _processPriority;
    private string _serverHost;

    #endregion Variables

    #region Properties

    /// <summary>
    /// IR Server host.
    /// </summary>
    /// <value>The server host.</value>
    [XmlElement]
    public string ServerHost
    {
      get { return _serverHost; }
      set { _serverHost = value; }
    }

    /// <summary>
    /// Gets or sets the process priority.
    /// </summary>
    /// <value>The process priority.</value>
    [XmlElement]
    public string ProcessPriority
    {
      get { return _processPriority; }
      set { _processPriority = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to hide the tray icon.
    /// </summary>
    /// <value><c>true</c> to hide the tray icon; otherwise, <c>false</c>.</value>
    [XmlElement]
    public bool HideTrayIcon
    {
      get { return _hideTrayIcon; }
      set { _hideTrayIcon = value; }
    }

    /// <summary>
    /// Gets system wide button mappings.
    /// </summary>
    /// <value>The system wide mappings.</value>
    [XmlArray]
    public List<ButtonMapping> SystemWideMappings
    {
      get { return _systemWideMappings; }
    }

    /// <summary>
    /// Gets program settings.
    /// </summary>
    /// <value>The programs.</value>
    [XmlArray]
    public List<ProgramSettings> Programs
    {
      get { return _programSettings; }
    }

    /// <summary>
    /// Gets mapped events.
    /// </summary>
    /// <value>The events.</value>
    [XmlArray]
    public List<MappedEvent> Events
    {
      get { return _mappedEvents; }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Configuration"/> class.
    /// </summary>
    public Configuration()
    {
      _serverHost = "localhost";
      _processPriority = "No Change";
      _hideTrayIcon = false;

      _systemWideMappings = new List<ButtonMapping>();
      _programSettings = new List<ProgramSettings>();
      _mappedEvents = new List<MappedEvent>();
    }

    #endregion Constructors

    #region Implementation

    /// <summary>
    /// Imports the specified configuration into this configuration.
    /// </summary>
    /// <param name="config">The configuration to merge in.</param>
    public void Import(Configuration config)
    {
      // TODO: Improve import logic ...

      _mappedEvents.AddRange(config.Events);
      _programSettings.AddRange(config.Programs);
      _systemWideMappings.AddRange(config.SystemWideMappings);
    }

    #endregion Implementation

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
        XmlSerializer writer = new XmlSerializer(typeof (Configuration));
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
      try
      {
        XmlSerializer reader = new XmlSerializer(typeof (Configuration));
        using (StreamReader file = new StreamReader(fileName))
          return (Configuration) reader.Deserialize(file);
      }
      catch (FileNotFoundException)
      {
        IrssLog.Warn("No configuration file found ({0}), creating new configuration", fileName);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }

      return null;
    }

    #endregion Static Methods
  }
}