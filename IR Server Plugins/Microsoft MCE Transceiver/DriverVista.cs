using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;

using Microsoft.Win32.SafeHandles;

namespace InputService.Plugin
{

  /// <summary>
  /// Driver class for Windows Vista eHome driver.
  /// </summary>
  class DriverVista : Driver
  {

    #region Interop

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool DeviceIoControl(
      SafeFileHandle handle,
      [MarshalAs(UnmanagedType.U4)] IoCtrl ioControlCode,
      IntPtr inBuffer, int inBufferSize,
      IntPtr outBuffer, int outBufferSize,
      out int bytesReturned,
      IntPtr overlapped);

    #endregion Interop

    #region Structures

    #region Notes

    // This is really weird and I don't know why this works, but apparently on
    // 64-bit systems the following structures require 64-bit integers.
    // The easiest way to do this is to use an IntPtr because it is 32-bits
    // wide on 32-bit systems, and 64-bits wide on 64-bit systems.
    // Given that it is exactly the same data on 32-bit or 64-bit systems it
    // makes no sense (to me) why Microsoft would do it this way ...

    // Note: I couldn't find any reference to this in the WinHEC or other
    // documentation I have seen.  When 64-bit users started reporting
    // "The data area passed to a system call is too small." errors (122) the
    // only thing I could think of was that the structures were differenly
    // sized on 64-bit systems.  And the only thing in C# that sizes
    // differently on 64-bit systems is the IntPtr.

    #endregion Notes
    
    /// <summary>
    /// Information for transmitting IR.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    struct TransmitChunk
    {
      /// <summary>
      /// Next chunk offset.
      /// </summary>
      public IntPtr OffsetToNextChunk;
      /// <summary>
      /// Repeat count.
      /// </summary>
      public IntPtr RepeatCount;
      /// <summary>
      /// Number of bytes.
      /// </summary>
      public IntPtr ByteCount;
    }

    /// <summary>
    /// Parameters for transmitting IR.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    struct TransmitParams
    {
      /// <summary>
      /// Bitmask containing ports to transmit on.
      /// </summary>
      public IntPtr TransmitPortMask;
      /// <summary>
      /// Carrier period.
      /// </summary>
      public IntPtr CarrierPeriod;
      /// <summary>
      /// Transmit Flags.
      /// </summary>
      public IntPtr Flags;
      /// <summary>
      /// Pulse Size.  If Pulse Mode Flag set.
      /// </summary>
      public IntPtr PulseSize;
    }

    /// <summary>
    /// Receive parameters.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    struct ReceiveParams
    {
      /// <summary>
      /// Last packet in block?
      /// </summary>
      public IntPtr DataEnd;
      /// <summary>
      /// Number of bytes in block.
      /// </summary>
      public IntPtr ByteCount;
      /// <summary>
      /// Carrier frequency of IR received.
      /// </summary>
      public IntPtr CarrierFrequency;
    }

    /// <summary>
    /// Parameters for StartReceive.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    struct StartReceiveParams
    {
      /// <summary>
      /// Index of the receiver to use.
      /// </summary>
      public IntPtr Receiver;
      /// <summary>
      /// Receive timeout, in milliseconds.
      /// </summary>
      public IntPtr Timeout;
    }

    /// <summary>
    /// Device Capabilities data structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    struct DeviceCapabilities
    {
      /// <summary>
      /// Device protocol version.
      /// </summary>
      public IntPtr ProtocolVersion;
      /// <summary>
      /// Number of transmit ports – 0-32.
      /// </summary>
      public IntPtr TransmitPorts;
      /// <summary>
      /// Number of receive ports – 0-32. For beanbag, this is two (one for learning, one for normal receiving).
      /// </summary>
      public IntPtr ReceivePorts;
      /// <summary>
      /// Bitmask identifying which receivers are learning receivers – low bit is the first receiver, second-low bit is the second receiver, etc ...
      /// </summary>
      public IntPtr LearningMask;
      /// <summary>
      /// Device flags.
      /// </summary>
      public IntPtr DetailsFlags;
    }

    /// <summary>
    /// Available Blasters data structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    struct AvailableBlasters
    {
      /// <summary>
      /// Blaster bit-mask.
      /// </summary>
      public IntPtr Blasters;
    }

    #endregion Structures

    #region Enumerations

    /// <summary>
    /// Device IO Control details.
    /// </summary>
    enum IoCtrl
    {
      /// <summary>
      /// Start receiving IR.
      /// </summary>
      StartReceive  = 0x0F608028,
      /// <summary>
      /// Stop receiving IR.
      /// </summary>
      StopReceive   = 0x0F60802C,
      /// <summary>
      /// Get IR device details.
      /// </summary>
      GetDetails    = 0x0F604004,
      /// <summary>
      /// Get IR blasters
      /// </summary>
      GetBlasters   = 0x0F604008,
      /// <summary>
      /// Receive IR.
      /// </summary>
      Receive       = 0x0F604022,
      /// <summary>
      /// Transmit IR.
      /// </summary>
      Transmit      = 0x0F608015,
    }

    /// <summary>
    /// IR Device Capability Flags.
    /// </summary>
    [Flags]
    enum DeviceCapabilityFlags
    {
      /// <summary>
      /// Hardware supports legacy key signing.
      /// </summary>
      LegacySigning = 0x0001, 
      /// <summary>
      /// Hardware has unique serial number.
      /// </summary>
      SerialNumber  = 0x0002,
      /// <summary>
      /// Can hardware flash LED to identify receiver? 
      /// </summary>
      FlashLed      = 0x0004,
      /// <summary>
      /// Is this a legacy device?
      /// </summary>
      Legacy        = 0x0008,
      /// <summary>
      /// Device can wake from S1.
      /// </summary>
      WakeS1        = 0x0010,
      /// <summary>
      /// Device can wake from S2.
      /// </summary>
      WakeS2        = 0x0020,
      /// <summary>
      /// Device can wake from S3.
      /// </summary>
      WakeS3        = 0x0040,
      /// <summary>
      /// Device can wake from S4.
      /// </summary>
      WakeS4        = 0x0080,
      /// <summary>
      /// Device can wake from S5.
      /// </summary>
      WakeS5        = 0x0100,
    }

    /// <summary>
    /// Used to set the carrier mode for IR blasting.
    /// </summary>
    enum TransmitMode
    {
      /// <summary>
      /// Carrier Mode.
      /// </summary>
      CarrierMode = 0,
      /// <summary>
      /// DC Mode.
      /// </summary>
      DCMode = 1,
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

    const int DeviceBufferSize  = 100;
    const int PacketTimeout     = 100;
    const int WriteSyncTimeout  = 10000;

    #endregion Constants

    #region Variables

    #region Device Details

    int _numTxPorts;
    int _txPortMask;
    int _learnPortMask;

    int _receivePort;
    int _learnPort;

    #endregion Device Details

    NotifyWindow _notifyWindow;

    SafeFileHandle _eHomeHandle;

    Thread _readThread;
    ReadThreadMode _readThreadMode;

    IrCode _learningCode;

    bool _deviceAvailable;

    #endregion Variables

    #region Constructor

    public DriverVista(Guid deviceGuid, string devicePath, RemoteCallback remoteCallback, KeyboardCallback keyboardCallback, MouseCallback mouseCallback)
      : base(deviceGuid, devicePath, remoteCallback, keyboardCallback, mouseCallback) { }

    #endregion Constructor

    #region Device Control Functions

    void StartReceive(int receivePort, int timeout)
    {
      if (!_deviceAvailable)
        throw new InvalidOperationException("Device not available");

      int bytesReturned;

      StartReceiveParams structure;
      structure.Receiver = new IntPtr(receivePort);
      structure.Timeout = new IntPtr(timeout);

      IntPtr structPtr = IntPtr.Zero;

      try
      {
        structPtr = Marshal.AllocHGlobal(Marshal.SizeOf(structure));

        Marshal.StructureToPtr(structure, structPtr, false);

        IoControl(IoCtrl.StartReceive, structPtr, Marshal.SizeOf(structure), IntPtr.Zero, 0, out bytesReturned);
      }
      finally
      {
        if (structPtr != IntPtr.Zero)
          Marshal.FreeHGlobal(structPtr);
      }
    }

    void StopReceive()
    {
      if (!_deviceAvailable)
        throw new InvalidOperationException("Device not available");

      int bytesReturned;
      IoControl(IoCtrl.StopReceive, IntPtr.Zero, 0, IntPtr.Zero, 0, out bytesReturned);
    }

    void GetDeviceCapabilities()
    {
      if (!_deviceAvailable)
        throw new InvalidOperationException("Device not available");

      int bytesReturned;

      DeviceCapabilities structure = new DeviceCapabilities();

      IntPtr structPtr = IntPtr.Zero;

      try
      {
        structPtr = Marshal.AllocHGlobal(Marshal.SizeOf(structure));

        Marshal.StructureToPtr(structure, structPtr, false);

        IoControl(IoCtrl.GetDetails, IntPtr.Zero, 0, structPtr, Marshal.SizeOf(structure), out bytesReturned);

        structure = (DeviceCapabilities)Marshal.PtrToStructure(structPtr, typeof(DeviceCapabilities));
      }
      finally
      {
        if (structPtr != IntPtr.Zero)
          Marshal.FreeHGlobal(structPtr);
      }

      _numTxPorts = structure.TransmitPorts.ToInt32();
      //_numRxPorts = structure.ReceivePorts.ToInt32();
      _learnPortMask = structure.LearningMask.ToInt32();

      int receivePort = FirstLowBit(_learnPortMask);
      if (receivePort != -1)
        _receivePort = receivePort;

      int learnPort = FirstHighBit(_learnPortMask);
      if (learnPort != -1)
        _learnPort = learnPort;
      else
        _learnPort = _receivePort;

      //DeviceCapabilityFlags flags = (DeviceCapabilityFlags)structure.DetailsFlags.ToInt32();
      //_legacyDevice = (int)(flags & DeviceCapabilityFlags.Legacy) != 0;
      //_canFlashLed = (int)(flags & DeviceCapabilityFlags.FlashLed) != 0;

#if DEBUG
      DebugWriteLine("Device Capabilities:");
      DebugWriteLine("NumTxPorts:     {0}", _numTxPorts);
      DebugWriteLine("NumRxPorts:     {0}", structure.ReceivePorts.ToInt32());
      DebugWriteLine("LearnPortMask:  {0}", _learnPortMask);
      DebugWriteLine("ReceivePort:    {0}", _receivePort);
      DebugWriteLine("LearnPort:      {0}", _learnPort);
      DebugWriteLine("DetailsFlags:   {0}", structure.DetailsFlags.ToInt32());
#endif
    }

    void GetBlasters()
    {
      if (!_deviceAvailable)
        throw new InvalidOperationException("Device not available");

      if (_numTxPorts <= 0)
        return;

      int bytesReturned;

      AvailableBlasters structure = new AvailableBlasters();

      IntPtr structPtr = IntPtr.Zero;

      try
      {
        structPtr = Marshal.AllocHGlobal(Marshal.SizeOf(structure));

        Marshal.StructureToPtr(structure, structPtr, false);

        IoControl(IoCtrl.GetBlasters, IntPtr.Zero, 0, structPtr, Marshal.SizeOf(structure), out bytesReturned);

        structure = (AvailableBlasters)Marshal.PtrToStructure(structPtr, typeof(AvailableBlasters));
      }
      finally
      {
        if (structPtr != IntPtr.Zero)
          Marshal.FreeHGlobal(structPtr);
      }

      _txPortMask = structure.Blasters.ToInt32();

#if DEBUG
      DebugWriteLine("TxPortMask:     {0}", _txPortMask);
#endif
    }

    void TransmitIR(byte[] irData, int carrier, int transmitPortMask)
    {
#if DEBUG
      DebugWriteLine("TransmitIR({0} bytes, carrier: {1}, port: {2})", irData.Length, carrier, transmitPortMask);
#endif

      if (!_deviceAvailable)
        throw new InvalidOperationException("Device not available");

      int bytesReturned;

      TransmitParams transmitParams = new TransmitParams();
      transmitParams.TransmitPortMask = new IntPtr(transmitPortMask);

      if (carrier == IrCode.CarrierFrequencyUnknown)
        carrier = IrCode.CarrierFrequencyDefault;

      TransmitMode mode = GetTransmitMode(carrier);
      if (mode == TransmitMode.CarrierMode)
        transmitParams.CarrierPeriod = new IntPtr(GetCarrierPeriod(carrier));
      else
        transmitParams.PulseSize = new IntPtr(carrier);

      transmitParams.Flags = new IntPtr((int)mode);

      TransmitChunk transmitChunk = new TransmitChunk();
      transmitChunk.OffsetToNextChunk = new IntPtr(0);
      transmitChunk.RepeatCount = new IntPtr(1);
      transmitChunk.ByteCount = new IntPtr(irData.Length);

      int bufferSize = irData.Length + Marshal.SizeOf(typeof(TransmitChunk)) + 8;
      byte[] buffer = new byte[bufferSize];

      byte[] rawTransmitChunk = RawSerialize(transmitChunk);
      Array.Copy(rawTransmitChunk, buffer, rawTransmitChunk.Length);

      Array.Copy(irData, 0, buffer, rawTransmitChunk.Length, irData.Length);

      IntPtr structurePtr = IntPtr.Zero;
      IntPtr bufferPtr = IntPtr.Zero;

      try
      {
        structurePtr = Marshal.AllocHGlobal(Marshal.SizeOf(transmitParams));
        bufferPtr = Marshal.AllocHGlobal(buffer.Length);

        Marshal.StructureToPtr(transmitParams, structurePtr, true);

        Marshal.Copy(buffer, 0, bufferPtr, buffer.Length);

        IoControl(IoCtrl.Transmit, structurePtr, Marshal.SizeOf(typeof(TransmitParams)), bufferPtr, bufferSize, out bytesReturned);
      }
      finally
      {
        if (structurePtr != IntPtr.Zero)
          Marshal.FreeHGlobal(structurePtr);

        if (bufferPtr != IntPtr.Zero)
          Marshal.FreeHGlobal(bufferPtr);
      }

      // Force a delay between blasts (hopefully solves back-to-back blast errors) ...
      Thread.Sleep(PacketTimeout);
    }

    void IoControl(IoCtrl ioControlCode, IntPtr inBuffer, int inBufferSize, IntPtr outBuffer, int outBufferSize, out int bytesReturned)
    {
      if (!_deviceAvailable)
        throw new InvalidOperationException("Device not available");

      try
      {
        int lastError;

        using (WaitHandle waitHandle = new ManualResetEvent(false))
        {
          DeviceIoOverlapped overlapped = new DeviceIoOverlapped();
          overlapped.ClearAndSetEvent(waitHandle.SafeWaitHandle.DangerousGetHandle());

          bool deviceIoControl = DeviceIoControl(_eHomeHandle, ioControlCode, inBuffer, inBufferSize, outBuffer, outBufferSize, out bytesReturned, overlapped.Overlapped);
          lastError = Marshal.GetLastWin32Error();

          if (!deviceIoControl)
          {
            if (lastError != ErrorIoPending)
              throw new Win32Exception(lastError);

            waitHandle.WaitOne();

            bool getOverlapped = GetOverlappedResult(_eHomeHandle, overlapped.Overlapped, out bytesReturned, false);
            lastError = Marshal.GetLastWin32Error();

            if (!getOverlapped)
              throw new Win32Exception(lastError);
          }
        }
      }
      catch
      {
        if (_eHomeHandle != null)
          CancelIo(_eHomeHandle);

        throw;
      }
    }

    #endregion Device Control Functions

    #region Driver overrides

    /// <summary>
    /// Start using the device.
    /// </summary>
    public override void Start()
    {
#if DEBUG
      DebugOpen("MicrosoftMceTransceiver_DriverVista.log");
      DebugWriteLine("Start()");
#endif

      _notifyWindow = new NotifyWindow();
      _notifyWindow.Create();
      _notifyWindow.Class = _deviceGuid;
      _notifyWindow.RegisterDeviceArrival();

      OpenDevice();
      InitializeDevice();

      StartReceive(_receivePort, PacketTimeout);
      StartReadThread(ReadThreadMode.Receiving);

      _notifyWindow.DeviceArrival += new DeviceEventHandler(OnDeviceArrival);
      _notifyWindow.DeviceRemoval += new DeviceEventHandler(OnDeviceRemoval);
    }

    /// <summary>
    /// Stop access to the device.
    /// </summary>
    public override void Stop()
    {
#if DEBUG
      DebugWriteLine("Stop()");
#endif

      try
      {
        _notifyWindow.DeviceArrival -= new DeviceEventHandler(OnDeviceArrival);
        _notifyWindow.DeviceRemoval -= new DeviceEventHandler(OnDeviceRemoval);

        StopReadThread();
        CloseDevice();
      }
#if DEBUG
      catch (Exception ex)
      {
        DebugWriteLine(ex.ToString());
#else
      catch
      {
#endif
        throw;
      }
      finally
      {
        _notifyWindow.Dispose();
        _notifyWindow = null;

#if DEBUG
        DebugClose();
#endif
      }
    }

    /// <summary>
    /// Computer is entering standby, suspend device.
    /// </summary>
    public override void Suspend()
    {
#if DEBUG
      DebugWriteLine("Suspend()");
#endif

      StopReadThread();
      CloseDevice();
    }

    /// <summary>
    /// Computer is returning from standby, resume device.
    /// </summary>
    public override void Resume()
    {
#if DEBUG
      DebugWriteLine("Resume()");
#endif

      try
      {
        if (String.IsNullOrEmpty(Driver.Find(_deviceGuid)))
        {
#if DEBUG
          DebugWriteLine("Device not available");
#endif
          return;
        }

        OpenDevice();
        InitializeDevice();

        StartReceive(_receivePort, PacketTimeout);
        StartReadThread(ReadThreadMode.Receiving);
      }
#if DEBUG
      catch (Exception ex)
      {
        DebugWriteLine(ex.ToString());
      }
#else
      catch
      {
        throw;
      }
#endif
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

      StopReadThread();

      learned = null;
      _learningCode = new IrCode();

      StartReceive(_learnPort, PacketTimeout);
      StartReadThread(ReadThreadMode.Learning);

      int learnStartTick = Environment.TickCount;

      // Wait for the learning to finish ...
      while (_readThreadMode == ReadThreadMode.Learning && Environment.TickCount < learnStartTick + learnTimeout)
        Thread.Sleep(PacketTimeout);

#if DEBUG
      DebugWriteLine("End Learn");
#endif

      ReadThreadMode modeWas = _readThreadMode;

      StopReadThread();

      StartReceive(_receivePort, PacketTimeout);
      StartReadThread(ReadThreadMode.Receiving);

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
    public override void Send(IrCode code, int port)
    {
#if DEBUG
      DebugWriteLine("Send()");
      DebugDump(code.TimingData);
#endif

      byte[] data = DataPacket(code);

      int portMask = 0;
      switch ((BlasterPort)port)
      {
        case BlasterPort.Both:    portMask = _txPortMask; break;
        case BlasterPort.Port_1:  portMask = GetHighBit(_txPortMask, 1); break;
        case BlasterPort.Port_2:  portMask = GetHighBit(_txPortMask, 2); break;
      }

      TransmitIR(data, code.Carrier, portMask);
    }

    #endregion Driver overrides

    #region Implementation

    /// <summary>
    /// Initializes the device.
    /// </summary>
    void InitializeDevice()
    {
      GetDeviceCapabilities();
      GetBlasters();
    }

    /// <summary>
    /// Converts an IrCode into raw data for the device.
    /// </summary>
    /// <param name="code">IrCode to convert.</param>
    /// <returns>Raw device data.</returns>
    static byte[] DataPacket(IrCode code)
    {
#if DEBUG
      DebugWriteLine("DataPacket()");
#endif

      if (code.TimingData.Length == 0)
        return null;

      byte[] data = new byte[code.TimingData.Length * 4];
      
      int dataIndex = 0;
      for (int timeIndex = 0; timeIndex < code.TimingData.Length; timeIndex++)
      {
        uint time = (uint)(50 * (int)Math.Round((double)code.TimingData[timeIndex] / 50));

        for (int timeShift =  0; timeShift < 4; timeShift++)
        {
          data[dataIndex++] = (byte)(time & 0xFF);
          time >>= 8;
        }
      }

      return data;
    }

    /// <summary>
    /// Start the device read thread.
    /// </summary>
    void StartReadThread(ReadThreadMode mode)
    {
#if DEBUG
      DebugWriteLine("StartReadThread({0})", Enum.GetName(typeof(ReadThreadMode), mode));
#endif

      if (_readThread != null)
      {
#if DEBUG
        DebugWriteLine("Read thread already started");
#endif
        return;
      }

      _readThreadMode = mode;

      _readThread = new Thread(new ThreadStart(ReadThread));
      _readThread.Name = "MicrosoftMceTransceiver.DriverVista.ReadThread";
      _readThread.IsBackground = true;
      _readThread.Start();
    }
    /// <summary>
    /// Stop the device read thread.
    /// </summary>
    void StopReadThread()
    {
#if DEBUG
      DebugWriteLine("StopReadThread()");
#endif

      if (_readThread == null)
      {
#if DEBUG
        DebugWriteLine("Read thread already stopped");
#endif
        return;
      }

      _readThreadMode = ReadThreadMode.Stop;

      _readThread.Abort();

      if (Thread.CurrentThread != _readThread)
        _readThread.Join();

      _readThread = null;
    }

    /// <summary>
    /// Opens the device.
    /// </summary>
    void OpenDevice()
    {
#if DEBUG
      DebugWriteLine("OpenDevice()");
#endif

      if (_eHomeHandle != null)
      {
#if DEBUG
        DebugWriteLine("Device already open");
#endif
        return;
      }

      _eHomeHandle = CreateFile(_devicePath, CreateFileAccessTypes.GenericRead | CreateFileAccessTypes.GenericWrite, CreateFileShares.None, IntPtr.Zero, CreateFileDisposition.OpenExisting, CreateFileAttributes.Overlapped, IntPtr.Zero);
      int lastError = Marshal.GetLastWin32Error();
      if (_eHomeHandle.IsInvalid)
      {
        _eHomeHandle = null;
        throw new Win32Exception(lastError);
      }

      _notifyWindow.RegisterDeviceRemoval(_eHomeHandle.DangerousGetHandle());

      Thread.Sleep(PacketTimeout); // Hopefully improves compatibility with Zalman remote which times out retrieving device capabilities. (2008-01-01)

      _deviceAvailable = true;
    }

    /// <summary>
    /// Close all handles to the device.
    /// </summary>
    void CloseDevice()
    {
#if DEBUG
      DebugWriteLine("CloseDevice()");
#endif

      _deviceAvailable = false;

      if (_eHomeHandle == null)
      {
#if DEBUG
        DebugWriteLine("Device already closed");
#endif
        return;
      }

      _notifyWindow.UnregisterDeviceRemoval();

      CloseHandle(_eHomeHandle);

      _eHomeHandle.Dispose();
      _eHomeHandle = null;
    }

    void OnDeviceArrival()
    {
#if DEBUG
      DebugWriteLine("OnDeviceArrival()");
#endif

      OpenDevice();
      InitializeDevice();

      StartReceive(_receivePort, PacketTimeout);
      StartReadThread(ReadThreadMode.Receiving);
    }
    void OnDeviceRemoval()
    {
#if DEBUG
      DebugWriteLine("OnDeviceRemoval()");
#endif

      StopReadThread();
      CloseDevice();
    }

    /// <summary>
    /// Device read thread method.
    /// </summary>
    void ReadThread()
    {
      int bytesRead;

      IntPtr deviceBufferPtr  = IntPtr.Zero;
      IntPtr receiveParamsPtr = IntPtr.Zero;

      try
      {
        deviceBufferPtr = Marshal.AllocHGlobal(DeviceBufferSize);

        int receiveParamsSize = Marshal.SizeOf(typeof(ReceiveParams)) + DeviceBufferSize + 8;
        receiveParamsPtr = Marshal.AllocHGlobal(receiveParamsSize);

        ReceiveParams receiveParams = new ReceiveParams();
        receiveParams.ByteCount = new IntPtr(DeviceBufferSize);
        Marshal.StructureToPtr(receiveParams, receiveParamsPtr, false);

        while (_readThreadMode != ReadThreadMode.Stop)
        {
          IoControl(IoCtrl.Receive, IntPtr.Zero, 0, receiveParamsPtr, receiveParamsSize, out bytesRead);

          if (bytesRead > Marshal.SizeOf(receiveParams))
          {
            int dataSize = bytesRead;

            bytesRead -= Marshal.SizeOf(receiveParams);

            byte[] packetBytes = new byte[bytesRead];
            byte[] dataBytes = new byte[dataSize];

            Marshal.Copy(receiveParamsPtr, dataBytes, 0, dataSize);
            Array.Copy(dataBytes, dataSize - bytesRead, packetBytes, 0, bytesRead);

            int[] timingData = GetTimingDataFromPacket(packetBytes);

#if DEBUG
            DebugWriteLine("Received:");
            DebugDump(timingData);
#endif

            if (_readThreadMode == ReadThreadMode.Learning)
              _learningCode.AddTimingData(timingData);
            else
              IrDecoder.DecodeIR(timingData, _remoteCallback, _keyboardCallback, _mouseCallback);              
          }

          // Determine carrier frequency when learning ...
          if (_readThreadMode == ReadThreadMode.Learning && bytesRead >= Marshal.SizeOf(receiveParams))
          {
            ReceiveParams receiveParams2 = (ReceiveParams)Marshal.PtrToStructure(receiveParamsPtr, typeof(ReceiveParams));

            if (receiveParams2.DataEnd.ToInt32() != 0)
            {
              _learningCode.Carrier = receiveParams2.CarrierFrequency.ToInt32();
              _readThreadMode = ReadThreadMode.LearningDone;
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

        if (_eHomeHandle != null)
          CancelIo(_eHomeHandle);
      }
      finally
      {
        if (deviceBufferPtr != IntPtr.Zero)
          Marshal.FreeHGlobal(deviceBufferPtr);

        if (receiveParamsPtr != IntPtr.Zero)
          Marshal.FreeHGlobal(receiveParamsPtr);

        StopReceive();
      }

#if DEBUG
      DebugWriteLine("Read Thread Ended");
#endif
    }

    #endregion Implementation

    #region Misc Methods

    static byte[] RawSerialize(object anything)
    {
      int rawSize = Marshal.SizeOf(anything);
      byte[] rawData = new byte[rawSize];

      GCHandle handle = GCHandle.Alloc(rawData, GCHandleType.Pinned);
      
      try
      {
        IntPtr buffer = handle.AddrOfPinnedObject();

        Marshal.StructureToPtr(anything, buffer, false);
      }
      finally
      {
        handle.Free();
      }

      return rawData;
    }

    static int GetHighBit(int mask, int bitCount)
    {
      int count = 0;
      for (int i = 0; i < 32; i++)
      {
        int bitMask = 1 << i;

        if ((mask & bitMask) != 0)
          if (++count == bitCount)
            return bitMask;
      }

      return 0;
    }

    static int FirstHighBit(int mask)
    {
      for (int i = 0; i < 32; i++)
        if ((mask & (1 << i)) != 0)
          return i;

      return -1;
    }
    static int FirstLowBit(int mask)
    {
      for (int i = 0; i < 32; i++)
        if ((mask & (1 << i)) == 0)
          return i;

      return -1;
    }

    static int GetCarrierPeriod(int carrier)
    {
      return (int)Math.Round(1000000.0 / (double)carrier);
    }

    static TransmitMode GetTransmitMode(int carrier)
    {
      if (carrier > 100)
        return TransmitMode.CarrierMode;
      else
        return TransmitMode.DCMode;
    }

    static int[] GetTimingDataFromPacket(byte[] packetBytes)
    {
      int[] timingData = new int[packetBytes.Length / 4];

      int timingDataIndex = 0;

      for (int index = 0; index < packetBytes.Length; index += 4)
        timingData[timingDataIndex++] =
          (int)
          (packetBytes[index] +
          (packetBytes[index + 1] << 8) +
          (packetBytes[index + 2] << 16) +
          (packetBytes[index + 3] << 24));

      return timingData;
    }

    #endregion Misc Methods

  }

}
