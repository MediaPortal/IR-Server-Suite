using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using IrssUtils;

namespace Commands.General
{

  /// <summary>
  /// Keystrokes general command.
  /// </summary>
  public class CommandKeystrokes : Command
  {

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandKeystrokes"/> class.
    /// </summary>
    public CommandKeystrokes() { InitParameters(1); }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandKeystrokes"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandKeystrokes(string[] parameters) : base(parameters) { }

    #endregion Constructors

    #region Public Methods

    /// <summary>
    /// Gets the category of this command.
    /// </summary>
    /// <returns>The category of this command.</returns>
    public override string GetCategory() { return "General Commands"; }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <returns>User interface text.</returns>
    public override string GetUserInterfaceText() { return "Keystrokes"; }

    /// <summary>
    /// Gets the user display text.
    /// </summary>
    /// <returns>The user display text.</returns>
    public override string GetUserDisplayText()
    {
      return String.Format("{0} ({1})", GetUserInterfaceText(), String.Join(", ", Parameters));
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="variables">The variable list of the calling code.</param>
    public override void Execute(VariableList variables)
    {
      Keyboard.ProcessCommand(Parameters[0]);
    }

    /// <summary>
    /// Edit this command.
    /// </summary>
    /// <param name="parent">The parent window.</param>
    /// <returns><c>true</c> if the command was modified; otherwise <c>false</c>.</returns>
    public override bool Edit(IWin32Window parent)
    {
      EditKeystrokes edit = new EditKeystrokes(Parameters[0]);
      if (edit.ShowDialog(parent) == DialogResult.OK)
      {
        Parameters[0] = edit.CommandString;
        return true;
      }

      return false;
    }

    #endregion Public Methods

  }

}
