using System;
using System.IO;
using MediaPortal.Configuration;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
using MediaPortal.Player;
using MediaPortal.Profile;
using MediaPortal.Util;

namespace MPUtils
{
  /// <summary>
  /// Contains common MediaPortal code and data.
  /// </summary>
  public static class MPCommon
  {
    #region Paths

    /// <summary>
    /// Folder for Input Device data default files.
    /// </summary>
    public static readonly string CustomInputDefault = Config.GetFolder(Config.Dir.CustomInputDefault);

    /// <summary>
    /// Folder for Custom Input Device data files.
    /// </summary>
    public static readonly string CustomInputDevice = Config.GetFolder(Config.Dir.CustomInputDevice);

    /// <summary>
    /// Path to the MediaPortal configuration file.
    /// </summary>
    public static readonly string MPConfigFile = Path.Combine(Config.GetFolder(Config.Dir.Config), "MediaPortal.xml");

    #endregion Paths

    #region Methods

    /// <summary>
    /// Pop up a dialog in MediaPortal.
    /// </summary>
    /// <param name="heading">Dialog heading text.</param>
    /// <param name="text">Dialog body text.</param>
    /// <param name="timeout">Dialog timeout in seconds, zero for no timeout.</param>
    public static void ShowNotifyDialog(string heading, string text, int timeout)
    {
      GUIDialogNotify dlgNotify =
        (GUIDialogNotify) GUIWindowManager.GetWindow((int) GUIWindow.Window.WINDOW_DIALOG_NOTIFY);
      if (dlgNotify == null)
        throw new InvalidOperationException("Failed to create GUIDialogNotify");

      dlgNotify.Reset();
      dlgNotify.ClearAll();
      dlgNotify.SetHeading(heading);
      dlgNotify.SetText(text);
      dlgNotify.TimeOut = timeout;
      dlgNotify.DoModal(GUIWindowManager.ActiveWindow);
      // TODO: Put this on a separate thread to allow caller to continue?
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

      int window = (int) GUIWindow.Window.WINDOW_INVALID;

      try
      {
        window = (int) Enum.Parse(typeof (GUIWindow.Window), "WINDOW_" + screen, true);
      }
      catch (ArgumentException)
      {
        // Parsing the window id as a GUIWindow.Window failed, so parse it as an int
      }

      if (window == (int) GUIWindow.Window.WINDOW_INVALID)
        int.TryParse(screen, out window);

      if (window == (int) GUIWindow.Window.WINDOW_INVALID)
        throw new ArgumentException(String.Format("Failed to parse Goto command window id \"{0}\"", screen), "screen");

      if (window == (int) GUIWindow.Window.WINDOW_HOME && useBasicHome)
        window = (int) GUIWindow.Window.WINDOW_SECOND_HOME;

      GUIGraphicsContext.ResetLastActivity();
      GUIWindowManager.SendThreadMessage(new GUIMessage(GUIMessage.MessageType.GUI_MSG_GOTO_WINDOW, 0, 0, 0, window, 0,
                                                        null));
    }

    /// <summary>
    /// Put the computer into Hibernate in a MediaPortal friendly way.
    /// </summary>
    public static void Hibernate()
    {
      bool mpBasicHome = false;
      using (Settings xmlreader = new Settings(MPConfigFile))
        mpBasicHome = xmlreader.GetValueAsBool("general", "startbasichome", false);

      GUIGraphicsContext.ResetLastActivity();
      // Stop all media before hibernating
      g_Player.Stop();

      GUIMessage msg;

      if (mpBasicHome)
        msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_GOTO_WINDOW, 0, 0, 0,
                             (int) GUIWindow.Window.WINDOW_SECOND_HOME, 0, null);
      else
        msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_GOTO_WINDOW, 0, 0, 0, (int) GUIWindow.Window.WINDOW_HOME, 0,
                             null);

      GUIWindowManager.SendThreadMessage(msg);

      WindowsController.ExitWindows(RestartOptions.Hibernate, false);
    }

    /// <summary>
    /// Put the computer into Standby in a MediaPortal friendly way.
    /// </summary>
    public static void Standby()
    {
      bool mpBasicHome;
      using (Settings xmlreader = new Settings(MPConfigFile))
        mpBasicHome = xmlreader.GetValueAsBool("general", "startbasichome", false);

      GUIGraphicsContext.ResetLastActivity();
      // Stop all media before suspending
      g_Player.Stop();

      GUIMessage msg;

      if (mpBasicHome)
        msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_GOTO_WINDOW, 0, 0, 0,
                             (int) GUIWindow.Window.WINDOW_SECOND_HOME, 0, null);
      else
        msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_GOTO_WINDOW, 0, 0, 0, (int) GUIWindow.Window.WINDOW_HOME, 0,
                             null);

      GUIWindowManager.SendThreadMessage(msg);

      WindowsController.ExitWindows(RestartOptions.Suspend, false);
    }

    /// <summary>
    /// Reboot the computer in a MediaPortal friendly way.
    /// </summary>
    public static void Reboot()
    {
      GUIGraphicsContext.OnAction(new Action(Action.ActionType.ACTION_REBOOT, 0, 0));
    }

    /// <summary>
    /// Shut Down the computer in a MediaPortal friendly way.
    /// </summary>
    public static void ShutDown()
    {
      GUIGraphicsContext.OnAction(new Action(Action.ActionType.ACTION_SHUTDOWN, 0, 0));
    }

    /// <summary>
    /// Exits MediaPortal.
    /// </summary>
    public static void ExitMP()
    {
      GUIGraphicsContext.OnAction(new Action(Action.ActionType.ACTION_EXIT, 0, 0));
    }

    /// <summary>
    /// Send a MediaPortal action.
    /// </summary>
    /// <param name="command">The command.</param>
    public static void ProcessSendMediaPortalAction(string[] command)
    {
      Action.ActionType type = (Action.ActionType) Enum.Parse(typeof (Action.ActionType), command[0], true);
      float f1 = float.Parse(command[1]);
      float f2 = float.Parse(command[2]);

      Action action = new Action(type, f1, f2);
      GUIGraphicsContext.OnAction(action);
    }

    /// <summary>
    /// Send a MediaPortal message.
    /// </summary>
    /// <param name="command">The command.</param>
    public static void ProcessSendMediaPortalMessage(string[] command)
    {
      GUIMessage.MessageType type =
        (GUIMessage.MessageType) Enum.Parse(typeof (GUIMessage.MessageType), command[0], true);
      int windowId = int.Parse(command[1]);
      int senderId = int.Parse(command[2]);
      int controlId = int.Parse(command[3]);
      int param1 = int.Parse(command[4]);
      int param2 = int.Parse(command[5]);

      GUIMessage message = new GUIMessage(type, windowId, senderId, controlId, param1, param2, null);

      GUIGraphicsContext.ResetLastActivity();
      GUIWindowManager.SendThreadMessage(message);
    }

    #endregion Methods
  }
}