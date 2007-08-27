using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Microsoft.Win32;

namespace MicrosoftMceTransceiver
{

  public partial class Advanced : Form
  {

    #region Constants

    const string HidIrRegKey  = "SYSTEM\\CurrentControlSet\\Services\\HidIr";

    const string StartValue   = "Start";

    const int HidIrEnabled    = 3;
    const int HidIrDisabled   = 4;

    #endregion Constants

    #region Constructor

    public Advanced()
    {
      InitializeComponent();

      RegistryKey regKey = Registry.LocalMachine.CreateSubKey(HidIrRegKey);

      if ((int)regKey.GetValue(StartValue, HidIrDisabled) == HidIrDisabled)
        radioButtonDisabled.Checked = true;
      else
        radioButtonEnabled.Checked = true;
    }

    #endregion Constructor

    private void buttonOK_Click(object sender, EventArgs e)
    {
      RegistryKey regKey = Registry.LocalMachine.CreateSubKey(HidIrRegKey);

      int currentValue = (int)regKey.GetValue(StartValue, HidIrDisabled);

      bool changedValue = false;

      if (radioButtonEnabled.Checked)
      {
        if (currentValue == HidIrDisabled)
        {
          regKey.SetValue(StartValue, HidIrEnabled);
          changedValue = true;
        }
      }
      else if (radioButtonDisabled.Checked)
      {
        if (currentValue != HidIrDisabled)
        {
          regKey.SetValue(StartValue, HidIrDisabled);
          changedValue = true;
        }
      }

      if (changedValue)
        MessageBox.Show(this, "You must reboot for changes to take effect", "Reboot required", MessageBoxButtons.OK, MessageBoxIcon.Information);

      this.Close();
    }

  }

}
