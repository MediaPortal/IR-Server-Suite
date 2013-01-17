namespace MediaPortal.Input
{
  public partial class PluginEnabledConditionConfig : BaseConditionConfig
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginEnabledConditionConfig"/> class.
    /// </summary>
    private PluginEnabledConditionConfig()
    {
      InitializeComponent();

      comboBox.Items.AddRange(MPUtils.MPCommon.GetAvailablePlugins().ToArray());
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginEnabledConditionConfig"/> class.
    /// </summary>
    /// <param name="property">The condition property.</param>
    public PluginEnabledConditionConfig(string property)
      : this()
    {
      if (string.IsNullOrEmpty(property)) return;

      if (!comboBox.Items.Contains(property))
        comboBox.Items.Add(property);
      comboBox.Text = property;
    }

    #endregion Constructors

    private void comboBox_TextChanged(object sender, System.EventArgs e)
    {
      Property = comboBox.Text;
    }
  }
}