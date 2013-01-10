namespace IrssCommands
{
  /// <summary>
  /// Clear Variables special command.
  /// </summary>
  public class CommandClearVariables : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandClearVariables"/> class.
    /// </summary>
    public CommandClearVariables()
    {
      InitParameters(0);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandClearVariables"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandClearVariables(string[] parameters) : base(parameters)
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
      get { return Processor.CategoryVariable; }
    }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <value>User interface text.</value>
    public override string UserInterfaceText
    {
      get { return "Clear Variables"; }
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    public override void Execute(VariableList variables)
    {
      variables.VariableClear();
    }

    #endregion Implementation
  }
}