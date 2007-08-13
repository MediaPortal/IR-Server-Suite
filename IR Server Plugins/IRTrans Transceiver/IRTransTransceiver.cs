using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Win32.SafeHandles;

using IRServerPluginInterface;

namespace IRTransTransceiver
{

  #region Delegates

  delegate void RemoteEventHandler(byte[] data);

  #endregion Delegates

  #region Enumerations

  public enum IrTransStatus
  {
    STATUS_MESSAGE          = 1,
    STATUS_TIMING           = 2,
    STATUS_DEVICEMODE       = 3,
    STATUS_RECEIVE          = 4,
    STATUS_LEARN            = 5,
    STATUS_REMOTELIST       = 6,
    STATUS_COMMANDLIST      = 7,
    STATUS_TRANSLATE        = 8,
    STATUS_FUNCTION         = 9,
    STATUS_DEVICEMODEEX     = 10,
    STATUS_DEVICEDATA       = 11,
    STATUS_LCDDATA          = 12,
    STATUS_FUNCTIONEX       = 13,
    STATUS_DEVICEMODEEXN    = 14,
    STATUS_IRDB             = 15,
    STATUS_TRANSLATIONFILE  = 16,
    STATUS_IRDBFILE         = 17,
    STATUS_BUSLIST          = 18,
    STATUS_LEARNDIRECT      = 19,
    STATUS_IRDBFLASH		    = 20,
    STATUS_ANALOGINPUT		  = 21,
  }
  public enum IrTransCommand
  {
    COMMAND_SEND            = 1,
    COMMAND_LRNREM          = 2,
    COMMAND_LRNTIM          = 3,
    COMMAND_LRNCOM          = 4,
    COMMAND_CLOSE           = 5,
    COMMAND_STATUS          = 6,
    COMMAND_RESEND          = 7,
    COMMAND_LRNRAW          = 8,
    COMMAND_LRNRPT          = 9,
    COMMAND_LRNTOG          = 10,
    COMMAND_SETSTAT         = 11,
    COMMAND_LRNLONG         = 12,
    COMMAND_LRNRAWRPT       = 13,
    COMMAND_RELOAD          = 14,
    COMMAND_LCD             = 15,
    COMMAND_LEARNSTAT       = 16,
    COMMAND_TEMP            = 17,
    COMMAND_GETREMOTES      = 18,
    COMMAND_GETCOMMANDS     = 19,
    COMMAND_STORETRANS      = 20,
    COMMAND_LOADTRANS       = 21,
    COMMAND_SAVETRANS       = 22,
    COMMAND_FLASHTRANS      = 23,
    COMMAND_FUNCTIONS       = 24,
    COMMAND_TESTCOM         = 25,
    COMMAND_LONGSEND        = 26,
    COMMAND_SHUTDOWN        = 27,
    COMMAND_SENDCCF         = 28,
    COMMAND_LCDINIT         = 29,
    COMMAND_SETSWITCH       = 30,
    COMMAND_STATUSEX        = 31,
    COMMAND_RESET           = 32,
    COMMAND_DEVICEDATA      = 33,
    COMMAND_STARTCLOCK      = 34,
    COMMAND_LCDSTATUS       = 35,
    COMMAND_FUNCTIONEX      = 36,
    COMMAND_MCE_CHARS       = 37,
    COMMAND_SUSPEND         = 38,
    COMMAND_RESUME          = 39,
    COMMAND_DELETECOM       = 40,
    COMMAND_EMPTY           = 41,
    COMMAND_SETSTAT2        = 42,
    COMMAND_STATUSEXN       = 43,
    COMMAND_BRIGHTNESS      = 44,
    COMMAND_DEFINECHAR      = 45,
    COMMAND_STOREIRDB       = 46,
    COMMAND_FLASHIRDB       = 47,
    COMMAND_SAVEIRDB        = 48,
    COMMAND_LOADIRDB        = 49,
    COMMAND_LED             = 50,
    COMMAND_TRANSFILE       = 51,
    COMMAND_IRDBFILE        = 52,
    COMMAND_LISTBUS         = 53,
    COMMAND_SENDCCFSTR      = 54,
    COMMAND_LEARNDIRECT     = 55,
    COMMAND_TESTCOMEX       = 56,
    COMMAND_SENDCCFSTRS     = 57,
    COMMAND_SETSTATEX       = 58,
    COMMAND_DELETEREM       = 59,
    COMMAND_READ_ANALOG     = 60,
  }

  #endregion Enumerations

  #region Interop Structures

  [StructLayout(LayoutKind.Sequential), CLSCompliant(false)]
  public struct NETWORKRECV
  {
    public UInt32 ClientID;
    public Int16 StatusLen;
    public Int16 StatusType;
    public Int16 Address;
    public UInt16 CommandNum;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
    public string Remote;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
    public string Command;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
    public string Data;
  }
  /*
  [StructLayout(LayoutKind.Sequential), CLSCompliant(false)]
  public struct NETWORKSTATUS
  {
	  public UInt32 ClientID;
	  public Int16 StatusLen;
	  public Int16 StatusType;
	  public Int16 Address;
	  public UInt16 NetStatus;
	  public UInt16 StatusLevel;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
    public string Align;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
	  public string Message;
  }
  
  [StructLayout(LayoutKind.Sequential), CLSCompliant(false)]
  public struct NETWORKCOMMAND
  {
	  public byte netcommand;
    public byte mode;
    public UInt16 timeout;
    public Int32 address;
    public Int32 protocol_version;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
    public string remote;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
    public string command;
    public byte trasmit_freq;
  }

  [StructLayout(LayoutKind.Sequential), CLSCompliant(false)]
  public struct LCDCOMMAND
  {
    public byte netcommand;
    public byte mode;
    public byte lcdcommand;
    public byte timeout;
    public Int32 address;
    public Int32 protocol_version;
    public byte wid;
    public byte hgt;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
    public string framebuffer;
  }
  */
  #endregion Interop Structures

  public class IRTransTransceiver : IIRServerPlugin
  {

    #region Constants

    static readonly string ConfigurationFile =
      Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
      "\\IR Server Suite\\IR Server\\IRTrans Transceiver.xml";

    static readonly string[] Ports = new string[] { "Default" };

    const string  DefaultRemoteModel    = "mediacenter";
    const string  DefaultServerAddress  = "localhost";
    const int     DefaultServerPort     = 21000;

    const int     IRTransClientID       = 0;
    const int     IRTransProtocolVer    = 209;

    #endregion Constants

    #region Variables

    static RemoteHandler _remoteButtonHandler = null;

    static string _blasterPort = Ports[0];

    static Socket _socket;
    static IAsyncResult _asynResult;
    static AsyncCallback _pfnCallBack;
    static string _irTransRemoteModel;
    static string _irTransServerAddress;
    static int _irTransServerPort;

    #endregion Variables
   
    #region IIRServerPlugin Members

    public string Name          { get { return "IRTrans (Experimental)"; } }
    public string Version       { get { return "1.0.3.3"; } }
    public string Author        { get { return "and-81"; } }
    public string Description   { get { return "IRTrans Transceiver (Experimental)"; } }
    public bool   CanReceive    { get { return true; } }
    public bool   CanTransmit   { get { return false; } }
    public bool   CanLearn      { get { return false; } }
    public bool   CanConfigure  { get { return true; } }

    public RemoteHandler RemoteCallback
    {
      get { return _remoteButtonHandler; }
      set { _remoteButtonHandler = value; }
    }

    public KeyboardHandler KeyboardCallback { get { return null; } set { } }

    public MouseHandler MouseCallback { get { return null; } set { } }

    public string[] AvailablePorts
    {
      get { return Ports; }
    }

    public void Configure()
    {
      LoadSettings();

      Configure config = new Configure();

      config.ServerAddress  = _irTransServerAddress;
      config.ServerPort     = _irTransServerPort;
      config.RemoteModel    = _irTransRemoteModel;

      if (config.ShowDialog() == DialogResult.OK)
      {
        _irTransServerAddress = config.ServerAddress;
        _irTransServerPort    = config.ServerPort;
        _irTransRemoteModel   = config.RemoteModel;

        SaveSettings();
      }
    }
    public bool Start()
    {
      LoadSettings();

      if (Connect(_irTransServerAddress, _irTransServerPort))
      {
        BeginReceive();
        return true;
      }

      return false;
    }
    public void Suspend() { }
    public void Resume() { }
    public void Stop()
    {
      try
      {
        _socket.Close();
      }
      catch (SocketException ex)
      {
        // Nothing to worry about
        Console.WriteLine(ex.ToString());
      }
    }

    public bool Transmit(string file)
    {
      return false;
      /*
      try
      {
        StreamReader streamReader = new StreamReader(file);
        string fileContents = streamReader.ReadToEnd();
        streamReader.Close();

        NETWORKCOMMAND networkCommand = new NETWORKCOMMAND();
        networkCommand.address = 0;
        networkCommand.command = fileContents;
        networkCommand.mode = (byte)'*';
        networkCommand.netcommand = (byte)IrTransCommand.COMMAND_SENDCCFSTR;
        networkCommand.protocol_version = IRTransProtocolVer;
        networkCommand.remote = _irTransRemoteModel;
        networkCommand.timeout = 0;
        networkCommand.trasmit_freq = 0;

        byte[] sendData = StructToByteArray(networkCommand, Marshal.SizeOf(networkCommand));
        _socket.Send(sendData, sendData.Length, SocketFlags.None);

        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        return false;
      }
      */
    }
    public LearnStatus Learn(out byte[] data)
    {
      data = null;

      /*
      try
      {

        NETWORKCOMMAND networkCommand = new NETWORKCOMMAND();
        networkCommand.address = 0;
        networkCommand.command = "";
        networkCommand.mode = (byte)'*';
        networkCommand.netcommand = (byte)IrTransCommand.COMMAND_LRNRAW;
        networkCommand.protocol_version = IRTransProtocolVer;
        networkCommand.remote = _irTransRemoteModel;
        networkCommand.timeout = 0;
        networkCommand.trasmit_freq = 0;

        byte[] sendData = StructToByteArray(networkCommand, Marshal.SizeOf(networkCommand));
        _socket.Send(sendData, sendData.Length, SocketFlags.None);

        return LearnStatus.Success;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        return LearnStatus.Failure;
      }
      */

      /*
      _irData = null;

      BeginLearn(new RemoteEventHandler(LearnIRDone));

      // Wait for the learning to finish ...
      while (_learning && Environment.TickCount < _learnStartTick + timeout)
        Thread.Sleep(100);

      if (Environment.TickCount >= _learnStartTick + timeout)
        return LearnStatus.Timeout;

      if (_irData != null)
      {
        FileStream fileStream = new FileStream(file, FileMode.Create);
        fileStream.Write(_irData, 0, _irData.Length);
        fileStream.Flush();
        fileStream.Close();

        return LearnStatus.Success;
      }
      */
      return LearnStatus.Failure;
    }

    public bool SetPort(string port)
    {
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

        _irTransRemoteModel   = doc.DocumentElement.Attributes["RemoteModel"].Value;
        _irTransServerAddress = doc.DocumentElement.Attributes["IrssUtils.Forms.ServerAddress"].Value;
        _irTransServerPort    = int.Parse(doc.DocumentElement.Attributes["ServerPort"].Value);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());

        _irTransRemoteModel   = DefaultRemoteModel;
        _irTransServerAddress = DefaultServerAddress;
        _irTransServerPort    = DefaultServerPort;
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

        writer.WriteAttributeString("RemoteModel",    _irTransRemoteModel.ToString());
        writer.WriteAttributeString("IrssUtils.Forms.ServerAddress",  _irTransServerAddress.ToString());
        writer.WriteAttributeString("ServerPort",     _irTransServerPort.ToString());

        writer.WriteEndElement(); // </settings>
        writer.WriteEndDocument();
        writer.Close();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }

    static bool Connect(string address, int port)
    {
      try
      {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _socket.Connect(address, port);
        
        // Send Client ID to Server
        byte[] sendData = BitConverter.GetBytes(IRTransClientID);
        _socket.Send(sendData, sendData.Length, SocketFlags.None);
      }
      catch (SocketException ex)
      {
        Console.WriteLine(ex.ToString());
        return false;
      }

      return true;
    }
    static void BeginReceive()
    {
      try
      {
        if (_pfnCallBack == null)
          _pfnCallBack = new AsyncCallback(OnDataReceived);

        CSocketPacket socketPkt = new CSocketPacket();
        socketPkt.ThisSocket = _socket;

        _asynResult = _socket.BeginReceive(socketPkt.ReceiveBuffer, 0, socketPkt.ReceiveBuffer.Length, SocketFlags.None, _pfnCallBack, socketPkt);
      }
      catch (SocketException ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }
    static void OnDataReceived(IAsyncResult asyn)
    {
      try
      {
        CSocketPacket theSockId = (CSocketPacket)asyn.AsyncState;
        
        int bytesReceived = theSockId.ThisSocket.EndReceive(asyn);
        
        IntPtr ptrReceive = Marshal.AllocHGlobal(bytesReceived);
        Marshal.Copy(theSockId.ReceiveBuffer, 0, ptrReceive, bytesReceived);
        NETWORKRECV received = (NETWORKRECV)Marshal.PtrToStructure(ptrReceive, typeof(NETWORKRECV));

        /*
          Log.Info("IRTrans: Command Start --------------------------------------------");
          Log.Info("IRTrans: Client       = {0}", netrecv.clientid);
          Log.Info("IRTrans: Status       = {0}", (IrTransStatus)netrecv.statustype);
          Log.Info("IRTrans: Remote       = {0}", netrecv.remote);
          Log.Info("IRTrans: Command Num. = {0}", netrecv.command_num.ToString());
          Log.Info("IRTrans: Command      = {0}", netrecv.command);
          Log.Info("IRTrans: Data         = {0}", netrecv.data);
          Log.Info("IRTrans: Command End ----------------------------------------------");
        */

        switch ((IrTransStatus)received.StatusType)
        {
          case IrTransStatus.STATUS_RECEIVE:
            if (received.Remote.Trim() == _irTransRemoteModel)
            {
              try
              {
                string keyCode = received.Command.Trim();

                if (_remoteButtonHandler != null)
                  _remoteButtonHandler(keyCode);
              }
              catch (Exception ex)
              {
                Console.WriteLine(ex.ToString());
              }
            }
            break;

          //case IrTransStatus.STATUS_LEARN:

          default:
            break;
        }

        Marshal.FreeHGlobal(ptrReceive);
        BeginReceive();
      }
      catch (ObjectDisposedException)
      {
      }
      catch (SocketException ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }





    static byte[] StructToByteArray(object structure, int size)
    {
      try
      {
        byte[] byteArray = new byte[size];

        IntPtr pointer = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(structure, pointer, false);
        Marshal.Copy(pointer, byteArray, 0, size);
        Marshal.FreeHGlobal(pointer);

        return byteArray;
      }
      catch
      {
        return null;
      }
    }

    #endregion Implementation

  }

}
