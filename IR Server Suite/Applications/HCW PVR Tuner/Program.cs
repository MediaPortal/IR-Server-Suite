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
using System.Runtime.InteropServices;
using IrssUtils;

namespace HcwPvrTuner
{
  internal static class Program
  {
    #region Interop

    [DllImport("hcwIRblast.dll")]
    private static extern ushort UIR_Open(uint bVerbose, ushort wIRPort);

    [DllImport("hcwIRblast.dll")]
    private static extern int UIR_Close();

    [DllImport("hcwIRblast.dll")]
    private static extern int UIR_GetConfig(int device, int codeset, ref UIR_CFG cfgPtr);

    [DllImport("hcwIRblast.dll")]
    private static extern int UIR_GotoChannel(int device, int codeset, int channel);

    #endregion Interop

    #region Structures

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    private struct UIR_CFG
    {
      public int a; // 0x38;
      public int b;
      public int c; // Region 
      public int d; // Device
      public int e; // Vendor
      public int f; // Code Set
      public int g;
      public int h;
      public int i; // Minimum Digits
      public int j; // Digit Delay
      public int k; // Need Enter
      public int l; // Enter Delay
      public int m; // Tune Delay
      public int n; // One Digit Delay
    }

    #endregion Structures

    #region Constants

    private const int ReturnError = 1;
    private const int ReturnSuccess = 0;

    #endregion Constants

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static int Main(string[] args)
    {
      Console.WriteLine("HCW PVR Tuner");
      Console.WriteLine();

#if DEBUG
      IrssLog.LogLevel = IrssLog.Level.Debug;
#else
      IrssLog.LogLevel = IrssLog.Level.Info;
#endif
      IrssLog.Append("Dbox Tuner.log");

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
          throw new ArgumentException(String.Format("Failed to convert command line parameter ({0}) to channel number",
                                                    args[0]));

        Info("Attempting to tune channel {0} ...", channelNumber);

        int returnValue = UIR_Open(0, 0);
        if (returnValue == 0)
          throw new InvalidOperationException(String.Format("Failed to start device access ({0})", returnValue));
        else
          deviceOpen = true;

        UIR_CFG config = new UIR_CFG();
        config.a = 0x38;

        returnValue = UIR_GetConfig(-1, -1, ref config);
        if (returnValue == 0)
        {
          Info("Device configuration ...");
          Info("Device:          {0}    Vendor:         {1}", config.d, config.e);
          Info("Region:          {0}    Code Set:       {1}", config.c, config.f);
          Info("Digit Delay:     {0}    Minimum Digits: {1}", config.j, config.i);
          Info("One Digit Delay: {0}    Tune Delay:     {1}", config.n, config.m);
          Info("Need Enter:      {0}    Enter Delay:    {1}", config.k, config.l);
        }
        else
        {
          throw new InvalidOperationException(String.Format("Failed to retrieve device configuration ({0})", returnValue));
        }

        returnValue = UIR_GotoChannel(config.d, config.f, channelNumber);
        if (returnValue == 0)
          throw new InvalidOperationException(String.Format("Failed to tune channel ({0})", returnValue));
      }
      catch (Exception ex)
      {
        Info(ex.ToString());
        return ReturnError;
      }
      finally
      {
        if (deviceOpen)
          UIR_Close();
      }

      Info("Done.");

      IrssLog.Close();

      return ReturnSuccess;
    }

    private static void Info(string format, params object[] args)
    {
      string message = String.Format(format, args);
      IrssLog.Info(message);
      Console.WriteLine(message);
    }
  }
}