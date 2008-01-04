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

    #endregion Properties

    #region Methods

    /// <summary>
    /// Detect the presence of this device.  Devices that cannot be detected will always return false.
    /// </summary>
    /// <returns>true if the device is present, otherwise false.</returns>
    public virtual bool Detect() { return false; }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public abstract bool Start();

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
