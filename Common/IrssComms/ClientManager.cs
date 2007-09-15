using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace IrssComms
{

  public class ClientManager
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

      _receiveThread.Abort();
      _receiveThread.Join();
      _receiveThread = null;
    }

    internal void Send(Message message)
    {
      byte[] data = message.ToBytes();

      _connection.Send(data);
    }
    
    void ReceiveThread()
    {
      byte[] buffer = new byte[8192];

      try
      {
        while (_processReceiveThread)
        {
          int bytesRead = _connection.Receive(buffer);

          byte[] packet = new byte[bytesRead];
          Array.Copy(buffer, 0, packet, 0, bytesRead);

          Message message = Message.FromBytes(packet);

          _messageSink(message, this);
        }
      }
      catch (SocketException)
      {

      }
    }

    #endregion Implementation

  }

}
