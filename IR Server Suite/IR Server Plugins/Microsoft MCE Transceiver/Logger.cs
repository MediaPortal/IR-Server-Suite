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
using IrssUtils;

namespace IRServer.Plugin
{
  public partial class MicrosoftMceTransceiver
  {
    private static class Logger
    {
      public static void Debug(string line, params object[] args)
      {
#if TEST_APPLICATION
        Console.WriteLine(line, args);
#else
        IrssLog.Debug(line, args);
#endif
      }

      public static void Info(string line, params object[] args)
      {
#if TEST_APPLICATION
        Console.WriteLine(line, args);
#else
        IrssLog.Info(line, args);
#endif
      }

      public static void Warning(string line, params object[] args)
      {
#if TEST_APPLICATION
        Console.WriteLine(line, args);
#else
        IrssLog.Warn(line, args);
#endif
      }

      public static void Error(string line, params object[] args)
      {
#if TEST_APPLICATION
        Console.WriteLine(line, args);
#else
        IrssLog.Error(line, args);
#endif
      }
    }
  }
}