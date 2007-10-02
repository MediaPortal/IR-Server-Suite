using System;

namespace IRServerPluginInterface
{

  #region Delegates

  /// <summary>
  /// IR Server callback for keyboard key press handling.
  /// </summary>
  /// <param name="vKey">Virtual key code.</param>
  /// <param name="keyUp">.</param>
  public delegate void KeyboardHandler(int vKey, bool keyUp);

  #endregion Delegates

  /// <summary>
  /// Plugins that implement this interface can receive keyboard button presses.
  /// </summary>
  public interface IKeyboardReceiver
  {

    /// <summary>
    /// Callback for keyboard presses.
    /// </summary>
    KeyboardHandler KeyboardCallback { get; set; }

  }

}
