using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using NamedPipes;
using IrssUtils;

namespace DebugClient
{

  public partial class MainForm : Form
  {

    #region Enumerations

    /// <summary>
    /// A list of MCE remote buttons.
    /// </summary>
    public enum MceButton
    {
      Custom        = -1,
      None          = 0,
      TV_Power      = 0x7b9a,
      Blue          = 0x7ba1,
      Yellow        = 0x7ba2,
      Green         = 0x7ba3,
      Red           = 0x7ba4,
      Teletext      = 0x7ba5,
      Radio         = 0x7baf,
      Print         = 0x7bb1,
      Videos        = 0x7bb5,
      Pictures      = 0x7bb6,
      Recorded_TV   = 0x7bb7,
      Music         = 0x7bb8,
      TV            = 0x7bb9,
      Guide         = 0x7bd9,
      Live_TV       = 0x7bda,
      DVD_Menu      = 0x7bdb,
      Back          = 0x7bdc,
      OK            = 0x7bdd,
      Right         = 0x7bde,
      Left          = 0x7bdf,
      Down          = 0x7be0,
      Up            = 0x7be1,
      Star          = 0x7be2,
      Hash          = 0x7be3,
      Replay        = 0x7be4,
      Skip          = 0x7be5,
      Stop          = 0x7be6,
      Pause         = 0x7be7,
      Record        = 0x7be8,
      Play          = 0x7be9,
      Rewind        = 0x7bea,
      Forward       = 0x7beb,
      Channel_Down  = 0x7bec,
      Channel_Up    = 0x7bed,
      Volume_Down   = 0x7bee,
      Volume_Up     = 0x7bef,
      Info          = 0x7bf0,
      Mute          = 0x7bf1,
      Start         = 0x7bf2,
      PC_Power      = 0x7bf3,
      Enter         = 0x7bf4,
      Escape        = 0x7bf5,
      Number_9      = 0x7bf6,
      Number_8      = 0x7bf7,
      Number_7      = 0x7bf8,
      Number_6      = 0x7bf9,
      Number_5      = 0x7bfa,
      Number_4      = 0x7bfb,
      Number_3      = 0x7bfc,
      Number_2      = 0x7bfd,
      Number_1      = 0x7bfe,
      Number_0      = 0x7bff,
    }

    #endregion Enumerations

    #region Constructor

    public MainForm()
    {
      InitializeComponent();

      _messageQueue = new MessageQueue(new MessageQueueSink(ReceivedMessage));
    }

    #endregion

    #region Constants

    static readonly string DebugIRFile = IrssUtils.Common.FolderIRCommands + "DebugClient.IR";

    #endregion

    #region Variables

    MessageQueue _messageQueue;

    string _serverHost      = Environment.MachineName;
    string _localPipeName   = null;
    string _learnIRFilename = null;

    bool _registered = false;
    bool _keepAlive = true;
    int _echoID = -1;
    Thread _keepAliveThread;

    IRServerInfo _irServerInfo = new IRServerInfo();

    #endregion Variables

    delegate void DelegateAddStatusLine(string status);
    DelegateAddStatusLine _AddStatusLine = null;

    void AddStatusLine(string status)
    {
      IrssLog.Info(status);

      listBoxStatus.Items.Add(status);

      listBoxStatus.SetSelected(listBoxStatus.Items.Count - 1, true);
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      IrssLog.LogLevel = IrssLog.Level.Debug;
      IrssLog.Open(Common.FolderIrssLogs + "Debug Client.log");

      _AddStatusLine = new DelegateAddStatusLine(AddStatusLine);

      comboBoxRemoteButtons.Items.AddRange(Enum.GetNames(typeof(MceButton)));
      comboBoxRemoteButtons.SelectedIndex = 0;

      comboBoxPort.Items.Clear();
      comboBoxPort.Items.Add("None");
      comboBoxPort.SelectedIndex = 0;

      ArrayList networkPCs = IrssUtils.Win32.GetNetworkComputers();
      if (networkPCs != null)
      {
        comboBoxComputer.Items.AddRange(networkPCs.ToArray());
        comboBoxComputer.Text = _serverHost;
      }
    }
    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      buttonDisconnect_Click(null, null);
      
      _AddStatusLine = null;

      IrssLog.Close();
    }

    void ReceivedMessage(string message)
    {
      PipeMessage received = PipeMessage.FromString(message);

      this.Invoke(_AddStatusLine, new Object[] { String.Format("Received Message: \"{0}, {1}\"", received.Type, received.Flags) });

      try
      {
        switch (received.Type)
        {
          case PipeMessageType.RegisterClient:
            if ((received.Flags & PipeMessageFlags.Success) == PipeMessageFlags.Success)
            {
              _registered = true;
              _irServerInfo = IRServerInfo.FromBytes(received.DataAsBytes);
              comboBoxPort.Items.Clear();
              comboBoxPort.Items.AddRange(_irServerInfo.Ports);
              comboBoxPort.SelectedIndex = 0;
            }
            else if ((received.Flags & PipeMessageFlags.Failure) == PipeMessageFlags.Failure)
            {
              _registered = false;
            }
            return;

          case PipeMessageType.RemoteEvent:
            RemoteHandlerCallback(received.DataAsString);
            return;

          case PipeMessageType.LearnIR:
            if ((received.Flags & PipeMessageFlags.Success) == PipeMessageFlags.Success)
            {
              byte[] dataBytes = received.DataAsBytes;

              FileStream file = new FileStream(_learnIRFilename, FileMode.Create);
              file.Write(dataBytes, 0, dataBytes.Length);
              file.Close();
            }

            _learnIRFilename = null;
            break;

          case PipeMessageType.ServerShutdown:
            _registered = false;
            return;

          case PipeMessageType.Echo:
            _echoID = BitConverter.ToInt32(received.DataAsBytes, 0);
            return;

          case PipeMessageType.Error:
            _learnIRFilename = null;
            this.Invoke(_AddStatusLine, new Object[] { received.DataAsString });
            return;
        }
      }
      catch (Exception ex)
      {
        this.Invoke(_AddStatusLine, new Object[] { ex.Message });
      }
    }

    bool LearnIR(string fileName)
    {
      try
      {
        if (_learnIRFilename != null)
          return false;

        _learnIRFilename = fileName;
        
        PipeMessage message = new PipeMessage(Environment.MachineName, _localPipeName, PipeMessageType.LearnIR, PipeMessageFlags.Request);
        PipeAccess.SendMessage(Common.ServerPipeName, _serverHost, message);

        AddStatusLine("Learning");
      }
      catch (Exception ex)
      {
        AddStatusLine(ex.Message);
        return false;
      }

      return true;
    }
    bool BlastIR(string fileName, string port)
    {
      try
      {
        if (!File.Exists(fileName))
          return false;

        FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

        byte[] outData = new byte[4 + port.Length + file.Length];

        BitConverter.GetBytes(port.Length).CopyTo(outData, 0);
        Encoding.ASCII.GetBytes(port).CopyTo(outData, 4);

        file.Read(outData, 4 + port.Length, (int)file.Length);
        file.Close();

        PipeMessage message = new PipeMessage(Environment.MachineName, _localPipeName, PipeMessageType.BlastIR, PipeMessageFlags.Request, outData);
        PipeAccess.SendMessage(Common.ServerPipeName, _serverHost, message);
      }
      catch (Exception ex)
      {
        AddStatusLine(ex.Message);
        return false;
      }

      return true;
    }
    void RemoteHandlerCallback(string keyCode)
    {
      string text = String.Format("Remote Event \"{0}\"", keyCode);

      this.Invoke(_AddStatusLine, new Object[] { text });
    }

    bool StartComms()
    {
      try
      {
        if (OpenLocalPipe())
        {
          _messageQueue.Start();

          _keepAliveThread = new Thread(new ThreadStart(KeepAliveThread));
          _keepAliveThread.Start();
          return true;
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
      }

      return false;
    }
    void StopComms()
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

          PipeMessage message = new PipeMessage(Environment.MachineName, _localPipeName, PipeMessageType.UnregisterClient, PipeMessageFlags.Request);
          PipeAccess.SendMessage(Common.ServerPipeName, _serverHost, message);
        }
      }
      catch { }

      _messageQueue.Stop();

      try
      {
        if (PipeAccess.ServerRunning)
          PipeAccess.StopServer();
      }
      catch { }
    }

    bool OpenLocalPipe()
    {
      try
      {
        int pipeNumber = 1;
        bool retry = false;

        do
        {
          string localPipeTest = String.Format("irserver\\debug{0:00}", pipeNumber);

          if (PipeAccess.PipeExists(Common.LocalPipePrefix + localPipeTest))
          {
            if (++pipeNumber <= Common.MaximumLocalClientCount)
              retry = true;
            else
              throw new Exception(String.Format("Maximum local client limit ({0}) reached", Common.MaximumLocalClientCount));
          }
          else
          {
            if (!PipeAccess.StartServer(localPipeTest, new PipeMessageHandler(_messageQueue.Enqueue)))
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
        IrssLog.Error(ex.ToString());
        return false;
      }
    }

    bool ConnectToServer()
    {
      try
      {
        PipeMessage message = new PipeMessage(Environment.MachineName, _localPipeName, PipeMessageType.RegisterClient, PipeMessageFlags.Request);
        PipeAccess.SendMessage(Common.ServerPipeName, _serverHost, message);
        return true;
      }
      catch (AppModule.NamedPipes.NamedPipeIOException)
      {
        return false;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
        return false;
      }
    }

    void KeepAliveThread()
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

        IrssLog.Info("Connecting ({0}) ...", _serverHost);
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
            PipeMessage message = new PipeMessage(Environment.MachineName, _localPipeName, PipeMessageType.Ping, PipeMessageFlags.Request, BitConverter.GetBytes(pingID));
            PipeAccess.SendMessage(Common.ServerPipeName, _serverHost, message);
          }
          catch
          {
            // Failed to ping ... reconnect ...
            IrssLog.Warn("Failed to ping, attempting to reconnect ...");
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
            IrssLog.Warn("No echo to ping, attempting to reconnect ...");

            // Break out of pinging cycle ...
            _registered = false;
            reconnect = true;
          }
        }

        #endregion Ping the server repeatedly

      }

    }



    #region Controls

    private void buttonConnect_Click(object sender, EventArgs e)
    {
      try
      {
        AddStatusLine("Connect");
        listBoxStatus.Update();

        if (PipeAccess.ServerRunning)
        {
          AddStatusLine("Already connected");
          return;
        }

        _serverHost = comboBoxComputer.Text;

        StartComms();
      }
      catch (Exception ex)
      {
        AddStatusLine(ex.Message);
      }
    }
    private void buttonDisconnect_Click(object sender, EventArgs e)
    {
      AddStatusLine("Disconnect");

      try
      {
        if (!PipeAccess.ServerRunning)
        {
          AddStatusLine(" - Not connected");
          return;
        }

        StopComms();
      }
      catch (Exception ex)
      {
        AddStatusLine(ex.Message);
      }
    }
    private void buttonBlast_Click(object sender, EventArgs e)
    {
      if (!PipeAccess.ServerRunning)
      {
        AddStatusLine("Not connected");
        return;
      }

      if (!_irServerInfo.CanTransmit)
      {
        AddStatusLine("IR Server is not setup to blast");
        return;
      }

      if (BlastIR(DebugIRFile, comboBoxPort.SelectedItem as string))
        AddStatusLine("Blasting");
      else
        AddStatusLine("Can't Blast");
    }
    private void buttonLearnIR_Click(object sender, EventArgs e)
    {
      if (!PipeAccess.ServerRunning)
      {
        AddStatusLine("Not connected");
        return;
      }

      if (!_irServerInfo.CanLearn)
      {
        AddStatusLine("IR Server is not setup to support learning");
        return;
      }

      if (LearnIR(DebugIRFile))
        AddStatusLine("Learning IR");
      else
        AddStatusLine("Learn IR Busy");
    }
    private void buttonShutdownServer_Click(object sender, EventArgs e)
    {
      AddStatusLine("Shutdown");

      if (!PipeAccess.ServerRunning)
      {
        AddStatusLine(" - Not connected");
        return;
      }

      try
      {
        PipeMessage message = new PipeMessage(Environment.MachineName, _localPipeName, PipeMessageType.ServerShutdown, PipeMessageFlags.Request);
        PipeAccess.SendMessage(Common.ServerPipeName, _serverHost, message);
      }
      catch (Exception ex)
      {
        AddStatusLine(ex.Message);
      }
    }
    private void buttonPing_Click(object sender, EventArgs e)
    {
      AddStatusLine("Ping Server");

      if (!PipeAccess.ServerRunning)
      {
        AddStatusLine(" - Not connected");
        return;
      }

      try
      {
        PipeMessage message = new PipeMessage(Environment.MachineName, _localPipeName, PipeMessageType.Ping, PipeMessageFlags.Request, BitConverter.GetBytes(24));
        PipeAccess.SendMessage(Common.ServerPipeName, _serverHost, message);
      }
      catch (Exception ex)
      {
        AddStatusLine(ex.Message);
      }
    }

    private void comboBoxRemoteButtons_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (comboBoxRemoteButtons.SelectedItem.ToString() == "Custom")
      {
        numericUpDownButton.Enabled = true;
      }
      else
      {
        numericUpDownButton.Enabled = false;
      }
    }

    private void buttonSendRemoteButton_Click(object sender, EventArgs e)
    {
      AddStatusLine("Send Remote Button");

      try
      {
        if (!PipeAccess.ServerRunning)
        {
          AddStatusLine(" - Not connected");
          return;
        }

        int keyCode = (int)Enum.Parse(typeof(MceButton), comboBoxRemoteButtons.SelectedItem.ToString(), true);
        if (keyCode == -1)
          keyCode = Decimal.ToInt32(numericUpDownButton.Value);

        byte[] data = new byte[8];
        
        BitConverter.GetBytes(keyCode).CopyTo(data, 0);
        BitConverter.GetBytes(0).CopyTo(data, 4);

        PipeMessage message = new PipeMessage(Environment.MachineName, _localPipeName, PipeMessageType.ForwardRemoteEvent, PipeMessageFlags.Notify, data);
        PipeAccess.SendMessage(Common.ServerPipeName, _serverHost, message);
      }
      catch (Exception ex)
      {
        AddStatusLine(ex.Message);
      }
    }

    private void buttonHelp_Click(object sender, EventArgs e)
    {
      try
      {
        Help.ShowHelp(this, SystemRegistry.GetInstallFolder() + "\\IR Server Suite.chm");
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Failed to load help", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #endregion Controls



    Thread AutoTest;

    private void buttonAutoTest_Click(object sender, EventArgs e)
    {
      AutoTest = new Thread(new ThreadStart(AutoTestThread));
      AutoTest.IsBackground = true;
      AutoTest.Start();
    }

    void AutoTestThread()
    {
      Random rand = new Random();

      int randomNumber;

      Process process = new Process();
      process.StartInfo.FileName = "IRBlast-NoWindow.exe";
      process.StartInfo.WorkingDirectory = "C:\\Program Files\\IR Server Suite\\IR Blast\\";

      while (true)
      {
        randomNumber = rand.Next(100000);

        this.Invoke(_AddStatusLine, new Object[] { String.Format("AutoTest: {0}", randomNumber) });

        process.StartInfo.Arguments = "-host localhost -pad 4 -channel " + randomNumber.ToString();

        process.Start();
        process.WaitForExit();

        Thread.Sleep(10000);
      }
    }

  }

}
