using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace VirtualRemote
{

  #region Delegates

  /// <summary>
  /// Message handling delegate for client.
  /// </summary>
  /// <param name="message">Message received.</param>
  public delegate void ClientMessageSink(IrssMessage message);

  #endregion Delegates

  /// <summary>
  /// TCP communications client class.
  /// </summary>
  public class Client : IDisposable
  {

    #region Variables

    IPEndPoint _serverEndpoint;
    Socket _serverSocket;

    bool _processConnectionThread = false;
    bool _connected = false;

    ClientMessageSink _messageSink;
    
    WaitCallback _connectCallback;
    WaitCallback _disconnectCallback;
    WaitCallback _commsFailureCallback;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Is this client connected?
    /// </summary>
    public bool Connected
    {
      get { return _connected; }
    }

    /// <summary>
    /// Gets or Sets the Connect callback.
    /// </summary>
    public WaitCallback ConnectCallback
    {
      get { return _connectCallback; }
      set { _connectCallback = value; }
    }

    /// <summary>
    /// Gets or Sets the Disconnect callback.
    /// </summary>
    public WaitCallback DisconnectCallback
    {
      get { return _disconnectCallback; }
      set { _disconnectCallback = value; }
    }

    /// <summary>
    /// Gets or Sets the Communications Failure callback.
    /// </summary>
    public WaitCallback CommsFailureCallback
    {
      get { return _commsFailureCallback; }
      set { _commsFailureCallback = value; }
    }

    #endregion Properties

    #region Constructor

    /// <summary>
    /// Create a TCP communications client.
    /// </summary>
    /// <param name="serverEndPoint">IP Address and Port combination of Server.</param>
    /// <param name="messageSink">The message sink to call for messages.</param>
    public Client(IPEndPoint serverEndPoint, ClientMessageSink messageSink)
    {
      _serverEndpoint = serverEndPoint;
      
      _messageSink = messageSink;
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
      }

      // Free native resources ...

    }

    #endregion IDisposable

    #region Implementation

    /// <summary>
    /// Start the client communications.
    /// </summary>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public bool Start()
    {
      if (_processConnectionThread)
        return false;

      _processConnectionThread = true;
      _connected = false;

      try
      {
        Thread connectionThread = new Thread(new ThreadStart(ConnectionThread));
        connectionThread.Start();
      }
      catch
      {
        _processConnectionThread = false;

        throw;
      }

      return true;
    }

    /// <summary>
    /// Stop the client communications.
    /// </summary>
    public void Stop()
    {
      if (!_processConnectionThread)
        return;

      _processConnectionThread = false;
      _connected = false;

      _serverSocket.Close();
      _serverSocket = null;
    }

    /// <summary>
    /// Send a message to the server.
    /// </summary>
    /// <param name="message">Message to send.</param>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public bool Send(IrssMessage message)
    {
      if (message == null)
        throw new ArgumentNullException("message");

      if (_serverSocket == null)
        return false;
      
      byte[] data = message.ToBytes();

      int dataLength = IPAddress.HostToNetworkOrder(data.Length);

      byte[] dataLengthBytes = BitConverter.GetBytes(dataLength);

      try
      {
        // Send packet size ...
        _serverSocket.Send(dataLengthBytes);

        // Send packet ...
        _serverSocket.Send(data);
        
        return true;
      }
      catch (SocketException)
      {
        return false;
      }
    }

    void QueueMessageSink(IrssMessage message)
    {
      _messageSink(message);
    }

    void ConnectionThread()
    {
      // Outer loop is for reconnection attempts ...
      while (_processConnectionThread)
      {
        _connected = false;

        _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        #region Attempt to connect

        while (_processConnectionThread)
        {
          try
          {
            _serverSocket.Connect(_serverEndpoint);
            break;
          }
          catch (SocketException socketException)
          {
            if (!_processConnectionThread)
              return;

            if (socketException.ErrorCode == 10061) // ConnectionRefused
            {
              Thread.Sleep(1000);
              continue;
            }

            if (_commsFailureCallback != null)
              _commsFailureCallback(socketException);
            else
              throw;
          }
          catch (Exception ex)
          {
            if (!_processConnectionThread)
              return;

            if (_commsFailureCallback != null)
              _commsFailureCallback(ex);
            else
              throw;
          }
        }

        #endregion Attempt to connect

        if (!_processConnectionThread)
          return;

        _connected = true;

        if (_connectCallback != null)
          _connectCallback(null);

        #region Read from socket

        try
        {
          byte[] buffer = new byte[4];

          int bytesRead;

          // Read data from socket ...
          while (_processConnectionThread)
          {
            bytesRead = _serverSocket.Receive(buffer, buffer.Length, SocketFlags.None);
            if (bytesRead != buffer.Length)
              break;

            int readSize = BitConverter.ToInt32(buffer, 0);
            readSize = IPAddress.NetworkToHostOrder(readSize);

            byte[] packet = new byte[readSize];

            bytesRead = _serverSocket.Receive(packet, packet.Length, SocketFlags.None);
            if (bytesRead != packet.Length)
              break;

            IrssMessage message = IrssMessage.FromBytes(packet);
            _messageSink(message);
          }

          if (!_processConnectionThread)
            return;

          if (_disconnectCallback != null)
            _disconnectCallback(null);

        }
        catch (SocketException socketException)
        {
          if (!_processConnectionThread)
            return;

          if (socketException.ErrorCode == 10054) // ConnectionReset
          {
            if (_disconnectCallback != null)
              _disconnectCallback(null);
          }
          else
          {
            if (_commsFailureCallback != null)
              _commsFailureCallback(socketException);
            else
              throw;
          }
        }
        catch (Exception ex)
        {
          if (!_processConnectionThread)
            return;

          if (_commsFailureCallback != null)
            _commsFailureCallback(ex);
          else
            throw;
        }

        #endregion Read from socket

      }
    }

    #endregion Implementation

  }

}
