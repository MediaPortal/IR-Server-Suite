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
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using IRServer.Plugin.Properties;
using IrssUtils;
using Microsoft.Win32.SafeHandles;

namespace IRServer.Plugin
{
  /// <summary>
  /// IR Server Plugin for the IR receiver built into the Mac Mini.
  /// </summary>
  public class MacMiniReceiver : PluginBase, IRemoteReceiver, IDisposable
  {
    #region Constants

    private const int DeviceBufferSize = 5;

    //New device path verified by "yvos" and "James"
    private const string DevicePathVidPid = "vid_05ac&pid_8242";

    private static readonly byte[] FlatCode = new byte[] { 0x25, 0x87, 0xE0 };
    private static readonly byte[] KeyCodeTemplate = new byte[] { 0x25, 0x87, 0xEE, 0xFF, 0xFF };
    private static readonly byte[] RepeatCode = new byte[] { 0x26, 0x00, 0x00, 0x00, 0x00 };

    #endregion Constants

    #region Variables

    private byte[] _deviceBuffer;
    private FileStream _deviceStream;
    private bool _disposed;

    private string _lastCode = String.Empty;
    private DateTime _lastCodeTime = DateTime.Now;
    private RemoteHandler _remoteButtonHandler;

    #endregion Variables

    #region Destructor

    /// <summary>
    /// Releases unmanaged resources and performs other cleanup operations before the
    /// <see cref="MacMiniReceiver"/> is reclaimed by garbage collection.
    /// </summary>
    ~MacMiniReceiver()
    {
      // Call Dispose with false.  Since we're in the destructor call, the managed resources will be disposed of anyway.
      Dispose(false);
    }

    #endregion Destructor

    #region IDisposable Members

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources
    /// </summary>
    public void Dispose()
    {
      // Dispose of the managed and unmanaged resources
      Dispose(true);

      // Tell the GC that the Finalize process no longer needs to be run for this object.
      GC.SuppressFinalize(this);
    }

    #endregion

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      // process only if mananged and unmanaged resources have
      // not been disposed of.
      if (!_disposed)
      {
        if (disposing)
        {
          // dispose managed resources
          Stop();
        }

        // dispose unmanaged resources
        _disposed = true;
      }
    }

    #region Implementation

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name
    {
      get { return "Mac Mini"; }
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
      get { return "Supports the IR receiver built into the Mac Mini"; }
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
      try
      {
        Guid guid = new Guid();
        Win32.HidD_GetHidGuid(ref guid);

        string devicePath = FindDevice(guid);

        if (devicePath != null)
        {
          return DetectionResult.DevicePresent;
        }
      }
      catch (Win32Exception ex)
      {
        if (ex.NativeErrorCode != 13)
        {
          IrssLog.Error("{0,15} exception: {1}", Name, ex.NativeErrorCode);
          return DetectionResult.DeviceException;
        }
        IrssLog.Debug("{0,15} exception: {1}", Name, ex.NativeErrorCode);
      }
      catch (Exception ex)
      {
        IrssLog.Error("{0,15} exception: {1} type: {2}", Name, ex.Message, ex.GetType());
        return DetectionResult.DeviceException;
      }

      return DetectionResult.DeviceNotFound;
    }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    public override void Start()
    {
      Guid guid = new Guid();
      Win32.HidD_GetHidGuid(ref guid);

      string devicePath = FindDevice(guid);
      if (String.IsNullOrEmpty(devicePath))
        throw new InvalidOperationException("Device not found");

      SafeFileHandle deviceHandle = Win32.CreateFile(devicePath, FileAccess.Read, FileShare.ReadWrite, IntPtr.Zero,
                                               FileMode.Open, Win32.EFileAttributes.Overlapped, IntPtr.Zero);
      int lastError = Marshal.GetLastWin32Error();

      if (deviceHandle.IsInvalid)
        throw new Win32Exception(lastError, "Failed to open device");

      // TODO: Add device removal notification.
      //_deviceWatcher.RegisterDeviceRemoval(deviceHandle);

      _deviceBuffer = new byte[DeviceBufferSize];

      _deviceStream = new FileStream(deviceHandle, FileAccess.Read, _deviceBuffer.Length, true);
      _deviceStream.BeginRead(_deviceBuffer, 0, _deviceBuffer.Length, OnReadComplete, null);
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
      if (_deviceStream == null)
        return;

      try
      {
        _deviceStream.Dispose();
      }
      catch (IOException)
      {
        // we are closing the stream so ignore this
      }
      finally
      {
        _deviceStream = null;
      }
    }

    /// <summary>
    /// Finds the device.
    /// </summary>
    /// <param name="classGuid">The class GUID.</param>
    /// <returns>Device path.</returns>
    private static string FindDevice(Guid classGuid)
    {
      // 0x12 = DIGCF_PRESENT | DIGCF_DEVICEINTERFACE
      IntPtr handle = Win32.SetupDiGetClassDevs(ref classGuid, 0, IntPtr.Zero, 0x12);
      int lastError = Marshal.GetLastWin32Error();

      if (handle.ToInt32() == -1)
        throw new Win32Exception(lastError);

      string devicePath = null;

      for (int deviceIndex = 0; ; deviceIndex++)
      {
        Win32.DeviceInfoData deviceInfoData = new Win32.DeviceInfoData();
        deviceInfoData.Size = Marshal.SizeOf(deviceInfoData);

        if (!Win32.SetupDiEnumDeviceInfo(handle, deviceIndex, ref deviceInfoData))
        {
          // out of devices or do we have an error?
          lastError = Marshal.GetLastWin32Error();
          if (lastError != 0x0103 && lastError != 0x007E)
          {
            Win32.SetupDiDestroyDeviceInfoList(handle);
            throw new Win32Exception(lastError);
          }

          Win32.SetupDiDestroyDeviceInfoList(handle);
          break;
        }

        Win32.DeviceInterfaceData deviceInterfaceData = new Win32.DeviceInterfaceData();
        deviceInterfaceData.Size = Marshal.SizeOf(deviceInterfaceData);

        if (!Win32.SetupDiEnumDeviceInterfaces(handle, ref deviceInfoData, ref classGuid, 0, ref deviceInterfaceData))
        {
          Win32.SetupDiDestroyDeviceInfoList(handle);
          throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        uint cbData = 0;

        if (!Win32.SetupDiGetDeviceInterfaceDetail(handle, ref deviceInterfaceData, IntPtr.Zero, 0, ref cbData, IntPtr.Zero) &&
          cbData == 0)
        {
          Win32.SetupDiDestroyDeviceInfoList(handle);
          throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        Win32.DeviceInterfaceDetailData deviceInterfaceDetailData = new Win32.DeviceInterfaceDetailData
                                                                {
                                                                  Size = Win32.Check64Bit() ? 8 : 5
                                                                };

        if (!Win32.SetupDiGetDeviceInterfaceDetail(handle, ref deviceInterfaceData, ref deviceInterfaceDetailData, cbData,
                                           IntPtr.Zero, IntPtr.Zero))
        {
          Win32.SetupDiDestroyDeviceInfoList(handle);
          throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        if (deviceInterfaceDetailData.DevicePath.IndexOf(DevicePathVidPid, StringComparison.OrdinalIgnoreCase) != -1)
        {
          Win32.SetupDiDestroyDeviceInfoList(handle);
          devicePath = deviceInterfaceDetailData.DevicePath;
          break;
        }
      }

      return devicePath;
    }

    /// <summary>
    /// Called when a device read is completed.
    /// </summary>
    /// <param name="asyncResult">The async result.</param>
    private void OnReadComplete(IAsyncResult asyncResult)
    {
      try
      {
        int readLength = _deviceStream.EndRead(asyncResult);
        if (readLength > 0 && _remoteButtonHandler != null)
        {
          byte[] codeBytes = new byte[readLength];
          Array.Copy(_deviceBuffer, 0, codeBytes, 0, readLength);

          TimeSpan timeSpan = DateTime.Now - _lastCodeTime;

          string keyCode = String.Empty;

          bool accept = false;
          if (Equals(codeBytes, FlatCode))
          {
#if TRACE
            Trace.WriteLine("MacMini Remote has a flat battery");
#endif
          }
          if (Equals(codeBytes, RepeatCode))
          {
            if (timeSpan.Milliseconds >= 250)
            {
              accept = true;
              keyCode = _lastCode;
            }
          }
          else
          {
            accept = true;
            keyCode = BitConverter.ToString(codeBytes, 3, 2);
          }

          if (accept)
          {
            _remoteButtonHandler(Name, keyCode);
            _lastCodeTime = DateTime.Now;
            _lastCode = keyCode;
          }
        }

        _deviceStream.BeginRead(_deviceBuffer, 0, _deviceBuffer.Length, OnReadComplete, null);
      }
      catch (Exception)
      {
      }
    }

    #endregion Implementation
  }
}