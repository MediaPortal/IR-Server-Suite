using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
      get { return (BlasterType)Enum.Parse(typeof(BlasterType), comboBoxBlasterType.SelectedItem as string); }
      set { comboBoxBlasterType.SelectedItem = Enum.GetName(typeof(BlasterType), value); }
    }

    public int RepeatDelay
    {
      get { return Decimal.ToInt32(numericUpDownButtonRepeatDelay.Value); }
      set { numericUpDownButtonRepeatDelay.Value = new Decimal(value); }
    }
    public int HeldDelay
    {
      get { return Decimal.ToInt32(numericUpDownButtonHeldDelay.Value); }
      set { numericUpDownButtonHeldDelay.Value = new Decimal(value); }
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
