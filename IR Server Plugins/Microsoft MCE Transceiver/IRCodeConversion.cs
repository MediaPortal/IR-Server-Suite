using System;
using System.Collections.Generic;
using System.Text;

namespace MceTransceiver
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
          jump = packetByte - 0x80;

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

    public static byte[] ConvertNativeDataToRawPacket(byte[] nativeData)
    {
      byte bytesToCopy;
      int dataBytesIndex = 0;
      List<byte> rawPacket = new List<byte>();

      for (int counter = 0; counter < nativeData.Length; counter += 4)
      {
        bytesToCopy = (byte)(nativeData.Length - counter < 4 ? nativeData.Length - counter : 4);

        rawPacket.Add((byte)(0x80 + bytesToCopy));

        for (int index = 0; index < bytesToCopy; index++)
          rawPacket.Add(nativeData[dataBytesIndex + index]);

        dataBytesIndex += bytesToCopy;
      }

      rawPacket.Add(0x80);

      return rawPacket.ToArray();
    }

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
          code -= 0x80;
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
