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
      return String.Format("{0} ({1})", GetUserInterfaceText(), String.Join(", ", Parameters));
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="variables">The variable list of the calling code.</param>
    /// <param name="blastIrDelegate">The blast ir delegate.</param>
    public void Execute(VariableList variables, BlastIrDelegate blastIrDelegate)
    {
      Macro macro = new Macro(_parameters[0]);
      macro.Execute(variables, blastIrDelegate);
    }

    #endregion Implementation

  }

}
