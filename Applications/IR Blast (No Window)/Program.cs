using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using IrssComms;
using IrssUtils;

namespace IRBlast
{

  static class Program
  {

    #region Variables

    static Client _client;

    static bool _registered;

    static string _serverHost = "localhost";

    static string _blastPort = "Default";

    static int _delay = 50;

    static bool _treatAsChannelNumber;
    static int _padChannelNumber;

    #endregion Variables

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
#if DEBUG
      IrssLog.LogLevel = IrssLog.Level.Debug;
#else
      IrssLog.LogLevel = IrssLog.Level.Info;
#endif
      IrssLog.Append(Common.FolderIrssLogs + "IR Blast (No Window).log");

      try
      {

        if (args.Length > 0) // Command Line Start ...
        {
          List<String> irCommands = new List<string>();

          for (int index = 0; index < args.Length; index++)
          {
            switch (args[index].ToUpperInvariant())
            {
              case "-HOST":
                _serverHost = args[++index];
                continue;

              case "-PORT":
                _blastPort = args[++index];
                continue;

              case "-DELAY":
                _delay = int.Parse(args[++index]);
                continue;

              case "-CHANNEL":
                _treatAsChannelNumber = true;
                continue;

              case "-PAD":
                _padChannelNumber = int.Parse(args[++index]);
                continue;

              default:
                irCommands.Add(args[index]);
                continue;
            }
          }

          if (irCommands.Count == 0)
          {
            Console.WriteLine("Malformed command line parameters ...");
            Console.WriteLine();

            ShowHelp();
          }
          else
          {
            IPAddress serverIP = Client.GetIPFromName(_serverHost);
            IPEndPoint endPoint = new IPEndPoint(serverIP, IrssComms.Server.DefaultPort);

            if (StartClient(endPoint))
            {
              Thread.Sleep(250);

              // Wait for registered ... Give up after 10 seconds ...
              int attempt = 0;
              while (!_registered)
              {
                if (++attempt >= 10)
                  break;
                else
                  Thread.Sleep(1000);
              }

              if (_registered)
              {
                string fileName;
                foreach (String command in irCommands)
                {
                  if (_treatAsChannelNumber)
                  {
                    IrssLog.Info("Processing channel: {0}", command);

                    StringBuilder channelNumber = new StringBuilder(command);

                    if (_padChannelNumber > 0)
                    {
                      for (int index = 0; index < _padChannelNumber - command.Length; index++)
                        channelNumber.Insert(0, '0');

                      IrssLog.Info("Padding channel number: {0} becomes {1}", command, channelNumber.ToString());
                    }

                    foreach (char digit in channelNumber.ToString())
                    {
                      if (digit.Equals('~'))
                      {
                        Thread.Sleep(500);
                      }
                      else
                      {
                        fileName = Common.FolderIRCommands + digit + Common.FileExtensionIR;
                        BlastIR(fileName, _blastPort);
                      }
                      
                      if (_delay > 0)
                        Thread.Sleep(_delay);
                    }
                  }
                  else if (command.StartsWith("~", StringComparison.OrdinalIgnoreCase))
                  {
                    Thread.Sleep(command.Length * 500);
                  }
                  else
                  {
                    fileName = Common.FolderIRCommands + command;
                    BlastIR(fileName, _blastPort);
                  }

                  if (_delay > 0)
                    Thread.Sleep(_delay);
                }

                Thread.Sleep(500);
              }
              else
              {
                IrssLog.Warn("Failed to register with server host \"{0}\", blasting not sent", _serverHost);
              }
            }
          }
        }
        else // Give help ...
        {
          ShowHelp();
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }

      StopClient();

      IrssLog.Close();
    }

    static void ShowHelp()
    {
      IrssLog.Debug("Show Help");

      MessageBox.Show(
@"IR Blast (No Window) is a windowless version on IR Blast.
Refer to IR Blast help for more information.",
"IR Blast (No Window)", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    static void CommsFailure(object obj)
    {
      Exception ex = obj as Exception;
      
      if (ex != null)
        IrssLog.Error("Communications failure: {0}", ex.Message);
      else
        IrssLog.Error("Communications failure");

      StopClient();
    }
    static void Connected(object obj)
    {
      IrssLog.Info("Connected to server");

      IrssMessage message = new IrssMessage(MessageType.RegisterClient, MessageFlags.Request);
      _client.Send(message);
    }
    static void Disconnected(object obj)
    {
      IrssLog.Warn("Communications with server has been lost");

      Thread.Sleep(1000);
    }

    static bool StartClient(IPEndPoint endPoint)
    {
      if (_client != null)
        return false;

      ClientMessageSink sink = new ClientMessageSink(ReceivedMessage);

      _client = new Client(endPoint, sink);
      _client.CommsFailureCallback  = new WaitCallback(CommsFailure);
      _client.ConnectCallback       = new WaitCallback(Connected);
      _client.DisconnectCallback    = new WaitCallback(Disconnected);
      
      if (_client.Start())
      {
        return true;
      }
      else
      {
        _client = null;
        return false;
      }
    }
    static void StopClient()
    {
      if (_client == null)
        return;

      _client.Dispose();
      _client = null;
    }

    static void ReceivedMessage(IrssMessage received)
    {
      IrssLog.Debug("Received Message \"{0}\"", received.Type);

      try
      {
        switch (received.Type)
        {
          case MessageType.BlastIR:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
              IrssLog.Info("Blast Success");
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
              IrssLog.Warn("Blast Failed!");
            break;

          case MessageType.RegisterClient:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              IrssLog.Info("Registered to Input Service");
              _registered = true;
              //_irServerInfo = TransceiverInfo.FromString(received.Data);
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              _registered = false;
              IrssLog.Warn("Input Service refused to register");
            }
            break;

          case MessageType.ServerShutdown:
            _registered = false;
            IrssLog.Warn("Input Service Shutdown - Blasting disabled until Input Service returns");
            break;

          case MessageType.Error:
            IrssLog.Warn(received.GetDataAsString());
            break;
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
    }

    static void BlastIR(string fileName, string port)
    {
      using (FileStream file = File.OpenRead(fileName))
      {
        if (file.Length == 0)
          throw new IOException(String.Format("Cannot Blast. IR file \"{0}\" has no data, possible IR learn failure", fileName));

        byte[] outData = new byte[4 + port.Length + file.Length];

        BitConverter.GetBytes(port.Length).CopyTo(outData, 0);
        Encoding.ASCII.GetBytes(port).CopyTo(outData, 4);

        file.Read(outData, 4 + port.Length, (int)file.Length);

        IrssMessage message = new IrssMessage(MessageType.BlastIR, MessageFlags.Request | MessageFlags.ForceNotRespond, outData);
        _client.Send(message);
      }
    }

  }

}
