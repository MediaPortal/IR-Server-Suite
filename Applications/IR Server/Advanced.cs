using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using IrssUtils;

namespace IRServer
{

  partial class Advanced : Form
  {

    #region Properties

    public bool AbstractRemoteMode
    {
      get { return checkBoxAbstractRemoteMode.Checked; }
      set { checkBoxAbstractRemoteMode.Checked = value; }
    }
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
    public string ProcessPriority
    {
      get
      {
        return comboBoxPriority.SelectedItem as string;
      }
      set
      {
        comboBoxPriority.SelectedItem = value;
      }

    }

    #endregion Properties

    #region Constructor

    public Advanced()
    {
      InitializeComponent();

      List<string> networkPCs = Network.GetComputers(false);
      if (networkPCs != null)
        comboBoxComputer.Items.AddRange(networkPCs.ToArray());

      comboBoxPriority.Items.Add("No Change");
      comboBoxPriority.Items.AddRange(Enum.GetNames(typeof(ProcessPriorityClass)));
      comboBoxPriority.SelectedIndex = 0;
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

    private void buttonExclusions_Click(object sender, EventArgs e)
    {
      /*
      Exclusions exclusions = new Exclusions(new string[] { "plugin1", "plugin2", "plugin3" });
      exclusions.ExclusionList = new string[] { "plugin1" };

      if (exclusions.ShowDialog(this) == DialogResult.OK)
      {

      }
      */
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

    private void checkBoxAbstractRemoteMode_CheckedChanged(object sender, EventArgs e)
    {
      //buttonExclusions.Enabled = checkBoxAbstractRemoteMode.Checked;
    }

    #endregion Controls

  }

}
