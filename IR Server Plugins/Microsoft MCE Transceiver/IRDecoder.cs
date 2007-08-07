using System;
using System.Collections.Generic;
using System.Text;

namespace MicrosoftMceTransceiver
{

  #region Enumerations

  /// <summary>
  /// Protocol of IR Code.
  /// </summary>
  public enum IRProtocol
  {
    None,
    //ITT,
    JVC,
    NEC,
    //NRC17,
    RC5,
    RC6,
    RCA,
    //RCMM,
    RECS80,
    //Sharp,
    SIRC,
    //XSAT,
  }

  #endregion Enumerations

  #region Delegates

  public delegate void RemoteCallback(IRProtocol codeType, uint keyCode);
  public delegate void KeyboardCallback(uint keyCode, uint modifiers);
  public delegate void MouseCallback(int deltaX, int deltaY, bool rightButton, bool leftButton);

  #endregion Delegates

  /// <summary>
  /// Used for decoding received IR codes.
  /// </summary>
  public static class IRDecoder
  {

    #region Constants

    public const uint PulseBit  = 0x01000000;
    public const uint PulseMask = 0x00FFFFFF;

    //const UInt16 ToggleBitMce   = 0x8000;
    const UInt16 ToggleMaskMce  = 0x7FFF;
    const UInt16 CustomerMce    = 0x800F;

    const UInt16 ToggleMaskRC5  = 0xF7FF;
    const UInt16 ToggleMaskRC5X = 0xFFFF;

    const uint PrefixRC6        = 0x000FC950;
    const uint PrefixRC6A       = 0x000FCA90;

    const uint MceMouse         = 1;
    const uint MceKeyboard      = 4;

    #endregion Constants

    #region Members

    //static bool _processITT = true;
    static bool _processJVC = true;
    static bool _processNEC = true;
    //static bool _processNRC17 = true;
    static bool _processRC5 = true;
    static bool _processRC6 = true;
    static bool _processRCA = true;
    //static bool _processRCMM = true;
    static bool _processRECS80 = true;
    //static bool _processSharp = true;
    static bool _processSIRC = true;
    //static bool _processXSAT = true;

    static bool _processMCE = true;

    #endregion Members

    #region Properties

    public static bool ProcessJVC
    {
      get { return _processJVC; }
      set { _processJVC = value; }
    }
    public static bool ProcessNEC
    {
      get { return _processNEC; }
      set { _processNEC = value; }
    }
    public static bool ProcessRC5
    {
      get { return _processRC5; }
      set { _processRC5 = value; }
    }
    public static bool ProcessRC6
    {
      get { return _processRC6; }
      set { _processRC6 = value; }
    }
    public static bool ProcessRCA
    {
      get { return _processRCA; }
      set { _processRCA = value; }
    }
    public static bool ProcessRECS80
    {
      get { return _processRECS80; }
      set { _processRECS80 = value; }
    }
    public static bool ProcessSIRC
    {
      get { return _processSIRC; }
      set { _processSIRC = value; }
    }

    public static bool ProcessMCE
    {
      get { return _processMCE; }
      set { _processMCE = value; }
    }

    #endregion Properties

    #region Detection Data

    static RemoteDetectionData JVC_Data     = null;
    static RemoteDetectionData NEC_Data     = null;
    static RemoteDetectionData RC5_Data     = null;
    static RemoteDetectionData RC6_Data     = null;
    static RemoteDetectionData RCA_Data     = null;
    static RemoteDetectionData RECS80_Data  = null;
    static RemoteDetectionData SIRC_Data    = null;

    static MceDetectionData MCE_Data        = null;

    #endregion Detection Data

    #region Methods

    /// <summary>
    /// Decode timing data to discover IR Protocol and button code.
    /// </summary>
    /// <param name="timingData">Input timing data.</param>
    /// <param name="remoteCallback">Method to call when Remote button decoded.</param>
    /// <param name="keyboardCallback">Method to call when Keyboard event decoded.</param>
    /// <param name="mouseCallback">Method to call when Mouse event decoded.</param>
    public static void DecodeIR(uint[] timingData, RemoteCallback remoteCallback, KeyboardCallback keyboardCallback, MouseCallback mouseCallback)
    {
//    if (_processITT)    DetectITT(timingData, remoteCallback);
      if (_processJVC)    DetectJVC(timingData, remoteCallback);
      if (_processNEC)    DetectNEC(timingData, remoteCallback);
//    if (_processNRC17)  DetectNRC17(timingData, remoteCallback);
      if (_processRC5)    DetectRC5(timingData, remoteCallback);
      if (_processRC6)    DetectRC6(timingData, remoteCallback);
      if (_processRCA)    DetectRCA(timingData, remoteCallback);
//    if (_processRCMM)   DetectRCMM(timingData, remoteCallback);
      if (_processRECS80) DetectRECS80(timingData, remoteCallback);
//    if (_processSharp)  DetectSharp(timingData, remoteCallback);
      if (_processSIRC)   DetectSIRC(timingData, remoteCallback); // 15 Bit
//    if (_processXSAT)   DetectXSAT(timingData, remoteCallback);

      if (_processMCE)
        DetectMCE(timingData, keyboardCallback, mouseCallback);
    }

    static void DetectJVC(uint[] timingData, RemoteCallback remoteCallback)
    {
      if (JVC_Data == null || timingData == null)
        JVC_Data = new RemoteDetectionData();

      if (timingData == null)
        return;

      for (int i = 0; i < timingData.Length; i++)
      {
        uint duration = timingData[i] & PulseMask;
        bool pulse = ((timingData[i] & PulseBit) != 0);
        bool ignored = true;

        switch (JVC_Data.State)
        {

          #region HeaderPulse
          case RemoteDetectionState.HeaderPulse:
            //Console.WriteLine("JVC HeaderPulse");

            if (pulse && duration >= 8200 && duration <= 8600)
            {
              JVC_Data.State = RemoteDetectionState.HeaderSpace;
              ignored = false;
            }
            break;
          #endregion HeaderPulse

          #region HeaderSpace
          case RemoteDetectionState.HeaderSpace:
            //Console.WriteLine("JVC HeaderSpace");

            if (!pulse && duration >= 4000 && duration <= 4400)
            {
              JVC_Data.State = RemoteDetectionState.Data;
              JVC_Data.HalfBit = 0;
              JVC_Data.Bit = 0;
              JVC_Data.Code = 0;
              ignored = false;
            }
            break;
          #endregion HeaderSpace

          #region Data
          case RemoteDetectionState.Data:
            //Console.WriteLine("JVC Data");

            if (pulse && duration >= 350 && duration <= 750)
            {
              JVC_Data.HalfBit = 1;
              ignored = false;
            }
            else if (!pulse && duration >= 250 && duration <= 650 && JVC_Data.HalfBit == 1)
            {
              JVC_Data.Code <<= 1;
              JVC_Data.Bit++;
              JVC_Data.HalfBit = 0;
              ignored = false;
            }
            else if (!pulse && duration >= 1450 && duration <= 1850 && JVC_Data.HalfBit == 1)
            {
              JVC_Data.Code <<= 1;
              JVC_Data.Code |= 1;
              JVC_Data.Bit++;
              JVC_Data.HalfBit = 0;
              ignored = false;
            }
            else
            {
              //Console.WriteLine("JVC Error");
            }

            if (JVC_Data.Bit == 16)
            {
              remoteCallback(IRProtocol.JVC, JVC_Data.Code);
              JVC_Data.State = RemoteDetectionState.Leading;
            }
            break;
          #endregion Data

          #region Leading
          case RemoteDetectionState.Leading:
            //Console.WriteLine("JVC Leading");

            if (pulse && duration >= 350 && duration <= 750)
            {
              JVC_Data = new RemoteDetectionData();
              JVC_Data.State = RemoteDetectionState.Data;
              ignored = false;
            }
            break;
          #endregion Leading

        }

        if (ignored && (JVC_Data.State != RemoteDetectionState.HeaderPulse))
          JVC_Data = new RemoteDetectionData();
      }
    }
    static void DetectNEC(uint[] timingData, RemoteCallback remoteCallback)
    {
      if (NEC_Data == null || timingData == null)
        NEC_Data = new RemoteDetectionData();

      if (timingData == null)
        return;

      for (int i = 0; i < timingData.Length; i++)
      {
        uint duration = timingData[i] & PulseMask;
        bool pulse = ((timingData[i] & PulseBit) != 0);
        bool ignored = true;

        switch (NEC_Data.State)
        {

          #region HeaderPulse
          case RemoteDetectionState.HeaderPulse:
            //Console.WriteLine("NEC HeaderPulse");

            if (pulse && duration >= 8800 && duration <= 9200)
            {
              NEC_Data.State = RemoteDetectionState.HeaderSpace;
              ignored = false;
            }
            break;
          #endregion HeaderPulse

          #region HeaderSpace
          case RemoteDetectionState.HeaderSpace:
            //Console.WriteLine("NEC HeaderSpace");

            if (!pulse && duration >= 4300 && duration <= 4700)
            {
              NEC_Data.State = RemoteDetectionState.Data;
              NEC_Data.HalfBit = 0;
              NEC_Data.Bit = 0;
              NEC_Data.Code = 0;
              ignored = false;
            }
            else if (!pulse && duration >= 2050 && duration <= 2450) // For Repeats
            {
              remoteCallback(IRProtocol.NEC, NEC_Data.Code);
              NEC_Data.State = RemoteDetectionState.HeaderPulse;
              ignored = false;
            }

            break;
          #endregion HeaderSpace

          #region Data
          case RemoteDetectionState.Data:
            //Console.WriteLine("NEC Data");

            if (pulse && duration >= 350 && duration <= 750)
            {
              NEC_Data.HalfBit = 1;
              ignored = false;
            }
            else if (!pulse && duration >= 250 && duration <= 650 && NEC_Data.HalfBit == 1)
            {
              NEC_Data.Code <<= 1;
              NEC_Data.Bit++;
              NEC_Data.HalfBit = 0;
              ignored = false;
            }
            else if (!pulse && duration >= 1550 && duration <= 1950 && NEC_Data.HalfBit == 1)
            {
              NEC_Data.Code <<= 1;
              NEC_Data.Code |= 1;
              NEC_Data.Bit++;
              NEC_Data.HalfBit = 0;
              ignored = false;
            }
            else
            {
              //Console.WriteLine("NEC Error");
            }

            if (NEC_Data.Bit == 32)
            {
              remoteCallback(IRProtocol.NEC, NEC_Data.Code);
              NEC_Data.State = RemoteDetectionState.HeaderPulse;
            }
            break;
          #endregion Data

        }

        if (ignored && (NEC_Data.State != RemoteDetectionState.HeaderPulse))
          NEC_Data = new RemoteDetectionData();
      }
    }
    static void DetectRC5(uint[] timingData, RemoteCallback remoteCallback)
    {
      if (RC5_Data == null || timingData == null)
        RC5_Data = new RemoteDetectionData();

      if (timingData == null)
        return;

      for (int i = 0; i < timingData.Length; i++)
      {
        uint duration = timingData[i] & PulseMask;
        bool pulse = ((timingData[i] & PulseBit) != 0);
        bool ignored = true;

        switch (RC5_Data.State)
        {

          #region HeaderPulse
          case RemoteDetectionState.HeaderPulse:
            //Console.WriteLine("RC5 HeaderPulse");

            if (pulse)
            {
              if ((duration >= 750) && (duration <= 1100))
              {
                RC5_Data.State = RemoteDetectionState.HeaderSpace;
                RC5_Data.Bit = 13;
                RC5_Data.Code = (uint)1 << RC5_Data.Bit;
                ignored = false;
              }
              else if ((duration >= 1500) && (duration <= 2000))
              {
                RC5_Data.State = RemoteDetectionState.Data;
                RC5_Data.Bit = 13;
                RC5_Data.Code = (uint)1 << RC5_Data.Bit;
                RC5_Data.HalfBit = 0;
                ignored = false;
              }
            }
            break;
          #endregion HeaderPulse

          #region HeaderSpace
          case RemoteDetectionState.HeaderSpace:
            //Console.WriteLine("RC5 HeaderSpace");

            if (!pulse && (duration >= 750) && (duration <= 1000))
            {
              RC5_Data.State = RemoteDetectionState.Data;
              RC5_Data.HalfBit = 0;
              ignored = false;
            }
            break;
          #endregion HeaderSpace

          #region Data
          case RemoteDetectionState.Data:
            //Console.WriteLine("RC5 Data");

            if (RC5_Data.HalfBit == 0)
            {
              if (pulse)
              {
                if (((duration >= 750) && (duration <= 1100)) || ((duration >= 1500) && (duration <= 2000)))
                {
                  RC5_Data.HalfBit = (byte)((duration >= 1500) ? 0 : 1);
                  RC5_Data.Bit--;
                  RC5_Data.Code |= (uint)1 << RC5_Data.Bit;
                  ignored = false;

                  if ((RC5_Data.Bit == 0) || ((RC5_Data.Bit == 1) && (duration >= 1500)))
                    RC5_Data.State = RemoteDetectionState.KeyCode;
                }
                else
                {
                  //Console.WriteLine("RC5 Error  {0} on bit {1}", duration, bit);
                }
              }
              else
              {
                if (((duration >= 750) && (duration <= 1100)) || ((duration >= 1500) && (duration <= 2000)))
                {
                  RC5_Data.HalfBit = (byte)((duration >= 1500) ? 0 : 1);
                  RC5_Data.Bit--;
                  ignored = false;

                  if (RC5_Data.Bit == 0)
                    RC5_Data.State = RemoteDetectionState.KeyCode;
                }
                else if ((RC5_Data.Bit == 7) && (((duration >= 4300) && (duration <= 4700)) || ((duration >= 5200) && (duration <= 5600))))
                {
                  ignored = false;
                  RC5_Data.HalfBit = (byte)((duration >= 5200) ? 0 : 1);
                  RC5_Data.Code <<= 6;
                  RC5_Data.Bit += 5;
                }
                else
                {
                  //Console.WriteLine("RC5 Space Error  {0} on bit {1}", duration, bit);
                }
              }
              break;
            }

            if ((duration >= 750) && (duration <= 1100))
            {
              RC5_Data.HalfBit = 0;
              ignored = false;

              if ((RC5_Data.Bit == 1) && pulse)
                RC5_Data.State = RemoteDetectionState.KeyCode;
            }
            else if ((RC5_Data.Bit == 7) && (((duration >= 3400) && (duration <= 3800)) || ((duration >= 4300) && (duration <= 4700))))
            {
              RC5_Data.HalfBit = (byte)((duration >= 4300) ? 0 : 1);
              RC5_Data.Code <<= 6;
              RC5_Data.Bit += 6;
              ignored = false;
            }
            else
            {
              //Console.WriteLine("RC5 Duration Error  {0} on bit {1}", duration, bit);
            }
            break;
          #endregion Data

          #region Leading
          case RemoteDetectionState.Leading:
            //Console.WriteLine("RC5 Leading");

            if (pulse)
              break;

            if (duration > 10000)
            {
              RC5_Data.State = RemoteDetectionState.HeaderPulse;
              ignored = false;
            }
            break;
          #endregion Leading

        }

        if (RC5_Data.State == RemoteDetectionState.KeyCode)
        {
          if (RC5_Data.Code > 0xFFFF)
            RC5_Data.Code &= ToggleMaskRC5X;
          else
            RC5_Data.Code &= ToggleMaskRC5;

          remoteCallback(IRProtocol.RC5, RC5_Data.Code);

          RC5_Data.State = RemoteDetectionState.HeaderPulse;
        }

        if (ignored && (RC5_Data.State != RemoteDetectionState.Leading) && (RC5_Data.State != RemoteDetectionState.HeaderPulse))
          RC5_Data.State = RemoteDetectionState.HeaderPulse;
      }

    }
    static void DetectRC6(uint[] timingData, RemoteCallback remoteCallback)
    {
      if (RC6_Data == null || timingData == null)
        RC6_Data = new RemoteDetectionData();

      if (timingData == null)
        return;

      for (int i = 0; i < timingData.Length; i++)
      {
        uint duration = timingData[i] & PulseMask;
        bool pulse = ((timingData[i] & PulseBit) != 0);
        bool ignored = true;

        switch (RC6_Data.State)
        {

          #region HeaderPulse
          case RemoteDetectionState.HeaderPulse:
            //Console.WriteLine("RC6 HeaderPulse");

            if (pulse && (duration >= 2600) && (duration <= 3300))
            {
              RC6_Data.State = RemoteDetectionState.HeaderSpace;
              RC6_Data.Header = 0x000FC000;
              RC6_Data.Bit = 14;
              RC6_Data.HalfBit = 0;
              RC6_Data.Code = 0;
              RC6_Data.LongPulse = false;
              RC6_Data.LongSpace = false;
              ignored = false;
            }
            break;
          #endregion HeaderPulse

          #region HeaderSpace
          case RemoteDetectionState.HeaderSpace:
            //Console.WriteLine("RC6 HeaderSpace");

            if (!pulse && (duration >= 750) && (duration <= 1000))
            {
              RC6_Data.State = RemoteDetectionState.PreData;
              RC6_Data.Bit -= 2;
              ignored = false;
            }
            break;
          #endregion HeaderSpace

          #region PreData
          case RemoteDetectionState.PreData:
            //Console.WriteLine("RC6 PreData");

            if (pulse)
            {
              if ((duration >= 350) && (duration <= 600))
              {
                ignored = false;
                if (RC6_Data.Bit != 0) RC6_Data.Header |= (uint)(1 << --RC6_Data.Bit);
              }
              else if ((duration >= 750) && (duration <= 1000))
              {
                ignored = false;
                if (RC6_Data.Bit != 0) RC6_Data.Header |= (uint)(1 << --RC6_Data.Bit);
                if (RC6_Data.Bit != 0) RC6_Data.Header |= (uint)(1 << --RC6_Data.Bit);
              }
              else if ((duration >= 1200) && (duration <= 1600))
              {
                ignored = false;
                if (RC6_Data.Bit != 0) RC6_Data.Header |= (uint)(1 << --RC6_Data.Bit);
                if (RC6_Data.Bit != 0) RC6_Data.Header |= (uint)(1 << --RC6_Data.Bit);
                if (RC6_Data.Bit != 0) RC6_Data.Header |= (uint)(1 << --RC6_Data.Bit);
                else
                {
                  RC6_Data.HalfBit = 1;
                  RC6_Data.LongPulse = true;
                }
              }
              else
              {
                //Console.WriteLine(string.Format("RC6 Error Bit {0} {1} {2}", bit, pulse ? "Pulse" : "Space", duration));
              }
            }
            else
            {
              if ((duration >= 300) && (duration <= 600))
              {
                RC6_Data.Bit--;
                ignored = false;
              }
              else if ((duration >= 750) && (duration <= 1000))
              {
                ignored = false;
                if (RC6_Data.Bit > 2)
                  RC6_Data.Bit -= 2;
                else
                  RC6_Data.Bit = 0;
              }
              else if ((duration >= 1200) && (duration <= 1600))
              {
                ignored = false;
                if (RC6_Data.Bit >= 3)
                {
                  RC6_Data.Bit -= 3;
                }
                else
                {
                  RC6_Data.HalfBit = 1;
                  RC6_Data.LongPulse = true;
                  RC6_Data.Bit = 0;
                }
              }
              else
              {
                //Console.WriteLine(string.Format("RC6 Error Bit {0} {1} {2}", bit, pulse ? "Pulse" : "Space", duration));
              }
            }

            if ((ignored == false) && (RC6_Data.Bit == 0))
            {
              if ((RC6_Data.Header & 0xFFFFFFF0) == PrefixRC6)
              {
                RC6_Data.Bit = 16;
              }
              else if ((RC6_Data.Header & 0xFFFFFFF0) == PrefixRC6A)
              {
                RC6_Data.Bit = 32;
              }
              else
              {
                ignored = true;
                break;
              }

              RC6_Data.State = RemoteDetectionState.Data;
            }
            break;
          #endregion PreData

          #region Data
          case RemoteDetectionState.Data:
            //Console.WriteLine("RC6 Data");

            if ((RC6_Data.HalfBit % 2) == 0)
            {
              if (pulse && (duration >= 350) && (duration <= 600))
              {
                ignored = false;
                RC6_Data.LongPulse = true;
                RC6_Data.HalfBit++;

                if (RC6_Data.Bit == 1)
                  RC6_Data.State = RemoteDetectionState.KeyCode;
              }
              else if (!pulse && (duration >= 300) && (duration <= 600))
              {
                ignored = false;
                RC6_Data.LongSpace = true;
                RC6_Data.HalfBit++;
              }
              else
              {
                //Console.WriteLine(string.Format("RC6 Error Halfbit0 {0} {1}", pulse ? "Pulse" : "Space", duration));
              }
              break;
            }

            if (RC6_Data.LongPulse)
            {
              RC6_Data.LongPulse = false;
              if (pulse)
              {
                //Console.WriteLine(string.Format("RC6 Error Pulse after LongPulse {0} {1}", pulse ? "Pulse" : "Space", duration));
                break;
              }

              if ((duration >= 750) && (duration <= 1000))
              {
                RC6_Data.Bit--;
                RC6_Data.LongSpace = true;
                RC6_Data.HalfBit += 2;
                ignored = false;
              }
              else if ((duration >= 300) && (duration <= 600))
              {
                RC6_Data.Bit--;
                RC6_Data.HalfBit++;
                ignored = false;
              }
              else
              {
                //Console.WriteLine(string.Format("RC6 Error Pulse LongPulse {0} {1}", pulse ? "Pulse" : "Space", duration));
              }
            }
            else if (RC6_Data.LongSpace)
            {
              RC6_Data.LongSpace = false;

              if (!pulse)
              {
                //Console.WriteLine(string.Format("RC6 Error Pulse after LongPulse {0} {1}", pulse ? "Pulse" : "Space", duration));
                break;
              }

              if (RC6_Data.Bit == 32)
                RC6_Data.Bit = 24;

              if ((duration >= 750) && (duration <= 1000))
              {
                RC6_Data.Bit--;
                RC6_Data.Code |= (uint)1 << RC6_Data.Bit;
                RC6_Data.LongPulse = true;
                RC6_Data.HalfBit += 2;
                ignored = false;

                if (RC6_Data.Bit == 1)
                  RC6_Data.State = RemoteDetectionState.KeyCode;
              }
              else if ((duration >= 350) && (duration <= 600))
              {
                RC6_Data.Bit--;
                RC6_Data.Code |= (uint)1 << RC6_Data.Bit;
                RC6_Data.HalfBit++;
                ignored = false;

                if (RC6_Data.Bit == 0)
                  RC6_Data.State = RemoteDetectionState.KeyCode;
              }
              else
              {
                //Console.WriteLine(string.Format("RC6 Error LongPulse {0} {1}", pulse ? "Pulse" : "Space", duration));
              }
            }
            break;
          #endregion Data

        }

        if (RC6_Data.State == RemoteDetectionState.KeyCode)
        {
          if ((~RC6_Data.Code >> 16) == CustomerMce)
            RC6_Data.Code &= ToggleMaskMce;

          remoteCallback(IRProtocol.RC6, RC6_Data.Code);

          RC6_Data.State = RemoteDetectionState.HeaderPulse;
        }

        if (ignored && (RC6_Data.State != RemoteDetectionState.HeaderPulse))
          RC6_Data.State = RemoteDetectionState.HeaderPulse;
      }

    }
    static void DetectRCA(uint[] timingData, RemoteCallback remoteCallback)
    {
      if (RCA_Data == null || timingData == null)
        RCA_Data = new RemoteDetectionData();

      if (timingData == null)
        return;

      for (int i = 0; i < timingData.Length; i++)
      {
        uint duration = timingData[i] & PulseMask;
        bool pulse = ((timingData[i] & PulseBit) != 0);
        bool ignored = true;

        switch (RCA_Data.State)
        {

          #region HeaderPulse
          case RemoteDetectionState.HeaderPulse:
            //Console.WriteLine("RCA HeaderPulse");

            if (pulse && duration >= 3800 && duration <= 4200)
            {
              RCA_Data.State = RemoteDetectionState.HeaderSpace;
              ignored = false;
            }
            break;
          #endregion HeaderPulse

          #region HeaderSpace
          case RemoteDetectionState.HeaderSpace:
            //Console.WriteLine("RCA HeaderSpace");

            if (!pulse && duration >= 3800 && duration <= 4200)
            {
              RCA_Data.State = RemoteDetectionState.Data;
              RCA_Data.HalfBit = 0;
              RCA_Data.Bit = 0;
              RCA_Data.Code = 0;
              ignored = false;
            }
            break;
          #endregion HeaderSpace

          #region Data
          case RemoteDetectionState.Data:
            //Console.WriteLine("RCA Data");

            if (pulse && duration >= 300 && duration <= 700)
            {
              RCA_Data.HalfBit = 1;
              ignored = false;
            }
            else if (!pulse && duration >= 800 && duration <= 1250 && RCA_Data.HalfBit == 1)
            {
              RCA_Data.Code <<= 1;
              RCA_Data.Bit++;
              RCA_Data.HalfBit = 0;
              ignored = false;
            }
            else if (!pulse && duration >= 1800 && duration <= 2200 && RCA_Data.HalfBit == 1)
            {
              RCA_Data.Code <<= 1;
              RCA_Data.Code |= 1;
              RCA_Data.Bit++;
              RCA_Data.HalfBit = 0;
              ignored = false;
            }
            else
            {
              //Console.WriteLine("RCA Error");
            }

            if (RCA_Data.Bit == 12)
            {
              remoteCallback(IRProtocol.RCA, RCA_Data.Code);
              RCA_Data.State = RemoteDetectionState.HeaderPulse;
            }
            break;
          #endregion Data

        }

        if (ignored && (RCA_Data.State != RemoteDetectionState.HeaderPulse))
          RCA_Data.State = RemoteDetectionState.HeaderPulse;
      }
    }
    static void DetectRECS80(uint[] timingData, RemoteCallback remoteCallback)
    {
      if (RECS80_Data == null || timingData == null)
        RECS80_Data = new RemoteDetectionData();

      if (timingData == null)
        return;

      for (int i = 0; i < timingData.Length; i++)
      {
        uint duration = timingData[i] & PulseMask;
        bool pulse = ((timingData[i] & PulseBit) != 0);
        bool ignored = true;

        switch (RECS80_Data.State)
        {

          #region HeaderPulse
          case RemoteDetectionState.HeaderPulse:
            //Console.WriteLine("RECS80 HeaderPulse");

            if (pulse && (duration >= 3300) && (duration <= 4100))
            {
              RECS80_Data.State = RemoteDetectionState.HeaderSpace;
              ignored = false;
            }
            break;
          #endregion HeaderPulse

          #region HeaderSpace
          case RemoteDetectionState.HeaderSpace:
            //Console.WriteLine("RECS80 HeaderSpace");

            if (!pulse && (duration >= 1400) && (duration <= 1800))
            {
              RECS80_Data.State = RemoteDetectionState.Data;
              RECS80_Data.HalfBit = 0;
              RECS80_Data.Bit = 48;
              RECS80_Data.Header = 0;
              RECS80_Data.Code = 0;
              ignored = false;
            }
            break;
          #endregion HeaderSpace

          #region Data
          case RemoteDetectionState.Data:
            //Console.WriteLine("RECS80 Data");

            if ((RECS80_Data.HalfBit % 2) == 0)
            {
              if (!pulse)
                break;

              if ((duration >= 350) && (duration <= 600))
              {
                RECS80_Data.HalfBit = 1;
                RECS80_Data.Bit--;
                ignored = false;
              }
              break;
            }
            else
            {
              if (pulse) 
                break;

              if ((duration >= 400) && (duration <= 750))
              {
                RECS80_Data.HalfBit = 0;
                ignored = false;
              }
              else if ((duration >= 1100) && (duration <= 1500))
              {
                RECS80_Data.HalfBit = 0;
                if (RECS80_Data.Bit > 15)
                  RECS80_Data.Header |= (uint)(1 << (RECS80_Data.Bit - 16));
                else
                  RECS80_Data.Code |= (uint)(1 << RECS80_Data.Bit);
                ignored = false;
              }
              else
              {
                break;
              }

              if (RECS80_Data.Bit == 0)
              {
                RECS80_Data.Code &= 0x0000FFFF;
                remoteCallback(IRProtocol.RECS80, RECS80_Data.Code);

                RECS80_Data.State = RemoteDetectionState.HeaderPulse;
              }
            }
            break;
          #endregion Data

        }

        if (ignored && (RECS80_Data.State != RemoteDetectionState.HeaderPulse))
          RECS80_Data.State = RemoteDetectionState.HeaderPulse;
      }
    }
    static void DetectSIRC(uint[] timingData, RemoteCallback remoteCallback)
    {
      if (SIRC_Data == null || timingData == null)
        SIRC_Data = new RemoteDetectionData();

      if (timingData == null)
        return;

      for (int i = 0; i < timingData.Length; i++)
      {
        uint duration = timingData[i] & PulseMask;
        bool pulse = ((timingData[i] & PulseBit) != 0);
        bool ignored = true;

        switch (SIRC_Data.State)
        {

          #region HeaderPulse
          case RemoteDetectionState.HeaderPulse:
            //Console.WriteLine("SIRC HeaderPulse");

            if (pulse && duration >= 2200 && duration <= 2600)
            {
              SIRC_Data.State = RemoteDetectionState.HeaderSpace;
              ignored = false;
            }
            break;
          #endregion HeaderPulse

          #region HeaderSpace
          case RemoteDetectionState.HeaderSpace:
            //Console.WriteLine("SIRC HeaderSpace");

            if (!pulse && duration >= 400 && duration <= 800)
            {
              SIRC_Data.State = RemoteDetectionState.Data;
              SIRC_Data.Bit = 0;
              SIRC_Data.Code = 0;
              ignored = false;
            }
            break;
          #endregion HeaderSpace

          #region Data
          case RemoteDetectionState.Data:
            //Console.WriteLine("SIRC Data");

            if (pulse && duration >= 400 && duration <= 800)
            {
              SIRC_Data.Code <<= 1;
              SIRC_Data.Bit++;
              ignored = false;
            }
            else if (pulse && duration >= 1000 && duration <= 1400)
            {
              SIRC_Data.Code <<= 1;
              SIRC_Data.Code |= 1;
              SIRC_Data.Bit++;
              ignored = false;
            }
            else if (!pulse && duration >= 400 && duration <= 800)
            {
              ignored = false;
            }
            else
            {
              //Console.WriteLine("SIRC Error");
            }

            if (SIRC_Data.Bit == 15)
            {
              remoteCallback(IRProtocol.SIRC, SIRC_Data.Code);
              SIRC_Data.State = RemoteDetectionState.HeaderPulse;
            }
            break;
          #endregion Data

        }

        if (ignored && (SIRC_Data.State != RemoteDetectionState.HeaderPulse))
          SIRC_Data.State = RemoteDetectionState.HeaderPulse;
      }

    }

    //static uint biggest = 0;
    //static uint smallest = 1000000;

    static void DetectMCE(uint[] timingData, KeyboardCallback keyboardCallback, MouseCallback mouseCallback)
    {
      // Mouse:    0 0001 xxxxxxxxxxxxxxxxxxxxxxxxxxxxx
      // Keyboard: 0 0100 xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

      const int HalfBit_None = 0;
      const int HalfBit_Zero = 1;
      const int HalfBit_One = 2;

      if (MCE_Data == null || timingData == null)
        MCE_Data = new MceDetectionData();

      if (timingData == null)
        return;

      for (int i = 0; i < timingData.Length; i++)
      {
        uint duration = timingData[i] & PulseMask;
        bool pulse = ((timingData[i] & PulseBit) != 0);

        #region Working data ...
        if (MCE_Data.State != MceKeyboardDetectState.Header)
        {
          switch (MCE_Data.HalfBit)
          {
            case HalfBit_None:
              if (duration >= 100 && duration <= 450)
                MCE_Data.HalfBit = (pulse ? 2 : 1);
              else if (duration >= 500 && duration <= 800)
              {
                //Console.WriteLine("Bad bit sequence double {0}", pulse);
                MCE_Data.HalfBit = (pulse ? 2 : 1);
                //MCE_Data = new MceDetectionData();
                return;
              }
              else
              {
                // Over Length duration (Treat as a Zero bit)
                //Console.WriteLine("Bad duration {0}", duration);
                MCE_Data.Working <<= 1;
                MCE_Data.Bit--;
              }
              break;

            case HalfBit_Zero:
              if (duration >= 100 && duration <= 450)
              {
                if (pulse)
                {
                  MCE_Data.Working <<= 1;
                  MCE_Data.Bit--;
                  MCE_Data.HalfBit = 0;
                }
                else
                {
                  //Console.WriteLine("Bad bit sequence 00");
                  MCE_Data = new MceDetectionData();
                  //return;
                }
              }
              else if (duration >= 500 && duration <= 800)
              {
                if (pulse)
                {
                  MCE_Data.Working <<= 1;
                  MCE_Data.Bit--;
                  MCE_Data.HalfBit = 2;
                }
                else
                {
                  //Console.WriteLine("Bad bit sequence 00 0");
                  MCE_Data = new MceDetectionData();
                  //return;
                }
              }
              else
              {
                //Console.WriteLine("Bad duration {0}", duration);
                if (MCE_Data.Bit == 1)
                {
                  MCE_Data.Working <<= 1;
                  //MceKeyboard_Data.Working |= 1;
                  MCE_Data.Bit--;
                  MCE_Data.HalfBit = 0;
                  //i--;
                }
                else
                  MCE_Data = new MceDetectionData();
                //return;
              }
              break;

            case HalfBit_One:
              if (duration >= 100 && duration <= 450)
              {
                if (!pulse)
                {
                  MCE_Data.Working <<= 1;
                  MCE_Data.Working |= 1;
                  MCE_Data.Bit--;
                  MCE_Data.HalfBit = 0;
                }
                else
                {
                  //Console.WriteLine("Bad bit sequence 11");
                  MCE_Data = new MceDetectionData();
                  //return;
                }
              }
              else if (duration >= 500 && duration <= 800)
              {
                if (!pulse)
                {
                  MCE_Data.Working <<= 1;
                  MCE_Data.Working |= 1;
                  MCE_Data.Bit--;
                  MCE_Data.HalfBit = 1;
                }
                else
                {
                  //Console.WriteLine("Bad bit sequence 11 1");
                  MCE_Data = new MceDetectionData();
                  //return;
                }
              }
              else
              {
                //Console.WriteLine("Bad duration {0}", duration);
                if (MCE_Data.Bit == 1)
                {
                  MCE_Data.Working <<= 1;
                  MCE_Data.Working |= 1;
                  MCE_Data.Bit--;
                  MCE_Data.HalfBit = 0;
                  //i--;
                }
                else
                  MCE_Data = new MceDetectionData();
                //return;
              }
              break;
          }
        }
        #endregion Working data ...

        switch (MCE_Data.State)
        {

          #region Header
          case MceKeyboardDetectState.Header:
            //Console.WriteLine("KB Header");

            if (pulse && (duration >= 2600) && (duration <= 3300))
            {
              MCE_Data.State = MceKeyboardDetectState.CodeType;
              MCE_Data.Type = 0;
              MCE_Data.Bit = 5;
              MCE_Data.Working = 0;
            }
            break;
          #endregion Header

          #region CodeType
          case MceKeyboardDetectState.CodeType:
            //Console.WriteLine("KB CodeType");

            if (MCE_Data.Bit == 0)
            {
              MCE_Data.Type = MCE_Data.Working;

              if (MCE_Data.Type == MceKeyboard)
              {
                MCE_Data.State = MceKeyboardDetectState.KeyboardIgnore;
                MCE_Data.Bit = 16;
                MCE_Data.Working = 0;
              }
              else if (MCE_Data.Type == MceMouse)
              {
                MCE_Data.State = MceKeyboardDetectState.MouseIgnore;
                MCE_Data.Bit = 8;
                MCE_Data.Working = 0;
              }
              else
              {
                //Console.WriteLine("KB: Invalid Type {0}", MceKeyboard_Data.Type);
                return;
              }
            }

            break;
          #endregion CodeType


          #region Keyboard

          #region KeyboardIgnore
          case MceKeyboardDetectState.KeyboardIgnore:
            //Console.WriteLine("KB KeyboardIgnore");

            if (MCE_Data.Bit == 0)
            {
              MCE_Data.State = MceKeyboardDetectState.KeyCode;
              MCE_Data.Bit = 8;
              MCE_Data.Working = 0;
            }
            break;
          #endregion KeyboardIgnore

          #region KeyCode
          case MceKeyboardDetectState.KeyCode:
            //Console.WriteLine("KB KeyCode");

            if (MCE_Data.Bit == 0)
            {
              MCE_Data.KeyCode = MCE_Data.Working;

              MCE_Data.State = MceKeyboardDetectState.Modifiers;
              MCE_Data.Bit = 8;
              MCE_Data.Working = 0;
            }
            break;
          #endregion KeyCode

          #region Modifiers
          case MceKeyboardDetectState.Modifiers:
            //Console.WriteLine("KB Modifiers");

            if (MCE_Data.Bit == 0)
            {
              MCE_Data.Modifiers = MCE_Data.Working;
              
              keyboardCallback(MCE_Data.KeyCode, MCE_Data.Modifiers);
              
              MCE_Data = new MceDetectionData();
            }
            break;
          #endregion Modifiers

          #endregion Keyboard

          #region Mouse

          #region MouseIgnore
          case MceKeyboardDetectState.MouseIgnore:
            //Console.WriteLine("KB MouseIgnore");

            if (MCE_Data.Bit == 0)
            {
              MCE_Data.State = MceKeyboardDetectState.DeltaY;
              MCE_Data.Bit = 7;
              MCE_Data.Working = 0;
            }
            break;
          #endregion MouseIgnore

          #region DeltaY
          case MceKeyboardDetectState.DeltaY:
            //Console.WriteLine("KB DeltaY");

            if (MCE_Data.Bit == 0)
            {
              //Console.WriteLine("KB DeltaY Set");
              MCE_Data.DeltaY = ScaleMouseDelta(MCE_Data.Working);

              MCE_Data.State = MceKeyboardDetectState.DeltaX;
              MCE_Data.Bit = 7;
              MCE_Data.Working = 0;
            }
            break;
          #endregion DeltaY

          #region DeltaX
          case MceKeyboardDetectState.DeltaX:
            //Console.WriteLine("KB DeltaX");

            if (MCE_Data.Bit == 0)
            {
              //Console.WriteLine("KB DeltaX Set");
              MCE_Data.DeltaX = ScaleMouseDelta(MCE_Data.Working);

              MCE_Data.State = MceKeyboardDetectState.Right;
              MCE_Data.Bit = 1;
              MCE_Data.Working = 0;
            }
            break;
          #endregion DeltaX

          #region Right
          case MceKeyboardDetectState.Right:
            //Console.WriteLine("KB Right");

            if (MCE_Data.Bit == 0)
            {
              //Console.WriteLine("KB Right Set");
              MCE_Data.Right = (MCE_Data.Working == 1);

              MCE_Data.State = MceKeyboardDetectState.Left;
              MCE_Data.Bit = 1;
              MCE_Data.Working = 0;
            }
            break;
          #endregion Right

          #region Left
          case MceKeyboardDetectState.Left:
            //Console.WriteLine("KB Left");

            if (MCE_Data.Bit == 0)
            {
              //Console.WriteLine("KB Left Set");
              MCE_Data.Left = (MCE_Data.Working == 1);

              MCE_Data.State = MceKeyboardDetectState.Checksum;
              MCE_Data.Bit = 5;
              MCE_Data.Working = 0;
            }
            break;
          #endregion Left

          #region Checksum
          case MceKeyboardDetectState.Checksum:
            //Console.WriteLine("KB Checksum");

            if (MCE_Data.Bit == 0)
            {
              //Console.WriteLine("KB Checksum Set");
              mouseCallback(MCE_Data.DeltaX, MCE_Data.DeltaY, MCE_Data.Right, MCE_Data.Left);

              MCE_Data = new MceDetectionData();
           }
            break;
          #endregion Checksum

          #endregion Mouse

        }

        if (MCE_Data.Bit < 0)
          MCE_Data = new MceDetectionData();

      }
    }

    static int ScaleMouseDelta(uint delta)
    {
      int scaledDelta = (int)delta;
      if (delta >= 0x62)
        scaledDelta -= 0x80;

      return scaledDelta;
    }

    #endregion Methods

  }

}
