using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

using Microsoft.Win32;

namespace HcwReceiver
{

  /// <summary>
  /// Callback for remote button press events.
  /// </summary>
  public delegate void ButtonReceived(int buttonCode);

  /// <summary>
  /// Wrapper class for irremote.dll
  /// </summary>
  public class IrRemoteWrapper
  {

    #region Interop

    /// <summary>
    /// The SetDllDirectory function adds a directory to the search path used to locate DLLs for the application.
    /// http://msdn.microsoft.com/library/en-us/dllproc/base/setdlldirectory.asp
    /// </summary>
    /// <param name="PathName">Pointer to a null-terminated string that specifies the directory to be added to the search path.</param>
    /// <returns></returns>
    [DllImport("kernel32.dll")]
    static extern bool SetDllDirectory(
      string PathName);

    /// <summary>
    /// Registers window handle with Hauppauge IR driver
    /// </summary>
    /// <param name="WindowHandle"></param>
    /// <param name="Msg"></param>
    /// <param name="Verbose"></param>
    /// <param name="IRPort"></param>
    /// <returns></returns>
    [DllImport("irremote.dll")]
    static extern bool IR_Open(
      IntPtr WindowHandle,
      uint Msg,
      bool Verbose,
      ushort IRPort);

    /// <summary>
    /// Gets the received key code (new version, works for PVR-150 as well)
    /// </summary>
    /// <param name="RepeatCount"></param>
    /// <param name="RemoteCode"></param>
    /// <param name="KeyCode"></param>
    /// <returns></returns>
    [DllImport("irremote.dll")]
    static extern bool IR_GetSystemKeyCode(
      out int RepeatCount,
      out int RemoteCode,
      out int KeyCode);

    /// <summary>
    /// Unregisters window handle from Hauppauge IR driver
    /// </summary>
    /// <param name="WindowHandle"></param>
    /// <param name="Msg"></param>
    /// <returns></returns>
    [DllImport("irremote.dll")]
    static extern bool IR_Close(
      IntPtr WindowHandle,
      uint Msg);

    #endregion Interop

    #region Constants

    /// <summary>
    /// Current Version.
    /// </summary>
    public const string CurrentVersion = "2.49.23332";

    const int HCWPVR2     = 0x001E;   // 45-Button Remote
    const int HCWPVR      = 0x001F;   // 34-Button Remote
    const int HCWCLASSIC  = 0x0000;   // 21-Button Remote

    const int WM_TIMER    = 0x0113;

    #endregion Constants

    #region Variables

    ButtonReceived _buttonReceived;

    ReceiverWindow _window;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Gets or sets the button callback.
    /// </summary>
    /// <value>The button callback.</value>
    public ButtonReceived ButtonCallback
    {
      get { return _buttonReceived; }
      set { _buttonReceived = value; }
    }

    #endregion Properties

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="IrRemoteWrapper"/> class.
    /// </summary>
    public IrRemoteWrapper()
    {
      _window = new ReceiverWindow();
      _window.ProcMsg = new ProcessMessage(WndProc);
    }

    #endregion Constructor

    #region Public Methods

    /// <summary>
    /// Starts this instance.
    /// </summary>
    public void Start()
    {
      if (!IR_Open(_window.Handle, 0, false, 0))
        throw new ApplicationException("Failed to open IR device");
    }

    /// <summary>
    /// Stops this instance.
    /// </summary>
    public void Stop()
    {
      if (!IR_Close(_window.Handle, 0))
        throw new ApplicationException("Failed to close IR device");
    }

    /// <summary>
    /// Starts the ir.exe.
    /// </summary>
    public void StartIrExe()
    {
      if (Process.GetProcessesByName("Ir").Length == 0)
        Process.Start(GetHCWPath() + "Ir.exe", "/QUIET");
    }

    /// <summary>
    /// Stops the ir.exe.
    /// </summary>
    public void StopIrExe()
    {
      Process.Start(GetHCWPath() + "Ir.exe", "/QUIT");
      Thread.Sleep(500);

      if (Process.GetProcessesByName("Ir").Length != 0)
        foreach (Process proc in Process.GetProcessesByName("Ir"))
          proc.Kill();
    }

    #endregion Public Methods

    static bool IRSetDllDirectory(string PathName)
    {
      return SetDllDirectory(PathName);
    }

    static string GetHCWPath()
    {
      string dllPath = null;

      using (RegistryKey rkey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Hauppauge WinTV Infrared Remote"))
      {
        if (rkey != null)
        {
          dllPath = rkey.GetValue("UninstallString").ToString();
          if (dllPath.IndexOf("UNir32") > 0)
            dllPath = dllPath.Substring(0, dllPath.IndexOf("UNir32"));
          else if (dllPath.IndexOf("UNIR32") > 0)
            dllPath = dllPath.Substring(0, dllPath.IndexOf("UNIR32"));
        }
      }

      return dllPath;
    }

    static string GetDllPath()
    {
      string dllPath = GetHCWPath();
      if (!File.Exists(dllPath + "irremote.DLL"))
        dllPath = null;

      return dllPath;
    }

    void WndProc(ref Message m)
    {
      if (m.Msg != WM_TIMER)
        return;

      int repeatCount = 0;
      int remoteCode  = 0;
      int keyCode     = 0;
      if (!IR_GetSystemKeyCode(out repeatCount, out remoteCode, out keyCode))
        return;

      int buttonCode = keyCode;
      switch (remoteCode)
      {
        case HCWCLASSIC:  buttonCode = keyCode;         break;
        case HCWPVR:      buttonCode = keyCode + 1000;  break;
        case HCWPVR2:     buttonCode = keyCode + 2000;  break;
#if TRACE
        default:
          Trace.WriteLine("IrRemoteWrapper - Unknown Remote Code: " + remoteCode);
          break;
#endif
      }

      if (_buttonReceived != null)
        _buttonReceived(buttonCode);
    }

  }

}
