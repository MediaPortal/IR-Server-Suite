using System;
using System.Windows.Forms;

namespace IrssCommands
{
  /// <summary>
  /// Maths Square Root macro command.
  /// </summary>
  public class CommandMathsSquareRoot : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandMathsSquareRoot"/> class.
    /// </summary>
    public CommandMathsSquareRoot()
    {
      InitParameters(2);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandMathsSquareRoot"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandMathsSquareRoot(string[] parameters) : base(parameters)
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
      get { return "Maths Square Root"; }
    }

    /// <summary>
    /// Edit this command.
    /// </summary>
    /// <param name="parent">The parent window.</param>
    /// <returns><c>true</c> if the command was modified; otherwise <c>false</c>.</returns>
    public override bool Edit(IWin32Window parent)
    {
      EditMathsOperation edit = new EditMathsOperation(Parameters);
      if (edit.ShowDialog(parent) == DialogResult.OK)
      {
        Parameters = edit.Parameters;
        return true;
      }

      return false;
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

      int output = (int) Math.Sqrt(input0Int);

      variables.VariableSet(processed[1], output.ToString());
    }

    #endregion Implementation
  }
}