using System;
using System.Windows.Forms;

namespace IrssUtils.Forms
{
  /// <summary>
  /// Window Command form.
  /// </summary>
  public partial class WindowCommand : Form
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
        string action = "ERROR";
        string target = "ERROR";

        if (radioButtonRestore.Checked)
          action = "RESTORE";

        if (radioButtonActiveWindow.Checked)
        {
          target = Common.TargetActive;
          textBoxMsgTarget.Text = "*";
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

        return String.Format("{0}|{1}|{2}",
                             action,
                             target,
                             textBoxMsgTarget.Text);
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowCommand"/> class.
    /// </summary>
    public WindowCommand()
    {
      InitializeComponent();

      radioButtonRestore.Checked = true;
      radioButtonActiveWindow.Checked = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowCommand"/> class.
    /// </summary>
    /// <param name="commands">The command elements.</param>
    public WindowCommand(string[] commands) : this()
    {
      if (commands != null)
      {
        string action = commands[0].ToUpperInvariant();
        switch (action)
        {
          case "RESTORE":
            radioButtonRestore.Checked = true;
            break;
        }

        string target = commands[1].ToUpperInvariant();
        switch (target)
        {
          case Common.TargetActive:
            radioButtonActiveWindow.Checked = true;
            break;
          case Common.TargetApplication:
            radioButtonApplication.Checked = true;
            break;
          case Common.TargetClass:
            radioButtonClass.Checked = true;
            break;
          case Common.TargetWindow:
            radioButtonWindowTitle.Checked = true;
            break;
          default:
            throw new ArgumentOutOfRangeException("commands", commands[0], "Invalid window target");
        }

        textBoxMsgTarget.Text = commands[2];
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
        find.Title = "Application to target";

        if (find.ShowDialog(this) == DialogResult.OK)
          textBoxMsgTarget.Text = find.FileName;
      }
      else if (radioButtonClass.Checked)
      {
        WindowList windowList = new WindowList(true);
        if (windowList.ShowDialog(this) == DialogResult.OK)
          textBoxMsgTarget.Text = windowList.SelectedItem;
      }
      else if (radioButtonWindowTitle.Checked)
      {
        WindowList windowList = new WindowList(false);
        if (windowList.ShowDialog(this) == DialogResult.OK)
          textBoxMsgTarget.Text = windowList.SelectedItem;
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

    private void radioButtonActiveWindow_CheckedChanged(object sender, EventArgs e)
    {
      buttonFindMsgTarget.Enabled = false;
      textBoxMsgTarget.Enabled = false;
    }

    private void radioButtonApplication_CheckedChanged(object sender, EventArgs e)
    {
      buttonFindMsgTarget.Enabled = true;
      textBoxMsgTarget.Enabled = true;
    }

    private void radioButtonClass_CheckedChanged(object sender, EventArgs e)
    {
      buttonFindMsgTarget.Enabled = true;
      textBoxMsgTarget.Enabled = true;
    }

    private void radioButtonWindowTitle_CheckedChanged(object sender, EventArgs e)
    {
      buttonFindMsgTarget.Enabled = true;
      textBoxMsgTarget.Enabled = true;
    }

    #endregion Controls
  }
}