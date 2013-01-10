using System;
using System.Windows.Forms;
using IrssUtils;
using IrssUtils.Forms;

namespace IrssCommands.General
{
  public partial class WindowStateConfig : BaseCommandConfig
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
            Enum.GetName(typeof (WindowStateAction), comboBoxAction.SelectedItem),
            Enum.GetName(typeof (WindowTargetType), comboBoxTargetType.SelectedItem),
            textBoxTarget.Text.Trim()
          };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowStateConfig"/> class.
    /// </summary>
    private WindowStateConfig()
    {
      InitializeComponent();

      comboBoxAction.Items.Clear();
      foreach (WindowStateAction s in Enum.GetValues(typeof (WindowStateAction)))
        comboBoxAction.Items.Add(s);
      comboBoxAction.SelectedIndex = 0;

      comboBoxTargetType.Items.Clear();
      foreach (WindowTargetType s in Enum.GetValues(typeof (WindowTargetType)))
        comboBoxTargetType.Items.Add(s);
      comboBoxTargetType.SelectedIndex = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowStateConfig"/> class.
    /// </summary>
    /// <param name="parameters">The command elements.</param>
    public WindowStateConfig(string[] parameters)
      : this()
    {
      comboBoxAction.SelectedItem =
        (WindowStateAction) Enum.Parse(typeof (WindowStateAction), parameters[0], true);
      comboBoxTargetType.SelectedItem =
        (WindowTargetType) Enum.Parse(typeof (WindowTargetType), parameters[1], true);
      textBoxTarget.Text = parameters[2];
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

    #endregion Implementation
  }
}