using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

using IrssUtils;

namespace Commands
{

  /// <summary>
  /// Call Macro macro command.
  /// </summary>
  public class CommandCallMacro : Command
  {

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandCallMacro"/> class.
    /// </summary>
    public CommandCallMacro() { InitParameters(1); }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandCallMacro"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandCallMacro(string [] parameters): base(parameters) { }

    #endregion Constructors

    #region Implementation

    /// <summary>
    /// Gets the category of this command.
    /// </summary>
    /// <returns>The category of this command.</returns>
    public override string GetCategory() { return Processor.CategoryHidden; }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <returns>User interface text.</returns>
    public override string GetUserInterfaceText() { return "Macro"; }

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

      if (fileDir.StartsWith(Common.FolderAppData, StringComparison.OrdinalIgnoreCase))
        fileDir = fileDir.Substring(Common.FolderAppData.Length);
      else if (fileDir.StartsWith(Common.FolderAppData, StringComparison.OrdinalIgnoreCase))
        fileDir = fileDir.Substring(Common.FolderAppData.Length);

      return String.Format("{0} ({1})", GetUserInterfaceText(), Path.Combine(fileDir, fileName));
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="commandProcessor">The command processor.</param>
    public void Execute(Processor commandProcessor)
    {
      Macro macro = new Macro(Parameters[0]);
      macro.Execute(commandProcessor);
    }

    /// <summary>
    /// Edit this command.
    /// </summary>
    /// <param name="parent">The parent window.</param>
    /// <returns><c>true</c> if the command was modified; otherwise <c>false</c>.</returns>
    public override bool Edit(IWin32Window parent)
    {
      if (String.IsNullOrEmpty(Parameters[0]))
      {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Macro Files|*" + Processor.FileExtensionMacro;
        if (openFileDialog.ShowDialog(parent) == DialogResult.OK)
          Parameters[0] = openFileDialog.FileName;
      }

      return true;
    }

    #endregion Implementation

  }

}
