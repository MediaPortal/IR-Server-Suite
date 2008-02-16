using System;
using System.ComponentModel;
#if TRACE
using System.Diagnostics;
#endif
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace InputService.Plugin
{

  /// <summary>
  /// IR Server Plugin for using Girder 3.x plugins.
  /// </summary>
  public class GirderPlugin : PluginBase, IRemoteReceiver, ITransmitIR, IConfigure
  {

    #region Constants

    static readonly string ConfigurationFile =
      Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
      "\\IR Server Suite\\Input Service\\Girder Plugin.xml";

    static readonly string[] Ports  = new string[] { "Plugin" };

    const int GIRINFO_POWERBROADCAST = 2;
    const int PBT_APMSUSPEND = 4;
    const int PBT_APMRESUMEAUTOMATIC = 18;

    #endregion Constants

    #region Variables

    string _pluginFile;

    RemoteHandler _remoteButtonHandler;

    GirderPluginWrapper _pluginWrapper;

    #endregion Variables

    #region IRServerPluginBase Members

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name         { get { return "Girder Plugin"; } }
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
    public override string Description  { get { return "Supports using Girder 3.x plugins with IR Server"; } }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    public override void Start()
    {
      LoadSettings();

      _pluginWrapper = new GirderPluginWrapper(_pluginFile);

      _pluginWrapper.EventCallback += new PluginEventCallback(PluginCallback);

      if (!_pluginWrapper.GirOpen())
      {
        _pluginWrapper.Dispose();
        _pluginWrapper = null;

        throw new ApplicationException("Failed to initiate girder plugin");
      }

      if (!_pluginWrapper.GirStart())
      {
        _pluginWrapper.GirClose();
        _pluginWrapper.Dispose();
        _pluginWrapper = null;

        throw new ApplicationException("Failed to start girder plugin");
      }

    }
    /// <summary>
    /// Suspend the IR Server plugin when computer enters standby.
    /// </summary>
    public override void Suspend()
    {
      if (_pluginWrapper == null)
        return;

      _pluginWrapper.GirInfo(GIRINFO_POWERBROADCAST, PBT_APMSUSPEND, 0);
    }
    /// <summary>
    /// Resume the IR Server plugin when the computer returns from standby.
    /// </summary>
    public override void Resume()
    {
      if (_pluginWrapper == null)
        return;

      _pluginWrapper.GirInfo(GIRINFO_POWERBROADCAST, PBT_APMRESUMEAUTOMATIC, 0);
    }
    /// <summary>
    /// Stop the IR Server plugin.
    /// </summary>
    public override void Stop()
    {
      if (_pluginWrapper == null)
        return;

      _pluginWrapper.GirStop();

      _pluginWrapper.GirClose();

      _pluginWrapper.Dispose();

      _pluginWrapper = null;
    }

    #endregion IRServerPluginBase Members

    #region IRemoteReceiver Members

    /// <summary>
    /// Callback for remote button presses.
    /// </summary>
    /// <value>The remote callback.</value>
    public RemoteHandler RemoteCallback
    {
      get { return _remoteButtonHandler; }
      set { _remoteButtonHandler = value; }
    }

    #endregion IRemoteReceiver Members

    #region ITransmitIR Members

    /// <summary>
    /// Lists the available blaster ports.
    /// </summary>
    /// <value>The available ports.</value>
    public string[] AvailablePorts
    {
      get { return Ports; }
    }

    /// <summary>
    /// Transmit an infrared command.
    /// </summary>
    /// <param name="port">Port to transmit on.</param>
    /// <param name="data">Data to transmit.</param>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public bool Transmit(string port, byte[] data)
    {
      string xmlData = Encoding.ASCII.GetString(data);

      GirCommand blastCommand = GirCommand.FromXml(xmlData);

      return _pluginWrapper.GirEvent(blastCommand, String.Empty, IntPtr.Zero, 0, String.Empty, 0);
    }

    #endregion ITransmitIR Members

    #region IConfigure Members

    /// <summary>
    /// Configure the IR Server plugin.
    /// </summary>
    public void Configure(IWin32Window owner)
    {
      LoadSettings();

      Config config = new Config();
      config.PluginFolder = _pluginFile;

      if (config.ShowDialog(owner) == DialogResult.OK)
      {
        _pluginFile = config.PluginFolder;

        SaveSettings();
      }
    }

    #endregion IConfigure Members

    #region Implementation

    void PluginCallback(string eventstring, IntPtr payload, int len, int device)
    {
      if (_remoteButtonHandler != null)
        _remoteButtonHandler(eventstring);
    }

    void LoadSettings()
    {
      XmlDocument doc = new XmlDocument();

      try { doc.Load(ConfigurationFile); }
      catch { return; }

      try { _pluginFile = doc.DocumentElement.Attributes["PluginFile"].Value; }
      catch { }
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

          writer.WriteAttributeString("PluginFile", _pluginFile);

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
