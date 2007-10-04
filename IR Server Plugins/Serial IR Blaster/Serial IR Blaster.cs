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

namespace SerialIRBlaster
{

  public class SerialIRBlaster : IRServerPluginBase, IConfigure, ITransmitIR
  {

    #region Constants

    static readonly string ConfigurationFile =
      Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
      "\\IR Server Suite\\IR Server\\Serial IR Blaster.xml";

    static readonly string[] Ports = new string[] { "Default" };

    #endregion Constants

    #region Variables

    SerialPort _serialPort;

    string _serialPortName;

    bool _disposed = false;

    #endregion Variables

    #region Implementation

    public override string Name         { get { return "Serial IR Blaster"; } }
    public override string Version      { get { return "1.0.3.4"; } }
    public override string Author       { get { return "and-81"; } }
    public override string Description  { get { return "Support for the Serial IR Blaster device"; } }

    public override bool Detect()
    {
      return false;
    }

    public override bool Start()
    {
      LoadSettings();

      _serialPort           = new SerialPort(_serialPortName, 19200, Parity.None, 8, StopBits.One);
      _serialPort.Handshake = Handshake.None;
      _serialPort.DtrEnable = true;
      _serialPort.RtsEnable = true;

      _serialPort.Open();

      return true;
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

    public void Configure()
    {
      LoadSettings();

      Configure config = new Configure();

      config.CommPort     = _serialPortName;

      if (config.ShowDialog() == DialogResult.OK)
      {
        _serialPortName   = config.CommPort;

        SaveSettings();
      }
    }
    
    public string[] AvailablePorts
    {
      get { return Ports; }
    }

    public bool Transmit(string port, byte[] data)
    {
      if (_serialPort == null)
        return false;

      _serialPort.Write(data, 0, data.Length);
      
      return true;
    }

    protected virtual void Dispose(bool disposeManagedResources)
    {
      // process only if mananged and unmanaged resources have
      // not been disposed of.
      if (!_disposed)
      {
        if (disposeManagedResources)
        {
          // dispose managed resources
          Stop();
        }

        // dispose unmanaged resources
        _disposed = true;
      }
    }

    void LoadSettings()
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationFile);

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

        _serialPortName = "COM1";
      }
    }
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
