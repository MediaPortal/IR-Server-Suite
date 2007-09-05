using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;

using Microsoft.Win32.SafeHandles;

namespace MicrosoftMceTransceiver
{

  public class DriverVista : Driver
  {

    #region Constants

    // Device variables
    const int DeviceBufferSize  = 100;
    const int PacketTimeout     = 100;
    const int WriteSyncTimeout  = 5000;

    #endregion Constants

    #region Enumerations

    public enum IoCtrl : uint
    {
      StartReceive  = 0x0F608028,
      StopReceive   = 0x0F60802C,
      GetDetails    = 0x0F604004,
      GetBlasters   = 0x0F604008,
      Receive       = 0x0F604022,
      Transmit      = 0x0F608015,
    }

    /// <summary>
    /// IR Device Capability Flags.
    /// </summary>
    [Flags]
    public enum DeviceCapabilityFlags : uint
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

    [Flags]
    public enum TransmitFlags : uint
    {
      /// <summary>
      /// Pulse Mode.
      /// </summary>
      PulseMode = 0x01,
      /// <summary>
      /// DC Mode.
      /// </summary>
      DCMode    = 0x02,
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

    #region Structures

    [StructLayout(LayoutKind.Sequential)]
    public struct TransmitChunk
    {
      /// <summary>
      /// Next chunk offset.
      /// </summary>
      public uint OffsetToNextChunk;
      /// <summary>
      /// Repeat count.
      /// </summary>
      public uint RepeatCount;
      /// <summary>
      /// Number of bytes.
      /// </summary>
      public uint ByteCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TransmitParams
    {
      /// <summary>
      /// Bitmask containing ports to transmit on.
      /// </summary>
      public uint TransmitPortMask;
      /// <summary>
      /// Carrier period.
      /// </summary>
      public uint CarrierPeriod;
      /// <summary>
      /// Transmit Flags.
      /// </summary>
      [MarshalAs(UnmanagedType.U4)]
      public TransmitFlags Flags;
      /// <summary>
      /// Pulse Size.  If Pulse Mode Flag set.
      /// </summary>
      public uint PulseSize;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ReceiveParams
    {
      /// <summary>
      /// Last packet in block?
      /// </summary>
      public uint DataEnd;
      /// <summary>
      /// Number of bytes in block.
      /// </summary>
      public uint ByteCount;
      /// <summary>
      /// Carrier frequency of IR received.
      /// </summary>
      public uint CarrierFrequency;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct StartReceiveParams
    {
      /// <summary>
      /// Index of the receiver to use.
      /// </summary>
      public uint Receiver;
      /// <summary>
      /// Receive timeout, in milliseconds?
      /// </summary>
      public uint Timeout;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DeviceCapabilities
    {
      /// <summary>
      /// Protocol version.  Currently must be 100 (1.0).
      /// </summary>
      public uint ProtocolVersion;
      /// <summary>
      /// Number of transmit ports – 0-32.
      /// </summary>
      public uint TransmitPorts;
      /// <summary>
      /// Number of receive ports – 0-32 (For beanbag, this is two (one for learning, one for normal).
      /// </summary>
      public uint ReceivePorts;
      /// <summary>
      /// Bitmask identifying which receivers are learning receivers – low bit is the first receiver, second-low bit is the second receiver, etc ...
      /// </summary>
      public uint LearningMask;
      /// <summary>
      /// Device flags.
      /// </summary>
      [MarshalAs(UnmanagedType.U4)]
      public DeviceCapabilityFlags DetailsFlags;
    }

    #endregion Structures

    #region Interop

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool DeviceIoControl(
      SafeFileHandle handle,
      [MarshalAs(UnmanagedType.U4)] IoCtrl ioControlCode,
      IntPtr inBuffer, int inBufferSize,
      IntPtr outBuffer, int outBufferSize,
      out int bytesReturned,
      ref NativeOverlapped overlapped);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool DeviceIoControl(
      SafeFileHandle handle,
      [MarshalAs(UnmanagedType.U4)] IoCtrl ioControlCode,
      IntPtr inBuffer, int inBufferSize,
      IntPtr outBuffer, int outBufferSize,
      out int bytesReturned,
      IntPtr overlapped);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetOverlappedResult(
      SafeFileHandle handle,
      ref NativeOverlapped overlapped,
      out int bytesTransferred,
      bool wait);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    static extern SafeFileHandle CreateFile(
      [MarshalAs(UnmanagedType.LPTStr)] string fileName,
      [MarshalAs(UnmanagedType.U4)] FileAccessTypes fileAccess,
      [MarshalAs(UnmanagedType.U4)] FileShares fileShare,
      IntPtr securityAttributes,
      [MarshalAs(UnmanagedType.U4)] CreationDisposition creationDisposition,
      [MarshalAs(UnmanagedType.U4)] FileAttributes flags,
      IntPtr templateFile);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool CancelIo(
      SafeFileHandle handle);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool CloseHandle(
      SafeFileHandle handle);

    #endregion Interop

    #region Variables

    #region Device Details

    uint _numTxPorts = 0;
    uint _numRxPorts = 0;
    uint _learnPortMask = 0;
    bool _legacyDevice = false;
    bool _canFlashLed = false;

    bool[] _blasters;

    uint _receivePort = 0;
    uint _learnPort = 0;

    #endregion Device Details

    NotifyWindow _notifyWindow;

    SafeFileHandle _eHomeHandle;

    Thread _readThread;
    ReadThreadMode _readThreadMode;

    IrCode _learningCode;

    //StreamWriter _debugFile;

    #endregion Variables

    #region Constructor

    public DriverVista(Guid deviceGuid, string devicePath, RemoteCallback remoteCallback, KeyboardCallback keyboardCallback, MouseCallback mouseCallback)
      : base(deviceGuid, devicePath, remoteCallback, keyboardCallback, mouseCallback)
    {

    }

    #endregion Constructor

    #region Device Control Functions

    void StartReceive(uint receivePort, uint timeout)
    {
      int bytesReturned;

      StartReceiveParams structure;
      structure.Receiver = receivePort;
      structure.Timeout = timeout;

      IntPtr structPtr = Marshal.AllocHGlobal(Marshal.SizeOf(structure));

      try
      {
        Marshal.StructureToPtr(structure, structPtr, false);

        IoControlSync(IoCtrl.StartReceive, structPtr, Marshal.SizeOf(structure), IntPtr.Zero, 0, out bytesReturned);
      }
      catch
      {
        throw;
      }
      finally
      {
        Marshal.FreeHGlobal(structPtr);
      }
    }

    void StopReceive()
    {
      int bytesReturned;
      IoControlSync(IoCtrl.StopReceive, IntPtr.Zero, 0, IntPtr.Zero, 0, out bytesReturned);
    }

    void GetDeviceCapabilities()
    {
      int bytesReturned;

      DeviceCapabilities structure = new DeviceCapabilities();

      IntPtr structPtr = Marshal.AllocHGlobal(Marshal.SizeOf(structure));

      try
      {
        Marshal.StructureToPtr(structure, structPtr, false);

        IoControlSync(IoCtrl.GetDetails, IntPtr.Zero, 0, structPtr, Marshal.SizeOf(structure), out bytesReturned);

        structure = (DeviceCapabilities)Marshal.PtrToStructure(structPtr, typeof(DeviceCapabilities));
      }
      catch
      {
        throw;
      }
      finally
      {
        Marshal.FreeHGlobal(structPtr);
      }

      _numTxPorts = structure.TransmitPorts;
      _numRxPorts = structure.ReceivePorts;
      _learnPortMask = structure.LearningMask;

      int receivePort = FirstLowBit(_learnPortMask);
      if (receivePort != -1)
        _receivePort = (uint)receivePort;

      int learnPort = FirstHighBit(_learnPortMask);
      if (learnPort != -1)
        _learnPort = (uint)learnPort;
      else
        _learnPort = _receivePort;

      DeviceCapabilityFlags flags = structure.DetailsFlags;
      _legacyDevice = (int)(flags & DeviceCapabilityFlags.Legacy) != 0;
      _canFlashLed = (int)(flags & DeviceCapabilityFlags.FlashLed) != 0;
    }

    void GetBlasters()
    {
      int bytesReturned;

      if (_numTxPorts == 0)
        return;

      _blasters = new bool[_numTxPorts];
      for (int i = 0; i < _blasters.Length; i++)
        _blasters[i] = false;

      uint data = 0;

      IntPtr pointerToData = Marshal.AllocHGlobal(sizeof(uint));

      try
      {
        Marshal.StructureToPtr(data, pointerToData, false);

        IoControlSync(IoCtrl.GetBlasters, IntPtr.Zero, 0, pointerToData, sizeof(uint), out bytesReturned);

        data = (uint)Marshal.PtrToStructure(pointerToData, typeof(uint));
      }
      catch
      {
        throw;
      }
      finally
      {
        Marshal.FreeHGlobal(pointerToData);
      }

      for (int j = 0; j < _blasters.Length; j++)
        _blasters[j] = ((data & (((int)1) << j)) != 0);
    }

    void TransmitIR(byte[] irData, int carrier, uint transmitPortMask)
    {
      int bytesReturned;

      TransmitParams transmitParams = new TransmitParams();
      transmitParams.TransmitPortMask = transmitPortMask;

      if (carrier == IrCode.CarrierFrequencyUnknown)
        carrier = IrCode.CarrierFrequencyDefault;

      if (IsPulseMode((uint)carrier))
      {
        transmitParams.Flags = TransmitFlags.PulseMode;
        transmitParams.PulseSize = (uint)carrier;
      }
      else
      {
        //transmitParams.Flags = TransmitFlags.DCMode;
        transmitParams.CarrierPeriod = GetCarrierPeriod((uint)carrier);
      }

      TransmitChunk transmitChunk = new TransmitChunk();
      transmitChunk.OffsetToNextChunk = 0;
      transmitChunk.RepeatCount = 1;
      transmitChunk.ByteCount = (uint)irData.Length;

      int bufferSize = irData.Length + Marshal.SizeOf(typeof(TransmitChunk)) + 8;
      byte[] buffer = new byte[bufferSize];

      byte[] rawTransmitChunk = RawSerialize(transmitChunk);
      Array.Copy(rawTransmitChunk, buffer, rawTransmitChunk.Length);

      Array.Copy(irData, 0, buffer, rawTransmitChunk.Length, irData.Length);

      IntPtr structurePtr = Marshal.AllocHGlobal(Marshal.SizeOf(transmitParams));
      IntPtr bufferPtr = Marshal.AllocHGlobal(buffer.Length);

      try
      {
        Marshal.StructureToPtr(transmitParams, structurePtr, true);

        Marshal.Copy(buffer, 0, bufferPtr, buffer.Length);

        IoControlSync(IoCtrl.Transmit, structurePtr, Marshal.SizeOf(typeof(TransmitParams)), bufferPtr, bufferSize, out bytesReturned);
      }
      catch
      {
        throw;
      }
      finally
      {
        Marshal.FreeHGlobal(structurePtr);
        Marshal.FreeHGlobal(bufferPtr);
      }
    }

    void IoControlSync(IoCtrl ioControlCode, IntPtr inBuffer, int inBufferSize, IntPtr outBuffer, int outBufferSize, out int bytesReturned)
    {
      NativeOverlapped overlapped;
      overlapped.InternalLow = IntPtr.Zero;
      overlapped.InternalHigh = IntPtr.Zero;
      overlapped.OffsetLow = 0;
      overlapped.OffsetHigh = 0;

      try
      {
        int lastError;

        using (WaitHandle waitHandle = new ManualResetEvent(false))
        {
          overlapped.EventHandle = waitHandle.SafeWaitHandle.DangerousGetHandle();

          if (!DeviceIoControl(_eHomeHandle, ioControlCode, inBuffer, inBufferSize, outBuffer, outBufferSize, out bytesReturned, ref overlapped))
          {
            lastError = Marshal.GetLastWin32Error();
            if (lastError != Win32ErrorCodes.ERROR_IO_PENDING)
              throw new Win32Exception(lastError);

            waitHandle.WaitOne();

            if (!GetOverlappedResult(_eHomeHandle, ref overlapped, out bytesReturned, false))
            {
              lastError = Marshal.GetLastWin32Error();
              throw new Win32Exception(lastError);
            }
          }
        }

      }
      catch
      {
        CancelIo(_eHomeHandle);
        throw;
      }
    }

    #endregion Device Control Functions

    #region Driver overrides

    public override void Start()
    {
      //_debugFile = new StreamWriter("\\DriverVista.log", false);
      //_debugFile.AutoFlush = true;

      _notifyWindow = new NotifyWindow();
      _notifyWindow.Class = _deviceGuid;

      _eHomeHandle = CreateFile(_devicePath, FileAccessTypes.GenericRead | FileAccessTypes.GenericWrite, FileShares.None, IntPtr.Zero, CreationDisposition.OpenExisting, FileAttributes.Overlapped, IntPtr.Zero);
      int lastError = Marshal.GetLastWin32Error();
      if (_eHomeHandle.IsInvalid)
        throw new Win32Exception(lastError);

      GetAllDeviceInformation();

      StartReceive(_receivePort, PacketTimeout);

      _readThreadMode = ReadThreadMode.Receiving;

      StartReadThread();

      _notifyWindow.Create();
      _notifyWindow.DeviceArrival += new DeviceEventHandler(OnDeviceArrival);
      _notifyWindow.DeviceRemoval += new DeviceEventHandler(OnDeviceRemoval);
      _notifyWindow.RegisterDeviceRemoval(_eHomeHandle.DangerousGetHandle());
    }
    public override void Stop()
    {
      OnDeviceRemoval();

      CloseDevice();

      //_debugFile.Close();
    }

    public override IrCode Learn(int learnTimeout)
    {
      //_debugFile.WriteLine("Learn");

      StopReadThread();

      _learningCode = new IrCode();

      StartReceive(_learnPort, PacketTimeout);

      _readThreadMode = ReadThreadMode.Learning;

      StartReadThread();

      int learnStartTick = Environment.TickCount;

      // Wait for the learning to finish ...
      while (_readThreadMode == ReadThreadMode.Learning && Environment.TickCount < learnStartTick + learnTimeout)
        Thread.Sleep(PacketTimeout);

      //_debugFile.WriteLine("End Learn");

      ReadThreadMode modeWas = _readThreadMode;

      StopReadThread();

      StartReceive(_receivePort, PacketTimeout);

      _readThreadMode = ReadThreadMode.Receiving;

      StartReadThread();

      switch (modeWas)
      {
        case ReadThreadMode.Learning:
          // Timeout.
          return null;

        case ReadThreadMode.LearningFailed:
          // Failure.
          return null;

        case ReadThreadMode.LearningDone:
          //_debugFile.WriteLine(_learningCode.ToByteArray());

          if (_learningCode.FinalizeData())
            return _learningCode; // Success.
          else
            return null; // Failure.

        default:
          return null;
      }
    }

    public override void Send(IrCode code, uint port)
    {
      byte[] data = DataPacket(code);

      TransmitIR(data, code.Carrier, port);
    }
    
    #endregion Driver overrides

    #region Implementation

    byte[] DataPacket(IrCode code)
    {
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

    void GetAllDeviceInformation()
    {
      GetDeviceCapabilities();
      GetBlasters();
    }

    void StartReadThread()
    {
      _readThread = new Thread(new ThreadStart(ReadThread));
      _readThread.IsBackground = true;
      _readThread.Name = "IR Server Microsoft MCE Transceiver Read";
      _readThread.Start();
    }
    void StopReadThread()
    {
      if (_readThread != null)
      {
        _readThreadMode = ReadThreadMode.Stop;

        _readThread.Abort();

        if (Thread.CurrentThread != _readThread)
          _readThread.Join();

        _readThread = null;
      }
    }

    void CloseDevice()
    {
      if (_eHomeHandle != null)
        CloseHandle(_eHomeHandle);
    }
    
    void OnDeviceArrival()
    {
      _notifyWindow.UnregisterDeviceArrival();

      StartReceive(_receivePort, PacketTimeout);

      _readThreadMode = ReadThreadMode.Receiving;

      StartReadThread();

      _notifyWindow.RegisterDeviceRemoval(_eHomeHandle.DangerousGetHandle());
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

      IntPtr deviceBufferPtr  = IntPtr.Zero;
      IntPtr receiveParamsPtr = IntPtr.Zero;

      try
      {
        deviceBufferPtr = Marshal.AllocHGlobal(DeviceBufferSize);

        int receiveParamsSize = Marshal.SizeOf(typeof(ReceiveParams)) + DeviceBufferSize + 8;
        receiveParamsPtr = Marshal.AllocHGlobal(receiveParamsSize);

        ReceiveParams receiveParams = new ReceiveParams();
        receiveParams.ByteCount = DeviceBufferSize;
        Marshal.StructureToPtr(receiveParams, receiveParamsPtr, false);

        while (_readThreadMode != ReadThreadMode.Stop)
        {
          IoControlSync(IoCtrl.Receive, IntPtr.Zero, 0, receiveParamsPtr, receiveParamsSize, out bytesRead);

          if (bytesRead > Marshal.SizeOf(receiveParams))
          {
            int dataSize = bytesRead;

            bytesRead -= Marshal.SizeOf(receiveParams);

            sinceLastPacket = DateTime.Now.Subtract(lastPacketTime);
            if (sinceLastPacket.TotalMilliseconds >= PacketTimeout + 50)
              IrDecoder.DecodeIR(null, null, null, null);

            lastPacketTime = DateTime.Now;

            byte[] packetBytes = new byte[bytesRead];
            byte[] dataBytes = new byte[dataSize];

            Marshal.Copy(receiveParamsPtr, dataBytes, 0, dataSize);
            Array.Copy(dataBytes, dataSize - bytesRead, packetBytes, 0, bytesRead);

            int[] timingData = GetTimingDataFromPacket(packetBytes);

            //_debugFile.WriteLine("Received:");
            //Dump(timingData);

            if (_readThreadMode == ReadThreadMode.Learning)
              _learningCode.AddTimingData(timingData);
            else
              IrDecoder.DecodeIR(timingData, _remoteCallback, _keyboardCallback, _mouseCallback);              
          }

          // Determine carrier frequency when learning ...
          if (_readThreadMode == ReadThreadMode.Learning && bytesRead >= Marshal.SizeOf(receiveParams))
          {
            ReceiveParams receiveParams2 = (ReceiveParams)Marshal.PtrToStructure(receiveParamsPtr, typeof(ReceiveParams));

            if (receiveParams2.DataEnd != 0 && receiveParams2.CarrierFrequency != 0)
            {
              _learningCode.Carrier = (int)receiveParams2.CarrierFrequency;
              _readThreadMode = ReadThreadMode.LearningDone;
            }
          }
        }
      }
      catch
      {
        CancelIo(_eHomeHandle);
      }
      finally
      {
        StopReceive();

        if (deviceBufferPtr != IntPtr.Zero)
          Marshal.FreeHGlobal(deviceBufferPtr);

        if (receiveParamsPtr != IntPtr.Zero)
          Marshal.FreeHGlobal(receiveParamsPtr);
      }
    }

    #endregion Implementation

    #region Misc Methods

    static byte[] RawSerialize(object anything)
    {
      int rawSize = Marshal.SizeOf(anything);
      byte[] rawData = new byte[rawSize];

      GCHandle handle = GCHandle.Alloc(rawData, GCHandleType.Pinned);
      IntPtr buffer = handle.AddrOfPinnedObject();

      Marshal.StructureToPtr(anything, buffer, false);

      handle.Free();

      return rawData;
    }

    static byte ConvertBcdToByte(byte b)
    {
      return (byte)(((b >> 4) * 10) + (b & 15));
    }

    int FirstHighBit(uint mask)
    {
      for (int i = 0; i < 32; i++)
        if ((mask & (1 << i)) != 0)
          return i;

      return -1;
    }
    int FirstLowBit(uint mask)
    {
      for (int i = 0; i < 32; i++)
        if ((mask & (1 << i)) == 0)
          return i;

      return -1;
    }

    static uint GetCarrierPeriod(uint carrier)
    {
      return (uint)(1000000 / carrier);
    }

    static bool IsPulseMode(uint carrier)
    {
      if (carrier > 0 && carrier < 100)
        return true;

      return false;
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
    /*
    void Dump(Array array)
    {
      foreach (object item in array)
      {
        _debugFile.Write(item);
        _debugFile.Write(", ");
      }

      _debugFile.WriteLine();
    }
    */
    #endregion Misc Methods

  }

}
