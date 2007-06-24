using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using IRServerPluginInterface;

namespace IRManReceiver
{

  public class IRManReceiver : IIRServerPlugin
  {

    #region Constants

    static readonly string ConfigurationFile =
      Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
      "\\IR Server Suite\\IR Server\\IRMan Receiver.xml";

    static readonly string[] Ports  = new string[] { "None" };
    static readonly string[] Speeds = new string[] { "None" };

    const int DeviceBufferSize = 6;

    #endregion Constants

    #region Variables

    SerialPort _serialPort;
    byte[] _deviceBuffer;

    RemoteButtonHandler _remoteButtonHandler = null;

    int _repeatDelay;
    string _serialPortName;

    bool _disposed = false;

    string _lastCode        = String.Empty;
    DateTime _lastCodeTime  = DateTime.Now;

    #endregion Variables

    #region IIRServerPlugin Members

    public string Name          { get { return "IRMan"; } }
    public string Version       { get { return "1.0.3.2"; } }
    public string Author        { get { return "and-81"; } }
    public string Description   { get { return "Receiver support for the Serial IRMan device"; } }
    public bool   CanReceive    { get { return true; } }
    public bool   CanTransmit   { get { return false; } }
    public bool   CanLearn      { get { return false; } }
    public bool   CanConfigure  { get { return true; } }

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
      config.CommPort     = _serialPortName;

      if (config.ShowDialog() == DialogResult.OK)
      {
        _repeatDelay    = config.RepeatDelay;
        _serialPortName = config.CommPort;

        SaveSettings();
      }
    }
    public bool Start()
    {
      LoadSettings();

      _deviceBuffer = new byte[DeviceBufferSize];

      _serialPort                 = new SerialPort(_serialPortName, 9600, Parity.None, 8, StopBits.One);
      _serialPort.Handshake       = Handshake.None;
      _serialPort.DtrEnable       = true;
      _serialPort.RtsEnable       = true;
      _serialPort.ReadBufferSize  = DeviceBufferSize;

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

        return true;
      }

      return false;
    }
    public void Suspend() { }
    public void Resume()  { }
    public void Stop()
    {
      if (_serialPort == null)
        return;

      try
      {
        if (_serialPort.IsOpen)
          _serialPort.Close();
      }
      catch { }

      _serialPort = null;
    }

    public bool Transmit(string file) { return false; }
    public LearnStatus Learn(string file) { return LearnStatus.Failure; }

    public bool SetPort(string port)    { return true; }
    public bool SetSpeed(string speed)  { return true; }

    #endregion IIRServerPlugin Members

    #region Implementation

    void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
      try
      {
        _serialPort.Read(_deviceBuffer, 0, DeviceBufferSize);
        
        StringBuilder keyCode = new StringBuilder(2*DeviceBufferSize);
        for (int index = 0; index < DeviceBufferSize; index++)
          keyCode.Append(_deviceBuffer[index].ToString("X2"));
        
        TimeSpan timeSpan = DateTime.Now - _lastCodeTime;

        if (keyCode.ToString() == _lastCode) // Repeated button
        {
          if (timeSpan.Milliseconds > _repeatDelay)
          {
            _remoteButtonHandler(keyCode.ToString());
            _lastCodeTime = DateTime.Now;
          }
        }
        else
        {
          _remoteButtonHandler(keyCode.ToString());
          _lastCodeTime = DateTime.Now;
        }

        _lastCode = keyCode.ToString();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }

    protected virtual void Dispose(bool disposeManagedResources)
    {
      // process only if mananged and unmanaged resources have
      // not been disposed of.
      if (!this._disposed)
      {
        if (disposeManagedResources)
        {
          // dispose managed resources
          if (_serialPort != null)
          {
            _serialPort.Dispose();
            _serialPort = null;
          }
        }

        // dispose unmanaged resources
        this._disposed = true;
      }
    }

    void LoadSettings()
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationFile);

        _repeatDelay = int.Parse(doc.DocumentElement.Attributes["RepeatDelay"].Value);
        _serialPortName = doc.DocumentElement.Attributes["SerialPortName"].Value;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());

        _repeatDelay = 500;
        _serialPortName = "COM1";
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
        writer.WriteAttributeString("SerialPortName", _serialPortName);

        writer.WriteEndElement(); // </settings>
        writer.WriteEndDocument();
        writer.Close();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }
    
    #endregion Implementation

  }

}
