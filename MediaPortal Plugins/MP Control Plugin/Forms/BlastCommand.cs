using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using IrssUtils;

namespace MediaPortal.Plugins
{

  public partial class BlastCommand : Form
  {

    #region Properties

    public string CommandString
    {
      get
      {
        return String.Format("{0}|{1}|{2}",
          labelIRCommandFile.Text,
          comboBoxPort.SelectedItem as string,
          comboBoxSpeed.SelectedItem as string);
      }
    }

    #endregion Properties

    #region Constructors

    public BlastCommand(string file)
    {
      InitializeComponent();

      SetupPortsAndSpeeds();

      labelIRCommandFile.Text = file;
    }
    public BlastCommand(string[] commands)
    {
      InitializeComponent();

      SetupPortsAndSpeeds();
      
      if (commands == null)
        return;

      labelIRCommandFile.Text = commands[0];
      
      if (comboBoxPort.Items.Contains(commands[1]))
        comboBoxPort.SelectedItem = commands[1];

      if (comboBoxSpeed.Items.Contains(commands[2]))
        comboBoxSpeed.SelectedItem = commands[2];
    }

    #endregion Constructor

    void SetupPortsAndSpeeds()
    {
      comboBoxPort.Items.AddRange(MPControlPlugin.TransceiverInformation.Ports);
      comboBoxPort.SelectedIndex = 0;

      comboBoxSpeed.Items.AddRange(MPControlPlugin.TransceiverInformation.Speeds);
      comboBoxSpeed.SelectedIndex = 0;
    }

    #region Buttons

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

    private void buttonTest_Click(object sender, EventArgs e)
    {
      string fileName = labelIRCommandFile.Text.Trim();

      if (fileName.Length == 0)
        return;

      try
      {
        MPControlPlugin.BlastIR(Common.FolderIRCommands + fileName + Common.FileExtensionIR,
          comboBoxPort.SelectedItem as string,
          comboBoxSpeed.SelectedItem as string);
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Test failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #endregion Buttons

  }

}
