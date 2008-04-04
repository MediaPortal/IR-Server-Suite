using System;
#if TRACE
using System.Diagnostics;
#endif
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace InputService.Plugin
{

  /// <summary>
  /// IR Server Plugin for the receiving speech commands.
  /// </summary>
  public class SpeechReceiver : PluginBase, IRemoteReceiver, IConfigure
  {

    #region Constants

    static readonly string ConfigurationFile =
      Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
      "\\IR Server Suite\\Input Service\\Speech Receiver.xml";

    #endregion Constants

    #region Variables

    RemoteHandler _remoteButtonHandler;


    #endregion Variables

    #region Implementation

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name         { get { return "Speech Receiver"; } }
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
    public override string Description  { get { return "Support using voice commands to control IR Server Suite"; } }
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

    }
    /// <summary>
    /// Suspend the IR Server plugin when computer enters standby.
    /// </summary>
    public override void Suspend()
    {
    }
    /// <summary>
    /// Resume the IR Server plugin when the computer returns from standby.
    /// </summary>
    public override void Resume()
    {
    }
    /// <summary>
    /// Stop the IR Server plugin.
    /// </summary>
    public override void Stop()
    {

    }

    /// <summary>
    /// Configure the IR Server plugin.
    /// </summary>
    /// <param name="owner">The owner window to use for creating modal dialogs.</param>
    public void Configure(IWin32Window owner)
    {/*
      LoadSettings();

      Setup setup = new Setup();

      setup.HandleMouseLocal  = _handleMouseLocally;
      setup.UseNunchukAsMouse = _useNunchukAsMouse;
      setup.MouseSensitivity  = _mouseSensitivity;
      setup.LED1 = _led1;
      setup.LED2 = _led2;
      setup.LED3 = _led3;
      setup.LED4 = _led4;

      if (setup.ShowDialog(owner) == DialogResult.OK)
      {
        _handleMouseLocally   = setup.HandleMouseLocal;
        _useNunchukAsMouse    = setup.UseNunchukAsMouse;
        _mouseSensitivity     = setup.MouseSensitivity;
        _led1 = setup.LED1;
        _led2 = setup.LED2;
        _led3 = setup.LED3;
        _led4 = setup.LED4;

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

    void LoadSettings()
    {/*
      XmlDocument doc = new XmlDocument();

      try { doc.Load(ConfigurationFile); }
      catch { return; }

      try { _handleMouseLocally = bool.Parse(doc.DocumentElement.Attributes["HandleMouseLocally"].Value); }
      catch { }

      try { _useNunchukAsMouse = bool.Parse(doc.DocumentElement.Attributes["UseNunchukAsMouse"].Value); }
      catch { }

      try { _mouseSensitivity = double.Parse(doc.DocumentElement.Attributes["MouseSensitivity"].Value); }
      catch { }

      try { _led1 = bool.Parse(doc.DocumentElement.Attributes["LED1"].Value); }
      catch { }
      try { _led2 = bool.Parse(doc.DocumentElement.Attributes["LED2"].Value); }
      catch { }
      try { _led3 = bool.Parse(doc.DocumentElement.Attributes["LED3"].Value); }
      catch { }
      try { _led4 = bool.Parse(doc.DocumentElement.Attributes["LED4"].Value); }
      catch { }*/
    }
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

          writer.WriteAttributeString("HandleMouseLocally", _handleMouseLocally.ToString());
          writer.WriteAttributeString("UseNunchukAsMouse", _useNunchukAsMouse.ToString());
          writer.WriteAttributeString("MouseSensitivity", _mouseSensitivity.ToString());

          writer.WriteAttributeString("LED1", _led1.ToString());
          writer.WriteAttributeString("LED2", _led2.ToString());
          writer.WriteAttributeString("LED3", _led3.ToString());
          writer.WriteAttributeString("LED4", _led4.ToString());

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

    #endregion Implementation

  }

}
