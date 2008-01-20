using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using MediaPortal.GUI.Library;

namespace Commands.MediaPortal
{

  /// <summary>
  /// Reboot macro command.
  /// </summary>
  public class CommandReboot : Command
  {

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandReboot"/> class.
    /// </summary>
    public CommandReboot() { InitParameters(0); }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandReboot"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandReboot(string[] parameters) : base(parameters) { }

    #endregion Constructors

    #region Implementation

    /// <summary>
    /// Gets the category of this command.
    /// </summary>
    /// <returns>The category of this command.</returns>
    public override string GetCategory() { return "MediaPortal Commands"; }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <returns>User interface text.</returns>
    public override string GetUserInterfaceText() { return "Reboot"; }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="variables">The variable list of the calling code.</param>
    public override void Execute(VariableList variables)
    {
      GUIGraphicsContext.OnAction(new Action(Action.ActionType.ACTION_REBOOT, 0, 0));
    }

    #endregion Implementation

  }

}
