#region Copyright (C) 2005-2009 Team MediaPortal

// Copyright (C) 2005-2009 Team MediaPortal
// http://www.team-mediaportal.com
// 
// This Program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2, or (at your option)
// any later version.
// 
// This Program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with GNU Make; see the file COPYING.  If not, write to
// the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
// http://www.gnu.org/copyleft/gpl.html

#endregion

using System;
using System.Globalization;

namespace InputService.Plugin
{
  /// <summary>
  /// Class containing information on a WinLIRC command
  /// </summary>
  internal class WinLircCommand
  {
    #region Variables

    private readonly string _button;
    private readonly string _remote;
    private readonly int _repeats;
    private readonly DateTime _time;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Gets the button.
    /// </summary>
    /// <value>The button.</value>
    public string Button
    {
      get { return _button; }
    }

    /// <summary>
    /// Gets the remote.
    /// </summary>
    /// <value>The remote.</value>
    public string Remote
    {
      get { return _remote; }
    }

    /// <summary>
    /// Gets the repeats reported by LIRC.
    /// </summary>
    /// <value>The repeat count.</value>
    public int Repeats
    {
      get { return _repeats; }
    }

    /// <summary>
    /// Gets the time.
    /// </summary>
    /// <value>The time.</value>
    public DateTime Time
    {
      get { return _time; }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="WinLircCommand"/> class.
    /// </summary>
    public WinLircCommand()
    {
      _time = DateTime.Now;
      _remote = String.Empty;
      _repeats = 0;
      _button = String.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WinLircCommand"/> class.
    /// </summary>
    /// <param name="data">The data.</param>
    public WinLircCommand(string data) : this()
    {
      string[] dataElements = data.Split(' ');

      _repeats = Int32.Parse(dataElements[1], NumberStyles.HexNumber);
      _button = dataElements[2];
      _remote = dataElements[3];
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

      return (_button.Equals(second.Button, StringComparison.Ordinal) &&
              _remote.Equals(second.Remote, StringComparison.Ordinal));
    }
  }
}