using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Win32.SafeHandles;

using IRServerPluginInterface;

namespace UirtTransceiver
{
  
  [CLSCompliant(false)]
  public class UirtTransceiver : IIRServerPlugin, IDisposable
  {

    #region Interop

    [StructLayout(LayoutKind.Sequential)]
    struct UUINFO
    {
      public int fwVersion;
      public int protVersion;
      public char fwDateDay;
      public char fwDateMonth;
      public char fwDateYear;
    }

    //Not used
    //[StructLayout(LayoutKind.Sequential)]
    //internal struct UUGPIO
    //{
    //  public byte[] irCode;
    //  public byte action;
    //  public byte duration;
    //}

    [DllImport("uuirtdrv.dll")]
    static extern IntPtr UUIRTOpen();

    [DllImport("uuirtdrv.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool UUIRTClose(
      IntPtr hHandle);

    //[DllImport("uuirtdrv.dll")]
    //[return: MarshalAs(UnmanagedType.Bool)]
    //internal static extern bool UUIRTGetDrvInfo(ref int puDrvVersion);

    //[DllImport("uuirtdrv.dll")]
    //[return: MarshalAs(UnmanagedType.Bool)]
    //internal static extern bool UUIRTGetUUIRTInfo(
    //  IntPtr hHandle,
    //  ref UUINFO puuInfo);

    //[DllImport("uuirtdrv.dll")]
    //[return: MarshalAs(UnmanagedType.Bool)]
    //internal static extern bool UUIRTGetUUIRTConfig(
    //  IntPtr hHandle,
    //  ref uint puConfig);

    //[DllImport("uuirtdrv.dll")]
    //[return: MarshalAs(UnmanagedType.Bool)]
    //internal static extern bool UUIRTSetUUIRTConfig(
    //  IntPtr hHandle,
    //  uint uConfig);

    [DllImport("uuirtdrv.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool UUIRTTransmitIR(
      IntPtr hHandle,
      string IRCode,
      int codeFormat,
      int repeatCount,
      int inactivityWaitTime,
      IntPtr hEvent,
      int res1,
      int res2);

    [DllImport("uuirtdrv.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool UUIRTLearnIR(
      IntPtr hHandle,
      int codeFormat,
      [MarshalAs(UnmanagedType.LPStr)] StringBuilder ircode,
      IRLearnCallbackDelegate progressProc,
      int userData,
      ref int pAbort,
      int param1,
      [MarshalAs(UnmanagedType.AsAny)] Object o,
      [MarshalAs(UnmanagedType.AsAny)] Object oo);

    [DllImport("uuirtdrv.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool UUIRTSetReceiveCallback(
      IntPtr hHandle,
      UUIRTReceiveCallbackDelegate receiveProc,
      int none);

    //[DllImport("uuirtdrv.dll")]
    //static extern bool UUIRTSetUUIRTGPIOCfg(IntPtr hHandle, int index, ref UUGPIO GpioSt);

    //[DllImport("uuirtdrv.dll")]
    //static extern bool UUIRTGetUUIRTGPIOCfg(IntPtr hHandle, ref int numSlots, ref uint dwPortPins,
    //                                                ref UUGPIO GpioSt);

    #endregion

    #region Delegates

    delegate void UUIRTReceiveCallbackDelegate(string irCode, IntPtr userData);
    delegate void IRLearnCallbackDelegate(uint progress, uint sigQuality, ulong carrierFreq, IntPtr userData);

    #endregion Delegates

    #region Constants

    static readonly string ConfigurationFile =
      Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
      "\\IR Server Suite\\IR Server\\USB-UIRT Transceiver.xml";

    const int UUIRTDRV_IRFMT_UUIRT             = 0x0000;
    const int UUIRTDRV_IRFMT_PRONTO            = 0x0010;
    const int UUIRTDRV_IRFMT_LEARN_FORCERAW    = 0x0100;
    const int UUIRTDRV_IRFMT_LEARN_FORCESTRUC  = 0x0200;
    const int UUIRTDRV_IRFMT_LEARN_FORCEFREQ   = 0x0400;
    const int UUIRTDRV_IRFMT_LEARN_FREQDETECT  = 0x0800;
    
    static readonly string[] Ports  = new string[] { "Default", "Port 1", "Port 2", "Port 3" };
    static readonly string[] Speeds = new string[] { "Default" };

    const int AbortLearn = -1;
    const int AllowLearn = 0;

    #endregion Constants

    #region Variables

    RemoteButtonHandler _remoteButtonHandler = null;

    string _blastPort = Ports[0];

    int _repeatDelay;
    int _blastRepeats;

    string _lastCode        = String.Empty;
    DateTime _lastCodeTime  = DateTime.Now;

    // -------

    int _abortLearn = AllowLearn;
    bool _learnTimedOut;
    UUIRTReceiveCallbackDelegate _receiveCallback = null;
    bool _isUsbUirtLoaded = false;
    IntPtr _usbUirtHandle = IntPtr.Zero;
    bool _disposed = false;

    #endregion Variables

    #region Deconstructor

    ~UirtTransceiver()
    {
      Dispose(false);
    }

    #endregion Deconstructor

    #region IDisposable Members

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposeManagedResources)
    {
      if (!_disposed)
      {
        _disposed = true;

        if (disposeManagedResources)
        {
          // Dispose any managed resources.
        }

        if (_isUsbUirtLoaded && _usbUirtHandle != new IntPtr(-1) && _usbUirtHandle != IntPtr.Zero)
        {
          UUIRTClose(_usbUirtHandle);
          _usbUirtHandle = IntPtr.Zero;
          _isUsbUirtLoaded = false;
        }
      }
    }

    #endregion

    #region IIRServerPlugin Members

    public string Name        { get { return "USB-UIRT"; } }
    public string Version     { get { return "1.0.3.2"; } }
    public string Author      { get { return "and-81"; } }
    public string Description { get { return "Support for the USB-UIRT transceiver"; } }
    public bool CanReceive    { get { return true; } }
    public bool CanTransmit   { get { return true; } }
    public bool CanLearn      { get { return true; } }
    public bool CanConfigure  { get { return true; } }

    public RemoteButtonHandler RemoteButtonCallback
    {
      get { return _remoteButtonHandler; }
      set { _remoteButtonHandler = value; }
    }

    public string[] AvailablePorts  { get { return Ports; }   }
    public string[] AvailableSpeeds { get { return Speeds; }  }

    public void Configure()
    {
      LoadSettings();

      Configure config = new Configure();

      config.RepeatDelay  = _repeatDelay;
      config.BlastRepeats = _blastRepeats;

      if (config.ShowDialog() == DialogResult.OK)
      {
        _repeatDelay  = config.RepeatDelay;
        _blastRepeats = config.BlastRepeats;

        SaveSettings();
      }
    }
    public bool Start()
    {
      LoadSettings();

      _usbUirtHandle = UUIRTOpen();

      if (_usbUirtHandle != new IntPtr(-1))
      {
        _isUsbUirtLoaded = true;

        // Setup callack to receive IR messages
        _receiveCallback = new UUIRTReceiveCallbackDelegate(UUIRTReceiveCallback);
        UUIRTSetReceiveCallback(_usbUirtHandle, _receiveCallback, 0);
      }

      return _isUsbUirtLoaded;
    }
    public void Suspend() { }
    public void Resume()  { }
    public void Stop()
    {
      UUIRTClose(_usbUirtHandle);

      _usbUirtHandle = IntPtr.Zero;
      _isUsbUirtLoaded = false;
    }

    public bool Transmit(string file)
    {
      bool result = false;

      try
      {
        StreamReader streamReader = new StreamReader(file);
        string irCode = streamReader.ReadToEnd();
        streamReader.Close();

        // Set blaster port ...
        if (_blastPort == Ports[1])
          irCode = "Z1" + irCode;
        else if (_blastPort == Ports[2])
          irCode = "Z2" + irCode;
        else if (_blastPort == Ports[3])
          irCode = "Z3" + irCode;

        result = UUIRTTransmitIR(
          _usbUirtHandle,         // Handle to USB-UIRT
          irCode,                 // IR Code
          UUIRTDRV_IRFMT_PRONTO,  // Code Format
          _blastRepeats,          // Repeat Count
          0,                      // Inactivity Wait Time
          IntPtr.Zero,            // hEvent
          0,                      // reserved1
          0                       // reserved2
        );
      }
      catch
      {
        result = false;
      }

      return result;
    }
    public LearnStatus Learn(string file)
    {
      bool result = false;
      
      StringBuilder irCode = new StringBuilder("1", 2048);
      _abortLearn = AllowLearn;
      _learnTimedOut = false;

      Timer timer = new Timer();
      
      // TODO: Implement proper timeout ...
      timer.Interval = 8000;
      timer.Tick += new EventHandler(timer_Tick);

      result = UirtTransceiver.UUIRTLearnIR(
        _usbUirtHandle,                                     // Handle to USB-UIRT
        UirtTransceiver.UUIRTDRV_IRFMT_PRONTO,              //  | UirtTransceiver.UUIRTDRV_IRFMT_LEARN_FORCERAW
        irCode,                                             // Where to put the IR Code
        null,                                               // Learn status callback
        0,                                                  // User data
        ref _abortLearn,                                    // Abort flag?
        0,
        null,
        null);

      timer.Stop();

      if (_learnTimedOut)
      {
        return LearnStatus.Timeout;
      }
      else if (result)
      {
        StreamWriter streamWriter = new StreamWriter(file, false);
        streamWriter.Write(irCode.ToString());
        streamWriter.Close();

        return LearnStatus.Success;
      }
      else
      {
        return LearnStatus.Failure;
      }
    }
    
    public bool SetPort(string port)
    {
      foreach (string availablePort in Ports)
      {
        if (port == availablePort)
        {
          _blastPort = availablePort;
          return true;
        }
      }

      return false;
    }
    public bool SetSpeed(string speed)
    {
      foreach (string availableSpeed in Speeds)
        if (speed == availableSpeed)
          return true;

      return false;
    }

    #endregion IIRServerPlugin Members

    #region Implementation

    void LoadSettings()
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationFile);

        _repeatDelay  = int.Parse(doc.DocumentElement.Attributes["RepeatDelay"].Value);
        _blastRepeats = int.Parse(doc.DocumentElement.Attributes["BlastRepeats"].Value);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());

        _repeatDelay  = 500;
        _blastRepeats = 4;
      }
    }
    void SaveSettings()
    {
      try
      {
        XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, System.Text.Encoding.UTF8);
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 1;
        writer.IndentChar = (char)9;
        writer.WriteStartDocument(true);
        writer.WriteStartElement("settings"); // <settings>

        writer.WriteAttributeString("RepeatDelay", _repeatDelay.ToString());
        writer.WriteAttributeString("BlastRepeats", _blastRepeats.ToString());

        writer.WriteEndElement(); // </settings>
        writer.WriteEndDocument();
        writer.Close();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }

    void UUIRTReceiveCallback(string keyCode, IntPtr userData)
    {
      if (_remoteButtonHandler == null)
        return;

      TimeSpan timeSpan = DateTime.Now - _lastCodeTime;

      if (keyCode == _lastCode) // Repeated button
      {
        if (timeSpan.Milliseconds > _repeatDelay)
        {
          _remoteButtonHandler(keyCode);
          _lastCodeTime = DateTime.Now;
        }
      }
      else
      {
        _remoteButtonHandler(keyCode);
        _lastCodeTime = DateTime.Now;
      }
      
      _lastCode = keyCode;
    }

    void timer_Tick(object sender, EventArgs e)
    {
      _abortLearn = AbortLearn;
      _learnTimedOut = true;

      ((Timer)sender).Stop();
    }

    #endregion Implementation

  }

}
