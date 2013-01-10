using System;
using System.IO;
using System.Windows.Forms;
using IrssUtils;

namespace IrssCommands
{
  /// <summary>
  /// Edit Blast IR Command form.
  /// </summary>
  internal partial class EditBlastIR : Form
  {
    #region Properties

    /// <summary>
    /// Gets the command parameters.
    /// </summary>
    /// <value>The command parameters.</value>
    public string[] Parameters
    {
      get
      {
        return new string[]
                 {
                   _fileName,
                   comboBoxPort.SelectedItem as string
                 };
      }
    }

    /// <summary>
    /// Gets or sets the blaster port.
    /// </summary>
    /// <value>The blaster port.</value>
    public string BlasterPort
    {
      get { return comboBoxPort.SelectedItem as string; }
      set { comboBoxPort.SelectedItem = value; }
    }

    /// <summary>
    /// Gets a value indicating whether to use this commands details for all in the batch.
    /// </summary>
    /// <value><c>true</c> if use for all; otherwise, <c>false</c>.</value>
    public bool UseForAll
    {
      get { return checkBoxUseForAll.Checked; }
    }

    #endregion Properties

    #region Variables

    private readonly BlastIrDelegate _blastIrDelegate;
    private readonly string _fileName;

    #endregion Variables

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EditBlastIR"/> class.
    /// </summary>
    /// <param name="blastIrDelegate">The blast IR delegate.</param>
    /// <param name="ports">The available ports.</param>
    private EditBlastIR(BlastIrDelegate blastIrDelegate, string[] ports)
    {
      if (blastIrDelegate == null)
        throw new ArgumentNullException("blastIrDelegate");

      if (ports == null)
        throw new ArgumentNullException("ports");

      InitializeComponent();

      _blastIrDelegate = blastIrDelegate;

      comboBoxPort.Items.AddRange(ports);
      comboBoxPort.SelectedIndex = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EditBlastIR"/> class.
    /// </summary>
    /// <param name="blastIrDelegate">The blast IR delegate.</param>
    /// <param name="ports">The available ports.</param>
    /// <param name="parameters">The command parameters.</param>
    public EditBlastIR(BlastIrDelegate blastIrDelegate, string[] ports, string[] parameters)
      : this(blastIrDelegate, ports)
    {
      if (parameters == null)
        throw new ArgumentNullException("parameters");

      if (String.IsNullOrEmpty(parameters[0]))
      {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "IR Command Files|*" + Processor.FileExtensionIR;
        if (openFileDialog.ShowDialog(this) == DialogResult.OK)
          parameters[0] = openFileDialog.FileName;
      }

      _fileName = parameters[0];

      string filePath = Path.GetDirectoryName(_fileName);
      string fileName = Path.GetFileNameWithoutExtension(_fileName);

      string displayName = Path.Combine(filePath, fileName);
      if (displayName.StartsWith(Common.FolderIRCommands, StringComparison.OrdinalIgnoreCase))
        displayName = displayName.Substring(Common.FolderIRCommands.Length);
      else if (displayName.StartsWith(Common.FolderAppData, StringComparison.OrdinalIgnoreCase))
        displayName = displayName.Substring(Common.FolderAppData.Length);

      labelIRCommandFile.Text = displayName;

      if (comboBoxPort.Items.Contains(parameters[1]))
        comboBoxPort.SelectedItem = parameters[1];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EditBlastIR"/> class.
    /// </summary>
    /// <param name="blastIrDelegate">The blast IR delegate.</param>
    /// <param name="ports">The available ports.</param>
    /// <param name="parameters">The command parameters.</param>
    /// <param name="commandCount">The command count for this batch of commands.</param>
    public EditBlastIR(BlastIrDelegate blastIrDelegate, string[] ports, string[] parameters, int commandCount)
      : this(blastIrDelegate, ports, parameters)
    {
      if (commandCount > 1)
      {
        checkBoxUseForAll.Text = String.Format("Use this port for all ({0})", commandCount);
        checkBoxUseForAll.Visible = true;
      }
    }

    #endregion Constructors

    #region Buttons

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

    private void buttonTest_Click(object sender, EventArgs e)
    {
      try
      {
        _blastIrDelegate(_fileName, comboBoxPort.SelectedItem as string);
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Test failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #endregion Buttons
  }
}