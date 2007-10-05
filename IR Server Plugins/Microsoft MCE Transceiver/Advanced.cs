using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;

using Microsoft.Win32;

namespace MicrosoftMceTransceiver
{

  [RegistryPermission(SecurityAction.Demand,
   Read = "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\HidIr",
   Write = "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\HidIr")]
  partial class Advanced : Form
  {

    #region Constants

    const string RegKey     = "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\HidIr";
    const string RegValue   = "Start";

    const int SetEnabled    = 3;
    const int SetDisabled   = 4;

    #endregion Constants

    #region Constructor

    public Advanced()
    {
      InitializeComponent();

      int currentValue = (int)Registry.GetValue(RegKey, RegValue, SetDisabled);

      if (currentValue == SetDisabled)
        radioButtonDisabled.Checked = true;
      else
        radioButtonEnabled.Checked = true;
    }

    #endregion Constructor

    private void buttonOK_Click(object sender, EventArgs e)
    {
      int currentValue = (int)Registry.GetValue(RegKey, RegValue, SetDisabled);

      bool changedValue = false;

      if (radioButtonEnabled.Checked)
      {
        if (currentValue == SetDisabled)
        {
          Registry.SetValue(RegKey, RegValue, SetEnabled, RegistryValueKind.DWord);
          changedValue = true;
        }
      }
      else if (radioButtonDisabled.Checked)
      {
        if (currentValue != SetDisabled)
        {
          Registry.SetValue(RegKey, RegValue, SetDisabled, RegistryValueKind.DWord);
          changedValue = true;
        }
      }

      if (changedValue)
        MessageBox.Show(this, "You must reboot for changes to take effect", "Reboot required", MessageBoxButtons.OK, MessageBoxIcon.Information);

      this.Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

  }

}
