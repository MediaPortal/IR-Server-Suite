using System;
using System.Windows.Forms;

namespace VirtualRemote
{
  /// <summary>
  /// Server Address form.
  /// </summary>
  internal partial class ServerAddress : Form
  {
    #region Properties

    /// <summary>
    /// Gets the server host.
    /// </summary>
    /// <value>The server host.</value>
    public string ServerHost
    {
      get { return textBoxComputer.Text; }
    }

    #endregion Properties

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerAddress"/> class.
    /// </summary>
    public ServerAddress()
    {
      InitializeComponent();

      inputPanel.Enabled = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerAddress"/> class.
    /// </summary>
    /// <param name="serverHost">The server host.</param>
    public ServerAddress(string serverHost) : this()
    {
      if (!String.IsNullOrEmpty(serverHost))
        textBoxComputer.Text = serverHost;
    }

    #endregion Constructor

    #region Implementation

    private void buttonOK_Click(object sender, EventArgs e)
    {
      inputPanel.Enabled = false;

      DialogResult = DialogResult.OK;
      Close();
    }

    #endregion Implementation
  }
}