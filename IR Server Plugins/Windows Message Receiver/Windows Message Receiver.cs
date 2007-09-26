using System;
using System.Diagnostics;
using System.IO;
//using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;

using IRServerPluginInterface;

namespace WindowsMessageReceiver
{

  public class WindowsMessageReceiver : IRServerPlugin, IConfigure, IRemoteReceiver
  {
    
    // TODO: Add Learn/Blast ability
    /*
    #region Interop

    [DllImport("user32")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32", SetLastError = false)]
    public static extern IntPtr SendMessage(IntPtr windowHandle, int msg, IntPtr wordParam, IntPtr longParam);

    #endregion Interop
    */

    #region Constants

    static readonly string ConfigurationFile =
      Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
      "\\IR Server Suite\\IR Server\\Windows Messages.xml";

    const int WM_APP            = 0x8000;
    const int DefaultMessageID  = 0x0018;

    internal static readonly string WindowTitle = "Windows Messages Receiver for IR Server";

    #endregion Constants

    #region Variables

    int _messageType  = WM_APP;
    int _wParam       = DefaultMessageID;

    RemoteHandler _remoteButtonHandler = null;

    ReceiverWindow _receiverWindow = null;

    #endregion Variables

    #region Implementation

    public override string Name         { get { return "Windows Messages"; } }
    public override string Version      { get { return "1.0.3.4"; } }
    public override string Author       { get { return "and-81"; } }
    public override string Description  { get { return "Supports receiving simulated button presses through Windows Messages"; } }

    public override bool Start()
    {
      LoadSettings();

      _receiverWindow = new ReceiverWindow(WindowTitle);
      _receiverWindow.ProcMsg += new ProcessMessage(ProcMsg);

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
      _receiverWindow.ProcMsg -= new ProcessMessage(ProcMsg);
      _receiverWindow.ReleaseHandle();
      _receiverWindow = null;
    }

    public void Configure()
    {
      LoadSettings();

      Configure config = new Configure();

      config.MessageType  = _messageType;
      config.WParam       = _wParam;

      if (config.ShowDialog() == DialogResult.OK)
      {
        _messageType      = config.MessageType;
        _wParam           = config.WParam;

        SaveSettings();
      }
    }

    public RemoteHandler RemoteCallback
    {
      get { return _remoteButtonHandler; }
      set { _remoteButtonHandler = value; }
    }

    void LoadSettings()
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationFile);

        _messageType  = int.Parse(doc.DocumentElement.Attributes["MessageType"].Value);
        _wParam       = int.Parse(doc.DocumentElement.Attributes["WParam"].Value);
      }
      catch (Exception ex)
      {
        Trace.WriteLine(ex.ToString());

        _messageType  = WM_APP;
        _wParam       = DefaultMessageID;
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

          writer.WriteAttributeString("MessageType", _messageType.ToString());
          writer.WriteAttributeString("WParam", _wParam.ToString());

          writer.WriteEndElement(); // </settings>
          writer.WriteEndDocument();
        }
      }
      catch (Exception ex)
      {
        Trace.WriteLine(ex.ToString());
      }
    }

    void ProcMsg(ref Message m)
    {
      if (m.Msg == _messageType && m.WParam.ToInt32() == _wParam)
      {
        int longParam = m.LParam.ToInt32();
        if (_remoteButtonHandler != null)
          _remoteButtonHandler(longParam.ToString());
      }
    }

    #endregion Implementation

  }

}
