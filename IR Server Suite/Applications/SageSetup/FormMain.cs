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
using System.Text;
using System.Windows.Forms;
using IrssUtils;
using Microsoft.Win32;

namespace SageSetup
{
  /// <summary>
  /// Main Sage Setup form.
  /// </summary>
  public partial class FormMain : Form
  {
    private string _irBlastLocation;

    /// <summary>
    /// Initializes a new instance of the <see cref="FormMain"/> class.
    /// </summary>
    public FormMain()
    {
      InitializeComponent();
    }

    private void FormMain_Load(object sender, EventArgs e)
    {
      List<string> networkPCs = Network.GetComputers(false);
      if (networkPCs == null)
      {
        MessageBox.Show(this, "No server names detected.", "Network Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        Application.Exit();
        return;
      }
      else
      {
        comboBoxComputer.Items.Add("localhost");
        comboBoxComputer.Items.AddRange(networkPCs.ToArray());
      }

      _irBlastLocation = SystemRegistry.GetInstallFolder();

      if (String.IsNullOrEmpty(_irBlastLocation))
      {
        MessageBox.Show(this, "IR Server Suite install location not found, please re-install IR Server Suite",
                        "Application Location Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        Application.Exit();
        return;
      }
    }

    private void buttonSet_Click(object sender, EventArgs e)
    {
/*
[HKEY_LOCAL_MACHINE\SOFTWARE\Sage\EXETunerPlugin]
"Command"="\"C:\\Program Files\\IR Server Suite\\IR Blast\\IRBlast.exe\" -host localhost -channel %CHANNEL%"

[HKEY_LOCAL_MACHINE\SOFTWARE\Frey Technologies\Common\EXEMultiTunerPlugin]
"Command"="\"C:\\Program Files\\IR Server Suite\\IR Blast\\IRBlast.exe\" -host localhost -port Port_%DEVICE% -channel %CHANNEL%"
*/
      string hostComputer = comboBoxComputer.Text;

      if (String.IsNullOrEmpty(hostComputer))
      {
        MessageBox.Show(this, "You must specify an IR Server host computer", "No Server Host", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
        return;
      }

      RegistryKey mainKey = null;

      try
      {
        if (radioButtonExeTuner.Checked)
          mainKey = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Sage\\EXETunerPlugin");
        else
          mainKey = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Frey Technologies\\Common\\EXEMultiTunerPlugin");

        StringBuilder command = new StringBuilder();
        command.Append("\"");
        command.Append(_irBlastLocation);
        command.Append("\\IR Blast\\");
        if (radioButtonConsole.Checked)
          command.Append("IRBlast.exe");
        else
          command.Append("IRBlast-NoWindow.exe");
        command.Append("\"");

        command.Append(" -host ");
        command.Append(hostComputer);

        if (radioButtonExeMultiTuner.Checked)
          command.Append(" -port Port_%DEVICE%");

        if (checkBoxPad.Checked)
          command.AppendFormat(" -pad {0}", numericUpDownPad.Value);

        command.Append(" -channel %CHANNEL%");

        mainKey.SetValue("Command", command.ToString(), RegistryValueKind.String);

        MessageBox.Show(this, "Sage plugin setup complete", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      finally
      {
        if (mainKey != null)
          mainKey.Close();
      }
    }
  }
}