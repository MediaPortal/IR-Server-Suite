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

namespace IRServer.Plugin
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
    #region Properties

    public MceKeyboardDetectState State { get; set; }
    public uint Type { get; set; }
    public uint KeyCode { get; set; }
    public uint Modifiers { get; set; }
    public int DeltaY { get; set; }
    public int DeltaX { get; set; }
    public bool Right { get; set; }
    public bool Left { get; set; }
    public int Bit { get; set; }
    public int HalfBit { get; set; }
    public uint Working { get; set; }

    #endregion Properties

    #region Constructors

    public MceDetectionData() : this(MceKeyboardDetectState.Header)
    {
    }

    public MceDetectionData(MceKeyboardDetectState state)
    {
      State = state;
    }

    #endregion Constructors
  }
}