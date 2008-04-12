using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace InputService.Plugin
{

  #region Enumerations

  /// <summary>
  /// The blaster port to send IR Commands to.
  /// </summary>
  public enum BlasterPort
  {
    /// <summary>
    /// Send IR Commands to both blaster ports.
    /// </summary>
    Both = 0,
    /// <summary>
    /// Send IR Commands to blaster port 1 only.
    /// </summary>
    Port_1 = 1,
    /// <summary>
    /// Send IR Commands to blaster port 2 only.
    /// </summary>
    Port_2 = 2
  }

  #endregion Enumerations

  #region Delegates

  delegate void DeviceEventHandler();

  #endregion Delegates

  /// <summary>
  /// IR Server Plugin for Microsoft MCE Transceiver device.
  /// </summary>
  public class MicrosoftMceTransceiver : PluginBase, IConfigure, ITransmitIR, ILearnIR, IRemoteReceiver, IKeyboardReceiver, IMouseReceiver
  {

    // #define TEST_APPLICATION in the project properties when creating the console test app ...
#if TEST_APPLICATION

    static MicrosoftMceTransceiver device;

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

    static void Dump(int[] timingData)
    {
      foreach (int time in timingData)
        Console.Write("{0}, ", time);
      Console.WriteLine();
    }

    [STAThread]
    static void Main()
    {
      Console.WriteLine("Microsoft MCE Transceiver Test App");
      Console.WriteLine("====================================");
      Console.WriteLine();

      SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);

      try
      {
        device = new MicrosoftMceTransceiver();

        Console.Write("Configure device? (y/n) ");

        if (Console.ReadKey().Key == ConsoleKey.Y)
        {
          Console.WriteLine();

          Console.WriteLine("Configuring ...");
          device.Configure(null);
        }
        else
        {
          Console.WriteLine();
        }

        device.RemoteCallback += new RemoteHandler(xRemote);
        device.KeyboardCallback += new KeyboardHandler(xKeyboard);
        device.MouseCallback += new MouseHandler(xMouse);

        Console.WriteLine("Starting device access ...");

        device.Start();

        Console.Write("Learn IR? (y/n) ");

        while (Console.ReadKey().Key == ConsoleKey.Y)
        {
          Console.WriteLine();
          Console.WriteLine("Learning IR Command ...");

          byte[] data;

          switch (device.Learn(out data))
          {
            case LearnStatus.Failure:
              Console.WriteLine("Learn process failed!");
              break;

            case LearnStatus.Success:
              Console.WriteLine("Learn successful");

              Console.Write("Blast IR back? (y/n) ");

              if (Console.ReadKey().Key == ConsoleKey.Y)
              {
                Console.WriteLine();
                Console.WriteLine("Blasting ...");

                if (device.Transmit("Both", data))
                {
                  Console.WriteLine("Blasting successful");
                }
                else
                {
                  Console.WriteLine("Blasting failure!");
                }
              }
              else
              {
                Console.WriteLine();
              }
              break;

            case LearnStatus.Timeout:
              Console.WriteLine("Learn process timed-out");
              break;
          }

          Console.Write("Learn another IR? (y/n) ");
        }
        Console.WriteLine();
        Console.WriteLine();

        Console.WriteLine("Press a button on your remote ...");

        Application.Run();

        device.Stop();
      }
      catch (Exception ex)
      {
        Console.WriteLine("Error:");
        Console.WriteLine(ex.ToString());
        Console.WriteLine();
        Console.WriteLine("");

        Console.ReadKey();
      }
      finally
      {
        device = null;
      }

      SystemEvents.PowerModeChanged -= new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);
    }

    static void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
    {
      Console.WriteLine("Power Event: {0}", Enum.GetName(typeof(PowerModes), e.Mode));

      switch (e.Mode)
      {

        case PowerModes.Suspend:
          if (device != null)
            device.Suspend();
          break;

        case PowerModes.Resume:
          if (device != null)
            device.Resume();
          break;

      }
    }

#endif


    #region Constants

    static readonly string ConfigurationFile = Path.Combine(ConfigurationPath, "Microsoft MCE Transceiver.xml");
    
    static readonly Guid MicrosoftGuid   = new Guid(0x7951772d, 0xcd50, 0x49b7, 0xb1, 0x03, 0x2b, 0xaa, 0xc4, 0x94, 0xfc, 0x57);
    static readonly Guid ReplacementGuid = new Guid(0x00873fdf, 0x61a8, 0x11d1, 0xaa, 0x5e, 0x00, 0xc0, 0x4f, 0xb1, 0x72, 0x8b);

    const int VistaVersionNumber  = 6;

    const string AutomaticButtonsRegKey = @"SYSTEM\CurrentControlSet\Services\HidIr\Remotes\745a17a0-74d3-11d0-b6fe-00a0c90f57da";

    #endregion Constants

    #region Variables

    #region Configuration

    int _learnTimeout             = 10000;
    bool _disableMceServices      = true;

    bool _enableRemoteInput       = true;
    bool _useSystemRatesRemote    = false;
    int _remoteFirstRepeat        = 400;
    int _remoteHeldRepeats        = 250;
    bool _disableAutomaticButtons = false;

    bool _enableKeyboardInput     = false;
    bool _useSystemRatesKeyboard  = true;
    int _keyboardFirstRepeat      = 350;
    int _keyboardHeldRepeats      = 0;
    bool _handleKeyboardLocally   = true;

    bool _enableMouseInput        = false;
    bool _handleMouseLocally      = true;
    double _mouseSensitivity      = 1.0d;

    #endregion Configuration

    Driver _driver;

    IrProtocol _lastRemoteButtonCodeType  = IrProtocol.None;
    uint _lastRemoteButtonKeyCode         = 0;
    DateTime _lastRemoteButtonTime        = DateTime.Now;
    bool _remoteButtonRepeated            = false;
    
    bool _keyboardKeyRepeated             = false;
    DateTime _lastKeyboardKeyTime         = DateTime.Now;

    uint _lastKeyboardKeyCode             = 0;
    uint _lastKeyboardModifiers           = 0;

    Mouse.MouseEvents _mouseButtons       = Mouse.MouseEvents.None;

    bool _ignoreAutomaticButtons          = false;

    RemoteHandler _remoteHandler;
    KeyboardHandler _keyboardHandler;
    MouseHandler _mouseHandler;

    #endregion Variables

    #region Implementation

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    public override string Name         { get { return "Microsoft MCE"; } }
    /// <summary>
    /// IR Server plugin version.
    /// </summary>
    public override string Version      { get { return "1.4.2.0"; } }
    /// <summary>
    /// The IR Server plugin's author.
    /// </summary>
    public override string Author       { get { return "and-81, inspired by Bruno Fleurette"; } }
    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    public override string Description  { get { return "Microsoft MCE Infrared Transceiver"; } }
    /// <summary>
    /// Gets a display icon for the plugin.
    /// </summary>
    /// <value>The icon.</value>
    public override Icon DeviceIcon     { get { return Properties.Resources.Icon; } }

    /// <summary>
    /// Detect the presence of this device.  Devices that cannot be detected will always return false.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the device is present, otherwise <c>false</c>.
    /// </returns>
    public override bool Detect()
    {
      try
      {
        Guid deviceGuid;
        string devicePath;

        return FindDevice(out deviceGuid, out devicePath);
      }
      catch
      {
        return false;
      }
    }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public override void Start()
    {
#if TRACE
      Trace.WriteLine("Start MicrosoftMceTransceiver");
#endif

      if (_driver != null)
        throw new InvalidOperationException("MicrosoftMceTransceiver already started");

      LoadSettings();

      // Put this in a try...catch so that if the registry keys don't exist we don't throw an ugly exception.
      try
      {
        _ignoreAutomaticButtons = CheckAutomaticButtons();
      }
      catch
      {
        _ignoreAutomaticButtons = false;
      }

      if (_disableMceServices)
        DisableMceServices();

      Guid deviceGuid;
      string devicePath;

      Driver newDriver = null;

      if (FindDevice(out deviceGuid, out devicePath))
      {
        if (deviceGuid == MicrosoftGuid)
        {
          if (Environment.OSVersion.Version.Major >= VistaVersionNumber)
            newDriver = new DriverVista(deviceGuid, devicePath, new RemoteCallback(RemoteEvent), new KeyboardCallback(KeyboardEvent), new MouseCallback(MouseEvent));
          else
            newDriver = new DriverXP(deviceGuid, devicePath, new RemoteCallback(RemoteEvent), new KeyboardCallback(KeyboardEvent), new MouseCallback(MouseEvent));
        }
        else
        {
          newDriver = new DriverReplacement(deviceGuid, devicePath, new RemoteCallback(RemoteEvent), new KeyboardCallback(KeyboardEvent), new MouseCallback(MouseEvent));
        }
      }
      else
      {
        throw new InvalidOperationException("Device not found");
      }

      newDriver.Start();

      _driver = newDriver;
    }
    /// <summary>
    /// Suspend the IR Server plugin when computer enters standby.
    /// </summary>
    public override void Suspend()
    {
#if TRACE
      Trace.WriteLine("Suspend MicrosoftMceTransceiver");
#endif

      if (_driver != null)
        _driver.Suspend();
    }
    /// <summary>
    /// Resume the IR Server plugin when the computer returns from standby.
    /// </summary>
    public override void Resume()
    {
#if TRACE
      Trace.WriteLine("Resume MicrosoftMceTransceiver");
#endif

      if (_driver != null)
        _driver.Resume();
    }
    /// <summary>
    /// Stop the IR Server plugin.
    /// </summary>
    public override void Stop()
    {
#if TRACE
      Trace.WriteLine("Stop MicrosoftMceTransceiver");
#endif

      if (_driver != null)
      {
        try
        {
          _driver.Stop();
        }
        finally
        {
          _driver = null;
        }
      }
    }

    /// <summary>
    /// Configure the IR Server plugin.
    /// </summary>
    public void Configure(IWin32Window owner)
    {
      LoadSettings();

      Configure config = new Configure();

      config.LearnTimeout         = _learnTimeout;
      config.DisableMceServices   = _disableMceServices;

      config.EnableRemote         = _enableRemoteInput;
      config.UseSystemRatesForRemote = _useSystemRatesRemote;
      config.RemoteRepeatDelay    = _remoteFirstRepeat;
      config.RemoteHeldDelay      = _remoteHeldRepeats;
      config.DisableAutomaticButtons = _disableAutomaticButtons;

      config.EnableKeyboard       = _enableKeyboardInput;
      config.UseSystemRatesForKeyboard = _useSystemRatesKeyboard;
      config.KeyboardRepeatDelay  = _keyboardFirstRepeat;
      config.KeyboardHeldDelay    = _keyboardHeldRepeats;
      config.HandleKeyboardLocal  = _handleKeyboardLocally;

      config.EnableMouse          = _enableMouseInput;
      config.HandleMouseLocal     = _handleMouseLocally;
      config.MouseSensitivity     = _mouseSensitivity;

      if (config.ShowDialog(owner) == DialogResult.OK)
      {
        _learnTimeout           = config.LearnTimeout;
        _disableMceServices     = config.DisableMceServices;

        _enableRemoteInput      = config.EnableRemote;
        _useSystemRatesRemote   = config.UseSystemRatesForRemote;
        _remoteFirstRepeat      = config.RemoteRepeatDelay;
        _remoteHeldRepeats      = config.RemoteHeldDelay;
        _disableAutomaticButtons = config.DisableAutomaticButtons;

        _enableKeyboardInput    = config.EnableKeyboard;
        _useSystemRatesKeyboard = config.UseSystemRatesForKeyboard;
        _keyboardFirstRepeat    = config.KeyboardRepeatDelay;
        _keyboardHeldRepeats    = config.KeyboardHeldDelay;
        _handleKeyboardLocally  = config.HandleKeyboardLocal;

        _enableMouseInput       = config.EnableMouse;
        _handleMouseLocally     = config.HandleMouseLocal;
        _mouseSensitivity       = config.MouseSensitivity;

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
    /// Lists the available blaster ports.
    /// </summary>
    public string[] AvailablePorts { get { return Enum.GetNames(typeof(BlasterPort)); } }

    /// <summary>
    /// Transmit an infrared command.
    /// </summary>
    /// <param name="port">Port to transmit on.</param>
    /// <param name="data">Data to transmit.</param>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public bool Transmit(string port, byte[] data)
    {
      BlasterPort blasterPort = BlasterPort.Both;
      try
      {
        blasterPort = (BlasterPort)Enum.Parse(typeof(BlasterPort), port, true);
      }
      catch
      {
#if TRACE
        Trace.WriteLine(String.Format("Invalid Blaster Port ({0}), using default ({1})", port, blasterPort));
#endif
      }

      IrCode code = IrCode.FromByteArray(data);

      if (code == null)
        throw new ArgumentException("Invalid IR Command data", "data");

      _driver.Send(code, (int)blasterPort);

      return true;
    }
    /// <summary>
    /// Learn an infrared command.
    /// </summary>
    /// <param name="data">New infrared command.</param>
    /// <returns>
    /// Tells the calling code if the learn was Successful, Failed or Timed Out.
    /// </returns>
    public LearnStatus Learn(out byte[] data)
    {
      IrCode code;
      
      LearnStatus status = _driver.Learn(_learnTimeout, out code);

      if (code != null)
        data = code.ToByteArray();
      else
        data = null;

      return status;
    }

    void LoadSettings()
    {
      XmlDocument doc = new XmlDocument();

      try   { doc.Load(ConfigurationFile); }
      catch { return; }

      try   { _learnTimeout = int.Parse(doc.DocumentElement.Attributes["LearnTimeout"].Value); } catch {}
      try   { _disableMceServices = bool.Parse(doc.DocumentElement.Attributes["DisableMceServices"].Value); } catch {}

      try   { _enableRemoteInput = bool.Parse(doc.DocumentElement.Attributes["EnableRemoteInput"].Value); } catch {}
      try   { _useSystemRatesRemote = bool.Parse(doc.DocumentElement.Attributes["UseSystemRatesRemote"].Value); } catch { }
      try   { _remoteFirstRepeat = int.Parse(doc.DocumentElement.Attributes["RemoteFirstRepeat"].Value); } catch {}
      try   { _remoteHeldRepeats = int.Parse(doc.DocumentElement.Attributes["RemoteHeldRepeats"].Value); } catch {}
      try   { _disableAutomaticButtons = bool.Parse(doc.DocumentElement.Attributes["DisableAutomaticButtons"].Value); } catch {}

      try   { _enableKeyboardInput = bool.Parse(doc.DocumentElement.Attributes["EnableKeyboardInput"].Value); } catch {}
      try   { _useSystemRatesKeyboard = bool.Parse(doc.DocumentElement.Attributes["UseSystemRatesKeyboard"].Value); } catch { }
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
        using (XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, Encoding.UTF8))
        {
          writer.Formatting = Formatting.Indented;
          writer.Indentation = 1;
          writer.IndentChar = (char)9;
          writer.WriteStartDocument(true);
          writer.WriteStartElement("settings"); // <settings>

          writer.WriteAttributeString("LearnTimeout", _learnTimeout.ToString());
          writer.WriteAttributeString("DisableMceServices", _disableMceServices.ToString());

          writer.WriteAttributeString("EnableRemoteInput", _enableRemoteInput.ToString());
          writer.WriteAttributeString("UseSystemRatesRemote", _useSystemRatesRemote.ToString());
          writer.WriteAttributeString("RemoteFirstRepeat", _remoteFirstRepeat.ToString());
          writer.WriteAttributeString("RemoteHeldRepeats", _remoteHeldRepeats.ToString());
          writer.WriteAttributeString("DisableAutomaticButtons", _disableAutomaticButtons.ToString());

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

    void RemoteEvent(IrProtocol codeType, uint keyCode, bool firstPress)
    {
#if TRACE
      Trace.WriteLine(String.Format("Remote: {0}, {1}, {2}", Enum.GetName(typeof(IrProtocol), codeType), keyCode, firstPress));
#endif

      if (!_enableRemoteInput)
        return;

      if (_ignoreAutomaticButtons && codeType == IrProtocol.RC6_MCE)
      {
        // Always ignore these buttones ...
        if (
          //keyCode == 0x7bdc ||  // Back (removed 12-April-2008)
            keyCode == 0x7bdd ||  // OK
            keyCode == 0x7bde ||  // Right
            keyCode == 0x7bdf ||  // Left
            keyCode == 0x7be0 ||  // Down
            keyCode == 0x7be1 ||  // Up
            keyCode == 0x7bee ||  // Volume_Down
            keyCode == 0x7bef ||  // Volume_Up
            keyCode == 0x7bf1 ||  // Mute
            keyCode == 0x7bf3 ||  // PC_Power
            keyCode == 0x7bf4 ||  // Enter
            keyCode == 0x7bf6 ||  // Number_9
            keyCode == 0x7bf7 ||  // Number_8
            keyCode == 0x7bf8 ||  // Number_7
            keyCode == 0x7bf9 ||  // Number_6
            keyCode == 0x7bfa ||  // Number_5
            keyCode == 0x7bfb ||  // Number_4
            keyCode == 0x7bfc ||  // Number_3
            keyCode == 0x7bfd ||  // Number_2
            keyCode == 0x7bfe ||  // Number_1
            keyCode == 0x7bff)    // Number_0
        {
#if TRACE
          Trace.WriteLine("Ignoring remote button due to automatic handling");
#endif
          return;
        }

        // Only ignore Start if the services aren't disabled ...
        if (keyCode == 0x7bf2 && !_disableMceServices)
        {
#if TRACE
          Trace.WriteLine("Ignoring Start button due to automatic handling and the MCE services being active");
#endif
          return;
        }
      }

      if (!firstPress && _lastRemoteButtonCodeType == codeType && _lastRemoteButtonKeyCode == keyCode)
      {
        TimeSpan timeBetween = DateTime.Now.Subtract(_lastRemoteButtonTime);

        int firstRepeat = _remoteFirstRepeat;
        int heldRepeats = _remoteHeldRepeats;
        if (_useSystemRatesRemote)
        {
          firstRepeat = 250 + (SystemInformation.KeyboardDelay * 250);
          heldRepeats = (int)(1000.0 / (2.5 + (SystemInformation.KeyboardSpeed * 0.888)));
        }

        if (!_remoteButtonRepeated && timeBetween.TotalMilliseconds < firstRepeat)
        {
#if TRACE
          Trace.WriteLine("Skip, First Repeat");
#endif
          return;
        }

        if (_remoteButtonRepeated && timeBetween.TotalMilliseconds < heldRepeats)
        {
#if TRACE
          Trace.WriteLine("Skip, Held Repeat");
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
        _lastRemoteButtonCodeType = codeType;
        _lastRemoteButtonKeyCode = keyCode;
        _remoteButtonRepeated = false;
      }

      _lastRemoteButtonTime = DateTime.Now;

      if (_remoteHandler != null)
        _remoteHandler(this.Name, keyCode.ToString());
    }
    void KeyboardEvent(uint keyCode, uint modifiers)
    {
#if TRACE
      Trace.WriteLine(String.Format("Keyboard: {0}, {1}", keyCode, modifiers));
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
          firstRepeat = 250 + (SystemInformation.KeyboardDelay * 250);
          heldRepeats = (int)(1000.0 / (2.5 + (SystemInformation.KeyboardSpeed * 0.888)));
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
    void MouseEvent(int deltaX, int deltaY, bool right, bool left)
    {
#if TRACE
      Trace.WriteLine(String.Format("Mouse: DX {0}, DY {1}, Right: {2}, Left: {3}", deltaX, deltaY, right, left));
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

      deltaX = (int)((double)deltaX * _mouseSensitivity);
      deltaY = (int)((double)deltaY * _mouseSensitivity);

      if (deltaX != 0 || deltaY != 0)
      {
        if (_handleMouseLocally)
          Mouse.Move(deltaX, deltaY, false);
      }

      #endregion Movement Delta

      if (!_handleMouseLocally)
        _mouseHandler(this.Name, deltaX, deltaY, (int)buttons);
    }

    void KeyUpRemote(uint keyCode, uint modifiers)
    {
      if (_keyboardHandler == null)
        return;

      if (keyCode != 0)
      {
        Keyboard.VKey vKey = ConvertMceKeyCodeToVKey(keyCode);
        _keyboardHandler(this.Name, (int)vKey, true);
      }

      if (modifiers != 0)
      {
        if ((modifiers & (uint)KeyModifiers.LeftAlt) != 0)
          _keyboardHandler(this.Name, (int)Keyboard.VKey.VK_LMENU, true);
        if ((modifiers & (uint)KeyModifiers.LeftControl) != 0)
          _keyboardHandler(this.Name, (int)Keyboard.VKey.VK_LCONTROL, true);
        if ((modifiers & (uint)KeyModifiers.LeftShift) != 0)
          _keyboardHandler(this.Name, (int)Keyboard.VKey.VK_LSHIFT, true);
        if ((modifiers & (uint)KeyModifiers.LeftWin) != 0)
          _keyboardHandler(this.Name, (int)Keyboard.VKey.VK_LWIN, true);

        if ((modifiers & (uint)KeyModifiers.RightAlt) != 0)
          _keyboardHandler(this.Name, (int)Keyboard.VKey.VK_RMENU, true);
        if ((modifiers & (uint)KeyModifiers.RightControl) != 0)
          _keyboardHandler(this.Name, (int)Keyboard.VKey.VK_RCONTROL, true);
        if ((modifiers & (uint)KeyModifiers.RightShift) != 0)
          _keyboardHandler(this.Name, (int)Keyboard.VKey.VK_RSHIFT, true);
        if ((modifiers & (uint)KeyModifiers.RightWin) != 0)
          _keyboardHandler(this.Name, (int)Keyboard.VKey.VK_RWIN, true);
      }
    }
    void KeyDownRemote(uint keyCode, uint modifiers)
    {
      if (_keyboardHandler == null)
        return;

      if (modifiers != 0)
      {
        if ((modifiers & (uint)KeyModifiers.LeftAlt) != 0)
          _keyboardHandler(this.Name, (int)Keyboard.VKey.VK_LMENU, false);
        if ((modifiers & (uint)KeyModifiers.LeftControl) != 0)
          _keyboardHandler(this.Name, (int)Keyboard.VKey.VK_LCONTROL, false);
        if ((modifiers & (uint)KeyModifiers.LeftShift) != 0)
          _keyboardHandler(this.Name, (int)Keyboard.VKey.VK_LSHIFT, false);
        if ((modifiers & (uint)KeyModifiers.LeftWin) != 0)
          _keyboardHandler(this.Name, (int)Keyboard.VKey.VK_LWIN, false);

        if ((modifiers & (uint)KeyModifiers.RightAlt) != 0)
          _keyboardHandler(this.Name, (int)Keyboard.VKey.VK_RMENU, false);
        if ((modifiers & (uint)KeyModifiers.RightControl) != 0)
          _keyboardHandler(this.Name, (int)Keyboard.VKey.VK_RCONTROL, false);
        if ((modifiers & (uint)KeyModifiers.RightShift) != 0)
          _keyboardHandler(this.Name, (int)Keyboard.VKey.VK_RSHIFT, false);
        if ((modifiers & (uint)KeyModifiers.RightWin) != 0)
          _keyboardHandler(this.Name, (int)Keyboard.VKey.VK_RWIN, false);
      }

      if (keyCode != 0)
      {
        Keyboard.VKey vKey = ConvertMceKeyCodeToVKey(keyCode);
        _keyboardHandler(this.Name, (int)vKey, false);
      }
    }

    internal static bool CheckAutomaticButtons()
    {
      using (RegistryKey key = Registry.LocalMachine.OpenSubKey(AutomaticButtonsRegKey, false))
      {
        return (key.GetValue("CodeSetNum0", null) != null);
      }
    }
    internal static void EnableAutomaticButtons()
    {
      using (RegistryKey key = Registry.LocalMachine.OpenSubKey(AutomaticButtonsRegKey, true))
      {
        key.SetValue("CodeSetNum0", 1, RegistryValueKind.DWord);
        key.SetValue("CodeSetNum1", 2, RegistryValueKind.DWord);
        key.SetValue("CodeSetNum2", 3, RegistryValueKind.DWord);
        key.SetValue("CodeSetNum3", 4, RegistryValueKind.DWord);
      }
    }
    internal static void DisableAutomaticButtons()
    {
      using (RegistryKey key = Registry.LocalMachine.OpenSubKey(AutomaticButtonsRegKey, true))
      {
        key.DeleteValue("CodeSetNum0", false);
        key.DeleteValue("CodeSetNum1", false);
        key.DeleteValue("CodeSetNum2", false);
        key.DeleteValue("CodeSetNum3", false);
      }
    }

    static bool FindDevice(out Guid deviceGuid, out string devicePath)
    {
      devicePath = null;

      // Try eHome driver ...
      deviceGuid = MicrosoftGuid;
      try
      {
        devicePath = Driver.Find(deviceGuid);

        if (!String.IsNullOrEmpty(devicePath))
          return true;
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
      // Try Replacement driver ...
      deviceGuid = ReplacementGuid;
      try
      {
        devicePath = Driver.Find(deviceGuid);

        if (!String.IsNullOrEmpty(devicePath))
          return true;
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
      return false;
    }

    static void DisableMceServices()
    {
      // Vista ...
      // Stop Microsoft MCE ehRecvr, mcrdsvc and ehSched processes (if they exist)
      try
      {
        ServiceController[] services = ServiceController.GetServices();
        foreach (ServiceController service in services)
        {
          if (service.ServiceName.Equals("ehRecvr", StringComparison.OrdinalIgnoreCase))
          {
            if (service.Status != ServiceControllerStatus.Stopped && service.Status != ServiceControllerStatus.StopPending)
              service.Stop();
          }
          else if (service.ServiceName.Equals("ehSched", StringComparison.OrdinalIgnoreCase))
          {
            if (service.Status != ServiceControllerStatus.Stopped && service.Status != ServiceControllerStatus.StopPending)
              service.Stop();
          }
          else if (service.ServiceName.Equals("mcrdsvc", StringComparison.OrdinalIgnoreCase))
          {
            if (service.Status != ServiceControllerStatus.Stopped && service.Status != ServiceControllerStatus.StopPending)
              service.Stop();
          }
        }
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

      // XP & Vista ...
      // Kill Microsoft MCE ehtray process (if it exists)
      try
      {
        Process[] processes = Process.GetProcesses();
        foreach (Process proc in processes)
          if (proc.ProcessName.Equals("ehtray", StringComparison.OrdinalIgnoreCase))
            proc.Kill();
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

    // TODO: Convert this function to a lookup from an XML file, then provide multiple files and a way to fine-tune...
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
          throw new ArgumentException(String.Format("Unknown Key Value {0}", keyCode), "keyCode");
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

    #endregion Implementation

  }

}
