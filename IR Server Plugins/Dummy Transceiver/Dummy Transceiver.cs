using System;

using IRServerPluginInterface;

namespace DummyTransceiver
{

  public class DummyTransceiver : IIRServerPlugin
  {

    #region Constants

    static readonly string[] Ports  = new string[] { "None" };
    static readonly string[] Speeds = new string[] { "None" };

    #endregion Constants

    #region Variables

    RemoteButtonHandler _remoteButtonHandler = null;

    #endregion Variables

    #region IIRServerPlugin Members

    public string Name          { get { return "Dummy Transceiver"; } }
    public string Version       { get { return "1.0.3.1"; } }
    public string Author        { get { return "and-81"; } }
    public string Description   { get { return "For use without an IR device attached"; } }
    public bool   CanReceive    { get { return true; } }
    public bool   CanTransmit   { get { return true; } }
    public bool   CanLearn      { get { return true; } }
    public bool   CanConfigure  { get { return false; } }

    public RemoteButtonHandler RemoteButtonCallback
    {
      get { return _remoteButtonHandler; }
      set { _remoteButtonHandler = value; }
    }

    public string[] AvailablePorts  { get { return Ports; }   }
    public string[] AvailableSpeeds { get { return Speeds; }  }

    public void Configure() { }
    public bool Start()     { return true; }
    public void Suspend()   { }
    public void Resume()    { }
    public void Stop()      { }

    public bool Transmit(string file)     { return false; }
    public LearnStatus Learn(string file) { return LearnStatus.Failure; }

    public bool SetPort(string port)    { return true; }
    public bool SetSpeed(string speed)  { return true; }

    #endregion IIRServerPlugin Members

  }

}
