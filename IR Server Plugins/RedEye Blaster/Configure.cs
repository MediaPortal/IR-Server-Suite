using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

namespace InputService.Plugin
{

  partial class Configure : Form
  {

    #region Variables

    string[] _ports;

    #endregion Variables
    
    #region Properties

    /// <summary>
    /// Gets or sets the communications port.
    /// </summary>
    /// <value>The communications port.</value>
    public string CommPort
    {
      get { return comboBoxPort.SelectedItem as string; }
      set { comboBoxPort.SelectedItem = value; }
    }

    /// <summary>
    /// Gets or sets the blaster mode.
    /// </summary>
    /// <value>The blaster mode.</value>
    public BlastMode BlasterMode
    {
      get
      {
        if (radioButtonIRDA.Checked)      return BlastMode.IRDA;
        else if (radioButtonRC5.Checked)  return BlastMode.RC5;
        else                              return BlastMode.Sky;
      }
      set
      {
        switch (value)
        {
          case BlastMode.IRDA:  radioButtonIRDA.Checked = true;   break;
          case BlastMode.RC5:   radioButtonRC5.Checked = true;    break;
          case BlastMode.Sky:   radioButtonSky.Checked = true;    break;
        }
      }
    }

    #endregion Properties

    #region Constructor

    public Configure()
    {
      InitializeComponent();

      comboBoxPort.Items.Clear();

      _ports = SerialPort.GetPortNames();
      if (_ports == null || _ports.Length == 0)
      {
        MessageBox.Show(this, "No available serial ports found!", "IRMan Receiver", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      comboBoxPort.Items.AddRange(_ports);
    }

    #endregion Constructor

    #region Buttons

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

    #endregion Buttons

  }

}
