using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Win32.SafeHandles;

using IRServerPluginInterface;

namespace HcwTransceiver
{

  [CLSCompliant(false)]
  public class HcwTransceiver :
    IRServerPlugin, IConfigure, ITransmitIR, ILearnIR, IRemoteReceiver
  {

    #region Interop


    const int WM_ACTIVATE = 0x0006;
    const int WM_POWERBROADCAST = 0x0218;
    const int WA_INACTIVE = 0;
    const int WA_ACTIVE = 1;
    const int WA_CLICKACTIVE = 2;

    const int PBT_APMRESUMEAUTOMATIC = 0x0012;
    const int PBT_APMRESUMECRITICAL = 0x0006;



    #endregion

    #region Delegates

    //Sets up callback so that other forms can catch a key press
    public delegate void HCWEvent(int keypress);
    public event HCWEvent HCWKeyPressed;

    #endregion Delegates

    #region Constants

    static readonly string ConfigurationFile =
      Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
      "\\IR Server Suite\\IR Server\\USB-UIRT Transceiver.xml";

    const int UUIRTDRV_IRFMT_UUIRT             = 0x0000;
    const int UUIRTDRV_IRFMT_PRONTO            = 0x0010;
    const int UUIRTDRV_IRFMT_LEARN_FORCERAW    = 0x0100;
    const int UUIRTDRV_IRFMT_LEARN_FORCESTRUC  = 0x0200;
    const int UUIRTDRV_IRFMT_LEARN_FORCEFREQ   = 0x0400;
    const int UUIRTDRV_IRFMT_LEARN_FREQDETECT  = 0x0800;
    
    static readonly string[] Ports  = new string[] { "Default", "Port 1", "Port 2", "Port 3" };

    const int AbortLearn = -1;
    const int AllowLearn = 0;

    #endregion Constants

    #region Variables

    RemoteHandler _remoteButtonHandler = null;

    string _blastPort = Ports[0];

    int _repeatDelay;
    int _blastRepeats;
    int _learnTimeout;

    //ulong _learnCarrierFreq;

    string _lastCode        = String.Empty;
    DateTime _lastCodeTime  = DateTime.Now;

    // -------

    int _abortLearn = AllowLearn;
    bool _learnTimedOut;

    #endregion Variables

    #region Implementation

    public override string Name         { get { return "HCW Transceiver"; } }
    public override string Version      { get { return "1.0.3.4"; } }
    public override string Author       { get { return "and-81"; } }
    public override string Description  { get { return "Support for the HCW transceiver"; } }

    public override bool Start()
    {
      LoadSettings();
      
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

    }

    public void Configure()
    {
      LoadSettings();

      Configure config = new Configure();

      config.RepeatDelay  = _repeatDelay;
      config.BlastRepeats = _blastRepeats;
      config.LearnTimeout = _learnTimeout;

      if (config.ShowDialog() == DialogResult.OK)
      {
        _repeatDelay  = config.RepeatDelay;
        _blastRepeats = config.BlastRepeats;
        _learnTimeout = config.LearnTimeout;

        SaveSettings();
      }
    }

    public RemoteHandler RemoteCallback
    {
      get { return _remoteButtonHandler; }
      set { _remoteButtonHandler = value; }
    }

    public string[] AvailablePorts { get { return Ports; }   }

    public bool Transmit(string port, byte[] data)
    {

      return false;
    }
    public LearnStatus Learn(out byte[] data)
    {
      data = null;
      return LearnStatus.Failure;
    }

    void LoadSettings()
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationFile);

        _repeatDelay  = int.Parse(doc.DocumentElement.Attributes["RepeatDelay"].Value);
        _blastRepeats = int.Parse(doc.DocumentElement.Attributes["BlastRepeats"].Value);
        _learnTimeout = int.Parse(doc.DocumentElement.Attributes["LearnTimeout"].Value);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());

        _repeatDelay  = 500;
        _blastRepeats = 4;
        _learnTimeout = 10000;
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
        writer.WriteAttributeString("BlastRepeats", _blastRepeats.ToString());
        writer.WriteAttributeString("LearnTimeout", _learnTimeout.ToString());

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
