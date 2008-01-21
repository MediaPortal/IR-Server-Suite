using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

//using IrssUtils;

namespace Commands
{

  /// <summary>
  /// Macro
  /// </summary>
  public class Macro
  {

    #region Variables

    List<Command> _commands;

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
        string xml = item.Attributes["xml"].Value;
        Command command = Processor.CreateCommand(xml);
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
          writer.WriteAttributeString("xml", command.ToString());
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
            string value1 = command.Parameters[0];
            string comparison = command.Parameters[1];
            string value2 = command.Parameters[2];

            if (value1.StartsWith(VariableList.VariablePrefix, StringComparison.OrdinalIgnoreCase))
              value1 = commandProcessor.Variables.VariableGet(value1.Substring(VariableList.VariablePrefix.Length));
            value1 = Common.ReplaceSpecial(value1);

            if (value2.StartsWith(VariableList.VariablePrefix, StringComparison.OrdinalIgnoreCase))
              value2 = commandProcessor.Variables.VariableGet(value2.Substring(VariableList.VariablePrefix.Length));
            value2 = Common.ReplaceSpecial(value2);

            if (CommandIf.Evaluate(value1, comparison, value2))
              position = FindLabel(command.Parameters[3]);
            else if (!String.IsNullOrEmpty(command.Parameters[4]))
              position = FindLabel(command.Parameters[4]);
          }
          else if (command is CommandGotoLabel)
          {
            position = FindLabel(command.Parameters[0]);
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
    int FindLabel(string label)
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
