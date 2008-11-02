using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using InputService.Plugin.Properties;

namespace InputService.Plugin
{
  /// <summary>
  /// IR Server plugin to support the Ads Tech PTV-335 Receiver device.
  /// </summary>
  public class AdsTechPTV335Receiver : PluginBase, IRemoteReceiver
  {
    #region Interop

    // int __cdecl ADS335RCP_GetKey(unsigned char &)
    [DllImport("ADS_335_RCPLIB.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "?ADS335RCP_GetKey@@YAHAAE@Z")]
    private static extern int GetKey(out byte key);

    //int __cdecl ADS335RCP_Init(void)
    [DllImport("ADS_335_RCPLIB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?ADS335RCP_Init@@YAHXZ")
    ]
    private static extern int Init();

    //int __cdecl ADS335RCP_UnInit(void)
    [DllImport("ADS_335_RCPLIB.dll", CallingConvention = CallingConvention.Cdecl,
      EntryPoint = "?ADS335RCP_UnInit@@YAHXZ")]
    private static extern int UnInit();

    #endregion Interop

    #region Consatnts

    private const int PacketTimeout = 200;

    #endregion Constants

    #region Variables

    private bool _processReadThread;
    private Thread _readThread;
    private RemoteHandler _remoteHandler;

    #endregion Variables

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name
    {
      get { return "Ads Tech PTV-335"; }
    }

    /// <summary>
    /// IR Server plugin version.
    /// </summary>
    /// <value>The version.</value>
    public override string Version
    {
      get { return "1.4.2.0"; }
    }

    /// <summary>
    /// The IR Server plugin's author.
    /// </summary>
    /// <value>The author.</value>
    public override string Author
    {
      get { return "and-81"; }
    }

    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    /// <value>The description.</value>
    public override string Description
    {
      get { return "Supports the Ads Tech PTV-335 Receiver"; }
    }

    /// <summary>
    /// Gets a display icon for the plugin.
    /// </summary>
    /// <value>The icon.</value>
    public override Icon DeviceIcon
    {
      get { return Resources.Icon; }
    }

    #region IRemoteReceiver Members

    /// <summary>
    /// Callback for remote button presses.
    /// </summary>
    public RemoteHandler RemoteCallback
    {
      get { return _remoteHandler; }
      set { _remoteHandler = value; }
    }

    #endregion

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


    private void StartReadThread()
    {
      if (_readThread != null)
        return;

      _processReadThread = true;

      _readThread = new Thread(ReadThread);
      _readThread.Name = "AdsTechPTV335Receiver.ReadThread";
      _readThread.IsBackground = true;
      _readThread.Start();
    }

    private void StopReadThread()
    {
      if (_readThread == null)
        return;

      _processReadThread = false;

      if (!_readThread.Join(PacketTimeout*2))
        _readThread.Abort();

      _readThread = null;
    }

    private void ReadThread()
    {
      byte key;
      int retVal;

      while (_processReadThread)
      {
        retVal = GetKey(out key);

        if (retVal == 0)
          Thread.Sleep(PacketTimeout);
        else if (_remoteHandler != null)
          _remoteHandler(Name, key.ToString("X2"));
      }
    }
  }
}