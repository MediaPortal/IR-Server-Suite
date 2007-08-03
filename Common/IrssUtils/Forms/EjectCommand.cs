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

  public partial class EjectCommand : Form
  {

    #region Properties

    public string CommandString
    {
      get
      {
        return comboBoxDrive.Text;
      }
    }

    #endregion Properties

    #region Constructors

    public EjectCommand() : this(null) { }
    public EjectCommand(string command)
    {
      InitializeComponent();

      DriveInfo[] drives = DriveInfo.GetDrives();
      foreach (DriveInfo drive in drives)
      {
        if (drive.DriveType == DriveType.CDRom)
        {
          comboBoxDrive.Items.Add(drive.Name);
          if (drive.Name == command)
            comboBoxDrive.SelectedItem = drive.Name;
        }
      }

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
