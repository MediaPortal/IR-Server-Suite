using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Commands
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
    public CommandSwapVariables() { InitParameters(2); }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandSwapVariables"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandSwapVariables(string[] parameters) : base(parameters) { }

    #endregion Constructors

    #region Implementation

    /// <summary>
    /// Gets the category of this command.
    /// </summary>
    /// <returns>The category of this command.</returns>
    public override string GetCategory() { return Processor.CategoryVariable; }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <returns>User interface text.</returns>
    public override string GetUserInterfaceText() { return "Swap Variables"; }

    /// <summary>
    /// Edit this command.
    /// </summary>
    /// <param name="parent">The parent window.</param>
    /// <returns><c>true</c> if the command was modified; otherwise <c>false</c>.</returns>
    public override bool Edit(IWin32Window parent)
    {
      EditSwapVariables edit = new EditSwapVariables(Parameters);
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

      string value0 = variables.VariableGet(processed[0]);
      string value1 = variables.VariableGet(processed[1]);

      variables.VariableSet(processed[0], value1);
      variables.VariableSet(processed[1], value0);
    }

    #endregion Implementation

  }

}
