using System.Windows.Forms;

namespace IrssCommands
{
  /// <summary>
  /// Maths Divide macro command.
  /// </summary>
  public class CommandMathsDivide : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandMathsDivide"/> class.
    /// </summary>
    public CommandMathsDivide()
    {
      InitParameters(3);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandMathsDivide"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandMathsDivide(string[] parameters) : base(parameters)
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
      get { return "Maths Divide"; }
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

      int input0Int = 0;
      int.TryParse(processed[0], out input0Int);

      int input1Int = 0;
      int.TryParse(processed[1], out input1Int);

      int output = input0Int/input1Int;

      variables.VariableSet(processed[2], output.ToString());
    }

    #endregion Implementation
  }
}