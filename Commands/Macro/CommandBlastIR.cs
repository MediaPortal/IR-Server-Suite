using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using IrssUtils;

namespace Commands
{

  /// <summary>
  /// Blast IR macro command.
  /// </summary>
  public class CommandBlastIR : Command
  {

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandBlastIR"/> class.
    /// </summary>
    public CommandBlastIR() { InitParameters(2); }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandBlastIR"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandBlastIR(string[] parameters) : base(parameters) { }

    #endregion Constructors

    #region Implementation

    /// <summary>
    /// Gets the category of this command.
    /// </summary>
    /// <returns>The category of this command.</returns>
    public override string GetCategory() { return Macro.Category; }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <returns>User interface text.</returns>
    public override string GetUserInterfaceText() { return "Blast IR"; }

    /// <summary>
    /// Gets the user display text.
    /// </summary>
    /// <returns>The user display text.</returns>
    public override string GetUserDisplayText()
    {
      return String.Format("{0} ({1})", GetUserInterfaceText(), String.Join(", ", Parameters));
    }
    /*
    public bool Edit(IWin32Window parent, BlastIrDelegate blastIrDelegate, string[] blastPorts)
    {
      EditBlastIR edit = new EditBlastIR(_parameters, blastIrDelegate, blastPorts);
      if (edit.ShowDialog(parent) == DialogResult.OK)
      {
        _parameters = edit.Parameters;
        return true;
      }

      return false;
    }

    public void Execute(BlastIrDelegate blastIrDelegate)
    {
      string irFile = Common.FolderIRCommands + _parameters[0] + Common.FileExtensionIR;
      string port = _parameters[1];

      blastIrDelegate(irFile, port);
    }
    */
    #endregion Implementation

  }

}
