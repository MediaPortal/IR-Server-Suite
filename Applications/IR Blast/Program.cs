using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;

using NamedPipes;
using IrssUtils;

namespace IRBlast
{

  static class Program
  {

    #region Variables

    static bool _registered = false;
    static bool _keepAlive = true;
    static int _echoID = -1;
    static Thread _keepAliveThread;

    static string _serverHost = null;
    static string _localPipeName;

    static string _blastPort = "None";

    static bool _treatAsChannelNumber = false;
    static int _padChannelNumber = 0;

    #endregion Variables

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
      // TODO: Change log level to info for release.
      IrssLog.LogLevel = IrssLog.Level.Debug;
      IrssLog.Append(Common.FolderIrssLogs + "IR Blast.log");

      ShowHeader();

      try
      {

        if (args.Length > 0) // Command Line Start ...
        {
          List<String> irCommands = new List<string>();

          for (int index = 0; index < args.Length; index++)
          {
            switch (args[index].ToLowerInvariant())
            {
              case "-host":
                _serverHost = args[++index];
                continue;

              case "-port":
                _blastPort = args[++index];
                continue;

              case "-channel":
                _treatAsChannelNumber = true;
                continue;

              case "-pad":
                int.TryParse(args[++index], out _padChannelNumber);
                continue;

              default:
                irCommands.Add(args[index]);
                continue;
            }
          }

          if (String.IsNullOrEmpty(_serverHost) || irCommands.Count == 0)
          {
            Console.WriteLine("Malformed command line parameters ...");
            Console.WriteLine();

            ShowHelp();
          }
          else if (StartComms())
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
                  }
                }
                else if (command.StartsWith("~"))
                {
                  Thread.Sleep(command.Length * 500);
                }
                else
                {
                  fileName = Common.FolderIRCommands + command;
                  BlastIR(fileName, _blastPort);
                }
              }

              Thread.Sleep(500);
            }
            else
            {
              Warn("Failed to register with server host \"{0}\", blasting not sent", _serverHost);
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

      StopComms();

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

      Console.WriteLine("IRBlast -host <server> [-port x] [-speed x] [-pad x] [-channel] <commands>");
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
      Console.WriteLine("IRBlast -host HTPC -pad 4 -channel 302");
      Console.WriteLine("");
      Console.WriteLine("This would blast the 0.IR (for padding), 3.IR, 0.IR, and 2.IR commands on");
      Console.WriteLine("the HTPC computer to the default blaster port at the default blaster speed.");
      Console.WriteLine("");
      Console.WriteLine("");
    }

    static bool StartComms()
    {
      try
      {
        if (OpenLocalPipe())
        {
          _keepAliveThread = new Thread(new ThreadStart(KeepAliveThread));
          _keepAliveThread.Start();
          return true;
        }
      }
      catch (Exception ex)
      {
        Error(ex);
      }

      return false;
    }
    static void StopComms()
    {
      _keepAlive = false;

      try
      {
        if (_keepAliveThread != null && _keepAliveThread.IsAlive)
          _keepAliveThread.Abort();
      }
      catch { }

      try
      {
        if (_registered)
        {
          _registered = false;

          PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Unregister", null);
          PipeAccess.SendMessage(Common.ServerPipeName, _serverHost, message.ToString());
        }
      }
      catch { }

      try
      {
        if (PipeAccess.ServerRunning)
          PipeAccess.StopServer();
      }
      catch { }
    }

    static bool OpenLocalPipe()
    {
      try
      {
        int pipeNumber = 1;
        bool retry = false;

        do
        {
          string localPipeTest = String.Format(Common.LocalPipeFormat, pipeNumber);

          if (PipeAccess.PipeExists(String.Format("\\\\.\\pipe\\{0}", localPipeTest)))
          {
            if (++pipeNumber <= Common.MaximumLocalClientCount)
              retry = true;
            else
              throw new Exception(String.Format("Maximum local client limit ({0}) reached", Common.MaximumLocalClientCount));
          }
          else
          {
            if (!PipeAccess.StartServer(localPipeTest, new PipeMessageHandler(ReceivedMessage)))
              throw new Exception(String.Format("Failed to start local pipe server \"{0}\"", localPipeTest));

            _localPipeName = localPipeTest;
            retry = false;
          }
        }
        while (retry);

        return true;
      }
      catch (Exception ex)
      {
        Error(ex);
        return false;
      }
    }

    static bool ConnectToServer()
    {
      try
      {
        PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Register", null);
        PipeAccess.SendMessage(Common.ServerPipeName, _serverHost, message.ToString());
        return true;
      }
      catch (AppModule.NamedPipes.NamedPipeIOException)
      {
        return false;
      }
      catch (Exception ex)
      {
        Error(ex);
        return false;
      }
    }

    static void KeepAliveThread()
    {
      Random random = new Random((int)DateTime.Now.Ticks);
      bool reconnect;
      int attempt;

      _registered = false;
      _keepAlive = true;
      while (_keepAlive)
      {
        reconnect = true;

        #region Connect to server

        Info("Connecting ({0}) ...", _serverHost);
        attempt = 0;
        while (_keepAlive && reconnect)
        {
          if (ConnectToServer())
          {
            reconnect = false;
          }
          else
          {
            int wait;

            if (attempt <= 50)
              attempt++;

            if (attempt > 50)
              wait = 30;      // 30 seconds
            else if (attempt > 20)
              wait = 10;      // 10 seconds
            else if (attempt > 10)
              wait = 5;       // 5 seconds
            else
              wait = 1;       // 1 second

            for (int sleeps = 0; sleeps < wait && _keepAlive; sleeps++)
              Thread.Sleep(1000);
          }
        }

        #endregion Connect to server

        #region Wait for registered

        // Give up after 10 seconds ...
        attempt = 0;
        while (_keepAlive && !_registered && !reconnect)
        {
          if (++attempt >= 10)
            reconnect = true;
          else
            Thread.Sleep(1000);
        }

        #endregion Wait for registered

        #region Ping the server repeatedly

        while (_keepAlive && _registered && !reconnect)
        {
          int pingID = random.Next();
          long pingTime = DateTime.Now.Ticks;

          try
          {
            PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Ping", BitConverter.GetBytes(pingID));
            PipeAccess.SendMessage(Common.ServerPipeName, _serverHost, message.ToString());
          }
          catch
          {
            // Failed to ping ... reconnect ...
            Warn("Failed to ping, attempting to reconnect ...");
            _registered = false;
            reconnect = true;
            break;
          }

          // Wait 10 seconds for a ping echo ...
          bool receivedEcho = false;
          while (_keepAlive && _registered && !reconnect &&
            !receivedEcho && DateTime.Now.Ticks - pingTime < 10 * 1000 * 10000)
          {
            if (_echoID == pingID)
            {
              receivedEcho = true;
            }
            else
            {
              Thread.Sleep(1000);
            }
          }

          if (receivedEcho) // Received ping echo ...
          {
            // Wait 60 seconds before re-pinging ...
            for (int sleeps = 0; sleeps < 60 && _keepAlive && _registered; sleeps++)
              Thread.Sleep(1000);
          }
          else // Didn't receive ping echo ...
          {
            Warn("No echo to ping, attempting to reconnect ...");

            // Break out of pinging cycle ...
            _registered = false;
            reconnect = true;
          }
        }

        #endregion Ping the server repeatedly

      }

    }

    static void ReceivedMessage(string message)
    {
      PipeMessage received = PipeMessage.FromString(message);

      IrssLog.Debug("Received Message \"{0}\"", received.Name);

      try
      {
        switch (received.Name)
        {
          case "Remote Event":
          case "Keyboard Event":
          case "Mouse Event":
            break;

          case "Blast Success":
            Info("Blast Success");
            break;

          case "Blast Failure":
            Warn("Blast Failed!");
            break;

          case "Register Success":
            {
              Info("Registered to IR Server");
              _registered = true;
              //_transceiverInfo = TransceiverInfo.FromBytes(received.Data);
              break;
            }

          case "Register Failure":
            {
              Warn("IR Server refused to register");
              _registered = false;
              break;
            }

          case "Server Shutdown":
            {
              Warn("IR Server Shutdown - Blasting disabled until IR Server returns");
              _registered = false;
              break;
            }

          case "Echo":
            {
              _echoID = BitConverter.ToInt32(received.Data, 0);
              break;
            }

          case "Error":
            {
              Warn(Encoding.ASCII.GetString(received.Data));
              break;
            }

          default:
            {
              Warn("Unknown message received from server: " + received.Name);
              break;
            }
        }
      }
      catch (Exception ex)
      {
        Error(ex);
      }
    }

    static void BlastIR(string fileName, string port)
    {
      FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

      byte[] outData = new byte[4 + port.Length + file.Length];

      BitConverter.GetBytes(port.Length).CopyTo(outData, 0);
      Encoding.ASCII.GetBytes(port).CopyTo(outData, 4);

      file.Read(outData, 4 + port.Length, (int)file.Length);
      file.Close();

      PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Blast", outData);
      PipeAccess.SendMessage(Common.ServerPipeName, _serverHost, message.ToString());
    }

    #region Log Commands

    static void Info(string format, params object[] args)
    {
      string message = String.Format(format, args);
      Console.WriteLine(message);
      IrssLog.Info(message);
    }
    static void Warn(string format, params object[] args)
    {
      string message = String.Format(format, args);
      Console.WriteLine(message);
      IrssLog.Warn(message);
    }
    static void Error(Exception ex)
    {
      Console.WriteLine(ex.Message);
      IrssLog.Error(ex.ToString());
    }

    #endregion Log Commands

  }

}
