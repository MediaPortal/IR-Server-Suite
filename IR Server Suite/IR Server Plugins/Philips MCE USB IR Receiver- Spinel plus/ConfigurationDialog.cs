#region Copyright (C) 2005-2012 Team MediaPortal

// Copyright (C) 2005-2012 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.Windows.Forms;

namespace IRServer.Plugin
{
  internal partial class ConfigurationDialog : Form
  {
    public ConfigurationDialog()
    {
      InitializeComponent();
      setEnabled();
    }

    public bool DoRepeats
    {
      get { return checkBoxDoRepeats.Checked; }
      set { checkBoxDoRepeats.Checked = value; }
    }

    public bool UseSystemRatesDelay
    {
      get { return checkBoxUseSystemRatesDelay.Checked; }
      set { checkBoxUseSystemRatesDelay.Checked = value; }
    }

    public int FirstRepeatDelay
    {
      get { return Decimal.ToInt32(numericUpDownFirstRepeatDelay.Value); }
      set { numericUpDownFirstRepeatDelay.Value = new Decimal(value); }
    }

    public int HeldRepeatDelay
    {
      get { return Decimal.ToInt32(numericUpDownHeldRepeatDelay.Value); }
      set { numericUpDownHeldRepeatDelay.Value = new Decimal(value); }
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

    private void checkBoxDoRepeats_CheckedChanged(object sender, EventArgs e)
    {
      setEnabled();
    }

    private void checkBoxUseSystemRatesDelay_CheckedChanged(object sender, EventArgs e)
    {
      setEnabled();
    }

    private void setEnabled()
    {
      checkBoxUseSystemRatesDelay.Enabled = checkBoxDoRepeats.Checked;
      numericUpDownFirstRepeatDelay.Enabled = checkBoxDoRepeats.Checked & !checkBoxUseSystemRatesDelay.Checked;
      numericUpDownHeldRepeatDelay.Enabled = checkBoxDoRepeats.Checked & !checkBoxUseSystemRatesDelay.Checked;
    }
  }
}