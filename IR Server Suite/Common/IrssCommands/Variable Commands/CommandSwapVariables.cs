using System.Windows.Forms;

namespace IrssCommands
{
  /// <summary>
  /// Swap Variables macro command.
  /// </summary>
  public class CommandSwapVariables : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandSwapVariables"/> class.
    /// </summary>
    public CommandSwapVariables()
    {
      InitParameters(2);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandSwapVariables"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandSwapVariables(string[] parameters) : base(parameters)
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
      get { return "Swap Variables"; }
    }

    /// <summary>
    /// Gets the edit control to be used within a common edit form.
    /// </summary>
    /// <returns>The edit control.</returns>
    public override BaseCommandConfig GetEditControl()
    {
      return new SwapVariablesConfig(Parameters);
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="variables">The variable list of the calling code.</param>
    public override void Execute(VariableList variables)
    {
      string[] processed = ProcessParameters(variables, Parameters);

      string value0 = variables.VariableGet(processed[0]);
      string value1 = variables.VariableGet(processed[1]);

      variables.VariableSet(processed[0], value1);
      variables.VariableSet(processed[1], value0);
    }

    #endregion Implementation
  }
}