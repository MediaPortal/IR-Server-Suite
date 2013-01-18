using System.Windows.Forms;

namespace IrssCommands
{
  /// <summary>
  /// Save Stack macro command.
  /// </summary>
  public class CommandSaveStack : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandSaveStack"/> class.
    /// </summary>
    public CommandSaveStack()
    {
      InitParameters(1);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandSaveStack"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandSaveStack(string[] parameters) : base(parameters)
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
      get { return "Save Stack"; }
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
    /// <param name="variables">The variable list of the calling code.</param>
    public override void Execute(VariableList variables)
    {
      string[] processed = ProcessParameters(variables, Parameters);
      variables.StackSave(processed[0]);
    }

    #endregion Implementation
  }
}