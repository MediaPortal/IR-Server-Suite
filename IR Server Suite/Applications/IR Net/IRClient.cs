#region Copyright (C) 2005-2015 Team MediaPortal

// Copyright (C) 2005-2015 Team MediaPortal
// http://www.team-mediaportal.com
// 
// This Program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2, or (at your option)
// any later version.
// 
// This Program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with GNU Make; see the file COPYING.  If not, write to
// the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
// http://www.gnu.org/copyleft/gpl.html

#endregion


using System;
using System.Linq;
using System.Text;
using System.IO;
using IrssComms;
using IrssUtils;
using System.Net;
using System.Threading;

namespace IRNet
{
    /// <summary>
    /// Create and control a .NET IRSS client from any .NET compatible script or software.
    /// This class is especially designed to provide an easy access of the IRSS possibilities in Windows PowerShell.
    /// Authors: Sebastien Mouy (alias Starwer)
    /// </summary>
    public class IRClient
    {

        //------------------------------------------------------------------------------------------------------------------
        #region Attributes

        /// <summary>
        /// Direct access to the IRSS Client class for advanced functions.
        /// </summary>
        public Client irss;

        /// <summary>
        /// Name of the computer from which the IR Server is run
        /// </summary>
        public string ServerHost { get; private set; }

        /// <summary>
        /// Indicate whether the client is actually connected to an IR Server
        /// </summary>
        public bool Connected { get; private set; }

        /// <summary>
        /// Contains information about the IRSS server, i.e. what features this server supports
        /// </summary>
        public IRServerInfo ServerInfo { get; private set; }


        private string _irFile = "";
        private string _logfile = "";
        private AutoResetEvent _connectedEvent;
        private AutoResetEvent _learntEvent;
        private bool _learnSuccess = false;

        #endregion Attributes

        //------------------------------------------------------------------------------------------------------------------
        #region ctors

        /// <summary>
        /// Just create a .NET instance of an IRSS client. The Connect() method must be used to connect the client to an IRSS server.
        /// </summary>
        public IRClient()
        {
            ServerHost = "";
            Connected = false;
            ServerInfo = new IRServerInfo();
            _connectedEvent = new AutoResetEvent(false);
            _learntEvent = new AutoResetEvent(false);
        }


        /// <summary>
        /// Destroy the IRSS client instance.
        /// </summary>
        public void Dispose()
        {
            if (Connected)
            {
                IrssMessage message = new IrssMessage(MessageType.UnregisterClient, MessageFlags.Request);
                irss.Send(message);
                Connected = false;
            }

            irss.Stop();
            irss.Dispose();
        }

        #endregion ctors

        //------------------------------------------------------------------------------------------------------------------
        #region methods

        /// <summary>
        /// Connect the IRSS client to an IRSS server.
        /// </summary>
        /// <param name="host">Computer running the IRSS server to which the client should connect</param>
        /// <param name="millisecondsTimeout">Wait for the connection to succeed for that many milliseconds</param>
        /// <returns>true if the connection is successful</returns>
        public bool Connect(string host = "", int millisecondsTimeout = 10000)
        {
            // If connected, first disconnect
            if (Connected)
            {
                if (ServerHost == host)
                {
                    LogWarn("Already connected to " + ServerHost);
                    return true;
                }
                else
                {
                    Disconnect();
                }
            }

            // reset connection states
            Connected = false;
            if (host == "") host = ServerHost;
            if (host == "") host = "localhost";


            IPAddress serverIP = Network.GetIPFromName(host);
            IPEndPoint endPoint = new IPEndPoint(serverIP, Server.DefaultPort);

            ClientMessageSink sink = ReceivedMessage;

            irss = new Client(endPoint, sink);
            if (irss == null)
            {
                LogErr("Failed to open connection to " + host);
                return false;
            }
            irss.CommsFailureCallback = CommsFailure;
            irss.ConnectCallback = SocketConnected;
            irss.DisconnectCallback = SocketDisconnected;

            bool ok = irss.Start();

            if (ok)
            {
                ServerHost = host;

                // Wait for the connection to occur.
                if (!_connectedEvent.WaitOne(millisecondsTimeout))
                {
                    LogErr("Failed to connect to " + ServerHost);
                    return false;
                }

                LogInfo("Client connected to " + host);
                Connected = true;
            }

            return ok;
        }

        /// <summary>
        /// Connect the IRSS client to any IRSS server.
        /// </summary>
        /// <returns>true if the disconnection is successful</returns>
        public bool Disconnect()
        {

            try
            {
                if (!Connected || irss == null)
                {
                    LogWarn("Not connected");
                    return true;
                }

                LogInfo("Disconnect " + ServerHost);
                IrssMessage message = new IrssMessage(MessageType.UnregisterClient, MessageFlags.Request);
                irss.Send(message);
                Connected = false;
                ServerHost = "";

                irss.Dispose();
                irss = null;

            }
            catch (Exception ex)
            {
                LogErr(ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Lean in IR code and save it to the given irName. 
        /// After this method is called, an IR event is expect unless the time-out is reached before that (typically: 10 sec).
        /// </summary>
        /// <param name="irName">This name will be used to create a file, so make sure the name supplied doesn't break any of the windows file name conventions. That is, don't use / ? \ : * | "</param>
        /// <param name="millisecondsTimeout">Wait for the learn-event response from server to succeed for that many milliseconds</param>
        public bool Learn(string irName = "", int millisecondsTimeout = 15000)
        {
            _learnSuccess = false;
            if (irss == null)
            {
                LogErr("Not connected");
                return _learnSuccess;
            }

            if (!irss.Connected)
            {
                LogWarn("...Connecting...");
                return _learnSuccess;
            }

            if (!ServerInfo.CanLearn)
            {
                LogErr("IR Server is not setup to support learning");
                return _learnSuccess;
            }

            try
            {
                // make filename
                _irFile = irName;
                if (_irFile == null) _irFile = "";
                if (_irFile != "" && !_irFile.ToUpper().EndsWith(Common.FileExtensionIR))
                    _irFile += Common.FileExtensionIR;

                IrssMessage message = new IrssMessage(MessageType.LearnIR, MessageFlags.Request);
                irss.Send(message);

                System.Console.WriteLine("Hold your remote close to the receiver and press the button to learn");
                LogInfo("Learning...");

                _learntEvent.WaitOne(millisecondsTimeout);

                if (_learnSuccess)
                {
                    LogInfo("Learning successful: " + _irFile);
                }
                else
                {
                    LogErr("Learning failed");
                }
            }
            catch (Exception ex)
            {
                LogErr(ex.Message);
            }
            return _learnSuccess;
        }

        /// <summary>
        /// Send an IR command which has been learnt by the Learn() method (or other learning component from IRSS, like translator).
        /// The command to send is defined by a file-name.
        /// </summary>
        /// <param name="irName">Name of the IR file</param>
        /// <param name="port">blaster-port on the tranceiver: must be "Both" (default), "Port_1" (or "1"), "Port_2" (or "2")</param>
        /// <returns>true if the blast command is successfully sent to the IRSS server</returns>
        public bool Blast(string irName, string port = "Both")
        {

            if (irss == null)
            {
                LogErr("Not connected");
                return false;
            }

            if (!irss.Connected)
            {
                LogWarn("Connecting...");
                return false;
            }

            if (!ServerInfo.CanTransmit)
            {
                LogErr("IR Server is not setup to blast");
                return false;
            }

            // make filename
            if (irName == null || irName == "") return false;
            if (!irName.ToUpper().EndsWith(Common.FileExtensionIR)) irName += Common.FileExtensionIR;

            // Find port
            if (!ServerInfo.Ports.Contains(port))
            {
                LogErr("Invalid port: supported Blast-ports for this server are: " + String.Join(" ", ServerInfo.Ports));
                return false;
            }


            try
            {
                string irFile = Path.Combine(Common.FolderIRCommands, irName);
                using (FileStream file = File.OpenRead(irFile))
                {
                    if (file.Length == 0)
                        throw new IOException(String.Format("Cannot Blast. IR file \"{0}\" has no data, possible IR learn failure",
                                                            irName));

                    byte[] outData = new byte[4 + port.Length + file.Length];

                    BitConverter.GetBytes(port.Length).CopyTo(outData, 0);
                    Encoding.ASCII.GetBytes(port).CopyTo(outData, 4);

                    file.Read(outData, 4 + port.Length, (int)file.Length);

                    IrssMessage message = new IrssMessage(MessageType.BlastIR, MessageFlags.Request, outData);
                    LogDebug("Blast " + irName + " to port: " + port);
                    irss.Send(message);
                }
            }
            catch (Exception ex)
            {
                LogErr(ex.Message);
                return false;
            }

            return true;
        }

        #endregion methods

        //------------------------------------------------------------------------------------------------------------------
        #region callbacks

        /// <summary>
        /// Socket-communication callback to receive message from the IRSS server.
        /// </summary>
        /// <param name="received">received message, defined mostly by a Type and Flags</param>
        private void ReceivedMessage(IrssMessage received)
        {
            // default (most common) values
            RemoteEventArgs args = new RemoteEventArgs();
            args.Sender = RemoteEventArgs.IrServer;
            args.Key = received.Type.ToString();
            args.Data = received.Flags.ToString() + ": " + received.GetDataAsString();

            try
            {

                switch (received.Type)
                {
                    case MessageType.RegisterClient:
                        if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
                        {
                            Connected = true;
                            ServerInfo = IRServerInfo.FromBytes(received.GetDataAsBytes());
                            irss.Send(new IrssMessage(MessageType.ActiveReceivers, MessageFlags.Request));
                            irss.Send(new IrssMessage(MessageType.ActiveBlasters, MessageFlags.Request));
                            _connectedEvent.Set();
                        }
                        else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
                        {
                            Connected = false;
                        }
                        break;

                    case MessageType.ActiveBlasters:
                        LogInfo(received.GetDataAsString());
                        break;

                    case MessageType.ActiveReceivers:
                        LogInfo(received.GetDataAsString());
                        break;

                    case MessageType.RemoteEvent:
                        // raise the event
                        args.Sender = received.MessageData[IrssMessage.DEVICE_NAME] as string;
                        args.Key = received.MessageData[IrssMessage.KEY_CODE] as string;
                        args.Data = "";
                        break;

                    case MessageType.LearnIR:
                        _learnSuccess = (received.Flags & MessageFlags.Success) == MessageFlags.Success;
                        if (_learnSuccess)
                        {
                            byte[] dataBytes = received.GetDataAsBytes();
                            if (_irFile != null && _irFile != "")
                            {

                                string DebugIRFile = Path.Combine(Common.FolderIRCommands, _irFile);
                                using (FileStream file = File.Create(DebugIRFile))
                                    file.Write(dataBytes, 0, dataBytes.Length);
                            }
                        }
                        _learntEvent.Set();

                        break;

                    case MessageType.ServerShutdown:
                        Connected = false;
                        LogInfo("Disconnected by host " + ServerHost);
                        LogInfo(received.GetDataAsString());
                        break;

                    case MessageType.Error:
                        LogErr(received.GetDataAsString());
                        break;
                }
            }
            catch (Exception ex)
            {
                LogErr(ex.Message);

                args.Key = "Error";
                args.Data = "ex.Message";

            }

            LogDebug(String.Format("rise RemoteEvent:  Sender: {0} - Key: {1} - Data: {2}", args.Sender, args.Key, args.Data));
            OnRemoteEvent(args);

        }

        /// <summary>
        /// Event raised when the client receive an message from the IRSS server.
        /// This event can represent any of the input from the enabled receivers on the server.
        /// It the Sender attribute not "IR Server", it represents an event from a plugin input device.
        /// </summary>
        public event EventHandler<RemoteEventArgs> RemoteEvent;
        private void OnRemoteEvent(RemoteEventArgs e)
        {
            EventHandler<RemoteEventArgs> handler = RemoteEvent;
            if (handler != null)
            {
                handler(this, e);
            }

        }


        /// <summary>
        /// Exception: communication failure
        /// </summary>
        /// <param name="obj"></param>
        private void CommsFailure(object obj)
        {
            Exception ex = obj as Exception;

            if (ex != null)
                LogInfo(String.Format("Communications failure: {0}", ex.Message));
            else
                LogErr("Communications failure");

            Disconnect();
        }

        /// <summary>
        /// Called back when the client is successfully connected to the  server.
        /// </summary>
        private void SocketConnected(object obj)
        {
            LogInfo("Connection to server " + ServerHost + " successful");

            IrssMessage message = new IrssMessage(MessageType.RegisterClient, MessageFlags.Request);
            irss.Send(message);
        }

        /// <summary>
        /// Detected a disconnection from the server. 
        /// </summary>
        private void SocketDisconnected(object obj)
        {
            // TODO: never called
            LogWarn("Communications with server has been lost");
            Connected = false;
        }

        #endregion callbacks

        //------------------------------------------------------------------------------------------------------------------
        #region reporting

        /// <summary>
        /// Setup the log name and verbosity level. 
        /// By default, IRPSClient does not create a log-file unless this method is called.  
        /// </summary>
        /// <param name="filename">filename (with extension) of the log-file (e.g. "IRPSClient.log"). 
        /// The log file will be created in the IRSS Log directory.
        /// Default: "": do not log.
        /// </param>
        /// <param name="level"> Define the verbosity level of the log-file (by increasing verbosity):
        /// <list type="ul">
        /// <item>Off: do not log anything</item>
        /// <item>Error: log only Error messages</item>
        /// <item>Warn: Log only Warning and Error messages</item>
        /// <item>Info: Log only Warning, Error and Information messages</item>
        /// <item>Debug: Log all messages</item>
        /// </list>
        /// </param>
        public void LogSetup(string filename = "", string level = "Debug")
        {

            IrssLog.Level elevel = (IrssLog.Level)Enum.Parse(typeof(IrssLog.Level), level);
            LogInfo("log verbosity level: " + level);

            // Disable logging
            if (filename == null || filename == "")
            {
                if (_logfile != "")
                {
                    LogInfo("logging disabled: " + _logfile);
                    IrssLog.Close();
                    _logfile = "";
                }
                return;
            }

            // close previously open logfile
            if (_logfile != "" && filename != _logfile)
            {
                IrssLog.Close();
                _logfile = "";
            }


            // open logfile
            IrssLog.LogLevel = elevel;
            if (_logfile == "") IrssLog.Open(filename);
            LogInfo("log to file: " + filename);
            _logfile = filename;
        }

        /// <summary>
        /// Generate a message (logged if enabled) of level: Info.
        /// </summary>
        /// <param name="str">message to be displayed</param>
        public void LogInfo(string str)
        {
            if (_logfile != "")
            {
                IrssLog.Info(str);
            }

            Console.WriteLine("+ " + str);
        }

        /// <summary>
        /// Generate a message (logged if enabled) of level: Debug.
        /// </summary>
        /// <param name="str">message to be displayed</param>
        public void LogDebug(string str)
        {
            if (_logfile != "")
            {
                IrssLog.Debug(str);
            }

            Console.WriteLine("- " + str);
        }

        /// <summary>
        /// Generate a message (logged if enabled) of level: Warn.
        /// </summary>
        /// <param name="str">message to be displayed</param>
        public void LogWarn(string str)
        {
            if (_logfile != "")
            {
                IrssLog.Error(str);
            }

            Console.WriteLine("! " + str);
        }

        /// <summary>
        /// Generate a message (logged if enabled) of level: Error.
        /// </summary>
        /// <param name="str">message to be displayed</param>
        public void LogErr(string str)
        {
            if (_logfile != "")
            {
                IrssLog.Error(str);
            }

            Console.WriteLine("# error: " + str);
        }

        #endregion reporting


    }
}
