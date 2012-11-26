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
    internal partial class Configuration : Form
    {
        public Configuration()
        {
            InitializeComponent();
            comboBoxRemoteMode.Items.AddRange(Enum.GetNames(typeof(iMonUSBReceivers.RemoteMode)));
            comboBoxPadMode.Items.AddRange(Enum.GetNames(typeof(iMonUSBReceivers.PadMode)));
        }

        public iMonUSBReceivers.RemoteMode RemoteMode
        {
            get
            {
                return
                  (iMonUSBReceivers.RemoteMode)
                  Enum.Parse(typeof(iMonUSBReceivers.RemoteMode), comboBoxRemoteMode.SelectedItem as string);
            }
            set { comboBoxRemoteMode.SelectedItem = Enum.GetName(typeof(iMonUSBReceivers.RemoteMode), value); }
        }

        public iMonUSBReceivers.PadMode PadMode
        {
            get
            {
                return
                  (iMonUSBReceivers.PadMode)
                  Enum.Parse(typeof(iMonUSBReceivers.PadMode), comboBoxPadMode.SelectedItem as string);
            }
            set { comboBoxPadMode.SelectedItem = Enum.GetName(typeof(iMonUSBReceivers.PadMode), value); }
        }

        public bool EnableRemote
        {
            get { return checkBoxEnableRemote.Checked; }
            set { checkBoxEnableRemote.Checked = value; }
        }

        public bool UsePadSwitch
        {
            get { return checkBoxUsePadSwitch.Checked; }
            set { checkBoxUsePadSwitch.Checked = value; }
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

        public int KeyPadSensitivity
        {
            get { return trackBarKeyPadSensitivity.Value; }
            set { trackBarKeyPadSensitivity.Value = value; }
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
            get { return ((double)trackBarMouseSensitivity.Value / 10); }
            set
            {
                trackBarMouseSensitivity.Value = (int)(value * 10);
                trackBarMouseSensitivity_Scroll((object) null, (EventArgs) null);
            }
        }

        private void trackBarMouseSensitivity_Scroll(object sender, EventArgs e)
        {
            labelMouseSensitivity.Text = string.Format("{0:0.0}", (double)trackBarMouseSensitivity.Value / 10);
        }

        public bool HandleMouseLocal
        {
            get { return checkBoxHandleMouseLocal.Checked; }
            set { checkBoxHandleMouseLocal.Checked = value; }
        }

        public bool KillImonM
        {
            get { return checkBoxKillImonM.Checked; }
            set { checkBoxKillImonM.Checked = value; }
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