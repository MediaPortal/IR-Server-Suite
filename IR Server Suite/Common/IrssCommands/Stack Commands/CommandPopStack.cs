using System.Windows.Forms;

namespace Commands
{
  /// <summary>
  /// Pop Stack stack command.
  /// </summary>
  public class CommandPopStack : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandPopStack"/> class.
    /// </summary>
    public CommandPopStack()
    {
      InitParameters(1);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandPopStack"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandPopStack(string[] parameters) : base(parameters)
    {
    }

    #endregion Constructors

    #region Implementation

    /// <summary>
    /// Gets the category of this command.
    /// </summary>
    /// <returns>The category of this command.</returns>
    public override string GetCategory()
    {
      return Processor.CategoryStack;
    }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <returns>User interface text.</returns>
    public override string GetUserInterfaceText()
    {
      return "Pop Stack";
    }

    /// <summary>
    /// Edit this command.
    /// </summary>
    /// <param name="parent">The parent window.</param>
    /// <returns><c>true</c> if the command was modified; otherwise <c>false</c>.</returns>
    public override bool Edit(IWin32Window parent)
    {
      EditStackFile edit = new EditStackFile(Parameters[0]);
      if (edit.ShowDialog(parent) == DialogResult.OK)
      {
        Parameters[0] = edit.FileName;
        return true;
      }

      return false;
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    public override void Execute(VariableList variables)
    {
      string[] processed = ProcessParameters(variables, Parameters);
      variables.StackPop(processed[0]);
    }

    #endregion Implementation
  }
}