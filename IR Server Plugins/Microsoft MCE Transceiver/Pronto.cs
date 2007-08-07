using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MicrosoftMceTransceiver
{

  /// <summary>
  /// Philips Pronto interface class.
  /// </summary>
  public static class Pronto
  {

    #region Enumerations

    enum CodeType : ushort
    {
      RawOscillated   = 0x0000,
      RawUnmodulated  = 0x0100,
      RC5             = 0x5000,
      RC5X            = 0x5001,
      RC6             = 0x6000,
      RC6A            = 0x6001,
      VariableLength  = 0x7000,
      IndexToUDB      = 0x8000,
      NEC_1           = 0x9000,
      NEC_2           = 0x900A,
      NEC_3           = 0x900B,
      NEC_4           = 0x900C,
      NEC_5           = 0x900D,
      NEC_6           = 0x900E,
      YamahaNEC       = 0x9001,
    }

    enum CarrierFrequency : ushort
    {
      Khz38 = 0x006D,
      Khz36 = 0x0073,
    }

    #endregion Enumerations

    #region Constants

    const double ProntoClock = 0.241246;

    const ushort DefaultProntoRawCarrier = (ushort)CarrierFrequency.Khz38;

    static readonly byte[] RC6NativeHeader  = new byte[] { 0xB6, 0x12, 0x89, 0x12, 0x89, 0x09, 0x89, 0x09, 0x89, 0x12 };
    static readonly byte[] RC6ANativeHeader = new byte[] { 0xBF, 0x12, 0x89, 0x09, 0x89, 0x09, 0x89, 0x12, 0x89, 0x12 };

    #endregion Constants

    #region Public Methods

    /// <summary>
    /// Write Pronto data to a file.
    /// </summary>
    /// <param name="fileName">File to write Pronto data to.</param>
    /// <param name="prontoData">Pronto data to write.</param>
    /// <returns>Success.</returns>
    public static bool WriteProntoFile(string fileName, ushort[] prontoData)
    {
      try
      {
        StreamWriter file = new StreamWriter(fileName, false);

        for (int index = 0; index < prontoData.Length; index++)
        {
          file.Write(String.Format("{0:X4}", prontoData[index]));
          if (index != prontoData.Length - 1)
            file.Write(' ');
        }

        file.Flush();
        file.Close();
      }
      catch
      {
        return false;
      }

      return true;
    }

    public static MceIrCode ConvertProntoDataToMceIrCode(ushort[] prontoData)
    {
      switch ((CodeType)prontoData[0])
    {
        case CodeType.RawOscillated:
        case CodeType.RawUnmodulated:
          return ConvertProntoRawToMceIrCode(prontoData);

        case CodeType.RC5:
          return ConvertProntoRC5ToMceIrCode(prontoData);

        case CodeType.RC5X:
          return ConvertProntoRC5XToMceIrCode(prontoData);

        case CodeType.RC6:
          return ConvertProntoRC6ToMceIrCode(prontoData);

        case CodeType.RC6A:
          return ConvertProntoRC6AToMceIrCode(prontoData);

        default:
          return null;
      }
    }

    static MceIrCode ConvertProntoRawToMceIrCode(ushort[] prontoData)
    {
      int length = prontoData.Length;
      if (length < 5)
        return null;

      ushort prontoCarrier = prontoData[1];
      if (prontoCarrier == 0x0000)
        prontoCarrier = DefaultProntoRawCarrier;

      double carrier = (double)prontoCarrier * ProntoClock;

      int firstSeq = 2 * prontoData[2];
      int repeatSeq = 2 * prontoData[3];

      List<byte> nativeData = new List<byte>();

      bool pulse = true;
      int remaining = 0;
      int repeatCount = 0;
      int start = 4;
      bool done = false;

      int index = start;
      int sequence = firstSeq;

      if (firstSeq == 0)
      {
        if (repeatSeq == 0)
          return null;

        sequence = repeatSeq;
        repeatCount = 1;
      }

      while (!done)
      {
        if (remaining == 0)
          remaining = (ushort)(((prontoData[index] * carrier) + 25) / 50);

        if (remaining != 0)
        {

          if (remaining < 0x80)
          {
            nativeData.Add((byte)(remaining | (pulse ? 0x80 : 0x00)));
            remaining = 0;
            index++;
            pulse = !pulse;
          }
          else
          {
            nativeData.Add((byte)(0x7F | (pulse ? 0x80 : 0x00)));
            remaining -= 0x7F;
          }
        }

        if (index == start + sequence)
        {
          switch (repeatCount)
        {
            case 0:
              if (repeatSeq != 0)
          {
                start += firstSeq;
                sequence = repeatSeq;
                index = start;
                pulse = true;
            repeatCount++;
          }
              else
                done = true;
              break;

            case 3:
              done = true;
              break;

            default:
              index = start;
              pulse = true;
              repeatCount++;
              break;
        }
      }
    }

      return new MceIrCode(ConvertFromProntoCarrier(prontoCarrier), nativeData.ToArray());
    }
    static MceIrCode ConvertProntoRC5ToMceIrCode(ushort[] prontoData)
{
      if (prontoData.Length != 6)
        return null;

      if (prontoData[0] != (ushort)CodeType.RC5)
        return null;

      ushort prontoCarrier = prontoData[1];
      if (prontoCarrier == 0x0000)
        prontoCarrier = DefaultProntoRawCarrier;

      if (prontoData[2] + prontoData[3] != 1)
        return null;

      ushort system   = prontoData[4];
      ushort command  = prontoData[5];

      if (system > 31)
        return null;

      if (command > 127)
        return null;

      ushort rc5 = 0;

      rc5 |= (1 << 13); //SS1
      
      if (command < 64)
        rc5 |= (1 << 12); //SS2

      rc5 |= (1 << 11); //Toggle

      rc5 |= (ushort)((system << 6) | command);

      List<byte> nativeData = new List<byte>();

      bool lastIsPulse = true;
      byte current = 0x92;

      for (int i = 13; i > 0; i--)
    {
        if ((rc5 & (1 << i)) != 0)
      {
          if (lastIsPulse)
        {
            nativeData.Add(current);
            current = 0;
          }

          current += 0x12;
          nativeData.Add(current);

          current = 0x92;
          lastIsPulse = true;
        }
        else
        {
          if (!lastIsPulse)
      {
            nativeData.Add(current);
            current = 0;
          }

          current |= 0x80;
          current += 0x12;

          nativeData.Add(current);
          current = 0x12;

          lastIsPulse = false;
        }
      }

      for (int j = 0; j < 13; j++)
        nativeData.Add(0x7F);

      return new MceIrCode(ConvertFromProntoCarrier(prontoCarrier), nativeData.ToArray());
    }
    static MceIrCode ConvertProntoRC5XToMceIrCode(ushort[] prontoData)
    {
      if (prontoData.Length != 7)
        return null;

      if (prontoData[0] != (ushort)CodeType.RC5X)
        return null;

      ushort prontoCarrier = prontoData[1];
      if (prontoCarrier == 0x0000)
        prontoCarrier = DefaultProntoRawCarrier;

      if (prontoData[2] + prontoData[3] != 2)
        return null;

      ushort system   = prontoData[4];
      ushort command  = prontoData[5];
      ushort data     = prontoData[6];

      if (system > 31)
        return null;

      if (command > 127)
        return null;

      if (data > 63)
        return null;

      uint rc5 = 0;

      rc5 |= (1 << 19); //SS1

      if (command < 64)
        rc5 |= (1 << 18); //SS2

      rc5 |= (1 << 17); //Toggle

      rc5 |= (uint)((system << 12) | (command << 6) | data);

      List<byte> nativeData = new List<byte>();

      bool lastIsPulse = true;
      byte current = 0x92;

      for (int i = 19; i > 0; i--)
      {
        if (i == 11)
        {
          if (lastIsPulse)
          {
            nativeData.Add(current);
            current = 0;
          }
          current += 0x48;
          lastIsPulse = false;
        }
        
        if ((rc5 & (1 << i)) != 0)
        {
          if (lastIsPulse)
          {
            nativeData.Add(current);
            current = 0;
          }

          current += 0x12;
          nativeData.Add(current);

          current = 0x92;
          lastIsPulse = true;
        }
        else
        {
          if (!lastIsPulse)
          {
            nativeData.Add(current);
            current = 0;
          }

          current |= 0x80;
          current += 0x12;

          nativeData.Add(current);
          current = 0x12;

          lastIsPulse = false;
        }
      }

      for (int j = 0; j < 13; j++)
        nativeData.Add(0x7F);

      return new MceIrCode(ConvertFromProntoCarrier(prontoCarrier), nativeData.ToArray());
    }
    static MceIrCode ConvertProntoRC6ToMceIrCode(ushort[] prontoData)
    {
      if (prontoData.Length != 6)
        return null;

      if (prontoData[0] != (ushort)CodeType.RC6)
        return null;

      ushort prontoCarrier = prontoData[1];
      if (prontoCarrier == 0x0000)
        prontoCarrier = DefaultProntoRawCarrier;

      if (prontoData[2] + prontoData[3] != 1)
        return null;

      ushort system   = prontoData[4];
      ushort command  = prontoData[5];

      if (system > 255)
        return null;

      if (command > 255)
        return null;

      ushort rc6 = (ushort)((system << 8) | command);

      List<byte> nativeData = new List<byte>();
      nativeData.AddRange(RC6NativeHeader);

      bool lastIsPulse = true;
      byte current = 0x92;

      for (int i = 16; i > 0; i--)
      {
        if ((rc6 & (1 << i)) != 0)
        {
          if (!lastIsPulse)
          {
            nativeData.Add(current);
            current = 0;
          }

          current |= 0x80;
          current += 0x09;
          nativeData.Add(current);

          current = 0x09;
          lastIsPulse = false;
        }
        else
        {
          if (lastIsPulse)
          {
            nativeData.Add(current);
            current = 0;
          }

          current += 0x09;
          nativeData.Add(current);

          current = 0x89;

          lastIsPulse = true;
        }
      }

      nativeData.Add(current);

      for (int j = 0; j < 13; j++)
        nativeData.Add(0x7F);

      return new MceIrCode(ConvertFromProntoCarrier(prontoCarrier), nativeData.ToArray());
    }
    static MceIrCode ConvertProntoRC6AToMceIrCode(ushort[] prontoData)
    {
      if (prontoData.Length != 6)
        return null;

      if (prontoData[0] != (ushort)CodeType.RC6A)
        return null;

      ushort prontoCarrier = prontoData[1];
      if (prontoCarrier == 0x0000)
        prontoCarrier = DefaultProntoRawCarrier;

      if (prontoData[2] + prontoData[3] != 2)
        return null;

      ushort customer = prontoData[5];
      ushort system   = prontoData[5];
      ushort command  = prontoData[6];

      if (system > 255)
        return null;
      
      if (command > 255)
        return null;
      
      if (customer > 127 && customer < 32768)
        return null;

      uint rc6 = (uint)((customer << 16) | (system << 8) | command);

      List<byte> nativeData = new List<byte>();
      nativeData.AddRange(RC6ANativeHeader);

      bool lastIsPulse = true;
      byte current = 0x92;

      for (int i = ((customer >= 32768) ? 32 : 24); i > 0; i--)
      {
        if ((rc6 & (1 << i)) != 0)
        {
          if (!lastIsPulse)
          {
            nativeData.Add(current);
            current = 0;
          }

          current |= 0x80;
          current += 0x09;
          nativeData.Add(current);

          current = 0x09;
          lastIsPulse = false;
        }
        else
        {
          if (lastIsPulse)
          {
            nativeData.Add(current);
            current = 0;
          }

          current += 0x09;
          nativeData.Add(current);

          current = 0x89;
          lastIsPulse = true;
        }
      }

      nativeData.Add(current);

      for (int j = 0; j < 13; j++)
        nativeData.Add(0x7F);

      return new MceIrCode(ConvertFromProntoCarrier(prontoCarrier), nativeData.ToArray());
    }

    public static ushort[] ConvertMceIrCodeToProntoRaw(MceIrCode mceIrCode)
    {
      List<ushort> prontoData = new List<ushort>();

      double carrier;
      ushort prontoCarrier;
      CodeType codeType;
      if (mceIrCode.Carrier != 0)
      {
        codeType = CodeType.RawOscillated;
        carrier = mceIrCode.Carrier * ProntoClock;
        prontoCarrier = ConvertToProntoCarrier(mceIrCode.Carrier);
      }
      else
      {
        codeType = CodeType.RawUnmodulated;
        carrier = DefaultProntoRawCarrier * ProntoClock;
        prontoCarrier = DefaultProntoRawCarrier;
      }

      double accDuration = 0;
      bool lastPulse = false;

      for (int index = 0; index < mceIrCode.Data.Length; index++)
      {
        int duration = mceIrCode.Data[index] & 0x7F;
        bool pulse = ((mceIrCode.Data[index] & 0x80) != 0);

        double carrierDuration = (duration * 50) / carrier;

        if (pulse == lastPulse)
        {
          accDuration += carrierDuration;
        }
        else
        {
          if (accDuration != 0)
            prontoData.Add((ushort)accDuration);

          accDuration = carrierDuration;
        }

        lastPulse = pulse;
      }

      prontoData.Add((ushort)accDuration);

      ushort burstPairs = (ushort)(prontoData.Count / 2);

      prontoData.Insert(0, (ushort)codeType); // Pronto Code Type
      prontoData.Insert(1, prontoCarrier);    // IR Frequency
      prontoData.Insert(2, burstPairs);       // First Burst Pairs
      prontoData.Insert(3, 0x0000);           // Repeat Burst Pairs

      return prontoData.ToArray();
    }

    public static bool IsProntoData(byte[] fileBytes)
    {
      ushort[] prontoData = null;

      try
      {
        prontoData = ConvertFileBytesToProntoData(fileBytes);
      }
      catch
      {
        return false;
      }

      switch ((CodeType)prontoData[0])
      {
        case CodeType.RawOscillated:
        case CodeType.RawUnmodulated:
        case CodeType.RC5:
        case CodeType.RC5X:
        case CodeType.RC6:
        case CodeType.RC6A:
          return true;

        default:
          return false;
      }
    }

    public static ushort[] ConvertFileBytesToProntoData(byte[] fileBytes)
    {
      StringBuilder dataStringBuilder = new StringBuilder(fileBytes.Length);
      foreach (char dataByte in fileBytes)
        dataStringBuilder.Append((char)dataByte);

      string[] stringData = dataStringBuilder.ToString().Split(new char[] { ' ' });

      ushort[] prontoData = new ushort[stringData.Length];
      for (int i = 0; i < stringData.Length; i++)
        prontoData[i] = ushort.Parse(stringData[i], System.Globalization.NumberStyles.HexNumber);

      return prontoData;
    }

    public static int ConvertFromProntoCarrier(ushort prontoCarrier)
    {
      return (int)(1000000 / (prontoCarrier * ProntoClock));
    }

    public static ushort ConvertToProntoCarrier(int carrierFrequency)
    {
      return (ushort)((1000000 / ProntoClock) / carrierFrequency);
    }

    #endregion Public Methods

  }

}
