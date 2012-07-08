#region Copyright (C) 2005-2009 Team MediaPortal

// Copyright (C) 2005-2009 Team MediaPortal
// http://www.team-mediaportal.com
// 
// This Program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2, or (at your option)
// any later version.
// 
// This Program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with GNU Make; see the file COPYING.  If not, write to
// the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
// http://www.gnu.org/copyleft/gpl.html

#endregion

using System;
using System.Windows.Forms;

namespace IRServer.Plugin
{
  internal partial class ConfigurationDialog : Form
  {
    #region Properties

    public int LearnTimeout
    {
      get { return Decimal.ToInt32(numericUpDownLearnTimeout.Value); }
      set { numericUpDownLearnTimeout.Value = new Decimal(value); }
    }

    public bool DisableMceServices
    {
      get { return checkBoxDisableMCEServices.Checked; }
      set { checkBoxDisableMCEServices.Checked = value; }
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

    public bool DisableAutomaticButtons
    {
      get { return checkBoxDisableAutomaticButtons.Checked; }
      set { checkBoxDisableAutomaticButtons.Checked = value; }
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

    public bool UseQwertzLayout
    {
      get { return checkBoxKeyboardQwertz.Checked; }
      set { checkBoxKeyboardQwertz.Checked = value; }
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

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationDialog"/> class.
    /// </summary>
    public ConfigurationDialog()
    {
      InitializeComponent();
      this.Icon = Properties.Resources.Icon;

      // Put this in a try...catch so that if the registry keys don't exist we don't throw an ugly exception.
      try
      {
        checkBoxDisableAutomaticButtons.Checked = !MCEBasicMP.CheckAutomaticButtons();
      }
      catch
      {
        checkBoxDisableAutomaticButtons.Enabled = false;
      }
    }

    #endregion Constructor

    #region Buttons

    private void buttonOK_Click(object sender, EventArgs e)
    {
      if (checkBoxDisableAutomaticButtons.Enabled)
      {
        try
        {
          bool changeMade = false;

          bool keysExist = MCEBasicMP.CheckAutomaticButtons();

          if (checkBoxDisableAutomaticButtons.Checked && keysExist)
          {
            MCEBasicMP.DisableAutomaticButtons();
            changeMade = true;
          }
          else if (!checkBoxDisableAutomaticButtons.Checked && !keysExist)
          {
            MCEBasicMP.EnableAutomaticButtons();
            changeMade = true;
          }

          if (changeMade)
            MessageBox.Show(this,
                            "You must restart the computer for changes to automatic button handling to take effect",
                            "Restart required", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
          MessageBox.Show(this, ex.ToString(), "Error modifiying the system registry", MessageBoxButtons.OK);
        }
      }

      DialogResult = DialogResult.OK;
      Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    #endregion Buttons

    #region CheckBoxes

    private void checkBoxEnableRemote_CheckedChanged(object sender, EventArgs e)
    {
      remotePanel.Enabled = checkBoxEnableRemote.Checked;
    }

    private void checkBoxUseSystemRatesRemote_CheckedChanged(object sender, EventArgs e)
    {
      groupBoxRemoteTiming.Enabled = !checkBoxUseSystemRatesRemote.Checked;
    }

    private void checkBoxEnableKeyboard_CheckedChanged(object sender, EventArgs e)
    {
      keyboardPanel.Enabled = checkBoxEnableKeyboard.Checked;
    }

    private void checkBoxUseSystemRatesKeyboard_CheckedChanged(object sender, EventArgs e)
    {
      groupBoxKeypressTiming.Enabled = !checkBoxUseSystemRatesKeyboard.Checked;
    }

    private void checkBoxEnableMouse_CheckedChanged(object sender, EventArgs e)
    {
      mousePanel.Enabled = checkBoxEnableMouse.Checked;
    }

    #endregion
  }
}