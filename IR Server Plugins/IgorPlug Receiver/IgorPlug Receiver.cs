using System;
using System.Collections.Generic;
using System.ComponentModel;
#if TRACE
using System.Diagnostics;
#endif
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

using Microsoft.Win32.SafeHandles;

namespace InputService.Plugin
{

  /// <summary>
  /// IR Server plugin supporting the IgorPlug device.
  /// </summary>
  public class IgorPlugReceiver : PluginBase, IRemoteReceiver
  {

    #region Constants

    const int NO_ERROR            = 0;
    const int DEVICE_NOT_PRESENT  = 1;
    const int NO_DATA_AVAILABLE   = 2;
    const int INVALID_BAUDRATE    = 3;
    const int OVERRUN_ERROR       = 4;

    const int FNCNumberDoSetInfraBufferEmpty = 1; // restart of infra reading (if was stopped by RAM reading)
    const int FNCNumberDoGetInfraCode = 2; // transmit of receved infra code (if some code in infra buffer)

    const int DeviceBuffer = 256;

    const double TimeCodeMultiplier = 85.3;

    #endregion Constants

    #region Enumerations

    [Flags]
    enum CreateFileAccessTypes : uint
    {
      GenericRead     = 0x80000000,
      GenericWrite    = 0x40000000,
      GenericExecute  = 0x20000000,
      GenericAll      = 0x10000000,
    }

    [Flags]
    enum CreateFileShares : uint
    {
       None   = 0x00,
       Read   = 0x01,
       Write  = 0x02,
       Delete = 0x04,
    }

    enum CreateFileDisposition : uint
    {
      None              = 0,
      New               = 1,
      CreateAlways      = 2,
      OpenExisting      = 3,
      OpenAlways        = 4,
      TruncateExisting  = 5,
    }

    [Flags]
    enum CreateFileAttributes : uint
    {
      None              = 0x00000000,
      Readonly          = 0x00000001,
      Hidden            = 0x00000002,
      System            = 0x00000004,
      Directory         = 0x00000010,
      Archive           = 0x00000020,
      Device            = 0x00000040,
      Normal            = 0x00000080,
      Temporary         = 0x00000100,
      SparseFile        = 0x00000200,
      ReparsePoint      = 0x00000400,
      Compressed        = 0x00000800,
      Offline           = 0x00001000,
      NotContentIndexed = 0x00002000,
      Encrypted         = 0x00004000,
      Write_Through     = 0x80000000,
      Overlapped        = 0x40000000,
      NoBuffering       = 0x20000000,
      RandomAccess      = 0x10000000,
      SequentialScan    = 0x08000000,
      DeleteOnClose     = 0x04000000,
      BackupSemantics   = 0x02000000,
      PosixSemantics    = 0x01000000,
      OpenReparsePoint  = 0x00200000,
      OpenNoRecall      = 0x00100000,
      FirstPipeInstance = 0x00080000,
    }

    #endregion Enumerations

    #region Interop

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    static extern SafeFileHandle CreateFile(
      [MarshalAs(UnmanagedType.LPTStr)] string fileName,
      [MarshalAs(UnmanagedType.U4)] CreateFileAccessTypes fileAccess,
      [MarshalAs(UnmanagedType.U4)] CreateFileShares fileShare,
      IntPtr securityAttributes,
      [MarshalAs(UnmanagedType.U4)] CreateFileDisposition creationDisposition,
      [MarshalAs(UnmanagedType.U4)] CreateFileAttributes flags,
      IntPtr templateFile);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool CloseHandle(
      SafeFileHandle handle);

    [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern bool DeviceIoControl(
      SafeFileHandle handle,
      uint ioControlCode,
      byte[] inBuffer, int inBufferSize,
      byte[] outBuffer, int outBufferSize,
      out int bytesReturned,
      IntPtr overlapped);

    #endregion Interop

    #region Variables

    int _remoteFirstRepeat      = 400;
    int _remoteHeldRepeats      = 250;

    RemoteHandler _remoteHandler;
    //KeyboardHandler _keyboardHandler;
    //MouseHandler _mouseHandler;

    Thread _readThread;

    IrProtocol _lastRemoteButtonCodeType  = IrProtocol.None;
    uint _lastRemoteButtonKeyCode         = 0;
    DateTime _lastRemoteButtonTime        = DateTime.Now;
    bool _remoteButtonRepeated            = false;

    #endregion Variables

    #region Implementation

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name         { get { return "IgorPlug"; } }
    /// <summary>
    /// IR Server plugin version.
    /// </summary>
    /// <value>The version.</value>
    public override string Version      { get { return "1.0.4.2"; } }
    /// <summary>
    /// The IR Server plugin's author.
    /// </summary>
    /// <value>The author.</value>
    public override string Author       { get { return "and-81"; } }
    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    /// <value>The description.</value>
    public override string Description  { get { return "IgorPlug USB Receiver"; } }
    /// <summary>
    /// Gets a display icon for the plugin.
    /// </summary>
    /// <value>The icon.</value>
    public override Icon DeviceIcon     { get { return Properties.Resources.Icon; } }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public override void Start()
    {
      ThreadStart readThreadStart = new ThreadStart(ReadThread);
      _readThread = new Thread(readThreadStart);
      _readThread.IsBackground = true;
      _readThread.Start();
    }
    /// <summary>
    /// Suspend the IR Server plugin when computer enters standby.
    /// </summary>
    public override void Suspend()
    {
      Stop();
    }
    /// <summary>
    /// Resume the IR Server plugin when the computer returns from standby.
    /// </summary>
    public override void Resume()
    {
      Start();
    }
    /// <summary>
    /// Stop the IR Server plugin.
    /// </summary>
    public override void Stop()
    {
      _readThread.Abort();

      if (_readThread.IsAlive)
        _readThread.Join();
    }

    /// <summary>
    /// Callback for remote button presses.
    /// </summary>
    public RemoteHandler RemoteCallback
    {
      get { return _remoteHandler; }
      set { _remoteHandler = value; }
    }

    void ReadThread()
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

          byte[] data = new byte[codeLength];
          Array.Copy(timingCode, data, codeLength);

          int[] timingData = GetTimingData(data);

          IrDecoder.DecodeIR(timingData, new RemoteCallback(RemoteEvent), null, null);

          Thread.Sleep(100);
          DoSetInfraBufferEmpty();
        }
      }
#if TRACE
      catch (Exception ex) // (ThreadAbortException ex)
      {
        Trace.WriteLine(ex.ToString());
#else
      catch // (ThreadAbortException)
      {
#endif
      }
    }

    void RemoteEvent(IrProtocol codeType, uint keyCode, bool firstPress)
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
          Trace.WriteLine("Skip First Repeat");
#endif
          return;
        }

        if (_remoteButtonRepeated && timeBetween.TotalMilliseconds < _remoteHeldRepeats)
        {
#if TRACE
          Trace.WriteLine("Skip Held Repeat");
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
        _remoteHandler(this.Name, keyCode.ToString());
    }

    static int[] GetTimingData(byte[] data)
    {
      List<int> timingData = new List<int>(data.Length);

      int multiplier = 1;

      foreach (byte dataByte in data)
      {
        timingData.Add((int)Math.Round(dataByte * TimeCodeMultiplier * multiplier));

        multiplier *= -1;
      }

      return timingData.ToArray();
    }

    static bool SendToDriver(byte FNumber, int Param1, int Param2, ref byte[] OutputData, ref int OutLength)
    {
      bool Result = false;

      SafeFileHandle handle = CreateFile(@"\\.\IgorPlugUSB_0",
        CreateFileAccessTypes.GenericRead | CreateFileAccessTypes.GenericWrite,
        CreateFileShares.Read | CreateFileShares.Write,
        IntPtr.Zero,
        CreateFileDisposition.OpenExisting,
        CreateFileAttributes.None,
        IntPtr.Zero);

      if (handle.IsInvalid)
        throw new Exception("Cannot Open IgorUSB Driver!");

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
        CloseHandle(handle);
      }

      return Result;
    }

    static int DoSetInfraBufferEmpty()
    {
      int OutLength = 1;
      byte[] OutputData = new byte[DeviceBuffer];
      
      if (SendToDriver(FNCNumberDoSetInfraBufferEmpty, 0, 0, ref OutputData, ref OutLength))
        return NO_ERROR;
      else
        return DEVICE_NOT_PRESENT;
    }

    static int DoGetInfraCode(ref byte[] TimeCodeDiagram, ref int DiagramLength)
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
        return DEVICE_NOT_PRESENT;   //dev not present

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

      j = LastWrittenIndex % BytesToRead;
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

  }

}
