using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using MediaPortal.GUI.Library;

namespace Commands.MediaPortal
{

  /// <summary>
  /// Send Action MediaPortal command.
  /// </summary>
  public class CommandSendAction : Command
  {

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandSendAction"/> class.
    /// </summary>
    public CommandSendAction() { InitParameters(3); }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandSendAction"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandSendAction(string[] parameters) : base(parameters) { }

    #endregion Constructors

    #region Public Methods

    /// <summary>
    /// Gets the category of this command.
    /// </summary>
    /// <returns>The category of this command.</returns>
    public override string GetCategory() { return "MediaPortal Commands"; }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <returns>User interface text.</returns>
    public override string GetUserInterfaceText() { return "Send Action"; }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="variables">The variable list of the calling code.</param>
    public override void Execute(VariableList variables)
    {
      string[] processed = ProcessParameters(variables, Parameters);

      Action.ActionType type = (Action.ActionType)Enum.Parse(typeof(Action.ActionType), processed[0]);
      float f1 = float.Parse(processed[1]);
      float f2 = float.Parse(processed[2]);

      Action action = new Action(type, f1, f2);
      GUIGraphicsContext.OnAction(action);
    }

    /// <summary>
    /// Edit this command.
    /// </summary>
    /// <param name="parent">The parent window.</param>
    /// <returns><c>true</c> if the command was modified; otherwise <c>false</c>.</returns>
    public override bool Edit(IWin32Window parent)
    {
      EditSendAction edit = new EditSendAction(Parameters);
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
