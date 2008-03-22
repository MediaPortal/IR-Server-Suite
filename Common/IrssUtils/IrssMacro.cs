using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace IrssUtils
{

  #region Delegates

  /// <summary>
  /// Used to process macros using the command handler from the calling code.
  /// </summary>
  public delegate void ProcessCommandCallback(object command);

  #endregion Delegates

  /// <summary>
  /// Methods for dealing with Macros.
  /// </summary>
  public static class IrssMacro
  {

    #region Constants

    // String comparisons ...
    public const string IfEquals              = "==";
    public const string IfNotEqual            = "!=";
    public const string IfContains            = "CONTAINS";
    public const string IfStartsWith          = "STARTS WITH";
    public const string IfEndsWith            = "ENDS WITH";

    // Integer comparisons ...
    public const string IfGreaterThan         = ">";
    public const string IfLessThan            = "<";
    public const string IfGreaterThanOrEqual  = ">=";
    public const string IfLessThanOrEqual     = "<=";

    #endregion Constants

    #region Public Methods

    /// <summary>
    /// Read a macro from the macro file provided.
    /// </summary>
    /// <param name="fileName">Name of Macro to read (full macro file path).</param>
    public static string[] ReadFromFile(string fileName)
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(fileName);

      XmlNodeList commandSequence = doc.DocumentElement.SelectNodes("item");

      string[] commandList = new string[commandSequence.Count];

      int index = 0;
      foreach (XmlNode item in commandSequence)
        commandList[index++] = item.Attributes["command"].Value;

      return commandList;
    }

    /// <summary>
    /// Write the supplied macro to a macro file provided.
    /// </summary>
    /// <param name="fileName">Name of Macro to write (full macro file path).</param>
    /// <param name="commandList">The command list.</param>
    public static void WriteToFile(string fileName, string[] commandList)
    {
      using (XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.UTF8))
      {
        writer.Formatting = Formatting.Indented;
        writer.WriteStartDocument(true);
        writer.WriteStartElement("macro");

        foreach (string item in commandList)
        {
          writer.WriteStartElement("item");
          writer.WriteAttributeString("command", item);
          writer.WriteEndElement();
        }

        writer.WriteEndElement();
        writer.WriteEndDocument();
      }
    }

    /// <summary>
    /// Called to execute the supplied Macro.
    /// </summary>
    /// <param name="fileName">Macro file to process (absolute path).</param>
    /// <param name="variables">Variable List from calling code.</param>
    /// <param name="procCommand">Callback to the command handler from the calling code.</param>
    public static void ExecuteMacro(string fileName, VariableList variables, ProcessCommandCallback procCommand)
    {
      string[] commandList = ReadFromFile(fileName);
      ExecuteMacro(commandList, variables, procCommand);
    }

    /// <summary>
    /// Called to execute the supplied Macro.
    /// </summary>
    /// <param name="commandList">The command list.</param>
    /// <param name="variables">Variable List from calling code.</param>
    /// <param name="procCommand">Callback to the command handler from the calling code.</param>
    public static void ExecuteMacro(string[] commandList, VariableList variables, ProcessCommandCallback procCommand)
    {
      for (int position = 0; position < commandList.Length; position++)
      {
        string command = commandList[position];

        if (command.StartsWith(Common.CmdPrefixIf, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitIfCommand(command.Substring(Common.CmdPrefixIf.Length));

          if (commands[0].StartsWith(Common.VariablePrefix, StringComparison.OrdinalIgnoreCase))
            commands[0] = variables.GetVariable(commands[0].Substring(Common.VariablePrefix.Length));
          commands[0] = Common.ReplaceSpecial(commands[0]);

          if (commands[2].StartsWith(Common.VariablePrefix, StringComparison.OrdinalIgnoreCase))
            commands[2] = variables.GetVariable(commands[2].Substring(Common.VariablePrefix.Length));
          commands[2] = Common.ReplaceSpecial(commands[2]);

          if (EvaluateIfCommand(commands))
            position = GetLabelPosition(commandList, commands[3]);
          else if (!String.IsNullOrEmpty(commands[4]))
            position = GetLabelPosition(commandList, commands[4]);
        }
        else if (command.StartsWith(Common.CmdPrefixLabel, StringComparison.OrdinalIgnoreCase))
        {
          continue;
        }
        else if (command.StartsWith(Common.CmdPrefixGotoLabel, StringComparison.OrdinalIgnoreCase))
        {
          string label = command.Substring(Common.CmdPrefixGotoLabel.Length);
          position = GetLabelPosition(commandList, label);
        }
        else if (command.StartsWith(Common.CmdPrefixSetVar, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitSetVarCommand(command.Substring(Common.CmdPrefixSetVar.Length));

          string variable = commands[0].Substring(Common.VariablePrefix.Length);
          string value = Common.ReplaceSpecial(commands[1]);

          variables.SetVariable(variable, value);
        }
        else if (command.Equals(Common.CmdPrefixClearVars, StringComparison.OrdinalIgnoreCase))
        {
          variables.Clear();
        }
        else if (command.StartsWith(Common.CmdPrefixSaveVars, StringComparison.OrdinalIgnoreCase))
        {
          variables.Save(command.Substring(Common.CmdPrefixSaveVars.Length));
        }
        else if (command.StartsWith(Common.CmdPrefixLoadVars, StringComparison.OrdinalIgnoreCase))
        {
          variables.Load(command.Substring(Common.CmdPrefixLoadVars.Length));
        }
        else
        {
          procCommand(command);
        }
      }
    }

    /// <summary>
    /// Returns a list of Macros in the specified folder.
    /// </summary>
    /// <param name="folder">The folder.</param>
    /// <param name="commandPrefix">Add the command prefix to each list item.</param>
    /// <returns>string[] of Macros.</returns>
    public static string[] GetMacroList(string folder, bool commandPrefix)
    {
      string[] files = Directory.GetFiles(folder, '*' + Common.FileExtensionMacro);

      for (int index = 0; index < files.Length; index++)
        if (commandPrefix)
          files[index] = Common.CmdPrefixMacro + Path.GetFileNameWithoutExtension(files[index]);
        else
          files[index] = Path.GetFileNameWithoutExtension(files[index]);

      return files;
    }

    #endregion Public Methods

    #region Implementation

    /// <summary>
    /// Gets the position of label within the macro.
    /// </summary>
    /// <param name="commands">The macro commands.</param>
    /// <param name="label">The label to find.</param>
    /// <returns>The label position.</returns>
    static int GetLabelPosition(string[] commandList, string label)
    {
      for (int position = 0; position < commandList.Length; position++)
      {
        string command = commandList[position];

        if (command.StartsWith(Common.CmdPrefixLabel, StringComparison.OrdinalIgnoreCase))
        {
          string thisLabel = command.Substring(Common.CmdPrefixLabel.Length);
          if (label.Equals(thisLabel, StringComparison.OrdinalIgnoreCase))
            return position;
        }
      }

      throw new Exceptions.MacroStructureException(String.Format("Macro label not found: {0}", label));
    }

    /// <summary>
    /// Given a split If Command this method will determine if the statement evaluates true.
    /// </summary>
    /// <param name="commands">An array of arguments for the method (the output of SplitIfCommand).</param>
    /// <returns><c>true</c> if the command evaluates true, otherwise <c>false</c>.</returns>
    static bool EvaluateIfCommand(string[] commands)
    {
      int value1AsInt;
      bool value1IsInt = int.TryParse(commands[0], out value1AsInt);

      int value2AsInt;
      bool value2IsInt = int.TryParse(commands[2], out value2AsInt);

      bool comparisonResult = false;
      switch (commands[1].ToUpperInvariant())
      {
        // Use string comparison ...
        case IfEquals:      comparisonResult = commands[0].Equals(commands[2], StringComparison.OrdinalIgnoreCase);         break;
        case IfNotEqual:    comparisonResult = !commands[0].Equals(commands[2], StringComparison.OrdinalIgnoreCase);        break;
        case IfContains:    comparisonResult = commands[0].ToUpperInvariant().Contains(commands[2].ToUpperInvariant());     break;
        case IfStartsWith:  comparisonResult = commands[0].StartsWith(commands[2], StringComparison.OrdinalIgnoreCase);     break;
        case IfEndsWith:    comparisonResult = commands[0].EndsWith(commands[2], StringComparison.OrdinalIgnoreCase);       break;

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
          throw new Exceptions.CommandExecutionException(String.Format("Invalid variable comparison method: {0}", commands[1]));
      }

      return comparisonResult;
    }

    #endregion Implementation

  }

}
