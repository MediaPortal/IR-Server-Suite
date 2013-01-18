using System.Windows.Forms;

namespace IrssCommands
{
  /// <summary>
  /// Set Variable macro command.
  /// </summary>
  public class CommandSetVariable : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandSetVariable"/> class.
    /// </summary>
    public CommandSetVariable()
    {
      InitParameters(2);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandSetVariable"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandSetVariable(string[] parameters) : base(parameters)
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
      get { return "Set Variable"; }
    }

    /// <summary>
    /// Gets the edit control to be used within a common edit form.
    /// </summary>
    /// <returns>The edit control.</returns>
    public override BaseCommandConfig GetEditControl()
    {
      return new SetVariableConfig(Parameters);
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="variables">The variable list of the calling code.</param>
    public override void Execute(VariableList variables)
    {
      string[] processed = ProcessParameters(variables, Parameters);
      variables.VariableSet(processed[0], processed[1]);
    }

    #endregion Implementation
  }
}