using System;
using System.Runtime.InteropServices;

namespace HcwPvrTuner
{

  static class Program
  {

    #region Interop

    [DllImport("hcwIRblast.dll")]
    static extern ushort UIR_Open(uint bVerbose, ushort wIRPort);

    [DllImport("hcwIRblast.dll")]
    static extern int UIR_Close();

    [DllImport("hcwIRblast.dll")]
    static extern int UIR_GetConfig(int device, int codeset, ref UIR_CFG cfgPtr);

    [DllImport("hcwIRblast.dll")]
    static extern int UIR_GotoChannel(int device, int codeset, int channel);

    #endregion Interop

    #region Structures

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    struct UIR_CFG
    {
      public int a;   // 0x38;
      public int b;
      public int c;   // Region 
      public int d;   // Device
      public int e;   // Vendor
      public int f;   // Code Set
      public int g;
      public int h;
      public int i;   // Minimum Digits
      public int j;   // Digit Delay
      public int k;   // Need Enter
      public int l;   // Enter Delay
      public int m;   // Tune Delay
      public int n;   // One Digit Delay
    }

    #endregion Structures

    #region Constants

    const int ReturnError   = 1;
    const int ReturnSuccess = 0;

    #endregion Constants

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static int Main(string[] args)
    {
      Console.WriteLine("HCW PVR Tuner");
      Console.WriteLine();

      if (args.Length != 1)
      {
        Console.WriteLine("Usage:");
        Console.WriteLine("       HcwPvrTuner.exe <Channel Number>");
        Console.WriteLine();
        return ReturnError;
      }

      bool deviceOpen = false;

      try
      {
        int channelNumber;
        if (!int.TryParse(args[0], out channelNumber))
          throw new ApplicationException(String.Format("Failed to convert command line parameter ({0}) to channel number", args[0]));

        Console.WriteLine("Attempting to tune channel {0} ...", channelNumber);
        Console.WriteLine();

        int returnValue = UIR_Open(0, 0);
        if (returnValue == 0)
          throw new ApplicationException(String.Format("Failed to start device access ({0})", returnValue));
        else
          deviceOpen = true;

        UIR_CFG config = new UIR_CFG();
        config.a = 0x38;

        returnValue = UIR_GetConfig(-1, -1, ref config);
        if (returnValue == 0)
        {
          Console.WriteLine("Device configuration ...");
          Console.WriteLine();
          Console.WriteLine("Device:          {0}    Vendor:         {1}", config.d, config.e);
          Console.WriteLine("Region:          {0}    Code Set:       {1}", config.c, config.f);
          Console.WriteLine("Digit Delay:     {0}    Minimum Digits: {1}", config.j, config.i);
          Console.WriteLine("One Digit Delay: {0}    Tune Delay:     {1}", config.n, config.m);
          Console.WriteLine("Need Enter:      {0}    Enter Delay:    {1}", config.k, config.l);
          Console.WriteLine();
        }
        else
        {
          throw new ApplicationException(String.Format("Failed to retrieve device configuration ({0})", returnValue));
        }

        returnValue = UIR_GotoChannel(config.d, config.f, channelNumber);
        if (returnValue == 0)
          throw new ApplicationException(String.Format("Failed to tune channel ({0})", returnValue));
      }
      catch (ApplicationException ex)
      {
        Console.WriteLine("Error: {0}", ex.Message);
        return ReturnError;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        return ReturnError;
      }
      finally
      {
        if (deviceOpen)
          UIR_Close();
      }

      Console.WriteLine("Done.");
      return ReturnSuccess;
    }

  }

}
