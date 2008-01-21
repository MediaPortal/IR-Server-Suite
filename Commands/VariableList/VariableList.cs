using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Commands
{

  /// <summary>
  /// List of Variables for use in macros and commands.
  /// </summary>
  public class VariableList
  {

    #region Constants

    /// <summary>
    /// File extension for stored Variable Lists.
    /// </summary>
    public const string FileExtensionVariableList = ".VariableList";

    /// <summary>
    /// File extension for a stored Stack.
    /// </summary>
    public const string FileExtensionStack = ".Stack";

    /// <summary>
    /// Variables must be prefixed with this string.
    /// </summary>
    public const string VariablePrefix = "var_";

    #endregion Constants

    #region Variables

    Dictionary<string, string> _variables;

    Stack<string> _stack;

    #endregion Variables

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="VariableList"/> class.
    /// </summary>
    public VariableList()
    {
      _variables = new Dictionary<string, string>();
      _stack = new Stack<string>();
    }

    #endregion Constructor

    #region Variable List Methods

    /// <summary>
    /// Sets a string variable.
    /// </summary>
    /// <param name="variable">The variable.</param>
    /// <param name="value">The value.</param>
    public void VariableSet(string variable, string value)
    {
      lock (_variables)
      {
        if (_variables.ContainsKey(variable))
          _variables[variable] = value;
        else
          _variables.Add(variable, value);
      }
    }

    /// <summary>
    /// Gets a string variable.
    /// </summary>
    /// <param name="variable">The variable.</param>
    /// <returns>The variable value.</returns>
    public string VariableGet(string variable)
    {
      lock (_variables)
      {
        if (_variables.ContainsKey(variable))
          return _variables[variable];
        else
          return String.Empty;
      }
    }

    /// <summary>
    /// Clears all the variables from this instance of the Variable List.
    /// </summary>
    public void VariableClear()
    {
      lock (_variables)
      {
        _variables.Clear();
      }
    }

    /// <summary>
    /// Saves the Variable List to a file.
    /// </summary>
    /// <param name="fileName">Absolute path of the file to save to.</param>
    public void VariableSave(string fileName)
    {
      if (String.IsNullOrEmpty(fileName))
        throw new ArgumentNullException("fileName");

      lock (_variables)
      {
        using (XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.UTF8))
        {
          writer.Formatting = Formatting.Indented;
          writer.Indentation = 1;
          writer.IndentChar = (char)9;
          writer.WriteStartDocument(true);
          writer.WriteStartElement("variables"); // <variables>

          foreach (string key in _variables.Keys)
            writer.WriteElementString(key, _variables[key]);

          writer.WriteEndElement(); // </variables>
        }
      }
    }

    /// <summary>
    /// Loads the variables from a given file into the current Variable List.
    /// </summary>
    /// <param name="fileName">Absolute path of the file to load.</param>
    public void VariableLoad(string fileName)
    {
      if (String.IsNullOrEmpty(fileName))
        throw new ArgumentNullException("fileName");

      XmlDocument doc = new XmlDocument();
      doc.Load(fileName);

      XmlNodeList variableNodes = doc.DocumentElement.SelectNodes("variables");

      lock (_variables)
      {
        foreach (XmlNode node in variableNodes)
          VariableSet(node.Name, node.Value);
      }
    }

    #endregion Variable List Methods

    #region Stack Methods

    /// <summary>
    /// Push an item onto the stack.
    /// </summary>
    /// <param name="value">The value.</param>
    public void StackPush(string value)
    {
      lock (_stack)
      {
        _stack.Push(value);
      }
    }

    /// <summary>
    /// Pop an item from the stack into a variable.
    /// </summary>
    /// <param name="variable">The variable to store the item in.</param>
    public void StackPop(string variable)
    {
      lock (_stack)
      {
        string stackItem = _stack.Pop();
        VariableSet(variable, stackItem);
      }
    }

    /// <summary>
    /// Peek at the top of the stack and place that item into a variable.
    /// </summary>
    /// <param name="variable">The variable to store the item in.</param>
    public void StackPeek(string variable)
    {
      lock (_stack)
      {
        string stackItem = _stack.Peek();
        VariableSet(variable, stackItem);
      }
    }

    /// <summary>
    /// Clears the stack.
    /// </summary>
    public void StackClear()
    {
      lock (_stack)
      {
        _stack.Clear();
      }
    }

    /// <summary>
    /// Saves the current stack.
    /// </summary>
    /// <param name="fileName">Name of the file to save the stack to.</param>
    public void StackSave(string fileName)
    {
      if (String.IsNullOrEmpty(fileName))
        throw new ArgumentNullException("fileName");

      lock (_stack)
      {
        using (XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.UTF8))
        {
          writer.Formatting = Formatting.Indented;
          writer.Indentation = 1;
          writer.IndentChar = (char)9;
          writer.WriteStartDocument(true);
          writer.WriteStartElement("stack"); // <stack>

          foreach (string item in _stack)
            writer.WriteElementString("item", item);

          writer.WriteEndElement(); // </stack>
        }
      }
    }

    /// <summary>
    /// Loads a stored stack into the current stack.
    /// </summary>
    /// <param name="fileName">Name of the file to load the stack from.</param>
    public void StackLoad(string fileName)
    {
      if (String.IsNullOrEmpty(fileName))
        throw new ArgumentNullException("fileName");

      XmlDocument doc = new XmlDocument();
      doc.Load(fileName);

      XmlNodeList stackNodes = doc.DocumentElement.SelectNodes("stack");

      lock (_stack)
      {
        foreach (XmlNode node in stackNodes)
          _stack.Push(node.Value);
      }

    }

    #endregion Implementation

  }

}
