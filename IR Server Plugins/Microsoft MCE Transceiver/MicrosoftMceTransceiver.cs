using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Win32.SafeHandles;

using IRServerPluginInterface;

namespace MicrosoftMceTransceiver
{

  #region Enumerations

  /// <summary>
  /// Type of blaster in use.
  /// </summary>
  public enum BlasterType
  {
    /// <summary>
    /// Device is a first party Microsoft MCE transceiver.
    /// </summary>
    Microsoft = 0,
    /// <summary>
    /// Device is an third party SMK MCE transceiver.
    /// </summary>
    SMK = 1
  }

  /// <summary>
  /// The blaster port to send IR codes to.
  /// </summary>
  enum BlasterPort
  {
    /// <summary>
    /// Send IR codes to both blaster ports.
    /// </summary>
    Both = 0,
    /// <summary>
    /// Send IR codes to blaster port 1 only.
    /// </summary>
    Port_1 = 1,
    /// <summary>
    /// Send IR codes to blaster port 2 only.
    /// </summary>
    Port_2 = 2
  }

  #endregion Enumerations

  #region Delegates

  delegate void DeviceEventHandler();

  #endregion Delegates

  public class MicrosoftMceTransceiver : IIRServerPlugin
  {

    #region Constants

    static readonly string ConfigurationFile =
      Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
      "\\IR Server Suite\\IR Server\\Microsoft MCE Transceiver.xml";
    
    const int DeviceBufferSize = 255;
    const int PacketTimeout = 100;

    // Microsoft Port Packets
    static readonly byte[][] MicrosoftPorts = new byte[][]
			{
				new byte[] { 0x9F, 0x08, 0x06 },        // Both
				new byte[] { 0x9F, 0x08, 0x04 },	      // 1
				new byte[] { 0x9F, 0x08, 0x02 },	      // 2
			};

    // SMK (or Topseed) Port Packets
    static readonly byte[][] SmkPorts = new byte[][]
			{
				new byte[] { 0x9F, 0x08, 0x00 },	      // Both
				new byte[] { 0x9F, 0x08, 0x01 },	      // 1
				new byte[] { 0x9F, 0x08, 0x02 },        // 2
			};

    // Device Initialization Packets
    static readonly byte[] InitPacket1  = { 0x00, 0xFF, 0xAA, 0xFF, 0x0B };
    static readonly byte[] InitPacket2  = { 0xFF, 0x18 };

    // Learn Initialization Packets
    static readonly byte[] LearnPacket1 = { 0x9F, 0x0C, 0x0F, 0xA0 };
    static readonly byte[] LearnPacket2 = { 0x9F, 0x14, 0x01 };

    #endregion Constants

    #region Variables

    bool _enableRemoteInput     = true;
    int _remoteFirstRepeat      = 400;
    int _remoteHeldRepeats      = 250;

    bool _enableKeyboardInput   = true;
    int _keyboardFirstRepeat    = 350;
    int _keyboardHeldRepeats    = 0;
    bool _handleKeyboardLocally = true;

    bool _enableMouseInput      = true;
    bool _handleMouseLocally    = true;
    double _mouseSensitivity    = 1.0d;

    int _learnTimeout           = 10000;

    BlasterType  _blasterType   = BlasterType.Microsoft;
    BlasterPort _blasterPort    = BlasterPort.Both;
    
    List<byte> _learnedNativeData = null;
    //int _learnedCarrier = 0;

    IRProtocol _lastRemoteButtonCodeType = IRProtocol.None;
    uint _lastRemoteButtonKeyCode = 0;
    DateTime _lastRemoteButtonTime = DateTime.Now;
    bool _remoteButtonRepeated = false;
    
    bool _keyboardKeyRepeated = false;
    DateTime _lastKeyboardKeyTime = DateTime.Now;

    uint _lastKeyboardKeyCode = 0;
    uint _lastKeyboardModifiers = 0;

    Mouse.MouseEvents _mouseButtons = Mouse.MouseEvents.None;
    
    Guid _deviceClass;
    SafeFileHandle _deviceHandle;
    FileStream _readStream;
    FileStream _writeStream;
    byte[] _deviceBuffer;
    NotifyWindow _notifyWindow;
    int _learnStartTick;
    bool _learning;

    DateTime _lastPacketTime = DateTime.Now;

    bool _replacementDriver = false;

    RemoteHandler _remoteButtonHandler = null;
    KeyboardHandler _keyboardHandler = null;
    MouseHandler _mouseHandler = null;

    #endregion Variables

    #region IIRServerPlugin Members

    public string Name          { get { return "Microsoft MCE"; } }
    public string Version       { get { return "1.0.3.3"; } }
    public string Author        { get { return "and-81"; } }
    public string Description   { get { return "Microsoft MCE Infrared Transceiver"; } }
    public bool   CanReceive    { get { return true; } }
    public bool   CanTransmit   { get { return true; } }
    public bool   CanLearn      { get { return true; } }
    public bool   CanConfigure  { get { return true; } }

    public RemoteHandler RemoteCallback
    {
      get { return _remoteButtonHandler; }
      set { _remoteButtonHandler = value; }
    }

    public KeyboardHandler KeyboardCallback
    {
      get { return _keyboardHandler; }
      set { _keyboardHandler = value; }
    }

    public MouseHandler MouseCallback
    {
      get { return _mouseHandler; }
      set { _mouseHandler = value; }
    }

    public string[] AvailablePorts
    {
      get { return Enum.GetNames(typeof(BlasterPort)); }
    }

    public void Configure()
    {
      LoadSettings();

      Configure config = new Configure();

      config.BlastType            = _blasterType;
      config.LearnTimeout         = _learnTimeout;

      config.EnableRemote         = _enableRemoteInput;
      config.RemoteRepeatDelay    = _remoteFirstRepeat;
      config.RemoteHeldDelay      = _remoteHeldRepeats;

      config.EnableKeyboard       = _enableKeyboardInput;
      config.KeyboardRepeatDelay  = _keyboardFirstRepeat;
      config.KeyboardHeldDelay    = _keyboardHeldRepeats;
      config.HandleKeyboardLocal  = _handleKeyboardLocally;

      config.EnableMouse          = _enableMouseInput;
      config.HandleMouseLocal     = _handleMouseLocally;
      config.MouseSensitivity     = _mouseSensitivity;

      if (config.ShowDialog() == DialogResult.OK)
      {
        _blasterType            = config.BlastType;
        _learnTimeout           = config.LearnTimeout;

        _enableRemoteInput      = config.EnableRemote;
        _remoteFirstRepeat      = config.RemoteRepeatDelay;
        _remoteHeldRepeats      = config.RemoteHeldDelay;

        _enableKeyboardInput    = config.EnableKeyboard;
        _keyboardFirstRepeat    = config.KeyboardRepeatDelay;
        _keyboardHeldRepeats    = config.KeyboardHeldDelay;
        _handleKeyboardLocally  = config.HandleKeyboardLocal;

        _enableMouseInput       = config.EnableMouse;
        _handleMouseLocally     = config.HandleMouseLocal;
        _mouseSensitivity       = config.MouseSensitivity;        

        SaveSettings();
      }
    }
    public bool Start()
    {
      LoadSettings();

      // Stop Microsoft MCE ehRecvr, mcrdsvc and ehSched processes (if they exist)
      try
      {
        ServiceController[] services = ServiceController.GetServices();
        foreach (ServiceController service in services)
        {
          if (service.ServiceName.Equals("ehRecvr", StringComparison.InvariantCultureIgnoreCase))
          {
            if (service.Status != ServiceControllerStatus.Stopped && service.Status != ServiceControllerStatus.StopPending)
            {
              service.Stop();
            }
          }
          else if (service.ServiceName.Equals("ehSched", StringComparison.InvariantCultureIgnoreCase))
          {
            if (service.Status != ServiceControllerStatus.Stopped && service.Status != ServiceControllerStatus.StopPending)
            {
              service.Stop();
            }
          }
          else if (service.ServiceName.Equals("mcrdsvc", StringComparison.InvariantCultureIgnoreCase))
          {
            if (service.Status != ServiceControllerStatus.Stopped && service.Status != ServiceControllerStatus.StopPending)
            {
              service.Stop();
            }
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }

      // Kill Microsoft MCE ehtray process (if it exists)
      try
      {
        Process[] processes = Process.GetProcesses();
        foreach (Process proc in processes)
          if (proc.ProcessName.Equals("ehtray", StringComparison.InvariantCultureIgnoreCase))
            proc.Kill();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
      
      Init();

      return true;
    }
    public void Suspend() { }
    public void Resume() { }
    public void Stop()
    {
      OnDeviceRemoval();
    }

    public bool Transmit(string file)
    {
      MceIrCode code = ReadIRFile(file);
      if (code == null)
        return false;

      return Send(code, _blasterPort, _blasterType);
    }
    public LearnStatus Learn(out byte[] data)
    {
      _learnedNativeData = new List<byte>();
      data = null;

      BeginLearn();

      // Wait for the learning to finish ...
      while (_learning && Environment.TickCount < _learnStartTick + _learnTimeout)
      {
        Thread.Sleep(100);
      }

      LearnStatus status = LearnStatus.Failure;

      if (_learning)
      {
        _learning = false;

        try
        {
          DeviceAccess.CancelDeviceIo(_readStream.SafeFileHandle);
        }
        catch { }

        status = LearnStatus.Timeout;
      }
      else if (_learnedNativeData != null && _learnedNativeData.Count > 0)
      {
        data = _learnedNativeData.ToArray();

        status = LearnStatus.Success;
      }

      // Start a new read ...
      _readStream.BeginRead(_deviceBuffer, 0, _deviceBuffer.Length, new AsyncCallback(OnReadComplete), null);
      
      return status;
    }

    public bool SetPort(string port)
    {
      try
      {
        _blasterPort = (BlasterPort)Enum.Parse(typeof(BlasterPort), port);
      }
      catch
      {
        return false;
      }

      return true;
    }

    #endregion IIRServerPlugin Members

    #region Implementation

    void LoadSettings()
    {
      XmlDocument doc = new XmlDocument();

      try   { doc.Load(ConfigurationFile); }
      catch { return; }

      try   { _blasterType = (BlasterType)Enum.Parse(typeof(BlasterType), doc.DocumentElement.Attributes["BlastType"].Value); } catch {}
      try   { _learnTimeout = int.Parse(doc.DocumentElement.Attributes["LearnTimeout"].Value); } catch {}

      try   { _enableRemoteInput = bool.Parse(doc.DocumentElement.Attributes["EnableRemoteInput"].Value); } catch {}
      try   { _remoteFirstRepeat = int.Parse(doc.DocumentElement.Attributes["RemoteFirstRepeat"].Value); } catch {}
      try   { _remoteHeldRepeats = int.Parse(doc.DocumentElement.Attributes["RemoteHeldRepeats"].Value); } catch {}

      try   { _enableKeyboardInput = bool.Parse(doc.DocumentElement.Attributes["EnableKeyboardInput"].Value); } catch {}
      try   { _keyboardFirstRepeat = int.Parse(doc.DocumentElement.Attributes["KeyboardFirstRepeat"].Value); } catch {}
      try   { _keyboardHeldRepeats = int.Parse(doc.DocumentElement.Attributes["KeyboardHeldRepeats"].Value); } catch {}
      try   { _handleKeyboardLocally = bool.Parse(doc.DocumentElement.Attributes["HandleKeyboardLocally"].Value); } catch {}

      try   { _enableMouseInput = bool.Parse(doc.DocumentElement.Attributes["EnableMouseInput"].Value); } catch {}
      try   { _handleMouseLocally = bool.Parse(doc.DocumentElement.Attributes["HandleMouseLocally"].Value); } catch {}
      try   { _mouseSensitivity = double.Parse(doc.DocumentElement.Attributes["MouseSensitivity"].Value); } catch {}
    }
    void SaveSettings()
    {
      try
      {
        XmlTextWriter writer  = new XmlTextWriter(ConfigurationFile, System.Text.Encoding.UTF8);
        writer.Formatting     = Formatting.Indented;
        writer.Indentation    = 1;
        writer.IndentChar     = (char)9;
        writer.WriteStartDocument(true);
        writer.WriteStartElement("settings"); // <settings>

        writer.WriteAttributeString("BlastType",              Enum.GetName(typeof(BlasterType), _blasterType));
        writer.WriteAttributeString("LearnTimeout",           _learnTimeout.ToString());

        writer.WriteAttributeString("EnableRemoteInput",      _enableRemoteInput.ToString());
        writer.WriteAttributeString("RemoteFirstRepeat",      _remoteFirstRepeat.ToString());
        writer.WriteAttributeString("RemoteHeldRepeats",      _remoteHeldRepeats.ToString());

        writer.WriteAttributeString("EnableKeyboardInput",    _enableKeyboardInput.ToString());
        writer.WriteAttributeString("KeyboardFirstRepeat",    _keyboardFirstRepeat.ToString());
        writer.WriteAttributeString("KeyboardHeldRepeats",    _keyboardHeldRepeats.ToString());
        writer.WriteAttributeString("HandleKeyboardLocally",  _handleKeyboardLocally.ToString());

        writer.WriteAttributeString("EnableMouseInput",       _enableMouseInput.ToString());
        writer.WriteAttributeString("HandleMouseLocally",     _handleMouseLocally.ToString());
        writer.WriteAttributeString("MouseSensitivity",       _mouseSensitivity.ToString());

        writer.WriteEndElement(); // </settings>
        writer.WriteEndDocument();
        writer.Close();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }

    void Init()
    {
      _deviceBuffer = new byte[DeviceBufferSize];

      _notifyWindow = new NotifyWindow();
      _notifyWindow.Create();

      Open();

      _notifyWindow.Class = _deviceClass;
      _notifyWindow.DeviceArrival += new DeviceEventHandler(OnDeviceArrival);
      _notifyWindow.DeviceRemoval += new DeviceEventHandler(OnDeviceRemoval);

      _notifyWindow.RegisterDeviceRemoval(_deviceHandle.DangerousGetHandle());

      _writeStream.Write(InitPacket1, 0, InitPacket1.Length);
      _writeStream.Write(InitPacket2, 0, InitPacket2.Length);
      _writeStream.Flush();

      _readStream.Flush();
      _readStream.BeginRead(_deviceBuffer, 0, _deviceBuffer.Length, new AsyncCallback(OnReadComplete), null);
    }

    void Open()
    {
      Guid deviceClass;
      string devicePath = null;

      // Try XP eHome driver ...
      deviceClass = new Guid(0x7951772d, 0xcd50, 0x49b7, 0xb1, 0x03, 0x2b, 0xaa, 0xc4, 0x94, 0xfc, 0x57);
      _replacementDriver = false;
      try
      {
        devicePath = DeviceAccess.FindDevice(deviceClass);
      }
      catch { }

      // Try replacement driver ...
      if (devicePath == null)
      {
        deviceClass = new Guid(0x00873fdf, 0x61a8, 0x11d1, 0xaa, 0x5e, 0x00, 0xc0, 0x4f, 0xb1, 0x72, 0x8b);
        _replacementDriver = true;
        try
        {
          devicePath = DeviceAccess.FindDevice(deviceClass);
        }
        catch { }
      }

      // Try Vista eHome driver ...
      /*if (devicePath == null)
      {
        deviceClass = new Guid(0x36fc9e60, 0xc465, 0x11cf, 0x80, 0x56, 0x44, 0x45, 0x53, 0x54, 0x00, 0x00);
        try
        {
          devicePath = DeviceAccess.FindDevice(deviceClass);
        }
        catch { }
      }*/

      if (devicePath == null)
        throw new Exception("No MCE Transceiver detected");

      int lastError;

      if (_replacementDriver)
      {
        SafeFileHandle deviceHandleRead = DeviceAccess.OpenHandle(devicePath + "\\Pipe01", DeviceAccess.FileAccessTypes.GenericRead, DeviceAccess.FileShares.Read | DeviceAccess.FileShares.Write);
        lastError = Marshal.GetLastWin32Error();

        if (lastError != 0)
          throw new Win32Exception(lastError);

        SafeFileHandle deviceHandleWrite = DeviceAccess.OpenHandle(devicePath + "\\Pipe00", DeviceAccess.FileAccessTypes.GenericWrite, DeviceAccess.FileShares.Read | DeviceAccess.FileShares.Write);
        lastError = Marshal.GetLastWin32Error();

        if (lastError != 0)
          throw new Win32Exception(lastError);

        _deviceHandle = deviceHandleRead;

        _readStream = new FileStream(deviceHandleRead, FileAccess.Read, _deviceBuffer.Length, true);
        _writeStream = new FileStream(deviceHandleWrite, FileAccess.Write, _deviceBuffer.Length, true);
      }
      else
      {
        SafeFileHandle deviceHandleReadWrite = DeviceAccess.OpenHandle(devicePath + "\\Pipe01", DeviceAccess.FileAccessTypes.GenericRead | DeviceAccess.FileAccessTypes.GenericWrite, DeviceAccess.FileShares.Read | DeviceAccess.FileShares.Write);
        lastError = Marshal.GetLastWin32Error();

        if (lastError != 0)
          throw new Win32Exception(lastError);

        _deviceHandle = deviceHandleReadWrite;

        _readStream = _writeStream = new FileStream(deviceHandleReadWrite, FileAccess.ReadWrite, _deviceBuffer.Length, true);
      }

      _deviceClass = deviceClass;
    }

    void OnDeviceArrival()
    {
      _notifyWindow.UnregisterDeviceArrival();

      if (_readStream != null)
        return;

      Open();

      _notifyWindow.RegisterDeviceRemoval(_deviceHandle.DangerousGetHandle());
    }
    void OnDeviceRemoval()
    {
      _notifyWindow.UnregisterDeviceRemoval();
      _notifyWindow.RegisterDeviceArrival();

      if (_readStream == null)
        return;

      try
      {
        _readStream.Close();
        _readStream = null;

        if (_writeStream != null)
        {
          _writeStream.Close();
          _writeStream = null;
        }
      }
      catch (IOException)
      {
        // we are closing the stream so ignore this
      }
    }

    void BeginLearn()
    {
      try
      {
        _writeStream.Write(LearnPacket1, 0, LearnPacket1.Length);
        _writeStream.Write(LearnPacket2, 0, LearnPacket2.Length);
        _writeStream.Flush();

        _learnStartTick = Environment.TickCount;
        _learning = true;
        _readStream.BeginRead(_deviceBuffer, 0, _deviceBuffer.Length, new AsyncCallback(OnReadComplete), null);
      }
      catch
      {
        _learning = false;
      }
    }

    void OnReadComplete(IAsyncResult asyncResult)
    {
      if (_readStream == null)
        return;

      int bytesRead = 0;
      try
      {
        bytesRead = _readStream.EndRead(asyncResult);
      }
      catch
      {
        return;
      }
      if (bytesRead == 0)
        return;

      TimeSpan sinceLastPacket = DateTime.Now.Subtract(_lastPacketTime);
      if (sinceLastPacket.TotalMilliseconds >= PacketTimeout)
      {
        IRCodeConversion.ConvertNativeDataToTimingData(null);
        IRDecoder.DecodeIR(null, null, null, null);
      }

      _lastPacketTime = DateTime.Now;

      byte[] packetBytes = new byte[bytesRead];
      Array.Copy(_deviceBuffer, packetBytes, bytesRead);

      if (packetBytes[0] >= 0x81 && packetBytes[0] <= 0x84)
        HandlePacket(packetBytes);

      // begin another asynchronous read from the device
      _readStream.BeginRead(_deviceBuffer, 0, _deviceBuffer.Length, new AsyncCallback(OnReadComplete), null);
    }

    void HandlePacket(byte[] rawPacket)
    {
      byte[] nativeData = IRCodeConversion.ConvertRawPacketToNativeData(rawPacket);

      if (_learning)
      {
        if (nativeData == null)
        {
          if (_learnedNativeData.Count > 0)
          {
            _learnedNativeData = null;
            _learning = false;
          }

          return;
        }

        _learnedNativeData.AddRange(nativeData);

        if (Array.IndexOf(rawPacket, (byte)0x9F) != -1)
          _learning = false;
      }
      else
      {
        if (nativeData == null)
          return;

        uint[] timingData = IRCodeConversion.ConvertNativeDataToTimingData(nativeData);

        IRDecoder.DecodeIR(
          timingData,
          new RemoteCallback(RemoteEvent),
          new KeyboardCallback(KeyboardEvent),
          new MouseCallback(MouseEvent));
      }
    }

    void RemoteEvent(IRProtocol codeType, uint keyCode)
    {
      if (!_enableRemoteInput)
        return;

      if (_lastRemoteButtonCodeType == codeType && _lastRemoteButtonKeyCode == keyCode)
      {
        TimeSpan timeBetween = DateTime.Now.Subtract(_lastRemoteButtonTime);

        if (!_remoteButtonRepeated && timeBetween.TotalMilliseconds < _remoteFirstRepeat)
          return;

        if (_remoteButtonRepeated && timeBetween.TotalMilliseconds < _remoteHeldRepeats)
          return;

        if (_remoteButtonRepeated && timeBetween.TotalMilliseconds > _remoteFirstRepeat)
          _remoteButtonRepeated = false;
        else
          _remoteButtonRepeated = true;
      }
      else
      {
        _lastRemoteButtonCodeType = codeType;
        _lastRemoteButtonKeyCode = keyCode;
        _remoteButtonRepeated = false;
      }

      _lastRemoteButtonTime = DateTime.Now;

      if (_remoteButtonHandler != null)
        _remoteButtonHandler(keyCode.ToString());

      //String.Format("{0}: {1}", Enum.GetName(typeof(IRProtocol), codeType), keyCode);
    }
    void KeyboardEvent(uint keyCode, uint modifiers)
    {
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

        if (!_keyboardKeyRepeated && timeBetween.TotalMilliseconds < _keyboardFirstRepeat)
          return;

        if (_keyboardKeyRepeated && timeBetween.TotalMilliseconds < _keyboardHeldRepeats)
          return;

        if (_keyboardKeyRepeated && timeBetween.TotalMilliseconds > _keyboardFirstRepeat)
          _keyboardKeyRepeated = false;
        else
          _keyboardKeyRepeated = true;

        if (_handleKeyboardLocally)
        {
          KeyDown(keyCode, modifiers);
        }
        else
        {
          KeyDownRemote(keyCode, modifiers);
        }
      }

      //Console.WriteLine("Keyboard button: {0}, {1}", keyCode, modifiers);

      _lastKeyboardKeyCode = keyCode;
      _lastKeyboardModifiers = modifiers;

      _lastKeyboardKeyTime = DateTime.Now;
    }
    void MouseEvent(int deltaX, int deltaY, bool right, bool left)
    {
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

      deltaX = (int)((double)deltaX * _mouseSensitivity);
      deltaY = (int)((double)deltaY * _mouseSensitivity);

      if (deltaX != 0 || deltaY != 0)
      {
        if (_handleMouseLocally)
          Mouse.Move(deltaX, deltaY, false);
      }

      #endregion Movement Delta

      if (!_handleMouseLocally)
        _mouseHandler(deltaX, deltaY, (int)buttons);

      //Console.WriteLine("Mouse: DX {0}, DY {1}, Right: {2}, Left: {3}", deltaX, deltaY, right, left);
    }

    static Keyboard.VKey ConvertMceKeyCodeToVKey(uint keyCode)
    {
      switch (keyCode)
      {
        case 0x04: return Keyboard.VKey.VK_A;
        case 0x05: return Keyboard.VKey.VK_B;
        case 0x06: return Keyboard.VKey.VK_C;
        case 0x07: return Keyboard.VKey.VK_D;
        case 0x08: return Keyboard.VKey.VK_E;
        case 0x09: return Keyboard.VKey.VK_F;
        case 0x0A: return Keyboard.VKey.VK_G;
        case 0x0B: return Keyboard.VKey.VK_H;
        case 0x0C: return Keyboard.VKey.VK_I;
        case 0x0D: return Keyboard.VKey.VK_J;
        case 0x0E: return Keyboard.VKey.VK_K;
        case 0x0F: return Keyboard.VKey.VK_L;
        case 0x10: return Keyboard.VKey.VK_M;
        case 0x11: return Keyboard.VKey.VK_N;
        case 0x12: return Keyboard.VKey.VK_O;
        case 0x13: return Keyboard.VKey.VK_P;
        case 0x14: return Keyboard.VKey.VK_Q;
        case 0x15: return Keyboard.VKey.VK_R;
        case 0x16: return Keyboard.VKey.VK_S;
        case 0x17: return Keyboard.VKey.VK_T;
        case 0x18: return Keyboard.VKey.VK_U;
        case 0x19: return Keyboard.VKey.VK_V;
        case 0x1A: return Keyboard.VKey.VK_W;
        case 0x1B: return Keyboard.VKey.VK_X;
        case 0x1C: return Keyboard.VKey.VK_Y;
        case 0x1D: return Keyboard.VKey.VK_Z;
        case 0x1E: return Keyboard.VKey.VK_1;
        case 0x1F: return Keyboard.VKey.VK_2;
        case 0x20: return Keyboard.VKey.VK_3;
        case 0x21: return Keyboard.VKey.VK_4;
        case 0x22: return Keyboard.VKey.VK_5;
        case 0x23: return Keyboard.VKey.VK_6;
        case 0x24: return Keyboard.VKey.VK_7;
        case 0x25: return Keyboard.VKey.VK_8;
        case 0x26: return Keyboard.VKey.VK_9;
        case 0x27: return Keyboard.VKey.VK_0;
        case 0x28: return Keyboard.VKey.VK_RETURN;
        case 0x29: return Keyboard.VKey.VK_ESCAPE;
        case 0x2A: return Keyboard.VKey.VK_BACK;
        case 0x2B: return Keyboard.VKey.VK_TAB;
        case 0x2C: return Keyboard.VKey.VK_SPACE;
        case 0x2D: return Keyboard.VKey.VK_OEM_MINUS;
        case 0x2E: return Keyboard.VKey.VK_OEM_PLUS;
        case 0x2F: return Keyboard.VKey.VK_OEM_4;
        case 0x30: return Keyboard.VKey.VK_OEM_6;
        case 0x31: return Keyboard.VKey.VK_OEM_5;
        //case 0x32:return Keyboard.VKEY.VK_Non-US #;
        case 0x33: return Keyboard.VKey.VK_OEM_1;
        case 0x34: return Keyboard.VKey.VK_OEM_7;
        case 0x35: return Keyboard.VKey.VK_OEM_3;
        case 0x36: return Keyboard.VKey.VK_OEM_COMMA;
        case 0x37: return Keyboard.VKey.VK_OEM_PERIOD;
        case 0x38: return Keyboard.VKey.VK_OEM_2;
        case 0x39: return Keyboard.VKey.VK_CAPITAL;
        case 0x3A: return Keyboard.VKey.VK_F1;
        case 0x3B: return Keyboard.VKey.VK_F2;
        case 0x3C: return Keyboard.VKey.VK_F3;
        case 0x3D: return Keyboard.VKey.VK_F4;
        case 0x3E: return Keyboard.VKey.VK_F5;
        case 0x3F: return Keyboard.VKey.VK_F6;
        case 0x40: return Keyboard.VKey.VK_F7;
        case 0x41: return Keyboard.VKey.VK_F8;
        case 0x42: return Keyboard.VKey.VK_F9;
        case 0x43: return Keyboard.VKey.VK_F10;
        case 0x44: return Keyboard.VKey.VK_F11;
        case 0x45: return Keyboard.VKey.VK_F12;
        case 0x46: return Keyboard.VKey.VK_PRINT;
        case 0x47: return Keyboard.VKey.VK_SCROLL;
        case 0x48: return Keyboard.VKey.VK_PAUSE;
        case 0x49: return Keyboard.VKey.VK_INSERT;
        case 0x4A: return Keyboard.VKey.VK_HOME;
        case 0x4B: return Keyboard.VKey.VK_PRIOR;
        case 0x4C: return Keyboard.VKey.VK_DELETE;
        case 0x4D: return Keyboard.VKey.VK_END;
        case 0x4E: return Keyboard.VKey.VK_NEXT;
        case 0x4F: return Keyboard.VKey.VK_RIGHT;
        case 0x50: return Keyboard.VKey.VK_LEFT;
        case 0x51: return Keyboard.VKey.VK_DOWN;
        case 0x52: return Keyboard.VKey.VK_UP;
        case 0x64: return Keyboard.VKey.VK_OEM_102;
        case 0x65: return Keyboard.VKey.VK_APPS;

        default:
          throw new Exception(string.Format("Unknown Key Value {0}", keyCode));
      }
    }

    static void KeyUp(uint keyCode, uint modifiers)
    {
      if (keyCode != 0)
      {
        Keyboard.VKey vKey = ConvertMceKeyCodeToVKey(keyCode);
        Keyboard.KeyUp(vKey);
      }

      if (modifiers != 0)
      {
        if ((modifiers & (uint)KeyModifiers.LeftAlt) != 0)
          Keyboard.KeyUp(Keyboard.VKey.VK_LMENU);
        if ((modifiers & (uint)KeyModifiers.LeftControl) != 0)
          Keyboard.KeyUp(Keyboard.VKey.VK_LCONTROL);
        if ((modifiers & (uint)KeyModifiers.LeftShift) != 0)
          Keyboard.KeyUp(Keyboard.VKey.VK_LSHIFT);
        if ((modifiers & (uint)KeyModifiers.LeftWin) != 0)
          Keyboard.KeyUp(Keyboard.VKey.VK_LWIN);

        if ((modifiers & (uint)KeyModifiers.RightAlt) != 0)
          Keyboard.KeyUp(Keyboard.VKey.VK_RMENU);
        if ((modifiers & (uint)KeyModifiers.RightControl) != 0)
          Keyboard.KeyUp(Keyboard.VKey.VK_RCONTROL);
        if ((modifiers & (uint)KeyModifiers.RightShift) != 0)
          Keyboard.KeyUp(Keyboard.VKey.VK_RSHIFT);
        if ((modifiers & (uint)KeyModifiers.RightWin) != 0)
          Keyboard.KeyUp(Keyboard.VKey.VK_RWIN);
      }
    }
    static void KeyDown(uint keyCode, uint modifiers)
    {
      if (modifiers != 0)
      {
        if ((modifiers & (uint)KeyModifiers.LeftAlt) != 0)
          Keyboard.KeyDown(Keyboard.VKey.VK_LMENU);
        if ((modifiers & (uint)KeyModifiers.LeftControl) != 0)
          Keyboard.KeyDown(Keyboard.VKey.VK_LCONTROL);
        if ((modifiers & (uint)KeyModifiers.LeftShift) != 0)
          Keyboard.KeyDown(Keyboard.VKey.VK_LSHIFT);
        if ((modifiers & (uint)KeyModifiers.LeftWin) != 0)
          Keyboard.KeyDown(Keyboard.VKey.VK_LWIN);

        if ((modifiers & (uint)KeyModifiers.RightAlt) != 0)
          Keyboard.KeyDown(Keyboard.VKey.VK_RMENU);
        if ((modifiers & (uint)KeyModifiers.RightControl) != 0)
          Keyboard.KeyDown(Keyboard.VKey.VK_RCONTROL);
        if ((modifiers & (uint)KeyModifiers.RightShift) != 0)
          Keyboard.KeyDown(Keyboard.VKey.VK_RSHIFT);
        if ((modifiers & (uint)KeyModifiers.RightWin) != 0)
          Keyboard.KeyDown(Keyboard.VKey.VK_RWIN);
      }

      if (keyCode != 0)
      {
        Keyboard.VKey vKey = ConvertMceKeyCodeToVKey(keyCode);
        Keyboard.KeyDown(vKey);
      }
    }

    void KeyUpRemote(uint keyCode, uint modifiers)
    {
      if (_keyboardHandler == null)
        return;

      if (keyCode != 0)
      {
        Keyboard.VKey vKey = ConvertMceKeyCodeToVKey(keyCode);
        _keyboardHandler((int)vKey, true);
      }

      if (modifiers != 0)
      {
        if ((modifiers & (uint)KeyModifiers.LeftAlt) != 0)
          _keyboardHandler((int)Keyboard.VKey.VK_LMENU, true);
        if ((modifiers & (uint)KeyModifiers.LeftControl) != 0)
          _keyboardHandler((int)Keyboard.VKey.VK_LCONTROL, true);
        if ((modifiers & (uint)KeyModifiers.LeftShift) != 0)
          _keyboardHandler((int)Keyboard.VKey.VK_LSHIFT, true);
        if ((modifiers & (uint)KeyModifiers.LeftWin) != 0)
          _keyboardHandler((int)Keyboard.VKey.VK_LWIN, true);

        if ((modifiers & (uint)KeyModifiers.RightAlt) != 0)
          _keyboardHandler((int)Keyboard.VKey.VK_RMENU, true);
        if ((modifiers & (uint)KeyModifiers.RightControl) != 0)
          _keyboardHandler((int)Keyboard.VKey.VK_RCONTROL, true);
        if ((modifiers & (uint)KeyModifiers.RightShift) != 0)
          _keyboardHandler((int)Keyboard.VKey.VK_RSHIFT, true);
        if ((modifiers & (uint)KeyModifiers.RightWin) != 0)
          _keyboardHandler((int)Keyboard.VKey.VK_RWIN, true);
      }
    }
    void KeyDownRemote(uint keyCode, uint modifiers)
    {
      if (_keyboardHandler == null)
        return;

      if (modifiers != 0)
      {
        if ((modifiers & (uint)KeyModifiers.LeftAlt) != 0)
          _keyboardHandler((int)Keyboard.VKey.VK_LMENU, false);
        if ((modifiers & (uint)KeyModifiers.LeftControl) != 0)
          _keyboardHandler((int)Keyboard.VKey.VK_LCONTROL, false);
        if ((modifiers & (uint)KeyModifiers.LeftShift) != 0)
          _keyboardHandler((int)Keyboard.VKey.VK_LSHIFT, false);
        if ((modifiers & (uint)KeyModifiers.LeftWin) != 0)
          _keyboardHandler((int)Keyboard.VKey.VK_LWIN, false);

        if ((modifiers & (uint)KeyModifiers.RightAlt) != 0)
          _keyboardHandler((int)Keyboard.VKey.VK_RMENU, false);
        if ((modifiers & (uint)KeyModifiers.RightControl) != 0)
          _keyboardHandler((int)Keyboard.VKey.VK_RCONTROL, false);
        if ((modifiers & (uint)KeyModifiers.RightShift) != 0)
          _keyboardHandler((int)Keyboard.VKey.VK_RSHIFT, false);
        if ((modifiers & (uint)KeyModifiers.RightWin) != 0)
          _keyboardHandler((int)Keyboard.VKey.VK_RWIN, false);
      }

      if (keyCode != 0)
      {
        Keyboard.VKey vKey = ConvertMceKeyCodeToVKey(keyCode);
        _keyboardHandler((int)vKey, false);
      }
    }

    /// <summary>
    /// Reads in an IR file (either Native or Pronto).
    /// </summary>
    /// <param name="fileName">IR file to load.</param>
    /// <returns>MCE IR Code.</returns>
    static MceIrCode ReadIRFile(string fileName)
    {
      try
      {
        FileStream irFile = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

        int length = (int)irFile.Length;

        byte[] fileData = new byte[length];

        irFile.Read(fileData, 0, length);

        irFile.Close();

        if (Pronto.IsProntoData(fileData))
        {
          ushort[] prontoData = Pronto.ConvertFileBytesToProntoData(fileData);

          return Pronto.ConvertProntoDataToMceIrCode(prontoData);
        }
        else
        {
          return new MceIrCode(fileData);
        }
      }
      catch
      {
        return null;
      }
    }

    /// <summary>
    /// Writes an IR file with the supplied data.
    /// </summary>
    /// <param name="fileName">Path to file.</param>
    /// <param name="MceIrCode">IR code data.</param>
    /// <returns>Success.</returns>
    static bool WriteIRFile(string fileName, MceIrCode code)
    {
      try
      {
        FileStream file = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read);

        foreach (byte dataByte in code.Data)
          file.WriteByte(dataByte);

        file.Flush();
        file.Close();
      }
      catch
      {
        return false;
      }

      return true;
    }

    /// <summary>
    /// Send an IR code.
    /// </summary>
    /// <param name="code">MCE IR code data to send.</param>
    /// <param name="port">IR port to send to.</param>
    /// <param name="type">Transceiver type.</param>
    /// <returns>Success.</returns>
    bool Send(MceIrCode code, BlasterPort port, BlasterType type)
    {
      if (_writeStream == null)
        return false;

      byte[][] portPackets;
      switch (type)
      {
        case BlasterType.Microsoft:   portPackets = MicrosoftPorts;   break;
        case BlasterType.SMK:         portPackets = SmkPorts;         break;
        default:                      return false;
      }

      // Set port
      _writeStream.Write(portPackets[(int)port], 0, portPackets[(int)port].Length);

      // Set carrier frequency
      byte[] carrierPacket = code.GetCarrierPacket();
      _writeStream.Write(carrierPacket, 0, carrierPacket.Length);

      // Send packet
      byte[] dataPacket = code.GetDataPacket();
      _writeStream.Write(dataPacket, 0, dataPacket.Length);

      // Flush the data stream
      _writeStream.Flush();

      // Wait to ensure the packet had time to be transmitted
      int packetTime = code.GetPacketTransmitTime() / 1000;
      Thread.Sleep(packetTime);

      return true;
    }

    #endregion Implementation

  }

}
