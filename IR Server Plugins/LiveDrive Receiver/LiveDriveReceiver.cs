using System;
#if TRACE
using System.Diagnostics;
#endif
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace InputService.Plugin
{

  /// <summary>
  /// IR Server Plugin for LiveDrive, Audigy Drive and compatible Creative MIDI IR input devices.
  /// </summary>
  [CLSCompliant(false)]
  public class LiveDriveReceiver : PluginBase, IConfigure, IRemoteReceiver
  {

    #region Interop

    [DllImport("winmm.dll")]
    internal static extern int midiInGetNumDevs();

    [DllImport("winmm.dll")]
    static extern uint midiInOpen(ref int lphMidiIn, int uDeviceID, MidiInProc dwCallback, int dwInstance, int dwFlags);

    [DllImport("winmm.dll")]
    static extern uint midiInStart(int hMidiIn);

    [DllImport("winmm.dll")]
    static extern uint midiInStop(int hMidiIn);

    [DllImport("winmm.dll")]
    static extern uint midiInReset(int hMidiIn);

    [DllImport("winmm.dll")]
    static extern uint midiInClose(int hMidiIn);

    [DllImport("winmm.dll")]
    static extern uint midiInPrepareHeader(int hMidiIn, ref MidiHdr lpMidiInHdr, int uSize);

    [DllImport("winmm.dll")]
    static extern uint midiInUnprepareHeader(int hMidiIn, ref MidiHdr lpMidiInHdr, int uSize);

    [DllImport("winmm.dll")]
    static extern uint midiInAddBuffer(int hMidiIn, int lpMidiInHdr, int uSize);

    #endregion Interop

    #region Structures

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct MidiHdr
    {
      public IntPtr data;
      public Int32 bufferLength;
      public Int32 bytesRecorded;
      public Int32 user;
      public Int32 flags;
      public Int32 next;
      public Int32 reserved1;
      public Int32 offset;
      public Int32 reserved2;
    }

    #endregion Structures

    #region Delegates

    delegate void MidiInProc(int hMidiIn, uint wMsg, int dwInstance, int dwParam1, int dwParam2);

    #endregion Delegates

    #region Constants

    static readonly string ConfigurationFile = Path.Combine(ConfigurationPath, "LiveDrive Receiver.xml");

    const int BufferLength  = 256;

    const int MM_OPEN       = 0x3C1;
		const int MM_CLOSE      = 0x3C2;
		const int MM_DATA       = 0x3C3;
		const int MM_LONGDATA   = 0x3C4;
		const int MM_ERROR      = 0x3C5;
		const int MM_LONGERROR  = 0x3C6;

		const int CALLBACK_FUNCTION     = 0x30000;

		const int MMSYSERR_NOERROR      = 0;
		const int MMSYSERR_BASE         = 0;
		const int MMSYSERR_ERROR        = (MMSYSERR_BASE + 1);  // unspecified error
		const int MMSYSERR_BADDEVICEID  = (MMSYSERR_BASE + 2);  // device ID out of range
		const int MMSYSERR_NOTENABLED   = (MMSYSERR_BASE + 3);  // driver failed enable
		const int MMSYSERR_ALLOCATED    = (MMSYSERR_BASE + 4);  // device already allocated
		const int MMSYSERR_INVALHANDLE  = (MMSYSERR_BASE + 5);  // device handle is invalid
		const int MMSYSERR_NODRIVER     = (MMSYSERR_BASE + 6);  // no device driver present
		const int MMSYSERR_NOMEM        = (MMSYSERR_BASE + 7);  // memory allocation error
		const int MMSYSERR_NOTSUPPORTED = (MMSYSERR_BASE + 8);  // function isn't supported
		const int MMSYSERR_BADERRNUM    = (MMSYSERR_BASE + 9);  // error value out of range
		const int MMSYSERR_INVALFLAG    = (MMSYSERR_BASE + 10); // invalid flag passed
		const int MMSYSERR_INVALPARAM   = (MMSYSERR_BASE + 11); // invalid parameter passed
		const int MMSYSERR_HANDLEBUSY   = (MMSYSERR_BASE + 12); // handle being used simultaneously on another thread (eg callback)
		const int MMSYSERR_INVALIDALIAS = (MMSYSERR_BASE + 13); // specified alias not found
		const int MMSYSERR_BADDB        = (MMSYSERR_BASE + 14); // bad registry database
		const int MMSYSERR_KEYNOTFOUND  = (MMSYSERR_BASE + 15); // registry key not found
		const int MMSYSERR_READERROR    = (MMSYSERR_BASE + 16); // registry read error
		const int MMSYSERR_WRITEERROR   = (MMSYSERR_BASE + 17); // registry write error
		const int MMSYSERR_DELETEERROR  = (MMSYSERR_BASE + 18); // registry delete error
		const int MMSYSERR_VALNOTFOUND  = (MMSYSERR_BASE + 19); // registry value not found
		const int MMSYSERR_NODRIVERCB   = (MMSYSERR_BASE + 20); // driver does not call DriverCallback
		const int MMSYSERR_MOREDATA     = (MMSYSERR_BASE + 21); // more data to be returned
		const int MMSYSERR_LASTERROR    = (MMSYSERR_BASE + 21); // last error in range

		const int MIDIERR_BASE          = 64;
		const int MIDIERR_STILLPLAYING  = (MIDIERR_BASE + 1);

    #endregion Constants

    #region Variables

    RemoteHandler _remoteButtonHandler;

    int _midiIndex = 2;

    bool _stopping = false;
    MidiInProc _midiCallback = null;
    MidiHdr _midiHeader;
    StringBuilder _buffer = null;

    int _midiInHandle = -1;





    #endregion Variables

    #region Implementation

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name         { get { return "LiveDrive"; } }
    /// <summary>
    /// IR Server plugin version.
    /// </summary>
    /// <value>The version.</value>
    public override string Version      { get { return "1.4.2.0"; } }
    /// <summary>
    /// The IR Server plugin's author.
    /// </summary>
    /// <value>The author.</value>
    public override string Author       { get { return "and-81, original MediaPortal plugin by Kenneth A. Burke"; } }
    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    /// <value>The description.</value>
    public override string Description  { get { return "Support for Creative LiveDrive, Audigy Drive and compatible MIDI-based IR receivers"; } }
    /// <summary>
    /// Gets a display icon for the plugin.
    /// </summary>
    /// <value>The icon.</value>
    public override Icon DeviceIcon     { get { return Properties.Resources.Icon; } }

    /// <summary>
    /// Detect the presence of this device.  Devices that cannot be detected will always return false.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the device is present, otherwise <c>false</c>.
    /// </returns>
    public override bool Detect()
    {

      return false;
    }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    public override void Start()
    {
      uint error;

      LoadSettings();

      _stopping = false;

      _buffer = new StringBuilder(BufferLength);

      //MidiInProc
      _midiCallback = new MidiInProc(MidiInCallback);
      _midiHeader = new MidiHdr();
      _midiHeader.data = Marshal.AllocHGlobal(BufferLength);

      _midiHeader.bufferLength = BufferLength;
      _midiHeader.flags = 0;

      error = midiInOpen(ref _midiInHandle, _midiIndex, _midiCallback, 0, CALLBACK_FUNCTION);

      if (error == MMSYSERR_NOERROR)
      {
        error = midiInPrepareHeader(_midiInHandle, ref _midiHeader, Marshal.SizeOf(typeof(MidiHdr)));

        if (error == MMSYSERR_NOERROR)
        {
          IntPtr iptMIDIHdrPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(MidiHdr)));
          Marshal.StructureToPtr(_midiHeader, iptMIDIHdrPtr, true);
          
          error = midiInAddBuffer(_midiInHandle, (int)iptMIDIHdrPtr, Marshal.SizeOf(typeof(MidiHdr)));

          if (error == MMSYSERR_NOERROR)
            error = midiInStart(_midiInHandle);
        }
      }
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
      uint error;

      _stopping = true;

      error = midiInReset(_midiInHandle);
      error = midiInStop(_midiInHandle);
      error = midiInUnprepareHeader(_midiInHandle, ref _midiHeader, Marshal.SizeOf(typeof(MidiHdr)));
      
      Marshal.FreeHGlobal(_midiHeader.data);

      while ((error = midiInClose(_midiInHandle)) == MIDIERR_STILLPLAYING)
        Thread.Sleep(50);
    }

    /// <summary>
    /// Configure the IR Server plugin.
    /// </summary>
    public void Configure(IWin32Window owner)
    {
      LoadSettings();

      Configure config = new Configure();

      config.DeviceIndex = _midiIndex;

      if (config.ShowDialog(owner) == DialogResult.OK)
      {
        _midiIndex = config.DeviceIndex;

        SaveSettings();
      }
    }

    /// <summary>
    /// Callback for remote button presses.
    /// </summary>
    /// <value>The remote callback.</value>
    public RemoteHandler RemoteCallback
    {
      get { return _remoteButtonHandler; }
      set { _remoteButtonHandler = value; }
    }

    
    /// <summary>
    /// Loads the settings.
    /// </summary>
    void LoadSettings()
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationFile);

        _midiIndex = int.Parse(doc.DocumentElement.Attributes["DeviceIndex"].Value);
      }
#if TRACE
      catch (Exception ex)
      {
        Trace.WriteLine(ex.ToString());
#else
      catch
      {
#endif

        _midiIndex = 2;
      }
    }
    /// <summary>
    /// Saves the settings.
    /// </summary>
    void SaveSettings()
    {
      try
      {
        using (XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, Encoding.UTF8))
        {
          writer.Formatting = Formatting.Indented;
          writer.Indentation = 1;
          writer.IndentChar = (char)9;
          writer.WriteStartDocument(true);
          writer.WriteStartElement("settings"); // <settings>

          writer.WriteAttributeString("DeviceIndex", _midiIndex.ToString());

          writer.WriteEndElement(); // </settings>
          writer.WriteEndDocument();
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
    }


    void MidiInCallback(int hMidiIn, uint wMsg, int dwInstance, int dwParam1, int dwParam2)
    {
      switch (wMsg)
      {
        case MM_OPEN:
          break;

        case MM_CLOSE:
          break;

        case MM_LONGDATA:
          MidiHdr midiHeader = (MidiHdr)Marshal.PtrToStructure((IntPtr)dwParam1, typeof(MidiHdr));

          for (int i = 0; midiHeader.bytesRecorded-- > 0; i++)
          {
            byte bteRead = Marshal.ReadByte(midiHeader.data, i);
            switch (bteRead)
            {
              case 0xF0:
              case 0xF7:
                if (_buffer.Length > 0)
                {
                  if (_remoteButtonHandler != null)
                    _remoteButtonHandler(this.Name, _buffer.ToString());

                  _buffer.Remove(0, _buffer.Length);
                }
                break;
              default:
                _buffer.Append(bteRead.ToString("x2"));
                break;
            }
          }

          midiHeader.bytesRecorded = 0;

          if (!_stopping)
            midiInAddBuffer(hMidiIn, dwParam1, Marshal.SizeOf(typeof(MidiHdr)));
          break;

        default:
          break;
      }
    }

    #endregion Implementation

  }

}
