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
using System.Drawing;
using System.IO;

namespace InputService.Plugin
{
  /// <summary>
  /// Base class for all IR Server Plugins.
  /// </summary>
  public abstract class PluginBase
  {
    #region Constants

    /// <summary>
    /// Plugin configuration file base path.
    /// </summary>
    public static readonly string ConfigurationPath =
      Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                   Path.Combine("IR Server Suite", "Input Service"));

    #endregion Constants

    #region Properties

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public abstract string Name { get; }

    /// <summary>
    /// IR Server plugin version.
    /// </summary>
    /// <value>The version.</value>
    public abstract string Version { get; }

    /// <summary>
    /// The IR Server plugin's author.
    /// </summary>
    /// <value>The author.</value>
    public abstract string Author { get; }

    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    /// <value>The description.</value>
    public abstract string Description { get; }

    /// <summary>
    /// Gets a display icon for the plugin.
    /// </summary>
    /// <value>The icon.</value>
    public virtual Icon DeviceIcon
    {
      get { return null; }
    }

    #endregion Properties

    #region Enums
    ///<summary>
    /// Store the result of Detect() in PluginBase 
    ///</summary>
    public enum DetectionResult
    {
      ///<summary>
      /// Device is not installed
      ///</summary>
      DeviceNotFound,
      ///<summary>
      /// Device is working
      ///</summary>
      DevicePresent,
      ///<summary>
      /// Init call fails
      ///</summary>
      DeviceException
    }
    #endregion

    #region Methods

    /// <summary>
    /// Detect the presence of this device.
    /// </summary>
    public virtual DetectionResult Detect()
    {
      return DetectionResult.DeviceNotFound;
    }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    public abstract void Start();

    /// <summary>
    /// Suspend the IR Server plugin when computer enters standby.
    /// </summary>
    public virtual void Suspend()
    {
    }

    /// <summary>
    /// Resume the IR Server plugin when the computer returns from standby.
    /// </summary>
    public virtual void Resume()
    {
    }

    /// <summary>
    /// Stop the IR Server plugin.
    /// </summary>
    public abstract void Stop();

    #endregion Methods
  }
}