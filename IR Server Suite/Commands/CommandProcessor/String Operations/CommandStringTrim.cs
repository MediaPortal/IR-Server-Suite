using System.Windows.Forms;

namespace Commands
{
  /// <summary>
  /// String Trim macro command.
  /// </summary>
  public class CommandStringTrim : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandStringTrim"/> class.
    /// </summary>
    public CommandStringTrim()
    {
      InitParameters(2);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandStringTrim"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandStringTrim(string[] parameters) : base(parameters)
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
      return Processor.CategoryString;
    }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <returns>User interface text.</returns>
    public override string GetUserInterfaceText()
    {
      return "String Trim";
    }

    /// <summary>
    /// Edit this command.
    /// </summary>
    /// <param name="parent">The parent window.</param>
    /// <returns><c>true</c> if the command was modified; otherwise <c>false</c>.</returns>
    public override bool Edit(IWin32Window parent)
    {
      EditStringOperation edit = new EditStringOperation(Parameters);
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
    /// <param name="variables">The variable list of the calling code.</param>
    public override void Execute(VariableList variables)
    {
      string[] processed = ProcessParameters(variables, Parameters);

      string output = processed[0].Trim();

      variables.VariableSet(processed[1], output);
    }

    #endregion Implementation
  }
}