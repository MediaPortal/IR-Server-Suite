#region Copyright (C) 2005-2009 Team MediaPortal

// Copyright (C) 2005-2009 Team MediaPortal
// http://www.team-mediaportal.com
// 
// This Program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2, or (at your option)
// any later version.
// 
// This Program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with GNU Make; see the file COPYING.  If not, write to
// the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
// http://www.gnu.org/copyleft/gpl.html

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace IrssUtils
{
  public enum WindowStateAction
  {
    Hide,
    Minimize,
    Maximize,
    Restore,
    Unhide
  }

  public enum WindowTargetType
  {
    Active,
    Application,
    Class,
    Window
  }

  #region Delegates

  /// <summary>
  /// Provides a delegate to call to test a command.
  /// </summary>
  /// <param name="fileName">Full file path to the IR Command file.</param>
  /// <param name="port">IR Blaster port to transmit on.</param>
  public delegate void BlastIrDelegate(string fileName, string port);

  /// <summary>
  /// Provides a delegate to call to learn a new IR Command.
  /// </summary>
  /// <param name="fileName">Full file path to the IR Command file.</param>
  /// <returns>Successfully started IR learn process.</returns>
  public delegate bool LearnIrDelegate(string fileName);

  #endregion Delegates

  /// <summary>
  /// Common code class.
  /// </summary>
  public static class Common
  {
    #region Constants

    #region Folders

    /// <summary>
    /// IR Server Suite "Application Data" folder location (includes trailing '\')
    /// </summary>
    public static readonly string FolderAppData =
      Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "IR Server Suite");

    /// <summary>
    /// IR Server Suite "IR Commands" folder location (includes trailing '\')
    /// </summary>
    public static readonly string FolderIRCommands = Path.Combine(FolderAppData, "IR Commands");

    /// <summary>
    /// IR Server Suite "Logs" folder location (includes trailing '\')
    /// </summary>
    public static readonly string FolderIrssLogs = Path.Combine(FolderAppData, "Logs");

    /// <summary>
    /// IR Server Suite "Set Top Boxes" folder location (includes trailing '\')
    /// </summary>
    public static readonly string FolderSTB = Path.Combine(FolderAppData, "Set Top Boxes");

    #endregion Folders

    #region File Extensions

    /// <summary>
    /// File extension for IR Commands.
    /// </summary>
    public const string FileExtensionIR = ".IR";

    /// <summary>
    /// File extension for Macros.
    /// </summary>
    public const string FileExtensionMacro = ".Macro";

    /// <summary>
    /// File extension for stored Variable Lists.
    /// </summary>
    public const string FileExtensionVariableList = ".VariableList";

    #endregion File Extensions

    #region SendWindowMessages

    public const string WM_ShowTranslatorOSD = "Show Translator OSD";
    public const string WM_ShowTrayIcon = "Show Tray Icon";

    #endregion SendWindowMessages

    #region Commands

    public const string CLASS_BlastIRCommand = "IrssCommands.CommandBlastIR";
    public const string CLASS_CallMacroCommand = "IrssCommands.CommandCallMacro";

    public const string CLASS_RunCommand = "IrssCommands.General.RunCommand";
    public const string CLASS_EjectCommand = "IrssCommands.General.EjectCommand";

    public const string CLASS_HibernateCommand = "IrssCommands.General.CommandHibernate";
    public const string CLASS_LogOffCommand = "IrssCommands.General.CommandLogOff";
    public const string CLASS_RebootCommand = "IrssCommands.General.CommandReboot";
    public const string CLASS_ShutdownCommand = "IrssCommands.General.ShutdownCommand";
    public const string CLASS_StandByCommand = "IrssCommands.General.StandByCommand";
    

    #endregion

    #region Strings

    /// <summary>
    /// Variables must be prefixed with this string.
    /// </summary>
    public const string VariablePrefix = "var_";

    #region Command Prefixes

    public const string CmdPrefixBlast = "Blast: ";
    public const string CmdPrefixClearVars = "Clear Variables";
    public const string CmdPrefixCommand = "Command: ";

    public const string CmdPrefixDisplayMode = "Display Mode: ";
    public const string CmdPrefixDisplayPower = "Display Power: ";
    public const string CmdPrefixExit = "Exit MediaPortal";
    public const string CmdPrefixFocus = "Get Focus";

    // Macro Commands ...
    public const string CmdPrefixGotoLabel = "Goto Label: ";
    public const string CmdPrefixGotoScreen = "Goto: ";
    public const string CmdPrefixIf = "If: ";
    public const string CmdPrefixInputLayer = "Toggle Input Layer";
    public const string CmdPrefixLabel = "Label: ";
    public const string CmdPrefixLoadVars = "Load Variables: ";
    public const string CmdPrefixMouseMode = "Mouse Mode: ";

    // For MediaPortal ...
    public const string CmdPrefixMultiMap = "Multi-Mapping: ";
    public const string CmdPrefixPause = "Pause: ";
    public const string CmdPrefixSaveVars = "Save Variables: ";
    public const string CmdPrefixSendMPAction = "MediaPortal Action: ";
    public const string CmdPrefixSendMPMsg = "MediaPortal Message: ";
    public const string CmdPrefixSetVar = "Set Variable: ";
    public const string CmdPrefixSTB = "STB: ";

    #endregion Command Prefixes

    #region User Interface Text

    public const string UITextClearVars = "Clear Variables";
    public const string UITextDisplayMode = "Display Mode";
    public const string UITextDisplayPower = "Display Power";
    public const string UITextExit = "Exit MediaPortal";
    public const string UITextFocus = "Get Focus";

    // Macro Commands ...
    public const string UITextGotoLabel = "Goto Label";
    public const string UITextGotoScreen = "Go To Screen";
    public const string UITextIf = "If Statement";
    public const string UITextInputLayer = "Toggle Input Handler Layer";
    public const string UITextLabel = "Insert Label";
    public const string UITextLoadVars = "Load Variables";
    public const string UITextMouseMode = "Set Mouse Mode";

    #endregion User Interface Text

    #endregion Strings

    #endregion Constants

    #region Methods

    /// <summary>
    /// Determines the validity of a given filename.
    /// </summary>
    /// <param name="fileName">File name to validate.</param>
    /// <returns>true if the name is valid; otherwise, false.</returns>
    public static bool IsValidFileName(string fileName)
    {
      if (String.IsNullOrEmpty(fileName))
        return false;

      Regex validate =
        new Regex("^(?!^(PRN|AUX|CLOCK\\$|NUL|CON|COM\\d|LPT\\d|\\..*)(\\..+)?$)[^\\x00-\\x1f\\\\?*:\\\";|/]+$",
                  RegexOptions.IgnoreCase);
      return validate.IsMatch(fileName);
    }

    /// <summary>
    /// Converts a string into a byte arrays
    /// </summary>
    /// <param name="input">The input to process.</param>
    /// <returns>Processed input string.</returns>
    public static Byte[] GetStringAsByte(string input)
    {
      if (String.IsNullOrEmpty(input))
        return null;

      List<byte> output = new List<byte>();

      foreach (char currentChar in input)
        output.Add((byte)currentChar);

      return output.ToArray();
    }

    #endregion Methods
  }
}