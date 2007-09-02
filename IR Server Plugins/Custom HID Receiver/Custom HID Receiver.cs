using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Win32.SafeHandles;

using IRServerPluginInterface;

namespace CustomHIDReceiver
{

  public class CustomHIDReceiver: IRServerPlugin, IDisposable
  {

    #region Constants

    public static readonly string ConfigurationFile =
      Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
      "\\IR Server Suite\\IR Server\\Custom HID Receiver.xml";

    static readonly string[] Ports  = new string[] { "None" };

    const int DeviceBufferSize = 255;

    #endregion Constants

    #region Variables

    RemoteHandler _remoteHandler = null;
    FileStream _deviceStream;
    byte[] _deviceBuffer;

    string _deviceID;
    int _inputByte;
    byte _byteMask;
    bool _useAllBytes;

    int _repeatDelay;

    string _lastKeyCode = String.Empty;
    DateTime _lastCodeTime = DateTime.Now;

    bool _disposed = false;

    #endregion Variables

    #region Constructor / Deconstructor

    public CustomHIDReceiver()
    {
      LoadSettings();
    }

    ~CustomHIDReceiver()
    {
      // call Dispose with false.  Since we're in the
      // destructor call, the managed resources will be
      // disposed of anyways.
      Dispose(false);
    }

    #endregion Constructor / Deconstructor

    #region IDisposable Members

    public void Dispose()
    {
      // dispose of the managed and unmanaged resources
      Dispose(true);

      // tell the GC that the Finalize process no longer needs
      // to be run for this object.
      GC.SuppressFinalize(this);
    }

    #endregion

    #region Implementation

    public override string Name { get { return "Custom HID Receiver"; } }
    public override string Version { get { return "1.0.3.4"; } }
    public override string Author { get { return "and-81"; } }
    public override string Description { get { return "Supports HID USB devices."; } }

    public RemoteHandler RemoteCallback
    {
      get { return _remoteHandler; }
      set { _remoteHandler = value; }
    }

    public string[] AvailablePorts  { get { return Ports; }   }

    public void Configure()
    {
      DeviceSelect deviceSelect = new DeviceSelect(_deviceID);
      if (deviceSelect.ShowDialog() == DialogResult.OK)
      {
        _deviceID = deviceSelect.DeviceID;
        SaveSettings();
      }
    }

    public override bool Start()
    {
      if (String.IsNullOrEmpty(_deviceID))
        throw new Exception("No HID Device selected for use");

      Guid guid = new Guid();
      NativeMethods.HidD_GetHidGuid(ref guid);

      string devicePath = FindDevice(guid, _deviceID);
      if (devicePath == null)
        return false;

      SafeFileHandle deviceHandle = NativeMethods.CreateFile(devicePath, FileAccess.Read, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, NativeMethods.EFileAttributes.Overlapped, IntPtr.Zero);

      if (deviceHandle.IsInvalid)
        throw new Win32Exception(Marshal.GetLastWin32Error(), "Failed to open USB HID Device");

      //_deviceWatcher.RegisterDeviceRemoval(deviceHandle);

      _deviceBuffer = new byte[DeviceBufferSize];
      
      _deviceStream = new FileStream(deviceHandle, FileAccess.Read, _deviceBuffer.Length, true);
      _deviceStream.BeginRead(_deviceBuffer, 0, _deviceBuffer.Length, new AsyncCallback(OnReadComplete), null);

      return true;
    }
    public override void Suspend() { }
    public override void Resume() { }
    public override void Stop()
    {
      if (_deviceStream == null)
        return;

      try
      {
        _deviceStream.Close();
      }
      catch (IOException)
      { }
      finally
      {
        _deviceStream = null;
      }
    }

    public bool Transmit(string file) { return false; }
    public LearnStatus Learn(string file) { return LearnStatus.Failure; }

    protected virtual void Dispose(bool disposeManagedResources)
    {
      // process only if mananged and unmanaged resources have
      // not been disposed of.
      if (!this._disposed)
      {
        if (disposeManagedResources)
        {
          // dispose managed resources
          if (_deviceStream != null)
          {
            _deviceStream.Dispose();
            _deviceStream = null;
          }
        }

        // dispose unmanaged resources
        this._disposed = true;
      }
    }

    void LoadSettings()
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationFile);

        _deviceID     = doc.DocumentElement.Attributes["DeviceID"].Value;
        _inputByte    = int.Parse(doc.DocumentElement.Attributes["InputByte"].Value);
        _byteMask     = byte.Parse(doc.DocumentElement.Attributes["ByteMask"].Value);
        _useAllBytes  = bool.Parse(doc.DocumentElement.Attributes["UseAllBytes"].Value);
        _repeatDelay  = int.Parse(doc.DocumentElement.Attributes["RepeatDelay"].Value);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());

        _deviceID     = String.Empty;
        _inputByte    = 3;
        _byteMask     = 0x7F;
        _useAllBytes  = false;
        _repeatDelay  = 250;
      }
    }
    void SaveSettings()
    {
      try
      {
        XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, System.Text.Encoding.UTF8);
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 1;
        writer.IndentChar = (char)9;
        writer.WriteStartDocument(true);
        writer.WriteStartElement("settings"); // <settings>

        writer.WriteAttributeString("DeviceID", _deviceID);
        writer.WriteAttributeString("InputByte", _inputByte.ToString());
        writer.WriteAttributeString("ByteMask", _byteMask.ToString());
        writer.WriteAttributeString("UseAllBytes", _useAllBytes.ToString());
        writer.WriteAttributeString("RepeatDelay", _repeatDelay.ToString());

        writer.WriteEndElement(); // </settings>
        writer.WriteEndDocument();
        writer.Close();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }

    static string FindDevice(Guid classGuid, string deviceID)
    {
      int lastError;

      // 0x12 = DIGCF_PRESENT | DIGCF_DEVICEINTERFACE
      IntPtr handle = NativeMethods.SetupDiGetClassDevs(ref classGuid, "", IntPtr.Zero, 0x12);
      lastError = Marshal.GetLastWin32Error();

      if (handle.ToInt32() == -1)
        throw new Win32Exception(lastError);

      string devicePath = null;

      for (int deviceIndex = 0; ; deviceIndex++)
      {
        NativeMethods.DeviceInfoData deviceInfoData = new NativeMethods.DeviceInfoData();
        deviceInfoData.Size = Marshal.SizeOf(deviceInfoData);

        if (NativeMethods.SetupDiEnumDeviceInfo(handle, deviceIndex, ref deviceInfoData) == false)
        {
          // out of devices or do we have an error?
          lastError = Marshal.GetLastWin32Error();
          if (lastError != 0x0103 && lastError != 0x007E)
          {
            NativeMethods.SetupDiDestroyDeviceInfoList(handle);
            throw new Win32Exception(Marshal.GetLastWin32Error());
          }

          NativeMethods.SetupDiDestroyDeviceInfoList(handle);
          break;
        }

        NativeMethods.DeviceInterfaceData deviceInterfaceData = new NativeMethods.DeviceInterfaceData();
        deviceInterfaceData.Size = Marshal.SizeOf(deviceInterfaceData);

        if (NativeMethods.SetupDiEnumDeviceInterfaces(handle, ref deviceInfoData, ref classGuid, 0, ref deviceInterfaceData) == false)
        {
          NativeMethods.SetupDiDestroyDeviceInfoList(handle);
          throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        uint cbData = 0;

        if (NativeMethods.SetupDiGetDeviceInterfaceDetail(handle, ref deviceInterfaceData, IntPtr.Zero, 0, ref cbData, IntPtr.Zero) == false && cbData == 0)
        {
          NativeMethods.SetupDiDestroyDeviceInfoList(handle);
          throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        NativeMethods.DeviceInterfaceDetailData deviceInterfaceDetailData = new NativeMethods.DeviceInterfaceDetailData();
        deviceInterfaceDetailData.Size = 5;

        if (NativeMethods.SetupDiGetDeviceInterfaceDetail(handle, ref deviceInterfaceData, ref deviceInterfaceDetailData, cbData, IntPtr.Zero, IntPtr.Zero) == false)
        {
          NativeMethods.SetupDiDestroyDeviceInfoList(handle);
          throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        if (deviceInterfaceDetailData.DevicePath.IndexOf(deviceID) != -1)
        {
          NativeMethods.SetupDiDestroyDeviceInfoList(handle);
          devicePath = deviceInterfaceDetailData.DevicePath;
          break;
        }
      }

      return devicePath;
    }

    void OnReadComplete(IAsyncResult asyncResult)
    {
      try
      {
        int bytesRead = _deviceStream.EndRead(asyncResult);

        if (bytesRead == 0)
          throw new Exception("Error reading from HID Device, zero bytes read");

        if (_remoteHandler != null)
        {
          string keyCode = String.Empty;

          if (_useAllBytes)
          {
            keyCode = BitConverter.ToString(_deviceBuffer, 0, bytesRead);
          }
          else if (bytesRead > _inputByte)
          {
            int keyByte = _deviceBuffer[_inputByte] & _byteMask;

            keyCode = keyByte.ToString("X2");
          }

          if (keyCode == _lastKeyCode)
          {
            TimeSpan timeSpan = DateTime.Now - _lastCodeTime;

            if (timeSpan.Milliseconds >= _repeatDelay)
            {
              _remoteHandler(keyCode);
              _lastCodeTime = DateTime.Now;
            }
          }
          else
          {
            _remoteHandler(keyCode);
            _lastCodeTime = DateTime.Now;
            _lastKeyCode = keyCode;
          }          
        }

        _deviceStream.BeginRead(_deviceBuffer, 0, _deviceBuffer.Length, new AsyncCallback(OnReadComplete), null);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }
    
    #endregion Implementation

  }

}
