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

    const int ENUM_CURRENT_SETTINGS   = -1;
    
    const int CDS_UPDATEREGISTRY      = 1;
    const int CDS_TEST                = 2;
    const int CDS_FULLSCREEN          = 4;
    
    const int DISP_CHANGE_SUCCESSFUL  = 0;
    const int DISP_CHANGE_RESTART     = 1;
    const int DISP_CHANGE_FAILED      = -1;

    #endregion Constants

    #region Interop

    [DllImport("user32.dll")]
    static extern int EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);
    
    [DllImport("user32.dll")]
    static extern int ChangeDisplaySettings(ref DEVMODE devMode, int flags);

    #endregion Interop

    #region Structures

    [StructLayout(LayoutKind.Sequential)]
    struct DEVMODE
    {
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
      public string dmDeviceName;
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
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
      public string dmFormName;
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

    static DEVMODE GetDevMode()
    {
      DEVMODE devMode = new DEVMODE();
      devMode.dmDeviceName  = new String(new char[32]);
      devMode.dmFormName    = new String(new char[32]);
      devMode.dmSize        = (short)Marshal.SizeOf(typeof(DEVMODE));

      if (EnumDisplaySettings(null, ENUM_CURRENT_SETTINGS, ref devMode) == 0)
        throw new ApplicationException("Failed to enumerate display settings");

      return devMode;
    }

    public static Size GetResolution()
    {
      DEVMODE devMode = GetDevMode();

      return new Size(devMode.dmPelsWidth, devMode.dmPelsHeight);
    }

    public static int GetBpp()
    {
      DEVMODE devMode = GetDevMode();

      return devMode.dmBitsPerPel;
    }

    public static int GetRefreshRate()
    {
      DEVMODE devMode = GetDevMode();

      return devMode.dmDisplayFrequency;
    }

    public static void ChangeDisplayMode(int width, int height)
    {
      ChangeDisplayMode(height, width, -1, -1);
    }

    public static void ChangeDisplayMode(int width, int height, short bpp)
    {
      ChangeDisplayMode(height, width, bpp, -1);
    }

    public static void ChangeDisplayMode(int width, int height, short bpp, int refreshRate)
    {
      DEVMODE devMode = GetDevMode();

      devMode.dmPelsWidth   = width;
      devMode.dmPelsHeight  = height;

      if (bpp != -1)          devMode.dmBitsPerPel = bpp;
      if (refreshRate != -1)  devMode.dmDisplayFrequency = refreshRate;

      int test = ChangeDisplaySettings(ref devMode, CDS_TEST);
      if (test != DISP_CHANGE_SUCCESSFUL)
        throw new ApplicationException(String.Format("Testing display mode change failed ({0})", test));

      int set = ChangeDisplaySettings(ref devMode, CDS_UPDATEREGISTRY);
      if (set != DISP_CHANGE_SUCCESSFUL)
        throw new ApplicationException(String.Format("Setting display mode failed ({0})", set));
    }

  }

}
