using System;
using System.Globalization;

namespace InputService.Plugin
{

  /// <summary>
  /// Class containing information on a WinLIRC command
  /// </summary>
  class WinLircCommand
  {

    #region Variables

    string _remote;
    string _button;
    int _repeats;
    DateTime _time;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Gets the button.
    /// </summary>
    /// <value>The button.</value>
    public string Button { get { return _button; } }
    /// <summary>
    /// Gets the remote.
    /// </summary>
    /// <value>The remote.</value>
    public string Remote { get { return _remote; } }
    /// <summary>
    /// Gets the repeats reported by LIRC.
    /// </summary>
    /// <value>The repeat count.</value>
    public int Repeats { get { return _repeats; } }
    /// <summary>
    /// Gets the time.
    /// </summary>
    /// <value>The time.</value>
    public DateTime Time { get { return _time; } }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="WinLircCommand"/> class.
    /// </summary>
    public WinLircCommand()
    {
      _time     = DateTime.Now;
      _remote   = String.Empty;
      _repeats  = 0;
      _button   = String.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WinLircCommand"/> class.
    /// </summary>
    /// <param name="data">The data.</param>
    public WinLircCommand(string data) : this()
    {
      string[] dataElements = data.Split(' ');
      
      _repeats  = Int32.Parse(dataElements[1], NumberStyles.HexNumber);
      _button   = dataElements[2];
      _remote   = dataElements[3];
    }

    #endregion Constructors

    /// <summary>
    /// Determines whether this command is same the same as the specified second.
    /// </summary>
    /// <param name="second">The second command to compare with.</param>
    /// <returns><c>true</c> if this command is the same as the specified second; otherwise, <c>false</c>.</returns>
    public bool IsSameCommand(WinLircCommand second)
    {
      if (second == null)
          return false;

      return (_button.Equals(second.Button, StringComparison.Ordinal) && _remote.Equals(second.Remote, StringComparison.Ordinal));
    }

  }

}
