using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Win32;

using MediaPortal.GUI.Library;
using MediaPortal.Util;

using IrssUtils;

namespace MediaPortal.Plugins
{

  public partial class SetupForm : Form
  {

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
        IrssUtils.Forms.ServerAddress serverAddress = new IrssUtils.Forms.ServerAddress(Environment.MachineName);
        serverAddress.ShowDialog();
        TV2BlasterPlugin.ServerHost = serverAddress.ServerHost;
      }

      if (!TV2BlasterPlugin.StartComms())
        MessageBox.Show(this, "Failed to start local comms. IR functions temporarily disabled.", "TV2 Blaster Plugin - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

      checkBoxLogVerbose.Checked = TV2BlasterPlugin.LogVerbose;

      RefreshIRList();
      RefreshMacroList();
    }

    #region Local Methods

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
          LearnIR learnIR = new LearnIR(false, command);
          learnIR.ShowDialog(this);
        }
        else
        {
          MessageBox.Show(this, "File not found: " + fileName, "File missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
          MacroEditor macroEditor = new MacroEditor(false, command);
          macroEditor.ShowDialog(this);
        }
        else
        {
          MessageBox.Show(this, "File not found: " + fileName, "File missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          RefreshMacroList();
        }
      }
    }

    #endregion Local Methods

    #region Buttons

    private void buttonNewIR_Click(object sender, EventArgs e)
    {
      LearnIR learnIR = new LearnIR(true, String.Empty);
      learnIR.ShowDialog(this);

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
          MessageBox.Show(this, "File not found: " + fileName, "File missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        RefreshIRList();
      }
    }

    private void buttonNewMacro_Click(object sender, EventArgs e)
    {
      MacroEditor macroEditor = new MacroEditor(true, String.Empty);
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
          MessageBox.Show(this, "File not found: " + fileName, "File missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
      TV2BlasterPlugin.StopComms();
      
      IrssUtils.Forms.ServerAddress serverAddress = new IrssUtils.Forms.ServerAddress(TV2BlasterPlugin.ServerHost);
      serverAddress.ShowDialog();
      TV2BlasterPlugin.ServerHost = serverAddress.ServerHost;

      TV2BlasterPlugin.StartComms();
    }

    private void buttonHelp_Click(object sender, EventArgs e)
    {
      RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\IR Server Suite\\");
      string installFolder = (string)registryKey.GetValue("Install_Dir", String.Empty);
      registryKey.Close();

      Help.ShowHelp(this, installFolder + "\\IR Server Suite.chm");
      // , HelpNavigator.Topic, "index.html"
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
