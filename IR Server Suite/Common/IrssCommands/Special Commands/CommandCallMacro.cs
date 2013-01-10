using System;
using System.IO;
using System.Windows.Forms;
using IrssUtils;

namespace IrssCommands
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
    public CommandCallMacro()
    {
      InitParameters(1);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandCallMacro"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandCallMacro(string[] parameters) : base(parameters)
    {
    }

    #endregion Constructors

    #region Implementation

    /// <summary>
    /// Gets the category of this command.
    /// </summary>
    /// <value>The category of this command.</value>
    public override string Category
    {
      get { return Processor.CategorySpecial; }
    }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <value>User interface text.</value>
    public override string UserInterfaceText
    {
      get { return "Macro"; }
    }

    /// <summary>
    /// Gets the user display text.
    /// </summary>
    /// <value>The user display text.</value>
    public override string UserDisplayText
    {
      get
      {
        string fileDir = Path.GetDirectoryName(Parameters[0]);
        if (!fileDir.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.OrdinalIgnoreCase))
          fileDir += Path.DirectorySeparatorChar;

        string fileName = Path.GetFileNameWithoutExtension(Parameters[0]);

        if (fileDir.StartsWith(Common.FolderAppData, StringComparison.OrdinalIgnoreCase))
          fileDir = fileDir.Substring(Common.FolderAppData.Length);
        else if (fileDir.StartsWith(Common.FolderAppData, StringComparison.OrdinalIgnoreCase))
          fileDir = fileDir.Substring(Common.FolderAppData.Length);

        return String.Format("{0} ({1})", UserInterfaceText, Path.Combine(fileDir, fileName));
      }
    }

    /// <summary>
    /// Gets the edit control to be used within a common edit form.
    /// </summary>
    /// <returns>The edit control.</returns>
    public override BaseCommandConfig GetEditControl()
    {
      return new CallMacroConfig(Parameters);
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="commandProcessor">The command processor.</param>
    public void Execute(Processor commandProcessor)
    {
      string[] processed = ProcessParameters(commandProcessor.Variables, Parameters);
      Macro macro = new Macro(processed[0]);
      macro.Execute(commandProcessor);
    }

    /// <summary>
    /// Edit this command.
    /// </summary>
    /// <param name="parent">The parent window.</param>
    /// <returns><c>true</c> if the command was modified; otherwise <c>false</c>.</returns>
    public override bool Edit(IWin32Window parent)
    {
      //OpenFileDialog openFileDialog = new OpenFileDialog();
      //openFileDialog.CheckFileExists = true;
      //openFileDialog.Multiselect = false;
      //openFileDialog.Filter = "Macro Files|*" + Processor.FileExtensionMacro;

      //openFileDialog.InitialDirectory = Processor.MacroFolder;
      
      //if (!string.IsNullOrEmpty(Parameters[0]))
      //{
      //  string dir = Path.GetDirectoryName(Parameters[0]);
      //  if (Directory.Exists(dir))
      //    openFileDialog.InitialDirectory = dir;
      //}

      //if (openFileDialog.ShowDialog(parent) == DialogResult.OK)
      //{
      //  Parameters[0] = openFileDialog.FileName;
      //  return true;
      //}

      //return false;
      CommandConfigForm edit = new CommandConfigForm(this);
      if (edit.ShowDialog(parent) == DialogResult.OK)
      {
        if (ReferenceEquals(edit.Parameters, null)) return false;
        if (ReferenceEquals(edit.Parameters, new string[] {})) return false;

        Parameters = edit.Parameters;
        return true;
      }

      return false;
    }

    #endregion Implementation
  }
}