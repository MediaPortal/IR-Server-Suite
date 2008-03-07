using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
#if TRACE
using System.Diagnostics;
#endif
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Win32.SafeHandles;

namespace InputService.Plugin
{

  /// <summary>
  /// IR Server Plugin for the Leadtek Winfast CoolCommand Receiver device.
  /// </summary>
  public class CoolCommandReceiver : PluginBase, IRemoteReceiver
  {

    #region Constants

    static readonly string ConfigurationFile = Path.Combine(ConfigurationPath, "CoolCommand Receiver.xml");

    const int WM_COOL = 0x0A00;

    #endregion Constants

    #region Variables

    static RemoteHandler _remoteHandler;

    static ReceiverWindow _receiverWindow;

    static DateTime _lastCodeTime = DateTime.Now;

    #endregion Variables

    #region Implementation

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name         { get { return "CoolCommand"; } }
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
    public override string Description  { get { return "Leadtek Winfast CoolCommand Receiver"; } }
    /// <summary>
    /// Gets a display icon for the plugin.
    /// </summary>
    /// <value>The icon.</value>
    public override Icon DeviceIcon     { get { return Properties.Resources.Icon; } }

    /// <summary>
    /// Detect the presence of this device.  Devices that cannot be detected will always return false.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the device is present, otherwise <c>false</c>.
    /// </returns>
    public override bool Detect()
    {
      // TODO: Add CoolCommand detection
      try
      {
        return false;
      }
      catch
      {
        return false;
      }
    }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    public override void Start()
    {
      _receiverWindow = new ReceiverWindow("CoolCommand Receiver");
      _receiverWindow.ProcMsg += new ProcessMessage(WndProc);
    }
    /// <summary>
    /// Suspend the IR Server plugin when computer enters standby.
    /// </summary>
    public override void Suspend()
    {
    }
    /// <summary>
    /// Resume the IR Server plugin when the computer returns from standby.
    /// </summary>
    public override void Resume()
    {
    }
    /// <summary>
    /// Stop the IR Server plugin.
    /// </summary>
    public override void Stop()
    {
      _receiverWindow.DestroyHandle();
      _receiverWindow = null;
    }

    /// <summary>
    /// Callback for remote button presses.
    /// </summary>
    /// <value>The remote callback.</value>
    public RemoteHandler RemoteCallback
    {
      get { return _remoteHandler; }
      set { _remoteHandler = value; }
    }

    #endregion Implementation

    void WndProc(ref Message msg)
    {
      if (msg.Msg == WM_COOL)
        if (_remoteHandler != null)
          _remoteHandler(this.Name, msg.WParam.ToInt32().ToString());
    }

  }

}
