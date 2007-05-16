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

    public static readonly string ConfigurationFile = Common.FolderAppData + "IR Server\\IR Server.xml";

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

    string _pluginName = String.Empty;
    IIRServerPlugin _plugin = null;

    bool _inConfiguration = false;

    #endregion Variables

    #region Constructor

    public IRServer()
    {
      // Setup taskbar icon
      _notifyIcon = new NotifyIcon();
      _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
      _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("&Setup", null, new EventHandler(ClickSetup)));
      _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("&Quit", null, new EventHandler(ClickQuit)));
      _notifyIcon.Icon = Properties.Resources.Icon16;
      _notifyIcon.Text = "IR Server";


    }

    #endregion Constructor

    #region Implementation

    /// <summary>
    /// Start the server
    /// </summary>
    /// <returns>returns true if successful</returns>
    internal bool Start()
    {
      try
      {
        IrssLog.Debug("Starting IR Server ...");

        LoadSettings();

        // Try to load the IR Plugin, if it fails (for whatever reason) then run configuration.
        _plugin = null;
        while (_plugin == null)
        {
          _plugin = Program.GetPlugin(_pluginName);
          IrssLog.Warn("Failed to load plugin \"{0}\"", _pluginName);

          if (_plugin == null)
          {
            if (Configure())
              SaveSettings();
            else
              return false;
          }
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
              {
                IrssLog.Error("Failed to start in Relay Mode");
                return false;
              }

              break;
            }

          case IRServerMode.RepeaterMode:
            {
              if (StartRepeater())
                IrssLog.Info("Started in Repeater Mode");
              else
              {
                IrssLog.Error("Failed to start in Repeater Mode");
                return false;
              }
              break;
            }
        }

        // Start transceiver ...
        if (!_plugin.Start())
        {
          IrssLog.Error("Failed to start transceiver plugin: \"{0}\"", _pluginName);
          
          if (PipeAccess.ServerRunning)
            PipeAccess.StopServer();
          return false;
        }

        IrssLog.Info("Transceiver plugin started: \"{0}\"", _pluginName);

        if (_plugin.CanReceive)
          _plugin.RemoteButtonCallback += new RemoteButtonHandler(RemoteButtonPressed);

        _notifyIcon.Visible = true;

        IrssLog.Info("IR Server started");
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
        return false;
      }

      return true;
    }

    /// <summary>
    /// Stop the server
    /// </summary>
    internal void Stop()
    {
      IrssLog.Info("Stopping IR Server ...");

      _notifyIcon.Visible = false;

      if (_plugin.CanReceive)
        _plugin.RemoteButtonCallback -= new RemoteButtonHandler(RemoteButtonPressed);
      
      // Stop Plugin
      try
      {
        _plugin.Stop();
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
        Config config = new Config();
        config.Mode = _mode;
        config.HostComputer = _hostComputer;
        config.Plugin = _pluginName;

        if (config.ShowDialog() == DialogResult.OK)
        {
          _mode = config.Mode;
          _hostComputer = config.HostComputer;
          _pluginName = config.Plugin;

          _inConfiguration = false;

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

    void RemoteButtonPressed(string keyCode)
    {
      IrssLog.Info("Remote Button Pressed: {0}", keyCode);
      
      byte[] bytes = Encoding.ASCII.GetBytes(keyCode);

      switch (_mode)
      {
        case IRServerMode.ServerMode:
          {
            PipeMessage message = new PipeMessage(Common.ServerPipeName, Environment.MachineName, "Remote Button", bytes);
            SendToAll(message);
            break;
          }

        case IRServerMode.RelayMode:
          {
            PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Forward Remote Button", bytes);
            SendTo(Common.ServerPipeName, _hostComputer, message);
            break;
          }

        case IRServerMode.RepeaterMode:
          {
            IrssLog.Info("Remote button press ignored, IR Server is in Repeater Mode.");
            break;
          }
      }
    }

    void SendToAll(PipeMessage message)
    {
      IrssLog.Debug("SendToAll({0})", message.ToString());

      IrssLog.Info("Message out: {0}", message.Name);

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

      IrssLog.Info("Message out: {0}", message.Name);

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

      IrssLog.Info("Message out: {0}", message.Name);

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

      IrssLog.Info("Message out: {0}", message.Name);

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

        if (!_plugin.CanTransmit)
          return false;

        int portLen = BitConverter.ToInt32(data, 0);
        if (portLen > 0)
          _plugin.SetPort(Encoding.ASCII.GetString(data, 4, portLen));

        int speedLen = BitConverter.ToInt32(data, 4 + portLen);
        if (speedLen > 0)
          _plugin.SetSpeed(Encoding.ASCII.GetString(data, 4 + portLen + 4, speedLen));

        byte[] fileData = new byte[data.Length - (4 + portLen + 4 + speedLen)];
        for (int index = (4 + portLen + 4 + speedLen); index < data.Length; index++)
          fileData[index - (4 + portLen + 4 + speedLen)] = data[index];

        string tempFile = Path.GetTempFileName();

        FileStream fileStream = new FileStream(tempFile, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
        fileStream.Write(fileData, 0, fileData.Length);
        fileStream.Flush();
        fileStream.Close();

        bool result = _plugin.Transmit(tempFile);

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

      if (!_plugin.CanLearn)
      {
        IrssLog.Debug("Active transceiver doesn't support learn");
        return null;
      }

      byte[] data = null;
      FileStream fileStream = null;

      try
      {
        string tempFile = Path.GetTempFileName();

        LearnStatus status = _plugin.Learn(tempFile);
        switch (status)
        {
          case LearnStatus.Success:
            fileStream = new FileStream(tempFile, FileMode.Open);

            if (fileStream != null && fileStream.Length != 0)
            {
              data = new byte[fileStream.Length];
              fileStream.Read(data, 0, (int)fileStream.Length);
            }

            fileStream.Close();
            fileStream = null;

            File.Delete(tempFile);
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

        if (fileStream != null)
          fileStream.Close();
      }

      return data;
    }

    void HandlePipeMessage(PipeMessage received)
    {
      IrssLog.Debug("Message received from client \\\\{0}\\pipe\\{1} = {2}", received.FromServer, received.FromPipe, received.ToString());

      IrssLog.Info("Message in: {0}", received.Name);

      try
      {
        switch (received.Name)
        {
          case "Remote Button":
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

          case "Forward Remote Button":
            {
              PipeMessage forward = new PipeMessage(Common.ServerPipeName, Environment.MachineName, "Remote Button", received.Data);
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

              // Pause 1 second before instructing the client to start the IR learning ...
              Thread.Sleep(1000);

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
              if (_mode == IRServerMode.ServerMode)
              {
                PipeMessage response = new PipeMessage(Common.ServerPipeName, Environment.MachineName, "Server Shutdown", null);
                SendToAll(response);
              }

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

                // Transceiver Info ...
                TransceiverInfo transceiverInfo = new TransceiverInfo();
                transceiverInfo.Name            = _plugin.Name;
                transceiverInfo.Ports           = _plugin.AvailablePorts;
                transceiverInfo.Speeds          = _plugin.AvailableSpeeds;
                transceiverInfo.CanConfigure    = _plugin.CanConfigure;
                transceiverInfo.CanLearn        = _plugin.CanLearn;
                transceiverInfo.CanReceive      = _plugin.CanReceive;
                transceiverInfo.CanTransmit     = _plugin.CanTransmit;

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
      IrssLog.Info("Setup");

      if (_inConfiguration)
        return;

      Stop();

      if (Configure())
        SaveSettings();

      Start();
    }
    void ClickQuit(object sender, EventArgs e)
    {
      IrssLog.Info("Quit");

      if (_inConfiguration)
        return;

      if (_mode == IRServerMode.ServerMode)
      {
        PipeMessage message = new PipeMessage(Common.ServerPipeName, Environment.MachineName, "Server Shutdown", null);
        SendToAll(message);
      }

      Stop();
      Application.Exit();
    }

    void LoadSettings()
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationFile);

        _mode         = (IRServerMode)Enum.Parse(typeof(IRServerMode), doc.DocumentElement.Attributes["Mode"].Value);
        _hostComputer = doc.DocumentElement.Attributes["HostComputer"].Value;
        _pluginName   = doc.DocumentElement.Attributes["Plugin"].Value;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());

        _mode         = IRServerMode.ServerMode;
        _hostComputer = String.Empty;
        _pluginName   = String.Empty;
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
        writer.WriteAttributeString("Plugin", _pluginName);

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
