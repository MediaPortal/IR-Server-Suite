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
//using MediaPortal.Hardware;

public enum RemoteButton
{
    // MCE MediaPortal.Hardware default keycode
    NumPad0 = 0,
    None = 0,
    NumPad1 = 1,
    NumPad2 = 2,
    NumPad3 = 3,
    NumPad4 = 4,
    NumPad5 = 5,
    NumPad6 = 6,
    NumPad7 = 7,
    NumPad8 = 8,
    NumPad9 = 9,
    Clear = 10,
    Enter = 11,
    Power2 = 12,
    Start = 13,
    Mute = 14,
    Info = 15,
    VolumeUp = 16,
    VolumeDown = 17,
    ChannelUp = 18,
    ChannelDown = 19,
    Forward = 20,
    Rewind = 21,
    Play = 22,
    Record = 23,
    Pause = 24,
    Stop = 25,
    Skip = 26,
    Replay = 27,
    OemGate = 28,
    Oem8 = 29,
    Up = 30,
    Down = 31,
    Left = 32,
    Right = 33,
    Ok = 34,
    Back = 35,
    DVDMenu = 36,
    LiveTV = 37,
    Guide = 38,
    AspectRatio = 39,
    MyTV = 70,
    MyMusic = 71,
    RecordedTV = 72,
    MyPictures = 73,
    MyVideos = 74,
    Print = 78,
    MyRadio = 80,
    Teletext = 90,
    Red = 91,
    Green = 92,
    Yellow = 93,
    Blue = 94,
    PowerTV = 101,
    Messenger = 105,
    Power1 = 165,
    // Imon specific keycode
    Open = 200,
    WinStart = 201,
    WinMenu = 202,
    LeftClick = 203,
    RightClick = 204,
    Esc = 205,
    Eject = 206,
    AppLauncher = 207,
    TaskSwitcher = 208,
    Timer = 209,
    ShiftTab = 210,
    Tab = 211,
    Bookmark = 212,
    Thumbnail = 213,
    FullScreen = 214,
    MyDVD = 215,
    Menu = 216,
    Caption = 217,
    Language = 218,
    Custom1 = 301,
    Custom2 = 302,
    Custom3 = 303,
    Custom4 = 304,
    Custom5 = 305,
}

namespace MediaPortal.Plugins.IRSS.MPControlPlugin
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