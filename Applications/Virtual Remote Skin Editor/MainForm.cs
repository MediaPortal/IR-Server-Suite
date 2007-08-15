using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using NamedPipes;
using IrssUtils;

namespace SkinEditor
{

  public partial class MainForm : Form
  {

    #region Constants

    static readonly string ConfigurationFile = Common.FolderAppData + "Virtual Remote Skin Editor\\Virtual Remote Skin Editor.xml";

    #endregion Constants

    #region Variables

    //static bool _keepAlive = true;
    static int _echoID = -1;

    string _serverHost = String.Empty;
    string _localPipeName = String.Empty;

    bool _registered = false;

    bool _unsavedChanges = false;
    //bool _fileOpen = false;
    string _fileName = String.Empty;

    Label _currentButton = null;

    Point _mouseOffset;
    bool _isMouseDown = false;

    #endregion Variables

    #region Constructor

    public MainForm()
    {
      InitializeComponent();

      LoadSettings();

      _currentButton = new Label();
      _currentButton.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left));
      _currentButton.BorderStyle = BorderStyle.FixedSingle;
      _currentButton.BackColor = Color.Transparent;
      _currentButton.Cursor = Cursors.Cross;
      _currentButton.Name = "Button";
      _currentButton.TabIndex = 3;
      //_currentButton.Text = "Hello";
      _currentButton.Visible = false;
      _currentButton.AutoSize = false;

      _currentButton.Location = new System.Drawing.Point(0, 0);
      _currentButton.Size = new System.Drawing.Size(0, 0);

      _currentButton.MouseDown += new MouseEventHandler(Button_MouseDown);
      _currentButton.MouseUp += new MouseEventHandler(Button_MouseUp);
      _currentButton.MouseMove += new MouseEventHandler(Button_MouseMove);

      pictureBoxRemote.Controls.Add(_currentButton);

      timerFlash.Start();
    }

    #endregion Constructor

    void Button_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        _mouseOffset = new Point(_currentButton.Left - Cursor.Position.X, _currentButton.Top - Cursor.Position.Y);
        _isMouseDown = true;
      }
    }
    void Button_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        _isMouseDown = false;

        UpdateWindowTitle();
      }
    }
    void Button_MouseMove(object sender, MouseEventArgs e)
    {
      if (_isMouseDown)
      {
        Point mousePos = Control.MousePosition;
        mousePos.Offset(_mouseOffset.X, _mouseOffset.Y);
        _currentButton.Location = mousePos;
        listViewButtons.SelectedItems[0].SubItems[3].Text = mousePos.Y.ToString();
        listViewButtons.SelectedItems[0].SubItems[4].Text = mousePos.X.ToString();

        _unsavedChanges = true;
      }
    }

    void UpdateWindowTitle()
    {
      if (String.IsNullOrEmpty(_fileName))      
      {
        this.Text = ProductName;
      }
      else
      {
        if (_unsavedChanges)
          this.Text = String.Format("{0} - {1} *", ProductName, Path.GetFileName(_fileName));
        else
          this.Text = String.Format("{0} - {1}", ProductName, Path.GetFileName(_fileName));
      }
    }

    void LoadSkinFile(string fileName)
    {
      IrssLog.Info("Loading Skin: {0}", fileName);
      
      XmlDocument doc = new XmlDocument();
      doc.Load(fileName);

      listViewButtons.Items.Clear();

      XmlNodeList commandSequence = doc.DocumentElement.SelectNodes("button");
      foreach (XmlNode item in commandSequence)
      {
        ListViewItem temp = new ListViewItem();
        temp.Text = item.Attributes["name"].Value;
        
        temp.SubItems.Add(item.Attributes["code"].Value);
        temp.SubItems.Add(item.Attributes["shortcut"].Value);
        temp.SubItems.Add(item.Attributes["top"].Value);
        temp.SubItems.Add(item.Attributes["left"].Value);
        temp.SubItems.Add(item.Attributes["height"].Value);
        temp.SubItems.Add(item.Attributes["width"].Value);
        
        listViewButtons.Items.Add(temp);
      }

      IrssLog.Info("Loaded Skin: {0}", fileName);
    }
    void SaveSkinFile(string fileName)
    {
      IrssLog.Info("Saving Skin: {0}", fileName);

      XmlTextWriter writer = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);
      writer.Formatting = Formatting.Indented;
      writer.Indentation = 1;
      writer.IndentChar = (char)9;
      writer.WriteStartDocument(true);
      writer.WriteStartElement("skin"); // <skin>

      foreach (ListViewItem item in listViewButtons.Items)
      {
        writer.WriteStartElement("button");
        writer.WriteAttributeString("name", item.Text);
        writer.WriteAttributeString("code", item.SubItems[1].Text);
        writer.WriteAttributeString("shortcut", item.SubItems[2].Text);
        writer.WriteAttributeString("top", item.SubItems[3].Text);
        writer.WriteAttributeString("left", item.SubItems[4].Text);
        writer.WriteAttributeString("height", item.SubItems[5].Text);
        writer.WriteAttributeString("width", item.SubItems[6].Text);
        writer.WriteEndElement(); // </button>
      }

      writer.WriteEndElement(); // </skin>

      writer.Close();

      IrssLog.Info("Saved Skin: {0}", fileName);
    }

    void LoadImage(string fileName)
    {
      IrssLog.Info("Loading Image: {0}", fileName);

      pictureBoxRemote.Image = new Bitmap(fileName);
      pictureBoxRemote.Height = pictureBoxRemote.Image.Height;
      pictureBoxRemote.Width = pictureBoxRemote.Image.Width;
      pictureBoxRemote.Visible = true;

      IrssLog.Info("Loaded Image: {0}", fileName);
    }

    void OpenFile()
    {
      if (openFileDialog.ShowDialog(this) == DialogResult.OK)
      {
        _fileName = openFileDialog.FileName;

        if (Path.GetExtension(_fileName) != ".xml")
          return;

        LoadSkinFile(_fileName);

        string imageFile = Path.ChangeExtension(_fileName, ".png");
        if (File.Exists(imageFile))
          LoadImage(imageFile);

        _unsavedChanges = false;

        UpdateWindowTitle();
      }
    }
    void NewFile()
    {
      _fileName = String.Empty;
      _currentButton.Visible = false;
      pictureBoxRemote.Image = null;
      pictureBoxRemote.Visible = false;
      listViewButtons.Items.Clear();
      _unsavedChanges = false;

      UpdateWindowTitle();
    }

    void HighlightSelectedButton(ListViewItem item)
    {
      int Top = int.Parse(item.SubItems[3].Text);
      int Left = int.Parse(item.SubItems[4].Text);
      int Height = int.Parse(item.SubItems[5].Text);
      int Width = int.Parse(item.SubItems[6].Text);

      _currentButton.Location = new System.Drawing.Point(Left, Top);
      _currentButton.Size = new System.Drawing.Size(Width, Height);
      _currentButton.BackColor = Color.Yellow;
      _currentButton.Visible = true;
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
        IrssLog.Error(ex.ToString());

        _serverHost = String.Empty;
      }
    }
    void SaveSettings()
    {
      try
      {
        XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, System.Text.Encoding.UTF8);
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 1;
        writer.IndentChar = (char)9;
        writer.WriteStartDocument(true);
        writer.WriteStartElement("settings"); // <settings>

        writer.WriteAttributeString("ServerHost", _serverHost);

        writer.WriteEndElement(); // </settings>
        writer.WriteEndDocument();
        writer.Close();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
      }
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      // TODO: Change log level to info for release.
      IrssLog.LogLevel = IrssLog.Level.Debug;
      IrssLog.Open(Common.FolderIrssLogs + "Virtual Remote Skin Editor.log");

      UpdateWindowTitle();

      comboBoxShortcut.Items.Clear();
      comboBoxShortcut.Items.AddRange(Enum.GetNames(typeof(Keys)));
    }

    private void buttonLoadImage_Click(object sender, EventArgs e)
    {
      if (openFileDialog.ShowDialog(this) == DialogResult.OK)
      {
        if (openFileDialog.FileName.EndsWith("*.xml"))
          return;

        LoadImage(openFileDialog.FileName);
      }
    }

    private void newToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (_unsavedChanges)
      {
        if (MessageBox.Show(this, "Are you sure you want to start a new file?", "Unsaved file in memory", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
          return;
      }

      NewFile();
    }
    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (_unsavedChanges)
      {
        if (MessageBox.Show(this, "Are you sure you want to close this file and open another?", "Unsaved file in memory", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
          return;
      }

      OpenFile();
    }
    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SaveSkinFile(_fileName);
      _unsavedChanges = false;
      UpdateWindowTitle();
    }
    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (saveFileDialog.ShowDialog(this) != DialogResult.OK)
        return;

      _fileName = saveFileDialog.FileName;

      SaveSkinFile(_fileName);
      _unsavedChanges = false;
      UpdateWindowTitle();
    }
    private void closeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (_unsavedChanges)
      {
        if (MessageBox.Show(this, "Are you sure you want to close this file?", "Unsaved file in memory", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
          return;
      }

      NewFile();
    }
    private void quitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (_unsavedChanges)
      {
        if (MessageBox.Show(this, "Are you sure you want to quit?", "Unsaved file in memory", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
          return;
      }

      Application.Exit();
    }

    private void listViewButtons_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (listViewButtons.SelectedItems.Count == 1)
        HighlightSelectedButton(listViewButtons.SelectedItems[0]);
    }

    private void timerFlash_Tick(object sender, EventArgs e)
    {
      if (_currentButton.BackColor == Color.Transparent)
        _currentButton.BackColor = Color.Red;
      else
        _currentButton.BackColor = Color.Transparent;
    }

    private void listViewButtons_KeyDown(object sender, KeyEventArgs e)
    {
      int val;

      switch (e.KeyCode)
      {
        case Keys.Delete:
          listViewButtons.Items.Remove(listViewButtons.SelectedItems[0]);
          _unsavedChanges = true;
          UpdateWindowTitle();
          e.SuppressKeyPress = true;
          break;

        case Keys.Add:
          _unsavedChanges = true;
          UpdateWindowTitle();
          e.SuppressKeyPress = true;
          break;

        case Keys.W:
          val = int.Parse(listViewButtons.SelectedItems[0].SubItems[5].Text);
          if (val > 3)
          {
            listViewButtons.SelectedItems[0].SubItems[5].Text = (val - 1).ToString();
            HighlightSelectedButton(listViewButtons.SelectedItems[0]);
            _unsavedChanges = true;
            UpdateWindowTitle();
          }
          e.SuppressKeyPress = true;
          break;
        
        case Keys.S:
          val = int.Parse(listViewButtons.SelectedItems[0].SubItems[5].Text);
          if (val < 1024)
          {
            listViewButtons.SelectedItems[0].SubItems[5].Text = (val + 1).ToString();
            HighlightSelectedButton(listViewButtons.SelectedItems[0]);
            _unsavedChanges = true;
            UpdateWindowTitle();
          }
          e.SuppressKeyPress = true;
          break;

        case Keys.A:
          val = int.Parse(listViewButtons.SelectedItems[0].SubItems[6].Text);
          if (val > 3)
          {
            listViewButtons.SelectedItems[0].SubItems[6].Text = (val - 1).ToString();
            HighlightSelectedButton(listViewButtons.SelectedItems[0]);
            _unsavedChanges = true;
            UpdateWindowTitle();
          }
          e.SuppressKeyPress = true;
          break;

        case Keys.D:
          val = int.Parse(listViewButtons.SelectedItems[0].SubItems[6].Text);
          if (val < 1024)
          {
            listViewButtons.SelectedItems[0].SubItems[6].Text = (val + 1).ToString();
            HighlightSelectedButton(listViewButtons.SelectedItems[0]);
            _unsavedChanges = true;
            UpdateWindowTitle();
          }
          e.SuppressKeyPress = true;
          break;
      }
      
    }

    private void buttonAddButton_Click(object sender, EventArgs e)
    {
      listViewButtons.Items.Add(new ListViewItem(new string[] {
        "Button", "X", "None", "0", "0", "10", "10" }));

      _unsavedChanges = true;
      UpdateWindowTitle();
    }
    private void buttonSetCode_Click(object sender, EventArgs e)
    {
      if (listViewButtons.SelectedItems.Count == 1)
        listViewButtons.SelectedItems[0].SubItems[1].Text = textBoxCode.Text;
    }
    private void buttonSetShortcut_Click(object sender, EventArgs e)
    {
      if (listViewButtons.SelectedItems.Count == 1)
        listViewButtons.SelectedItems[0].SubItems[2].Text = comboBoxShortcut.Text;
    }

    private void connectToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Connect();
    }
    private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Disconnect();
    }
    private void changeServerToolStripMenuItem_Click(object sender, EventArgs e)
    {
      IrssUtils.Forms.ServerAddress serverAddress = new IrssUtils.Forms.ServerAddress(Environment.MachineName);
      serverAddress.ShowDialog(this);
      _serverHost = serverAddress.ServerHost;

      SaveSettings();
    }

    void Connect()
    {
      if (_registered)
        return;

      IrssUtils.Forms.ServerAddress serverAddress;

      if (String.IsNullOrEmpty(_serverHost))
      {
        serverAddress = new IrssUtils.Forms.ServerAddress(Environment.MachineName);
        serverAddress.ShowDialog(this);
        _serverHost = serverAddress.ServerHost;
      }

      while (!ConnectToServer(_serverHost))
      {
        if (DialogResult.Retry == MessageBox.Show(this, "Could not connect to server", "Virtual Remote Skin Editor Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error))
        {
          serverAddress = new IrssUtils.Forms.ServerAddress(_serverHost);
          serverAddress.ShowDialog(this);
          _serverHost = serverAddress.ServerHost;
        }
        else
        {
          return;
        }
      }
    }
    void Disconnect()
    {
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

    void ReceivedMessage(string message)
    {
      PipeMessage received = PipeMessage.FromString(message);

      IrssLog.Debug("Received Message \"{0}\"", received.Name);

      try
      {
        switch (received.Name)
        {
          case "Keyboard Event":
          case "Mouse Event":
            break;

          case "Remote Event":
            if (listViewButtons.SelectedItems.Count == 1)
              listViewButtons.SelectedItems[0].SubItems[1].Text = Encoding.ASCII.GetString(received.Data);
            return;

          case "Register Success":
            {
              _registered = true;
              return;
            }

          case "Register Failure":
            {
              PipeAccess.StopServer();
              MessageBox.Show(this, "Failed to register with server", "Virtual Remote Skin Editor Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
              Application.Exit();
              return;
            }

          case "Server Shutdown":
            {
              PipeAccess.StopServer();
              MessageBox.Show(this, "Server has been shut down", "Virtual Remote Skin Editor Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
              Application.Exit();
              return;
            }

          case "Echo":
            {
              _echoID = BitConverter.ToInt32(received.Data, 0);
              break;
            }

          case "Error":
            {
              MessageBox.Show(this, Encoding.ASCII.GetString(received.Data), "Virtual Remote Skin Editor Error from Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
              return;
            }

          default:
            {
              MessageBox.Show(this, "Unknown message received from server: " + received.Name, "Virtual Remote Skin Editor Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
              return;
            }
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Virtual Remote Skin Editor Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    bool ConnectToServer(string serverHost)
    {
      bool retry = false;
      int pipeNumber = 1;
      string localPipeTest;

      try
      {

        do
        {
          localPipeTest = String.Format(Common.LocalPipeFormat, pipeNumber);

          if (PipeAccess.PipeExists(String.Format("\\\\.\\pipe\\{0}", localPipeTest)))
          {
            pipeNumber++;
            if (pipeNumber <= Common.MaximumLocalClientCount)
            {
              retry = true;
            }
            else
            {
              MessageBox.Show(this, "Maximum local client limit reached", "Virtual Remote Skin Editor Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        PipeAccess.SendMessage(Common.ServerPipeName, serverHost, message.ToString());

      }
      catch
      {
        return false;
      }

      return true;
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      Disconnect();

      IrssLog.Close();
    }

    private void helpToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        Help.ShowHelp(this, SystemRegistry.GetInstallFolder() + "\\IR Server Suite.chm", HelpNavigator.Topic, "Virtual Remote\\Skin Editor\\index.html");
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Failed to load help", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

  }

}
