using System;
using System.Collections.Generic;
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
  /// <param name="message"></param>
  /// <param name="from"></param>
  public delegate void ServerMessageSink(Message message, ClientManager from);

  #endregion Delegates

  /// <summary>
  /// TCP communications server class.
  /// </summary>
  public class Server
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

    bool _processConnectThread = false;
    Thread _connectThread;

    List<ClientManager> _clientManagers;

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
    }

    #endregion Constructor

    #region Implementation

    /// <summary>
    /// Start the server.
    /// </summary>
    /// <returns>Success.</returns>
    public bool Start()
    {
      if (_processConnectThread)
        return false;

      _processConnectThread = true;

      _clientManagers = new List<ClientManager>();

      _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      _serverSocket.Bind(_localEndPoint);
      _serverSocket.Listen(SocketBacklog);

      _connectThread = new Thread(new ThreadStart(ConnectionThread));
      _connectThread.Name = "IrssComms.Server";
      _connectThread.IsBackground = true;
      _connectThread.Start();

      return true;
    }

    /// <summary>
    /// Stop the server.
    /// </summary>
    public void Stop()
    {
      if (!_processConnectThread)
        return;

      _processConnectThread = false;

      _serverSocket.Close();
      _serverSocket = null;

      lock (_clientManagers)
      {
        foreach (ClientManager manager in _clientManagers)
          manager.Stop();

        _clientManagers.Clear();
        _clientManagers = null;
      }

      _connectThread.Abort();
      _connectThread.Join();
      _connectThread = null;
    }

    /// <summary>
    /// Send a message to a particular client.
    /// </summary>
    /// <param name="sendTo">Client to send to.</param>
    /// <param name="message">Message to send.</param>
    /// <returns>Success.</returns>
    public bool Send(ClientManager sendTo, Message message)
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

    void ConnectionThread()
    {
      try
      {
        while (_processConnectThread)
        {
          Socket socket = _serverSocket.Accept();

          ClientManager manager = new ClientManager(socket, _messageSink);

          lock (_clientManagers)
            _clientManagers.Add(manager);

          manager.Start();          
        }
      }
      catch (SocketException)
      {

      }
    }

    #endregion Implementation

  }

}
