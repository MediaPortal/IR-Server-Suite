using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Win32;

using TvLibrary.Log;
using TvEngine;
using TvControl;
using TvDatabase;

using IrssUtils;

namespace SetupTv.Sections
{

  public partial class PluginSetup : SetupTv.SectionSettings
  {

    #region Constructor

    public PluginSetup()
    {
      InitializeComponent();

      TvBusinessLayer layer = new TvBusinessLayer();
      TV3BlasterPlugin.LogVerbose = checkBoxLogVerbose.Checked = Convert.ToBoolean(layer.GetSetting("TV3BlasterPlugin_LogVerbose", "False").Value);
      TV3BlasterPlugin.ServerHost = layer.GetSetting("TV3BlasterPlugin_ServerHost", String.Empty).Value;

      if (String.IsNullOrEmpty(TV3BlasterPlugin.ServerHost))
      {
        buttonHostSetup_Click(null, null);
      }

      if (!TV3BlasterPlugin.StartComms())
        MessageBox.Show(this, "Failed to start local comms. IR functions temporarily disabled.", "TV3 Blaster Plugin - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

      RefreshIRList();
      RefreshMacroList();
    }

    #endregion Constructor

    #region SetupTv.SectionSettings

    public override void OnSectionDeActivated()
    {
      Log.Info("TV3BlasterPlugin: Configuration deactivated");

      TvBusinessLayer layer = new TvBusinessLayer();
      Setting setting;

      TV3BlasterPlugin.LogVerbose = checkBoxLogVerbose.Checked;

      setting = layer.GetSetting("TV3BlasterPlugin_LogVerbose");
      setting.Value = TV3BlasterPlugin.LogVerbose.ToString();
      setting.Persist();

      setting = layer.GetSetting("TV3BlasterPlugin_ServerHost");
      setting.Value = TV3BlasterPlugin.ServerHost;
      setting.Persist();

      TV3BlasterPlugin.LoadExternalConfigs();

      TV3BlasterPlugin.InConfiguration = false;

      base.OnSectionDeActivated();
    }
    public override void OnSectionActivated()
    {
      Log.Info("TV3BlasterPlugin: Configuration activated");

      TV3BlasterPlugin.InConfiguration = true;

      TV3BlasterPlugin.LoadExternalConfigs();

      base.OnSectionActivated();
    }

    #endregion SetupTv.SectionSettings

    #region Implementation

    void RefreshIRList()
    {
      listBoxIR.Items.Clear();
      listBoxIR.Items.AddRange(Common.GetIRList(false));
    }
    void RefreshMacroList()
    {
      listBoxMacro.Items.Clear();
      listBoxMacro.Items.AddRange(TV3BlasterPlugin.GetMacroList(false));
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
        string fileName = TV3BlasterPlugin.FolderMacros + command + Common.FileExtensionMacro;

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

    #endregion Implementation

    #region Controls

    private void buttonSTB_Click(object sender, EventArgs e)
    {
      if (TvDatabase.Card.ListAll().Count == 0)
      {
        MessageBox.Show(this, "There are no capture cards installed in the TV server", "No TV Cards", MessageBoxButtons.OK, MessageBoxIcon.Stop);
      }
      else
      {
        ExternalChannels externalChannels = new ExternalChannels();
        externalChannels.ShowDialog(this);
      }
    }

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
          if (MessageBox.Show(this, "Are you sure you want to delete \"" + file + "\"?", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
        string fileName = TV3BlasterPlugin.FolderMacros + file + Common.FileExtensionMacro;
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

      string fileName = TV3BlasterPlugin.FolderMacros + listBoxMacro.SelectedItem as string + Common.FileExtensionMacro;

      try
      {
        TV3BlasterPlugin.ProcessMacro(fileName);
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Test failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void listBoxIR_DoubleClick(object sender, EventArgs e)
    {
      EditIR();
    }
    private void listBoxMacro_DoubleClick(object sender, EventArgs e)
    {
      EditMacro();
    }

    private void buttonHostSetup_Click(object sender, EventArgs e)
    {
      TV3BlasterPlugin.StopComms();

      IrssUtils.Forms.ServerAddress serverAddress = new IrssUtils.Forms.ServerAddress(TV3BlasterPlugin.ServerHost);
      serverAddress.ShowDialog();
      TV3BlasterPlugin.ServerHost = serverAddress.ServerHost;

      TV3BlasterPlugin.StartComms();
    }

    private void buttonHelp_Click(object sender, EventArgs e)
    {
      RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\IR Server Suite\\");
      string installFolder = (string)registryKey.GetValue("Install_Dir", String.Empty);
      registryKey.Close();

      Help.ShowHelp(this, installFolder + "\\IR Server Suite.chm");
      // , HelpNavigator.Topic, "index.html"
    }

    #endregion Controls

  }

}
