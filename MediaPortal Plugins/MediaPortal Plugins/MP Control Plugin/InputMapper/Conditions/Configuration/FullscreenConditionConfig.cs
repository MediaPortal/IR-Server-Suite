namespace MediaPortal.Input
{
  public partial class FullscreenConditionConfig : BaseConditionConfig
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="FullscreenConditionConfig"/> class.
    /// </summary>
    private FullscreenConditionConfig()
    {
      InitializeComponent();

      radioButtonFullscreen.Checked = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FullscreenConditionConfig"/> class.
    /// </summary>
    /// <param name="property">The condition property.</param>
    public FullscreenConditionConfig(string property)
      : this()
    {
      if (string.IsNullOrEmpty(property)) return;

      bool isFullscreen = bool.Parse(property);

      if (isFullscreen)
        radioButtonFullscreen.Checked = true;
      else
        radioButtonNoFullscreen.Checked = true;
    }

    #endregion Constructors

    private void CheckedChanged(object sender, System.EventArgs e)
    {
      Property = radioButtonFullscreen.Checked.ToString();
    }
  }
}