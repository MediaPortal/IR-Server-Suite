using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using MediaPortal.GUI.Library;

namespace Commands.MediaPortal
{

  /// <summary>
  /// Send Message MediaPortal command.
  /// </summary>
  public class CommandSendMessage : Command
  {

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandSendMessage"/> class.
    /// </summary>
    public CommandSendMessage() { InitParameters(6); }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandSendMessage"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandSendMessage(string[] parameters) : base(parameters) { }

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
    public override string GetUserInterfaceText() { return "Send Message"; }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="variables">The variable list of the calling code.</param>
    public override void Execute(VariableList variables)
    {
      string messageType = Parameters[0];
      if (messageType.StartsWith(VariableList.VariablePrefix, StringComparison.OrdinalIgnoreCase))
        messageType = variables.VariableGet(messageType.Substring(VariableList.VariablePrefix.Length));
      messageType = IrssUtils.Common.ReplaceSpecial(messageType);

      GUIMessage.MessageType type = (GUIMessage.MessageType)Enum.Parse(typeof(GUIMessage.MessageType), messageType);
      int windowId  = int.Parse(Parameters[1]);
      int senderId  = int.Parse(Parameters[2]);
      int controlId = int.Parse(Parameters[3]);
      int param1    = int.Parse(Parameters[4]);
      int param2    = int.Parse(Parameters[5]);

      GUIMessage message = new GUIMessage(type, windowId, senderId, controlId, param1, param2, null);

      GUIGraphicsContext.ResetLastActivity();
      GUIWindowManager.SendThreadMessage(message);
    }

    /// <summary>
    /// Edit this command.
    /// </summary>
    /// <param name="parent">The parent window.</param>
    /// <returns><c>true</c> if the command was modified; otherwise <c>false</c>.</returns>
    public override bool Edit(IWin32Window parent)
    {
      EditSendMessage edit = new EditSendMessage(Parameters);
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
