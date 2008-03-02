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
      IrssLog.Append(Common.FolderIrssLogs + "IR Blast.log");

      ShowHeader();

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
                    Info("Processing channel: {0}", command);

                    StringBuilder channelNumber = new StringBuilder(command);

                    if (_padChannelNumber > 0)
                    {
                      for (int index = 0; index < _padChannelNumber - command.Length; index++)
                        channelNumber.Insert(0, '0');

                      Info("Padding channel number: {0} becomes {1}", command, channelNumber.ToString());
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
                Warn("Failed to register with server host \"{0}\", blasting not sent", _serverHost);
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
        Error(ex);
      }

      StopClient();

      IrssLog.Close();
    }

    static void ShowHeader()
    {
      Console.WriteLine("");
      Console.WriteLine("IR Blast");
      Console.WriteLine("-----------------------------------------------------------------------------");
      Console.WriteLine("Command line IR blaster for IR Server Suite.");
      Console.WriteLine("By and-81.");
      Console.WriteLine("");
      Console.WriteLine("");
    }

    static void ShowHelp()
    {
      IrssLog.Debug("Show Help");

      Console.WriteLine("IRBlast [-host x] [-port x] [-speed x] [-pad x] [-channel] <commands>");
      Console.WriteLine("");
      Console.WriteLine("Use -host to specify the computer that is hosting the IR Server.");
      Console.WriteLine("Use -port to blast to a particular blaster port (Optional).");
      Console.WriteLine("Use -speed to set the blaster speed (Optional).");
      Console.WriteLine("Use -channel to tell IR Blast to break apart the following IR Command and");
      Console.WriteLine(" use each digit for a separate IR blast (Optional).");
      Console.WriteLine("Use -pad to tell IR Blast to pad channel numbers to a certain length");
      Console.WriteLine(" (Optional, Requires -channel).");
      Console.WriteLine("Use a tilde ~ between commands to insert half second pauses.");
      Console.WriteLine("");
      Console.WriteLine("");
      Console.WriteLine("Examples:");
      Console.WriteLine("");
      Console.WriteLine("IRBlast -host HTPC TV_Power.IR");
      Console.WriteLine("");
      Console.WriteLine("This would blast the TV_Power.IR command on the HTPC computer to the default");
      Console.WriteLine("blaster port at the default blaster speed.");
      Console.WriteLine("");
      Console.WriteLine("IRBlast -host MEDIAPC -port Port_2 \"Turn on surround.IR\"");
      Console.WriteLine("");
      Console.WriteLine("This would blast the \"Turn on surround.IR\" command on the MEDIAPC computer");
      Console.WriteLine("to blaster port 2 at the default blaster speed.");
      Console.WriteLine("");
      Console.WriteLine("IRBlast -pad 4 -channel 302");
      Console.WriteLine("");
      Console.WriteLine("This would blast the 0.IR (for padding), 3.IR, 0.IR, and 2.IR commands on");
      Console.WriteLine("the local computer to the default blaster port at the default blaster speed.");
      Console.WriteLine("");
      Console.WriteLine("");
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

      _registered = false;
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
              Info("Blast Success");
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
              Warn("Blast Failed!");
            break;

          case MessageType.RegisterClient:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              Info("Registered to Input Service");
              _registered = true;
              //_irServerInfo = TransceiverInfo.FromString(received.Data);
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              _registered = false;
              Warn("Input Service refused to register");
            }
            break;

          case MessageType.ServerShutdown:
            _registered = false;
            Warn("Input Service Shutdown - Blasting disabled until Input Service returns");
            break;

          case MessageType.Error:
            Warn(received.GetDataAsString());
            break;
        }
      }
      catch (Exception ex)
      {
        Error(ex);
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

    #region Log Commands

    static void Info(string format, params object[] args)
    {
      Console.WriteLine(format, args);
      IrssLog.Info(format, args);
    }
    static void Warn(string format, params object[] args)
    {
      Console.WriteLine(format, args);
      IrssLog.Warn(format, args);
    }
    static void Error(Exception ex)
    {
      Console.WriteLine(ex.Message);
      IrssLog.Error(ex);
    }

    #endregion Log Commands

  }

}
