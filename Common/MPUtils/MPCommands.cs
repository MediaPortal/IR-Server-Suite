using System;
using System.Collections.Generic;
using System.Text;

using MediaPortal.Configuration;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
using MediaPortal.Player;
using MediaPortal.Util;

namespace MPUtils
{

  /// <summary>
  /// Contains MediaPortal commands.
  /// </summary>
  public static class MPCommands
  {

    /// <summary>
    /// Pop up a dialog in MediaPortal.
    /// </summary>
    /// <param name="heading">Dialog heading text.</param>
    /// <param name="text">Dialog body text.</param>
    /// <param name="timeout">Dialog timeout in seconds, zero for no timeout.</param>
    public static void ShowNotifyDialog(string heading, string text, int timeout)
    {
      GUIDialogNotify dlgNotify = (GUIDialogNotify)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_NOTIFY);
      if (dlgNotify == null)
        throw new ApplicationException("Failed to create GUIDialogNotify");

      dlgNotify.Reset();
      dlgNotify.ClearAll();
      dlgNotify.SetHeading(heading);
      dlgNotify.SetText(text);
      dlgNotify.TimeOut = timeout;
      dlgNotify.DoModal(GUIWindowManager.ActiveWindow);
    }

    /// <summary>
    /// Takes a MediaPortal window name or window number and activates it.
    /// </summary>
    /// <param name="screen">MediaPortal window name or number.</param>
    /// <param name="useBasicHome">Use the basic home screen when home is requested.</param>
    public static void ProcessGoTo(string screen, bool useBasicHome)
    {
      if (String.IsNullOrEmpty(screen))
        throw new ArgumentNullException("screen");

      int window = (int)GUIWindow.Window.WINDOW_INVALID;

      try
      {
        window = (int)Enum.Parse(typeof(GUIWindow.Window), "WINDOW_" + screen, true);
      }
      catch
      {
        // Parsing the window id as a GUIWindow.Window failed, so parse it as an int
      }

      try
      {
        if (window == (int)GUIWindow.Window.WINDOW_INVALID)
          window = Convert.ToInt32(screen);
      }
      catch
      {
        // Parsing the window id as an int failed, give up.
      }

      if (window == (int)GUIWindow.Window.WINDOW_INVALID)
        throw new ArgumentException(String.Format("Failed to parse Goto command window id \"{0}\"", screen), "screen");

      if (window == (int)GUIWindow.Window.WINDOW_HOME && useBasicHome)
        window = (int)GUIWindow.Window.WINDOW_SECOND_HOME;

      GUIGraphicsContext.ResetLastActivity();
      GUIWindowManager.SendThreadMessage(new GUIMessage(GUIMessage.MessageType.GUI_MSG_GOTO_WINDOW, 0, 0, 0, window, 0, null));
    }

  }

}
