using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Microsoft.Win32.SafeHandles;

namespace MicrosoftMceTransceiver
{

  class NotifyWindow : NativeWindow, IDisposable
  {

    #region Interop

    const int WM_DEVICECHANGE           = 0x0219;
    const int WM_SETTINGSCHANGE         = 0x001A;

    const int DBT_DEVICEARRIVAL         = 0x8000;
    const int DBT_DEVICEREMOVECOMPLETE  = 0x8004;

    [StructLayout(LayoutKind.Sequential)]
    struct DeviceBroadcastHeader
    {
      public int Size;
      public int DeviceType;
      public int Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct DeviceBroadcastInterface
    {
      public int Size;
      public int DeviceType;
      public int Reserved;
      public Guid ClassGuid;

      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
      public string Name;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct DeviceBroadcastHandle
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

    [DllImport("user32", SetLastError = true)]
    static extern IntPtr RegisterDeviceNotification(
      IntPtr handle, 
      ref DeviceBroadcastHandle filter, 
      int flags);

    [DllImport("user32", SetLastError = true)]
    static extern IntPtr RegisterDeviceNotification(
      IntPtr handle, 
      ref DeviceBroadcastInterface filter, 
      int flags);

    [DllImport("user32")]
    static extern int UnregisterDeviceNotification(
      IntPtr handle);

    [DllImport("kernel32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool CancelIo(
      IntPtr handle);

    #endregion Interop

    #region Constructor / Destructor

    public NotifyWindow()
    {

    }

    ~NotifyWindow()
    {
      // Call Dispose with false.  Since we're in the destructor call, the managed resources will be disposed of anyway.
      Dispose(false);
    }

    #endregion Constructor / Destructor

    #region IDisposable Members

    public void Dispose()
    {
      // Dispose of the managed and unmanaged resources
      Dispose(true);

      // Tell the GC that the Finalize process no longer needs to be run for this object.
      GC.SuppressFinalize(this);
    }

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

    #endregion IDisposable Members

    #region Methods
    
    internal void Create()
    {
      if (Handle != IntPtr.Zero)
        return;

      CreateParams Params = new CreateParams();
      Params.ExStyle = 0x80;
      Params.Style = unchecked((int)0x80000000);
      CreateHandle(Params);
    }

    void Destroy()
    {
      if (Handle == IntPtr.Zero)
        return;

      DestroyHandle();
    }

    #endregion Methods

    #region Properties

    internal Guid Class 
    { 
      get { return _deviceClass; }
      set { _deviceClass = value; } 
    }

    #endregion Properties

    #region Overrides
    
    protected override void WndProc(ref Message m)
    {
      if (m.Msg == WM_DEVICECHANGE)
      {
        switch (m.WParam.ToInt32())
        {
          case DBT_DEVICEARRIVAL:
            OnDeviceArrival((DeviceBroadcastHeader)Marshal.PtrToStructure(m.LParam, typeof(DeviceBroadcastHeader)), m.LParam);
            break;

          case DBT_DEVICEREMOVECOMPLETE:
            OnDeviceRemoval((DeviceBroadcastHeader)Marshal.PtrToStructure(m.LParam, typeof(DeviceBroadcastHeader)), m.LParam);
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
      dbi.ClassGuid = _deviceClass;

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

    void OnDeviceArrival(DeviceBroadcastHeader dbh, IntPtr ptr)
    {
      if (dbh.DeviceType == 0x05)
      {
        DeviceBroadcastInterface dbi = (DeviceBroadcastInterface)Marshal.PtrToStructure(ptr, typeof(DeviceBroadcastInterface));

        if (dbi.ClassGuid == _deviceClass && DeviceArrival != null)
          DeviceArrival();
      }
    }

    void OnDeviceRemoval(DeviceBroadcastHeader header, IntPtr ptr)
    {
      if (header.DeviceType == 0x06)
      {
        DeviceBroadcastHandle dbh = (DeviceBroadcastHandle)Marshal.PtrToStructure(ptr, typeof(DeviceBroadcastHandle));

        if (dbh.Handle != _deviceHandle)
          return;

        CancelIo(_deviceHandle);
        UnregisterDeviceRemoval();

        if (DeviceRemoval != null)
          DeviceRemoval();
      }
    }

    #endregion Implementation

    #region Delegates

    internal DeviceEventHandler DeviceArrival;
    internal DeviceEventHandler DeviceRemoval;
    //internal SettingsChanged SettingsChanged;

    #endregion Delegates

    #region Members

    IntPtr _handleDeviceArrival;
    IntPtr _handleDeviceRemoval;
    IntPtr _deviceHandle;
    Guid _deviceClass;

    #endregion Members

  }

}
