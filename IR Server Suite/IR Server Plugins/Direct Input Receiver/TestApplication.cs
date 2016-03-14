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

namespace IRServer.Plugin
{
  /// <summary>
  /// IR Server Plugin for Direct Input game controllers.
  /// </summary>
  public partial class DirectInputReceiver
  {
    // #define TEST_APPLICATION in the project properties when creating the console test app ...
#if TEST_APPLICATION

    private static void Remote(string deviceName, string code)
    {
      Console.WriteLine("Remote: {0}", code);
    }

    private static void Mouse(string deviceName, int x, int y, int buttons)
    {
      Console.WriteLine("Mouse: ({0}, {1}) - {2}", x, y, buttons);
    }

    [STAThread]
    private static void Main()
    {
      DirectInputReceiver c = new DirectInputReceiver();

      c.Configure(null);

      c.RemoteCallback += Remote;
      c.MouseCallback += Mouse;

      c.Start();

      Application.Run();

      c.Stop();
      c = null;
    }

#endif
  }
}