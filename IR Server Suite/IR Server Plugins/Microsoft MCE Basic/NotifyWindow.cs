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
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace IRServer.Plugin
{
  internal class NotifyWindow : NativeWindow, IDisposable
  {
    #region Interop

    private const int DBT_DEVICEARRIVAL = 0x8000;
    private const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
    private const int WM_DEVICECHANGE = 0x0219;
    //private const int WM_SETTINGSCHANGE = 0x001A;

    [DllImport("user32", SetLastError = true)]
    private static extern IntPtr RegisterDeviceNotification(
      IntPtr handle,
      ref DeviceBroadcastHandle filter,
      int flags);

    [DllImport("user32", SetLastError = true)]
    private static extern IntPtr RegisterDeviceNotification(
      IntPtr handle,
      ref DeviceBroadcastInterface filter,
      int flags);

    [DllImport("user32")]
    private static extern int UnregisterDeviceNotification(
      IntPtr handle);

    [DllImport("kernel32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CancelIo(
      IntPtr handle);

    #region Nested type: DeviceBroadcastHandle

    [StructLayout(LayoutKind.Sequential)]
    private struct DeviceBroadcastHandle
    {
      public int Size;
      public int DeviceType;
      public int Reserved;
      public IntPtr Handle;
      public IntPtr HandleNotify;
      public Guid EventGuid;
      public int NameOffset;
      public byte Data;
    }

    #endregion

    #region Nested type: DeviceBroadcastHeader

    [StructLayout(LayoutKind.Sequential)]
    private struct DeviceBroadcastHeader
    {
      public int Size;
      public int DeviceType;
      public int Reserved;
    }

    #endregion

    #region Nested type: DeviceBroadcastInterface

    [StructLayout(LayoutKind.Sequential)]
    private struct DeviceBroadcastInterface
    {
      public int Size;
      public int DeviceType;
      public int Reserved;
      public Guid ClassGuid;

      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)] public string Name;
    }

    #endregion

    #endregion Interop

    #region Constructor / Destructor

    ~NotifyWindow()
    {
      // Call Dispose with false.  Since we're in the destructor call, the managed resources will be disposed of anyway.
      Dispose(false);
    }

    #endregion Constructor / Destructor

    internal DeviceEventHandler DeviceArrival;
    internal DeviceEventHandler DeviceRemoval;

    #region Members

    private IntPtr _deviceHandle;
    private IntPtr _handleDeviceArrival;
    private IntPtr _handleDeviceRemoval;

    #endregion Members

    #region IDisposable Members

    public void Dispose()
    {
      // Dispose of the managed and unmanaged resources
      Dispose(true);

      // Tell the GC that the Finalize process no longer needs to be run for this object.
      GC.SuppressFinalize(this);
    }

    #endregion

    private void Dispose(bool disposing)
    {
      if (disposing)
      {
        // Dispose managed resources ...

        Destroy();
      }

      // Free native resources ...
      UnregisterDeviceArrival();
      UnregisterDeviceRemoval();
    }

    #region Methods

    /// <summary>
    /// Creates the window handle.
    /// </summary>
    internal void Create()
    {
      if (Handle != IntPtr.Zero)
        return;

      CreateParams Params = new CreateParams();
      Params.ExStyle = 0x80;
      Params.Style = unchecked((int) 0x80000000);
      CreateHandle(Params);
    }

    /// <summary>
    /// Destroys the window handle.
    /// </summary>
    private void Destroy()
    {
      if (Handle == IntPtr.Zero)
        return;

      DestroyHandle();
    }

    #endregion Methods

    #region Properties

    internal Guid Class { get; set; }

    #endregion Properties

    #region Overrides

    protected override void WndProc(ref Message m)
    {
      if (m.Msg == WM_DEVICECHANGE)
      {
        switch (m.WParam.ToInt32())
        {
          case DBT_DEVICEARRIVAL:
            OnDeviceArrival((DeviceBroadcastHeader) Marshal.PtrToStructure(m.LParam, typeof (DeviceBroadcastHeader)),
                            m.LParam);
            break;

          case DBT_DEVICEREMOVECOMPLETE:
            OnDeviceRemoval((DeviceBroadcastHeader) Marshal.PtrToStructure(m.LParam, typeof (DeviceBroadcastHeader)),
                            m.LParam);
            break;
        }
      }
      /*else if (m.Msg == WM_SETTINGSCHANGE)
      {
        if (SettingsChanged != null)
          SettingsChanged();
      }*/

      base.WndProc(ref m);
    }

    #endregion Overrides

    #region Implementation

    internal void RegisterDeviceArrival()
    {
      DeviceBroadcastInterface dbi = new DeviceBroadcastInterface();

      dbi.Size = Marshal.SizeOf(dbi);
      dbi.DeviceType = 0x5;
      dbi.ClassGuid = Class;

      _handleDeviceArrival = RegisterDeviceNotification(Handle, ref dbi, 0);
      int lastError = Marshal.GetLastWin32Error();
      if (_handleDeviceArrival == IntPtr.Zero)
        throw new Win32Exception(lastError);
    }

    internal void RegisterDeviceRemoval(IntPtr deviceHandle)
    {
      DeviceBroadcastHandle dbh = new DeviceBroadcastHandle();

      dbh.Size = Marshal.SizeOf(dbh);
      dbh.DeviceType = 0x6;
      dbh.Handle = deviceHandle;

      _deviceHandle = deviceHandle;
      _handleDeviceRemoval = RegisterDeviceNotification(Handle, ref dbh, 0);
      int lastError = Marshal.GetLastWin32Error();
      if (_handleDeviceRemoval == IntPtr.Zero)
        throw new Win32Exception(lastError);
    }

    internal void UnregisterDeviceArrival()
    {
      if (_handleDeviceArrival == IntPtr.Zero)
        return;

      UnregisterDeviceNotification(_handleDeviceArrival);
      _handleDeviceArrival = IntPtr.Zero;
    }

    internal void UnregisterDeviceRemoval()
    {
      if (_handleDeviceRemoval == IntPtr.Zero)
        return;

      UnregisterDeviceNotification(_handleDeviceRemoval);
      _handleDeviceRemoval = IntPtr.Zero;
      _deviceHandle = IntPtr.Zero;
    }

    private void OnDeviceArrival(DeviceBroadcastHeader dbh, IntPtr ptr)
    {
      if (dbh.DeviceType == 0x05)
      {
        DeviceBroadcastInterface dbi =
          (DeviceBroadcastInterface) Marshal.PtrToStructure(ptr, typeof (DeviceBroadcastInterface));

        if (dbi.ClassGuid == Class && DeviceArrival != null)
          DeviceArrival();
      }
    }

    private void OnDeviceRemoval(DeviceBroadcastHeader header, IntPtr ptr)
    {
      if (header.DeviceType == 0x06)
      {
        DeviceBroadcastHandle dbh = (DeviceBroadcastHandle) Marshal.PtrToStructure(ptr, typeof (DeviceBroadcastHandle));

        if (dbh.Handle != _deviceHandle)
          return;

        CancelIo(_deviceHandle);
        UnregisterDeviceRemoval();

        if (DeviceRemoval != null)
          DeviceRemoval();
      }
    }

    #endregion Implementation

    //internal SettingsChanged SettingsChanged;
  }
}