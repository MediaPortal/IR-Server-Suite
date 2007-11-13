using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Win32;

using MediaPortal.Configuration;
using MediaPortal.Devices;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
using MediaPortal.Hardware;
using MediaPortal.Player;
using MediaPortal.Util;

using IrssComms;
using IrssUtils;
using MPUtils;

namespace MediaPortal.Plugins
{

  /// <summary>
  /// MediaPortal Control Plugin for IR Server.
  /// </summary>
  public class MPControlPlugin : IPlugin, ISetupForm
  {

    #region Constants

    /// <summary>
    /// The plugin version string.
    /// </summary>
    internal const string PluginVersion = "MP Control Plugin 1.0.3.5 for IR Server";

    internal static readonly string FolderMacros = Common.FolderAppData + "MP Control Plugin\\Macro\\";

    internal static readonly string RemotesFile = Common.FolderAppData + "MP Control Plugin\\Remotes.xml";
    internal static readonly string MultiMappingFile = Common.FolderAppData + "MP Control Plugin\\MultiMapping.xml";
    internal static readonly string EventMappingFile = Common.FolderAppData + "MP Control Plugin\\EventMapping.xml";

    internal static readonly string RemotePresetsFolder = Common.FolderAppData + "MP Control Plugin\\Remote Presets\\";

    #endregion Constants

    #region Variables

    static Client _client;

    static string _serverHost;
    static string _learnIRFilename;

    static bool _registered;

    static bool _logVerbose;
    static bool _requireFocus;
    static bool _multiMappingEnabled;
    static bool _eventMapperEnabled;

    static RemoteButton _mouseModeButton = RemoteButton.None;
    static bool _mouseModeEnabled;
    static int _mouseModeStep;
    static bool _mouseModeAcceleration;

    static bool _mouseModeActive;
    static RemoteButton _mouseModeLastButton = RemoteButton.None;
    static long _mouseModeLastButtonTicks;
    static int _mouseModeRepeatCount;
    static bool _mouseModeRightHeld;
    static bool _mouseModeLeftHeld;
    static bool _mouseModeMiddleHeld;

    static List<MappedEvent> _eventMappings;

    static RemoteButton _multiMappingButton;
    static string[] _multiMaps;
    static int _multiMappingSet;

    static InputHandler _defaultInputHandler;
    static List<InputHandler> _multiInputHandlers;

    static ClientMessageSink _handleMessage;

    static bool _inConfiguration;

    static bool _mpBasicHome;

    static MappedKeyCode[] _remoteMap;

    static IRServerInfo _irServerInfo = new IRServerInfo();

    static List<string> _macroStack;

    #endregion Variables

    #region Properties

    internal static string ServerHost
    {
      get { return _serverHost; }
      set { _serverHost = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to log verbosely.
    /// </summary>
    /// <value><c>true</c> if logging is set to verbose; otherwise, <c>false</c>.</value>
    internal static bool LogVerbose
    {
      get { return _logVerbose; }
      set { _logVerbose = value; }
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
      InConfiguration = false;

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
      IPEndPoint endPoint = new IPEndPoint(serverIP, IrssComms.Server.DefaultPort);

      if (!StartClient(endPoint))
        Log.Error("MPControlPlugin: Failed to start local comms, IR input and IR blasting is disabled for this session");

      // Load the event mapper mappings
      if (EventMapperEnabled)
      {
        LoadEventMappings();

        MapEvent(MappedEvent.MappingEvent.MediaPortal_Start);

        // Register with MediaPortal to receive GUI Messages ...
        GUIWindowManager.Receivers += new SendMessageHandler(OnMessage);
      }

      // Register for Power State message ...
      //SystemEvents.SessionEnding += new SessionEndingEventHandler(SystemEvents_SessionEnding);
      SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);

      if (LogVerbose)
        Log.Info("MPControlPlugin: Started");
    }
    /// <summary>
    /// Stops this instance.
    /// </summary>
    public void Stop()
    {
      SystemEvents.PowerModeChanged -= new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);

      if (EventMapperEnabled)
      {
        GUIWindowManager.Receivers -= new SendMessageHandler(OnMessage);

        MapEvent(MappedEvent.MappingEvent.MediaPortal_Stop);
      }

      StopClient();

      _defaultInputHandler = null;

      if (MultiMappingEnabled)
        for (int i = 0; i < _multiInputHandlers.Count; i++)
          _multiInputHandlers[i] = null;

      if (LogVerbose)
        Log.Info("MPControlPlugin: Stopped");
    }

    #endregion IPlugin methods

    #region ISetupForm methods

    /// <summary>
    /// Determines whether this plugin can be enabled.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this plugin can be enabled; otherwise, <c>false</c>.
    /// </returns>
    public bool CanEnable()       { return true; }
    /// <summary>
    /// Determines whether this plugin has setup.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this plugin has setup; otherwise, <c>false</c>.
    /// </returns>
    public bool HasSetup()        { return true; }
    /// <summary>
    /// Gets the plugin name.
    /// </summary>
    /// <returns>The plugin name.</returns>
    public string PluginName()    { return "MP Control Plugin for IR Server"; }
    /// <summary>
    /// Defaults enabled.
    /// </summary>
    /// <returns>true if this plugin is enabled by default, otherwise false.</returns>
    public bool DefaultEnabled()  { return true; }
    /// <summary>
    /// Gets the window id.
    /// </summary>
    /// <returns>The window id.</returns>
    public int GetWindowId()      { return 0; }
    /// <summary>
    /// Gets the plugin author.
    /// </summary>
    /// <returns>The plugin author.</returns>
    public string Author()        { return "and-81"; }
    /// <summary>
    /// Gets the description of the plugin.
    /// </summary>
    /// <returns>The plugin description.</returns>
    public string Description()   { return "This plugin uses the IR Server to replace MediaPortal's native remote control support"; }

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

        InConfiguration = true;

        if (LogVerbose)
          Log.Info("MPControlPlugin: ShowPlugin()");

        SetupForm setupForm = new SetupForm();
        if (setupForm.ShowDialog() == DialogResult.OK)
          SaveSettings();

        StopClient();

        if (LogVerbose)
          Log.Info("MPControlPlugin: ShowPlugin() - End");
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
    public bool GetHome(out string strButtonText, out string strButtonImage, out string strButtonImageFocus, out string strPictureImage)
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
    static bool HandleMouseMode(RemoteButton button)
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

          _mouseModeLeftHeld    = false;
          _mouseModeRightHeld   = false;
          _mouseModeMiddleHeld  = false;
        }

        MPCommon.ShowNotifyDialog("Mouse Mode", notifyMessage, 2);

        if (LogVerbose)
          Log.Info("MPControlPlugin: {0}", notifyMessage);

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

          case RemoteButton.Replay:   // Left Single-Click
            if (_mouseModeLeftHeld)
              Mouse.Button(Mouse.MouseEvents.LeftUp);

            if (_mouseModeRightHeld)
              Mouse.Button(Mouse.MouseEvents.RightUp);

            if (_mouseModeMiddleHeld)
              Mouse.Button(Mouse.MouseEvents.MiddleUp);

            _mouseModeLeftHeld    = false;
            _mouseModeRightHeld   = false;
            _mouseModeMiddleHeld  = false;

            Mouse.Button(Mouse.MouseEvents.LeftDown);
            Mouse.Button(Mouse.MouseEvents.LeftUp);
            return true;

          case RemoteButton.Skip:     // Right Single-Click
            if (_mouseModeLeftHeld)
              Mouse.Button(Mouse.MouseEvents.LeftUp);

            if (_mouseModeRightHeld)
              Mouse.Button(Mouse.MouseEvents.RightUp);

            if (_mouseModeMiddleHeld)
              Mouse.Button(Mouse.MouseEvents.MiddleUp);

            _mouseModeLeftHeld    = false;
            _mouseModeRightHeld   = false;
            _mouseModeMiddleHeld  = false;

            Mouse.Button(Mouse.MouseEvents.RightDown);
            Mouse.Button(Mouse.MouseEvents.RightUp);
            return true;

          case RemoteButton.Play:     // Middle Single-Click
            if (_mouseModeLeftHeld)
              Mouse.Button(Mouse.MouseEvents.LeftUp);

            if (_mouseModeRightHeld)
              Mouse.Button(Mouse.MouseEvents.RightUp);

            if (_mouseModeMiddleHeld)
              Mouse.Button(Mouse.MouseEvents.MiddleUp);

            _mouseModeLeftHeld    = false;
            _mouseModeRightHeld   = false;
            _mouseModeMiddleHeld  = false;

            Mouse.Button(Mouse.MouseEvents.MiddleDown);
            Mouse.Button(Mouse.MouseEvents.MiddleUp);
            return true;

          case RemoteButton.Ok:       // Double-Click (Left)
            if (_mouseModeLeftHeld)
              Mouse.Button(Mouse.MouseEvents.LeftUp);

            if (_mouseModeRightHeld)
              Mouse.Button(Mouse.MouseEvents.RightUp);

            if (_mouseModeMiddleHeld)
              Mouse.Button(Mouse.MouseEvents.MiddleUp);

            _mouseModeLeftHeld    = false;
            _mouseModeRightHeld   = false;
            _mouseModeMiddleHeld  = false;

            Mouse.Button(Mouse.MouseEvents.LeftDown);
            Mouse.Button(Mouse.MouseEvents.LeftUp);

            Mouse.Button(Mouse.MouseEvents.LeftDown);
            Mouse.Button(Mouse.MouseEvents.LeftUp);
            return true;

          case RemoteButton.Back:     // Left Click & Hold
            if (_mouseModeRightHeld)
              Mouse.Button(Mouse.MouseEvents.RightUp);

            if (_mouseModeMiddleHeld)
              Mouse.Button(Mouse.MouseEvents.MiddleUp);

            if (_mouseModeLeftHeld)
              Mouse.Button(Mouse.MouseEvents.LeftUp);
            else
              Mouse.Button(Mouse.MouseEvents.LeftDown);

            _mouseModeLeftHeld    = !_mouseModeLeftHeld;
            _mouseModeRightHeld   = false;
            _mouseModeMiddleHeld  = false;
            return true;

          case RemoteButton.Info:     // Right Click & Hold
            if (_mouseModeLeftHeld)
              Mouse.Button(Mouse.MouseEvents.LeftUp);

            if (_mouseModeMiddleHeld)
              Mouse.Button(Mouse.MouseEvents.MiddleUp);

            if (_mouseModeRightHeld)
              Mouse.Button(Mouse.MouseEvents.RightUp);
            else
              Mouse.Button(Mouse.MouseEvents.RightDown);

            _mouseModeRightHeld   = !_mouseModeRightHeld;
            _mouseModeLeftHeld    = false;
            _mouseModeMiddleHeld  = false;
            return true;

          case RemoteButton.Stop:     // Middle Click & Hold
            if (_mouseModeLeftHeld)
              Mouse.Button(Mouse.MouseEvents.LeftUp);

            if (_mouseModeRightHeld)
              Mouse.Button(Mouse.MouseEvents.RightUp);

            if (_mouseModeMiddleHeld)
              Mouse.Button(Mouse.MouseEvents.MiddleUp);
            else
              Mouse.Button(Mouse.MouseEvents.MiddleDown);

            _mouseModeMiddleHeld  = !_mouseModeMiddleHeld;
            _mouseModeLeftHeld    = false;
            _mouseModeRightHeld   = false;
            return true;

          case RemoteButton.ChannelUp:    // Scroll Up
            Mouse.Scroll(Mouse.ScrollDir.Up);
            return true;

          case RemoteButton.ChannelDown:  // Scroll Down
            Mouse.Scroll(Mouse.ScrollDir.Down);
            return true;
        }
      }

      return false;
    }

    /// <summary>
    /// Handles remote buttons received.
    /// </summary>
    /// <param name="keyCode">The remote button.</param>
    static void RemoteHandler(string keyCode)
    {
      // If user has stipulated that MP must have focus to recognize commands ...
      if (RequireFocus && !GUIGraphicsContext.HasFocus)
        return;

      foreach (MappedKeyCode mapping in _remoteMap)
      {
        if (mapping.KeyCode.Equals(keyCode, StringComparison.Ordinal))
        {
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
            gotMapped = _multiInputHandlers[_multiMappingSet].MapAction((int)mapping.Button);
          else
            gotMapped = _defaultInputHandler.MapAction((int)mapping.Button);

          if (LogVerbose)
          {
            if (gotMapped)
              Log.Debug("MPControlPlugin: Command \"{0}\" mapped to remote", mapping.Button);
            else
              Log.Debug("MPControlPlugin: Command \"{0}\" not mapped to remote", mapping.Button);
          }

          return;
        }
      }

      if (LogVerbose)
        Log.Info("MPControlPlugin: keyCode \"{0}\" was not handled", keyCode);

      return;
    }

    static void CommsFailure(object obj)
    {
      Exception ex = obj as Exception;
      
      if (ex != null)
        Log.Error("MPControlPlugin: Communications failure: {0}", ex.Message);
      else
        Log.Error("MPControlPlugin: Communications failure");

      StopClient();

      Log.Warn("MPControlPlugin: Attempting communications restart ...");

      IPAddress serverIP = Client.GetIPFromName(_serverHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, IrssComms.Server.DefaultPort);

      StartClient(endPoint);
    }
    static void Connected(object obj)
    {
      Log.Info("MPControlPlugin: Connected to server");

      IrssMessage message = new IrssMessage(MessageType.RegisterClient, MessageFlags.Request);
      _client.Send(message);
    }
    static void Disconnected(object obj)
    {
      Log.Warn("MPControlPlugin: Communications with server has been lost");

      Thread.Sleep(1000);
    }

    internal static bool StartClient(IPEndPoint endPoint)
    {
      if (_client != null)
        return false;

      ClientMessageSink sink = new ClientMessageSink(ReceivedMessage);

      _client = new Client(endPoint, sink);
      _client.CommsFailureCallback  = new WaitCallback(CommsFailure);
      _client.ConnectCallback       = new WaitCallback(Connected);
      _client.DisconnectCallback    = new WaitCallback(Disconnected);

      if (_client.Start())
      {
        return true;
      }
      else
      {
        _client = null;
        return false;
      }
    }
    internal static void StopClient()
    {
      if (_client != null)
      {
        _client.Dispose();
        _client = null;
      }
    }

    static void ReceivedMessage(IrssMessage received)
    {
      if (LogVerbose)
        Log.Debug("MPControlPlugin: Received Message \"{0}\"", received.Type);

      try
      {
        switch (received.Type)
        {
          case MessageType.RemoteEvent:
            if (!InConfiguration)
              RemoteHandler(received.GetDataAsString());
            break;

          case MessageType.BlastIR:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              if (LogVerbose)
                Log.Info("MPControlPlugin: Blast successful");
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

              if (LogVerbose)
                Log.Info("MPControlPlugin: Registered to IR Server");
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
              if (LogVerbose)
                Log.Info("MPControlPlugin: Learned IR Successfully");

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
        Log.Error("MPControlPlugin - ReveivedMessage(): {0}", ex.Message);
      }
    }

    /// <summary>
    /// OnMessage is used for event mapping.
    /// </summary>
    /// <param name="msg">Message.</param>
    void OnMessage(GUIMessage msg)
    {
      if (EventMapperEnabled)
        MapEvent(msg);
    }

    /// <summary>
    /// Changes the multi mapping.
    /// </summary>
    static void ChangeMultiMapping()
    {
      // Cycle through Multi-Mappings ...
      _multiMappingSet = (_multiMappingSet + 1) % MultiMaps.Length;

      // Show the mapping set name on screen ...
      string setName = MultiMaps[_multiMappingSet];

      if (LogVerbose)
        Log.Debug("MPControlPlugin: Multi-Mapping has changed to \"{0}\"", setName);

      MPCommon.ShowNotifyDialog("Multi-Mapping", setName, 2);
    }
    /// <summary>
    /// Changes the multi mapping.
    /// </summary>
    /// <param name="multiMapping">The multi mapping.</param>
    static void ChangeMultiMapping(string multiMapping)
    {
      Log.Debug("MPControlPlugin: ChangeMultiMapping: {0}", multiMapping);

      if (multiMapping.Equals("TOGGLE", StringComparison.OrdinalIgnoreCase))
      {
        ChangeMultiMapping();
        return;
      }

      for (int index = 0; index < MultiMaps.Length; index++)
      {
        if (MultiMaps[index].Equals(multiMapping, StringComparison.OrdinalIgnoreCase))
        {
          _multiMappingSet = index;

          // Show the mapping set name on screen ...
          string setName = MultiMaps[_multiMappingSet];

          if (LogVerbose)
            Log.Info("MPControlPlugin: Multi-Mapping has changed to \"{0}\"", setName);

          MPCommon.ShowNotifyDialog("Multi-Mapping", setName, 2);

          return;
        }
      }
    }

    /// <summary>
    /// Loads the remote map.
    /// </summary>
    /// <param name="remoteFile">The remote file.</param>
    /// <returns>Remote map.</returns>
    static MappedKeyCode[] LoadRemoteMap(string remoteFile)
    {
      List<MappedKeyCode> remoteMap = new List<MappedKeyCode>();

      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(remoteFile);

        //string remoteName;
        string remoteButton;
        string remoteCode;

        XmlNodeList listRemotes = doc.DocumentElement.SelectNodes("remote");
        foreach (XmlNode nodeRemote in listRemotes)
        {
          //remoteName = nodeRemote.Attributes["name"].Value;

          XmlNodeList listButtons = nodeRemote.SelectNodes("button");
          foreach (XmlNode nodeButton in listButtons)
          {
            remoteButton = nodeButton.Attributes["name"].Value;

            XmlNodeList listIRCodes = nodeButton.SelectNodes("code");
            foreach (XmlNode nodeCode in listIRCodes)
            {
              remoteCode = nodeCode.Attributes["value"].Value;
              remoteMap.Add(new MappedKeyCode(remoteButton, remoteCode));
            }
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("MPControlPlugin: LoadRemoteMap() - {0}", ex.Message);
      }

      return remoteMap.ToArray();
    }

    /// <summary>
    /// Loads the event mappings.
    /// </summary>
    static void LoadEventMappings()
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
        Log.Error("MPControlPlugin: LoadEventMappings() {0}", ex.Message);
      }
    }

    /// <summary>
    /// Run the event mapper over the supplied GUIMessage.
    /// </summary>
    /// <param name="msg">MediaPortal Message to run through the event mapper.</param>
    static void MapEvent(GUIMessage msg)
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
              case "Label 1":               matched = (msg.Label.Equals(paramValueString, StringComparison.OrdinalIgnoreCase));   break;
              case "Label 2":               matched = (msg.Label2.Equals(paramValueString, StringComparison.OrdinalIgnoreCase));  break;
              case "Label 3":               matched = (msg.Label3.Equals(paramValueString, StringComparison.OrdinalIgnoreCase));  break;
              case "Label 4":               matched = (msg.Label4.Equals(paramValueString, StringComparison.OrdinalIgnoreCase));  break;
              case "Parameter 1":           matched = (msg.Param1 == paramValueInt);              break;
              case "Parameter 2":           matched = (msg.Param2 == paramValueInt);              break;
              case "Parameter 3":           matched = (msg.Param3 == paramValueInt);              break;
              case "Parameter 4":           matched = (msg.Param4 == paramValueInt);              break;
              case "Sender Control ID":     matched = (msg.SenderControlId == paramValueInt);     break;
              case "Send To Target Window": matched = (msg.SendToTargetWindow == paramValueBool); break;
              case "Target Control ID":     matched = (msg.TargetControlId == paramValueInt);     break;
              case "Target Window ID":      matched = (msg.TargetWindowId == paramValueInt);      break;
              default:                      matched = false;                                      break;
            }
          }

          if (!matched)
            continue;

          if (LogVerbose)
            Log.Info("MPControlPlugin: Event Mapper - Event \"{0}\"", Enum.GetName(typeof(MappedEvent.MappingEvent), eventType));

          try
          {
            ProcessCommand(mappedEvent.Command);
          }
          catch (Exception ex)
          {
            Log.Error(ex);
          }
        }
      }
    }

    /// <summary>
    /// Run the event mapper over the supplied MappedEvent type.
    /// </summary>
    /// <param name="eventType">MappedEvent to run through the event mapper.</param>
    static void MapEvent(MappedEvent.MappingEvent eventType)
    {
      foreach (MappedEvent mappedEvent in EventMappings)
      {
        if (mappedEvent.EventType == eventType)
        {
          if (mappedEvent.MatchParam)
            continue;

          if (LogVerbose)
            Log.Info("MPControlPlugin: Event Mapper - Event \"{0}\"", Enum.GetName(typeof(MappedEvent.MappingEvent), eventType));

          try
          {
            ProcessCommand(mappedEvent.Command);
          }
          catch (Exception ex)
          {
            Log.Error(ex);
          }
        }
      }
    }

    /// <summary>
    /// Handles the PowerModeChanged event of the SystemEvents control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="Microsoft.Win32.PowerModeChangedEventArgs"/> instance containing the event data.</param>
    static void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
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

      string map;
      for (int index = 0; index < mappings.Count; index++)
      {
        map = mappings.Item(index).Attributes["name"].Value;

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
      if (!InConfiguration && EventMapperEnabled)
        MapEvent(MappedEvent.MappingEvent.PC_Suspend);
    }

    /// <summary>
    /// Call this when leaving standby to ensure the Event Mapper is informed.
    /// </summary>
    internal static void OnResume()
    {
      if (!InConfiguration && EventMapperEnabled)
        MapEvent(MappedEvent.MappingEvent.PC_Resume);
    }

    static void Hibernate()
    {
      if (InConfiguration)
        return;

      GUIGraphicsContext.ResetLastActivity();
      // Stop all media before hibernating
      g_Player.Stop();

      GUIMessage msg;

      if (_mpBasicHome)
        msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_GOTO_WINDOW, 0, 0, 0, (int)GUIWindow.Window.WINDOW_SECOND_HOME, 0, null);
      else
        msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_GOTO_WINDOW, 0, 0, 0, (int)GUIWindow.Window.WINDOW_HOME, 0, null);

      GUIWindowManager.SendThreadMessage(msg);

      OnSuspend();
      WindowsController.ExitWindows(RestartOptions.Hibernate, false);
    }

    static void Standby()
    {
      if (InConfiguration)
        return;

      GUIGraphicsContext.ResetLastActivity();
      // Stop all media before suspending
      g_Player.Stop();

      GUIMessage msg;

      if (_mpBasicHome)
        msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_GOTO_WINDOW, 0, 0, 0, (int)GUIWindow.Window.WINDOW_SECOND_HOME, 0, null);
      else
        msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_GOTO_WINDOW, 0, 0, 0, (int)GUIWindow.Window.WINDOW_HOME, 0, null);

      GUIWindowManager.SendThreadMessage(msg);

      OnSuspend();
      WindowsController.ExitWindows(RestartOptions.Suspend, false);
    }

    static void Reboot()
    {
      if (!InConfiguration)
        GUIGraphicsContext.OnAction(new Action(Action.ActionType.ACTION_SHUTDOWN, 0, 0));
    }

    static void ShutDown()
    {
      if (!InConfiguration)
        GUIGraphicsContext.OnAction(new Action(Action.ActionType.ACTION_REBOOT, 0, 0));
    }

    /// <summary>
    /// Adds to the Macro Stack.
    /// </summary>
    /// <param name="fileName">Name of the macro file.</param>
    static void MacroStackAdd(string fileName)
    {
      string upperCasedFileName = fileName.ToUpperInvariant();

      if (_macroStack == null)
      {
        _macroStack = new List<string>();
      }
      else if (_macroStack.Contains(upperCasedFileName))
      {
        StringBuilder macroStackTrace = new StringBuilder();
        macroStackTrace.AppendLine("Macro infinite loop detected!");
        macroStackTrace.AppendLine();
        macroStackTrace.AppendLine("Stack trace:");

        foreach (string macro in _macroStack)
        {
          if (macro.Equals(upperCasedFileName))
            macroStackTrace.AppendLine(String.Format("--> {0}", macro));
          else
            macroStackTrace.AppendLine(macro);
        }

        macroStackTrace.AppendLine(String.Format("--> {0}", upperCasedFileName));

        throw new ApplicationException(macroStackTrace.ToString());
      }

      _macroStack.Add(upperCasedFileName);
    }
    /// <summary>
    /// Removes from the Macro Stack.
    /// </summary>
    /// <param name="fileName">Name of the macro file.</param>
    static void MacroStackRemove(string fileName)
    {
      string upperCasedFileName = fileName.ToUpperInvariant();

      if (_macroStack.Contains(upperCasedFileName))
        _macroStack.Remove(upperCasedFileName);

      if (_macroStack.Count == 0)
        _macroStack = null;
    }

    /// <summary>
    /// Process the supplied Macro file.
    /// </summary>
    /// <param name="fileName">Macro file to process (absolute path).</param>
    internal static void ProcessMacro(string fileName)
    {
      MacroStackAdd(fileName);

      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(fileName);

        if (doc.DocumentElement.InnerText.Contains(Common.XmlTagBlast) && !_registered)
          throw new ApplicationException("Cannot process Macro with Blast commands when not registered to an active IR Server");

        XmlNodeList commandSequence = doc.DocumentElement.SelectNodes("action");
        string commandProperty;

        foreach (XmlNode item in commandSequence)
        {
          commandProperty = item.Attributes["cmdproperty"].Value;

          switch (item.Attributes["command"].Value)
          {
            case Common.XmlTagMacro:
              {
                ProcessMacro(FolderMacros + commandProperty + Common.FileExtensionMacro);
                break;
              }

            case Common.XmlTagBlast:
              {
                string[] commands = Common.SplitBlastCommand(commandProperty);
                BlastIR(Common.FolderIRCommands + commands[0] + Common.FileExtensionIR, commands[1]);
                break;
              }

            case Common.XmlTagPause:
              {
                int sleep = int.Parse(commandProperty);
                Thread.Sleep(sleep);
                break;
              }

            case Common.XmlTagRun:
              {
                string[] commands = Common.SplitRunCommand(commandProperty);
                Common.ProcessRunCommand(commands);
                break;
              }

            case Common.XmlTagSerial:
              {
                string[] commands = Common.SplitSerialCommand(commandProperty);
                Common.ProcessSerialCommand(commands);
                break;
              }

            case Common.XmlTagGoto:
              {
                if (InConfiguration)
                  MessageBox.Show(commandProperty, Common.UITextGoto, MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                  MPCommon.ProcessGoTo(commandProperty, MP_BasicHome);
                break;
              }

            case Common.XmlTagPopup:
              {
                string[] commands = Common.SplitPopupCommand(commandProperty);

                if (InConfiguration)
                  MessageBox.Show(commands[1], commands[0], MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                  MPCommon.ShowNotifyDialog(commands[0], commands[1], int.Parse(commands[2]));

                break;
              }

            case Common.XmlTagWindowMsg:
              {
                string[] commands = Common.SplitWindowMessageCommand(commandProperty);
                Common.ProcessWindowMessageCommand(commands);
                break;
              }

            case Common.XmlTagTcpMsg:
              {
                string[] commands = Common.SplitTcpMessageCommand(commandProperty);
                Common.ProcessTcpMessageCommand(commands);
                break;
              }

            case Common.XmlTagKeys:
              {
                if (InConfiguration)
                  MessageBox.Show(commandProperty, Common.UITextKeys, MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                  Common.ProcessKeyCommand(commandProperty);
                break;
              }

            case Common.XmlTagMultiMap:
              {
                if (InConfiguration)
                  MessageBox.Show(commandProperty, Common.UITextMultiMap, MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (MultiMappingEnabled)
                  ChangeMultiMapping(commandProperty);

                break;
              }

            case Common.XmlTagMouseMode:
              {
                if (InConfiguration)
                {
                  MessageBox.Show("Command to toggle the Mouse Mode cannot be processed in configuration.", Common.UITextMouseMode, MessageBoxButtons.OK, MessageBoxIcon.Information);
                  break;
                }

                if (!MouseModeEnabled)
                  break;

                switch (commandProperty)
                {
                  case "ON":
                    MouseModeActive = true;
                    break;

                  case "OFF":
                    MouseModeActive = false;
                    break;

                  case "TOGGLE":
                    MouseModeActive = !MouseModeActive;
                    break;
                }

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

                if (LogVerbose)
                  Log.Info("MPControlPlugin: {0}", notifyMessage);

                break;
              }

            case Common.XmlTagInputLayer:
              {
                if (InConfiguration)
                {
                  MessageBox.Show("Command to toggle the input handler layer cannot be processed in configuration.", Common.UITextInputLayer, MessageBoxButtons.OK, MessageBoxIcon.Information);
                  break;
                }

                InputHandler inputHandler;

                if (MultiMappingEnabled)
                  inputHandler = _multiInputHandlers[_multiMappingSet];
                else
                  inputHandler = _defaultInputHandler;

                if (inputHandler.CurrentLayer == 1)
                  inputHandler.CurrentLayer = 2;
                else
                  inputHandler.CurrentLayer = 1;

                break;
              }
            /*
            case Common.XmlTagWindowState:
              {
                if (InConfiguration)
                {
                  MessageBox.Show("Command to toggle the window state cannot be processed in configuration.", Common.UITextWindowState, MessageBoxButtons.OK, MessageBoxIcon.Information);
                  break;
                }

                GUIMessage msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_SWITCH_FULL_WINDOWED, 0, 0, 0, 0, 0, null);

                if (GUIGraphicsContext.DX9Device.PresentationParameters.Windowed)
                  msg.Param1 = 1;

                GUIWindowManager.SendMessage(msg);
                break;
              }
            */
            case Common.XmlTagFocus:
              {
                if (InConfiguration)
                {
                  MessageBox.Show("Command to get focus cannot be processed in configuration.", Common.UITextFocus, MessageBoxButtons.OK, MessageBoxIcon.Information);
                  break;
                }

                GUIGraphicsContext.ResetLastActivity();
                GUIMessage msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_GETFOCUS, 0, 0, 0, 0, 0, null);
                GUIWindowManager.SendThreadMessage(msg);
                break;
              }

            case Common.XmlTagExit:
              {
                if (!InConfiguration)
                  GUIGraphicsContext.OnAction(new Action(Action.ActionType.ACTION_EXIT, 0, 0));
                break;
              }

            case Common.XmlTagEject:
              {
                Common.ProcessEjectCommand(commandProperty);
                break;
              }

            case Common.XmlTagStandby:
              {
                Standby();
                break;
              }

            case Common.XmlTagHibernate:
              {
                Hibernate();
                break;
              }

            case Common.XmlTagShutdown:
              {
                ShutDown();
                break;
              }

            case Common.XmlTagReboot:
              {
                Reboot();
                break;
              }
          }
        }
      }
      finally
      {
        MacroStackRemove(fileName);
      }
    }

    /// <summary>
    /// Learn an IR command.
    /// </summary>
    /// <param name="fileName">File to place learned IR command in (absolute path).</param>
    /// <returns>true if successful, otherwise false.</returns>
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
        Log.Error("MPControlPlugin - LearnIR(): {0}", ex.Message);
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
      if (!_registered)
        throw new ApplicationException("Cannot Blast, not registered to an active IR Server");

      using (FileStream file = File.OpenRead(fileName))
      {
        if (file.Length == 0)
          throw new IOException(String.Format("Cannot Blast. IR file \"{0}\" has no data, possible IR learn failure", fileName));

        byte[] outData = new byte[4 + port.Length + file.Length];

        BitConverter.GetBytes(port.Length).CopyTo(outData, 0);
        Encoding.ASCII.GetBytes(port).CopyTo(outData, 4);

        file.Read(outData, 4 + port.Length, (int)file.Length);

        IrssMessage message = new IrssMessage(MessageType.BlastIR, MessageFlags.Request, outData);
        _client.Send(message);
      }
    }

    /// <summary>
    /// Given a command this method processes the request accordingly.
    /// </summary>
    /// <param name="command">Command to process.</param>
    internal static void ProcessCommand(string command)
    {
      if (String.IsNullOrEmpty(command))
        throw new ArgumentNullException("command");

      if (command.StartsWith(Common.CmdPrefixMacro, StringComparison.OrdinalIgnoreCase)) // Macro
      {
        string fileName = FolderMacros + command.Substring(Common.CmdPrefixMacro.Length) + Common.FileExtensionMacro;
        ProcessMacro(fileName);
      }
      else if (command.StartsWith(Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))  // IR Code
      {
        string[] commands = Common.SplitBlastCommand(command.Substring(Common.CmdPrefixBlast.Length));
        BlastIR(Common.FolderIRCommands + commands[0] + Common.FileExtensionIR, commands[1]);
      }
      else if (command.StartsWith(Common.CmdPrefixRun, StringComparison.OrdinalIgnoreCase)) // External Program
      {
        string[] commands = Common.SplitRunCommand(command.Substring(Common.CmdPrefixRun.Length));
        Common.ProcessRunCommand(commands);
      }
      else if (command.StartsWith(Common.CmdPrefixSerial, StringComparison.OrdinalIgnoreCase)) // Serial Port Command
      {
        string[] commands = Common.SplitSerialCommand(command.Substring(Common.CmdPrefixSerial.Length));
        Common.ProcessSerialCommand(commands);
      }
      else if (command.StartsWith(Common.CmdPrefixWindowMsg, StringComparison.OrdinalIgnoreCase))  // Message Command
      {
        string[] commands = Common.SplitWindowMessageCommand(command.Substring(Common.CmdPrefixWindowMsg.Length));
        Common.ProcessWindowMessageCommand(commands);
      }
      else if (command.StartsWith(Common.CmdPrefixKeys, StringComparison.OrdinalIgnoreCase))  // Keystroke Command
      {
        string keyCommand = command.Substring(Common.CmdPrefixKeys.Length);
        if (InConfiguration)
          MessageBox.Show(keyCommand, Common.UITextKeys, MessageBoxButtons.OK, MessageBoxIcon.Information);
        else
          Common.ProcessKeyCommand(keyCommand);
      }
      else if (command.StartsWith(Common.CmdPrefixMouse, StringComparison.OrdinalIgnoreCase)) // Mouse Command
      {
        string mouseCommand = command.Substring(Common.CmdPrefixMouse.Length);
        Common.ProcessMouseCommand(mouseCommand);
      }
      else if (command.StartsWith(Common.CmdPrefixEject, StringComparison.OrdinalIgnoreCase)) // Eject Command
      {
        string ejectCommand = command.Substring(Common.CmdPrefixEject.Length);
        Common.ProcessEjectCommand(ejectCommand);
      }
      else if (command.StartsWith(Common.CmdPrefixHibernate, StringComparison.OrdinalIgnoreCase)) // Hibernate Command
      {
        Hibernate();
      }
      else if (command.StartsWith(Common.CmdPrefixReboot, StringComparison.OrdinalIgnoreCase)) // Reboot Command
      {
        Reboot();
      }
      else if (command.StartsWith(Common.CmdPrefixShutdown, StringComparison.OrdinalIgnoreCase)) // Shutdown Command
      {
        ShutDown();
      }
      else if (command.StartsWith(Common.CmdPrefixStandby, StringComparison.OrdinalIgnoreCase)) // Standby Command
      {
        Standby();
      }
      else if (command.StartsWith(Common.CmdPrefixGoto, StringComparison.OrdinalIgnoreCase)) // Go To Screen
      {
        MPCommon.ProcessGoTo(command.Substring(Common.CmdPrefixGoto.Length), MP_BasicHome);
      }
      else
      {
        throw new ArgumentException(String.Format("Cannot process unrecognized command \"{0}\"", command), "command");
      }
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
    static void LoadSettings()
    {
      try
      {
        using (MediaPortal.Profile.Settings xmlreader = new MediaPortal.Profile.Settings(MPCommon.MPConfigFile))
        {
          ServerHost = xmlreader.GetValueAsString("MPControlPlugin", "ServerHost", "localhost");

          LogVerbose = xmlreader.GetValueAsBool("MPControlPlugin", "LogVerbose", false);
          RequireFocus = xmlreader.GetValueAsBool("MPControlPlugin", "RequireFocus", true);
          MultiMappingEnabled = xmlreader.GetValueAsBool("MPControlPlugin", "MultiMappingEnabled", false);
          MultiMappingButton = (RemoteButton)xmlreader.GetValueAsInt("MPControlPlugin", "MultiMappingButton", (int)RemoteButton.Start);
          EventMapperEnabled = xmlreader.GetValueAsBool("MPControlPlugin", "EventMapperEnabled", false);
          MouseModeButton = (RemoteButton)xmlreader.GetValueAsInt("MPControlPlugin", "MouseModeButton", (int)RemoteButton.Teletext);
          MouseModeEnabled = xmlreader.GetValueAsBool("MPControlPlugin", "MouseModeEnabled", false);
          MouseModeStep = xmlreader.GetValueAsInt("MPControlPlugin", "MouseModeStep", 10);
          MouseModeAcceleration = xmlreader.GetValueAsBool("MPControlPlugin", "MouseModeAcceleration", true);

          // MediaPortal settings ...
          _mpBasicHome = xmlreader.GetValueAsBool("general", "startbasichome", false);
        }
      }
      catch (Exception ex)
      {
        Log.Error("MPControlPlugin: LoadSettings() {0}", ex.Message);
      }
    }
    /// <summary>
    /// Saves the settings.
    /// </summary>
    static void SaveSettings()
    {
      try
      {
        using (MediaPortal.Profile.Settings xmlwriter = new MediaPortal.Profile.Settings(MPCommon.MPConfigFile))
        {
          xmlwriter.SetValue("MPControlPlugin", "ServerHost", ServerHost);

          xmlwriter.SetValueAsBool("MPControlPlugin", "LogVerbose", LogVerbose);
          xmlwriter.SetValueAsBool("MPControlPlugin", "RequireFocus", RequireFocus);
          xmlwriter.SetValueAsBool("MPControlPlugin", "MultiMappingEnabled", MultiMappingEnabled);
          xmlwriter.SetValue("MPControlPlugin", "MultiMappingButton", (int)MultiMappingButton);
          xmlwriter.SetValueAsBool("MPControlPlugin", "EventMapperEnabled", EventMapperEnabled);
          xmlwriter.SetValue("MPControlPlugin", "MouseModeButton", (int)MouseModeButton);
          xmlwriter.SetValueAsBool("MPControlPlugin", "MouseModeEnabled", MouseModeEnabled);
          xmlwriter.SetValue("MPControlPlugin", "MouseModeStep", MouseModeStep);
          xmlwriter.SetValueAsBool("MPControlPlugin", "MouseModeAcceleration", MouseModeAcceleration);
        }
      }
      catch (Exception ex)
      {
        Log.Error("MPControlPlugin: SaveSettings() {0}", ex.Message);
      }
    }

    #endregion Implementation

  }

}
