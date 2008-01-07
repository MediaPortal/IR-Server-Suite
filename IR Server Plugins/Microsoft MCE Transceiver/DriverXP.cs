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

  /// <summary>
  /// Driver class for Windows XP eHome driver.
  /// </summary>
  class DriverXP : Driver
  {

    #region Interop

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool ReadFile(
      SafeFileHandle handle,
      IntPtr buffer,
      int bytesToRead,
      out int bytesRead,
      IntPtr overlapped);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool WriteFile(
      SafeFileHandle handle,
      byte[] buffer,
      int bytesToWrite,
      out int bytesWritten,
      IntPtr overlapped);

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

    const int TimingResolution = 50; // In microseconds.

    const double ClockFrequency = 2412460.0;

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
      new byte[] { 0x9F, 0x08, 0x04 },        // 1
      new byte[] { 0x9F, 0x08, 0x02 },        // 2
    };

    // SMK / Topseed Port Packets
    static readonly byte[][] SmkTopseedPorts = new byte[][]
    {
      new byte[] { 0x9F, 0x08, 0x00 },        // Both
      new byte[] { 0x9F, 0x08, 0x01 },        // 1
      new byte[] { 0x9F, 0x08, 0x02 },        // 2
    };

    // Start, Stop and Reset Packets
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
    static readonly byte[] ResetPacket = { 0xFF, 0xFE };

    // Misc Packets
    static readonly byte[] SetCarrierFreqPacket   = { 0x9F, 0x06, 0x01, 0x80 };
    static readonly byte[] SetInputPortPacket     = { 0x9F, 0x14, 0x00 };
    static readonly byte[] SetTimeoutPacket       = { 0x9F, 0x0C, 0x00, 0x00 };

    #endregion Constants

    #region Variables

    NotifyWindow _notifyWindow;

    SafeFileHandle _eHomeHandle;

    Thread _readThread;
    ReadThreadMode _readThreadMode;
    ManualResetEvent _stopReadThread;

    IrCode _learningCode;

    DeviceType _deviceType = DeviceType.Microsoft;

    bool _deviceAvailable;

    int _decodeCarry;

    #endregion Variables

    #region Constructor

    public DriverXP(Guid deviceGuid, string devicePath, RemoteCallback remoteCallback, KeyboardCallback keyboardCallback, MouseCallback mouseCallback)
      : base(deviceGuid, devicePath, remoteCallback, keyboardCallback, mouseCallback)
    {
      if (devicePath.IndexOf(VidSMK, StringComparison.OrdinalIgnoreCase) != -1 || devicePath.IndexOf(VidTopseed, StringComparison.OrdinalIgnoreCase) != -1)
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
      DebugOpen("\\MicrosoftMceTransceiver_DriverXP.log");
      DebugWriteLine("Start()");
#endif

      _notifyWindow = new NotifyWindow();
      _notifyWindow.Class = _deviceGuid;

      OpenDevice();
      StartReadThread();

      // Initialize device ...
      WriteSync(StartPacket);

      // Testing some commands that MCE sends, but I don't know what they mean (what do these get back?)
      WriteSync(new byte[] { 0xFF, 0x0B });
      WriteSync(new byte[] { 0x9F, 0x05 });
      WriteSync(new byte[] { 0x9F, 0x0D });
      Thread.Sleep(4 * PacketTimeout);
      
      SetTimeout(PacketTimeout);
      SetInputPort(InputPort.Receive);

      _notifyWindow.Create();
      _notifyWindow.RegisterDeviceArrival();
      _notifyWindow.RegisterDeviceRemoval(_eHomeHandle.DangerousGetHandle());
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

      _notifyWindow.DeviceArrival -= new DeviceEventHandler(OnDeviceArrival);
      _notifyWindow.DeviceRemoval -= new DeviceEventHandler(OnDeviceRemoval);

      /*
      try
      {
        WriteSync(StopPacket);
        Thread.Sleep(PacketTimeout);
      }
#if DEBUG
      catch (Exception ex)
      {
        DebugWriteLine(ex.ToString());
      }
#else
      catch
      {
      }
#endif
      */

      StopReadThread();
      CloseDevice();

      _notifyWindow.Dispose();
      _notifyWindow = null;

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

      WriteSync(StopPacket);

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
        OpenDevice();
        StartReadThread();
      }
      catch
      {
#if DEBUG
        DebugWriteLine("Resume(): Failed to start device");
#endif
      }

      //WriteSync(StartPacket);
      //Thread.Sleep(PacketTimeout);
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
    public override void Send(IrCode code, int port)
    {
#if DEBUG
      DebugWriteLine("Send()");
      DebugDump(code.TimingData);
#endif

      // Reset device (hopefully this will stop the blaster from stalling)
      //WriteSync(ResetPacket);
      //Thread.Sleep(PacketTimeout);

      SetOutputPort(port);
      SetCarrierFrequency(code.Carrier);

      // Send packet
      WriteSync(DataPacket(code));

      // Force a delay between blasts (hopefully solves back-to-back blast errors) ...
      //Thread.Sleep(PacketTimeout);
    }

    #endregion Driver overrides

    #region Implementation

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

      // Construct data bytes into "packet" ...
      List<byte> packet = new List<byte>();

      for (int index = 0; index < code.TimingData.Length; index++)
      {
        double time = (double)code.TimingData[index];

        byte duration = (byte)Math.Abs(Math.Round(time / TimingResolution));
        bool pulse = (time > 0);

#if DEBUG
        DebugWrite("{0}{1}, ", pulse ? '+' : '-', duration * TimingResolution);
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

      int timeoutSamples = 1000 * timeout / TimingResolution;   // Timeout as a multiple of the timing resolution

      timeoutPacket[2] = (byte)(timeoutSamples >> 8);
      timeoutPacket[3] = (byte)(timeoutSamples & 0xFF);

      WriteSync(timeoutPacket);
    }

    /// <summary>
    /// Sets the IR receiver input port.
    /// </summary>
    /// <param name="port">The input port.</param>
    void SetInputPort(InputPort port)
    {
      byte[] inputPortPacket = new byte[SetInputPortPacket.Length];
      SetInputPortPacket.CopyTo(inputPortPacket, 0);

      inputPortPacket[2] = (byte)(port + 1);

      WriteSync(inputPortPacket);
    }

    /// <summary>
    /// Sets the IR blaster port.
    /// </summary>
    /// <param name="port">The output port.</param>
    void SetOutputPort(int port)
    {
      byte[] portPacket;
      switch (_deviceType)
      {
        case DeviceType.Microsoft:  portPacket = MicrosoftPorts[port];  break;
        case DeviceType.SmkTopseed: portPacket = SmkTopseedPorts[port]; break;
        default:
          throw new ApplicationException("Invalid device type");
      }

      WriteSync(portPacket);
    }

    /// <summary>
    /// Sets the IR output carrier frequency.
    /// </summary>
    /// <param name="carrier">The IR carrier frequency.</param>
    void SetCarrierFrequency(int carrier)
    {
      if (carrier == IrCode.CarrierFrequencyUnknown)
      {
        carrier = IrCode.CarrierFrequencyDefault;
#if DEBUG
        DebugWriteLine("SetCarrierFrequency(): No carrier frequency specificied, using default ({0})", carrier);
#endif
      }
#if DEBUG
      else
      {
        DebugWriteLine("SetCarrierFrequency({0})", carrier);
      }
#endif

      byte[] carrierPacket = new byte[SetCarrierFreqPacket.Length];
      SetCarrierFreqPacket.CopyTo(carrierPacket, 0);

      if (carrier != IrCode.CarrierFrequencyDCMode)
      {
        carrierPacket[2] = 0x80;
        carrierPacket[3] = (byte)Math.Round(ClockFrequency / carrier);
      }

      WriteSync(carrierPacket);
    }

    /// <summary>
    /// Start the device read thread.
    /// </summary>
    void StartReadThread()
    {
#if DEBUG
      DebugWriteLine("StartReadThread()");
#endif

      if (_readThread != null)
        return;

      _stopReadThread = new ManualResetEvent(false);
      _readThreadMode = ReadThreadMode.Receiving;

      _readThread = new Thread(new ThreadStart(ReadThread));
      _readThread.Name = "MicrosoftMceTransceiver.DriverXP.ReadThread";
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
        return;

      _readThreadMode = ReadThreadMode.Stop;
      _stopReadThread.Set();

      if (!_readThread.Join(PacketTimeout * 2))
        _readThread.Abort();

      _stopReadThread.Close();
      _stopReadThread = null;

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
        return;

      _eHomeHandle = CreateFile(_devicePath, CreateFileAccessTypes.GenericRead | CreateFileAccessTypes.GenericWrite, CreateFileShares.None, IntPtr.Zero, CreateFileDisposition.OpenExisting, CreateFileAttributes.Overlapped, IntPtr.Zero);
      int lastError = Marshal.GetLastWin32Error();
      if (_eHomeHandle.IsInvalid)
      {
        _eHomeHandle = null;
        throw new Win32Exception(lastError);
      }

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
        return;

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
      StartReadThread();
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
      byte[] packetBytes;
      int lastError;

      WaitHandle waitHandle = new ManualResetEvent(false);
      SafeHandle safeWaitHandle = waitHandle.SafeWaitHandle;
      WaitHandle[] waitHandles = new WaitHandle[] { waitHandle, _stopReadThread };

      IntPtr deviceBufferPtr = IntPtr.Zero;

      try
      {
        bool success = false;
        safeWaitHandle.DangerousAddRef(ref success);
        if (!success)
          throw new ApplicationException("Failed to initialize safe wait handle");

        DeviceIoOverlapped overlapped = new DeviceIoOverlapped();
        overlapped.ClearAndSetEvent(safeWaitHandle.DangerousGetHandle());

        deviceBufferPtr = Marshal.AllocHGlobal(DeviceBufferSize);

        while (_readThreadMode != ReadThreadMode.Stop)
        {
          bytesRead = 0;

          bool readDevice = ReadFile(_eHomeHandle, deviceBufferPtr, DeviceBufferSize, out bytesRead, overlapped.Overlapped);
          lastError = Marshal.GetLastWin32Error();

          if (!readDevice)
          {
            if (lastError != ErrorSuccess && lastError != ErrorIoPending)
              throw new Win32Exception(lastError);

            while (true)
            {
              int handle = WaitHandle.WaitAny(waitHandles, 2 * PacketTimeout, false);

              if (handle == ErrorWaitTimeout)
                continue;
              else if (handle == 0)
                break;
              else if (handle == 1)
                throw new ThreadInterruptedException("Read thread stopping by request");
              else
                throw new ApplicationException("Invalid wait handle return");
            }

            bool getOverlapped = GetOverlappedResult(_eHomeHandle, overlapped.Overlapped, out bytesRead, true);
            lastError = Marshal.GetLastWin32Error();

            if (!getOverlapped && lastError != ErrorSuccess)
              throw new Win32Exception(lastError);
          }

          if (bytesRead == 0)
            continue;

          packetBytes = new byte[bytesRead];
          Marshal.Copy(deviceBufferPtr, packetBytes, 0, bytesRead);

          int[] timingData = null;

          if (_decodeCarry != 0 || packetBytes[0] >= 0x81 && packetBytes[0] <= 0x8F)
          {
            timingData = GetTimingDataFromPacket(packetBytes);
          }
#if DEBUG
          else
          {
            DebugWriteLine("Received data:");
            DebugDump(packetBytes);

            int indexOf9F = Array.IndexOf(packetBytes, (byte)0x9F);
            while (indexOf9F != -1)
            {

              if (packetBytes.Length > indexOf9F + 3 && packetBytes[indexOf9F + 1] == 0x04) // 9F 04 XX XX - IR Sample Period
              {
                byte b1 = packetBytes[indexOf9F + 2];
                byte b2 = packetBytes[indexOf9F + 3];

                int irPeriod = ((b1 * 256) + b2);
                DebugWriteLine("IR Sample Period: {0}", irPeriod);
              }

              if (packetBytes.Length > indexOf9F + 3 && packetBytes[indexOf9F + 1] == 0x0C) // 9F 0C XX XX - Timeout
              {
                byte b1 = packetBytes[indexOf9F + 2];
                byte b2 = packetBytes[indexOf9F + 3];

                int timeout = ((b1 * 256) + b2);
                DebugWriteLine("IR Timeout Period: {0}", TimingResolution * timeout / 1000);
              }

              if (packetBytes.Length > indexOf9F + 2 && packetBytes[indexOf9F + 1] == 0x14) // 9F 14 XX - Port
              {
                byte b1 = packetBytes[indexOf9F + 2];

                DebugWriteLine("IR Input Port: {0}", b1);
              }


              if (packetBytes.Length > indexOf9F + 1)
              {
                indexOf9F = Array.IndexOf(packetBytes, (byte)0x9F, indexOf9F + 1);
              }
              else
              {
                break;
              }
            }

            double firmware = 0.0;

            int indexOfFF = Array.IndexOf(packetBytes, (byte)0xFF);
            while (indexOfFF != -1)
            {

              if (packetBytes.Length > indexOfFF + 2 && packetBytes[indexOfFF + 1] == 0x0B) // 9F 0B XX - Firmware x.x00
              {
                byte b1 = packetBytes[indexOfFF + 2];

                firmware += (b1 >> 4) + (0.1 * (b1 & 0x0F));
                DebugWriteLine("Firmware: {0}", firmware);
              }

              if (packetBytes.Length > indexOfFF + 2 && packetBytes[indexOfFF + 1] == 0x1B) // 9F 1B XX - Firmware 0.0xx
              {
                byte b1 = packetBytes[indexOfFF + 2];

                firmware += (0.01 * (b1 >> 4)) + (0.001 * (b1 & 0x0F));
                DebugWriteLine("Firmware: {0}", firmware);
              }


              if (packetBytes.Length > indexOfFF + 1)
              {
                indexOfFF = Array.IndexOf(packetBytes, (byte)0xFF, indexOfFF + 1);
              }
              else
              {
                break;
              }
            }


          }
#endif

          switch (_readThreadMode)
          {
            case ReadThreadMode.Receiving:
              {
                IrDecoder.DecodeIR(timingData, _remoteCallback, _keyboardCallback, _mouseCallback);
                break;
              }

            case ReadThreadMode.Learning:
              {
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

                // Example: 9F 01 02 9F 15 00 BE 80
                int indexOf9F = Array.IndexOf(packetBytes, (byte)0x9F);
                while (indexOf9F != -1)
                {
                  if (packetBytes.Length > indexOf9F + 3 && packetBytes[indexOf9F + 1] == 0x15) // 9F 15 XX XX
                  {
                    byte b1 = packetBytes[indexOf9F + 2];
                    byte b2 = packetBytes[indexOf9F + 3];

                    int onTime, onCount;
                    GetIrCodeLengths(_learningCode, out onTime, out onCount);

                    double carrierCount = (b1 * 256) + b2;

                    if (carrierCount / onCount < 2.0)
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

        if (_eHomeHandle != null)
          CancelIo(_eHomeHandle);
      }
      finally
      {
        if (deviceBufferPtr != IntPtr.Zero)
          Marshal.FreeHGlobal(deviceBufferPtr);

        safeWaitHandle.DangerousRelease();
        waitHandle.Close();
      }

#if DEBUG
      DebugWriteLine("Read Thread Ended");
#endif
    }

    /// <summary>
    /// Synchronously write a packet of bytes to the device.
    /// </summary>
    /// <param name="data">Packet to write to device.</param>
    void WriteSync(byte[] data)
    {
#if DEBUG
      DebugWriteLine("WriteSync()");
      DebugDump(data);
#endif

      if (!_deviceAvailable)
        throw new ApplicationException("Device not available");

      int lastError;

      WaitHandle waitHandle = new ManualResetEvent(false);
      SafeHandle safeWaitHandle = waitHandle.SafeWaitHandle;
      WaitHandle[] waitHandles = new WaitHandle[] { waitHandle };

      try
      {
        bool success = false;
        safeWaitHandle.DangerousAddRef(ref success);
        if (!success)
          throw new ApplicationException("Failed to initialize safe wait handle");

        DeviceIoOverlapped overlapped = new DeviceIoOverlapped();
        overlapped.ClearAndSetEvent(safeWaitHandle.DangerousGetHandle());

        int bytesWritten = 0;
        bool writeDevice = WriteFile(_eHomeHandle, data, data.Length, out bytesWritten, overlapped.Overlapped);
        lastError = Marshal.GetLastWin32Error();

        if (writeDevice)
          return;

        if (lastError != ErrorSuccess && lastError != ErrorIoPending)
          throw new Win32Exception(lastError);

        int handle = WaitHandle.WaitAny(waitHandles, WriteSyncTimeout, false);

        if (handle == ErrorWaitTimeout)
          throw new ApplicationException("Timeout trying to write data to device");
        else if (handle != 0)
          throw new ApplicationException("Invalid wait handle return");

        bool getOverlapped = GetOverlappedResult(_eHomeHandle, overlapped.Overlapped, out bytesWritten, true);
        lastError = Marshal.GetLastWin32Error();

        if (!getOverlapped && lastError != ErrorSuccess)
          throw new Win32Exception(lastError);

        Thread.Sleep(PacketTimeout);
      }
      catch
      {
        if (_eHomeHandle != null)
          CancelIo(_eHomeHandle);

        throw;
      }
      finally
      {
        safeWaitHandle.DangerousRelease();
        waitHandle.Close();
      }
    }

    /// <summary>
    /// Turn raw packet data int pulse timing data.
    /// </summary>
    /// <param name="packet">The raw device packet.</param>
    /// <returns>Timing data.</returns>
    int[] GetTimingDataFromPacket(byte[] packet)
    {
#if DEBUG
      DebugWriteLine("GetTimingDataFromPacket()");

      // TODO: Remove this try/catch block once the IndexOutOfRangeException is corrected...
      try
#endif
      {
#if DEBUG
        if (_decodeCarry != 0)
        {
          DebugWriteLine("Decode Carry EXISTS: {0}", _decodeCarry);
          DebugDump(packet);
        }
#endif

        List<int> timingData = new List<int>();

        int len = 0;

        for (int index = 0; index < packet.Length; )
        {
          byte curByte = packet[index];

          if (_decodeCarry == 0)
          {
            if (curByte == 0x9F)
              break;

            if (curByte < 0x81 || curByte > 0x8F)
              return null;
          }

          int bytes = _decodeCarry;
          if (_decodeCarry == 0)
            bytes = curByte & 0x7F;
          else
            _decodeCarry = 0;

          if (index + bytes + 1 > packet.Length)
          {
            _decodeCarry = (index + bytes + 1) - packet.Length;
            bytes -= _decodeCarry;

#if DEBUG
            DebugWriteLine("Decode Carry SET: {0}", _decodeCarry);
            DebugDump(packet);
#endif
          }

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
              timingData.Add(len * TimingResolution);
              len = 0;
            }
          }

          index = j;
        }

        if (len != 0)
          timingData.Add(len * TimingResolution);

#if DEBUG
        DebugWriteLine("Received:");
        DebugDump(timingData.ToArray());
#endif

        return timingData.ToArray();
      }
#if DEBUG
      catch (Exception ex)
      {
        DebugWriteLine(ex.ToString());
        DebugWriteLine("Method Input:");
        DebugDump(packet);

        return null;
      }
#endif
    }

    /// <summary>
    /// Determines the pulse count and total time of the supplied IrCode.
    /// This is used to determine the carrier frequency.
    /// </summary>
    /// <param name="code">The IrCode to analyse.</param>
    /// <param name="onTime">The total ammount of pulse time.</param>
    /// <param name="onCount">The total count of pulses.</param>
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
