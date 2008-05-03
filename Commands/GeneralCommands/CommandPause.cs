using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Commands.General
{

  /// <summary>
  /// Pause command.
  /// </summary>
  public class CommandPause : Command
  {

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandPause"/> class.
    /// </summary>
    public CommandPause() { InitParameters(1); }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandPause"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandPause(string[] parameters) : base(parameters) { }

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
    public override string GetUserInterfaceText() { return "Pause"; }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="variables">The variable list of the calling code.</param>
    public override void Execute(VariableList variables)
    {
      string[] processed = ProcessParameters(variables, Parameters);
      int timeout = int.Parse(processed[0]);
      Thread.Sleep(timeout);
    }

    /// <summary>
    /// Edit this command.
    /// </summary>
    /// <param name="parent">The parent window.</param>
    /// <returns><c>true</c> if the command was modified; otherwise <c>false</c>.</returns>
    public override bool Edit(IWin32Window parent)
    {
      EditPause edit = new EditPause(Parameters);
      if (edit.ShowDialog(parent) == DialogResult.OK)
      {
        Parameters = edit.Parameters;
        return true;
      }

      return false;
    }

    #endregion Public Methods

  }

}
