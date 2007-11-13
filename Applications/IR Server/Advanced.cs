using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using IrssUtils;

namespace IRServer
{

  partial class Advanced : Form
  {

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

    #endregion Properties

    #region Constructor

    public Advanced()
    {
      InitializeComponent();

      List<string> networkPCs = Network.GetComputers(false);
      if (networkPCs != null)
        comboBoxComputer.Items.AddRange(networkPCs.ToArray());
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
