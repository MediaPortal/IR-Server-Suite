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
using System.IO;
using IrssCommands;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using MediaPortal.Profile;
using MPUtils;

namespace MediaPortal.Plugins.IRSS.MPControlPlugin.InputMapper
{
  /// <summary>
  /// Remotecontrol-mapping class
  /// Expects an XML file with mappings on construction
  /// Maps button code numbers to conditions and actions
  /// </summary>
  internal class InputHandler
  {
    #region Constants

    private const string CLASS_ToggleLayerCommand = "IrssCommands.MediaPortal.CommandToggleLayer";

    #endregion Constants

    private readonly bool _basicHome;
    //private readonly List<RemoteMap> _remoteMaps = new List<RemoteMap>();
    //private const int XML_VERSION = 3;
    private InputMapping _inputMapping;

    /// <summary>
    /// Constructor: Initializes mappings from XML file
    /// </summary>
    /// <param name="deviceXmlName">Input device name</param>
    public InputHandler(string deviceXmlName)
    {
      CurrentLayer = 1;
      using (Settings xmlreader = new Settings(Config.GetFile(Config.Dir.Config, "MediaPortal.xml")))
        _basicHome = xmlreader.GetValueAsBool("general", "startbasichome", false);

      string xmlPath = GetXmlPath(deviceXmlName);

      //LoadMapping(xmlPath);
      _inputMapping = InputMapping.Load(xmlPath);
    }

    /// <summary>
    /// Mapping successful loaded
    /// </summary>
    public bool IsLoaded
    {
      get { return !ReferenceEquals(_inputMapping, null); }
    }

    /// <summary>
    /// Get current Layer (Multi-Layer support)
    /// </summary>
    public int CurrentLayer { get; set; }

    /// <summary>
    /// Get path to XML mmapping file for given device name
    /// </summary>
    /// <param name="deviceXmlName">Input device name</param>
    /// <returns>Path to XML file</returns>
    /// Possible exceptions: System.IO.FileNotFoundException
    ///                      System.Xml.XmlException
    ///                      ApplicationException("XML version mismatch")
    public string GetXmlPath(string deviceXmlName)
    {
      string path = string.Empty;
      string pathCustom = Path.Combine(MPCommon.InputDeviceMappings, deviceXmlName + ".xml");
      string pathDefault = Path.Combine(MPCommon.DefaultInputDeviceMappings, deviceXmlName + ".xml");

      if (File.Exists(pathCustom))
      {
        path = pathCustom;
        Log.Info("MAP: using custom mappings for {0}", deviceXmlName);
      }
      else if (File.Exists(pathDefault))
      {
        path = pathDefault;
        Log.Info("MAP: using default mappings for {0}", deviceXmlName);
      }
      return path;
    }

    /// <summary>
    /// Evaluates the button number, gets its mapping and executes the action
    /// </summary>
    /// <param name="btnCode">Button code (ref: XML file)</param>
    public bool MapAction(int btnCode)
    {
      return DoMapAction(btnCode.ToString());
    }

    /// <summary>
    /// Evaluates the button number, gets its mapping and executes the action
    /// </summary>
    /// <param name="btnCode">Button code (ref: XML file)</param>
    public bool MapAction(string btnCode)
    {
      return DoMapAction(btnCode);
    }

    /// <summary>
    /// Evaluates the button number, gets its mapping and executes the action with an optional paramter
    /// </summary>
    /// <param name="btnCode">Button code (ref: XML file)</param>
    public bool MapAction(int btnCode, int processID = -1)
    {
      return DoMapAction(btnCode.ToString());
    }

    /// <summary>
    /// Evaluates the button number, gets its mapping and executes the action with an optional paramter
    /// </summary>
    /// <param name="btnCode">Button code (ref: XML file)</param>
    /// <param name="processID">Process-ID for close/kill commands</param>
    public bool MapAction(string btnCode, int processID = -1)
    {
      return DoMapAction(btnCode);
    }

    /// <summary>
    /// Evaluates the button number, gets its mapping and executes the action
    /// </summary>
    /// <param name="btnCode">Button code (ref: XML file)</param>
    private bool DoMapAction(string btnCode)
    {
      if (!IsLoaded) // No mapping loaded
      {
        Log.Info("Map: No button mapping loaded");
        return false;
      }

      InputMapping.Action map = GetMapping(btnCode);
      if (ReferenceEquals(map,null)) return false;
      
      Log.Debug("Mapping found: Condition={0} / Command={1}", map.Condition.UserInterfaceText, map.Command.UserInterfaceText);

      if (!string.IsNullOrEmpty(map.Sound))
        Util.Utils.PlaySound(map.Sound, false, true);

      //manually handle CommandToggleLayer
      if (map.CommandType.Equals(CLASS_ToggleLayerCommand))
      {
        CurrentLayer = CurrentLayer == 1 ? 2 : 1;
        return true;
      }

      try
      {
        map.Command.Execute(new VariableList());
      }
      catch (Exception exception)
      {
        Log.Error(exception);
        return false;
      }

      return true;
    }


    /// <summary>
    /// Get mappings for a given button code based on the current conditions
    /// </summary>
    /// <param name="btnCode">Button code (ref: XML file)</param>
    /// <returns>Mapping</returns>
    public InputMapping.Action GetMapping(string btnCode)
    {
      InputMapping.Button button = null;

      foreach (InputMapping.Remote remote in _inputMapping.Remotes)
        foreach (InputMapping.Button b in remote.Buttons)
        {
          if (btnCode == b.Code)
          {
            button = b;
            break;
          }
        }
      if (ReferenceEquals(button, null)) return null;

      foreach (InputMapping.Action action in button.Actions)
        if (action.Layer == 0 || action.Layer == CurrentLayer)
          if (action.Condition.Validate())
            return action;

      return null;
    }

    #region Nested type: Mapping

    ///// <summary>
    ///// Condition/action class
    ///// </summary>
    //public class Mapping
    //{
    //  public Mapping(int newLayer, string newCondition, string newConProperty, string newCommand,
    //                 string newCmdProperty, int newCmdKeyChar, int newCmdKeyCode, string newSound, bool newFocus)
    //  {
    //    Layer = newLayer;
    //    Condition = newCondition;
    //    ConProperty = newConProperty;
    //    Command = newCommand;
    //    CmdProperty = newCmdProperty;
    //    CmdKeyChar = newCmdKeyChar;
    //    CmdKeyCode = newCmdKeyCode;
    //    Sound = newSound;
    //    Focus = newFocus;
    //  }

    //  public int Layer { get; private set; }
    //  public string Condition { get; private set; }
    //  public string ConProperty { get; private set; }
    //  public string Command { get; private set; }
    //  public string CmdProperty { get; private set; }
    //  public int CmdKeyChar { get; private set; }
    //  public int CmdKeyCode { get; private set; }
    //  public string Sound { get; private set; }
    //  public bool Focus { get; private set; }
    //}

    //#endregion

    //#region Nested type: RemoteMap

    ///// <summary>
    ///// Button/mapping class
    ///// </summary>
    //private class RemoteMap
    //{
    //  public RemoteMap(string newCode, string newName, List<Mapping> newMappings)
    //  {
    //    Code = newCode;
    //    Name = newName;
    //    Mappings = newMappings;
    //  }

    //  public string Code { get; private set; }
    //  public string Name { get; private set; }
    //  public List<Mapping> Mappings { get; private set; }
    //}

    #endregion
  }
}