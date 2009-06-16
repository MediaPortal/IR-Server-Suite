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
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace InputService.Plugin
{
  /// <summary>
  /// Callback for remote button press events.
  /// </summary>
  internal delegate void ButtonReceived(int buttonCode);

  /// <summary>
  /// Wrapper class for irremote.dll
  /// </summary>
  internal class IrRemoteWrapper
  {
    #region Interop

    /// <summary>
    /// The SetDllDirectory function adds a directory to the search path used to locate DLLs for the application.
    /// http://msdn.microsoft.com/library/en-us/dllproc/base/setdlldirectory.asp
    /// </summary>
    /// <param name="PathName">Pointer to a null-terminated string that specifies the directory to be added to the search path.</param>
    /// <returns></returns>
    [DllImport("kernel32.dll")]
    private static extern bool SetDllDirectory(
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
    private static extern bool IR_Open(
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
    private static extern bool IR_GetSystemKeyCode(
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
    private static extern bool IR_Close(
      IntPtr WindowHandle,
      uint Msg);

    #endregion Interop

    #region Constants

    /// <summary>
    /// Current Version.
    /// </summary>
    public const string CurrentVersion = "2.49.23332";

    private const int HCWCLASSIC = 0x0000; // 21-Button Remote
    private const int HCWPVR = 0x001F; // 34-Button Remote
    private const int HCWPVR2 = 0x001E; // 45-Button Remote

    private const int WM_TIMER = 0x0113;

    #endregion Constants

    #region Variables

    private readonly ReceiverWindow _window;
    private ButtonReceived _buttonReceived;

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
      string dllPath = GetDllPath();
      if (String.IsNullOrEmpty(dllPath))
        throw new DirectoryNotFoundException("Could not find IrRemote.dll folder");

      SetDllDirectory(dllPath);

      _window = new ReceiverWindow();
      _window.ProcMsg = WndProc;
    }

    #endregion Constructor

    #region Public Methods

    /// <summary>
    /// Starts this instance.
    /// </summary>
    public void Start()
    {
      if (!IR_Open(_window.Handle, 0, false, 0))
        throw new InvalidOperationException("Failed to open IR device");
    }

    /// <summary>
    /// Stops this instance.
    /// </summary>
    public void Stop()
    {
      if (!IR_Close(_window.Handle, 0))
        throw new InvalidOperationException("Failed to close IR device");
    }

    /// <summary>
    /// Starts the ir.exe.
    /// </summary>
    public void StartIrExe()
    {
      string exe = GetHCWPath() + "Ir.exe";

      if (Process.GetProcessesByName("Ir").Length == 0 && File.Exists(exe))
        Process.Start(exe, "/QUIET");
    }

    /// <summary>
    /// Stops the ir.exe.
    /// </summary>
    public void StopIrExe()
    {
      string exe = GetHCWPath() + "Ir.exe";

      if (File.Exists(exe))
      {
        Process.Start(exe, "/QUIT");
        Thread.Sleep(500);
      }

      if (Process.GetProcessesByName("Ir").Length != 0)
        foreach (Process proc in Process.GetProcessesByName("Ir"))
          proc.Kill();
    }

    #endregion Public Methods

    private static string GetHCWPath()
    {
      string dllPath = null;

      using (
        RegistryKey rkey =
          Registry.LocalMachine.OpenSubKey(
            "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Hauppauge WinTV Infrared Remote"))
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

    private static string GetDllPath()
    {
      string dllPath = GetHCWPath();
      if (!File.Exists(dllPath + "irremote.DLL"))
        dllPath = null;

      return dllPath;
    }

    private void WndProc(ref Message m)
    {
      if (m.Msg != WM_TIMER)
        return;

      int repeatCount = 0;
      int remoteCode = 0;
      int keyCode = 0;
      if (!IR_GetSystemKeyCode(out repeatCount, out remoteCode, out keyCode))
        return;

      int buttonCode = keyCode;
      switch (remoteCode)
      {
        case HCWCLASSIC:
          buttonCode = keyCode;
          break;
        case HCWPVR:
          buttonCode = keyCode + 1000;
          break;
        case HCWPVR2:
          buttonCode = keyCode + 2000;
          break;
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