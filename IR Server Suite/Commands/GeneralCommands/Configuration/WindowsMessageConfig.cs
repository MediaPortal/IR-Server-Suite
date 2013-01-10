using System;
using System.Windows.Forms;
using IrssUtils;
using IrssUtils.Forms;

namespace IrssCommands.General
{
  public partial class WindowsMessageConfig : BaseCommandConfig
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
            Enum.GetName(typeof (WindowTargetType), comboBoxTargetType.SelectedItem),
            textBoxTarget.Text.Trim(),
            numericUpDownMsg.Value.ToString(),
            numericUpDownWParam.Value.ToString(),
            numericUpDownLParam.Value.ToString()
          };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowsMessageConfig"/> class.
    /// </summary>
    private WindowsMessageConfig()
    {
      InitializeComponent();

      comboBoxTargetType.Items.Clear();
      foreach (WindowTargetType s in Enum.GetValues(typeof (WindowTargetType)))
        comboBoxTargetType.Items.Add(s);
      comboBoxTargetType.SelectedIndex = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowsMessageConfig"/> class.
    /// </summary>
    /// <param name="parameters">The command elements.</param>
    public WindowsMessageConfig(string[] parameters)
      : this()
    {
      comboBoxTargetType.SelectedItem =
        (WindowTargetType) Enum.Parse(typeof (WindowTargetType), parameters[0], true);

      textBoxTarget.Text = parameters[1];

      if (!String.IsNullOrEmpty(parameters[2]))
        numericUpDownMsg.Value = Convert.ToDecimal(parameters[2]);

      if (!String.IsNullOrEmpty(parameters[3]))
        numericUpDownWParam.Value = Convert.ToDecimal(parameters[3]);

      if (!String.IsNullOrEmpty(parameters[4]))
        numericUpDownLParam.Value = Convert.ToDecimal(parameters[4]);
    }

    #endregion Constructors

    #region Implementation

    private void comboBoxTargetType_SelectedValueChanged(object sender, EventArgs e)
    {
      if ((WindowTargetType) comboBoxTargetType.SelectedItem == WindowTargetType.Active)
      {
        buttonLocate.Enabled = false;
        textBoxTarget.Enabled = false;
      }
      else
      {
        buttonLocate.Enabled = true;
        textBoxTarget.Enabled = true;
      }
    }

    private void buttonLocate_Click(object sender, EventArgs e)
    {
      WindowTargetType type = (WindowTargetType) comboBoxTargetType.SelectedItem;

      switch (type)
      {
        case WindowTargetType.Application:
          {
            OpenFileDialog find = new OpenFileDialog();
            find.Filter = "All files|*.*";
            find.Multiselect = false;
            find.Title = "Application to target";

            if (find.ShowDialog(this) == DialogResult.OK)
              textBoxTarget.Text = find.FileName;
          }
          break;

        case WindowTargetType.Class:
          {
            WindowList windowList = new WindowList(true);
            if (windowList.ShowDialog(this) == DialogResult.OK)
              textBoxTarget.Text = windowList.SelectedItem;
          }
          break;

        case WindowTargetType.Window:
          {
            WindowList windowList = new WindowList(false);
            if (windowList.ShowDialog(this) == DialogResult.OK)
              textBoxTarget.Text = windowList.SelectedItem;
          }
          break;
      }
    }

    private void wMAPPToolStripMenuItem_Click(object sender, EventArgs e)
    {
      numericUpDownMsg.Value = new decimal((int) Win32.WindowsMessage.WM_APP);
    }

    private void wMUSERToolStripMenuItem_Click(object sender, EventArgs e)
    {
      numericUpDownMsg.Value = new decimal((int) Win32.WindowsMessage.WM_USER);
    }

    #endregion Implementation
  }
}