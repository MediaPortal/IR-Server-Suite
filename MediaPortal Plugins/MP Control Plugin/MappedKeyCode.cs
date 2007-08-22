using System;

using MediaPortal.Hardware;

namespace MediaPortal.Plugins
{

  [CLSCompliant(false)]
  public class MappedKeyCode
  {

    #region Variables

    RemoteButton _button;
    string _keyCode;
    
    #endregion Variables

    #region Properties

    public RemoteButton Button
    {
      get { return _button; }
      set { _button = value; }
    }
    public string KeyCode
    {
      get { return _keyCode; }
      set { _keyCode = value; }
    }

    #endregion Properties

    #region Constructors

    public MappedKeyCode() : this(RemoteButton.None, String.Empty) { }
    public MappedKeyCode(string button, string keyCode) : this((RemoteButton)Enum.Parse(typeof(RemoteButton), button, true), keyCode) { }
    public MappedKeyCode(RemoteButton button, string keyCode)
    {
      _button   = button;
      _keyCode  = keyCode;
    }

    #endregion Constructors

  }

}
