using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Commands
{

  /// <summary>
  /// Push Stack stack command.
  /// </summary>
  public class CommandPushStack : Command
  {

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandPushStack"/> class.
    /// </summary>
    public CommandPushStack() { InitParameters(1); }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandPushStack"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandPushStack(string[] parameters) : base(parameters) { }

    #endregion Constructors

    #region Implementation

    /// <summary>
    /// Gets the category of this command.
    /// </summary>
    /// <returns>The category of this command.</returns>
    public override string GetCategory() { return Processor.CategoryStack; }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <returns>User interface text.</returns>
    public override string GetUserInterfaceText() { return "Push Stack"; }

    /// <summary>
    /// Edit this command.
    /// </summary>
    /// <param name="parent">The parent window.</param>
    /// <returns><c>true</c> if the command was modified; otherwise <c>false</c>.</returns>
    public override bool Edit(IWin32Window parent)
    {
      EditStackFile edit = new EditStackFile(Parameters[0]);
      if (edit.ShowDialog(parent) == DialogResult.OK)
      {
        Parameters[0] = edit.FileName;
        return true;
      }

      return false;
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    public override void Execute(VariableList variables)
    {
      string value = Parameters[0];

      if (value.StartsWith(VariableList.VariablePrefix, StringComparison.OrdinalIgnoreCase))
        value = variables.VariableGet(value.Substring(VariableList.VariablePrefix.Length));

      variables.StackPush(value);
    }

    #endregion Implementation

  }

}
