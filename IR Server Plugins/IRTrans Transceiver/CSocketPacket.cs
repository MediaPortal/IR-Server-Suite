using System;
using System.Net.Sockets;

namespace InputService.Plugin
{

  /// <summary>
  /// Encapsulates a Socket and a Receive Buffer.
  /// </summary>
  class CSocketPacket
  {
    public Socket ThisSocket;
    public byte[] ReceiveBuffer = new byte[350];
  }

}
