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
using MediaPortal.Hardware;

namespace MediaPortal.Plugins
{
  internal class MappedKeyCode
  {
    #region Variables

    private RemoteButton _button;
    private string _keyCode;

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

    public MappedKeyCode() : this(RemoteButton.None, String.Empty)
    {
    }

    public MappedKeyCode(string button, string keyCode)
      : this((RemoteButton) Enum.Parse(typeof (RemoteButton), button, true), keyCode)
    {
    }

    public MappedKeyCode(RemoteButton button, string keyCode)
    {
      _button = button;
      _keyCode = keyCode;
    }

    #endregion Constructors
  }
}