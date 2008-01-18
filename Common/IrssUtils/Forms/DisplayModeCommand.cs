using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace IrssUtils.Forms
{

  /// <summary>
  /// Display Mode Command form.
  /// </summary>
  public partial class DisplayModeCommand : Form
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
        return String.Format("{0}|{1}|{2}|{3}",
          textBoxWidth.Text,
          textBoxHeight.Text,
          checkBoxBpp.Checked ? textBoxBpp.Text : "-1",
          checkBoxRefresh.Checked ? textBoxRefresh.Text : "-1");
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="DisplayModeCommand"/> class.
    /// </summary>
    public DisplayModeCommand()
    {
      InitializeComponent();

      Size res = Display.GetResolution();

      textBoxWidth.Text = res.Width.ToString();
      textBoxHeight.Text = res.Height.ToString();

      textBoxBpp.Text = Display.GetBpp().ToString();
      textBoxRefresh.Text = Display.GetRefreshRate().ToString();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DisplayModeCommand"/> class.
    /// </summary>
    /// <param name="commands">The command elements.</param>
    public DisplayModeCommand(string[] commands)
    {
      InitializeComponent();

      textBoxWidth.Text   = commands[0];
      textBoxHeight.Text  = commands[1];
      textBoxBpp.Text     = commands[2];
      textBoxRefresh.Text = commands[3];
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
