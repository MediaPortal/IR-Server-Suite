using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;

using Microsoft.Win32.SafeHandles;

using IRServerPluginInterface;

namespace MicrosoftMceTransceiver
{

  class DriverReplacement : Driver
  {

    #region Interop

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetOverlappedResult(
      SafeFileHandle handle,
      ref NativeOverlapped overlapped,
      out int bytesTransferred,
      [MarshalAs(UnmanagedType.Bool)] bool wait);

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
    static extern bool ReadFile(
      SafeFileHandle handle,
      IntPtr buffer,
      int bytesToRead,
      out int bytesRead,
      ref NativeOverlapped overlapped);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool WriteFile(
      SafeFileHandle handle,
      byte[] buffer,
      int bytesToWrite,
      out int bytesWritten,
      ref NativeOverlapped overlapped);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool CancelIo(
      SafeFileHandle handle);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool CloseHandle(
      SafeFileHandle handle);

    #endregion Interop

    #region Enumerations

    /// <summary>
    /// Type of device in use.
    /// This is used to determine the blaster port selection method.
    /// </summary>
    enum DeviceType
    {
      /// <summary>
      /// Device is a first party Microsoft MCE transceiver.
      /// </summary>
      Microsoft = 0,
      /// <summary>
      /// Device is an third party SMK or Topseed MCE transceiver.
      /// </summary>
      SmkTopseed = 1,
    }

    /// <summary>
    /// Device input port.
    /// </summary>
    enum InputPort
    {
      Receive   = 0,
      Learning  = 1,
    }

    /// <summary>
    /// Read Thread Mode.
    /// </summary>
    enum ReadThreadMode
    {
      Receiving,
      Learning,
      LearningDone,
      LearningFailed,
      Stop,
    }

    #endregion Enumerations

    #region Constants

    // Vendor ID's for SMK and Topseed devices.
    const string VidSMK       = "vid_1784";
    const string VidTopseed   = "vid_0609";

    // Device variables
    const int DeviceBufferSize  = 100;
    const int PacketTimeout     = 100;
    const int WriteSyncTimeout  = 10000;

    // Microsoft Port Packets
    static readonly byte[][] MicrosoftPorts = new byte[][]
			{
				new byte[] { 0x9F, 0x08, 0x06 },        // Both
				new byte[] { 0x9F, 0x08, 0x04 },	      // 1
				new byte[] { 0x9F, 0x08, 0x02 },	      // 2
			};

    // SMK or Topseed Port Packets
    static readonly byte[][] SmkTopseedPorts = new byte[][]
			{
				new byte[] { 0x9F, 0x08, 0x00 },	      // Both
				new byte[] { 0x9F, 0x08, 0x01 },	      // 1
				new byte[] { 0x9F, 0x08, 0x02 },        // 2
			};

    // Start and Stop Packets
    static readonly byte[] StartPacket            = { 0x00, 0xFF, 0xAA };
    static readonly byte[] StopPacket             = { 0xFF, 0xBB, 
      0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
      0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
      0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
      0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
      0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
      0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
      0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
      0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
      0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
      0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
      0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
      0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
      0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
      0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
      0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
      0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
      0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
      0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
      0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
      0xFF, 0xFF };

    // Misc Packets
    static readonly byte[] SetCarrierFreqPacket   = { 0x9F, 0x06, 0x01, 0x80 };
    static readonly byte[] SetInputPortPacket     = { 0x9F, 0x14, 0x00 };
    static readonly byte[] SetTimeoutPacket       = { 0x9F, 0x0C, 0x00, 0x00 };

    #endregion Constants

    #region Variables

    NotifyWindow _notifyWindow;

    SafeFileHandle _readHandle;
    SafeFileHandle _writeHandle;

    Thread _readThread;
    ReadThreadMode _readThreadMode;
    ManualResetEvent _stopReadThread;
    
    IrCode _learningCode;

    DeviceType _deviceType = DeviceType.Microsoft;

    #endregion Variables

    #region Constructor

    public DriverReplacement(Guid deviceGuid, string devicePath, RemoteCallback remoteCallback, KeyboardCallback keyboardCallback, MouseCallback mouseCallback)
      : base(deviceGuid, devicePath, remoteCallback, keyboardCallback, mouseCallback)
    {
      if (devicePath.IndexOf(VidSMK, StringComparison.InvariantCultureIgnoreCase) != -1 || devicePath.IndexOf(VidTopseed, StringComparison.InvariantCultureIgnoreCase) != -1)
        _deviceType = DeviceType.SmkTopseed;
      else
        _deviceType = DeviceType.Microsoft;
    }

    #endregion Constructor

    #region Driver overrides

    /// <summary>
    /// Start using the device.
    /// </summary>
    public override void Start()
    {
#if DEBUG
      DebugOpen("\\MicrosoftMceTransceiver_DriverReplacement.log");
      DebugWriteLine("Start()");
#endif

      _notifyWindow = new NotifyWindow();
      _notifyWindow.Class = _deviceGuid;

      int lastError;

      _readHandle = CreateFile(_devicePath + "\\Pipe01", CreateFileAccessTypes.GenericRead, CreateFileShares.None, IntPtr.Zero, CreateFileDisposition.OpenExisting, CreateFileAttributes.Overlapped, IntPtr.Zero);
      lastError = Marshal.GetLastWin32Error();
      if (_readHandle.IsInvalid)
        throw new Win32Exception(lastError);

      _writeHandle = CreateFile(_devicePath + "\\Pipe00", CreateFileAccessTypes.GenericWrite, CreateFileShares.None, IntPtr.Zero, CreateFileDisposition.OpenExisting, CreateFileAttributes.Overlapped, IntPtr.Zero);
      lastError = Marshal.GetLastWin32Error();
      if (_writeHandle.IsInvalid)
        throw new Win32Exception(lastError);

      // Initialize device ...
      WriteSync(StartPacket);
      SetTimeout(PacketTimeout);
      SetInputPort(InputPort.Receive);

      StartReadThread();

      _notifyWindow.Create();
      _notifyWindow.DeviceArrival += new DeviceEventHandler(OnDeviceArrival);
      _notifyWindow.DeviceRemoval += new DeviceEventHandler(OnDeviceRemoval);
      _notifyWindow.RegisterDeviceRemoval(_readHandle.DangerousGetHandle());
    }

    /// <summary>
    /// Stop access to the device.
    /// </summary>
    public override void Stop()
    {
#if DEBUG
      DebugWriteLine("Stop()");
#endif

      _notifyWindow.DeviceArrival -= new DeviceEventHandler(OnDeviceArrival);
      _notifyWindow.DeviceRemoval -= new DeviceEventHandler(OnDeviceRemoval);

      WriteSync(StopPacket);

      StopReadThread();

      _notifyWindow.UnregisterDeviceRemoval();
      _notifyWindow.Dispose();
      _notifyWindow = null;

      CloseDevice();

#if DEBUG
      DebugClose();
#endif
    }

    /// <summary>
    /// Computer is entering standby, suspend device.
    /// </summary>
    public override void Suspend()
    {
#if DEBUG
      DebugWriteLine("Suspend()");
#endif

      Stop(); // Unlike the default driver, the replacement driver requires this.
    }

    /// <summary>
    /// Computer is returning from standby, resume device.
    /// </summary>
    public override void Resume()
    {
#if DEBUG
      DebugWriteLine("Resume()");
#endif

      Start(); // Unlike the default driver, the replacement driver requires this.
    }

    /// <summary>
    /// Learn an IR Command.
    /// </summary>
    /// <param name="learnTimeout">How long to wait before aborting learn.</param>
    /// <param name="learned">Newly learned IR Command.</param>
    /// <returns>Learn status.</returns>
    public override LearnStatus Learn(int learnTimeout, out IrCode learned)
    {
#if DEBUG
      DebugWriteLine("Learn()");
#endif

      learned = null;
      _learningCode = new IrCode();

      SetInputPort(InputPort.Learning);

      int learnStartTick = Environment.TickCount;
      _readThreadMode = ReadThreadMode.Learning;

      // Wait for the learning to finish ...
      while (_readThreadMode == ReadThreadMode.Learning && Environment.TickCount < learnStartTick + learnTimeout)
        Thread.Sleep(PacketTimeout);

#if DEBUG
      DebugWriteLine("End Learn");
#endif

      ReadThreadMode modeWas = _readThreadMode;

      _readThreadMode = ReadThreadMode.Receiving;
      SetInputPort(InputPort.Receive);

      LearnStatus status = LearnStatus.Failure;

      switch (modeWas)
      {
        case ReadThreadMode.Learning:
          status = LearnStatus.Timeout;
          break;

        case ReadThreadMode.LearningFailed:
          status = LearnStatus.Failure;
          break;

        case ReadThreadMode.LearningDone:
#if DEBUG
          DebugDump(_learningCode.TimingData);
#endif
          if (_learningCode.FinalizeData())
          {
            learned = _learningCode;
            status = LearnStatus.Success;
          }
          break;
      }

      _learningCode = null;
      return status;
    }

    /// <summary>
    /// Send an IR Command.
    /// </summary>
    /// <param name="code">IR Command data to send.</param>
    /// <param name="port">IR port to send to.</param>
    public override void Send(IrCode code, uint port)
    {
#if DEBUG
      DebugWriteLine("Send()");
      DebugDump(code.TimingData);
#endif

      byte[] portPacket;
      switch (_deviceType)
      {
        case DeviceType.Microsoft:  portPacket = MicrosoftPorts[port];  break;
        case DeviceType.SmkTopseed: portPacket = SmkTopseedPorts[port]; break;
        default:
          throw new ApplicationException("Invalid device type");
      }

      // Set port
      WriteSync(portPacket);

      // Set carrier frequency
      WriteSync(CarrierPacket(code));

      // Send packet
      WriteSync(DataPacket(code));
    }

    #endregion Driver overrides

    #region Implementation

    static byte[] CarrierPacket(IrCode code)
    {
      byte[] carrierPacket = new byte[SetCarrierFreqPacket.Length];
      SetCarrierFreqPacket.CopyTo(carrierPacket, 0);

      if (code.Carrier == IrCode.CarrierFrequencyDCMode)
        return carrierPacket;

      int carrier = code.Carrier;

      if (code.Carrier == IrCode.CarrierFrequencyUnknown)
      {
        carrier = IrCode.CarrierFrequencyDefault;
#if DEBUG
        DebugWriteLine(String.Format("CarrierPacket(): No carrier frequency specificied, using default ({0})", carrier));
#endif
      }

      for (int scaler = 1; scaler <= 4; scaler++)
      {
        int divisor = (10000000 >> (2 * scaler)) / carrier;

        if (divisor < 0xFF)
        {
          carrierPacket[2] = (byte)scaler;
          carrierPacket[3] = (byte)divisor;
          break;
        }
      }

      return carrierPacket;
    }

    static byte[] DataPacket(IrCode code)
    {
      if (code.TimingData.Length == 0)
        return null;

      // Construct data bytes into "packet" ...
      List<byte> packet = new List<byte>();

#if DEBUG
      DebugWriteLine("DataPacket()");
#endif

      for (int index = 0; index < code.TimingData.Length; index++)
      {
        double time = (double)code.TimingData[index];
        
        byte duration = (byte)Math.Abs(Math.Round(time / 50));
        bool pulse = (time > 0);

#if DEBUG
        DebugWrite(String.Format("{0}{1}, ", pulse ? '+' : '-', duration * 50));
#endif

        while (duration > 0x7F)
        {
          packet.Add((byte)(pulse ? 0xFF : 0x7F));

          duration -= 0x7F;
        }

        packet.Add((byte)(pulse ? 0x80 | duration : duration));
      }

#if DEBUG
      DebugWriteNewLine();
#endif

      // Insert byte count markers into packet data bytes ...
      int subpackets = (int)Math.Ceiling(packet.Count / (double)4);

      byte[] output = new byte[packet.Count + subpackets + 1];

      int outputPos = 0;
      
      for (int packetPos = 0; packetPos < packet.Count; )
      {
        byte copyCount = (byte)(packet.Count - packetPos < 4 ? packet.Count - packetPos : 0x04);

        output[outputPos++] = (byte)(0x80 | copyCount);

        for (int index = 0; index < copyCount; index++)
          output[outputPos++] = packet[packetPos++];
      }

      output[outputPos] = 0x80;

      return output;
    }

    /// <summary>
    /// Set the receive buffer timeout.
    /// </summary>
    /// <param name="timeout">Packet timeout (in milliseconds).</param>
    void SetTimeout(int timeout)
    {
      byte[] timeoutPacket = new byte[SetTimeoutPacket.Length];
      SetTimeoutPacket.CopyTo(timeoutPacket, 0);

      int timeoutSamples = 20 * timeout;

      timeoutPacket[2] = (byte)(timeoutSamples >> 8);
      timeoutPacket[3] = (byte)(timeoutSamples % 256);

      WriteSync(timeoutPacket);
    }

    void SetInputPort(InputPort port)
    {
      byte[] inputPortPacket = new byte[SetInputPortPacket.Length];
      SetInputPortPacket.CopyTo(inputPortPacket, 0);

      inputPortPacket[2] = (byte)(port + 1);

      WriteSync(inputPortPacket);
    }

    void StartReadThread()
    {
#if DEBUG
      DebugWriteLine("StartReadThread()");
#endif

      _stopReadThread = new ManualResetEvent(false);
      _readThreadMode = ReadThreadMode.Receiving;

      _readThread = new Thread(new ThreadStart(ReadThread));
      _readThread.Name = "MicrosoftMceTransceiver.DriverReplacement.ReadThread";
      _readThread.IsBackground = true;
      _readThread.Start();
    }
    void StopReadThread()
    {
#if DEBUG
      DebugWriteLine("StopReadThread()");
#endif

      if (_readThread != null)
      {
        _readThreadMode = ReadThreadMode.Stop;
        _stopReadThread.Set();

        if (Thread.CurrentThread != _readThread)
          _readThread.Join();

        _stopReadThread.Close();
        _stopReadThread = null;

        _readThread = null;
      }
    }

    void CloseDevice()
    {
#if DEBUG
      DebugWriteLine("CloseDevice()");
#endif

      if (_readHandle != null)
      {
        CloseHandle(_readHandle);

        _readHandle.Dispose();
        _readHandle = null;
      }

      if (_writeHandle != null)
      {
        CloseHandle(_writeHandle);

        _writeHandle.Dispose();
        _writeHandle = null;
      }
    }

    void OnDeviceArrival()
    {
      _notifyWindow.UnregisterDeviceArrival();

      StartReadThread();

      _notifyWindow.RegisterDeviceRemoval(_readHandle.DangerousGetHandle());
    }
    void OnDeviceRemoval()
    {
      _notifyWindow.UnregisterDeviceRemoval();
      _notifyWindow.RegisterDeviceArrival();

      StopReadThread();
    }
    
    void ReadThread()
    {
      int bytesRead;
      TimeSpan sinceLastPacket;
      DateTime lastPacketTime = DateTime.Now;

      byte[] packetBytes;

      int lastError;

      NativeOverlapped lpOverlapped = new NativeOverlapped();
      lpOverlapped.InternalLow = IntPtr.Zero;
      lpOverlapped.InternalHigh = IntPtr.Zero;
      lpOverlapped.OffsetLow = 0;
      lpOverlapped.OffsetHigh = 0;

      WaitHandle waitHandle = new ManualResetEvent(false);
      SafeHandle safeWaitHandle = waitHandle.SafeWaitHandle;
      WaitHandle[] waitHandles = new WaitHandle[] { waitHandle, _stopReadThread };

      bool success = false;
      safeWaitHandle.DangerousAddRef(ref success);
      if (!success)
      {
        waitHandle.Close();
        return;
      }

      IntPtr deviceBufferPtr = IntPtr.Zero;

      try
      {
        deviceBufferPtr = Marshal.AllocHGlobal(DeviceBufferSize);

        while (_readThreadMode != ReadThreadMode.Stop)
        {
          bytesRead = 0;

          lpOverlapped.EventHandle = safeWaitHandle.DangerousGetHandle();

          bool readDevice = ReadFile(_readHandle, deviceBufferPtr, DeviceBufferSize, out bytesRead, ref lpOverlapped);
          lastError = Marshal.GetLastWin32Error();

          if (!readDevice)
          {
            if (lastError != Win32ErrorCodes.ERROR_SUCCESS && lastError != Win32ErrorCodes.ERROR_IO_PENDING)
              throw new Win32Exception(lastError);

            while (true)
            {
              int handle = WaitHandle.WaitAny(waitHandles, PacketTimeout + 50, false);

              if (handle == Win32ErrorCodes.WAIT_TIMEOUT)
                continue;
              else if (handle == 0)
                break;
              else if (handle == 1)
                throw new ApplicationException("Stop Read Thread");
              else
                throw new ApplicationException("Invalid wait handle return");
            }

            bool getOverlapped = GetOverlappedResult(_readHandle, ref lpOverlapped, out bytesRead, true);
            lastError = Marshal.GetLastWin32Error();

            if (!getOverlapped)
            {
              if (lastError != Win32ErrorCodes.ERROR_SUCCESS)
                throw new Win32Exception(lastError);
            }
          }

          if (bytesRead == 0)
            continue;

          sinceLastPacket = DateTime.Now.Subtract(lastPacketTime);
          //if (sinceLastPacket.TotalMilliseconds >= PacketTimeout + 50)
            //IrDecoder.DecodeIR(null, null, null, null);

          lastPacketTime = DateTime.Now;

          packetBytes = new byte[bytesRead];
          Marshal.Copy(deviceBufferPtr, packetBytes, 0, bytesRead);

          int[] timingData;

          switch (_readThreadMode)
          {
            case ReadThreadMode.Receiving:
              {
                if (packetBytes[0] >= 0x81 && packetBytes[0] <= 0x8F)
                {
                  timingData = GetTimingDataFromPacket(packetBytes);
                  if (timingData == null)
                    break;

                  IrDecoder.DecodeIR(timingData, _remoteCallback, _keyboardCallback, _mouseCallback);
                }
                break;
              }

            case ReadThreadMode.Learning:
              {
                timingData = GetTimingDataFromPacket(packetBytes);
                if (timingData == null)
                {
                  if (_learningCode.TimingData.Length > 0)
                  {
                    _learningCode = null;
                    _readThreadMode = ReadThreadMode.LearningFailed;
                  }
                  break;
                }

                _learningCode.AddTimingData(timingData);

                // 9F 01 02 9F 15 00 BE 80
                int indexOf9F = Array.IndexOf(packetBytes, (byte)0x9F);
                while (indexOf9F != -1)
                {
                  if (packetBytes.Length > indexOf9F + 3 && packetBytes[indexOf9F + 1] == 0x15)
                  {
                    byte b1 = packetBytes[indexOf9F + 2];
                    byte b2 = packetBytes[indexOf9F + 3];

                    int onTime, onCount;
                    GetIrCodeLengths(_learningCode, out onTime, out onCount);

                    double carrierCount = ((b1 * 256) + b2);

                    if (carrierCount / onCount < 2)
                    {
                      _learningCode.Carrier = IrCode.CarrierFrequencyDCMode;
                    }
                    else
                    {
                      double carrier = (double)1000000 * carrierCount / onTime;

                      if (carrier > 32000)
                        _learningCode.Carrier = (int)(carrier + 0.05 * carrier - 32000 / 48000);
                      else
                        _learningCode.Carrier = (int)carrier;
                    }

                    _readThreadMode = ReadThreadMode.LearningDone;
                    break;
                  }

                  if (packetBytes.Length > indexOf9F + 1)
                  {
                    indexOf9F = Array.IndexOf(packetBytes, (byte)0x9F, indexOf9F + 1);
                  }
                  else
                  {
                    _readThreadMode = ReadThreadMode.LearningFailed;
                    break;
                  }
                }

                break;
              }
          }

        }
      }
#if DEBUG
      catch (Exception ex)
      {
        DebugWriteLine(ex.ToString());
#else
      catch (Exception)
      {
#endif

        if (_readHandle != null)
          CancelIo(_readHandle);
      }
      finally
      {
        if (deviceBufferPtr != IntPtr.Zero)
          Marshal.FreeHGlobal(deviceBufferPtr);

        safeWaitHandle.DangerousRelease();
        waitHandle.Close();
      }
    }

    void WriteSync(byte[] data)
    {
#if DEBUG
      DebugWriteLine("WriteSync()");
      DebugDump(data);
#endif

      NativeOverlapped lpOverlapped = new NativeOverlapped();
      lpOverlapped.InternalLow = IntPtr.Zero;
      lpOverlapped.InternalHigh = IntPtr.Zero;
      lpOverlapped.OffsetLow = 0;
      lpOverlapped.OffsetHigh = 0;

      int bytesWritten = 0;

      int lastError;

      WaitHandle waitHandle = new ManualResetEvent(false);
      SafeHandle safeWaitHandle = waitHandle.SafeWaitHandle;
      WaitHandle[] waitHandles = new WaitHandle[] { waitHandle };

      bool success = false;
      safeWaitHandle.DangerousAddRef(ref success);
      if (!success)
        return;

      try
      {
        lpOverlapped.EventHandle = safeWaitHandle.DangerousGetHandle();

        bool writeDevice = WriteFile(_writeHandle, data, data.Length, out bytesWritten, ref lpOverlapped);
        lastError = Marshal.GetLastWin32Error();

        if (!writeDevice)
        {
          if (lastError != Win32ErrorCodes.ERROR_SUCCESS && lastError != Win32ErrorCodes.ERROR_IO_PENDING)
            throw new Win32Exception(lastError);

          int handle = WaitHandle.WaitAny(waitHandles, WriteSyncTimeout, false);

          if (handle == Win32ErrorCodes.WAIT_TIMEOUT)
            throw new ApplicationException("Timeout trying to write data to device");
          else if (handle != 0)
            throw new ApplicationException("Invalid wait handle return");

          bool getOverlapped = GetOverlappedResult(_writeHandle, ref lpOverlapped, out bytesWritten, true);
          lastError = Marshal.GetLastWin32Error();

          if (!getOverlapped)
          {
            if (lastError != Win32ErrorCodes.ERROR_SUCCESS)
              throw new Win32Exception(lastError);
          }
        }
      }
      catch
      {
        if (_writeHandle != null)
          CancelIo(_writeHandle);

        throw;
      }
      finally
      {
        safeWaitHandle.DangerousRelease();
        waitHandle.Close();
      }
    }

    static int[] GetTimingDataFromPacket(byte[] packet)
    {
      List<int> timingData = new List<int>();

      int len = 0;

      for (int index = 0; index < packet.Length; )
      {
        byte curByte = packet[index];

        if (curByte == 0x9F)
          break;

        if (curByte < 0x81 || curByte > 0x8F)
          return null;
        
        int bytes = curByte & 0x7F;
        int j;
        
        for (j = index + 1; j < index + bytes + 1; j++)
        {
          curByte = packet[j];

          if ((curByte & 0x80) != 0)
            len += (int)(curByte & 0x7F);
          else
            len -= (int)curByte;

          if ((curByte & 0x7F) != 0x7F)
          {
            timingData.Add(len * 50);
            len = 0;
          }
        }

        index = j;
      }

      if (len != 0)
        timingData.Add(len * 50);

#if DEBUG
      DebugWriteLine("Received:");
      DebugDump(timingData.ToArray());
#endif

      return timingData.ToArray();
    }

    static void GetIrCodeLengths(IrCode code, out int onTime, out int onCount)
    {
      onTime  = 0;
      onCount = 0;

      foreach (int time in code.TimingData)
      {
        if (time > 0)
        {
          onTime += time;
          onCount++;
        }
      }
    }

    #endregion Implementation

  }

}
