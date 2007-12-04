using System;
using System.Collections.Generic;
#if TRACE
using System.Diagnostics;
#endif
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace IrssComms
{

  /// <summary>
  /// Manages Server socket connections.
  /// </summary>
  public class ClientManager : IDisposable
  {

    #region Variables

    bool _processReceiveThread = false;
    Thread _receiveThread;
    
    ServerMessageSink _messageSink;

    Socket _connection;

    WaitCallback _disconnectCallback;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Gets or Sets the Disconnect callback.
    /// </summary>
    public WaitCallback DisconnectCallback
    {
      get { return _disconnectCallback; }
      set { _disconnectCallback = value; }
    }

    #endregion Properties

    #region Constructor

    internal ClientManager(Socket connection, ServerMessageSink sink)
    {
      _connection = connection;
      _messageSink = sink;
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

    internal void Start()
    {
      if (_processReceiveThread)
        return;

      _processReceiveThread = true;

      _receiveThread = new Thread(new ThreadStart(ReceiveThread));
      _receiveThread.Name = "IrssComms.ClientManager";
      _receiveThread.IsBackground = true;
      _receiveThread.Start();
    }

    internal void Stop()
    {
      if (!_processReceiveThread)
        return;

      _processReceiveThread = false;

      if (_connection != null)
      {
        _connection.Close(100);
        _connection = null;
      }

      _messageSink = null;

      //_receiveThread.Abort();
      //_receiveThread.Join();
      _receiveThread = null;
    }

    internal void Send(IrssMessage message)
    {
      byte[] data = message.ToBytes();

      int dataLength = IPAddress.HostToNetworkOrder(data.Length);

      byte[] dataLengthBytes = BitConverter.GetBytes(dataLength);

      // Send packet size ...
      _connection.Send(dataLengthBytes);

      // Send packet ...
      _connection.Send(data);
    }

    void ReceiveThread()
    {
      try
      {
        byte[] buffer = new byte[4];
        int bytesRead;

        while (_processReceiveThread)
        {
          bytesRead = _connection.Receive(buffer, buffer.Length, SocketFlags.None);
          if (bytesRead != buffer.Length)
            break;

          int readSize = BitConverter.ToInt32(buffer, 0);
          readSize = IPAddress.NetworkToHostOrder(readSize);

          byte[] packet = new byte[readSize];

          bytesRead = _connection.Receive(packet, packet.Length, SocketFlags.None);
          if (bytesRead != packet.Length)
            break;

          IrssMessage message = IrssMessage.FromBytes(packet);
          MessageManagerCombo combo = new MessageManagerCombo(message, this);
          
          if (_messageSink != null)
            _messageSink(combo);
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
      finally
      {
        if (_connection != null)
        {
          _connection.Close(100);
          _connection = null;
        }

        _disconnectCallback(this);
      }
    }

    #endregion Implementation

  }

}
