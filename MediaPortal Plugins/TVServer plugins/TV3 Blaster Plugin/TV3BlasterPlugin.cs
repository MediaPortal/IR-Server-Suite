#region Copyright (C) 2005-2009 Team MediaPortal

// Copyright (C) 2005-2009 Team MediaPortal
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
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;
using IrssComms;
using IrssUtils;
using MPUtils;
using SetupTv;
using TvControl;
using TvDatabase;
using TvEngine.Events;
using TvEngine.Plugins.IRSS.TV3BlasterPlugin.Forms;
using TvLibrary.Implementations;
using TvLibrary.Interfaces;
using TvLibrary.Log;
using Server = IrssComms.Server;

namespace TvEngine.Plugins.IRSS.TV3BlasterPlugin
{
  /// <summary>
  /// MediaPortal TV3 Blaster Plugin for IR Server.
  /// </summary>
  public class TV3BlasterPlugin : ITvServerPlugin
  {
    #region Constants

    /// <summary>
    /// The plugin version string.
    /// </summary>
    internal const string PluginVersion = "TV3 Blaster Plugin 1.4.2.0 for TV Server";

    private const string ProcessCommandThreadName = "ProcessCommand";
    internal static readonly string ExtCfgFolder = Path.Combine(Common.FolderAppData, "TV3 Blaster Plugin");
    internal static readonly string FolderMacros = Path.Combine(Common.FolderAppData, "TV3 Blaster Plugin\\Macro");

    #endregion Constants

    #region Variables

    private static Client _client;

    private static ExternalChannelConfig[] _externalChannelConfigs;

    private static ClientMessageSink _handleMessage;

    private static bool _inConfiguration;

    private static IRServerInfo _irServerInfo = new IRServerInfo();
    private static string _learnIRFilename;
    private static bool _registered;
    private static string _serverHost;

    #endregion Variables

    #region Properties

    internal static string ServerHost
    {
      get { return _serverHost; }
      set { _serverHost = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether in configuration.
    /// </summary>
    /// <value><c>true</c> if in configuration; otherwise, <c>false</c>.</value>
    internal static bool InConfiguration
    {
      get { return _inConfiguration; }
      set { _inConfiguration = value; }
    }

    internal static ClientMessageSink HandleMessage
    {
      get { return _handleMessage; }
      set { _handleMessage = value; }
    }

    internal static IRServerInfo TransceiverInformation
    {
      get { return _irServerInfo; }
    }

    /// <summary>
    /// Returns the name of the plugin.
    /// </summary>
    public string Name
    {
      get { return "TV3 Blaster Plugin for IR Server"; }
    }

    /// <summary>
    /// Returns the version of the plugin.
    /// </summary>
    public string Version
    {
      get { return "1.4.2.0"; }
    }

    /// <summary>
    /// Returns the author of the plugin.
    /// </summary>
    public string Author
    {
      get { return "and-81"; }
    }

    /// <summary>
    /// Returns if the plugin should only run on the master server or also on slave servers.
    /// </summary>
    public bool MasterOnly
    {
      get { return false; }
    }

    #endregion Properties

    #region ITvServerPlugin methods

    /// <summary>
    /// Starts this instance.
    /// </summary>
    [CLSCompliant(false)]
    public void Start(IController controller)
    {
      _inConfiguration = false;

      Log.Info("TV3BlasterPlugin: Starting ({0})", PluginVersion);

      // Load basic settings
      LoadSettings();

      LoadExternalConfigs();

      ITvServerEvent events = GlobalServiceProvider.Instance.Get<ITvServerEvent>();
      events.OnTvServerEvent += events_OnTvServerEvent;

      IPAddress serverIP = Network.GetIPFromName(_serverHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, Server.DefaultPort);

      if (!StartClient(endPoint))
        Log.Error("TV3BlasterPlugin: Failed to start local comms, IR blasting is disabled for this session");

      Log.Debug("TV3BlasterPlugin: Started");
    }

    /// <summary>
    /// Stops this instance.
    /// </summary>
    public void Stop()
    {
      ITvServerEvent events = GlobalServiceProvider.Instance.Get<ITvServerEvent>();
      events.OnTvServerEvent -= events_OnTvServerEvent;

      StopClient();

      Log.Debug("TV3BlasterPlugin: Stopped");
    }

    /// <summary>
    /// Gets the setup form control.
    /// </summary>
    /// <value>The setup form control.</value>
    [CLSCompliant(false)]
    public SectionSettings Setup
    {
      get { return new PluginSetup(); }
    }

    #endregion ITvServerPlugin methods

    #region Implementation

    private static void CommsFailure(object obj)
    {
      Exception ex = obj as Exception;

      if (ex != null)
        Log.Error("TV3BlasterPlugin: Communications failure: {0}", ex.ToString());
      else
        Log.Error("TV3BlasterPlugin: Communications failure");

      StopClient();

      Log.Info("TV3BlasterPlugin: Attempting communications restart ...");

      IPAddress serverIP = Network.GetIPFromName(_serverHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, Server.DefaultPort);

      StartClient(endPoint);
    }

    private static void Connected(object obj)
    {
      Log.Info("TV3BlasterPlugin: Connected to server");

      IrssMessage message = new IrssMessage(MessageType.RegisterClient, MessageFlags.Request);
      _client.Send(message);
    }

    private static void Disconnected(object obj)
    {
      Log.Info("TV3BlasterPlugin: Communications with server has been lost");

      Thread.Sleep(1000);
    }

    internal static bool StartClient(IPEndPoint endPoint)
    {
      if (_client != null)
        return false;

      ClientMessageSink sink = ReceivedMessage;

      _client = new Client(endPoint, sink);
      _client.CommsFailureCallback = CommsFailure;
      _client.ConnectCallback = Connected;
      _client.DisconnectCallback = Disconnected;

      if (_client.Start())
        return true;

      _client = null;
      return false;
    }

    internal static void StopClient()
    {
      if (_client != null)
      {
        _client.Dispose();
        _client = null;
      }
    }

    private static void ReceivedMessage(IrssMessage received)
    {
      Log.Debug("TV3BlasterPlugin: Received Message \"{0}\"", received.Type);

      try
      {
        switch (received.Type)
        {
          case MessageType.BlastIR:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              Log.Debug("TV3BlasterPlugin: Blast successful");
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              Log.Error("TV3BlasterPlugin: Failed to blast IR command");
            }
            break;

          case MessageType.RegisterClient:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              _irServerInfo = IRServerInfo.FromBytes(received.GetDataAsBytes());
              _registered = true;

              Log.Debug("TV3BlasterPlugin: Registered to IR Server");
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              _registered = false;
              Log.Error("TV3BlasterPlugin: IR Server refused to register");
            }
            break;

          case MessageType.LearnIR:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              Log.Debug("TV3BlasterPlugin: Learned IR Successfully");

              byte[] dataBytes = received.GetDataAsBytes();

              using (FileStream file = File.Create(_learnIRFilename))
                file.Write(dataBytes, 0, dataBytes.Length);
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              Log.Error("TV3BlasterPlugin: Failed to learn IR command");
            }
            else if ((received.Flags & MessageFlags.Timeout) == MessageFlags.Timeout)
            {
              Log.Error("TV3BlasterPlugin: Learn IR command timed-out");
            }

            _learnIRFilename = null;
            break;

          case MessageType.ServerShutdown:
            Log.Info("TV3BlasterPlugin: IR Server Shutdown - Plugin disabled until IR Server returns");
            _registered = false;
            break;

          case MessageType.Error:
            _learnIRFilename = null;
            Log.Error("TV3BlasterPlugin: Received error: {0}", received.GetDataAsString());
            break;
        }

        if (_handleMessage != null)
          _handleMessage(received);
      }
      catch (Exception ex)
      {
        _learnIRFilename = null;
        Log.Error(ex.ToString());
      }
    }

    /// <summary>
    /// events_OnTvServerEvent is used to receive requests to Tune External Channels.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="eventArgs">Event arguments.</param>
    private void events_OnTvServerEvent(object sender, EventArgs eventArgs)
    {
      try
      {
        TvServerEventArgs tvEvent = (TvServerEventArgs)eventArgs;

        Log.Debug("TV3BlasterPlugin: Received TV Server Event \"{0}\"",
                  Enum.GetName(typeof(TvServerEventType), tvEvent.EventType));

        if (tvEvent.EventType != TvServerEventType.StartZapChannel)
          return;

        AnalogChannel analogChannel = tvEvent.channel as AnalogChannel;
        if (analogChannel == null)
          return;

        Log.Debug("TV3BlasterPlugin: Analog channel input source \"{0}\"",
                  Enum.GetName(typeof(AnalogChannel.VideoInputType), analogChannel.VideoSource));

        //if (analogChannel.VideoSource == AnalogChannel.VideoInputType.Tuner)
        //return;

        Log.Info("TV3BlasterPlugin: Tune request - Card: {0}, Channel: {1}, {2}", tvEvent.Card.Id,
                 analogChannel.ChannelNumber, analogChannel.Name);

        if (_externalChannelConfigs == null)
          throw new InvalidOperationException("Cannot process tune request, no STB settings are loaded");

        Thread newThread = new Thread(ProcessExternalChannel);
        newThread.Name = "ProcessExternalChannel";
        newThread.Priority = ThreadPriority.AboveNormal;
        newThread.IsBackground = true;
        newThread.Start(new int[] { analogChannel.ChannelNumber, tvEvent.Card.Id });
      }
      catch (Exception ex)
      {
        Log.Error(ex.ToString());
      }
    }

    /// <summary>
    /// Load external channel configurations.
    /// </summary>
    internal static void LoadExternalConfigs()
    {
      IList<Card> cards = Card.ListAll();

      if (cards.Count == 0)
      {
        Log.Info("Cannot load external channel configurations, there are no TV cards registered");

        Card dummyCard = new Card(0, "device path", "Dummy TV Card", 0, false, DateTime.Now, "recording folder", 0,
                                  false, 0, "timeshifting folder", 0, 0, false, false, false, 0);
        cards.Add(dummyCard);
      }

      _externalChannelConfigs = new ExternalChannelConfig[cards.Count];

      int index = 0;
      foreach (Card card in cards)
      {
        string fileName = Path.Combine(ExtCfgFolder, String.Format("ExternalChannelConfig{0}.xml", card.IdCard));
        try
        {
          _externalChannelConfigs[index] = ExternalChannelConfig.Load(fileName);
        }
        catch (Exception ex)
        {
          _externalChannelConfigs[index] = new ExternalChannelConfig(fileName);
          Log.Error(ex.ToString());
        }

        _externalChannelConfigs[index].CardId = card.IdCard;
        index++;
      }
    }

    /// <summary>
    /// Given a card ID returns the configuration for that card.
    /// </summary>
    /// <param name="cardId">ID of card to retreive configuration for.</param>
    /// <returns>Card configuration, null if it doesn't exist.</returns>
    internal static ExternalChannelConfig GetExternalChannelConfig(int cardId)
    {
      if (_externalChannelConfigs == null)
        return null;

      foreach (ExternalChannelConfig config in _externalChannelConfigs)
        if (config.CardId == cardId)
          return config;

      return null;
    }

    /// <summary>
    /// Processes the external channel.
    /// </summary>
    /// <param name="args">Integer array of parameters.</param>
    private static void ProcessExternalChannel(object args)
    {
      try
      {
        int[] data = args as int[];
        if (data == null)
          throw new ArgumentException("Parameter is not of type int[]", "args");

        ExternalChannelConfig config = GetExternalChannelConfig(data[1]);
        if (config == null)
          throw new InvalidOperationException(String.Format("External channel config for card \"{0}\" not found",
                                                            data[1]));

        // Clean up the "data[0]" string into "channel".
        StringBuilder channel = new StringBuilder();
        foreach (char digit in data[0].ToString())
          if (char.IsDigit(digit))
            channel.Append(digit);

        // Pad the channel number with leading 0's to meet ChannelDigits length.
        while (channel.ToString().Length < config.ChannelDigits)
          channel.Insert(0, '0');

        // Process the channel and blast the relevant IR Commands.
        string channelString = channel.ToString();

        for (int repeatCount = 0; repeatCount <= config.RepeatChannelCommands; repeatCount++)
        {
          string command;

          if (repeatCount > 0 && config.RepeatPauseTime > 0)
            Thread.Sleep(config.RepeatPauseTime);

          if (config.UsePreChangeCommand)
          {
            command = config.PreChangeCommand;
            if (!String.IsNullOrEmpty(command))
            {
              ProcessExternalCommand(command, -1, channelString);

              if (config.PauseTime > 0)
                Thread.Sleep(config.PauseTime);
            }
          }

          foreach (char digit in channelString)
          {
            int charVal = digit - 48;

            command = config.Digits[charVal];
            if (!String.IsNullOrEmpty(command))
            {
              ProcessExternalCommand(command, charVal, channelString);

              if (config.PauseTime > 0)
                Thread.Sleep(config.PauseTime);
            }
          }

          if (config.SendSelect)
          {
            command = config.SelectCommand;
            if (!String.IsNullOrEmpty(command))
            {
              ProcessExternalCommand(command, -1, channelString);

              if (config.DoubleChannelSelect)
              {
                if (config.PauseTime > 0)
                  Thread.Sleep(config.PauseTime);

                ProcessExternalCommand(command, -1, channelString);
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error(ex.ToString());
      }
    }

    /// <summary>
    /// Processes the external channel change command.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="channelDigit">The current channel digit.</param>
    /// <param name="channelFull">The channel full ID.</param>
    internal static void ProcessExternalCommand(string command, int channelDigit, string channelFull)
    {
      Log.Debug("TV3BlasterPlugin: ProcessExternalCommand(\"{0}\", {1}, {2})", command, channelDigit, channelFull);

      if (command.StartsWith(Common.CmdPrefixRun, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitRunCommand(command.Substring(Common.CmdPrefixRun.Length));

        commands[2] = commands[2].Replace("%1", channelDigit.ToString());
        commands[2] = commands[2].Replace("%2", channelFull);

        Common.ProcessRunCommand(commands);
      }
      else if (command.StartsWith(Common.CmdPrefixSerial, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitSerialCommand(command.Substring(Common.CmdPrefixSerial.Length));

        commands[0] = commands[0].Replace("%1", channelDigit.ToString());
        commands[0] = commands[0].Replace("%2", channelFull);

        Common.ProcessSerialCommand(commands);
      }
      else if (command.StartsWith(Common.CmdPrefixWindowMsg, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitWindowMessageCommand(command.Substring(Common.CmdPrefixWindowMsg.Length));

        commands[3] = commands[3].Replace("%1", channelDigit.ToString());
        commands[3] = commands[3].Replace("%2", channelFull);

        commands[4] = commands[4].Replace("%1", channelDigit.ToString());
        commands[4] = commands[4].Replace("%2", channelFull);

        Common.ProcessWindowMessageCommand(commands);
      }
      else if (command.StartsWith(Common.CmdPrefixTcpMsg, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitTcpMessageCommand(command.Substring(Common.CmdPrefixTcpMsg.Length));

        commands[0] = commands[0].Replace("%1", channelDigit.ToString());
        commands[0] = commands[0].Replace("%2", channelFull);

        commands[2] = commands[2].Replace("%1", channelDigit.ToString());
        commands[2] = commands[2].Replace("%2", channelFull);

        Common.ProcessTcpMessageCommand(commands);
      }
      else if (command.StartsWith(Common.CmdPrefixHttpMsg, StringComparison.OrdinalIgnoreCase))
      {
        string[] commands = Common.SplitHttpMessageCommand(command.Substring(Common.CmdPrefixHttpMsg.Length));

        commands[0] = commands[0].Replace("%1", channelDigit.ToString());
        commands[0] = commands[0].Replace("%2", channelFull);

        Common.ProcessHttpCommand(commands);
      }
      else
      {
        ProcessCommand(command, false);
      }
    }

    /// <summary>
    /// Learn an IR command.
    /// </summary>
    /// <param name="fileName">File to place learned IR command in (absolute path).</param>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    internal static bool LearnIR(string fileName, LearnStatusDelegate learnStatus = null)
    {
      try
      {
        if (String.IsNullOrEmpty(fileName))
        {
          Log.Error("TV3BlasterPlugin: Null or Empty file name for LearnIR()");
          return false;
        }

        if (!_registered)
        {
          Log.Error("TV3BlasterPlugin: Not registered to an active IR Server");
          return false;
        }

        if (_learnIRFilename != null)
        {
          Log.Error("TV3BlasterPlugin: Already trying to learn an IR command");
          return false;
        }

        _learnIRFilename = fileName;

        IrssMessage message = new IrssMessage(MessageType.LearnIR, MessageFlags.Request);
        _client.Send(message);
      }
      catch (Exception ex)
      {
        _learnIRFilename = null;
        Log.Error(ex.ToString());
        return false;
      }

      return true;
    }

    /// <summary>
    /// Blast an IR command.
    /// </summary>
    /// <param name="fileName">File to blast (absolute path).</param>
    /// <param name="port">Port to blast to.</param>
    internal static void BlastIR(string fileName, string port)
    {
      Log.Debug("TV3BlasterPlugin - BlastIR(): {0}, {1}", fileName, port);

      if (!_registered)
        throw new InvalidOperationException("Cannot Blast, not registered to an active IR Server");

      using (FileStream file = File.OpenRead(fileName))
      {
        if (file.Length == 0)
          throw new IOException(String.Format("Cannot Blast. IR file \"{0}\" has no data, possible IR learn failure",
                                              fileName));

        byte[] outData = new byte[4 + port.Length + file.Length];

        BitConverter.GetBytes(port.Length).CopyTo(outData, 0);
        Encoding.ASCII.GetBytes(port).CopyTo(outData, 4);

        file.Read(outData, 4 + port.Length, (int)file.Length);

        IrssMessage message = new IrssMessage(MessageType.BlastIR, MessageFlags.Request, outData);
        _client.Send(message);
      }
    }

    /// <summary>
    /// Given a command this method processes the request accordingly.
    /// Throws exceptions only if run synchronously, if async exceptions are logged instead.
    /// </summary>
    /// <param name="command">Command to process.</param>
    /// <param name="async">Process command asynchronously?</param>
    internal static void ProcessCommand(string command, bool async)
    {
      if (async)
      {
        try
        {
          Thread newThread = new Thread(ProcCommand);
          newThread.Name = ProcessCommandThreadName;
          newThread.IsBackground = true;
          newThread.Start(command);
        }
        catch (Exception ex)
        {
          Log.Error(ex.ToString());
        }
      }
      else
      {
        ProcCommand(command);
      }
    }

    /// <summary>
    /// Used by ProcessCommand to actually handle the command.
    /// Can be called Synchronously or as a Parameterized Thread.
    /// </summary>
    /// <param name="commandObj">Command string to process.</param>
    private static void ProcCommand(object commandObj)
    {
      try
      {
        if (commandObj == null)
          throw new ArgumentNullException("commandObj");

        string command = commandObj as string;

        if (String.IsNullOrEmpty(command))
          throw new ArgumentException("commandObj translates to empty or null string", "commandObj");

        if (command.StartsWith(Common.CmdPrefixMacro, StringComparison.OrdinalIgnoreCase))
        {
          string fileName = PathCombine(command.Substring(Common.CmdPrefixMacro.Length));
          ProcMacro(fileName);
        }
        else if (command.StartsWith(Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitBlastCommand(command.Substring(Common.CmdPrefixBlast.Length));
          BlastIR(Path.Combine(Common.FolderIRCommands, commands[0] + Common.FileExtensionIR), commands[1]);
        }
        else if (command.StartsWith(Common.CmdPrefixSTB, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitBlastCommand(command.Substring(Common.CmdPrefixSTB.Length));
          BlastIR(Path.Combine(Common.FolderSTB, commands[0] + Common.FileExtensionIR), commands[1]);
        }
        else if (command.StartsWith(Common.CmdPrefixPause, StringComparison.OrdinalIgnoreCase))
        {
          int pauseTime = int.Parse(command.Substring(Common.CmdPrefixPause.Length));
          Thread.Sleep(pauseTime);
        }
        else if (command.StartsWith(Common.CmdPrefixRun, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitRunCommand(command.Substring(Common.CmdPrefixRun.Length));
          Common.ProcessRunCommand(commands);
        }
        else if (command.StartsWith(Common.CmdPrefixSerial, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitSerialCommand(command.Substring(Common.CmdPrefixSerial.Length));
          Common.ProcessSerialCommand(commands);
        }
        else if (command.StartsWith(Common.CmdPrefixWindowMsg, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitWindowMessageCommand(command.Substring(Common.CmdPrefixWindowMsg.Length));
          Common.ProcessWindowMessageCommand(commands);
        }
        else if (command.StartsWith(Common.CmdPrefixTcpMsg, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitTcpMessageCommand(command.Substring(Common.CmdPrefixTcpMsg.Length));
          Common.ProcessTcpMessageCommand(commands);
        }
        else if (command.StartsWith(Common.CmdPrefixHttpMsg, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitHttpMessageCommand(command.Substring(Common.CmdPrefixHttpMsg.Length));
          Common.ProcessHttpCommand(commands);
        }
        else if (command.StartsWith(Common.CmdPrefixEject, StringComparison.OrdinalIgnoreCase))
        {
          string ejectCommand = command.Substring(Common.CmdPrefixEject.Length);
          Common.ProcessEjectCommand(ejectCommand);
        }
        else
        {
          throw new ArgumentException(String.Format("Cannot process unrecognized command \"{0}\"", command),
                                      "commandObj");
        }
      }
      catch (Exception ex)
      {
        if (!String.IsNullOrEmpty(Thread.CurrentThread.Name) &&
            Thread.CurrentThread.Name.Equals(ProcessCommandThreadName, StringComparison.OrdinalIgnoreCase))
          Log.Error(ex.ToString());
        else
          throw;
      }
    }

    /// <summary>
    /// Called by ProcCommand to process the supplied Macro file.
    /// </summary>
    /// <param name="fileName">Macro file to process (absolute path).</param>
    private static void ProcMacro(string fileName)
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(fileName);

      XmlNodeList commandSequence = doc.DocumentElement.SelectNodes("item");

      foreach (XmlNode item in commandSequence)
        ProcCommand(item.Attributes["command"].Value);
    }

    /// <summary>
    /// Returns a list of Macros.
    /// </summary>
    /// <param name="commandPrefix">Add the command prefix to each list item.</param>
    /// <returns>string[] of Macros.</returns>
    internal static string[] GetMacroList(bool commandPrefix)
    {
      string[] files = Directory.GetFiles(FolderMacros, '*' + Common.FileExtensionMacro);
      string[] list = new string[files.Length];

      int i = 0;
      foreach (string file in files)
      {
        if (commandPrefix)
          list[i++] = Common.CmdPrefixMacro + Path.GetFileNameWithoutExtension(file);
        else
          list[i++] = Path.GetFileNameWithoutExtension(file);
      }

      return list;
    }

    /// <summary>
    /// Returns a combined list of IR Commands and Macros.
    /// </summary>
    /// <param name="commandPrefix">Add the command prefix to each list item.</param>
    /// <returns>string[] of IR Commands and Macros.</returns>
    internal static string[] GetFileList(bool commandPrefix)
    {
      string[] MacroFiles = Directory.GetFiles(FolderMacros, '*' + Common.FileExtensionMacro);
      string[] IRFiles = Directory.GetFiles(Common.FolderIRCommands, '*' + Common.FileExtensionIR);
      string[] list = new string[MacroFiles.Length + IRFiles.Length];

      int i = 0;
      foreach (string file in MacroFiles)
      {
        if (commandPrefix)
          list[i++] = Common.CmdPrefixMacro + Path.GetFileNameWithoutExtension(file);
        else
          list[i++] = Path.GetFileNameWithoutExtension(file);
      }

      foreach (string file in IRFiles)
      {
        if (commandPrefix)
          list[i++] = Common.CmdPrefixBlast + Path.GetFileNameWithoutExtension(file);
        else
          list[i++] = Path.GetFileNameWithoutExtension(file);
      }

      return list;
    }

    /// <summary>
    /// Loads the settings.
    /// </summary>
    private static void LoadSettings()
    {
      TvBusinessLayer layer = new TvBusinessLayer();
      ServerHost = layer.GetSetting("TV3BlasterPlugin_ServerHost", "localhost").Value;
    }

    public static string PathCombine(string filename)
    {
      return Path.Combine(FolderMacros, filename + Common.FileExtensionMacro);
    }

    #endregion Implementation
  }
}