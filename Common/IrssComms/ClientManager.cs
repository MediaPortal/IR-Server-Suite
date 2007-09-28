using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    #endregion Variables

    #region Constructor

    internal ClientManager(Socket connection, ServerMessageSink sink)
    {
      _connection = connection;
      _messageSink = sink;
    }

    #endregion Constructor

    #region IDisposable

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposeManagedResources)
    {
      if (disposeManagedResources)
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

      _connection.Close();
      _connection = null;

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
            break; // TODO: Inform server to remove clientmanager from list? (Low)

          int readSize = BitConverter.ToInt32(buffer, 0);
          readSize = IPAddress.NetworkToHostOrder(readSize);

          byte[] packet = new byte[readSize];

          bytesRead = _connection.Receive(packet, packet.Length, SocketFlags.None);
          if (bytesRead != packet.Length)
            break;

          IrssMessage message = IrssMessage.FromBytes(packet);
          MessageManagerCombo combo = new MessageManagerCombo(message, this);
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
    }

    #endregion Implementation

  }

}