using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

using IRServerPluginInterface;

using X10;

namespace X10Transceiver
{

  public class X10Transceiver : _DIX10InterfaceEvents, IIRServerPlugin
  {

    #region Constants

    static readonly string ConfigurationFile =
      Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
      "\\IR Server Suite\\IR Server\\X10 Receiver.xml";

    static readonly string[] Ports  = new string[] { "None" };
    static readonly string[] Speeds = new string[] { "None" };

    #endregion Constants

    #region Variables

    static RemoteButtonHandler _remoteButtonHandler = null;

    static string _blasterSpeed = Speeds[0];
    static string _blasterPort  = Ports[0];

    X10Interface X10Inter = null;
    IConnectionPointContainer icpc = null;
    IConnectionPoint icp = null;
    int cookie = 0;

    #endregion Variables
   
    #region IIRServerPlugin Members

    public string Name          { get { return "X10 (Experimental)"; } }
    public string Version       { get { return "1.0.3.1"; } }
    public string Author        { get { return "and-81"; } }
    public string Description   { get { return "X10 Transceiver (Experimental)"; } }
    public bool   CanReceive    { get { return true; } }
    public bool   CanTransmit   { get { return false; } }
    public bool   CanLearn      { get { return false; } }
    public bool   CanConfigure  { get { return false; } }

    public RemoteButtonHandler RemoteButtonCallback
    {
      get { return _remoteButtonHandler; }
      set { _remoteButtonHandler = value; }
    }

    public string[] AvailablePorts
    {
      get { return Ports; }
    }
    public string[] AvailableSpeeds
    {
      get { return Speeds; }
    }

    public void Configure() { }
    public bool Start()
    {
      //LoadSettings();

      try
      {
        if (X10Inter == null)
        {
          X10Inter = new X10Interface();
          if (X10Inter == null)
            throw new Exception("Failed to start X10 interface");

          // Bind the interface using a connection point
          icpc = (IConnectionPointContainer)X10Inter;
          Guid IID_InterfaceEvents = typeof(_DIX10InterfaceEvents).GUID;
          icpc.FindConnectionPoint(ref IID_InterfaceEvents, out icp);
          icp.Advise(this, out cookie);
        }

        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        return false;
      }
    }
    public void Suspend() { }
    public void Resume() { }
    public void Stop()
    {
      X10Inter = null;
    }

    public bool Transmit(string file)
    {
      return false;
    }
    public LearnStatus Learn(string file)
    {
      return LearnStatus.Failure;
    }

    public bool SetPort(string port)
    {
      return true;
    }
    public bool SetSpeed(string speed)
    {
      return true;
    }

    #endregion IIRServerPlugin Members

    #region _DIX10InterfaceEvents Members

    [CLSCompliant(false)]
    public void X10Command(string bszCommand, EX10Command eCommand, int lAddress, EX10Key EKeyState, int lSequence, EX10Comm eCommandType, object varTimestamp)
    {
      if ((EKeyState == X10.EX10Key.X10KEY_ON || EKeyState == X10.EX10Key.X10KEY_REPEAT) && lSequence != 2)
      {
        try
        {
          string keyCode = Enum.GetName(typeof(X10.EX10Command), eCommand);

          if (RemoteButtonCallback != null)
            RemoteButtonCallback(keyCode);
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.ToString());
        }
      }
    }

    public void X10HelpEvent(int hwndDialog, int lHelpID) { }

    #endregion

  }

}
