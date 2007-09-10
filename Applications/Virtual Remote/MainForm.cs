using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using NamedPipes;
using IrssUtils;

namespace VirtualRemote
{

  public partial class MainForm : Form
  {

    #region Variables

    RemoteButton []_buttons;

    Thread _updateThread;

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

      _updateThread = new Thread(new ThreadStart(SetLabel));
      _updateThread.Start();
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      _updateThread.Abort();
    }

    private void MainForm_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Left)
        return;

      if (_buttons == null)
        return;

      foreach (RemoteButton button in _buttons)
      {
        if (e.Y >= button.Top &&
            e.Y < button.Top + button.Height &&
            e.X >= button.Left &&
            e.X <= button.Left + button.Width)
        {
          ButtonPress(button.Code);
          break;
        }
      }
    }
    private void MainForm_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.None)
        return;

      foreach (RemoteButton button in _buttons)
      {
        if (button.Shortcut == e.KeyCode)
        {
          ButtonPress(button.Code);
          break;
        }
      }
    }

    bool LoadSkinXml(string xmlFile)
    {
      if (!File.Exists(xmlFile))
        return false;

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

      _buttons = (RemoteButton[])buttons.ToArray(typeof(RemoteButton));

      return true;
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
        IrssLog.Error(ex.Message);
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
          throw new FileNotFoundException("Skin graphic file not found", skin);

        // Try to load xml file of same name, failing that load using first word of skin name ...
        string xmlFile = String.Format("{0}\\Skins\\{1}.xml", Program.InstallFolder, skin);
        if (!File.Exists(xmlFile))
        {
          string firstWord = skin.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];

          xmlFile = String.Format("{0}\\Skins\\{1}.xml", Program.InstallFolder, firstWord);

          if (!File.Exists(xmlFile))
            throw new FileNotFoundException("Skin file not found", xmlFile);
        }

        if (LoadSkinXml(xmlFile))
        {
          this.BackgroundImage = new Bitmap(skinFile);
          this.ClientSize = new System.Drawing.Size(this.BackgroundImage.Width, this.BackgroundImage.Height);
          Program.RemoteSkin = skin;
        }
        else
          throw new Exception(String.Format("Failed to load skin file not found \"{0}\"", xmlFile));
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Virtual Remote - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    void ButtonPress(string keyCode)
    {
      if (!PipeAccess.ServerRunning)
        return;

      PipeMessage message = new PipeMessage(Program.LocalPipeName, Environment.MachineName, PipeMessageType.ForwardRemoteEvent, PipeMessageFlags.None, keyCode);
      PipeAccess.SendMessage(Common.ServerPipeName, Program.ServerHost, message);
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
      Program.StopComms();

      IrssUtils.Forms.ServerAddress serverAddress = new IrssUtils.Forms.ServerAddress(Program.ServerHost);
      serverAddress.ShowDialog(this);
      Program.ServerHost = serverAddress.ServerHost;

      Program.StartComms();
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
