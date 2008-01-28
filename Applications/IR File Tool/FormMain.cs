using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
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
  partial class FormMain : Form
  {

    #region Constants

    static readonly string ConfigurationFile = Common.FolderAppData + "IR File Tool\\IR File Tool.xml";

    #endregion Constants

    #region Variables

    string _fileName = String.Empty;

    IrCode _code = new IrCode();


    Client _client;

    bool _registered;

    string _serverHost = String.Empty;


    #endregion Variables

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="FormMain"/> class.
    /// </summary>
    public FormMain()
    {
      InitializeComponent();

      LoadSettings();
    }

    #endregion Constructor


    void RefreshForm()
    {
      if (String.IsNullOrEmpty(_fileName))
        this.Text = "IR File Tool";
      else
        this.Text = "IR File Tool - " + _fileName;

      textBoxPronto.Text = Encoding.ASCII.GetString(_code.ToByteArray(true));

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

    void Save()
    {
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


    void LoadSettings()
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationFile);

        _serverHost = doc.DocumentElement.Attributes["ServerHost"].Value;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);

        _serverHost = "localhost";
      }
    }
    void SaveSettings()
    {
      try
      {
        using (XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, Encoding.UTF8))
        {
          writer.Formatting = Formatting.Indented;
          writer.Indentation = 1;
          writer.IndentChar = (char)9;
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



    void CommsFailure(object obj)
    {
      Exception ex = obj as Exception;

      if (ex != null)
        IrssLog.Error("Communications failure: {0}", ex.Message);
      else
        IrssLog.Error("Communications failure");

      StopClient();

      MessageBox.Show(this, "Please report this error.", "IR File Tool - Communications failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
      _client.CommsFailureCallback = new WaitCallback(CommsFailure);
      _client.ConnectCallback = new WaitCallback(Connected);
      _client.DisconnectCallback = new WaitCallback(Disconnected);

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

    void ReceivedMessage(IrssMessage received)
    {
      IrssLog.Debug("Received Message \"{0}\"", received.Type);

      try
      {
        switch (received.Type)
        {
          case MessageType.RegisterClient:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              _registered = true;

              groupBoxTestBlast.Enabled = true;
              buttonLearn.Enabled = true;
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              _registered = false;

              groupBoxTestBlast.Enabled = false;
              buttonLearn.Enabled = false;

              MessageBox.Show(this, "Failed to register with server", "IR File Tool Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return;

          case MessageType.ServerShutdown:
            MessageBox.Show(this, "Server has been shut down", "IR File Tool Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;

          case MessageType.Error:
            MessageBox.Show(this, received.GetDataAsString(), "IR File Tool Error from Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(this, ex.Message, "IR File Tool Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }









    private void newToolStripMenuItem_Click(object sender, EventArgs e)
    {
      _code = new IrCode();
      _fileName = "New File.IR";

      RefreshForm();
    }
    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (openFileDialog.ShowDialog(this) != DialogResult.OK)
        return;

      if (String.IsNullOrEmpty(openFileDialog.InitialDirectory))
        openFileDialog.InitialDirectory = Common.FolderIRCommands;

      using (FileStream file = File.OpenRead(openFileDialog.FileName))
      {
        if (file.Length == 0)
        {
          MessageBox.Show(this, "The selected file is empty", "Empty file", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return;
        }

        byte[] fileData = new byte[file.Length];

        file.Read(fileData, 0, (int)file.Length);

        _code = IrCode.FromByteArray(fileData);
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
      if (String.IsNullOrEmpty(saveFileDialog.InitialDirectory))
        saveFileDialog.InitialDirectory = Common.FolderIRCommands;

      if (saveFileDialog.ShowDialog(this) != DialogResult.OK)
        return;

      _fileName = saveFileDialog.FileName;

      Save();
    }
    private void quitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Close();
    }
    private void buttonAttemptDecode_Click(object sender, EventArgs e)
    {
      IrDecoder.DecodeIR(_code.TimingData, new RemoteCallback(RemoteEvent), new KeyboardCallback(KeyboardEvent), new MouseCallback(MouseEvent));
    }

    static byte[] DataPacket(IrCode code)
    {
      if (code.TimingData.Length == 0)
        return null;

      // Construct data bytes into "packet" ...
      List<byte> packet = new List<byte>();

      for (int index = 0; index < code.TimingData.Length; index++)
      {
        double time = (double)code.TimingData[index];

        byte duration = (byte)Math.Abs(Math.Round(time / 50));
        bool pulse = (time > 0);

        while (duration > 0x7F)
        {
          packet.Add((byte)(pulse ? 0xFF : 0x7F));

          duration -= 0x7F;
        }

        packet.Add((byte)(pulse ? 0x80 | duration : duration));
      }

      // Insert byte count markers into packet data bytes ...
      int subpackets = (int)Math.Ceiling(packet.Count / (double)4);

      byte[] output = new byte[packet.Count + subpackets + 1];

      int outputPos = 0;

      for (int packetPos = 0; packetPos < packet.Count; )
      {
        byte copyCount = (byte)(packet.Count - packetPos < 4 ? packet.Count - packetPos : 0x04);

        output[outputPos++] = (byte)(0x80 | copyCount);

        for (int index = 0; index < copyCount; index++)
          output[outputPos++] = packet[packetPos++];
      }

      output[outputPos] = 0x80;

      return output;
    }

    void RemoteEvent(IrProtocol codeType, uint keyCode, bool firstPress)
    {
      MessageBox.Show(this, String.Format("Remote: {0}, {1}", Enum.GetName(typeof(IrProtocol), codeType), keyCode), "Decode IR", MessageBoxButtons.OK, MessageBoxIcon.Information);

      if (textBoxCarrier.Text.Equals("Unknown", StringComparison.OrdinalIgnoreCase))
      {
        switch (codeType)
        {
          case IrProtocol.Daewoo: textBoxCarrier.Text = "38000"; break;
          case IrProtocol.JVC: textBoxCarrier.Text = "38000"; break;
          case IrProtocol.Matsushita: textBoxCarrier.Text = "56800"; break;
          case IrProtocol.Mitsubishi: textBoxCarrier.Text = "40000"; break;
          case IrProtocol.NEC: textBoxCarrier.Text = "38000"; break;
          case IrProtocol.NRC17: textBoxCarrier.Text = "38000"; break;
          case IrProtocol.Panasonic: textBoxCarrier.Text = "38000"; break;
          case IrProtocol.RC5: textBoxCarrier.Text = "36000"; break;
          case IrProtocol.RC5X: textBoxCarrier.Text = "36000"; break;
          case IrProtocol.RC6: textBoxCarrier.Text = "36000"; break;
          case IrProtocol.RC6A: textBoxCarrier.Text = "36000"; break;
          case IrProtocol.RC6_MCE: textBoxCarrier.Text = "36000"; break;
          case IrProtocol.RC6_Foxtel: textBoxCarrier.Text = "36000"; break;
          case IrProtocol.RCA: textBoxCarrier.Text = "56000"; break;
          case IrProtocol.RCMM: textBoxCarrier.Text = "36000"; break;
          case IrProtocol.RECS80: textBoxCarrier.Text = "38000"; break;
          case IrProtocol.Sharp: textBoxCarrier.Text = "38000"; break;
          case IrProtocol.SIRC: textBoxCarrier.Text = "40000"; break;
          case IrProtocol.Toshiba: textBoxCarrier.Text = "38000"; break;
          case IrProtocol.XSAT: textBoxCarrier.Text = "38000"; break;

          default:
            return;
        }

        _code.Carrier = int.Parse(textBoxCarrier.Text);

        RefreshForm();
      }
    }
    void KeyboardEvent(uint keyCode, uint modifiers)
    {
      MessageBox.Show(this, String.Format("Keyboard: {0}, {1}", keyCode, modifiers), "Decode IR", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    void MouseEvent(int deltaX, int deltaY, bool right, bool left)
    {
      MessageBox.Show(this, String.Format("Mouse: DX {0}, DY {1}, Right: {2}, Left: {3}", deltaX, deltaY, right, left), "Decode IR", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

    private void connectToolStripMenuItem_Click(object sender, EventArgs e)
    {
      IPAddress serverIP = Client.GetIPFromName(_serverHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, IrssComms.Server.DefaultPort);

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

      groupBoxTestBlast.Enabled = false;
      buttonLearn.Enabled = false;

      StopClient();
    }
    private void changeServerToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ServerAddress serverAddress = new ServerAddress(_serverHost);
      serverAddress.ShowDialog(this);

      _serverHost = serverAddress.ServerHost;

      SaveSettings();
    }

    private void buttonBlast_Click(object sender, EventArgs e)
    {

    }

    private void buttonLearn_Click(object sender, EventArgs e)
    {

    }


  }

}
