using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Commands
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
    public CommandMathsAbsolute() { InitParameters(2); }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandMathsAbsolute"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandMathsAbsolute(string[] parameters) : base(parameters) { }

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
    public override string GetUserInterfaceText() { return "Maths Absolute"; }

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
      string input = Parameters[0];
      if (input.StartsWith(VariableList.VariablePrefix, StringComparison.OrdinalIgnoreCase))
        input = variables.VariableGet(input.Substring(VariableList.VariablePrefix.Length));
      input = IrssUtils.Common.ReplaceSpecial(input);
      
      int inputInt = 0;
      int.TryParse(input, out inputInt);

      int output = Math.Abs(inputInt);

      variables.VariableSet(Parameters[1], output.ToString());
    }

    #endregion Implementation

  }

}
