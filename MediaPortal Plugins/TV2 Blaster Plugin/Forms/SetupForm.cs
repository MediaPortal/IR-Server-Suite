using System;
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

using MediaPortal.GUI.Library;
using MediaPortal.Util;

using IrssUtils;
using IrssComms;

namespace MediaPortal.Plugins
{

  partial class SetupForm : Form
  {

    #region Variables

    IrssUtils.Forms.LearnIR _learnIR = null;

    #endregion Variables

    #region Constructor

    public SetupForm()
    {
      InitializeComponent();
    }

    #endregion Constructor

    private void SetupForm_Load(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(TV2BlasterPlugin.ServerHost))
      {
        IrssUtils.Forms.ServerAddress serverAddress = new IrssUtils.Forms.ServerAddress();
        serverAddress.ShowDialog(this);

        TV2BlasterPlugin.ServerHost = serverAddress.ServerHost;
      }

      IPAddress serverIP = Client.GetIPFromName(TV2BlasterPlugin.ServerHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, IrssComms.Server.DefaultPort);

      if (!TV2BlasterPlugin.StartClient(endPoint))
        MessageBox.Show(this, "Failed to start local comms. IR functions temporarily disabled.", "TV2 Blaster Plugin - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

      checkBoxLogVerbose.Checked = TV2BlasterPlugin.LogVerbose;

      RefreshIRList();
      RefreshMacroList();

      TV2BlasterPlugin.HandleMessage += new ClientMessageSink(ReceivedMessage);
    }
    private void SetupForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      TV2BlasterPlugin.HandleMessage -= new ClientMessageSink(ReceivedMessage);
    }

    #region Local Methods

    void ReceivedMessage(IrssMessage received)
    {
      if (_learnIR != null && received.Type == MessageType.LearnIR)
      {
        if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
        {
          _learnIR.LearnStatus("Learned IR successfully", true);
        }
        else if ((received.Flags & MessageFlags.Timeout) == MessageFlags.Timeout)
        {
          _learnIR.LearnStatus("Learn IR timed out", false);
        }
        else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
        {
          _learnIR.LearnStatus("Learn IR failed", false);
        }
      }
    }

    void RefreshIRList()
    {
      listBoxIR.Items.Clear();
      listBoxIR.Items.AddRange(Common.GetIRList(false));
    }
    void RefreshMacroList()
    {
      listBoxMacro.Items.Clear();
      listBoxMacro.Items.AddRange(TV2BlasterPlugin.GetMacroList(false));
    }

    void EditIR()
    {
      if (listBoxIR.SelectedIndex != -1)
      {
        string command = listBoxIR.SelectedItem as string;
        string fileName = Common.FolderIRCommands + command + Common.FileExtensionIR;

        if (File.Exists(fileName))
        {
          _learnIR = new IrssUtils.Forms.LearnIR(
            new LearnIrDelegate(TV2BlasterPlugin.LearnIRCommand),
            new BlastIrDelegate(TV2BlasterPlugin.BlastIR),
            TV2BlasterPlugin.TransceiverInformation.Ports,
            command);
          
          _learnIR.ShowDialog(this);

          _learnIR = null;
        }
        else
        {
          MessageBox.Show(this, "File not found: " + fileName, "IR file missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          RefreshIRList();
        }
      }
    }
    void EditMacro()
    {
      if (listBoxMacro.SelectedIndex != -1)
      {
        string command = listBoxMacro.SelectedItem as string;
        string fileName = TV2BlasterPlugin.FolderMacros + command + Common.FileExtensionMacro;

        if (File.Exists(fileName))
        {
          MacroEditor macroEditor = new MacroEditor(command);
          macroEditor.ShowDialog(this);
        }
        else
        {
          MessageBox.Show(this, "File not found: " + fileName, "Macro file missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          RefreshMacroList();
        }
      }
    }

    #endregion Local Methods

    #region Buttons

    private void buttonNewIR_Click(object sender, EventArgs e)
    {
      _learnIR = new IrssUtils.Forms.LearnIR(
        new LearnIrDelegate(TV2BlasterPlugin.LearnIRCommand),
        new BlastIrDelegate(TV2BlasterPlugin.BlastIR),
        TV2BlasterPlugin.TransceiverInformation.Ports);
      
      _learnIR.ShowDialog(this);

      _learnIR = null;

      RefreshIRList();
    }
    private void buttonEditIR_Click(object sender, EventArgs e)
    {
      EditIR();
    }
    private void buttonDeleteIR_Click(object sender, EventArgs e)
    {
      if (listBoxIR.SelectedIndex != -1)
      {
        string file = listBoxIR.SelectedItem as string;
        string fileName = Common.FolderIRCommands + file + Common.FileExtensionIR;
        if (File.Exists(fileName))
        {
          if (MessageBox.Show(this, "Are you sure you want to delete \"" + file + "\"?", "Confirm delete",  MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            File.Delete(fileName);
        }
        else
        {
          MessageBox.Show(this, "File not found: " + fileName, "IR file missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        RefreshIRList();
      }
    }

    private void buttonNewMacro_Click(object sender, EventArgs e)
    {
      MacroEditor macroEditor = new MacroEditor();
        macroEditor.ShowDialog(this);

      RefreshMacroList();
    }
    private void buttonEditMacro_Click(object sender, EventArgs e)
    {
      EditMacro();
    }
    private void buttonDeleteMacro_Click(object sender, EventArgs e)
    {
      if (listBoxMacro.SelectedIndex != -1)
      {
        string file = listBoxMacro.SelectedItem as string;
        string fileName = TV2BlasterPlugin.FolderMacros + file + Common.FileExtensionMacro;
        if (File.Exists(fileName))
        {
          if (MessageBox.Show(this, "Are you sure you want to delete \"" + file + "\"?", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            File.Delete(fileName);
        }
        else
        {
          MessageBox.Show(this, "File not found: " + fileName, "Macro file missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        RefreshMacroList();
      }
    }
    private void buttonTestMacro_Click(object sender, EventArgs e)
    {
      if (listBoxMacro.SelectedIndex == -1)
        return;

      string fileName = TV2BlasterPlugin.FolderMacros + listBoxMacro.SelectedItem as string + Common.FileExtensionMacro;

      try
      {
        TV2BlasterPlugin.ProcessMacro(fileName);
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Test failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
    private void buttonOK_Click(object sender, EventArgs e)
    {
      TV2BlasterPlugin.LogVerbose = checkBoxLogVerbose.Checked;
      
      this.DialogResult = DialogResult.OK;
      this.Close();
    }
    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void buttonExtChannels_Click(object sender, EventArgs e)
    {
      ExternalChannels externalChannels = new ExternalChannels();
      externalChannels.ShowDialog(this);
    }

    private void buttonChangeServer_Click(object sender, EventArgs e)
    {
      TV2BlasterPlugin.StopClient();
      
      IrssUtils.Forms.ServerAddress serverAddress = new IrssUtils.Forms.ServerAddress(TV2BlasterPlugin.ServerHost);
      serverAddress.ShowDialog(this);

      TV2BlasterPlugin.ServerHost = serverAddress.ServerHost;

      IPAddress serverIP = Client.GetIPFromName(TV2BlasterPlugin.ServerHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, IrssComms.Server.DefaultPort);

      TV2BlasterPlugin.StartClient(endPoint);
    }

    private void buttonHelp_Click(object sender, EventArgs e)
    {
      try
      {
        Help.ShowHelp(this, SystemRegistry.GetInstallFolder() + "\\IR Server Suite.chm", HelpNavigator.Topic, "Plugins\\TV2 Blaster Plugin\\index.html");
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Failed to load help", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #endregion Buttons

    #region Other Controls

    private void listBoxIR_DoubleClick(object sender, EventArgs e)
    {
      EditIR();
    }
    private void listBoxMacro_DoubleClick(object sender, EventArgs e)
    {
      EditMacro();
    }

    #endregion Other Controls

  }

}
