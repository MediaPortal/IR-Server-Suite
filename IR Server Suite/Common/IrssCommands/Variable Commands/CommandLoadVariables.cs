using System.Windows.Forms;

namespace IrssCommands
{
  /// <summary>
  /// Load Variables macro command.
  /// </summary>
  public class CommandLoadVariables : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandLoadVariables"/> class.
    /// </summary>
    public CommandLoadVariables()
    {
      InitParameters(1);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandLoadVariables"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandLoadVariables(string[] parameters) : base(parameters)
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
      get { return "Load Variables"; }
    }

    /// <summary>
    /// Gets the edit control to be used within a common edit form.
    /// </summary>
    /// <returns>The edit control.</returns>
    public override BaseCommandConfig GetEditControl()
    {
      return new VariablesFileConfig(Parameters);
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="variables">The variable list of the calling code.</param>
    public override void Execute(VariableList variables)
    {
      string[] processed = ProcessParameters(variables, Parameters);
      variables.VariableLoad(processed[0]);
    }

    #endregion Implementation
  }
}