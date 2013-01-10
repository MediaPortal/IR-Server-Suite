using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace IrssCommands.General
{
  public partial class RunConfig : BaseCommandConfig
  {
    #region Properties

    /// <summary>
    /// Gets the command parameters.
    /// </summary>
    /// <value>The command parameters.</value>
    public override string[] Parameters
    {
      get
      {
        return new[]
          {
            textBoxApp.Text.Trim(),
            textBoxAppStartFolder.Text.Trim(),
            textBoxApplicationParameters.Text.Trim(),
            Enum.GetName(typeof (ProcessWindowStyle), comboBoxWindowStyle.SelectedItem),
            checkBoxNoWindow.Checked.ToString(),
            checkBoxShellExecute.Checked.ToString(),
            checkBoxWaitForExit.Checked.ToString(),
            checkBoxForceFocus.Checked.ToString()
          };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="RunConfig"/> class.
    /// </summary>
    private RunConfig()
    {
      InitializeComponent();

      comboBoxWindowStyle.Items.Clear();
      foreach (ProcessWindowStyle s in Enum.GetValues(typeof (ProcessWindowStyle)))
        comboBoxWindowStyle.Items.Add(s);
      comboBoxWindowStyle.SelectedIndex = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RunConfig"/> class.
    /// </summary>
    /// <param name="parameters">The command elements.</param>
    public RunConfig(string[] parameters) : this()
    {
      textBoxApp.Text = parameters[0];
      textBoxAppStartFolder.Text = parameters[1];
      textBoxApplicationParameters.Text = parameters[2];

      comboBoxWindowStyle.SelectedItem =
        (ProcessWindowStyle) Enum.Parse(typeof (ProcessWindowStyle), parameters[3], true);

      checkBoxNoWindow.Checked = bool.Parse(parameters[4]);
      checkBoxShellExecute.Checked = bool.Parse(parameters[5]);
      checkBoxWaitForExit.Checked = bool.Parse(parameters[6]);
      checkBoxForceFocus.Checked = bool.Parse(parameters[7]);
    }

    #endregion Constructors

    #region Implementation

    private void buttonLocate_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "All files|*.*";
      openFileDialog.Title = "Select Program Executable";
      openFileDialog.Multiselect = false;

      if (openFileDialog.ShowDialog(this) == DialogResult.OK)
      {
        textBoxApp.Text = openFileDialog.FileName;

        if (textBoxAppStartFolder.Text.Trim().Length == 0)
          textBoxAppStartFolder.Text = Path.GetDirectoryName(openFileDialog.FileName);
      }
    }

    private void buttonStartupFolder_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      folderBrowserDialog.Description = "Select the startup folder for the program to run from";
      folderBrowserDialog.ShowNewFolderButton = true;

      if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
      {
        textBoxAppStartFolder.Text = folderBrowserDialog.SelectedPath;
      }
    }

#warning check for parameters
    //private void buttonParamQuestion_Click(object sender, EventArgs e)
    //{
    //  MessageBox.Show(this, _parametersMessage, "Parameters", MessageBoxButtons.OK, MessageBoxIcon.Information);
    //}

    //private void RunCommandConfig_Load(object sender, EventArgs e)
    //{
    //  if (String.IsNullOrEmpty(_parametersMessage) || _parametersMessage.Trim().Length == 0)
    //    buttonParamQuestion.Visible = false;
    //}

    #endregion Implementation
  }
}