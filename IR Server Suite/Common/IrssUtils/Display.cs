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
using System.Drawing;
using System.Runtime.InteropServices;

namespace IrssUtils
{
  /// <summary>
  /// Used to interogate and modify display settings.
  /// </summary>
  public static class Display
  {
    #region Constants

    private const int CDS_FULLSCREEN = 4;
    private const int CDS_TEST = 2;
    private const int CDS_UPDATEREGISTRY = 1;

    private const int DISP_CHANGE_FAILED = -1;
    private const int DISP_CHANGE_RESTART = 1;
    private const int DISP_CHANGE_SUCCESSFUL = 0;
    private const int ENUM_CURRENT_SETTINGS = -1;
    private const int MONITOR_OFF = 2;

    private const int MONITOR_ON = -1;
    private const int MONITOR_STANBY = 1;
    private const int SC_MONITORPOWER = 0xF170;
    private const int WM_SYSCOMMAND = 0x0112;

    #endregion Constants

    #region Interop

    [DllImport("user32.dll")]
    private static extern int EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);

    [DllImport("user32.dll")]
    private static extern int ChangeDisplaySettings(ref DEVMODE devMode, int flags);

    #endregion Interop

    #region Structures

    [StructLayout(LayoutKind.Sequential)]
    private struct DEVMODE
    {
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] public string dmDeviceName;
      public short dmSpecVersion;
      public short dmDriverVersion;
      public short dmSize;
      public short dmDriverExtra;
      public int dmFields;

      public short dmOrientation;
      public short dmPaperSize;
      public short dmPaperLength;
      public short dmPaperWidth;

      public short dmScale;
      public short dmCopies;
      public short dmDefaultSource;
      public short dmPrintQuality;
      public short dmColor;
      public short dmDuplex;
      public short dmYResolution;
      public short dmTTOption;
      public short dmCollate;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] public string dmFormName;
      public short dmLogPixels;
      public short dmBitsPerPel;
      public int dmPelsWidth;
      public int dmPelsHeight;

      public int dmDisplayFlags;
      public int dmDisplayFrequency;

      public int dmICMMethod;
      public int dmICMIntent;
      public int dmMediaType;
      public int dmDitherType;
      public int dmReserved1;
      public int dmReserved2;

      public int dmPanningWidth;
      public int dmPanningHeight;
    }

    #endregion Structures

    /// <summary>
    /// Gets the device mode.
    /// </summary>
    /// <returns>Current device mode.</returns>
    private static DEVMODE GetDevMode()
    {
      DEVMODE devMode = new DEVMODE();
      devMode.dmDeviceName = new String(new char[32]);
      devMode.dmFormName = new String(new char[32]);
      devMode.dmSize = (short) Marshal.SizeOf(typeof (DEVMODE));

      if (EnumDisplaySettings(null, ENUM_CURRENT_SETTINGS, ref devMode) == 0)
        throw new InvalidOperationException("Failed to enumerate display settings");

      return devMode;
    }

    /// <summary>
    /// Gets the resolution.
    /// </summary>
    /// <returns>The current screen resolution.</returns>
    public static Size GetResolution()
    {
      DEVMODE devMode = GetDevMode();

      return new Size(devMode.dmPelsWidth, devMode.dmPelsHeight);
    }

    /// <summary>
    /// Gets the BPP.
    /// </summary>
    /// <returns>The current screen Bits Per Pixel.</returns>
    public static int GetBpp()
    {
      DEVMODE devMode = GetDevMode();

      return devMode.dmBitsPerPel;
    }

    /// <summary>
    /// Gets the refresh rate.
    /// </summary>
    /// <returns>The current refresh rate in Hertz.</returns>
    public static int GetRefreshRate()
    {
      DEVMODE devMode = GetDevMode();

      return devMode.dmDisplayFrequency;
    }

    /// <summary>
    /// Changes the display mode.
    /// </summary>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    public static void ChangeDisplayMode(int width, int height)
    {
      ChangeDisplayMode(height, width, -1, -1);
    }

    /// <summary>
    /// Changes the display mode.
    /// </summary>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="bpp">The BPP.</param>
    public static void ChangeDisplayMode(int width, int height, short bpp)
    {
      ChangeDisplayMode(height, width, bpp, -1);
    }

    /// <summary>
    /// Changes the display mode.
    /// </summary>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="bpp">The BPP.</param>
    /// <param name="refreshRate">The refresh rate in Hertz.</param>
    public static void ChangeDisplayMode(int width, int height, short bpp, int refreshRate)
    {
      DEVMODE devMode = GetDevMode();

      devMode.dmPelsWidth = width;
      devMode.dmPelsHeight = height;

      if (bpp != -1) devMode.dmBitsPerPel = bpp;
      if (refreshRate != -1) devMode.dmDisplayFrequency = refreshRate;

      int test = ChangeDisplaySettings(ref devMode, CDS_TEST);
      if (test != DISP_CHANGE_SUCCESSFUL)
        throw new InvalidOperationException(String.Format("Testing display mode change failed ({0})", test));

      int set = ChangeDisplaySettings(ref devMode, CDS_UPDATEREGISTRY);
      if (set != DISP_CHANGE_SUCCESSFUL)
        throw new InvalidOperationException(String.Format("Setting display mode failed ({0})", set));
    }

    /// <summary>
    /// Sets the display power state.
    /// </summary>
    /// <param name="powerState">New display power state.</param>
    public static void SetPowerState(int powerState)
    {
      IntPtr desktop = Win32.GetDesktopWindowHandle();
      if (desktop == IntPtr.Zero)
        throw new InvalidOperationException("Failed to get handle to dekstop");

      Win32.SendWindowsMessage(desktop, WM_SYSCOMMAND, SC_MONITORPOWER, powerState);
    }
  }
}