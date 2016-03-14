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
using System.Windows.Forms;
using IrssUtils;
using Microsoft.Win32;

namespace IRServer.Plugin
{
  public partial class MicrosoftMceTransceiver
  {
    // #define TEST_APPLICATION in the project properties when creating the console test app ...
#if TEST_APPLICATION

    static MicrosoftMceTransceiver device;

    static void xRemote(string deviceName, string code)
    {
      Console.WriteLine("Remote: {0}", code);
    }
    static void xKeyboard(string deviceName, int button, bool up)
    {
      char chr = Keyboard.GetCharFromVKey((Keyboard.VKey)button);

      Console.WriteLine("Keyboard: {0}, {1} - \"{2}\"", button, up, chr);
    }
    static void xMouse(string deviceName, int x, int y, int buttons)
    {
      Console.WriteLine("Mouse: ({0}, {1}) - {2}", x, y, buttons);
    }

    static void Dump(int[] timingData)
    {
      foreach (int time in timingData)
        Console.Write("{0}, ", time);
      Console.WriteLine();
    }

    [STAThread]
    static void Main()
    {
      Console.WriteLine("Microsoft MCE Transceiver Test App");
      Console.WriteLine("====================================");
      Console.WriteLine();

      SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);

      try
      {
        device = new MicrosoftMceTransceiver();

        //Keyboard.LoadLayout(Keyboard.German_DE);

        Console.Write("Configure device? (y/n) ");

        if (Console.ReadKey().Key == ConsoleKey.Y)
        {
          Console.WriteLine();

          Console.WriteLine("Configuring ...");
          device.Configure(null);
        }
        else
        {
          Console.WriteLine();
        }

        device.RemoteCallback += new RemoteHandler(xRemote);
        device.KeyboardCallback += new KeyboardHandler(xKeyboard);
        device.MouseCallback += new MouseHandler(xMouse);

        Console.WriteLine("Starting device access ...");

        device.Start();

        Console.Write("Learn IR? (y/n) ");

        while (Console.ReadKey().Key == ConsoleKey.Y)
        {
          Console.WriteLine();
          Console.WriteLine("Learning IR Command ...");

          byte[] data;

          switch (device.Learn(out data))
          {
            case LearnStatus.Failure:
              Console.WriteLine("Learn process failed!");
              break;

            case LearnStatus.Success:
              Console.WriteLine("Learn successful");

              Console.Write("Blast IR back? (y/n) ");

              if (Console.ReadKey().Key == ConsoleKey.Y)
              {
                Console.WriteLine();
                Console.WriteLine("Blasting ...");

                if (device.Transmit("Both", data))
                {
                  Console.WriteLine("Blasting successful");
                }
                else
                {
                  Console.WriteLine("Blasting failure!");
                }
              }
              else
              {
                Console.WriteLine();
              }
              break;

            case LearnStatus.Timeout:
              Console.WriteLine("Learn process timed-out");
              break;
          }

          Console.Write("Learn another IR? (y/n) ");
        }
        Console.WriteLine();
        Console.WriteLine();

        Console.WriteLine("Press a button on your remote ...");

        Application.Run();

        device.Stop();
      }
      catch (Exception ex)
      {
        Console.WriteLine("Error:");
        Console.WriteLine(ex.ToString());
        Console.WriteLine();
        Console.WriteLine("");

        Console.ReadKey();
      }
      finally
      {
        device = null;
      }

      SystemEvents.PowerModeChanged -= new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);
    }

    static void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
    {
      Console.WriteLine("Power Event: {0}", Enum.GetName(typeof(PowerModes), e.Mode));

      switch (e.Mode)
      {

        case PowerModes.Suspend:
          if (device != null)
            device.Suspend();
          break;

        case PowerModes.Resume:
          if (device != null)
            device.Resume();
          break;

      }
    }

#endif
  }
}