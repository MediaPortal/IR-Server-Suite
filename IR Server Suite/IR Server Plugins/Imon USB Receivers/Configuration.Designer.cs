namespace InputService.Plugin
{

    partial class Configuration
    {

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labelButtonRepeatDelay = new System.Windows.Forms.Label();
            this.labelButtonHeldDelay = new System.Windows.Forms.Label();
            this.numericUpDownButtonRepeatDelay = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownButtonHeldDelay = new System.Windows.Forms.NumericUpDown();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBoxRemoteTiming = new System.Windows.Forms.GroupBox();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.numericUpDownKeyHeldDelay = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownKeyRepeatDelay = new System.Windows.Forms.NumericUpDown();
            this.checkBoxHandleKeyboardLocal = new System.Windows.Forms.CheckBox();
            this.checkBoxHandleMouseLocal = new System.Windows.Forms.CheckBox();
            this.numericUpDownMouseSensitivity = new System.Windows.Forms.NumericUpDown();
            this.checkBoxEnableRemote = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableKeyboard = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableMouse = new System.Windows.Forms.CheckBox();
            this.checkBoxUseSystemRatesRemote = new System.Windows.Forms.CheckBox();
            this.checkBoxUseSystemRatesKeyboard = new System.Windows.Forms.CheckBox();
            this.comboBoxHardwareMode = new System.Windows.Forms.ComboBox();
            this.comboBoxRemoteMode = new System.Windows.Forms.ComboBox();
            this.labelHardwareMode = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageRemote = new System.Windows.Forms.TabPage();
            this.labelRemoteMode = new System.Windows.Forms.Label();
            this.tabPageKeyboard = new System.Windows.Forms.TabPage();
            this.groupBoxKeypressTiming = new System.Windows.Forms.GroupBox();
            this.labelKeyRepeatDelay = new System.Windows.Forms.Label();
            this.labelKeyHeldDelay = new System.Windows.Forms.Label();
            this.tabPageMouse = new System.Windows.Forms.TabPage();
            this.labelMouseSensitivity = new System.Windows.Forms.Label();
            this.trackBarKeyPadSensitivity = new System.Windows.Forms.TrackBar();
            this.labelKeyPadSensitivity = new System.Windows.Forms.Label();
            this.labelKpsSoft = new System.Windows.Forms.Label();
            this.labelKpsHard = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButtonRepeatDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButtonHeldDelay)).BeginInit();
            this.groupBoxRemoteTiming.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownKeyHeldDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownKeyRepeatDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMouseSensitivity)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabPageRemote.SuspendLayout();
            this.tabPageKeyboard.SuspendLayout();
            this.groupBoxKeypressTiming.SuspendLayout();
            this.tabPageMouse.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarKeyPadSensitivity)).BeginInit();
            this.SuspendLayout();
            // 
            // labelButtonRepeatDelay
            // 
            this.labelButtonRepeatDelay.Location = new System.Drawing.Point(8, 24);
            this.labelButtonRepeatDelay.Name = "labelButtonRepeatDelay";
            this.labelButtonRepeatDelay.Size = new System.Drawing.Size(128, 20);
            this.labelButtonRepeatDelay.TabIndex = 0;
            this.labelButtonRepeatDelay.Text = "Button repeat delay:";
            this.labelButtonRepeatDelay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelButtonHeldDelay
            // 
            this.labelButtonHeldDelay.Location = new System.Drawing.Point(8, 56);
            this.labelButtonHeldDelay.Name = "labelButtonHeldDelay";
            this.labelButtonHeldDelay.Size = new System.Drawing.Size(128, 20);
            this.labelButtonHeldDelay.TabIndex = 2;
            this.labelButtonHeldDelay.Text = "Button held delay:";
            this.labelButtonHeldDelay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numericUpDownButtonRepeatDelay
            // 
            this.numericUpDownButtonRepeatDelay.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownButtonRepeatDelay.Location = new System.Drawing.Point(144, 24);
            this.numericUpDownButtonRepeatDelay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownButtonRepeatDelay.Name = "numericUpDownButtonRepeatDelay";
            this.numericUpDownButtonRepeatDelay.Size = new System.Drawing.Size(80, 20);
            this.numericUpDownButtonRepeatDelay.TabIndex = 1;
            this.numericUpDownButtonRepeatDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownButtonRepeatDelay.ThousandsSeparator = true;
            this.toolTips.SetToolTip(this.numericUpDownButtonRepeatDelay, "When the button is held this is the time between the first press and the first re" +
                    "peat");
            this.numericUpDownButtonRepeatDelay.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // numericUpDownButtonHeldDelay
            // 
            this.numericUpDownButtonHeldDelay.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownButtonHeldDelay.Location = new System.Drawing.Point(144, 56);
            this.numericUpDownButtonHeldDelay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownButtonHeldDelay.Name = "numericUpDownButtonHeldDelay";
            this.numericUpDownButtonHeldDelay.Size = new System.Drawing.Size(80, 20);
            this.numericUpDownButtonHeldDelay.TabIndex = 3;
            this.numericUpDownButtonHeldDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownButtonHeldDelay.ThousandsSeparator = true;
            this.toolTips.SetToolTip(this.numericUpDownButtonHeldDelay, "When the button is held this is the time between repeats");
            this.numericUpDownButtonHeldDelay.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(128, 263);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(64, 24);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(200, 263);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(64, 24);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // groupBoxRemoteTiming
            // 
            this.groupBoxRemoteTiming.Controls.Add(this.labelButtonRepeatDelay);
            this.groupBoxRemoteTiming.Controls.Add(this.numericUpDownButtonHeldDelay);
            this.groupBoxRemoteTiming.Controls.Add(this.labelButtonHeldDelay);
            this.groupBoxRemoteTiming.Controls.Add(this.numericUpDownButtonRepeatDelay);
            this.groupBoxRemoteTiming.Location = new System.Drawing.Point(8, 72);
            this.groupBoxRemoteTiming.Name = "groupBoxRemoteTiming";
            this.groupBoxRemoteTiming.Size = new System.Drawing.Size(232, 88);
            this.groupBoxRemoteTiming.TabIndex = 2;
            this.groupBoxRemoteTiming.TabStop = false;
            this.groupBoxRemoteTiming.Text = "Remote button timing (in milliseconds)";
            // 
            // numericUpDownKeyHeldDelay
            // 
            this.numericUpDownKeyHeldDelay.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownKeyHeldDelay.Location = new System.Drawing.Point(144, 56);
            this.numericUpDownKeyHeldDelay.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownKeyHeldDelay.Name = "numericUpDownKeyHeldDelay";
            this.numericUpDownKeyHeldDelay.Size = new System.Drawing.Size(80, 20);
            this.numericUpDownKeyHeldDelay.TabIndex = 4;
            this.numericUpDownKeyHeldDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownKeyHeldDelay.ThousandsSeparator = true;
            this.toolTips.SetToolTip(this.numericUpDownKeyHeldDelay, "When a key is held this is the time between repeats");
            this.numericUpDownKeyHeldDelay.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // numericUpDownKeyRepeatDelay
            // 
            this.numericUpDownKeyRepeatDelay.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownKeyRepeatDelay.Location = new System.Drawing.Point(144, 24);
            this.numericUpDownKeyRepeatDelay.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownKeyRepeatDelay.Name = "numericUpDownKeyRepeatDelay";
            this.numericUpDownKeyRepeatDelay.Size = new System.Drawing.Size(80, 20);
            this.numericUpDownKeyRepeatDelay.TabIndex = 2;
            this.numericUpDownKeyRepeatDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownKeyRepeatDelay.ThousandsSeparator = true;
            this.toolTips.SetToolTip(this.numericUpDownKeyRepeatDelay, "When a key is held this is the time between the first press and the first repeat");
            this.numericUpDownKeyRepeatDelay.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // checkBoxHandleKeyboardLocal
            // 
            this.checkBoxHandleKeyboardLocal.AutoSize = true;
            this.checkBoxHandleKeyboardLocal.Checked = true;
            this.checkBoxHandleKeyboardLocal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxHandleKeyboardLocal.Location = new System.Drawing.Point(8, 168);
            this.checkBoxHandleKeyboardLocal.Name = "checkBoxHandleKeyboardLocal";
            this.checkBoxHandleKeyboardLocal.Size = new System.Drawing.Size(139, 17);
            this.checkBoxHandleKeyboardLocal.TabIndex = 2;
            this.checkBoxHandleKeyboardLocal.Text = "Handle keyboard locally";
            this.toolTips.SetToolTip(this.checkBoxHandleKeyboardLocal, "Act on key presses locally (on the machine Input Servie is running on)");
            this.checkBoxHandleKeyboardLocal.UseVisualStyleBackColor = true;
            // 
            // checkBoxHandleMouseLocal
            // 
            this.checkBoxHandleMouseLocal.AutoSize = true;
            this.checkBoxHandleMouseLocal.Checked = true;
            this.checkBoxHandleMouseLocal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxHandleMouseLocal.Location = new System.Drawing.Point(8, 40);
            this.checkBoxHandleMouseLocal.Name = "checkBoxHandleMouseLocal";
            this.checkBoxHandleMouseLocal.Size = new System.Drawing.Size(126, 17);
            this.checkBoxHandleMouseLocal.TabIndex = 1;
            this.checkBoxHandleMouseLocal.Text = "Handle mouse locally";
            this.toolTips.SetToolTip(this.checkBoxHandleMouseLocal, "Act on mouse locally (on the machine Input Service is running on)");
            this.checkBoxHandleMouseLocal.UseVisualStyleBackColor = true;
            // 
            // numericUpDownMouseSensitivity
            // 
            this.numericUpDownMouseSensitivity.DecimalPlaces = 1;
            this.numericUpDownMouseSensitivity.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownMouseSensitivity.Location = new System.Drawing.Point(160, 80);
            this.numericUpDownMouseSensitivity.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownMouseSensitivity.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.numericUpDownMouseSensitivity.Name = "numericUpDownMouseSensitivity";
            this.numericUpDownMouseSensitivity.Size = new System.Drawing.Size(80, 20);
            this.numericUpDownMouseSensitivity.TabIndex = 3;
            this.numericUpDownMouseSensitivity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTips.SetToolTip(this.numericUpDownMouseSensitivity, "Multiply mouse movements by this number");
            this.numericUpDownMouseSensitivity.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            // 
            // checkBoxEnableRemote
            // 
            this.checkBoxEnableRemote.AutoSize = true;
            this.checkBoxEnableRemote.Checked = true;
            this.checkBoxEnableRemote.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnableRemote.Location = new System.Drawing.Point(8, 8);
            this.checkBoxEnableRemote.Name = "checkBoxEnableRemote";
            this.checkBoxEnableRemote.Size = new System.Drawing.Size(155, 17);
            this.checkBoxEnableRemote.TabIndex = 0;
            this.checkBoxEnableRemote.Text = "Enable remote control input";
            this.toolTips.SetToolTip(this.checkBoxEnableRemote, "Decode remote control button presses");
            this.checkBoxEnableRemote.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableKeyboard
            // 
            this.checkBoxEnableKeyboard.AutoSize = true;
            this.checkBoxEnableKeyboard.Checked = true;
            this.checkBoxEnableKeyboard.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnableKeyboard.Location = new System.Drawing.Point(8, 8);
            this.checkBoxEnableKeyboard.Name = "checkBoxEnableKeyboard";
            this.checkBoxEnableKeyboard.Size = new System.Drawing.Size(132, 17);
            this.checkBoxEnableKeyboard.TabIndex = 0;
            this.checkBoxEnableKeyboard.Text = "Enable keyboard input";
            this.toolTips.SetToolTip(this.checkBoxEnableKeyboard, "Decode remote keyboard input");
            this.checkBoxEnableKeyboard.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableMouse
            // 
            this.checkBoxEnableMouse.AutoSize = true;
            this.checkBoxEnableMouse.Checked = true;
            this.checkBoxEnableMouse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnableMouse.Location = new System.Drawing.Point(8, 8);
            this.checkBoxEnableMouse.Name = "checkBoxEnableMouse";
            this.checkBoxEnableMouse.Size = new System.Drawing.Size(119, 17);
            this.checkBoxEnableMouse.TabIndex = 0;
            this.checkBoxEnableMouse.Text = "Enable mouse input";
            this.toolTips.SetToolTip(this.checkBoxEnableMouse, "Decode remote mouse input");
            this.checkBoxEnableMouse.UseVisualStyleBackColor = true;
            // 
            // checkBoxUseSystemRatesRemote
            // 
            this.checkBoxUseSystemRatesRemote.AutoSize = true;
            this.checkBoxUseSystemRatesRemote.Location = new System.Drawing.Point(8, 40);
            this.checkBoxUseSystemRatesRemote.Name = "checkBoxUseSystemRatesRemote";
            this.checkBoxUseSystemRatesRemote.Size = new System.Drawing.Size(187, 17);
            this.checkBoxUseSystemRatesRemote.TabIndex = 1;
            this.checkBoxUseSystemRatesRemote.Text = "Use system keyboard rate settings";
            this.toolTips.SetToolTip(this.checkBoxUseSystemRatesRemote, "Use the system keyboard repeat rate settings for remote button timing");
            this.checkBoxUseSystemRatesRemote.UseVisualStyleBackColor = true;
            // 
            // checkBoxUseSystemRatesKeyboard
            // 
            this.checkBoxUseSystemRatesKeyboard.AutoSize = true;
            this.checkBoxUseSystemRatesKeyboard.Location = new System.Drawing.Point(8, 40);
            this.checkBoxUseSystemRatesKeyboard.Name = "checkBoxUseSystemRatesKeyboard";
            this.checkBoxUseSystemRatesKeyboard.Size = new System.Drawing.Size(187, 17);
            this.checkBoxUseSystemRatesKeyboard.TabIndex = 0;
            this.checkBoxUseSystemRatesKeyboard.Text = "Use system keyboard rate settings";
            this.toolTips.SetToolTip(this.checkBoxUseSystemRatesKeyboard, "Use the system keyboard repeat rate settings for remote keyboard repeat rates");
            this.checkBoxUseSystemRatesKeyboard.UseVisualStyleBackColor = true;
            // 
            // comboBoxHardwareMode
            // 
            this.comboBoxHardwareMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxHardwareMode.FormattingEnabled = true;
            this.comboBoxHardwareMode.Location = new System.Drawing.Point(152, 168);
            this.comboBoxHardwareMode.Name = "comboBoxHardwareMode";
            this.comboBoxHardwareMode.Size = new System.Drawing.Size(89, 21);
            this.comboBoxHardwareMode.TabIndex = 4;
            this.toolTips.SetToolTip(this.comboBoxHardwareMode, "Choose between MCE and iMon remote types");
            // 
            // comboBoxRemoteMode
            // 
            this.comboBoxRemoteMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRemoteMode.FormattingEnabled = true;
            this.comboBoxRemoteMode.Location = new System.Drawing.Point(152, 194);
            this.comboBoxRemoteMode.Name = "comboBoxRemoteMode";
            this.comboBoxRemoteMode.Size = new System.Drawing.Size(89, 21);
            this.comboBoxRemoteMode.TabIndex = 6;
            this.toolTips.SetToolTip(this.comboBoxRemoteMode, "Choose between MCE and iMon remote types");
            // 
            // labelHardwareMode
            // 
            this.labelHardwareMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHardwareMode.Location = new System.Drawing.Point(8, 168);
            this.labelHardwareMode.Name = "labelHardwareMode";
            this.labelHardwareMode.Size = new System.Drawing.Size(136, 21);
            this.labelHardwareMode.TabIndex = 3;
            this.labelHardwareMode.Text = "Hardware mode:";
            this.labelHardwareMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTips.SetToolTip(this.labelHardwareMode, "IMPORTANT: Set the hardware mode here");
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageRemote);
            this.tabControl.Controls.Add(this.tabPageKeyboard);
            this.tabControl.Controls.Add(this.tabPageMouse);
            this.tabControl.Location = new System.Drawing.Point(8, 8);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(256, 247);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageRemote
            // 
            this.tabPageRemote.Controls.Add(this.comboBoxRemoteMode);
            this.tabPageRemote.Controls.Add(this.labelRemoteMode);
            this.tabPageRemote.Controls.Add(this.comboBoxHardwareMode);
            this.tabPageRemote.Controls.Add(this.labelHardwareMode);
            this.tabPageRemote.Controls.Add(this.checkBoxUseSystemRatesRemote);
            this.tabPageRemote.Controls.Add(this.checkBoxEnableRemote);
            this.tabPageRemote.Controls.Add(this.groupBoxRemoteTiming);
            this.tabPageRemote.Location = new System.Drawing.Point(4, 22);
            this.tabPageRemote.Name = "tabPageRemote";
            this.tabPageRemote.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRemote.Size = new System.Drawing.Size(248, 221);
            this.tabPageRemote.TabIndex = 1;
            this.tabPageRemote.Text = "Remote";
            this.tabPageRemote.UseVisualStyleBackColor = true;
            // 
            // labelRemoteMode
            // 
            this.labelRemoteMode.Location = new System.Drawing.Point(8, 194);
            this.labelRemoteMode.Name = "labelRemoteMode";
            this.labelRemoteMode.Size = new System.Drawing.Size(136, 21);
            this.labelRemoteMode.TabIndex = 5;
            this.labelRemoteMode.Text = "Remote MouseStick mode:";
            this.labelRemoteMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageKeyboard
            // 
            this.tabPageKeyboard.Controls.Add(this.checkBoxUseSystemRatesKeyboard);
            this.tabPageKeyboard.Controls.Add(this.checkBoxHandleKeyboardLocal);
            this.tabPageKeyboard.Controls.Add(this.checkBoxEnableKeyboard);
            this.tabPageKeyboard.Controls.Add(this.groupBoxKeypressTiming);
            this.tabPageKeyboard.Location = new System.Drawing.Point(4, 22);
            this.tabPageKeyboard.Name = "tabPageKeyboard";
            this.tabPageKeyboard.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageKeyboard.Size = new System.Drawing.Size(248, 221);
            this.tabPageKeyboard.TabIndex = 2;
            this.tabPageKeyboard.Text = "Keyboard";
            this.tabPageKeyboard.UseVisualStyleBackColor = true;
            // 
            // groupBoxKeypressTiming
            // 
            this.groupBoxKeypressTiming.Controls.Add(this.labelKeyRepeatDelay);
            this.groupBoxKeypressTiming.Controls.Add(this.numericUpDownKeyHeldDelay);
            this.groupBoxKeypressTiming.Controls.Add(this.labelKeyHeldDelay);
            this.groupBoxKeypressTiming.Controls.Add(this.numericUpDownKeyRepeatDelay);
            this.groupBoxKeypressTiming.Location = new System.Drawing.Point(8, 72);
            this.groupBoxKeypressTiming.Name = "groupBoxKeypressTiming";
            this.groupBoxKeypressTiming.Size = new System.Drawing.Size(232, 88);
            this.groupBoxKeypressTiming.TabIndex = 1;
            this.groupBoxKeypressTiming.TabStop = false;
            this.groupBoxKeypressTiming.Text = "Key press timing (in milliseconds)";
            // 
            // labelKeyRepeatDelay
            // 
            this.labelKeyRepeatDelay.Location = new System.Drawing.Point(8, 24);
            this.labelKeyRepeatDelay.Name = "labelKeyRepeatDelay";
            this.labelKeyRepeatDelay.Size = new System.Drawing.Size(128, 20);
            this.labelKeyRepeatDelay.TabIndex = 1;
            this.labelKeyRepeatDelay.Text = "Key repeat delay:";
            this.labelKeyRepeatDelay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelKeyHeldDelay
            // 
            this.labelKeyHeldDelay.Location = new System.Drawing.Point(8, 56);
            this.labelKeyHeldDelay.Name = "labelKeyHeldDelay";
            this.labelKeyHeldDelay.Size = new System.Drawing.Size(128, 20);
            this.labelKeyHeldDelay.TabIndex = 3;
            this.labelKeyHeldDelay.Text = "Key held delay:";
            this.labelKeyHeldDelay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageMouse
            // 
            this.tabPageMouse.Controls.Add(this.labelKpsHard);
            this.tabPageMouse.Controls.Add(this.labelKpsSoft);
            this.tabPageMouse.Controls.Add(this.labelKeyPadSensitivity);
            this.tabPageMouse.Controls.Add(this.trackBarKeyPadSensitivity);
            this.tabPageMouse.Controls.Add(this.labelMouseSensitivity);
            this.tabPageMouse.Controls.Add(this.numericUpDownMouseSensitivity);
            this.tabPageMouse.Controls.Add(this.checkBoxHandleMouseLocal);
            this.tabPageMouse.Controls.Add(this.checkBoxEnableMouse);
            this.tabPageMouse.Location = new System.Drawing.Point(4, 22);
            this.tabPageMouse.Name = "tabPageMouse";
            this.tabPageMouse.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMouse.Size = new System.Drawing.Size(248, 221);
            this.tabPageMouse.TabIndex = 3;
            this.tabPageMouse.Text = "Mouse";
            this.tabPageMouse.UseVisualStyleBackColor = true;
            // 
            // labelMouseSensitivity
            // 
            this.labelMouseSensitivity.Location = new System.Drawing.Point(8, 80);
            this.labelMouseSensitivity.Name = "labelMouseSensitivity";
            this.labelMouseSensitivity.Size = new System.Drawing.Size(144, 20);
            this.labelMouseSensitivity.TabIndex = 2;
            this.labelMouseSensitivity.Text = "Mouse sensitivity:";
            this.labelMouseSensitivity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trackBarKeyPadSensitivity
            // 
            this.trackBarKeyPadSensitivity.BackColor = System.Drawing.Color.White;
            this.trackBarKeyPadSensitivity.LargeChange = 4;
            this.trackBarKeyPadSensitivity.Location = new System.Drawing.Point(6, 148);
            this.trackBarKeyPadSensitivity.Maximum = 15;
            this.trackBarKeyPadSensitivity.Name = "trackBarKeyPadSensitivity";
            this.trackBarKeyPadSensitivity.Size = new System.Drawing.Size(236, 45);
            this.trackBarKeyPadSensitivity.TabIndex = 4;
            this.trackBarKeyPadSensitivity.Value = 8;
            // 
            // labelKeyPadSensitivity
            // 
            this.labelKeyPadSensitivity.AutoSize = true;
            this.labelKeyPadSensitivity.Location = new System.Drawing.Point(6, 132);
            this.labelKeyPadSensitivity.Name = "labelKeyPadSensitivity";
            this.labelKeyPadSensitivity.Size = new System.Drawing.Size(229, 13);
            this.labelKeyPadSensitivity.TabIndex = 5;
            this.labelKeyPadSensitivity.Text = "KeyPad Sensitivity (if Pad works as arrow keys)";
            // 
            // labelKpsSoft
            // 
            this.labelKpsSoft.AutoSize = true;
            this.labelKpsSoft.Location = new System.Drawing.Point(6, 180);
            this.labelKpsSoft.Name = "labelKpsSoft";
            this.labelKpsSoft.Size = new System.Drawing.Size(33, 13);
            this.labelKpsSoft.TabIndex = 6;
            this.labelKpsSoft.Text = "softer";
            // 
            // labelKpsHard
            // 
            this.labelKpsHard.AutoSize = true;
            this.labelKpsHard.Location = new System.Drawing.Point(205, 180);
            this.labelKpsHard.Name = "labelKpsHard";
            this.labelKpsHard.Size = new System.Drawing.Size(37, 13);
            this.labelKpsHard.TabIndex = 7;
            this.labelKpsHard.Text = "harder";
            // 
            // Configuration
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(272, 295);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(280, 306);
            this.Name = "Configuration";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "iMon Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButtonRepeatDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButtonHeldDelay)).EndInit();
            this.groupBoxRemoteTiming.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownKeyHeldDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownKeyRepeatDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMouseSensitivity)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabPageRemote.ResumeLayout(false);
            this.tabPageRemote.PerformLayout();
            this.tabPageKeyboard.ResumeLayout(false);
            this.tabPageKeyboard.PerformLayout();
            this.groupBoxKeypressTiming.ResumeLayout(false);
            this.tabPageMouse.ResumeLayout(false);
            this.tabPageMouse.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarKeyPadSensitivity)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelButtonRepeatDelay;
        private System.Windows.Forms.Label labelButtonHeldDelay;
        private System.Windows.Forms.NumericUpDown numericUpDownButtonRepeatDelay;
        private System.Windows.Forms.NumericUpDown numericUpDownButtonHeldDelay;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBoxRemoteTiming;
        private System.Windows.Forms.ToolTip toolTips;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageRemote;
        private System.Windows.Forms.CheckBox checkBoxEnableRemote;
        private System.Windows.Forms.TabPage tabPageKeyboard;
        private System.Windows.Forms.GroupBox groupBoxKeypressTiming;
        private System.Windows.Forms.Label labelKeyRepeatDelay;
        private System.Windows.Forms.NumericUpDown numericUpDownKeyHeldDelay;
        private System.Windows.Forms.Label labelKeyHeldDelay;
        private System.Windows.Forms.NumericUpDown numericUpDownKeyRepeatDelay;
        private System.Windows.Forms.CheckBox checkBoxHandleKeyboardLocal;
        private System.Windows.Forms.CheckBox checkBoxEnableKeyboard;
        private System.Windows.Forms.TabPage tabPageMouse;
        private System.Windows.Forms.Label labelMouseSensitivity;
        private System.Windows.Forms.NumericUpDown numericUpDownMouseSensitivity;
        private System.Windows.Forms.CheckBox checkBoxHandleMouseLocal;
        private System.Windows.Forms.CheckBox checkBoxEnableMouse;
        private System.Windows.Forms.CheckBox checkBoxUseSystemRatesRemote;
        private System.Windows.Forms.CheckBox checkBoxUseSystemRatesKeyboard;
        private System.Windows.Forms.ComboBox comboBoxHardwareMode;
        private System.Windows.Forms.Label labelHardwareMode;
        private System.Windows.Forms.ComboBox comboBoxRemoteMode;
        private System.Windows.Forms.Label labelRemoteMode;
        private System.Windows.Forms.TrackBar trackBarKeyPadSensitivity;
        private System.Windows.Forms.Label labelKpsSoft;
        private System.Windows.Forms.Label labelKeyPadSensitivity;
        private System.Windows.Forms.Label labelKpsHard;

    }

}
