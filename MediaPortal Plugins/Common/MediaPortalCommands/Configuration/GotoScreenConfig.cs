using System;

namespace IrssCommands.MediaPortal.Configuration
{
  public partial class GotoScreenConfig : BaseCommandConfig
  {
    #region Constants

    private const string UNKOWN_WINDOW_ID = "[unknown]";

    #endregion Constants

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
            numericUpDown.Value.ToString()
          };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="GotoScreenConfig"/> class.
    /// </summary>
    private GotoScreenConfig()
    {
      InitializeComponent();

      comboBox.Items.Add(UNKOWN_WINDOW_ID);
      comboBox.Items.AddRange(MPUtils.MPCommon.GetFrientlyWindowList());

      comboBox.SelectedIndex = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GotoScreenConfig"/> class.
    /// </summary>
    /// <param name="parameters">The command elements.</param>
    public GotoScreenConfig(string[] parameters)
      : this()
    {
      numericUpDown.Value = int.Parse(parameters[0]);
    }

    #endregion Constructors

    private bool comboBoxSelection;

    private void numericUpDown_ValueChanged(object sender, EventArgs e)
    {
      if (comboBoxSelection) return;

      string friendlyName = MPUtils.MPCommon.GetFriendlyWindowName((int) numericUpDown.Value);
      if (friendlyName.Equals(numericUpDown.Value.ToString()))
      {
        // no friendly name found
        comboBox.SelectedItem = UNKOWN_WINDOW_ID;
        return;
      }

      comboBoxSelection = true;
      comboBox.SelectedItem = friendlyName;
      comboBoxSelection = false;
    }

    private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (comboBox.SelectedItem.Equals(UNKOWN_WINDOW_ID)) return;
      if (comboBoxSelection) return;

      numericUpDown.Value = MPUtils.MPCommon.GetWindowID(comboBox.SelectedItem as string);
    }

    private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
    {
      IrssUtils.IrssHelp.Open(MPUtils.MPCommon.MISSING_FRIENDLY_WINDOW_NAME_HELPID);
    }
  }
}