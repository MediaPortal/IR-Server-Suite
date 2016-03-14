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
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using MediaPortal.Player;
using MediaPortal.Profile;
using MediaPortal.Util;
using MPUtils;
using Action = MediaPortal.GUI.Library.Action;

namespace MediaPortal.Plugins.IRSS.MPControlPlugin.InputMapper
{
  /// <summary>
  /// Remotecontrol-mapping class
  /// Expects an XML file with mappings on construction
  /// Maps button code numbers to conditions and actions
  /// </summary>
  internal class InputHandler
  {
    private readonly bool _basicHome;
    private int _currentLayer = 1;
    private bool _isLoaded;
    private readonly List<RemoteMap> _remoteMaps = new List<RemoteMap>();
    private const int XML_VERSION = 3;

    /// <summary>
    /// Constructor: Initializes mappings from XML file
    /// </summary>
    /// <param name="deviceXmlName">Input device name</param>
    public InputHandler(string deviceXmlName)
    {
      using (Settings xmlreader = new Settings(Config.GetFile(Config.Dir.Config, "MediaPortal.xml")))
        _basicHome = xmlreader.GetValueAsBool("general", "startbasichome", false);

      string xmlPath = GetXmlPath(deviceXmlName);
      LoadMapping(xmlPath);
    }

    /// <summary>
    /// Mapping successful loaded
    /// </summary>
    public bool IsLoaded
    {
      get { return _isLoaded; }
    }

    /// <summary>
    /// Get current Layer (Multi-Layer support)
    /// </summary>
    public int CurrentLayer
    {
      get { return _currentLayer; }
      set { _currentLayer = value; }
    }


    /// <summary>
    /// Get version of XML mapping file 
    /// </summary>
    /// <param name="xmlPath">Path to XML file</param>
    /// Possible exceptions: System.Xml.XmlException
    public int GetXmlVersion(string xmlPath)
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(xmlPath);
      return Convert.ToInt32(doc.DocumentElement.SelectSingleNode("/mappings").Attributes["version"].Value);
    }


    /// <summary>
    /// Check if XML file exists and version is current
    /// </summary>
    /// <param name="xmlPath">Path to XML file</param>
    /// Possible exceptions: System.IO.FileNotFoundException
    ///                      System.Xml.XmlException
    ///                      ApplicationException("XML version mismatch")
    public bool CheckXmlFile(string xmlPath)
    {
      if (!File.Exists(xmlPath) || (GetXmlVersion(xmlPath) != XML_VERSION))
        return false;
      return true;
    }


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
      string pathCustom = Path.Combine(MPCommon.CustomInputDevice, deviceXmlName + ".xml");
      string pathDefault = Path.Combine(MPCommon.CustomInputDefault, deviceXmlName + ".xml");

      if (File.Exists(pathCustom) && CheckXmlFile(pathCustom))
      {
        path = pathCustom;
        Log.Info("MAP: using custom mappings for {0}", deviceXmlName);
      }
      else if (File.Exists(pathDefault) && CheckXmlFile(pathDefault))
      {
        path = pathDefault;
        Log.Info("MAP: using default mappings for {0}", deviceXmlName);
      }
      return path;
    }


    /// <summary>
    /// Load mapping from XML file
    /// </summary>
    /// <param name="xmlPath">Path to XML file</param>
    public void LoadMapping(string xmlPath)
    {
      if (xmlPath != string.Empty)
      {
        _remoteMaps.Clear();
        XmlDocument doc = new XmlDocument();
        doc.Load(xmlPath);
        XmlNodeList listButtons = doc.DocumentElement.SelectNodes("/mappings/remote/button");
        foreach (XmlNode nodeButton in listButtons)
        {
          string name = nodeButton.Attributes["name"].Value;
          string value = nodeButton.Attributes["code"].Value;

          List<Mapping> mappings = new List<Mapping>();
          XmlNodeList listActions = nodeButton.SelectNodes("action");
          foreach (XmlNode nodeAction in listActions)
          {
            int cmdKeyChar = 0;
            int cmdKeyCode = 0;
            string condition = nodeAction.Attributes["condition"].Value.ToUpper();
            string conProperty = nodeAction.Attributes["conproperty"].Value;
            // conProperty of  PLUGIN is case sensitive, chefkoch
            if (condition != "PLUGIN")
              conProperty = conProperty.ToUpper();
            string command = nodeAction.Attributes["command"].Value.ToUpper();
            string cmdProperty = nodeAction.Attributes["cmdproperty"].Value.ToUpper();
            if ((command == "ACTION") && (cmdProperty == "93"))
            {
              cmdKeyChar = Convert.ToInt32(nodeAction.Attributes["cmdkeychar"].Value);
              cmdKeyCode = Convert.ToInt32(nodeAction.Attributes["cmdkeycode"].Value);
            }
            string sound = string.Empty;
            XmlAttribute soundAttribute = nodeAction.Attributes["sound"];
            if (soundAttribute != null)
              sound = soundAttribute.Value;
            bool focus = false;
            XmlAttribute focusAttribute = nodeAction.Attributes["focus"];
            if (focusAttribute != null)
              focus = Convert.ToBoolean(focusAttribute.Value);
            int layer = Convert.ToInt32(nodeAction.Attributes["layer"].Value);
            Mapping conditionMap = new Mapping(layer, condition, conProperty, command, cmdProperty, cmdKeyChar,
                                               cmdKeyCode, sound, focus);
            mappings.Add(conditionMap);
          }
          RemoteMap remoteMap = new RemoteMap(value, name, mappings);
          _remoteMaps.Add(remoteMap);
        }
        _isLoaded = true;
      }
    }


    /// <summary>
    /// Evaluates the button number, gets its mapping and executes the action
    /// </summary>
    /// <param name="btnCode">Button code (ref: XML file)</param>
    public bool MapAction(int btnCode)
    {
      return DoMapAction(btnCode.ToString(), -1);
    }

    /// <summary>
    /// Evaluates the button number, gets its mapping and executes the action
    /// </summary>
    /// <param name="btnCode">Button code (ref: XML file)</param>
    public bool MapAction(string btnCode)
    {
      return DoMapAction(btnCode, -1);
    }


    /// <summary>
    /// Evaluates the button number, gets its mapping and executes the action with an optional paramter
    /// </summary>
    /// <param name="btnCode">Button code (ref: XML file)</param>
    /// <param name="processID">Process-ID for close/kill commands</param>
    public bool MapAction(int btnCode, int processID)
    {
      return DoMapAction(btnCode.ToString(), processID);
    }


    /// <summary>
    /// Evaluates the button number, gets its mapping and executes the action with an optional paramter
    /// </summary>
    /// <param name="btnCode">Button code (ref: XML file)</param>
    /// <param name="processID">Process-ID for close/kill commands</param>
    public bool MapAction(string btnCode, int processID)
    {
      return DoMapAction(btnCode, processID);
    }

    private int StopPlayback(int p1, int p2, object d)
    {
      //Log.Debug("gibman StopPlayback {0}", GUIWindowManager.ActiveWindow);
      // we have to save the fullscreen status of the tv3 plugin for later use for the lastactivemodulefullscreen feature.
      //bool currentmodulefullscreen = (GUIWindowManager.ActiveWindow == (int)GUIWindow.Window.WINDOW_TVFULLSCREEN || GUIWindowManager.ActiveWindow == (int)GUIWindow.Window.WINDOW_FULLSCREEN_MUSIC || GUIWindowManager.ActiveWindow == (int)GUIWindow.Window.WINDOW_FULLSCREEN_VIDEO || GUIWindowManager.ActiveWindow == (int)GUIWindow.Window.WINDOW_FULLSCREEN_TELETEXT);
      //GUIPropertyManager.SetProperty("#currentmodulefullscreenstate", Convert.ToString(currentmodulefullscreen));
      g_Player.Stop();
      return 0;
    }


    /// <summary>
    /// Evaluates the button number, gets its mapping and executes the action
    /// </summary>
    /// <param name="btnCode">Button code (ref: XML file)</param>
    /// <param name="processID">Process-ID for close/kill commands</param>
    private bool DoMapAction(string btnCode, int processID)
    {
      if (!_isLoaded) // No mapping loaded
      {
        Log.Info("Map: No button mapping loaded");
        return false;
      }
      Mapping map = null;
      map = GetMapping(btnCode);
      if (map == null)
        return false;

      Log.Debug("{0} / {1} / {2} / {3}", map.Condition, map.ConProperty, map.Command, map.CmdProperty);

      Action action;
      if (map.Sound != string.Empty) // && !g_Player.Playing)
        Util.Utils.PlaySound(map.Sound, false, true);
      if (map.Focus && !GUIGraphicsContext.HasFocus)
      {
        GUIGraphicsContext.ResetLastActivity();
        GUIMessage msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_GETFOCUS, 0, 0, 0, 0, 0, null);
        GUIWindowManager.SendThreadMessage(msg);
        return true;
      }
      switch (map.Command)
      {
        case "ACTION": // execute Action x
          Key key = new Key(map.CmdKeyChar, map.CmdKeyCode);

          Log.Debug("Executing: key {0} / {1} / Action: {2} / {3}", map.CmdKeyChar, map.CmdKeyCode, map.CmdProperty,
                   ((Action.ActionType) Convert.ToInt32(map.CmdProperty)).ToString());

          action = new Action(key, (Action.ActionType) Convert.ToInt32(map.CmdProperty), 0, 0);
          GUIGraphicsContext.OnAction(action);
          break;
        case "KEY": // send Key x
          SendKeys.SendWait(map.CmdProperty);
          break;
        case "WINDOW": // activate Window x
          GUIGraphicsContext.ResetLastActivity();
          GUIMessage msg;
          if ((Convert.ToInt32(map.CmdProperty) == (int) GUIWindow.Window.WINDOW_HOME) ||
              (Convert.ToInt32(map.CmdProperty) == (int) GUIWindow.Window.WINDOW_SECOND_HOME))
          {
            if (_basicHome)
              msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_GOTO_WINDOW, 0, 0, 0,
                                   (int) GUIWindow.Window.WINDOW_SECOND_HOME, 0, null);
            else
              msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_GOTO_WINDOW, 0, 0, 0,
                                   (int) GUIWindow.Window.WINDOW_HOME, 0, null);
          }
          else
            msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_GOTO_WINDOW, 0, 0, 0, Convert.ToInt32(map.CmdProperty),
                                 0, null);

          GUIWindowManager.SendThreadMessage(msg);
          break;
        case "TOGGLE": // toggle Layer 1/2
          if (_currentLayer == 1)
            _currentLayer = 2;
          else
            _currentLayer = 1;
          break;
        case "POWER": // power down commands

          if ((map.CmdProperty == "STANDBY") || (map.CmdProperty == "HIBERNATE"))
          {
            GUIGraphicsContext.ResetLastActivity();

            //Stop all media before suspending or hibernating
            if (g_Player.Playing)
            {
              GUIWindowManager.SendThreadCallbackAndWait(StopPlayback, 0, 0, null);
            }

            // this is all handled in mediaportal.cs - OnSuspend          
            /*
            if (_basicHome)
              msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_GOTO_WINDOW, 0, 0, 0, (int)GUIWindow.Window.WINDOW_SECOND_HOME, 0, null);
            else
              msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_GOTO_WINDOW, 0, 0, 0, (int)GUIWindow.Window.WINDOW_HOME, 0, null);
            
            GUIWindowManager.SendThreadMessage(msg);             
            */
          }

          switch (map.CmdProperty)
          {
            case "EXIT":
              action = new Action(Action.ActionType.ACTION_EXIT, 0, 0);
              GUIGraphicsContext.OnAction(action);
              break;
            case "REBOOT":
              action = new Action(Action.ActionType.ACTION_REBOOT, 0, 0);
              GUIGraphicsContext.OnAction(action);
              break;
            case "SHUTDOWN":
              action = new Action(Action.ActionType.ACTION_SHUTDOWN, 0, 0);
              GUIGraphicsContext.OnAction(action);
              break;
            case "STANDBY":
              // we need a slow standby (force=false), in order to have the onsuspend method being called on mediportal.cs
              // this is needed in order to have "ShowLastActiveModule" working correctly.
              // also using force=true results in a silent non critical D3DERR_DEVICELOST exception when resuming from powerstate.
              MPControlPlugin.OnSuspend();
              WindowsController.ExitWindows(RestartOptions.Suspend, false);
              break;
            case "HIBERNATE":
              // we need a slow hibernation (force=false), in order to have the onsuspend method being called on mediportal.cs
              // this is needed in order to have "ShowLastActiveModule" working correctly.
              // also using force=true results in a silent non critical D3DERR_DEVICELOST exception when resuming from powerstate.
              MPControlPlugin.OnSuspend();
              WindowsController.ExitWindows(RestartOptions.Hibernate, false);
              break;
          }
          break;
        case "PROCESS":
          {
            GUIGraphicsContext.ResetLastActivity();
            if (processID > 0)
            {
              Process proc = Process.GetProcessById(processID);
              if (null != proc)
                switch (map.CmdProperty)
                {
                  case "CLOSE":
                    proc.CloseMainWindow();
                    break;
                  case "KILL":
                    proc.Kill();
                    break;
                }
            }
          }
          break;
        case "BLAST":
          MPControlPlugin.ProcessCommand(map.CmdProperty, true);
          break;
        default:
          return false;
      }
      return true;
    }


    /// <summary>
    /// Get mappings for a given button code based on the current conditions
    /// </summary>
    /// <param name="btnCode">Button code (ref: XML file)</param>
    /// <returns>Mapping</returns>
    public Mapping GetMapping(string btnCode)
    {
      RemoteMap button = null;
      Mapping found = null;

      foreach (RemoteMap btn in _remoteMaps)
        if (btnCode == btn.Code)
        {
          button = btn;
          break;
        }
      if (button != null)
        foreach (Mapping map in button.Mappings)
          if ((map.Layer == 0) || (map.Layer == _currentLayer))
          {
            switch (map.Condition)
            {
              case "*": // wildcard, no further condition
                found = map;
                break;
              case "WINDOW": // Window-ID = x
                if ((!GUIWindowManager.IsOsdVisible &&
                     (GUIWindowManager.ActiveWindowEx == Convert.ToInt32(map.ConProperty))) ||
                    ((int) GUIWindowManager.VisibleOsd == Convert.ToInt32(map.ConProperty)))
                  found = map;
                break;
              case "FULLSCREEN": // Fullscreen = true/false
                if ((GUIGraphicsContext.IsFullScreenVideo == Convert.ToBoolean(map.ConProperty)) &&
                    !GUIWindowManager.IsRouted && !GUIWindowManager.IsOsdVisible)
                  found = map;
                break;
              case "PLAYER": // Playing TV/DVD/general
                if (!GUIWindowManager.IsRouted)
                  switch (map.ConProperty)
                  {
                    case "TV":
                      if (g_Player.IsTimeShifting || g_Player.IsTV || g_Player.IsTVRecording)
                        found = map;
                      break;
                    case "DVD":
                      if (g_Player.IsDVD)
                        found = map;
                      break;
                    case "MUSIC":
                      if (g_Player.Playing && g_Player.IsMusic)
                      
                        found = map;
                      
                      break;
                    case "MEDIA":
                      if (g_Player.Playing)
                        found = map;
                      break;
                  }
                break;
              case "PLUGIN": // plugin name: ISetupForm.PluginName()
                if (PluginManager.IsPluginNameEnabled2(map.ConProperty))
                  found = map;
                break;
            }
            if (found != null)
              return found;
          }
      return null;
    }

    #region Nested type: Mapping

    /// <summary>
    /// Condition/action class
    /// </summary>
    public class Mapping
    {
      public Mapping(int newLayer, string newCondition, string newConProperty, string newCommand,
                     string newCmdProperty, int newCmdKeyChar, int newCmdKeyCode, string newSound, bool newFocus)
      {
        Layer = newLayer;
        Condition = newCondition;
        ConProperty = newConProperty;
        Command = newCommand;
        CmdProperty = newCmdProperty;
        CmdKeyChar = newCmdKeyChar;
        CmdKeyCode = newCmdKeyCode;
        Sound = newSound;
        Focus = newFocus;
      }

      public int Layer { get; private set; }
      public string Condition { get; private set; }
      public string ConProperty { get; private set; }
      public string Command { get; private set; }
      public string CmdProperty { get; private set; }
      public int CmdKeyChar { get; private set; }
      public int CmdKeyCode { get; private set; }
      public string Sound { get; private set; }
      public bool Focus { get; private set; }
    }

    #endregion

    #region Nested type: RemoteMap

    /// <summary>
    /// Button/mapping class
    /// </summary>
    private class RemoteMap
    {
      public RemoteMap(string newCode, string newName, List<Mapping> newMappings)
      {
        Code = newCode;
        Name = newName;
        Mappings = newMappings;
      }

      public string Code { get; private set; }
      public string Name { get; private set; }
      public List<Mapping> Mappings { get; private set; }
    }

    #endregion
  }
}