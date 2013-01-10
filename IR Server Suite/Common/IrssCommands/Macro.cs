using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace IrssCommands
{
  /// <summary>
  /// Macro
  /// </summary>
  public class Macro
  {
    #region Variables

    private readonly List<Command> _commands;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Gets the commands in this macro.
    /// </summary>
    /// <value>The commands in this macro.</value>
    public List<Command> Commands
    {
      get { return _commands; }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Macro"/> class.
    /// </summary>
    public Macro()
    {
      _commands = new List<Command>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Macro"/> class.
    /// </summary>
    /// <param name="fileName">Name of the macro file to open.</param>
    public Macro(string fileName) : this()
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(fileName);

      XmlNodeList commandSequence = doc.DocumentElement.SelectNodes("item");

      foreach (XmlNode item in commandSequence)
      {
        string commandType = item.Attributes["CommandType"].Value;
        string[] parameters = null;

        XmlNodeList parameterSequence = item.SelectNodes("parameter");
        if (!ReferenceEquals(parameterSequence, null))
        {
          if (parameterSequence.Count > 0)
            parameters = new string[parameterSequence.Count];

          for (int i = 0; i < parameterSequence.Count; i++)
            parameters[i] = parameterSequence[i].InnerText;
        }

        Command command = Processor.CreateCommand(commandType, parameters);
        _commands.Add(command);
      }
    }

    #endregion Constructors

    #region Implementation

    /// <summary>
    /// Saves this macro to the specified file name.
    /// </summary>
    /// <param name="fileName">Name of the file to save to.</param>
    public void Save(string fileName)
    {
      using (XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.UTF8))
      {
        writer.Formatting = Formatting.Indented;
        writer.WriteStartDocument(true);
        writer.WriteStartElement("macro");

        foreach (Command command in _commands)
        {
          writer.WriteStartElement("item");
          writer.WriteAttributeString("CommandType", command.GetType().FullName);
          foreach (string s in command.Parameters)
          {
            writer.WriteElementString("parameter", s);
          }
          writer.WriteEndElement();
        }

        writer.WriteEndElement();
        writer.WriteEndDocument();
      }
    }

    /// <summary>
    /// Exceutes this macro the specified variables and process command method.
    /// </summary>
    /// <param name="commandProcessor">The command processor.</param>
    public void Execute(Processor commandProcessor)
    {
      try
      {
        for (int position = 0; position < _commands.Count; position++)
        {
          Command command = _commands[position];

          if (command is CommandIf)
          {
            string[] processed = Command.ProcessParameters(commandProcessor.Variables, command.Parameters);

            if (CommandIf.Evaluate(processed[0], processed[1], processed[2]))
              position = FindLabel(processed[3]);
            else if (!String.IsNullOrEmpty(processed[4]))
              position = FindLabel(processed[4]);
          }
          else if (command is CommandGotoLabel)
          {
            string[] processed = Command.ProcessParameters(commandProcessor.Variables, command.Parameters);

            position = FindLabel(processed[0]);
          }
          else
          {
            commandProcessor.Execute(command, false);
          }
        }
      }
      catch (Exception ex)
      {
        throw new MacroExecutionException("Error executing macro", ex);
      }
    }

    /// <summary>
    /// Gets the position of label within the macro.
    /// </summary>
    /// <param name="label">The label to find.</param>
    /// <returns>The label position.</returns>
    private int FindLabel(string label)
    {
      for (int position = 0; position < _commands.Count; position++)
      {
        Command command = _commands[position];

        if (command is CommandLabel)
        {
          string thisLabel = command.Parameters[0];
          if (label.Equals(thisLabel, StringComparison.OrdinalIgnoreCase))
            return position;
        }
      }

      throw new MacroStructureException(String.Format("Label not found: {0}", label));
    }

    #endregion Implementation
  }
}