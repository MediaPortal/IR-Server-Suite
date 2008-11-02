using System.Net.Sockets;

namespace InputService.Plugin
{
  /// <summary>
  /// Encapsulates a Socket and a Receive Buffer.
  /// </summary>
  internal class CSocketPacket
  {
    public byte[] ReceiveBuffer = new byte[350];
    public Socket ThisSocket;
  }
}