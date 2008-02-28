using System;

namespace InputService.Plugin
{

  #region Delegates

  /// <summary>
  /// IR Server callback for mouse event handling.
  /// </summary>
  /// <param name="deviceName">The device that detected the mouse event.</param>
  /// <param name="deltaX">Mouse movement on the X-axis.</param>
  /// <param name="deltaY">Mouse movement on the Y-axis.</param>
  /// <param name="buttons">Which (if any) buttons are pressed?</param>
  public delegate void MouseHandler(string deviceName, int deltaX, int deltaY, int buttons);

  #endregion Delegates

  /// <summary>
  /// Plugins that implement this interface can receive mouse movements and button presses.
  /// </summary>
  public interface IMouseReceiver
  {

    /// <summary>
    /// Callback for mouse events.
    /// </summary>
    /// <value>The mouse callback.</value>
    MouseHandler MouseCallback { get; set; }

  }

}
