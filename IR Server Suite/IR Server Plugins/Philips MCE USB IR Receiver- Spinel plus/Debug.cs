using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace IRServer.Plugin
{
  public partial class PhilipsMceUsbIrReceiverSpinelPlus
  {

    private static class Debug
    {
      private static StreamWriter _debugFile;

      /// <summary>
      /// Opens a debug output file.
      /// </summary>
      /// <param name="fileName">Name of the file.</param>
      public static void Open(string fileName)
      {
        if (_debugFile != null) return;
        try
        {
          string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                                     String.Format("IR Server Suite\\Logs\\{0}", fileName));
          _debugFile = new StreamWriter(path, false);
          _debugFile.AutoFlush = true;
        }
        catch
        {
          _debugFile = null;
        }
      }

      /// <summary>
      /// Closes the debug output file.
      /// </summary>
      public static void Close()
      {
        if (_debugFile != null)
        {
          _debugFile.Close();
          _debugFile.Dispose();
          _debugFile = null;
        }
      }

      /// <summary>
      /// Writes a line to the debug output file.
      /// </summary>
      /// <param name="line">The line.</param>
      /// <param name="args">Formatting arguments.</param>
      public static void WriteLine(string line, params object[] args)
      {
        if (_debugFile != null)
        {
          _debugFile.Write("{0:yyyy-MM-dd HH:mm:ss.ffffff} - ", DateTime.Now);
          _debugFile.WriteLine(line, args);
        }
#if TEST_APPLICATION
            Console.Write("{0:yyyy-MM-dd HH:mm:ss.ffffff} - ", DateTime.Now);
            Console.WriteLine(line, args);
#endif
      }

      /// <summary>
      /// Writes a string to the debug output file.
      /// </summary>
      /// <param name="text">The string to write.</param>
      /// <param name="args">Formatting arguments.</param>
      public static void Write(string text, params object[] args)
      {
        if (_debugFile != null)
        {
          _debugFile.Write(text, args);
        }
#if TEST_APPLICATION
            Console.Write(text, args);
#endif
      }

      /// <summary>
      /// Writes a new line to the debug output file.
      /// </summary>
      public static void WriteNewLine()
      {
        if (_debugFile != null)
        {
          _debugFile.WriteLine();
        }
#if TEST_APPLICATION
            Console.WriteLine();
#endif
      }

      /// <summary>
      /// Dumps an Array to the debug output file.
      /// </summary>
      /// <param name="array">The array.</param>
      public static void Dump(Array array)
      {
        foreach (object item in array)
        {
          if (item is byte)
          {
            Write("{0:X2}", (byte)item);
          }
          else if (item is ushort)
          {
            Write("{0:X4}", (ushort)item);
          }
          else if (item is int)
          {
            Write("{1}{0}", (int)item, (int)item > 0 ? "+" : String.Empty);
          }
          else
          {
            Write("{0}", item);
          }

          Write(", ");
        }

        WriteNewLine();
      }
    }
  }
}
