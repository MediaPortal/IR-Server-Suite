using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MceReplacementTransceiver
{

  public partial class Configure : Form
  {

    #region Properties

    public MceIrApi.BlasterType BlastType
    {
      get { return (MceIrApi.BlasterType)Enum.Parse(typeof(MceIrApi.BlasterType), comboBoxBlasterType.SelectedItem as string); }
      set { comboBoxBlasterType.SelectedItem = Enum.GetName(typeof(MceIrApi.BlasterType), value); }
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

    public int LearnTimeout
    {
      get { return Decimal.ToInt32(numericUpDownLearnTimeout.Value); }
      set { numericUpDownLearnTimeout.Value = new Decimal(value); }
    }

    #endregion Properties

    #region Constructor

    public Configure()
    {
      InitializeComponent();

      comboBoxBlasterType.Items.Clear();
      comboBoxBlasterType.Items.AddRange(Enum.GetNames(typeof(MceIrApi.BlasterType)));
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
