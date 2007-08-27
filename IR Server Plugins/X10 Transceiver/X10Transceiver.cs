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

  public class X10Transceiver : IRServerPlugin, IRemoteReceiver, _DIX10InterfaceEvents
  {

    #region Constants

    static readonly string ConfigurationFile =
      Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
      "\\IR Server Suite\\IR Server\\X10 Receiver.xml";

    #endregion Constants

    #region Variables

    static RemoteHandler _remoteButtonHandler = null;

    X10Interface X10Inter = null;
    IConnectionPointContainer icpc = null;
    IConnectionPoint icp = null;
    int cookie = 0;

    #endregion Variables
   
    #region IIRServerPlugin Members

    public override string Name         { get { return "X10 (Experimental)"; } }
    public override string Version      { get { return "1.0.3.4"; } }
    public override string Author       { get { return "and-81"; } }
    public override string Description  { get { return "X10 Transceiver (Experimental)"; } }

    public override bool Start()
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
      X10Inter = null;
    }

    public RemoteHandler RemoteCallback
    {
      get { return _remoteButtonHandler; }
      set { _remoteButtonHandler = value; }
    }

    #endregion Implementation

    #region _DIX10InterfaceEvents Members

    [CLSCompliant(false)]
    public void X10Command(string bszCommand, EX10Command eCommand, int lAddress, EX10Key EKeyState, int lSequence, EX10Comm eCommandType, object varTimestamp)
    {
      if ((EKeyState == X10.EX10Key.X10KEY_ON || EKeyState == X10.EX10Key.X10KEY_REPEAT) && lSequence != 2)
      {
        try
        {
          string keyCode = Enum.GetName(typeof(X10.EX10Command), eCommand);

          if (RemoteCallback != null)
            RemoteCallback(keyCode);
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.ToString());
        }
      }
    }

    public void X10HelpEvent(int hwndDialog, int lHelpID) { }

    #endregion _DIX10InterfaceEvents Members

  }

}
