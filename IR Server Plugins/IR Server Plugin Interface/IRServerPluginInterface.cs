namespace IRServerPluginInterface
{

  #region Delegates

  /// <summary>
  /// IR Server callback for remote button press handling.
  /// </summary>
  /// <param name="keyCode">Remote button code.</param>
  public delegate void RemoteHandler(string keyCode);

  /// <summary>
  /// IR Server callback for keyboard key press handling.
  /// </summary>
  /// <param name="vKey">Virtual key code.</param>
  /// <param name="keyUp">.</param>
  public delegate void KeyboardHandler(int vKey, bool keyUp);

  /// <summary>
  /// IR Server callback for mouse event handling.
  /// </summary>
  /// <param name="deltaX">Mouse movement on the X-axis.</param>
  /// <param name="deltaY">Mouse movement on the Y-axis.</param>
  /// <param name="rightButton">Is the right button pressed?</param>
  /// <param name="leftButton">Is the left button pressed?</param>
  public delegate void MouseHandler(int deltaX, int deltaY, int buttons);
  
  #endregion Delegates

  #region Enumerations

  /// <summary>
  /// Provides information about the status of learning an infrared command.
  /// </summary>
  public enum LearnStatus
  {
    /// <summary>
    /// Failed to learn infrared command.
    /// </summary>
    Failure,
    /// <summary>
    /// Succeeded in learning infrared command.
    /// </summary>
    Success,
    /// <summary>
    /// Infrared command learning timed out.
    /// </summary>
    Timeout,
  }

  #endregion Enumerations

  public abstract class IRServerPlugin
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

  public interface IConfigure
  {

    /// <summary>
    /// Configure the IR Server plugin.
    /// </summary>
    void Configure();

  }

  public interface ITransmitIR
  {

    /// <summary>
    /// Lists the available blaster ports.
    /// </summary>
    string[] AvailablePorts { get; }

    /// <summary>
    /// Transmit an infrared command.
    /// </summary>
    /// <param name="file">Infrared command file.</param>
    /// <returns>Success.</returns>
    bool Transmit(string port, byte[] data);

  }

  public interface ILearnIR
  {

    /// <summary>
    /// Learn an infrared command.
    /// </summary>
    /// <param name="data">New infrared command.</param>
    /// <returns>Tells the calling code if the learn was Successful, Failed or Timed Out.</returns>
    LearnStatus Learn(out byte[] data);

  }

  public interface IRemoteReceiver
  {

    /// <summary>
    /// Callback for remote button presses.
    /// </summary>
    RemoteHandler RemoteCallback { get; set; }

  }

  public interface IKeyboardReceiver
  {

    /// <summary>
    /// Callback for keyboard presses.
    /// </summary>
    KeyboardHandler KeyboardCallback { get; set; }

  }

  public interface IMouseReceiver
  {

    /// <summary>
    /// Callback for mouse events.
    /// </summary>
    MouseHandler MouseCallback { get; set; }

  }

}
