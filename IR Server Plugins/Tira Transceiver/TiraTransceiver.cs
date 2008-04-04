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
  /// IR Server Plugin for Tira Transceiver device.
  /// </summary>
  [CLSCompliant(false)]
  public class TiraTransceiver : PluginBase, IConfigure, ITransmitIR, ILearnIR, IRemoteReceiver
  {

    #region Constants

    static readonly string ConfigurationFile = Path.Combine(ConfigurationPath, "Tira Transceiver.xml");

    static readonly string[] Ports = new string[] { "Default" };

    #endregion Constants

    #region Interop

    //delegate void TiraCallback(StringBuilder code);

    [DllImport("Tira2.dll")]
    static extern int tira_init();

    [DllImport("Tira2.dll")]
    static extern int tira_cleanup();

    [DllImport("Tira2.dll")]
    static extern int tira_start(int PortID);

    [DllImport("Tira2.dll")]
    static extern int tira_stop();

    //[DllImport("Tira2.dll")]
    //static extern int tira_set_handler(TiraCallback callback);

    [DllImport("Tira2.dll")]
    static extern int tira_get_ir_data(out string IRDataString, IntPtr DataSize);

    [DllImport("Tira2.dll")]
    static extern int tira_start_capture();

    //[DllImport("Tira2.dll")]
    //static extern int tira_get_captured_data(IntPtr Data, IntPtr Size);
    
    [DllImport("Tira2.dll")]
    static extern int tira_get_captured_data_vb(out string Data, IntPtr Size);
    
    [DllImport("Tira2.dll")]
    static extern int tira_cancel_capture();

    [DllImport("Tira2.dll")]
    static extern int tira_delete(IntPtr Data);

    [DllImport("Tira2.dll")]
    static extern int tira_access_feature(int FeatureID, bool Write, IntPtr Value, int Mask);

    [DllImport("Tira2.dll")]
    static extern int tira_transmit(int Repeat, int Frequency, string IRData, int DataSize);

    #endregion Interop

    #region Variables

    int _port;

    RemoteHandler _remoteButtonHandler;


    #endregion Variables

    #region Implementation

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name         { get { return "Tira"; } }
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
    public override string Description  { get { return "Support for the Tira transceiver"; } }
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
        if (tira_init() != 0)
          return false;

        if (tira_cleanup() == 0)
          return true;
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

      if (tira_init() != 0)
        throw new ApplicationException("Failed to start Tira interface");

      if (tira_start(_port) != 0)
        throw new ApplicationException("Failed to start Tira device");
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
      if (tira_cleanup() != 0)
        throw new ApplicationException("Failed to close Tira interface");
    }

    /// <summary>
    /// Configure the IR Server plugin.
    /// </summary>
    public void Configure(IWin32Window owner)
    {
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

      return LearnStatus.Failure;
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

    #endregion Implementation


  }

}
