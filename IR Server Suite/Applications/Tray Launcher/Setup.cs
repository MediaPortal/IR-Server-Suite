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
using System.Windows.Forms;
using IrssUtils;
using TrayLauncher.Properties;

namespace TrayLauncher
{
  internal partial class Setup : Form
  {
    #region Variables

    private string _launchKeyCode;
    private OpenFileDialog openFileDialog;

    #endregion Variables

    #region Properties

    public string ServerHost
    {
      get { return comboBoxComputer.Text; }
      set { comboBoxComputer.Text = value; }
    }

    public string ProgramFile
    {
      get { return textBoxApplication.Text; }
      set { textBoxApplication.Text = value; }
    }

    public bool AutoRun
    {
      get { return checkBoxAuto.Checked; }
      set { checkBoxAuto.Checked = value; }
    }

    public bool LaunchOnLoad
    {
      get { return checkBoxLaunchOnLoad.Checked; }
      set { checkBoxLaunchOnLoad.Checked = value; }
    }

    public bool OneInstanceOnly
    {
      get { return checkBoxOneInstance.Checked; }
      set
      {
        checkBoxOneInstance.Checked = value;
        checkBoxRepeatsFocus.Enabled = value;
      }
    }

    public bool RepeatsFocus
    {
      get { return checkBoxRepeatsFocus.Checked; }
      set { checkBoxRepeatsFocus.Checked = value; }
    }

    public string LaunchKeyCode
    {
      get { return _launchKeyCode; }
      set { _launchKeyCode = value; }
    }

    #endregion Properties

    #region Constructor

    public Setup()
    {
      InitializeComponent();

      Icon = Resources.Icon16;

      openFileDialog.Filter = "All files|*.*";
      openFileDialog.Title = "Select Application to Launch";

      comboBoxComputer.Items.Clear();
      comboBoxComputer.Items.Add("localhost");

      List<string> networkPCs = Network.GetComputers(false);
      if (networkPCs != null)
        comboBoxComputer.Items.AddRange(networkPCs.ToArray());
    }

    #endregion Constructor

    private void buttonOK_Click(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(textBoxApplication.Text))
      {
        MessageBox.Show("You must specify an application to launch", "No application", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
        return;
      }

      DialogResult = DialogResult.OK;
      Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    private void buttonFind_Click(object sender, EventArgs e)
    {
      if (openFileDialog.ShowDialog() == DialogResult.OK)
        textBoxApplication.Text = openFileDialog.FileName;
    }

    private void buttonRemoteButton_Click(object sender, EventArgs e)
    {
      if (!Tray.Registered)
      {
        MessageBox.Show(this, "Cannot learn a new launch button without being connected to an active IR Server",
                        "Can't learn button", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        return;
      }

      GetKeyCodeForm getKeyCode = new GetKeyCodeForm();
      getKeyCode.ShowDialog(this);

      string keyCode = getKeyCode.KeyCode;

      if (String.IsNullOrEmpty(keyCode))
        return;

      _launchKeyCode = keyCode;
    }

    private void checkBoxOneInstance_CheckedChanged(object sender, EventArgs e)
    {
      checkBoxRepeatsFocus.Enabled = checkBoxOneInstance.Checked;
    }
  }
}