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
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using IrssComms;
using IrssUtils;
using IrssUtils.Forms;

namespace SkinEditor
{
  /// <summary>
  /// Virtual Remote Skin Editor Main Form.
  /// </summary>
  internal partial class MainForm : Form
  {
    #region Constants

    private static readonly string ConfigurationFolder = Path.Combine(Common.FolderAppData,
                                                                    "Virtual Remote");
    private static readonly string ConfigurationFile = Path.Combine(ConfigurationFolder,
                                                                    "Virtual Remote Skin Editor.xml");

    #endregion Constants

    #region Variables

    private readonly Label _currentButton;
    private Client _client;
    private string _fileName = String.Empty;
    private bool _isMouseDown;
    private Point _mouseOffset;

    private bool _registered;

    private string _serverHost = String.Empty;

    private bool _unsavedChanges;
    //bool _fileOpen;

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

      _currentButton = new Label();
      _currentButton.Anchor = ((AnchorStyles.Top | AnchorStyles.Left));
      _currentButton.BorderStyle = BorderStyle.FixedSingle;
      _currentButton.BackColor = Color.Transparent;
      _currentButton.Cursor = Cursors.Cross;
      _currentButton.Name = "Button";
      _currentButton.TabIndex = 3;
      //_currentButton.Text = "Hello";
      _currentButton.Visible = false;
      _currentButton.AutoSize = false;

      _currentButton.Location = new Point(0, 0);
      _currentButton.Size = new Size(0, 0);

      _currentButton.MouseDown += Button_MouseDown;
      _currentButton.MouseUp += Button_MouseUp;
      _currentButton.MouseMove += Button_MouseMove;

      pictureBoxRemote.Controls.Add(_currentButton);

      timerFlash.Start();
    }

    private void SetImages()
    {
      this.newToolStripMenuItem.Image = IrssUtils.Properties.Resources.NewDocument;
      this.openToolStripMenuItem.Image = IrssUtils.Properties.Resources.OpenDocument;
      this.closeToolStripMenuItem.Image = IrssUtils.Properties.Resources.CloseDocument;
      this.saveToolStripMenuItem.Image = IrssUtils.Properties.Resources.Save;
      this.saveAsToolStripMenuItem.Image = IrssUtils.Properties.Resources.SaveAs;

      this.connectToolStripMenuItem.Image = IrssUtils.Properties.Resources.Connect;
      this.disconnectToolStripMenuItem.Image = IrssUtils.Properties.Resources.Disconnect;
      this.changeServerToolStripMenuItem.Image = IrssUtils.Properties.Resources.ChangeServer;

      this.contentsToolStripMenuItem.Image = IrssUtils.Properties.Resources.Help;
      this.aboutToolStripMenuItem.Image = IrssUtils.Properties.Resources.Info;
    }

    #endregion Constructor

    private void Button_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        _mouseOffset = new Point(_currentButton.Left - Cursor.Position.X, _currentButton.Top - Cursor.Position.Y);
        _isMouseDown = true;
      }
    }

    private void Button_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        _isMouseDown = false;

        UpdateWindowTitle();
      }
    }

    private void Button_MouseMove(object sender, MouseEventArgs e)
    {
      if (_isMouseDown)
      {
        Point mousePos = MousePosition;
        mousePos.Offset(_mouseOffset.X, _mouseOffset.Y);
        _currentButton.Location = mousePos;
        listViewButtons.SelectedItems[0].SubItems[3].Text = mousePos.Y.ToString();
        listViewButtons.SelectedItems[0].SubItems[4].Text = mousePos.X.ToString();

        _unsavedChanges = true;
      }
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      UpdateWindowTitle();

      comboBoxShortcut.Items.Clear();
      comboBoxShortcut.Items.AddRange(Enum.GetNames(typeof (Keys)));
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      StopClient();
    }



    #region Controls

    private void buttonLoadImage_Click(object sender, EventArgs e)
    {
      if (openFileDialog.ShowDialog(this) == DialogResult.OK)
      {
        if (openFileDialog.FileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
          return;

        LoadImage(openFileDialog.FileName);
      }
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
      listViewButtons.Items.Add(new ListViewItem(new string[]
                                                   {
                                                     "Button", "X", "None", "0", "0", "10", "10"
                                                   }));

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

    #endregion Controls

    #region Menus

    private void newToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (_unsavedChanges)
      {
        if (
          MessageBox.Show(this, "Are you sure you want to start a new file?", "Unsaved file in memory",
                          MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
          return;
      }

      NewFile();
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (_unsavedChanges)
      {
        if (
          MessageBox.Show(this, "Are you sure you want to close this file and open another?", "Unsaved file in memory",
                          MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
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
        if (
          MessageBox.Show(this, "Are you sure you want to close this file?", "Unsaved file in memory",
                          MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
          return;
      }

      NewFile();
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (_unsavedChanges)
      {
        if (
          MessageBox.Show(this, "Are you sure you want to quit?", "Unsaved file in memory", MessageBoxButtons.YesNo,
                          MessageBoxIcon.Warning) == DialogResult.No)
          return;
      }

      Application.Exit();
    }


    private void connectToolStripMenuItem_Click(object sender, EventArgs e)
    {
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

    #region Implementation

    private void UpdateWindowTitle()
    {
      if (String.IsNullOrEmpty(_fileName))
      {
        Text = ProductName;
      }
      else
      {
        if (_unsavedChanges)
          Text = String.Format("{0} - {1} *", ProductName, Path.GetFileName(_fileName));
        else
          Text = String.Format("{0} - {1}", ProductName, Path.GetFileName(_fileName));
      }
    }

    private void LoadSkinFile(string fileName)
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

    private void SaveSkinFile(string fileName)
    {
      IrssLog.Info("Saving Skin: {0}", fileName);

      using (XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.UTF8))
      {
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 1;
        writer.IndentChar = (char) 9;
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
      }

      IrssLog.Info("Saved Skin: {0}", fileName);
    }

    private void LoadImage(string fileName)
    {
      IrssLog.Info("Loading Image: {0}", fileName);

      pictureBoxRemote.Image = new Bitmap(fileName);
      pictureBoxRemote.Height = pictureBoxRemote.Image.Height;
      pictureBoxRemote.Width = pictureBoxRemote.Image.Width;
      pictureBoxRemote.Visible = true;

      IrssLog.Info("Loaded Image: {0}", fileName);
    }

    private void OpenFile()
    {
      openFileDialog.Title = "Open a Virtual Remote Skin ...";
      openFileDialog.Filter = "Xml Files|*.xml";

      if (openFileDialog.ShowDialog(this) == DialogResult.OK)
      {
        _fileName = openFileDialog.FileName;

        if (!Path.GetExtension(_fileName).Equals(".xml", StringComparison.OrdinalIgnoreCase))
          return;

        LoadSkinFile(_fileName);

        string imageFile = Path.ChangeExtension(_fileName, ".png");
        if (File.Exists(imageFile))
          LoadImage(imageFile);

        _unsavedChanges = false;

        UpdateWindowTitle();
      }
    }

    private void NewFile()
    {
      _fileName = String.Empty;
      _currentButton.Visible = false;
      pictureBoxRemote.Image = null;
      pictureBoxRemote.Visible = false;
      listViewButtons.Items.Clear();
      _unsavedChanges = false;

      UpdateWindowTitle();
    }

    private void HighlightSelectedButton(ListViewItem item)
    {
      int Top = int.Parse(item.SubItems[3].Text);
      int Left = int.Parse(item.SubItems[4].Text);
      int Height = int.Parse(item.SubItems[5].Text);
      int Width = int.Parse(item.SubItems[6].Text);

      _currentButton.Location = new Point(Left, Top);
      _currentButton.Size = new Size(Width, Height);
      _currentButton.BackColor = Color.Yellow;
      _currentButton.Visible = true;
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

    private void CommsFailure(object obj)
    {
      Exception ex = obj as Exception;

      if (ex != null)
        IrssLog.Error("Communications failure: {0}", ex.Message);
      else
        IrssLog.Error("Communications failure");

      StopClient();

      MessageBox.Show(this, "Please report this error.", "Virtual Remote Skin Editor - Communications failure",
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    private void ReceivedMessage(IrssMessage received)
    {
      IrssLog.Debug("Received Message \"{0}\"", received.Type);

      try
      {
        switch (received.Type)
        {
          case MessageType.RemoteEvent:
            if (listViewButtons.SelectedItems.Count == 1)
            {
              string keyCode = received.MessageData[IrssMessage.KEY_CODE] as string;

              listViewButtons.SelectedItems[0].SubItems[1].Text = keyCode;
            }
            return;

          case MessageType.RegisterClient:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              _registered = true;
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              _registered = false;
              MessageBox.Show(this, "Failed to register with server", "Virtual Remote Skin Editor Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return;

          case MessageType.ServerShutdown:
            MessageBox.Show(this, "Server has been shut down", "Virtual Remote Skin Editor Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
            return;

          case MessageType.Error:
            MessageBox.Show(this, received.GetDataAsString(), "Virtual Remote Skin Editor Error from Server",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(this, ex.Message, "Virtual Remote Skin Editor Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #endregion Implementation

  }
}