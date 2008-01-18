using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using IrssUtils;

namespace Commands
{

  /// <summary>
  /// Macro
  /// </summary>
  public class Macro
  {

    #region Constants

    /// <summary>
    /// Macro file extension.
    /// </summary>
    public const string FileExtension = ".Macro";

    /// <summary>
    /// Category for Macro Commands.
    /// </summary>
    public const string Category = "Macro Commands";

    /// <summary>
    /// Category for commands that should not be visible in command lists.
    /// </summary>
    public const string HiddenCategory = "Hidden";

    /// <summary>
    /// User Interface Text.
    /// </summary>
    public const string UserInterfaceText = "Macro";

    #endregion Constants

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
        Command command = Macro.CreateCommand(xml);
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
    /// <param name="variables">The variables to use.</param>
    public void Execute(VariableList variables)
    {
      for (int position = 0; position < _commands.Count; position++)
      {
        Command command = _commands[position];
        
        if (command is CommandIf)
        {
          if (command.Parameters[0].StartsWith(VariableList.VariablePrefix, StringComparison.OrdinalIgnoreCase))
            command.Parameters[0] = variables.GetVariable(command.Parameters[0].Substring(VariableList.VariablePrefix.Length));
          command.Parameters[0] = Common.ReplaceSpecial(command.Parameters[0]);

          if (command.Parameters[2].StartsWith(VariableList.VariablePrefix, StringComparison.OrdinalIgnoreCase))
            command.Parameters[2] = variables.GetVariable(command.Parameters[2].Substring(VariableList.VariablePrefix.Length));
          command.Parameters[2] = Common.ReplaceSpecial(command.Parameters[2]);

          if (((CommandIf)command).Evaluate())
            position = FindLabel(command.Parameters[3]);
          else if (!String.IsNullOrEmpty(command.Parameters[4]))
            position = FindLabel(command.Parameters[4]);
        }
        else if (command is CommandGotoLabel)
        {
          position = FindLabel(command.Parameters[0]);
        }
        else if (command is CommandBlastIR)
        {
          //((CommandBlastIR)command).Execute(_blastIrDelegate);
        }
        else
        {
          command.Execute(variables);
        }
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

    #region Static Methods

    /// <summary>
    /// Returns a list of Macros in the specified folder.
    /// </summary>
    /// <returns>List of Macros.</returns>
    public static string[] GetList(string folder)
    {
      string[] files = Directory.GetFiles(folder, '*' + FileExtension);

      for (int index = 0; index < files.Length; index++)
          files[index] = Path.GetFileNameWithoutExtension(files[index]);

      return files;
    }


    /// <summary>
    /// Creates a new <c>Command</c> from the supplied information.
    /// </summary>
    /// <param name="commandString">The command string representation.</param>
    /// <returns>A new <c>Command</c>.</returns>
    public static Command CreateCommand(string commandString)
    {
      if (String.IsNullOrEmpty(commandString))
        throw new ArgumentNullException("commandString");

      string splitAt = ", ";

      int splitPoint = commandString.IndexOf(splitAt);
      if (splitPoint == -1)
        throw new ArgumentException("Invalid command string", "commandString");

      string commandType = commandString.Substring(0, splitPoint);
      string parametersXml = commandString.Substring(splitPoint + splitAt.Length);

      return CreateCommand(commandType, parametersXml);
    }

    /// <summary>
    /// Creates a new <c>Command</c> from the supplied information.
    /// </summary>
    /// <param name="commandType">Type of command to create (FullName).</param>
    /// <param name="parametersXml">The parameters of this command in XML.</param>
    /// <returns>A new <c>Command</c>.</returns>
    public static Command CreateCommand(string commandType, string parametersXml)
    {
      if (String.IsNullOrEmpty(commandType))
        throw new ArgumentNullException("commandType");
      
      if (String.IsNullOrEmpty(parametersXml))
        throw new ArgumentNullException("parametersXml");

      string[] parameters;
      using (StringReader stringReader = new StringReader(parametersXml))
      {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(string[]));
        parameters = (string[])xmlSerializer.Deserialize(stringReader);
      }

      return CreateCommand(commandType, parameters);
    }

    /// <summary>
    /// Creates a new <c>Command</c> from the supplied information.
    /// </summary>
    /// <param name="commandType">Type of command to create (FullName).</param>
    /// <param name="parameters">The command parameters.</param>
    /// <returns>A new <c>Command</c>.</returns>
    public static Command CreateCommand(string commandType, string[] parameters)
    {
      List<Type> allCommands = new List<Type>();

      Type[] specialCommands = GetSpecialCommands();
      if (specialCommands != null)
        allCommands.AddRange(specialCommands);

      Type[] libraryCommands = Common.GetLibraryCommands();
      if (libraryCommands != null)
        allCommands.AddRange(libraryCommands);

      foreach (Type type in allCommands)
        if (type.FullName.Equals(commandType))
          return (Command)Activator.CreateInstance(type, new object[] { parameters });

      throw new InvalidOperationException(String.Format("Could not find command type: {0}", commandType));
    }

    /// <summary>
    /// Gets the special commands.
    /// </summary>
    /// <returns>List of special command types.</returns>
    public static Type[] GetSpecialCommands()
    {
      List<Type> specialCommands = new List<Type>();

      specialCommands.Add(typeof(CommandBlastIR));

      specialCommands.Add(typeof(CommandIf));
      specialCommands.Add(typeof(CommandLabel));
      specialCommands.Add(typeof(CommandGotoLabel));
      specialCommands.Add(typeof(CommandSetVariable));
      specialCommands.Add(typeof(CommandClearVariables));
      specialCommands.Add(typeof(CommandSaveVariables));
      specialCommands.Add(typeof(CommandLoadVariables));


      // Hidden commands ...
      specialCommands.Add(typeof(CommandCallMacro));

      return specialCommands.ToArray();
    }

    #endregion Static Methods

  }

}
