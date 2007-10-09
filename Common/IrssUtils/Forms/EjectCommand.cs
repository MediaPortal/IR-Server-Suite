using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace IrssUtils.Forms
{

  /// <summary>
  /// Eject Command form.
  /// </summary>
  public partial class EjectCommand : Form
  {

    #region Properties

    /// <summary>
    /// Gets the command string.
    /// </summary>
    /// <value>The command string.</value>
    public string CommandString
    {
      get
      {
        return comboBoxDrive.Text;
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EjectCommand"/> class.
    /// </summary>
    public EjectCommand()
    {
      InitializeComponent();

      DriveInfo[] drives = DriveInfo.GetDrives();
      foreach (DriveInfo drive in drives)
        if (drive.DriveType == DriveType.CDRom)
          comboBoxDrive.Items.Add(drive.Name);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EjectCommand"/> class.
    /// </summary>
    /// <param name="command">The currently selected drive.</param>
    public EjectCommand(string command)
      : this()
    {
      if (!String.IsNullOrEmpty(command))
        comboBoxDrive.SelectedItem = command;
    }

    #endregion

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

    #endregion Buttons

  }

}
