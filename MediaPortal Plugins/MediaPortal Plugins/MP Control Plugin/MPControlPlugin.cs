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
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using IrssComms;
using IrssUtils;
using IrssUtils.Forms;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using MediaPortal.Hardware;
using MediaPortal.Profile;
using Microsoft.Win32;
using MPUtils;

namespace MediaPortal.Plugins
{
  [PluginIcons("MediaPortal.Plugins.MPControlPlugin.IRSS.iconGreen.gif",
    "MediaPortal.Plugins.MPControlPlugin.IRSS.iconGray.gif")]

  /// <summary>
    /// MediaPortal Control Plugin for IR Server.
    /// </summary>
  public class MPControlPlugin : IPlugin, ISetupForm
  {
    #region Constants

    /// <summary>
    /// The plugin version string.
    /// </summary>
    internal const string PluginVersion = "MP Control Plugin 1.4.2.0 for IR Server";

    private const string ProcessCommandThreadName = "ProcessCommand";

    internal static readonly string EventMappingFile = Path.Combine(Common.FolderAppData,
                                                                    "MP Control Plugin\\EventMapping.xml");

    internal static readonly string FolderMacros = Path.Combine(Common.FolderAppData, "MP Control Plugin\\Macro");

    internal static readonly string MultiMappingFile = Path.Combine(Common.FolderAppData,
                                                                    "MP Control Plugin\\MultiMapping.xml");

    internal static readonly string RemotePresetsFolder = Path.Combine(Common.FolderAppData,
                                                                       "MP Control Plugin\\Remote Presets");

    internal static readonly string RemotesFile = Path.Combine(Common.FolderAppData, "MP Control Plugin\\Remotes.xml");

    #endregion Constants

    #region Variables

    private static Client _client;
    private static InputHandler _defaultInputHandler;
    private static bool _eventMapperEnabled;
    private static List<MappedEvent> _eventMappings;
    private static ClientMessageSink _handleMessage;

    private static bool _inConfiguration;
    private static IRServerInfo _irServerInfo = new IRServerInfo();

    private static string _learnIRFilename;

    private static bool _mouseModeAcceleration;

    private static bool _mouseModeActive;
    private static RemoteButton _mouseModeButton = RemoteButton.None;
    private static bool _mouseModeEnabled;
    private static RemoteButton _mouseModeLastButton = RemoteButton.None;
    private static long _mouseModeLastButtonTicks;
    private static bool _mouseModeLeftHeld;
    private static bool _mouseModeMiddleHeld;
    private static int _mouseModeRepeatCount;
    private static bool _mouseModeRightHeld;
    private static int _mouseModeStep;
    private static bool _mpBasicHome;
    private static List<InputHandler> _multiInputHandlers;

    private static RemoteButton _multiMappingButton;
    private static bool _multiMappingEnabled;
    private static int _multiMappingSet;
    private static string[] _multiMaps;
    private static bool _registered;

    private static MappedKeyCode[] _remoteMap;
    private static bool _requireFocus;
    private static string _serverHost;

    #endregion Variables

    #region Properties

    internal static string ServerHost
    {
      get { return _serverHost; }
      set { _serverHost = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether MediaPortal will require focus to handle input.
    /// </summary>
    /// <value><c>true</c> if requires focus; otherwise, <c>false</c>.</value>
    internal static bool RequireFocus
    {
      get { return _requireFocus; }
      set { _requireFocus = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether multi mapping is enabled.
    /// </summary>
    /// <value><c>true</c> if multi mapping is enabled; otherwise, <c>false</c>.</value>
    internal static bool MultiMappingEnabled
    {
      get { return _multiMappingEnabled; }
      set { _multiMappingEnabled = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the event mapper is enabled.
    /// </summary>
    /// <value><c>true</c> if the event mapper is enabled; otherwise, <c>false</c>.</value>
    internal static bool EventMapperEnabled
    {
      get { return _eventMapperEnabled; }
      set { _eventMapperEnabled = value; }
    }

    /// <summary>
    /// Gets or sets the mouse mode button.
    /// </summary>
    /// <value>The mouse mode button.</value>
    internal static RemoteButton MouseModeButton
    {
      get { return _mouseModeButton; }
      set { _mouseModeButton = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether mouse mode is enabled.
    /// </summary>
    /// <value><c>true</c> if mouse mode is enabled; otherwise, <c>false</c>.</value>
    internal static bool MouseModeEnabled
    {
      get { return _mouseModeEnabled; }
      set { _mouseModeEnabled = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether mouse mode is active.
    /// </summary>
    /// <value><c>true</c> if mouse mode is active; otherwise, <c>false</c>.</value>
    internal static bool MouseModeActive
    {
      get { return _mouseModeActive; }
      set { _mouseModeActive = value; }
    }

    /// <summary>
    /// Gets or sets the mouse mode step distance.
    /// </summary>
    /// <value>The mouse mode step distance.</value>
    internal static int MouseModeStep
    {
      get { return _mouseModeStep; }
      set { _mouseModeStep = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether mouse mode acceleration is enabled.
    /// </summary>
    /// <value>
    /// <c>true</c> if mouse mode acceleration is enabled; otherwise, <c>false</c>.
    /// </value>
    internal static bool MouseModeAcceleration
    {
      get { return _mouseModeAcceleration; }
      set { _mouseModeAcceleration = value; }
    }

    /// <summary>
    /// Gets the event mappings.
    /// </summary>
    /// <value>The event mappings.</value>
    internal static List<MappedEvent> EventMappings
    {
      get { return _eventMappings; }
    }

    /// <summary>
    /// Gets or sets the multi mapping button.
    /// </summary>
    /// <value>The multi mapping button.</value>
    internal static RemoteButton MultiMappingButton
    {
      get { return _multiMappingButton; }
      set { _multiMappingButton = value; }
    }

    /// <summary>
    /// Gets the multi maps.
    /// </summary>
    /// <value>The multi maps.</value>
    internal static string[] MultiMaps
    {
      get { return _multiMaps; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether in configuration.
    /// </summary>
    /// <value><c>true</c> if in configuration; otherwise, <c>false</c>.</value>
    internal static bool InConfiguration
    {
      get { return _inConfiguration; }
      set { _inConfiguration = value; }
    }

    internal static ClientMessageSink HandleMessage
    {
      get { return _handleMessage; }
      set { _handleMessage = value; }
    }

    internal static IRServerInfo TransceiverInformation
    {
      get { return _irServerInfo; }
    }

    /// <summary>
    /// Gets a value indicating whether MediaPortal has basic home enabled.
    /// </summary>
    /// <value><c>true</c> if MediaPortal has basic home enabled; otherwise, <c>false</c>.</value>
    internal static bool MP_BasicHome
    {
      get { return _mpBasicHome; }
    }

    #endregion Properties

    #region IPlugin methods

    /// <summary>
    /// Starts this instance.
    /// </summary>
    public void Start()
    {
      _inConfiguration = false;

      Log.Info("MPControlPlugin: Starting ({0})", PluginVersion);

      // Load basic settings
      LoadSettings();

      // Load the remote button mappings
      _remoteMap = LoadRemoteMap(RemotesFile);

      // Load input handler
      LoadDefaultMapping();

      // Load multi-mappings
      if (MultiMappingEnabled)
        LoadMultiMappings();

      IPAddress serverIP = Client.GetIPFromName(_serverHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, Server.DefaultPort);

      if (!StartClient(endPoint))
        Log.Error("MPControlPlugin: Failed to start local comms, IR input and IR blasting is disabled for this session");

      // Load the event mapper mappings
      if (EventMapperEnabled)
      {
        LoadEventMappings();

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

      if (EventMapperEnabled)
      {
        GUIWindowManager.Receivers -= OnMessage;

        MapEvent(MappedEvent.MappingEvent.MediaPortal_Stop);
      }

      StopClient();

      _defaultInputHandler = null;

      if (MultiMappingEnabled)
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
      try
      {
        LoadSettings();
        LoadDefaultMapping();
        LoadMultiMappings();

        _inConfiguration = true;

        Log.Debug("MPControlPlugin: ShowPlugin()");

        SetupForm setupForm = new SetupForm();
        if (setupForm.ShowDialog() == DialogResult.OK)
          SaveSettings();

        StopClient();
        
        Log.Debug("MPControlPlugin: ShowPlugin() - End");
      }
      catch (Exception ex)
      {
        Log.Error(ex);
      }
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
      if (button == MouseModeButton)
      {
        MouseModeActive = !MouseModeActive; // Toggle Mouse Mode

        string notifyMessage;

        if (MouseModeActive)
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
      else if (MouseModeActive)
      {
        // Determine repeat count ...
        long ticks = DateTime.Now.Ticks;
        if (button != _mouseModeLastButton || new TimeSpan(ticks - _mouseModeLastButtonTicks).Milliseconds >= 500)
          _mouseModeRepeatCount = 0;
        else
          _mouseModeRepeatCount++;

        _mouseModeLastButtonTicks = ticks;
        _mouseModeLastButton = button;


        int distance = MouseModeStep;

        if (MouseModeAcceleration)
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
      if (RequireFocus && !GUIGraphicsContext.HasFocus)
        return;

      foreach (MappedKeyCode mapping in _remoteMap)
      {
        if (!mapping.KeyCode.Equals(keyCode, StringComparison.OrdinalIgnoreCase))
          continue;

        if (MultiMappingEnabled && mapping.Button == MultiMappingButton)
        {
          ChangeMultiMapping();
          return;
        }

        if (MouseModeEnabled)
          if (HandleMouseMode(mapping.Button))
            return;

        // Get & execute Mapping
        bool gotMapped;
        if (MultiMappingEnabled)
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

      IPAddress serverIP = Client.GetIPFromName(_serverHost);
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
            if (!_inConfiguration)
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
              _irServerInfo = IRServerInfo.FromBytes(received.GetDataAsBytes());
              _registered = true;

              Log.Debug("MPControlPlugin: Registered to IR Server");
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              _registered = false;
              Log.Warn("MPControlPlugin: IR Server refused to register");
            }
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
            break;

          case MessageType.Error:
            _learnIRFilename = null;
            Log.Error("MPControlPlugin: Received error: {0}", received.GetDataAsString());
            break;
        }

        if (_handleMessage != null)
          _handleMessage(received);
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
    /// Changes the multi mapping.
    /// </summary>
    private static void ChangeMultiMapping()
    {
      // Cycle through Multi-Mappings ...
      _multiMappingSet = (_multiMappingSet + 1) % MultiMaps.Length;

      // Show the mapping set name on screen ...
      string setName = MultiMaps[_multiMappingSet];

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

      for (int index = 0; index < MultiMaps.Length; index++)
      {
        if (MultiMaps[index].Equals(multiMapping, StringComparison.CurrentCultureIgnoreCase))
        {
          _multiMappingSet = index;

          // Show the mapping set name on screen ...
          string setName = MultiMaps[_multiMappingSet];

          Log.Debug("MPControlPlugin: Multi-Mapping has changed to \"{0}\"", setName);

          MPCommon.ShowNotifyDialog("Multi-Mapping", setName, 2);
          return;
        }
      }

      Log.Warn("MPControlPlugin: Could not find Multi-Mapping \"{0}\"", multiMapping);
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

    /// <summary>
    /// Loads the event mappings.
    /// </summary>
    private static void LoadEventMappings()
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(EventMappingFile);

        XmlNodeList eventList = doc.DocumentElement.SelectNodes("mapping");

        _eventMappings = new List<MappedEvent>(eventList.Count);

        foreach (XmlNode item in eventList)
          EventMappings.Add(MappedEvent.FromStrings(item.Attributes["event"].Value, item.Attributes["command"].Value));
      }
      catch (Exception ex)
      {
        Log.Error(ex);
      }
    }

    /// <summary>
    /// Run the event mapper over the supplied GUIMessage.
    /// </summary>
    /// <param name="msg">MediaPortal Message to run through the event mapper.</param>
    private static void MapEvent(GUIMessage msg)
    {
      MappedEvent.MappingEvent eventType = MappedEvent.GetEventType(msg.Message);
      if (eventType == MappedEvent.MappingEvent.None)
        return;

      foreach (MappedEvent mappedEvent in EventMappings)
      {
        if (mappedEvent.EventType == eventType)
        {
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

          if (!matched)
            continue;

          Log.Debug("MPControlPlugin: Event Mapper - Event \"{0}\"",
                    Enum.GetName(typeof (MappedEvent.MappingEvent), eventType));

          try
          {
            ProcessCommand(mappedEvent.Command, false);
          }
          catch (Exception ex)
          {
            Log.Error("MPControlPlugin: Failed to execute Event Mapper command \"{0}\" - {1}", mappedEvent.EventType,
                      ex.ToString());
          }
        }
      }
    }

    /// <summary>
    /// Run the event mapper over the supplied MappedEvent type.
    /// </summary>
    /// <param name="eventType">MappedEvent to run through the event mapper.</param>
    private static void MapEvent(MappedEvent.MappingEvent eventType)
    {
      foreach (MappedEvent mappedEvent in EventMappings)
      {
        if (mappedEvent.EventType == eventType)
        {
          if (mappedEvent.MatchParam)
            continue;

          Log.Debug("MPControlPlugin: Event Mapper - Event \"{0}\"",
                    Enum.GetName(typeof (MappedEvent.MappingEvent), eventType));

          try
          {
            ProcessCommand(mappedEvent.Command, false);
          }
          catch (Exception ex)
          {
            Log.Error("MPControlPlugin: Failed to execute Event Mapper command \"{0}\" - {1}", mappedEvent.EventType,
                      ex.ToString());
          }
        }
      }
    }

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
    /// Loads multi-mappings for input handling.
    /// </summary>
    internal static void LoadMultiMappings()
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(MultiMappingFile);

      XmlNodeList mappings = doc.DocumentElement.SelectNodes("map");

      _multiInputHandlers = new List<InputHandler>(mappings.Count);
      _multiMaps = new string[mappings.Count];

      for (int index = 0; index < mappings.Count; index++)
      {
        string map = mappings.Item(index).Attributes["name"].Value;

        _multiMaps[index] = map;
        _multiInputHandlers.Add(new InputHandler(map));

        if (!_multiInputHandlers[index].IsLoaded)
          Log.Error("MPControlPlugin: Error loading default mapping file for {0}", map);
      }
    }

    /// <summary>
    /// Call this when entering standby to ensure that the Event Mapper is informed.
    /// </summary>
    internal static void OnSuspend()
    {
      if (!_inConfiguration && EventMapperEnabled)
        MapEvent(MappedEvent.MappingEvent.PC_Suspend);
    }

    /// <summary>
    /// Call this when leaving standby to ensure the Event Mapper is informed.
    /// </summary>
    internal static void OnResume()
    {
      if (!_inConfiguration && EventMapperEnabled)
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

    /// <summary>
    /// Given a command this method processes the request accordingly.
    /// Throws exceptions only if run synchronously, if async exceptions are logged instead.
    /// </summary>
    /// <param name="command">Command to process.</param>
    /// <param name="async">Process command asynchronously?</param>
    internal static void ProcessCommand(string command, bool async)
    {
      if (async)
      {
        try
        {
          Thread newThread = new Thread(ProcCommand);
          newThread.Name = ProcessCommandThreadName;
          newThread.IsBackground = true;
          newThread.Start(command);
        }
        catch (Exception ex)
        {
          Log.Error(ex);
        }
      }
      else
      {
        ProcCommand(command);
      }
    }

    /// <summary>
    /// Used by ProcessCommand to actually handle the command.
    /// Can be called Synchronously or as a Parameterized Thread.
    /// </summary>
    /// <param name="commandObj">Command string to process.</param>
    private static void ProcCommand(object commandObj)
    {
      try
      {
        if (commandObj == null)
          throw new ArgumentNullException("commandObj");

        string command = commandObj as string;

        if (String.IsNullOrEmpty(command))
          throw new ArgumentException("commandObj translates to empty or null string", "commandObj");

        if (command.StartsWith(Common.CmdPrefixMacro, StringComparison.OrdinalIgnoreCase))
        {
          string fileName = Path.Combine(FolderMacros,
                                         command.Substring(Common.CmdPrefixMacro.Length) + Common.FileExtensionMacro);
          ProcMacro(fileName);
        }
        else if (command.StartsWith(Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitBlastCommand(command.Substring(Common.CmdPrefixBlast.Length));
          BlastIR(Path.Combine(Common.FolderIRCommands, commands[0] + Common.FileExtensionIR), commands[1]);
        }
        else if (command.StartsWith(Common.CmdPrefixPause, StringComparison.OrdinalIgnoreCase))
        {
          int pauseTime = int.Parse(command.Substring(Common.CmdPrefixPause.Length));
          Thread.Sleep(pauseTime);
        }
        else if (command.StartsWith(Common.CmdPrefixRun, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitRunCommand(command.Substring(Common.CmdPrefixRun.Length));
          Common.ProcessRunCommand(commands);
        }
        else if (command.StartsWith(Common.CmdPrefixSerial, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitSerialCommand(command.Substring(Common.CmdPrefixSerial.Length));
          Common.ProcessSerialCommand(commands);
        }
        else if (command.StartsWith(Common.CmdPrefixWindowMsg, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitWindowMessageCommand(command.Substring(Common.CmdPrefixWindowMsg.Length));
          Common.ProcessWindowMessageCommand(commands);
        }
        else if (command.StartsWith(Common.CmdPrefixTcpMsg, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitTcpMessageCommand(command.Substring(Common.CmdPrefixTcpMsg.Length));
          Common.ProcessTcpMessageCommand(commands);
        }
        else if (command.StartsWith(Common.CmdPrefixHttpMsg, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitHttpMessageCommand(command.Substring(Common.CmdPrefixHttpMsg.Length));
          Common.ProcessHttpCommand(commands);
        }
        else if (command.StartsWith(Common.CmdPrefixKeys, StringComparison.OrdinalIgnoreCase))
        {
          string keyCommand = command.Substring(Common.CmdPrefixKeys.Length);
          if (_inConfiguration)
            MessageBox.Show(keyCommand, Common.UITextKeys, MessageBoxButtons.OK, MessageBoxIcon.Information);
          else
            Common.ProcessKeyCommand(keyCommand);
        }
        else if (command.StartsWith(Common.CmdPrefixMouse, StringComparison.OrdinalIgnoreCase))
        {
          string mouseCommand = command.Substring(Common.CmdPrefixMouse.Length);
          Common.ProcessMouseCommand(mouseCommand);
        }
        else if (command.StartsWith(Common.CmdPrefixEject, StringComparison.OrdinalIgnoreCase))
        {
          string ejectCommand = command.Substring(Common.CmdPrefixEject.Length);
          Common.ProcessEjectCommand(ejectCommand);
        }
        else if (command.StartsWith(Common.CmdPrefixMultiMap, StringComparison.OrdinalIgnoreCase))
        {
          string multiMapping = command.Substring(Common.CmdPrefixMultiMap.Length);
          if (_inConfiguration)
            MessageBox.Show(multiMapping, Common.UITextMultiMap, MessageBoxButtons.OK, MessageBoxIcon.Information);
          else
            ChangeMultiMapping(multiMapping);
        }
        else if (command.StartsWith(Common.CmdPrefixPopup, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitPopupCommand(command.Substring(Common.CmdPrefixPopup.Length));
          if (_inConfiguration)
            MessageBox.Show(commands[1], commands[0], MessageBoxButtons.OK, MessageBoxIcon.Information);
          else
            MPCommon.ShowNotifyDialog(commands[0], commands[1], int.Parse(commands[2]));
        }
        else if (command.StartsWith(Common.CmdPrefixGotoScreen, StringComparison.OrdinalIgnoreCase))
        {
          string screenID = command.Substring(Common.CmdPrefixGotoScreen.Length);

          if (_inConfiguration)
            MessageBox.Show(screenID, Common.UITextGotoScreen, MessageBoxButtons.OK, MessageBoxIcon.Information);
          else
            MPCommon.ProcessGoTo(screenID, _mpBasicHome);
        }
        else if (command.StartsWith(Common.CmdPrefixSendMPAction, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitSendMPActionCommand(command.Substring(Common.CmdPrefixSendMPAction.Length));
          if (_inConfiguration)
            MessageBox.Show(commands[0], Common.UITextSendMPAction, MessageBoxButtons.OK, MessageBoxIcon.Information);
          else
            MPCommon.ProcessSendMediaPortalAction(commands);
        }
        else if (command.StartsWith(Common.CmdPrefixSendMPMsg, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitSendMPMsgCommand(command.Substring(Common.CmdPrefixSendMPMsg.Length));
          if (_inConfiguration)
            MessageBox.Show(commands[0], Common.UITextSendMPMsg, MessageBoxButtons.OK, MessageBoxIcon.Information);
          else
            MPCommon.ProcessSendMediaPortalMessage(commands);
        }
        else if (command.StartsWith(Common.CmdPrefixInputLayer, StringComparison.OrdinalIgnoreCase))
        {
          if (_inConfiguration)
          {
            MessageBox.Show("Cannot toggle the input handler layer while in configuration", Common.UITextInputLayer,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
          }
          else
          {
            InputHandler inputHandler;

            if (_multiMappingEnabled)
              inputHandler = _multiInputHandlers[_multiMappingSet];
            else
              inputHandler = _defaultInputHandler;

            if (inputHandler.CurrentLayer == 1)
              inputHandler.CurrentLayer = 2;
            else
              inputHandler.CurrentLayer = 1;
          }
        }
        else if (command.StartsWith(Common.CmdPrefixExit, StringComparison.OrdinalIgnoreCase))
        {
          if (_inConfiguration)
            MessageBox.Show("Cannot exit MediaPortal in configuration", Common.UITextExit, MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
          else
            MPCommon.ExitMP();
        }
        else if (command.StartsWith(Common.CmdPrefixHibernate, StringComparison.OrdinalIgnoreCase))
        {
          if (_inConfiguration)
            MessageBox.Show("Cannot Hibernate in configuration", Common.UITextHibernate, MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
          else
            MPCommon.Hibernate();
        }
        else if (command.StartsWith(Common.CmdPrefixReboot, StringComparison.OrdinalIgnoreCase))
        {
          if (_inConfiguration)
            MessageBox.Show("Cannot Reboot in configuration", Common.UITextReboot, MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
          else
            MPCommon.Reboot();
        }
        else if (command.StartsWith(Common.CmdPrefixShutdown, StringComparison.OrdinalIgnoreCase))
        {
          if (_inConfiguration)
            MessageBox.Show("Cannot Shutdown in configuration", Common.UITextShutdown, MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
          else
            MPCommon.ShutDown();
        }
        else if (command.StartsWith(Common.CmdPrefixStandby, StringComparison.OrdinalIgnoreCase))
        {
          if (_inConfiguration)
            MessageBox.Show("Cannot enter Standby in configuration", Common.UITextStandby, MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
          else
            MPCommon.Standby();
        }
        else if (command.StartsWith(Common.CmdPrefixVirtualKB, StringComparison.OrdinalIgnoreCase))
        {
          if (_inConfiguration)
          {
            MessageBox.Show("Cannot show Virtual Keyboard in configuration", Common.UITextVirtualKB,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
          }
          else
          {
            VirtualKeyboard vk = new VirtualKeyboard();
            if (vk.ShowDialog() == DialogResult.OK)
              Keyboard.ProcessCommand(vk.TextOutput);
          }
        }
        else if (command.StartsWith(Common.CmdPrefixSmsKB, StringComparison.OrdinalIgnoreCase))
        {
          if (_inConfiguration)
          {
            MessageBox.Show("Cannot show SMS Keyboard in configuration", Common.UITextSmsKB, MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
          }
          else
          {
            SmsKeyboard sms = new SmsKeyboard();
            if (sms.ShowDialog() == DialogResult.OK)
              Keyboard.ProcessCommand(sms.TextOutput);
          }
        }
        else
        {
          throw new ArgumentException(String.Format("Cannot process unrecognized command \"{0}\"", command),
                                      "commandObj");
        }
      }
      catch (Exception ex)
      {
        if (!String.IsNullOrEmpty(Thread.CurrentThread.Name) &&
            Thread.CurrentThread.Name.Equals(ProcessCommandThreadName, StringComparison.OrdinalIgnoreCase))
          Log.Error(ex);
        else
          throw;
      }
    }

    /// <summary>
    /// Called by ProcCommand to process the supplied Macro file.
    /// </summary>
    /// <param name="fileName">Macro file to process (absolute path).</param>
    private static void ProcMacro(string fileName)
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(fileName);

      XmlNodeList commandSequence = doc.DocumentElement.SelectNodes("item");

      foreach (XmlNode item in commandSequence)
        ProcCommand(item.Attributes["command"].Value);
    }

    /// <summary>
    /// Returns a list of Macros.
    /// </summary>
    /// <param name="commandPrefix">Add the command prefix to each list item.</param>
    /// <returns>string[] of Macros.</returns>
    internal static string[] GetMacroList(bool commandPrefix)
    {
      string[] files = Directory.GetFiles(FolderMacros, '*' + Common.FileExtensionMacro);
      string[] list = new string[files.Length];

      int i = 0;
      foreach (string file in files)
      {
        if (commandPrefix)
          list[i++] = Common.CmdPrefixMacro + Path.GetFileNameWithoutExtension(file);
        else
          list[i++] = Path.GetFileNameWithoutExtension(file);
      }

      return list;
    }

    /// <summary>
    /// Returns a combined list of IR Commands and Macros.
    /// </summary>
    /// <param name="commandPrefix">Add the command prefix to each list item.</param>
    /// <returns>string[] of IR Commands and Macros.</returns>
    internal static string[] GetFileList(bool commandPrefix)
    {
      string[] MacroFiles = Directory.GetFiles(FolderMacros, '*' + Common.FileExtensionMacro);
      string[] IRFiles = Directory.GetFiles(Common.FolderIRCommands, '*' + Common.FileExtensionIR);
      string[] list = new string[MacroFiles.Length + IRFiles.Length];

      int i = 0;
      foreach (string file in MacroFiles)
      {
        if (commandPrefix)
          list[i++] = Common.CmdPrefixMacro + Path.GetFileNameWithoutExtension(file);
        else
          list[i++] = Path.GetFileNameWithoutExtension(file);
      }

      foreach (string file in IRFiles)
      {
        if (commandPrefix)
          list[i++] = Common.CmdPrefixBlast + Path.GetFileNameWithoutExtension(file);
        else
          list[i++] = Path.GetFileNameWithoutExtension(file);
      }

      return list;
    }

    /// <summary>
    /// Loads the settings.
    /// </summary>
    private static void LoadSettings()
    {
      try
      {
        using (Settings xmlreader = new Settings(MPCommon.MPConfigFile))
        {
          ServerHost = xmlreader.GetValueAsString("MPControlPlugin", "ServerHost", "localhost");

          RequireFocus = xmlreader.GetValueAsBool("MPControlPlugin", "RequireFocus", true);
          MultiMappingEnabled = xmlreader.GetValueAsBool("MPControlPlugin", "MultiMappingEnabled", false);
          MultiMappingButton =
            (RemoteButton) xmlreader.GetValueAsInt("MPControlPlugin", "MultiMappingButton", (int) RemoteButton.Start);
          EventMapperEnabled = xmlreader.GetValueAsBool("MPControlPlugin", "EventMapperEnabled", false);
          MouseModeButton =
            (RemoteButton) xmlreader.GetValueAsInt("MPControlPlugin", "MouseModeButton", (int) RemoteButton.Teletext);
          MouseModeEnabled = xmlreader.GetValueAsBool("MPControlPlugin", "MouseModeEnabled", false);
          MouseModeStep = xmlreader.GetValueAsInt("MPControlPlugin", "MouseModeStep", 10);
          MouseModeAcceleration = xmlreader.GetValueAsBool("MPControlPlugin", "MouseModeAcceleration", true);

          // MediaPortal settings ...
          _mpBasicHome = xmlreader.GetValueAsBool("general", "startbasichome", false);
        }
      }
      catch (Exception ex)
      {
        Log.Error(ex);
      }
    }

    /// <summary>
    /// Saves the settings.
    /// </summary>
    private static void SaveSettings()
    {
      try
      {
        using (Settings xmlwriter = new Settings(MPCommon.MPConfigFile))
        {
          xmlwriter.SetValue("MPControlPlugin", "ServerHost", ServerHost);

          xmlwriter.SetValueAsBool("MPControlPlugin", "RequireFocus", RequireFocus);
          xmlwriter.SetValueAsBool("MPControlPlugin", "MultiMappingEnabled", MultiMappingEnabled);
          xmlwriter.SetValue("MPControlPlugin", "MultiMappingButton", (int) MultiMappingButton);
          xmlwriter.SetValueAsBool("MPControlPlugin", "EventMapperEnabled", EventMapperEnabled);
          xmlwriter.SetValue("MPControlPlugin", "MouseModeButton", (int) MouseModeButton);
          xmlwriter.SetValueAsBool("MPControlPlugin", "MouseModeEnabled", MouseModeEnabled);
          xmlwriter.SetValue("MPControlPlugin", "MouseModeStep", MouseModeStep);
          xmlwriter.SetValueAsBool("MPControlPlugin", "MouseModeAcceleration", MouseModeAcceleration);
        }
      }
      catch (Exception ex)
      {
        Log.Error(ex);
      }
    }

    #endregion Implementation
  }
}