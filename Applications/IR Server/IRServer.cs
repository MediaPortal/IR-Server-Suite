using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

using InputService.Plugin;
using IrssComms;
using IrssUtils;

namespace IRServer
{

  #region Enumerations

  /// <summary>
  /// Describes the operation mode of the IR Server.
  /// </summary>
  public enum IRServerMode
  {
    /// <summary>
    /// Acts as a standard IR Server (Default).
    /// </summary>
    ServerMode    = 0,
    /// <summary>
    /// Relays button presses to another IR Server.
    /// </summary>
    RelayMode     = 1,
    /// <summary>
    /// Acts as a repeater for another IR Server's IR blasting.
    /// </summary>
    RepeaterMode  = 2,
  }

  #endregion Enumerations

  /// <summary>
  /// IR Server.
  /// </summary>
  public class IRServer : IDisposable
  {

    #region Constants

    static readonly string ConfigurationFile        = Path.Combine(Common.FolderAppData, "IR Server\\IR Server.xml");

    static readonly string AbstractRemoteMapFolder  = Path.Combine(Common.FolderAppData, "Input Service\\Abstract Remote Maps");

    static readonly string AbstractRemoteSchemaFile = Path.Combine(Common.FolderAppData, "Input Service\\Abstract Remote Maps\\RemoteTable.xsd");

    #endregion Constants

    #region Variables

    NotifyIcon _notifyIcon;
    bool _inConfiguration;

    bool _abstractRemoteMode;
    IRServerMode _mode;
    string _hostComputer;
    string _processPriority;

    string[] _pluginNameReceive;
    List<PluginBase> _pluginReceive;

    string _pluginNameTransmit;
    PluginBase _pluginTransmit;

    Server _server;
    Client _client;

    List<ClientManager> _registeredClients;
    List<ClientManager> _registeredRepeaters;

    bool _registered; // Used for relay and repeater modes.

    DataSet _abstractRemoteButtons;

    #endregion Variables

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="IRServer"/> class.
    /// </summary>
    public IRServer()
    {
      // Setup taskbar icon
      _notifyIcon = new NotifyIcon();
      _notifyIcon.ContextMenuStrip = new ContextMenuStrip();

      _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripLabel("IR Server"));
      _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
      _notifyIcon.ContextMenuStrip.Items.Add("&Setup", null, new EventHandler(ClickSetup));
      _notifyIcon.ContextMenuStrip.Items.Add("&Quit", null, new EventHandler(ClickQuit));
      _notifyIcon.Icon = Properties.Resources.Icon16;
      _notifyIcon.Text = "IR Server";
    }

    /*
    ~IRServer()
    {
      Dispose(false);
    }
    */

    #endregion Constructor

    #region IDisposable

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        // Dispose managed resources ...

        StopServer();
        StopClient();

        _notifyIcon.Dispose();
        _notifyIcon = null;
      }

      // Free native resources ...

    }

    #endregion IDisposable

    #region Implementation

    /// <summary>
    /// Start the server.
    /// </summary>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    internal bool Start()
    {
      IrssLog.Info("Starting IR Server ...");

      LoadSettings();

      #region Process Priority Adjustment

      if (!_processPriority.Equals("No Change", StringComparison.OrdinalIgnoreCase))
      {
        try
        {
          ProcessPriorityClass priority = (ProcessPriorityClass)Enum.Parse(typeof(ProcessPriorityClass), _processPriority);
          Process.GetCurrentProcess().PriorityClass = priority;

          IrssLog.Info("Process priority set to: {0}", _processPriority);
        }
        catch (Exception ex)
        {
          IrssLog.Error(ex);
        }
      }

      #endregion Process Priority Adjustment

      #region Load plugin(s)

      _pluginReceive  = null;
      _pluginTransmit = null;

      if (_pluginNameReceive == null && String.IsNullOrEmpty(_pluginNameTransmit))
      {
        IrssLog.Warn("No transmit or receive plugins loaded");
      }
      else
      {
        if (_pluginNameReceive == null)
        {
          IrssLog.Warn("No receiver plugins loaded");
        }
        else
        {
          _pluginReceive = new List<PluginBase>(_pluginNameReceive.Length);

          for (int index = 0; index < _pluginNameReceive.Length; index++)
          {
            try
            {
              string pluginName = _pluginNameReceive[index];

              PluginBase plugin = Program.GetPlugin(pluginName);

              if (plugin == null)
              {
                IrssLog.Warn("Receiver plugin not found: {0}", pluginName);
              }
              else
              {
                _pluginReceive.Add(plugin);

                if (!String.IsNullOrEmpty(_pluginNameTransmit) && plugin.Name.Equals(_pluginNameTransmit, StringComparison.OrdinalIgnoreCase))
                  _pluginTransmit = plugin;
              }
            }
            catch (Exception ex)
            {
              IrssLog.Error(ex);
            }
          }

          if (_pluginReceive.Count == 0)
            _pluginReceive = null;
        }

        if (String.IsNullOrEmpty(_pluginNameTransmit))
        {
          IrssLog.Warn("No transmit plugin loaded");
        }
        else if (_pluginTransmit == null)
        {
          try
          {
            _pluginTransmit = Program.GetPlugin(_pluginNameTransmit);
          }
          catch (Exception ex)
          {
            IrssLog.Error(ex);
          }
        }
      }
      
      #endregion Load plugin(s)

      #region Mode select

      try
      {
        switch (_mode)
        {
          case IRServerMode.ServerMode:
            StartServer();
            IrssLog.Info("Started in Server Mode");
            break;

          case IRServerMode.RelayMode:
            if (StartRelay())
              IrssLog.Info("Started in Relay Mode");
            else
              IrssLog.Error("Failed to start in Relay Mode");
            break;

          case IRServerMode.RepeaterMode:
            if (StartRepeater())
              IrssLog.Info("Started in Repeater Mode");
            else
              IrssLog.Error("Failed to start in Repeater Mode");
            break;
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error("Failed to start Input Service Communications");
        IrssLog.Error(ex);
      }

      #endregion Mode select

      #region Start plugin(s)

      bool startedTransmit = false;

      if (_pluginReceive != null)
      {
        List<PluginBase> removePlugins = new List<PluginBase>();

        foreach (PluginBase plugin in _pluginReceive)
        {
          try
          {
            plugin.Start();

            IRemoteReceiver remoteReceiver = plugin as IRemoteReceiver;
            if (remoteReceiver != null)
              remoteReceiver.RemoteCallback += new RemoteHandler(RemoteHandlerCallback);

            IKeyboardReceiver keyboardReceiver = plugin as IKeyboardReceiver;
            if (keyboardReceiver != null)
              keyboardReceiver.KeyboardCallback += new KeyboardHandler(KeyboardHandlerCallback);

            IMouseReceiver mouseReceiver = plugin as IMouseReceiver;
            if (mouseReceiver != null)
              mouseReceiver.MouseCallback += new MouseHandler(MouseHandlerCallback);

            if (plugin.Name.Equals(_pluginNameTransmit, StringComparison.OrdinalIgnoreCase))
            {
              startedTransmit = true;
              IrssLog.Info("Transmit and Receive plugin started: \"{0}\"", plugin.Name);
            }
            else
            {
              IrssLog.Info("Receiver plugin started: \"{0}\"", plugin.Name);
            }
          }
          catch (Exception ex)
          {
            IrssLog.Error("Failed to start receive plugin: \"{0}\"", plugin.Name);
            IrssLog.Error(ex);

            removePlugins.Add(plugin);
          }
        }

        if (removePlugins.Count > 0)
          foreach (PluginBase plugin in removePlugins)
            _pluginReceive.Remove(plugin);

        if (_pluginReceive.Count == 0)
          _pluginReceive = null;
      }

      if (_pluginTransmit != null && !startedTransmit)
      {
        try
        {
          _pluginTransmit.Start();

          IrssLog.Info("Transmit plugin started: \"{0}\"", _pluginNameTransmit);
        }
        catch (Exception ex)
        {
          IrssLog.Error("Failed to start transmit plugin: \"{0}\"", _pluginNameTransmit);
          IrssLog.Error(ex);

          _pluginTransmit = null;
        }
      }

      #endregion Start plugin(s)

      #region Setup Abstract Remote Model processing

      if (_abstractRemoteMode)
      {
        IrssLog.Info("IR Server is running in Abstract Remote mode");

        _abstractRemoteButtons = new DataSet("AbstractRemoteButtons");
        _abstractRemoteButtons.CaseSensitive = true;

        if (_pluginReceive != null)
          foreach (PluginBase plugin in _pluginReceive)
            if (plugin is IRemoteReceiver)
              LoadAbstractDeviceFiles(plugin.Name);
      }

      #endregion Setup Abstract Remote Model processing

      _notifyIcon.Visible = true;

      SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);

      IrssLog.Info("IR Server started");

      return true;
    }

    /// <summary>
    /// Stop the server.
    /// </summary>
    internal void Stop()
    {
      IrssLog.Info("Stopping IR Server ...");

      SystemEvents.PowerModeChanged -= new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);

      _notifyIcon.Visible = false;

      if (_mode == IRServerMode.ServerMode)
      {
        try
        {
          IrssMessage message = new IrssMessage(MessageType.ServerShutdown, MessageFlags.Notify);
          SendToAll(message);
        }
        catch (Exception ex)
        {
          IrssLog.Error(ex);
        }
      }

      // Stop Plugin(s) ...
      bool stoppedTransmit = false;

      if (_pluginReceive != null && _pluginReceive.Count > 0)
      {
        foreach (PluginBase plugin in _pluginReceive)
        {
          try
          {
            IRemoteReceiver remoteReceiver = plugin as IRemoteReceiver;
            if (remoteReceiver != null)
              remoteReceiver.RemoteCallback -= new RemoteHandler(RemoteHandlerCallback);

            IKeyboardReceiver keyboardReceiver = plugin as IKeyboardReceiver;
            if (keyboardReceiver != null)
              keyboardReceiver.KeyboardCallback -= new KeyboardHandler(KeyboardHandlerCallback);

            IMouseReceiver mouseReceiver = plugin as IMouseReceiver;
            if (mouseReceiver != null)
              mouseReceiver.MouseCallback -= new MouseHandler(MouseHandlerCallback);

            plugin.Stop();

            if (plugin == _pluginTransmit)
            {
              stoppedTransmit = true;
              IrssLog.Info("Transmit and Receive plugin stopped: \"{0}\"", plugin.Name);
            }
            else
            {
              IrssLog.Info("Receiver plugin stopped: \"{0}\"", plugin.Name);
            }
          }
          catch (Exception ex)
          {
            IrssLog.Error(ex);
          }
        }
      }

      _pluginReceive = null;

      try
      {
        if (_pluginTransmit != null && !stoppedTransmit)
          _pluginTransmit.Stop();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
      finally
      {
        _pluginTransmit = null;
      }

      // Stop Server
      try
      {
        switch (_mode)
        {
          case IRServerMode.ServerMode:
            StopServer();
            break;

          case IRServerMode.RelayMode:
            StopRelay();
            break;

          case IRServerMode.RepeaterMode:
            StopRepeater();
            break;
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
    }

    void Configure()
    {
      _inConfiguration = true;

      try
      {
        Config config             = new Config();
        config.AbstractRemoteMode = _abstractRemoteMode;
        config.Mode               = _mode;
        config.HostComputer       = _hostComputer;
        config.ProcessPriority    = _processPriority;
        config.PluginReceive      = _pluginNameReceive;
        config.PluginTransmit     = _pluginNameTransmit;

        if (config.ShowDialog() == DialogResult.OK)
        {
          if ((_abstractRemoteMode != config.AbstractRemoteMode)  ||
              (_mode               != config.Mode)                ||
              (_hostComputer       != config.HostComputer)        ||
              (_processPriority    != config.ProcessPriority)     ||
              (_pluginNameReceive  != config.PluginReceive)       ||
              (_pluginNameTransmit != config.PluginTransmit)  )
          {
            Stop(); // Shut down communications

            // Change settings ...
            _abstractRemoteMode = config.AbstractRemoteMode;
            _mode               = config.Mode;
            _hostComputer       = config.HostComputer;
            _processPriority    = config.ProcessPriority;
            _pluginNameReceive  = config.PluginReceive;
            _pluginNameTransmit = config.PluginTransmit;

            SaveSettings();

            Thread.Sleep(1000);

            Start();  // Restart communications
          }
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }

      _inConfiguration = false;
    }

    void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
    {
      switch (e.Mode)
      {

        #region Suspend
        case PowerModes.Suspend:
          IrssLog.Info("Enter standby ...");

          bool suspendedTransmit = false;

          if (_pluginReceive != null)
          {
            foreach (PluginBase plugin in _pluginReceive)
            {
              try
              {
                plugin.Suspend();

                if (plugin == _pluginTransmit)
                  suspendedTransmit = true;
              }
              catch (Exception ex)
              {
                IrssLog.Error(ex);
              }
            }
          }

          if (_pluginTransmit != null && !suspendedTransmit)
          {
            try
            {
              _pluginTransmit.Suspend();
            }
            catch (Exception ex)
            {
              IrssLog.Error(ex);
            }
          }

          // Inform clients ...
          if (_mode == IRServerMode.ServerMode)
          {
            IrssMessage message = new IrssMessage(MessageType.ServerSuspend, MessageFlags.Notify);
            SendToAll(message);
          }
          break;
        #endregion Suspend

        #region Resume
        case PowerModes.Resume:
          IrssLog.Info("Resume from standby ...");

          bool resumedTransmit = false;

          if (_pluginReceive != null)
          {
            foreach (PluginBase plugin in _pluginReceive)
            {
              try
              {
                if (plugin == _pluginTransmit)
                  resumedTransmit = true;

                plugin.Resume();
              }
              catch (Exception ex)
              {
                IrssLog.Error(ex);
              }
            }
          }

          if (_pluginTransmit != null && !resumedTransmit)
          {
            try
            {
              _pluginTransmit.Resume();
            }
            catch (Exception ex)
            {
              IrssLog.Error(ex);
            }
          }

          // Inform clients ...
          if (_mode == IRServerMode.ServerMode)
          {
            IrssMessage message = new IrssMessage(MessageType.ServerResume, MessageFlags.Notify);
            SendToAll(message);
          }
          break;
        #endregion Resume

      }
    }

    void StartServer()
    {
      if (_server != null)
        return;

      // Initialize registered client lists ...
      _registeredClients = new List<ClientManager>();
      _registeredRepeaters = new List<ClientManager>();

      ServerMessageSink sink = new ServerMessageSink(ServerReceivedMessage);
      _server = new Server(Server.DefaultPort, sink);
      _server.ClientDisconnectCallback = new WaitCallback(ClientDisconnect);

      _server.Start();
    }
    void StopServer()
    {
      if (_server != null)
      {
        _server.Dispose();
        _server = null;
      }

      if (_registeredClients != null)
      {
        _registeredClients.Clear();
        _registeredClients = null;
      }

      if (_registeredRepeaters != null)
      {
        _registeredRepeaters.Clear();
        _registeredRepeaters = null;
      }
    }

    void CommsFailure(object obj)
    {
      Exception ex = obj as Exception;

      if (ex != null)
        IrssLog.Error("Communications failure: {0}", ex.Message);
      else
        IrssLog.Error("Communications failure");

      StopClient();

      MessageBox.Show("Please report this error", "IR Server - Communications failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    void Connected(object obj)
    {
      IrssLog.Info("Connected to another server");

      if (_mode == IRServerMode.RepeaterMode)
      {
        IrssMessage message = new IrssMessage(MessageType.RegisterRepeater, MessageFlags.Request);
        _client.Send(message);
      }
    }
    void Disconnected(object obj)
    {
      IrssLog.Warn("Communications with other server has been lost");

      Thread.Sleep(1000);
    }

    void ClientDisconnect(object obj)
    {
      ClientManager clientManager = obj as ClientManager;

      if (clientManager != null)
      {
        UnregisterClient(clientManager);
        UnregisterRepeater(clientManager);
      }
    }

    bool StartClient(IPEndPoint endPoint)
    {
      if (_client != null)
        return false;

      ClientMessageSink sink = new ClientMessageSink(ClientReceivedMessage);

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
    void StopClient()
    {
      if (_client == null)
        return;

      _client.Dispose();
      _client = null;

      _registered = false;
    }

    bool StartRelay()
    {
      try
      {
        StartServer();

        IPAddress serverIP = Client.GetIPFromName(_hostComputer);
        IPEndPoint endPoint = new IPEndPoint(serverIP, IrssComms.Server.DefaultPort);

        StartClient(endPoint);

        return true;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        return false;
      }
    }
    void StopRelay()
    {
      try
      {
        StopServer();
        StopClient();
      }
      catch { }
    }

    bool StartRepeater()
    {
      try
      {
        StartServer();

        IPAddress serverIP = Client.GetIPFromName(_hostComputer);
        IPEndPoint endPoint = new IPEndPoint(serverIP, IrssComms.Server.DefaultPort);

        StartClient(endPoint);

        return true;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        return false;
      }
    }
    void StopRepeater()
    {
      try
      {
        if (_registered)
        {
          _registered = false;

          IrssMessage message = new IrssMessage(MessageType.UnregisterRepeater, MessageFlags.Request);
          _client.Send(message);
        }
      }
      catch
      {
        throw;
      }
      finally
      {
        StopServer();
        StopClient();
      }
    }

    void RemoteHandlerCallback(string deviceName, string keyCode)
    {
      IrssLog.Debug("{0} generated a remote event: {1}", deviceName, keyCode);

      string messageDeviceName  = deviceName;
      string messageKeyCode     = keyCode;

      switch (_mode)
      {
        case IRServerMode.ServerMode:
          if (_abstractRemoteMode)
          {
            string abstractButton = LookupAbstractButton(deviceName, keyCode);
            if (!String.IsNullOrEmpty(abstractButton))
            {
              messageDeviceName = "Abstract";
              messageKeyCode    = abstractButton;

              IrssLog.Info("Abstract Remote Button mapped: {0}", abstractButton);
            }
            else
            {
              IrssLog.Info("Abstract Remote Button not found: {0} ({1})", deviceName, keyCode);
            }
          }
          break;

        case IRServerMode.RelayMode:
          // Don't do anything in relay mode, just pass it on.
          break;

        case IRServerMode.RepeaterMode:
          IrssLog.Debug("Remote event ignored, IR Server is in Repeater Mode.");
          return;
      }

      byte[] deviceNameBytes = Encoding.ASCII.GetBytes(messageDeviceName);
      byte[] keyCodeBytes = Encoding.ASCII.GetBytes(messageKeyCode);

      byte[] bytes = new byte[8 + deviceNameBytes.Length + keyCodeBytes.Length];

      BitConverter.GetBytes(deviceNameBytes.Length).CopyTo(bytes, 0);
      deviceNameBytes.CopyTo(bytes, 4);
      BitConverter.GetBytes(keyCodeBytes.Length).CopyTo(bytes, 4 + deviceNameBytes.Length);
      keyCodeBytes.CopyTo(bytes, 8 + deviceNameBytes.Length);

      switch (_mode)
      {
        case IRServerMode.ServerMode:
          {
            IrssMessage message = new IrssMessage(MessageType.RemoteEvent, MessageFlags.Notify, bytes);
            SendToAll(message);
            break;
          }

        case IRServerMode.RelayMode:
          {
            IrssMessage message = new IrssMessage(MessageType.ForwardRemoteEvent, MessageFlags.Request, bytes);
            _client.Send(message);
            break;
          }
      }
    }
    void KeyboardHandlerCallback(string deviceName, int vKey, bool keyUp)
    {
      IrssLog.Debug("{0} generated a keyboard event: {1}, keyUp: {2}", deviceName, vKey, keyUp);

      byte[] bytes = new byte[8];
      BitConverter.GetBytes(vKey).CopyTo(bytes, 0);
      BitConverter.GetBytes(keyUp).CopyTo(bytes, 4);
      
      switch (_mode)
      {
        case IRServerMode.ServerMode:
          {
            IrssMessage message = new IrssMessage(MessageType.KeyboardEvent, MessageFlags.Notify, bytes);
            SendToAll(message);
            break;
          }

        case IRServerMode.RelayMode:
          {
            IrssMessage message = new IrssMessage(MessageType.ForwardKeyboardEvent, MessageFlags.Request, bytes);
            _client.Send(message);
            break;
          }

        case IRServerMode.RepeaterMode:
          {
            IrssLog.Debug("Keyboard event ignored, IR Server is in Repeater Mode.");
            break;
          }
      }
    }
    void MouseHandlerCallback(string deviceName, int deltaX, int deltaY, int buttons)
    {
      IrssLog.Debug("{0} generated a mouse Event - deltaX: {1}, deltaY: {2}, buttons: {3}", deviceName, deltaX, deltaY, buttons);

      byte[] bytes = new byte[12];
      BitConverter.GetBytes(deltaX).CopyTo(bytes, 0);
      BitConverter.GetBytes(deltaY).CopyTo(bytes, 4);
      BitConverter.GetBytes(buttons).CopyTo(bytes, 8);

      switch (_mode)
      {
        case IRServerMode.ServerMode:
          {
            IrssMessage message = new IrssMessage(MessageType.MouseEvent, MessageFlags.Notify, bytes);
            SendToAll(message);
            break;
          }

        case IRServerMode.RelayMode:
          {
            IrssMessage message = new IrssMessage(MessageType.ForwardMouseEvent, MessageFlags.Request, bytes);
            _client.Send(message);
            break;
          }

        case IRServerMode.RepeaterMode:
          {
            IrssLog.Debug("Mouse event ignored, IR Server is in Repeater Mode.");
            break;
          }
      }
    }

    void SendToAll(IrssMessage message)
    {
      IrssLog.Debug("SendToAll({0}, {1})", message.Type, message.Flags);

      List<ClientManager> unregister = new List<ClientManager>();

      lock (_registeredClients)
      {
        foreach (ClientManager client in _registeredClients)
        {
          if (!_server.Send(client, message))
          {
            IrssLog.Warn("Failed to send message to a client, unregistering client");

            // If a message doesn't get through then unregister that client
            unregister.Add(client);
          }
        }

        // Unregistering clients must be done as a two part process because otherwise the
        // foreach statement above would fail if you modified the _registeredClients list
        // while enumerating it.
        foreach (ClientManager client in unregister)
        {
          UnregisterClient(client);
        }
      }
    }
    void SendToAllExcept(ClientManager exceptClient, IrssMessage message)
    {
      IrssLog.Debug("SendToAllExcept({0}, {1})", message.Type, message.Flags);

      List<ClientManager> unregister = new List<ClientManager>();

      lock (_registeredClients)
      {
        foreach (ClientManager client in _registeredClients)
        {
          if (client == exceptClient)
            continue;
          
          if (!_server.Send(client, message))
          {
            IrssLog.Warn("Failed to send message to a client, unregistering client");

            // If a message doesn't get through then unregister that client
            unregister.Add(client);
          }
        }

        // Unregistering clients must be done as a two part process because otherwise the
        // foreach statement above would fail if you modified the _registeredClients list
        // while enumerating it.
        foreach (ClientManager client in unregister)
        {
          UnregisterClient(client);
        }
      }
    }
    void SendTo(ClientManager receiver, IrssMessage message)
    {
      IrssLog.Debug("SendTo({0}, {1})", message.Type, message.Flags);
      
      if (!_server.Send(receiver, message))
      {
        IrssLog.Warn("Failed to send message to a client, unregistering client");

        // If a message doesn't get through then unregister that client
        UnregisterClient(receiver);
      }
    }
    void SendToRepeaters(IrssMessage message)
    {
      IrssLog.Debug("SendToRepeaters({0}, {1})", message.Type, message.Flags);

      List<ClientManager> unregister = new List<ClientManager>();

      lock (_registeredRepeaters)
      {
        foreach (ClientManager client in _registeredRepeaters)
        {
          if (!_server.Send(client, message))
          {
            IrssLog.Warn("Failed to send message to a repeater, unregistering repeater");

            // If a message doesn't get through then unregister that repeater
            unregister.Add(client);
          }
        }

        // Unregistering repeaters must be done as a two part process because otherwise the
        // foreach statement above would fail if you modified the _registeredRepeaters list
        // while enumerating it.
        foreach (ClientManager repeater in unregister)
        {
          UnregisterRepeater(repeater);
        }
      }
    }

    bool RegisterClient(ClientManager addClient)
    {
      lock (_registeredClients)
      {
        if (!_registeredClients.Contains(addClient))
          _registeredClients.Add(addClient);
      }

      IrssLog.Info("Registered a client");
      return true;
    }
    bool UnregisterClient(ClientManager removeClient)
    {
      lock (_registeredClients)
      {
        if (!_registeredClients.Contains(removeClient))
          return false;

        _registeredClients.Remove(removeClient);
      }

      IrssLog.Info("Unregistered a client");
      return true;
    }

    bool RegisterRepeater(ClientManager addRepeater)
    {
      lock (_registeredRepeaters)
      {
        if (!_registeredRepeaters.Contains(addRepeater))
          _registeredRepeaters.Add(addRepeater);
      }

      IrssLog.Info("Registered a repeater");
      return true;
    }
    bool UnregisterRepeater(ClientManager removeRepeater)
    {
      lock (_registeredRepeaters)
      {
        if (!_registeredRepeaters.Contains(removeRepeater))
          return false;

        _registeredRepeaters.Remove(removeRepeater);
      }

      IrssLog.Info("Unregistered a repeater");
      return true;
    }

    bool BlastIR(byte[] data)
    {
      try
      {
        IrssLog.Info("Blast IR");

        if (_pluginTransmit == null)
        {
          IrssLog.Warn("No transmit plugin loaded, can't blast");
          return false;
        }

        ITransmitIR _blaster = _pluginTransmit as ITransmitIR;
        if (_blaster == null)
        {
          IrssLog.Error("Active transmit plugin doesn't support blasting!");
          return false;
        }

        string port = "Default";

        int portLen = BitConverter.ToInt32(data, 0);
        if (portLen > 0)
          port = Encoding.ASCII.GetString(data, 4, portLen);

        byte[] codeData = new byte[data.Length - (4 + portLen)];
        Array.Copy(data, 4 + portLen, codeData, 0, codeData.Length);

        return _blaster.Transmit(port, codeData);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        return false;
      }
    }
    LearnStatus LearnIR(out byte[] data)
    {
      IrssLog.Info("Learn IR");

      data = null;

      if (_pluginTransmit == null)
      {
        IrssLog.Warn("No transmit plugin loaded, can't learn");
        return LearnStatus.Failure;
      }

      ILearnIR _learner = _pluginTransmit as ILearnIR;
      if (_learner == null)
      {
        IrssLog.Warn("Active transmit plugin doesn't support learn");
        return LearnStatus.Failure;
      }

      Thread.Sleep(250);

      LearnStatus status = LearnStatus.Failure;

      try
      {
        status = _learner.Learn(out data);
        switch (status)
        {
          case LearnStatus.Success:
            IrssLog.Info("Learn IR success");
            break;

          case LearnStatus.Failure:
            IrssLog.Error("Failed to learn IR Code");
            break;

          case LearnStatus.Timeout:
            IrssLog.Warn("IR Code learn timed out");
            break;
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }

      return status;
    }

    void ServerReceivedMessage(MessageManagerCombo combo)
    {
      IrssLog.Debug("Server message received: {0}, {1}", combo.Message.Type, combo.Message.Flags);

      try
      {
        switch (combo.Message.Type)
        {

          #region ForwardRemoteEvent
          case MessageType.ForwardRemoteEvent:
            if (_mode == IRServerMode.RelayMode)
            {
              IrssMessage forward = new IrssMessage(MessageType.ForwardRemoteEvent, MessageFlags.Request, combo.Message.GetDataAsBytes());
              _client.Send(forward);
            }
            else
            {
              byte[] data = combo.Message.GetDataAsBytes();

              if (_abstractRemoteMode)
              {
                // Decode message ...
                int deviceNameSize = BitConverter.ToInt32(data, 0);
                string deviceName = Encoding.ASCII.GetString(data, 4, deviceNameSize);
                int keyCodeSize = BitConverter.ToInt32(data, 4 + deviceNameSize);
                string keyCode = Encoding.ASCII.GetString(data, 8 + deviceNameSize, keyCodeSize);

                // Check that the device maps are loaded for the forwarded device
                bool foundDevice = false;
                foreach (PluginBase plugin in _pluginReceive)
                {
                  if (plugin is IRemoteReceiver && plugin.Name.Equals(deviceName, StringComparison.OrdinalIgnoreCase))
                  {
                    foundDevice = true;
                    break;
                  }
                }

                // If the remote maps are not already loaded for this device then attempt to load them
                if (!foundDevice)
                  foundDevice = LoadAbstractDeviceFiles(deviceName);

                // If the device map is loaded then try to convert the button to abstract
                if (foundDevice)
                {
                  // Find abstract button mapping
                  string abstractButton = LookupAbstractButton(deviceName, keyCode);
                  if (String.IsNullOrEmpty(abstractButton))
                  {
                    IrssLog.Info("Abstract Remote Button mapped from forwarded remote event: {0}", abstractButton);

                    // Encode new message ...
                    byte[] deviceNameBytes = Encoding.ASCII.GetBytes("Abstract");
                    byte[] keyCodeBytes = Encoding.ASCII.GetBytes(abstractButton);

                    data = new byte[8 + deviceNameBytes.Length + keyCodeBytes.Length];

                    BitConverter.GetBytes(deviceNameBytes.Length).CopyTo(data, 0);
                    deviceNameBytes.CopyTo(data, 4);
                    BitConverter.GetBytes(keyCodeBytes.Length).CopyTo(data, 4 + deviceNameBytes.Length);
                    keyCodeBytes.CopyTo(data, 8 + deviceNameBytes.Length);
                  }
                  else
                  {
                    IrssLog.Info("Abstract Remote Button not found for forwarded remote event: {0} ({1})", deviceName, keyCode);
                  }
                }
              }

              IrssMessage forward = new IrssMessage(MessageType.RemoteEvent, MessageFlags.Notify, data);
              SendToAllExcept(combo.Manager, forward);
            }
            break;
          #endregion ForwardRemoteEvent

          #region ForwardKeyboardEvent
          case MessageType.ForwardKeyboardEvent:
            if (_mode == IRServerMode.RelayMode)
            {
              IrssMessage forward = new IrssMessage(MessageType.ForwardKeyboardEvent, MessageFlags.Request, combo.Message.GetDataAsBytes());
              _client.Send(forward);
            }
            else
            {
              IrssMessage forward = new IrssMessage(MessageType.KeyboardEvent, MessageFlags.Notify, combo.Message.GetDataAsBytes());
              SendToAllExcept(combo.Manager, forward);
            }
            break;
          #endregion ForwardKeyboardEvent

          #region ForwardMouseEvent
          case MessageType.ForwardMouseEvent:
            if (_mode == IRServerMode.RelayMode)
            {
              IrssMessage forward = new IrssMessage(MessageType.ForwardMouseEvent, MessageFlags.Request, combo.Message.GetDataAsBytes());
              _client.Send(forward);
            }
            else
            {
              IrssMessage forward = new IrssMessage(MessageType.MouseEvent, MessageFlags.Notify, combo.Message.GetDataAsBytes());
              SendToAllExcept(combo.Manager, forward);
            }
            break;
          #endregion ForwardMouseEvent

          #region BlastIR
          case MessageType.BlastIR:
          {
            IrssMessage response = new IrssMessage(MessageType.BlastIR, MessageFlags.Response);

            if (_mode == IRServerMode.RelayMode)
            {
              response.Flags |= MessageFlags.Failure;
            }
            else
            {
              if (_registeredRepeaters.Count > 0)
                SendToRepeaters(combo.Message);

              if (BlastIR(combo.Message.GetDataAsBytes()))
                response.Flags |= MessageFlags.Success;
              else
                response.Flags |= MessageFlags.Failure;
            }

            if ((combo.Message.Flags & MessageFlags.ForceNotRespond) != MessageFlags.ForceNotRespond)
              SendTo(combo.Manager, response);
            
            break;
          }
          #endregion BlastIR

          #region LearnIR
          case MessageType.LearnIR:
          {
            IrssMessage response = new IrssMessage(MessageType.LearnIR, MessageFlags.Response);

            if (_mode == IRServerMode.RelayMode)
            {
              response.Flags |= MessageFlags.Failure;
            }
            else
            {
              byte[] bytes;
              LearnStatus status = LearnIR(out bytes);

              switch (status)
              {
                case LearnStatus.Success:
                  response.Flags |= MessageFlags.Success;
                  response.SetDataAsBytes(bytes);
                  break;

                case LearnStatus.Failure:
                  response.Flags |= MessageFlags.Failure;
                  break;

                case LearnStatus.Timeout:
                  response.Flags |= MessageFlags.Timeout;
                  break;
              }
            }

            SendTo(combo.Manager, response);
            break;
          }
          #endregion LearnIR

          #region ServerShutdown
          case MessageType.ServerShutdown:
            if ((combo.Message.Flags & MessageFlags.Request) == MessageFlags.Request)
            {
              IrssLog.Info("Shutdown command received");
              Stop();
              Application.Exit();
            }
            break;
          #endregion ServerShutdown

          #region RegisterClient
          case MessageType.RegisterClient:
          {
            IrssMessage response = new IrssMessage(MessageType.RegisterClient, MessageFlags.Response);

            if (RegisterClient(combo.Manager))
            {
              IRServerInfo irServerInfo = new IRServerInfo();

              if (_pluginReceive != null)
                irServerInfo.CanReceive = true;

              if (_pluginTransmit != null)
              {
                irServerInfo.CanLearn = (_pluginTransmit is ILearnIR);
                irServerInfo.CanTransmit = true;
                irServerInfo.Ports = (_pluginTransmit as ITransmitIR).AvailablePorts;
              }

              response.SetDataAsBytes(irServerInfo.ToBytes());
              response.Flags |= MessageFlags.Success;
            }
            else
            {
              response.Flags |= MessageFlags.Failure;
            }

            SendTo(combo.Manager, response);
            break;
          }
          #endregion RegisterClient

          #region UnregisterClient
          case MessageType.UnregisterClient:
            UnregisterClient(combo.Manager);
            break;
          #endregion UnregisterClient

          #region RegisterRepeater
          case MessageType.RegisterRepeater:
            {
              IrssMessage response = new IrssMessage(MessageType.RegisterRepeater, MessageFlags.Response);

              if (RegisterRepeater(combo.Manager))
                response.Flags |= MessageFlags.Success;
              else
                response.Flags |= MessageFlags.Failure;

              SendTo(combo.Manager, response);
              break;
            }
          #endregion RegisterRepeater

          #region UnregisterRepeater
          case MessageType.UnregisterRepeater:
            UnregisterRepeater(combo.Manager);
            break;
          #endregion UnregisterRepeater

          #region ActiveBlasters
          case MessageType.ActiveBlasters:
            {
              IrssMessage response = new IrssMessage(MessageType.ActiveBlasters, MessageFlags.Response);
              response.SetDataAsString(_pluginNameTransmit);
              
              SendTo(combo.Manager, response);
              break;
            }
          #endregion ActiveBlasters

          #region ActiveReceivers
          case MessageType.ActiveReceivers:
            {
              IrssMessage response = new IrssMessage(MessageType.ActiveReceivers, MessageFlags.Response);

              if (_pluginNameReceive != null)
              {
                StringBuilder receivers = new StringBuilder();
                for (int index = 0; index < _pluginNameReceive.Length; index++)
                {
                  receivers.Append(_pluginNameReceive[index]);

                  if (index < _pluginNameReceive.Length - 1)
                    receivers.Append(',');
                }

                response.SetDataAsString(receivers.ToString());
              }
              else
              {
                response.SetDataAsString(null);
              }

              SendTo(combo.Manager, response);
              break;
            }
          #endregion ActiveReceivers

          #region AvailableBlasters
          case MessageType.AvailableBlasters:
            {
              IrssMessage response = new IrssMessage(MessageType.AvailableBlasters, MessageFlags.Response);

              PluginBase[] plugins = Program.AvailablePlugins();
              StringBuilder blasters = new StringBuilder();

              for (int index = 0; index < plugins.Length; index++)
              {
                PluginBase plugin = plugins[index];

                if (plugin is ITransmitIR)
                {
                  blasters.Append(plugin.Name);
                  blasters.Append(',');
                }
              }

              if (blasters.Length == 0)
              {
                response.SetDataAsString(null);
              }
              else
              {
                blasters.Remove(blasters.Length - 1, 1);
                response.SetDataAsString(blasters.ToString());
              }
              
              SendTo(combo.Manager, response);
              break;
            }
          #endregion AvailableBlasters

          #region AvailableReceivers
          case MessageType.AvailableReceivers:
            {
              IrssMessage response = new IrssMessage(MessageType.AvailableReceivers, MessageFlags.Response);

              PluginBase[] plugins = Program.AvailablePlugins();
              StringBuilder receivers = new StringBuilder();

              for (int index = 0; index < plugins.Length; index++)
              {
                PluginBase plugin = plugins[index];

                if (plugin is IRemoteReceiver || plugin is IKeyboardReceiver || plugin is IMouseReceiver)
                {
                  receivers.Append(plugin.Name);
                  receivers.Append(',');
                }
              }

              if (receivers.Length == 0)
              {
                response.SetDataAsString(null);
              }
              else
              {
                receivers.Remove(receivers.Length - 1, 1);
                response.SetDataAsString(receivers.ToString());
              }

              SendTo(combo.Manager, response);
              break;
            }
          #endregion AvailableReceivers

          #region DetectedBlasters
          case MessageType.DetectedBlasters:
            {
              IrssMessage response = new IrssMessage(MessageType.DetectedBlasters, MessageFlags.Response);
              string[] detectedBlasters = Program.DetectBlasters();

              if (detectedBlasters != null)
              {
                StringBuilder blasters = new StringBuilder();
                for (int index = 0; index < detectedBlasters.Length; index++)
                {
                  blasters.Append(detectedBlasters[index]);

                  if (index < detectedBlasters.Length - 1)
                    blasters.Append(',');
                }

                response.SetDataAsString(blasters.ToString());
              }
              else
              {
                response.SetDataAsString(null);
              }

              SendTo(combo.Manager, response);
              break;
            }
          #endregion DetectedBlasters

          #region DetectedReceivers
          case MessageType.DetectedReceivers:
            {
              IrssMessage response = new IrssMessage(MessageType.DetectedReceivers, MessageFlags.Response);
              string[] detectedReceivers = Program.DetectReceivers();

              if (detectedReceivers != null)
              {
                StringBuilder receivers = new StringBuilder();
                for (int index = 0; index < detectedReceivers.Length; index++)
                {
                  receivers.Append(detectedReceivers[index]);

                  if (index < detectedReceivers.Length - 1)
                    receivers.Append(',');
                }

                response.SetDataAsString(receivers.ToString());
              }
              else
              {
                response.SetDataAsString(null);
              }

              SendTo(combo.Manager, response);
              break;
            }
          #endregion DetectedReceivers

        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        IrssMessage response = new IrssMessage(MessageType.Error, MessageFlags.Notify, ex.Message);
        SendTo(combo.Manager, response);
      }
    }
    void ClientReceivedMessage(IrssMessage received)
    {
      IrssLog.Debug("Client message received: {0}, {1}", received.Type, received.Flags);

      try
      {
        switch (received.Type)
        {
          case MessageType.RegisterClient:
            if ((received.Flags & MessageFlags.Response) == MessageFlags.Response)
            {
              if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
              {
                IrssLog.Info("Registered with host server");
                _registered = true;
              }
              else
              {
                IrssLog.Warn("Host server refused registration");
                _registered = false;
              }
            }
            break;

          case MessageType.ServerShutdown:
            if ((received.Flags & MessageFlags.Notify) == MessageFlags.Notify)
            {
              IrssLog.Warn("Host server has shut down");
              _registered = false;
            }
            break;
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        IrssMessage response = new IrssMessage(MessageType.Error, MessageFlags.Notify, ex.Message);
        _client.Send(response);
      }
    }

    void ClickSetup(object sender, EventArgs e)
    {
      if (_inConfiguration)
        return;

      IrssLog.Info("Setup");

      Configure();
    }
    void ClickQuit(object sender, EventArgs e)
    {
      if (_inConfiguration)
        return;

      IrssLog.Info("Quit");

      Stop();
      Application.Exit();
    }

    void LoadSettings()
    {
      _abstractRemoteMode = false;
      _mode               = IRServerMode.ServerMode;
      _hostComputer       = String.Empty;
      _processPriority    = "No Change";
      _pluginNameReceive  = null;
      _pluginNameTransmit = String.Empty;

      XmlDocument doc = new XmlDocument();

      try
      {
        doc.Load(ConfigurationFile);
      }
      catch (DirectoryNotFoundException)
      {
        IrssLog.Error("No configuration file found ({0}), folder not found! Creating default configuration file", ConfigurationFile);

        Directory.CreateDirectory(Path.GetDirectoryName(ConfigurationFile));

        CreateDefaultSettings();
        return;
      }
      catch (FileNotFoundException)
      {
        IrssLog.Warn("No configuration file found ({0}), creating default configuration file", ConfigurationFile);

        CreateDefaultSettings();
        return;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        return;
      }

      try { _abstractRemoteMode = bool.Parse(doc.DocumentElement.Attributes["AbstractRemoteMode"].Value); }
      catch (Exception ex) { IrssLog.Warn(ex.ToString()); }

      try { _mode               = (IRServerMode)Enum.Parse(typeof(IRServerMode), doc.DocumentElement.Attributes["Mode"].Value, true); }
      catch (Exception ex) { IrssLog.Warn(ex.ToString()); }

      try { _hostComputer       = doc.DocumentElement.Attributes["HostComputer"].Value; }
      catch (Exception ex) { IrssLog.Warn(ex.ToString()); }

      try { _processPriority    = doc.DocumentElement.Attributes["ProcessPriority"].Value; }
      catch (Exception ex) { IrssLog.Warn(ex.ToString()); }

      try { _pluginNameTransmit = doc.DocumentElement.Attributes["PluginTransmit"].Value; }
      catch (Exception ex) { IrssLog.Warn(ex.ToString()); }

      try
      {
        string receivers        = doc.DocumentElement.Attributes["PluginReceive"].Value;
        if (!String.IsNullOrEmpty(receivers))
          _pluginNameReceive    = receivers.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
      }
      catch (Exception ex)
      {
        IrssLog.Warn(ex.ToString());
      }
    }
    void SaveSettings()
    {
      try
      {
        using (XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, Encoding.UTF8))
        {
          writer.Formatting = Formatting.Indented;
          writer.Indentation = 1;
          writer.IndentChar = (char)9;
          writer.WriteStartDocument(true);
          writer.WriteStartElement("settings"); // <settings>

          writer.WriteAttributeString("AbstractRemoteMode", _abstractRemoteMode.ToString());
          writer.WriteAttributeString("Mode", Enum.GetName(typeof(IRServerMode), _mode));
          writer.WriteAttributeString("HostComputer", _hostComputer);
          writer.WriteAttributeString("ProcessPriority", _processPriority);
          writer.WriteAttributeString("PluginTransmit", _pluginNameTransmit);

          if (_pluginNameReceive != null)
          {
            StringBuilder receivers = new StringBuilder();
            for (int index = 0; index < _pluginNameReceive.Length; index++)
            {
              receivers.Append(_pluginNameReceive[index]);

              if (index < _pluginNameReceive.Length - 1)
                receivers.Append(',');
            }
            writer.WriteAttributeString("PluginReceive", receivers.ToString());
          }
          else
          {
            writer.WriteAttributeString("PluginReceive", String.Empty);
          }

          writer.WriteEndElement(); // </settings>
          writer.WriteEndDocument();
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
    }
    void CreateDefaultSettings()
    {
      try
      {
        string[] blasters = Program.DetectBlasters();
        if (blasters == null)
          _pluginNameTransmit = String.Empty;
        else
          _pluginNameTransmit = blasters[0];

        string[] receivers = Program.DetectReceivers();
        if (receivers == null)
          _pluginNameReceive = null;
        else
          _pluginNameReceive = receivers;

        SaveSettings();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
    }

    string LookupAbstractButton(string deviceName, string keyCode)
    {
      if (_abstractRemoteButtons == null || _abstractRemoteButtons.Tables.Count == 0)
        return null;

      try
      {
        foreach (DataTable table in _abstractRemoteButtons.Tables)
        {
          string device = table.ExtendedProperties["Device"] as string;

          if (device.Equals(deviceName, StringComparison.OrdinalIgnoreCase))
          {
            string expression = String.Format("RawCode = '{0}'", keyCode);

            DataRow[] rows = table.Select(expression);
            if (rows.Length == 1)
            {
              string button = rows[0]["AbstractButton"] as string;
              if (!String.IsNullOrEmpty(button))
              {
                IrssLog.Debug("{0}, remote: {1}, device: {2}", button, table.ExtendedProperties["Remote"] as string, deviceName);
                return button;
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }

      return null;
    }

    bool LoadAbstractDeviceFiles(string device)
    {
      if (String.IsNullOrEmpty(device))
        return false;

      string path = Path.Combine(AbstractRemoteMapFolder, device);
      if (Directory.Exists(path))
        return false;

      try
      {
        string[] files = Directory.GetFiles(path, "*.xml", SearchOption.TopDirectoryOnly);
        foreach (string file in files)
        {
          string remote = Path.GetFileNameWithoutExtension(file);
          string tableName = String.Format("{0}:{1}", device, remote);
          if (_abstractRemoteButtons.Tables.Contains(tableName))
          {
            IrssLog.Warn("Abstract Remote Table already loaded ({0})", tableName);
            continue;
          }

          DataTable table = _abstractRemoteButtons.Tables.Add("RemoteTable");
          table.ReadXmlSchema(AbstractRemoteSchemaFile);
          table.ReadXml(file);

          table.ExtendedProperties.Add("Device", device);
          table.ExtendedProperties.Add("Remote", remote);

          table.TableName = tableName;

          IrssLog.Info("Abstract Remote Table ({0}) loaded", tableName);
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        return false;
      }

      return true;
    }

    #endregion Implementation

  }

}
