using System;
using System.Collections;
using System.Windows.Forms;

namespace IrssUtils.Forms
{

  public partial class ServerAddress : Form
  {

    #region Properties

    public string ServerHost
    {
      get { return comboBoxComputer.Text; }
    }

    #endregion Properties

    #region Constructor

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
