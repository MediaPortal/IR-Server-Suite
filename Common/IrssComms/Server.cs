using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace IrssComms
{

  #region Delegates

  /// <summary>
  /// Message handling delegate for server.
  /// </summary>
  /// <param name="combo">Combination of Message and ClientManager objects.</param>
  public delegate void ServerMessageSink(MessageManagerCombo combo);

  #endregion Delegates

  /// <summary>
  /// TCP communications server class.
  /// </summary>
  public class Server : IDisposable
  {

    #region Constants

    /// <summary>
    /// The default port that Server sockets will be opened on.
    /// </summary>
    public const int DefaultPort = 24000;

    /// <summary>
    /// Backlog of Socket requests that can build up on the server socket.
    /// </summary>
    const int SocketBacklog = 10;

    #endregion Constants

    #region Variables

    ServerMessageSink _messageSink;
    IPEndPoint _localEndPoint;

    Socket _serverSocket;

    bool _processConnectionThread = false;
    Thread _connectionThread;

    List<ClientManager> _clientManagers;

    GenericPCQueue<MessageManagerCombo> _messageQueue;

    WaitCallback _clientDisconnectCallback;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Gets or Sets the Client Disconnect callback.
    /// </summary>
    public WaitCallback ClientDisconnectCallback
    {
      get { return _clientDisconnectCallback; }
      set { _clientDisconnectCallback = value; }
    }

    #endregion Properties

    #region Constructor

    /// <summary>
    /// Create a TCP communications server.
    /// </summary>
    /// <param name="port">Port to open for listening.</param>
    /// <param name="messageSink">Where to send received messages.</param>
    public Server(int port, ServerMessageSink messageSink)
    {
      _localEndPoint = new IPEndPoint(IPAddress.Any, port);

      _messageSink = messageSink;

      _messageQueue = new GenericPCQueue<MessageManagerCombo>(new GenericPCQueueSink<MessageManagerCombo>(QueueMessageSink));
    }

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
        Stop();
        
        _messageQueue.Dispose();
      }

      // Free native resources ...

    }

    #endregion IDisposable

    #region Implementation

    /// <summary>
    /// Start the server.
    /// </summary>
    /// <returns>true if successful, otherwise false.</returns>
    public bool Start()
    {
      if (_processConnectionThread)
        return false;

      _processConnectionThread = true;

      try
      {
        _messageQueue.ClearQueue();
        _messageQueue.Start();
      }
      catch
      {
        _processConnectionThread = false;

        throw;
      }

      try
      {
        _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _serverSocket.Bind(_localEndPoint);
        _serverSocket.Listen(SocketBacklog);
      }
      catch
      {
        _processConnectionThread = false;
        _serverSocket = null;

        throw;
      }

      try
      {
        _clientManagers = new List<ClientManager>();

        _connectionThread = new Thread(new ThreadStart(ConnectionThread));
        _connectionThread.Name = "IrssComms.Server.ConnectionThread";
        _connectionThread.IsBackground = true;
        _connectionThread.Start();
      }
      catch
      {
        _processConnectionThread = false;
        _serverSocket     = null;
        _clientManagers   = null;
        _connectionThread = null;

        throw;
      }

      return true;
    }

    /// <summary>
    /// Stop the server.
    /// </summary>
    public void Stop()
    {
      if (!_processConnectionThread)
        return;

      _messageQueue.Stop();

      _processConnectionThread = false;

      _serverSocket.Close();
      _serverSocket = null;

      lock (_clientManagers)
      {
        foreach (ClientManager manager in _clientManagers)
          manager.Dispose();

        _clientManagers.Clear();
        _clientManagers = null;
      }

      //_connectThread.Abort();
      //_connectThread.Join();
      _connectionThread = null;
    }

    /// <summary>
    /// Send a message to a particular client.
    /// </summary>
    /// <param name="sendTo">Client to send to.</param>
    /// <param name="message">Message to send.</param>
    /// <returns>true if successful, otherwise false.</returns>
    public bool Send(ClientManager sendTo, IrssMessage message)
    {
      if (!_clientManagers.Contains(sendTo))
        return false;

      try
      {
        sendTo.Send(message);
        return true;
      }
      catch (SocketException)
      {
        ClientDisconnect(sendTo);
        return false;
      }
    }

    void ClientManagerMessageSink(MessageManagerCombo combo)
    {
      _messageQueue.Enqueue(combo);
    }

    void QueueMessageSink(MessageManagerCombo combo)
    {
      _messageSink(combo);
    }

    void ClientDisconnect(object obj)
    {
      ClientManager clientManager = obj as ClientManager;

      if (clientManager == null || _clientManagers == null)
        return;

      lock (_clientManagers)
      {
        if (_clientManagers.Contains(clientManager))
          _clientManagers.Remove(clientManager);
      }

      if (_clientDisconnectCallback != null)
        _clientDisconnectCallback(clientManager);

      clientManager.Dispose();
    }

    void ConnectionThread()
    {
      try
      {
        ServerMessageSink clientManagerMessageSink = new ServerMessageSink(ClientManagerMessageSink);

        while (_processConnectionThread)
        {
          Socket socket = _serverSocket.Accept();

          ClientManager manager = new ClientManager(socket, clientManagerMessageSink);
          manager.DisconnectCallback = new WaitCallback(ClientDisconnect);

          lock (_clientManagers)
            _clientManagers.Add(manager);

          manager.Start();          
        }
      }
#if TRACE
      catch (SocketException socketException)
      {
        Trace.WriteLine(socketException.ToString());
      }
#else
      catch (SocketException)
      {
      }
#endif
    }

    #endregion Implementation

  }

}
