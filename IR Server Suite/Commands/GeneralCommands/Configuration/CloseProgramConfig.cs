using System;
using System.Windows.Forms;
using IrssUtils;
using IrssUtils.Forms;

namespace IrssCommands.General
{
  public partial class CloseProgramConfig : BaseCommandConfig
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
          };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CloseProgramConfig"/> class.
    /// </summary>
    private CloseProgramConfig()
    {
      InitializeComponent();

      comboBoxTargetType.Items.Clear();
      foreach (WindowTargetType s in Enum.GetValues(typeof (WindowTargetType)))
        comboBoxTargetType.Items.Add(s);
      comboBoxTargetType.SelectedIndex = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CloseProgramConfig"/> class.
    /// </summary>
    /// <param name="parameters">The command elements.</param>
    public CloseProgramConfig(string[] parameters)
      : this()
    {
      comboBoxTargetType.SelectedItem =
        (WindowTargetType) Enum.Parse(typeof (WindowTargetType), parameters[0], true);
      textBoxTarget.Text = parameters[1];
    }

    #endregion Constructors

    #region Implementation

    private void comboBoxTargetType_SelectedValueChanged(object sender, EventArgs e)
    {
      if ((WindowTargetType) comboBoxTargetType.SelectedItem == WindowTargetType.Active)
      {
        label1.Enabled = false;
        buttonLocate.Enabled = false;
        textBoxTarget.Enabled = false;
      }
      else
      {
        label1.Enabled = true;
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

    #endregion Implementation
  }
}