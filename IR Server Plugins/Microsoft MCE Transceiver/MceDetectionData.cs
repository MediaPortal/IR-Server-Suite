using System;

namespace MceTransceiver
{

  #region Enumerations

  public enum MceKeyboardDetectState
  {
    Header,
    CodeType,

    KeyboardIgnore,
    KeyCode,
    Modifiers,

    MouseIgnore,
    DeltaY,
    DeltaX,
    Right,
    Left,
    Checksum,
  }

  [Flags]
  public enum KeyModifiers : uint
  {
    None = 0,
    LeftControl = 1,
    LeftShift = 2,
    LeftAlt = 4,
    LeftWin = 8,
    RightControl = 16,
    RightShift = 32,
    RightAlt = 64,
    RightWin = 128,
  }

  #endregion Enumerations

  public class MceDetectionData
  {

    #region Members

    MceKeyboardDetectState _state = MceKeyboardDetectState.Header;
    uint _type = 0;

    uint _keyCode = 0;
    uint _modifiers = 0;

    int _deltaY = 0;
    int _deltaX = 0;
    bool _right = false;
    bool _left = false;

    int _bit = 0;

    int _halfBit = 0;
    uint _working = 0;
    
    #endregion Members

    #region Properties
    
    public MceKeyboardDetectState State
    {
      get { return _state; }
      set { _state = value; }
    }
    public uint Type
    {
      get { return _type; }
      set { _type = value; }
    }

    public uint KeyCode
    {
      get { return _keyCode; }
      set { _keyCode = value; }
    }
    public uint Modifiers
    {
      get { return _modifiers; }
      set { _modifiers = value; }
    }

    public int DeltaY
    {
      get { return _deltaY; }
      set { _deltaY = value; }
    }
    public int DeltaX
    {
      get { return _deltaX; }
      set { _deltaX = value; }
    }
    public bool Right
    {
      get { return _right; }
      set { _right = value; }
    }
    public bool Left
    {
      get { return _left; }
      set { _left = value; }
    }

    public int Bit
    {
      get { return _bit; }
      set { _bit = value; }
    }
    public int HalfBit
    {
      get { return _halfBit; }
      set { _halfBit = value; }
    }
    public uint Working
    {
      get { return _working; }
      set { _working = value; }
    }

    #endregion Properties

    #region Constructors

    public MceDetectionData() : this(MceKeyboardDetectState.Header) { }
    public MceDetectionData(MceKeyboardDetectState state) { _state = state; }

    #endregion Constructors

  }

}
