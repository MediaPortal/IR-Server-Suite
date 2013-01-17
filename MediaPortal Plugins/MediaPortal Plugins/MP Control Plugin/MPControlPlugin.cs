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
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using IrssCommands;
using IrssComms;
using IrssUtils;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using MediaPortal.Hardware;
using MediaPortal.Plugins.IRSS.MPControlPlugin.Forms;
using MediaPortal.Plugins.IRSS.MPControlPlugin.InputMapper;
using MediaPortal.Profile;
using Microsoft.Win32;
using MPUtils;

namespace MediaPortal.Plugins.IRSS.MPControlPlugin
{
  /// <summary>
  /// MediaPortal Control Plugin for IR Server.
  /// </summary>
  [PluginIcons("MediaPortal.Plugins.IRSS.MPControlPlugin.iconGreen.gif",
    "MediaPortal.Plugins.IRSS.MPControlPlugin.iconGray.gif")]
  public class MPControlPlugin : IPlugin, ISetupForm
  {
    #region Constants

    /// <summary>
    /// The plugin version string.
    /// </summary>
    internal const string PluginVersion = "MP Control Plugin 1.4.2.0 for IR Server";

    private const string ProcessCommandThreadName = "ProcessCommand";

    internal static readonly string SettingsFile = Path.Combine(IrssUtils.Common.FolderAppData,
                                                               "MP Control Plugin\\settings.xml");

    internal static readonly string EventMappingFile = Path.Combine(IrssUtils.Common.FolderAppData,
                                                                    "MP Control Plugin\\EventMapping.xml");

    internal static readonly string FolderMacros = Path.Combine(IrssUtils.Common.FolderAppData,
                                                                "MP Control Plugin\\Macro");

    internal static readonly string MultiMappingFile = Path.Combine(IrssUtils.Common.FolderAppData,
                                                                    "MP Control Plugin\\MultiMapping.xml");

    internal static readonly string RemotePresetsFolder = Path.Combine(IrssUtils.Common.FolderAppData,
                                                                       "MP Control Plugin\\Remote Presets");

    internal static readonly string RemotesFile = Path.Combine(IrssUtils.Common.FolderAppData,
                                                               "MP Control Plugin\\Remotes.xml");

    internal static readonly string[] CommandCategories = new string[]
        {
          Processor.CategoryGeneral, Processor.CategorySpecial, Processor.CategoryMediaPortal
        };

    internal static readonly string[] MacroCategories = new string[]
        {
          Processor.CategoryGeneral, Processor.CategoryIRCommands, Processor.CategoryMacros, Processor.CategoryMediaPortal,
          Processor.CategoryControl, Processor.CategoryMaths, Processor.CategoryStack, Processor.CategoryString, Processor.CategoryVariable
        };

    #endregion Constants

    #region Variables

    private static SetupForm setupForm;

    private static Client _client;
    private static InputHandler _defaultInputHandler;

    private static string _learnIRFilename;

    private static RemoteButton _mouseModeLastButton = RemoteButton.None;
    private static long _mouseModeLastButtonTicks;
    private static bool _mouseModeLeftHeld;
    private static bool _mouseModeMiddleHeld;
    private static int _mouseModeRepeatCount;
    private static bool _mouseModeRightHeld;
    private static List<InputHandler> _multiInputHandlers;

    private static int _multiMappingSet;
    private static bool _registered;

    private static MappedKeyCode[] _remoteMap;

    public MPControlPlugin()
    {
      
    }

    static MPControlPlugin()
    {
      TransceiverInformation = new IRServerInfo();
      //Config.MouseModeButton = RemoteButton.None;

      // Set directory for command plugin
      CommandProcessor = new Processor(BlastIR, TransceiverInformation.Ports);
      string dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      Processor.LibraryFolder = Path.Combine(dllPath, "Commands");
      Processor.MacroFolder = FolderMacros;
    }

    #endregion Variables

    #region Properties

    internal static Configuration Config { get; set; }

    /// <summary>
    /// Gets the event mappings.
    /// </summary>
    /// <value>The event mappings.</value>
    internal static EventMappings EventMappings { get; private set; }

    /// <summary>
    /// Gets the multi maps.
    /// </summary>
    /// <value>The multi maps.</value>
    internal static MultiMappings MultiMappings { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether in configuration.
    /// </summary>
    /// <value><c>true</c> if in configuration; otherwise, <c>false</c>.</value>
    internal static bool InConfiguration { get; set; }

    internal static ClientMessageSink HandleMessage { get; set; }

    internal static IRServerInfo TransceiverInformation { get; private set; }

    internal static Processor CommandProcessor { get; private set; }

    /// <summary>
    /// Gets a value indicating whether MediaPortal has basic home enabled.
    /// </summary>
    /// <value><c>true</c> if MediaPortal has basic home enabled; otherwise, <c>false</c>.</value>
    internal static bool MP_BasicHome { get; private set; }

    #endregion Properties

    #region IPlugin methods

    /// <summary>
    /// Starts this instance.
    /// </summary>
    public void Start()
    {
      InConfiguration = false;

      Log.Info("MPControlPlugin: Starting ({0})", PluginVersion);

      // Load basic settings
      LoadMediaPortalSettings();
      Config = Configuration.Load(SettingsFile);

      // Load the remote button mappings
      _remoteMap = LoadRemoteMap(RemotesFile);

      // Load input handler
      LoadDefaultMapping();

      // Load multi-mappings
      if (Config.MultiMappingEnabled)
        MultiMappings = MultiMappings.Load(MultiMappingFile);

      IPAddress serverIP = Network.GetIPFromName(Config.ServerHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, Server.DefaultPort);

      if (!StartClient(endPoint))
        Log.Error("MPControlPlugin: Failed to start local comms, IR input and IR blasting is disabled for this session");

      // Load the event mapper mappings
      if (Config.EventMapperEnabled)
      {
        EventMappings = EventMappings.Load(EventMappingFile);

        MapEvent(MappedEvent.MappingEvent.MediaPortal_Start);

        // Register with MediaPortal to receive GUI Messages ...
        GUIWindowManager.Receivers += OnMessage;
      }

      // Register for Power State message ...
      //SystemEvents.SessionEnding += new SessionEndingEventHandler(SystemEvents_SessionEnding);
      SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;

      Log.Debug("MPControlPlugin: Started");
    }

    /// <summary>
    /// Stops this instance.
    /// </summary>
    public void Stop()
    {
      SystemEvents.PowerModeChanged -= SystemEvents_PowerModeChanged;

      if (Config.EventMapperEnabled)
      {
        GUIWindowManager.Receivers -= OnMessage;

        MapEvent(MappedEvent.MappingEvent.MediaPortal_Stop);
      }

      StopClient();

      _defaultInputHandler = null;

      if (Config.MultiMappingEnabled)
        for (int i = 0; i < _multiInputHandlers.Count; i++)
          _multiInputHandlers[i] = null;

      Log.Debug("MPControlPlugin: Stopped");
    }

    #endregion IPlugin methods

    #region ISetupForm methods

    /// <summary>
    /// Determines whether this plugin can be enabled.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this plugin can be enabled; otherwise, <c>false</c>.
    /// </returns>
    public bool CanEnable()
    {
      return true;
    }

    /// <summary>
    /// Determines whether this plugin has setup.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this plugin has setup; otherwise, <c>false</c>.
    /// </returns>
    public bool HasSetup()
    {
      return true;
    }

    /// <summary>
    /// Gets the plugin name.
    /// </summary>
    /// <returns>The plugin name.</returns>
    public string PluginName()
    {
      return "MP Control Plugin for IR Server";
    }

    /// <summary>
    /// Defaults enabled.
    /// </summary>
    /// <returns>true if this plugin is enabled by default, otherwise false.</returns>
    public bool DefaultEnabled()
    {
      return true;
    }

    /// <summary>
    /// Gets the window id.
    /// </summary>
    /// <returns>The window id.</returns>
    public int GetWindowId()
    {
      return 0;
    }

    /// <summary>
    /// Gets the plugin author.
    /// </summary>
    /// <returns>The plugin author.</returns>
    public string Author()
    {
      return "and-81";
    }

    /// <summary>
    /// Gets the description of the plugin.
    /// </summary>
    /// <returns>The plugin description.</returns>
    public string Description()
    {
      return "This plugin uses the IR Server to replace MediaPortal's native remote control support";
    }

    /// <summary>
    /// Shows the plugin configuration.
    /// </summary>
    public void ShowPlugin()
    {
      LoadMediaPortalSettings();
      Config = Configuration.Load(SettingsFile);
      EventMappings = EventMappings.Load(EventMappingFile);
      MultiMappings = MultiMappings.Load(MultiMappingFile);

      InConfiguration = true;
      Log.Debug("MPControlPlugin: ShowPlugin()");

      setupForm = new SetupForm();
      if (setupForm.ShowDialog() == DialogResult.OK)
      {
        Configuration.Save(Config, SettingsFile);
        EventMappings.Save(EventMappings, EventMappingFile);
        MultiMappings.Save(MultiMappings, MultiMappingFile);
      }

      StopClient();

      Log.Debug("MPControlPlugin: ShowPlugin() - End");
    }

    /// <summary>
    /// Gets the home screen details for the plugin.
    /// </summary>
    /// <param name="strButtonText">The button text.</param>
    /// <param name="strButtonImage">The button image.</param>
    /// <param name="strButtonImageFocus">The button image focus.</param>
    /// <param name="strPictureImage">The picture image.</param>
    /// <returns>true if the plugin can be seen, otherwise false.</returns>
    public bool GetHome(out string strButtonText, out string strButtonImage, out string strButtonImageFocus,
                        out string strPictureImage)
    {
      strButtonText = strButtonImage = strButtonImageFocus = strPictureImage = String.Empty;
      return false;
    }

    #endregion ISetupForm methods

    #region Implementation

    /// <summary>
    /// Handles the mouse mode.
    /// </summary>
    /// <param name="button">The button pressed.</param>
    /// <returns>true if handled successfully, otherwise false.</returns>
    private static bool HandleMouseMode(RemoteButton button)
    {
      if (button == Config.MouseModeButton)
      {
        Config.MouseModeActive = !Config.MouseModeActive; // Toggle Mouse Mode

        string notifyMessage;

        if (Config.MouseModeActive)
        {
          notifyMessage = "Mouse Mode is now ON";
        }
        else
        {
          notifyMessage = "Mouse Mode is now OFF";

          if (_mouseModeLeftHeld)
            Mouse.Button(Mouse.MouseEvents.LeftUp);

          if (_mouseModeRightHeld)
            Mouse.Button(Mouse.MouseEvents.RightUp);

          if (_mouseModeMiddleHeld)
            Mouse.Button(Mouse.MouseEvents.MiddleUp);

          _mouseModeLeftHeld = false;
          _mouseModeRightHeld = false;
          _mouseModeMiddleHeld = false;
        }

        MPCommon.ShowNotifyDialog("Mouse Mode", notifyMessage, 2);

        Log.Debug("MPControlPlugin: {0}", notifyMessage);

        return true;
      }
      else if (Config.MouseModeActive)
      {
        // Determine repeat count ...
        long ticks = DateTime.Now.Ticks;
        if (button != _mouseModeLastButton || new TimeSpan(ticks - _mouseModeLastButtonTicks).Milliseconds >= 500)
          _mouseModeRepeatCount = 0;
        else
          _mouseModeRepeatCount++;

        _mouseModeLastButtonTicks = ticks;
        _mouseModeLastButton = button;


        int distance = Config.MouseModeStep;

        if (Config.MouseModeAcceleration)
          distance += (2 * _mouseModeRepeatCount);

        switch (button)
        {
          case RemoteButton.Up:
            Mouse.Move(0, -distance, false);
            return true;

          case RemoteButton.Down:
            Mouse.Move(0, distance, false);
            return true;

          case RemoteButton.Left:
            Mouse.Move(-distance, 0, false);
            return true;

          case RemoteButton.Right:
            Mouse.Move(distance, 0, false);
            return true;

          case RemoteButton.Replay: // Left Single-Click
            if (_mouseModeLeftHeld)
              Mouse.Button(Mouse.MouseEvents.LeftUp);

            if (_mouseModeRightHeld)
              Mouse.Button(Mouse.MouseEvents.RightUp);

            if (_mouseModeMiddleHeld)
              Mouse.Button(Mouse.MouseEvents.MiddleUp);

            _mouseModeLeftHeld = false;
            _mouseModeRightHeld = false;
            _mouseModeMiddleHeld = false;

            Mouse.Button(Mouse.MouseEvents.LeftDown);
            Mouse.Button(Mouse.MouseEvents.LeftUp);
            return true;

          case RemoteButton.Skip: // Right Single-Click
            if (_mouseModeLeftHeld)
              Mouse.Button(Mouse.MouseEvents.LeftUp);

            if (_mouseModeRightHeld)
              Mouse.Button(Mouse.MouseEvents.RightUp);

            if (_mouseModeMiddleHeld)
              Mouse.Button(Mouse.MouseEvents.MiddleUp);

            _mouseModeLeftHeld = false;
            _mouseModeRightHeld = false;
            _mouseModeMiddleHeld = false;

            Mouse.Button(Mouse.MouseEvents.RightDown);
            Mouse.Button(Mouse.MouseEvents.RightUp);
            return true;

          case RemoteButton.Play: // Middle Single-Click
            if (_mouseModeLeftHeld)
              Mouse.Button(Mouse.MouseEvents.LeftUp);

            if (_mouseModeRightHeld)
              Mouse.Button(Mouse.MouseEvents.RightUp);

            if (_mouseModeMiddleHeld)
              Mouse.Button(Mouse.MouseEvents.MiddleUp);

            _mouseModeLeftHeld = false;
            _mouseModeRightHeld = false;
            _mouseModeMiddleHeld = false;

            Mouse.Button(Mouse.MouseEvents.MiddleDown);
            Mouse.Button(Mouse.MouseEvents.MiddleUp);
            return true;

          case RemoteButton.Ok: // Double-Click (Left)
            if (_mouseModeLeftHeld)
              Mouse.Button(Mouse.MouseEvents.LeftUp);

            if (_mouseModeRightHeld)
              Mouse.Button(Mouse.MouseEvents.RightUp);

            if (_mouseModeMiddleHeld)
              Mouse.Button(Mouse.MouseEvents.MiddleUp);

            _mouseModeLeftHeld = false;
            _mouseModeRightHeld = false;
            _mouseModeMiddleHeld = false;

            Mouse.Button(Mouse.MouseEvents.LeftDown);
            Mouse.Button(Mouse.MouseEvents.LeftUp);

            Mouse.Button(Mouse.MouseEvents.LeftDown);
            Mouse.Button(Mouse.MouseEvents.LeftUp);
            return true;

          case RemoteButton.Back: // Left Click & Hold
            if (_mouseModeRightHeld)
              Mouse.Button(Mouse.MouseEvents.RightUp);

            if (_mouseModeMiddleHeld)
              Mouse.Button(Mouse.MouseEvents.MiddleUp);

            if (_mouseModeLeftHeld)
              Mouse.Button(Mouse.MouseEvents.LeftUp);
            else
              Mouse.Button(Mouse.MouseEvents.LeftDown);

            _mouseModeLeftHeld = !_mouseModeLeftHeld;
            _mouseModeRightHeld = false;
            _mouseModeMiddleHeld = false;
            return true;

          case RemoteButton.Info: // Right Click & Hold
            if (_mouseModeLeftHeld)
              Mouse.Button(Mouse.MouseEvents.LeftUp);

            if (_mouseModeMiddleHeld)
              Mouse.Button(Mouse.MouseEvents.MiddleUp);

            if (_mouseModeRightHeld)
              Mouse.Button(Mouse.MouseEvents.RightUp);
            else
              Mouse.Button(Mouse.MouseEvents.RightDown);

            _mouseModeRightHeld = !_mouseModeRightHeld;
            _mouseModeLeftHeld = false;
            _mouseModeMiddleHeld = false;
            return true;

          case RemoteButton.Stop: // Middle Click & Hold
            if (_mouseModeLeftHeld)
              Mouse.Button(Mouse.MouseEvents.LeftUp);

            if (_mouseModeRightHeld)
              Mouse.Button(Mouse.MouseEvents.RightUp);

            if (_mouseModeMiddleHeld)
              Mouse.Button(Mouse.MouseEvents.MiddleUp);
            else
              Mouse.Button(Mouse.MouseEvents.MiddleDown);

            _mouseModeMiddleHeld = !_mouseModeMiddleHeld;
            _mouseModeLeftHeld = false;
            _mouseModeRightHeld = false;
            return true;

          case RemoteButton.ChannelUp: // Scroll Up
            Mouse.Scroll(Mouse.ScrollDir.Up);
            return true;

          case RemoteButton.ChannelDown: // Scroll Down
            Mouse.Scroll(Mouse.ScrollDir.Down);
            return true;
        }
      }

      return false;
    }

    /// <summary>
    /// Handles remote buttons received.
    /// </summary>
    /// <param name="deviceName">Name of the device.</param>
    /// <param name="keyCode">The remote button.</param>
    private static void RemoteHandler(string deviceName, string keyCode)
    {
      // If user has stipulated that MP must have focus to recognize commands ...
      if (Config.RequireFocus && !GUIGraphicsContext.HasFocus)
        return;

      foreach (MappedKeyCode mapping in _remoteMap)
      {
        if (!mapping.KeyCode.Equals(keyCode, StringComparison.OrdinalIgnoreCase))
          continue;

        if (Config.MultiMappingEnabled && mapping.Button == Config.MultiMappingButton)
        {
          ChangeMultiMapping();
          return;
        }

        if (Config.MouseModeEnabled)
          if (HandleMouseMode(mapping.Button))
            return;

        // Get & execute Mapping
        bool gotMapped;
        if (Config.MultiMappingEnabled)
          gotMapped = _multiInputHandlers[_multiMappingSet].MapAction((int) mapping.Button);
        else
          gotMapped = _defaultInputHandler.MapAction((int) mapping.Button);

        if (gotMapped)
          Log.Debug("MPControlPlugin: Command \"{0}\" mapped to remote", mapping.Button);
        else
          Log.Debug("MPControlPlugin: Command \"{0}\" not mapped to remote", mapping.Button);

        return;
      }
      
      Log.Debug("MPControlPlugin: keyCode \"{0}\" was not handled", keyCode);

      return;
    }

    private static void CommsFailure(object obj)
    {
      Exception ex = obj as Exception;

      if (ex != null)
        Log.Error("MPControlPlugin: Communications failure: {0}", ex.ToString());
      else
        Log.Error("MPControlPlugin: Communications failure");

      StopClient();

      Log.Warn("MPControlPlugin: Attempting communications restart ...");

      IPAddress serverIP = Network.GetIPFromName(Config.ServerHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, Server.DefaultPort);

      StartClient(endPoint);
    }

    private static void Connected(object obj)
    {
      Log.Info("MPControlPlugin: Connected to server");

      IrssMessage message = new IrssMessage(MessageType.RegisterClient, MessageFlags.Request);
      _client.Send(message);
    }

    private static void Disconnected(object obj)
    {
      Log.Warn("MPControlPlugin: Communications with server has been lost");

      Thread.Sleep(1000);
    }

    internal static bool StartClient(IPEndPoint endPoint)
    {
      if (_client != null)
        return false;

      ClientMessageSink sink = ReceivedMessage;

      _client = new Client(endPoint, sink);
      _client.CommsFailureCallback = CommsFailure;
      _client.ConnectCallback = Connected;
      _client.DisconnectCallback = Disconnected;

      if (_client.Start())
        return true;

      _client = null;
      return false;
    }

    internal static void StopClient()
    {
      if (_client == null)
        return;

      _client.Dispose();
      _client = null;

      _registered = false;
    }

    private static void ReceivedMessage(IrssMessage received)
    {
      Log.Debug("MPControlPlugin: Received Message \"{0}\"", received.Type);

      try
      {
        switch (received.Type)
        {
          case MessageType.RemoteEvent:
            if (!InConfiguration)
            {
              string deviceName = received.MessageData[IrssMessage.DEVICE_NAME] as string;
              string keyCode = received.MessageData[IrssMessage.KEY_CODE] as string;

              RemoteHandler(deviceName, keyCode);
            }
            break;

          case MessageType.BlastIR:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              Log.Debug("MPControlPlugin: Blast successful");
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              Log.Warn("MPControlPlugin: Failed to blast IR command");
            }
            break;

          case MessageType.RegisterClient:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              TransceiverInformation = IRServerInfo.FromBytes(received.GetDataAsBytes());
              _registered = true;

              Log.Debug("MPControlPlugin: Registered to IR Server");
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              _registered = false;
              Log.Warn("MPControlPlugin: IR Server refused to register");
            }
            UpdateCommandProcessor();
            break;

          case MessageType.LearnIR:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              Log.Debug("MPControlPlugin: Learned IR Successfully");

              byte[] dataBytes = received.GetDataAsBytes();

              using (FileStream file = File.Create(_learnIRFilename))
                file.Write(dataBytes, 0, dataBytes.Length);
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              Log.Error("MPControlPlugin: Failed to learn IR command");
            }
            else if ((received.Flags & MessageFlags.Timeout) == MessageFlags.Timeout)
            {
              Log.Error("MPControlPlugin: Learn IR command timed-out");
            }

            _learnIRFilename = null;
            break;

          case MessageType.ServerShutdown:
            Log.Warn("MPControlPlugin: IR Server Shutdown - Plugin disabled until IR Server returns");
            _registered = false;
            UpdateCommandProcessor();
            break;

          case MessageType.Error:
            _learnIRFilename = null;
            Log.Error("MPControlPlugin: Received error: {0}", received.GetDataAsString());
            break;
        }

        if (HandleMessage != null)
          HandleMessage(received);
      }
      catch (Exception ex)
      {
        _learnIRFilename = null;
        Log.Error(ex);
      }
    }

    /// <summary>
    /// OnMessage is used for event mapping.
    /// </summary>
    /// <param name="msg">Message.</param>
    private void OnMessage(GUIMessage msg)
    {
      MapEvent(msg);
    }

    /// <summary>
    /// Loads the remote map.
    /// </summary>
    /// <param name="remoteFile">The remote file.</param>
    /// <returns>Remote map.</returns>
    private static MappedKeyCode[] LoadRemoteMap(string remoteFile)
    {
      List<MappedKeyCode> remoteMap = new List<MappedKeyCode>();

      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(remoteFile);

        XmlNodeList listRemotes = doc.DocumentElement.SelectNodes("remote");
        foreach (XmlNode nodeRemote in listRemotes)
        {
          //string remoteName = nodeRemote.Attributes["name"].Value;

          XmlNodeList listButtons = nodeRemote.SelectNodes("button");
          foreach (XmlNode nodeButton in listButtons)
          {
            string remoteButton = nodeButton.Attributes["name"].Value;

            XmlNodeList listIRCodes = nodeButton.SelectNodes("code");
            foreach (XmlNode nodeCode in listIRCodes)
            {
              string remoteCode = nodeCode.Attributes["value"].Value;
              remoteMap.Add(new MappedKeyCode(remoteButton, remoteCode));
            }
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("MPControlPlugin - LoadRemoteMap(): {0}", ex.ToString());
      }

      return remoteMap.ToArray();
    }

    #region Event Mapping

    /// <summary>
    /// Run the event mapper over the supplied GUIMessage.
    /// </summary>
    /// <param name="msg">MediaPortal Message to run through the event mapper.</param>
    private static void MapEvent(GUIMessage msg)
    {
      MappedEvent.MappingEvent eventType = MappedEvent.GetEventType(msg.Message);
      if (eventType == MappedEvent.MappingEvent.None) return;

      foreach (MappedEvent mappedEvent in EventMappings.Events)
      {
        if (mappedEvent.EventType != eventType) continue;
        bool matched = true;

        if (mappedEvent.MatchParam)
        {
          string paramValueString = mappedEvent.ParamValue;
          int paramValueInt = -1;
          int.TryParse(mappedEvent.ParamValue, out paramValueInt);
          bool paramValueBool = false;
          bool.TryParse(mappedEvent.ParamValue, out paramValueBool);

          switch (mappedEvent.Param)
          {
            case "Label 1":
              matched = (msg.Label.Equals(paramValueString, StringComparison.OrdinalIgnoreCase));
              break;
            case "Label 2":
              matched = (msg.Label2.Equals(paramValueString, StringComparison.OrdinalIgnoreCase));
              break;
            case "Label 3":
              matched = (msg.Label3.Equals(paramValueString, StringComparison.OrdinalIgnoreCase));
              break;
            case "Label 4":
              matched = (msg.Label4.Equals(paramValueString, StringComparison.OrdinalIgnoreCase));
              break;
            case "Parameter 1":
              matched = (msg.Param1 == paramValueInt);
              break;
            case "Parameter 2":
              matched = (msg.Param2 == paramValueInt);
              break;
            case "Parameter 3":
              matched = (msg.Param3 == paramValueInt);
              break;
            case "Parameter 4":
              matched = (msg.Param4 == paramValueInt);
              break;
            case "Sender Control ID":
              matched = (msg.SenderControlId == paramValueInt);
              break;
            case "Send To Target Window":
              matched = (msg.SendToTargetWindow == paramValueBool);
              break;
            case "Target Control ID":
              matched = (msg.TargetControlId == paramValueInt);
              break;
            case "Target Window ID":
              matched = (msg.TargetWindowId == paramValueInt);
              break;
            default:
              matched = false;
              break;
          }
        }

        if (!matched) continue;

        if (ReferenceEquals(mappedEvent.Command, null))
        {
          Log.Warn("MPControlPlugin: Mapped command to event ({0}) is not available. (Command: {1})",
                   Enum.GetName(typeof(MappedEvent.MappingEvent), eventType), mappedEvent.EventType);
          continue;
        }

        Log.Debug("MPControlPlugin: Event Mapper - Event \"{0}\"",
                  Enum.GetName(typeof(MappedEvent.MappingEvent), eventType));

        try
        {
          ProcessCommand(mappedEvent.Command);
        }
        catch (Exception ex)
        {
          Log.Error("MPControlPlugin: Failed to execute Event Mapper command \"{0}\" - {1}", mappedEvent.EventType,
                    ex.ToString());
        }
      }
    }

    /// <summary>
    /// Run the event mapper over the supplied MappedEvent type.
    /// </summary>
    /// <param name="eventType">MappedEvent to run through the event mapper.</param>
    private static void MapEvent(MappedEvent.MappingEvent eventType)
    {
      foreach (MappedEvent mappedEvent in EventMappings.Events)
      {
        if (mappedEvent.EventType != eventType) continue;
        if (mappedEvent.MatchParam) continue;

        Log.Debug("MPControlPlugin: Event Mapper - Event \"{0}\"",
                  Enum.GetName(typeof(MappedEvent.MappingEvent), eventType));

        if (ReferenceEquals(mappedEvent.Command, null))
        {
          Log.Warn("MPControlPlugin: Mapped command to event ({0}) is not available. (Command: {1})",
                   Enum.GetName(typeof(MappedEvent.MappingEvent), eventType), mappedEvent.EventType);
          continue;
        }

        try
        {
          ProcessCommand(mappedEvent.Command);
        }
        catch (Exception ex)
        {
          Log.Error("MPControlPlugin: Failed to execute Event Mapper command \"{0}\" - {1}", mappedEvent.EventType,
                    ex.ToString());
        }
      }
    }

    #endregion

    #region Multi Mapping

    /// <summary>
    /// Changes the multi mapping.
    /// </summary>
    private static void ChangeMultiMapping()
    {
      // Cycle through Multi-Mappings ...
      _multiMappingSet = (_multiMappingSet + 1) % MultiMappings.Count;

      // Show the mapping set name on screen ...
      string setName = MultiMappings[_multiMappingSet];

      Log.Debug("MPControlPlugin: Multi-Mapping has cycled to \"{0}\"", setName);

      MPCommon.ShowNotifyDialog("Multi-Mapping", setName, 2);
    }

    /// <summary>
    /// Changes the multi mapping.
    /// </summary>
    /// <param name="multiMapping">The multi mapping.</param>
    private static void ChangeMultiMapping(string multiMapping)
    {
      Log.Debug("MPControlPlugin: ChangeMultiMapping: {0}", multiMapping);

      if (multiMapping.Equals("TOGGLE", StringComparison.OrdinalIgnoreCase))
      {
        ChangeMultiMapping();
        return;
      }

      for (int index = 0; index < MultiMappings.Count; index++)
      {
        if (MultiMappings[index].Equals(multiMapping, StringComparison.CurrentCultureIgnoreCase))
        {
          _multiMappingSet = index;

          // Show the mapping set name on screen ...
          string setName = MultiMappings[_multiMappingSet];

          Log.Debug("MPControlPlugin: Multi-Mapping has changed to \"{0}\"", setName);

          MPCommon.ShowNotifyDialog("Multi-Mapping", setName, 2);
          return;
        }
      }

      Log.Warn("MPControlPlugin: Could not find Multi-Mapping \"{0}\"", multiMapping);
    }

    #endregion

    /// <summary>
    /// Handles the PowerModeChanged event of the SystemEvents control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="Microsoft.Win32.PowerModeChangedEventArgs"/> instance containing the event data.</param>
    private static void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
    {
      switch (e.Mode)
      {
        case PowerModes.Resume:
          OnResume();
          break;

        case PowerModes.Suspend:
          OnSuspend();
          break;
      }
    }

    /// <summary>
    /// Loads the default remote button input handler.
    /// </summary>
    internal static void LoadDefaultMapping()
    {
      _defaultInputHandler = new InputHandler("MPControlPlugin");

      if (!_defaultInputHandler.IsLoaded)
        Log.Error("MPControlPlugin: Error loading default mapping file");
    }

    /// <summary>
    /// Call this when entering standby to ensure that the Event Mapper is informed.
    /// </summary>
    internal static void OnSuspend()
    {
      if (!InConfiguration && Config.EventMapperEnabled)
        MapEvent(MappedEvent.MappingEvent.PC_Suspend);
    }

    /// <summary>
    /// Call this when leaving standby to ensure the Event Mapper is informed.
    /// </summary>
    internal static void OnResume()
    {
      if (!InConfiguration && Config.EventMapperEnabled)
        MapEvent(MappedEvent.MappingEvent.PC_Resume);
    }

    /// <summary>
    /// Learn an IR command.
    /// </summary>
    /// <param name="fileName">File to place learned IR command in (absolute path).</param>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    internal static bool LearnIR(string fileName)
    {
      try
      {
        if (String.IsNullOrEmpty(fileName))
        {
          Log.Error("MPControlPlugin: Null or Empty file name for LearnIR()");
          return false;
        }

        if (!_registered)
        {
          Log.Warn("MPControlPlugin: Not registered to an active IR Server");
          return false;
        }

        if (_learnIRFilename != null)
        {
          Log.Warn("MPControlPlugin: Already trying to learn an IR command");
          return false;
        }

        _learnIRFilename = fileName;

        IrssMessage message = new IrssMessage(MessageType.LearnIR, MessageFlags.Request);
        _client.Send(message);
      }
      catch (Exception ex)
      {
        _learnIRFilename = null;
        Log.Error(ex);
        return false;
      }

      return true;
    }

    /// <summary>
    /// Blast an IR command.
    /// </summary>
    /// <param name="fileName">File to blast (absolute path).</param>
    /// <param name="port">Port to blast to.</param>
    internal static void BlastIR(string fileName, string port)
    {
      Log.Debug("MPControlPlugin - BlastIR(): {0}, {1}", fileName, port);

      if (!_registered)
        throw new InvalidOperationException("Cannot Blast, not registered to an active IR Server");

      using (FileStream file = File.OpenRead(fileName))
      {
        if (file.Length == 0)
          throw new IOException(String.Format("Cannot Blast. IR file \"{0}\" has no data, possible IR learn failure",
                                              fileName));

        byte[] outData = new byte[4 + port.Length + file.Length];

        BitConverter.GetBytes(port.Length).CopyTo(outData, 0);
        Encoding.ASCII.GetBytes(port).CopyTo(outData, 4);

        file.Read(outData, 4 + port.Length, (int) file.Length);

        IrssMessage message = new IrssMessage(MessageType.BlastIR, MessageFlags.Request, outData);
        _client.Send(message);
      }
    }



    ///// <summary>
    ///// Given a command this method processes the request accordingly.
    ///// Throws exceptions only if run synchronously, if async exceptions are logged instead.
    ///// </summary>
    ///// <param name="command">Command string to process.</param>
    ///// <param name="async">Process command asynchronously?</param>
    //internal static void ProcessCommand(string command, bool async = false)
    //{
    //  if (command == null)
    //    throw new ArgumentNullException("command");

    //  if (async)
    //  {
    //    try
    //    {
    //      Thread newThread = new Thread(ProcCommand);
    //      newThread.Name = ProcessCommandThreadName;
    //      newThread.IsBackground = true;
    //      newThread.Start(command);
    //    }
    //    catch (Exception ex)
    //    {
    //      IrssLog.Error(ex);
    //    }
    //  }
    //  else
    //  {
    //    ProcCommand(command);
    //  }
    //}

    /// <summary>
    /// Given a command this method processes the request accordingly.
    /// Throws exceptions only if run synchronously, if async exceptions are logged instead.
    /// </summary>
    /// <param name="command">Command to process.</param>
    /// <param name="async">Process command asynchronously?</param>
    internal static void ProcessCommand(Command command, bool async = false)
    {
      CommandProcessor.Execute(command, async);
      //if (command == null)
      //  throw new ArgumentNullException("command");

      //if (async)
      //{
      //  try
      //  {
      //    Thread newThread = new Thread(ProcCommand);
      //    newThread.Name = ProcessCommandThreadName;
      //    newThread.IsBackground = true;
      //    newThread.Start(command);
      //  }
      //  catch (Exception ex)
      //  {
      //    IrssLog.Error(ex);
      //  }
      //}
      //else
      //{
      //  ProcCommand(command);
      //}
    }

//    /// <summary>
//    /// Used by ProcessCommand to actually handle the command, if it is a string, otherwise it will call the other overload ProcCommand using the object as type Command.
//    /// Can be called Synchronously or as a Parameterized Thread.
//    /// </summary>
//    /// <param name="commandObj">Command object to process. This could be a string or Command.</param>
//    private static void ProcCommand(object commandObj)
//    {
//      if (commandObj == null)
//        throw new ArgumentNullException("commandObj");

//      try
//      {
//        Command command = commandObj as Command;
//        if (command != null)
//        {
//          ProcCommand(command);
//          return;
//        }

//        string strCommand = commandObj as string;
//        // is obj is not a command nor a string, stop here
//        if (strCommand == null) return;


//        // check for serialized Command
//        try
//        {
//          command = Processor.CreateCommand(strCommand);
//        }
//        catch (Exception)
//        {
//          // catch all exception, as the provided text might not be a serialized command
//        }
//        if (command != null)
//        {
//          ProcCommand(command);
//          return;
//        }

//#warning fixme multimap
//        //else if (command.StartsWith(IrssUtils.Common.CmdPrefixMultiMap, StringComparison.OrdinalIgnoreCase))
//        //{
//        //  string multiMapping = command.Substring(IrssUtils.Common.CmdPrefixMultiMap.Length);
//        //  if (_inConfiguration)
//        //    MessageBox.Show(multiMapping, IrssUtils.Common.UITextMultiMap, MessageBoxButtons.OK, MessageBoxIcon.Information);
//        //  else
//        //    ChangeMultiMapping(multiMapping);
//        //}
//#warning fixme input layer
//        //else if (command.StartsWith(IrssUtils.Common.CmdPrefixInputLayer, StringComparison.OrdinalIgnoreCase))
//        //{
//        //  if (_inConfiguration)
//        //  {
//        //    MessageBox.Show("Cannot toggle the input handler layer while in configuration", IrssUtils.Common.UITextInputLayer,
//        //                    MessageBoxButtons.OK, MessageBoxIcon.Information);
//        //  }
//        //  else
//        //  {
//        //    InputHandler inputHandler;

//        //    if (_multiMappingEnabled)
//        //      inputHandler = _multiInputHandlers[_multiMappingSet];
//        //    else
//        //      inputHandler = _defaultInputHandler;

//        //    if (inputHandler.CurrentLayer == 1)
//        //      inputHandler.CurrentLayer = 2;
//        //    else
//        //      inputHandler.CurrentLayer = 1;
//        //  }
//        //}
//        //else
//        //{
//        //  throw new ArgumentException(String.Format("Cannot process unrecognized command \"{0}\"", command),
//        //                              "commandObj");
//        //}
//      }
//      catch (Exception ex)
//      {
//        if (!String.IsNullOrEmpty(Thread.CurrentThread.Name) &&
//            Thread.CurrentThread.Name.Equals(ProcessCommandThreadName, StringComparison.OrdinalIgnoreCase))
//          Log.Error(ex);
//        else
//          throw;
//      }
//    }

    ///// <summary>
    ///// Used by ProcessCommand to actually handle the command, if it is a Command.
    ///// Can be called Synchronously or as a Parameterized Thread.
    ///// </summary>
    ///// <param name="command">Command to process.</param>
    //private static void ProcCommand(Command command)
    //{
    //  if (command == null)
    //    throw new ArgumentNullException("command");

    //  if (InConfiguration)
    //  {
    //    MessageBox.Show(command.UserDisplayText, command.UserInterfaceText, MessageBoxButtons.OK, MessageBoxIcon.Information);
    //    return;
    //  }

    //  //if (command is MacroCommand)
    //  //  IrssMacro.ExecuteMacro(command as MacroCommand, _variables, ProcCommand);
    //  //else
    //  command.Execute(new VariableList());
    //}

    /// <summary>
    /// Loads the settings only. No Mappings.
    /// </summary>
    private static void LoadMediaPortalSettings()
    {
      // load settings from MP
      try
      {
        using (Settings xmlreader = new Settings(MPCommon.MPConfigFile))
        {
          // MediaPortal settings ...
          MP_BasicHome = xmlreader.GetValueAsBool("general", "startbasichome", false);
        }
      }
      catch (Exception ex)
      {
        Log.Error(ex);
      }
    }

    private static void UpdateCommandProcessor()
    {
      //CommandProcessor = null;
      //if (!_registered) return;

      CommandProcessor = new Processor(BlastIR, TransceiverInformation.Ports);

      if (!ReferenceEquals(setupForm, null) && !ReferenceEquals(setupForm._macroPanel, null))
        setupForm._macroPanel.SetCommandProcessor(CommandProcessor);
    }

    #endregion Implementation
  }
}