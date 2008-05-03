using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Commands
{

  /// <summary>
  /// Base class for all IR Server Suite commands.
  /// </summary>
  public abstract class Command
  {

    #region Variables

    /// <summary>
    /// Command parameters.
    /// </summary>
    string[] _parameters;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Gets or sets the command parameters.
    /// </summary>
    /// <value>The command parameters.</value>
    public virtual string[] Parameters
    {
      get { return _parameters; }
      set { _parameters = value; }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Command"/> class.
    /// </summary>
    protected Command() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Command"/> class.
    /// </summary>
    /// <param name="parameters">The command parameters.</param>
    protected Command(string[] parameters) { _parameters = parameters; }

    #endregion Constructors

    #region Implementation

    /// <summary>
    /// Gets the category of this command.
    /// This method must be replaced in sub-classes.
    /// </summary>
    /// <returns>The category of this command.</returns>
    public abstract string GetCategory();

    /// <summary>
    /// Gets the user interface text.
    /// This method must be replaced in sub-classes.
    /// </summary>
    /// <returns>The user interface text.</returns>
    public abstract string GetUserInterfaceText();

    /// <summary>
    /// Gets the user display text.
    /// </summary>
    /// <returns>The user display text.</returns>
    public virtual string GetUserDisplayText()
    {
      if (Parameters == null)
        return GetUserInterfaceText();
      else
        return String.Format("{0} ({1})", GetUserInterfaceText(), String.Join(", ", Parameters));
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="variables">The variable list of the calling code.</param>
    public virtual void Execute(VariableList variables) { }

    /// <summary>
    /// Edit this command.
    /// </summary>
    /// <param name="parent">The parent window.</param>
    /// <returns><c>true</c> if the command was modified; otherwise <c>false</c>.</returns>
    public virtual bool Edit(IWin32Window parent) { return true; }

    /// <summary>
    /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
    /// </summary>
    /// <returns>A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.</returns>
    public override string ToString()
    {
      StringBuilder xml = new StringBuilder();
      using (StringWriter stringWriter = new StringWriter(xml))
      {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(string[]));
        xmlSerializer.Serialize(stringWriter, _parameters);
      }

      return String.Format("{0}, {1}", this.GetType().FullName, xml);
    }

    /// <summary>
    /// Initialises the parameters.
    /// </summary>
    /// <param name="parameterCount">The parameter count.</param>
    protected virtual void InitParameters(int parameterCount)
    {
      if (parameterCount == 0)
      {
        _parameters = null;
      }
      else
      {
        _parameters = new string[parameterCount];
        for (int index = 0; index < _parameters.Length; index++)
          _parameters[index] = String.Empty;
      }
    }

    #endregion Implementation

    #region Static Methods

    /// <summary>
    /// Processes an array of strings to derefrence variables and replace special characters and strings.
    /// </summary>
    /// <param name="variables">The variable list.</param>
    /// <param name="parameters">Array of parameter strings.</param>
    /// <returns>Processed parameters array of strings.</returns>
    public static string[] ProcessParameters(VariableList variables, string[] parameters)
    {
      string[] processed = new string[parameters.Length];
      for (int index = 0; index < parameters.Length; index++)
      {
        if (String.IsNullOrEmpty(parameters[index]))
        {
          processed[index] = parameters[index];
        }
        else
        {
          string newParam = parameters[index];
          if (newParam.StartsWith(VariableList.VariablePrefix, StringComparison.OrdinalIgnoreCase))
            newParam = variables.VariableGet(newParam.Substring(VariableList.VariablePrefix.Length));
          newParam = ReplaceSpecial(newParam);

          processed[index] = newParam;
        }
      }

      return processed;
    }

    /// <summary>
    /// Replace all instances of environment variables, special values and escape codes.
    /// </summary>
    /// <param name="input">The input to process.</param>
    /// <returns>Processed input string.</returns>
    public static string ReplaceSpecial(string input)
    {
      if (String.IsNullOrEmpty(input))
        return input;

      // Process Special Codes ...
      if (input.Contains("%"))
      {
        foreach (Match match in Regex.Matches(input, @"%\w+%"))
        {
          string varName = match.Value.Substring(1, match.Value.Length - 2);

          string envVar = String.Empty;

          switch (varName.ToUpperInvariant())
          {
            case "$CLIPBOARD_TEXT$":
              if (Clipboard.ContainsText())
                envVar = Clipboard.GetText();
              break;

            case "$TIME$":
              envVar = DateTime.Now.ToShortTimeString();
              break;

            case "$HOUR$":
              envVar = DateTime.Now.Hour.ToString();
              break;

            case "$MINUTE$":
              envVar = DateTime.Now.Minute.ToString();
              break;

            case "$SECOND$":
              envVar = DateTime.Now.Second.ToString();
              break;

            case "$DATE$":
              envVar = DateTime.Now.ToShortDateString();
              break;

            case "$YEAR$":
              envVar = DateTime.Now.Year.ToString();
              break;

            case "$MONTH$":
              envVar = DateTime.Now.Month.ToString();
              break;

            case "$DAY$":
              envVar = DateTime.Now.Day.ToString();
              break;

            case "$DAYOFWEEK$":
              envVar = DateTime.Now.DayOfWeek.ToString();
              break;

            case "$DAYOFYEAR$":
              envVar = DateTime.Now.DayOfYear.ToString();
              break;

            case "$USERNAME$":
              envVar = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
              break;

            case "$MACHINENAME$":
              envVar = Environment.MachineName;
              break;

            default:
              envVar = Environment.GetEnvironmentVariable(varName);
              break;
          }

          input = input.Replace(match.Value, envVar);
        }
      }

      // Process Escape Codes ...
      bool inEscapeCode = false;
      bool inHexCode = false;
      byte hexParsed;
      StringBuilder hexCode = new StringBuilder();
      StringBuilder output = new StringBuilder();

      foreach (char currentChar in input)
      {
        if (inEscapeCode)
        {
          switch (currentChar)
          {
            case 'a':
              output.Append((char)7);
              break;
            case 'b':
              output.Append((char)Keys.Back);
              break;
            case 'f':
              output.Append((char)12);
              break;
            case 'n':
              output.Append((char)Keys.LineFeed);
              break;
            case 'r':
              output.Append((char)Keys.Return);
              break;
            case 't':
              output.Append((char)Keys.Tab);
              break;
            case 'v':
              output.Append((char)11);
              break;
            case 'x':
              hexCode = new StringBuilder();
              inHexCode = true;
              inEscapeCode = false;
              break;
            case '0':   // I've got a bad feeling about this
              output.Append((char)0);
              break;

            default:    // If it doesn't know it as an escape code, just use the char
              output.Append(currentChar);
              break;
          }

          inEscapeCode = false;
        }
        else if (inHexCode)
        {
          switch (currentChar)
          {
            case 'h':   // 'h' terminates the hex code
              if (byte.TryParse(hexCode.ToString(), System.Globalization.NumberStyles.HexNumber, null, out hexParsed))
                output.Append((char)hexParsed);
              else
                throw new ArgumentException(String.Format("Bad Hex Code \"{0}\"", hexCode.ToString()), "input");

              inHexCode = false;
              break;

            default:
              hexCode.Append(currentChar);
              break;
          }
        }
        else if (currentChar == '\\')
        {
          inEscapeCode = true;
        }
        else
        {
          output.Append(currentChar);
        }
      }

      return output.ToString();
    }

    #endregion Static Methods

  }

}
