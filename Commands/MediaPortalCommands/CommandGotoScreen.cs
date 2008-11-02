using System;
using System.Windows.Forms;
using MediaPortal.GUI.Library;

namespace Commands.MediaPortal
{
  /// <summary>
  /// Goto Screen MediaPortal command.
  /// </summary>
  public class CommandGotoScreen : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandGotoScreen"/> class.
    /// </summary>
    public CommandGotoScreen()
    {
      InitParameters(1);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandGotoScreen"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandGotoScreen(string[] parameters) : base(parameters)
    {
    }

    #endregion Constructors

    #region Public Methods

    /// <summary>
    /// Gets the category of this command.
    /// </summary>
    /// <returns>The category of this command.</returns>
    public override string GetCategory()
    {
      return "MediaPortal Commands";
    }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <returns>User interface text.</returns>
    public override string GetUserInterfaceText()
    {
      return "Goto Screen";
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="variables">The variable list of the calling code.</param>
    public override void Execute(VariableList variables)
    {
      string[] processed = ProcessParameters(variables, Parameters);

      string windowID = processed[0];

      int window = (int) GUIWindow.Window.WINDOW_INVALID;

      try
      {
        window = (int) Enum.Parse(typeof (GUIWindow.Window), "WINDOW_" + windowID, true);
      }
      catch (ArgumentException)
      {
        // Parsing the window id as a GUIWindow.Window failed, so parse it as an int
      }

      if (window == (int) GUIWindow.Window.WINDOW_INVALID)
        int.TryParse(windowID, out window);

      if (window == (int) GUIWindow.Window.WINDOW_INVALID)
        throw new CommandStructureException(String.Format("Failed to parse Goto screen command window id \"{0}\"",
                                                          windowID));

      GUIGraphicsContext.ResetLastActivity();
      GUIWindowManager.SendThreadMessage(new GUIMessage(GUIMessage.MessageType.GUI_MSG_GOTO_WINDOW, 0, 0, 0, window, 0,
                                                        null));
    }

    /// <summary>
    /// Edit this command.
    /// </summary>
    /// <param name="parent">The parent window.</param>
    /// <returns><c>true</c> if the command was modified; otherwise <c>false</c>.</returns>
    public override bool Edit(IWin32Window parent)
    {
      EditGotoScreen edit = new EditGotoScreen(Parameters[0]);
      if (edit.ShowDialog(parent) == DialogResult.OK)
      {
        Parameters[0] = edit.ScreenID;
        return true;
      }

      return false;
    }

    #endregion Public Methods
  }
}