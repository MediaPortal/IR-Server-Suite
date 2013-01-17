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
using System.Xml;
using MediaPortal.Configuration;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
using Action = MediaPortal.GUI.Library.Action;

namespace MPUtils
{
  /// <summary>
  /// Contains common MediaPortal code and data.
  /// </summary>
  public static class MPCommon
  {
    #region Constants

    #region Paths

    /// <summary>
    /// Folder for Input Device data default files.
    /// </summary>
    public static readonly string DefaultInputDeviceMappings = Config.GetSubFolder(Config.Dir.Base, @"Defaults\InputDeviceMappings");

    /// <summary>
    /// Folder for Custom Input Device data files.
    /// </summary>
    public static readonly string InputDeviceMappings = Config.GetSubFolder(Config.Dir.Config, @"InputDeviceMappings");

    /// <summary>
    /// Path to the MediaPortal configuration file.
    /// </summary>
    public static readonly string MPConfigFile = Path.Combine(Config.GetFolder(Config.Dir.Config), "MediaPortal.xml");

    #endregion Paths

    #region UITexts

    // For MediaPortal ...
    public const string UITextMultiMap = "Set Multi-Mapping";
    public const string UITextPause = "Pause";
    public const string UITextSaveVars = "Save Variables";
    public const string UITextSendMPAction = "Send MediaPortal Action";
    public const string UITextSendMPMsg = "Send MediaPortal Message";
    public const string UITextSetVar = "Set Variable";

    #endregion

    private static readonly Dictionary<int, string> CustomWindowNames = new Dictionary<int, string>()
      {
        {96742, "MovingPictures"},
        {9811, "MP-TVSeries"}
      };

    #endregion

    #region Methods

    public static string GetDefaultMappingFilePath(string inputName)
    {
      return Path.Combine(DefaultInputDeviceMappings, inputName + ".xml");
    }

    public static string GetCustomMappingFilePath(string inputName)
    {
      return Path.Combine(InputDeviceMappings, inputName + ".xml");
    }

    /// <summary>
    /// Pop up a dialog in MediaPortal.
    /// </summary>
    /// <param name="heading">Dialog heading text.</param>
    /// <param name="text">Dialog body text.</param>
    /// <param name="timeout">Dialog timeout in seconds, zero for no timeout.</param>
    public static void ShowNotifyDialog(string heading, string text, int timeout)
    {
      GUIDialogNotify dlgNotify =
        (GUIDialogNotify)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_NOTIFY);
      if (dlgNotify == null)
        throw new InvalidOperationException("Failed to create GUIDialogNotify");

      dlgNotify.Reset();
      dlgNotify.ClearAll();
      dlgNotify.SetHeading(heading);
      dlgNotify.SetText(text);
      dlgNotify.TimeOut = timeout;
      dlgNotify.DoModal(GUIWindowManager.ActiveWindow);
      // TODO: Put this on a separate thread to allow caller to continue?
    }

    #endregion Methods

    #region Window ID Helpers

    public const string MISSING_FRIENDLY_WINDOW_NAME_HELPID = "UNKOWN_WINDOW_ID";

    //private readonly Array _nativeWindowsList = ;
    private static List<string> _friendlyWindowList; // being used for conditions
    private static List<string> _friendlyWindowListFiltered; // being used for commands
    //private readonly ArrayList _windowsList = new ArrayList();
    //private readonly ArrayList _windowsListFiltered = new ArrayList();

    private static void InitializeFriendlyWindowLists()
    {
      _friendlyWindowList = new List<string>();
      _friendlyWindowListFiltered = new List<string>();

      _friendlyWindowList.AddRange(CustomWindowNames.Values);
      _friendlyWindowListFiltered.AddRange(CustomWindowNames.Values);

      foreach (GUIWindow.Window wnd in Enum.GetValues(typeof(GUIWindow.Window)))
      {
        if (wnd.ToString().IndexOf("DIALOG") == -1)
          switch ((int)Enum.Parse(typeof(GUIWindow.Window), wnd.ToString()))
          {
            case (int)GUIWindow.Window.WINDOW_ARTIST_INFO:
            case (int)GUIWindow.Window.WINDOW_DIALOG_DATETIME:
            case (int)GUIWindow.Window.WINDOW_DIALOG_EXIF:
            case (int)GUIWindow.Window.WINDOW_DIALOG_FILE:
            case (int)GUIWindow.Window.WINDOW_DIALOG_FILESTACKING:
            case (int)GUIWindow.Window.WINDOW_DIALOG_MENU:
            case (int)GUIWindow.Window.WINDOW_DIALOG_MENU_BOTTOM_RIGHT:
            case (int)GUIWindow.Window.WINDOW_DIALOG_NOTIFY:
            case (int)GUIWindow.Window.WINDOW_DIALOG_OK:
            case (int)GUIWindow.Window.WINDOW_DIALOG_PROGRESS:
            case (int)GUIWindow.Window.WINDOW_DIALOG_RATING:
            case (int)GUIWindow.Window.WINDOW_DIALOG_SELECT:
            case (int)GUIWindow.Window.WINDOW_DIALOG_SELECT2:
            case (int)GUIWindow.Window.WINDOW_DIALOG_TEXT:
            case (int)GUIWindow.Window.WINDOW_DIALOG_TVGUIDE:
            case (int)GUIWindow.Window.WINDOW_DIALOG_YES_NO:
            case (int)GUIWindow.Window.WINDOW_INVALID:
            case (int)GUIWindow.Window.WINDOW_MINI_GUIDE:
            case (int)GUIWindow.Window.WINDOW_TV_CROP_SETTINGS:
            case (int)GUIWindow.Window.WINDOW_MUSIC:
            case (int)GUIWindow.Window.WINDOW_MUSIC_COVERART_GRABBER_RESULTS:
            case (int)GUIWindow.Window.WINDOW_MUSIC_INFO:
            case (int)GUIWindow.Window.WINDOW_OSD:
            case (int)GUIWindow.Window.WINDOW_TOPBAR:
            case (int)GUIWindow.Window.WINDOW_TVOSD:
            case (int)GUIWindow.Window.WINDOW_TVZAPOSD:
            case (int)GUIWindow.Window.WINDOW_VIDEO_ARTIST_INFO:
            case (int)GUIWindow.Window.WINDOW_VIDEO_INFO:
            case (int)GUIWindow.Window.WINDOW_VIRTUAL_KEYBOARD:
              break;
            default:
              _friendlyWindowListFiltered.Add(GetFriendlyWindowName(wnd));
              break;
          }
        _friendlyWindowList.Add(GetFriendlyWindowName(wnd));
      }
      
      _friendlyWindowList.Sort();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Window list for conditions</returns>
    public static string[] GetFrientlyWindowList()
    {
      if (ReferenceEquals(_friendlyWindowList, null))
        InitializeFriendlyWindowLists();

      return _friendlyWindowList.ToArray();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Window list for commands</returns>
    public static string[] GetFrientlyWindowListFiltered()
    {
      if (ReferenceEquals(_friendlyWindowListFiltered, null))
        InitializeFriendlyWindowLists();

      return _friendlyWindowListFiltered.ToArray();
    }

    public static string GetFriendlyWindowName(GUIWindow.Window window)
    {
      return GetFriendlyWindowName((int)window);
    }

    public static string GetFriendlyWindowName(int window)
    {
      if (CustomWindowNames.ContainsKey(window))
        return CustomWindowNames[window];

#warning switch to TryParse as soon as MP1 switched to .NET 4
      GUIWindow.Window eWindow;
      try
      {
        // try to get the name from int value
        eWindow = (GUIWindow.Window)window;
      }
      catch
      {
        // if failed to parse, return input value
        return window.ToString();
      }

      //test if eWindow is a integer only or a enum-text
      if (eWindow.ToString().Equals(window.ToString()))
        return window.ToString();

      return GetFriendlyName(eWindow.ToString());
    }

    public static int GetWindowID(string friendlyName)
    {
      foreach (KeyValuePair<int, string> pair in CustomWindowNames)
      {
        if (pair.Value.Equals(friendlyName))
          return pair.Key;
      }

      GUIWindow.Window window = (GUIWindow.Window)Enum.Parse(typeof(GUIWindow.Window), "WINDOW_" + friendlyName.Replace(' ', '_').ToUpper());
      return (int)window;
    }

    #endregion

    public static string GetFriendlyActionName(string action)
    {
#warning switch to TryParse as soon as MP1 switched to .NET 4
      Action.ActionType actionType;
      try
      {
        // try to get the name from int value
        actionType = (Action.ActionType)Enum.Parse(typeof(Action.ActionType), action, true);
      }
      catch
      {
        // if failed to parse, return input value
        return action;
      }

      return GetFriendlyActionName(actionType);
    }

    public static string GetFriendlyActionName(Action.ActionType actionType)
    {
      return GetFriendlyName(Enum.GetName(typeof (Action.ActionType), actionType));
    }

    private static string GetFriendlyName(string name)
    {
      if ((name.IndexOf("ACTION") != -1) || (name.IndexOf("WINDOW") != -1))
        name = name.Substring(7);

      bool upcase = true;
      string newName = String.Empty;

      foreach (char c in name)
      {
        if (c == '_')
        {
          newName += " ";
          upcase = true;
        }
        else if (upcase)
        {
          newName += c.ToString();
          upcase = false;
        }
        else
        {
          newName += c.ToString().ToLower();
        }
      }

      CleanAbbreviation(ref newName, "TV");
      CleanAbbreviation(ref newName, "DVD");
      CleanAbbreviation(ref newName, "UI");
      CleanAbbreviation(ref newName, "Guide");
      CleanAbbreviation(ref newName, "MSN");
      CleanAbbreviation(ref newName, "OSD");
      CleanAbbreviation(ref newName, "LCD");
      CleanAbbreviation(ref newName, "EPG");
      CleanAbbreviation(ref newName, "DVBC");
      CleanAbbreviation(ref newName, "DVBS");
      CleanAbbreviation(ref newName, "DVBT");

      return newName;
    }

    private static string GetActionName(string friendlyName)
    {
      string actionName = String.Empty;

      try
      {
        if (Enum.Parse(typeof(Action.ActionType), "ACTION_" + friendlyName.Replace(' ', '_').ToUpper()) != null)
          actionName = "ACTION_" + friendlyName.Replace(' ', '_').ToUpper();
      }
      catch (ArgumentException)
      {
        if (Enum.Parse(typeof(Action.ActionType), friendlyName.Replace(' ', '_').ToUpper()) != null)
          actionName = friendlyName.Replace(' ', '_').ToUpper();
      }

      return actionName;
    }

    private static void CleanAbbreviation(ref string name, string abbreviation)
    {
      int index = name.ToUpper().IndexOf(abbreviation.ToUpper());
      if (index != -1)
        name = name.Substring(0, index) + abbreviation + name.Substring(index + abbreviation.Length);
    }

    #region MP Plugin Helpers
    
    public static List<string> GetAvailablePlugins()
    {
      List<string> pluginList = new List<string>();

      string path = Config.GetFile(Config.Dir.Config, "MediaPortal.xml");
      if (!File.Exists(path))
      {
        pluginList.Add("Music");
        pluginList.Add("Video");
        return pluginList;
      }

      XmlDocument doc = new XmlDocument();
      doc.Load(path);


      //TreeNode remoteNode = new TreeNode(nodeRemote.Attributes["family"].Value);
      //remoteNode.Tag = new Data("REMOTE", null, nodeRemote.Attributes["family"].Value);
      //XmlNodeList listButtons = nodeRemote.SelectNodes("button");
      //foreach (XmlNode nodeButton in listButtons)
      //{
      //  TreeNode buttonNode = new TreeNode(nodeButton.Attributes["name"].Value);
      //  buttonNode.Tag = new Data("BUTTON", nodeButton.Attributes["name"].Value, nodeButton.Attributes["code"].Value);
      //  remoteNode.Nodes.Add(buttonNode);

      XmlNode plugins = null;
      XmlNodeList listSections = doc.DocumentElement.SelectNodes("/profile/section");
      foreach (XmlNode nodeSection in listSections)
        if (nodeSection.Attributes["name"].Value == "plugins")
        {
          plugins = nodeSection;
          break;
        }

      if (plugins == null)
      {
        pluginList.Add("Music");
        pluginList.Add("Video");
        return pluginList;
      }

      foreach (XmlNode nodePlugin in plugins.ChildNodes)
        pluginList.Add(nodePlugin.Attributes["name"].Value);

      return pluginList;
    }

    #endregion MP Plugin Helpers
  }
}