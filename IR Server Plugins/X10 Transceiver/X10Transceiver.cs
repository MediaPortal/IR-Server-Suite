using System;
using System.Collections;
#if TRACE
using System.Diagnostics;
#endif
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

using X10;

namespace InputService.Plugin
{

  /// <summary>
  /// IR Server Plugin for X10 Transceiver devices.
  /// </summary>
  public class X10Transceiver : PluginBase, IRemoteReceiver, _DIX10InterfaceEvents
  {

    #region Constants

    static readonly string ConfigurationFile =
      Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
      "\\IR Server Suite\\Input Service\\X10 Receiver.xml";

    #endregion Constants

    #region Variables

    static RemoteHandler _remoteButtonHandler;

    X10Interface X10Inter;
    IConnectionPointContainer icpc;
    IConnectionPoint icp;
    int cookie;

    #endregion Variables
   
    #region IIRServerPlugin Members

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name         { get { return "X10"; } }
    /// <summary>
    /// IR Server plugin version.
    /// </summary>
    /// <value>The version.</value>
    public override string Version      { get { return "1.0.4.2"; } }
    /// <summary>
    /// The IR Server plugin's author.
    /// </summary>
    /// <value>The author.</value>
    public override string Author       { get { return "and-81, with original MediaPortal code by CoolHammer, mPod and diehard2"; } }
    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    /// <value>The description.</value>
    public override string Description  { get { return "X10 Transceiver"; } }

    /// <summary>
    /// Detect the presence of this device.  Devices that cannot be detected will always return false.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the device is present, otherwise <c>false</c>.
    /// </returns>
    public override bool Detect()
    {
      X10Interface test;

      try
      {
        test = new X10Interface();
        if (test != null)
          return true;
      }
      catch
      {
      }
      finally 
      {
        test = null;
      }

      return false;
    }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    public override void Start()
    {
      X10Inter = new X10Interface();
      if (X10Inter == null)
        throw new ApplicationException("Failed to start X10 interface");

      // Bind the interface using a connection point
      icpc = (IConnectionPointContainer)X10Inter;
      Guid IID_InterfaceEvents = typeof(_DIX10InterfaceEvents).GUID;
      icpc.FindConnectionPoint(ref IID_InterfaceEvents, out icp);
      icp.Advise(this, out cookie);
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
      if (X10Inter != null)
      {
        icp.Unadvise(cookie);
        icpc = null;
        X10Inter = null;
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

    #endregion Implementation

    #region _DIX10InterfaceEvents Members

    /// <summary>
    /// X10 command.
    /// </summary>
    /// <param name="bszCommand">The command.</param>
    /// <param name="eCommand">The command type.</param>
    /// <param name="lAddress">The address.</param>
    /// <param name="EKeyState">State of the key.</param>
    /// <param name="lSequence">The sequence.</param>
    /// <param name="eCommandType">Type of the command.</param>
    /// <param name="varTimestamp">The timestamp.</param>
    [CLSCompliant(false)]
    public void X10Command(string bszCommand, EX10Command eCommand, int lAddress, EX10Key EKeyState, int lSequence, EX10Comm eCommandType, object varTimestamp)
    {
      if ((EKeyState == X10.EX10Key.X10KEY_ON || EKeyState == X10.EX10Key.X10KEY_REPEAT) && lSequence != 2)
      {
        try
        {
          string keyCode = Enum.GetName(typeof(X10.EX10Command), eCommand);

          if (RemoteCallback != null)
            RemoteCallback(this.Name, keyCode);
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
    }

    /// <summary>
    /// X10 help event.
    /// </summary>
    /// <param name="hwndDialog">The HWND of the dialog.</param>
    /// <param name="lHelpID">The help ID.</param>
    public void X10HelpEvent(int hwndDialog, int lHelpID) { }

    #endregion _DIX10InterfaceEvents Members

  }

}
