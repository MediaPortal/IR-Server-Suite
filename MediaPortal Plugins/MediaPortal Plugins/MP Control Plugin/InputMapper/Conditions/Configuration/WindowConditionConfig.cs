using System;

namespace MediaPortal.Input
{
  public partial class WindowConditionConfig : BaseConditionConfig
  {
    #region Constants

    private const string UNKOWN_WINDOW_ID = "[unknown]";

    #endregion Constants

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowConditionConfig"/> class.
    /// </summary>
    private WindowConditionConfig()
    {
      InitializeComponent();

      comboBox.Items.Add(UNKOWN_WINDOW_ID);
      comboBox.Items.AddRange(MPUtils.MPCommon.GetFrientlyWindowList());

      comboBox.SelectedIndex = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowConditionConfig"/> class.
    /// </summary>
    /// <param name="property">The condition property.</param>
    public WindowConditionConfig(string property)
      : this()
    {
      if (string.IsNullOrEmpty(property)) return;

      numericUpDown.Value = int.Parse(property);
    }

    #endregion Constructors

    private bool comboBoxSelection;

    private void numericUpDown_ValueChanged(object sender, EventArgs e)
    {
      if (comboBoxSelection) return;

      Property = numericUpDown.Value.ToString();

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