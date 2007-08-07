using System;
using System.Collections.Generic;
using System.Text;

namespace MicrosoftMceTransceiver
{

  public static class IRCodeConversion
  {

    #region Member Variables

    static uint[] timingData = null;
    static int timingDataCount = 0;
    static uint lastSpace = IRDecoder.PulseMask;

    #endregion Member Variables

    public static byte[] ConvertRawPacketToNativeData(byte[] rawPacket)
    {
      List<byte> nativeData = new List<byte>();
      byte packetByte;
      int jump = 0;

      for (int index = 0; index < rawPacket.Length; index += jump + 1)
      {
        packetByte = rawPacket[index];

        if (packetByte == 0x9F)
        {
          // Packet is finished
          break;
        }

        if (packetByte >= 0x81 && packetByte <= 0x84)
        {
          jump = packetByte & 0x7F;

          if (index + jump + 1 > rawPacket.Length)
          {
            // bad packet
            return null;
          }

          for (int get = index + 1; get < index + jump + 1; get++)
            nativeData.Add(rawPacket[get]);
        }
        else
        {
          // Bad packet
          return null;
        }
      }

      return nativeData.ToArray();
    }

    // TODO: Rewrite
    public static uint[] ConvertNativeDataToTimingData(byte[] nativeData)
    {
      int index = 0;
      uint code;

      if (timingData == null || nativeData == null)
      {
        timingData = new uint[100];
        timingData[1] = lastSpace;
        timingData[2] = 0;
        timingDataCount = 2;
      }

      if (nativeData == null)
        return null;

      bool pulse;

      while (index < nativeData.Length)
      {
        pulse = false;
        code = nativeData[index++];

        if ((code & 0x80) != 0)
        {
          pulse = true;
          code &= 0x7F;
        }
        code *= 50;

        if (pulse)
        {
          if (lastSpace != 0)
          {
            timingData[timingDataCount++] = lastSpace;
            lastSpace = 0;
            timingData[timingDataCount] = 0;
          }

          timingData[timingDataCount] += code;
          timingData[timingDataCount] |= IRDecoder.PulseBit;
        }
        else
        {
          if (timingData[timingDataCount] != 0 && lastSpace == 0)
            timingDataCount++;

          lastSpace += code;
        }
      }

      uint[] outData = new uint[timingDataCount];
      Array.Copy(timingData, outData, outData.Length);

      timingData[0] = timingData[timingDataCount];
      timingDataCount = 0;

      return outData;
    }

  }

}
