#region Copyright (C) 2005-2009 Team MediaPortal

// Copyright (C) 2005-2009 Team MediaPortal
// http://www.team-mediaportal.com
// 
// This Program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2, or (at your option)
// any later version.
// 
// This Program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with GNU Make; see the file COPYING.  If not, write to
// the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
// http://www.gnu.org/copyleft/gpl.html

#endregion

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using IRServer.Plugin.Properties;
using IrssUtils;

namespace IRServer.Plugin
{
  /// <summary>
  /// IR Server Plugin for the Technotrend Receivers.
  /// </summary>
  public class TechnotrendReceiver : PluginBase, IRemoteReceiver
  {
    #region Enumerations

    #region Nested type: DEVICE_CAT

    /// <summary>
    /// Device Category.
    /// </summary>
    private enum DEVICE_CAT
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

    #endregion

    #region Nested type: TYPE_RET_VAL

    /// <summary>
    /// Return Value.
    /// </summary>
    private enum TYPE_RET_VAL
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

    #endregion

    #endregion Enumerations

    // #define TEST_APPLICATION in the project properties when creating the console test app ...
#if TEST_APPLICATION

    static void Remote(string deviceName, string code)
    {
      Console.WriteLine("Remote: {0}", code);
    }

    [STAThread]
    static void Main()
    {
      TechnotrendReceiver c;

      try
      {
        c = new TechnotrendReceiver();

        c.RemoteCallback += new RemoteHandler(Remote);

        c.Start();

        System.Windows.Forms.Application.Run();

        c.Stop();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
      finally
      {
        c = null;
      }

      Console.ReadKey();
    }

#endif

    #region Nested type: CallbackDef

    /// <summary>
    /// Called by the Technotrend api dll to signal a remote button has been received.
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private unsafe delegate void CallbackDef(int context, uint* buffer);

    #endregion

    #region Interop

    [DllImport("ttBdaDrvApi_Dll.dll", EntryPoint = "bdaapiEnumerate")]
    private static extern UInt32 bdaapiEnumerate(DEVICE_CAT deviceType);

    [DllImport("ttBdaDrvApi_Dll.dll", EntryPoint = "bdaapiOpen")]
    private static extern IntPtr bdaapiOpen(DEVICE_CAT deviceType, uint deviceId);

    [DllImport("ttBdaDrvApi_Dll.dll", EntryPoint = "bdaapiClose")]
    private static extern void bdaapiClose(IntPtr handle);

    [DllImport("ttBdaDrvApi_Dll.dll", EntryPoint = "bdaapiOpenIR")]
    private static extern TYPE_RET_VAL bdaapiOpenIR(IntPtr handle, IntPtr callback, int context);

    [DllImport("ttBdaDrvApi_Dll.dll", EntryPoint = "bdaapiCloseIR")]
    private static extern TYPE_RET_VAL bdaapiCloseIR(IntPtr handle);

    #endregion Interop

    #region Variables

    private readonly CallbackDef _callback;
    private readonly IntPtr _callbackPtr;
    private readonly List<IntPtr> _handles;

    private int _lastCode = -1;
    private DateTime _lastCodeTime = DateTime.Now;
    private RemoteHandler _remoteButtonHandler;

    #endregion Variables

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="TechnotrendReceiver"/> class.
    /// </summary>
    public TechnotrendReceiver()
    {
      _handles = new List<IntPtr>();

      unsafe
      {
        _callback = OnIRCode;
        _callbackPtr = Marshal.GetFunctionPointerForDelegate(_callback);
      }
    }

    #endregion Constructor

    #region Implementation

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name
    {
      get { return "Technotrend"; }
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
      get { return "and-81, original MediaPortal plugin by Alexander Plas"; }
    }

    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    /// <value>The description.</value>
    public override string Description
    {
      get { return "Supports the Technotrend receivers"; }
    }

    /// <summary>
    /// Gets a display icon for the plugin.
    /// </summary>
    /// <value>The icon.</value>
    public override Icon DeviceIcon
    {
      get { return Resources.Icon; }
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
    /// Detect the presence of this device.
    /// </summary>
    public override DetectionResult Detect()
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
                  return DetectionResult.DevicePresent;
              }
            }
            catch
            {
              bdaapiClose(handle);
            }
          }
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error("{0} exception: {1}", Name, ex.Message);
        return DetectionResult.DeviceException;
      }

      return DetectionResult.DeviceNotFound;
    }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    public override void Start()
    {
      IntPtr handle;
      TYPE_RET_VAL error;

      int context = 1;

      for (DEVICE_CAT cat = DEVICE_CAT.UNKNOWN; cat <= DEVICE_CAT.USB_2_DSS; cat++)
      {
        uint enumerate = bdaapiEnumerate(cat);

        for (uint index = 0; index < enumerate; index++)
        {
          handle = bdaapiOpen(cat, index);

          if ((handle != IntPtr.Zero) && (handle.ToInt32() != -1))
          {
            error = bdaapiOpenIR(handle, _callbackPtr, context++);
            if (error == TYPE_RET_VAL.RET_SUCCESS)
              _handles.Add(handle);
            else
              bdaapiClose(handle);
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
    /// Called when an IR code is received.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="buffer">The code.</param>
    private unsafe void OnIRCode(int context, uint* buffer)
    {
      try
      {
        int code = ((int) buffer[0]) & 0x00FF;

        DateTime now = DateTime.Now;
        TimeSpan timeSpan = now - _lastCodeTime;

        if (code != _lastCode || timeSpan.Milliseconds >= 250)
        {
          if (_remoteButtonHandler != null)
            _remoteButtonHandler(Name, code.ToString());

          _lastCodeTime = now;
        }

        _lastCode = code;
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

    #endregion Implementation
  }
}