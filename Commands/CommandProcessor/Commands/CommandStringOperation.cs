using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Commands
{

  /// <summary>
  /// String Operation macro command.
  /// </summary>
  public class CommandStringOperation : Command
  {

    #region String Operations

    internal const string StrOpConcatenate  = "Join";
    internal const string StrOpTrim         = "Trim";
    internal const string StrOpUpper        = "Upper Case";
    internal const string StrOpLower        = "Lower Case";

    // TODO: Add more string operations

    //internal const string StrOpTrimStart = "Trim Start (Input 1)";
    //internal const string StrOpTrimEnd = "Trim End (Input 1)";
    //internal const string StrOpLeft = "Left (Input 1 by Input 2)";
    //internal const string StrOpRight = "Right (Input 1 by Input 2)";

    #endregion String Operations

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandStringOperation"/> class.
    /// </summary>
    public CommandStringOperation() { InitParameters(4); }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandStringOperation"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandStringOperation(string[] parameters) : base(parameters) { }

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
    public override string GetUserInterfaceText() { return "String Operation"; }

    /// <summary>
    /// Edit this command.
    /// </summary>
    /// <param name="parent">The parent window.</param>
    /// <returns><c>true</c> if the command was modified; otherwise <c>false</c>.</returns>
    public override bool Edit(IWin32Window parent)
    {
      EditStringOperation edit = new EditStringOperation(Parameters);
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
      string input1 = Processor.ReplaceSpecial(Parameters[1]);
      string input2 = Processor.ReplaceSpecial(Parameters[2]);

      string output = String.Empty;

      switch (Parameters[0])
      {
        case StrOpConcatenate:    output = input1 + input2;     break;
        case StrOpTrim:           output = input1.Trim();       break;
        case StrOpUpper:          output = input1.ToUpper();    break;
        case StrOpLower:          output = input1.ToLower();    break;

        default:
          throw new CommandStructureException(String.Format("Invalid String Operation: {0}", Parameters[0]));
      }

      variables.SetVariable(Parameters[3], output);
    }

    #endregion Implementation

  }

}
