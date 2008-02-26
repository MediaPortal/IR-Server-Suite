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

using IrssComms;
using IrssUtils;
using IrssUtils.Forms;

namespace MediaPortal.Plugins
{

  partial class SetupForm : Form
  {

    #region Variables

    LearnIR _learnIR;

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
        ServerAddress serverAddress = new ServerAddress();
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
      listViewIR.Items.Clear();

      string[] irList = Common.GetIRList(false);
      if (irList != null && irList.Length > 0)
        foreach (string irFile in irList)
          listViewIR.Items.Add(irFile);
    }
    void RefreshMacroList()
    {
      listViewMacro.Items.Clear();

      string[] macroList = TV2BlasterPlugin.GetMacroList(false);
      if (macroList != null && macroList.Length > 0)
        foreach (string macroFile in macroList)
          listViewMacro.Items.Add(macroFile);
    }

    void EditIR()
    {
      if (listViewIR.SelectedItems.Count != 1)
        return;

      try
      {
        string command = listViewIR.SelectedItems[0].Text;
        string fileName = Common.FolderIRCommands + command + Common.FileExtensionIR;

        if (File.Exists(fileName))
        {
          _learnIR = new LearnIR(
            new LearnIrDelegate(TV2BlasterPlugin.LearnIR),
            new BlastIrDelegate(TV2BlasterPlugin.BlastIR),
            TV2BlasterPlugin.TransceiverInformation.Ports,
            command);

          _learnIR.ShowDialog(this);

          _learnIR = null;
        }
        else
        {
          RefreshIRList();

          throw new FileNotFoundException("IR file missing", fileName);
        }
      }
      catch (Exception ex)
      {
        Log.Error(ex);
        MessageBox.Show(this, ex.Message, "Failed to edit IR file", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
    void EditMacro()
    {
      if (listViewMacro.SelectedItems.Count != 1)
        return;

      try
      {
        string command = listViewMacro.SelectedItems[0].Text;
        string fileName = TV2BlasterPlugin.FolderMacros + command + Common.FileExtensionMacro;

        if (File.Exists(fileName))
        {
          MacroEditor macroEditor = new MacroEditor(command);
          macroEditor.ShowDialog(this);
        }
        else
        {
          RefreshMacroList();

          throw new FileNotFoundException("Macro file missing", fileName);
        }
      }
      catch (Exception ex)
      {
        Log.Error(ex);
        MessageBox.Show(this, ex.Message, "Failed to edit macro", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #endregion Local Methods

    #region Buttons

    private void buttonHelp_Click(object sender, EventArgs e)
    {
      try
      {
        string file = Path.Combine(SystemRegistry.GetInstallFolder(), "IR Server Suite.chm");
        Help.ShowHelp(this, file, HelpNavigator.Topic, "Plugins\\TV2 Blaster Plugin\\index.html");
      }
      catch (Exception ex)
      {
        Log.Error(ex);
        MessageBox.Show(this, ex.Message, "Failed to load help", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void buttonNewIR_Click(object sender, EventArgs e)
    {
      _learnIR = new LearnIR(
        new LearnIrDelegate(TV2BlasterPlugin.LearnIR),
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
      if (listViewIR.SelectedItems.Count != 1)
        return;

      string file = listViewIR.SelectedItems[0].Text;
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
      if (listViewMacro.SelectedItems.Count != 1)
        return;

      string file = listViewMacro.SelectedItems[0].Text;
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
    private void buttonTestMacro_Click(object sender, EventArgs e)
    {
      if (listViewMacro.SelectedItems.Count != 1)
        return;

      try
      {
        TV2BlasterPlugin.ProcessCommand(Common.CmdPrefixMacro + listViewMacro.SelectedItems[0].Text, false);
      }
      catch (Exception ex)
      {
        Log.Error(ex);
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

    private void buttonChangeServer_Click(object sender, EventArgs e)
    {
      TV2BlasterPlugin.StopClient();

      ServerAddress serverAddress = new ServerAddress(TV2BlasterPlugin.ServerHost);
      serverAddress.ShowDialog(this);

      TV2BlasterPlugin.ServerHost = serverAddress.ServerHost;

      IPAddress serverIP = Client.GetIPFromName(TV2BlasterPlugin.ServerHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, IrssComms.Server.DefaultPort);

      TV2BlasterPlugin.StartClient(endPoint);
    }

    private void buttonExtChannels_Click(object sender, EventArgs e)
    {
      ExternalChannels externalChannels = new ExternalChannels();
      externalChannels.ShowDialog(this);
    }

    #endregion Buttons

    #region Other Controls

    private void listViewIR_DoubleClick(object sender, EventArgs e)
    {
      EditIR();
    }
    private void listViewMacro_DoubleClick(object sender, EventArgs e)
    {
      EditMacro();
    }

    private void listViewIR_AfterLabelEdit(object sender, LabelEditEventArgs e)
    {
      ListView origin = sender as ListView;
      if (origin == null)
      {
        e.CancelEdit = true;
        return;
      }

      if (String.IsNullOrEmpty(e.Label))
      {
        e.CancelEdit = true;
        return;
      }

      ListViewItem originItem = origin.Items[e.Item];

      string oldFileName = Common.FolderIRCommands + originItem.Text + Common.FileExtensionIR;
      if (!File.Exists(oldFileName))
      {
        MessageBox.Show("File not found: " + oldFileName, "Cannot rename, Original file not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
        e.CancelEdit = true;
        return;
      }

      string name = e.Label.Trim();

      if (!Common.IsValidFileName(name))
      {
        MessageBox.Show("File name not valid: " + name, "Cannot rename, New file name not valid", MessageBoxButtons.OK, MessageBoxIcon.Error);
        e.CancelEdit = true;
        return;
      }

      try
      {
        string newFileName = Common.FolderIRCommands + name + Common.FileExtensionIR;

        File.Move(oldFileName, newFileName);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(ex.Message, "Failed to rename file", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
    private void listViewMacro_AfterLabelEdit(object sender, LabelEditEventArgs e)
    {
      ListView origin = sender as ListView;
      if (origin == null)
      {
        e.CancelEdit = true;
        return;
      }

      if (String.IsNullOrEmpty(e.Label))
      {
        e.CancelEdit = true;
        return;
      }

      ListViewItem originItem = origin.Items[e.Item];

      string oldFileName = TV2BlasterPlugin.FolderMacros + originItem.Text + Common.FileExtensionMacro;
      if (!File.Exists(oldFileName))
      {
        MessageBox.Show("File not found: " + oldFileName, "Cannot rename, Original file not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
        e.CancelEdit = true;
        return;
      }

      string name = e.Label.Trim();

      if (!Common.IsValidFileName(name))
      {
        MessageBox.Show("File name not valid: " + name, "Cannot rename, New file name not valid", MessageBoxButtons.OK, MessageBoxIcon.Error);
        e.CancelEdit = true;
        return;
      }

      try
      {
        string newFileName = TV2BlasterPlugin.FolderMacros + name + Common.FileExtensionMacro;

        File.Move(oldFileName, newFileName);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(ex.Message, "Failed to rename file", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #endregion Other Controls

  }

}
