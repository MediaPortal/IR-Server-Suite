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
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using IRServer.Plugin;
using IrssComms;
using IrssUtils;

namespace IRServer
{
  /// <summary>
  /// IR Server.
  /// </summary>
  internal class IRServer : ServiceBase
  {
    #region Constants

    private static readonly string AbstractRemoteMapFolder = Path.Combine(Common.FolderAppData,
                                                                          "IR Server\\Abstract Remote Maps");

    private static readonly string AbstractRemoteSchemaFile = Path.Combine(Common.FolderAppData,
                                                                           "IR Server\\Abstract Remote Maps\\RemoteTable.xsd");

    private const int TimeToWaitForRestart = 10000;

    #endregion Constants

    #region Variables

    private DataSet _abstractRemoteButtons;
    private Client _client;

    private List<PluginBase> _receivePlugins;
    private PluginBase _transmitPlugin;
    private bool _registered; // Used for relay and repeater modes.

    private List<ClientManager> _registeredClients;
    private List<ClientManager> _registeredRepeaters;
    private Server _server;

    private HardwareMonitor _hardwareMonitor;
    private DateTime _lastDeviceEvent = DateTime.MinValue;

    #endregion Variables

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="IRServer"/> class.
    /// </summary>
    public IRServer()
    {
      ServiceName = Shared.ServerName;

      //this.EventLog.Log = "Application";
      //this.AutoLog = true;

      CanHandlePowerEvent = true;
      CanHandleSessionChangeEvent = false;
      CanPauseAndContinue = false;
      CanShutdown = true;
      CanStop = true;
    }

    #endregion Constructor

    #region IDisposable

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
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

    /// <summary>
    /// Called when the service is started.
    /// </summary>
    /// <param name="args">The arguments.</param>
    protected override void OnStart(string[] args)
    {
      IrssLog.Info("Starting IR Server ...");

      Settings.LoadSettings();

      #region Process Priority Adjustment

      if (!Settings.ProcessPriority.Equals("No Change", StringComparison.OrdinalIgnoreCase))
      {
        try
        {
          ProcessPriorityClass priority =
            (ProcessPriorityClass)Enum.Parse(typeof(ProcessPriorityClass), Settings.ProcessPriority, true);
          Process.GetCurrentProcess().PriorityClass = priority;

          IrssLog.Info("Process priority set to: {0}", Settings.ProcessPriority);
        }
        catch (Exception ex)
        {
          IrssLog.Error(ex);
        }
      }

      #endregion Process Priority Adjustment

      LoadPlugins();

      #region Mode select

      try
      {
        switch (Settings.Mode)
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
        IrssLog.Error("Failed to start IR Server Communications");
        IrssLog.Error(ex);
      }

      #endregion Mode select

      StartPlugins();

      #region Setup Abstract Remote Model processing

      if (Settings.AbstractRemoteMode)
      {
        IrssLog.Info("IR Server is running in Abstract Remote mode");

        _abstractRemoteButtons = new DataSet("AbstractRemoteButtons");
        _abstractRemoteButtons.CaseSensitive = true;

        if (_receivePlugins != null)
          foreach (PluginBase plugin in _receivePlugins)
            if (plugin is IRemoteReceiver)
              LoadAbstractDeviceFiles(plugin.Name);
      }

      #endregion Setup Abstract Remote Model processing

      #region Setup Hardware Monitoring

      if (Settings.RestartOnUSBChanges)
      {
        _hardwareMonitor = new HardwareMonitor();
        _hardwareMonitor.DeviceConnected += new HardwareMonitor.HardwareMonitorEvent(OnDeviceConnected);
        _hardwareMonitor.Start();
      }

      #endregion

      IrssLog.Info("IR Server started");
    }

    /// <summary>
    /// Called when the service is stopped.
    /// </summary>
    protected override void OnStop()
    {
      IrssLog.Info("Stopping IR Server ...");

      if (_hardwareMonitor != null)
      {
        _hardwareMonitor.Stop();
        _hardwareMonitor = null;
      }

      if (Settings.Mode == IRServerMode.ServerMode)
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

      StopPlugins();

      // Stop Service
      try
      {
        switch (Settings.Mode)
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

    /// <summary>
    /// Called when the service is shut down.
    /// </summary>
    protected override void OnShutdown()
    {
      OnStop();
    }

    /// <summary>
    /// Called when there is a power event.
    /// </summary>
    /// <param name="powerStatus">The power status.</param>
    /// <returns>true if the event is handled, otherwise false.</returns>
    protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
    {
      IrssLog.Info("PowerEvent: {0}", Enum.GetName(typeof(PowerBroadcastStatus), powerStatus));

      switch (powerStatus)
      {
        #region Suspend

        case PowerBroadcastStatus.Suspend:
          // TODO: if anyone has any trouble with on-suspend commands then try changing this to QuerySuspend.
          IrssLog.Info("Entering standby ...");

          bool suspendedTransmit = false;

          if (_receivePlugins != null)
          {
            foreach (PluginBase plugin in _receivePlugins)
            {
              try
              {
                plugin.Suspend();

                if (plugin == _transmitPlugin)
                  suspendedTransmit = true;
              }
              catch (Exception ex)
              {
                IrssLog.Error(ex);
              }
            }
          }

          if (_transmitPlugin != null && !suspendedTransmit)
          {
            try
            {
              _transmitPlugin.Suspend();
            }
            catch (Exception ex)
            {
              IrssLog.Error(ex);
            }
          }

          // Inform clients ...
          if (Settings.Mode == IRServerMode.ServerMode)
          {
            IrssMessage message = new IrssMessage(MessageType.ServerSuspend, MessageFlags.Notify);
            SendToAll(message);
          }
          break;

        #endregion Suspend

        #region Resume

        case PowerBroadcastStatus.ResumeAutomatic:
          //case PowerBroadcastStatus.ResumeCritical:
          //case PowerBroadcastStatus.ResumeSuspend:
          IrssLog.Info("Resume from standby ...");

          bool resumedTransmit = false;

          if (_receivePlugins != null)
          {
            foreach (PluginBase plugin in _receivePlugins)
            {
              try
              {
                if (plugin == _transmitPlugin)
                  resumedTransmit = true;

                plugin.Resume();
              }
              catch (Exception ex)
              {
                IrssLog.Error(ex);
              }
            }
          }

          if (_transmitPlugin != null && !resumedTransmit)
          {
            try
            {
              _transmitPlugin.Resume();
            }
            catch (Exception ex)
            {
              IrssLog.Error(ex);
            }
          }

          // Inform clients ...
          if (Settings.Mode == IRServerMode.ServerMode)
          {
            IrssMessage message = new IrssMessage(MessageType.ServerResume, MessageFlags.Notify);
            SendToAll(message);
          }
          break;

        #endregion Resume
      }

      return true;
    }

    #endregion Service Methods

    #region Implementation

    #region Plugin handling

    private void LoadPlugins()
    {
      _receivePlugins = null;
      _transmitPlugin = null;

      if (Settings.PluginNameReceive == null && String.IsNullOrEmpty(Settings.PluginNameTransmit))
      {
        IrssLog.Warn("No transmit or receive plugins loaded");
        return;
      }

      #region Load receive plugins

      if (Settings.PluginNameReceive == null)
      {
        IrssLog.Warn("No receiver plugins loaded");
      }
      else
      {
        _receivePlugins = new List<PluginBase>(Settings.PluginNameReceive.Length);

        for (int index = 0; index < Settings.PluginNameReceive.Length; index++)
        {
          try
          {
            string pluginName = Settings.PluginNameReceive[index];

            PluginBase plugin = Program.GetPlugin(pluginName);

            if (plugin == null)
            {
              IrssLog.Warn("Receiver plugin not found: {0}", pluginName);
            }
            else
            {
              _receivePlugins.Add(plugin);

              if (!String.IsNullOrEmpty(Settings.PluginNameTransmit) &&
                  plugin.Name.Equals(Settings.PluginNameTransmit, StringComparison.OrdinalIgnoreCase))
                _transmitPlugin = plugin;
            }
          }
          catch (Exception ex)
          {
            IrssLog.Error(ex);
          }
        }

        if (_receivePlugins.Count == 0)
          _receivePlugins = null;
      }

      #endregion

      #region Load transmit plugin

      if (String.IsNullOrEmpty(Settings.PluginNameTransmit))
      {
        IrssLog.Warn("No transmit plugin loaded");
      }
      else if (_transmitPlugin == null)
      {
        try
        {
          _transmitPlugin = Program.GetPlugin(Settings.PluginNameTransmit);
        }
        catch (Exception ex)
        {
          IrssLog.Error(ex);
        }
      }

      #endregion
    }

    private void StartPlugins()
    {
      bool transmitPluginAlreadyStarted = false;

      #region Start receive plugins

      if (_receivePlugins != null)
      {
        List<PluginBase> removePlugins = new List<PluginBase>();

        foreach (PluginBase plugin in _receivePlugins)
        {
          try
          {
            plugin.Start();

            IRemoteReceiver remoteReceiver = plugin as IRemoteReceiver;
            if (remoteReceiver != null)
              remoteReceiver.RemoteCallback += RemoteHandlerCallback;

            IKeyboardReceiver keyboardReceiver = plugin as IKeyboardReceiver;
            if (keyboardReceiver != null)
              keyboardReceiver.KeyboardCallback += KeyboardHandlerCallback;

            IMouseReceiver mouseReceiver = plugin as IMouseReceiver;
            if (mouseReceiver != null)
              mouseReceiver.MouseCallback += MouseHandlerCallback;

            if (plugin.Name.Equals(Settings.PluginNameTransmit, StringComparison.OrdinalIgnoreCase))
            {
              transmitPluginAlreadyStarted = true;
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
            _receivePlugins.Remove(plugin);

        if (_receivePlugins.Count == 0)
          _receivePlugins = null;
      }

      #endregion

      #region Start transmit plugin

      if (_transmitPlugin != null && !transmitPluginAlreadyStarted)
      {
        try
        {
          _transmitPlugin.Start();

          IrssLog.Info("Transmit plugin started: \"{0}\"", Settings.PluginNameTransmit);
        }
        catch (Exception ex)
        {
          IrssLog.Error("Failed to start transmit plugin: \"{0}\"", Settings.PluginNameTransmit);
          IrssLog.Error(ex);

          _transmitPlugin = null;
        }
      }

      #endregion
    }

    private void StopPlugins()
    {
      bool transmitPluginAlreadyStopped = false;

      #region Stop receive plugins

      if (_receivePlugins != null && _receivePlugins.Count > 0)
      {
        foreach (PluginBase plugin in _receivePlugins)
        {
          try
          {
            IRemoteReceiver remoteReceiver = plugin as IRemoteReceiver;
            if (remoteReceiver != null)
              remoteReceiver.RemoteCallback -= RemoteHandlerCallback;

            IKeyboardReceiver keyboardReceiver = plugin as IKeyboardReceiver;
            if (keyboardReceiver != null)
              keyboardReceiver.KeyboardCallback -= KeyboardHandlerCallback;

            IMouseReceiver mouseReceiver = plugin as IMouseReceiver;
            if (mouseReceiver != null)
              mouseReceiver.MouseCallback -= MouseHandlerCallback;

            plugin.Stop();

            if (plugin == _transmitPlugin)
            {
              transmitPluginAlreadyStopped = true;
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

      _receivePlugins = null;

      #endregion

      #region Stop transmit plugin

      try
      {
        if (_transmitPlugin != null && !transmitPluginAlreadyStopped)
          _transmitPlugin.Stop();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
      finally
      {
        _transmitPlugin = null;
      }

      #endregion
    }

    #endregion

    private void StartServer()
    {
      if (_server != null)
        return;

      // Initialize registered client lists ...
      _registeredClients = new List<ClientManager>();
      _registeredRepeaters = new List<ClientManager>();

      _server = new Server(Server.DefaultPort, ServerReceivedMessage);
      _server.ClientDisconnectCallback = ClientDisconnect;

      _server.Start();
    }

    private void StopServer()
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

    private void CommsFailure(object obj)
    {
      Exception ex = obj as Exception;

      if (ex != null)
        IrssLog.Error("Communications failure: {0}", ex.Message);
      else
        IrssLog.Error("Communications failure");

      StopClient();

      IrssLog.Error("Please report this error");
    }

    private void Connected(object obj)
    {
      IrssLog.Info("Connected to another server");

      if (Settings.Mode == IRServerMode.RepeaterMode)
      {
        IrssMessage message = new IrssMessage(MessageType.RegisterRepeater, MessageFlags.Request);
        _client.Send(message);
      }
    }

    private void Disconnected(object obj)
    {
      IrssLog.Warn("Communications with other server has been lost");

      Thread.Sleep(1000);
    }

    private void ClientDisconnect(object obj)
    {
      ClientManager clientManager = obj as ClientManager;

      if (clientManager != null)
      {
        UnregisterClient(clientManager);
        UnregisterRepeater(clientManager);
      }
    }

    private bool StartClient(IPEndPoint endPoint)
    {
      if (_client != null)
        return false;

      _client = new Client(endPoint, ClientReceivedMessage);
      _client.CommsFailureCallback = CommsFailure;
      _client.ConnectCallback = Connected;
      _client.DisconnectCallback = Disconnected;

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

    private void StopClient()
    {
      if (_client == null)
        return;

      _client.Dispose();
      _client = null;

      _registered = false;
    }

    private bool StartRelay()
    {
      try
      {
        StartServer();

        IPAddress serverIP = Network.GetIPFromName(Settings.HostComputer);
        IPEndPoint endPoint = new IPEndPoint(serverIP, Server.DefaultPort);

        StartClient(endPoint);

        return true;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        return false;
      }
    }

    private void StopRelay()
    {
      try
      {
        StopServer();
        StopClient();
      }
      catch (Exception e)
      {
        IrssLog.Error(e);
      }
    }

    private bool StartRepeater()
    {
      try
      {
        StartServer();

        IPAddress serverIP = Network.GetIPFromName(Settings.HostComputer);
        IPEndPoint endPoint = new IPEndPoint(serverIP, Server.DefaultPort);

        StartClient(endPoint);

        return true;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        return false;
      }
    }

    private void StopRepeater()
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

    private void RemoteHandlerCallback(string deviceName, string keyCode)
    {
      IrssLog.Debug("{0} generated a remote event: {1}", deviceName, keyCode);

      string messageDeviceName = deviceName;
      string messageKeyCode = keyCode;

      switch (Settings.Mode)
      {
        case IRServerMode.ServerMode:
          if (Settings.AbstractRemoteMode)
          {
            string abstractButton = LookupAbstractButton(deviceName, keyCode);
            if (!String.IsNullOrEmpty(abstractButton))
            {
              messageDeviceName = "Abstract";
              messageKeyCode = abstractButton;

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

      byte[] bytes = IrssMessage.EncodeRemoteEventData(messageDeviceName, messageKeyCode);

      switch (Settings.Mode)
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

    private void KeyboardHandlerCallback(string deviceName, int vKey, bool keyUp)
    {
      IrssLog.Debug("{0} generated a keyboard event: {1}, keyUp: {2}", deviceName, vKey, keyUp);

      byte[] bytes = IrssMessage.EncodeKeyboardEventData(vKey, keyUp);

      switch (Settings.Mode)
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

    private void MouseHandlerCallback(string deviceName, int deltaX, int deltaY, int buttons)
    {
      IrssLog.Debug("{0} generated a mouse Event - deltaX: {1}, deltaY: {2}, buttons: {3}", deviceName, deltaX, deltaY,
                    buttons);

      byte[] bytes = IrssMessage.EncodeMouseEventData(deltaX, deltaY, buttons);

      switch (Settings.Mode)
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

    private void SendToAll(IrssMessage message)
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

    private void SendToAllExcept(ClientManager exceptClient, IrssMessage message)
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

    private void SendTo(ClientManager receiver, IrssMessage message)
    {
      IrssLog.Debug("SendTo({0}, {1})", message.Type, message.Flags);

      if (!_server.Send(receiver, message))
      {
        IrssLog.Warn("Failed to send message to a client, unregistering client");

        // If a message doesn't get through then unregister that client
        UnregisterClient(receiver);
      }
    }

    private void SendToRepeaters(IrssMessage message)
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

    private bool RegisterClient(ClientManager addClient)
    {
      if (_registeredClients == null) return false;

      lock (_registeredClients)
      {
        if (!_registeredClients.Contains(addClient))
          _registeredClients.Add(addClient);
      }

      IrssLog.Info("Registered a client");
      return true;
    }

    private bool UnregisterClient(ClientManager removeClient)
    {
      if (_registeredClients == null) return false;

      lock (_registeredClients)
      {
        if (!_registeredClients.Contains(removeClient))
          return false;

        _registeredClients.Remove(removeClient);
      }

      IrssLog.Info("Unregistered a client");
      return true;
    }

    private bool RegisterRepeater(ClientManager addRepeater)
    {
      if (_registeredRepeaters == null) return false;

      lock (_registeredRepeaters)
      {
        if (!_registeredRepeaters.Contains(addRepeater))
          _registeredRepeaters.Add(addRepeater);
      }

      IrssLog.Info("Registered a repeater");
      return true;
    }

    private bool UnregisterRepeater(ClientManager removeRepeater)
    {
      if (_registeredRepeaters == null) return false;

      lock (_registeredRepeaters)
      {
        if (!_registeredRepeaters.Contains(removeRepeater))
          return false;

        _registeredRepeaters.Remove(removeRepeater);
      }

      IrssLog.Info("Unregistered a repeater");
      return true;
    }

    private bool BlastIR(byte[] data)
    {
      try
      {
        IrssLog.Info("Blast IR");

        if (_transmitPlugin == null)
        {
          IrssLog.Warn("No transmit plugin loaded, can't blast");
          return false;
        }

        ITransmitIR _blaster = _transmitPlugin as ITransmitIR;
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

    private LearnStatus LearnIR(out byte[] data)
    {
      IrssLog.Info("Learn IR");

      data = null;

      if (_transmitPlugin == null)
      {
        IrssLog.Warn("No transmit plugin loaded, can't learn");
        return LearnStatus.Failure;
      }

      ILearnIR _learner = _transmitPlugin as ILearnIR;
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

    private void ServerReceivedMessage(MessageManagerCombo combo)
    {
      IrssLog.Debug("Server message received: {0}, {1}", combo.Message.Type, combo.Message.Flags);

      try
      {
        switch (combo.Message.Type)
        {
          #region ForwardRemoteEvent

          case MessageType.ForwardRemoteEvent:
            if (Settings.Mode == IRServerMode.RelayMode)
            {
              IrssMessage forward = new IrssMessage(MessageType.ForwardRemoteEvent, MessageFlags.Request,
                                                    combo.Message.GetDataAsBytes());
              _client.Send(forward);
            }
            else
            {
              byte[] data = combo.Message.GetDataAsBytes();

              if (Settings.AbstractRemoteMode)
              {
                // Decode message ...
                string deviceName = combo.Message.MessageData[IrssMessage.DEVICE_NAME] as string;
                string keyCode = combo.Message.MessageData[IrssMessage.KEY_CODE] as string;

                // Check that the device maps are loaded for the forwarded device
                bool foundDevice = false;
                if (_receivePlugins != null)
                {
                  foreach (PluginBase plugin in _receivePlugins)
                  {
                    if (plugin is IRemoteReceiver && plugin.Name.Equals(deviceName, StringComparison.OrdinalIgnoreCase))
                    {
                      foundDevice = true;
                      break;
                    }
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
                    data = IrssMessage.EncodeRemoteEventData("Abstract", abstractButton);
                  }
                  else
                  {
                    IrssLog.Info("Abstract Remote Button not found for forwarded remote event: {0} ({1})", deviceName,
                                 keyCode);
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
            if (Settings.Mode == IRServerMode.RelayMode)
            {
              IrssMessage forward = new IrssMessage(MessageType.ForwardKeyboardEvent, MessageFlags.Request,
                                                    combo.Message.GetDataAsBytes());
              _client.Send(forward);
            }
            else
            {
              IrssMessage forward = new IrssMessage(MessageType.KeyboardEvent, MessageFlags.Notify,
                                                    combo.Message.GetDataAsBytes());
              SendToAllExcept(combo.Manager, forward);
            }
            break;

          #endregion ForwardKeyboardEvent

          #region ForwardMouseEvent

          case MessageType.ForwardMouseEvent:
            if (Settings.Mode == IRServerMode.RelayMode)
            {
              IrssMessage forward = new IrssMessage(MessageType.ForwardMouseEvent, MessageFlags.Request,
                                                    combo.Message.GetDataAsBytes());
              _client.Send(forward);
            }
            else
            {
              IrssMessage forward = new IrssMessage(MessageType.MouseEvent, MessageFlags.Notify,
                                                    combo.Message.GetDataAsBytes());
              SendToAllExcept(combo.Manager, forward);
            }
            break;

          #endregion ForwardMouseEvent

          #region BlastIR

          case MessageType.BlastIR:
            {
              IrssMessage response = new IrssMessage(MessageType.BlastIR, MessageFlags.Response);

              if (Settings.Mode == IRServerMode.RelayMode)
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

              if (Settings.Mode == IRServerMode.RelayMode)
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

                if (_receivePlugins != null)
                  irServerInfo.CanReceive = true;

                if (_transmitPlugin != null)
                {
                  irServerInfo.CanLearn = (_transmitPlugin is ILearnIR);
                  irServerInfo.CanTransmit = true;
                  irServerInfo.Ports = ((ITransmitIR)_transmitPlugin).AvailablePorts;
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
              response.SetDataAsString(Settings.PluginNameTransmit);

              SendTo(combo.Manager, response);
              break;
            }

          #endregion ActiveBlasters

          #region ActiveReceivers

          case MessageType.ActiveReceivers:
            {
              IrssMessage response = new IrssMessage(MessageType.ActiveReceivers, MessageFlags.Response);

              if (Settings.PluginNameReceive != null)
              {
                StringBuilder receivers = new StringBuilder();
                for (int index = 0; index < Settings.PluginNameReceive.Length; index++)
                {
                  receivers.Append(Settings.PluginNameReceive[index]);

                  if (index < Settings.PluginNameReceive.Length - 1)
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

              PluginBase[] plugins = BasicFunctions.AvailablePlugins();
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

              PluginBase[] plugins = BasicFunctions.AvailablePlugins();
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
              string[] detectedBlasters = Shared.DetectBlasters();

              if (detectedBlasters != null && detectedBlasters.Length > 0)
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
              string[] detectedReceivers = Shared.DetectReceivers();

              if (detectedReceivers != null && detectedReceivers.Length > 0)
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

    private void ClientReceivedMessage(IrssMessage received)
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

    private string LookupAbstractButton(string deviceName, string keyCode)
    {
      if (_abstractRemoteButtons == null || _abstractRemoteButtons.Tables.Count == 0)
        return null;

      try
      {
        foreach (DataTable table in _abstractRemoteButtons.Tables)
        {
          string device = table.ExtendedProperties["Device"] as string;
          if (String.IsNullOrEmpty(device)) continue;

          if (device.Equals(deviceName, StringComparison.OrdinalIgnoreCase))
          {
            string expression = String.Format("RawCode = '{0}'", keyCode);

            DataRow[] rows = table.Select(expression);
            if (rows.Length == 1)
            {
              string button = rows[0]["AbstractButton"] as string;
              if (!String.IsNullOrEmpty(button))
              {
                IrssLog.Debug("{0}, remote: {1}, device: {2}", button, table.ExtendedProperties["Remote"] as string,
                              deviceName);
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

    private bool LoadAbstractDeviceFiles(string device)
    {
      if (String.IsNullOrEmpty(device))
        return false;

      IrssLog.Info("Load Abstract Device Files: {0}", device);

      string path = Path.Combine(AbstractRemoteMapFolder, device);
      if (!Directory.Exists(path))
      {
        IrssLog.Debug("No Abstract Remote Tables for \"{0}\" were found", device);
        return false;
      }

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

    #region Hardware Monitoring

    private void OnDeviceConnected()
    {
      Thread lazyRestartThread = new Thread(LazyRestart);
      lazyRestartThread.Start();
    }

    private void LazyRestart()
    {
      DateTime tempLastDeviceEvent = DateTime.Now;
      _lastDeviceEvent = tempLastDeviceEvent;

      // wait, if new decice events occur
      Thread.Sleep(TimeToWaitForRestart);

      // if new device event occured, stop here
      if (!tempLastDeviceEvent.Equals(_lastDeviceEvent)) return;

      // restart service
      IrssLog.Info("New device event. Restarting Input Service.");
      StopServer();
      StopPlugins();
      LoadPlugins();
      StartPlugins();
      StartServer();
    }

    #endregion

    #endregion Implementation

    #region Wrapper-Functions for Executing as Application

    public bool DoStart()
    {
      this.OnStart(new string[] { });
      return true;
    }

    public bool DoStop()
    {
      this.OnStop();
      return true;
    }

    public bool DoPowerEvent(PowerBroadcastStatus powerStatus)
    {
      this.OnPowerEvent(powerStatus);
      return true;
    }

    #endregion
  }
}
