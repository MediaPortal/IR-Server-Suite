using System;
using System.Collections.Generic;
using System.ComponentModel;
#if TRACE
using System.Diagnostics;
#endif
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace InputService.Plugin
{

  /// <summary>
  /// IR Server Plugin for the Technotrend Receivers.
  /// </summary>
  public class TechnotrendReceiver : PluginBase, IRemoteReceiver
  {

    #region Debug

    [STAThread]
    static void Main()
    {
      try
      {
        TechnotrendReceiver c = new TechnotrendReceiver();

        //c.Configure(null);

        //c.RemoteCallback += new RemoteHandler(xRemote);

        c.Start();

        Console.ReadKey();
        //System.Windows.Forms.Application.Run();

        c.Stop();
        c = null;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }

      Console.ReadKey();
    }

    #endregion Debug


    #region Enumerations

    /// <summary>
    /// Device Category.
    /// </summary>
    enum DEVICE_CAT
    {
      /// <summary>
      /// Not set.
      /// </summary>
      UNKNOWN = 0,
      /// <summary>
      /// Budget 2.
      /// </summary>
      BUDGET_2,
      /// <summary>
      /// Budget 3 aka TT-budget T-3000.
      /// </summary>
      BUDGET_3,
      /// <summary>
      /// USB 2.0
      /// </summary>
      USB_2,
      /// <summary>
      /// USB 2.0 Pinnacle.
      /// </summary>
      USB_2_PINNACLE,
      /// <summary>
      /// USB 2.0 DSS.
      /// </summary>
      USB_2_DSS,
    }

    /// <summary>
    /// Return Value.
    /// </summary>
    enum TYPE_RET_VAL
    {
      /// <summary>
      /// 0 operation finished successful.
      /// </summary>
      RET_SUCCESS = 0,
      /// <summary>
      /// 1 operation is not implemented for the opened handle.
      /// </summary>
      RET_NOT_IMPL,
      /// <summary>
      /// 2 operation is not supported for the opened handle.
      /// </summary>
      RET_NOT_SUPPORTED,
      /// <summary>
      /// 3 the given HANDLE seems not to be correct.
      /// </summary>
      RET_ERROR_HANDLE,
      /// <summary>
      /// 4 the internal IOCTL subsystem has no device handle.
      /// </summary>
      RET_IOCTL_NO_DEV_HANDLE,
      /// <summary>
      /// 5 the internal IOCTL failed.
      /// </summary>
      RET_IOCTL_FAILED,
      /// <summary>
      /// 6 the infrared interface is already initialised.
      /// </summary>
      RET_IR_ALREADY_OPENED,
      /// <summary>
      /// 7 the infrared interface is not initialised.
      /// </summary>
      RET_IR_NOT_OPENED,
      /// <summary>
      /// 8 length exceeds maximum in EEPROM-Userspace operation.
      /// </summary>
      RET_TO_MANY_BYTES,
      /// <summary>
      /// 9 common interface hardware error.
      /// </summary>
      RET_CI_ERROR_HARDWARE,
      /// <summary>
      /// a common interface already opened.
      /// </summary>
      RET_CI_ALREADY_OPENED,
      /// <summary>
      /// b operation finished with timeout.
      /// </summary>
      RET_TIMEOUT,
      /// <summary>
      /// c read psi failed.
      /// </summary>
      RET_READ_PSI_FAILED,
      /// <summary>
      /// d not set.
      /// </summary>
      RET_NOT_SET,
      /// <summary>
      /// e operation finished with general error.
      /// </summary>
      RET_ERROR,
      /// <summary>
      /// f operation finished with ilegal pointer.
      /// </summary>
      RET_ERROR_POINTER,
    }

    #endregion Enumerations

    #region Delegates

    /// <summary>
    /// Called by the Technotrend api dll to signal a remote button has been received.
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    unsafe delegate void CallbackDef(int context, uint* buffer);

    #endregion Delegates

    #region Interop

    [DllImport("ttBdaDrvApi_Dll.dll", EntryPoint = "bdaapiEnumerate")]
    static extern UInt32 bdaapiEnumerate(DEVICE_CAT deviceType);

    [DllImport("ttBdaDrvApi_Dll.dll", EntryPoint = "bdaapiOpen")]
    static extern IntPtr bdaapiOpen(DEVICE_CAT deviceType, uint deviceId);

    [DllImport("ttBdaDrvApi_Dll.dll", EntryPoint = "bdaapiClose")]
    static extern void bdaapiClose(IntPtr handle);

    [DllImport("ttBdaDrvApi_Dll.dll", EntryPoint = "bdaapiOpenIR")]
    static extern TYPE_RET_VAL bdaapiOpenIR(IntPtr handle, IntPtr callback, int context);

    [DllImport("ttBdaDrvApi_Dll.dll", EntryPoint = "bdaapiCloseIR")]
    static extern TYPE_RET_VAL bdaapiCloseIR(IntPtr handle);

    #endregion Interop

    #region Variables

    List<IntPtr> _handles;
    CallbackDef _callback;
    IntPtr _callbackPtr;

    RemoteHandler _remoteButtonHandler;

    int _lastTrigger        = -1;
    int _lastCode           = -1;
    DateTime _lastCodeTime  = DateTime.Now;

    #endregion Variables

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="TechnotrendReceiver"/> class.
    /// </summary>
    public TechnotrendReceiver()
    {
      _handles        = new List<IntPtr>();

      unsafe
      {
        _callback     = new CallbackDef(OnIRCode);
        _callbackPtr  = Marshal.GetFunctionPointerForDelegate(_callback);
      }
    }

    #endregion Constructor

    #region Implementation

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name         { get { return "Technotrend"; } }
    /// <summary>
    /// IR Server plugin version.
    /// </summary>
    /// <value>The version.</value>
    public override string Version      { get { return "1.0.4.2"; } }
    /// <summary>
    /// The IR Server plugin's author.
    /// </summary>
    /// <value>The author.</value>
    public override string Author       { get { return "and-81, original MediaPortal plugin by Alexander Plas"; } }
    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    /// <value>The description.</value>
    public override string Description  { get { return "Supports the Technotrend receivers"; } }
    /// <summary>
    /// Gets a display icon for the plugin.
    /// </summary>
    /// <value>The icon.</value>
    public override Icon DeviceIcon     { get { return Properties.Resources.Icon; } }

    /// <summary>
    /// Detect the presence of this device.  Devices that cannot be detected will always return false.
    /// </summary>
    /// <returns><c>true</c> if the device is present, otherwise <c>false</c>.</returns>
    public override bool Detect()
    {
      IntPtr handle;
      TYPE_RET_VAL error;

      try
      {
        for (DEVICE_CAT cat = DEVICE_CAT.UNKNOWN; cat <= DEVICE_CAT.USB_2_DSS; cat++)
        {
          if (bdaapiEnumerate(cat) > 0)
          {
            handle = bdaapiOpen(cat, 0);
            try
            {
              if ((handle != IntPtr.Zero) && (handle.ToInt32() != -1))
              {
                error = bdaapiOpenIR(handle, _callbackPtr, 0);

                bdaapiClose(handle);

                if (error == TYPE_RET_VAL.RET_SUCCESS)
                  return true;
              }
            }
            catch
            {
              bdaapiClose(handle);
            }
          }
        }
      }
      catch
      {
      }

      return false;
    }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    public override void Start()
    {
      IntPtr handle;
      TYPE_RET_VAL error;

      for (DEVICE_CAT cat = DEVICE_CAT.UNKNOWN; cat <= DEVICE_CAT.USB_2_DSS; cat++)
      {
        if (bdaapiEnumerate(cat) > 0)
        {
          handle = bdaapiOpen(cat, 0);
          if ((handle != IntPtr.Zero) && (handle.ToInt32() != -1))
          {
            error = bdaapiOpenIR(handle, _callbackPtr, 0);
            if (error == TYPE_RET_VAL.RET_SUCCESS)
            {
              _handles.Add(handle);
            }
            else
            {
              bdaapiClose(handle);
            }
          }
        }
      }

      if (_handles.Count == 0)
        throw new InvalidOperationException("No Technotrend IR devices found");
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
      if (_handles.Count == 0)
        return;

      foreach (IntPtr handle in _handles)
      {
        bdaapiCloseIR(handle);
        bdaapiClose(handle);
      }

      _handles.Clear();
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

    /// <summary>
    /// Called when an IR code is received.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="buffer">The code.</param>
    unsafe void OnIRCode(int context, uint *buffer)
    {
      try
      {
        int code    = ((int)buffer[0]) & 0x00FF;
        int trigger = ((int)buffer[0]) & 0x0800;

        DateTime now = DateTime.Now;
        TimeSpan timeSpan = now - _lastCodeTime;

        if (code != _lastCode || trigger != _lastTrigger || timeSpan.Milliseconds >= 250)
        {
          if (_remoteButtonHandler != null)
            _remoteButtonHandler("Technotrend", code.ToString());

          _lastCodeTime = now;
        }

        _lastCode     = code;
        _lastTrigger  = trigger;
      }
      catch
      {
        //MessageBox.Show(ex.ToString());
      }
    }

    #endregion Implementation

  }

}
