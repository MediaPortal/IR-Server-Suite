using System;
using System.Windows.Forms;

namespace InputService.Plugin
{
  internal partial class Configuration : Form
  {
    public Configuration()
    {
      InitializeComponent();
      comboBoxHardwareMode.Items.AddRange(Enum.GetNames(typeof (iMonUSBReceivers.RcMode)));
      comboBoxRemoteMode.Items.AddRange(Enum.GetNames(typeof (iMonUSBReceivers.RemoteMode)));
    }

    public iMonUSBReceivers.RcMode HardwareMode
    {
      get
      {
        return
          (iMonUSBReceivers.RcMode)
          Enum.Parse(typeof (iMonUSBReceivers.RcMode), comboBoxHardwareMode.SelectedItem as string);
      }
      set { comboBoxHardwareMode.SelectedItem = Enum.GetName(typeof (iMonUSBReceivers.RcMode), value); }
    }

    public iMonUSBReceivers.RemoteMode RemoteMode
    {
      get
      {
        return
          (iMonUSBReceivers.RemoteMode)
          Enum.Parse(typeof (iMonUSBReceivers.RemoteMode), comboBoxRemoteMode.SelectedItem as string);
      }
      set { comboBoxRemoteMode.SelectedItem = Enum.GetName(typeof (iMonUSBReceivers.RemoteMode), value); }
    }

    public bool EnableRemote
    {
      get { return checkBoxEnableRemote.Checked; }
      set { checkBoxEnableRemote.Checked = value; }
    }

    public bool UseSystemRatesForRemote
    {
      get { return checkBoxUseSystemRatesRemote.Checked; }
      set { checkBoxUseSystemRatesRemote.Checked = value; }
    }

    public int RemoteRepeatDelay
    {
      get { return Decimal.ToInt32(numericUpDownButtonRepeatDelay.Value); }
      set { numericUpDownButtonRepeatDelay.Value = new Decimal(value); }
    }

    public int RemoteHeldDelay
    {
      get { return Decimal.ToInt32(numericUpDownButtonHeldDelay.Value); }
      set { numericUpDownButtonHeldDelay.Value = new Decimal(value); }
    }

    public bool EnableKeyboard
    {
      get { return checkBoxEnableKeyboard.Checked; }
      set { checkBoxEnableKeyboard.Checked = value; }
    }

    public bool UseSystemRatesForKeyboard
    {
      get { return checkBoxUseSystemRatesKeyboard.Checked; }
      set { checkBoxUseSystemRatesKeyboard.Checked = value; }
    }

    public int KeyboardRepeatDelay
    {
      get { return Decimal.ToInt32(numericUpDownKeyRepeatDelay.Value); }
      set { numericUpDownKeyRepeatDelay.Value = new Decimal(value); }
    }

    public int KeyboardHeldDelay
    {
      get { return Decimal.ToInt32(numericUpDownKeyHeldDelay.Value); }
      set { numericUpDownKeyHeldDelay.Value = new Decimal(value); }
    }

    public bool HandleKeyboardLocal
    {
      get { return checkBoxHandleKeyboardLocal.Checked; }
      set { checkBoxHandleKeyboardLocal.Checked = value; }
    }

    public bool EnableMouse
    {
      get { return checkBoxEnableMouse.Checked; }
      set { checkBoxEnableMouse.Checked = value; }
    }

    public double MouseSensitivity
    {
      get { return Decimal.ToDouble(numericUpDownMouseSensitivity.Value); }
      set { numericUpDownMouseSensitivity.Value = new Decimal(value); }
    }

    public bool HandleMouseLocal
    {
      get { return checkBoxHandleMouseLocal.Checked; }
      set { checkBoxHandleMouseLocal.Checked = value; }
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