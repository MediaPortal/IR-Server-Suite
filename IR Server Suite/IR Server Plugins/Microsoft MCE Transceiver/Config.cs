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
  public partial class MicrosoftMceTransceiver
  {
    public class Config
    {
      public bool _disableAutomaticButtons { get; set; }
      public bool _disableMceServices { get; set; }
      public bool _enableKeyboardInput { get; set; }
      public bool _enableMouseInput { get; set; }
      public bool _enableRemoteInput { get; set; }
      public bool _handleKeyboardLocally { get; set; }
      public bool _handleMouseLocally { get; set; }
      public int _keyboardFirstRepeat { get; set; }
      public int _keyboardHeldRepeats { get; set; }
      public int _learnTimeout { get; set; }
      public double _mouseSensitivity { get; set; }
      public int _remoteFirstRepeat { get; set; }
      public int _remoteHeldRepeats { get; set; }
      public bool _useQwertzLayout { get; set; }
      public bool _useSystemRatesKeyboard { get; set; }
      public bool _useSystemRatesRemote { get; set; }

      public Config()
      {
        this.Reset();
      }

      public bool Reset()
      {
        try
        {
          _disableAutomaticButtons = false;
          _disableMceServices = true;

          _enableKeyboardInput = false;

          _enableMouseInput = false;
          _enableRemoteInput = true;
          _handleKeyboardLocally = true;
          _handleMouseLocally = true;
          _keyboardFirstRepeat = 350;
          _keyboardHeldRepeats = 0;
          _learnTimeout = 10000;
          _mouseSensitivity = 1.0d;
          _remoteFirstRepeat = 400;
          _remoteHeldRepeats = 250;
          _useQwertzLayout = false;
          _useSystemRatesKeyboard = true;
          _useSystemRatesRemote = false;
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