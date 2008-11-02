using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace IrssUtils.Forms
{
  /// <summary>
  /// Learn IR form.
  /// </summary>
  public partial class LearnIR : Form
  {
    #region Variables

    private readonly BlastIrDelegate _blastIrDelegate;

    private readonly bool _isNewCode;
    private readonly LearnIrDelegate _learnIrDelegate;

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

      labelStatus.Text = "Nothing learned yet";
      groupBoxTest.Enabled = false;
      textBoxName.Text = "New";
      textBoxName.Enabled = true;
      _isNewCode = true;
    }

    /// <summary>
    /// Relearn an existing IR Command.
    /// </summary>
    /// <param name="learnIrDelegate">Delegate to call to start the IR learn process.</param>
    /// <param name="blastIrDelegate">Delegate to call to test an IR Command.</param>
    /// <param name="ports">Available blast ports to transmit on.</param>
    /// <param name="existingCodeName">Name of the existing IR Command.</param>
    public LearnIR(LearnIrDelegate learnIrDelegate, BlastIrDelegate blastIrDelegate, string[] ports,
                   string existingCodeName)
      : this(learnIrDelegate, blastIrDelegate, ports)
    {
      if (String.IsNullOrEmpty(existingCodeName))
        throw new ArgumentNullException("existingCodeName");

      labelStatus.Text = "IR Command is unchanged";
      groupBoxTest.Enabled = true;
      textBoxName.Text = existingCodeName;
      textBoxName.Enabled = false;
      _isNewCode = false;
    }

    #endregion Constructor

    /// <summary>
    /// Updates the Learn IR status.
    /// </summary>
    /// <param name="status">The status.</param>
    /// <param name="success">Success status.</param>
    public void LearnStatus(string status, bool success)
    {
      if (labelStatus.InvokeRequired)
      {
        Invoke(new LearnStatusDelegate(LearnStatus), new object[] {status, success});
      }
      else
      {
        labelStatus.Text = status;
        labelStatus.ForeColor = success ? Color.Green : Color.Red;
        labelStatus.Update();

        textBoxName.Enabled = _isNewCode;
        buttonLearn.Enabled = true;
        groupBoxTest.Enabled = success || !_isNewCode;
        buttonDone.Enabled = true;
      }
    }

    #region Buttons

    private void buttonLearn_Click(object sender, EventArgs e)
    {
      string name = textBoxName.Text.Trim();

      if (name.Length == 0)
      {
        MessageBox.Show(this, "You must supply a name for this IR Command", "Missing name", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
        textBoxName.Focus();
        return;
      }

      if (!Common.IsValidFileName(name))
      {
        MessageBox.Show(this, "You must supply a valid name for this IR Command", "Invalid name", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
        textBoxName.Focus();
        return;
      }

      textBoxName.Enabled = false;
      buttonLearn.Enabled = false;
      groupBoxTest.Enabled = false;
      buttonDone.Enabled = false;

      string fileName = Path.Combine(Common.FolderIRCommands, name + Common.FileExtensionIR);
      if (_learnIrDelegate(fileName))
      {
        labelStatus.Text = "Hold your remote close to the receiver and tap the button to learn";
        labelStatus.ForeColor = Color.Blue;
        labelStatus.Update();
      }
      else
      {
        labelStatus.Text = "Failed to learn IR";
        labelStatus.ForeColor = Color.Red;

        textBoxName.Enabled = _isNewCode;
        buttonLearn.Enabled = true;
        groupBoxTest.Enabled = !_isNewCode;
        buttonDone.Enabled = true;
      }
    }

    private void buttonDone_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void buttonTest_Click(object sender, EventArgs e)
    {
      string name = textBoxName.Text.Trim();

      if (name.Length == 0)
        return;

      try
      {
        string fileName = Path.Combine(Common.FolderIRCommands, name + Common.FileExtensionIR);
        string port = comboBoxPort.SelectedItem as string;

        _blastIrDelegate(fileName, port);
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Test failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #endregion Buttons

    #region Nested type: LearnStatusDelegate

    private delegate void LearnStatusDelegate(string status, bool success);

    #endregion
  }
}