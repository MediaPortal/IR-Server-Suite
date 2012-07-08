#region Copyright (C) 2005-2012 Team MediaPortal

// Copyright (C) 2005-2012 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.

#endregion

namespace IRServer.Plugin
{
  public partial class MCEBasic
  {
    public class Config
    {
      public bool DoRepeats { get; set; }

      public bool UseSystemRatesDelay { get; set; }

      public int FirstRepeatDelay { get; set; }

      public int HeldRepeatDelay { get; set; }

      public Config()
      {
        this.Reset();
      }

      public bool Reset()
      {
        try
        {
          DoRepeats = false;
          UseSystemRatesDelay = true;
          FirstRepeatDelay = 400;
          HeldRepeatDelay = 100;
          return true;
        }
        catch
        {
          return false;
        }
      }
    }

    public class Config2
    {
      public bool _disableAutomaticButtons { get; set; }
      public bool _disableMceServices { get; set; }
      public bool EnableKeyboardInput { get; set; }
      public bool EnableMouseInput { get; set; }
      public bool EnableRemoteInput { get; set; }
      public bool HandleKeyboardLocally { get; set; }
      public bool HandleMouseLocally { get; set; }
      public int KeyboardFirstRepeat { get; set; }
      public int KeyboardHeldRepeats { get; set; }
      public int LearnTimeout { get; set; }
      public double MouseSensitivity { get; set; }
      public int RemoteFirstRepeat { get; set; }
      public int RemoteHeldRepeats { get; set; }
      public bool UseQwertzLayout { get; set; }
      public bool UseSystemRatesKeyboard { get; set; }
      public bool UseSystemRatesRemote { get; set; }

      public Config2()
      {
        this.Reset();
      }

      public bool Reset()
      {
        try
        {
          _disableAutomaticButtons = false;
          _disableMceServices = true;

          EnableKeyboardInput = false;

          EnableMouseInput = false;
          EnableRemoteInput = true;
          HandleKeyboardLocally = true;
          HandleMouseLocally = true;
          KeyboardFirstRepeat = 350;
          KeyboardHeldRepeats = 0;
          LearnTimeout = 10000;
          MouseSensitivity = 1.0d;
          RemoteFirstRepeat = 400;
          RemoteHeldRepeats = 250;
          UseQwertzLayout = false;
          UseSystemRatesKeyboard = true;
          UseSystemRatesRemote = false;
          return true;
        }
        catch
        {
          return false;
        }
      }
    }
  }
}