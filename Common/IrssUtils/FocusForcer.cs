using System;
using System.Diagnostics;
using System.Threading;

namespace IrssUtils
{

  /// <summary>
  /// Used to monitor a process and force windows to give that process focus.
  /// </summary>
  public class FocusForcer
  {

    #region Variables

    int _processId;
    Thread _forcerThread;
    IntPtr _windowHandle;

    #endregion Variables

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="FocusForcer"/> class.
    /// </summary>
    /// <param name="processId">The ID of the process to monitor.</param>
    public FocusForcer(int processId)
    {
      _processId = processId;
    }

    #endregion Constructor


    public void Start()
    {
      if (_forcerThread != null)
        throw new InvalidOperationException("Focus Forcer is already running");

      _forcerThread = new Thread(new ThreadStart(ForcerThread));
      _forcerThread.Name = "Focus Forcer";
      _forcerThread.IsBackground = true;
      _forcerThread.Start();
    }
    
    public void Stop()
    {
      if (_forcerThread == null)
        return;

      _forcerThread.Abort();
      _forcerThread = null;
    }

    void ForcerThread()
    {
      Process process = Process.GetProcessById(_processId);

      if (process == null || process.HasExited)
        return;

      Win32.EnumWindowsProc ewc = new Win32.EnumWindowsProc(CheckWindow);            

      while (!process.HasExited)
      {
        int focused = Win32.GetForegroundWindowPID();
        if (focused != _processId)
          Win32.EnumerateWindows(ewc, IntPtr.Zero);

        Thread.Sleep(5000);
      }
    }

    bool CheckWindow(IntPtr hWnd, IntPtr lParam)
    {
      if (hWnd == IntPtr.Zero)
      {
        _windowHandle = IntPtr.Zero;
        return false;
      }

      if (Win32.GetWindowPID(hWnd) == _processId)
      {
        bool result = Win32.SetForegroundWindow(hWnd, true);
#if TRACE
        Trace.WriteLine(String.Format("SetForegroundWindow: {0}", result));
#endif
        return false;
      }

      return true;
    }

  }

}
