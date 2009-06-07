using System;

namespace InputService.Plugin
{

  #region Enumerations

  internal enum MceKeyboardDetectState
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
  internal enum KeyModifiers
  {
    None = 0x00,
    LeftControl = 0x01,
    LeftShift = 0x02,
    LeftAlt = 0x04,
    LeftWin = 0x08,
    RightControl = 0x10,
    RightShift = 0x20,
    RightAlt = 0x40,
    RightWin = 0x80,
  }

  #endregion Enumerations

  internal class MceDetectionData
  {
    #region Members

    private int _bit;
    private int _deltaX;
    private int _deltaY;
    private int _halfBit;
    private uint _keyCode;
    private bool _left;
    private uint _modifiers;

    private bool _right;
    private MceKeyboardDetectState _state = MceKeyboardDetectState.Header;
    private uint _type;
    private uint _working;

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

    public MceDetectionData() : this(MceKeyboardDetectState.Header)
    {
    }

    public MceDetectionData(MceKeyboardDetectState state)
    {
      _state = state;
    }

    #endregion Constructors
  }
}