using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Commands
{

  /// <summary>
  /// Maths Add macro command.
  /// </summary>
  public class CommandMathsAdd : Command
  {

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandMathsAdd"/> class.
    /// </summary>
    public CommandMathsAdd() { InitParameters(3); }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandMathsAdd"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandMathsAdd(string[] parameters) : base(parameters) { }

    #endregion Constructors

    #region Implementation

    /// <summary>
    /// Gets the category of this command.
    /// </summary>
    /// <returns>The category of this command.</returns>
    public override string GetCategory() { return Processor.CategoryMaths; }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <returns>User interface text.</returns>
    public override string GetUserInterfaceText() { return "Maths Add"; }

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
      string input1 = Parameters[0];
      if (input1.StartsWith(VariableList.VariablePrefix, StringComparison.OrdinalIgnoreCase))
        input1 = variables.VariableGet(input1);
      
      int input1Int = 0;
      int.TryParse(input1, out input1Int);

      string input2 = Parameters[1];
      if (input2.StartsWith(VariableList.VariablePrefix, StringComparison.OrdinalIgnoreCase))
        input2 = variables.VariableGet(input2);

      int input2Int = 0;
      int.TryParse(input2, out input2Int);

      int output = input1Int + input2Int;

      variables.VariableSet(Parameters[2], output.ToString());
    }

    #endregion Implementation

  }

}
