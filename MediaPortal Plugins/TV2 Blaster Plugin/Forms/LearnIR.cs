using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using NamedPipes;
using IrssUtils;

namespace MediaPortal.Plugins
{

  public partial class LearnIR : Form
  {

    #region Delegate

    delegate void DelegateLearnStatus(string status, bool success);

    #endregion Delegate

    #region Variables

    DelegateLearnStatus _learnStatus = null;

    bool _resetTextBoxNameEnabled;

    #endregion Variables

    #region Constructor

    public LearnIR(bool newCode, string name)
    {
      InitializeComponent();

      textBoxName.Text = name;
      textBoxName.Enabled = newCode;
      _resetTextBoxNameEnabled = newCode;

      if (!newCode)
      {
        labelLearned.Text = "Not yet re-learned";
        buttonTest.Enabled = true;
      }
      else
      {
        labelLearned.Text = "Not yet learned";
        buttonTest.Enabled = false;
      }

      comboBoxPort.Items.Clear();
      comboBoxPort.Items.AddRange(TV2BlasterPlugin.TransceiverInformation.Ports);
      comboBoxPort.SelectedIndex = 0;

      comboBoxSpeed.Items.Clear();
      comboBoxSpeed.Items.AddRange(TV2BlasterPlugin.TransceiverInformation.Speeds);
      comboBoxSpeed.SelectedIndex = 0;
    }

    #endregion Constructor

    void LearnStatus(string status, bool success)
    {
      labelLearned.Text = status;
      labelLearned.ForeColor = success ? Color.Green : Color.Red;
      labelLearned.Update();

      textBoxName.Enabled = _resetTextBoxNameEnabled;
      buttonLearn.Enabled = true;
      buttonTest.Enabled = success;
      buttonDone.Enabled = true;
    }

    void MessageReceiver(string message)
    {
      PipeMessage received = PipeMessage.FromString(message);

      switch (received.Name)
      {
        case "Learn Success":
          {
            if (_learnStatus != null)
            {
              this.Invoke(_learnStatus, new Object[] { "Learned IR", true });
              _learnStatus = null;
            }

            TV2BlasterPlugin.HandleMessage -= new Common.MessageHandler(MessageReceiver);
            return;
          }

        case "Learn Failure":
          {
            if (_learnStatus != null)
            {
              this.Invoke(_learnStatus, new Object[] { "Failed to learn IR", false });
              _learnStatus = null;
            }

            TV2BlasterPlugin.HandleMessage -= new Common.MessageHandler(MessageReceiver);
            return;
          }
      }
    }

    private void buttonLearn_Click(object sender, EventArgs e)
    {
      string command = textBoxName.Text.Trim();

      if (command.Length == 0)
      {
        MessageBox.Show(this, "You must supply a name for this IR code", "Missing name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        textBoxName.Focus();
        return;
      }

      TV2BlasterPlugin.HandleMessage += new Common.MessageHandler(MessageReceiver);
      _learnStatus = new DelegateLearnStatus(LearnStatus);

      textBoxName.Enabled = false;
      buttonLearn.Enabled = false;
      buttonTest.Enabled = false;
      buttonDone.Enabled = false;

      if (TV2BlasterPlugin.LearnIRCommand(Common.FolderIRCommands + command + Common.FileExtensionIR))
      {
        labelLearned.Text = "Press button to learn now";
        labelLearned.ForeColor = Color.Blue;
        labelLearned.Update();
      }
      else
      {
        TV2BlasterPlugin.HandleMessage -= new Common.MessageHandler(MessageReceiver);
        _learnStatus = null;

        labelLearned.Text = "Failed to learn IR";
        labelLearned.ForeColor = Color.Red;

        textBoxName.Enabled = _resetTextBoxNameEnabled;
        buttonLearn.Enabled = true;
        buttonTest.Enabled = false;
        buttonDone.Enabled = true;
      }
    }

    private void buttonDone_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void buttonTest_Click(object sender, EventArgs e)
    {
      string command = textBoxName.Text.Trim();

      if (command.Length == 0)
        return;

      try
      {
        TV2BlasterPlugin.BlastIR(Common.FolderIRCommands + command + Common.FileExtensionIR,
          comboBoxPort.SelectedItem as string,
          comboBoxSpeed.SelectedItem as string);
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Test failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

  }

}
