using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using InputService.Plugin.Properties;
using Microsoft.Win32.SafeHandles;

namespace InputService.Plugin
{
  /// <summary>
  /// IR Server plugin supporting the IgorPlug device.
  /// </summary>
  public class IgorPlugReceiver : PluginBase, IRemoteReceiver
  {
    #region Constants

    private const int DEVICE_NOT_PRESENT = 1;
    private const int DeviceBuffer = 256;
    private const string DevicePath = @"\\.\IgorPlugUSB_0";
    private const int FNCNumberDoGetInfraCode = 2; // transmit of receved infra code (if some code in infra buffer)
    private const int FNCNumberDoSetInfraBufferEmpty = 1; // restart of infra reading (if was stopped by RAM reading)
    private const int INVALID_BAUDRATE = 3;
    private const int NO_DATA_AVAILABLE = 2;
    private const int NO_ERROR = 0;
    private const int OVERRUN_ERROR = 4;

    private const double TimeCodeMultiplier = 85.3;

    #endregion Constants

    #region Enumerations

    #region Nested type: CreateFileAccessTypes

    [Flags]
    private enum CreateFileAccessTypes : uint
    {
      GenericRead = 0x80000000,
      GenericWrite = 0x40000000,
      GenericExecute = 0x20000000,
      GenericAll = 0x10000000,
    }

    #endregion

    #region Nested type: CreateFileAttributes

    [Flags]
    private enum CreateFileAttributes : uint
    {
      None = 0x00000000,
      Readonly = 0x00000001,
      Hidden = 0x00000002,
      System = 0x00000004,
      Directory = 0x00000010,
      Archive = 0x00000020,
      Device = 0x00000040,
      Normal = 0x00000080,
      Temporary = 0x00000100,
      SparseFile = 0x00000200,
      ReparsePoint = 0x00000400,
      Compressed = 0x00000800,
      Offline = 0x00001000,
      NotContentIndexed = 0x00002000,
      Encrypted = 0x00004000,
      Write_Through = 0x80000000,
      Overlapped = 0x40000000,
      NoBuffering = 0x20000000,
      RandomAccess = 0x10000000,
      SequentialScan = 0x08000000,
      DeleteOnClose = 0x04000000,
      BackupSemantics = 0x02000000,
      PosixSemantics = 0x01000000,
      OpenReparsePoint = 0x00200000,
      OpenNoRecall = 0x00100000,
      FirstPipeInstance = 0x00080000,
    }

    #endregion

    #region Nested type: CreateFileDisposition

    private enum CreateFileDisposition : uint
    {
      None = 0,
      New = 1,
      CreateAlways = 2,
      OpenExisting = 3,
      OpenAlways = 4,
      TruncateExisting = 5,
    }

    #endregion

    #region Nested type: CreateFileShares

    [Flags]
    private enum CreateFileShares : uint
    {
      None = 0x00,
      Read = 0x01,
      Write = 0x02,
      Delete = 0x04,
    }

    #endregion

    #endregion Enumerations

    #region Interop

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool DeviceIoControl(
      SafeFileHandle handle,
      uint ioControlCode,
      byte[] inBuffer, int inBufferSize,
      byte[] outBuffer, int outBufferSize,
      out int bytesReturned,
      IntPtr overlapped);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern SafeFileHandle CreateFile(
      [MarshalAs(UnmanagedType.LPTStr)] string fileName,
      [MarshalAs(UnmanagedType.U4)] CreateFileAccessTypes fileAccess,
      [MarshalAs(UnmanagedType.U4)] CreateFileShares fileShare,
      IntPtr securityAttributes,
      [MarshalAs(UnmanagedType.U4)] CreateFileDisposition creationDisposition,
      [MarshalAs(UnmanagedType.U4)] CreateFileAttributes flags,
      IntPtr templateFile);

    #endregion Interop

    #region Variables

    private IrProtocol _lastRemoteButtonCodeType = IrProtocol.None;
    private uint _lastRemoteButtonKeyCode;
    private DateTime _lastRemoteButtonTime = DateTime.Now;
    private Thread _readThread;
    private bool _remoteButtonRepeated;
    private int _remoteFirstRepeat = 400;
    private RemoteHandler _remoteHandler;
    private int _remoteHeldRepeats = 250;

    #endregion Variables

    #region Implementation

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name
    {
      get { return "IgorPlug"; }
    }

    /// <summary>
    /// IR Server plugin version.
    /// </summary>
    /// <value>The version.</value>
    public override string Version
    {
      get { return "1.4.2.0"; }
    }

    /// <summary>
    /// The IR Server plugin's author.
    /// </summary>
    /// <value>The author.</value>
    public override string Author
    {
      get { return "and-81"; }
    }

    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    /// <value>The description.</value>
    public override string Description
    {
      get { return "IgorPlug USB Receiver"; }
    }

    /// <summary>
    /// Gets a display icon for the plugin.
    /// </summary>
    /// <value>The icon.</value>
    public override Icon DeviceIcon
    {
      get { return Resources.Icon; }
    }

    /// <summary>
    /// Callback for remote button presses.
    /// </summary>
    public RemoteHandler RemoteCallback
    {
      get { return _remoteHandler; }
      set { _remoteHandler = value; }
    }

    /// <summary>
    /// Detect the presence of this device.  Devices that cannot be detected will always return false.
    /// This method should not throw exceptions.
    /// </summary>
    /// <returns><c>true</c> if the device is present, otherwise <c>false</c>.</returns>
    public override bool Detect()
    {
      try
      {
        SafeFileHandle deviceHandle = CreateFile(DevicePath,
                                                 CreateFileAccessTypes.GenericRead | CreateFileAccessTypes.GenericWrite,
                                                 CreateFileShares.Read | CreateFileShares.Write, IntPtr.Zero,
                                                 CreateFileDisposition.OpenExisting, CreateFileAttributes.Normal,
                                                 IntPtr.Zero);
        int lastError = Marshal.GetLastWin32Error();

        if (deviceHandle.IsInvalid)
          throw new Win32Exception(lastError, "Failed to open device");

        deviceHandle.Dispose();

        return true;
      }
      catch
      {
        return false;
      }
    }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    public override void Start()
    {
#if DEBUG
      DebugOpen("IgorPlug Receiver.log");
      DebugWriteLine("Start()");
#endif

      if (!Detect())
        throw new InvalidOperationException("IgorPlug not found");

      _readThread = new Thread(ReadThread);
      _readThread.Name = "IgorPlug.ReadThread";
      _readThread.IsBackground = true;
      _readThread.Start();
    }

    /// <summary>
    /// Suspend the IR Server plugin when computer enters standby.
    /// </summary>
    public override void Suspend()
    {
#if DEBUG
      DebugWriteLine("Suspend()");
#endif

      Stop();
    }

    /// <summary>
    /// Resume the IR Server plugin when the computer returns from standby.
    /// </summary>
    public override void Resume()
    {
#if DEBUG
      DebugWriteLine("Resume()");
#endif

      Start();
    }

    /// <summary>
    /// Stop the IR Server plugin.
    /// </summary>
    public override void Stop()
    {
#if DEBUG
      DebugWriteLine("Stop()");
#endif

      _readThread.Abort();

      if (_readThread.IsAlive)
        _readThread.Join();

#if DEBUG
      DebugClose();
#endif
    }

    private void ReadThread()
    {
      try
      {
        byte[] timingCode = new byte[DeviceBuffer];
        int codeLength = 0;
        int returnCode;

        DoSetInfraBufferEmpty();

        while (true)
        {
          returnCode = DoGetInfraCode(ref timingCode, ref codeLength);

          switch (returnCode)
          {
            case NO_ERROR:
              byte[] data = new byte[codeLength];
              Array.Copy(timingCode, data, codeLength);

              int[] timingData = GetTimingData(data);

              IrDecoder.DecodeIR(timingData, RemoteEvent, null, null);

              Thread.Sleep(100);
              DoSetInfraBufferEmpty();
              break;

            case NO_DATA_AVAILABLE:
              continue;

            case DEVICE_NOT_PRESENT:
              throw new IOException("Device not present");

            case INVALID_BAUDRATE:
              throw new IOException("Invalid baud rate");

            case OVERRUN_ERROR:
              throw new IOException("Overrun error");

            default:
              throw new IOException(String.Format("Unknown error ({0})", returnCode));
          }
        }
      }
#if TRACE
      catch (ThreadAbortException ex)
      {
        Trace.WriteLine(ex.ToString());
      }
#else
      catch (ThreadAbortException)
      {
      }
#endif
    }

    private void RemoteEvent(IrProtocol codeType, uint keyCode, bool firstPress)
    {
#if TRACE
      Trace.WriteLine(String.Format("Remote: {0}, {1}, {2}", Enum.GetName(typeof(IrProtocol), codeType), keyCode, firstPress));
#endif

      if (!firstPress && _lastRemoteButtonCodeType == codeType && _lastRemoteButtonKeyCode == keyCode)
      {
        TimeSpan timeBetween = DateTime.Now.Subtract(_lastRemoteButtonTime);

        if (!_remoteButtonRepeated && timeBetween.TotalMilliseconds < _remoteFirstRepeat)
        {
#if TRACE
          Trace.WriteLine("Skip, First Repeat");
#endif
          return;
        }

        if (_remoteButtonRepeated && timeBetween.TotalMilliseconds < _remoteHeldRepeats)
        {
#if TRACE
          Trace.WriteLine("Skip, Held Repeat");
#endif
          return;
        }

        if (_remoteButtonRepeated && timeBetween.TotalMilliseconds > _remoteFirstRepeat)
          _remoteButtonRepeated = false;
        else
          _remoteButtonRepeated = true;
      }
      else
      {
        _lastRemoteButtonCodeType = codeType;
        _lastRemoteButtonKeyCode = keyCode;
        _remoteButtonRepeated = false;
      }

      _lastRemoteButtonTime = DateTime.Now;

      if (_remoteHandler != null)
        _remoteHandler(Name, keyCode.ToString());
    }

    private static int[] GetTimingData(byte[] data)
    {
      List<int> timingData = new List<int>(data.Length);

      int multiplier = 1;

      foreach (byte dataByte in data)
      {
        timingData.Add((int) Math.Round(dataByte*TimeCodeMultiplier*multiplier));

        multiplier *= -1;
      }

      return timingData.ToArray();
    }

    private static bool SendToDriver(byte FNumber, int Param1, int Param2, ref byte[] OutputData, ref int OutLength)
    {
      bool Result = false;

      SafeFileHandle handle = CreateFile(DevicePath,
                                         CreateFileAccessTypes.GenericRead | CreateFileAccessTypes.GenericWrite,
                                         CreateFileShares.Read | CreateFileShares.Write, IntPtr.Zero,
                                         CreateFileDisposition.OpenExisting, CreateFileAttributes.None, IntPtr.Zero);
      int lastError = Marshal.GetLastWin32Error();

      if (handle.IsInvalid)
        throw new Win32Exception(lastError, "Failed to open device");

      try
      {
        int RepeatCount = 3;
        int OutLengthMax;

        OutLengthMax = (OutLength > 255 ? 256 : OutLength) & 0xFF;

        byte[] Input = new byte[5];
        byte[] tmp;
        Input[0] = FNumber;
        tmp = BitConverter.GetBytes(Param1);
        Input[1] = tmp[0];
        Input[2] = tmp[1];
        tmp = BitConverter.GetBytes(Param2);
        Input[3] = tmp[0];
        Input[4] = tmp[1];

        try
        {
          do
          {
            Result = DeviceIoControl(handle, 0x808, Input, 5, OutputData, OutLengthMax, out OutLength, IntPtr.Zero);
            Result = Result && (OutLength > 0);
            RepeatCount--;
          } while ((OutLength == 0) && (RepeatCount > 0));
        }
        catch
        {
          Result = false;
        }
      }
      finally
      {
        handle.Dispose();
      }

      return Result;
    }

    private static int DoSetInfraBufferEmpty()
    {
      int OutLength = 1;
      byte[] OutputData = new byte[DeviceBuffer];

      if (SendToDriver(FNCNumberDoSetInfraBufferEmpty, 0, 0, ref OutputData, ref OutLength))
        return NO_ERROR;
      else
        return DEVICE_NOT_PRESENT;
    }

    private static int DoGetInfraCode(ref byte[] TimeCodeDiagram, ref int DiagramLength)
    {
      int OutLength;
      byte[] OutputData = new byte[DeviceBuffer];

      int LastReadedCode = -1;
      int BytesToRead;
      byte[] tmpData = new byte[DeviceBuffer];
      int LastWrittenIndex;
      int i, j, k;

      DiagramLength = 0;
      OutLength = 3;
      if (!SendToDriver(FNCNumberDoGetInfraCode, 0, 0, ref OutputData, ref OutLength))
        return DEVICE_NOT_PRESENT;

      BytesToRead = OutputData[0];
      if ((LastReadedCode == OutputData[1]) || (OutLength <= 1) || (BytesToRead == 0))
        return NO_ERROR;

      LastReadedCode = OutputData[1];
      LastWrittenIndex = OutputData[2];
      i = 0;
      while (i < BytesToRead)
      {
        OutLength = BytesToRead - i;
        if (!SendToDriver(2, i + 3, 0, ref tmpData, ref OutLength))
        {
          DoSetInfraBufferEmpty();
          LastReadedCode = -1;
          return DEVICE_NOT_PRESENT;
        }

        for (j = 0; j < OutLength; j++)
          OutputData[i + j] = tmpData[j]; // 'memcpy

        i += OutLength;
      }

      j = LastWrittenIndex%BytesToRead;
      k = 0;
      for (i = j; i < BytesToRead; i++)
        TimeCodeDiagram[k++] = OutputData[i];

      for (i = 0; i < j; i++)
        TimeCodeDiagram[k++] = OutputData[i];

      DiagramLength = BytesToRead;
      DoSetInfraBufferEmpty();

      return NO_ERROR;
    }

    #endregion Implementation

    #region Debug

#if DEBUG

    private static StreamWriter _debugFile;

    /// <summary>
    /// Opens a debug output file.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    private static void DebugOpen(string fileName)
    {
      try
      {
#if TEST_APPLICATION
        string path = fileName;
#else
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                                   String.Format("IR Server Suite\\Logs\\{0}", fileName));
#endif
        _debugFile = new StreamWriter(path, false);
        _debugFile.AutoFlush = true;
      }
#if TRACE
      catch (Exception ex)
      {
        Trace.WriteLine(ex.ToString());
#else
      catch
      {
#endif
        _debugFile = null;
      }
    }

    /// <summary>
    /// Closes the debug output file.
    /// </summary>
    private static void DebugClose()
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
    private static void DebugWriteLine(string line, params object[] args)
    {
      if (_debugFile != null)
      {
        _debugFile.Write("{0:yyyy-MM-dd HH:mm:ss.ffffff} - ", DateTime.Now);
        _debugFile.WriteLine(line, args);
      }
#if TRACE
      else
      {
        Trace.WriteLine(String.Format(line, args));
      }
#endif
    }

    /// <summary>
    /// Writes a string to the debug output file.
    /// </summary>
    /// <param name="text">The string to write.</param>
    /// <param name="args">Formatting arguments.</param>
    private static void DebugWrite(string text, params object[] args)
    {
      if (_debugFile != null)
      {
        _debugFile.Write(text, args);
      }
#if TRACE
      else
      {
        Trace.Write(String.Format(text, args));
      }
#endif
    }

    /// <summary>
    /// Writes a new line to the debug output file.
    /// </summary>
    private static void DebugWriteNewLine()
    {
      if (_debugFile != null)
      {
        _debugFile.WriteLine();
      }
#if TRACE
      else
      {
        Trace.WriteLine(String.Empty);
      }
#endif
    }

    /// <summary>
    /// Dumps an Array to the debug output file.
    /// </summary>
    /// <param name="array">The array.</param>
    private static void DebugDump(Array array)
    {
      foreach (object item in array)
      {
        if (item is byte) DebugWrite("{0:X2}", (byte) item);
        else if (item is ushort) DebugWrite("{0:X4}", (ushort) item);
        else if (item is int) DebugWrite("{1}{0}", (int) item, (int) item > 0 ? "+" : String.Empty);
        else DebugWrite("{0}", item);

        DebugWrite(", ");
      }

      DebugWriteNewLine();
    }

#endif

    #endregion Debug

    // #define TEST_APPLICATION in the project properties when creating the console test app ...
#if TEST_APPLICATION

    static void xRemote(string deviceName, string code)
    {
      Console.WriteLine("Remote: {0}", code);
    }
    static void xKeyboard(string deviceName, int button, bool up)
    {
      Console.WriteLine("Keyboard: {0}, {1}", button, up);
    }
    static void xMouse(string deviceName, int x, int y, int buttons)
    {
      Console.WriteLine("Mouse: ({0}, {1}) - {2}", x, y, buttons);
    }

    [STAThread]
    static void Main()
    {
      IgorPlugReceiver device;

      try
      {
        device = new IgorPlugReceiver();

        //device.Configure(null);

        device.RemoteCallback += new RemoteHandler(xRemote);
        //device.KeyboardCallback += new KeyboardHandler(xKeyboard);
        //device.MouseCallback += new MouseHandler(xMouse);

        device.Start();

        Console.WriteLine("Press a button on your remote ...");

        System.Windows.Forms.Application.Run();

        device.Stop();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
      finally
      {
        device = null;
      }

      Console.ReadKey();
    }

#endif
  }
}