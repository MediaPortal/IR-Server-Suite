namespace Commands.General
{
  /// <summary>
  /// Shutdown macro command.
  /// </summary>
  public class CommandShutdown : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandShutdown"/> class.
    /// </summary>
    public CommandShutdown()
    {
      InitParameters(0);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandShutdown"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandShutdown(string[] parameters) : base(parameters)
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
      return "General Commands";
    }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <returns>User interface text.</returns>
    public override string GetUserInterfaceText()
    {
      return "Shutdown";
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="variables">The variable list of the calling code.</param>
    public override void Execute(VariableList variables)
    {
      if (!Win32.WindowsExit(Win32.ExitWindows.ShutDown, Win32.ShutdownReasons.FlagUserDefined))
        throw new CommandExecutionException("Shutdown command refused");
    }

    #endregion Implementation
  }
}