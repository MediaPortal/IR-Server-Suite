using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Commands
{

  /// <summary>
  /// Provides a delegate to call to execute an IR Command.
  /// </summary>
  /// <param name="fileName">Full file path to the IR Command file.</param>
  /// <param name="port">IR Blaster port to transmit on.</param>
  public delegate void BlastIrDelegate(string fileName, string port);

  /// <summary>
  /// Encapsulates all Command Processing.
  /// </summary>
  public class Processor
  {

    #region Constants

    /// <summary>
    /// Macro file extension.
    /// </summary>
    public const string FileExtensionMacro  = ".Macro";

    /// <summary>
    /// IR Command file extension.
    /// </summary>
    public const string FileExtensionIR     = ".IR";


    /// <summary>
    /// Standard text for the Macro Category.
    /// </summary>
    public const string CategoryMacro       = "Macro Commands";
    /// <summary>
    /// Standard text for the Macro Category.
    /// </summary>
    public const string CategoryGeneral     = "General Commands";
    /// <summary>
    /// Standard text for the Macro Category.
    /// </summary>
    public const string CategoryMediaPortal = "MediaPortal Commands";
    /// <summary>
    /// Standard text for the Macro Category.
    /// </summary>
    public const string CategoryHidden      = "Hidden";

    #endregion Constants

    #region Variables

    VariableList _variables;

    BlastIrDelegate _blastIrDelegate;
    string[] _blastIrPorts;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Gets the variable list.
    /// </summary>
    /// <value>The variable list.</value>
    public VariableList Variables
    {
      get { return _variables; }
    }

    /// <summary>
    /// Gets the blast ir delegate.
    /// </summary>
    /// <value>The blast ir delegate.</value>
    public BlastIrDelegate BlastIr
    {
      get { return _blastIrDelegate; }
    }

    /// <summary>
    /// Gets the blast ir ports.
    /// </summary>
    /// <value>The blast ir ports.</value>
    public string[] BlastIrPorts
    {
      get { return _blastIrPorts; }
    }

    #endregion Properties

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Processor"/> class.
    /// </summary>
    /// <param name="blastIrDelegate">The blast ir delegate.</param>
    /// <param name="blastIrPorts">The blast ir ports.</param>
    public Processor(BlastIrDelegate blastIrDelegate, string[] blastIrPorts)
    {
      _variables = new VariableList();

      _blastIrDelegate  = blastIrDelegate;
      _blastIrPorts     = blastIrPorts;
    }

    #endregion Constructor

    #region Implementation

    /// <summary>
    /// Executes the specified command.
    /// </summary>
    /// <param name="command">The command.</param>
    public void Execute(Command command)
    {
      if (command is CommandBlastIR)
        (command as CommandBlastIR).Execute(_blastIrDelegate);
      else if (command is CommandCallMacro)
        (command as CommandCallMacro).Execute(_variables, _blastIrDelegate);
      else
        command.Execute(_variables);
    }

    /// <summary>
    /// Edits the specified command.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="parent">The parent window.</param>
    /// <returns><c>true</c> if the edit </returns>
    public bool Edit(Command command, IWin32Window parent)
    {
      if (command is CommandBlastIR)
        return (command as CommandBlastIR).Edit(parent, _blastIrDelegate, _blastIrPorts);
      else
        return command.Edit(parent);
    }

    #endregion Implementation

    #region Static Methods

    /// <summary>
    /// Returns a list of IR Command files.
    /// </summary>
    /// <returns>List of IR Command files.</returns>
    public static string[] GetListIR()
    {
      string[] files = Directory.GetFiles(IrssUtils.Common.FolderIRCommands, '*' + FileExtensionIR);

      for (int index = 0; index < files.Length; index++)
        files[index] = Path.GetFileNameWithoutExtension(files[index]);

      return files;
    }

    /// <summary>
    /// Returns a list of Macros in the specified folder.
    /// </summary>
    /// <param name="folder">The folder.</param>
    /// <returns>List of Macros.</returns>
    public static string[] GetListMacro(string folder)
    {
      string[] files = Directory.GetFiles(folder, '*' + FileExtensionMacro);

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

      Type[] libraryCommands = IrssUtils.Common.GetLibraryCommands();
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

      specialCommands.Add(typeof(CommandIf));
      specialCommands.Add(typeof(CommandLabel));
      specialCommands.Add(typeof(CommandGotoLabel));
      specialCommands.Add(typeof(CommandSetVariable));
      specialCommands.Add(typeof(CommandClearVariables));
      specialCommands.Add(typeof(CommandSaveVariables));
      specialCommands.Add(typeof(CommandLoadVariables));

      // Hidden commands ...
      specialCommands.Add(typeof(CommandBlastIR));
      specialCommands.Add(typeof(CommandCallMacro));

      return specialCommands.ToArray();
    }

    #endregion Static Methods





  }


}
