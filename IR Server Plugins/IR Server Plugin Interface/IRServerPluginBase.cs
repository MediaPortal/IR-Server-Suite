namespace IRServerPluginInterface
{

  /// <summary>
  /// Base class for all IR Server Plugins.
  /// </summary>
  public abstract class IRServerPluginBase
  {

    #region Properties

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// IR Server plugin version.
    /// </summary>
    public abstract string Version { get; }

    /// <summary>
    /// The IR Server plugin's author.
    /// </summary>
    public abstract string Author { get; }

    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    public abstract string Description { get; }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Detect the presence of this device.  Devices that cannot be detected will always return false.
    /// </summary>
    /// <returns>true if the device is present, otherwise false.</returns>
    public abstract bool Detect();

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    /// <returns>Success.</returns>
    public abstract bool Start();

    /// <summary>
    /// Suspend the IR Server plugin when computer enters standby.
    /// </summary>
    public abstract void Suspend();

    /// <summary>
    /// Resume the IR Server plugin when the computer returns from standby.
    /// </summary>
    public abstract void Resume();

    /// <summary>
    /// Stop the IR Server plugin.
    /// </summary>
    public abstract void Stop();

    #endregion Methods

  }

}
