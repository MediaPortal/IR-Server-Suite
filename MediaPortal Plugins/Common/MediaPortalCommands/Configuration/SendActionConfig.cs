using System;
using Action = MediaPortal.GUI.Library.Action;

namespace IrssCommands.MediaPortal
{
  public partial class SendActionConfig : BaseCommandConfig
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
            comboBoxActionType.Text.Trim(),
            textBoxFloat1.Text.Trim(),
            textBoxFloat2.Text.Trim()
          };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SendActionConfig"/> class.
    /// </summary>
    private SendActionConfig()
    {
      InitializeComponent();
      SetupComboBox();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SendActionConfig"/> class.
    /// </summary>
    /// <param name="parameters">The command elements.</param>
    public SendActionConfig(string[] parameters)
      : this()
    {
      comboBoxActionType.Text = parameters[0];
      textBoxFloat1.Text = parameters[1];
      textBoxFloat2.Text = parameters[2];
    }

    #endregion Constructors

    private void SetupComboBox()
    {
      comboBoxActionType.Items.Clear();

      string[] items = Enum.GetNames(typeof(Action.ActionType));
      Array.Sort(items);

      comboBoxActionType.Items.AddRange(items);
    }
  }
}