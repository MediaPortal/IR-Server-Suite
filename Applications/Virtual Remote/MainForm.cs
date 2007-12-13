using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

namespace VirtualRemote
{

  partial class MainForm : Form
  {

    #region Variables

    //Thread _updateThread;

    #endregion Variables

    #region Constructor

    public MainForm()
    {
      InitializeComponent();
    }

    #endregion Constructor

    private void MainForm_Load(object sender, EventArgs e)
    {
      SetSkinList();
      SetSkin(Program.RemoteSkin);

      //_updateThread = new Thread(new ThreadStart(SetLabel));
      //_updateThread.Start();
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      //_updateThread.Abort();
    }

    private void MainForm_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Left)
        return;

      Program.ProcessClick(e.X, e.Y);
    }
    private void MainForm_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.None)
        return;

      foreach (RemoteButton button in Program.Buttons)
      {
        if (button.Shortcut == e.KeyCode)
        {
          Program.ButtonPress(button.Code);
          break;
        }
      }
    }

    static void LoadSkinXml(string xmlFile)
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(xmlFile);

      ArrayList buttons = new ArrayList();
      RemoteButton temp;
      string key = String.Empty;

      XmlNodeList commandSequence = doc.DocumentElement.SelectNodes("button");
      foreach (XmlNode item in commandSequence)
      {
        temp = new RemoteButton();
        temp.Name = item.Attributes["name"].Value;
        temp.Code = item.Attributes["code"].Value;

        try
        {
          key = item.Attributes["shortcut"].Value;
          temp.Shortcut = (Keys)Enum.Parse(typeof(Keys), key, true);
        }
        catch (ArgumentException)
        {
          IrssLog.Error("Invalid Key Shortcut \"{0}\" in skin \"{1}\"", key, xmlFile);
        }

        temp.Top    = int.Parse(item.Attributes["top"].Value);
        temp.Left   = int.Parse(item.Attributes["left"].Value);
        temp.Height = int.Parse(item.Attributes["height"].Value);
        temp.Width  = int.Parse(item.Attributes["width"].Value);
        buttons.Add(temp);
      }

      Program.Buttons = (RemoteButton[])buttons.ToArray(typeof(RemoteButton));
    }

    void SetSkinList()
    {
      try
      {
        string[] skins = Directory.GetFiles(Program.InstallFolder + "\\Skins\\", "*.png", SearchOption.TopDirectoryOnly);
        for (int index = 0; index < skins.Length; index++)
          skins[index] = Path.GetFileNameWithoutExtension(skins[index]);

        toolStripComboBoxSkin.Items.Clear();
        toolStripComboBoxSkin.Items.AddRange(skins);

        toolStripComboBoxSkin.SelectedItem = Program.RemoteSkin;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
      }
    }

    void SetSkin(string skin)
    {
      try
      {
        if (String.IsNullOrEmpty(skin))
          return;

        string skinFile = String.Format("{0}\\Skins\\{1}.png", Program.InstallFolder, skin);
        if (!File.Exists(skinFile))
          throw new FileNotFoundException("Skin graphic file not found", skinFile);

        // Try to load xml file of same name, failing that load using first word of skin name ...
        string xmlFile = String.Format("{0}\\Skins\\{1}.xml", Program.InstallFolder, skin);
        if (!File.Exists(xmlFile))
        {
          string firstWord = skin.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];

          xmlFile = String.Format("{0}\\Skins\\{1}.xml", Program.InstallFolder, firstWord);

          if (!File.Exists(xmlFile))
            throw new FileNotFoundException("Skin file not found", xmlFile);
        }

        LoadSkinXml(xmlFile);

        this.BackgroundImage = new Bitmap(skinFile);
        this.ClientSize = new Size(this.BackgroundImage.Width, this.BackgroundImage.Height);
        
        Program.RemoteSkin = skin;
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Virtual Remote - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    void SetLabel()
    {
      while (true)
      {
        labelDisabled.Visible = !Program.Registered;
        Thread.Sleep(2500);
      }
    }

    private void toolStripMenuItemQuit_Click(object sender, EventArgs e)
    {
      IrssLog.Info("User quit");

      this.Close();
    }

    private void toolStripComboBoxSkin_SelectedIndexChanged(object sender, EventArgs e)
    {
      SetSkin(toolStripComboBoxSkin.SelectedItem as string);
      contextMenuStrip.Hide();
    }

    private void changeServerHostToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Program.StopClient();

      ServerAddress serverAddress = new ServerAddress(Program.ServerHost);
      serverAddress.ShowDialog(this);

      Program.ServerHost = serverAddress.ServerHost;

      IPAddress serverIP = Client.GetIPFromName(Program.ServerHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, IrssComms.Server.DefaultPort);

      Program.StartClient(endPoint);
    }

    private void toolStripMenuItemHelp_Click(object sender, EventArgs e)
    {
      try
      {
        Help.ShowHelp(this, SystemRegistry.GetInstallFolder() + "\\IR Server Suite.chm", HelpNavigator.Topic, "Virtual Remote\\index.html");
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Failed to load help", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

  }

}
