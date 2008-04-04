using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Win32.SafeHandles;

namespace InputService.Plugin
{

  /// <summary>
  /// IR Server plugin supporting Hauppauge devices.
  /// </summary>
  [CLSCompliant(false)]
  public class HcwReceiver : PluginBase, IRemoteReceiver, IConfigure
  {

    #region Constants

    static readonly string ConfigurationFile = Path.Combine(ConfigurationPath, "HCW Receiver.xml");

    #endregion Constants

    #region Variables

    int _repeatDelay;
    bool _stopIrExe;
    bool _startIrExe;

    IrRemoteWrapper _irRemoteWrapper    = null;
    RemoteHandler _remoteButtonHandler  = null;
    
    int _lastCode           = 0;
    DateTime _lastCodeTime  = DateTime.Now;

    #endregion Variables

    #region Implementation

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name         { get { return "Hauppauge"; } }
    /// <summary>
    /// IR Server plugin version.
    /// </summary>
    /// <value>The version.</value>
    public override string Version      { get { return "1.4.2.0"; } }
    /// <summary>
    /// The IR Server plugin's author.
    /// </summary>
    /// <value>The author.</value>
    public override string Author       { get { return "and-81, original MediaPortal code by mPod"; } }
    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    /// <value>The description.</value>
    public override string Description  { get { return "Support for Hauppauge IR devices"; } }
    /// <summary>
    /// Gets a display icon for the plugin.
    /// </summary>
    /// <value>The icon.</value>
    public override Icon DeviceIcon     { get { return Properties.Resources.Icon; } }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    public override void Start()
    {
      LoadSettings();

      _irRemoteWrapper = new IrRemoteWrapper();
      _irRemoteWrapper.ButtonCallback = new ButtonReceived(ButtonCallback);

      if (_stopIrExe)
        _irRemoteWrapper.StopIrExe();

      _irRemoteWrapper.Start();
    }
    /// <summary>
    /// Suspend the IR Server plugin when computer enters standby.
    /// </summary>
    public override void Suspend()
    {
      //_irRemoteWrapper.Stop();
    }
    /// <summary>
    /// Resume the IR Server plugin when the computer returns from standby.
    /// </summary>
    public override void Resume()
    {
      //_irRemoteWrapper.Start();
    }
    /// <summary>
    /// Stop the IR Server plugin.
    /// </summary>
    public override void Stop()
    {
      _irRemoteWrapper.Stop();

      if (_startIrExe)
        _irRemoteWrapper.StartIrExe();
    }
    

    /// <summary>
    /// Configure the IR Server plugin.
    /// </summary>
    /// <param name="owner">The owner window to use for creating modal dialogs.</param>
    public void Configure(IWin32Window owner)
    {
      LoadSettings();

      Configure config = new Configure();

      config.RepeatDelay  = _repeatDelay;

      if (config.ShowDialog(owner) == DialogResult.OK)
      {
        _repeatDelay  = config.RepeatDelay;

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


    void ButtonCallback(int button)
    {
      DateTime now = DateTime.Now;
      TimeSpan timeSpan = now - _lastCodeTime;

      if (_lastCode != button || timeSpan.Milliseconds >= _repeatDelay)
      {
        if (_remoteButtonHandler != null)
          _remoteButtonHandler(this.Name, button.ToString());

        _lastCodeTime = now;
      }

      _lastCode = button;
    }


    void LoadSettings()
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationFile);

        _repeatDelay  = int.Parse(doc.DocumentElement.Attributes["RepeatDelay"].Value);
        _stopIrExe    = bool.Parse(doc.DocumentElement.Attributes["StopIrExe"].Value);
        _startIrExe   = bool.Parse(doc.DocumentElement.Attributes["StartIrExe"].Value);
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
        _stopIrExe    = true;
        _startIrExe   = true;
      }
    }
    void SaveSettings()
    {
      try
      {
        XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, Encoding.UTF8);
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 1;
        writer.IndentChar = (char)9;
        writer.WriteStartDocument(true);
        writer.WriteStartElement("settings"); // <settings>

        writer.WriteAttributeString("RepeatDelay", _repeatDelay.ToString());
        writer.WriteAttributeString("StopIrExe", _stopIrExe.ToString());
        writer.WriteAttributeString("StartIrExe", _startIrExe.ToString());

        writer.WriteEndElement(); // </settings>
        writer.WriteEndDocument();
        writer.Close();
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
