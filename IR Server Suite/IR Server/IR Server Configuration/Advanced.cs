#region Copyright (C) 2005-2009 Team MediaPortal

// Copyright (C) 2005-2009 Team MediaPortal
// http://www.team-mediaportal.com
// 
// This Program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2, or (at your option)
// any later version.
// 
// This Program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with GNU Make; see the file COPYING.  If not, write to
// the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
// http://www.gnu.org/copyleft/gpl.html

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using IrssUtils;

namespace IRServer.Configuration
{
  /// <summary>
  /// Advanced Configuration Form.
  /// </summary>
  internal partial class Advanced : Form
  {
    #region Properties

    public bool AbstractRemoteMode
    {
      get { return checkBoxAbstractRemoteMode.Checked; }
      set { buttonExclusions.Enabled = checkBoxAbstractRemoteMode.Checked = value; }
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
      get { return comboBoxPriority.SelectedItem as string; }
      set { comboBoxPriority.SelectedItem = value; }
    }

    #endregion Properties

    #region Constructor

    public Advanced()
    {
      InitializeComponent();
      Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);

      List<string> networkPCs = Network.GetComputers(false);
      if (networkPCs != null)
        comboBoxComputer.Items.AddRange(networkPCs.ToArray());

      comboBoxPriority.Items.Add("No Change");
      comboBoxPriority.Items.AddRange(Enum.GetNames(typeof (ProcessPriorityClass)));
      comboBoxPriority.SelectedIndex = 0;
    }

    #endregion Constructor

    #region Controls

    private void buttonOK_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.OK;
      Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    private void buttonExclusions_Click(object sender, EventArgs e)
    {
      Exclusions exclusions = new Exclusions();
      //exclusions.ExclusionList = new string[] { "Microsoft MCE:" };

      if (exclusions.ShowDialog(this) == DialogResult.OK)
      {
      }
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
      buttonExclusions.Enabled = checkBoxAbstractRemoteMode.Checked;
    }

    #endregion Controls
  }
}