using System;
using System.Collections.Generic;
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
      string fileName = _parameters[0];

      if (_parameters[0].StartsWith(Common.FolderAppData, StringComparison.OrdinalIgnoreCase))
        fileName = _parameters[0].Substring(Common.FolderAppData.Length);

      return String.Format("{0} ({1})", GetUserInterfaceText(), fileName);
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="commandProcessor">The command processor.</param>
    public void Execute(Processor commandProcessor)
    {
      Macro macro = new Macro(_parameters[0]);
      macro.Execute(commandProcessor);
    }

    #endregion Implementation

  }

}
