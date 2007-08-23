using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Win32.SafeHandles;

using IRServerPluginInterface;

namespace HcwTransceiver
{

  [CLSCompliant(false)]
  public class HcwTransceiver :
    IRServerPlugin, IConfigure, ITransmitIR, ILearnIR, IRemoteReceiver, IDisposable
  {

    #region Interop


    const int WM_ACTIVATE = 0x0006;
    const int WM_POWERBROADCAST = 0x0218;
    const int WA_INACTIVE = 0;
    const int WA_ACTIVE = 1;
    const int WA_CLICKACTIVE = 2;

    const int PBT_APMRESUMEAUTOMATIC = 0x0012;
    const int PBT_APMRESUMECRITICAL = 0x0006;



    #endregion

    #region Delegates

    //Sets up callback so that other forms can catch a key press
    public delegate void HCWEvent(int keypress);
    public event HCWEvent HCWKeyPressed;

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

    const int AbortLearn = -1;
    const int AllowLearn = 0;

    #endregion Constants

    #region Variables

    RemoteHandler _remoteButtonHandler = null;

    string _blastPort = Ports[0];

    int _repeatDelay;
    int _blastRepeats;
    int _learnTimeout;

    //ulong _learnCarrierFreq;

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

    ~HcwTransceiver()
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

    #endregion IDisposable Members

    #region Implementation

    public override string Name         { get { return "USB-UIRT"; } }
    public override string Version      { get { return "1.0.3.3"; } }
    public override string Author       { get { return "and-81"; } }
    public override string Description  { get { return "Support for the USB-UIRT transceiver"; } }

    public override bool Start()
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
    public override void Suspend()
    {
      Stop();
    }
    public override void Resume()
    {
      Start();
    }
    public override void Stop()
    {
      UUIRTClose(_usbUirtHandle);

      _usbUirtHandle = IntPtr.Zero;
      _isUsbUirtLoaded = false;
    }

    public void Configure()
    {
      LoadSettings();

      Configure config = new Configure();

      config.RepeatDelay  = _repeatDelay;
      config.BlastRepeats = _blastRepeats;
      config.LearnTimeout = _learnTimeout;

      if (config.ShowDialog() == DialogResult.OK)
      {
        _repeatDelay  = config.RepeatDelay;
        _blastRepeats = config.BlastRepeats;
        _learnTimeout = config.LearnTimeout;

        SaveSettings();
      }
    }

    public RemoteHandler RemoteCallback
    {
      get { return _remoteButtonHandler; }
      set { _remoteButtonHandler = value; }
    }

    public string[] AvailablePorts { get { return Ports; }   }

    public bool Transmit(string port, byte[] data)
    {
      bool result = false;

      string irCode = Encoding.ASCII.GetString(data);

      // Set blaster port ...
      if (port.Equals(Ports[1], StringComparison.InvariantCultureIgnoreCase))
        irCode = "Z1" + irCode;
      else if (port.Equals(Ports[2], StringComparison.InvariantCultureIgnoreCase))
        irCode = "Z2" + irCode;
      else if (port.Equals(Ports[3], StringComparison.InvariantCultureIgnoreCase))
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

      return result;
    }
    public LearnStatus Learn(out byte[] data)
    {
      bool result = false;

      data = null;

      StringBuilder irCode = new StringBuilder("1", 2048);
      _abortLearn = AllowLearn;
      _learnTimedOut = false;
      
      //_learnCarrierFreq = 0;

      Timer timer = new Timer();
      timer.Interval = _learnTimeout;
      timer.Tick += new EventHandler(timer_Tick);
      timer.Enabled = true;
      timer.Start();

      //IRLearnCallbackDelegate learnCallback = new IRLearnCallbackDelegate(UUIRTLearnCallback);

      result = UirtTransceiver.UUIRTLearnIR(
        _usbUirtHandle,                                     // Handle to USB-UIRT
        UirtTransceiver.UUIRTDRV_IRFMT_PRONTO | UirtTransceiver.UUIRTDRV_IRFMT_LEARN_FREQDETECT,              //  | UirtTransceiver.UUIRTDRV_IRFMT_LEARN_FORCERAW
        irCode,                                             // Where to put the IR Code
        null,                                               // Learn status callback
        0,                                                  // User data
        ref _abortLearn,                                    // Abort flag?
        0,
        null,
        null);

      timer.Stop();

      //MessageBox.Show(_learnCarrierFreq.ToString());

      if (_learnTimedOut)
      {
        return LearnStatus.Timeout;
      }
      else if (result)
      {
        data = Encoding.ASCII.GetBytes(irCode.ToString());

        return LearnStatus.Success;
      }
      else
      {
        return LearnStatus.Failure;
      }
    }

    void LoadSettings()
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationFile);

        _repeatDelay  = int.Parse(doc.DocumentElement.Attributes["RepeatDelay"].Value);
        _blastRepeats = int.Parse(doc.DocumentElement.Attributes["BlastRepeats"].Value);
        _learnTimeout = int.Parse(doc.DocumentElement.Attributes["LearnTimeout"].Value);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());

        _repeatDelay  = 500;
        _blastRepeats = 4;
        _learnTimeout = 10000;
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
        writer.WriteAttributeString("LearnTimeout", _learnTimeout.ToString());

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
    /*
    void UUIRTLearnCallback(uint progress, uint sigQuality, ulong carrierFreq, IntPtr userData)
    {
      _learnCarrierFreq = carrierFreq;
      //MessageBox.Show(_learnCarrierFreq.ToString());
    }
    */
    void timer_Tick(object sender, EventArgs e)
    {
      _abortLearn = AbortLearn;
      _learnTimedOut = true;

      ((Timer)sender).Stop();
    }

    #endregion Implementation

  }

}
