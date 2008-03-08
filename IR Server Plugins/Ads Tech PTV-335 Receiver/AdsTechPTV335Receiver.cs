using System;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

namespace InputService.Plugin
{

  /// <summary>
  /// IR Server plugin to support the Ads Tech PTV-335 Receiver device.
  /// </summary>
  public class AdsTechPTV335Receiver : PluginBase, IRemoteReceiver
  {

    #region Interop

    // int __cdecl ADS335RCP_GetKey(unsigned char &)
    [DllImport("ADS_335_RCPLIB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ADS335RCP_GetKey@@YAHAAE@Z")]
    static extern int GetKey(out byte key);

    //int __cdecl ADS335RCP_Init(void)
    [DllImport("ADS_335_RCPLIB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ADS335RCP_Init@@YAHXZ")]
    static extern int Init();

    //int __cdecl ADS335RCP_UnInit(void)
    [DllImport("ADS_335_RCPLIB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ADS335RCP_UnInit@@YAHXZ")]
    static extern int UnInit();

    #endregion Interop

    #region Consatnts

    const int PacketTimeout = 200;

    #endregion Constants

    #region Variables

    RemoteHandler _remoteHandler;

    bool _processReadThread;
    Thread _readThread;

    #endregion Variables


    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name         { get { return "Ads Tech PTV-335"; } }
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
    public override string Description  { get { return "Supports the Ads Tech PTV-335 Receiver"; } }
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
      try
      {
        return (Init() != 0);
      }
      catch
      {
        return false;
      }
    }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public override void Start()
    {
      int retVal = Init();
      if (retVal == 0)
        throw new InvalidOperationException("Failed to initialize device access");

      StartReadThread();
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
      StopReadThread();

      UnInit();
    }
    
    /// <summary>
    /// Callback for remote button presses.
    /// </summary>
    public RemoteHandler RemoteCallback
    {
      get { return _remoteHandler; }
      set { _remoteHandler = value; }
    }


    void StartReadThread()
    {
      if (_readThread != null)
        return;

      _processReadThread = true;

      _readThread = new Thread(new ThreadStart(ReadThread));
      _readThread.Name = "AdsTechPTV335Receiver.ReadThread";
      _readThread.IsBackground = true;
      _readThread.Start();
    }

    void StopReadThread()
    {
      if (_readThread == null)
        return;

      _processReadThread = false;

      if (!_readThread.Join(PacketTimeout * 2))
        _readThread.Abort();

      _readThread = null;
    }

    void ReadThread()
    {
      byte key;
      int retVal;

      while (_processReadThread)
      {
        retVal = GetKey(out key);

        if (retVal == 0)
          Thread.Sleep(PacketTimeout);
        else if (_remoteHandler != null)
          _remoteHandler(this.Name, key.ToString("X2"));
      }
    }
    
  }

}
