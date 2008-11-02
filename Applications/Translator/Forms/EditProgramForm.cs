using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using IrssUtils;

namespace Translator
{
  internal partial class EditProgramForm : Form
  {
    #region Properties

    public string DisplayName
    {
      get { return textBoxDisplayName.Text; }
      set { textBoxDisplayName.Text = value; }
    }

    public string Filename
    {
      get { return textBoxApp.Text; }
      set { textBoxApp.Text = value; }
    }

    public string StartupFolder
    {
      get { return textBoxAppStartFolder.Text; }
      set { textBoxAppStartFolder.Text = value; }
    }

    public string Parameters
    {
      get { return textBoxApplicationParameters.Text; }
      set { textBoxApplicationParameters.Text = value; }
    }

    public ProcessWindowStyle StartState
    {
      get
      {
        return
          (ProcessWindowStyle) Enum.Parse(typeof (ProcessWindowStyle), comboBoxWindowStyle.SelectedItem as string, true);
      }
      set { comboBoxWindowStyle.SelectedItem = Enum.GetName(typeof (ProcessWindowStyle), value); }
    }

    public bool UseShellExecute
    {
      get { return checkBoxShellExecute.Checked; }
      set { checkBoxShellExecute.Checked = value; }
    }

    public bool ForceWindowFocus
    {
      get { return checkBoxForceFocus.Checked; }
      set { checkBoxForceFocus.Checked = value; }
    }

    public bool IgnoreSystemWide
    {
      get { return checkBoxIgnoreSystemWide.Checked; }
      set { checkBoxIgnoreSystemWide.Checked = value; }
    }

    #endregion Properties

    #region Constructor

    public EditProgramForm(ProgramSettings progSettings)
    {
      InitializeComponent();

      comboBoxWindowStyle.Items.AddRange(Enum.GetNames(typeof (ProcessWindowStyle)));

      if (progSettings != null)
      {
        DisplayName = progSettings.Name;
        Filename = progSettings.FileName;
        StartupFolder = progSettings.Folder;
        Parameters = progSettings.Arguments;
        StartState = progSettings.WindowState;
        UseShellExecute = progSettings.UseShellExecute;
        ForceWindowFocus = progSettings.ForceWindowFocus;
        IgnoreSystemWide = progSettings.IgnoreSystemWide;
      }
    }

    #endregion Constructor

    #region Buttons

    private void buttonLocate_Click(object sender, EventArgs e)
    {
      OpenFileDialog find = new OpenFileDialog();
      find.Filter = "All files|*.*";
      find.Multiselect = false;
      find.Title = "Application to launch";

      if (find.ShowDialog(this) == DialogResult.OK)
      {
        textBoxApp.Text = find.FileName;
        if (String.IsNullOrEmpty(textBoxAppStartFolder.Text))
          textBoxAppStartFolder.Text = Path.GetDirectoryName(find.FileName);

        if (String.IsNullOrEmpty(textBoxDisplayName.Text) ||
            textBoxDisplayName.Text.Equals(ProgramSettings.NewProgramName, StringComparison.Ordinal))
          textBoxDisplayName.Text = Path.GetFileNameWithoutExtension(find.FileName);
      }
    }

    private void buttonStartupFolder_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog find = new FolderBrowserDialog();
      find.Description = "Please specify the starting folder for the application";
      find.ShowNewFolderButton = true;
      if (find.ShowDialog(this) == DialogResult.OK)
        textBoxAppStartFolder.Text = find.SelectedPath;
    }

    private void buttonTest_Click(object sender, EventArgs e)
    {
      try
      {
        string[] launchCommand = new string[]
                                   {
                                     Filename,
                                     StartupFolder,
                                     Parameters,
                                     Enum.GetName(typeof (ProcessWindowStyle), StartState),
                                     false.ToString(),
                                     UseShellExecute.ToString(),
                                     false.ToString(),
                                     ForceWindowFocus.ToString()
                                   };

        Common.ProcessRunCommand(launchCommand);
      }
      catch (Exception ex)
      {
        IrssLog.Error("Test Application: {0}", ex.ToString());
      }
    }

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

    #endregion Buttons
  }
}