using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using IrssUtils;

namespace WebRemote
{

  /// <summary>
  /// Setup Form.
  /// </summary>
  public partial class Setup : Form
  {

    #region Properties

    /// <summary>
    /// Gets or sets the server host.
    /// </summary>
    /// <value>The server host.</value>
    public string ServerHost
    {
      get { return comboBoxComputer.Text; }
      set { comboBoxComputer.Text = value; }
    }

    /// <summary>
    /// Gets or sets the remote skin.
    /// </summary>
    /// <value>The remote skin.</value>
    public string RemoteSkin
    {
      get { return comboBoxSkin.Text; }
      set { comboBoxSkin.Text = value; }
    }

    /// <summary>
    /// Gets or sets the web server port.
    /// </summary>
    /// <value>The web server port.</value>
    public int WebPort
    {
      get { return Decimal.ToInt32(numericUpDownWebPort.Value); }
      set { numericUpDownWebPort.Value = new Decimal(value); }
    }
    
    #endregion Properties

    #region Constructor

    public Setup()
    {
      InitializeComponent();

      UpdateComputerList();

      UpdateSkinList();
    }

    #endregion Constructor

    #region Implementation

    void UpdateComputerList()
    {
      comboBoxComputer.Items.Clear();
      comboBoxComputer.Items.Add("localhost");

      List<string> networkPCs = Network.GetComputers(false);
      if (networkPCs != null)
        comboBoxComputer.Items.AddRange(networkPCs.ToArray());
    }

    void UpdateSkinList()
    {
      try
      {
        string[] skins = Directory.GetFiles(Program.InstallFolder + "\\Skins\\", "*.png", SearchOption.TopDirectoryOnly);
        for (int index = 0; index < skins.Length; index++)
          skins[index] = Path.GetFileNameWithoutExtension(skins[index]);

        comboBoxSkin.Items.Clear();
        comboBoxSkin.Items.AddRange(skins);

        comboBoxSkin.SelectedItem = Program.RemoteSkin;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.Message);
      }
    }

    #endregion Implementation

    private void buttonOK_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }
    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

  }

}
