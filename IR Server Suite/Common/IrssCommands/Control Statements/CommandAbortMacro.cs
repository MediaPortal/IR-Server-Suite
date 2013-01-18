using System;
using System.Windows.Forms;

namespace IrssCommands
{
  /// <summary>
  /// Abort special command.
  /// </summary>
  public class CommandAbortMacro : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandAbortMacro"/> class.
    /// </summary>
    public CommandAbortMacro()
    {
      InitParameters(1);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandAbortMacro"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandAbortMacro(string[] parameters) : base(parameters)
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
      get { return Processor.CategoryControl; }
    }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <value>User interface text.</value>
    public override string UserInterfaceText
    {
      get { return "Abort Macro"; }
    }

    /// <summary>
    /// Gets the user display text.
    /// </summary>
    /// <value>The user display text.</value>
    public override string UserDisplayText
    {
      get
      {
        if (String.IsNullOrEmpty(Parameters[0]))
          return UserInterfaceText;
        else
          return String.Format("{0} \"{1}\"", UserInterfaceText, Parameters[0]);
      }
    }

    /// <summary>
    /// Gets the edit control to be used within a common edit form.
    /// </summary>
    /// <returns>The edit control.</returns>
    public override BaseCommandConfig GetEditControl()
    {
      return new AbortMacroConfig(Parameters);
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    public override void Execute(VariableList variables)
    {
      string[] processed = ProcessParameters(variables, Parameters);

      if (String.IsNullOrEmpty(processed[0]))
        throw new MacroExecutionException(UserInterfaceText);
      else
        throw new MacroExecutionException(String.Format("{0}: {1}", UserInterfaceText, processed[0]));
    }

    #endregion Implementation
  }
}