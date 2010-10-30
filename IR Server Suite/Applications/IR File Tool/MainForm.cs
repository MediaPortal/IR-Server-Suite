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
using System.Xml;
using IrssComms;
using IrssUtils;
using IrssUtils.Forms;

namespace IrFileTool
{
  /// <summary>
  /// IR File Tool Main Form.
  /// </summary>
  internal partial class MainForm : Form
  {
    #region Constants

    private static readonly string ConfigurationFolder = Path.Combine(Common.FolderAppData,
                                                                    "IR File Tool");
    private static readonly string ConfigurationFile = Path.Combine(ConfigurationFolder,
                                                                    "IR File Tool.xml");

    #endregion Constants

    #region Variables

    private Client _client;
    private IrCode _code = new IrCode();
    private string _fileName = String.Empty;
    private IRServerInfo _irServerInfo = new IRServerInfo();

    private bool _registered;

    private string _serverHost = String.Empty;

    #endregion Variables

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="MainForm"/> class.
    /// </summary>
    public MainForm()
    {
      InitializeComponent();

      SetImages();
      
      LoadSettings();
    }

    private void SetImages()
    {
      this.newToolStripMenuItem.Image = IrssUtils.Properties.Resources.NewDocument;
      this.openToolStripMenuItem.Image = IrssUtils.Properties.Resources.OpenDocument;
      this.saveToolStripMenuItem.Image = IrssUtils.Properties.Resources.Save;
      this.saveasToolStripMenuItem.Image = IrssUtils.Properties.Resources.SaveAs;

      this.connectToolStripMenuItem.Image = IrssUtils.Properties.Resources.Connect;
      this.disconnectToolStripMenuItem.Image = IrssUtils.Properties.Resources.Disconnect;
      this.changeServerToolStripMenuItem.Image = IrssUtils.Properties.Resources.ChangeServer;

      this.contentsToolStripMenuItem.Image = IrssUtils.Properties.Resources.Help;
      this.aboutToolStripMenuItem.Image = IrssUtils.Properties.Resources.Info;
    }

    #endregion Constructor

    private void RefreshForm()
    {
      if (String.IsNullOrEmpty(_fileName))
        Text = "IR File Tool";
      else
        Text = "IR File Tool - " + _fileName;

      textBoxPronto.Text = Encoding.ASCII.GetString(_code.ToByteArray());

      switch (_code.Carrier)
      {
        case IrCode.CarrierFrequencyDCMode:
          textBoxCarrier.Text = "DC Mode";
          break;

        case IrCode.CarrierFrequencyUnknown:
          textBoxCarrier.Text = "Unknown";
          break;

        default:
          textBoxCarrier.Text = _code.Carrier.ToString();
          break;
      }
    }

    private void Save()
    {
      if (String.IsNullOrEmpty(_fileName))
        if (!GetSaveFileName())
          return;

      if (!checkBoxStoreBinary.Checked)
      {
        Pronto.WriteProntoFile(_fileName, Pronto.ConvertIrCodeToProntoRaw(_code));
      }
      else
      {
        using (FileStream file = File.OpenWrite(_fileName))
        {
          byte[] fileBytes = DataPacket(_code);

          file.Write(fileBytes, 0, fileBytes.Length);
        }
      }
    }

    private bool GetSaveFileName()
    {
      if (String.IsNullOrEmpty(saveFileDialog.InitialDirectory))
        saveFileDialog.InitialDirectory = Common.FolderIRCommands;

      if (saveFileDialog.ShowDialog(this) != DialogResult.OK)
        return false;

      _fileName = saveFileDialog.FileName;
      return true;
    }


    private void LoadSettings()
    {
      XmlDocument doc = new XmlDocument();

      try
      {
        doc.Load(ConfigurationFile);
      }
      catch (DirectoryNotFoundException)
      {
        IrssLog.Warn("Configuration directory not found, using default settings");

        Directory.CreateDirectory(ConfigurationFolder);
        CreateDefaultSettings();
        return;
      }
      catch (FileNotFoundException)
      {
        IrssLog.Warn("Configuration file not found, using default settings");

        CreateDefaultSettings();
        return;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);

        CreateDefaultSettings();
        return;
      }

      try
      {
        _serverHost = doc.DocumentElement.Attributes["ServerHost"].Value;
      }
      catch
      {
        _serverHost = "localhost";
      }
    }

    private void SaveSettings()
    {
      try
      {
        using (XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, Encoding.UTF8))
        {
          writer.Formatting = Formatting.Indented;
          writer.Indentation = 1;
          writer.IndentChar = (char) 9;
          writer.WriteStartDocument(true);
          writer.WriteStartElement("settings"); // <settings>

          writer.WriteAttributeString("ServerHost", _serverHost);

          writer.WriteEndElement(); // </settings>
          writer.WriteEndDocument();
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
    }

    private void CreateDefaultSettings()
    {
      _serverHost = "localhost";

      SaveSettings();
    }


    private void UpdateWindow(string status)
    {
      toolStripStatusLabelConnected.Text = status;

      comboBoxPort.Items.Clear();
      comboBoxPort.Items.AddRange(_irServerInfo.Ports);
      comboBoxPort.SelectedIndex = 0;

      if (_registered)
      {
        groupBoxTestBlast.Enabled = _irServerInfo.CanTransmit;
        buttonLearn.Enabled = _irServerInfo.CanLearn;
      }
      else
      {
        groupBoxTestBlast.Enabled = false;
        buttonLearn.Enabled = false;
      }
    }


    private void CommsFailure(object obj)
    {
      Exception ex = obj as Exception;

      if (ex != null)
        IrssLog.Error("Communications failure: {0}", ex.Message);
      else
        IrssLog.Error("Communications failure");

      StopClient();

      MessageBox.Show(this, "Please report this error.", "IR File Tool - Communications failure", MessageBoxButtons.OK,
                      MessageBoxIcon.Error);
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
    }

    private void ReceivedMessage(IrssMessage received)
    {
      IrssLog.Debug("Received Message \"{0}\"", received.Type);

      try
      {
        switch (received.Type)
        {
          case MessageType.RegisterClient:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              _irServerInfo = IRServerInfo.FromBytes(received.GetDataAsBytes());
              _registered = true;

              string message = "Connected";
              IrssLog.Info(message);
              Invoke(new UpdateWindowDel(UpdateWindow), new string[] {message});
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              _registered = false;

              string message = "Failed to connect";
              IrssLog.Warn(message);
              Invoke(new UpdateWindowDel(UpdateWindow), new string[] {message});
            }
            return;

          case MessageType.BlastIR:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              string message = "Blast successful";
              IrssLog.Info(message);
              Invoke(new UpdateWindowDel(UpdateWindow), new string[] {message});
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              string message = "Failed to blast IR command";
              IrssLog.Error(message);
              Invoke(new UpdateWindowDel(UpdateWindow), new string[] {message});
            }
            break;

          case MessageType.LearnIR:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              byte[] dataBytes = received.GetDataAsBytes();

              _code = IrCode.FromByteArray(dataBytes);

              _fileName = null;

              string message = "Learned IR Successfully";
              IrssLog.Info(message);
              Invoke(new UpdateWindowDel(UpdateWindow), new string[] {message});

              RefreshForm();
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              string message = "Failed to learn IR command";

              IrssLog.Warn(message);
              Invoke(new UpdateWindowDel(UpdateWindow), new string[] {message});
            }
            else if ((received.Flags & MessageFlags.Timeout) == MessageFlags.Timeout)
            {
              string message = "Learn IR command timed-out";

              IrssLog.Warn(message);
              Invoke(new UpdateWindowDel(UpdateWindow), new string[] {message});
            }
            break;

          case MessageType.ServerShutdown:
            _registered = false;
            Invoke(new UpdateWindowDel(UpdateWindow), new string[] {"Server shut down"});
            return;

          case MessageType.Error:
            IrssLog.Error("Error from server: " + received.GetDataAsString());
            return;
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(this, ex.Message, "IR File Tool Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #region Menus

    private void newToolStripMenuItem_Click(object sender, EventArgs e)
    {
      _code = new IrCode();
      _fileName = null;

      RefreshForm();
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(openFileDialog.InitialDirectory))
        openFileDialog.InitialDirectory = Common.FolderIRCommands;

      if (openFileDialog.ShowDialog(this) != DialogResult.OK)
        return;

      using (FileStream file = File.OpenRead(openFileDialog.FileName))
      {
        if (file.Length == 0)
        {
          MessageBox.Show(this, "The selected file is empty", "Empty file", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return;
        }

        byte[] fileData = new byte[file.Length];
        file.Read(fileData, 0, (int) file.Length);

        IrCode newCode = IrCode.FromByteArray(fileData);
        if (newCode == null)
        {
          MessageBox.Show(this, "Not a valid IR code file", "Bad file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          return;
        }

        _code = newCode;
      }

      _fileName = openFileDialog.FileName;

      RefreshForm();
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Save();
    }

    private void saveasToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (GetSaveFileName())
        Save();
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Close();
    }


    private void connectToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Invoke(new UpdateWindowDel(UpdateWindow), new string[] { "Connecting ..." });

      IPAddress serverIP = Network.GetIPFromName(_serverHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, Server.DefaultPort);

      StartClient(endPoint);
    }

    private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (_registered)
      {
        IrssMessage message = new IrssMessage(MessageType.UnregisterClient, MessageFlags.Request);
        _client.Send(message);

        _registered = false;
      }

      Invoke(new UpdateWindowDel(UpdateWindow), new string[] { "Disconnected" });

      StopClient();
    }

    private void changeServerToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ServerAddress serverAddress = new ServerAddress(_serverHost);
      serverAddress.ShowDialog(this);

      _serverHost = serverAddress.ServerHost;

      SaveSettings();
    }
    
    
    private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      IrssHelp.Open(this);
    }

    private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      new AboutForm().ShowDialog();
    }

    #endregion

    private void MainForm_HelpRequested(object sender, HelpEventArgs hlpevent)
    {
      IrssHelp.Open(sender);
    }

    private void buttonAttemptDecode_Click(object sender, EventArgs e)
    {
      // NOTES:
      // All start with a correct RC6 0xFCA93 header.

      /*
      int[][] timingData = new int[][]
      {

new int[] { 2800, -750, 550, -350, 500, -400, 500, -800, 550, -800, 1400, -800, 550, -350, 500, -400, 500, -400, 500, -350, 500, -400, 500, -400, 500, -400, 500, -350, 500, -400, 500, -400, 950, -350, 500, -400, 500, -400, 500, -850, 450, -400, 500, -400, 500, -400, 6550, -850, 450, -400, 500, -400, 500, -850, 500, -800, 1400, -850, 500, -400, 450, -400, 500, -400, 500, -400, 500, -400, 450, -400, 500, -400, 500, -400, 500, -2800, 350, -400, 500, -400, 450, -450, 900, -25400 },        
        
new int[] { 2800, -750, 550, -350, 550, -350, 500, -800, 550, -800, 1400, -800, 550, -350, 550, -350, 500, -400, 500, -350, 550, -350, 500, -400, 500, -400, 500, -350, 550, -350, 500, -3750, 250, -400, 500, -800, 500, -400, 500, -400, 500, -400, 900, -850, 500, -400, 500, -400, 500, -350, 500, -400, 500, -400, 500, -400, 500, -350, 500, -400, 950, -31550 },

new int[] { 2700, -850, 500, -400, 450, -400, 500, -850, 500, -850, 1350, -850, 500, -400, 500, -400, 450, -400, 500, -400, 500, -400, 500, -400, 450, -400, 500, -400, 500, -400, 500, -400, 900, -400, 500, -400, 500, -400, 450, -450, 450, -850, 500, -400, 450, -31750 },

new int[] { 2700, -850, 450, -400, 500, -400, 500, -850, 450, -850, 1400, -850, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 1850, -400, 500, -400, 450, -450, 450, -450, 900, -25400 },

new int[] { 2700, -850, 450, -400, 500, -400, 450, -900, 450, -900, 1350, -850, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -5350, 450, -400, 500, -850, 450, -450, 450, -400, 500, -400, 900, -900, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 900, -31750 },

new int[] { 2700, -850, 450, -450, 450, -400, 500, -850, 450, -900, 1350, -850, 450, -450, 450, -450, 450, -400, 450, -450, 450, -450, 450, -450, 450, -400, 450, -450, 450, -450, 1100, -300, 450, -450, 450, -450, 450, -400, 500, -400, 900, -25400, },

new int[] { 2700, -850, 450, -450, 450, -400, 500, -850, 450, -900, 1350, -850, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -4400, 450, -450, 450, -450, 450, -900, 400, -450, 450, -450, 450, -400, 950, -850, 450, -450, 450, -450, 450, -400, 450, -450, 450, -450, 450, -450, 450, -400, 450, -31750 },

new int[] { 2650, -900, 450, -400, 500, -400, 450, -900, 450, -900, 1300, -900, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 900, -450, 450, -450, 450, -450, 450, -400, 500, -850, 5150, -31750 },

new int[] { 2650, -900, 450, -400, 500, -400, 450, -900, 450, -900, 1300, -900, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -950, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 900, -25400  },

new int[] { 2700, -900, 400, -450, 450, -450, 450, -900, 450, -850, 1350, -900, 450, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -3750, 250, -400, 500, -400, 450, -450, 450, -900, 450, -400, 500, -400, 450, -450, 900, -900, 450, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -31750 },

new int[] { 2700, -900, 400, -450, 450, -450, 450, -850, 450, -900, 1350, -900, 400, -450, 450, -450, 450, -450, 450, -400, 450, -450, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 900, -400, 500, -400, 450, -450, 450, -450, 450, -31750 },

new int[] { 2700, -850, 450, -450, 450, -450, 450, -850, 450, -900, 1350, -900, 400, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -450, 450, -400, 1000, -400, 450, -450, 450, -450, 450, -450, 450, -400, 500, -400, 900, -25400 },

new int[] { 2700, -850, 450, -400, 500, -400, 450, -900, 450, -900, 1350, -850, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -3550, 450, -450, 450, -450, 450, -400, 500, -850, 450, -450, 450, -400, 500, -400, 900, -900, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -31750 },

new int[] { 2700, -900, 450, -400, 450, -450, 450, -900, 450, -850, 1350, -900, 450, -400, 450, -450, 450, -450, 450, -450, 450, -400, 450, -450, 450, -450, 450, -450, 450, -400, 450, -450, 900, -450, 450, -400, 500, -400, 450, -450, 5300, -31750 },

new int[] { 2700, -850, 450, -450, 450, -450, 450, -900, 400, -900, 1350, -900, 400, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -12900, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 900, -25400 },

new int[] { 2700, -850, 450, -450, 450, -400, 450, -900, 450, -900, 1350, -850, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -2500, 650, -400, 450, -450, 450, -450, 450, -400, 500, -850, 450, -450, 450, -400, 500, -400, 900, -900, 450, -400, 500, -400, 450, -450, 450, -450, 450, -450, 450, -31750 },

new int[] { 2700, -850, 450, -400, 500, -400, 450, -900, 450, -900, 1350, -850, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 950, -400, 450, -450, 450, -450, 450, -400, 4650, -31750 },

new int[] { 2700, -900, 400, -450, 450, -450, 450, -900, 450, -850, 1350, -900, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -11950, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 900, -19050 },

new int[] { 2700, -850, 450, -450, 450, -400, 500, -850, 450, -900, 1350, -850, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 2400, -300, 900, -400, 500, -400, 450, -450, 450, -450, 450, -900, 400, -450, 450, -450, 450, -400, 950, -850, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 4700, -31750 },

new int[] { 2650, -900, 450, -400, 500, -400, 450, -900, 450, -900, 1300, -900, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 950, -400, 450, -450, 450, -450, 450, -31750 },

new int[] { 2700, -900, 450, -400, 450, -450, 450, -900, 450, -850, 1350, -900, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 5350, -31750 },

new int[] { 2700, -900, 400, -450, 450, -450, 450, -900, 450, -850, 1350, -900, 450, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -1300, 450, -450, 900, -450, 450, -400, 500, -400, 450, -450, 450, -900, 450, -400, 450, -450, 450, -450, 900, -900, 400, -450, 450, -450, 450, -450, 450, -400, 450, -31750 },

new int[] { 2700, -850, 450, -450, 450, -400, 500, -850, 450, -900, 1350, -850, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 900, -400, 500, -400, 450, -450, 450, -31750 },

new int[] { 2700, -850, 450, -400, 500, -400, 450, -900, 450, -900, 1300, -900, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 4700, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 900, -19050 },

new int[] { 2700, -900, 450, -400, 450, -450, 450, -900, 450, -850, 1350, -900, 450, -400, 500, -400, 450, -450, 450, -450, 450, 3350, 350, -900, 1350, -900, 450, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 900, -400, 500, -400, 450, -31750 },

new int[] { 2650, -900, 450, -400, 500, -400, 450, -900, 450, -900, 1300, -900, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 10650, -900, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 900, -19050 },

new int[] { 2700, -900, 450, -400, 450, -450, 450, -900, 450, -850, 1350, -900, 450, -400, 450, -450, 450, -450, 450, -450, 450, -400, 450, -450, 450, -450, 550, -350, 450, -400, 450, -450, 900, -450, 450, -400, 500, -400, 450, -450, 450, -900, 450, -400, 450, -450, 450, -450, 900, -900, 450, -400, 450, -450, 450, -450, 4700, -31750 },

new int[] { 2700, -850, 450, -450, 450, -400, 500, -850, 450, -900, 1350, -900, 400, -450, 450, -450, 450, -450, 450, -400, 450, -450, 450, -450, 450, -450, 450, -400, 450, -450, 450, -450, 900, -400, 500, -400, 450, -31750 },

new int[] { 2650, -900, 450, -400, 500, -400, 450, -900, 450, -900, 1300, -900, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 10650, -900, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 900, -19050 },

new int[] { 2700, -900, 450, -400, 450, -450, 450, -900, 450, -850, 1350, -900, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 900, -450, 450, -450, 450, -400, 500, -400, 450, -900, 450, -400, 500, -400, 450, -450, 900, -900, 450, -400, 450, -450, 450, -31750 },

new int[] { 2700, -900, 400, -450, 450, -450, 450, -900, 450, -850, 1350, -900, 450, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 900, -450, 450, -400, 3350, -31750 },

new int[] { 2650, -900, 450, -400, 450, -450, 450, -900, 450, -900, 1300, -900, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, 2400, 50, -450, 900, -900, 450, -400, 450, -450, 450, -450, 450, -450, 450, -400, 450, -450, 450, -450, 450, -450, 900, -19050 },

new int[] { 2700, -900, 400, -450, 450, -450, 450, -900, 450, -850, 1350, -900, 450, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 900, -450, 450, -400, 500, -400, 450, -450, 450, -900, 450, -400, 450, -450, 450, -450, 900, -900, 400, -450, 450, -450, 4300, -31750 },

new int[] { 2700, -900, 400, -450, 450, -450, 450, -900, 450, -850, 1350, -900, 450, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 900, -450, 3600, -31750 },

new int[] { 2700, -850, 450, -450, 450, -400, 500, -850, 450, -900, 1350, -850, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 2300, -200, 450, -400, 900, -900, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 900, -19050 },

new int[] { 2650, -900, 450, -400, 450, -450, 450, -900, 450, -900, 1300, -900, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 900, -450, 450, -450, 450, -400, 500, -400, 450, -900, 450, -400, 500, -400, 450, -450, 900, -900, 450, -400, 4650, -31750 },

new int[] { 2700, -900, 400, -450, 450, -450, 450, -900, 400, -900, 1350, -900, 450, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 450, -450, 450, -400, 500, -400, 450, -450, 900, -3500, 550, -25400 },

      };

      for (int index = 0; index < timingData.GetLength(0); index++)
      {
        IrDecoder.DecodeIR(timingData[index], new RemoteCallback(RemoteEvent), new KeyboardCallback(KeyboardEvent), new MouseCallback(MouseEvent));

        //IrCode newCode = new IrCode(timingData[index]);
        //Pronto.WriteProntoFile(String.Format("C:\\{0}.ir", index), Pronto.ConvertIrCodeToProntoRaw(newCode));
      }
      */
      IrDecoder.DecodeIR(_code.TimingData, RemoteEvent, KeyboardEvent, MouseEvent);
    }

    private static byte[] DataPacket(IrCode code)
    {
      if (code.TimingData.Length == 0)
        return null;

      // Construct data bytes into "packet" ...
      List<byte> packet = new List<byte>();

      for (int index = 0; index < code.TimingData.Length; index++)
      {
        double time = code.TimingData[index];

        byte duration = (byte) Math.Abs(Math.Round(time / 50));
        bool pulse = (time > 0);

        while (duration > 0x7F)
        {
          packet.Add((byte) (pulse ? 0xFF : 0x7F));

          duration -= 0x7F;
        }

        packet.Add((byte) (pulse ? 0x80 | duration : duration));
      }

      // Insert byte count markers into packet data bytes ...
      int subpackets = (int) Math.Ceiling(packet.Count / (double) 4);

      byte[] output = new byte[packet.Count + subpackets + 1];

      int outputPos = 0;

      for (int packetPos = 0; packetPos < packet.Count;)
      {
        byte copyCount = (byte) (packet.Count - packetPos < 4 ? packet.Count - packetPos : 0x04);

        output[outputPos++] = (byte) (0x80 | copyCount);

        for (int index = 0; index < copyCount; index++)
          output[outputPos++] = packet[packetPos++];
      }

      output[outputPos] = 0x80;

      return output;
    }

    private void RemoteEvent(IrProtocol codeType, uint keyCode, bool firstPress)
    {
      MessageBox.Show(this,
                      String.Format("Protocol: {0}\nCode: {1}", Enum.GetName(typeof (IrProtocol), codeType), keyCode),
                      "Decode IR", MessageBoxButtons.OK, MessageBoxIcon.Information);

      int newCarrier;
      switch (codeType)
      {
        case IrProtocol.Daewoo:
          newCarrier = 38000;
          break;
        case IrProtocol.JVC:
          newCarrier = 38000;
          break;
        case IrProtocol.Matsushita:
          newCarrier = 56800;
          break;
        case IrProtocol.Mitsubishi:
          newCarrier = 40000;
          break;
        case IrProtocol.NEC:
          newCarrier = 38000;
          break;
        case IrProtocol.NRC17:
          newCarrier = 38000;
          break;
        case IrProtocol.Panasonic:
          newCarrier = 38000;
          break;
        case IrProtocol.RC5:
          newCarrier = 36000;
          break;
        case IrProtocol.RC5X:
          newCarrier = 36000;
          break;
        case IrProtocol.RC6:
          newCarrier = 36000;
          break;
        case IrProtocol.RC6A:
          newCarrier = 36000;
          break;
        case IrProtocol.RC6_MCE:
          newCarrier = 36000;
          break;
        case IrProtocol.RC6_16:
          newCarrier = 36000;
          break;
        case IrProtocol.RC6_20:
          newCarrier = 36000;
          break;
        case IrProtocol.RC6_24:
          newCarrier = 36000;
          break;
        case IrProtocol.RC6_32:
          newCarrier = 36000;
          break;
        case IrProtocol.RCA:
          newCarrier = 56000;
          break;
        case IrProtocol.RCMM:
          newCarrier = 36000;
          break;
        case IrProtocol.RECS80:
          newCarrier = 38000;
          break;
        case IrProtocol.Sharp:
          newCarrier = 38000;
          break;
        case IrProtocol.SIRC:
          newCarrier = 40000;
          break;
        case IrProtocol.Toshiba:
          newCarrier = 38000;
          break;
        case IrProtocol.XSAT:
          newCarrier = 38000;
          break;

        default:
          return;
      }

      // If the current carrier frequency is within +- 50 hz then it's close enough.
      if (_code.Carrier >= newCarrier - 50 && _code.Carrier <= newCarrier + 50)
        return;

      if (DialogResult.Yes ==
          MessageBox.Show(this, String.Format("Use this protocol's carrier frequency ({0})?", newCarrier), "Decode IR",
                          MessageBoxButtons.YesNo, MessageBoxIcon.Question))
      {
        textBoxCarrier.Text = newCarrier.ToString();

        _code.Carrier = newCarrier;

        RefreshForm();
      }
    }

    private void KeyboardEvent(uint keyCode, uint modifiers)
    {
      MessageBox.Show(this, String.Format("Keyboard: {0}, {1}", keyCode, modifiers), "Decode IR", MessageBoxButtons.OK,
                      MessageBoxIcon.Information);
    }

    private void MouseEvent(int deltaX, int deltaY, bool right, bool left)
    {
      MessageBox.Show(this, String.Format("Mouse: DX {0}, DY {1}, Right: {2}, Left: {3}", deltaX, deltaY, right, left),
                      "Decode IR", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void buttonSetCarrier_Click(object sender, EventArgs e)
    {
      if (textBoxCarrier.Text == "Unknown")
        return;

      if (textBoxCarrier.Text == "DC Mode")
        _code.Carrier = IrCode.CarrierFrequencyDCMode;

      _code.Carrier = int.Parse(textBoxCarrier.Text);

      RefreshForm();
    }


    private void buttonBlast_Click(object sender, EventArgs e)
    {
      if (!_registered)
        MessageBox.Show(this, "Not registered to an active IR Server", "Cannot blast", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

      Invoke(new UpdateWindowDel(UpdateWindow), new string[] {"Blasting ..."});

      string port = comboBoxPort.Text;
      byte[] codeBytes = _code.ToByteArray();

      byte[] outData = new byte[4 + port.Length + codeBytes.Length];

      BitConverter.GetBytes(port.Length).CopyTo(outData, 0);
      Encoding.ASCII.GetBytes(port).CopyTo(outData, 4);

      Array.Copy(codeBytes, 0, outData, 4 + port.Length, codeBytes.Length);

      IrssMessage message = new IrssMessage(MessageType.BlastIR, MessageFlags.Request, outData);
      _client.Send(message);
    }

    private void buttonLearn_Click(object sender, EventArgs e)
    {
      if (!_registered)
        MessageBox.Show(this, "Not registered to an active IR Server", "Cannot learn", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

      Invoke(new UpdateWindowDel(UpdateWindow), new string[] {"Learning ..."});

      IrssMessage message = new IrssMessage(MessageType.LearnIR, MessageFlags.Request);
      _client.Send(message);
    }

    #region Nested type: UpdateWindowDel

    private delegate void UpdateWindowDel(string status);

    #endregion



  }
}