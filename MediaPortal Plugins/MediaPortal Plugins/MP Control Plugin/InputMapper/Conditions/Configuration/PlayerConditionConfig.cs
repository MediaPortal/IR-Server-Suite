using System;

namespace MediaPortal.Input
{
  public partial class PlayerConditionConfig : BaseConditionConfig
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerConditionConfig"/> class.
    /// </summary>
    private PlayerConditionConfig()
    {
      InitializeComponent();
      comboBox.DataSource = PlayerCondition.PlayerTexts;
      comboBox.SelectedIndex = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerConditionConfig"/> class.
    /// </summary>
    /// <param name="property">The condition property.</param>
    public PlayerConditionConfig(string property)
      : this()
    {
      if (string.IsNullOrEmpty(property)) return;

      PlayerCondition.PlayerType playerType =
        (PlayerCondition.PlayerType)Enum.Parse(typeof(PlayerCondition.PlayerType), property, true);

      comboBox.SelectedIndex = (int)playerType;
    }

    #endregion Constructors

    private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      PlayerCondition.PlayerType playerType = (PlayerCondition.PlayerType)comboBox.SelectedIndex;
      Property = Enum.GetName(typeof(PlayerCondition.PlayerType), playerType);
    }
  }
}