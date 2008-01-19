using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Commands
{

  /// <summary>
  /// If Statement macro command.
  /// </summary>
  public class CommandIf : Command
  {

    #region Comparisons

    // String comparisons ...
    internal const string IfEquals              = "==";
    internal const string IfNotEqual            = "!=";
    internal const string IfContains            = "CONTAINS";
    internal const string IfStartsWith          = "STARTS WITH";
    internal const string IfEndsWith            = "ENDS WITH";

    // Integer comparisons ...
    internal const string IfGreaterThan         = ">";
    internal const string IfLessThan            = "<";
    internal const string IfGreaterThanOrEqual  = ">=";
    internal const string IfLessThanOrEqual     = "<=";

    #endregion Comparisons

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandIf"/> class.
    /// </summary>
    public CommandIf() { InitParameters(5); }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandIf"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandIf(string[] parameters) : base(parameters) { }

    #endregion Constructors

    #region Implementation

    /// <summary>
    /// Gets the category of this command.
    /// </summary>
    /// <returns>The category of this command.</returns>
    public override string GetCategory() { return Processor.CategoryMacro; }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <returns>User interface text.</returns>
    public override string GetUserInterfaceText() { return "If Statement"; }

    /// <summary>
    /// Edit this command.
    /// </summary>
    /// <param name="parent">The parent window.</param>
    /// <returns><c>true</c> if the command was modified; otherwise <c>false</c>.</returns>
    public override bool Edit(IWin32Window parent)
    {
      EditIf edit = new EditIf(_parameters);
      if (edit.ShowDialog(parent) == DialogResult.OK)
      {
        _parameters = edit.Parameters;
        return true;
      }

      return false;
    }

    #endregion Implementation

    #region Static Methods

    /// <summary>
    /// This method will determine if an If Statement evaluates true.
    /// </summary>
    /// <param name="value1">The first value for comparison.</param>
    /// <param name="comparison">The comparison type.</param>
    /// <param name="value2">The second value for comparison.</param>
    /// <returns><c>true</c> if the parameters evaluates true, otherwise <c>false</c>.</returns>
    public static bool Evaluate(string value1, string comparison, string value2)
    {
      int value1AsInt;
      bool value1IsInt = int.TryParse(value1, out value1AsInt);

      int value2AsInt;
      bool value2IsInt = int.TryParse(value2, out value2AsInt);

      bool comparisonResult = false;
      switch (comparison.ToUpperInvariant())
      {
        // Use string comparison ...
        case IfEquals:      comparisonResult = value1.Equals(value2, StringComparison.OrdinalIgnoreCase);         break;
        case IfNotEqual:    comparisonResult = !value1.Equals(value2, StringComparison.OrdinalIgnoreCase);        break;
        case IfContains:    comparisonResult = value1.ToUpperInvariant().Contains(value2.ToUpperInvariant());     break;
        case IfStartsWith:  comparisonResult = value1.ToUpperInvariant().StartsWith(value2.ToUpperInvariant());   break;
        case IfEndsWith:    comparisonResult = value1.ToUpperInvariant().EndsWith(value2.ToUpperInvariant());     break;

        // Use integer comparison ...
        case IfGreaterThan:
          if (value1IsInt && value2IsInt)
            comparisonResult = (value1AsInt > value2AsInt);
          break;
        
        case IfLessThan:
          if (value1IsInt && value2IsInt)
            comparisonResult = (value1AsInt < value2AsInt);
          break;
        
        case IfGreaterThanOrEqual:
          if (value1IsInt && value2IsInt)
            comparisonResult = (value1AsInt >= value2AsInt);
          break;
        
        case IfLessThanOrEqual:
          if (value1IsInt && value2IsInt)
            comparisonResult = (value1AsInt <= value2AsInt);
          break;

        default:
          throw new CommandStructureException(String.Format("Invalid variable comparison method: {0}", comparison));
      }

      return comparisonResult;
    }

    #endregion Static Methods

  }

}
