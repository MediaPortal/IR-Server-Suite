using MediaPortal.GUI.Library;

namespace Commands.MediaPortal
{
  /// <summary>
  /// Exit MediaPortal macro command.
  /// </summary>
  public class CommandExit : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandExit"/> class.
    /// </summary>
    public CommandExit()
    {
      InitParameters(0);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandExit"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandExit(string[] parameters) : base(parameters)
    {
    }

    #endregion Constructors

    #region Implementation

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
      return "Exit MediaPortal";
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="variables">The variable list of the calling code.</param>
    public override void Execute(VariableList variables)
    {
      GUIGraphicsContext.OnAction(new Action(Action.ActionType.ACTION_EXIT, 0, 0));
    }

    #endregion Implementation
  }
}