using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

using Microsoft.Win32.SafeHandles;

using IRServerPluginInterface;

namespace GirderPlugin
{

  public class GirderPlugin : IRServerPluginBase, IRemoteReceiver, ITransmitIR, ILearnIR, IConfigure
  {

    #region Constants

    static readonly string[] Ports  = new string[] { "None" };

    #endregion Constants

    #region Variables

    RemoteHandler _remoteButtonHandler = null;

    IntPtr _pluginDll;

    #endregion Variables

    #region Interop

    [DllImport("kernel32")]
    extern static IntPtr LoadLibrary(
      string dllFileName);

    [DllImport("kernel32.dll")]
    static extern IntPtr GetProcAddress(
      IntPtr module,
      string functionName);

    [DllImport("kernel32")]
    extern static Int32 FreeLibrary(
      IntPtr handle);

    #endregion Interop

    #region Girder Plugin Delegates

    /*
void WINAPI gir_version       (char *data, int size);
void WINAPI gir_name          (char *data, int size);
void WINAPI gir_description   (char *data, int size);
int  WINAPI gir_devicenum     ();
int  WINAPI gir_requested_api (int max_api);

int WINAPI gir_open  (int gir_major_ver, int gir_minor_ver, int gir_micro_ver, p_functions api_functions );
int WINAPI gir_close ();
int WINAPI gir_learn_event(char *old, char *newevent, int len);
int WINAPI gir_info  (int message, int wparam, int lparam);

int WINAPI gir_start ();
int WINAPI gir_stop  ();

int WINAPI gir_event(p_command command, char *eventstring, void *payload, int len, char * status, int statuslen);
void WINAPI gir_command_gui();
void WINAPI gir_command_changed(p_command command);
    */

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void gir_version(string data, int size);
    gir_version _girVersion;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void gir_name(string data, int size);
    gir_name _girName;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void gir_description(string data, int size);
    gir_description _girDescription;

    /*
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate int gir_devicenum();
    gir_devicenum _girDevicenum;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate int gir_requested_api();
    gir_requested_api _girRequestedApi;
    */


    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate int gir_start();
    gir_start _girStart;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate int gir_stop();
    gir_stop _girStop;



    #endregion Girder Plugin Delegates


    #region IRServerPluginBase Members

    public override string Name         { get { return "Girder Plugin"; } }
    public override string Version      { get { return "1.0.3.4"; } }
    public override string Author       { get { return "and-81"; } }
    public override string Description  { get { return "Supports using Girder plugins"; } }

    public override bool Detect()
    {
      return false;
    }

    public override bool Start()
    {      
      LoadGirderPlugin("MceIr.dll");
      
      _girStart();

      return true;
    }
    public override void Suspend()
    {

    }
    public override void Resume()
    {

    }
    public override void Stop()
    {
      if (_pluginDll == IntPtr.Zero)
        return;

      _girStop();

      FreeLibrary(_pluginDll);
    }

    #endregion IRServerPluginBase Members

    #region IRemoteReceiver Members

    public RemoteHandler RemoteCallback
    {
      get { return _remoteButtonHandler; }
      set { _remoteButtonHandler = value; }
    }

    #endregion IRemoteReceiver Members

    #region ITransmitIR Members

    public string[] AvailablePorts
    {
      get { return Ports; }
    }

    public bool Transmit(string port, byte[] data)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    #endregion ITransmitIR Members

    #region ILearnIR Members

    public LearnStatus Learn(out byte[] data)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    #endregion ILearnIR Members

    #region IConfigure Members

    public void Configure()
    {
    }

    #endregion IConfigure Members

    #region Implementation

    void LoadGirderPlugin(string girderPluginFile)
    {
      IntPtr function;

      _pluginDll = LoadLibrary(girderPluginFile);
      if (_pluginDll == IntPtr.Zero)
        throw new ApplicationException(String.Format("Failed to load girder plugin ({0})", girderPluginFile));
      
      try
      {
        function = GetProcAddress(_pluginDll, "gir_version");
        _girVersion = (gir_version)Marshal.GetDelegateForFunctionPointer(function, typeof(gir_version));

        function = GetProcAddress(_pluginDll, "gir_name");
        _girName = (gir_name)Marshal.GetDelegateForFunctionPointer(function, typeof(gir_name));

        function = GetProcAddress(_pluginDll, "gir_description");
        _girDescription = (gir_description)Marshal.GetDelegateForFunctionPointer(function, typeof(gir_description));



        function = GetProcAddress(_pluginDll, "gir_start");
        _girStart = (gir_start)Marshal.GetDelegateForFunctionPointer(function, typeof(gir_start));

        function = GetProcAddress(_pluginDll, "gir_stop");
        _girStop = (gir_stop)Marshal.GetDelegateForFunctionPointer(function, typeof(gir_stop));
      }
      catch
      {
        FreeLibrary(_pluginDll);
        throw;
      }

    }
    
    #endregion Implementation

  }

}
