using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

//using IrssUtils;

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
    public override string GetCategory() { return Processor.CategorySpecial; }

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
      string fileDir = Path.GetDirectoryName(Parameters[0]);
      if (!fileDir.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.OrdinalIgnoreCase))
        fileDir += Path.DirectorySeparatorChar;

      string fileName = Path.GetFileNameWithoutExtension(Parameters[0]);

      if (fileDir.StartsWith(Common.FolderIRCommands, StringComparison.OrdinalIgnoreCase))
        fileDir = fileDir.Substring(Common.FolderIRCommands.Length);
      else if (fileDir.StartsWith(Common.FolderAppData, StringComparison.OrdinalIgnoreCase))
        fileDir = fileDir.Substring(Common.FolderAppData.Length);

      return String.Format("{0} ({1}, {2})", GetUserInterfaceText(), Path.Combine(fileDir, fileName), Parameters[1]);
    }

    /// <summary>
    /// Edit this command.
    /// </summary>
    /// <param name="parent">The parent window.</param>
    /// <param name="blastIrDelegate">The blast ir delegate.</param>
    /// <param name="blastPorts">The blast ports.</param>
    /// <returns><c>true</c> if the command was modified; otherwise <c>false</c>.</returns>
    public bool Edit(IWin32Window parent, BlastIrDelegate blastIrDelegate, string[] blastPorts)
    {
      EditBlastIR edit = new EditBlastIR(blastIrDelegate, blastPorts, Parameters);
      if (edit.ShowDialog(parent) == DialogResult.OK)
      {
        Parameters = edit.Parameters;
        return true;
      }

      return false;
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="blastIrDelegate">The blast ir delegate.</param>
    public void Execute(BlastIrDelegate blastIrDelegate)
    {
      string irFile = Parameters[0] + Common.FileExtensionIR;
      string port = Parameters[1];

      blastIrDelegate(irFile, port);
    }

    #endregion Implementation

  }

}
