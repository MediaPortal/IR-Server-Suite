using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows.Forms;

using Microsoft.Win32;

using IRServerPluginInterface;
using IrssUtils;

namespace IRServer
{

  public partial class Config : Form
  {

    #region Variables

    IIRServerPlugin[] _transceivers;

    #endregion Variables

    #region Properties

    public IRServerMode Mode
    {
      get
      {
        if (radioButtonRelay.Checked)
          return IRServerMode.RelayMode;
        else if (radioButtonRepeater.Checked)
          return IRServerMode.RepeaterMode;
        else
          return IRServerMode.ServerMode;
      }
      set
      {
        switch (value)
        {
          case IRServerMode.ServerMode:
            radioButtonServer.Checked = true;
            break;

          case IRServerMode.RelayMode:
            radioButtonRelay.Checked = true;
            break;

          case IRServerMode.RepeaterMode:
            radioButtonRepeater.Checked = true;
            break;
        }
      }
    }
    public string HostComputer
    {
      get { return comboBoxComputer.Text; }
      set { comboBoxComputer.Text = value; }
    }
    public string Plugin
    {
      get { return textBoxPlugin.Text; }
      set { textBoxPlugin.Text = value; }
    }

    #endregion Properties

    #region Constructor

    public Config()
    {
      InitializeComponent();

      // Add transceivers to list ...
      _transceivers = Program.AvailablePlugins();

      listViewTransceiver.Items.Clear();

      if (_transceivers == null || _transceivers.Length == 0)
      {
        MessageBox.Show(this, "No IR Server Plugins found!", "IR Server Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      else
      {
        foreach (IIRServerPlugin transceiver in _transceivers)
        {
          ListViewItem listViewItem = new ListViewItem(
            new string[] { 
            transceiver.Name, 
            transceiver.CanReceive.ToString(), 
            transceiver.CanTransmit.ToString()
          });

          listViewItem.ToolTipText = String.Format("{0}\nVersion: {1}\nAuthor: {2}\n{3}", transceiver.Name, transceiver.Version, transceiver.Author, transceiver.Description);

          listViewTransceiver.Items.Add(listViewItem);
        }
      }

      try
      {
        RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
        checkBoxRunAtBoot.Checked = (key.GetValue("IR Server", null) != null);
        key.Close();
      }
      catch
      {
      }

      ArrayList networkPCs = IrssUtils.Win32.GetNetworkComputers();
      if (networkPCs != null)
      {
        foreach (string computer in networkPCs.ToArray(typeof(string)))
          if (computer != Environment.MachineName)
            comboBoxComputer.Items.Add(computer);
      }

    }

    #endregion Constructor

    private void Config_Load(object sender, EventArgs e)
    {
      if (_transceivers != null && !String.IsNullOrEmpty(textBoxPlugin.Text))
        foreach (IIRServerPlugin tx in _transceivers)
          if (tx.Name == textBoxPlugin.Text)
            buttonConfigureTransceiver.Enabled = tx.CanConfigure;
    }

    #region Controls

    private void buttonOK_Click(object sender, EventArgs e)
    {
      try
      {
        RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
        if (checkBoxRunAtBoot.Checked)
          key.SetValue("IR Server", Application.ExecutablePath);
        else
          key.DeleteValue("IR Server", false);
        key.Close();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
      }

      this.DialogResult = DialogResult.OK;
      this.Close();
    }
    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void buttonConfigureTransceiver_Click(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(textBoxPlugin.Text))
        return;

      foreach (IIRServerPlugin tx in _transceivers)
        if (tx.Name == textBoxPlugin.Text)
          if (tx.CanConfigure)
            tx.Configure();
    }

    private void listViewTransceiver_DoubleClick(object sender, EventArgs e)
    {
      textBoxPlugin.Text = (sender as ListView).FocusedItem.Text;

      foreach (IIRServerPlugin tx in _transceivers)
        if (tx.Name == textBoxPlugin.Text)
          buttonConfigureTransceiver.Enabled = tx.CanConfigure;
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

    private void radioButtonServer_CheckedChanged(object sender, EventArgs e)
    {
      comboBoxComputer.Enabled = false;
    }

    private void radioButtonRelay_CheckedChanged(object sender, EventArgs e)
    {
      comboBoxComputer.Enabled = true;
    }

    private void radioButtonRepeater_CheckedChanged(object sender, EventArgs e)
    {
      comboBoxComputer.Enabled = true;
    }

  }

}
