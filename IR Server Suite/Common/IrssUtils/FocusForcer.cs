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

    private readonly int _processId;
    //Thread _forcerThread;

    private EventWaitHandle _waitHandle;
    private IntPtr _windowHandle;

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

    #region Public Methods

    public bool ForceOnce()
    {
      Process process = Process.GetProcessById(_processId);
      if (process == null || process.HasExited)
        throw new InvalidOperationException("Cannot force focus, process is not running");

      _waitHandle = new AutoResetEvent(false);

      try
      {
        Win32.EnumWindowsProc ewc = CheckWindow;

        int focusedId = Win32.GetForegroundWindowPID();
        if (focusedId != _processId)
        {
          _windowHandle = IntPtr.Zero;
          Win32.EnumerateWindows(ewc, IntPtr.Zero);

          bool waitResult = _waitHandle.WaitOne(5000, false);
          if (waitResult && _windowHandle != IntPtr.Zero)
            return Win32.SetForegroundWindow(_windowHandle, true);
        }
      }
      finally
      {
        _waitHandle.Close();
        _waitHandle = null;
      }

      return false;
    }

    /// <summary>
    /// Forces the process into focus (once).
    /// </summary>
    /// <returns></returns>
    public bool Force()
    {
      Process process = Process.GetProcessById(_processId);
      if (process == null || process.HasExited)
        throw new InvalidOperationException("Cannot force focus, process is not running");

      //if (_forcerThread != null)
      //throw new InvalidOperationException("Cannot force focus, Forcer thread already running");

      _waitHandle = new AutoResetEvent(false);

      try
      {
        Win32.EnumWindowsProc ewc = CheckWindow;

        while (!process.HasExited)
        {
          int focusedId = Win32.GetForegroundWindowPID();
          if (focusedId != _processId)
          {
            _windowHandle = IntPtr.Zero;
            Win32.EnumerateWindows(ewc, IntPtr.Zero);

            bool waitResult = _waitHandle.WaitOne(5000, false);
            if (waitResult && _windowHandle != IntPtr.Zero)
              return Win32.SetForegroundWindow(_windowHandle, true);
          }

          Thread.Sleep(5000);
        }
      }
      finally
      {
        _waitHandle.Close();
        _waitHandle = null;
      }

      return false;
    }

    /*
    /// <summary>
    /// Starts the forcer thread.
    /// </summary>
    public void StartThread()
    {
      if (_forcerThread != null)
        throw new InvalidOperationException("Focus Forcer is already running");

      _forcerThread = new Thread(new ThreadStart(ForcerThread));
      _forcerThread.Name = "Focus Forcer Thread";
      _forcerThread.IsBackground = true;
      _forcerThread.Start();
    }

    /// <summary>
    /// Stops the forcer thread.
    /// </summary>
    public void StopThread()
    {
      if (_forcerThread == null)
        return;

      _forcerThread.Abort();
      _forcerThread = null;
    }
    */

    #endregion Public Methods

    #region Implementation

    /*
    void ForcerThread()
    {
      Process process = Process.GetProcessById(_processId);
      if (process == null)
        return;

      _waitHandle = new AutoResetEvent(false);

      try
      {
        Win32.EnumWindowsProc ewc = new Win32.EnumWindowsProc(CheckWindow);

        while (!process.HasExited)
        {
          int focusedId = Win32.GetForegroundWindowPID();
          if (focusedId != _processId)
          {
            _windowHandle = IntPtr.Zero;
            Win32.EnumerateWindows(ewc, IntPtr.Zero);

            bool waitResult = _waitHandle.WaitOne(5000, false);

#if TRACE
            Trace.WriteLine(String.Format("ForcerThread: waitResult = {0}", waitResult));
#endif

            if (_windowHandle != IntPtr.Zero)
            {
              bool result = Win32.SetForegroundWindow(_windowHandle, true);
#if TRACE
              Trace.WriteLine(String.Format("ForcerThread: SetForegroundWindow returned {0}", result));
#endif
            }
          }

          Thread.Sleep(5000);
        }
      }
      finally
      {
        _waitHandle.Close();
        _waitHandle = null;
      }
    }
    */

    private bool CheckWindow(IntPtr hWnd, IntPtr lParam)
    {
      if (hWnd == IntPtr.Zero)
      {
        _windowHandle = IntPtr.Zero;
#if TRACE
        Trace.WriteLine("CheckWindow: hWnd == IntPtr.Zero, _waitHandle.Set();");
#endif
        _waitHandle.Set();
        return false;
      }

      if (Win32.GetWindowPID(hWnd) == _processId)
      {
        _windowHandle = hWnd;
#if TRACE
        Trace.WriteLine("CheckWindow: Win32.GetWindowPID(hWnd) == _processId, _waitHandle.Set();");
#endif
        _waitHandle.Set();
        return false;
      }

      return true;
    }

    #endregion Implementation
  }
}