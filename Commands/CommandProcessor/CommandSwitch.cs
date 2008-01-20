using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Commands
{

  /// <summary>
  /// Switch Statement macro command.
  /// </summary>
  public class CommandSwitch : Command
  {

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandSwitch"/> class.
    /// </summary>
    public CommandSwitch() { InitParameters(3); }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandSwitch"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandSwitch(string[] parameters) : base(parameters) { }

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
    public override string GetUserInterfaceText() { return "Switch Statement"; }

    /// <summary>
    /// Edit this command.
    /// </summary>
    /// <param name="parent">The parent window.</param>
    /// <returns><c>true</c> if the command was modified; otherwise <c>false</c>.</returns>
    public override bool Edit(IWin32Window parent)
    {
      EditSwitch edit = new EditSwitch(Parameters);
      if (edit.ShowDialog(parent) == DialogResult.OK)
      {
        Parameters = edit.Parameters;
        return true;
      }

      return false;
    }

    #endregion Implementation

    #region Static Methods

    /// <summary>
    /// This method will determine which (if any) case in a Switch Statement evaluates true.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="casesXml">The cases XML.</param>
    /// <returns>Label to goto (null for default).</returns>
    public static string Evaluate(string value, string casesXml)
    {
      string[] cases;
      using (StringReader stringReader = new StringReader(casesXml))
      {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(string[]));
        cases = (string[])xmlSerializer.Deserialize(stringReader);
      }

      for (int index = 0; index < cases.Length; index += 2)
        if (value.Equals(cases[index], StringComparison.OrdinalIgnoreCase))
          return cases[index + 1];

      return null;
    }

    #endregion Static Methods

  }

}
