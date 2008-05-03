using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;

namespace Commands.MediaPortal
{

  /// <summary>
  /// Popup Message command.
  /// </summary>
  public class CommandPopup : Command
  {

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandPopup"/> class.
    /// </summary>
    public CommandPopup() { InitParameters(3); }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandPopup"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandPopup(string[] parameters) : base(parameters) { }

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
    public override string GetUserInterfaceText() { return "Popup Message"; }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="variables">The variable list of the calling code.</param>
    public override void Execute(VariableList variables)
    {
      GUIDialogNotify dlgNotify = (GUIDialogNotify)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_NOTIFY);
      if (dlgNotify == null)
        throw new CommandExecutionException("Failed to create GUIDialogNotify");

      string[] processed = ProcessParameters(variables, Parameters);

      int timeout = int.Parse(processed[2]);

      dlgNotify.Reset();
      dlgNotify.ClearAll();
      dlgNotify.SetHeading(processed[0]);
      dlgNotify.SetText(processed[1]);
      dlgNotify.TimeOut = timeout;

      dlgNotify.DoModal(GUIWindowManager.ActiveWindow);
    }

    /// <summary>
    /// Edit this command.
    /// </summary>
    /// <param name="parent">The parent window.</param>
    /// <returns><c>true</c> if the command was modified; otherwise <c>false</c>.</returns>
    public override bool Edit(IWin32Window parent)
    {
      EditPopup edit = new EditPopup(Parameters);
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
