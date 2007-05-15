using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows.Forms;

using Microsoft.Win32;

using NamedPipes;
using IrssUtils;

namespace DebugClient
{

  public partial class MainForm : Form
  {

    #region Enumerations

    /// <summary>
    /// A list of MCE remote buttons
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

    const string  TempIRFile = "test.IR";

    #endregion

    #region Variables

    string _serverAddress   = Environment.MachineName;
    string _localPipeName   = null;
    string _learnIRFilename = null;

    //static bool _keepAlive  = true;
    static int _echoID      = -1;

    TransceiverInfo _transceiverInfo = new TransceiverInfo();

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

      string customTemp = String.Format(Common.LocalPipeFormat, 1);
      textBoxCustom.Text = String.Format("{0},{1},Register,null", customTemp, Environment.MachineName);

      _AddStatusLine = new DelegateAddStatusLine(AddStatusLine);

      comboBoxRemoteButtons.Items.AddRange(Enum.GetNames(typeof(MceButton)));
      comboBoxRemoteButtons.SelectedIndex = 0;

      comboBoxPort.Items.Clear();
      comboBoxPort.Items.Add("None");
      comboBoxPort.SelectedIndex = 0;

      comboBoxSpeed.Items.Clear();
      comboBoxSpeed.Items.Add("None");
      comboBoxSpeed.SelectedIndex = 0;

      ArrayList networkPCs = IrssUtils.Win32.GetNetworkComputers();
      if (networkPCs != null)
      {
        comboBoxComputer.Items.AddRange(networkPCs.ToArray());
        comboBoxComputer.Text = _serverAddress;
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

      this.Invoke(_AddStatusLine, new Object[] { received.Name });

      try
      {
        switch (received.Name)
        {
          case "Blast Success":
          case "Blast Failure":
          case "Start Learn":
            return;

          case "Clients":
          {
            string clients = String.Format("There are {0} client(s) attached to the server", BitConverter.ToInt32(received.Data, 0));
            this.Invoke(_AddStatusLine, new Object[] { clients });
            return;
          }

          case "Register Success":
          {
            _transceiverInfo = TransceiverInfo.FromBytes(received.Data);

            comboBoxPort.Items.Add(_transceiverInfo.Ports);
            comboBoxPort.SelectedIndex = 0;

            comboBoxSpeed.Items.Add(_transceiverInfo.Speeds);
            comboBoxSpeed.SelectedIndex = 0;
            return;
          }

          case "Register Failure":
          {
            if (PipeAccess.ServerRunning)
              PipeAccess.StopServer();
            return;
          }

          case "Remote Button":
          {
            string keyCode = Encoding.ASCII.GetString(received.Data);
            RemoteButtonPressed(keyCode);
            return;
          }

          case "Learn Success":
          {
            FileStream file = new FileStream(_learnIRFilename, FileMode.Create, FileAccess.Write, FileShare.None);
            file.Write(received.Data, 0, received.Data.Length);
            file.Flush();
            file.Close();

            _learnIRFilename = null;
            return;
          }

          case "Learn Failure":
          {
            _learnIRFilename = null;
            return;
          }


          case "Server Shutdown":
          {
            if (PipeAccess.ServerRunning)
              PipeAccess.StopServer();
            return;
          }

          case "Echo":
          {
            _echoID = BitConverter.ToInt32(received.Data, 0);
            break;
          }
          
          case "Error":
          {
            this.Invoke(_AddStatusLine, new Object[] { Encoding.ASCII.GetString(received.Data) });
            return;
          }

          default:
          {
            this.Invoke(_AddStatusLine, new Object[] { "Unknown message received: " + received.Name });
            return;
          }
        }
      }
      catch (Exception ex)
      {
        this.Invoke(_AddStatusLine, new Object[] { ex.Message });
      }
    }

    bool ConnectToServer(string server)
    {
      _serverAddress = server;

      bool retry = false;
      int pipeNumber = 1;
      string localPipeTest;

      do
      {
        localPipeTest = String.Format(Common.LocalPipeFormat, pipeNumber);
        AddStatusLine("Trying pipe: " + localPipeTest);
        listBoxStatus.Update();

        if (PipeAccess.PipeExists(String.Format("\\\\.\\pipe\\{0}", localPipeTest)))
        {
          pipeNumber++;
          if (pipeNumber <= Common.MaximumLocalClientCount)
          {
            retry = true;
          }
          else
          {
            AddStatusLine("Maximum local client limit reached");
            return false;
          }
        }
        else
        {
          PipeAccess.StartServer(localPipeTest, new PipeMessageHandler(ReceivedMessage));
          _localPipeName = localPipeTest;
          retry = false;
        }
      }
      while (retry);

      PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Register", null);
      PipeAccess.SendMessage(Common.ServerPipeName, _serverAddress, message.ToString());

      return true;
    }

    bool LearnIR(string fileName)
    {
      try
      {
        if (_learnIRFilename != null)
          return false;

        _learnIRFilename = fileName;
        
        PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Learn", null);
        PipeAccess.SendMessage(Common.ServerPipeName, _serverAddress, message.ToString());

        AddStatusLine("Learning");
      }
      catch (Exception ex)
      {
        AddStatusLine(ex.Message);
        return false;
      }

      return true;
    }
    bool BlastIR(string fileName, string port, string speed)
    {
      try
      {
        if (!File.Exists(fileName))
          return false;

        FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

        byte[] outData = new byte[8 + port.Length + speed.Length + file.Length];

        BitConverter.GetBytes(port.Length).CopyTo(outData, 0);
        Encoding.ASCII.GetBytes(port).CopyTo(outData, 4);
        BitConverter.GetBytes(speed.Length).CopyTo(outData, 4 + port.Length);
        Encoding.ASCII.GetBytes(speed).CopyTo(outData, 8 + port.Length);

        file.Read(outData, 8 + port.Length + speed.Length, (int)file.Length);
        file.Close();

        PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Blast", outData);
        PipeAccess.SendMessage(Common.ServerPipeName, _serverAddress, message.ToString());
      }
      catch (Exception ex)
      {
        AddStatusLine(ex.Message);
        return false;
      }

      return true;
    }
    void RemoteButtonPressed(string keyCode)
    {
      string text = String.Format(" - {0}", keyCode);

      this.Invoke(_AddStatusLine, new Object[] { text });
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

        ConnectToServer(comboBoxComputer.Text);
      }
      catch (Exception ex)
      {
        AddStatusLine(ex.Message);

        if (PipeAccess.ServerRunning)
          PipeAccess.StopServer();
      }
    }
    private void buttonDisconnect_Click(object sender, EventArgs e)
    {
      AddStatusLine("Disconnect");

      if (!PipeAccess.ServerRunning)
      {
        AddStatusLine(" - Not connected");
        return;
      }

      try
      {
        PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Unregister", null);
        PipeAccess.SendMessage(Common.ServerPipeName, _serverAddress, message.ToString());
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

      if (!_transceiverInfo.CanTransmit)
      {
        AddStatusLine(String.Format("Transceiver: \"{0}\" doesn't blast.", _transceiverInfo.Name));
        return;
      }

      if (BlastIR(TempIRFile, comboBoxPort.SelectedItem as string, comboBoxSpeed.SelectedItem as string))
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

      if (!_transceiverInfo.CanLearn)
      {
        AddStatusLine(String.Format("Transceiver: \"{0}\" doesn't support learning", _transceiverInfo.Name));
        return;
      }

      if (LearnIR(TempIRFile))
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
        PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Shutdown", null);
        PipeAccess.SendMessage(Common.ServerPipeName, _serverAddress, message.ToString());
      }
      catch (Exception ex)
      {
        AddStatusLine(ex.Message);
      }
    }
    private void buttonCrash_Click(object sender, EventArgs e)
    {
      throw new System.InvalidOperationException("User initiated exception thrown");
    }
    private void buttonListConnected_Click(object sender, EventArgs e)
    {
      AddStatusLine("List Clients");

      if (!PipeAccess.ServerRunning)
      {
        AddStatusLine(" - Not connected");
        return;
      }

      try
      {
        PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "List", null);
        PipeAccess.SendMessage(Common.ServerPipeName, _serverAddress, message.ToString());
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
        PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Ping", BitConverter.GetBytes(24));
        PipeAccess.SendMessage(Common.ServerPipeName, _serverAddress, message.ToString());
      }
      catch (Exception ex)
      {
        AddStatusLine(ex.Message);
      }
    }

    private void buttonSendCustom_Click(object sender, EventArgs e)
    {
      try
      {
        AddStatusLine("Sending custom message to server ...");

        if (PipeMessage.FromString(textBoxCustom.Text) == null)
          AddStatusLine("Warning: The specified custom message is not a valid message structure");

        PipeAccess.SendMessage(Common.ServerPipeName, _serverAddress, textBoxCustom.Text);

        AddStatusLine("Custom message sent");
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

        int keyCode = (int)Enum.Parse(typeof(MceButton), comboBoxRemoteButtons.SelectedItem.ToString());
        if (keyCode == -1)
          keyCode = Decimal.ToInt32(numericUpDownButton.Value);

        byte[] data = new byte[8];
        
        BitConverter.GetBytes(keyCode).CopyTo(data, 0);
        BitConverter.GetBytes(0).CopyTo(data, 4);

        PipeMessage message = new PipeMessage(_localPipeName, Environment.MachineName, "Forward Remote Button", data);
        PipeAccess.SendMessage(Common.ServerPipeName, _serverAddress, message.ToString());
      }
      catch (Exception ex)
      {
        AddStatusLine(ex.Message);
      }
    }

    private void buttonHelp_Click(object sender, EventArgs e)
    {
      RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\IR Server Suite\\");
      string installFolder = (string)registryKey.GetValue("Install_Dir", String.Empty);
      registryKey.Close();

      Help.ShowHelp(this, installFolder + "\\IR Server Suite.chm");
      // , HelpNavigator.Topic, "index.html"
    }

    #endregion Controls

  }

}
