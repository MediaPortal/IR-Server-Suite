using System;
using System.ComponentModel;
#if TRACE
using System.Diagnostics;
#endif
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using IrssComms;
using IrssUtils;
using IrssUtils.Forms;

namespace KeyboardInputRelay
{

  static class Program
  {

    #region Constants

    static readonly string ConfigurationFile = Path.Combine(Common.FolderAppData, "Keyboard Input Relay\\Keyboard Input Relay.xml");

    #endregion Constants

    #region Interop

    [DllImport("user32.dll")]
    static extern IntPtr SetWindowsHookEx(HookType code, HookDelegate func, IntPtr hInstance, int threadID);

    [DllImport("user32.dll")]
    static extern int UnhookWindowsHookEx(IntPtr hhook);

    [DllImport("user32.dll")]
    static extern int CallNextHookEx(IntPtr hhook, int code, int wParam, IntPtr lParam);

    [DllImport("kernel32.dll")]
    static extern IntPtr LoadLibrary(string lpFileName);

    #endregion Interop

    #region Delegates

    delegate int HookDelegate(int code, int wParam, IntPtr lParam);

    #endregion Delegates

    #region Enumerations

    enum HookType
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

    enum AppCommands
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

    #endregion Enumerations

    #region Structures

    struct KeyboardHookStruct
    {

      /// <summary>
      /// Initializes a new instance of the <see cref="KeyboardHookStruct"/> struct.
      /// </summary>
      /// <param name="lParam">The lParam to derive from.</param>
      public KeyboardHookStruct(IntPtr lParam)
      {
        KeyboardHookStruct khs = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
        
        virtualKey  = khs.virtualKey;
        scanCode    = khs.scanCode;
        flags       = khs.flags;
        time        = khs.time;
        dwExtraInfo = khs.dwExtraInfo;
      }

      public int virtualKey;
      public int scanCode;
      public int flags;
      public int time;
      public int dwExtraInfo;
    }

    #endregion Structures

    #region Variables

    static NotifyIcon _notifyIcon;

    static bool _stealAppCommands = true;

    static IntPtr _hookHandle;
    static HookDelegate _hookDelegate;
    static IntPtr _libPtr;

    static Client _client;
    static bool _registered;
    static string _serverHost;

    #endregion Variables

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

#if DEBUG
      IrssLog.LogLevel = IrssLog.Level.Debug;
#else
      IrssLog.LogLevel = IrssLog.Level.Info;
#endif
      IrssLog.Open("Keyboard Input Relay.log");

      Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

      SetupNotify();

      LoadSettings();

      bool clientStarted = false;

      IPAddress serverIP = Client.GetIPFromName(_serverHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, IrssComms.Server.DefaultPort);

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

      Application.ThreadException -= new ThreadExceptionEventHandler(Application_ThreadException);

      IrssLog.Close();
    }

    /// <summary>
    /// Handles unhandled exceptions.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">Event args.</param>
    static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
    {
      IrssLog.Error(e.Exception);
    }

    static void LoadSettings()
    {
      XmlDocument doc = new XmlDocument();

      try
      {
        doc.Load(ConfigurationFile);
      }
      catch (FileNotFoundException)
      {
        IrssLog.Warn("Configuration file not found, using defaults");

        CreateDefaultSettings();
        return;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);

        CreateDefaultSettings();
        return;
      }

      try { _serverHost = doc.DocumentElement.Attributes["ServerHost"].Value; } catch { _serverHost = "localhost"; }
    }
    static void SaveSettings()
    {
      try
      {
        using (XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, Encoding.UTF8))
        {
          writer.Formatting = Formatting.Indented;
          writer.Indentation = 1;
          writer.IndentChar = (char)9;
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
    static void CreateDefaultSettings()
    {
      _serverHost = "localhost";

      SaveSettings();
    }

    static void CommsFailure(object obj)
    {
      Exception ex = obj as Exception;
      
      if (ex != null)
        IrssLog.Error("Communications failure: {0}", ex.Message);
      else
        IrssLog.Error("Communications failure");

      _notifyIcon.Text = "Keyboard Input Relay - Serious Communications Failure";

      StopClient();

      MessageBox.Show("Please report this error.", "Keyboard Input Relay - Communications failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    static void Connected(object obj)
    {
      IrssLog.Info("Connected to server");

      _notifyIcon.Text = "Keyboard Input Relay";

      IrssMessage message = new IrssMessage(MessageType.RegisterClient, MessageFlags.Request);
      _client.Send(message);
    }
    static void Disconnected(object obj)
    {
      IrssLog.Warn("Communications with server has been lost");

      _notifyIcon.Text = "Keyboard Input Relay - Connecting ...";

      Thread.Sleep(1000);
    }

    static bool StartClient(IPEndPoint endPoint)
    {
      if (_client != null)
        return false;

      ClientMessageSink sink = new ClientMessageSink(ReceivedMessage);

      _client = new Client(endPoint, sink);
      _client.CommsFailureCallback  = new WaitCallback(CommsFailure);
      _client.ConnectCallback       = new WaitCallback(Connected);
      _client.DisconnectCallback    = new WaitCallback(Disconnected);
      
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
    static void StopClient()
    {
      if (_client == null)
        return;

      _client.Dispose();
      _client = null;

      _registered = false;
    }

    static void ReceivedMessage(IrssMessage received)
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

              IrssLog.Info("Registered to Input Service");
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              _registered = false;
              IrssLog.Warn("Input Service refused to register");
            }
            break;

          case MessageType.ServerShutdown:
            IrssLog.Warn("Input Service Shutdown - Keyboard Input Relay disabled until Input Service returns");

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

    static void SetupNotify()
    {
      // Setup notify icon ...
      _notifyIcon = new NotifyIcon();
      _notifyIcon.Icon = Properties.Resources.Icon;
      _notifyIcon.Text = "Keyboard Input Relay - Connecting ...";

      _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
      _notifyIcon.ContextMenuStrip.Items.Add("&Setup", null, new EventHandler(NotifyIcon_ClickSetup));
      _notifyIcon.ContextMenuStrip.Items.Add("&Quit", null, new EventHandler(NotifyIcon_ClickQuit));

      _notifyIcon.Visible = true;
    }

    static void NotifyIcon_ClickSetup(object sender, EventArgs e)
    {
      Setup();
    }
    static void NotifyIcon_ClickQuit(object sender, EventArgs e)
    {
      Application.Exit();
    }

    static void Setup()
    {
      ServerAddress serverAddress = new ServerAddress();
      if (serverAddress.ShowDialog() == DialogResult.OK)
      {
        _serverHost = serverAddress.ServerHost;
        SaveSettings();
      }
    }

    static void StartHook()
    {
      _hookDelegate = new HookDelegate(InternalHookDelegate);
      _libPtr = LoadLibrary("User32");
      _hookHandle = SetWindowsHookEx(HookType.WH_KEYBOARD_LL, _hookDelegate, _libPtr, 0);
    }
    static void StopHook()
    {
      UnhookWindowsHookEx(_hookHandle);

      _hookHandle = IntPtr.Zero;
      _hookDelegate = null;
      _libPtr = IntPtr.Zero;
    }

    static int InternalHookDelegate(int code, int wParam, IntPtr lParam)
    {
      try
      {
        if (code >= 0 && wParam == 256)
        {
          KeyboardHookStruct khs = new KeyboardHookStruct(lParam);
          int keyCode = khs.virtualKey;

          AppCommands appCommand = KeyCodeToAppCommand((Keys)khs.virtualKey);
          if (appCommand == AppCommands.None)
          {
            if (khs.virtualKey == (int)Keys.LShiftKey || khs.virtualKey == (int)Keys.LControlKey ||
                khs.virtualKey == (int)Keys.RShiftKey || khs.virtualKey == (int)Keys.RControlKey)
              return CallNextHookEx(_hookHandle, code, wParam, lParam);

            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)      keyCode |= 0x00100000;
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)  keyCode |= 0x01000000;
            if ((Control.ModifierKeys & Keys.Alt) == Keys.Alt)          keyCode |= 0x10000000;
          }
          else
          {
            keyCode |= (((int)appCommand) << 8);
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
            return 1;
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }

      return CallNextHookEx(_hookHandle, code, wParam, lParam);
    }

    static AppCommands KeyCodeToAppCommand(Keys keyCode)
    {
      switch (keyCode)
      {
        case Keys.BrowserBack:        return AppCommands.BrowserBackward;
        case Keys.BrowserFavorites:   return AppCommands.BrowserFavorites;
        case Keys.BrowserForward:     return AppCommands.BrowserForward;
        case Keys.BrowserHome:        return AppCommands.BrowserHome;
        case Keys.BrowserRefresh:     return AppCommands.BrowserRefresh;
        case Keys.BrowserSearch:      return AppCommands.BrowserSearch;
        case Keys.BrowserStop:        return AppCommands.BrowserStop;
        case Keys.Help:               return AppCommands.Help;
        case Keys.LaunchApplication1: return AppCommands.LaunchApp1;
        case Keys.LaunchApplication2: return AppCommands.LaunchApp2;
        case Keys.LaunchMail:         return AppCommands.LaunchMail;
        case Keys.MediaNextTrack:     return AppCommands.MediaNextTrack;
        case Keys.MediaPlayPause:     return AppCommands.MediaPlayPause;
        case Keys.MediaPreviousTrack: return AppCommands.MediaPreviousTrack;
        case Keys.MediaStop:          return AppCommands.MediaStop;
        case Keys.SelectMedia:        return AppCommands.LaunchMediaSelect;
        case Keys.VolumeDown:         return AppCommands.VolumeDown;
        case Keys.VolumeMute:         return AppCommands.VolumeMute;
        case Keys.VolumeUp:           return AppCommands.VolumeUp;
        default:                      return AppCommands.None;
      }
    }

  }

}
