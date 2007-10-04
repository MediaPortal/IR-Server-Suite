using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using IrssComms;
using IrssUtils;

namespace DebugClient
{

  partial class MainForm : Form
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
    }

    #endregion

    #region Constants

    static readonly string DebugIRFile = IrssUtils.Common.FolderIRCommands + "DebugClient.IR";

    #endregion

    #region Variables

    Client _client = null;

    string _serverHost      = "localhost";
    string _learnIRFilename = null;

    bool _registered = false;

    IRServerInfo _irServerInfo = new IRServerInfo();

    #endregion Variables

    delegate void DelegateAddStatusLine(string status);
    DelegateAddStatusLine _addStatusLine = null;

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

      _addStatusLine = new DelegateAddStatusLine(AddStatusLine);

      comboBoxRemoteButtons.Items.AddRange(Enum.GetNames(typeof(MceButton)));
      comboBoxRemoteButtons.SelectedIndex = 0;

      comboBoxPort.Items.Clear();
      comboBoxPort.Items.Add("None");
      comboBoxPort.SelectedIndex = 0;

      comboBoxComputer.Items.Clear();
      comboBoxComputer.Items.Add("localhost");

      string[] networkPCs = IrssUtils.Win32.GetNetworkComputers(false);
      if (networkPCs != null)
        comboBoxComputer.Items.AddRange(networkPCs);

      comboBoxComputer.Text = _serverHost;
    }
    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      buttonDisconnect_Click(null, null);
      
      _addStatusLine = null;

      IrssLog.Close();
    }

    void ReceivedMessage(IrssMessage received)
    {

      this.Invoke(_addStatusLine, new Object[] { String.Format("Received Message: \"{0}, {1}\"", received.Type, received.Flags) });

      try
      {
        switch (received.Type)
        {
          case MessageType.RegisterClient:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              _registered = true;
              _irServerInfo = IRServerInfo.FromBytes(received.GetDataAsBytes());
              comboBoxPort.Items.Clear();
              comboBoxPort.Items.AddRange(_irServerInfo.Ports);
              comboBoxPort.SelectedIndex = 0;

              _client.Send(new IrssMessage(MessageType.ActiveReceivers, MessageFlags.Request));
              _client.Send(new IrssMessage(MessageType.ActiveBlasters, MessageFlags.Request));
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              _registered = false;
            }
            return;

          case MessageType.ActiveBlasters:
            this.Invoke(_addStatusLine, new Object[] { received.GetDataAsString() });
            break;

          case MessageType.ActiveReceivers:
            this.Invoke(_addStatusLine, new Object[] { received.GetDataAsString() });
            break;

          case MessageType.RemoteEvent:
            RemoteHandlerCallback(received.GetDataAsString());
            return;

          case MessageType.LearnIR:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              byte[] dataBytes = received.GetDataAsBytes();

              using (FileStream file = File.Create(_learnIRFilename))
                file.Write(dataBytes, 0, dataBytes.Length);
            }

            _learnIRFilename = null;
            break;

          case MessageType.ServerShutdown:
            _registered = false;
            return;

          case MessageType.Error:
            _learnIRFilename = null;
            this.Invoke(_addStatusLine, new Object[] { received.GetDataAsString() });
            return;
        }
      }
      catch (Exception ex)
      {
        this.Invoke(_addStatusLine, new Object[] { ex.Message });
      }
    }

    bool LearnIR(string fileName)
    {
      try
      {
        if (_learnIRFilename != null)
          return false;

        _learnIRFilename = fileName;

        IrssMessage message = new IrssMessage(MessageType.LearnIR, MessageFlags.Request);
        _client.Send(message);

        AddStatusLine("Learning");
      }
      catch (Exception ex)
      {
        _learnIRFilename = null;
        AddStatusLine(ex.Message);
        return false;
      }

      return true;
    }
    bool BlastIR(string fileName, string port)
    {
      try
      {
        using (FileStream file = File.OpenRead(fileName))
        {
          if (file.Length == 0)
            throw new IOException(String.Format("Cannot Blast. IR file \"{0}\" has no data, possible IR learn failure", fileName));

          byte[] outData = new byte[4 + port.Length + file.Length];

          BitConverter.GetBytes(port.Length).CopyTo(outData, 0);
          Encoding.ASCII.GetBytes(port).CopyTo(outData, 4);

          file.Read(outData, 4 + port.Length, (int)file.Length);

          IrssMessage message = new IrssMessage(MessageType.BlastIR, MessageFlags.Request, outData);
          _client.Send(message);
        }
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

      this.Invoke(_addStatusLine, new Object[] { text });
    }

    void CommsFailure(object obj)
    {
      Exception ex = obj as Exception;
      
      if (ex != null)
        this.Invoke(_addStatusLine, new Object[] { String.Format("Communications failure: {0}", ex.Message) });
      else
        this.Invoke(_addStatusLine, new Object[] { "Communications failure" });

      StopClient();
    }
    void Connected(object obj)
    {
      IrssLog.Info("Connected to server");

      IrssMessage message = new IrssMessage(MessageType.RegisterClient, MessageFlags.Request);
      _client.Send(message);
    }
    void Disconnected(object obj)
    {
      IrssLog.Warn("Communications with server has been lost");

      Thread.Sleep(1000);
    }

    bool StartClient(IPEndPoint endPoint)
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
    void StopClient()
    {
      if (_client == null)
        return;

      _client.Dispose();
      _client = null;
    }

    #region Controls

    private void buttonConnect_Click(object sender, EventArgs e)
    {
      try
      {
        AddStatusLine("Connect");
        listBoxStatus.Update();

        if (_client != null)
        {
          AddStatusLine("Already connected/connecting");
          return;
        }

        _serverHost = comboBoxComputer.Text;

        IPAddress serverIP = Client.GetIPFromName(_serverHost);
        IPEndPoint endPoint = new IPEndPoint(serverIP, IrssComms.Server.DefaultPort);

        StartClient(endPoint);
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
        if (_client == null)
        {
          AddStatusLine(" - Not connected");
          return;
        }

        if (_registered)
        {
          IrssMessage message = new IrssMessage(MessageType.UnregisterClient, MessageFlags.Request);
          _client.Send(message);
        }

        StopClient();
      }
      catch (Exception ex)
      {
        AddStatusLine(ex.Message);
      }
    }
    private void buttonBlast_Click(object sender, EventArgs e)
    {
      AddStatusLine("Blast IR");

      if (_client == null)
      {
        AddStatusLine(" - Not connected");
        return;
      }

      if (!_client.Connected)
      {
        AddStatusLine(" - Connecting...");
        return;
      }

      if (!_irServerInfo.CanTransmit)
      {
        AddStatusLine(" - IR Server is not setup to blast");
        return;
      }

      if (BlastIR(DebugIRFile, comboBoxPort.SelectedItem as string))
        AddStatusLine(" - Blasting ...");
      else
        AddStatusLine(" - Can't Blast");
    }
    private void buttonLearnIR_Click(object sender, EventArgs e)
    {
      AddStatusLine("Learn IR");

      if (_client == null)
      {
        AddStatusLine(" - Not connected");
        return;
      }

      if (!_client.Connected)
      {
        AddStatusLine(" - Connecting...");
        return;
      }

      if (!_irServerInfo.CanLearn)
      {
        AddStatusLine(" - IR Server is not setup to support learning");
        return;
      }

      if (LearnIR(DebugIRFile))
        AddStatusLine(" - Learning IR ...");
      else
        AddStatusLine(" - Learn IR Busy");
    }
    private void buttonShutdownServer_Click(object sender, EventArgs e)
    {
      AddStatusLine("Shutdown");

      if (_client == null)
      {
        AddStatusLine(" - Not connected");
        return;
      }

      if (!_client.Connected)
      {
        AddStatusLine(" - Connecting...");
        return;
      }

      try
      {
        IrssMessage message = new IrssMessage(MessageType.ServerShutdown, MessageFlags.Request);
        _client.Send(message);
      }
      catch (Exception ex)
      {
        AddStatusLine(ex.Message);
      }
    }

    private void comboBoxRemoteButtons_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (comboBoxRemoteButtons.SelectedItem.ToString() == "Custom")
        numericUpDownButton.Enabled = true;
      else
        numericUpDownButton.Enabled = false;
    }

    private void buttonSendRemoteButton_Click(object sender, EventArgs e)
    {
      AddStatusLine("Send Remote Button");

      try
      {
        if (_client == null)
        {
          AddStatusLine(" - Not connected");
          return;
        }

        if (!_client.Connected)
        {
          AddStatusLine(" - Connecting...");
          return;
        }

        int keyCode = (int)Enum.Parse(typeof(MceButton), comboBoxRemoteButtons.SelectedItem.ToString(), true);
        if (keyCode == -1)
          keyCode = Decimal.ToInt32(numericUpDownButton.Value);

        IrssMessage message = new IrssMessage(MessageType.ForwardRemoteEvent, MessageFlags.Notify, keyCode.ToString());
        _client.Send(message);
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
      AutoTest.Name = "DebugClient.AutoTest";
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

        this.Invoke(_addStatusLine, new Object[] { String.Format("AutoTest: {0}", randomNumber) });

        process.StartInfo.Arguments = "-host localhost -pad 4 -channel " + randomNumber.ToString();

        process.Start();
        process.WaitForExit();

        Thread.Sleep(10000);
      }
    }

  }

}
