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

    GenericMessageQueue<MessageManagerCombo> _messageQueue;

    #endregion Variables

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

      _messageQueue = new GenericMessageQueue<MessageManagerCombo>(new GenericMessageQueueSink<MessageManagerCombo>(QueueMessageSink));
    }

    #endregion Constructor
    
    #region IDisposable

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        // Dispose managed resources ...
        if (_processConnectionThread)
          Stop();
      }

      // Free native resources ...

    }

    #endregion IDisposable

    #region Implementation

    /// <summary>
    /// Start the server.
    /// </summary>
    /// <returns>Success.</returns>
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
        _serverSocket = null;
        _clientManagers = null;
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
          manager.Stop();

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
    /// <returns>Success.</returns>
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
        _clientManagers.Remove(sendTo);
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

    void ConnectionThread()
    {
      try
      {
        ServerMessageSink clientManagerMessageSink = new ServerMessageSink(ClientManagerMessageSink);

        while (_processConnectionThread)
        {
          Socket socket = _serverSocket.Accept();

          ClientManager manager = new ClientManager(socket, clientManagerMessageSink);

          lock (_clientManagers)
            _clientManagers.Add(manager);

          manager.Start();          
        }
      }
      catch (SocketException socketException)
      {
        Trace.Write(socketException.ToString());
      }
    }

    #endregion Implementation

  }

}
