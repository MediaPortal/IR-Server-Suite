using System;
using System.Windows.Forms;

namespace IrssCommands
{
  /// <summary>
  /// Maths Absolute macro command.
  /// </summary>
  public class CommandMathsAbsolute : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandMathsAbsolute"/> class.
    /// </summary>
    public CommandMathsAbsolute()
    {
      InitParameters(2);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandMathsAbsolute"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandMathsAbsolute(string[] parameters) : base(parameters)
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
      get { return Processor.CategoryMaths; }
    }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <value>User interface text.</value>
    public override string UserInterfaceText
    {
      get { return "Maths Absolute"; }
    }

    /// <summary>
    /// Gets the edit control to be used within a common edit form.
    /// </summary>
    /// <returns>The edit control.</returns>
    public override BaseCommandConfig GetEditControl()
    {
      return new MathsStringConfig(Parameters);
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="variables">The variable list of the calling code.</param>
    public override void Execute(VariableList variables)
    {
      string[] processed = ProcessParameters(variables, Parameters);

      int inputInt = 0;
      int.TryParse(processed[0], out inputInt);

      int output = Math.Abs(inputInt);

      variables.VariableSet(processed[1], output.ToString());
    }

    #endregion Implementation
  }
}