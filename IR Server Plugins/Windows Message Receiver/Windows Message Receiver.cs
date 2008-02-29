using System;
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
  /// IR Server Plugin for receiving Windows Messages.
  /// </summary>
  public class WindowsMessageReceiver : PluginBase, IConfigure, IRemoteReceiver
  {
    
    // TODO: Add Learn/Blast ability
    /*
    #region Interop

    [DllImport("user32")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32")]
    public static extern IntPtr SendMessage(IntPtr windowHandle, int msg, IntPtr wordParam, IntPtr longParam);

    #endregion Interop
    */

    #region Constants

    static readonly string ConfigurationFile = Path.Combine(ConfigurationPath, "Windows Messages.xml");

    const int WM_APP            = 0x8000;
    const int DefaultMessageID  = 0x0018;

    internal const string WindowTitle = "WM Receiver for IR Server";

    #endregion Constants

    #region Variables

    int _messageType  = WM_APP;
    int _wParam       = DefaultMessageID;

    RemoteHandler _remoteButtonHandler;

    ReceiverWindow _receiverWindow;

    #endregion Variables

    #region Implementation

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name         { get { return "Windows Messages"; } }
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
    public override string Description  { get { return "Supports receiving simulated button presses through Windows Messages"; } }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    public override void Start()
    {
      LoadSettings();

      _receiverWindow = new ReceiverWindow(WindowTitle);
      _receiverWindow.ProcMsg += new ProcessMessage(ProcMsg);
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
      _receiverWindow.ProcMsg -= new ProcessMessage(ProcMsg);
      _receiverWindow.ReleaseHandle();
      _receiverWindow = null;
    }

    /// <summary>
    /// Configure the IR Server plugin.
    /// </summary>
    public void Configure(IWin32Window owner)
    {
      LoadSettings();

      Configure config = new Configure();

      config.MessageType  = _messageType;
      config.WParam       = _wParam;

      if (config.ShowDialog(owner) == DialogResult.OK)
      {
        _messageType      = config.MessageType;
        _wParam           = config.WParam;

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
    /// Loads the settings.
    /// </summary>
    void LoadSettings()
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationFile);

        _messageType  = int.Parse(doc.DocumentElement.Attributes["MessageType"].Value);
        _wParam       = int.Parse(doc.DocumentElement.Attributes["WParam"].Value);
      }
#if TRACE
      catch (Exception ex)
      {
        Trace.WriteLine(ex.ToString());
#else
      catch
      {
#endif

        _messageType  = WM_APP;
        _wParam       = DefaultMessageID;
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

          writer.WriteAttributeString("MessageType", _messageType.ToString());
          writer.WriteAttributeString("WParam", _wParam.ToString());

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
    /// Proccesses the incoming Windows Message.
    /// </summary>
    /// <param name="m">The message.</param>
    void ProcMsg(ref Message m)
    {
      if (m.Msg == _messageType && m.WParam.ToInt32() == _wParam)
      {
        int longParam = m.LParam.ToInt32();
        if (_remoteButtonHandler != null)
          _remoteButtonHandler(this.Name, longParam.ToString());
      }
    }

    #endregion Implementation

  }

}
