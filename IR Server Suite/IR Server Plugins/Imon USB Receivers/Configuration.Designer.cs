namespace IRServer.Plugin
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
            this.checkBoxEnableRemote = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableKeyboard = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableMouse = new System.Windows.Forms.CheckBox();
            this.checkBoxUseSystemRatesRemote = new System.Windows.Forms.CheckBox();
            this.checkBoxUseSystemRatesKeyboard = new System.Windows.Forms.CheckBox();
            this.comboBoxRemoteMode = new System.Windows.Forms.ComboBox();
            this.comboBoxPadMode = new System.Windows.Forms.ComboBox();
            this.labelRemoteMode = new System.Windows.Forms.Label();
            this.checkBoxKillImonM = new System.Windows.Forms.CheckBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageRemote = new System.Windows.Forms.TabPage();
            this.groupBoxHardwareConfig = new System.Windows.Forms.GroupBox();
            this.labelPadMode = new System.Windows.Forms.Label();
            this.checkBoxUsePadSwitch = new System.Windows.Forms.CheckBox();
            this.tabPageKeyboard = new System.Windows.Forms.TabPage();
            this.groupBoxKeyPadSensitivity = new System.Windows.Forms.GroupBox();
            this.labelKeyPadHarder = new System.Windows.Forms.Label();
            this.labelKeyPadSofter = new System.Windows.Forms.Label();
            this.trackBarKeyPadSensitivity = new System.Windows.Forms.TrackBar();
            this.groupBoxKeypressTiming = new System.Windows.Forms.GroupBox();
            this.labelKeyRepeatDelay = new System.Windows.Forms.Label();
            this.labelKeyHeldDelay = new System.Windows.Forms.Label();
            this.tabPageMouse = new System.Windows.Forms.TabPage();
            this.groupBoxMouseSensitivity = new System.Windows.Forms.GroupBox();
            this.labelMouseFaster = new System.Windows.Forms.Label();
            this.labelMouseSlower = new System.Windows.Forms.Label();
            this.labelMouseSensitivity = new System.Windows.Forms.Label();
            this.trackBarMouseSensitivity = new System.Windows.Forms.TrackBar();
            this.tabPageAdvanced = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButtonRepeatDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButtonHeldDelay)).BeginInit();
            this.groupBoxRemoteTiming.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownKeyHeldDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownKeyRepeatDelay)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabPageRemote.SuspendLayout();
            this.groupBoxHardwareConfig.SuspendLayout();
            this.tabPageKeyboard.SuspendLayout();
            this.groupBoxKeyPadSensitivity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarKeyPadSensitivity)).BeginInit();
            this.groupBoxKeypressTiming.SuspendLayout();
            this.tabPageMouse.SuspendLayout();
            this.groupBoxMouseSensitivity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMouseSensitivity)).BeginInit();
            this.tabPageAdvanced.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelButtonRepeatDelay
            // 
            this.labelButtonRepeatDelay.Location = new System.Drawing.Point(6, 17);
            this.labelButtonRepeatDelay.Name = "labelButtonRepeatDelay";
            this.labelButtonRepeatDelay.Size = new System.Drawing.Size(128, 20);
            this.labelButtonRepeatDelay.TabIndex = 0;
            this.labelButtonRepeatDelay.Text = "Button repeat delay:";
            this.labelButtonRepeatDelay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelButtonHeldDelay
            // 
            this.labelButtonHeldDelay.Location = new System.Drawing.Point(6, 43);
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
            this.numericUpDownButtonRepeatDelay.Location = new System.Drawing.Point(150, 19);
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
            this.numericUpDownButtonHeldDelay.Location = new System.Drawing.Point(150, 45);
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
            this.buttonOK.Location = new System.Drawing.Point(128, 275);
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
            this.buttonCancel.Location = new System.Drawing.Point(200, 275);
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
            this.groupBoxRemoteTiming.Location = new System.Drawing.Point(6, 154);
            this.groupBoxRemoteTiming.Name = "groupBoxRemoteTiming";
            this.groupBoxRemoteTiming.Size = new System.Drawing.Size(236, 73);
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
            this.numericUpDownKeyHeldDelay.Location = new System.Drawing.Point(150, 45);
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
            this.numericUpDownKeyRepeatDelay.Location = new System.Drawing.Point(150, 19);
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
            this.checkBoxHandleKeyboardLocal.Location = new System.Drawing.Point(6, 29);
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
            this.checkBoxHandleMouseLocal.Location = new System.Drawing.Point(6, 29);
            this.checkBoxHandleMouseLocal.Name = "checkBoxHandleMouseLocal";
            this.checkBoxHandleMouseLocal.Size = new System.Drawing.Size(126, 17);
            this.checkBoxHandleMouseLocal.TabIndex = 1;
            this.checkBoxHandleMouseLocal.Text = "Handle mouse locally";
            this.toolTips.SetToolTip(this.checkBoxHandleMouseLocal, "Act on mouse locally (on the machine IR Server is running on)");
            this.checkBoxHandleMouseLocal.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableRemote
            // 
            this.checkBoxEnableRemote.AutoSize = true;
            this.checkBoxEnableRemote.Checked = true;
            this.checkBoxEnableRemote.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnableRemote.Location = new System.Drawing.Point(6, 6);
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
            this.checkBoxEnableKeyboard.Location = new System.Drawing.Point(6, 6);
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
            this.checkBoxEnableMouse.Location = new System.Drawing.Point(6, 6);
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
            this.checkBoxUseSystemRatesRemote.Location = new System.Drawing.Point(6, 131);
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
            this.checkBoxUseSystemRatesKeyboard.Location = new System.Drawing.Point(6, 131);
            this.checkBoxUseSystemRatesKeyboard.Name = "checkBoxUseSystemRatesKeyboard";
            this.checkBoxUseSystemRatesKeyboard.Size = new System.Drawing.Size(187, 17);
            this.checkBoxUseSystemRatesKeyboard.TabIndex = 0;
            this.checkBoxUseSystemRatesKeyboard.Text = "Use system keyboard rate settings";
            this.toolTips.SetToolTip(this.checkBoxUseSystemRatesKeyboard, "Use the system keyboard repeat rate settings for remote keyboard repeat rates");
            this.checkBoxUseSystemRatesKeyboard.UseVisualStyleBackColor = true;
            // 
            // comboBoxRemoteMode
            // 
            this.comboBoxRemoteMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRemoteMode.FormattingEnabled = true;
            this.comboBoxRemoteMode.Location = new System.Drawing.Point(141, 19);
            this.comboBoxRemoteMode.Name = "comboBoxRemoteMode";
            this.comboBoxRemoteMode.Size = new System.Drawing.Size(89, 21);
            this.comboBoxRemoteMode.TabIndex = 4;
            this.toolTips.SetToolTip(this.comboBoxRemoteMode, "Choose between MCE and iMon remote types");
            // 
            // comboBoxPadMode
            // 
            this.comboBoxPadMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPadMode.FormattingEnabled = true;
            this.comboBoxPadMode.Location = new System.Drawing.Point(141, 46);
            this.comboBoxPadMode.Name = "comboBoxPadMode";
            this.comboBoxPadMode.Size = new System.Drawing.Size(89, 21);
            this.comboBoxPadMode.TabIndex = 6;
            this.toolTips.SetToolTip(this.comboBoxPadMode, "Choose between MCE and iMon remote types");
            // 
            // labelRemoteMode
            // 
            this.labelRemoteMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRemoteMode.Location = new System.Drawing.Point(6, 18);
            this.labelRemoteMode.Name = "labelRemoteMode";
            this.labelRemoteMode.Size = new System.Drawing.Size(129, 21);
            this.labelRemoteMode.TabIndex = 3;
            this.labelRemoteMode.Text = "Remote:";
            this.labelRemoteMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTips.SetToolTip(this.labelRemoteMode, "IMPORTANT: Set the hardware mode here");
            // 
            // checkBoxKillImonM
            // 
            this.checkBoxKillImonM.AutoSize = true;
            this.checkBoxKillImonM.Checked = true;
            this.checkBoxKillImonM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxKillImonM.Location = new System.Drawing.Point(16, 18);
            this.checkBoxKillImonM.Name = "checkBoxKillImonM";
            this.checkBoxKillImonM.Size = new System.Drawing.Size(113, 17);
            this.checkBoxKillImonM.TabIndex = 0;
            this.checkBoxKillImonM.Text = "Kill Imon Manager ";
            this.toolTips.SetToolTip(this.checkBoxKillImonM, "Kill Imon Manager software when IRSS Start");
            this.checkBoxKillImonM.UseVisualStyleBackColor = true;
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageRemote);
            this.tabControl.Controls.Add(this.tabPageKeyboard);
            this.tabControl.Controls.Add(this.tabPageMouse);
            this.tabControl.Controls.Add(this.tabPageAdvanced);
            this.tabControl.Location = new System.Drawing.Point(8, 8);
            this.tabControl.Name = "tabControl";
            this.tabControl.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(256, 259);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageRemote
            // 
            this.tabPageRemote.Controls.Add(this.groupBoxHardwareConfig);
            this.tabPageRemote.Controls.Add(this.checkBoxUsePadSwitch);
            this.tabPageRemote.Controls.Add(this.checkBoxUseSystemRatesRemote);
            this.tabPageRemote.Controls.Add(this.checkBoxEnableRemote);
            this.tabPageRemote.Controls.Add(this.groupBoxRemoteTiming);
            this.tabPageRemote.Location = new System.Drawing.Point(4, 22);
            this.tabPageRemote.Name = "tabPageRemote";
            this.tabPageRemote.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRemote.Size = new System.Drawing.Size(248, 233);
            this.tabPageRemote.TabIndex = 1;
            this.tabPageRemote.Text = "Remote";
            this.tabPageRemote.UseVisualStyleBackColor = true;
            // 
            // groupBoxHardwareConfig
            // 
            this.groupBoxHardwareConfig.Controls.Add(this.labelRemoteMode);
            this.groupBoxHardwareConfig.Controls.Add(this.comboBoxRemoteMode);
            this.groupBoxHardwareConfig.Controls.Add(this.comboBoxPadMode);
            this.groupBoxHardwareConfig.Controls.Add(this.labelPadMode);
            this.groupBoxHardwareConfig.Location = new System.Drawing.Point(6, 52);
            this.groupBoxHardwareConfig.Name = "groupBoxHardwareConfig";
            this.groupBoxHardwareConfig.Size = new System.Drawing.Size(236, 73);
            this.groupBoxHardwareConfig.TabIndex = 9;
            this.groupBoxHardwareConfig.TabStop = false;
            this.groupBoxHardwareConfig.Text = "Hardware configuration";
            // 
            // labelPadMode
            // 
            this.labelPadMode.Location = new System.Drawing.Point(6, 45);
            this.labelPadMode.Name = "labelPadMode";
            this.labelPadMode.Size = new System.Drawing.Size(129, 21);
            this.labelPadMode.TabIndex = 5;
            this.labelPadMode.Text = "Remote Pad mode:";
            this.labelPadMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkBoxUsePadSwitch
            // 
            this.checkBoxUsePadSwitch.AutoSize = true;
            this.checkBoxUsePadSwitch.Location = new System.Drawing.Point(6, 29);
            this.checkBoxUsePadSwitch.Name = "checkBoxUsePadSwitch";
            this.checkBoxUsePadSwitch.Size = new System.Drawing.Size(236, 17);
            this.checkBoxUsePadSwitch.TabIndex = 8;
            this.checkBoxUsePadSwitch.Text = "Enable mouse/keyboard switch (iMON PAD)";
            this.checkBoxUsePadSwitch.UseVisualStyleBackColor = true;
            // 
            // tabPageKeyboard
            // 
            this.tabPageKeyboard.Controls.Add(this.groupBoxKeyPadSensitivity);
            this.tabPageKeyboard.Controls.Add(this.checkBoxUseSystemRatesKeyboard);
            this.tabPageKeyboard.Controls.Add(this.checkBoxHandleKeyboardLocal);
            this.tabPageKeyboard.Controls.Add(this.checkBoxEnableKeyboard);
            this.tabPageKeyboard.Controls.Add(this.groupBoxKeypressTiming);
            this.tabPageKeyboard.Location = new System.Drawing.Point(4, 22);
            this.tabPageKeyboard.Name = "tabPageKeyboard";
            this.tabPageKeyboard.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageKeyboard.Size = new System.Drawing.Size(248, 233);
            this.tabPageKeyboard.TabIndex = 2;
            this.tabPageKeyboard.Text = "Keyboard";
            this.tabPageKeyboard.UseVisualStyleBackColor = true;
            // 
            // groupBoxKeyPadSensitivity
            // 
            this.groupBoxKeyPadSensitivity.Controls.Add(this.labelKeyPadHarder);
            this.groupBoxKeyPadSensitivity.Controls.Add(this.labelKeyPadSofter);
            this.groupBoxKeyPadSensitivity.Controls.Add(this.trackBarKeyPadSensitivity);
            this.groupBoxKeyPadSensitivity.Location = new System.Drawing.Point(6, 52);
            this.groupBoxKeyPadSensitivity.Name = "groupBoxKeyPadSensitivity";
            this.groupBoxKeyPadSensitivity.Size = new System.Drawing.Size(236, 73);
            this.groupBoxKeyPadSensitivity.TabIndex = 11;
            this.groupBoxKeyPadSensitivity.TabStop = false;
            this.groupBoxKeyPadSensitivity.Text = "KeyPad sensitivity (Pad in keyboard mode)";
            // 
            // labelKeyPadHarder
            // 
            this.labelKeyPadHarder.AutoSize = true;
            this.labelKeyPadHarder.Location = new System.Drawing.Point(193, 51);
            this.labelKeyPadHarder.Name = "labelKeyPadHarder";
            this.labelKeyPadHarder.Size = new System.Drawing.Size(37, 13);
            this.labelKeyPadHarder.TabIndex = 10;
            this.labelKeyPadHarder.Text = "harder";
            // 
            // labelKeyPadSofter
            // 
            this.labelKeyPadSofter.AutoSize = true;
            this.labelKeyPadSofter.Location = new System.Drawing.Point(6, 51);
            this.labelKeyPadSofter.Name = "labelKeyPadSofter";
            this.labelKeyPadSofter.Size = new System.Drawing.Size(33, 13);
            this.labelKeyPadSofter.TabIndex = 9;
            this.labelKeyPadSofter.Text = "softer";
            // 
            // trackBarKeyPadSensitivity
            // 
            this.trackBarKeyPadSensitivity.BackColor = System.Drawing.Color.White;
            this.trackBarKeyPadSensitivity.LargeChange = 4;
            this.trackBarKeyPadSensitivity.Location = new System.Drawing.Point(6, 19);
            this.trackBarKeyPadSensitivity.Maximum = 14;
            this.trackBarKeyPadSensitivity.Name = "trackBarKeyPadSensitivity";
            this.trackBarKeyPadSensitivity.Size = new System.Drawing.Size(224, 45);
            this.trackBarKeyPadSensitivity.TabIndex = 5;
            this.trackBarKeyPadSensitivity.Value = 7;
            // 
            // groupBoxKeypressTiming
            // 
            this.groupBoxKeypressTiming.Controls.Add(this.labelKeyRepeatDelay);
            this.groupBoxKeypressTiming.Controls.Add(this.numericUpDownKeyHeldDelay);
            this.groupBoxKeypressTiming.Controls.Add(this.labelKeyHeldDelay);
            this.groupBoxKeypressTiming.Controls.Add(this.numericUpDownKeyRepeatDelay);
            this.groupBoxKeypressTiming.Location = new System.Drawing.Point(6, 154);
            this.groupBoxKeypressTiming.Name = "groupBoxKeypressTiming";
            this.groupBoxKeypressTiming.Size = new System.Drawing.Size(236, 73);
            this.groupBoxKeypressTiming.TabIndex = 1;
            this.groupBoxKeypressTiming.TabStop = false;
            this.groupBoxKeypressTiming.Text = "Key press timing (in milliseconds)";
            // 
            // labelKeyRepeatDelay
            // 
            this.labelKeyRepeatDelay.Location = new System.Drawing.Point(6, 17);
            this.labelKeyRepeatDelay.Name = "labelKeyRepeatDelay";
            this.labelKeyRepeatDelay.Size = new System.Drawing.Size(128, 20);
            this.labelKeyRepeatDelay.TabIndex = 1;
            this.labelKeyRepeatDelay.Text = "Key repeat delay:";
            this.labelKeyRepeatDelay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelKeyHeldDelay
            // 
            this.labelKeyHeldDelay.Location = new System.Drawing.Point(6, 43);
            this.labelKeyHeldDelay.Name = "labelKeyHeldDelay";
            this.labelKeyHeldDelay.Size = new System.Drawing.Size(128, 20);
            this.labelKeyHeldDelay.TabIndex = 3;
            this.labelKeyHeldDelay.Text = "Key held delay:";
            this.labelKeyHeldDelay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageMouse
            // 
            this.tabPageMouse.Controls.Add(this.groupBoxMouseSensitivity);
            this.tabPageMouse.Controls.Add(this.checkBoxHandleMouseLocal);
            this.tabPageMouse.Controls.Add(this.checkBoxEnableMouse);
            this.tabPageMouse.Location = new System.Drawing.Point(4, 22);
            this.tabPageMouse.Name = "tabPageMouse";
            this.tabPageMouse.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMouse.Size = new System.Drawing.Size(248, 233);
            this.tabPageMouse.TabIndex = 3;
            this.tabPageMouse.Text = "Mouse";
            this.tabPageMouse.UseVisualStyleBackColor = true;
            // 
            // groupBoxMouseSensitivity
            // 
            this.groupBoxMouseSensitivity.Controls.Add(this.labelMouseFaster);
            this.groupBoxMouseSensitivity.Controls.Add(this.labelMouseSlower);
            this.groupBoxMouseSensitivity.Controls.Add(this.labelMouseSensitivity);
            this.groupBoxMouseSensitivity.Controls.Add(this.trackBarMouseSensitivity);
            this.groupBoxMouseSensitivity.Location = new System.Drawing.Point(6, 52);
            this.groupBoxMouseSensitivity.Name = "groupBoxMouseSensitivity";
            this.groupBoxMouseSensitivity.Size = new System.Drawing.Size(236, 73);
            this.groupBoxMouseSensitivity.TabIndex = 12;
            this.groupBoxMouseSensitivity.TabStop = false;
            this.groupBoxMouseSensitivity.Text = "Mouse sensitivity";
            // 
            // labelMouseFaster
            // 
            this.labelMouseFaster.AutoSize = true;
            this.labelMouseFaster.Location = new System.Drawing.Point(197, 51);
            this.labelMouseFaster.Name = "labelMouseFaster";
            this.labelMouseFaster.Size = new System.Drawing.Size(33, 13);
            this.labelMouseFaster.TabIndex = 10;
            this.labelMouseFaster.Text = "faster";
            // 
            // labelMouseSlower
            // 
            this.labelMouseSlower.AutoSize = true;
            this.labelMouseSlower.Location = new System.Drawing.Point(6, 51);
            this.labelMouseSlower.Name = "labelMouseSlower";
            this.labelMouseSlower.Size = new System.Drawing.Size(37, 13);
            this.labelMouseSlower.TabIndex = 9;
            this.labelMouseSlower.Text = "slower";
            // 
            // labelMouseSensitivity
            // 
            this.labelMouseSensitivity.Location = new System.Drawing.Point(6, 51);
            this.labelMouseSensitivity.Name = "labelMouseSensitivity";
            this.labelMouseSensitivity.Size = new System.Drawing.Size(224, 20);
            this.labelMouseSensitivity.TabIndex = 11;
            this.labelMouseSensitivity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBarMouseSensitivity
            // 
            this.trackBarMouseSensitivity.BackColor = System.Drawing.Color.White;
            this.trackBarMouseSensitivity.Cursor = System.Windows.Forms.Cursors.Default;
            this.trackBarMouseSensitivity.Location = new System.Drawing.Point(6, 19);
            this.trackBarMouseSensitivity.Maximum = 50;
            this.trackBarMouseSensitivity.Name = "trackBarMouseSensitivity";
            this.trackBarMouseSensitivity.Size = new System.Drawing.Size(224, 45);
            this.trackBarMouseSensitivity.TabIndex = 5;
            this.trackBarMouseSensitivity.TickFrequency = 5;
            this.trackBarMouseSensitivity.Value = 10;
            this.trackBarMouseSensitivity.Scroll += new System.EventHandler(this.trackBarMouseSensitivity_Scroll);
            // 
            // tabPageAdvanced
            // 
            this.tabPageAdvanced.Controls.Add(this.checkBoxKillImonM);
            this.tabPageAdvanced.Location = new System.Drawing.Point(4, 22);
            this.tabPageAdvanced.Name = "tabPageAdvanced";
            this.tabPageAdvanced.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAdvanced.Size = new System.Drawing.Size(248, 233);
            this.tabPageAdvanced.TabIndex = 4;
            this.tabPageAdvanced.Text = "Advanced";
            this.tabPageAdvanced.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(193, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "harder";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "softer";
            // 
            // trackBar1
            // 
            this.trackBar1.BackColor = System.Drawing.Color.White;
            this.trackBar1.LargeChange = 4;
            this.trackBar1.Location = new System.Drawing.Point(6, 19);
            this.trackBar1.Maximum = 15;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(224, 45);
            this.trackBar1.TabIndex = 5;
            this.trackBar1.Value = 8;
            // 
            // Configuration
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(272, 307);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(280, 320);
            this.Name = "Configuration";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "iMon Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButtonRepeatDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButtonHeldDelay)).EndInit();
            this.groupBoxRemoteTiming.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownKeyHeldDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownKeyRepeatDelay)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabPageRemote.ResumeLayout(false);
            this.tabPageRemote.PerformLayout();
            this.groupBoxHardwareConfig.ResumeLayout(false);
            this.tabPageKeyboard.ResumeLayout(false);
            this.tabPageKeyboard.PerformLayout();
            this.groupBoxKeyPadSensitivity.ResumeLayout(false);
            this.groupBoxKeyPadSensitivity.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarKeyPadSensitivity)).EndInit();
            this.groupBoxKeypressTiming.ResumeLayout(false);
            this.tabPageMouse.ResumeLayout(false);
            this.tabPageMouse.PerformLayout();
            this.groupBoxMouseSensitivity.ResumeLayout(false);
            this.groupBoxMouseSensitivity.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMouseSensitivity)).EndInit();
            this.tabPageAdvanced.ResumeLayout(false);
            this.tabPageAdvanced.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
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
        private System.Windows.Forms.CheckBox checkBoxHandleMouseLocal;
        private System.Windows.Forms.CheckBox checkBoxEnableMouse;
        private System.Windows.Forms.CheckBox checkBoxUseSystemRatesRemote;
        private System.Windows.Forms.CheckBox checkBoxUseSystemRatesKeyboard;
        private System.Windows.Forms.ComboBox comboBoxRemoteMode;
        private System.Windows.Forms.Label labelRemoteMode;
        private System.Windows.Forms.ComboBox comboBoxPadMode;
        private System.Windows.Forms.Label labelPadMode;
        private System.Windows.Forms.CheckBox checkBoxUsePadSwitch;
        private System.Windows.Forms.TrackBar trackBarKeyPadSensitivity;
        private System.Windows.Forms.GroupBox groupBoxKeyPadSensitivity;
        private System.Windows.Forms.Label labelKeyPadHarder;
        private System.Windows.Forms.Label labelKeyPadSofter;
        private System.Windows.Forms.GroupBox groupBoxMouseSensitivity;
        private System.Windows.Forms.Label labelMouseFaster;
        private System.Windows.Forms.Label labelMouseSlower;
        private System.Windows.Forms.TrackBar trackBarMouseSensitivity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label labelMouseSensitivity;
        private System.Windows.Forms.GroupBox groupBoxHardwareConfig;
        private System.Windows.Forms.TabPage tabPageAdvanced;
        private System.Windows.Forms.CheckBox checkBoxKillImonM;

    }

}
