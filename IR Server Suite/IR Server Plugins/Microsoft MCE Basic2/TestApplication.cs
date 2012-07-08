#region Copyright (C) 2005-2012 Team MediaPortal

// Copyright (C) 2005-2012 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.Windows.Forms;
using IrssUtils;
using Microsoft.Win32;

namespace IRServer.Plugin
{
  public partial class MCEBasicMP
  {
#if TEST_APPLICATION

    static MCEBasicMP device;

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
        device = new MCEBasicMP();

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

        Console.WriteLine("Starting device access ...");

        device.Start();

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