using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using IrssUtils;

namespace Commands
{

  /// <summary>
  /// Edit Blast IR Command form.
  /// </summary>
  public partial class EditBlastIR : Form
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
        return new string[] {
          _fileName,
          comboBoxPort.SelectedItem as string };
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

    BlastIrDelegate _blastIrDelegate;
    string _fileName;

    #endregion Variables

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EditBlastIR"/> class.
    /// </summary>
    /// <param name="blastIrDelegate">The blast IR delegate.</param>
    /// <param name="ports">The available ports.</param>
    EditBlastIR(BlastIrDelegate blastIrDelegate, string[] ports)
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

      _fileName = parameters[0];

      string displayName = _fileName;
      if (_fileName.StartsWith(Common.FolderAppData, StringComparison.OrdinalIgnoreCase))
        displayName = _fileName.Substring(Common.FolderAppData.Length);

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
      try
      {
        string fileName = _fileName + Common.FileExtensionIR;
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
