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
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using IrssComms;
using IrssUtils;
using IrssUtils.Forms;
using KeyboardInputRelay.Properties;

namespace KeyboardInputRelay
{
  internal static class Program
  {
    #region Constants

    private static readonly string ConfigurationFolder = Path.Combine(Common.FolderAppData,
                                                                    "Keyboard Input Relay");
    private static readonly string ConfigurationFile = Path.Combine(ConfigurationFolder,
                                                                    "Keyboard Input Relay.xml");

    #endregion Constants

    #region Interop

    [DllImport("user32.dll")]
    private static extern IntPtr SetWindowsHookEx(HookType code, HookDelegate func, IntPtr hInstance, int threadID);

    [DllImport("user32.dll")]
    private static extern int UnhookWindowsHookEx(IntPtr hhook);

    [DllImport("user32.dll")]
    private static extern IntPtr CallNextHookEx(IntPtr hhook, int code, int wParam, IntPtr lParam);

    [DllImport("kernel32.dll")]
    private static extern IntPtr LoadLibrary(string lpFileName);

    #endregion Interop

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main(string[] args)
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      IrssLog.LogLevel = IrssLog.Level.Debug;
      IrssLog.Open("Keyboard Input Relay.log");

      Application.ThreadException += Application_ThreadException;

      SetupNotify();

      LoadSettings();

      bool clientStarted = false;

      IPAddress serverIP = Client.GetIPFromName(_serverHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, Server.DefaultPort);

      try
      {
        clientStarted = StartClient(endPoint);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        clientStarted = false;
      }

      if (clientStarted)
      {
        StartHook();

        _notifyIcon.Visible = true;

        Application.Run();

        _notifyIcon.Visible = false;

        StopHook();

        StopClient();
      }

      Application.ThreadException -= Application_ThreadException;

      IrssLog.Close();
    }

    /// <summary>
    /// Handles unhandled exceptions.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">Event args.</param>
    private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
    {
      IrssLog.Error(e.Exception);
    }

    private static void LoadSettings()
    {
      XmlDocument doc = new XmlDocument();

      try
      {
        doc.Load(ConfigurationFile);
      }
      catch (DirectoryNotFoundException)
      {
        IrssLog.Warn("Configuration directory not found, using default settings");

        Directory.CreateDirectory(ConfigurationFolder);
        CreateDefaultSettings();
        return;
      }
      catch (FileNotFoundException)
      {
        IrssLog.Warn("Configuration file not found, using default settings");

        CreateDefaultSettings();
        return;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);

        CreateDefaultSettings();
        return;
      }

      try
      {
        _serverHost = doc.DocumentElement.Attributes["ServerHost"].Value;
      }
      catch
      {
        _serverHost = "localhost";
      }
    }

    private static void SaveSettings()
    {
      try
      {
        using (XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, Encoding.UTF8))
        {
          writer.Formatting = Formatting.Indented;
          writer.Indentation = 1;
          writer.IndentChar = (char) 9;
          writer.WriteStartDocument(true);
          writer.WriteStartElement("settings"); // <settings>

          writer.WriteAttributeString("ServerHost", _serverHost);

          writer.WriteEndElement(); // </settings>
          writer.WriteEndDocument();
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
    }

    private static void CreateDefaultSettings()
    {
      _serverHost = "localhost";

      SaveSettings();
    }

    private static void CommsFailure(object obj)
    {
      Exception ex = obj as Exception;

      if (ex != null)
        IrssLog.Error("Communications failure: {0}", ex.Message);
      else
        IrssLog.Error("Communications failure");

      _notifyIcon.Text = "Keyboard Input Relay - Serious Communications Failure";

      StopClient();

      MessageBox.Show("Please report this error.", "Keyboard Input Relay - Communications failure", MessageBoxButtons.OK,
                      MessageBoxIcon.Error);
    }

    private static void Connected(object obj)
    {
      IrssLog.Info("Connected to server");

      _notifyIcon.Text = "Keyboard Input Relay";

      IrssMessage message = new IrssMessage(MessageType.RegisterClient, MessageFlags.Request);
      _client.Send(message);
    }

    private static void Disconnected(object obj)
    {
      IrssLog.Warn("Communications with server has been lost");

      _notifyIcon.Text = "Keyboard Input Relay - Connecting ...";

      Thread.Sleep(1000);
    }

    private static bool StartClient(IPEndPoint endPoint)
    {
      if (_client != null)
        return false;

      ClientMessageSink sink = ReceivedMessage;

      _client = new Client(endPoint, sink);
      _client.CommsFailureCallback = CommsFailure;
      _client.ConnectCallback = Connected;
      _client.DisconnectCallback = Disconnected;

      if (_client.Start())
      {
        return true;
      }
      else
      {
        _client = null;
        return false;
      }
    }

    private static void StopClient()
    {
      if (_client == null)
        return;

      _client.Dispose();
      _client = null;

      _registered = false;
    }

    private static void ReceivedMessage(IrssMessage received)
    {
      IrssLog.Debug("Received Message \"{0}\"", received.Type);

      try
      {
        switch (received.Type)
        {
          case MessageType.RegisterClient:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              //_irServerInfo = IRServerInfo.FromBytes(received.DataAsBytes);
              _registered = true;

              IrssLog.Info("Registered to IR Server");
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              _registered = false;
              IrssLog.Warn("IR Server refused to register");
            }
            break;

          case MessageType.ServerShutdown:
            IrssLog.Warn("IR Server Shutdown - Keyboard Input Relay disabled until IR Server returns");

            _notifyIcon.Text = "Keyboard Input Relay - Connecting ...";

            _registered = false;
            break;

          case MessageType.Error:
            IrssLog.Error(received.GetDataAsString());
            break;
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
    }

    private static void SetupNotify()
    {
      // Setup notify icon ...
      _container = new Container();
      _notifyIcon = new NotifyIcon(_container);
      _notifyIcon.Icon = Resources.Icon;
      _notifyIcon.Text = "Keyboard Input Relay - Connecting ...";

      _notifyIcon.ContextMenuStrip = new ContextMenuStrip();

      _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripLabel("Keyboard Input Relay"));
      _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
      _notifyIcon.ContextMenuStrip.Items.Add("&Setup", null, NotifyIcon_ClickSetup);
      _notifyIcon.ContextMenuStrip.Items.Add("&Quit", null, NotifyIcon_ClickQuit);

      _notifyIcon.Visible = true;
    }

    private static void NotifyIcon_ClickSetup(object sender, EventArgs e)
    {
      Setup();
    }

    private static void NotifyIcon_ClickQuit(object sender, EventArgs e)
    {
      Application.Exit();
    }

    private static void Setup()
    {
      ServerAddress serverAddress = new ServerAddress();
      if (serverAddress.ShowDialog() == DialogResult.OK)
      {
        _serverHost = serverAddress.ServerHost;
        SaveSettings();
      }
    }

    private static void StartHook()
    {
      _hookDelegate = InternalHookDelegate;
      _libPtr = LoadLibrary("User32");
      _hookHandle = SetWindowsHookEx(HookType.WH_KEYBOARD_LL, _hookDelegate, _libPtr, 0);
    }

    private static void StopHook()
    {
      UnhookWindowsHookEx(_hookHandle);

      _hookHandle = IntPtr.Zero;
      _hookDelegate = null;
      _libPtr = IntPtr.Zero;
    }

    private static IntPtr InternalHookDelegate(int code, int wParam, IntPtr lParam)
    {
      try
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

            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
              keyCode |= 0x00100000;
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
              keyCode |= 0x01000000;
            if ((Control.ModifierKeys & Keys.Alt) == Keys.Alt)
              keyCode |= 0x10000000;
          }
          else
          {
            keyCode |= (((int) appCommand) << 8);
          }

          if (_registered)
          {
            byte[] deviceNameBytes = Encoding.ASCII.GetBytes("Keyboard");
            byte[] keyCodeBytes = Encoding.ASCII.GetBytes(String.Format("{0:X8}", keyCode));

            byte[] bytes = new byte[8 + deviceNameBytes.Length + keyCodeBytes.Length];

            BitConverter.GetBytes(deviceNameBytes.Length).CopyTo(bytes, 0);
            deviceNameBytes.CopyTo(bytes, 4);
            BitConverter.GetBytes(keyCodeBytes.Length).CopyTo(bytes, 4 + deviceNameBytes.Length);
            keyCodeBytes.CopyTo(bytes, 8 + deviceNameBytes.Length);

            IrssMessage message = new IrssMessage(MessageType.ForwardRemoteEvent, MessageFlags.Notify, bytes);
            _client.Send(message);
          }

          if (_stealAppCommands && appCommand != AppCommands.None)
            return new IntPtr(1);
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
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

    #region Nested type: HookDelegate

    private delegate IntPtr HookDelegate(int code, int wParam, IntPtr lParam);

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

    private static Client _client;
    private static Container _container;
    private static HookDelegate _hookDelegate;
    private static IntPtr _hookHandle;
    private static IntPtr _libPtr;
    private static NotifyIcon _notifyIcon;

    private static bool _registered;
    private static string _serverHost;
    private static bool _stealAppCommands = true;

    #endregion Variables
  }
}