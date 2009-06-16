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
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using InputService.Plugin.Properties;
using Microsoft.Win32.SafeHandles;

#if TRACE
using System.Diagnostics;
#endif

namespace InputService.Plugin
{
  /// <summary>
  /// IR Server Plugin for Imon IR Receiver hardware.
  /// </summary>
  public class ImonReceiver : PluginBase, IConfigure, IRemoteReceiver, IKeyboardReceiver, IMouseReceiver, IDisposable
  {
    // #define TEST_APPLICATION in the project properties when creating the console test app ...
#if TEST_APPLICATION

    static void xRemote(string deviceName, string code)
    {
      Console.WriteLine("Remote: {0}", code);
    }
    static void xKeyboard(string deviceName, int button, bool up)
    {
      Console.WriteLine("Keyboard: {0}, {1}", button, up);
    }
    static void xMouse(string deviceName, int x, int y, int buttons)
    {
      Console.WriteLine("Mouse: ({0}, {1}) - {2}", x, y, buttons);
    }

    [STAThread]
    static void Main()
    {
      ImonReceiver device;

      try
      {
        device = new ImonReceiver();

        device.Configure(null);

        device.RemoteCallback += new RemoteHandler(xRemote);
        device.KeyboardCallback += new KeyboardHandler(xKeyboard);
        device.MouseCallback += new MouseHandler(xMouse);

        device.Start();

        Console.WriteLine("Press a button on your remote ...");

        Application.Run();

        device.Stop();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
      finally
      {
        device = null;
      }

      Console.ReadKey();
    }

#endif

    #region Constants

    private static readonly string ConfigurationFile = Path.Combine(ConfigurationPath, "Imon Receiver.xml");

    private const int DeviceBufferSize = 8;
    private const string DevicePath = @"\\.\SGIMON";

    private const int ErrorIoPending = 997;

    // IOCTL definitions 0x0022xxxx
    private const uint IOCTL_IMON_READ = 0x00222008; // function 0x802 - read data (64 bytes?) from device
    private const uint IOCTL_IMON_WRITE = 0x00222018; // function 0x806 - write data (8 bytes) to device
    private const uint IOCTL_IMON_READ_RC = 0x00222030; // function 0x80C - read data (8 bytes) from device
    private const uint IOCTL_IMON_RC_SET = 0x00222010; // function 0x804 - write RCset data (2 bytes) to device
    private const uint IOCTL_IMON_FW_VER = 0x00222014; // function 0x805 - read FW version (4 bytes)
    private const uint IOCTL_IMON_READ2 = 0x00222034; // function 0x80D - ??? read (8 bytes)

    private const uint IMON_PAD_BUTTON = 1000;
    private const uint IMON_MCE_BUTTON = 2000;
    private const uint IMON_PANEL_BUTTON = 3000;
    private const uint IMON_VOLUME_UP = 4001;
    private const uint IMON_VOLUME_DOWN = 4002;

    private static readonly byte[][] SetModeMCE = new byte[][]
                                                    {
                                                      new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x00},
                                                      new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x02},
                                                      new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x04},
                                                      new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x06},
                                                      new byte[] {0x20, 0x20, 0x20, 0x20, 0x01, 0x00, 0x00, 0x08},
                                                      new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x0A}
                                                    };

    private static readonly byte[][] SetModeImon = new byte[][]
                                                     {
                                                       new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x00},
                                                       new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x02},
                                                       new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x04},
                                                       new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x06},
                                                       new byte[] {0x20, 0x20, 0x20, 0x20, 0x00, 0x00, 0x00, 0x08},
                                                       new byte[] {0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x0A}
                                                     };

    #endregion Constants

    #region Enumerations

    /// <summary>
    /// Hardware mode (either MCE or Imon).
    /// </summary>
    internal enum RcMode
    {
      /// <summary>
      /// Microsoft MCE Mode.
      /// </summary>
      Mce,
      /// <summary>
      /// Soundgraph Imon Mode.
      /// </summary>
      Imon,
    }

    [Flags]
    private enum KeyModifiers
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

    [Flags]
    private enum CreateFileAccessTypes : uint
    {
      GenericRead = 0x80000000,
      GenericWrite = 0x40000000,
      GenericExecute = 0x20000000,
      GenericAll = 0x10000000,
    }

    [Flags]
    private enum CreateFileShares : uint
    {
      None = 0x00,
      Read = 0x01,
      Write = 0x02,
      Delete = 0x04,
    }

    private enum CreateFileDisposition : uint
    {
      None = 0,
      New = 1,
      CreateAlways = 2,
      OpenExisting = 3,
      OpenAlways = 4,
      TruncateExisting = 5,
    }

    [Flags]
    private enum CreateFileAttributes : uint
    {
      None = 0x00000000,
      Readonly = 0x00000001,
      Hidden = 0x00000002,
      System = 0x00000004,
      Directory = 0x00000010,
      Archive = 0x00000020,
      Device = 0x00000040,
      Normal = 0x00000080,
      Temporary = 0x00000100,
      SparseFile = 0x00000200,
      ReparsePoint = 0x00000400,
      Compressed = 0x00000800,
      Offline = 0x00001000,
      NotContentIndexed = 0x00002000,
      Encrypted = 0x00004000,
      Write_Through = 0x80000000,
      Overlapped = 0x40000000,
      NoBuffering = 0x20000000,
      RandomAccess = 0x10000000,
      SequentialScan = 0x08000000,
      DeleteOnClose = 0x04000000,
      BackupSemantics = 0x02000000,
      PosixSemantics = 0x01000000,
      OpenReparsePoint = 0x00200000,
      OpenNoRecall = 0x00100000,
      FirstPipeInstance = 0x00080000,
    }

    #endregion Enumerations

    #region Interop

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool DeviceIoControl(
      SafeFileHandle handle,
      uint ioControlCode,
      IntPtr inBuffer, int inBufferSize,
      IntPtr outBuffer, int outBufferSize,
      out int bytesReturned,
      IntPtr overlapped);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetOverlappedResult(
      SafeFileHandle handle,
      IntPtr overlapped,
      out int bytesTransferred,
      [MarshalAs(UnmanagedType.Bool)] bool wait);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern SafeFileHandle CreateFile(
      [MarshalAs(UnmanagedType.LPTStr)] string fileName,
      [MarshalAs(UnmanagedType.U4)] CreateFileAccessTypes fileAccess,
      [MarshalAs(UnmanagedType.U4)] CreateFileShares fileShare,
      IntPtr securityAttributes,
      [MarshalAs(UnmanagedType.U4)] CreateFileDisposition creationDisposition,
      [MarshalAs(UnmanagedType.U4)] CreateFileAttributes flags,
      IntPtr templateFile);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CancelIo(
      SafeFileHandle handle);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CloseHandle(
      SafeFileHandle handle);

    #endregion Interop

    #region Variables

    #region Configuration

    private RcMode _hardwareMode = RcMode.Mce;

    private bool _enableRemoteInput = true;
    private bool _useSystemRatesRemote = false;
    private int _remoteFirstRepeat = 400;
    private int _remoteHeldRepeats = 250;

    private bool _enableKeyboardInput = false;
    private bool _useSystemRatesKeyboard = true;
    private int _keyboardFirstRepeat = 350;
    private int _keyboardHeldRepeats = 0;
    private bool _handleKeyboardLocally = true;

    private bool _enableMouseInput = false;
    private bool _handleMouseLocally = true;
    private double _mouseSensitivity = 1.0d;

    #endregion Configuration

    private RemoteHandler _remoteHandler;
    private KeyboardHandler _keyboardHandler;
    private MouseHandler _mouseHandler;

    private uint _lastRemoteButtonKeyCode = 0;
    private DateTime _lastRemoteButtonTime = DateTime.Now;
    private bool _remoteButtonRepeated = false;

    private byte _remoteToggle = 0;

    private bool _keyboardKeyRepeated = false;
    private DateTime _lastKeyboardKeyTime = DateTime.Now;

    private uint _lastKeyboardKeyCode = 0;
    private uint _lastKeyboardModifiers = 0;

    private Mouse.MouseEvents _mouseButtons = Mouse.MouseEvents.None;

    private SafeFileHandle _deviceHandle;

    private Thread _receiveThread;
    private bool _processReceiveThread;

    private bool _disposed;

    #endregion Variables

    #region Destructor

    /// <summary>
    /// Releases unmanaged resources and performs other cleanup operations before the
    /// <see cref="ImonReceiver"/> is reclaimed by garbage collection.
    /// </summary>
    ~ImonReceiver()
    {
      // Call Dispose with false.  Since we're in the destructor call, the managed resources will be disposed of anyway.
      Dispose(false);
    }

    #endregion Destructor

    #region IDisposable Members

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources
    /// </summary>
    public void Dispose()
    {
      // Dispose of the managed and unmanaged resources
      Dispose(true);

      // Tell the GC that the Finalize process no longer needs to be run for this object.
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      // process only if mananged and unmanaged resources have
      // not been disposed of.
      if (!_disposed)
      {
        if (disposing)
        {
          // dispose managed resources
          Stop();
        }

        // dispose unmanaged resources
        _disposed = true;
      }
    }

    #endregion

    #region Implementation

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name
    {
      get { return "Imon"; }
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
      get { return "and-81, with code by cybrmage"; }
    }

    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    /// <value>The description.</value>
    public override string Description
    {
      get { return "Supports the Imon IR receiver hardware"; }
    }

    /// <summary>
    /// Gets a display icon for the plugin.
    /// </summary>
    /// <value>The icon.</value>
    public override Icon DeviceIcon
    {
      get { return Resources.Icon; }
    }

    /// <summary>
    /// Configure the IR Server plugin.
    /// </summary>
    public void Configure(IWin32Window owner)
    {
      LoadSettings();

      Configure config = new Configure();

      config.HardwareMode = _hardwareMode;

      config.EnableRemote = _enableRemoteInput;
      config.UseSystemRatesForRemote = _useSystemRatesRemote;
      config.RemoteRepeatDelay = _remoteFirstRepeat;
      config.RemoteHeldDelay = _remoteHeldRepeats;

      config.EnableKeyboard = _enableKeyboardInput;
      config.UseSystemRatesForKeyboard = _useSystemRatesKeyboard;
      config.KeyboardRepeatDelay = _keyboardFirstRepeat;
      config.KeyboardHeldDelay = _keyboardHeldRepeats;
      config.HandleKeyboardLocal = _handleKeyboardLocally;

      config.EnableMouse = _enableMouseInput;
      config.HandleMouseLocal = _handleMouseLocally;
      config.MouseSensitivity = _mouseSensitivity;

      if (config.ShowDialog(owner) == DialogResult.OK)
      {
        _hardwareMode = config.HardwareMode;

        _enableRemoteInput = config.EnableRemote;
        _useSystemRatesRemote = config.UseSystemRatesForRemote;
        _remoteFirstRepeat = config.RemoteRepeatDelay;
        _remoteHeldRepeats = config.RemoteHeldDelay;

        _enableKeyboardInput = config.EnableKeyboard;
        _useSystemRatesKeyboard = config.UseSystemRatesForKeyboard;
        _keyboardFirstRepeat = config.KeyboardRepeatDelay;
        _keyboardHeldRepeats = config.KeyboardHeldDelay;
        _handleKeyboardLocally = config.HandleKeyboardLocal;

        _enableMouseInput = config.EnableMouse;
        _handleMouseLocally = config.HandleMouseLocal;
        _mouseSensitivity = config.MouseSensitivity;

        SaveSettings();
      }
    }

    /// <summary>
    /// Detect the presence of this device.  Devices that cannot be detected will always return false.
    /// This method should not throw exceptions.
    /// </summary>
    /// <returns><c>true</c> if the device is present, otherwise <c>false</c>.</returns>
    public override bool Detect()
    {
      try
      {
        //SafeFileHandle deviceHandle = CreateFile(DevicePath, CreateFileAccessTypes.GenericRead | CreateFileAccessTypes.GenericWrite, CreateFileShares.Read | CreateFileShares.Write, IntPtr.Zero, CreateFileDisposition.OpenExisting, CreateFileAttributes.Overlapped, IntPtr.Zero);
        SafeFileHandle deviceHandle = CreateFile(DevicePath,
                                                 CreateFileAccessTypes.GenericRead | CreateFileAccessTypes.GenericWrite,
                                                 CreateFileShares.Read | CreateFileShares.Write, IntPtr.Zero,
                                                 CreateFileDisposition.OpenExisting, CreateFileAttributes.Normal,
                                                 IntPtr.Zero);
        int lastError = Marshal.GetLastWin32Error();

        if (deviceHandle.IsInvalid)
          throw new Win32Exception(lastError, "Failed to open device");

        CloseHandle(deviceHandle);

        return true;
      }
      catch
      {
        return false;
      }
    }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    public override void Start()
    {
#if DEBUG
      DebugOpen("Imon Receiver.log");
      DebugWriteLine("Start()");
#endif

      LoadSettings();

      //_deviceHandle = CreateFile(DevicePath, CreateFileAccessTypes.GenericRead | CreateFileAccessTypes.GenericWrite, CreateFileShares.Read | CreateFileShares.Write, IntPtr.Zero, CreateFileDisposition.OpenExisting, CreateFileAttributes.Overlapped, IntPtr.Zero);
      _deviceHandle = CreateFile(DevicePath, CreateFileAccessTypes.GenericRead | CreateFileAccessTypes.GenericWrite,
                                 CreateFileShares.Read | CreateFileShares.Write, IntPtr.Zero,
                                 CreateFileDisposition.OpenExisting, CreateFileAttributes.Normal, IntPtr.Zero);
      int lastError = Marshal.GetLastWin32Error();

      if (_deviceHandle.IsInvalid)
        throw new Win32Exception(lastError, "Failed to open device");

      SetHardwareMode(_hardwareMode);

      _processReceiveThread = true;
      _receiveThread = new Thread(new ThreadStart(ReceiveThread));
      _receiveThread.Name = "Imon Receive Thread";
      _receiveThread.IsBackground = true;
      _receiveThread.Start();
    }

    /// <summary>
    /// Suspend the IR Server plugin when computer enters standby.
    /// </summary>
    public override void Suspend()
    {
#if DEBUG
      DebugWriteLine("Suspend()");
#endif

      Stop();
    }

    /// <summary>
    /// Resume the IR Server plugin when the computer returns from standby.
    /// </summary>
    public override void Resume()
    {
#if DEBUG
      DebugWriteLine("Resume()");
#endif

      Start();
    }

    /// <summary>
    /// Stop the IR Server plugin.
    /// </summary>
    public override void Stop()
    {
#if DEBUG
      DebugWriteLine("Stop()");
#endif

      if (_processReceiveThread)
      {
        _processReceiveThread = false;

        if (_deviceHandle != null && !_deviceHandle.IsClosed)
          CancelIo(_deviceHandle);
      }

      if (_receiveThread != null && _receiveThread.IsAlive)
        _receiveThread.Abort();

      _receiveThread = null;

      if (_deviceHandle != null && !_deviceHandle.IsClosed)
        CloseHandle(_deviceHandle);

#if DEBUG
      DebugClose();
#endif
    }

    /// <summary>
    /// Callback for remote button presses.
    /// </summary>
    public RemoteHandler RemoteCallback
    {
      get { return _remoteHandler; }
      set { _remoteHandler = value; }
    }

    /// <summary>
    /// Callback for keyboard presses.
    /// </summary>
    public KeyboardHandler KeyboardCallback
    {
      get { return _keyboardHandler; }
      set { _keyboardHandler = value; }
    }

    /// <summary>
    /// Callback for mouse events.
    /// </summary>
    public MouseHandler MouseCallback
    {
      get { return _mouseHandler; }
      set { _mouseHandler = value; }
    }

    private void LoadSettings()
    {
#if DEBUG
      DebugWriteLine("LoadSettings()");
#endif

      XmlDocument doc = new XmlDocument();

      try
      {
        doc.Load(ConfigurationFile);
      }
      catch
      {
        return;
      }

      try
      {
        _hardwareMode = (RcMode) Enum.Parse(typeof (RcMode), doc.DocumentElement.Attributes["HardwareMode"].Value);
      }
      catch
      {
      }

      try
      {
        _enableRemoteInput = bool.Parse(doc.DocumentElement.Attributes["EnableRemoteInput"].Value);
      }
      catch
      {
      }
      try
      {
        _useSystemRatesRemote = bool.Parse(doc.DocumentElement.Attributes["UseSystemRatesRemote"].Value);
      }
      catch
      {
      }
      try
      {
        _remoteFirstRepeat = int.Parse(doc.DocumentElement.Attributes["RemoteFirstRepeat"].Value);
      }
      catch
      {
      }
      try
      {
        _remoteHeldRepeats = int.Parse(doc.DocumentElement.Attributes["RemoteHeldRepeats"].Value);
      }
      catch
      {
      }

      try
      {
        _enableKeyboardInput = bool.Parse(doc.DocumentElement.Attributes["EnableKeyboardInput"].Value);
      }
      catch
      {
      }
      try
      {
        _useSystemRatesKeyboard = bool.Parse(doc.DocumentElement.Attributes["UseSystemRatesKeyboard"].Value);
      }
      catch
      {
      }
      try
      {
        _keyboardFirstRepeat = int.Parse(doc.DocumentElement.Attributes["KeyboardFirstRepeat"].Value);
      }
      catch
      {
      }
      try
      {
        _keyboardHeldRepeats = int.Parse(doc.DocumentElement.Attributes["KeyboardHeldRepeats"].Value);
      }
      catch
      {
      }
      try
      {
        _handleKeyboardLocally = bool.Parse(doc.DocumentElement.Attributes["HandleKeyboardLocally"].Value);
      }
      catch
      {
      }

      try
      {
        _enableMouseInput = bool.Parse(doc.DocumentElement.Attributes["EnableMouseInput"].Value);
      }
      catch
      {
      }
      try
      {
        _handleMouseLocally = bool.Parse(doc.DocumentElement.Attributes["HandleMouseLocally"].Value);
      }
      catch
      {
      }
      try
      {
        _mouseSensitivity = double.Parse(doc.DocumentElement.Attributes["MouseSensitivity"].Value);
      }
      catch
      {
      }
    }

    private void SaveSettings()
    {
#if DEBUG
      DebugWriteLine("SaveSettings()");
#endif

      try
      {
        using (XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, Encoding.UTF8))
        {
          writer.Formatting = Formatting.Indented;
          writer.Indentation = 1;
          writer.IndentChar = (char) 9;
          writer.WriteStartDocument(true);
          writer.WriteStartElement("settings"); // <settings>

          writer.WriteAttributeString("HardwareMode", Enum.GetName(typeof (RcMode), _hardwareMode));

          writer.WriteAttributeString("EnableRemoteInput", _enableRemoteInput.ToString());
          writer.WriteAttributeString("UseSystemRatesRemote", _useSystemRatesRemote.ToString());
          writer.WriteAttributeString("RemoteFirstRepeat", _remoteFirstRepeat.ToString());
          writer.WriteAttributeString("RemoteHeldRepeats", _remoteHeldRepeats.ToString());

          writer.WriteAttributeString("EnableKeyboardInput", _enableKeyboardInput.ToString());
          writer.WriteAttributeString("UseSystemRatesKeyboard", _useSystemRatesKeyboard.ToString());
          writer.WriteAttributeString("KeyboardFirstRepeat", _keyboardFirstRepeat.ToString());
          writer.WriteAttributeString("KeyboardHeldRepeats", _keyboardHeldRepeats.ToString());
          writer.WriteAttributeString("HandleKeyboardLocally", _handleKeyboardLocally.ToString());

          writer.WriteAttributeString("EnableMouseInput", _enableMouseInput.ToString());
          writer.WriteAttributeString("HandleMouseLocally", _handleMouseLocally.ToString());
          writer.WriteAttributeString("MouseSensitivity", _mouseSensitivity.ToString());

          writer.WriteEndElement(); // </settings>
          writer.WriteEndDocument();
        }
      }
#if DEBUG
      catch (Exception ex)
      {
        DebugWriteLine(ex.ToString());
      }
#else
      catch
      {
      }
#endif
    }

    private void ProcessInput(byte[] dataBytes)
    {
#if DEBUG
      DebugWriteLine("Data Received:");
      DebugDump(dataBytes);
#endif

      if ((dataBytes[0] & 0xFC) == 0x28)
      {
#if DEBUG
        DebugWriteLine("iMon PAD remote button");
#endif
        uint keyCode = IMON_PAD_BUTTON;
        keyCode += (uint) ((dataBytes[0] & 0x03) << 6);
        keyCode += (uint) (dataBytes[1] & 0x30);
        keyCode += (uint) ((dataBytes[1] & 0x06) << 1);
        keyCode += (uint) ((dataBytes[2] & 0xC0) >> 6);

        if ((keyCode & 0x01) == 0 && (dataBytes[2] & 0x40) == 0)
        {
          RemoteEvent(keyCode, _remoteToggle != 1);
          _remoteToggle = 1;
        }
        else
        {
          _remoteToggle = 0;
        }
      }
      else if ((dataBytes[0] & 0xFC) == 0x68)
      {
#if DEBUG
        DebugWriteLine("iMon PAD mouse move/button");
#endif
        int xSign = (((dataBytes[0] & 0x02) != 0) ? 1 : -1);
        int ySign = (((dataBytes[0] & 0x01) != 0) ? 1 : -1);
        int xSize = ((dataBytes[1] & 0x78) >> 3);
        int ySize = ((dataBytes[2] & 0x78) >> 3);

        bool right = ((dataBytes[1] & 0x04) != 0);
        bool left = ((dataBytes[1] & 0x01) != 0);

        MouseEvent(xSign*xSize, ySign*ySize, right, left);
      }
      else if (dataBytes[7] == 0xAE)
      {
#if DEBUG
        DebugWriteLine("MCE remote button");
#endif
        uint keyCode = IMON_MCE_BUTTON + dataBytes[3];

        RemoteEvent(keyCode, _remoteToggle != dataBytes[2]);
        _remoteToggle = dataBytes[2];
      }
      else if (dataBytes[7] == 0xBE)
      {
#if DEBUG
        DebugWriteLine("MCE Keyboard key press");
#endif
        KeyboardEvent(dataBytes[2], dataBytes[3]);
      }
      else if (dataBytes[7] == 0xCE)
      {
#if DEBUG
        DebugWriteLine("MCE Keyboard mouse move/button");
#endif
        int xSign = (dataBytes[2] & 0x20) == 0 ? 1 : -1;
        int ySign = (dataBytes[1] & 0x10) == 0 ? 1 : -1;

        int xSize = (dataBytes[3] & 0x0F);
        int ySize = (dataBytes[2] & 0x0F);

        bool right = (dataBytes[3] & 0x40) != 0;
        bool left = (dataBytes[3] & 0x20) != 0;

        MouseEvent(xSign*xSize, ySign*ySize, right, left);
      }
      else if (dataBytes[7] == 0xEE)
      {
#if DEBUG
        DebugWriteLine("Front panel buttons/volume knob");
#endif
        if (dataBytes[3] > 0x01)
        {
          uint keyCode = IMON_PANEL_BUTTON + dataBytes[3];
          RemoteEvent(keyCode, _remoteToggle != dataBytes[3]);
        }
        _remoteToggle = dataBytes[3];

        if (dataBytes[0] == 0x01 && _remoteHandler != null)
          _remoteHandler(Name, IMON_VOLUME_DOWN.ToString());

        if (dataBytes[1] == 0x01 && _remoteHandler != null)
          _remoteHandler(Name, IMON_VOLUME_UP.ToString());
      }
    }

    private void ReceiveThread()
    {
#if DEBUG
      DebugWriteLine("ReceiveThread()");
#endif

      int bytesRead;

      IntPtr deviceBufferPtr = IntPtr.Zero;

      try
      {
        deviceBufferPtr = Marshal.AllocHGlobal(DeviceBufferSize);

        while (_processReceiveThread)
        {
          IoControl(IOCTL_IMON_READ_RC, IntPtr.Zero, 0, deviceBufferPtr, DeviceBufferSize, out bytesRead);

          if (bytesRead == DeviceBufferSize)
          {
            byte[] dataBytes = new byte[bytesRead];
            Marshal.Copy(deviceBufferPtr, dataBytes, 0, bytesRead);

            // Rubbish data:
            // FF, FF, FF, FF, FF, FF, 9F, FF, 
            // 00, 00, 00, 00, 00, 00, 00, F0, 

            if ((dataBytes[0] != 0xFF || dataBytes[1] != 0xFF || dataBytes[2] != 0xFF || dataBytes[3] != 0xFF ||
                 dataBytes[4] != 0xFF || dataBytes[5] != 0xFF) &&
                (dataBytes[0] != 0x00 || dataBytes[1] != 0x00 || dataBytes[2] != 0x00 || dataBytes[3] != 0x00 ||
                 dataBytes[4] != 0x00 || dataBytes[5] != 0x00))
            {
              ProcessInput(dataBytes);
            }
          }
        }
      }
#if DEBUG
      catch (Exception ex)
      {
        DebugWriteLine(ex.ToString());
#else
      catch
      {
#endif
        if (_deviceHandle != null)
          CancelIo(_deviceHandle);
      }
      finally
      {
        if (deviceBufferPtr != IntPtr.Zero)
          Marshal.FreeHGlobal(deviceBufferPtr);
      }
    }

    private void IoControl(uint ioControlCode, IntPtr inBuffer, int inBufferSize, IntPtr outBuffer, int outBufferSize,
                           out int bytesReturned)
    {
      try
      {
        DeviceIoControl(_deviceHandle, ioControlCode, inBuffer, inBufferSize, outBuffer, outBufferSize,
                        out bytesReturned, IntPtr.Zero);
      }
      catch
      {
        if (_deviceHandle != null)
          CancelIo(_deviceHandle);

        throw;
      }
    }

    private void IoControlOverlapped(uint ioControlCode, IntPtr inBuffer, int inBufferSize, IntPtr outBuffer,
                                     int outBufferSize, out int bytesReturned)
    {
      int lastError;

      using (WaitHandle waitHandle = new ManualResetEvent(false))
      {
        SafeHandle safeWaitHandle = waitHandle.SafeWaitHandle;

        bool success = false;
        safeWaitHandle.DangerousAddRef(ref success);
        if (!success)
          throw new InvalidOperationException("Failed to initialize safe wait handle");

        try
        {
          IntPtr dangerousWaitHandle = safeWaitHandle.DangerousGetHandle();

          DeviceIoOverlapped overlapped = new DeviceIoOverlapped();
          overlapped.ClearAndSetEvent(dangerousWaitHandle);

          bool deviceIoControl = DeviceIoControl(_deviceHandle, ioControlCode, inBuffer, inBufferSize, outBuffer,
                                                 outBufferSize, out bytesReturned, overlapped.Overlapped);
          lastError = Marshal.GetLastWin32Error();

          if (!deviceIoControl)
          {
            if (lastError != ErrorIoPending)
              throw new Win32Exception(lastError);

            waitHandle.WaitOne();

            bool getOverlapped = GetOverlappedResult(_deviceHandle, overlapped.Overlapped, out bytesReturned, false);
            lastError = Marshal.GetLastWin32Error();

            if (!getOverlapped)
              throw new Win32Exception(lastError);
          }
        }
        catch
        {
          if (_deviceHandle != null)
            CancelIo(_deviceHandle);

          throw;
        }
        finally
        {
          safeWaitHandle.DangerousRelease();
        }
      }
    }

    private void SetHardwareMode(RcMode mode)
    {
#if DEBUG
      DebugWriteLine("SetHardwareMode({0})", Enum.GetName(typeof (RcMode), mode));
#endif

      int bytesRead;

      IntPtr deviceBufferPtr = IntPtr.Zero;

      try
      {
        switch (mode)
        {
          case RcMode.Imon:
            foreach (byte[] send in SetModeImon)
            {
              deviceBufferPtr = Marshal.AllocHGlobal(send.Length);

              Marshal.Copy(send, 0, deviceBufferPtr, send.Length);
              IoControl(IOCTL_IMON_WRITE, deviceBufferPtr, send.Length, IntPtr.Zero, 0, out bytesRead);

              Marshal.FreeHGlobal(deviceBufferPtr);
            }
            break;

          case RcMode.Mce:
            foreach (byte[] send in SetModeMCE)
            {
              deviceBufferPtr = Marshal.AllocHGlobal(send.Length);

              Marshal.Copy(send, 0, deviceBufferPtr, send.Length);
              IoControl(IOCTL_IMON_WRITE, deviceBufferPtr, send.Length, IntPtr.Zero, 0, out bytesRead);

              Marshal.FreeHGlobal(deviceBufferPtr);
            }
            break;
        }
      }
#if DEBUG
      catch (Exception ex)
      {
        DebugWriteLine(ex.ToString());
#else
      catch
      {
#endif
        if (_deviceHandle != null)
          CancelIo(_deviceHandle);
      }
      finally
      {
        if (deviceBufferPtr != IntPtr.Zero)
          Marshal.FreeHGlobal(deviceBufferPtr);
      }
    }

    private void RemoteEvent(uint keyCode, bool firstPress)
    {
#if DEBUG
      DebugWriteLine("RemoteEvent: {0}, {1}", keyCode, firstPress);
#endif

      if (!_enableRemoteInput)
        return;

      if (!firstPress && _lastRemoteButtonKeyCode == keyCode)
      {
        TimeSpan timeBetween = DateTime.Now.Subtract(_lastRemoteButtonTime);

        int firstRepeat = _remoteFirstRepeat;
        int heldRepeats = _remoteHeldRepeats;
        if (_useSystemRatesRemote)
        {
          firstRepeat = 250 + (SystemInformation.KeyboardDelay*250);
          heldRepeats = (int) (1000.0/(2.5 + (SystemInformation.KeyboardSpeed*0.888)));
        }

        if (!_remoteButtonRepeated && timeBetween.TotalMilliseconds < firstRepeat)
        {
#if DEBUG
          DebugWriteLine("Skip, First Repeat");
#endif
          return;
        }

        if (_remoteButtonRepeated && timeBetween.TotalMilliseconds < heldRepeats)
        {
#if DEBUG
          DebugWriteLine("Skip, Held Repeat");
#endif
          return;
        }

        if (_remoteButtonRepeated && timeBetween.TotalMilliseconds > firstRepeat)
          _remoteButtonRepeated = false;
        else
          _remoteButtonRepeated = true;
      }
      else
      {
        _lastRemoteButtonKeyCode = keyCode;
        _remoteButtonRepeated = false;
      }

      _lastRemoteButtonTime = DateTime.Now;

      if (_remoteHandler != null)
        _remoteHandler(Name, keyCode.ToString());
    }

    private void KeyboardEvent(uint keyCode, uint modifiers)
    {
#if DEBUG
      DebugWriteLine("KeyboardEvent: {0}, {1}", keyCode, modifiers);
#endif

      if (!_enableKeyboardInput)
        return;

      if (keyCode != _lastKeyboardKeyCode && modifiers == _lastKeyboardModifiers)
      {
        if (_handleKeyboardLocally)
        {
          KeyUp(_lastKeyboardKeyCode, 0);
          KeyDown(keyCode, 0);
        }
        else
        {
          KeyUpRemote(_lastKeyboardKeyCode, 0);
          KeyDownRemote(keyCode, 0);
        }

        _keyboardKeyRepeated = false;
      }
      else if (keyCode == _lastKeyboardKeyCode && modifiers != _lastKeyboardModifiers)
      {
        uint turnOff = _lastKeyboardModifiers & ~modifiers;
        uint turnOn = modifiers & ~_lastKeyboardModifiers;

        if (_handleKeyboardLocally)
        {
          KeyUp(0, turnOff);
          KeyDown(0, turnOn);
        }
        else
        {
          KeyUpRemote(0, turnOff);
          KeyDownRemote(0, turnOn);
        }

        _keyboardKeyRepeated = false;
      }
      else if (keyCode != _lastKeyboardKeyCode && modifiers != _lastKeyboardModifiers)
      {
        uint turnOff = _lastKeyboardModifiers & ~modifiers;
        uint turnOn = modifiers & ~_lastKeyboardModifiers;

        if (_handleKeyboardLocally)
        {
          KeyUp(_lastKeyboardKeyCode, turnOff);
          KeyDown(keyCode, turnOn);
        }
        else
        {
          KeyUpRemote(_lastKeyboardKeyCode, turnOff);
          KeyDownRemote(keyCode, turnOn);
        }

        _keyboardKeyRepeated = false;
      }
      else if (keyCode == _lastKeyboardKeyCode && modifiers == _lastKeyboardModifiers)
      {
        // Repeats ...
        TimeSpan timeBetween = DateTime.Now.Subtract(_lastKeyboardKeyTime);

        int firstRepeat = _keyboardFirstRepeat;
        int heldRepeats = _keyboardHeldRepeats;
        if (_useSystemRatesRemote)
        {
          firstRepeat = 250 + (SystemInformation.KeyboardDelay*250);
          heldRepeats = (int) (1000.0/(2.5 + (SystemInformation.KeyboardSpeed*0.888)));
        }

        if (!_keyboardKeyRepeated && timeBetween.TotalMilliseconds < firstRepeat)
          return;

        if (_keyboardKeyRepeated && timeBetween.TotalMilliseconds < heldRepeats)
          return;

        if (_keyboardKeyRepeated && timeBetween.TotalMilliseconds > firstRepeat)
          _keyboardKeyRepeated = false;
        else
          _keyboardKeyRepeated = true;

        if (_handleKeyboardLocally)
          KeyDown(keyCode, modifiers);
        else
          KeyDownRemote(keyCode, modifiers);
      }

      _lastKeyboardKeyCode = keyCode;
      _lastKeyboardModifiers = modifiers;

      _lastKeyboardKeyTime = DateTime.Now;
    }

    private void MouseEvent(int deltaX, int deltaY, bool right, bool left)
    {
#if DEBUG
      DebugWriteLine("Mouse: DX {0}, DY {1}, Right: {2}, Left: {3}", deltaX, deltaY, right, left);
#endif

      if (!_enableMouseInput)
        return;

      #region Buttons

      Mouse.MouseEvents buttons = Mouse.MouseEvents.None;
      if ((_mouseButtons & Mouse.MouseEvents.RightDown) != 0)
      {
        if (!right)
        {
          buttons |= Mouse.MouseEvents.RightUp;
          _mouseButtons &= ~Mouse.MouseEvents.RightDown;
        }
      }
      else
      {
        if (right)
        {
          buttons |= Mouse.MouseEvents.RightDown;
          _mouseButtons |= Mouse.MouseEvents.RightDown;
        }
      }
      if ((_mouseButtons & Mouse.MouseEvents.LeftDown) != 0)
      {
        if (!left)
        {
          buttons |= Mouse.MouseEvents.LeftUp;
          _mouseButtons &= ~Mouse.MouseEvents.LeftDown;
        }
      }
      else
      {
        if (left)
        {
          buttons |= Mouse.MouseEvents.LeftDown;
          _mouseButtons |= Mouse.MouseEvents.LeftDown;
        }
      }

      if (buttons != Mouse.MouseEvents.None)
      {
        if (_handleMouseLocally)
          Mouse.Button(buttons);
      }

      #endregion Buttons

      #region Movement Delta

      deltaX = (int) ((double) deltaX*_mouseSensitivity);
      deltaY = (int) ((double) deltaY*_mouseSensitivity);

      if (deltaX != 0 || deltaY != 0)
      {
        if (_handleMouseLocally)
          Mouse.Move(deltaX, deltaY, false);
      }

      #endregion Movement Delta

      if (!_handleMouseLocally)
        _mouseHandler(Name, deltaX, deltaY, (int) buttons);
    }

    private void KeyUpRemote(uint keyCode, uint modifiers)
    {
      if (_keyboardHandler == null)
        return;

      if (keyCode != 0)
      {
        Keyboard.VKey vKey = ConvertMceKeyCodeToVKey(keyCode);
        _keyboardHandler(Name, (int) vKey, true);
      }

      if (modifiers != 0)
      {
        if ((modifiers & (uint) KeyModifiers.LeftAlt) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_LMENU, true);
        if ((modifiers & (uint) KeyModifiers.LeftControl) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_LCONTROL, true);
        if ((modifiers & (uint) KeyModifiers.LeftShift) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_LSHIFT, true);
        if ((modifiers & (uint) KeyModifiers.LeftWin) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_LWIN, true);

        if ((modifiers & (uint) KeyModifiers.RightAlt) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_RMENU, true);
        if ((modifiers & (uint) KeyModifiers.RightControl) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_RCONTROL, true);
        if ((modifiers & (uint) KeyModifiers.RightShift) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_RSHIFT, true);
        if ((modifiers & (uint) KeyModifiers.RightWin) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_RWIN, true);
      }
    }

    private void KeyDownRemote(uint keyCode, uint modifiers)
    {
      if (_keyboardHandler == null)
        return;

      if (modifiers != 0)
      {
        if ((modifiers & (uint) KeyModifiers.LeftAlt) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_LMENU, false);
        if ((modifiers & (uint) KeyModifiers.LeftControl) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_LCONTROL, false);
        if ((modifiers & (uint) KeyModifiers.LeftShift) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_LSHIFT, false);
        if ((modifiers & (uint) KeyModifiers.LeftWin) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_LWIN, false);

        if ((modifiers & (uint) KeyModifiers.RightAlt) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_RMENU, false);
        if ((modifiers & (uint) KeyModifiers.RightControl) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_RCONTROL, false);
        if ((modifiers & (uint) KeyModifiers.RightShift) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_RSHIFT, false);
        if ((modifiers & (uint) KeyModifiers.RightWin) != 0)
          _keyboardHandler(Name, (int) Keyboard.VKey.VK_RWIN, false);
      }

      if (keyCode != 0)
      {
        Keyboard.VKey vKey = ConvertMceKeyCodeToVKey(keyCode);
        _keyboardHandler(Name, (int) vKey, false);
      }
    }

    // TODO: Convert this function to a lookup from an XML file, then provide multiple files and a way to fine-tune...
    private static Keyboard.VKey ConvertMceKeyCodeToVKey(uint keyCode)
    {
      switch (keyCode)
      {
        case 0x04:
          return Keyboard.VKey.VK_A;
        case 0x05:
          return Keyboard.VKey.VK_B;
        case 0x06:
          return Keyboard.VKey.VK_C;
        case 0x07:
          return Keyboard.VKey.VK_D;
        case 0x08:
          return Keyboard.VKey.VK_E;
        case 0x09:
          return Keyboard.VKey.VK_F;
        case 0x0A:
          return Keyboard.VKey.VK_G;
        case 0x0B:
          return Keyboard.VKey.VK_H;
        case 0x0C:
          return Keyboard.VKey.VK_I;
        case 0x0D:
          return Keyboard.VKey.VK_J;
        case 0x0E:
          return Keyboard.VKey.VK_K;
        case 0x0F:
          return Keyboard.VKey.VK_L;
        case 0x10:
          return Keyboard.VKey.VK_M;
        case 0x11:
          return Keyboard.VKey.VK_N;
        case 0x12:
          return Keyboard.VKey.VK_O;
        case 0x13:
          return Keyboard.VKey.VK_P;
        case 0x14:
          return Keyboard.VKey.VK_Q;
        case 0x15:
          return Keyboard.VKey.VK_R;
        case 0x16:
          return Keyboard.VKey.VK_S;
        case 0x17:
          return Keyboard.VKey.VK_T;
        case 0x18:
          return Keyboard.VKey.VK_U;
        case 0x19:
          return Keyboard.VKey.VK_V;
        case 0x1A:
          return Keyboard.VKey.VK_W;
        case 0x1B:
          return Keyboard.VKey.VK_X;
        case 0x1C:
          return Keyboard.VKey.VK_Y;
        case 0x1D:
          return Keyboard.VKey.VK_Z;
        case 0x1E:
          return Keyboard.VKey.VK_1;
        case 0x1F:
          return Keyboard.VKey.VK_2;
        case 0x20:
          return Keyboard.VKey.VK_3;
        case 0x21:
          return Keyboard.VKey.VK_4;
        case 0x22:
          return Keyboard.VKey.VK_5;
        case 0x23:
          return Keyboard.VKey.VK_6;
        case 0x24:
          return Keyboard.VKey.VK_7;
        case 0x25:
          return Keyboard.VKey.VK_8;
        case 0x26:
          return Keyboard.VKey.VK_9;
        case 0x27:
          return Keyboard.VKey.VK_0;
        case 0x28:
          return Keyboard.VKey.VK_RETURN;
        case 0x29:
          return Keyboard.VKey.VK_ESCAPE;
        case 0x2A:
          return Keyboard.VKey.VK_BACK;
        case 0x2B:
          return Keyboard.VKey.VK_TAB;
        case 0x2C:
          return Keyboard.VKey.VK_SPACE;
        case 0x2D:
          return Keyboard.VKey.VK_OEM_MINUS;
        case 0x2E:
          return Keyboard.VKey.VK_OEM_PLUS;
        case 0x2F:
          return Keyboard.VKey.VK_OEM_4;
        case 0x30:
          return Keyboard.VKey.VK_OEM_6;
        case 0x31:
          return Keyboard.VKey.VK_OEM_5;
          //case 0x32:return Keyboard.VKEY.VK_Non-US #;
        case 0x33:
          return Keyboard.VKey.VK_OEM_1;
        case 0x34:
          return Keyboard.VKey.VK_OEM_7;
        case 0x35:
          return Keyboard.VKey.VK_OEM_3;
        case 0x36:
          return Keyboard.VKey.VK_OEM_COMMA;
        case 0x37:
          return Keyboard.VKey.VK_OEM_PERIOD;
        case 0x38:
          return Keyboard.VKey.VK_OEM_2;
        case 0x39:
          return Keyboard.VKey.VK_CAPITAL;
        case 0x3A:
          return Keyboard.VKey.VK_F1;
        case 0x3B:
          return Keyboard.VKey.VK_F2;
        case 0x3C:
          return Keyboard.VKey.VK_F3;
        case 0x3D:
          return Keyboard.VKey.VK_F4;
        case 0x3E:
          return Keyboard.VKey.VK_F5;
        case 0x3F:
          return Keyboard.VKey.VK_F6;
        case 0x40:
          return Keyboard.VKey.VK_F7;
        case 0x41:
          return Keyboard.VKey.VK_F8;
        case 0x42:
          return Keyboard.VKey.VK_F9;
        case 0x43:
          return Keyboard.VKey.VK_F10;
        case 0x44:
          return Keyboard.VKey.VK_F11;
        case 0x45:
          return Keyboard.VKey.VK_F12;
        case 0x46:
          return Keyboard.VKey.VK_PRINT;
        case 0x47:
          return Keyboard.VKey.VK_SCROLL;
        case 0x48:
          return Keyboard.VKey.VK_PAUSE;
        case 0x49:
          return Keyboard.VKey.VK_INSERT;
        case 0x4A:
          return Keyboard.VKey.VK_HOME;
        case 0x4B:
          return Keyboard.VKey.VK_PRIOR;
        case 0x4C:
          return Keyboard.VKey.VK_DELETE;
        case 0x4D:
          return Keyboard.VKey.VK_END;
        case 0x4E:
          return Keyboard.VKey.VK_NEXT;
        case 0x4F:
          return Keyboard.VKey.VK_RIGHT;
        case 0x50:
          return Keyboard.VKey.VK_LEFT;
        case 0x51:
          return Keyboard.VKey.VK_DOWN;
        case 0x52:
          return Keyboard.VKey.VK_UP;
        case 0x64:
          return Keyboard.VKey.VK_OEM_102;
        case 0x65:
          return Keyboard.VKey.VK_APPS;

        default:
          throw new ArgumentException(String.Format("Unknown Key Value {0}", keyCode), "keyCode");
      }
    }

    private static void KeyUp(uint keyCode, uint modifiers)
    {
      if (keyCode != 0)
      {
        Keyboard.VKey vKey = ConvertMceKeyCodeToVKey(keyCode);
        Keyboard.KeyUp(vKey);
      }

      if (modifiers != 0)
      {
        if ((modifiers & (uint) KeyModifiers.LeftAlt) != 0)
          Keyboard.KeyUp(Keyboard.VKey.VK_LMENU);
        if ((modifiers & (uint) KeyModifiers.LeftControl) != 0)
          Keyboard.KeyUp(Keyboard.VKey.VK_LCONTROL);
        if ((modifiers & (uint) KeyModifiers.LeftShift) != 0)
          Keyboard.KeyUp(Keyboard.VKey.VK_LSHIFT);
        if ((modifiers & (uint) KeyModifiers.LeftWin) != 0)
          Keyboard.KeyUp(Keyboard.VKey.VK_LWIN);

        if ((modifiers & (uint) KeyModifiers.RightAlt) != 0)
          Keyboard.KeyUp(Keyboard.VKey.VK_RMENU);
        if ((modifiers & (uint) KeyModifiers.RightControl) != 0)
          Keyboard.KeyUp(Keyboard.VKey.VK_RCONTROL);
        if ((modifiers & (uint) KeyModifiers.RightShift) != 0)
          Keyboard.KeyUp(Keyboard.VKey.VK_RSHIFT);
        if ((modifiers & (uint) KeyModifiers.RightWin) != 0)
          Keyboard.KeyUp(Keyboard.VKey.VK_RWIN);
      }
    }

    private static void KeyDown(uint keyCode, uint modifiers)
    {
      if (modifiers != 0)
      {
        if ((modifiers & (uint) KeyModifiers.LeftAlt) != 0)
          Keyboard.KeyDown(Keyboard.VKey.VK_LMENU);
        if ((modifiers & (uint) KeyModifiers.LeftControl) != 0)
          Keyboard.KeyDown(Keyboard.VKey.VK_LCONTROL);
        if ((modifiers & (uint) KeyModifiers.LeftShift) != 0)
          Keyboard.KeyDown(Keyboard.VKey.VK_LSHIFT);
        if ((modifiers & (uint) KeyModifiers.LeftWin) != 0)
          Keyboard.KeyDown(Keyboard.VKey.VK_LWIN);

        if ((modifiers & (uint) KeyModifiers.RightAlt) != 0)
          Keyboard.KeyDown(Keyboard.VKey.VK_RMENU);
        if ((modifiers & (uint) KeyModifiers.RightControl) != 0)
          Keyboard.KeyDown(Keyboard.VKey.VK_RCONTROL);
        if ((modifiers & (uint) KeyModifiers.RightShift) != 0)
          Keyboard.KeyDown(Keyboard.VKey.VK_RSHIFT);
        if ((modifiers & (uint) KeyModifiers.RightWin) != 0)
          Keyboard.KeyDown(Keyboard.VKey.VK_RWIN);
      }

      if (keyCode != 0)
      {
        Keyboard.VKey vKey = ConvertMceKeyCodeToVKey(keyCode);
        Keyboard.KeyDown(vKey);
      }
    }

    #endregion Implementation

    #region Debug

#if DEBUG

    private static StreamWriter _debugFile;

    /// <summary>
    /// Opens a debug output file.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    private static void DebugOpen(string fileName)
    {
      try
      {
#if TEST_APPLICATION
        string path = fileName;
#else
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                                   String.Format("IR Server Suite\\Logs\\{0}", fileName));
#endif
        _debugFile = new StreamWriter(path, false);
        _debugFile.AutoFlush = true;
      }
#if TRACE
      catch (Exception ex)
      {
        Trace.WriteLine(ex.ToString());
#else
      catch
      {
#endif
        _debugFile = null;
      }
    }

    /// <summary>
    /// Closes the debug output file.
    /// </summary>
    private static void DebugClose()
    {
      if (_debugFile != null)
      {
        _debugFile.Close();
        _debugFile.Dispose();
        _debugFile = null;
      }
    }

    /// <summary>
    /// Writes a line to the debug output file.
    /// </summary>
    /// <param name="line">The line.</param>
    /// <param name="args">Formatting arguments.</param>
    private static void DebugWriteLine(string line, params object[] args)
    {
      if (_debugFile != null)
      {
        _debugFile.Write("{0:yyyy-MM-dd HH:mm:ss.ffffff} - ", DateTime.Now);
        _debugFile.WriteLine(line, args);
      }
#if TRACE
      else
      {
        Trace.WriteLine(String.Format(line, args));
      }
#endif
    }

    /// <summary>
    /// Writes a string to the debug output file.
    /// </summary>
    /// <param name="text">The string to write.</param>
    /// <param name="args">Formatting arguments.</param>
    private static void DebugWrite(string text, params object[] args)
    {
      if (_debugFile != null)
      {
        _debugFile.Write(text, args);
      }
#if TRACE
      else
      {
        Trace.Write(String.Format(text, args));
      }
#endif
    }

    /// <summary>
    /// Writes a new line to the debug output file.
    /// </summary>
    private static void DebugWriteNewLine()
    {
      if (_debugFile != null)
      {
        _debugFile.WriteLine();
      }
#if TRACE
      else
      {
        Trace.WriteLine(String.Empty);
      }
#endif
    }

    /// <summary>
    /// Dumps an Array to the debug output file.
    /// </summary>
    /// <param name="array">The array.</param>
    private static void DebugDump(Array array)
    {
      foreach (object item in array)
      {
        if (item is byte) DebugWrite("{0:X2}", (byte) item);
        else if (item is ushort) DebugWrite("{0:X4}", (ushort) item);
        else if (item is int) DebugWrite("{1}{0}", (int) item, (int) item > 0 ? "+" : String.Empty);
        else DebugWrite("{0}", item);

        DebugWrite(", ");
      }

      DebugWriteNewLine();
    }

#endif

    #endregion Debug
  }
}