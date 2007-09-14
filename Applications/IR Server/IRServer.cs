using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

using NamedPipes;
using IRServerPluginInterface;
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

  internal class IRServer
  {

    #region Constants

    static readonly string ConfigurationFile = Common.FolderAppData + "IR Server\\IR Server.xml";

    #endregion Constants

    #region Variables

    NotifyIcon _notifyIcon;

    List<Client> _registeredClients;
    List<Client> _registeredRepeaters;

    MessageQueue _messageQueue;

    IRServerMode _mode;
    string _hostComputer;

    string _localPipeName = String.Empty;
    bool _registered = false; // Used for relay and repeater modes.

    string _pluginNameReceive       = String.Empty;
    IRServerPlugin _pluginReceive   = null;

    string _pluginNameTransmit      = String.Empty;
    IRServerPlugin _pluginTransmit  = null;

    bool _inConfiguration = false;

    #endregion Variables

    #region Constructor

    public IRServer()
    {
      _messageQueue = new MessageQueue(new MessageQueueSink(HandlePipeMessage));

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

    #endregion Constructor

    #region Implementation

    /// <summary>
    /// Start the server
    /// </summary>
    /// <returns>success.</returns>
    internal bool Start()
    {
      try
      {
        IrssLog.Info("Starting IR Server ...");

        LoadSettings();

        // Load IR Plugins ...
        _pluginReceive  = null;
        _pluginTransmit = null;

        if (String.IsNullOrEmpty(_pluginNameReceive) && String.IsNullOrEmpty(_pluginNameTransmit))
        {
          IrssLog.Warn("No transmit or receive plugin loaded");
        }
        else
        {
          if (String.IsNullOrEmpty(_pluginNameReceive))
          {
            IrssLog.Warn("No receiver plugin loaded");
          }
          else
          {
            _pluginReceive = Program.GetPlugin(_pluginNameReceive);
          }

          if (_pluginNameTransmit.Equals(_pluginNameReceive, StringComparison.InvariantCultureIgnoreCase))
          {
            _pluginTransmit = _pluginReceive;
            IrssLog.Info("Using the same plugin for transmit and receive");
          }
          else if (String.IsNullOrEmpty(_pluginNameTransmit))
          {
            IrssLog.Warn("No transmit plugin loaded");
          }
          else
          {
            _pluginTransmit = Program.GetPlugin(_pluginNameTransmit);
          }
            
        }

        _messageQueue.Start();

        switch (_mode)
        {
          case IRServerMode.ServerMode:
            {
              // Initialize registered client lists ...
              _registeredClients = new List<Client>();
              _registeredRepeaters = new List<Client>();

              // Start server pipe
              PipeAccess.StartServer(Common.ServerPipeName, new PipeMessageHandler(_messageQueue.Enqueue));

              IrssLog.Info("Server Mode: \\\\" + Environment.MachineName + "\\pipe\\" + Common.ServerPipeName);
              break;
            }

          case IRServerMode.RelayMode:
            {
              if (StartRelay())
                IrssLog.Info("Started in Relay Mode");
              else
                IrssLog.Error("Failed to start in Relay Mode");
              break;
            }

          case IRServerMode.RepeaterMode:
            {
              if (StartRepeater())
                IrssLog.Info("Started in Repeater Mode");
              else
                IrssLog.Error("Failed to start in Repeater Mode");
              break;
            }
        }

        // Start plugin(s) ...
        if (_pluginReceive != null)
        {
          try
          {
            if (_pluginReceive.Start())
              IrssLog.Info("Receiver plugin started: \"{0}\"", _pluginNameReceive);
            else
              IrssLog.Error("Failed to start receive plugin: \"{0}\"", _pluginNameReceive);
          }
          catch (Exception ex)
          {
            IrssLog.Error("Failed to start receive plugin: \"{0}\"", _pluginNameReceive);
            IrssLog.Error(ex.ToString());
          }
        }

        if (!_pluginNameTransmit.Equals(_pluginNameReceive, StringComparison.InvariantCultureIgnoreCase))
        {
          if (_pluginTransmit != null)
          {
            try
            {
              if (_pluginTransmit.Start())
                IrssLog.Info("Transmit plugin started: \"{0}\"", _pluginNameTransmit);
              else
                IrssLog.Error("Failed to start transmit plugin: \"{0}\"", _pluginNameTransmit);
            }
            catch (Exception ex)
            {
              IrssLog.Error("Failed to start transmit plugin: \"{0}\"", _pluginNameTransmit);
              IrssLog.Error(ex.ToString());
            }
          }
        }

        if (_pluginReceive != null)
        {
          if (_pluginReceive is IRemoteReceiver)
            (_pluginReceive as IRemoteReceiver).RemoteCallback += new RemoteHandler(RemoteHandlerCallback);

          if (_pluginReceive is IKeyboardReceiver)
            (_pluginReceive as IKeyboardReceiver).KeyboardCallback += new KeyboardHandler(KeyboardHandlerCallback);
          
          if (_pluginReceive is IMouseReceiver)
            (_pluginReceive as IMouseReceiver).MouseCallback += new MouseHandler(MouseHandlerCallback);
        }

        _notifyIcon.Visible = true;

        SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);

        IrssLog.Info("IR Server started");

        return true;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
        return false;
      }
    }

    /// <summary>
    /// Stop the server
    /// </summary>
    internal void Stop()
    {
      IrssLog.Info("Stopping IR Server ...");

      SystemEvents.PowerModeChanged -= new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);

      _notifyIcon.Visible = false;

      if (_mode == IRServerMode.ServerMode)
      {
        PipeMessage message = new PipeMessage(Common.ServerPipeName, Environment.MachineName, PipeMessageType.ServerShutdown, PipeMessageFlags.Notify);
        SendToAll(message);
      }

      if (_pluginReceive != null)
      {
        if (_pluginReceive is IRemoteReceiver)
          (_pluginReceive as IRemoteReceiver).RemoteCallback -= new RemoteHandler(RemoteHandlerCallback);
        
        if (_pluginReceive is IKeyboardReceiver)
          (_pluginReceive as IKeyboardReceiver).KeyboardCallback -= new KeyboardHandler(KeyboardHandlerCallback);
        
        if (_pluginReceive is IMouseReceiver)
          (_pluginReceive as IMouseReceiver).MouseCallback -= new MouseHandler(MouseHandlerCallback);
      }
      
      // Stop Plugin(s)
      try
      {
        if (_pluginReceive != null)
          _pluginReceive.Stop();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
      }
      try
      {
        if (_pluginTransmit != null && _pluginTransmit != _pluginReceive)
          _pluginTransmit.Stop();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
      }

      _messageQueue.Stop();

      // Stop Server
      try
      {
        switch (_mode)
        {
          case IRServerMode.ServerMode:
            if (PipeAccess.ServerRunning)
              PipeAccess.StopServer();
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
        IrssLog.Error(ex.ToString());
      }
    }

    void Configure()
    {
      _inConfiguration = true;

      try
      {
        Config config         = new Config();
        config.Mode           = _mode;
        config.HostComputer   = _hostComputer;
        config.PluginReceive  = _pluginNameReceive;
        config.PluginTransmit = _pluginNameTransmit;

        if (config.ShowDialog() == DialogResult.OK)
        {
          if ((_mode               != config.Mode)            ||
              (_hostComputer       != config.HostComputer)    ||
              (_pluginNameReceive  != config.PluginReceive)   ||
              (_pluginNameTransmit != config.PluginTransmit)  )
          {
            Stop(); // Shut down communications

            // Change settings ...
            _mode               = config.Mode;
            _hostComputer       = config.HostComputer;
            _pluginNameReceive  = config.PluginReceive;
            _pluginNameTransmit = config.PluginTransmit;

            SaveSettings(); // Save Settings

            Start();  // Restart communications
          }
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
      }

      _inConfiguration = false;
    }

    bool StartRelay()
    {
      bool retry = false;
      int pipeNumber = 1;
      string localPipeTest;

      try
      {
        do
        {
          localPipeTest = String.Format(Common.LocalPipeFormat, pipeNumber);

          if (PipeAccess.PipeExists(String.Format("\\\\.\\pipe\\{0}", localPipeTest)))
          {
            pipeNumber++;
            if (pipeNumber <= Common.MaximumLocalClientCount)
            {
              retry = true;
            }
            else
            {
              IrssLog.Error("Maximum local client limit ({0}) reached", Common.MaximumLocalClientCount);
              return false;
            }
          }
          else
          {
            PipeAccess.StartServer(localPipeTest, new PipeMessageHandler(_messageQueue.Enqueue));
            _localPipeName = localPipeTest;
            retry = false;
          }
        }
        while (retry);

        PipeMessage message = new PipeMessage(Environment.MachineName, _localPipeName, PipeMessageType.RegisterClient, PipeMessageFlags.Request);
        PipeAccess.SendMessage(Common.ServerPipeName, _hostComputer, message);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
        return false;
      }

      return true;
    }
    void StopRelay()
    {
      try
      {
        if (_registered)
        {
          _registered = false;

          PipeMessage message = new PipeMessage(Environment.MachineName, _localPipeName, PipeMessageType.UnregisterClient, PipeMessageFlags.Request);
          PipeAccess.SendMessage(Common.ServerPipeName, _hostComputer, message);
        }
      }
      catch { }

      try
      {
        if (PipeAccess.ServerRunning)
          PipeAccess.StopServer();
      }
      catch { }
    }

    bool StartRepeater()
    {
      bool retry = false;
      int pipeNumber = 1;
      string localPipeTest;

      try
      {
        do
        {
          localPipeTest = String.Format(Common.LocalPipeFormat, pipeNumber);

          if (PipeAccess.PipeExists(String.Format("\\\\.\\pipe\\{0}", localPipeTest)))
          {
            pipeNumber++;
            if (pipeNumber <= Common.MaximumLocalClientCount)
            {
              retry = true;
            }
            else
            {
              IrssLog.Error("Maximum local client limit ({0}) reached", Common.MaximumLocalClientCount);
              return false;
            }
          }
          else
          {
            PipeAccess.StartServer(localPipeTest, new PipeMessageHandler(_messageQueue.Enqueue));
            _localPipeName = localPipeTest;
            retry = false;
          }
        }
        while (retry);

        PipeMessage message = new PipeMessage(Environment.MachineName, _localPipeName, PipeMessageType.RegisterRepeater, PipeMessageFlags.Request);
        PipeAccess.SendMessage(Common.ServerPipeName, _hostComputer, message);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
        return false;
      }

      return true;
    }
    void StopRepeater()
    {
      try
      {
        if (_registered)
        {
          _registered = false;

          PipeMessage message = new PipeMessage(Environment.MachineName, _localPipeName, PipeMessageType.UnregisterRepeater, PipeMessageFlags.Request);
          PipeAccess.SendMessage(Common.ServerPipeName, _hostComputer, message);
        }
      }
      catch { }

      try
      {
        if (PipeAccess.ServerRunning)
          PipeAccess.StopServer();
      }
      catch { }
    }

    void RemoteHandlerCallback(string keyCode)
    {
      IrssLog.Debug("Remote Event: {0}", keyCode);

      byte[] bytes = Encoding.ASCII.GetBytes(keyCode);

      switch (_mode)
      {
        case IRServerMode.ServerMode:
          {
            PipeMessage message = new PipeMessage(Common.ServerPipeName, Environment.MachineName, PipeMessageType.RemoteEvent, PipeMessageFlags.Notify, bytes);
            SendToAll(message);
            break;
          }

        case IRServerMode.RelayMode:
          {
            PipeMessage message = new PipeMessage(Environment.MachineName, _localPipeName, PipeMessageType.ForwardRemoteEvent, PipeMessageFlags.Request, bytes);
            SendTo(Common.ServerPipeName, _hostComputer, message);
            break;
          }

        case IRServerMode.RepeaterMode:
          {
            IrssLog.Debug("Remote event ignored, IR Server is in Repeater Mode.");
            break;
          }
      }
    }

    void KeyboardHandlerCallback(int vKey, bool keyUp)
    {
      IrssLog.Debug("Keyboard Event: {0}, keyUp: {1}", vKey, keyUp);

      byte[] bytes = new byte[8];
      BitConverter.GetBytes(vKey).CopyTo(bytes, 0);
      BitConverter.GetBytes(keyUp).CopyTo(bytes, 4);
      
      switch (_mode)
      {
        case IRServerMode.ServerMode:
          {
            PipeMessage message = new PipeMessage(Common.ServerPipeName, Environment.MachineName, PipeMessageType.KeyboardEvent, PipeMessageFlags.Notify, bytes);
            SendToAll(message);
            break;
          }

        case IRServerMode.RelayMode:
          {
            PipeMessage message = new PipeMessage(Environment.MachineName, _localPipeName, PipeMessageType.ForwardKeyboardEvent, PipeMessageFlags.Request, bytes);
            SendTo(Common.ServerPipeName, _hostComputer, message);
            break;
          }

        case IRServerMode.RepeaterMode:
          {
            IrssLog.Debug("Keyboard event ignored, IR Server is in Repeater Mode.");
            break;
          }
      }
    }

    void MouseHandlerCallback(int deltaX, int deltaY, int buttons)
    {
      IrssLog.Debug("Mouse Event - deltaX: {0}, deltaY: {1}, buttons: {2}", deltaX, deltaY, buttons);

      byte[] bytes = new byte[12];
      BitConverter.GetBytes(deltaX).CopyTo(bytes, 0);
      BitConverter.GetBytes(deltaY).CopyTo(bytes, 4);
      BitConverter.GetBytes(buttons).CopyTo(bytes, 8);

      switch (_mode)
      {
        case IRServerMode.ServerMode:
          {
            PipeMessage message = new PipeMessage(Common.ServerPipeName, Environment.MachineName, PipeMessageType.MouseEvent, PipeMessageFlags.Notify, bytes);
            SendToAll(message);
            break;
          }

        case IRServerMode.RelayMode:
          {
            PipeMessage message = new PipeMessage(Environment.MachineName, _localPipeName, PipeMessageType.ForwardMouseEvent, PipeMessageFlags.Request, bytes);
            SendTo(Common.ServerPipeName, _hostComputer, message);
            break;
          }

        case IRServerMode.RepeaterMode:
          {
            IrssLog.Debug("Mouse event ignored, IR Server is in Repeater Mode.");
            break;
          }
      }
    }

    void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
    {
      switch (e.Mode)
      {
        case PowerModes.Resume:
          {
            IrssLog.Info("Resume from standby ...");

            if (_pluginReceive != null)
              _pluginReceive.Resume();

            if (_pluginTransmit != null && _pluginTransmit != _pluginReceive)
              _pluginTransmit.Resume();

            // TODO: Inform clients ?
            break;
          }

        case PowerModes.Suspend:
          {
            IrssLog.Info("Enter low-power standby ...");

            if (_pluginReceive != null)
              _pluginReceive.Suspend();

            if (_pluginTransmit != null && _pluginTransmit != _pluginReceive)
              _pluginTransmit.Suspend();

            // Inform clients ...
            if (_mode == IRServerMode.ServerMode)
            {
              PipeMessage message = new PipeMessage(Common.ServerPipeName, Environment.MachineName, PipeMessageType.ServerShutdown, PipeMessageFlags.Notify);
              SendToAll(message);
            }
            break;
          }
      }
    }

    void SendToAll(PipeMessage message)
    {
      IrssLog.Debug("SendToAll({0})", message);

      List<Client> unregister = new List<Client>();

      lock (_registeredClients)
      {
        foreach (Client client in _registeredClients)
        {
          try
          {
            PipeAccess.SendMessage(client.Pipe, client.Server, message);
          }
          catch (Exception ex)
          {
            IrssLog.Warn("Failed to send message to client ({0}\\{1}): {2}", client.Server, client.Pipe, ex.Message);

            // If a message doesn't get through then unregister that client
            unregister.Add(client);
          }
        }

        // Unregistering clients must be done as a two part process because otherwise the
        // foreach statement above would fail if you modified the _registeredClients list
        // while enumerating it.
        foreach (Client client in unregister)
        {
          UnregisterClient(client.Pipe, client.Server);
        }
      }
    }
    void SendToAllExcept(string exceptPipe, string exceptServer, PipeMessage message)
    {
      IrssLog.Debug("SendToAllExcept({0}, {1}, {2})", exceptPipe, exceptServer, message);

      List<Client> unregister = new List<Client>();

      lock (_registeredClients)
      {
        foreach (Client client in _registeredClients)
        {
          try
          {
            if (client.Pipe == exceptPipe && client.Server == exceptServer)
              continue;

            PipeAccess.SendMessage(client.Pipe, client.Server, message);
          }
          catch (Exception ex)
          {
            IrssLog.Warn("Failed to send message to client ({0}\\{1}): {2}", client.Server, client.Pipe, ex.Message);

            // If a message doesn't get through then unregister that client
            unregister.Add(client);
          }
        }

        // Unregistering clients must be done as a two part process because otherwise the
        // foreach statement above would fail if you modified the _registeredClients list
        // while enumerating it.
        foreach (Client client in unregister)
        {
          UnregisterClient(client.Pipe, client.Server);
        }
      }
    }
    void SendTo(string pipe, string server, PipeMessage message)
    {
      IrssLog.Debug("SendTo({0}, {1}, {2})", pipe, server, message);

      try
      {
        PipeAccess.SendMessage(pipe, server, message);
      }
      catch (Exception ex)
      {
        IrssLog.Warn("Failed to send message to client ({0}\\{1}): {2}", server, pipe, ex.Message);

        // If a message doesn't get through then unregister that client
        UnregisterClient(pipe, server);
      }
    }
    void SendToRepeaters(PipeMessage message)
    {
      IrssLog.Debug("SendToRepeaters({0})", message);

      List<Client> unregister = new List<Client>();

      lock (_registeredRepeaters)
      {
        foreach (Client client in _registeredRepeaters)
        {
          try
          {
            PipeAccess.SendMessage(client.Pipe, client.Server, message);
          }
          catch (Exception ex)
          {
            IrssLog.Warn("Failed to send message to client ({0}\\{1}): {2}", client.Server, client.Pipe, ex.Message);

            // If a message doesn't get through then unregister that client
            unregister.Add(client);
          }
        }

        // Unregistering clients must be done as a two part process because otherwise the
        // foreach statement above would fail if you modified the _registeredClients list
        // while enumerating it.
        foreach (Client client in unregister)
        {
          UnregisterRepeater(client.Pipe, client.Server);
        }
      }
    }

    bool RegisterClient(string pipe, string server)
    {
      if (String.IsNullOrEmpty(pipe) || String.IsNullOrEmpty(server))
        return false;

      if (_mode != IRServerMode.ServerMode)
        return false;

      bool alreadyRegistered = false;
      
      lock (_registeredClients)
      {
        foreach (Client client in _registeredClients)
        {
          if (client.Pipe == pipe && client.Server == server)
          {
            alreadyRegistered = true;
            break;
          }
        }

        if (!alreadyRegistered)
        {
          if (_registeredClients.Count >= Common.MaximumLocalClientCount)
            return false;
          else
            _registeredClients.Add(new Client(pipe, server));
        }
      }

      IrssLog.Info("Registered: \\\\{0}\\pipe\\{1}", server, pipe);
      return true;
    }
    bool UnregisterClient(string pipe, string server)
    {
      if (String.IsNullOrEmpty(pipe) || String.IsNullOrEmpty(server))
        return false;

      if (_mode != IRServerMode.ServerMode)
        return false;

      Client removeClient = new Client(pipe, server);

      lock (_registeredClients)
      {
        if (!_registeredClients.Contains(removeClient))
          return false;

        _registeredClients.Remove(removeClient);
      }

      IrssLog.Info("Unregistered: \\\\{0}\\pipe\\{1}", server, pipe);
      return true;
    }

    bool RegisterRepeater(string pipe, string server)
    {
      if (String.IsNullOrEmpty(pipe) || String.IsNullOrEmpty(server))
        return false;

      if (_mode != IRServerMode.ServerMode)
        return false;

      bool alreadyRegistered = false;

      lock (_registeredRepeaters)
      {
        foreach (Client client in _registeredRepeaters)
        {
          if (client.Pipe == pipe && client.Server == server)
          {
            alreadyRegistered = true;
            break;
          }
        }

        if (!alreadyRegistered)
        {
          if (_registeredRepeaters.Count >= Common.MaximumLocalClientCount)
            return false;
          else
            _registeredRepeaters.Add(new Client(pipe, server));
        }
      }

      IrssLog.Info("Registered Repeater: \\\\{0}\\pipe\\{1}", server, pipe);
      return true;
    }
    bool UnregisterRepeater(string pipe, string server)
    {
      if (String.IsNullOrEmpty(pipe) || String.IsNullOrEmpty(server))
        return false;

      if (_mode != IRServerMode.RepeaterMode)
        return false;

      Client removeClient = new Client(pipe, server);

      lock (_registeredRepeaters)
      {
        if (!_registeredRepeaters.Contains(removeClient))
          return false;

        _registeredRepeaters.Remove(removeClient);
      }

      IrssLog.Info("Unregistered Repeater: \\\\{0}\\pipe\\{1}", server, pipe);
      return true;
    }

    bool BlastIR(byte[] data)
    {
      try
      {
        IrssLog.Debug("Blast IR");

        if (_pluginTransmit == null || !(_pluginTransmit is ITransmitIR))
          return false;

        string port = "Default";

        int portLen = BitConverter.ToInt32(data, 0);
        if (portLen > 0)
          port = Encoding.ASCII.GetString(data, 4, portLen);

        byte[] codeData = new byte[data.Length - (4 + portLen)];
        Array.Copy(data, 4 + portLen, codeData, 0, codeData.Length);

        return (_pluginTransmit as ITransmitIR).Transmit(port, codeData);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
        return false;
      }
    }
    LearnStatus LearnIR(out byte[] data)
    {
      IrssLog.Debug("Learn IR");

      data = null;

      if (_pluginTransmit == null)
      {
        IrssLog.Debug("No transmit plugin loaded, can't learn");
        return LearnStatus.Failure;
      }
      else if (!(_pluginTransmit is ILearnIR))
      {
        IrssLog.Debug("Active transmit plugin doesn't support learn");
        return LearnStatus.Failure;
      }

      Thread.Sleep(250);

      LearnStatus status = LearnStatus.Failure;

      try
      {
        status = (_pluginTransmit as ILearnIR).Learn(out data);
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
        IrssLog.Error(ex.ToString());
      }

      return status;
    }

    void HandlePipeMessage(string message)
    {
      PipeMessage received = PipeMessage.FromString(message);
      if (received == null)
      {
        IrssLog.Warn("Invalid message received: {0}", message);
        return;
      }

      IrssLog.Debug("Message received from client \\\\{0}\\pipe\\{1} = {2}", received.FromServer, received.FromPipe, message);

      try
      {
        switch (received.Type)
        {
          case PipeMessageType.ForwardRemoteEvent:
            if (_mode == IRServerMode.RelayMode)
            {
              PipeMessage forward = new PipeMessage(Common.ServerPipeName, Environment.MachineName, PipeMessageType.ForwardRemoteEvent, PipeMessageFlags.Request, received.DataAsBytes);
              SendTo(Common.ServerPipeName, _hostComputer, forward);
            }
            else
            {
              PipeMessage forward = new PipeMessage(Common.ServerPipeName, Environment.MachineName, PipeMessageType.RemoteEvent, PipeMessageFlags.Notify, received.DataAsBytes);
              SendToAllExcept(received.FromPipe, received.FromServer, forward);
            }
            break;

          case PipeMessageType.ForwardKeyboardEvent:
            if (_mode == IRServerMode.RelayMode)
            {
              PipeMessage forward = new PipeMessage(Common.ServerPipeName, Environment.MachineName, PipeMessageType.ForwardKeyboardEvent, PipeMessageFlags.Request, received.DataAsBytes);
              SendTo(Common.ServerPipeName, _hostComputer, forward);
            }
            else
            {
              PipeMessage forward = new PipeMessage(Common.ServerPipeName, Environment.MachineName, PipeMessageType.KeyboardEvent, PipeMessageFlags.Notify, received.DataAsBytes);
              SendToAllExcept(received.FromPipe, received.FromServer, forward);
            }
            break;

          case PipeMessageType.ForwardMouseEvent:
            if (_mode == IRServerMode.RelayMode)
            {
              PipeMessage forward = new PipeMessage(Common.ServerPipeName, Environment.MachineName, PipeMessageType.ForwardMouseEvent, PipeMessageFlags.Request, received.DataAsBytes);
              SendTo(Common.ServerPipeName, _hostComputer, forward);
            }
            else
            {
              PipeMessage forward = new PipeMessage(Common.ServerPipeName, Environment.MachineName, PipeMessageType.MouseEvent, PipeMessageFlags.Notify, received.DataAsBytes);
              SendToAllExcept(received.FromPipe, received.FromServer, forward);
            }
            break;

          case PipeMessageType.BlastIR:
          {
            PipeMessage response = new PipeMessage(Common.ServerPipeName, Environment.MachineName, PipeMessageType.BlastIR, PipeMessageFlags.Response);

            if (_mode == IRServerMode.RelayMode)
            {
              response.Flags |= PipeMessageFlags.Failure;
            }
            else
            {
              if (_registeredRepeaters.Count > 0)
                SendToRepeaters(received);

              if (BlastIR(received.DataAsBytes))
                response.Flags |= PipeMessageFlags.Success;
              else
                response.Flags |= PipeMessageFlags.Failure;
            }

            if ((received.Flags & PipeMessageFlags.ForceNotRespond) != PipeMessageFlags.ForceNotRespond)
              SendTo(received.FromPipe, received.FromServer, response);
            
            break;
          }

          case PipeMessageType.LearnIR:
          {
            PipeMessage response = new PipeMessage(Common.ServerPipeName, Environment.MachineName, PipeMessageType.LearnIR, PipeMessageFlags.Response);

            if (_mode == IRServerMode.RelayMode)
            {
              response.Flags |= PipeMessageFlags.Failure;
            }
            else
            {
              byte[] bytes = null;

              LearnStatus status = LearnIR(out bytes);

              switch (status)
              {
                case LearnStatus.Success:
                  response.Flags |= PipeMessageFlags.Success;
                  response.DataAsBytes = bytes;
                  break;

                case LearnStatus.Failure:
                  response.Flags |= PipeMessageFlags.Failure;
                  break;

                case LearnStatus.Timeout:
                  response.Flags |= PipeMessageFlags.Timeout;
                  break;
              }
            }

            SendTo(received.FromPipe, received.FromServer, response);
            break;
          }

          case PipeMessageType.ServerShutdown:
            if (_mode == IRServerMode.ServerMode)
            {
              if ((received.Flags & PipeMessageFlags.Request) == PipeMessageFlags.Request)
              {
                IrssLog.Info("Shutdown command received");
                Stop();
                Application.Exit();
              }
            }
            else
            {
              if ((received.Flags & PipeMessageFlags.Notify) == PipeMessageFlags.Notify)
              {

                IrssLog.Warn("Host server has shut down");
                _registered = false;
              }
            }
            break;

          case PipeMessageType.Ping:
          {
            PipeMessage response = new PipeMessage(Common.ServerPipeName, Environment.MachineName, PipeMessageType.Echo, PipeMessageFlags.Response, received.DataAsBytes);
            SendTo(received.FromPipe, received.FromServer, response);
            break;
          }

          case PipeMessageType.RegisterClient:
            if (_mode == IRServerMode.ServerMode)
            {
              if ((received.Flags & PipeMessageFlags.Request) == PipeMessageFlags.Request)
              {
                PipeMessage response = new PipeMessage(Common.ServerPipeName, Environment.MachineName, PipeMessageType.RegisterClient, PipeMessageFlags.Response);

                if (RegisterClient(received.FromPipe, received.FromServer))
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

                  response.DataAsBytes = irServerInfo.ToBytes();
                  response.Flags |= PipeMessageFlags.Success;
                }
                else
                {
                  response.Flags |= PipeMessageFlags.Failure;
                }

                SendTo(received.FromPipe, received.FromServer, response);
              }
            }
            else if ((received.Flags & PipeMessageFlags.Response) == PipeMessageFlags.Response)
            {
              if ((received.Flags & PipeMessageFlags.Success) == PipeMessageFlags.Success)
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

          case PipeMessageType.UnregisterClient:
            UnregisterClient(received.FromPipe, received.FromServer);
            break;

          case PipeMessageType.RegisterRepeater:
            {
              PipeMessage response = new PipeMessage(Common.ServerPipeName, Environment.MachineName, PipeMessageType.RegisterRepeater, PipeMessageFlags.Response);
              
              if (RegisterRepeater(received.FromPipe, received.FromServer))
                response.Flags |= PipeMessageFlags.Success;
              else
                response.Flags |= PipeMessageFlags.Failure;

              SendTo(received.FromPipe, received.FromServer, response);
              break;
            }

          case PipeMessageType.UnregisterRepeater:
            UnregisterRepeater(received.FromPipe, received.FromServer);
            break;
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
        PipeMessage response = new PipeMessage(Common.ServerPipeName, Environment.MachineName, PipeMessageType.Error, PipeMessageFlags.Notify, ex.Message);
        SendTo(received.FromPipe, received.FromServer, response);
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
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationFile);

        _mode               = (IRServerMode)Enum.Parse(typeof(IRServerMode), doc.DocumentElement.Attributes["Mode"].Value, true);
        _hostComputer       = doc.DocumentElement.Attributes["HostComputer"].Value;
        _pluginNameReceive  = doc.DocumentElement.Attributes["PluginReceive"].Value;
        _pluginNameTransmit = doc.DocumentElement.Attributes["PluginTransmit"].Value;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());

        _mode               = IRServerMode.ServerMode;
        _hostComputer       = String.Empty;
        _pluginNameReceive  = String.Empty;
        _pluginNameTransmit = String.Empty;
      }
    }
    void SaveSettings()
    {
      try
      {
        XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, System.Text.Encoding.UTF8);
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 1;
        writer.IndentChar = (char)9;
        writer.WriteStartDocument(true);
        writer.WriteStartElement("settings"); // <settings>

        writer.WriteAttributeString("Mode", Enum.GetName(typeof(IRServerMode), _mode));
        writer.WriteAttributeString("HostComputer", _hostComputer);
        writer.WriteAttributeString("PluginReceive", _pluginNameReceive);
        writer.WriteAttributeString("PluginTransmit", _pluginNameTransmit);

        writer.WriteEndElement(); // </settings>
        writer.WriteEndDocument();
        writer.Close();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
      }
    }

    #endregion Implementation

  }

}
