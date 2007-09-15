using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace IrssComms
{

  #region Delegates

  /// <summary>
  /// Message handling delegate for client.
  /// </summary>
  /// <param name="message">Message received.</param>
  public delegate void ClientMessageSink(Message message);

  #endregion Delegates

  /// <summary>
  /// TCP communications client class.
  /// </summary>
  public class Client
  {

    #region Variables

    IPEndPoint _serverEndPoint;

    Socket _serverSocket = null;

    bool _processReceiveThread = false;
    Thread _receiveThread;

    ClientMessageSink _messageSink;

    #endregion Variables

    #region Constructor

    /// <summary>
    /// Create a TCP communications client.
    /// </summary>
    /// <param name="server">IP Address of Server.</param>
    /// <param name="port">Port to open on Server.</param>
    /// <param name="messageSink">Where to send incoming messages.</param>
    public Client(IPAddress server, int port, ClientMessageSink messageSink)
    {
      _serverEndPoint = new IPEndPoint(server, port);

      _messageSink  = messageSink;
    }

    #endregion Constructor

    #region Implementation

    /// <summary>
    /// Start the client communications.
    /// </summary>
    /// <returns>Success.</returns>
    public bool Start()
    {
      if (_processReceiveThread)
        return false;

      _processReceiveThread = true;

      _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

      try
      {
        _serverSocket.Connect(_serverEndPoint);
      }
      catch (SocketException)
      {
        return false;
      }

      _receiveThread = new Thread(new ThreadStart(ReceiveThread));
      _receiveThread.Name = "IrssComms.Client";
      _receiveThread.IsBackground = true;
      _receiveThread.Start();

      return true;
    }

    /// <summary>
    /// Stop the client communications.
    /// </summary>
    public void Stop()
    {
      if (!_processReceiveThread)
        return;

      _processReceiveThread = false;

      _serverSocket.Close();
      _serverSocket = null;

      _receiveThread.Abort();
      _receiveThread.Join();
      _receiveThread = null;
    }

    /// <summary>
    /// Send a message to the server.
    /// </summary>
    /// <param name="message">Message to send.</param>
    /// <returns>Success.</returns>
    public bool Send(Message message)
    {
      if (_serverSocket == null)
        return false;
      
      byte[] data = message.ToBytes();

      try
      {
        _serverSocket.Send(data);
        return true;
      }
      catch (SocketException)
      {
        return false;
      }
    }

    void ReceiveThread()
    {
      byte[] buffer = new byte[4096];

      try
      {
        while (_processReceiveThread)
        {
          int bytesRead = _serverSocket.Receive(buffer);

          byte[] packet = new byte[bytesRead];
          Array.Copy(buffer, 0, packet, 0, bytesRead);

          Message message = Message.FromBytes(packet);

          _messageSink(message);
        }
      }
      catch (SocketException)
      {

      }
    }

    #endregion Implementation

  }

}
