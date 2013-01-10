using System.IO;

namespace IrssCommands.General
{
  public partial class EjectConfig : BaseCommandConfig
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
            comboBoxDrive.Text
          };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EjectConfig"/> class.
    /// </summary>
    private EjectConfig()
    {
      InitializeComponent();

      comboBoxDrive.Items.Clear();
      DriveInfo[] drives = DriveInfo.GetDrives();
      foreach (DriveInfo drive in drives)
        if (drive.DriveType == DriveType.CDRom)
          comboBoxDrive.Items.Add(drive.Name);

      if (comboBoxDrive.Items.Count > 0)
        comboBoxDrive.SelectedIndex = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EjectConfig"/> class.
    /// </summary>
    /// <param name="parameters">The command elements.</param>
    public EjectConfig(string[] parameters)
      : this()
    {
      comboBoxDrive.Text = parameters[0];
    }

    #endregion Constructors
  }
}