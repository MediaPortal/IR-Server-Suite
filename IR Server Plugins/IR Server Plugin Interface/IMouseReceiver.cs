using System;

namespace IRServerPluginInterface
{

  #region Delegates

  /// <summary>
  /// IR Server callback for mouse event handling.
  /// </summary>
  /// <param name="deltaX">Mouse movement on the X-axis.</param>
  /// <param name="deltaY">Mouse movement on the Y-axis.</param>
  /// <param name="rightButton">Is the right button pressed?</param>
  /// <param name="leftButton">Is the left button pressed?</param>
  public delegate void MouseHandler(int deltaX, int deltaY, int buttons);

  #endregion Delegates

  /// <summary>
  /// Plugins that implement this interface can receive mouse movements and button presses.
  /// </summary>
  public interface IMouseReceiver
  {

    /// <summary>
    /// Callback for mouse events.
    /// </summary>
    MouseHandler MouseCallback { get; set; }

  }

}
