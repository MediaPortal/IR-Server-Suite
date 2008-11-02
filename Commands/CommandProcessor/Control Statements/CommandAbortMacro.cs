using System;
using System.Windows.Forms;

namespace Commands
{
  /// <summary>
  /// Abort special command.
  /// </summary>
  public class CommandAbortMacro : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandAbortMacro"/> class.
    /// </summary>
    public CommandAbortMacro()
    {
      InitParameters(1);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandAbortMacro"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandAbortMacro(string[] parameters) : base(parameters)
    {
    }

    #endregion Constructors

    #region Implementation

    /// <summary>
    /// Gets the category of this command.
    /// </summary>
    /// <returns>The category of this command.</returns>
    public override string GetCategory()
    {
      return Processor.CategoryControl;
    }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <returns>User interface text.</returns>
    public override string GetUserInterfaceText()
    {
      return "Abort Macro";
    }

    /// <summary>
    /// Gets the user display text.
    /// </summary>
    /// <returns>The user display text.</returns>
    public override string GetUserDisplayText()
    {
      if (String.IsNullOrEmpty(Parameters[0]))
        return GetUserInterfaceText();
      else
        return String.Format("{0} \"{1}\"", GetUserInterfaceText(), Parameters[0]);
    }

    /// <summary>
    /// Edit this command.
    /// </summary>
    /// <param name="parent">The parent window.</param>
    /// <returns><c>true</c> if the command was modified; otherwise <c>false</c>.</returns>
    public override bool Edit(IWin32Window parent)
    {
      EditAbortMessage edit = new EditAbortMessage(Parameters[0]);
      if (edit.ShowDialog(parent) == DialogResult.OK)
      {
        Parameters[0] = edit.AbortMessage;
        return true;
      }

      return false;
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    public override void Execute(VariableList variables)
    {
      string[] processed = ProcessParameters(variables, Parameters);

      if (String.IsNullOrEmpty(processed[0]))
        throw new MacroExecutionException(GetUserInterfaceText());
      else
        throw new MacroExecutionException(String.Format("{0}: {1}", GetUserInterfaceText(), processed[0]));
    }

    #endregion Implementation
  }
}