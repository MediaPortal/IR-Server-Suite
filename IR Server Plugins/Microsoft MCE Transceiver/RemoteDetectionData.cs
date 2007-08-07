using System;

namespace MicrosoftMceTransceiver
{

  #region Enumerations

  public enum RemoteDetectionState
  {
    HeaderPulse,
    HeaderSpace,
    PreData,
    Data,
    KeyCode,
    Leading
  }

  #endregion Enumerations

  public class RemoteDetectionData
  {

    #region Member Variables

    RemoteDetectionState _state = RemoteDetectionState.HeaderPulse;
    byte _bit = 0;
    byte _halfBit = 0;
    uint _code = 0;
    uint _header = 0;
    bool _longPulse = false;
    bool _longSpace = false;

    #endregion Member Variables

    #region Properties

    public RemoteDetectionState State
    {
      get { return _state; }
      set { _state = value; }
    }

    public byte Bit
    {
      get { return _bit; }
      set { _bit = value; }
    }
    public byte HalfBit
    {
      get { return _halfBit; }
      set { _halfBit = value; }
    }
    public uint Code
    {
      get { return _code; }
      set { _code = value; }
    }

    public uint Header
    {
      get { return _header; }
      set { _header = value; }
    }

    public bool LongPulse
    {
      get { return _longPulse; }
      set { _longPulse = value; }
    }
    public bool LongSpace
    {
      get { return _longSpace; }
      set { _longSpace = value; }
    }

    #endregion Properties

    #region Constructors

    public RemoteDetectionData() : this(RemoteDetectionState.HeaderPulse) { }
    public RemoteDetectionData(RemoteDetectionState state) { _state = state; }

    #endregion Constructors

  }

}
