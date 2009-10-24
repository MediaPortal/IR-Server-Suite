using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace IrssUtils
{
  public static class ProcessHelper
  {
    /// <summary>
    /// Checks if the Process is already running.
    /// </summary>
    /// <returns>true if process is already running.</returns>
    public static bool IsProcessAlreadyRunning()
    {
      return IsProcessAlreadyRunning(true);
    }

    /// <summary>
    /// Checks if the Process is already running.
    /// </summary>
    /// <param name="activateMainForm">If true, the mainForm will be activated, if process is already running.</param>
    /// <returns>true if process is already running.</returns>
    public static bool IsProcessAlreadyRunning(bool activateMainForm)
    {
      Process process = RunningInstance();
      if (process == null) return false;

      // activates the main form of the application
      if (activateMainForm && process.MainWindowHandle != IntPtr.Zero)
        ShowWindow(process.MainWindowHandle, SW_SHOWNORMAL);

      return true;
    }

    public static Process RunningInstance()
    {
      Process current = Process.GetCurrentProcess();
      Process[] processes = Process.GetProcessesByName(current.ProcessName);

      // Loop through the running processes in with the same name
      foreach (Process process in processes)
      {
        // Ignore the current process
        if (process.Id == current.Id) continue;

        // Make sure that the process is running from the same exe file.
        // todo: is this really needed???
        //if (process.MainModule.FileName == current.MainModule.FileName)
        //{
          // Return the other process instance.
          return process;
        //}
      }
      
      // No other instance was found, return null
      return null;
    }

    #region InterOp

    private const int SW_SHOWNORMAL = 1;
    [DllImport("user32.dll")]
    private static extern IntPtr ShowWindow(IntPtr hwnd, int nCmdShow);

    #endregion
  }
}
