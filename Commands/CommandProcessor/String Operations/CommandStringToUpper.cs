using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Commands
{

  /// <summary>
  /// String To Upper macro command.
  /// </summary>
  public class CommandStringToUpper : Command
  {

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandStringToUpper"/> class.
    /// </summary>
    public CommandStringToUpper() { InitParameters(2); }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandStringToUpper"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandStringToUpper(string[] parameters) : base(parameters) { }

    #endregion Constructors

    #region Implementation

    /// <summary>
    /// Gets the category of this command.
    /// </summary>
    /// <returns>The category of this command.</returns>
    public override string GetCategory() { return Processor.CategoryString; }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <returns>User interface text.</returns>
    public override string GetUserInterfaceText() { return "String To Upper"; }

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
      string input = Parameters[0];
      if (input.StartsWith(VariableList.VariablePrefix, StringComparison.OrdinalIgnoreCase))
        input = variables.VariableGet(input);

      string output = input.ToUpper(System.Globalization.CultureInfo.CurrentCulture);

      variables.VariableSet(Parameters[2], output);
    }

    #endregion Implementation

  }

}
