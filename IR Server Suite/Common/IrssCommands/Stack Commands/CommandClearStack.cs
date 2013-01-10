namespace IrssCommands
{
  /// <summary>
  /// Clear Stack stack command.
  /// </summary>
  public class CommandClearStack : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandClearStack"/> class.
    /// </summary>
    public CommandClearStack()
    {
      InitParameters(0);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandClearStack"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandClearStack(string[] parameters) : base(parameters)
    {
    }

    #endregion Constructors

    #region Implementation

    /// <summary>
    /// Gets the category of this command.
    /// </summary>
    /// <value>The category of this command.</value>
    public override string Category
    {
      get { return Processor.CategoryStack; }
    }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <value>User interface text.</value>
    public override string UserInterfaceText
    {
      get { return "Clear Stack"; }
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    public override void Execute(VariableList variables)
    {
      variables.StackClear();
    }

    #endregion Implementation
  }
}