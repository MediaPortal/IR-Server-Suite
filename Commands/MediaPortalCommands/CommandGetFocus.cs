using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using MediaPortal.GUI.Library;

namespace Commands.MediaPortal
{

  /// <summary>
  /// Get Focus MediaPortal macro command.
  /// </summary>
  public class CommandGetFocus : Command
  {

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandGetFocus"/> class.
    /// </summary>
    public CommandGetFocus() { InitParameters(0); }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandGetFocus"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandGetFocus(string[] parameters) : base(parameters) { }

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
    public override string GetUserInterfaceText() { return "Get Focus"; }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="variables">The variable list of the calling code.</param>
    public override void Execute(VariableList variables)
    {
      GUIGraphicsContext.ResetLastActivity();
      GUIMessage msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_GETFOCUS, 0, 0, 0, 0, 0, null);
      GUIWindowManager.SendThreadMessage(msg);
    }

    #endregion Implementation

  }

}
