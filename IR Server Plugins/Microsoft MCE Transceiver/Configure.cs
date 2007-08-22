using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MicrosoftMceTransceiver
{

  public partial class Configure : Form
  {

    #region Properties

    public BlasterType BlastType
    {
      get { return (BlasterType)Enum.Parse(typeof(BlasterType), comboBoxBlasterType.SelectedItem as string, true); }
      set { comboBoxBlasterType.SelectedItem = Enum.GetName(typeof(BlasterType), value); }
    }

    public int LearnTimeout
    {
      get { return Decimal.ToInt32(numericUpDownLearnTimeout.Value); }
      set { numericUpDownLearnTimeout.Value = new Decimal(value); }
    }

    public bool LearnAsPronto
    {
      get { return checkBoxLearnAsPronto.Checked; }
      set { checkBoxLearnAsPronto.Checked = value; }
    }

    public bool EnableRemote
    {
      get { return checkBoxEnableRemote.Checked; }
      set { checkBoxEnableRemote.Checked = value; }
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

    #endregion Properties

    #region Constructor

    public Configure()
    {
      InitializeComponent();

      comboBoxBlasterType.Items.Clear();
      comboBoxBlasterType.Items.AddRange(Enum.GetNames(typeof(BlasterType)));
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
