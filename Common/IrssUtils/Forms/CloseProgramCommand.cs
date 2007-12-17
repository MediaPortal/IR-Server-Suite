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
  /// Close Program Command form.
  /// </summary>
  public partial class CloseProgramCommand : Form
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
        string target = "ERROR";

        if (radioButtonActiveWindow.Checked)
        {
          target = Common.TargetActive;
          textBoxTarget.Text = "*";
        }
        else if (radioButtonApplication.Checked)
        {
          target = Common.TargetApplication;
        }
        else if (radioButtonClass.Checked)
        {
          target = Common.TargetClass;
        }
        else if (radioButtonWindowTitle.Checked)
        {
          target = Common.TargetWindow;
        }

        return String.Format("{0}|{1}",
          target,
          textBoxTarget.Text);
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CloseProgramCommand"/> class.
    /// </summary>
    public CloseProgramCommand() : this(new string[] { Common.TargetActive, String.Empty }) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="CloseProgramCommand"/> class.
    /// </summary>
    /// <param name="commands">The command elements.</param>
    public CloseProgramCommand(string[] commands)
    {
      InitializeComponent();

      if (commands != null)
      {
        string target = commands[0].ToUpperInvariant();
        switch (target)
        {
          case Common.TargetActive:       radioButtonActiveWindow.Checked = true;   break;
          case Common.TargetApplication:  radioButtonApplication.Checked  = true;   break;
          case Common.TargetClass:        radioButtonClass.Checked        = true;   break;
          case Common.TargetWindow:       radioButtonWindowTitle.Checked  = true;   break;
          default:
            throw new ArgumentOutOfRangeException("commands", commands[0], "Invalid message target");
        }

        textBoxTarget.Text = commands[1];
      }
    }

    #endregion Constructors

    #region Controls

    private void buttonFindMsgApp_Click(object sender, EventArgs e)
    {
      if (radioButtonApplication.Checked)
      {
        OpenFileDialog find = new OpenFileDialog();
        find.Filter = "All files|*.*";
        find.Multiselect = false;
        find.Title = "Application to close";

        if (find.ShowDialog(this) == DialogResult.OK)
            textBoxTarget.Text = find.FileName;
      }
      else if (radioButtonClass.Checked)
      {
        // TODO: Locate Class
      }
      else if (radioButtonWindowTitle.Checked)
      {
        WindowList windowList = new WindowList();
        if (windowList.ShowDialog(this) == DialogResult.OK)
          textBoxTarget.Text = windowList.SelectedWindowTitle;
      }
    }

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

    private void radioButtonActiveWindow_CheckedChanged(object sender, EventArgs e)
    {
      buttonFindTarget.Enabled = false;
      textBoxTarget.Enabled = false;
    }
    private void radioButtonApplication_CheckedChanged(object sender, EventArgs e)
    {
      buttonFindTarget.Enabled = true;
      textBoxTarget.Enabled = true;
    }
    private void radioButtonClass_CheckedChanged(object sender, EventArgs e)
    {
      buttonFindTarget.Enabled = false;
      textBoxTarget.Enabled = true;
    }
    private void radioButtonWindowTitle_CheckedChanged(object sender, EventArgs e)
    {
      buttonFindTarget.Enabled = true;
      textBoxTarget.Enabled = true;
    }

    #endregion Controls

  }

}
