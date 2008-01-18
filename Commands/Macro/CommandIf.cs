using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Commands
{

  /// <summary>
  /// If macro command.
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
    public override string GetCategory() { return Macro.Category; }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <returns>User interface text.</returns>
    public override string GetUserInterfaceText() { return "If Statement"; }
/*
    /// <summary>
    /// Gets the user display text.
    /// </summary>
    /// <returns>The user display text.</returns>
    public override string GetUserDisplayText()
    {
      return String.Format("{0} ({1})", GetUserInterfaceText(), String.Join(", ", Parameters));
    }
*/
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

    /// <summary>
    /// This method will determine if this If Command evaluates true.
    /// </summary>
    /// <returns><c>true</c> if the command evaluates true, otherwise <c>false</c>.</returns>
    public bool Evaluate()
    {
      int value1AsInt;
      bool value1IsInt = int.TryParse(_parameters[0], out value1AsInt);

      int value2AsInt;
      bool value2IsInt = int.TryParse(_parameters[2], out value2AsInt);

      bool comparisonResult = false;
      switch (_parameters[1].ToUpperInvariant())
      {
        // Use string comparison ...
        case IfEquals:      comparisonResult = _parameters[0].Equals(_parameters[2], StringComparison.OrdinalIgnoreCase);         break;
        case IfNotEqual:    comparisonResult = !_parameters[0].Equals(_parameters[2], StringComparison.OrdinalIgnoreCase);        break;
        case IfContains:    comparisonResult = _parameters[0].ToUpperInvariant().Contains(_parameters[2].ToUpperInvariant());     break;
        case IfStartsWith:  comparisonResult = _parameters[0].ToUpperInvariant().StartsWith(_parameters[2].ToUpperInvariant());   break;
        case IfEndsWith:    comparisonResult = _parameters[0].ToUpperInvariant().EndsWith(_parameters[2].ToUpperInvariant());     break;

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
          throw new InvalidOperationException(String.Format("Invalid variable comparison method: {0}", _parameters[1]));
      }

      return comparisonResult;
    }

    #endregion Implementation
    
  }

}
