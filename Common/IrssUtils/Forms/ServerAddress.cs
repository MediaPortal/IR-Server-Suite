using System;
using System.Collections;
using System.Windows.Forms;

namespace IrssUtils.Forms
{

  /// <summary>
  /// Server Address form.
  /// </summary>
  public partial class ServerAddress : Form
  {

    #region Properties

    /// <summary>
    /// Gets the server host.
    /// </summary>
    /// <value>The server host.</value>
    public string ServerHost
    {
      get { return comboBoxComputer.Text; }
    }

    #endregion Properties

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerAddress"/> class.
    /// </summary>
    public ServerAddress()
    {
      InitializeComponent();

      comboBoxComputer.Items.Clear();
      comboBoxComputer.Items.Add("localhost");

      string[] networkPCs = Win32.GetNetworkComputers(false);
      if (networkPCs != null)
        comboBoxComputer.Items.AddRange(networkPCs);

      comboBoxComputer.SelectedIndex = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerAddress"/> class.
    /// </summary>
    /// <param name="serverHost">The server host.</param>
    public ServerAddress(string serverHost)
      : this()
    {
      if (!String.IsNullOrEmpty(serverHost))
        comboBoxComputer.Text = serverHost;
    }

    #endregion Constructor

    #region Implementation

    private void buttonOK_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    #endregion Implementation

  }

}
