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

    Thread _messageHandlerThread;

    Queue _messageQueue;
    bool _processMessageQueue;

    IRServerMode _mode;
    string _hostComputer;

    string _localPipeName = String.Empty;
    bool _registered = false;

    string _pluginNameReceive       = String.Empty;
    IIRServerPlugin _pluginReceive  = null;

    string _pluginNameTransmit      = String.Empty;
    IIRServerPlugin _pluginTransmit = null;

    bool _inConfiguration = false;

    #endregion Variables

    #region Constructor

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
          IrssLog.Warn("No transmit/receive plugin loaded");
        }
        else
        {
          if (!String.IsNullOrEmpty(_pluginNameReceive))
            _pluginReceive = Program.GetPlugin(_pluginNameReceive);
          else
            IrssLog.Warn("No receiver plugin loaded");

          if (_pluginNameTransmit.Equals(_pluginNameReceive, StringComparison.InvariantCultureIgnoreCase))
            _pluginTransmit = _pluginReceive;
          else if (!String.IsNullOrEmpty(_pluginNameTransmit))
            _pluginTransmit = Program.GetPlugin(_pluginNameTransmit);
          else
            IrssLog.Warn("No transmit plugin loaded");
        }

        StartMessageQueue();

        switch (_mode)
        {
          case IRServerMode.ServerMode:
            {
              // Initialize registered client lists ...
              _registeredClients = new List<Client>();
              _registeredRepeaters = new List<Client>();

              // Start server pipe
              PipeAccess.StartServer(Common.ServerPipeName, new PipeMessageHandler(QueueMessage));

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

        if (_pluginReceive != null && _pluginReceive.CanReceive)
        {
          _pluginReceive.RemoteCallback += new RemoteHandler(RemoteHandlerCallback);
          _pluginReceive.KeyboardCallback += new KeyboardHandler(KeyboardHandlerCallback);
          _pluginReceive.MouseCallback += new MouseHandler(MouseHandlerCallback);
        }

        _notifyIcon.Visible = true;

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

      _notifyIcon.Visible = false;

      if (_mode == IRServerMode.ServerMode)
      {
        PipeMessage message = new PipeMessage(Common.ServerPipeName, Environment.MachineName, "Server Shutdown", null);
        SendToAll(message);
      }

      if (_pluginReceive != null && _pluginReceive.CanReceive)
      {
        _pluginReceive.RemoteCallback -= new RemoteHandler(RemoteHandlerCallback);
        _pluginReceive.KeyboardCallback -= new KeyboardHandler(KeyboardHandlerCallback);
        _pluginReceive.MouseCallback -= new MouseHandler(MouseHandlerCallback);
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
        if (_pluginTransmit != null)
          _pluginTransmit.Stop();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
      }

      // Stop Message Queue
      try
      {
        StopMessageQueue();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
      }

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

    bool Configure()
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
          _mode               = config.Mode;
          _hostComputer       = config.HostComputer;
          _pluginNameReceive  = config.PluginReceive;
          _pluginNameTransmit = config.PluginTransmit;

          _inConfiguration  = false;

          return true;
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
      }

      _inConfiguration = false;

      return false;
    }

    void StartMessageQueue()
    {
      _processMessageQueue = true;
      
      // Create a FIFO message queue
      _messageQueue = Queue.Synchronized(new Queue());

      // Start message queue handling thread
      _messageHandlerThread = new Thread(new ThreadStart(MessageHandlerThread));
      _messageHandlerThread.IsBackground = true;
      _messageHandlerThread.Name = "IR Server Message Queue";
      _messageHandlerThread.Start();
    }
    void StopMessageQueue()
    {
      _processMessageQueue = false;

      try
      {
        if (_messageHandlerThread != null && _messageHandlerThread.IsAlive)
          _messageHandlerThread.Abort();
      }
      catch { }

      _messageQueue.Clear();
      _messageQueue = null;
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
            PipeAccess.StartServer(localPipeTest, new PipeMessageHandler(QueueMessage));
            _localPipeName = localPipeTest;
            retry = false;
          }
        }
        while (retry);

        PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Register", null);
        PipeAccess.SendMessage(Common.ServerPipeName, _hostComputer, message.ToString());
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

          PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Unregister", null);
          PipeAccess.SendMessage(Common.ServerPipeName, _hostComputer, message.ToString());
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
            PipeAccess.StartServer(localPipeTest, new PipeMessageHandler(QueueMessage));
            _localPipeName = localPipeTest;
            retry = false;
          }
        }
        while (retry);

        PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Register Repeater", null);
        PipeAccess.SendMessage(Common.ServerPipeName, _hostComputer, message.ToString());
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

          PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Unregister Repeater", null);
          PipeAccess.SendMessage(Common.ServerPipeName, _hostComputer, message.ToString());
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
            PipeMessage message = new PipeMessage(Common.ServerPipeName, Environment.MachineName, "Remote Event", bytes);
            SendToAll(message);
            break;
          }

        case IRServerMode.RelayMode:
          {
            PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Forward Remote Event", bytes);
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
            PipeMessage message = new PipeMessage(Common.ServerPipeName, Environment.MachineName, "Keyboard Event", bytes);
            SendToAll(message);
            break;
          }

        case IRServerMode.RelayMode:
          {
            PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Forward Keyboard Event", bytes);
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
            PipeMessage message = new PipeMessage(Common.ServerPipeName, Environment.MachineName, "Mouse Event", bytes);
            SendToAll(message);
            break;
          }

        case IRServerMode.RelayMode:
          {
            PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Forward Mouse Event", bytes);
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


    void SendToAll(PipeMessage message)
    {
      IrssLog.Debug("SendToAll({0})", message.ToString());

      List<Client> unregister = new List<Client>();

      lock (_registeredClients)
      {
        foreach (Client client in _registeredClients)
        {
          try
          {
            PipeAccess.SendMessage(client.Pipe, client.Server, message.ToString());
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
      IrssLog.Debug("SendToAllExcept({0}, {1}, {2})", exceptPipe, exceptServer, message.ToString());

      List<Client> unregister = new List<Client>();

      lock (_registeredClients)
      {
        foreach (Client client in _registeredClients)
        {
          try
          {
            if (client.Pipe == exceptPipe && client.Server == exceptServer)
              continue;

            PipeAccess.SendMessage(client.Pipe, client.Server, message.ToString());
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
      IrssLog.Debug("SendTo({0}, {1}, {2})", pipe, server, message.ToString());

      try
      {
        PipeAccess.SendMessage(pipe, server, message.ToString());
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
      IrssLog.Debug("SendToRepeaters({0})", message.ToString());

      List<Client> unregister = new List<Client>();

      lock (_registeredRepeaters)
      {
        foreach (Client client in _registeredRepeaters)
        {
          try
          {
            PipeAccess.SendMessage(client.Pipe, client.Server, message.ToString());
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

      Client removeClient = null;

      lock (_registeredClients)
      {
        foreach (Client client in _registeredClients)
        {
          if (client.Pipe == pipe && client.Server == server)
          {
            removeClient = client;
            break;
          }
        }

        if (removeClient != null)
          _registeredClients.Remove(removeClient);
        else
          return false;
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

      if (_mode != IRServerMode.ServerMode)
        return false;

      Client removeClient = null;

      lock (_registeredRepeaters)
      {
        foreach (Client client in _registeredRepeaters)
        {
          if (client.Pipe == pipe && client.Server == server)
          {
            removeClient = client;
            break;
          }
        }

        if (removeClient != null)
          _registeredRepeaters.Remove(removeClient);
        else
          return false;
      }

      IrssLog.Info("Unregistered Repeater: \\\\{0}\\pipe\\{1}", server, pipe);
      return true;
    }

    bool BlastIR(byte[] data)
    {
      try
      {
        IrssLog.Debug("Blast IR");

        if (_pluginTransmit == null || !_pluginTransmit.CanTransmit)
          return false;

        int portLen = BitConverter.ToInt32(data, 0);
        if (portLen > 0)
          _pluginTransmit.SetPort(Encoding.ASCII.GetString(data, 4, portLen));

        byte[] fileData = new byte[data.Length - (4 + portLen)];
        for (int index = 4 + portLen; index < data.Length; index++)
          fileData[index - (4 + portLen)] = data[index];

        string tempFile = Path.GetTempFileName();

        FileStream fileStream = new FileStream(tempFile, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
        fileStream.Write(fileData, 0, fileData.Length);
        fileStream.Flush();
        fileStream.Close();

        bool result = _pluginTransmit.Transmit(tempFile);

        File.Delete(tempFile);

        return result;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
      }

      return false;
    }
    byte[] LearnIR()
    {
      IrssLog.Debug("Learn IR");

      Thread.Sleep(500);

      if (_pluginTransmit == null || !_pluginTransmit.CanLearn)
      {
        IrssLog.Debug("Active transceiver doesn't support learn");
        return null;
      }

      byte[] data = null;

      try
      {
        LearnStatus status = _pluginTransmit.Learn(out data);
        switch (status)
        {
          case LearnStatus.Success:
            IrssLog.Info("Learn IR success");
            break;

          case LearnStatus.Failure:
            IrssLog.Error("Failed to learn IR Code");
            break;

          case LearnStatus.Timeout:
            IrssLog.Error("IR Code learn timed out");
            break;
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
      }

      return data;
    }

    void HandlePipeMessage(PipeMessage received)
    {
      IrssLog.Debug("Message received from client \\\\{0}\\pipe\\{1} = {2}", received.FromServer, received.FromPipe, received.ToString());

      try
      {
        switch (received.Name)
        {
          case "Remote Event":
          case "Keyboard Event":
          case "Mouse Event":
            break;

          case "Register Success":
            {
              IrssLog.Info("Registered with host server");
              _registered = true;
              break;
            }

          case "Register Failure":
            {
              IrssLog.Warn("Host server refused registration"); 
              _registered = false;
              break;
            }

          case "Forward Remote Event":
            {
              PipeMessage forward = new PipeMessage(Common.ServerPipeName, Environment.MachineName, "Remote Event", received.Data);
              if (_mode == IRServerMode.RelayMode)
              {
                forward.Name = received.Name;
                SendTo(Common.ServerPipeName, _hostComputer, forward);
              }
              else
              {
                SendToAllExcept(received.FromPipe, received.FromServer, forward);
              }
              break;
            }

          case "Forward Keyboard Event":
            {
              PipeMessage forward = new PipeMessage(Common.ServerPipeName, Environment.MachineName, "Keyboard Event", received.Data);
              if (_mode == IRServerMode.RelayMode)
              {
                forward.Name = received.Name;
                SendTo(Common.ServerPipeName, _hostComputer, forward);
              }
              else
              {
                SendToAllExcept(received.FromPipe, received.FromServer, forward);
              }
              break;
            }

          case "Forward Mouse Event":
            {
              PipeMessage forward = new PipeMessage(Common.ServerPipeName, Environment.MachineName, "Mouse Event", received.Data);
              if (_mode == IRServerMode.RelayMode)
              {
                forward.Name = received.Name;
                SendTo(Common.ServerPipeName, _hostComputer, forward);
              }
              else
              {
                SendToAllExcept(received.FromPipe, received.FromServer, forward);
              }
              break;
            }

          case "List":
            {
              if (_mode != IRServerMode.RelayMode)
              {
                PipeMessage response = new PipeMessage(Common.ServerPipeName, Environment.MachineName, "Clients", BitConverter.GetBytes(_registeredClients.Count));
                SendTo(received.FromPipe, received.FromServer, response);
              }
              break;
            }

          case "Blast":
            {
              if (_mode == IRServerMode.RelayMode)
              {
                PipeMessage reply = new PipeMessage(Common.ServerPipeName, Environment.MachineName, received.Name + " Failure", null);
                SendTo(received.FromPipe, received.FromServer, reply);
                break;
              }

              if (_registeredRepeaters.Count > 0)
                SendToRepeaters(received);

              PipeMessage response = new PipeMessage(Common.ServerPipeName, Environment.MachineName, received.Name + " Failure", null);

              if (BlastIR(received.Data))
                response.Name = received.Name + " Success";

              SendTo(received.FromPipe, received.FromServer, response);
              break;
            }

          case "Learn":
            {
              if (_mode == IRServerMode.RelayMode)
              {
                PipeMessage reply = new PipeMessage(Common.ServerPipeName, Environment.MachineName, received.Name + " Failure", null);
                SendTo(received.FromPipe, received.FromServer, reply);
                break;
              }

              // Pause half a second before instructing the client to start the IR learning ...
              Thread.Sleep(500);

              // Send back a "Start Learn" trigger ...
              PipeMessage trigger = new PipeMessage(Common.ServerPipeName, Environment.MachineName, "Start Learn", null);
              SendTo(received.FromPipe, received.FromServer, trigger);

              // Prepare response ...
              PipeMessage response = new PipeMessage(Common.ServerPipeName, Environment.MachineName, received.Name + " Failure", null);
              
              byte[] bytes = LearnIR();

              if (bytes != null)
              {
                response.Name = received.Name + " Success";
                response.Data = bytes;
              }

              SendTo(received.FromPipe, received.FromServer, response);
              break;
            }

          case "Shutdown":
            {
              IrssLog.Info("Shutdown command received");

              Stop();
              Application.Exit();
              break;
            }

          case "Ping":
            {
              PipeMessage response = new PipeMessage(Common.ServerPipeName, Environment.MachineName, "Echo", received.Data);
              SendTo(received.FromPipe, received.FromServer, response);
              break;
            }

          case "Register":
            {
              PipeMessage response = new PipeMessage(Common.ServerPipeName, Environment.MachineName, received.Name + " Failure", null);
              if (RegisterClient(received.FromPipe, received.FromServer))
              {
                response.Name = received.Name + " Success";

                TransceiverInfo transceiverInfo = new TransceiverInfo();

                if (_pluginReceive != null)
                {
                  transceiverInfo.Name          = _pluginReceive.Name;
                  transceiverInfo.CanReceive    = _pluginReceive.CanReceive;
                }

                if (_pluginTransmit != null)
                {
                  transceiverInfo.Name          = _pluginTransmit.Name;
                  transceiverInfo.Ports         = _pluginTransmit.AvailablePorts;
                  transceiverInfo.CanLearn      = _pluginTransmit.CanLearn;
                  transceiverInfo.CanTransmit   = _pluginTransmit.CanTransmit;
                }                

                response.Data = TransceiverInfo.ToBytes(transceiverInfo);
              }

              SendTo(received.FromPipe, received.FromServer, response);
              break;
            }

          case "Unregister":
            {
              UnregisterClient(received.FromPipe, received.FromServer);
              break;
            }

          case "Register Repeater":
            {
              PipeMessage response = new PipeMessage(Common.ServerPipeName, Environment.MachineName, received.Name + " Failure", null);
              if (RegisterRepeater(received.FromPipe, received.FromServer))
                response.Name = received.Name + " Success";

              SendTo(received.FromPipe, received.FromServer, response);
              break;
            }

          case "Unregister Repeater":
            {
              UnregisterRepeater(received.FromPipe, received.FromServer);
              break;
            }

          case "Server Shutdown":
            {
              IrssLog.Info("Host server shut down");
              _registered = false;
              break;
            }
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
        PipeMessage response = new PipeMessage(Common.ServerPipeName, Environment.MachineName, "Error", Encoding.ASCII.GetBytes(ex.Message));
        SendTo(received.FromPipe, received.FromServer, response);
      }

    }
    void QueueMessage(string message)
    {
      PipeMessage pipeMessage = PipeMessage.FromString(message);
      if (pipeMessage == null)
        return;

      lock (((ICollection)_messageQueue).SyncRoot)
        _messageQueue.Enqueue(pipeMessage);
    }
    void MessageHandlerThread()
    {
      try
      {
        while (_processMessageQueue)
        {
          Thread.Sleep(50);
          
          lock (((ICollection)_messageQueue).SyncRoot)
          {
            if (_messageQueue.Count > 0)
              HandlePipeMessage((PipeMessage)_messageQueue.Dequeue());
          }
        }
      }
      catch { }
    }

    void ClickSetup(object sender, EventArgs e)
    {
      if (_inConfiguration)
        return;

      Stop();

      IrssLog.Info("Setup");

      if (Configure())
        SaveSettings();

      Start();
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

        _mode               = (IRServerMode)Enum.Parse(typeof(IRServerMode), doc.DocumentElement.Attributes["Mode"].Value);
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
