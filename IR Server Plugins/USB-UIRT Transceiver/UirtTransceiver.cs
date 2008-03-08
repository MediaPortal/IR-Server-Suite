using System;
#if TRACE
using System.Diagnostics;
#endif
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Win32.SafeHandles;

namespace InputService.Plugin
{

  /// <summary>
  /// IR Server Plugin for USB-UIRT Transceiver device.
  /// </summary>
  [CLSCompliant(false)]
  public class UirtTransceiver : PluginBase, IConfigure, ITransmitIR, ILearnIR, IRemoteReceiver, IDisposable
  {

    #region Constants

    static readonly string ConfigurationFile = Path.Combine(ConfigurationPath, "USB-UIRT Transceiver.xml");

    const int UUIRTDRV_IRFMT_UUIRT             = 0x0000;
    const int UUIRTDRV_IRFMT_PRONTO            = 0x0010;
    const int UUIRTDRV_IRFMT_LEARN_FORCERAW    = 0x0100;
    const int UUIRTDRV_IRFMT_LEARN_FORCESTRUC  = 0x0200;
    const int UUIRTDRV_IRFMT_LEARN_FORCEFREQ   = 0x0400;
    const int UUIRTDRV_IRFMT_LEARN_FREQDETECT  = 0x0800;
    
    static readonly string[] Ports  = new string[] { "Default", "Port 1", "Port 2", "Port 3" };

    const int AbortLearn = 1;
    const int AllowLearn = 0;

    #endregion Constants

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
    //struct UUGPIO
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
    //static extern bool UUIRTGetDrvInfo(ref int puDrvVersion);

    //[DllImport("uuirtdrv.dll")]
    //[return: MarshalAs(UnmanagedType.Bool)]
    //static extern bool UUIRTGetUUIRTInfo(
    //  IntPtr hHandle,
    //  ref UUINFO puuInfo);

    //[DllImport("uuirtdrv.dll")]
    //[return: MarshalAs(UnmanagedType.Bool)]
    // static extern bool UUIRTGetUUIRTConfig(
    //  IntPtr hHandle,
    //  ref uint puConfig);

    //[DllImport("uuirtdrv.dll")]
    //[return: MarshalAs(UnmanagedType.Bool)]
    //static extern bool UUIRTSetUUIRTConfig(
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
      //[MarshalAs(UnmanagedType.LPStr)]
      StringBuilder ircode,
      IRLearnCallbackDelegate progressProc,
      IntPtr userData,
      IntPtr abort,
      int param1,
      IntPtr reserved1,
      IntPtr reserved2);

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

    #endregion Interop

    #region Delegates

    delegate void UUIRTReceiveCallbackDelegate(string irCode, IntPtr userData);
    delegate void IRLearnCallbackDelegate(uint progress, uint sigQuality, ulong carrierFreq, IntPtr userData);

    #endregion Delegates

    #region Variables

    RemoteHandler _remoteButtonHandler;

    int _repeatDelay;
    int _blastRepeats;
    int _learnTimeout;

    string _lastCode        = String.Empty;
    DateTime _lastCodeTime  = DateTime.Now;

    // -------

    IntPtr _abortLearn = IntPtr.Zero;
    bool _learnTimedOut;
    bool _isUsbUirtLoaded;
    IntPtr _usbUirtHandle = IntPtr.Zero;
    UUIRTReceiveCallbackDelegate _receiveCallbackDelegate;
    bool _disposed;

    #endregion Variables

    #region Destructor

    /// <summary>
    /// Releases unmanaged resources and performs other cleanup operations before the
    /// <see cref="UirtTransceiver"/> is reclaimed by garbage collection.
    /// </summary>
    ~UirtTransceiver()
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
    private void Dispose(bool disposing)
    {
      if (!_disposed)
      {
        _disposed = true;

        if (disposing)
        {
          // Dispose managed resources ...
        }

        // Free native resources ...

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

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name         { get { return "USB-UIRT"; } }
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
    public override string Description  { get { return "Support for the USB-UIRT transceiver"; } }
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
        IntPtr handle = UUIRTOpen();

        if (handle != new IntPtr(-1) && handle != IntPtr.Zero)
        {
          UUIRTClose(handle);
          return true;
        }
      }
      catch
      {
      }

      return false;
    }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    public override void Start()
    {
      LoadSettings();

      _usbUirtHandle = UUIRTOpen();

      if (_usbUirtHandle == IntPtr.Zero)
        throw new InvalidOperationException("Failed to initialize");

      _isUsbUirtLoaded = true;

      _receiveCallbackDelegate = new UUIRTReceiveCallbackDelegate(UUIRTReceiveCallback);

      UUIRTSetReceiveCallback(_usbUirtHandle, _receiveCallbackDelegate, 0);
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
      if (_abortLearn != IntPtr.Zero)
        Marshal.WriteInt32(_abortLearn, AbortLearn);

      if (_usbUirtHandle != IntPtr.Zero)
      {
        UUIRTClose(_usbUirtHandle);
        _usbUirtHandle = IntPtr.Zero;
        _receiveCallbackDelegate = null;
      }

      _isUsbUirtLoaded = false;
    }

    /// <summary>
    /// Configure the IR Server plugin.
    /// </summary>
    public void Configure(IWin32Window owner)
    {
      LoadSettings();

      Configure config = new Configure();

      config.RepeatDelay  = _repeatDelay;
      config.BlastRepeats = _blastRepeats;
      config.LearnTimeout = _learnTimeout;

      if (config.ShowDialog(owner) == DialogResult.OK)
      {
        _repeatDelay  = config.RepeatDelay;
        _blastRepeats = config.BlastRepeats;
        _learnTimeout = config.LearnTimeout;

        SaveSettings();
      }
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
    /// Lists the available blaster ports.
    /// </summary>
    /// <value>The available ports.</value>
    public string[] AvailablePorts { get { return Ports; } }

    /// <summary>
    /// Transmit an infrared command.
    /// </summary>
    /// <param name="port">Port to transmit on.</param>
    /// <param name="data">Data to transmit.</param>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public bool Transmit(string port, byte[] data)
    {
      if (String.IsNullOrEmpty(port))
        throw new ArgumentNullException("port");

      if (data == null)
        throw new ArgumentNullException("data");

      bool result = false;

      string irCode = Encoding.ASCII.GetString(data);

      // Set blaster port ...
      if (port.Equals(Ports[1], StringComparison.OrdinalIgnoreCase))
        irCode = "Z1" + irCode;
      else if (port.Equals(Ports[2], StringComparison.OrdinalIgnoreCase))
        irCode = "Z2" + irCode;
      else if (port.Equals(Ports[3], StringComparison.OrdinalIgnoreCase))
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

    /// <summary>
    /// Learn an infrared command.
    /// </summary>
    /// <param name="data">New infrared command.</param>
    /// <returns>
    /// Tells the calling code if the learn was Successful, Failed or Timed Out.
    /// </returns>
    public LearnStatus Learn(out byte[] data)
    {
      bool result = false;

      data = null;

      StringBuilder irCode = new StringBuilder(4096);
      _learnTimedOut = false;
      
      Timer timer = new Timer();
      timer.Interval = _learnTimeout;
      timer.Tick += new EventHandler(timer_Tick);
      timer.Enabled = true;
      timer.Start();

      try
      {
        _abortLearn = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Int32)));
        Marshal.WriteInt32(_abortLearn, AllowLearn);

        result = UUIRTLearnIR(
          _usbUirtHandle,                                     // Handle to USB-UIRT
          UUIRTDRV_IRFMT_PRONTO,
          irCode,                                             // Where to put the IR Code
          null,                                               // Learn status callback
          IntPtr.Zero,                                        // User data
          _abortLearn,                                        // Abort flag?
          0,
          IntPtr.Zero,
          IntPtr.Zero);
      }
      finally
      {
        Marshal.FreeHGlobal(_abortLearn);
        _abortLearn = IntPtr.Zero;
      }

      timer.Stop();

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

    /// <summary>
    /// Loads the settings.
    /// </summary>
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
#if TRACE
      catch (Exception ex)
      {
        Trace.WriteLine(ex.ToString());
#else
      catch
      {
#endif

        _repeatDelay  = 500;
        _blastRepeats = 3;
        _learnTimeout = 10000;
      }
    }
    /// <summary>
    /// Saves the settings.
    /// </summary>
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

          writer.WriteAttributeString("RepeatDelay", _repeatDelay.ToString());
          writer.WriteAttributeString("BlastRepeats", _blastRepeats.ToString());
          writer.WriteAttributeString("LearnTimeout", _learnTimeout.ToString());

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

    /// <summary>
    /// The receive callback.
    /// </summary>
    /// <param name="keyCode">The key code.</param>
    /// <param name="userData">The user data.</param>
    void UUIRTReceiveCallback(string keyCode, IntPtr userData)
    {
      if (_remoteButtonHandler == null)
        return;

      TimeSpan timeSpan = DateTime.Now - _lastCodeTime;

      if (keyCode.Equals(_lastCode, StringComparison.Ordinal)) // Repeated button
      {
        if (timeSpan.Milliseconds > _repeatDelay)
        {
          _remoteButtonHandler(this.Name, keyCode);
          _lastCodeTime = DateTime.Now;
        }
      }
      else
      {
        _remoteButtonHandler(this.Name, keyCode);
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

    /// <summary>
    /// Handles the Tick event of the timer control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    void timer_Tick(object sender, EventArgs e)
    {
      if (_abortLearn != IntPtr.Zero)
        Marshal.WriteInt32(_abortLearn, AbortLearn);

      _learnTimedOut = true;

      ((Timer)sender).Stop();
    }

    #endregion Implementation

  }

}
