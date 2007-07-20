using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MceTransceiver
{

  /// <summary>
  /// Phillips Pronto interface class
  /// </summary>
  public static class Pronto
  {

    #region Constants

    const double Clock = 0.241246;

    const ushort DefaultProntoLearnedFrequency = 0x006D; // 38khz

    #endregion Constants

    #region Public Methods

    /// <summary>
    /// Write Pronto data to a file
    /// </summary>
    /// <param name="fileName">File to write Pronto data to</param>
    /// <param name="prontoData">Pronto data to write</param>
    /// <returns>Success</returns>
    public static bool WriteFile(string fileName, ushort[] prontoData)
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

    public static ushort[] ConvertNativeDataToProntoLearned(byte[] nativeData)
    {

      return null;
    }

    public static byte[] ConvertProntoDataToNativeData(ushort[] prontoData)
    {
      byte[] nativeData = null;

      if (prontoData[0] == 0x0000 || prontoData[0] == 0x0100)
        nativeData = ConvertProntoLearnedToNativeData(prontoData);
      else if (prontoData[0] == 0x5000)
        nativeData = ConvertProntoRC5ToNativeData(prontoData);
      else if (prontoData[0] == 0x5001)
        nativeData = ConvertProntoRC5XToNativeData(prontoData);
      else if (prontoData[0] == 0x6000)
        nativeData = ConvertProntoRC6ToNativeData(prontoData);
      else if (prontoData[0] == 0x6001)
        nativeData = ConvertProntoRC6XToNativeData(prontoData);

      return nativeData;
    }

    public static byte[] ConvertProntoLearnedToNativeData(ushort[] prontoData)
    {
      int length = prontoData.Length;
      if (length < 5)
        return null;

      ushort clock = prontoData[1];
      if (clock == 0)
        clock = DefaultProntoLearnedFrequency;

      double carrier = (double)clock * Clock;

      ushort sequence;
      if (prontoData[2] != 0)
        sequence = (ushort)(prontoData[2] * 2);
      else
        sequence = (ushort)(prontoData[3] * 2);

      bool nextIsPulse = true;

      int remaining = 0;
      int repeatCount = 0;

      int dataPosition;
      int index = 0;

      List<byte> nativeData = new List<byte>();

      for (; ; )
      {
        dataPosition = 4 + index;

        if (remaining != 0)
        {
          if (remaining < 0x80)
          {
            nativeData.Add((byte)(remaining | (nextIsPulse ? 0x00 : 0x80)));
            remaining = 0;
          }
          else
          {
            nativeData.Add((byte)(0x7F | (nextIsPulse ? 0x00 : 0x80)));
            remaining -= 0x7F;
          }
        }

        if (remaining == 0)
        {
          if ((dataPosition >= prontoData.Length) || (sequence == 0))
          {
            if (repeatCount >= 3)
              return nativeData.ToArray();

            index = 0; // restart
            repeatCount++;
            nextIsPulse = true;
            continue;
          }

          index++;

          if (dataPosition > prontoData.Length)
            return null;

          sequence--;
          remaining = (ushort)(((prontoData[dataPosition] * carrier) + 25) / 50);
          nextIsPulse = !nextIsPulse;
        }
      }
    }

    /*
     static BOOL MceIrImportProntoLearned(PUCHAR pData, ULONG Length, PUCHAR *pBuf, PULONG pReadCount)
{
  PCHAR pHeader;
  double Carrier;
  ULONG Sequence, Seq;
  USHORT Clock, Seq1, Seq2;
  PUCHAR pBuffer = NULL;
  PUCHAR pData2 = NULL;
  PUCHAR pPronto;
  USHORT Remaining = 0;
  BYTE RepeatCount = 0;
  BOOL NextIsPulse = TRUE;
  BOOL Result = FALSE;

  do
  {
    //Read 15 bytes of header
    if (Length < 20) break;
    pHeader = (PCHAR)pData + 5;

    if (sscanf_s(pHeader, " %04x %04x %04x", &Clock, &Seq1, &Seq2) != 3) break;
    if (Clock == 0) break;

    pBuffer = pData2 = (PUCHAR) HeapAlloc(GetProcessHeap(), 0, (Length << 4));
    if (!pBuffer) break;

    Carrier = Clock * .241246;
    Sequence = Seq = (Seq1 ? Seq1 : Seq2) * 2;
    NextIsPulse = TRUE;
    Remaining = 0;
    RepeatCount = 0;

    *pReadCount = 0;
    pPronto = pData + 15;

    for(;;)
    {
      if (Remaining)
      {
        if (Remaining < 128)
        {
          *pBuffer++ = (UCHAR)Remaining | (NextIsPulse ? 0x00 : 0x80);
          (*pReadCount)++;
          Remaining = 0;
        }
        else
        {
          *pBuffer++ = 0x7F | (NextIsPulse ? 0x00 : 0x80);
          (*pReadCount)++;
          Remaining -= 0x7F;
        }
      }
      if (!Remaining)
      {
        if ((pPronto >= pData + Length) || (Seq == 0))
        {
          if (RepeatCount >= 3)
          {
            Result = TRUE;
            break;
          }
          pPronto = pData + 15;
          RepeatCount++;
          NextIsPulse = TRUE;
          Seq = Sequence;
          continue;
        }
        pPronto += 5;
        if (pPronto > pData + Length) break;
        if (sscanf_s((PCHAR)pPronto, " %04x", &Remaining) != 1)  break;
        Seq--;
        Remaining = (USHORT)(((Remaining * Carrier) + 25) / 50);
        NextIsPulse = !NextIsPulse;
      }
    }
    if (!Result)
    {
      if (pData2) HeapFree(GetProcessHeap(), 0, pData2);
      pData2 = NULL;
      break;
    }
    HeapFree(GetProcessHeap(), 0, pData);
    *pBuf = pData2;

  } while (FALSE);

  return Result;
}

*/


    public static byte[] ConvertProntoRC5ToNativeData(ushort[] prontoData)
    {
      return null;
    }
    public static byte[] ConvertProntoRC5XToNativeData(ushort[] prontoData)
    {
      return null;
    }
    public static byte[] ConvertProntoRC6ToNativeData(ushort[] prontoData)
    {
      return null;
    }
    public static byte[] ConvertProntoRC6XToNativeData(ushort[] prontoData)
    {
      return null;
    }

/*
static BOOL MceIrImportProntoRC5(PUCHAR pData, ULONG Length, PUCHAR *pBuf, PULONG pReadCount)
{
  USHORT Proto, Clock, Seq1, Seq2;
  USHORT System, Command;
  USHORT RC5 = 0;
  PUCHAR pBuffer = NULL;
  //  PUCHAR pData2 = NULL;
  BOOL LastIsPulse = TRUE;
  int i;

  //Read 15 bytes of header
  if (Length < 20)
    return FALSE;
  if (sscanf_s((PCHAR)pData, "%04x %04x %04x %04x", &Proto, &Clock, &Seq1, &Seq2) != 4) 
    return FALSE;
  if (Proto != PRONTO_PROTO_RC5)
    return FALSE;
  if (Seq1 + Seq2 != 1)
    return FALSE;

  pData += 20;
  if (sscanf_s((PCHAR)pData, "%04x %04x", &System, &Command) != 2)
    return FALSE;
  if (System > 31)
    return FALSE;
  if (Command > 127)
    return FALSE;

  BIT_SET(RC5, 13); //SS1
  if (Command < 64)
    BIT_SET(RC5, 12); //SS2
  BIT_SET(RC5, 11); //Toggle

  RC5 |= (System << 6) | Command;

  Trace("PLAY_RC5_KEYCODE code:%04X !!!\n", RC5);

  pBuffer = *pBuf = (PUCHAR) HeapAlloc(GetProcessHeap(), HEAP_ZERO_MEMORY, 1000);
  if (!pBuffer)
    return FALSE;

  *pBuffer = 0x92;
  for (i = 13 ; i-- ; )
  {
    if (BIT_TEST(RC5, i))
    {
      if (LastIsPulse)
        pBuffer++;

      *pBuffer += 0x12;
      pBuffer++;
      *pBuffer = 0x92;
      LastIsPulse = TRUE;
    }
    else
    {
      if (!LastIsPulse)
        pBuffer++;

      *pBuffer |= 0x80;
      *pBuffer += 0x12;
      pBuffer++;
      *pBuffer = 0x12;
      LastIsPulse = FALSE;
    }
  }
  pBuffer++;

  *pBuffer++ = 0x7F;
  *pBuffer++ = 0x7F;
  *pBuffer++ = 0x7F;
  *pBuffer++ = 0x7F;
  *pBuffer++ = 0x7F;
  *pBuffer++ = 0x7F;
  *pBuffer++ = 0x7F;
  *pBuffer++ = 0x7F;
  *pBuffer++ = 0x7F;
  *pBuffer++ = 0x7F;
  *pBuffer++ = 0x7F;
  *pBuffer++ = 0x7F;
  *pBuffer++ = 0x7F;

  memcpy(pBuffer, *pBuf, pBuffer - *pBuf);
  pBuffer += pBuffer - *pBuf;
  memcpy(pBuffer, *pBuf, pBuffer - *pBuf);
  pBuffer += pBuffer - *pBuf;

  *pReadCount = (ULONG) (pBuffer - *pBuf);

  return TRUE;
}

static BOOL MceIrImportProntoRC5X(PUCHAR pData, ULONG Length, PUCHAR *pBuf, PULONG pReadCount)
{
  BOOL Result = FALSE;
  USHORT Proto, Clock, Seq1, Seq2;
  USHORT System, Command, Data;
  ULONG RC5 = 0;
  PUCHAR pBuffer = NULL;
  //  PUCHAR pData2 = NULL;
  BOOL LastIsPulse = TRUE;
  int i;

  do
  {
    //Read 15 bytes of header
    if (Length < 20)
      break;
    if (sscanf_s((PCHAR)pData, "%04x %04x %04x %04x", &Proto, &Clock, &Seq1, &Seq2) != 4) 
      break;
    if (Proto != PRONTO_PROTO_RC5X) 
      break;
    if (Seq1 + Seq2 != 2) 
      break;

    pData += 20;
    if (sscanf_s((PCHAR)pData, "%04x %04x %04x", &System, &Command, &Data) != 3) 
      break;
    if (System > 31) 
      break;
    if (Command > 127) 
      break;
    if (Data > 63) 
      break;

    BIT_SET(RC5, 19); //SS1
    if (Command < 64)
      BIT_SET(RC5, 18); //SS2
    BIT_SET(RC5, 17); //Toggle

    RC5 |= (System << 12) | (Command << 6) | Data;

    Trace("PLAY_RC5X_KEYCODE code:%08X !!!\n", RC5);

    pBuffer = *pBuf = (PUCHAR) HeapAlloc(GetProcessHeap(), HEAP_ZERO_MEMORY, 1000);
    if (!pBuffer)
      break;

    *pBuffer = 0x92;
    for (i = 19 ; i-- ; )
    {
      if (i == 11)
      {
        if (LastIsPulse) 
          pBuffer++;
        *pBuffer += 0x48;
        LastIsPulse = FALSE;
      }
      if (BIT_TEST(RC5, i))
      {
        if (LastIsPulse) 
          pBuffer++;
        *pBuffer += 0x12;
        pBuffer++;
        *pBuffer = 0x92;
        LastIsPulse = TRUE;
      }
      else
      {
        if (!LastIsPulse) 
          pBuffer++;
        *pBuffer |= 0x80;
        *pBuffer += 0x12;
        pBuffer++;
        *pBuffer = 0x12;
        LastIsPulse = FALSE;
      }
    }
    pBuffer++;

    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;

    memcpy(pBuffer, *pBuf, pBuffer - *pBuf);
    pBuffer += pBuffer - *pBuf;
    memcpy(pBuffer, *pBuf, pBuffer - *pBuf);
    pBuffer += pBuffer - *pBuf;

    *pReadCount = (ULONG) (pBuffer - *pBuf);

    Result = TRUE;

  } while (FALSE);

  return Result;
}

static BOOL MceIrImportProntoRC6(PUCHAR pData, ULONG Length, PUCHAR *pBuf, PULONG pReadCount)
{
  BOOL Result = FALSE;
  USHORT Proto, Clock, Seq1, Seq2;
  USHORT System, Command;
  ULONG RC6 = 0;
  PUCHAR pBuffer = NULL;
  //  PUCHAR pData2 = NULL;
  BOOL LastIsPulse = TRUE;
  int i;

  do
  {
    //Read 15 bytes of header
    if (Length < 20) break;
    if (sscanf_s((PCHAR)pData, "%04x %04x %04x %04x", &Proto, &Clock, &Seq1, &Seq2) != 4) break;
    if (Proto != PRONTO_PROTO_RC6) break;
    if (Seq1 + Seq2 != 1) break;

    pData += 20;
    if (sscanf_s((PCHAR)pData, "%04x %04x", &System, &Command) != 2) break;
    if (System > 255) break;
    if (Command > 255) break;

    RC6 = (System << 8) | Command;

    Trace("PLAY_RC6_KEYCODE code:%08X !!!\n", RC6);

    pBuffer = *pBuf = (PUCHAR) HeapAlloc(GetProcessHeap(), HEAP_ZERO_MEMORY, 1000);
    if (!pBuffer) break;

    //RC6 header
    *pBuffer++ = 0xB6;
    *pBuffer++ = 0x12;
    *pBuffer++ = 0x89;
    *pBuffer++ = 0x12;
    *pBuffer++ = 0x89;
    *pBuffer++ = 0x09;
    *pBuffer++ = 0x89;
    *pBuffer++ = 0x09;
    *pBuffer++ = 0x89;
    *pBuffer++ = 0x12;
    *pBuffer   = 0x92;

    for (i = 16 ; i-- ; )
    {
      if (BIT_TEST(RC6, i))
      {
        if (!LastIsPulse) pBuffer++;
        *pBuffer |= 0x80;
        *pBuffer += 0x09;
        pBuffer++;
        *pBuffer = 0x09;
        LastIsPulse = FALSE;
      }
      else
      {
        if (LastIsPulse) pBuffer++;
        *pBuffer += 0x09;
        pBuffer++;
        *pBuffer = 0x89;
        LastIsPulse = TRUE;
      }
    }
    pBuffer++;

    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;

    memcpy(pBuffer, *pBuf, pBuffer - *pBuf);
    pBuffer += pBuffer - *pBuf;
    memcpy(pBuffer, *pBuf, pBuffer - *pBuf);
    pBuffer += pBuffer - *pBuf;

    *pReadCount = (ULONG) (pBuffer - *pBuf);

    Result = TRUE;

  } while (FALSE);

  return Result;
}

static BOOL MceIrImportProntoRC6A(PUCHAR pData, ULONG Length, PUCHAR *pBuf, PULONG pReadCount)
{
  BOOL Result = FALSE;
  USHORT Proto, Clock, Seq1, Seq2;
  USHORT Customer, System, Command;
  ULONG RC6 = 0;
  PUCHAR pBuffer = NULL;
  //  PUCHAR pData2 = NULL;
  BOOL LastIsPulse = TRUE;
  BYTE BitCount;
  int i;

  do
  {
    //Read 15 bytes of header
    if (Length < 20) break;
    if (sscanf_s((PCHAR)pData, "%04x %04x %04x %04x", &Proto, &Clock, &Seq1, &Seq2) != 4) break;
    if (Proto != PRONTO_PROTO_RC6A) break;
    if (Seq1 + Seq2 != 2) break;

    pData += 20;
    if (sscanf_s((PCHAR)pData, "%04x %04x %04x", &Customer, &System, &Command) != 3) break;
    if (System > 255) break;
    if (Command > 255) break;
    if ((Customer > 127) && (Customer < 32768)) break;

    RC6 = (Customer << 16) | (System << 8) | Command;

    Trace("PLAY_RC6A_KEYCODE code:%08X !!!\n", RC6);

    pBuffer = *pBuf = (PUCHAR) HeapAlloc(GetProcessHeap(), HEAP_ZERO_MEMORY, 1000);
    if (!pBuffer) break;

    //RC6A header
    *pBuffer++ = 0xBF;
    *pBuffer++ = 0x12;
    *pBuffer++ = 0x89;
    *pBuffer++ = 0x09;
    *pBuffer++ = 0x89;
    *pBuffer++ = 0x09;
    *pBuffer++ = 0x89;
    *pBuffer++ = 0x12;
    *pBuffer++ = 0x89;
    *pBuffer++ = 0x12;
    *pBuffer   = 0x92;

    BitCount = (Customer >= 32768) ? 16 : 8;

    for (i = (Customer >= 32768) ? 32 : 24 ; i-- ; )
    {
      if (BIT_TEST(RC6, i))
      {
        if (!LastIsPulse) pBuffer++;
        *pBuffer |= 0x80;
        *pBuffer += 0x09;
        pBuffer++;
        *pBuffer = 0x09;
        LastIsPulse = FALSE;
      }
      else
      {
        if (LastIsPulse) pBuffer++;
        *pBuffer += 0x09;
        pBuffer++;
        *pBuffer = 0x89;
        LastIsPulse = TRUE;
      }
    }
    pBuffer++;

    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;
    *pBuffer++ = 0x7F;

    memcpy(pBuffer, *pBuf, pBuffer - *pBuf);
    pBuffer += pBuffer - *pBuf;
    memcpy(pBuffer, *pBuf, pBuffer - *pBuf);
    pBuffer += pBuffer - *pBuf;

    *pReadCount = (ULONG) (pBuffer - *pBuf);

    Result = TRUE;

  } while (FALSE);

  return Result;
}

*/

    public static bool IsProntoData(byte[] fileBytes)
    {
      if ((fileBytes[0] == '0' && fileBytes[1] == '0' && fileBytes[2] == '0' && fileBytes[3] == '0' && fileBytes[4] == ' ') ||
          (fileBytes[0] == '0' && fileBytes[1] == '1' && fileBytes[2] == '0' && fileBytes[3] == '0' && fileBytes[4] == ' ') ||
          (fileBytes[0] == '5' && fileBytes[1] == '0' && fileBytes[2] == '0' && fileBytes[3] == '0' && fileBytes[4] == ' ') ||
          (fileBytes[0] == '5' && fileBytes[1] == '0' && fileBytes[2] == '0' && fileBytes[3] == '1' && fileBytes[4] == ' ') ||
          (fileBytes[0] == '6' && fileBytes[1] == '0' && fileBytes[2] == '0' && fileBytes[3] == '0' && fileBytes[4] == ' ') ||
          (fileBytes[0] == '6' && fileBytes[1] == '0' && fileBytes[2] == '0' && fileBytes[3] == '1' && fileBytes[4] == ' '))
      {
        return true;
      }
      else
      {
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

    #endregion Public Methods

  }

}
