using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Win32.SafeHandles;

using IRServerPluginInterface;

namespace MceReplacementTransceiver
{

  public class MceReplacementTransceiver : NativeWindow, IIRServerPlugin
  {

    #region Constants

    public static readonly string ConfigurationFile =
      Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
      "\\IR Server Suite\\IR Server\\MCE Replacement Transceiver.xml";

    const int WM_USER           = 0x0400;
    const int ID_MCEIR_KEYCODE  = 0x37FF0;

    #endregion Constants

    #region Variables

    RemoteButtonHandler _remoteButtonHandler = null;

    MceIrApi.BlasterType  _blasterType  = MceIrApi.BlasterType.Microsoft;
    MceIrApi.BlasterSpeed _blasterSpeed = MceIrApi.BlasterSpeed.None;
    MceIrApi.BlasterPort  _blasterPort  = MceIrApi.BlasterPort.Both;

    int _repeatDelay;
    int _heldDelay;

    #endregion Variables

    #region IIRServerPlugin Members

    public string Name          { get { return "MCE Replacement"; } }
    public string Version       { get { return "1.0.3.2"; } }
    public string Author        { get { return "and-81"; } }
    public string Description   { get { return "Supports the MCE Replacement Driver for accessing the Microsoft MCE transceiver"; } }
    public bool   CanReceive    { get { return true; } }
    public bool   CanTransmit   { get { return true; } }
    public bool   CanLearn      { get { return true; } }
    public bool   CanConfigure  { get { return true; } }

    public RemoteButtonHandler RemoteButtonCallback
    {
      get { return _remoteButtonHandler; }
      set { _remoteButtonHandler = value; }
    }

    public string[] AvailablePorts
    {
      get { return Enum.GetNames(typeof(MceIrApi.BlasterPort)); }
    }
    public string[] AvailableSpeeds
    {
      get { return Enum.GetNames(typeof(MceIrApi.BlasterSpeed)); }
    }

    public void Configure()
    {
      LoadSettings();

      Configure config = new Configure();

      config.BlastType    = _blasterType;
      config.RepeatDelay  = _repeatDelay;
      config.HeldDelay    = _heldDelay;

      if (config.ShowDialog() == DialogResult.OK)
      {
        _blasterType  = config.BlastType;
        _repeatDelay  = config.RepeatDelay;
        _heldDelay    = config.HeldDelay;

        SaveSettings();
      }
    }
    public bool Start()
    {
      LoadSettings();

      CreateParams createParams = new CreateParams();
      createParams.Caption = "MCE Replacement Transceiver";
      createParams.ExStyle = 0x80;
      createParams.Style = unchecked((int)0x80000000);
      CreateHandle(createParams);

      HandleRef handleRef = new HandleRef(this, this.Handle);

      if (MceIrApi.RegisterEvents(handleRef))
      {
        MceIrApi.Resume();

        MceIrApi.SetBlasterType(_blasterType);
        MceIrApi.SetRepeatTimes(_repeatDelay, _heldDelay);

        return true;
      }

      return false;
    }
    public void Suspend()
    {
      MceIrApi.Suspend();
    }
    public void Resume()
    {
      MceIrApi.Resume();
    }
    public void Stop()
    {
      if (MceIrApi.UnregisterEvents())
        MceIrApi.Suspend();
    }

    public bool Transmit(string file)
    {
      bool result = false;

      try
      {
        FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);

        result = MceIrApi.CheckFile(fileStream.SafeFileHandle);

        if (result)
        {
          MceIrApi.SelectBlaster(_blasterPort);
          MceIrApi.SetBlasterSpeed(_blasterSpeed);

          result = MceIrApi.PlaybackFromFile(fileStream.SafeFileHandle);
        }
        
        fileStream.Close();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());

        result = false;
      }

      return result;    
    }
    public LearnStatus Learn(string file)
    {
      FileStream fileStream = new FileStream(file, FileMode.Create);

      DateTime start = DateTime.Now;

      // TODO: Implement proper timeout ...
      int timeout = 8000;

      bool result = MceIrApi.RecordToFile(fileStream.SafeFileHandle, timeout);

      fileStream.Close();

      TimeSpan timeTaken = start.Subtract(DateTime.Now);

      if (timeTaken.Milliseconds >= timeout)
        return LearnStatus.Timeout;
      else if (result)
        return LearnStatus.Success;
      else
        return LearnStatus.Failure;
    }

    public bool SetPort(string port)
    {
      try
      {
        _blasterPort = (MceIrApi.BlasterPort)Enum.Parse(typeof(MceIrApi.BlasterPort), port);
      }
      catch
      {
        return false;
      }

      return true;
    }
    public bool SetSpeed(string speed)
    {
      try
      {
        _blasterSpeed = (MceIrApi.BlasterSpeed)Enum.Parse(typeof(MceIrApi.BlasterSpeed), speed);
      }
      catch
      {
        return false;
      }

      return true;
    }

    #endregion IIRServerPlugin Members

    #region Implementation

    void LoadSettings()
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationFile);

        _blasterType  = (MceIrApi.BlasterType)Enum.Parse(typeof(MceIrApi.BlasterType), doc.DocumentElement.Attributes["BlastType"].Value);
        _repeatDelay  = int.Parse(doc.DocumentElement.Attributes["RepeatDelay"].Value);
        _heldDelay    = int.Parse(doc.DocumentElement.Attributes["HeldDelay"].Value);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        
        _blasterType  = MceIrApi.BlasterType.Microsoft;
        _repeatDelay  = 500;
        _heldDelay    = 250;
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

        writer.WriteAttributeString("BlastType", Enum.GetName(typeof(MceIrApi.BlasterType), _blasterType));
        writer.WriteAttributeString("RepeatDelay", _repeatDelay.ToString());
        writer.WriteAttributeString("HeldDelay", _heldDelay.ToString());

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
      if (m.Msg == WM_USER && m.WParam.ToInt32() == ID_MCEIR_KEYCODE)
      {
        int keyCode = m.LParam.ToInt32() & 0xFFFF;

        if (_remoteButtonHandler != null)
          _remoteButtonHandler(keyCode.ToString());
      }

      base.WndProc(ref m);
    }

    #endregion Implementation

  }

}
