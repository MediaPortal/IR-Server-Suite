using System.Windows.Forms;

namespace WebRemote
{
  internal struct RemoteButton
  {
    #region Variables

    private string _code;
    private int _height;
    private int _left;
    private string _name;
    private Keys _shortcut;
    private int _top;
    private int _width;

    #endregion Variables

    #region Properties

    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    public string Code
    {
      get { return _code; }
      set { _code = value; }
    }

    public Keys Shortcut
    {
      get { return _shortcut; }
      set { _shortcut = value; }
    }

    public int Top
    {
      get { return _top; }
      set { _top = value; }
    }

    public int Left
    {
      get { return _left; }
      set { _left = value; }
    }

    public int Width
    {
      get { return _width; }
      set { _width = value; }
    }

    public int Height
    {
      get { return _height; }
      set { _height = value; }
    }

    #endregion Properties
  }
}