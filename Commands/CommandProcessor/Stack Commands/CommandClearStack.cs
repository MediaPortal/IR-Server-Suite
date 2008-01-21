using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Commands
{

  /// <summary>
  /// Clear Stack stack command.
  /// </summary>
  public class CommandClearStack : Command
  {

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandClearStack"/> class.
    /// </summary>
    public CommandClearStack() { InitParameters(0); }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandClearStack"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandClearStack(string[] parameters) : base(parameters) { }

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
    public override string GetUserInterfaceText() { return "Clear Stack"; }

    /// <summary>
    /// Execute this command.
    /// </summary>
    public override void Execute(VariableList variables)
    {
      variables.StackClear();
    }

    #endregion Implementation

  }

}
