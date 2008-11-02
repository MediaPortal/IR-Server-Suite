using System;
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
    public CommandSendMessage()
    {
      InitParameters(6);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandSendMessage"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandSendMessage(string[] parameters) : base(parameters)
    {
    }

    #endregion Constructors

    #region Public Methods

    /// <summary>
    /// Gets the category of this command.
    /// </summary>
    /// <returns>The category of this command.</returns>
    public override string GetCategory()
    {
      return "MediaPortal Commands";
    }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <returns>User interface text.</returns>
    public override string GetUserInterfaceText()
    {
      return "Send Message";
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="variables">The variable list of the calling code.</param>
    public override void Execute(VariableList variables)
    {
      string[] processed = ProcessParameters(variables, Parameters);

      GUIMessage.MessageType type = (GUIMessage.MessageType) Enum.Parse(typeof (GUIMessage.MessageType), processed[0]);
      int windowId = int.Parse(processed[1]);
      int senderId = int.Parse(processed[2]);
      int controlId = int.Parse(processed[3]);
      int param1 = int.Parse(processed[4]);
      int param2 = int.Parse(processed[5]);

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