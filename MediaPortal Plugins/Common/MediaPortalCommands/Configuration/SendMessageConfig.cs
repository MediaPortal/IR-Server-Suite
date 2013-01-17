using System;
using MediaPortal.GUI.Library;

namespace IrssCommands.MediaPortal
{
  public partial class SendMessageConfig : BaseCommandConfig
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
            comboBoxMessageType.Text.Trim(),
            textBoxWindowId.Text.Trim(),
            textBoxSenderId.Text.Trim(),
            textBoxControlId.Text.Trim(),
            textBoxParam1.Text.Trim(),
            textBoxParam2.Text.Trim()
          };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SendMessageConfig"/> class.
    /// </summary>
    private SendMessageConfig()
    {
      InitializeComponent();
      SetupComboBox();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SendMessageConfig"/> class.
    /// </summary>
    /// <param name="parameters">The command elements.</param>
    public SendMessageConfig(string[] parameters)
      : this()
    {
      comboBoxMessageType.Text = parameters[0];
      textBoxWindowId.Text = parameters[1];
      textBoxSenderId.Text = parameters[2];
      textBoxControlId.Text = parameters[3];
      textBoxParam1.Text = parameters[4];
      textBoxParam2.Text = parameters[5];
    }

    #endregion Constructors

    private void SetupComboBox()
    {
      comboBoxMessageType.Items.Clear();

      string[] items = Enum.GetNames(typeof(GUIMessage.MessageType));
      Array.Sort(items);

      comboBoxMessageType.Items.AddRange(items);
    }
  }
}