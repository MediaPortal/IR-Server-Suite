using System;
using System.Windows.Forms;

namespace VirtualRemote
{

  /// <summary>
  /// Data structure for Virtual Remote button representation.
  /// </summary>
  struct RemoteButton
  {

    #region Variables

    string _name;
    string _code;
    Keys _shortcut;
    int _top;
    int _left;
    int _width;
    int _height;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }
    /// <summary>
    /// Gets or sets the code.
    /// </summary>
    /// <value>The code.</value>
    public string Code
    {
      get { return _code; }
      set { _code = value; }
    }
    /// <summary>
    /// Gets or sets the shortcut.
    /// </summary>
    /// <value>The shortcut.</value>
    public Keys Shortcut
    {
      get { return _shortcut; }
      set { _shortcut = value; }
    }
    /// <summary>
    /// Gets or sets the distance from top.
    /// </summary>
    /// <value>The distance from top.</value>
    public int Top
    {
      get { return _top; }
      set { _top = value; }
    }
    /// <summary>
    /// Gets or sets the distance from left.
    /// </summary>
    /// <value>The distance from left.</value>
    public int Left
    {
      get { return _left; }
      set { _left = value; }
    }
    /// <summary>
    /// Gets or sets the width.
    /// </summary>
    /// <value>The width.</value>
    public int Width
    {
      get { return _width; }
      set { _width = value; }
    }
    /// <summary>
    /// Gets or sets the height.
    /// </summary>
    /// <value>The height.</value>
    public int Height
    {
      get { return _height; }
      set { _height = value; }
    }

    #endregion Properties

  }

}
