using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Win32.SafeHandles;

using IRServerPluginInterface;

namespace MceReplacementTransceiverV2
{

  public class MceReplacementTransceiverV2 : IIRServerPlugin
  {

    #region Constants

    public static readonly string ConfigurationFile =
      Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
      "\\IR Server Suite\\IR Server\\MCE Replacement Transceiver.xml";

    #endregion Constants

    #region Variables

    RemoteButtonHandler _remoteButtonHandler = null;

    MceIrApi.BlasterType  _blasterType  = MceIrApi.BlasterType.Microsoft;
    MceIrApi.BlasterSpeed _blasterSpeed = MceIrApi.BlasterSpeed.None;
    MceIrApi.BlasterPort  _blasterPort  = MceIrApi.BlasterPort.Both;

    int _repeatDelay;
    int _heldDelay;
    int _learnTimeout;

    #endregion Variables

    #region IIRServerPlugin Members

    public string Name          { get { return "MCE Replacement v2"; } }
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
      config.LearnTimeout = _learnTimeout;

      if (config.ShowDialog() == DialogResult.OK)
      {
        _blasterType  = config.BlastType;
        _repeatDelay  = config.RepeatDelay;
        _heldDelay    = config.HeldDelay;
        _learnTimeout = config.LearnTimeout;

        SaveSettings();
      }
    }
    public bool Start()
    {
      LoadSettings();

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

      bool result = MceIrApi.RecordToFile(fileStream.SafeFileHandle, _learnTimeout);

      fileStream.Close();

      TimeSpan timeTaken = start.Subtract(DateTime.Now);

      if (result)
        return LearnStatus.Success;
      else if (timeTaken.Milliseconds >= _learnTimeout)
        return LearnStatus.Timeout;
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
        _learnTimeout = int.Parse(doc.DocumentElement.Attributes["LearnTimeout"].Value);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        
        _blasterType  = MceIrApi.BlasterType.Microsoft;
        _repeatDelay  = 500;
        _heldDelay    = 250;
        _learnTimeout = 8000;
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

    void KeyPressCallBack(string keyCode)
    {
      if (_remoteButtonHandler != null)
        _remoteButtonHandler(keyCode);
    }

    #endregion Implementation

  }

}
