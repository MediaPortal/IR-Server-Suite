using System;
using System.Net.Sockets;

namespace IRTransTransceiver
{

  public class CSocketPacket
  {
    public Socket ThisSocket;
    public byte[] ReceiveBuffer = new byte[350];
  }

}
