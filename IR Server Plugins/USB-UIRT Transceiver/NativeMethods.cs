using System;
using System.Runtime.InteropServices;
using System.Text;

namespace UirtTransceiver
{

  /// <summary>
  /// Native Methods Class.
  /// </summary>
  internal static class NativeMethods
  {

    #region Interop

    [StructLayout(LayoutKind.Sequential)]
    internal struct UUINFO
    {
      public int fwVersion;
      public int protVersion;
      public char fwDateDay;
      public char fwDateMonth;
      public char fwDateYear;
    }

    //Not used
    //[StructLayout(LayoutKind.Sequential)]
    //internal struct UUGPIO
    //{
    //  public byte[] irCode;
    //  public byte action;
    //  public byte duration;
    //}

    [DllImport("uuirtdrv.dll")]
    internal static extern IntPtr UUIRTOpen();

    [DllImport("uuirtdrv.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool UUIRTClose(
      IntPtr hHandle);

    //[DllImport("uuirtdrv.dll")]
    //[return: MarshalAs(UnmanagedType.Bool)]
    //internal static extern bool UUIRTGetDrvInfo(ref int puDrvVersion);

    //[DllImport("uuirtdrv.dll")]
    //[return: MarshalAs(UnmanagedType.Bool)]
    //internal static extern bool UUIRTGetUUIRTInfo(
    //  IntPtr hHandle,
    //  ref UUINFO puuInfo);

    //[DllImport("uuirtdrv.dll")]
    //[return: MarshalAs(UnmanagedType.Bool)]
    //internal static extern bool UUIRTGetUUIRTConfig(
    //  IntPtr hHandle,
    //  ref uint puConfig);

    //[DllImport("uuirtdrv.dll")]
    //[return: MarshalAs(UnmanagedType.Bool)]
    //internal static extern bool UUIRTSetUUIRTConfig(
    //  IntPtr hHandle,
    //  uint uConfig);

    [DllImport("uuirtdrv.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool UUIRTTransmitIR(
      IntPtr hHandle,
      string IRCode,
      int codeFormat,
      int repeatCount,
      int inactivityWaitTime,
      IntPtr hEvent,
      int res1,
      int res2);

    [DllImport("uuirtdrv.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool UUIRTLearnIR(
      IntPtr hHandle,
      int codeFormat,
      //[MarshalAs(UnmanagedType.LPStr)]
      StringBuilder ircode,
      IRLearnCallbackDelegate progressProc,
      IntPtr userData,
      IntPtr abort,
      int param1,
      IntPtr reserved1,
      IntPtr reserved2);

    [DllImport("uuirtdrv.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool UUIRTSetReceiveCallback(
      IntPtr hHandle,
      UUIRTReceiveCallbackDelegate receiveProc,
      int none);

    //[DllImport("uuirtdrv.dll")]
    //internal static extern bool UUIRTSetUUIRTGPIOCfg(IntPtr hHandle, int index, ref UUGPIO GpioSt);

    //[DllImport("uuirtdrv.dll")]
    //internal static extern bool UUIRTGetUUIRTGPIOCfg(IntPtr hHandle, ref int numSlots, ref uint dwPortPins,
    //                                                ref UUGPIO GpioSt);

    #endregion

    #region Delegates

    internal delegate void UUIRTReceiveCallbackDelegate(string irCode, IntPtr userData);
    internal delegate void IRLearnCallbackDelegate(uint progress, uint sigQuality, ulong carrierFreq, IntPtr userData);

    #endregion Delegates

  }

}
