using System;
using System.ComponentModel;
#if TRACE
using System.Diagnostics;
#endif
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Win32.SafeHandles;

namespace InputService.Plugin
{

  /// <summary>
  /// IR Server plugin to support HID USB devices.
  /// </summary>
  public class CustomHIDReceiver : PluginBase, IConfigure, IRemoteReceiver, IKeyboardReceiver, IMouseReceiver
  {

    #region Debug

    static void Remote(string deviceName, string code)
    {
      Console.WriteLine("Remote: {0}", code);
    }
    static void Keyboard(string deviceName, int button, bool up)
    {
      Console.WriteLine("Keyboard: {0}, {1}", button, up);
    }
    static void Mouse(string deviceName, int x, int y, int buttons)
    {
      Console.WriteLine("Mouse: ({0}, {1}) - {2}", x, y, buttons);
    }

    [STAThread]
    static void Main()
    {
      CustomHIDReceiver c = new CustomHIDReceiver();

      c.Configure(null);

      c.RemoteCallback += new RemoteHandler(Remote);
      c.KeyboardCallback += new KeyboardHandler(Keyboard);
      c.MouseCallback += new MouseHandler(Mouse);      

      Console.WriteLine("Usage: {0}", c._device.usUsage);
      Console.WriteLine("UsagePage: {0}", c._device.usUsagePage);
      Console.WriteLine();

      c.Start();

      Application.Run();

      c.Stop();
      c = null;
    }

    #endregion Debug

    #region Constants

    static readonly string ConfigurationFile = Path.Combine(ConfigurationPath, "Custom HID Receiver.xml");

    const int DeviceBufferSize = 255;

    #endregion Constants

    #region Variables

    #region Configuration

    RawInput.RAWINPUTDEVICE _device;

    int _inputByte;
    byte _byteMask;
    bool _useAllBytes;

    int _repeatDelay;

    #endregion Configuration

    ReceiverWindow _receiverWindow;

    RemoteHandler _remoteHandler;
    KeyboardHandler _keyboardHandler;
    MouseHandler _mouseHandler;

    string _lastKeyCode = String.Empty;
    DateTime _lastCodeTime = DateTime.Now;

    #endregion Variables

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name         { get { return "Custom HID"; } }
    /// <summary>
    /// IR Server plugin version.
    /// </summary>
    /// <value>The version.</value>
    public override string Version      { get { return "1.0.4.2"; } }
    /// <summary>
    /// The IR Server plugin's author.
    /// </summary>
    /// <value>The author.</value>
    public override string Author       { get { return "and-81"; } }
    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    /// <value>The description.</value>
    public override string Description  { get { return "Supports HID USB devices"; } }
    /// <summary>
    /// Gets a display icon for the plugin.
    /// </summary>
    /// <value>The icon.</value>
    public override Icon DeviceIcon     { get { return Properties.Resources.Icon; } }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    public override void Start()
    {
      LoadSettings();

      _receiverWindow = new ReceiverWindow("Custom HID Receiver");
      _receiverWindow.ProcMsg += new ProcessMessage(ProcMessage);

      _device.dwFlags = RawInput.RawInputDeviceFlags.InputSink;      
      _device.hwndTarget = _receiverWindow.Handle;

      if (!RegisterForRawInput(_device))
        throw new ApplicationException("Failed to register for HID Raw input");
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
      _device.dwFlags |= RawInput.RawInputDeviceFlags.Remove;
      RegisterForRawInput(_device);

      _receiverWindow.ProcMsg -= new ProcessMessage(ProcMessage);
      _receiverWindow.DestroyHandle();
      _receiverWindow = null;
    }

    /// <summary>
    /// Configure the IR Server plugin.
    /// </summary>
    public void Configure(IWin32Window owner)
    {
      LoadSettings();

      DeviceSelect deviceSelect   = new DeviceSelect();
      deviceSelect.SelectedDevice = _device;
      deviceSelect.InputByte      = _inputByte;
      deviceSelect.ByteMask       = _byteMask;
      deviceSelect.UseAllBytes    = _useAllBytes;
      deviceSelect.RepeatDelay    = _repeatDelay;

      if (deviceSelect.ShowDialog(owner) == DialogResult.OK)
      {
        _device       = deviceSelect.SelectedDevice;
        _inputByte    = deviceSelect.InputByte;
        _byteMask     = deviceSelect.ByteMask;
        _useAllBytes  = deviceSelect.UseAllBytes;
        _repeatDelay  = deviceSelect.RepeatDelay;

        SaveSettings();
      }
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

    /// <summary>
    /// Loads the settings.
    /// </summary>
    void LoadSettings()
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationFile);

        _device.usUsage = ushort.Parse(doc.DocumentElement.Attributes["DeviceUsage"].Value);
        _device.usUsagePage = ushort.Parse(doc.DocumentElement.Attributes["DeviceUsagePage"].Value);

        _inputByte    = int.Parse(doc.DocumentElement.Attributes["InputByte"].Value);
        _byteMask     = byte.Parse(doc.DocumentElement.Attributes["ByteMask"].Value);
        _useAllBytes  = bool.Parse(doc.DocumentElement.Attributes["UseAllBytes"].Value);
        _repeatDelay  = int.Parse(doc.DocumentElement.Attributes["RepeatDelay"].Value);
      }
#if TRACE
      catch (Exception ex)
      {
        Trace.WriteLine(ex.ToString());
#else
      catch
      {
#endif

        _device.usUsage     = 0x00;
        _device.usUsagePage = 0x00;        
        
        _inputByte    = 3;
        _byteMask     = 0x7F;
        _useAllBytes  = true;
        _repeatDelay  = 250;
      }
    }
    /// <summary>
    /// Saves the settings.
    /// </summary>
    void SaveSettings()
    {
      try
      {
        XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, Encoding.UTF8);
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 1;
        writer.IndentChar = (char)9;
        writer.WriteStartDocument(true);
        writer.WriteStartElement("settings"); // <settings>

        writer.WriteAttributeString("DeviceUsage", _device.usUsage.ToString());
        writer.WriteAttributeString("DeviceUsagePage", _device.usUsagePage.ToString());

        writer.WriteAttributeString("InputByte", _inputByte.ToString());
        writer.WriteAttributeString("ByteMask", _byteMask.ToString());
        writer.WriteAttributeString("UseAllBytes", _useAllBytes.ToString());
        writer.WriteAttributeString("RepeatDelay", _repeatDelay.ToString());

        writer.WriteEndElement(); // </settings>
        writer.WriteEndDocument();
        writer.Close();
      }
#if TRACE
      catch (Exception ex)
      {
        Trace.WriteLine(ex.ToString());
      }
#else
      catch
      {
      }
#endif
    }

    bool RegisterForRawInput(RawInput.RAWINPUTDEVICE device)
    {
      RawInput.RAWINPUTDEVICE[] devices = new RawInput.RAWINPUTDEVICE[1];
      devices[0] = device;

      return RegisterForRawInput(devices);
    }
    bool RegisterForRawInput(RawInput.RAWINPUTDEVICE[] devices)
    {
      return RawInput.RegisterRawInputDevices(devices, (uint)devices.Length, (uint)Marshal.SizeOf(devices[0]));
    }

    void ProcMessage(ref Message m)
    {
      switch (m.Msg)
      {
        case RawInput.WM_INPUT:
          ProcessInputCommand(ref m);
          break;

        case RawInput.WM_KEYDOWN:
          ProcessKeyDown(m.WParam.ToInt32());
          break;

        case RawInput.WM_KEYUP:
          ProcessKeyUp(m.WParam.ToInt32());
          break;

        case RawInput.WM_APPCOMMAND:
          ProcessAppCommand(m.LParam.ToInt32());
          break;
      }
    }
    
    void ProcessKeyDown(int param)
    {
#if TRACE
      Trace.WriteLine(String.Format("KeyDown - Param: {0}", param));
#endif

      if (_keyboardHandler != null)
        _keyboardHandler(this.Name, param, false);
    }

    void ProcessKeyUp(int param)
    {
#if TRACE
      Trace.WriteLine(String.Format("KeyUp - Param: {0}", param));
#endif

      if (_keyboardHandler != null)
        _keyboardHandler(this.Name, param, true);
    }

    void ProcessAppCommand(int param)
    {
#if TRACE
      Trace.WriteLine(String.Format("AppCommand - Param: {0}", param));
#endif
    }

    void ProcessInputCommand(ref Message message)
    {
      uint dwSize = 0;

      RawInput.GetRawInputData(message.LParam, RawInput.RawInputCommand.Input, IntPtr.Zero, ref dwSize, (uint)Marshal.SizeOf(typeof(RawInput.RAWINPUTHEADER)));

      IntPtr buffer = Marshal.AllocHGlobal((int)dwSize);
      try
      {
        if (buffer == IntPtr.Zero)
          return;

        if (RawInput.GetRawInputData(message.LParam, RawInput.RawInputCommand.Input, buffer, ref dwSize, (uint)Marshal.SizeOf(typeof(RawInput.RAWINPUTHEADER))) != dwSize)
          return;

        RawInput.RAWINPUT raw = (RawInput.RAWINPUT)Marshal.PtrToStructure(buffer, typeof(RawInput.RAWINPUT));

        switch (raw.header.dwType)
        {
          case RawInput.RawInputType.HID:
            {
              int offset = Marshal.SizeOf(typeof(RawInput.RAWINPUTHEADER)) + Marshal.SizeOf(typeof(RawInput.RAWHID));

              byte[] bRawData = new byte[offset + raw.hid.dwSizeHid];
              Marshal.Copy(buffer, bRawData, 0, bRawData.Length);

              byte[] newArray = new byte[raw.hid.dwSizeHid];
              Array.Copy(bRawData, offset, newArray, 0, newArray.Length);

              string code = BitConverter.ToString(newArray);

              if (!_useAllBytes)
              {
                int val = newArray[_inputByte] & _byteMask;
                code = String.Format("{0:X2}", val);
              }

#if TRACE
              Trace.WriteLine(code);
#endif

              if (_remoteHandler != null)
                _remoteHandler(this.Name, code);

              break;
            }

          case RawInput.RawInputType.Mouse:
            {
#if TRACE
              Trace.WriteLine(String.Format("Mouse Event"));
              Trace.WriteLine(String.Format("Buttons: {0}", raw.mouse.ulButtons));
              Trace.WriteLine(String.Format("Raw Buttons: {0}", raw.mouse.ulRawButtons));
              Trace.WriteLine(String.Format("Flags: {0}", raw.mouse.usFlags));
              Trace.WriteLine(String.Format("Extra: {0}", raw.mouse.ulExtraInformation));
              Trace.WriteLine(String.Format("Button Data: {0}", raw.mouse.buttonsStr.usButtonData));
              Trace.WriteLine(String.Format("Button Flags: {0}", raw.mouse.buttonsStr.usButtonFlags));
              Trace.WriteLine(String.Format("Last X: {0}", raw.mouse.lLastX));
              Trace.WriteLine(String.Format("Last Y: {0}", raw.mouse.lLastY));
#endif
              if (_mouseHandler != null)
                _mouseHandler(this.Name, raw.mouse.lLastX, raw.mouse.lLastY, (int)raw.mouse.ulButtons);

              break;
            }

          case RawInput.RawInputType.Keyboard:
            {
#if TRACE
              Trace.WriteLine("Keyboard Event");
#endif
              
              switch (raw.keyboard.Flags)
              {
                case RawInput.RawKeyboardFlags.KeyBreak:
#if TRACE
                  Trace.WriteLine(String.Format("Break: {0}", raw.keyboard.VKey));
#endif

                  if (_keyboardHandler != null)
                    _keyboardHandler(this.Name, raw.keyboard.VKey, true);

                  break;

                case RawInput.RawKeyboardFlags.KeyE0:
#if TRACE
                  Trace.WriteLine(String.Format("E0: {0}", raw.keyboard.MakeCode));
#endif
                  //if (_keyboardHandler != null)
                  //  _keyboardHandler(this.Name, 0xE000 | raw.keyboard.MakeCode, true);

                  break;

                case RawInput.RawKeyboardFlags.KeyE1:
#if TRACE
                  Trace.WriteLine("E1");
#endif
                  //if (_keyboardHandler != null)
                  //  _keyboardHandler(this.Name, 0xE100, true);

                  break;

                case RawInput.RawKeyboardFlags.KeyMake:
#if TRACE
                  Trace.WriteLine(String.Format("Make: {0}", raw.keyboard.VKey));
#endif

                  if (_keyboardHandler != null)
                    _keyboardHandler(this.Name, raw.keyboard.VKey, false);

                  break;

                case RawInput.RawKeyboardFlags.TerminalServerSetLED:
#if TRACE
                  Trace.WriteLine("TerminalServerSetLED");
#endif
                  break;

                case RawInput.RawKeyboardFlags.TerminalServerShadow:
#if TRACE
                  Trace.WriteLine("TerminalServerShadow");
#endif
                  break;
              }

              break;
            }
        }
      }
      finally
      {
        Marshal.FreeHGlobal(buffer);
      }

    }

  }

}
