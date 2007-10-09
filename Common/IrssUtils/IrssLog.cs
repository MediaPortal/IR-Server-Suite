using System;
using System.Diagnostics;
using System.IO;

namespace IrssUtils
{

  /// <summary>
  /// Log file recording class.
  /// </summary>
  public static class IrssLog
  {

    #region Enumerations

    /// <summary>
    /// Log detail levels.
    /// </summary>
    public enum Level
    {
      /// <summary>
      /// Do not log any messages.
      /// </summary>
      Off   = 0,
      /// <summary>
      /// Log only Error messages.
      /// </summary>
      Error = 1,
      /// <summary>
      /// Log only Warning and Error messages.
      /// </summary>
      Warn  = 2,
      /// <summary>
      /// Log only Warning, Error and Information messages.
      /// </summary>
      Info  = 3,
      /// <summary>
      /// Log all messages.
      /// </summary>
      Debug = 4,
    }

    #endregion Enumerations

    #region Variables

    static Level _logLevel = Level.Debug;
    static StreamWriter _streamWriter;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Level of detail to record in log file.
    /// </summary>
    /// <value>The log level.</value>
    public static Level LogLevel
    {
      get { return _logLevel; }
      set { _logLevel = value; }
    }

    #endregion Properties

    #region Implementation

    #region Log file opening and closing

    /// <summary>
    /// Open a log file to record to.
    /// </summary>
    /// <param name="fileName">File path, absolute.</param>
    public static void Open(string fileName)
    {
      if (_streamWriter == null && _logLevel > Level.Off)
      {
        if (File.Exists(fileName))
        {
          try
          {
            string backup = Path.ChangeExtension(fileName, ".bak");

            if (File.Exists(backup))
              File.Delete(backup);

            File.Move(fileName, backup);
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

        try
        {
          _streamWriter = new StreamWriter(fileName, false);
          _streamWriter.AutoFlush = true;

          string message = DateTime.Now.ToString() + ":\tLog Opened";
          _streamWriter.WriteLine(message);
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
    }

    /// <summary>
    /// Open a log file to append log entries to.
    /// </summary>
    /// <param name="fileName">File path, absolute.</param>
    public static void Append(string fileName)
    {
      if (_streamWriter == null && _logLevel > Level.Off)
      {
        try
        {
          if (File.Exists(fileName) && File.GetCreationTime(fileName).Ticks < DateTime.Now.Subtract(TimeSpan.FromDays(7)).Ticks)
          {
            string backup = Path.ChangeExtension(fileName, ".bak");

            if (File.Exists(backup))
              File.Delete(backup);

            File.Move(fileName, backup);
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
          _streamWriter = new StreamWriter(fileName, true);
          _streamWriter.AutoFlush = true;

          string message = DateTime.Now.ToString() + ":\tLog Opened";
          _streamWriter.WriteLine(message);
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
    }
    
    /// <summary>
    /// Close the currently open log file.
    /// </summary>
    public static void Close()
    {
      if (_streamWriter == null)
        return;

      try
      {
        string message = DateTime.Now.ToString() + ":\tLog Closed";
        _streamWriter.WriteLine(message);
        _streamWriter.WriteLine();
      }
      catch
      {
        throw;
      }
      finally
      {
        _streamWriter.Dispose();
        _streamWriter = null;
      }
    }

    #endregion Log file opening and closing

    #region Log recording methods

    /// <summary>
    /// Log an Error.
    /// </summary>
    /// <param name="format">String format.</param>
    /// <param name="args">String format arguments.</param>
    public static void Error(string format, params object[] args)
    {
      if (_streamWriter != null && _logLevel >= Level.Error)
      {
        string message = DateTime.Now.ToString() + " - Error:\t" + String.Format(format, args);
        _streamWriter.WriteLine(message);
#if TRACE
        Trace.WriteLine(message);
#endif
      }
    }
    
    /// <summary>
    /// Log a Warning.
    /// </summary>
    /// <param name="format">String format.</param>
    /// <param name="args">String format arguments.</param>
    public static void Warn(string format, params object[] args)
    {
      if (_streamWriter != null && _logLevel >= Level.Warn)
      {
        string message = DateTime.Now.ToString() + " - Warn: \t" + String.Format(format, args);
        _streamWriter.WriteLine(message);
#if TRACE
        Trace.WriteLine(message);
#endif
      }
    }
    
    /// <summary>
    /// Log Information.
    /// </summary>
    /// <param name="format">String format.</param>
    /// <param name="args">String format arguments.</param>
    public static void Info(string format, params object[] args)
    {
      if (_streamWriter != null && _logLevel >= Level.Info)
      {
        string message = DateTime.Now.ToString() + " - Info: \t" + String.Format(format, args);
        _streamWriter.WriteLine(message);
#if TRACE
        Trace.WriteLine(message);
#endif
      }
    }
    
    /// <summary>
    /// Log a Debug message.
    /// </summary>
    /// <param name="format">String format.</param>
    /// <param name="args">String format arguments.</param>
    public static void Debug(string format, params object[] args)
    {
      if (_streamWriter != null && _logLevel >= Level.Debug)
      {
        string message = DateTime.Now.ToString() + " - Debug:\t" + String.Format(format, args);
        _streamWriter.WriteLine(message);
#if TRACE
        Trace.WriteLine(message);
#endif
      }
    }

    #endregion Log recording methods

    #endregion Implementation

  }

}
