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
    public virtual Icon DeviceIcon { get { return null; } }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Detect the presence of this device.  Devices that cannot be detected will always return false.
    /// This method should not throw exceptions.
    /// </summary>
    /// <returns><c>true</c> if the device is present, otherwise <c>false</c>.</returns>
    public virtual bool Detect() { return false; }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    public abstract void Start();

    /// <summary>
    /// Suspend the IR Server plugin when computer enters standby.
    /// </summary>
    public virtual void Suspend() { }

    /// <summary>
    /// Resume the IR Server plugin when the computer returns from standby.
    /// </summary>
    public virtual void Resume() { }

    /// <summary>
    /// Stop the IR Server plugin.
    /// </summary>
    public abstract void Stop();

    #endregion Methods

  }

}
