using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Commands
{

  /// <summary>
  /// Set Variable macro command.
  /// </summary>
  public class CommandSetVariable : Command
  {

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandSetVariable"/> class.
    /// </summary>
    public CommandSetVariable() { InitParameters(2); }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandSetVariable"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandSetVariable(string[] parameters) : base(parameters) { }

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
    public override string GetUserInterfaceText() { return "Set Variable"; }

    /// <summary>
    /// Edit this command.
    /// </summary>
    /// <param name="parent">The parent window.</param>
    /// <returns><c>true</c> if the command was modified; otherwise <c>false</c>.</returns>
    public override bool Edit(IWin32Window parent)
    {
      EditSetVariable edit = new EditSetVariable(Parameters);
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
      string value = Parameters[1];
      if (value.StartsWith(VariableList.VariablePrefix, StringComparison.OrdinalIgnoreCase))
        value = variables.VariableGet(value);

      variables.VariableSet(Parameters[0], value);
    }

    #endregion Implementation

  }

}
