using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

using Microsoft.Win32.SafeHandles;

using IRServerPluginInterface;

namespace IgorPlugUSBReceiver
{

  public class IgorPlugUSBReceiver : IIRServerPlugin
  {

    #region Constants

    static readonly string[] Ports  = new string[] { "None" };

    const int DeviceBufferSize = 256;

    #endregion Constants

    #region Variables

    RemoteHandler _remoteButtonHandler = null;

    Thread _readThread;

    #endregion Variables

    #region Interop

    const int NO_ERROR            = 0;
    const int DEVICE_NOT_PRESENT  = 1;
    const int NO_DATA_AVAILABLE   = 2;
    const int INVALID_BAUDRATE    = 3;
    const int OVERRUN_ERROR       = 4;

    [DllImport("IgorPlug.dll", SetLastError = true)]
    static extern int DoGetInfraCode(out byte[] TimeCodeDiagram, out int DiagramLength);

    /*
    static extern int DoSetDataPortDirection(uchar DirectionByte);
    static extern int DoGetDataPortDirection(uchar* DataDirectionByte);
    static extern int DoSetOutDataPort(uchar DataOutByte);
    static extern int DoGetOutDataPort(uchar* DataOutByte);
    static extern int DoGetInDataPort(uchar* DataInByte);
    static extern int DoEEPROMRead(uchar Address, uchar* DataInByte);
    static extern int DoEEPROMWrite(uchar Address, uchar DataOutByte);
    static extern int DoRS232Send(uchar DataOutByte);
    static extern int DoRS232Read(uchar* DataInByte);
    static extern int DoSetRS232Baud(int BaudRate);
    static extern int DoGetRS232Baud(int* BaudRate);
    */

    #endregion Interop

    #region IIRServerPlugin Members

    public string Name          { get { return "IgorPlug USB"; } }
    public string Version       { get { return "1.0.3.3"; } } 
    public string Author        { get { return "and-81"; } }
    public string Description   { get { return "IgorPlug USB Receiver"; } }
    public bool   CanReceive    { get { return true; } }
    public bool   CanTransmit   { get { return false; } }
    public bool   CanLearn      { get { return false; } }
    public bool   CanConfigure  { get { return false; } }

    public RemoteHandler RemoteCallback
    {
      get { return _remoteButtonHandler; }
      set { _remoteButtonHandler = value; }
    }

    public KeyboardHandler KeyboardCallback { get { return null; } set { } }

    public MouseHandler MouseCallback { get { return null; } set { } }

    public string[] AvailablePorts { get { return Ports; } }

    public void Configure() { }
    public bool Start()
    {
      ThreadStart readThreadStart = new ThreadStart(ReadThread);
      _readThread = new Thread(readThreadStart);
      _readThread.Start();

      return true;
    }
    public void Suspend()   { }
    public void Resume()    { }
    public void Stop()
    {
      _readThread.Abort();
    }

    public bool Transmit(string file) { return false; }
    public LearnStatus Learn(out byte[] data)
    {
      data = null;
      return LearnStatus.Failure;
    }

    public bool SetPort(string port)    { return true; }

    #endregion IIRServerPlugin Members

    #region Implementation

    void ReadThread()
    {
      try
      {
        byte[] deviceBuffer = new byte[DeviceBufferSize];
        int codeLength;
        int returnCode;
        StringBuilder keyCode;

        string lastCode = String.Empty;
        DateTime lastCodeTime = DateTime.Now;
        TimeSpan timeSpan;

        while (true)
        {
          returnCode = DoGetInfraCode(out deviceBuffer, out codeLength);

          switch (returnCode)
          {
            case NO_ERROR:
            case NO_DATA_AVAILABLE:
              break;

            case DEVICE_NOT_PRESENT:
              throw new IOException("Device not present");

            case INVALID_BAUDRATE:
              throw new IOException("Invalid baud rate");

            case OVERRUN_ERROR:
              throw new IOException("Overrun error");

            default:
              throw new IOException(String.Format("Unknown error ({0})", returnCode));
          }

          if (codeLength == 0)
            continue;

          keyCode = new StringBuilder();
          foreach (byte b in deviceBuffer)
            keyCode.Append(b.ToString("X2"));

          timeSpan = DateTime.Now - lastCodeTime;

          if (lastCode != keyCode.ToString()  || timeSpan.Milliseconds > 250)
          {
            lastCode = keyCode.ToString();
            lastCodeTime = DateTime.Now;
            if (_remoteButtonHandler != null)
              _remoteButtonHandler(lastCode);
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }
    
    #endregion Implementation

  }

}
