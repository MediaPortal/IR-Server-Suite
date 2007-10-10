using System;
using System.Runtime.InteropServices;

namespace IrssUtils
{

  /// <summary>
  /// Win32 native method wrapper for Mouse control functions.
  /// </summary>
  public static class Mouse
  {

    #region Interop

    [DllImport("user32")]
    static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, IntPtr dwExtraInfo);

    #endregion Interop

    #region Enumerations

    /// <summary>
    /// Used to simulate mouse actions.
    /// </summary>
    [Flags]
    public enum MouseEvents
    {
      /// <summary>
      /// No Event.
      /// </summary>
      None        = 0x0000,
      /// <summary>
      /// Move.
      /// </summary>
      Move        = 0x0001,
      /// <summary>
      /// Left Button Down.
      /// </summary>
      LeftDown    = 0x0002,
      /// <summary>
      /// Left Button Up.
      /// </summary>
      LeftUp      = 0x0004,
      /// <summary>
      /// Right Button Down.
      /// </summary>
      RightDown   = 0x0008,
      /// <summary>
      /// Right Button Up.
      /// </summary>
      RightUp     = 0x0010,
      /// <summary>
      /// Middle Button Down.
      /// </summary>
      MiddleDown  = 0x0020,
      /// <summary>
      /// Middle Button Up.
      /// </summary>
      MiddleUp    = 0x0040,
      /// <summary>
      /// Scroll.
      /// </summary>
      Scroll      = 0x0800,
      /// <summary>
      /// Position Absolute.
      /// </summary>
      Absolute    = 0x8000
    }

    /// <summary>
    /// Used to simulate mouse wheel scrolling.
    /// </summary>
    public enum ScrollDir
    {
      /// <summary>
      /// No Scroll.
      /// </summary>
      None  =    0,
      /// <summary>
      /// Scroll Up.
      /// </summary>
      Up    =  120,
      /// <summary>
      /// Scroll Down.
      /// </summary>
      Down  = -120
    }

    #endregion Enumerations

    #region Public Methods

    /// <summary>
    /// Simulates mouse button actions.
    /// </summary>
    /// <param name="flags">The button action to simulate.</param>
    public static void Button(MouseEvents flags)
    {
      mouse_event((int)flags, 0, 0, 0, IntPtr.Zero);
    }

    /// <summary>
    /// Simulate mouse movements.
    /// </summary>
    /// <param name="dx">Movement on the X axis.</param>
    /// <param name="dy">Movement on the Y axis.</param>
    /// <param name="absolute">If true, dx and dy are taken as absolute position.  If false, dx and dy are taken as relative to the current position.</param>
    public static void Move(int dx, int dy, bool absolute)
    {
      mouse_event((int)(absolute ? MouseEvents.Move | MouseEvents.Absolute : MouseEvents.Move), dx, dy, 0, IntPtr.Zero);
    }

    /// <summary>
    /// Simulates mouse wheel scrolling.
    /// </summary>
    /// <param name="direction">The direction to scroll.</param>
    public static void Scroll(ScrollDir direction)
    {
      mouse_event((int)MouseEvents.Scroll, 0, 0, (int)direction, IntPtr.Zero);
    }

    #endregion Public Methods

  }

}
