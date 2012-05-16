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

namespace IRServer.Plugin
{

  #region Enumerations

  internal enum RemoteDetectionState
  {
    HeaderPulse,
    HeaderSpace,
    PreData,
    Data,
    KeyCode,
    Leading
  }

  #endregion Enumerations

  internal class RemoteDetectionData
  {
    #region Properties

    public RemoteDetectionState State { get; set; }
    public byte Bit { get; set; }
    public byte HalfBit { get; set; }
    public uint Code { get; set; }
    public uint Header { get; set; }
    public bool LongPulse { get; set; }
    public bool LongSpace { get; set; }
    public int Toggle { get; set; }

    #endregion Properties

    #region Constructors

    public RemoteDetectionData() : this(RemoteDetectionState.HeaderPulse)
    {
    }

    public RemoteDetectionData(RemoteDetectionState state)
    {
      State = state;
    }

    #endregion Constructors
  }
}