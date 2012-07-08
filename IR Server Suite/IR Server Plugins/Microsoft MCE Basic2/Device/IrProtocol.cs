namespace IRServer.Plugin
{
  /// <summary>
  /// Protocol of IR Code.
  /// </summary>
  internal enum IrProtocol
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
    /// RC6-6-16 protocol variation of Philips RC6. 36kHz carrier.  Used by some Vista MCE remotes (I think).
    /// </summary>
    RC6_16,
    /// <summary>
    /// RC6-6-20 protocol variation of Philips RC6. 36kHz carrier.  Used by Pace Foxtel STB's.
    /// </summary>
    RC6_20,
    /// <summary>
    /// RC6-6-24 protocol variation of Philips RC6. 36kHz carrier.  RC6A.
    /// </summary>
    RC6_24,
    /// <summary>
    /// RC6-6-32 protocol variation of Philips RC6. 36kHz carrier.  Used by Microsoft MCE remote.
    /// </summary>
    RC6_32,
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
}