using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace IrssUtils.Forms
{

  /// <summary>
  /// External Program Command form.
  /// </summary>
  public partial class ExternalProgram : Form
  {

    #region Variables

    string _parametersMessage = String.Empty;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Gets the command string.
    /// </summary>
    /// <value>The command string.</value>
    public string CommandString
    {
      get
      {
        return String.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}",
          textBoxProgram.Text,
          textBoxStartup.Text,
          textBoxParameters.Text,
          comboBoxWindowStyle.SelectedItem as string,
          checkBoxNoWindow.Checked.ToString(),
          checkBoxShellExecute.Checked.ToString(),
          checkBoxWaitForExit.Checked.ToString(),
          checkBoxForceFocus.Checked.ToString());
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalProgram"/> class.
    /// </summary>
    public ExternalProgram() : this(null, String.Empty, true) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalProgram"/> class.
    /// </summary>
    /// <param name="canWait">Enable the "Wait for program to finish" option.</param>
    public ExternalProgram(bool canWait) : this(null, String.Empty, canWait) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalProgram"/> class.
    /// </summary>
    /// <param name="parametersMessage">The optional parameters message.</param>
    public ExternalProgram(string parametersMessage) : this(null, parametersMessage, true) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalProgram"/> class.
    /// </summary>
    /// <param name="commands">The command elements.</param>
    public ExternalProgram(string[] commands) : this(commands, String.Empty, true) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalProgram"/> class.
    /// </summary>
    /// <param name="commands">The command elements.</param>
    /// <param name="canWait">Enable the "Wait for program to finish" option.</param>
    public ExternalProgram(string[] commands, bool canWait) : this(commands, String.Empty, canWait) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalProgram"/> class.
    /// </summary>
    /// <param name="commands">The command elements.</param>
    /// <param name="parametersMessage">The optional parameters message.</param>
    public ExternalProgram(string[] commands, string parametersMessage) : this(commands, parametersMessage, true) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalProgram"/> class.
    /// </summary>
    /// <param name="commands">The command elements.</param>
    /// <param name="parametersMessage">The optional parameters message.</param>
    /// <param name="canWait">Enable the "Wait for program to finish" option.</param>
    public ExternalProgram(string[] commands, string parametersMessage, bool canWait)
    {
      InitializeComponent();

      if (canWait)
      {
        checkBoxWaitForExit.Visible = true;
        checkBoxWaitForExit.Enabled = true;
      }
      else
      {
        checkBoxWaitForExit.Visible = false;
        checkBoxWaitForExit.Enabled = false;
        checkBoxWaitForExit.Checked = false;
      }

      _parametersMessage = parametersMessage;

      comboBoxWindowStyle.Items.Clear();
      comboBoxWindowStyle.Items.AddRange(Enum.GetNames(typeof(ProcessWindowStyle)));

      if (commands != null)
      {
        textBoxProgram.Text           = commands[0];
        textBoxStartup.Text           = commands[1];
        textBoxParameters.Text        = commands[2];

        checkBoxNoWindow.Checked      = bool.Parse(commands[4]);
        checkBoxShellExecute.Checked  = bool.Parse(commands[5]);
        checkBoxWaitForExit.Checked   = bool.Parse(commands[6]);
        checkBoxForceFocus.Checked    = bool.Parse(commands[7]);

        comboBoxWindowStyle.SelectedItem  = ((ProcessWindowStyle)Enum.Parse(typeof(ProcessWindowStyle), commands[3], true)).ToString();
      }
      else
      {
        comboBoxWindowStyle.SelectedIndex = 0;
      }
    }

    #endregion Constructors

    private void ExternalProgram_Load(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(_parametersMessage) || _parametersMessage.Trim().Length == 0)
        buttonParamQuestion.Visible = false;
    }

    #region Buttons

    private void buttonProgam_Click(object sender, EventArgs e)
    {
      if (openFileDialog.ShowDialog(this) == DialogResult.OK)
      {
        textBoxProgram.Text = openFileDialog.FileName;

        if (textBoxStartup.Text.Trim().Length == 0)
        {
          textBoxStartup.Text = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
        }
      }
    }

    private void buttonStartup_Click(object sender, EventArgs e)
    {
      if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
      {
        textBoxProgram.Text = folderBrowserDialog.SelectedPath;
      }
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      if (textBoxProgram.Text.Trim().Length == 0)
      {
        MessageBox.Show(this, "You must specify a program to run", "Missing program path", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
      }
      
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void buttonParamQuestion_Click(object sender, EventArgs e)
    {
      MessageBox.Show(this, _parametersMessage, "Parameters", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void buttonTest_Click(object sender, EventArgs e)
    {
      if (textBoxProgram.Text.Trim().Length == 0)
      {
        MessageBox.Show(this, "You must specify a program to run", "Missing program path", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
      }

      try
      {
        using (Process process = new Process())
        {
          process.StartInfo.FileName          = textBoxProgram.Text;
          process.StartInfo.WorkingDirectory  = textBoxStartup.Text;
          process.StartInfo.Arguments         = textBoxParameters.Text;
          process.StartInfo.WindowStyle       = (ProcessWindowStyle)Enum.Parse(typeof(ProcessWindowStyle), comboBoxWindowStyle.SelectedItem as string, true);
          process.StartInfo.CreateNoWindow    = checkBoxNoWindow.Checked;
          process.StartInfo.UseShellExecute   = checkBoxShellExecute.Checked;

          process.Start();

          // Give new process focus ...
          if (!process.StartInfo.CreateNoWindow &&
            process.StartInfo.WindowStyle != ProcessWindowStyle.Hidden &&
            checkBoxForceFocus.Checked)
          {
            int attempt = 0;
            while (!process.HasExited && attempt++ < 50)
            {
              if (process.MainWindowHandle != IntPtr.Zero)
              {
                Win32.SetForegroundWindow(process.MainWindowHandle, true);
                break;
              }

              Thread.Sleep(500);
            }
          }
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Test Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #endregion Buttons

  }

}
