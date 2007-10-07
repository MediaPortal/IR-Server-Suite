using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Configuration
{

  partial class Advanced : Form
  {

    #region Properties

    public InputServiceMode Mode
    {
      get
      {
        if (radioButtonRelay.Checked)
          return InputServiceMode.RelayMode;
        else if (radioButtonRepeater.Checked)
          return InputServiceMode.RepeaterMode;
        else
          return InputServiceMode.ServerMode;
      }
      set
      {
        switch (value)
        {
          case InputServiceMode.ServerMode:
            radioButtonServer.Checked = true;
            break;

          case InputServiceMode.RelayMode:
            radioButtonRelay.Checked = true;
            break;

          case InputServiceMode.RepeaterMode:
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

    #endregion Properties

    #region Constructor

    public Advanced()
    {
      InitializeComponent();

      string[] networkPCs = IrssUtils.Win32.GetNetworkComputers(false);
      if (networkPCs != null)
        comboBoxComputer.Items.AddRange(networkPCs);
    }

    #endregion Constructor

    #region Controls

    private void buttonOK_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }
    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

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

    #endregion Controls

  }

}
