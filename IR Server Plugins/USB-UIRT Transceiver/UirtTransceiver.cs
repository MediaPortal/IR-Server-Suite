using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Win32.SafeHandles;

using IRServerPluginInterface;

namespace UirtTransceiver
{

  /// <summary>
  /// IR Server Plugin for USB-UIRT Transceiver device.
  /// </summary>
  [CLSCompliant(false)]
  public class UirtTransceiver :
    IRServerPluginBase, IConfigure, ITransmitIR, ILearnIR, IRemoteReceiver, IDisposable
  {

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

    const int AbortLearn = 1;
    const int AllowLearn = 0;

    #endregion Constants

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
    IntPtr _usbUirtHandle = new IntPtr(-1);
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
          NativeMethods.UUIRTClose(_usbUirtHandle);
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
    public override string Version      { get { return "1.0.3.5"; } }
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
    /// Detect the presence of this device.  Devices that cannot be detected will always return false.
    /// </summary>
    /// <returns>
    /// true if the device is present, otherwise false.
    /// </returns>
    public override bool Detect()
    {
      try
      {
        IntPtr handle = NativeMethods.UUIRTOpen();

        if (handle != new IntPtr(-1))
        {
          NativeMethods.UUIRTClose(handle);
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
    /// <returns>true if successful, otherwise false.</returns>
    public override bool Start()
    {
      LoadSettings();

      _usbUirtHandle = NativeMethods.UUIRTOpen();

      if (_usbUirtHandle != new IntPtr(-1))
      {
        if (_usbUirtHandle == IntPtr.Zero)
          throw new ApplicationException("USBUIRT LOGIC ERROR 1, REPORT THIS TO and-81");

        _isUsbUirtLoaded = true;

        NativeMethods.UUIRTSetReceiveCallback(
          _usbUirtHandle,
          new NativeMethods.UUIRTReceiveCallbackDelegate(UUIRTReceiveCallback),
          0);
      }

      return _isUsbUirtLoaded;
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

      if (_usbUirtHandle != new IntPtr(-1))
      {
        if (_usbUirtHandle == IntPtr.Zero)
          throw new ApplicationException("USBUIRT LOGIC ERROR 2, REPORT THIS TO and-81");

        NativeMethods.UUIRTClose(_usbUirtHandle);
        _usbUirtHandle = new IntPtr(-1);
      }

      _isUsbUirtLoaded = false;
    }

    /// <summary>
    /// Configure the IR Server plugin.
    /// </summary>
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
    /// <returns>true if successful, otherwise false.</returns>
    public bool Transmit(string port, byte[] data)
    {
      if (String.IsNullOrEmpty(port))
        throw new ArgumentNullException("port");

      if (data == null)
        throw new ArgumentNullException("data");

      bool result = false;

      string irCode = Encoding.ASCII.GetString(data);

      // Set blaster port ...
      if (port.Equals(Ports[1], StringComparison.InvariantCultureIgnoreCase))
        irCode = "Z1" + irCode;
      else if (port.Equals(Ports[2], StringComparison.InvariantCultureIgnoreCase))
        irCode = "Z2" + irCode;
      else if (port.Equals(Ports[3], StringComparison.InvariantCultureIgnoreCase))
        irCode = "Z3" + irCode;

      result = NativeMethods.UUIRTTransmitIR(
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

        result = NativeMethods.UUIRTLearnIR(
          _usbUirtHandle,                                     // Handle to USB-UIRT
          UirtTransceiver.UUIRTDRV_IRFMT_PRONTO,
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
        using (XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, System.Text.Encoding.UTF8))
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
