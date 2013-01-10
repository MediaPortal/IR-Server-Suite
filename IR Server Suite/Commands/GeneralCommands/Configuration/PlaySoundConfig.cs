using System;
using System.Windows.Forms;

namespace IrssCommands.General
{
  public partial class PlaySoundConfig : BaseCommandConfig
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
            textBoxPath.Text
          };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaySoundConfig"/> class.
    /// </summary>
    private PlaySoundConfig()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaySoundConfig"/> class.
    /// </summary>
    /// <param name="parameters">The command elements.</param>
    public PlaySoundConfig(string[] parameters)
      : this()
    {
      textBoxPath.Text = parameters[0];
    }

    #endregion Constructors

    #region Implementation

    private void buttonBrowse_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "Wave Files|*.wav";
      openFileDialog.Multiselect = false;


      if (openFileDialog.ShowDialog(this) == DialogResult.OK)
        textBoxPath.Text = openFileDialog.FileName;
    }

    #endregion Implementation
  }
}