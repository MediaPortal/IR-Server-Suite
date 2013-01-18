using System.Windows.Forms;

namespace IrssCommands
{
  /// <summary>
  /// Peek Stack stack command.
  /// </summary>
  public class CommandPeekStack : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandPeekStack"/> class.
    /// </summary>
    public CommandPeekStack()
    {
      InitParameters(1);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandPeekStack"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandPeekStack(string[] parameters) : base(parameters)
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
      get { return "Peek Stack"; }
    }

    /// <summary>
    /// Gets the edit control to be used within a common edit form.
    /// </summary>
    /// <returns>The edit control.</returns>
    public override BaseCommandConfig GetEditControl()
    {
      return new StackConfig(Parameters);
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    public override void Execute(VariableList variables)
    {
      string[] processed = ProcessParameters(variables, Parameters);
      variables.StackPeek(processed[0]);
    }

    #endregion Implementation
  }
}