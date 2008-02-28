using System;
using System.Collections.Generic;
#if TRACE
using System.Diagnostics;
#endif
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace InputService.Plugin
{

  /// <summary>
  /// IR Server Plugin for IRMan Receiver device.
  /// </summary>
  public class IRManReceiver : PluginBase, IConfigure, IRemoteReceiver
  {

    #region Constants

    static readonly string ConfigurationFile =
      Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
      "\\IR Server Suite\\Input Service\\IRMan Receiver.xml";

    const int DeviceBufferSize = 6;

    #endregion Constants

    #region Variables

    SerialPort _serialPort;
    byte[] _deviceBuffer;

    RemoteHandler _remoteButtonHandler;

    int _repeatDelay;
    string _serialPortName;

    bool _disposed;

    string _lastCode        = String.Empty;
    DateTime _lastCodeTime  = DateTime.Now;

    #endregion Variables

    #region Implementation

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name         { get { return "IRMan"; } }
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
    public override string Description  { get { return "Receiver support for the Serial IRMan device"; } }

    /// <summary>
    /// Detect the presence of this device.  Devices that cannot be detected will always return false.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the device is present, otherwise <c>false</c>.
    /// </returns>
    public override bool Detect()
    {
      return false;
    }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    public override void Start()
    {
      LoadSettings();

      _deviceBuffer = new byte[DeviceBufferSize];

      _serialPort                 = new SerialPort(_serialPortName, 9600, Parity.None, 8, StopBits.One);
      _serialPort.Handshake       = Handshake.None;
      _serialPort.DtrEnable       = true;
      _serialPort.RtsEnable       = true;
      _serialPort.ReadBufferSize  = DeviceBufferSize;
      _serialPort.ReadTimeout     = 1000;

      _serialPort.Open();
      Thread.Sleep(100);
      _serialPort.DiscardInBuffer();

      _serialPort.Write("I");
      Thread.Sleep(100);
      _serialPort.Write("R");
      Thread.Sleep(100);

      _serialPort.Read(_deviceBuffer, 0, 2);

      if (_deviceBuffer[0] == 'O' && _deviceBuffer[1] == 'K')
      {
        _serialPort.ReceivedBytesThreshold = DeviceBufferSize;
        _serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
      }
      else
      {
        throw new ApplicationException("Failed to initialize device");
      }
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
      if (_serialPort == null)
        return;

      try
      {
        _serialPort.Dispose();
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
      finally
      {
        _serialPort = null;
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
    /// Configure the IR Server plugin.
    /// </summary>
    public void Configure(IWin32Window owner)
    {
      LoadSettings();

      Configure config = new Configure();

      config.RepeatDelay  = _repeatDelay;
      config.CommPort     = _serialPortName;

      if (config.ShowDialog(owner) == DialogResult.OK)
      {
        _repeatDelay      = config.RepeatDelay;
        _serialPortName   = config.CommPort;

        SaveSettings();
      }
    }

    /// <summary>
    /// Handles the DataReceived event of the SerialPort control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.IO.Ports.SerialDataReceivedEventArgs"/> instance containing the event data.</param>
    void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
      try
      {
        _serialPort.Read(_deviceBuffer, 0, DeviceBufferSize);

        TimeSpan timeSpan = DateTime.Now - _lastCodeTime;

        StringBuilder keyCode = new StringBuilder(2 * DeviceBufferSize);
        for (int index = 0; index < DeviceBufferSize; index++)
          keyCode.Append(_deviceBuffer[index].ToString("X2"));

        string thisCode = keyCode.ToString();

        if (thisCode.Equals(_lastCode, StringComparison.Ordinal)) // Repeated button
        {
          if (timeSpan.Milliseconds > _repeatDelay)
          {
            _remoteButtonHandler(this.Name, thisCode);
            _lastCodeTime = DateTime.Now;
          }
        }
        else
        {
          _remoteButtonHandler(this.Name, thisCode);
          _lastCodeTime = DateTime.Now;
        }

        _lastCode = thisCode;
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
    /// Releases unmanaged and - optionally - managed resources
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      // process only if mananged and unmanaged resources have
      // not been disposed of.
      if (!this._disposed)
      {
        if (disposing)
        {
          // dispose managed resources
          Stop();
        }

        // dispose unmanaged resources
        this._disposed = true;
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

        _repeatDelay    = int.Parse(doc.DocumentElement.Attributes["RepeatDelay"].Value);
        _serialPortName = doc.DocumentElement.Attributes["SerialPortName"].Value;
      }
#if TRACE
      catch (Exception ex)
      {
        Trace.WriteLine(ex.ToString());
#else
      catch
      {
#endif

        _repeatDelay    = 500;
        _serialPortName = "COM1";
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
          writer.WriteAttributeString("SerialPortName", _serialPortName);

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
    
    #endregion Implementation

  }

}
