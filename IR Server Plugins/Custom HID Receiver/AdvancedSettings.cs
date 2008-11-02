using System;
using System.Windows.Forms;

namespace InputService.Plugin
{
  internal partial class AdvancedSettings : Form
  {
    public AdvancedSettings()
    {
      InitializeComponent();
    }

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

    private void buttonOK_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.OK;
      Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }
  }
}