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
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using InputService.Plugin.Properties;

namespace InputService.Plugin
{
  /// <summary>
  /// Input Service Plugin to capture ALL keyboard button presses and basic key combinations and forward as remote control commands.
  /// </summary>
  public class KeyboardInput : PluginBase, IRemoteReceiver
  {
    #region Interop

    [DllImport("user32.dll")]
    private static extern IntPtr SetWindowsHookEx(HookType code, HookDelegate func, IntPtr hInstance, int threadID);

    [DllImport("user32.dll")]
    private static extern int UnhookWindowsHookEx(IntPtr hhook);

    [DllImport("user32.dll")]
    private static extern int CallNextHookEx(IntPtr hhook, int code, int wParam, IntPtr lParam);

    [DllImport("kernel32.dll")]
    private static extern IntPtr LoadLibrary(string lpFileName);

    #endregion Interop

    // #define TEST_APPLICATION in the project properties when creating the console test app ...
#if TEST_APPLICATION

    static void Remote(string deviceName, string code)
    {
      Console.WriteLine("Remote: {0}", code);
    }

    [STAThread]
    static void Main()
    {
      KeyboardInput c;

      try
      {
        c = new KeyboardInput();

        c.RemoteCallback += new RemoteHandler(Remote);

        c.Start();

        Application.Run();

        c.Stop();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
      finally
      {
        c = null;
      }

      Console.ReadKey();
    }

#endif

    #region Nested type: HookDelegate

    private delegate int HookDelegate(int code, int wParam, IntPtr lParam);

    #endregion

    #region Enumerations

    #region Nested type: AppCommands

    private enum AppCommands
    {
      None = 0,
      BrowserBackward = 1,
      BrowserForward = 2,
      BrowserRefresh = 3,
      BrowserStop = 4,
      BrowserSearch = 5,
      BrowserFavorites = 6,
      BrowserHome = 7,
      VolumeMute = 8,
      VolumeDown = 9,
      VolumeUp = 10,
      MediaNextTrack = 11,
      MediaPreviousTrack = 12,
      MediaStop = 13,
      MediaPlayPause = 14,
      LaunchMail = 15,
      LaunchMediaSelect = 16,
      LaunchApp1 = 17,
      LaunchApp2 = 18,
      BassDown = 19,
      BassBoost = 20,
      BassUp = 21,
      TrebleDown = 22,
      TrebleUp = 23,
      MicrophoneVolumeMute = 24,
      MicrophoneVolumeDown = 25,
      MicrophoneVolumeUp = 26,
      Help = 27,
      Find = 28,
      New = 29,
      Open = 30,
      Close = 31,
      Save = 32,
      Print = 33,
      Undo = 34,
      Redo = 35,
      Copy = 36,
      Cut = 37,
      Paste = 38,
      ReplyToMail = 39,
      ForwardMail = 40,
      SendMail = 41,
      SpellCheck = 42,
      DictateOrCommandControlToggle = 43,
      MicrophoneOnOffToggle = 44,
      CorrectionList = 45,
      MediaPlay = 46,
      MediaPause = 47,
      MediaRecord = 48,
      MediaFastForward = 49,
      MediaRewind = 50,
      MediaChannelUp = 51,
      MediaChannelDown = 52,
      Delete = 53,
      Flip3D = 54,
    }

    #endregion

    #region Nested type: HookType

    private enum HookType
    {
      WH_JOURNALRECORD = 0,
      WH_JOURNALPLAYBACK = 1,
      WH_KEYBOARD = 2,
      WH_GETMESSAGE = 3,
      WH_CALLWNDPROC = 4,
      WH_CBT = 5,
      WH_SYSMSGFILTER = 6,
      WH_MOUSE = 7,
      WH_HARDWARE = 8,
      WH_DEBUG = 9,
      WH_SHELL = 10,
      WH_FOREGROUNDIDLE = 11,
      WH_CALLWNDPROCRET = 12,
      WH_KEYBOARD_LL = 13,
      WH_MOUSE_LL = 14
    }

    #endregion

    #endregion Enumerations

    #region Structures

    private struct KeyboardHookStruct
    {
      public readonly int dwExtraInfo;
      public readonly int flags;
      public readonly int scanCode;
      public readonly int time;
      public readonly int virtualKey;

      /// <summary>
      /// Initializes a new instance of the <see cref="KeyboardHookStruct"/> struct.
      /// </summary>
      /// <param name="lParam">The lParam to derive from.</param>
      public KeyboardHookStruct(IntPtr lParam)
      {
        KeyboardHookStruct khs = (KeyboardHookStruct) Marshal.PtrToStructure(lParam, typeof (KeyboardHookStruct));

        virtualKey = khs.virtualKey;
        scanCode = khs.scanCode;
        flags = khs.flags;
        time = khs.time;
        dwExtraInfo = khs.dwExtraInfo;
      }
    }

    #endregion Structures

    #region Variables

    private HookDelegate _hookDelegate;
    private IntPtr _hookHandle;
    private IntPtr _libPtr;
    private RemoteHandler _remoteButtonHandler;
    private bool _stealAppCommands = true;

    #endregion Variables

    #region Implementation

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name
    {
      get { return "Keyboard Input"; }
    }

    /// <summary>
    /// IR Server plugin version.
    /// </summary>
    /// <value>The version.</value>
    public override string Version
    {
      get { return "1.4.2.0"; }
    }

    /// <summary>
    /// The IR Server plugin's author.
    /// </summary>
    /// <value>The author.</value>
    public override string Author
    {
      get { return "and-81"; }
    }

    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    /// <value>The description.</value>
    public override string Description
    {
      get { return "Captures ALL keyboard button presses and basic key combinations as remote control commands"; }
    }

    /// <summary>
    /// Gets the plugin icon.
    /// </summary>
    /// <value>The plugin icon.</value>
    public override Icon DeviceIcon
    {
      get { return Resources.Icon; }
    }

    /// <summary>
    /// Callback for remote button presses.
    /// </summary>
    /// <value>The remote callback.</value>
    public RemoteHandler RemoteCallback
    {
      get { return _remoteButtonHandler; }
      set { _remoteButtonHandler = value; }
    }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    public override void Start()
    {
      _hookDelegate = InternalHookDelegate;
      _libPtr = LoadLibrary("User32");
      _hookHandle = SetWindowsHookEx(HookType.WH_KEYBOARD_LL, _hookDelegate, _libPtr, 0);
    }

    /// <summary>
    /// Suspend the IR Server plugin when computer enters standby.
    /// </summary>
    public override void Suspend()
    {
      Stop();
    }

    /// <summary>
    /// Resume the IR Server plugin when the computer returns from standby.
    /// </summary>
    public override void Resume()
    {
      Start();
    }

    /// <summary>
    /// Stop the IR Server plugin.
    /// </summary>
    public override void Stop()
    {
      UnhookWindowsHookEx(_hookHandle);

      _hookHandle = IntPtr.Zero;
      _hookDelegate = null;
      _libPtr = IntPtr.Zero;
    }


    private int InternalHookDelegate(int code, int wParam, IntPtr lParam)
    {
      if (code >= 0 && wParam == 256)
      {
        KeyboardHookStruct khs = new KeyboardHookStruct(lParam);
        int keyCode = khs.virtualKey;

        AppCommands appCommand = KeyCodeToAppCommand((Keys) khs.virtualKey);
        if (appCommand == AppCommands.None)
        {
          if (khs.virtualKey == (int) Keys.LShiftKey || khs.virtualKey == (int) Keys.LControlKey ||
              khs.virtualKey == (int) Keys.RShiftKey || khs.virtualKey == (int) Keys.RControlKey)
            return CallNextHookEx(_hookHandle, code, wParam, lParam);

          if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift) keyCode |= 0x00100000;
          if ((Control.ModifierKeys & Keys.Control) == Keys.Control) keyCode |= 0x01000000;
          if ((Control.ModifierKeys & Keys.Alt) == Keys.Alt) keyCode |= 0x10000000;
        }
        else
        {
          keyCode |= (((int) appCommand) << 8);
        }

        if (_remoteButtonHandler != null)
          _remoteButtonHandler(Name, String.Format("{0:X8}", keyCode));

        if (_stealAppCommands && appCommand != AppCommands.None)
          return 1;
      }

      return CallNextHookEx(_hookHandle, code, wParam, lParam);
    }

    private static AppCommands KeyCodeToAppCommand(Keys keyCode)
    {
      switch (keyCode)
      {
        case Keys.BrowserBack:
          return AppCommands.BrowserBackward;
        case Keys.BrowserFavorites:
          return AppCommands.BrowserFavorites;
        case Keys.BrowserForward:
          return AppCommands.BrowserForward;
        case Keys.BrowserHome:
          return AppCommands.BrowserHome;
        case Keys.BrowserRefresh:
          return AppCommands.BrowserRefresh;
        case Keys.BrowserSearch:
          return AppCommands.BrowserSearch;
        case Keys.BrowserStop:
          return AppCommands.BrowserStop;
        case Keys.Help:
          return AppCommands.Help;
        case Keys.LaunchApplication1:
          return AppCommands.LaunchApp1;
        case Keys.LaunchApplication2:
          return AppCommands.LaunchApp2;
        case Keys.LaunchMail:
          return AppCommands.LaunchMail;
        case Keys.MediaNextTrack:
          return AppCommands.MediaNextTrack;
        case Keys.MediaPlayPause:
          return AppCommands.MediaPlayPause;
        case Keys.MediaPreviousTrack:
          return AppCommands.MediaPreviousTrack;
        case Keys.MediaStop:
          return AppCommands.MediaStop;
        case Keys.SelectMedia:
          return AppCommands.LaunchMediaSelect;
        case Keys.VolumeDown:
          return AppCommands.VolumeDown;
        case Keys.VolumeMute:
          return AppCommands.VolumeMute;
        case Keys.VolumeUp:
          return AppCommands.VolumeUp;
        default:
          return AppCommands.None;
      }
    }

    #endregion Implementation
  }
}