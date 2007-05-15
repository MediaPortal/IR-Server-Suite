using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;

using IRServerPluginInterface;

namespace WindowsMessageReceiver
{

  public class WindowsMessageReceiver : NativeWindow, IIRServerPlugin
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

    static readonly string[] Ports  = new string[] { "None" };
    static readonly string[] Speeds = new string[] { "None" };

    const int WM_APP            = 0x8000;
    const int DefaultMessageID  = 0x0018;

    internal static readonly string WindowTitle = "Windows Messages Receiver for IR Server";

    #endregion Constants

    #region Variables

    int _messageType  = WM_APP;
    int _wParam       = DefaultMessageID;

    RemoteButtonHandler _remoteButtonHandler = null;

    #endregion Variables

    #region IIRServerPlugin Members

    public string Name          { get { return "Windows Messages"; } }
    public string Version       { get { return "1.0.3.1"; } }
    public string Author        { get { return "and-81"; } }
    public string Description   { get { return "Supports receiving simulated button presses through Windows Messages"; } }
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

      config.MessageType  = _messageType;
      config.WParam       = _wParam;

      if (config.ShowDialog() == DialogResult.OK)
      {
        _messageType  = config.MessageType;
        _wParam       = config.WParam;

        SaveSettings();
      }
    }
    public bool Start()
    {
      LoadSettings();

      CreateParams createParams = new CreateParams();
      createParams.Caption = WindowTitle;
      createParams.ExStyle = 0x80;
      createParams.Style = unchecked((int)0x80000000);
      CreateHandle(createParams);

      return true;
    }
    public void Suspend()   { }
    public void Resume()    { }
    public void Stop()      { }

    public bool Transmit(string file)
    {
      /*
      StreamReader reader = new StreamReader(file);

      int msg = int.Parse(reader.ReadLine());
      IntPtr wordParam = new IntPtr(int.Parse(reader.ReadLine()));
      IntPtr longParam = new IntPtr(int.Parse(reader.ReadLine()));

      reader.Close();

      IntPtr windowHandle = GetForegroundWindow();
      SendMessage(windowHandle, msg, wordParam, longParam);
      */
      return false;
    }
    public LearnStatus Learn(string file)
    {
      return LearnStatus.Failure;
    }

    public bool SetPort(string port)    { return false; }
    public bool SetSpeed(string speed)  { return false; }

    #endregion IIRServerPlugin Members

    #region Implementation

    void LoadSettings()
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationFile);

        _messageType = int.Parse(doc.DocumentElement.Attributes["MessageType"].Value);
        _wParam = int.Parse(doc.DocumentElement.Attributes["WParam"].Value);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());

        _messageType  = WM_APP;
        _wParam       = DefaultMessageID;
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

        writer.WriteAttributeString("MessageType", _messageType.ToString());
        writer.WriteAttributeString("WParam", _wParam.ToString());

        writer.WriteEndElement(); // </settings>
        writer.WriteEndDocument();
        writer.Close();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }

    protected override void WndProc(ref Message m)
    {
      if (m.Msg == _messageType && m.WParam.ToInt32() == _wParam)
      {
        int longParam = m.LParam.ToInt32();
        if (_remoteButtonHandler != null)
          _remoteButtonHandler(longParam.ToString());
      }
      
      base.WndProc(ref m);
    }

    #endregion Implementation

  }

}
