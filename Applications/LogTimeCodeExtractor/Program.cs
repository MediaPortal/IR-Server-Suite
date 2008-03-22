using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LogTimeCodeExtractor
{

  class Program
  {

    static void Main(string[] args)
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
          string[] timesStrings = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

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
