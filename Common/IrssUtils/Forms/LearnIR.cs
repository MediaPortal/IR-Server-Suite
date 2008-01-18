using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IrssUtils.Forms
{

  /// <summary>
  /// Learn IR form.
  /// </summary>
  public partial class LearnIR : Form
  {

    #region Variables

    LearnIrDelegate _learnIrDelegate;
    BlastIrDelegate _blastIrDelegate;

    bool _resetTextBoxNameEnabled;

    #endregion Variables

    #region Constructor

    /// <summary>
    /// Create a new IR Command.
    /// </summary>
    /// <param name="learnIrDelegate">Delegate to call to start the IR learn process.</param>
    /// <param name="blastIrDelegate">Delegate to call to test an IR Command.</param>
    /// <param name="ports">Available blaster ports to transmit on.</param>
    public LearnIR(LearnIrDelegate learnIrDelegate, BlastIrDelegate blastIrDelegate, string[] ports)
    {
      if (learnIrDelegate == null)
        throw new ArgumentNullException("learnIrDelegate");

      if (blastIrDelegate == null)
        throw new ArgumentNullException("blastIrDelegate");

      if (ports == null)
        throw new ArgumentNullException("ports");

      _learnIrDelegate = learnIrDelegate;
      _blastIrDelegate = blastIrDelegate;

      InitializeComponent();

      comboBoxPort.Items.Clear();
      comboBoxPort.Items.AddRange(ports);
      comboBoxPort.SelectedIndex = 0;

      labelLearned.Text         = "Not yet learned";
      buttonTest.Enabled        = false;
      textBoxName.Text          = "New";
      textBoxName.Enabled       = true;
      _resetTextBoxNameEnabled  = true;
    }

    /// <summary>
    /// Relearn an existing IR Command.
    /// </summary>
    /// <param name="learnIrDelegate">Delegate to call to start the IR learn process.</param>
    /// <param name="blastIrDelegate">Delegate to call to test an IR Command.</param>
    /// <param name="ports">Available blast ports to transmit on.</param>
    /// <param name="existingCodeName">Name of the existing IR Command.</param>
    public LearnIR(LearnIrDelegate learnIrDelegate, BlastIrDelegate blastIrDelegate, string[] ports, string existingCodeName)
      : this(learnIrDelegate, blastIrDelegate, ports)
    {
      if (String.IsNullOrEmpty(existingCodeName))
        throw new ArgumentNullException("existingCodeName");

      labelLearned.Text         = "Not yet re-learned";
      buttonTest.Enabled        = true;
      textBoxName.Text          = existingCodeName;
      textBoxName.Enabled       = false;
      _resetTextBoxNameEnabled  = false;
    }

    #endregion Constructor

    delegate void LearnStatusDelegate(string status, bool success);

    /// <summary>
    /// Updates the Learn IR status.
    /// </summary>
    /// <param name="status">The status.</param>
    /// <param name="success">Success status.</param>
    public void LearnStatus(string status, bool success)
    {
      if (labelLearned.InvokeRequired)
      {
        this.Invoke(new LearnStatusDelegate(LearnStatus), new object[] { status, success });
      }
      else
      {
        labelLearned.Text = status;
        labelLearned.ForeColor = success ? Color.Green : Color.Red;
        labelLearned.Update();

        textBoxName.Enabled = _resetTextBoxNameEnabled;
        buttonLearn.Enabled = true;
        buttonTest.Enabled = success;
        buttonDone.Enabled = true;
      }
    }

    #region Buttons

    private void buttonLearn_Click(object sender, EventArgs e)
    {
      string name = textBoxName.Text.Trim();

      if (name.Length == 0)
      {
        MessageBox.Show(this, "You must supply a name for this IR Command", "Missing name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        textBoxName.Focus();
        return;
      }

      if (!Common.IsValidFileName(name))
      {
        MessageBox.Show(this, "You must supply a valid name for this IR Command", "Invalid name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        textBoxName.Focus();
        return;
      }

      textBoxName.Enabled = false;
      buttonLearn.Enabled = false;
      buttonTest.Enabled = false;
      buttonDone.Enabled = false;

      string fileName = Common.FolderIRCommands + name + Common.FileExtensionIR;

      if (_learnIrDelegate(fileName))
      {
        labelLearned.Text = "Press button to learn now";
        labelLearned.ForeColor = Color.Blue;
        labelLearned.Update();
      }
      else
      {
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
      string name = textBoxName.Text.Trim();

      if (name.Length == 0)
        return;

      try
      {
        string fileName = Common.FolderIRCommands + name + Common.FileExtensionIR;
        string port = comboBoxPort.SelectedItem as string;

        _blastIrDelegate(fileName, port);
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Test failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #endregion Buttons

  }

}
