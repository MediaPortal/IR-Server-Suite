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
#if TRACE
using System.Diagnostics;
#endif
using System.IO;
using System.Reflection;
using MediaPortal.Core.Logging;
using MediaPortal.Core.Services.Logging;

namespace IrssUtils
{
  /// <summary>
  /// Log file recording class.
  /// </summary>
  public static class IrssLog
  {
    #region Constants

    /// <summary>
    /// File extension for log file backups.
    /// </summary>
    public const string ExtensionBackupFile = ".old.log";

    #endregion Constants

    #region Enumerations

    /// <summary>
    /// Log detail levels.
    /// </summary>
    public enum Level
    {
      /// <summary>
      /// Do not log any messages.
      /// </summary>
      Off = 0,
      /// <summary>
      /// Log only Error messages.
      /// </summary>
      Error = 1,
      /// <summary>
      /// Log only Warning and Error messages.
      /// </summary>
      Warn = 2,
      /// <summary>
      /// Log only Warning, Error and Information messages.
      /// </summary>
      Info = 3,
      /// <summary>
      /// Log all messages.
      /// </summary>
      Debug = 4,
    }

    #endregion Enumerations

    #region Variables

    private static StreamWriter _streamWriter;

    #endregion Variables

    static IrssLog()
    {
      LogLevel = Level.Debug;
    }

    #region Properties

    /// <summary>
    /// Level of detail to record in log file.
    /// </summary>
    /// <value>The log level.</value>
    public static Level LogLevel { get; set; }

    public static ILogger Logger { get; set; }

    #endregion Properties

    #region Implementation

    #region Log file opening and closing

    /// <summary>
    /// Open a log file to record to.
    /// </summary>
    /// <param name="fileName">Log file name.</param>
    public static void Open(string fileName)
    {
      string filePath = Path.Combine(Common.FolderIrssLogs, fileName);

      if (File.Exists(filePath))
      {
        try
        {
          string backup = Path.ChangeExtension(filePath, ExtensionBackupFile);

          if (File.Exists(backup))
            File.Delete(backup);

          File.Move(filePath, backup);
        }
        catch (Exception ex)
        {
          Logger.Error(ex);
        }
      }

      Logger = FileLogger.CreateFileLogger(filePath, MediaPortal.Core.Logging.LogLevel.All, false, true);

      Logger.Info("Log Opened");
      Logger.Info(Assembly.GetCallingAssembly().FullName);
    }

    /// <summary>
    /// Open a log file to append log entries to.
    /// </summary>
    /// <param name="fileName">Log file name.</param>
    [Obsolete]
    public static void Append(string fileName)
    {
      if (_streamWriter != null || LogLevel == Level.Off)
        return;

      string filePath = Path.Combine(Common.FolderIrssLogs, fileName);

      try
      {
        if (File.Exists(filePath) &&
            File.GetCreationTime(filePath).Ticks < DateTime.Now.Subtract(TimeSpan.FromDays(7)).Ticks)
        {
          string backup = Path.ChangeExtension(filePath, ExtensionBackupFile);

          if (File.Exists(backup))
            File.Delete(backup);

          File.Move(filePath, backup);
        }
      }
#if TRACE
      catch (Exception ex)
      {
        Trace.WriteLine(ex.ToString());
      }
#else
      catch
      {
      }
#endif

      try
      {
        _streamWriter = new StreamWriter(filePath, true);
        _streamWriter.AutoFlush = true;

        string message = String.Format("{0:yyyy-MM-dd HH:mm:ss.ffffff} - Log Opened", DateTime.Now);
        _streamWriter.WriteLine(message);
#if TRACE
        Trace.WriteLine(message);
#endif
      }
#if TRACE
      catch (Exception ex)
      {
        Trace.WriteLine(ex.ToString());
      }
#else
      catch
      {
      }
#endif
    }

    /// <summary>
    /// Close the currently open log file.
    /// </summary>
    public static void Close()
    {
      Logger.Info("Log Closed");
    }

    #endregion Log file opening and closing

    #region Log recording methods

    /// <summary>
    /// Log an Error.
    /// </summary>
    /// <param name="ex">Exception to log.</param>
    public static void Error(Exception ex)
    {
      Logger.Error(ex);
    }

    /// <summary>
    /// Log an Error.
    /// </summary>
    /// <param name="format">String format.</param>
    /// <param name="args">String format arguments.</param>
    public static void Error(string format, params object[] args)
    {
      Logger.Error(format, args);
    }

    /// <summary>
    /// Log an Error.
    /// </summary>
    /// <param name="format">String format.</param>
    /// <param name="ex">Exception to log.</param>
    /// <param name="args">String format arguments.</param>
    public static void Error(string format, Exception ex, params object[] args)
    {
      Logger.Error(format, ex, args);
    }

    /// <summary>
    /// Log a Warning.
    /// </summary>
    /// <param name="format">String format.</param>
    /// <param name="args">String format arguments.</param>
    public static void Warn(string format, params object[] args)
    {
      Logger.Warn(format, args);
    }

    /// <summary>
    /// Log a Warning.
    /// </summary>
    /// <param name="format">String format.</param>
    /// <param name="ex">Exception to log.</param>
    /// <param name="args">String format arguments.</param>
    public static void Warn(string format, Exception ex, params object[] args)
    {
      Logger.Warn(format, ex, args);
    }

    /// <summary>
    /// Log Information.
    /// </summary>
    /// <param name="format">String format.</param>
    /// <param name="args">String format arguments.</param>
    public static void Info(string format, params object[] args)
    {
      Logger.Info(format, args);
    }

    /// <summary>
    /// Log Information.
    /// </summary>
    /// <param name="format">String format.</param>
    /// <param name="ex">Exception to log.</param>
    /// <param name="args">String format arguments.</param>
    public static void Info(string format, Exception ex, params object[] args)
    {
      Logger.Info(format, ex, args);
    }

    /// <summary>
    /// Log a Debug message.
    /// </summary>
    /// <param name="format">String format.</param>
    /// <param name="args">String format arguments.</param>
    public static void Debug(string format, params object[] args)
    {
      Logger.Debug(format, args);
    }

    /// <summary>
    /// Log a Debug message.
    /// </summary>
    /// <param name="format">String format.</param>
    /// <param name="ex">Exception to log.</param>
    /// <param name="args">String format arguments.</param>
    public static void Debug(string format, Exception ex, params object[] args)
    {
      Logger.Debug(format, ex, args);
    }

    #endregion Log recording methods

    #endregion Implementation
  }
}