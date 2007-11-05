using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Xml;

using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

using IRServerPluginInterface;
using IrssComms;
using IrssUtils;

namespace InputService
{

  #region Enumerations

  /// <summary>
  /// Describes the operation mode of the Input Service.
  /// </summary>
  public enum InputServiceMode
  {
    /// <summary>
    /// Acts as a standard Server (Default).
    /// </summary>
    ServerMode = 0,
    /// <summary>
    /// Relays button presses to another Input Service.
    /// </summary>
    RelayMode = 1,
    /// <summary>
    /// Acts as a repeater for another Input Service's blasting.
    /// </summary>
    RepeaterMode = 2,
  }

  #endregion Enumerations

  public class InputService : ServiceBase
  {

    #region Constants

    static readonly string ConfigurationFile = Common.FolderAppData + "Input Service\\Input Service.xml";

    #endregion Constants

    #region Variables

    List<ClientManager> _registeredClients;
    List<ClientManager> _registeredRepeaters;

    Server _server;
    Client _client;

    InputServiceMode _mode;
    string _hostComputer;

    bool _registered; // Used for relay and repeater modes.

    string[] _pluginNameReceive;
    IRServerPluginBase[] _pluginReceive;

    string _pluginNameTransmit;
    IRServerPluginBase _pluginTransmit;

    #endregion Variables

    #region Constructor / Destructor

    public InputService()
    {
      this.ServiceName = Program.ServiceName;
      
      //this.EventLog.Log = "Application";
      //this.AutoLog = true;

      this.CanHandlePowerEvent          = true;
      this.CanHandleSessionChangeEvent  = false;
      this.CanPauseAndContinue          = false;
      this.CanShutdown                  = true;
      this.CanStop                      = true;
    }

    #endregion Constructor / Destructor

    #region IDisposable

    protected override void Dispose(bool disposing)
    {
      try
      {
        if (disposing)
        {
          // Dispose managed resources ...

          StopServer();
          StopClient();
        }

        // Free native resources ...

      }
      finally
      {
        base.Dispose(disposing);
      }
    }

    #endregion IDisposable

    #region Service Methods

    protected override void OnStart(string[] args)
    {
      // TODO: Change log level to info for release.
      IrssLog.LogLevel = IrssLog.Level.Debug;
      IrssLog.Open(Common.FolderIrssLogs + "Input Service.log");

      try
      {
        IrssLog.Info("Starting Input Service ...");

        LoadSettings();

        // Load IR Plugins ...
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
            List<IRServerPluginBase> plugins = new List<IRServerPluginBase>(_pluginNameReceive.Length);

            for (int index = 0; index < _pluginNameReceive.Length; index++)
            {
              string pluginName = _pluginNameReceive[index];

              IRServerPluginBase plugin = Program.GetPlugin(pluginName);

              if (plugin == null)
              {
                IrssLog.Warn("Receiver plugin not found: {0}", pluginName);
              }
              else
              {
                plugins.Add(plugin);

                if (!String.IsNullOrEmpty(_pluginNameTransmit) && plugin.Name.Equals(_pluginNameTransmit))
                  _pluginTransmit = plugin;
              }
            }

            _pluginReceive = plugins.ToArray();
          }

          if (String.IsNullOrEmpty(_pluginNameTransmit))
          {
            IrssLog.Warn("No transmit plugin loaded");
          }
          else if (_pluginTransmit == null)
          {
            _pluginTransmit = Program.GetPlugin(_pluginNameTransmit);
          }
        }

        // Mode select ...
        switch (_mode)
        {
          case InputServiceMode.ServerMode:
            StartServer();
            IrssLog.Info("Started in Server Mode");
            break;

          case InputServiceMode.RelayMode:
            if (StartRelay())
              IrssLog.Info("Started in Relay Mode");
            else
              IrssLog.Error("Failed to start in Relay Mode");
            break;

          case InputServiceMode.RepeaterMode:
            if (StartRepeater())
              IrssLog.Info("Started in Repeater Mode");
            else
              IrssLog.Error("Failed to start in Repeater Mode");
            break;
        }

        // Start plugin(s) ...

        bool startedTransmit = false;

        if (_pluginReceive != null)
        {
          foreach (IRServerPluginBase plugin in _pluginReceive)
          {
            try
            {
              if (plugin.Start())
              {
                IRemoteReceiver remoteReceiver = plugin as IRemoteReceiver;
                if (remoteReceiver != null)
                  remoteReceiver.RemoteCallback += new RemoteHandler(RemoteHandlerCallback);

                IKeyboardReceiver keyboardReceiver = plugin as IKeyboardReceiver;
                if (keyboardReceiver != null)
                  keyboardReceiver.KeyboardCallback += new KeyboardHandler(KeyboardHandlerCallback);

                IMouseReceiver mouseReceiver = plugin as IMouseReceiver;
                if (mouseReceiver != null)
                  mouseReceiver.MouseCallback += new MouseHandler(MouseHandlerCallback);

                if (plugin.Name.Equals(_pluginTransmit.Name))
                {
                  startedTransmit = true;
                  IrssLog.Info("Transmit and Receive plugin started: \"{0}\"", plugin.Name);
                }
                else
                {
                  IrssLog.Info("Receiver plugin started: \"{0}\"", plugin.Name);
                }
              }
              else
              {
                IrssLog.Error("Failed to start receive plugin: \"{0}\"", plugin.Name);
              }
            }
            catch (Exception ex)
            {
              IrssLog.Error("Failed to start receive plugin: \"{0}\"", plugin.Name);
              IrssLog.Error(ex.ToString());
            }
          }
        }
        
        if (_pluginTransmit != null && !startedTransmit)
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


        IrssLog.Info("Input Service started");
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
      }
    }

    protected override void OnStop()
    {
      IrssLog.Info("Stopping Input Service ...");

      if (_mode == InputServiceMode.ServerMode)
      {
        IrssMessage message = new IrssMessage(MessageType.ServerShutdown, MessageFlags.Notify);
        SendToAll(message);
      }

      // Stop Plugin(s) ...
      bool stoppedTransmit = false;

      if (_pluginReceive != null)
      {
        foreach (IRServerPluginBase plugin in _pluginReceive)
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
            IrssLog.Error(ex.ToString());
          }
        }

        _pluginReceive = null;
      }

      try
      {
        if (_pluginTransmit != null && !stoppedTransmit)
          _pluginTransmit.Stop();

        _pluginTransmit = null;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
      }

      // Stop Service
      try
      {
        switch (_mode)
        {
          case InputServiceMode.ServerMode:
            StopServer();
            break;

          case InputServiceMode.RelayMode:
            StopRelay();
            break;

          case InputServiceMode.RepeaterMode:
            StopRepeater();
            break;
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
      }
      
      IrssLog.Close();
    }

    protected override void OnShutdown()
    {
      OnStop();
    }

    protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
    {
      switch (powerStatus)
      {

        #region Suspend
        case PowerBroadcastStatus.Suspend:
          IrssLog.Info("Entering standby ...");

          bool suspendedTransmit = false;

          if (_pluginReceive != null)
          {
            foreach (IRServerPluginBase plugin in _pluginReceive)
            {
              try
              {
                plugin.Suspend();

                if (plugin == _pluginTransmit)
                  suspendedTransmit = true;
              }
              catch (Exception ex)
              {
                IrssLog.Error(ex.ToString());
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
              IrssLog.Error(ex.ToString());
            }
          }

          // Inform clients ...
          if (_mode == InputServiceMode.ServerMode)
          {
            IrssMessage message = new IrssMessage(MessageType.ServerSuspend, MessageFlags.Notify);
            SendToAll(message);
          }
          break;
        #endregion Suspend

        #region Resume
        case PowerBroadcastStatus.ResumeAutomatic:
        case PowerBroadcastStatus.ResumeCritical:
        case PowerBroadcastStatus.ResumeSuspend:
          IrssLog.Info("Resume from standby ...");

          bool resumedTransmit = false;

          if (_pluginReceive != null)
          {
            foreach (IRServerPluginBase plugin in _pluginReceive)
            {
              try
              {
                plugin.Resume();

                if (plugin == _pluginTransmit)
                  resumedTransmit = true;
              }
              catch (Exception ex)
              {
                IrssLog.Error(ex.ToString());
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
              IrssLog.Error(ex.ToString());
            }
          }

          // Inform clients ...
          if (_mode == InputServiceMode.ServerMode)
          {
            IrssMessage message = new IrssMessage(MessageType.ServerResume, MessageFlags.Notify);
            SendToAll(message);
          }
          break;
        #endregion Resume

      }

      return true;
    }

    #region Unused

    protected override void OnPause()
    {
      base.OnPause();
    }

    protected override void OnContinue()
    {
      base.OnContinue();
    }

    protected override void OnSessionChange(SessionChangeDescription changeDescription)
    {
      switch (changeDescription.Reason)
      {
        case SessionChangeReason.SessionLogon:
          break;

        case SessionChangeReason.SessionLogoff:
          break;
      }

      base.OnSessionChange(changeDescription);
    }

    protected override void OnCustomCommand(int command)
    {
      switch (command)
      {
        case 128:
          break;
      }

      base.OnCustomCommand(command);
    }

    #endregion Unused

    #endregion Service Methods

    #region Implementation

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

      IrssLog.Error("Please report this error");
    }
    void Connected(object obj)
    {
      IrssLog.Info("Connected to another server");

      if (_mode == InputServiceMode.RepeaterMode)
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
      if (_client != null)
      {
        _client.Dispose();
        _client = null;
      }
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
        IrssLog.Error(ex.ToString());
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
        IrssLog.Error(ex.ToString());
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

    void RemoteHandlerCallback(string keyCode)
    {
      IrssLog.Debug("Remote Event: {0}", keyCode);

      byte[] bytes = Encoding.ASCII.GetBytes(keyCode);

      switch (_mode)
      {
        case InputServiceMode.ServerMode:
          {
            IrssMessage message = new IrssMessage(MessageType.RemoteEvent, MessageFlags.Notify, bytes);
            SendToAll(message);
            break;
          }

        case InputServiceMode.RelayMode:
          {
            IrssMessage message = new IrssMessage(MessageType.ForwardRemoteEvent, MessageFlags.Request, bytes);
            _client.Send(message);
            break;
          }

        case InputServiceMode.RepeaterMode:
          {
            IrssLog.Debug("Remote event ignored, Input Service is in Repeater Mode.");
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
        case InputServiceMode.ServerMode:
          {
            IrssMessage message = new IrssMessage(MessageType.KeyboardEvent, MessageFlags.Notify, bytes);
            SendToAll(message);
            break;
          }

        case InputServiceMode.RelayMode:
          {
            IrssMessage message = new IrssMessage(MessageType.ForwardKeyboardEvent, MessageFlags.Request, bytes);
            _client.Send(message);
            break;
          }

        case InputServiceMode.RepeaterMode:
          {
            IrssLog.Debug("Keyboard event ignored, Input Service is in Repeater Mode.");
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
        case InputServiceMode.ServerMode:
          {
            IrssMessage message = new IrssMessage(MessageType.MouseEvent, MessageFlags.Notify, bytes);
            SendToAll(message);
            break;
          }

        case InputServiceMode.RelayMode:
          {
            IrssMessage message = new IrssMessage(MessageType.ForwardMouseEvent, MessageFlags.Request, bytes);
            _client.Send(message);
            break;
          }

        case InputServiceMode.RepeaterMode:
          {
            IrssLog.Debug("Mouse event ignored, Input Service is in Repeater Mode.");
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

    void ServerReceivedMessage(MessageManagerCombo combo)
    {
      IrssLog.Debug("Server message received: {0}, {1}", combo.Message.Type, combo.Message.Flags);

      try
      {
        switch (combo.Message.Type)
        {

          #region ForwardRemoteEvent
          case MessageType.ForwardRemoteEvent:
            if (_mode == InputServiceMode.RelayMode)
            {
              IrssMessage forward = new IrssMessage(MessageType.ForwardRemoteEvent, MessageFlags.Request, combo.Message.GetDataAsBytes());
              _client.Send(forward);
            }
            else
            {
              IrssMessage forward = new IrssMessage(MessageType.RemoteEvent, MessageFlags.Notify, combo.Message.GetDataAsBytes());
              SendToAllExcept(combo.Manager, forward);
            }
            break;
          #endregion ForwardRemoteEvent

          #region ForwardKeyboardEvent
          case MessageType.ForwardKeyboardEvent:
            if (_mode == InputServiceMode.RelayMode)
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
            if (_mode == InputServiceMode.RelayMode)
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

            if (_mode == InputServiceMode.RelayMode)
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

            if (_mode == InputServiceMode.RelayMode)
            {
              response.Flags |= MessageFlags.Failure;
            }
            else
            {
              byte[] bytes = null;

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

              IRServerPluginBase[] plugins = Program.AvailablePlugins();
              StringBuilder blasters = new StringBuilder();

              for (int index = 0; index < plugins.Length; index++)
              {
                IRServerPluginBase plugin = plugins[index];

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

              IRServerPluginBase[] plugins = Program.AvailablePlugins();
              StringBuilder receivers = new StringBuilder();

              for (int index = 0; index < plugins.Length; index++)
              {
                IRServerPluginBase plugin = plugins[index];

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
        IrssLog.Error(ex.ToString());
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
        IrssLog.Error(ex.ToString());
        IrssMessage response = new IrssMessage(MessageType.Error, MessageFlags.Notify, ex.Message);
        _client.Send(response);
      }
    }

    void LoadSettings()
    {
      _mode               = InputServiceMode.ServerMode;
      _hostComputer       = String.Empty;
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
        IrssLog.Error(ex.ToString());
        return;
      }

      try { _mode               = (InputServiceMode)Enum.Parse(typeof(InputServiceMode), doc.DocumentElement.Attributes["Mode"].Value, true); }
      catch (Exception ex) { IrssLog.Warn(ex.ToString()); }

      try { _hostComputer       = doc.DocumentElement.Attributes["HostComputer"].Value; }
      catch (Exception ex) { IrssLog.Warn(ex.ToString()); }

      try { _pluginNameTransmit = doc.DocumentElement.Attributes["PluginTransmit"].Value; }
      catch (Exception ex) { IrssLog.Warn(ex.ToString()); }

      try
      {
        string receivers        = doc.DocumentElement.Attributes["PluginReceive"].Value;
        if (!String.IsNullOrEmpty(receivers))
          _pluginNameReceive = receivers.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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
        using (XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, System.Text.Encoding.UTF8))
        {
          writer.Formatting = Formatting.Indented;
          writer.Indentation = 1;
          writer.IndentChar = (char)9;
          writer.WriteStartDocument(true);
          writer.WriteStartElement("settings"); // <settings>

          writer.WriteAttributeString("Mode", Enum.GetName(typeof(InputServiceMode), _mode));
          writer.WriteAttributeString("HostComputer", _hostComputer);
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
        IrssLog.Error(ex.ToString());
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
        IrssLog.Error(ex.ToString());
      }
    }

    #endregion Implementation

  }

}