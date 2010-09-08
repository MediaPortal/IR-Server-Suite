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
using System.Windows.Forms;
using IrssComms;
using IrssUtils;

namespace DebugClient
{
  internal partial class MainForm : Form
  {
    #region Enumerations

    /// <summary>
    /// A list of MCE remote buttons.
    /// </summary>
    public enum MceButton
    {
      Custom = -1,
      None = 0,
      TV_Power = 0x7b9a,
      Blue = 0x7ba1,
      Yellow = 0x7ba2,
      Green = 0x7ba3,
      Red = 0x7ba4,
      Teletext = 0x7ba5,
      Radio = 0x7baf,
      Print = 0x7bb1,
      Videos = 0x7bb5,
      Pictures = 0x7bb6,
      Recorded_TV = 0x7bb7,
      Music = 0x7bb8,
      TV = 0x7bb9,
      Guide = 0x7bd9,
      Live_TV = 0x7bda,
      DVD_Menu = 0x7bdb,
      Back = 0x7bdc,
      OK = 0x7bdd,
      Right = 0x7bde,
      Left = 0x7bdf,
      Down = 0x7be0,
      Up = 0x7be1,
      Star = 0x7be2,
      Hash = 0x7be3,
      Replay = 0x7be4,
      Skip = 0x7be5,
      Stop = 0x7be6,
      Pause = 0x7be7,
      Record = 0x7be8,
      Play = 0x7be9,
      Rewind = 0x7bea,
      Forward = 0x7beb,
      Channel_Down = 0x7bec,
      Channel_Up = 0x7bed,
      Volume_Down = 0x7bee,
      Volume_Up = 0x7bef,
      Info = 0x7bf0,
      Mute = 0x7bf1,
      Start = 0x7bf2,
      PC_Power = 0x7bf3,
      Enter = 0x7bf4,
      Escape = 0x7bf5,
      Number_9 = 0x7bf6,
      Number_8 = 0x7bf7,
      Number_7 = 0x7bf8,
      Number_6 = 0x7bf9,
      Number_5 = 0x7bfa,
      Number_4 = 0x7bfb,
      Number_3 = 0x7bfc,
      Number_2 = 0x7bfd,
      Number_1 = 0x7bfe,
      Number_0 = 0x7bff,
    }

    #endregion Enumerations

    #region Constructor

    public MainForm()
    {
      InitializeComponent();
    }

    #endregion

    #region Constants

    private static readonly string DebugIRFile = Path.Combine(Common.FolderIRCommands, "DebugClient.IR");

    #endregion

    #region Variables

    private Client _client;
    private IRServerInfo _irServerInfo = new IRServerInfo();

    private string _learnIRFilename;

    private bool _registered;
    private string _serverHost = "localhost";

    #endregion Variables

    private DelegateAddStatusLine _addStatusLine;

    private void AddStatusLine(string status)
    {
      IrssLog.Info(status);

      listBoxStatus.Items.Add(status);

      listBoxStatus.SetSelected(listBoxStatus.Items.Count - 1, true);
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      IrssLog.LogLevel = IrssLog.Level.Debug;
      IrssLog.Open("Debug Client.log");

      _addStatusLine = AddStatusLine;

      comboBoxPort.Items.Clear();
      comboBoxPort.Items.Add("None");
      comboBoxPort.SelectedIndex = 0;

      comboBoxComputer.Items.Clear();
      comboBoxComputer.Items.Add("localhost");

      List<string> networkPCs = Network.GetComputers(false);
      if (networkPCs != null)
        comboBoxComputer.Items.AddRange(networkPCs.ToArray());

      comboBoxComputer.Text = _serverHost;
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      buttonDisconnect_Click(null, null);

      _addStatusLine = null;

      IrssLog.Close();
    }

    private void ReceivedMessage(IrssMessage received)
    {
      Invoke(_addStatusLine,
             new Object[] {String.Format("Received Message: \"{0}, {1}\"", received.Type, received.Flags)});

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
            Invoke(_addStatusLine, new Object[] {received.GetDataAsString()});
            break;

          case MessageType.ActiveReceivers:
            Invoke(_addStatusLine, new Object[] {received.GetDataAsString()});
            break;

          case MessageType.RemoteEvent:
            string deviceName = received.MessageData[IrssMessage.DEVICE_NAME] as string;
            string keyCode = received.MessageData[IrssMessage.KEY_CODE] as string;

            Invoke(_addStatusLine, new Object[] {String.Format("{0} ({1})", deviceName, keyCode)});
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
            Invoke(_addStatusLine, new Object[] {received.GetDataAsString()});
            return;
        }
      }
      catch (Exception ex)
      {
        Invoke(_addStatusLine, new Object[] {ex.Message});
      }
    }

    private bool LearnIR(string fileName)
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

    private bool BlastIR(string fileName, string port)
    {
      try
      {
        using (FileStream file = File.OpenRead(fileName))
        {
          if (file.Length == 0)
            throw new IOException(String.Format("Cannot Blast. IR file \"{0}\" has no data, possible IR learn failure",
                                                fileName));

          byte[] outData = new byte[4 + port.Length + file.Length];

          BitConverter.GetBytes(port.Length).CopyTo(outData, 0);
          Encoding.ASCII.GetBytes(port).CopyTo(outData, 4);

          file.Read(outData, 4 + port.Length, (int) file.Length);

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

    private void RemoteHandlerCallback(string keyCode)
    {
      string text = String.Format("Remote Event \"{0}\"", keyCode);

      Invoke(_addStatusLine, new Object[] {text});
    }

    private void CommsFailure(object obj)
    {
      Exception ex = obj as Exception;

      if (ex != null)
        Invoke(_addStatusLine, new Object[] {String.Format("Communications failure: {0}", ex.Message)});
      else
        Invoke(_addStatusLine, new Object[] {"Communications failure"});

      StopClient();
    }

    private void Connected(object obj)
    {
      IrssLog.Info("Connected to server");

      IrssMessage message = new IrssMessage(MessageType.RegisterClient, MessageFlags.Request);
      _client.Send(message);
    }

    private void Disconnected(object obj)
    {
      IrssLog.Warn("Communications with server has been lost");

      Thread.Sleep(1000);
    }

    private bool StartClient(IPEndPoint endPoint)
    {
      if (_client != null)
        return false;

      ClientMessageSink sink = ReceivedMessage;

      _client = new Client(endPoint, sink);
      _client.CommsFailureCallback = CommsFailure;
      _client.ConnectCallback = Connected;
      _client.DisconnectCallback = Disconnected;

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

    private void StopClient()
    {
      if (_client == null)
        return;

      _client.Dispose();
      _client = null;

      _registered = false;
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
        IPEndPoint endPoint = new IPEndPoint(serverIP, Server.DefaultPort);

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

        byte[] deviceNameBytes = Encoding.ASCII.GetBytes(textBoxRemoteDevice.Text);
        byte[] keyCodeBytes = Encoding.ASCII.GetBytes(textBoxRemoteCode.Text);

        byte[] bytes = new byte[8 + deviceNameBytes.Length + keyCodeBytes.Length];

        BitConverter.GetBytes(deviceNameBytes.Length).CopyTo(bytes, 0);
        deviceNameBytes.CopyTo(bytes, 4);
        BitConverter.GetBytes(keyCodeBytes.Length).CopyTo(bytes, 4 + deviceNameBytes.Length);
        keyCodeBytes.CopyTo(bytes, 8 + deviceNameBytes.Length);

        IrssMessage message = new IrssMessage(MessageType.ForwardRemoteEvent, MessageFlags.Notify, bytes);
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
        string file = Path.Combine(SystemRegistry.GetInstallFolder(), "IR Server Suite.chm");
        Help.ShowHelp(this, file);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(this, ex.Message, "Failed to load help", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #endregion Controls

    #region Nested type: DelegateAddStatusLine

    private delegate void DelegateAddStatusLine(string status);

    #endregion

    private void MainForm_HelpRequested(object sender, HelpEventArgs hlpevent)
    {
      IrssHelp.Open(sender);
    }
  }
}