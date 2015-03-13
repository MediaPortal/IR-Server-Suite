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
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace IrssUtils.Forms
{
    /// <summary>
    /// Learn IR form.
    /// </summary>
    public partial class IREditor : Form
    {


        // --------------------------------------------------------------------------------------------------
        #region Variables

        internal static readonly string tempfile = Path.Combine(Common.FolderIRCommands, Common.FileExtensionIR);

        private readonly BlastIrDelegate _blastIrDelegate;
        private readonly LearnIrDelegate _learnIrDelegate;

        private bool _editionEnabled;
        private bool _insertionEnabled;
        private bool _blastEnabled;
        private bool _IrNameValid;
        private bool _codeValid;
        private string _name;

        #endregion Variables


        // --------------------------------------------------------------------------------------------------
        #region Properties

        /// <summary>
        /// Gets the command string.
        /// </summary>
        /// <value>The command string.</value>
        public string CommandString
        {
            get
            {
                return Common.JoinCommand(new string[] {
                           textBoxName.Text,
                           comboBoxPort.SelectedItem as string
                           });
            }
        }

        /// <summary>
        /// Gets or sets the blaster port.
        /// </summary>
        /// <value>The blaster port.</value>
        public string BlasterPort
        {
            get { return comboBoxPort.SelectedItem as string; }
            set { comboBoxPort.SelectedItem = value; }
        }

        /// <summary>
        /// Get the Blast-command name
        /// </summary>
        public string BlastName
        {
            get { return textBoxName.Text; }
        } 

        #endregion Properties


        // --------------------------------------------------------------------------------------------------
        #region Constructor

        /// <summary>
        /// Create, insert or modify an IR Command.
        /// </summary>
        /// <param name="learnIrDelegate">Delegate to call to start the IR learn process.</param>
        /// <param name="blastIrDelegate">Delegate to call to test an IR Command.</param>
        /// <param name="ports">Available blast ports to transmit on.</param>
        /// <param name="name">Name of the existing IR Command, "" if new command.</param>
        public IREditor(LearnIrDelegate learnIrDelegate, BlastIrDelegate blastIrDelegate, string[] ports, string name = "")
        {
            _learnIrDelegate = learnIrDelegate;
            _blastIrDelegate = blastIrDelegate;
            _name = name;
            if (name == null) _name = "";

            _editionEnabled = learnIrDelegate != null;
            _blastEnabled = blastIrDelegate != null && ports != null;
            _insertionEnabled = name != "";
            _codeValid = name != "";

            InitializeComponent();
            
            // Initialize temporary file
            if (_editionEnabled)
            {
                if (name != "")
                {
                    try
                    {
                        string filename = Path.Combine(Common.FolderIRCommands, name + Common.FileExtensionIR);
                        if (File.Exists(tempfile)) File.Delete(tempfile);
                        File.Copy(filename, tempfile, true);
                        labelStatus.Text = "IR Command is unchanged";
                    }
                    catch
                    {
                        _editionEnabled = false;
                    }
                }
            }


            comboBoxPort.Items.Clear();
            comboBoxPort.Items.AddRange(ports);
            comboBoxPort.SelectedIndex = 0;

            textBoxName.Text = name;
            comboBoxPort.Enabled = _blastEnabled && _codeValid;
            buttonTest.Enabled = _blastEnabled && _codeValid;
            buttonLearn.Enabled = _editionEnabled;
            textBoxName.Enabled = _editionEnabled;

            EditedIr = !_insertionEnabled;

        }

        #endregion Constructor


        // --------------------------------------------------------------------------------------------------
        #region Edition

        private bool _editedIr;
        private bool EditedIr
        {
            get { return _editedIr; }
            set
            {
                _editedIr = value && _editionEnabled;
                if (_editedIr)
                {
                    buttonOk.Enabled = _IrNameValid && _codeValid;
                    buttonOk.Text = "Save";
                    this.buttonOk.Image = global::IrssUtils.Properties.Resources.Save;
                    this.toolTipIr.SetToolTip(this.buttonOk, "Save this IR command (Ctrl + S)");
                    buttonCancel.Text = "Cancel";
                    this.toolTipIr.SetToolTip(this.buttonCancel, "Discard all changes and close editor (Esc)");
                }
                else if (_insertionEnabled)
                {
                    buttonOk.Enabled = _IrNameValid;
                    buttonOk.Text = "Insert";
                    this.buttonOk.Image = global::IrssUtils.Properties.Resources.ScrollLeft;
                    this.toolTipIr.SetToolTip(this.buttonOk, "Insert this IR command in the calling list and close editor (Enter)");
                    buttonCancel.Text = "Cancel";
                    this.toolTipIr.SetToolTip(this.buttonCancel, "Do not insert and close editor (Esc)");
                }
                else
                {
                    buttonOk.Enabled = false;
                    buttonCancel.Text = "Close";
                    this.toolTipIr.SetToolTip(this.buttonCancel, "Close IR command (Esc)");
                }
            }
        }


        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            _IrNameValid = Common.IsValidFileName(textBoxName.Text);
            labelInvalid.Visible = textBoxName.Text.Length > 0 && !_IrNameValid;
            EditedIr = true;
        }

        #endregion Edition


        // --------------------------------------------------------------------------------------------------
        #region Key mapping

        private void LearnIR_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift || e.Alt) return;
            if (e.Control)
            {
                if (e.KeyCode == Keys.S && EditedIr)
                {
                    buttonOk_Click(null, null);
                    e.Handled = true;
                }
            }
            else if (e.KeyCode == Keys.F5)
            {
                buttonTest_Click(null, null);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.F7 && _editionEnabled)
            {
                buttonLearn_Click(null, null);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter && !EditedIr && _insertionEnabled)
            {
                buttonOk_Click(null, null);
                e.Handled = true;
            }
        }

        #endregion Key mapping


        // --------------------------------------------------------------------------------------------------
        #region Delegates

        /// <summary>
        /// Updates the Learn IR status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="success">Success status.</param>
        public void LearnStatus(string status, bool success)
        {
            if (labelStatus.InvokeRequired)
            {
                Invoke(new LearnStatusDelegate(LearnStatus), new object[] { status, success });
            }
            else
            {
                labelStatus.Text = status;
                labelStatus.ForeColor = success ? Color.Green : Color.Red;
                labelStatus.Height = 23;
                labelStatus.Location = new System.Drawing.Point(8, 68);
                labelStatus.Update();

                _codeValid = success;
                EditedIr = true;

                textBoxName.Enabled = _editionEnabled;
                comboBoxPort.Enabled = success && _blastIrDelegate != null;
                buttonLearn.Enabled = _editionEnabled;
                buttonTest.Enabled = success && _blastIrDelegate != null;


            }
        }


        #endregion Delegates


        // --------------------------------------------------------------------------------------------------
        #region Buttons

        private void buttonLearn_Click(object sender, EventArgs e)
        {
            if (!_editionEnabled) return;


            textBoxName.Enabled = false;
            comboBoxPort.Enabled = false;
            buttonLearn.Enabled = false;
            buttonTest.Enabled = false;
            buttonOk.Enabled = false;

            labelStatus.Enabled = true;
            labelStatus.Height = 48;
            labelStatus.Location = new System.Drawing.Point(8, 43);

            if (_learnIrDelegate(tempfile, LearnStatus))
            {
                labelStatus.Text = "Hold your remote close to the receiver and tap the button to learn";
                labelStatus.ForeColor = Color.Blue;
                labelStatus.Update();
            }
            else
            {
                LearnStatus("Failed to learn IR", false);
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (!_insertionEnabled && !_editionEnabled) return;

            if (!EditedIr)
            {
                // Insert
                DialogResult = DialogResult.OK;

                try
                {
                    if (File.Exists(tempfile)) File.Delete(tempfile);
                }
                catch { }
                Close();
                return;
            }

            // Save
            string name = textBoxName.Text.Trim();

            if (name.Length == 0)
            {
                MessageBox.Show(this, "You must supply a name for this IR Command", "Missing name", MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
                textBoxName.Focus();
                return;
            }

            if (!Common.IsValidFileName(name))
            {
                MessageBox.Show(this, "You must supply a valid name for this IR Command", "Invalid name", MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
                textBoxName.Focus();
                return;
            }

            string fname = name + Common.FileExtensionIR;
            string fileName = Path.Combine(Common.FolderIRCommands, fname);
            if (name != _name && File.Exists(fileName))
            {
                DialogResult result = MessageBox.Show(this, "This IR Command already exists.\r\nDo you want to overwrite " + fname + " ?"
                                                     , "Overwrite file", MessageBoxButtons.YesNo
                                                     , MessageBoxIcon.Exclamation);

                if (result != System.Windows.Forms.DialogResult.Yes)
                {
                    textBoxName.Focus();
                    return;
                }
            }


            try
            {
                File.Copy(tempfile, fileName, true);
                EditedIr = false;

                // Renamed IR command: remove previous file
                if (name != _name && _name != "")
                {
                    string oldName = Path.Combine(Common.FolderIRCommands, _name + Common.FileExtensionIR);
                    File.Delete(oldName);
                }
            }
            catch (Exception ex)
            {
                IrssLog.Error(ex);
                MessageBox.Show(this, ex.Message, "Failed writing IR Command to file: " + fname, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            textBoxName.Text = "";
            try
            {
                if (File.Exists(tempfile)) File.Delete(tempfile);
            }
            catch { }
            
            Close();
        }


        private void buttonTest_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text.Trim();

            string fileName;
            if (_editionEnabled) fileName = tempfile;
            else fileName = Path.Combine(Common.FolderIRCommands, name + Common.FileExtensionIR);

            if (name.Length == 0 || _blastIrDelegate==null)  return;

            try
            {
                string port = comboBoxPort.SelectedItem as string;
                _blastIrDelegate(fileName, port);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Test failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LearnIR_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            IrssHelp.Open(this);
        }

        private void LearnIR_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            IrssHelp.Open(this);
        }

        #endregion Buttons

    }
}