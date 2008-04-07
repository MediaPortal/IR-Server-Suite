using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace InputService.Plugin
{

  partial class AdvancedSettings : Form
  {

    public int InputByte
    {
      get { return decimal.ToInt32(numericUpDownInputByte.Value); }
      set { numericUpDownInputByte.Value = new decimal(value); }
    }
    public byte ByteMask
    {
      get { return decimal.ToByte(numericUpDownInputByteMask.Value); }
      set { numericUpDownInputByteMask.Value = new decimal(value); }
    }
    public bool UseAllBytes
    {
      get { return checkBoxUseAllBytes.Checked; }
      set { checkBoxUseAllBytes.Checked = value; }
    }
    public int RepeatDelay
    {
      get { return decimal.ToInt32(numericUpDownRepeatDelay.Value); }
      set { numericUpDownRepeatDelay.Value = new decimal(value); }
    }

    public AdvancedSettings()
    {
      InitializeComponent();
    }

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
