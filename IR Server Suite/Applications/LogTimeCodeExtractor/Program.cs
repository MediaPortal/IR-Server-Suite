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
using System.Collections.Generic;
using System.IO;

namespace LogTimeCodeExtractor
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      if (args.Length != 1)
      {
        Console.WriteLine("Usage: LogTimeCodeExtractor <filename>");
        return;
      }

      StreamReader reader = new StreamReader(args[0]);

      string line;

      List<int> times = new List<int>();
      while (!String.IsNullOrEmpty(line = reader.ReadLine()))
      {
        if (line.StartsWith("+") || line.StartsWith("-"))
        {
          string[] timesStrings = line.Split(new char[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries);

          foreach (string time in timesStrings)
          {
            int intTime = int.Parse(time);
            times.Add(intTime);
          }
        }
      }

      foreach (int time in times)
      {
        Console.Write(time);
        Console.Write(", ");
      }
    }
  }
}