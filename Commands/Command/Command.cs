using System;
using System.IO;
using System.Text;
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
    protected string[] _parameters;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Gets the command parameters.
    /// </summary>
    /// <value>The command parameters.</value>
    public virtual string[] Parameters { get { return _parameters; } }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Command"/> class.
    /// </summary>
    public Command() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Command"/> class.
    /// </summary>
    /// <param name="parameters">The command parameters.</param>
    public Command(string[] parameters) { _parameters = parameters; }

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
    /// <returns>
    /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
    /// </returns>
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
        {
          _parameters[index] = String.Empty;
        }
      }
    }

    #endregion Implementation

  }

}
