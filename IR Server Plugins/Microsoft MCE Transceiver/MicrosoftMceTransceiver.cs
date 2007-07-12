using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Win32.SafeHandles;

using IRServerPluginInterface;

namespace MicrosoftMceTransceiver
{

  #region Enumerations

  /// <summary>
  /// The blaster port to send IR codes to
  /// </summary>
  public enum BlasterPort
  {
    /// <summary>
    /// Send IR codes to both blaster ports
    /// </summary>
    Both = 0,
    /// <summary>
    /// Send IR codes to blaster port 1 only
    /// </summary>
    Port_1 = 1,
    /// <summary>
    /// Send IR codes to blaster port 2 only
    /// </summary>
    Port_2 = 2
  }

  /// <summary>
  /// Type of blaster in use
  /// </summary>
  public enum BlasterType
  {
    /// <summary>
    /// Device is a first party Microsoft MCE transceiver
    /// </summary>
    Microsoft = 0,
    /// <summary>
    /// Device is an third party SMK MCE transceiver
    /// </summary>
    SMK = 1
  }

  /// <summary>
  /// Speed to transmit IR codes at
  /// </summary>
  public enum BlasterSpeed
  {
    /// <summary>
    /// None - Do not set the blaster speed
    /// (Note: If an IR code has been sent with a speed setting previously
    /// then that speed setting will continue to take effect, until the
    /// unit's power is cycled)
    /// </summary>
    None    = 0,
    /// <summary>
    /// Fast - Set blaster speed to fast
    /// </summary>
    Fast    = 1,
    /// <summary>
    /// Medium - Set blaster speed to medium
    /// </summary>
    Medium  = 2,
    /// <summary>
    /// Slow - Set blaster speed to slow
    /// </summary>
    Slow    = 3,
  }

  #endregion Enumerations

  #region Delegates

  delegate void RemoteEventHandler(byte[] data);

  delegate void DeviceEventHandler();

  #endregion Delegates

  public class MicrosoftMceTransceiver : IIRServerPlugin
  {

    #region Constants

    static readonly string ConfigurationFile =
      Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
      "\\IR Server Suite\\IR Server\\Microsoft MCE Transceiver.xml";

    static readonly Guid BlasterGuid = new Guid(0x7951772d, 0xcd50, 0x49b7, 0xb1, 0x03, 0x2b, 0xaa, 0xc4, 0x94, 0xfc, 0x57);

    const UInt32 DIGCF_ALLCLASSES       = 0x00000004;
    const UInt32 DIGCF_DEVICEINTERFACE  = 0x00000010;
    const UInt32 DIGCF_PRESENT          = 0x00000002;
    const UInt32 DIGCF_PROFILE          = 0x00000008;

    const double PRONTO_CLOCK = 0.241246;

    const UInt32 PULSE_BIT  = 0x01000000;
    const UInt32 PULSE_MASK = 0x00FFFFFF;

    const UInt16 MCE_TOGGLE_BIT         = 0x8000;
    const UInt16 MCE_TOGGLE_MASK        = 0x7FFF;
    const UInt16 MCE_CUSTOMER_MICROSOFT = 0x800F;

    const UInt16 RC5_TOGGLE_MASK  = 0xF7FF;
    const UInt16 RC5X_TOGGLE_MASK = 0xFFFF;

    const UInt32 RC6_PREFIX_RC6   = 0x000FC950;
    const UInt32 RC6_PREFIX_RC6A  = 0x000FCA90;

    // File Access Types
    const uint GENERIC_READ     = 0x80000000;
    const uint GENERIC_WRITE    = 0x40000000;
    const uint GENERIC_EXECUTE  = 0x20000000;
    const uint GENERIC_ALL      = 0x10000000;

    // Speed packets
    static readonly byte[][] SpeedPackets = new byte[][]
			{
				new byte[] { 0x9F, 0x06, 0x01, 0x44 },	// Fast
				new byte[] { 0x9F, 0x06, 0x01, 0x4A },	// Medium
				new byte[] { 0x9F, 0x06, 0x01, 0x50 },  // Slow
			};

    // Microsoft Port Packets
    static readonly byte[][] MicrosoftPorts = new byte[][]    //MS Device
			{
				new byte[] { 0x9F, 0x08, 0x06 },        // Both
				new byte[] { 0x9F, 0x08, 0x04 },	      // 1
				new byte[] { 0x9F, 0x08, 0x02 },	      // 2
			};

    // SMK Port Packets
    static readonly byte[][] SmkPorts = new byte[][]          //SMK Device
			{
				new byte[] { 0x9F, 0x08, 0x00 },	      // Both
				new byte[] { 0x9F, 0x08, 0x01 },	      // 1
				new byte[] { 0x9F, 0x08, 0x02 },        // 2
			};

    // Device Initialization Packets
    static readonly byte[] initPacket1 = { 0x00, 0xFF, 0xAA, 0xFF, 0x0B };
    static readonly byte[] initPacket2 = { 0xFF, 0x18 };

    // Learn Initialization Packets
    static readonly byte[] learnPacket1 = { 0x9F, 0x0C, 0x0F, 0xA0 };
    static readonly byte[] learnPacket2 = { 0x9F, 0x14, 0x01 };

    #endregion Constants

    #region Interop

    [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Auto)]
    static extern SafeFileHandle CreateFile(
      [MarshalAs(UnmanagedType.LPTStr)] string fileName,
      uint fileAccess,
      [MarshalAs(UnmanagedType.U4)] EFileShare fileShare,
      //[In, Out, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(SecurityAttributesMarshaler))] SecurityAttributes lpSecurityAttributes,
      IntPtr sa,
      [MarshalAs(UnmanagedType.U4)] ECreationDisposition creationDisposition,
      [MarshalAs(UnmanagedType.U4)] EFileAttributes flags,
      IntPtr templateFile);

    [DllImport("kernel32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool CancelIo(SafeFileHandle handle);

    [CLSCompliant(false)]
    [Flags]
    public enum EFileShare : uint
    {
       None   = 0x00000000,
       Read   = 0x00000001,
       Write  = 0x00000002,
       Delete = 0x00000004
    }

    [CLSCompliant(false)]
    public enum ECreationDisposition : uint
    {
       New              = 1,
       CreateAlways     = 2,
       OpenExisting     = 3,
       OpenAlways       = 4,
       TruncateExisting = 5
    }

    [Flags]
    enum EFileAttributes : uint
    {
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
      FirstPipeInstance = 0x00080000
    }

    [StructLayout(LayoutKind.Sequential)]
    struct DeviceInfoData
    {
      public int Size;
      public Guid Class;
      public uint DevInst;
      public IntPtr Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct DeviceInterfaceData
    {
      public int Size;
      public Guid Class;
      public uint Flags;
      public uint Reserved;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct DeviceInterfaceDetailData
    {
      public int Size;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
      public string DevicePath;
    }

    [DllImport("hid")]
    static extern void HidD_GetHidGuid(
      ref Guid guid);

    [DllImport("setupapi", CharSet = CharSet.Auto)]
    static extern IntPtr SetupDiGetClassDevs(
      ref Guid ClassGuid,
      [MarshalAs(UnmanagedType.LPTStr)] string Enumerator,
      IntPtr hwndParent,
      UInt32 Flags);

    [DllImport("setupapi", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetupDiEnumDeviceInfo(
      IntPtr handle,
      int Index,
      ref DeviceInfoData deviceInfoData);

    [DllImport("setupapi", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetupDiEnumDeviceInterfaces(
      IntPtr handle,
      ref DeviceInfoData deviceInfoData,
      ref Guid guidClass,
      int MemberIndex,
      ref DeviceInterfaceData deviceInterfaceData);

    [DllImport("setupapi", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetupDiGetDeviceInterfaceDetail(
      IntPtr handle,
      ref DeviceInterfaceData deviceInterfaceData,
      IntPtr unused1,
      int unused2,
      ref uint requiredSize,
      IntPtr unused3);

    [DllImport("setupapi", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetupDiGetDeviceInterfaceDetail(
      IntPtr handle,
      ref DeviceInterfaceData deviceInterfaceData,
      ref DeviceInterfaceDetailData deviceInterfaceDetailData,
      uint detailSize,
      IntPtr unused1,
      IntPtr unused2);

    [DllImport("setupapi")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetupDiDestroyDeviceInfoList(IntPtr handle);

    #endregion Interop

    #region Variables

    static RemoteButtonHandler _remoteButtonHandler = null;

    static BlasterType  _blasterType  = BlasterType.Microsoft;
    static BlasterSpeed _blasterSpeed = BlasterSpeed.None;
    static BlasterPort _blasterPort   = BlasterPort.Both;

    static int _repeatDelay;
    static int _heldDelay;

    static int _learnTimeout;

    static ArrayList _packetArray;
    static Guid _blasterGuid;
    static FileStream _blasterStream;
    static FileStream _buttonStream;
    static byte[] _learnBuffer;
    static byte[] _buttonBuffer;
    static NotifyWindow _notifyWindow;
    static int _learnStartTick;
    static bool _learning;
    
    static byte[] _irData;

    #endregion Variables
   
    #region IIRServerPlugin Members

    public string Name          { get { return "Microsoft MCE"; } }
    public string Version       { get { return "1.0.3.2"; } }
    public string Author        { get { return "and-81"; } }
    public string Description   { get { return "Microsoft MCE Infrared Transceiver"; } }
    public bool   CanReceive    { get { return true; } }
    public bool   CanTransmit   { get { return true; } }
    public bool   CanLearn      { get { return true; } }
    public bool   CanConfigure  { get { return true; } }

    public RemoteButtonHandler RemoteButtonCallback
    {
      get { return _remoteButtonHandler; }
      set { _remoteButtonHandler = value; }
    }

    public string[] AvailablePorts
    {
      get { return Enum.GetNames(typeof(BlasterPort)); }
    }
    public string[] AvailableSpeeds
    {
      get { return Enum.GetNames(typeof(BlasterSpeed)); }
    }

    public void Configure()
    {
      LoadSettings();

      Configure config = new Configure();

      config.BlastType    = _blasterType;
      config.RepeatDelay  = _repeatDelay;
      config.HeldDelay    = _heldDelay;
      config.LearnTimeout = _learnTimeout;

      if (config.ShowDialog() == DialogResult.OK)
      {
        _blasterType  = config.BlastType;
        _repeatDelay  = config.RepeatDelay;
        _heldDelay    = config.HeldDelay;
        _learnTimeout = config.LearnTimeout;

        SaveSettings();
      }
    }
    public bool Start()
    {
      LoadSettings();

      // Stop Microsoft MCE ehRecvr and ehSched process (if they exist)
      try
      {
        ServiceController[] services = ServiceController.GetServices();
        foreach (ServiceController service in services)
        {
          if (service.ServiceName.Equals("ehRecvr", StringComparison.InvariantCultureIgnoreCase))
          {
            if (service.Status != ServiceControllerStatus.Stopped && service.Status != ServiceControllerStatus.StopPending)
            {
              service.Stop();
            }
          }

          if (service.ServiceName.Equals("ehSched", StringComparison.InvariantCultureIgnoreCase))
          {
            if (service.Status != ServiceControllerStatus.Stopped && service.Status != ServiceControllerStatus.StopPending)
            {
              service.Stop();
            }
          }

          if (service.ServiceName.Equals("mcrdsvc", StringComparison.InvariantCultureIgnoreCase))
          {
            if (service.Status != ServiceControllerStatus.Stopped && service.Status != ServiceControllerStatus.StopPending)
            {
              service.Stop();
            }
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }

      // Kill Microsoft MCE ehtray process (if it exists)
      try
      {
        Process[] processes = Process.GetProcesses();
        foreach (Process proc in processes)
          if (proc.ProcessName.Equals("ehtray", StringComparison.InvariantCultureIgnoreCase))
            proc.Kill();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
      
      _blasterGuid = BlasterGuid;
      Init();

      return true;
    }
    public void Suspend() { }
    public void Resume() { }
    public void Stop()
    {
      OnDeviceRemoval();
    }

    public bool Transmit(string file)
    {
      byte[] fileData = ReadIRFile(file);
      if (fileData == null)
        return false;

      return Send(fileData, _blasterPort, _blasterSpeed, _blasterType);
    }
    public LearnStatus Learn(string file)
    {
      _irData = null;
      
      BeginLearn(new RemoteEventHandler(LearnIRDone));

      // Wait for the learning to finish ...
      while (_learning && Environment.TickCount < _learnStartTick + _learnTimeout)
        Thread.Sleep(500);

      if (_learning)
      {
        try
        {
          CancelIo(_blasterStream.SafeFileHandle);
        }
        catch { }

        _learning = false;
        
        return LearnStatus.Timeout;
      }

      if (_irData != null)
      {
        FileStream fileStream = new FileStream(file, FileMode.Create);
        fileStream.Write(_irData, 0, _irData.Length);
        fileStream.Flush();
        fileStream.Close();

        return LearnStatus.Success;
      }

      return LearnStatus.Failure;
    }

    public bool SetPort(string port)
    {
      try
      {
        _blasterPort = (BlasterPort)Enum.Parse(typeof(BlasterPort), port);
      }
      catch
      {
        return false;
      }

      return true;
    }
    public bool SetSpeed(string speed)
    {
      try
      {
        _blasterSpeed = (BlasterSpeed)Enum.Parse(typeof(BlasterSpeed), speed);
      }
      catch
      {
        return false;
      }

      return true;
    }

    #endregion IIRServerPlugin Members

    #region Implementation

    static void LoadSettings()
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationFile);

        _blasterType  = (BlasterType)Enum.Parse(typeof(BlasterType), doc.DocumentElement.Attributes["BlastType"].Value);
        _repeatDelay  = int.Parse(doc.DocumentElement.Attributes["RepeatDelay"].Value);
        _heldDelay    = int.Parse(doc.DocumentElement.Attributes["HeldDelay"].Value);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());

        _blasterType  = BlasterType.Microsoft;
        _repeatDelay  = 500;
        _heldDelay    = 250;
      }
    }
    static void SaveSettings()
    {
      try
      {
        XmlTextWriter writer  = new XmlTextWriter(ConfigurationFile, System.Text.Encoding.UTF8);
        writer.Formatting     = Formatting.Indented;
        writer.Indentation    = 1;
        writer.IndentChar     = (char)9;
        writer.WriteStartDocument(true);
        writer.WriteStartElement("settings"); // <settings>

        writer.WriteAttributeString("BlastType",    Enum.GetName(typeof(BlasterType), _blasterType));
        writer.WriteAttributeString("RepeatDelay",  _repeatDelay.ToString());
        writer.WriteAttributeString("HeldDelay",    _heldDelay.ToString());

        writer.WriteEndElement(); // </settings>
        writer.WriteEndDocument();
        writer.Close();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }

    static void Init()
    {
      _learnBuffer = new byte[4096];
      _buttonBuffer = new byte[256];

      _notifyWindow = new NotifyWindow();
      _notifyWindow.Create();
      _notifyWindow.Class = _blasterGuid;
      _notifyWindow.DeviceArrival += new DeviceEventHandler(OnDeviceArrival);
      _notifyWindow.DeviceRemoval += new DeviceEventHandler(OnDeviceRemoval);
      _notifyWindow.RegisterDeviceArrival();

      // we need somewhere to store the smaller packets as they arrive
      _packetArray = new ArrayList();

      Open();

      // Initialize Device ...
      _blasterStream.Write(initPacket1, 0, initPacket1.Length);
      _blasterStream.Write(initPacket2, 0, initPacket2.Length);
      _blasterStream.Flush();

      _buttonStream.BeginRead(_buttonBuffer, 0, _buttonBuffer.Length, new AsyncCallback(OnButtonReadComplete), null);
    }

    static void Open()
    {
      string devicePath;
      SafeFileHandle deviceHandle;
      int lastError;

      devicePath = FindDevice(_blasterGuid);

      if (devicePath == null)
        throw new Exception("No Microsoft MCE Blaster device detected");

      deviceHandle = CreateFile(devicePath + "\\Pipe01", GENERIC_READ | GENERIC_WRITE, 0, IntPtr.Zero, ECreationDisposition.OpenExisting, EFileAttributes.Overlapped, IntPtr.Zero);
      lastError = Marshal.GetLastWin32Error();

      if (lastError != 0)
        throw new Win32Exception(lastError);

      _notifyWindow.RegisterDeviceRemoval(deviceHandle.DangerousGetHandle());

      _blasterStream = new FileStream(deviceHandle, FileAccess.ReadWrite, _learnBuffer.Length, true);



      Guid hidGuid = new Guid();
      HidD_GetHidGuid(ref hidGuid);
      
      devicePath = FindDevice(hidGuid);

      if (devicePath == null)
        throw new Exception("No Microsoft MCE Receiver device detected");

      deviceHandle = CreateFile(devicePath, GENERIC_READ, EFileShare.Read | EFileShare.Write, IntPtr.Zero, ECreationDisposition.OpenExisting, EFileAttributes.Overlapped, IntPtr.Zero);
      lastError = Marshal.GetLastWin32Error();

      if (lastError != 0)
        throw new Win32Exception(lastError);

      _buttonStream = new FileStream(deviceHandle, FileAccess.Read, 128, true);
    }

    static string FindDevice(Guid classGuid)
    {
      string devicePath = null;

      IntPtr handle = SetupDiGetClassDevs(ref classGuid, null, IntPtr.Zero, DIGCF_DEVICEINTERFACE | DIGCF_PRESENT);

      int lastError = Marshal.GetLastWin32Error();

      if (lastError != 0)
        throw new Win32Exception(lastError);

      for (int deviceIndex = 0; ; deviceIndex++)
      {
        DeviceInfoData deviceInfoData = new DeviceInfoData();
        deviceInfoData.Size = Marshal.SizeOf(deviceInfoData);

        if (!SetupDiEnumDeviceInfo(handle, deviceIndex, ref deviceInfoData))
        {
          lastError = Marshal.GetLastWin32Error();

          // out of devices or do we have an error?
          if (lastError != 0x0103 && lastError != 0x007E)
          {
            SetupDiDestroyDeviceInfoList(handle);
            throw new Win32Exception(lastError);
          }

          SetupDiDestroyDeviceInfoList(handle);
          break;
        }

        DeviceInterfaceData deviceInterfaceData = new DeviceInterfaceData();
        deviceInterfaceData.Size = Marshal.SizeOf(deviceInterfaceData);

        if (!SetupDiEnumDeviceInterfaces(handle, ref deviceInfoData, ref classGuid, 0, ref deviceInterfaceData))
        {
          SetupDiDestroyDeviceInfoList(handle);
          throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        uint cbData = 0;

        if (!SetupDiGetDeviceInterfaceDetail(handle, ref deviceInterfaceData, IntPtr.Zero, 0, ref cbData, IntPtr.Zero) && cbData == 0)
        {
          SetupDiDestroyDeviceInfoList(handle);
          throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        DeviceInterfaceDetailData deviceInterfaceDetailData = new DeviceInterfaceDetailData();
        deviceInterfaceDetailData.Size = 5;

        if (!SetupDiGetDeviceInterfaceDetail(handle, ref deviceInterfaceData, ref deviceInterfaceDetailData, cbData, IntPtr.Zero, IntPtr.Zero))
        {
          SetupDiDestroyDeviceInfoList(handle);
          throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        if ((deviceInterfaceDetailData.DevicePath.IndexOf("#vid_0471&pid_0815") != -1) ||   // Microsoft/Philips 2005
            (deviceInterfaceDetailData.DevicePath.IndexOf("#vid_045e&pid_006d") != -1) ||   // Microsoft/Philips 2004
            (deviceInterfaceDetailData.DevicePath.IndexOf("#vid_1460&pid_9150") != -1) ||   // HP
            (deviceInterfaceDetailData.DevicePath.IndexOf("#vid_107b&pid_3009") != -1) ||   // FIC Spectra/Mycom Mediacenter
            (deviceInterfaceDetailData.DevicePath.IndexOf("#vid_0609&pid_031d") != -1) ||   // Toshiba/Hauppauge SMK MCE remote
            (deviceInterfaceDetailData.DevicePath.IndexOf("#vid_1784&pid_0001") != -1) ||   // Another Hauppauge OEM MCE remote
            (deviceInterfaceDetailData.DevicePath.IndexOf("#vid_03ee&pid_2501") != -1) ||   // Mitsumi MCE remote
            (deviceInterfaceDetailData.DevicePath.IndexOf("#vid_1509&pid_9242") != -1) ||   // Fujitsu Scaleo-E
            (deviceInterfaceDetailData.DevicePath.StartsWith(@"\\?\hid#irdevice&col01#2"))) // Microsoft/Philips 2005 (Vista)
        {
          SetupDiDestroyDeviceInfoList(handle);
          devicePath = deviceInterfaceDetailData.DevicePath;
          break;
        }
      }

      return devicePath;
    }

    static void OnButtonReadComplete(IAsyncResult asyncResult)
    {
      try
      {
        if (_learning)
          return;

        if (_buttonStream == null)
          return;

        int bytesRead = _buttonStream.EndRead(asyncResult);
        if (bytesRead == 0)
        {
          _buttonStream.Close();
          _buttonStream = null;
          return;
        }

        if (bytesRead == 13 && _buttonBuffer[1] == 1)
        {
          string keyCode = _buttonBuffer[5].ToString();

          if (_remoteButtonHandler != null)
            _remoteButtonHandler(keyCode);
        }

        // begin another asynchronous read from the device
        if (_buttonStream != null)
          _buttonStream.BeginRead(_buttonBuffer, 0, _buttonBuffer.Length, new AsyncCallback(OnButtonReadComplete), null);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }

    static void OnLearnComplete(IAsyncResult asyncResult)
    {
      try
      {
        if (!_learning)
          return;

        if (_blasterStream == null)
          return;

        int bytesRead = _blasterStream.EndRead(asyncResult);
        if (bytesRead == 0)
        {
          _blasterStream.Close();
          _blasterStream = null;
          return;
        }

        if (_learnBuffer[0] == 0x9F || (_learnBuffer[0] & 0x80) != 0x80)
        {
          // ignore garbage - begin another asynchronous read from the device
          if (_blasterStream != null)
            _blasterStream.BeginRead(_learnBuffer, 0, _learnBuffer.Length, new AsyncCallback(OnLearnComplete), asyncResult.AsyncState);
          return;
        }

        byte[] packetBuffer = new byte[bytesRead];

        Array.Copy(_learnBuffer, packetBuffer, bytesRead);

        lock (_packetArray)
          _packetArray.Add(packetBuffer);

        byte[] finalPacket = FinalizePacket();

        if (Array.IndexOf(packetBuffer, (byte)0x80) != -1)
        {
          ((RemoteEventHandler)asyncResult.AsyncState)(finalPacket);
          return;
        }

        // begin another asynchronous read from the device
        if (_blasterStream != null)
          _blasterStream.BeginRead(_learnBuffer, 0, _learnBuffer.Length, new AsyncCallback(OnLearnComplete), asyncResult.AsyncState);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }

    static byte[] FinalizePacket()
    {
      int packetLength = 0;
      int packetOffset = 0;

      foreach (byte[] packetBytes in _packetArray)
      {
        int indexOf9F = Array.IndexOf(packetBytes, (byte)0x9F);

        if (indexOf9F == -1)
          packetLength += packetBytes.Length;
        else
          packetLength += indexOf9F + 1;
      }

      byte[] packetFinal = new byte[packetLength];

      foreach (byte[] packetBytes in _packetArray)
      {
        foreach (byte packetByte in packetBytes)
        {
          if (packetByte == 0x9F)
            packetFinal[packetOffset++] = (byte)0x80;
          else
            packetFinal[packetOffset++] = packetByte;

          if (packetByte == 0x9F)
            break;
        }
      }

      _packetArray = new ArrayList();

      return packetFinal;
    }

    static void OnDeviceArrival()
    {
      if (_blasterStream != null)
        return;

      Open();
    }
    static void OnDeviceRemoval()
    {
      if (_blasterStream == null)
        return;

      try
      {
        _blasterStream.Close();
        _blasterStream = null;

        _buttonStream.Close();
        _buttonStream = null;
      }
      catch (IOException)
      {
        // we are closing the stream so ignore this
      }
    }

    static void BeginLearn(RemoteEventHandler learnCallback)
    {
      try
      {
        CancelIo(_blasterStream.SafeFileHandle);

        lock (_packetArray)
          _packetArray = new ArrayList();

        _blasterStream.Write(learnPacket1, 0, learnPacket1.Length);
        _blasterStream.Write(learnPacket2, 0, learnPacket2.Length);
        _blasterStream.Flush();

        _learnStartTick = Environment.TickCount;
        _learning = true;
        _blasterStream.BeginRead(_learnBuffer, 0, _learnBuffer.Length, new AsyncCallback(OnLearnComplete), learnCallback);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());

        if (learnCallback != null)
          learnCallback(null);
      }
    }

    static void LearnIRDone(byte[] data)
    {
      _irData = data;
      _learning = false;
    }

    static byte[] ReadIRFile(string filename)
    {
      try
      {
        if (!File.Exists(filename))
          return null;

        FileStream irFile = new FileStream(filename, FileMode.Open);

        int length = (int)irFile.Length;

        byte[] data = new byte[length];

        irFile.Read(data, 0, length);

        irFile.Close();

        if (IsNativeData(data))
          return ImportNative(data);
        else if (IsProntoData(data))
          return ImportPronto(data);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }

      return null;
    }

    static bool Send(byte[] packet, BlasterPort port, BlasterSpeed speed, BlasterType type)
    {
      if (_blasterStream == null)
        return false;

      byte[][] portPackets;
      switch (type)
      {
        case BlasterType.Microsoft:
          portPackets = MicrosoftPorts;
          break;

        case BlasterType.SMK:
          portPackets = SmkPorts;
          break;

        default:
          return false;
      }

      // Set speed
      if (speed != BlasterSpeed.None)
      {
        int speedIndex = (int)speed - 1;
        _blasterStream.Write(SpeedPackets[speedIndex], 0, SpeedPackets[speedIndex].Length);
      }

      // Set port
      _blasterStream.Write(portPackets[(int)port], 0, portPackets[(int)port].Length);

      // Send packet
      _blasterStream.Write(packet, 0, packet.Length);

      // Flush packet
      _blasterStream.Flush();

      return true;
    }

    static bool IsNativeData(byte[] data)
    {
      if ((data[0] & 0x80) != 0)
        return true;
      else
        return false;
    }
    static bool IsProntoData(byte[] data)
    {
      if ((data[0] == '0' && data[1] == '0' && data[2] == '0' && data[3] == '0' && data[4] == ' ') ||
          (data[0] == '0' && data[1] == '1' && data[2] == '0' && data[3] == '0' && data[4] == ' ') ||
          (data[0] == '5' && data[1] == '0' && data[2] == '0' && data[3] == '0' && data[4] == ' ') ||
          (data[0] == '5' && data[1] == '0' && data[2] == '0' && data[3] == '1' && data[4] == ' ') ||
          (data[0] == '6' && data[1] == '0' && data[2] == '0' && data[3] == '0' && data[4] == ' ') ||
          (data[0] == '6' && data[1] == '0' && data[2] == '0' && data[3] == '1' && data[4] == ' '))
      {
        return true;
      }
      else
      {
        return false;
      }
    }

    static ushort[] ProntoData(byte[] fileBytes)
    {
      StringBuilder dataStringBuilder = new StringBuilder(fileBytes.Length);
      foreach (char dataByte in fileBytes)
        dataStringBuilder.Append((char)dataByte);

      string[] stringData = dataStringBuilder.ToString().Split(new char[] { ' ' });

      ushort[] prontoData = new ushort[stringData.Length];
      for (int i = 0; i < stringData.Length; i++)
        prontoData[i] = ushort.Parse(stringData[i], System.Globalization.NumberStyles.HexNumber);

      return prontoData;
    }

    static byte[] ImportNative(byte[] data)
    {
      byte bytesToCopy;
      int dataBytesIndex = 0;
      List<byte> outBytes = new List<byte>();

      for (int counter = 0; counter < data.Length; counter += 4)
      {
        bytesToCopy = (byte)(data.Length - counter < 4 ? data.Length - counter : 4);

        outBytes.Add((byte)(0x80 + bytesToCopy));

        for (int index = 0; index < bytesToCopy; index++)
          outBytes.Add(data[dataBytesIndex + index]);

        dataBytesIndex += bytesToCopy;
      }

      outBytes.Add(0x80);

      return outBytes.ToArray();
    }
    static byte[] ImportPronto(byte[] file)
    {
      ushort[] data = ProntoData(file);

      byte[] dataBytes = null;

      if (data[0] == 0x0000 || data[0] == 0x0100)
        dataBytes = ImportProntoLearned(data);
      else if (data[0] == 0x5000)
        dataBytes = ImportProntoRC5(data);
      else if (data[0] == 0x5001)
        dataBytes = ImportProntoRC5X(data);
      else if (data[0] == 0x6000)
        dataBytes = ImportProntoRC6(data);
      else if (data[0] == 0x6001)
        dataBytes = ImportProntoRC6X(data);

      if (dataBytes == null)
        return null;

      return ImportNative(dataBytes);
    }
    static byte[] ImportProntoLearned(ushort[] prontoData)
    {
      int length = prontoData.Length;
      if (length < 5)
        return null;

      ushort clock = prontoData[1];
      if (clock == 0)
        return null;

      double carrier = (double)clock * PRONTO_CLOCK;

      ushort sequence;
      if (prontoData[2] != 0)
        sequence = (ushort)(prontoData[2] * 2);
      else
        sequence = (ushort)(prontoData[3] * 2);

      bool nextIsPulse = true;

      int remaining = 0;
      int repeatCount = 0;

      int dataPosition;
      int index = 0;

      List<byte> outData = new List<byte>();

      for (; ; )
      {
        dataPosition = 4 + index;

        if (remaining != 0)
        {
          if (remaining < 0x80)
          {
            outData.Add((byte)(remaining | (nextIsPulse ? 0x00 : 0x80)));
            remaining = 0;
          }
          else
          {
            outData.Add((byte)(0x7F | (nextIsPulse ? 0x00 : 0x80)));
            remaining -= 0x7F;
          }
        }

        if (remaining == 0)
        {
          if ((dataPosition >= prontoData.Length) || (sequence == 0))
          {
            if (repeatCount >= 3)
              return outData.ToArray();

            index = 0; // restart
            repeatCount++;
            nextIsPulse = true;
            continue;
          }

          index++;

          if (dataPosition > prontoData.Length)
            return null;

          sequence--;
          remaining = (ushort)(((prontoData[dataPosition] * carrier) + 25) / 50);
          nextIsPulse = !nextIsPulse;
        }
      }
    }

    // TODO: Import Pronto RCx types ...
    static byte[] ImportProntoRC5(ushort[] prontoData)
    {
      return null;
    }
    static byte[] ImportProntoRC5X(ushort[] prontoData)
    {
      return null;
    }
    static byte[] ImportProntoRC6(ushort[] prontoData)
    {
      return null;
    }
    static byte[] ImportProntoRC6X(ushort[] prontoData)
    {
      return null;
    }

    #endregion Implementation

  }

}
