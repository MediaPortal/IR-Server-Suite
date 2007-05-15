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

    public ServerAddress(string serverHost)
    {
      InitializeComponent();

      ArrayList networkPCs = Win32.GetNetworkComputers();
      if (networkPCs != null)
      {
        comboBoxComputer.Items.AddRange(networkPCs.ToArray());
        comboBoxComputer.Text = serverHost;
      }
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
