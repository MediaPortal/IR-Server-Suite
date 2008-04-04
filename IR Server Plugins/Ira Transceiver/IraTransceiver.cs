using System;
using System.Collections.Generic;
#if TRACE
using System.Diagnostics;
#endif
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace InputService.Plugin
{

  /// <summary>
  /// Ira/Tira device type.
  /// </summary>
  internal enum DeviceType
  {
    /// <summary>
    /// Ira.
    /// </summary>
    Ira,
    /// <summary>
    /// Ira-2.
    /// </summary>
    Ira2,
    /// <summary>
    /// Tira.
    /// </summary>
    Tira,
  }

  /// <summary>
  /// IR Server Plugin for Ira Transceiver device.
  /// </summary>
  [CLSCompliant(false)]
  public class IraTransceiver : PluginBase, IConfigure, ITransmitIR, ILearnIR, IRemoteReceiver
  {

    #region Constants

    static readonly string ConfigurationFile = Path.Combine(ConfigurationPath, "Ira Transceiver.xml");

    static readonly string[] Ports = new string[] { "Default" };

    const int DeviceBufferSize = 6;

    #endregion Constants

    #region Variables

    //DeviceType _deviceType;

    //string _port;


    RemoteHandler _remoteButtonHandler;

    SerialPort _serialPort;

    #endregion Variables

    #region Implementation

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name         { get { return "Ira"; } }
    /// <summary>
    /// IR Server plugin version.
    /// </summary>
    /// <value>The version.</value>
    public override string Version      { get { return "1.4.2.0"; } }
    /// <summary>
    /// The IR Server plugin's author.
    /// </summary>
    /// <value>The author.</value>
    public override string Author       { get { return "and-81"; } }
    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    /// <value>The description.</value>
    public override string Description  { get { return "Support for the Ira/Tira transceiver"; } }
    /// <summary>
    /// Gets the plugin icon.
    /// </summary>
    /// <value>The plugin icon.</value>
    public override Icon DeviceIcon     { get { return Properties.Resources.Icon; } }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    public override void Start()
    {
      LoadSettings();

      Connect(9600);

      _serialPort.Write("IR");
    }
    /// <summary>
    /// Suspend the IR Server plugin when computer enters standby.
    /// </summary>
    public override void Suspend()
    {
      /*if (_deviceType == DeviceType.Tira)
      {
        byte[] suspendCommand = new byte[] { 49, 57, 0, 0, 0, 0, 0, 0 };
        Array.Copy(_suspendCommand, 0, suspendCommand, 2, _suspendCommand.Length);

        _serialPort.Write(suspendCommand);
      }*/

      Disconnect();
    }
    /// <summary>
    /// Resume the IR Server plugin when the computer returns from standby.
    /// </summary>
    public override void Resume()
    {
      Connect(9600);

      _serialPort.Write("IR");
    }
    /// <summary>
    /// Stop the IR Server plugin.
    /// </summary>
    public override void Stop()
    {
      Disconnect();

      _serialPort.Dispose();
      _serialPort = null;
    }

    /// <summary>
    /// Configure the IR Server plugin.
    /// </summary>
    public void Configure(IWin32Window owner)
    {/*
      LoadSettings();

      Configure config = new Configure();

      config.RepeatDelay = _repeatDelay;
      config.BlastRepeats = _blastRepeats;
      config.LearnTimeout = _learnTimeout;

      if (config.ShowDialog(owner) == DialogResult.OK)
      {
        _repeatDelay = config.RepeatDelay;
        _blastRepeats = config.BlastRepeats;
        _learnTimeout = config.LearnTimeout;

        SaveSettings();
      }*/
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
      if (_deviceType != DeviceType.Tira)
        throw new ApplicationException("Ira and Ira-2 do not support IR Blasting");

      if (String.IsNullOrEmpty(port))
        throw new ArgumentNullException("port");

      if (data == null)
        throw new ArgumentNullException("data");

      bool result = false;




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
      LearnStatus status = LearnStatus.Failure;

      data = null;

      if (_deviceType != DeviceType.Tira)
        throw new ApplicationException("Ira and Ira-2 do not support IR Learn");

      Disconnect();
      Connect(57600);
      /*
      if (_learnTimedOut)
      {
        status = LearnStatus.Timeout;
      }
      else if (result)
      {
        data = Encoding.ASCII.GetBytes(irCode.ToString());

        status = LearnStatus.Success;
      }
      else
      {
        status = LearnStatus.Failure;
      }
      */
      Disconnect();
      Connect(9600);

      return status;
    }

    /// <summary>
    /// Loads the settings.
    /// </summary>
    void LoadSettings()
    {/*
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationFile);

        _repeatDelay = int.Parse(doc.DocumentElement.Attributes["RepeatDelay"].Value);
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

        _repeatDelay = 500;
        _blastRepeats = 3;
        _learnTimeout = 10000;
      }*/
    }
    /// <summary>
    /// Saves the settings.
    /// </summary>
    void SaveSettings()
    {/*
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
#endif*/
    }

    void Connect(int baud)
    {
      _serialPort = new SerialPort(_port, baud, Parity.None, 8, StopBits.One);
      //_serialPort.CtsHolding      = (_deviceType == DeviceType.Tira);
      _serialPort.RtsEnable       = (_deviceType == DeviceType.Tira);
      _serialPort.ReadBufferSize  = DeviceBufferSize;
      _serialPort.ReadTimeout     = 1000;

      _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceived);

      _serialPort.Open();
    }

    void Disconnect()
    {
      _serialPort.Close();
    }

    void DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
      string received = _serialPort.ReadExisting();

      switch (received)
      {
        case "OK":
#if TRACE
          Trace.WriteLine("Ira acknowledged mode set");
#endif    
          break;

        case "OIW":
#if TRACE
          Trace.WriteLine("Ira acknowledged Wake-Up command set");
#endif
          break;

        // Assume it is a button press.
        default:
          if (_remoteButtonHandler != null)
            _remoteButtonHandler(this.Name, received);
          break;
      }

    }

    #endregion Implementation

  }

}
