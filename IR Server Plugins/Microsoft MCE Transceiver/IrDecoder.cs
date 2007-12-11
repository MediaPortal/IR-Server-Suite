using System;
using System.Collections.Generic;
#if TRACE
using System.Diagnostics;
#endif
using System.Text;

namespace MicrosoftMceTransceiver
{

  #region Enumerations

  /// <summary>
  /// Protocol of IR Code.
  /// </summary>
  enum IrProtocol
  {
    /// <summary>
    /// No protocol.
    /// </summary>
    None,

    /// <summary>
    /// Daewoo protocol. 38kHz carrier.
    /// </summary>
    Daewoo,
    /// <summary>
    /// ITT protocol (unsupported).
    /// </summary>
    ITT,
    /// <summary>
    /// JVC protocol. 38kHz carrier.
    /// </summary>
    JVC,
    /// <summary>
    /// Matsushita protocol. 56.8kHz carrier.
    /// </summary>
    Matsushita,
    /// <summary>
    /// Mitsubishi protocol. 40kHz carrier.
    /// </summary>
    Mitsubishi,
    /// <summary>
    /// NEC protocol. 38kHz carrier.
    /// </summary>
    NEC,
    /// <summary>
    /// Nokia NRC17 protocol. 38kHz carrier.
    /// </summary>
    NRC17,
    /// <summary>
    /// Panasonic protocol. 38kHz carrier.
    /// </summary>
    Panasonic,
    /// <summary>
    /// Philips RC5 protocol. 36kHz carrier.
    /// </summary>
    RC5,
    /// <summary>
    /// Philips RC5X protocol. 36kHz carrier.
    /// </summary>
    RC5X,
    /// <summary>
    /// Philips RC6 protocol (Mode 0). 36kHz carrier.
    /// </summary>
    RC6,
    /// <summary>
    /// Philips RC6 protocol (Mode 6A). 36kHz carrier.
    /// </summary>
    RC6A,
    /// <summary>
    /// Microsoft's protocol variation of Philips RC6. 36kHz carrier.
    /// </summary>
    RC6_MCE,
    /// <summary>
    /// Foxtel's protocol variation of Philips RC6. 36kHz carrier.
    /// </summary>
    RC6_Foxtel,
    /// <summary>
    /// RCA protocol. 56kHz carrier.
    /// </summary>
    RCA,
    /// <summary>
    /// Philips RC-MM protocol.  This protocol cannot be reliably (if at all) decoded by the MCE device. 36kHz carrier.
    /// </summary>
    RCMM,
    /// <summary>
    /// RECS-80 protocol. 38kHz carrier.
    /// </summary>
    RECS80,
    /// <summary>
    /// Sharp protocol (unsupported). 38kHz carrier
    /// </summary>
    Sharp,
    /// <summary>
    /// Sony SIRC protocol. 40kHz carrier.
    /// </summary>
    SIRC,
    /// <summary>
    /// Toshiba protocol. 38kHz carrier.
    /// </summary>
    Toshiba,
    /// <summary>
    /// X-Sat protocol (unsupported). 38kHz carrier.
    /// </summary>
    XSAT,

    /// <summary>
    /// Unknown protocol.
    /// </summary>
    Unknown,
  }

  #endregion Enumerations

  #region Delegates

  delegate void RemoteCallback(IrProtocol codeType, uint keyCode, bool firstPress);
  delegate void KeyboardCallback(uint keyCode, uint modifiers);
  delegate void MouseCallback(int deltaX, int deltaY, bool rightButton, bool leftButton);

  #endregion Delegates

  /// <summary>
  /// Used for decoding received IR Codes.
  /// </summary>
  static class IrDecoder
  {

    #region Constants

    const ushort ToggleBitMce   = 0x8000;
    const ushort ToggleMaskMce  = 0x7FFF;
    const ushort CustomerMce    = 0x800F;

    const ushort ToggleBitRC5   = 0x0800;
    const ushort ToggleMaskRC5  = 0xF7FF;

    const uint ToggleBitRC5X    = 0x00020000;
    const ushort ToggleMaskRC5X = 0xFFFF;

    const uint RC6HeaderMask    = 0xFFFFFFF0;

    const uint PrefixRC6        = 0x000FC950;
    const uint PrefixRC6A       = 0x000FCA90;
    const uint PrefixRC6Foxtel  = 0x000FCA93; 

    const uint MceMouse         = 1;
    const uint MceKeyboard      = 4;

    #endregion Constants

    #region Detection Data

    static RemoteDetectionData Daewoo_Data      = new RemoteDetectionData();
    static RemoteDetectionData JVC_Data         = new RemoteDetectionData();
    static RemoteDetectionData Matsushita_Data  = new RemoteDetectionData();
    static RemoteDetectionData Mitsubishi_Data  = new RemoteDetectionData();
    static RemoteDetectionData NEC_Data         = new RemoteDetectionData();
    static RemoteDetectionData NRC17_Data       = new RemoteDetectionData();
    static RemoteDetectionData Panasonic_Data   = new RemoteDetectionData();
    static RemoteDetectionData RC5_Data         = new RemoteDetectionData();
    static RemoteDetectionData RC6_Data         = new RemoteDetectionData();
    static RemoteDetectionData RCA_Data         = new RemoteDetectionData();
    static RemoteDetectionData RECS80_Data      = new RemoteDetectionData();
    static RemoteDetectionData SIRC_Data        = new RemoteDetectionData();
    static RemoteDetectionData Toshiba_Data     = new RemoteDetectionData();

    static MceDetectionData MCE_Data            = new MceDetectionData();

    #endregion Detection Data

    #region Methods

    /// <summary>
    /// Decode timing data to discover IR Protocol and packet payload.
    /// </summary>
    /// <param name="timingData">Input timing data.</param>
    /// <param name="remoteCallback">Method to call when Remote button decoded.</param>
    /// <param name="keyboardCallback">Method to call when Keyboard event decoded.</param>
    /// <param name="mouseCallback">Method to call when Mouse event decoded.</param>
    public static void DecodeIR(int[] timingData, RemoteCallback remoteCallback, KeyboardCallback keyboardCallback, MouseCallback mouseCallback)
    {
      if (timingData == null)
        return;

      try
      {
        DetectDaewoo(timingData, remoteCallback);
        //DetectITT(timingData, remoteCallback);
        DetectJVC(timingData, remoteCallback);
        DetectMatsushita(timingData, remoteCallback);
        DetectMitsubishi(timingData, remoteCallback);
        DetectNEC(timingData, remoteCallback);
        DetectNRC17(timingData, remoteCallback);
        DetectPanasonic(timingData, remoteCallback);
        DetectRC5(timingData, remoteCallback);
        DetectRC6(timingData, remoteCallback);
        DetectRCA(timingData, remoteCallback);
        //DetectRCMM(timingData, remoteCallback);
        DetectRECS80(timingData, remoteCallback);
        //DetectSharp(timingData, remoteCallback);
        DetectSIRC(timingData, remoteCallback);
        DetectToshiba(timingData, remoteCallback);
        //DetectXSAT(timingData, remoteCallback);

        DetectMCE(timingData, keyboardCallback, mouseCallback);
        //DetectIMon(timingData, keyboardCallback, mouseCallback);
      }
#if TRACE
      catch (Exception ex)
      {
        Trace.WriteLine(ex.ToString());
#else
      catch
      {
#endif
        Daewoo_Data     = new RemoteDetectionData();
        JVC_Data        = new RemoteDetectionData();
        Matsushita_Data = new RemoteDetectionData();
        Mitsubishi_Data = new RemoteDetectionData();
        NEC_Data        = new RemoteDetectionData();
        NRC17_Data      = new RemoteDetectionData();
        Panasonic_Data  = new RemoteDetectionData();
        RC5_Data        = new RemoteDetectionData();
        RC6_Data        = new RemoteDetectionData();
        RCA_Data        = new RemoteDetectionData();
        RECS80_Data     = new RemoteDetectionData();
        SIRC_Data       = new RemoteDetectionData();
        Toshiba_Data    = new RemoteDetectionData();

        MCE_Data        = new MceDetectionData();
      }
    }

    static void DetectDaewoo(int[] timingData, RemoteCallback remoteCallback)
    {
      for (int i = 0; i < timingData.Length; i++)
      {
        int duration = Math.Abs(timingData[i]);
        bool pulse = (timingData[i] > 0);
        bool ignored = true;

        //Trace.WriteLine("Daewoo - {0}: {1}", Enum.GetName(typeof(RemoteDetectionState), Daewoo_Data.State), timingData[i]);

        switch (Daewoo_Data.State)
        {

          #region HeaderPulse
          case RemoteDetectionState.HeaderPulse:
            if (pulse && IsBetween(duration, 7800, 8200))
            {
              Daewoo_Data.State = RemoteDetectionState.HeaderSpace;
              ignored = false;
            }
            break;
          #endregion HeaderPulse

          #region HeaderSpace
          case RemoteDetectionState.HeaderSpace:
            if (!pulse && IsBetween(duration, 3800 , 4200))
            {
              Daewoo_Data.State = RemoteDetectionState.Data;
              Daewoo_Data.HalfBit = 0;
              Daewoo_Data.Bit = 0;
              Daewoo_Data.Code = 0;
              ignored = false;
            }
            else if (!pulse && IsBetween(duration, 10000 , 40000)) // For Repeats
            {
              Daewoo_Data.State = RemoteDetectionState.Data;
              Daewoo_Data.HalfBit = 0;
              Daewoo_Data.Bit = 0;
              Daewoo_Data.Code = 0;
              ignored = false;
            }
            break;
          #endregion HeaderSpace

          #region Data
          case RemoteDetectionState.Data:
            if (pulse && IsBetween(duration, 350, 750))
            {
              Daewoo_Data.HalfBit = 1;
              ignored = false;
            }
            else if (!pulse && IsBetween(duration, 250, 650) && Daewoo_Data.HalfBit == 1)
            {
              Daewoo_Data.Code <<= 1;
              Daewoo_Data.Bit++;
              Daewoo_Data.HalfBit = 0;
              ignored = false;
            }
            else if (!pulse && IsBetween(duration, 1250, 1650) && Daewoo_Data.HalfBit == 1)
            {
              Daewoo_Data.Code <<= 1;
              Daewoo_Data.Code |= 1;
              Daewoo_Data.Bit++;
              Daewoo_Data.HalfBit = 0;
              ignored = false;
            }
            else
            {
              //Trace.WriteLine("Daewoo Error");
            }

            if (Daewoo_Data.Bit == 16)
            {
              remoteCallback(IrProtocol.Daewoo, Daewoo_Data.Code, false);
              Daewoo_Data.State = RemoteDetectionState.Leading;
            }
            break;
          #endregion Data

          #region Leading
          case RemoteDetectionState.Leading:
            if (pulse && IsBetween(duration, 350, 750))
            {
              Daewoo_Data.State = RemoteDetectionState.HeaderSpace;
              ignored = false;
            }
            break;
          #endregion Leading

        }

        if (ignored && (Daewoo_Data.State != RemoteDetectionState.HeaderPulse))
          Daewoo_Data.State = RemoteDetectionState.HeaderPulse;
      }
    }
    static void DetectJVC(int[] timingData, RemoteCallback remoteCallback)
    {
      for (int i = 0; i < timingData.Length; i++)
      {
        int duration = Math.Abs(timingData[i]);
        bool pulse = (timingData[i] > 0);
        bool ignored = true;

        //Trace.WriteLine("JVC - {0}: {1}", Enum.GetName(typeof(RemoteDetectionState), JVC_Data.State), timingData[i]);

        switch (JVC_Data.State)
        {

          #region HeaderPulse
          case RemoteDetectionState.HeaderPulse:
            if (pulse && IsBetween(duration, 8300, 8500))
            {
              JVC_Data.State = RemoteDetectionState.HeaderSpace;
              ignored = false;
            }
            break;
          #endregion HeaderPulse

          #region HeaderSpace
          case RemoteDetectionState.HeaderSpace:
            if (!pulse && IsBetween(duration, 4100, 4300))
            {
              JVC_Data.Toggle = 0;

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
            if (pulse && IsBetween(duration, 450, 650))
            {
              JVC_Data.HalfBit = 1;
              ignored = false;
            }
            else if (!pulse && IsBetween(duration, 450, 650) && JVC_Data.HalfBit == 1)
            {
              JVC_Data.Code <<= 1;
              JVC_Data.Bit++;
              JVC_Data.HalfBit = 0;
              ignored = false;
            }
            else if (!pulse && IsBetween(duration, 1450, 1700) && JVC_Data.HalfBit == 1)
            {
              JVC_Data.Code <<= 1;
              JVC_Data.Code |= 1;
              JVC_Data.Bit++;
              JVC_Data.HalfBit = 0;
              ignored = false;
            }
            else if (!pulse && duration >= 10000)
            {
              if (JVC_Data.Bit == 16)
              {
                bool first = false;
                if (JVC_Data.Code != JVC_Data.Toggle)
                  first = true;

                remoteCallback(IrProtocol.JVC, JVC_Data.Code, first);
                ignored = false;

                JVC_Data.Toggle = (int)JVC_Data.Code;

                if (duration > 25000)
                  JVC_Data.State = RemoteDetectionState.HeaderPulse;
                else
                  JVC_Data.State = RemoteDetectionState.Data;

                JVC_Data.HalfBit = 0;
                JVC_Data.Bit = 0;
                JVC_Data.Code = 0;
              }
              else if (JVC_Data.Bit == 32)
              {
                remoteCallback(IrProtocol.Unknown, JVC_Data.Code, false);
              }
              else
              {
                //Trace.WriteLine("JVC Error");
              }
 
            }
            else
            {
              //Trace.WriteLine("JVC Error");
            }

            break;
          #endregion Data

        }

        if (ignored && (JVC_Data.State != RemoteDetectionState.HeaderPulse))
          JVC_Data.State = RemoteDetectionState.HeaderPulse;
      }
    }
    static void DetectMatsushita(int[] timingData, RemoteCallback remoteCallback)
    {
      for (int i = 0; i < timingData.Length; i++)
      {
        int duration = Math.Abs(timingData[i]);
        bool pulse = (timingData[i] > 0);
        bool ignored = true;

        //Trace.WriteLine("Matsushita - {0}: {1}", Enum.GetName(typeof(RemoteDetectionState), Matsushita_Data.State), timingData[i]);

        switch (Matsushita_Data.State)
        {

          #region HeaderPulse
          case RemoteDetectionState.HeaderPulse:
            if (pulse && IsBetween(duration, 3300, 3700))
            {
              Matsushita_Data.State = RemoteDetectionState.HeaderSpace;
              ignored = false;
            }
            //else
              //Trace.WriteLine("HeaderPulse fall through");

            break;
          #endregion HeaderPulse

          #region HeaderSpace
          case RemoteDetectionState.HeaderSpace:
            if (!pulse && IsBetween(duration, 3300, 3700))
            {
              Matsushita_Data.State = RemoteDetectionState.Data;
              Matsushita_Data.HalfBit = 0;
              Matsushita_Data.Bit = 0;
              Matsushita_Data.Code = 0;
              ignored = false;
            }
            //else
              //Trace.WriteLine("HeaderSpace fell through");

            break;
          #endregion HeaderSpace

          #region Data
          case RemoteDetectionState.Data:
            if (pulse && IsBetween(duration, 650, 1050))
            {
              Matsushita_Data.HalfBit = 1;
              ignored = false;
            }
            else if (!pulse && IsBetween(duration, 650, 1050) && Matsushita_Data.HalfBit == 1)
            {
              Matsushita_Data.Code <<= 1;
              Matsushita_Data.Bit++;
              Matsushita_Data.HalfBit = 0;
              ignored = false;
            }
            else if (!pulse && IsBetween(duration, 2450, 2850) && Matsushita_Data.HalfBit == 1)
            {
              Matsushita_Data.Code <<= 1;
              Matsushita_Data.Code |= 1;
              Matsushita_Data.Bit++;
              Matsushita_Data.HalfBit = 0;
              ignored = false;
            }
            else if (!pulse && duration >= 20000 && Matsushita_Data.HalfBit == 1)
            {
              if (Matsushita_Data.Bit != 22)
                break;

              uint code = Matsushita_Data.Code >> 12;
              remoteCallback(IrProtocol.Matsushita, code, false);
              Matsushita_Data.State = RemoteDetectionState.HeaderPulse;
              Matsushita_Data.HalfBit = 0;
              ignored = false;
            }
            else
            {
              //Trace.WriteLine("Matsushita Error");
            }

            break;
          #endregion Data

        }

        if (ignored && (Matsushita_Data.State != RemoteDetectionState.HeaderPulse))
        {
          //Trace.WriteLine("ignored");
          Matsushita_Data.State = RemoteDetectionState.HeaderPulse;
        }
      }
    }
    static void DetectMitsubishi(int[] timingData, RemoteCallback remoteCallback)
    {
      for (int i = 0; i < timingData.Length; i++)
      {
        int duration = Math.Abs(timingData[i]);
        bool pulse = (timingData[i] > 0);
        bool ignored = true;

        //Trace.WriteLine("Mitsubishi - {0}: {1}", Enum.GetName(typeof(RemoteDetectionState), Mitsubishi_Data.State), timingData[i]);

        switch (Mitsubishi_Data.State)
        {

          #region HeaderPulse
          case RemoteDetectionState.HeaderPulse:
            if (pulse && IsBetween(duration, 7800, 8200))
            {
              Mitsubishi_Data.State = RemoteDetectionState.HeaderSpace;
              ignored = false;
            }

            break;
          #endregion HeaderPulse

          #region HeaderSpace
          case RemoteDetectionState.HeaderSpace:
            if (!pulse && IsBetween(duration, 3800, 4200))
            {
              Mitsubishi_Data.State = RemoteDetectionState.Data;
              Mitsubishi_Data.HalfBit = 0;
              Mitsubishi_Data.Bit = 0;
              Mitsubishi_Data.Code = 0;
              ignored = false;
            }

            break;
          #endregion HeaderSpace

          #region Data
          case RemoteDetectionState.Data:
            if (pulse && IsBetween(duration, 350, 650))
            {
              Mitsubishi_Data.HalfBit = 1;
              ignored = false;
            }
            else if (!pulse && IsBetween(duration, 350, 650) && Mitsubishi_Data.HalfBit == 1)
            {
              Mitsubishi_Data.Code <<= 1;
              Mitsubishi_Data.Bit++;
              Mitsubishi_Data.HalfBit = 0;
              ignored = false;
            }
            else if (!pulse && IsBetween(duration, 1300, 1700) && Mitsubishi_Data.HalfBit == 1)
            {
              Mitsubishi_Data.Code <<= 1;
              Mitsubishi_Data.Code |= 1;
              Mitsubishi_Data.Bit++;
              Mitsubishi_Data.HalfBit = 0;
              ignored = false;
            }
            else if (!pulse && IsBetween(duration, 3800, 4200) && Mitsubishi_Data.HalfBit == 1 && Mitsubishi_Data.Bit == 8)
            {
              Mitsubishi_Data.HalfBit = 0;
              ignored = false;
            }
            else if (!pulse && duration >= 20000 && Mitsubishi_Data.HalfBit == 1 && Mitsubishi_Data.Bit == 16)
            {
              remoteCallback(IrProtocol.Mitsubishi, Mitsubishi_Data.Code, false);
              Mitsubishi_Data.State = RemoteDetectionState.HeaderPulse;
              Mitsubishi_Data.HalfBit = 0;
              ignored = false;
            }

            break;
          #endregion Data

        }

        if (ignored && (Mitsubishi_Data.State != RemoteDetectionState.HeaderPulse))
        {
          //Trace.WriteLine("ignored");
          Mitsubishi_Data.State = RemoteDetectionState.HeaderPulse;
        }
      }
    }
    static void DetectNEC(int[] timingData, RemoteCallback remoteCallback)
    {
      for (int i = 0; i < timingData.Length; i++)
      {
        int duration = Math.Abs(timingData[i]);
        bool pulse = (timingData[i] > 0);
        bool ignored = true;

        //Trace.WriteLine(String.Format("NEC - {0}: {1}", Enum.GetName(typeof(RemoteDetectionState), NEC_Data.State), timingData[i]));

        switch (NEC_Data.State)
        {

          #region HeaderPulse
          case RemoteDetectionState.HeaderPulse:
            if (pulse && IsBetween(duration, 8800, 9200))
            {
              NEC_Data.State = RemoteDetectionState.HeaderSpace;
              ignored = false;
            }

            break;
          #endregion HeaderPulse

          #region HeaderSpace
          case RemoteDetectionState.HeaderSpace:
            if (!pulse && IsBetween(duration, 4300, 4700))
            {
              NEC_Data.State = RemoteDetectionState.Data;
              NEC_Data.HalfBit = 0;
              NEC_Data.Bit = 0;
              NEC_Data.Code = 0;
              ignored = false;
            }
            else if (!pulse && IsBetween(duration, 2050, 2450)) // For Repeats
            {
              //Trace.Write("Repeat");

              if (NEC_Data.Code != 0)
              {
                uint address = (NEC_Data.Code >> 24) & 0xFF;
                uint command = (NEC_Data.Code >> 8) & 0xFF;

                uint code = (address << 8) + command;

                //Trace.WriteLine(" Code: {0}", code);

                remoteCallback(IrProtocol.NEC, code, false);

                NEC_Data.State = RemoteDetectionState.Leading;
                ignored = false;
              }
            }

            break;
          #endregion HeaderSpace

          #region Data
          case RemoteDetectionState.Data:
            if (pulse && IsBetween(duration, 350, 750))
            {
              NEC_Data.HalfBit = 1;
              ignored = false;
            }
            else if (!pulse && IsBetween(duration, 350, 650) && NEC_Data.HalfBit == 1)
            {
              NEC_Data.Code <<= 1;
              NEC_Data.Bit++;
              NEC_Data.HalfBit = 0;
              ignored = false;
            }
            else if (!pulse && IsBetween(duration, 1200, 2800) && NEC_Data.HalfBit == 1)
            {
              NEC_Data.Code <<= 1;
              NEC_Data.Code |= 1;
              NEC_Data.Bit++;
              NEC_Data.HalfBit = 0;
              ignored = false;
            }
            else if (!pulse && duration >= 12000)
            {
              if (NEC_Data.Bit != 32)
              {
                if (NEC_Data.Code != 0)
                {
                  //Trace.WriteLine("Invalid NEC: {0}bit, {1:X}", NEC_Data.Bit, NEC_Data.Code);
                  remoteCallback(IrProtocol.Unknown, NEC_Data.Code, false);
                }
                break;
              }

              uint address     = (NEC_Data.Code >> 24) & 0xFF;
              uint notAddress  = (NEC_Data.Code >> 16) & 0xFF;

              uint command     = (NEC_Data.Code >> 8) & 0xFF;
              uint notCommand  = NEC_Data.Code & 0xFF;

              if ((address + notAddress == 0xFF) && (command + notCommand == 0xFF))
              {
                uint code = (address << 8) + command;
                remoteCallback(IrProtocol.NEC, code, true);
                NEC_Data.State = RemoteDetectionState.HeaderPulse;
                ignored = false;
              }
              else
              {
                //Trace.WriteLine("Invalid NEC: {0:X}", NEC_Data.Code);
                remoteCallback(IrProtocol.Unknown, NEC_Data.Code, false);
              }
            }

            break;
          #endregion Data

          #region Leading
          case RemoteDetectionState.Leading:
            if (pulse && IsBetween(duration, 400, 800))
            {
              ignored = false;
            }
            else if (!pulse && duration > 10000)
            {
              ignored = false;
              NEC_Data.State = RemoteDetectionState.HeaderPulse;
            }

            break;
          #endregion Leading

        }

        if (ignored && (NEC_Data.State != RemoteDetectionState.HeaderPulse))
          NEC_Data.State = RemoteDetectionState.HeaderPulse;
      }
    }
    static void DetectNRC17(int[] timingData, RemoteCallback remoteCallback)
    {
      for (int i = 0; i < timingData.Length; i++)
      {
        int duration = Math.Abs(timingData[i]);
        bool pulse = (timingData[i] > 0);
        bool ignored = true;

        //Trace.WriteLine("NRC17 - {0}: {1}", Enum.GetName(typeof(RemoteDetectionState), NRC17_Data.State), timingData[i]);

        switch (NRC17_Data.State)
        {

          #region HeaderPulse
          case RemoteDetectionState.HeaderPulse:

            if (pulse && IsBetween(duration, 400, 650))
            {
              NRC17_Data.State = RemoteDetectionState.HeaderSpace;
              ignored = false;
            }
            break;
          #endregion HeaderPulse

          #region HeaderSpace
          case RemoteDetectionState.HeaderSpace:

            if (!pulse &&
              (IsBetween(duration, 2350, 2600)  ||  // Normal battery
               IsBetween(duration, 3350, 3600)))    // Low battery              
            {
              NRC17_Data.State = RemoteDetectionState.Data;
              NRC17_Data.HalfBit = 0;
              NRC17_Data.Bit = 17;
              NRC17_Data.Header = 0;
              NRC17_Data.Code = 0;
              ignored = false;
            }
            break;
          #endregion HeaderSpace

          #region Data
          case RemoteDetectionState.Data:
            if (NRC17_Data.HalfBit == 0)
            {
              if (pulse && IsBetween(duration, 300, 700))
              {
                // Logic 1
                NRC17_Data.HalfBit = 1;
                NRC17_Data.Code |= (uint)(1 << NRC17_Data.Bit--);
                ignored = false;
              }
              else if (!pulse && IsBetween(duration, 300, 700))
              {
                // Logic 0
                NRC17_Data.HalfBit = 1;
                NRC17_Data.Bit--;
                ignored = false;
              }
            }
            else
            {
              if (!pulse && IsBetween(duration, 300, 700))
              {
                NRC17_Data.HalfBit = 0;
                ignored = false;
              }
              else if (pulse && IsBetween(duration, 300, 700))
              {
                NRC17_Data.HalfBit = 0;
                ignored = false;
              }
              else if (!pulse && IsBetween(duration, 800, 1200))
              {
                NRC17_Data.HalfBit = 1;
                NRC17_Data.Bit--;
                ignored = false;
              }
              else if (pulse && IsBetween(duration, 800, 1200))
              {
                NRC17_Data.HalfBit = 1;
                NRC17_Data.Code |= (uint)(1 << NRC17_Data.Bit--);
                ignored = false;
              }
            }

            if (NRC17_Data.Bit == 0)
            {
              NRC17_Data.Code &= 0xFFFF;  // 16-bits (Ignore leading bit which is always 1)
              remoteCallback(IrProtocol.NRC17, NRC17_Data.Code, false);

              NRC17_Data.State = RemoteDetectionState.HeaderPulse;
            }

            break;
          #endregion Data

        }

        if (ignored && (NRC17_Data.State != RemoteDetectionState.HeaderPulse))
          NRC17_Data.State = RemoteDetectionState.HeaderPulse;
      }
    }
    static void DetectPanasonic(int[] timingData, RemoteCallback remoteCallback)
    {
      for (int i = 0; i < timingData.Length; i++)
      {
        int duration = Math.Abs(timingData[i]);
        bool pulse = (timingData[i] > 0);
        bool ignored = true;

        switch (Panasonic_Data.State)
        {

          #region HeaderPulse
          case RemoteDetectionState.HeaderPulse:
            if (pulse && IsBetween(duration, 3150, 3900))
            {
              Panasonic_Data.State = RemoteDetectionState.HeaderSpace;
              ignored = false;
            }

            break;
          #endregion HeaderPulse

          #region HeaderSpace
          case RemoteDetectionState.HeaderSpace:
            if (!pulse && IsBetween(duration, 3150, 3900))
            {
              Panasonic_Data.State = RemoteDetectionState.Data;
              Panasonic_Data.HalfBit = 0;
              Panasonic_Data.Bit = 0;
              Panasonic_Data.Code = 0;
              ignored = false;
            }
            else if (!pulse && IsBetween(duration, 2050, 2450)) // For Repeats
            {
              //Trace.Write("Repeat");

              if (Panasonic_Data.Code != 0)
              {
                uint address = (Panasonic_Data.Code >> 24) & 0xFF;
                uint command = (Panasonic_Data.Code >> 8) & 0xFF;

                uint code = (address << 8) + command;

                //Trace.WriteLine(" Code: {0}", code);

                remoteCallback(IrProtocol.Panasonic, code, false);

                Panasonic_Data.State = RemoteDetectionState.Leading;
                ignored = false;
              }
            }

            break;
          #endregion HeaderSpace

          #region Data
          case RemoteDetectionState.Data:
            if (pulse && IsBetween(duration, 600, 1150))
            {
              Panasonic_Data.HalfBit = 1;
              ignored = false;
            }
            else if (!pulse && IsBetween(duration, 600, 1150) && Panasonic_Data.HalfBit == 1)
            {
              Panasonic_Data.Code <<= 1;
              Panasonic_Data.Bit++;
              Panasonic_Data.HalfBit = 0;
              ignored = false;
            }
            else if (!pulse && IsBetween(duration, 1800, 3450) && Panasonic_Data.HalfBit == 1)
            {
              Panasonic_Data.Code <<= 1;
              Panasonic_Data.Code |= 1;
              Panasonic_Data.Bit++;
              Panasonic_Data.HalfBit = 0;
              ignored = false;
            }
            else if (!pulse && duration >= 8000)
            {
              if (Panasonic_Data.Bit != 22)
                break;

              uint custom = (Panasonic_Data.Code >> 17) & 0x1F;
              uint data = (Panasonic_Data.Code >> 11) & 0x3F;

              uint notCustom = (Panasonic_Data.Code >> 6) & 0x1F;
              uint notData = Panasonic_Data.Code & 0x3F;

              if ((custom + notCustom == 0x1F) && (data + notData == 0x3F))
              {
                uint code = (custom << 8) + data;
                remoteCallback(IrProtocol.Panasonic, code, true);
                Panasonic_Data.State = RemoteDetectionState.HeaderPulse;
                ignored = false;
              }
              else
              {
                //Trace.WriteLine("custom != notCustom || data != notData    fall through");
                //Trace.WriteLine("{0:X}", Panasonic_Data.Code);
              }
            }

            break;
          #endregion Data

          #region Leading
          case RemoteDetectionState.Leading:
            if (pulse && IsBetween(duration, 400, 800))
            {
              ignored = false;
            }
            else if (!pulse && IsBetween(duration, 38000, 40000)) // First Repeat
            {
              ignored = false;
              Panasonic_Data.State = RemoteDetectionState.HeaderPulse;
            }
            else if (!pulse && IsBetween(duration, 94600, 95000)) // Multiple Repeats
            {
              ignored = false;
              Panasonic_Data.State = RemoteDetectionState.HeaderPulse;
            }

            break;
          #endregion Leading

        }

        if (ignored && (Panasonic_Data.State != RemoteDetectionState.HeaderPulse))
        {
          //Trace.WriteLine("Panasonic Ignored");
          Panasonic_Data.State = RemoteDetectionState.HeaderPulse;
        }
      }
    }
    static void DetectRC5(int[] timingData, RemoteCallback remoteCallback)
    {
      for (int i = 0; i < timingData.Length; i++)
      {
        int duration = Math.Abs(timingData[i]);
        bool pulse = (timingData[i] > 0);
        bool ignored = true;

        switch (RC5_Data.State)
        {

          #region HeaderPulse
          case RemoteDetectionState.HeaderPulse:
            if (pulse)
            {
              if (IsBetween(duration, 750, 1100))
              {
                RC5_Data.State = RemoteDetectionState.HeaderSpace;
                RC5_Data.Bit = 13;
                RC5_Data.Code = (uint)1 << RC5_Data.Bit;
                ignored = false;
              }
              else if (IsBetween(duration, 1500, 2000))
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
            if (!pulse && IsBetween(duration, 750, 1000))
            {
              RC5_Data.State = RemoteDetectionState.Data;
              RC5_Data.HalfBit = 0;
              ignored = false;
            }
            break;
          #endregion HeaderSpace

          #region Data
          case RemoteDetectionState.Data:
            if (RC5_Data.HalfBit == 0)
            {
              if (pulse)
              {
                if (IsBetween(duration, 750, 1100) || IsBetween(duration, 1500, 2000))
                {
                  RC5_Data.HalfBit = (byte)((duration >= 1500) ? 0 : 1);
                  RC5_Data.Bit--;
                  RC5_Data.Code |= (uint)1 << RC5_Data.Bit;
                  ignored = false;

                  if (RC5_Data.Bit == 0 || (RC5_Data.Bit == 1 && duration >= 1500))
                    RC5_Data.State = RemoteDetectionState.KeyCode;
                }
              }
              else
              {
                if (IsBetween(duration, 750, 1100) || IsBetween(duration, 1500, 2000))
                {
                  RC5_Data.HalfBit = (byte)((duration >= 1500) ? 0 : 1);
                  RC5_Data.Bit--;
                  ignored = false;

                  if (RC5_Data.Bit == 0)
                    RC5_Data.State = RemoteDetectionState.KeyCode;
                }
                else if (RC5_Data.Bit == 7 && (IsBetween(duration, 4300, 4700) || IsBetween(duration, 5200, 5600)))
                {
                  ignored = false;
                  RC5_Data.HalfBit = (byte)((duration >= 5200) ? 0 : 1);
                  RC5_Data.Code <<= 6;
                  RC5_Data.Bit += 5;
                }
              }
              break;
            }

            if (IsBetween(duration, 750, 1100))
            {
              RC5_Data.HalfBit = 0;
              ignored = false;

              if ((RC5_Data.Bit == 1) && pulse)
                RC5_Data.State = RemoteDetectionState.KeyCode;
            }
            else if (RC5_Data.Bit == 7 && (IsBetween(duration, 3400, 3800) || IsBetween(duration, 4300, 4700)))
            {
              RC5_Data.HalfBit = (byte)((duration >= 4300) ? 0 : 1);
              RC5_Data.Code <<= 6;
              RC5_Data.Bit += 6;
              ignored = false;
            }

            break;
          #endregion Data

          #region Leading
          case RemoteDetectionState.Leading:
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
          bool toggleOn;
          
          bool first = true;
          bool RC5X;

          if (RC5_Data.Code > 0xFFFF) // RC5X
          {
            toggleOn = ((RC5_Data.Code & ToggleBitRC5X) == ToggleBitRC5X);
            RC5_Data.Code &= ToggleMaskRC5X;
            RC5X = true;
          }
          else // RC5
          {
            toggleOn = ((RC5_Data.Code & ToggleBitRC5) == ToggleBitRC5);
            RC5_Data.Code &= ToggleMaskRC5;
            RC5X = false;
          }

          if ((toggleOn && RC5_Data.Toggle == 1) || (!toggleOn && RC5_Data.Toggle == 2))
            first = false;

          RC5_Data.Toggle = toggleOn ? 1 : 2;

          if (RC5X)
            remoteCallback(IrProtocol.RC5X, RC5_Data.Code, first);
          else
            remoteCallback(IrProtocol.RC5, RC5_Data.Code, first);

          RC5_Data.State = RemoteDetectionState.HeaderPulse;
        }

        if (ignored && (RC5_Data.State != RemoteDetectionState.Leading) && (RC5_Data.State != RemoteDetectionState.HeaderPulse))
          RC5_Data.State = RemoteDetectionState.HeaderPulse;
      }

    }
    static void DetectRC6(int[] timingData, RemoteCallback remoteCallback)
    {
      for (int i = 0; i < timingData.Length; i++)
      {
        int duration = Math.Abs(timingData[i]);
        bool pulse = (timingData[i] > 0);
        bool ignored = true;

        //Trace.WriteLine(String.Format("RC6 - {0}: {1}", Enum.GetName(typeof(RemoteDetectionState), RC6_Data.State), timingData[i]));

        switch (RC6_Data.State)
        {

          #region HeaderPulse
          case RemoteDetectionState.HeaderPulse:
            if (pulse && IsBetween(duration, 2600, 3300))
            {
              RC6_Data.State = RemoteDetectionState.HeaderSpace;
              RC6_Data.Header = 0x000FC000;
              RC6_Data.Bit = 14;
              RC6_Data.HalfBit = 0;
              RC6_Data.Code = 0;
              RC6_Data.LongPulse = false;
              RC6_Data.LongSpace = false;
              RC6_Data.Toggle &= 0xFE;
              ignored = false;
            }
            break;
          #endregion HeaderPulse

          #region HeaderSpace
          case RemoteDetectionState.HeaderSpace:
            if (!pulse && IsBetween(duration, 750, 1000))
            {
              RC6_Data.State = RemoteDetectionState.PreData;
              RC6_Data.Bit -= 2;
              ignored = false;
            }
            break;
          #endregion HeaderSpace

          #region PreData
          case RemoteDetectionState.PreData:
            if (pulse)
            {
              if (IsBetween(duration, 300, 600))
              {
                ignored = false;
                if (RC6_Data.Bit != 0) RC6_Data.Header |= (uint)(1 << --RC6_Data.Bit);
              }
              else if (IsBetween(duration, 750, 1000))
              {
                ignored = false;
                if (RC6_Data.Bit != 0) RC6_Data.Header |= (uint)(1 << --RC6_Data.Bit);
                if (RC6_Data.Bit != 0) RC6_Data.Header |= (uint)(1 << --RC6_Data.Bit);
              }
              else if (IsBetween(duration, 1200, 1600))
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
            }
            else
            {
              if (IsBetween(duration, 300, 600))
              {
                RC6_Data.Bit--;
                ignored = false;
              }
              else if (IsBetween(duration, 750, 1000))
              {
                ignored = false;
                if (RC6_Data.Bit > 2)
                  RC6_Data.Bit -= 2;
                else
                  RC6_Data.Bit = 0;
              }
              else if (IsBetween(duration, 1200, 1600))
              {
                ignored = false;
                if (RC6_Data.Bit >= 3)
                {
                  RC6_Data.Bit -= 3;
                }
                else
                {
                  RC6_Data.HalfBit = 1;
                  //RC6_Data.LongPulse = true;
                  RC6_Data.LongSpace = true;
                  RC6_Data.Bit = 0;
                  RC6_Data.Toggle |= 1;
                }
              }
            }

            if (!ignored && RC6_Data.Bit == 0)
            {
              if ((RC6_Data.Header & RC6HeaderMask) == PrefixRC6)
              {
                RC6_Data.Bit = 16;
              }
              else if (RC6_Data.Header == PrefixRC6Foxtel)
              {
                RC6_Data.Bit = 20;
              }
              else if ((RC6_Data.Header & RC6HeaderMask) == PrefixRC6A)
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
            if ((RC6_Data.HalfBit % 2) == 0)
            {
              if (pulse && IsBetween(duration, 300, 600))
              {
                ignored = false;
                RC6_Data.LongPulse = true;
                RC6_Data.HalfBit++;

                if (RC6_Data.Bit == 1)
                  RC6_Data.State = RemoteDetectionState.KeyCode;
              }
              else if (!pulse && IsBetween(duration, 300, 600))
              {
                ignored = false;
                RC6_Data.LongSpace = true;
                RC6_Data.HalfBit++;
              }
              break;
            }

            if (RC6_Data.LongPulse)
            {
              RC6_Data.LongPulse = false;
              if (pulse)
                break;

              if (IsBetween(duration, 750, 1000))
              {
                RC6_Data.Bit--;
                RC6_Data.LongSpace = true;
                RC6_Data.HalfBit += 2;
                ignored = false;
              }
              else if (IsBetween(duration, 300, 600))
              {
                RC6_Data.Bit--;
                RC6_Data.HalfBit++;
                ignored = false;
              }
            }
            else if (RC6_Data.LongSpace)
            {
              RC6_Data.LongSpace = false;

              if (!pulse)
                break;

              if (RC6_Data.Bit == 32)
                RC6_Data.Bit = 24;

              if (IsBetween(duration, 750, 1000))
              {
                RC6_Data.Bit--;
                RC6_Data.Code |= (uint)1 << RC6_Data.Bit;
                RC6_Data.LongPulse = true;
                RC6_Data.HalfBit += 2;
                ignored = false;

                if (RC6_Data.Bit == 1)
                  RC6_Data.State = RemoteDetectionState.KeyCode;
              }
              else if (IsBetween(duration, 300, 600))
              {
                RC6_Data.Bit--;
                RC6_Data.Code |= (uint)1 << RC6_Data.Bit;
                RC6_Data.HalfBit++;
                ignored = false;

                if (RC6_Data.Bit == 0)
                  RC6_Data.State = RemoteDetectionState.KeyCode;
              }
            }
            break;
          #endregion Data

        }

        if (RC6_Data.State == RemoteDetectionState.KeyCode)
        {
          bool first = false;

          IrProtocol protocolVariation = IrProtocol.RC6;

          if ((~RC6_Data.Code >> 16) == CustomerMce) // MCE RC6 variation
          {
            bool toggleOn = ((RC6_Data.Code & ToggleBitMce) == ToggleBitMce);

            if ((toggleOn && RC6_Data.Toggle != 8) || (!toggleOn && RC6_Data.Toggle != 16))
              first = true;

            // Use this to signal toggle in MCE RC6 variation
            RC6_Data.Toggle = toggleOn ? 8 : 16;

            RC6_Data.Code &= ToggleMaskMce;

            protocolVariation = IrProtocol.RC6_MCE;
          }
          else // Standard RC6 or Non-MCE variations
          {
            bool toggleOn = (RC6_Data.Toggle & 1) == 1;

            if (RC6_Data.Toggle == 0 || RC6_Data.Toggle == 1 || RC6_Data.Toggle == 2 || RC6_Data.Toggle == 5)
              first = true;

            // Use this to signal toggle in standard RC6
            if (toggleOn)
              RC6_Data.Toggle = 2;
            else
              RC6_Data.Toggle = 4;

            if (RC6_Data.Header == PrefixRC6Foxtel)
              protocolVariation = IrProtocol.RC6_Foxtel;
            else if ((RC6_Data.Header & RC6HeaderMask) == PrefixRC6A)
              protocolVariation = IrProtocol.RC6A;
          }

          remoteCallback(protocolVariation, RC6_Data.Code, first);

          RC6_Data.State = RemoteDetectionState.HeaderPulse;
        }

        if (ignored && (RC6_Data.State != RemoteDetectionState.HeaderPulse))
          RC6_Data.State = RemoteDetectionState.HeaderPulse;
      }

    }
    static void DetectRCA(int[] timingData, RemoteCallback remoteCallback)
    {
      for (int i = 0; i < timingData.Length; i++)
      {
        int duration = Math.Abs(timingData[i]);
        bool pulse = (timingData[i] > 0);
        bool ignored = true;

        switch (RCA_Data.State)
        {

          #region HeaderPulse
          case RemoteDetectionState.HeaderPulse:
            if (pulse && IsBetween(duration, 3800, 4200))
            {
              RCA_Data.State = RemoteDetectionState.HeaderSpace;
              ignored = false;
            }
            break;
          #endregion HeaderPulse

          #region HeaderSpace
          case RemoteDetectionState.HeaderSpace:
            if (!pulse && IsBetween(duration, 3800, 4200))
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
            if (pulse && IsBetween(duration, 300, 700))
            {
              RCA_Data.HalfBit = 1;
              ignored = false;
            }
            else if (!pulse && IsBetween(duration, 800, 1250) && RCA_Data.HalfBit == 1)
            {
              RCA_Data.Code <<= 1;
              RCA_Data.Bit++;
              RCA_Data.HalfBit = 0;
              ignored = false;
            }
            else if (!pulse && IsBetween(duration, 1800, 2200) && RCA_Data.HalfBit == 1)
            {
              RCA_Data.Code <<= 1;
              RCA_Data.Code |= 1;
              RCA_Data.Bit++;
              RCA_Data.HalfBit = 0;
              ignored = false;
            }

            if (RCA_Data.Bit == 12)
            {
              remoteCallback(IrProtocol.RCA, RCA_Data.Code, false);
              RCA_Data.State = RemoteDetectionState.HeaderPulse;
            }
            break;
          #endregion Data

        }

        if (ignored && (RCA_Data.State != RemoteDetectionState.HeaderPulse))
          RCA_Data.State = RemoteDetectionState.HeaderPulse;
      }
    }
    static void DetectRECS80(int[] timingData, RemoteCallback remoteCallback)
    {
      for (int i = 0; i < timingData.Length; i++)
      {
        int duration = Math.Abs(timingData[i]);
        bool pulse = (timingData[i] > 0);
        bool ignored = true;

        switch (RECS80_Data.State)
        {

          #region HeaderPulse
          case RemoteDetectionState.HeaderPulse:
            if (pulse && IsBetween(duration, 3300, 4100))
            {
              RECS80_Data.State = RemoteDetectionState.HeaderSpace;
              ignored = false;
            }

            break;
          #endregion HeaderPulse

          #region HeaderSpace
          case RemoteDetectionState.HeaderSpace:
            if (!pulse && IsBetween(duration, 1400, 1800))
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
            if ((RECS80_Data.HalfBit % 2) == 0)
            {
              if (!pulse)
                break;

              if (IsBetween(duration, 350, 600))
              {
                RECS80_Data.HalfBit = 1;
                RECS80_Data.Bit--;
                ignored = false;
              }
              else
                break;
            }
            else
            {
              if (pulse) 
                break;

              if (IsBetween(duration, 300, 750))
              {
                RECS80_Data.HalfBit = 0;
                ignored = false;
              }
              else if (IsBetween(duration, 1100, 1500))
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
                RECS80_Data.Code &= 0xFFFF;
                remoteCallback(IrProtocol.RECS80, RECS80_Data.Code, false);

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
    static void DetectSIRC(int[] timingData, RemoteCallback remoteCallback)
    {
      for (int i = 0; i < timingData.Length; i++)
      {
        int duration = Math.Abs(timingData[i]);
        bool pulse = (timingData[i] > 0);
        bool ignored = true;

        switch (SIRC_Data.State)
        {

          #region HeaderPulse
          case RemoteDetectionState.HeaderPulse:
            if (pulse && IsBetween(duration, 2100, 2700))
            {
              SIRC_Data.State = RemoteDetectionState.HeaderSpace;
              ignored = false;
            }

            break;
          #endregion HeaderPulse

          #region HeaderSpace
          case RemoteDetectionState.HeaderSpace:
            if (!pulse && IsBetween(duration, 400, 800))
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
            if (pulse && IsBetween(duration, 400, 800))
            {
              SIRC_Data.Code <<= 1;
              SIRC_Data.Bit++;
              ignored = false;
            }
            else if (pulse && IsBetween(duration, 1000, 1400))
            {
              SIRC_Data.Code <<= 1;
              SIRC_Data.Code |= 1;
              SIRC_Data.Bit++;
              ignored = false;
            }
            else if (!pulse && IsBetween(duration, 400, 800))
            {
              ignored = false;
            }
            else if (!pulse && duration >= 6000)
            {
              if ((SIRC_Data.Bit == 8) || (SIRC_Data.Bit == 12) || (SIRC_Data.Bit == 15) || (SIRC_Data.Bit == 20))
              {
                remoteCallback(IrProtocol.SIRC, SIRC_Data.Code, false);
                SIRC_Data.State = RemoteDetectionState.HeaderPulse;
                ignored = false;
              }
           }

            break;
          #endregion Data

        }

        if (ignored && (SIRC_Data.State != RemoteDetectionState.HeaderPulse))
          SIRC_Data.State = RemoteDetectionState.HeaderPulse;
      }

    }
    static void DetectToshiba(int[] timingData, RemoteCallback remoteCallback)
    {
      for (int i = 0; i < timingData.Length; i++)
      {
        int duration = Math.Abs(timingData[i]);
        bool pulse = (timingData[i] > 0);
        bool ignored = true;

        switch (Toshiba_Data.State)
        {

          #region HeaderPulse
          case RemoteDetectionState.HeaderPulse:
            if (pulse && IsBetween(duration, 4300, 4700))
            {
              Toshiba_Data.State = RemoteDetectionState.HeaderSpace;
              ignored = false;
            }

            break;
          #endregion HeaderPulse

          #region HeaderSpace
          case RemoteDetectionState.HeaderSpace:
            if (!pulse && IsBetween(duration, 4300, 4700))
            {
              Toshiba_Data.State = RemoteDetectionState.Data;
              Toshiba_Data.HalfBit = 0;
              Toshiba_Data.Bit = 0;
              Toshiba_Data.Code = 0;
              ignored = false;
            }

            break;
          #endregion HeaderSpace

          #region Data
          case RemoteDetectionState.Data:
            if (pulse && IsBetween(duration, 350, 750))
            {
              Toshiba_Data.HalfBit = 1;
              ignored = false;
            }
            else if (!pulse && IsBetween(duration, 350, 750) && Toshiba_Data.HalfBit == 1)
            {
              Toshiba_Data.Code <<= 1;
              Toshiba_Data.Bit++;
              Toshiba_Data.HalfBit = 0;
              ignored = false;
            }
            else if (!pulse && IsBetween(duration, 1500, 1900) && Toshiba_Data.HalfBit == 1)
            {
              Toshiba_Data.Code <<= 1;
              Toshiba_Data.Code |= 1;
              Toshiba_Data.Bit++;
              Toshiba_Data.HalfBit = 0;
              ignored = false;
            }
            else if (!pulse && duration >= 6100 && Toshiba_Data.HalfBit == 1 && Toshiba_Data.Bit == 32)
            {
              uint custom       = (Toshiba_Data.Code >> 24) & 0xFF;
              uint repeatCustom = (Toshiba_Data.Code >> 16) & 0xFF;

              uint data         = (Toshiba_Data.Code >> 8) & 0xFF;
              uint notData      = Toshiba_Data.Code & 0xFF;

              if (custom == repeatCustom && (data + notData == 0xFF))
              {
                uint code = (custom << 8) + data;
                remoteCallback(IrProtocol.Toshiba, code, false);
                Toshiba_Data.State = RemoteDetectionState.HeaderPulse;
                ignored = false;
              }
            }

            break;
          #endregion Data

        }

        if (ignored && (Toshiba_Data.State != RemoteDetectionState.HeaderPulse))
          Toshiba_Data.State = RemoteDetectionState.HeaderPulse;
      }
    }

    static void DetectMCE(int[] timingData, KeyboardCallback keyboardCallback, MouseCallback mouseCallback)
    {
      // Mouse:    0 0001 xxxxxxxxxxxxxxxxxxxxxxxxxxxxx
      // Keyboard: 0 0100 xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

      const int HalfBit_None = 0;
      const int HalfBit_Zero = 1;
      const int HalfBit_One = 2;

      for (int i = 0; i < timingData.Length; i++)
      {
        int duration = Math.Abs(timingData[i]);
        bool pulse = (timingData[i] > 0);

        #region Working data ...
        if (MCE_Data.State != MceKeyboardDetectState.Header)
        {
          switch (MCE_Data.HalfBit)
          {

            #region HalfBit_None
            case HalfBit_None:
              if (IsBetween(duration, 100, 450))
              {
                MCE_Data.HalfBit = (pulse ? HalfBit_One : HalfBit_Zero);
              }
              else if (IsBetween(duration, 500, 800))
              {
                //Trace.WriteLine("Bad bit sequence double {0}", pulse);
                MCE_Data.HalfBit = (pulse ? HalfBit_One : HalfBit_Zero);
                //MCE_Data = new MceDetectionData();
                return;
              }
              else
              {
                // Over Length duration (Treat as a Zero bit)
                //Trace.WriteLine("Bad duration {0}", duration);
                MCE_Data.Working <<= 1;
                MCE_Data.Bit--;
              }
              break;
            #endregion HalfBit_None

            #region HalfBit_Zero
            case HalfBit_Zero:
              if (IsBetween(duration, 100, 450))
              {
                if (pulse)
                {
                  MCE_Data.Working <<= 1;
                  MCE_Data.Bit--;
                  MCE_Data.HalfBit = HalfBit_None;
                }
                else
                {
                  //Trace.WriteLine("Bad bit sequence 00");
                  MCE_Data = new MceDetectionData();
                  //return;
                }
              }
              else if (IsBetween(duration, 500, 800))
              {
                if (pulse)
                {
                  MCE_Data.Working <<= 1;
                  MCE_Data.Bit--;
                  MCE_Data.HalfBit = HalfBit_One;
                }
                else
                {
                  //Trace.WriteLine("Bad bit sequence 00 0");
                  MCE_Data = new MceDetectionData();
                  //return;
                }
              }
              else
              {
                //Trace.WriteLine("Bad duration {0}", duration);
                if (MCE_Data.Bit == 1)
                {
                  MCE_Data.Working <<= 1;
                  //MceKeyboard_Data.Working |= 1;
                  MCE_Data.Bit--;
                  MCE_Data.HalfBit = HalfBit_None;
                  //i--;
                }
                else
                {
                  MCE_Data = new MceDetectionData();
                }
                //return;
              }
              break;
            #endregion HalfBit_Zero

            #region HalfBit_One
            case HalfBit_One:
              if (IsBetween(duration, 100, 450))
              {
                if (!pulse)
                {
                  MCE_Data.Working <<= 1;
                  MCE_Data.Working |= 1;
                  MCE_Data.Bit--;
                  MCE_Data.HalfBit = HalfBit_None;
                }
                else
                {
                  //Trace.WriteLine("Bad bit sequence 11");
                  MCE_Data = new MceDetectionData();
                  //return;
                }
              }
              else if (IsBetween(duration, 500, 800))
              {
                if (!pulse)
                {
                  MCE_Data.Working <<= 1;
                  MCE_Data.Working |= 1;
                  MCE_Data.Bit--;
                  MCE_Data.HalfBit = HalfBit_Zero;
                }
                else
                {
                  //Trace.WriteLine("Bad bit sequence 11 1");
                  MCE_Data = new MceDetectionData();
                  //return;
                }
              }
              else
              {
                //Trace.WriteLine("Bad duration {0}", duration);
                if (MCE_Data.Bit == 1)
                {
                  MCE_Data.Working <<= 1;
                  MCE_Data.Working |= 1;
                  MCE_Data.Bit--;
                  MCE_Data.HalfBit = HalfBit_None;
                  //i--;
                }
                else
                {
                  MCE_Data = new MceDetectionData();
                  //return;
                }
              }
              break;
            #endregion HalfBit_One

          }
        }
        #endregion Working data ...

        switch (MCE_Data.State)
        {

          #region Header
          case MceKeyboardDetectState.Header:
            if (pulse && IsBetween(duration, 2600, 3300))
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
                //Trace.WriteLine("KB: Invalid Type {0}", MceKeyboard_Data.Type);
                return;
              }
            }

            break;
          #endregion CodeType


          #region Keyboard

          #region KeyboardIgnore
          case MceKeyboardDetectState.KeyboardIgnore:
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
            if (MCE_Data.Bit == 0)
            {
              MCE_Data.DeltaY = ScaleMouseDelta((int)MCE_Data.Working);

              MCE_Data.State = MceKeyboardDetectState.DeltaX;
              MCE_Data.Bit = 7;
              MCE_Data.Working = 0;
            }

            break;
          #endregion DeltaY

          #region DeltaX
          case MceKeyboardDetectState.DeltaX:
            if (MCE_Data.Bit == 0)
            {
              MCE_Data.DeltaX = ScaleMouseDelta((int)MCE_Data.Working);

              MCE_Data.State = MceKeyboardDetectState.Right;
              MCE_Data.Bit = 1;
              MCE_Data.Working = 0;
            }

            break;
          #endregion DeltaX

          #region Right
          case MceKeyboardDetectState.Right:
            if (MCE_Data.Bit == 0)
            {
              MCE_Data.Right = (MCE_Data.Working == 1);

              MCE_Data.State = MceKeyboardDetectState.Left;
              MCE_Data.Bit = 1;
              MCE_Data.Working = 0;
            }

            break;
          #endregion Right

          #region Left
          case MceKeyboardDetectState.Left:
            if (MCE_Data.Bit == 0)
            {
              MCE_Data.Left = (MCE_Data.Working == 1);

              MCE_Data.State = MceKeyboardDetectState.Checksum;
              MCE_Data.Bit = 5;
              MCE_Data.Working = 0;
            }

            break;
          #endregion Left

          #region Checksum
          case MceKeyboardDetectState.Checksum:
            if (MCE_Data.Bit == 0)
            {
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

    static int ScaleMouseDelta(int delta)
    {
      int scaledDelta = delta;
      if (delta >= 0x62)
        scaledDelta -= 0x80;

      return scaledDelta;
    }

    static bool IsBetween(int test, int minValue, int maxValue)
    {
      return ((test >= minValue) && (test <= maxValue));
    }

    #endregion Methods

  }

}
