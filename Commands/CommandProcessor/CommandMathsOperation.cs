using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Commands
{

  /// <summary>
  /// Maths Operation macro command.
  /// </summary>
  public class CommandMathsOperation : Command
  {

    #region Maths Operations

    internal const string MathOpAdd         = "Add";
    internal const string MathOpSubtract    = "Subtract";
    internal const string MathOpMultiply    = "Multiply";
    internal const string MathOpDivide      = "Divide";
    internal const string MathOpModulo      = "Modulo";
    internal const string MathOpPower       = "Power";
    internal const string MathOpAbsolute    = "Absolute";
    internal const string MathOpRoot        = "Square Root";

    // TODO: Add more maths operations

    //internal const string MathOpToBinary    = "To Binary (Input 1)";
    //internal const string MathOpFromBinary  = "From Binary (Input 1)";

    //internal const string MathOpToHex       = "To Hex (Input 1)";
    //internal const string MathOpFromHex     = "From Hex (Input 1)";

    //internal const string MathOpToOctal     = "To Octal (Input 1)";
    //internal const string MathOpFromOctal   = "From Octal (Input 1)";

    #endregion Maths Operations

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandMathsOperation"/> class.
    /// </summary>
    public CommandMathsOperation() { InitParameters(4); }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandMathsOperation"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandMathsOperation(string[] parameters) : base(parameters) { }

    #endregion Constructors

    #region Implementation

    /// <summary>
    /// Gets the category of this command.
    /// </summary>
    /// <returns>The category of this command.</returns>
    public override string GetCategory() { return Processor.CategoryMacro; }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <returns>User interface text.</returns>
    public override string GetUserInterfaceText() { return "Maths Operation"; }

    /// <summary>
    /// Edit this command.
    /// </summary>
    /// <param name="parent">The parent window.</param>
    /// <returns><c>true</c> if the command was modified; otherwise <c>false</c>.</returns>
    public override bool Edit(IWin32Window parent)
    {
      EditMathsOperation edit = new EditMathsOperation(_parameters);
      if (edit.ShowDialog(parent) == DialogResult.OK)
      {
        _parameters = edit.Parameters;
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
      string input1 = Processor.ReplaceSpecial(_parameters[1]);
      int input1Int = 0;
      int.TryParse(input1, out input1Int);

      string input2 = Processor.ReplaceSpecial(_parameters[2]);
      int input2Int = 0;
      int.TryParse(input2, out input2Int);

      int output = 0;

      switch (_parameters[0])
      {
        case MathOpAdd:         output = input1Int + input2Int;                                 break;
        case MathOpDivide:      output = input1Int / input2Int;                                 break;
        case MathOpModulo:      output = input1Int % input2Int;                                 break;
        case MathOpMultiply:    output = input1Int * input2Int;                                 break;
        case MathOpPower:       output = (int)Math.Pow((double)input1Int, (double)input2Int);   break;
        case MathOpSubtract:    output = input1Int - input2Int;                                 break;
        case MathOpAbsolute:    output = Math.Abs(input1Int);                                   break;
        case MathOpRoot:        output = (int)Math.Sqrt((double) input1Int);                    break;

        default:
          throw new CommandStructureException(String.Format("Invalid Maths Operation: {0}", _parameters[0]));
      }

      variables.SetVariable(_parameters[3], output.ToString());
    }

    #endregion Implementation

  }

}
