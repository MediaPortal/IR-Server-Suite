namespace IRServer.Plugin
{
  partial class ConfigurationDialog
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
      this.numericUpDownLearnTimeout = new System.Windows.Forms.NumericUpDown();
      this.numericUpDownKeyHeldDelay = new System.Windows.Forms.NumericUpDown();
      this.numericUpDownKeyRepeatDelay = new System.Windows.Forms.NumericUpDown();
      this.checkBoxHandleKeyboardLocal = new System.Windows.Forms.CheckBox();
      this.checkBoxHandleMouseLocal = new System.Windows.Forms.CheckBox();
      this.numericUpDownMouseSensitivity = new System.Windows.Forms.NumericUpDown();
      this.checkBoxDisableMCEServices = new System.Windows.Forms.CheckBox();
      this.checkBoxEnableRemote = new System.Windows.Forms.CheckBox();
      this.checkBoxEnableKeyboard = new System.Windows.Forms.CheckBox();
      this.checkBoxEnableMouse = new System.Windows.Forms.CheckBox();
      this.checkBoxUseSystemRatesRemote = new System.Windows.Forms.CheckBox();
      this.checkBoxUseSystemRatesKeyboard = new System.Windows.Forms.CheckBox();
      this.checkBoxDisableAutomaticButtons = new System.Windows.Forms.CheckBox();
      this.tabPageBasic = new System.Windows.Forms.TabPage();
      this.groupBoxMceServices = new System.Windows.Forms.GroupBox();
      this.radioButton1 = new System.Windows.Forms.RadioButton();
      this.radioButtonDoNothing = new System.Windows.Forms.RadioButton();
      this.radioButtonStopAtStartup = new System.Windows.Forms.RadioButton();
      this.labelLearnIRTimeout = new System.Windows.Forms.Label();
      this.tabPageRemote = new System.Windows.Forms.TabPage();
      this.remotePanel = new System.Windows.Forms.Panel();
      this.tabPageKeyboard = new System.Windows.Forms.TabPage();
      this.keyboardPanel = new System.Windows.Forms.Panel();
      this.checkBoxKeyboardQwertz = new System.Windows.Forms.CheckBox();
      this.groupBoxKeypressTiming = new System.Windows.Forms.GroupBox();
      this.labelKeyRepeatDelay = new System.Windows.Forms.Label();
      this.labelKeyHeldDelay = new System.Windows.Forms.Label();
      this.tabPageMouse = new System.Windows.Forms.TabPage();
      this.mousePanel = new System.Windows.Forms.Panel();
      this.labelMouseSensitivity = new System.Windows.Forms.Label();
      this.tabControl = new System.Windows.Forms.TabControl();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButtonRepeatDelay)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButtonHeldDelay)).BeginInit();
      this.groupBoxRemoteTiming.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLearnTimeout)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownKeyHeldDelay)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownKeyRepeatDelay)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMouseSensitivity)).BeginInit();
      this.tabPageBasic.SuspendLayout();
      this.groupBoxMceServices.SuspendLayout();
      this.tabPageRemote.SuspendLayout();
      this.remotePanel.SuspendLayout();
      this.tabPageKeyboard.SuspendLayout();
      this.keyboardPanel.SuspendLayout();
      this.groupBoxKeypressTiming.SuspendLayout();
      this.tabPageMouse.SuspendLayout();
      this.mousePanel.SuspendLayout();
      this.tabControl.SuspendLayout();
      this.SuspendLayout();
      // 
      // labelButtonRepeatDelay
      // 
      this.labelButtonRepeatDelay.AutoSize = true;
      this.labelButtonRepeatDelay.Location = new System.Drawing.Point(6, 21);
      this.labelButtonRepeatDelay.Name = "labelButtonRepeatDelay";
      this.labelButtonRepeatDelay.Size = new System.Drawing.Size(102, 13);
      this.labelButtonRepeatDelay.TabIndex = 0;
      this.labelButtonRepeatDelay.Text = "Button repeat delay:";
      this.labelButtonRepeatDelay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelButtonHeldDelay
      // 
      this.labelButtonHeldDelay.AutoSize = true;
      this.labelButtonHeldDelay.Location = new System.Drawing.Point(6, 47);
      this.labelButtonHeldDelay.Name = "labelButtonHeldDelay";
      this.labelButtonHeldDelay.Size = new System.Drawing.Size(92, 13);
      this.labelButtonHeldDelay.TabIndex = 2;
      this.labelButtonHeldDelay.Text = "Button held delay:";
      this.labelButtonHeldDelay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // numericUpDownButtonRepeatDelay
      // 
      this.numericUpDownButtonRepeatDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.numericUpDownButtonRepeatDelay.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
      this.numericUpDownButtonRepeatDelay.Location = new System.Drawing.Point(144, 19);
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
      this.numericUpDownButtonHeldDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.numericUpDownButtonHeldDelay.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
      this.numericUpDownButtonHeldDelay.Location = new System.Drawing.Point(144, 45);
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
      this.buttonOK.Location = new System.Drawing.Point(106, 233);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(75, 23);
      this.buttonOK.TabIndex = 1;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(187, 233);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(75, 23);
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
      this.groupBoxRemoteTiming.Location = new System.Drawing.Point(6, 49);
      this.groupBoxRemoteTiming.Name = "groupBoxRemoteTiming";
      this.groupBoxRemoteTiming.Size = new System.Drawing.Size(230, 73);
      this.groupBoxRemoteTiming.TabIndex = 3;
      this.groupBoxRemoteTiming.TabStop = false;
      this.groupBoxRemoteTiming.Text = "Remote button timing (in milliseconds)";
      // 
      // numericUpDownLearnTimeout
      // 
      this.numericUpDownLearnTimeout.Increment = new decimal(new int[] {
            500,
            0,
            0,
            0});
      this.numericUpDownLearnTimeout.Location = new System.Drawing.Point(148, 10);
      this.numericUpDownLearnTimeout.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
      this.numericUpDownLearnTimeout.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
      this.numericUpDownLearnTimeout.Name = "numericUpDownLearnTimeout";
      this.numericUpDownLearnTimeout.Size = new System.Drawing.Size(88, 20);
      this.numericUpDownLearnTimeout.TabIndex = 1;
      this.numericUpDownLearnTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownLearnTimeout.ThousandsSeparator = true;
      this.toolTips.SetToolTip(this.numericUpDownLearnTimeout, "When teaching IR commands this is how long before the process times out");
      this.numericUpDownLearnTimeout.Value = new decimal(new int[] {
            8000,
            0,
            0,
            0});
      // 
      // numericUpDownKeyHeldDelay
      // 
      this.numericUpDownKeyHeldDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.numericUpDownKeyHeldDelay.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
      this.numericUpDownKeyHeldDelay.Location = new System.Drawing.Point(144, 45);
      this.numericUpDownKeyHeldDelay.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
      this.numericUpDownKeyHeldDelay.Name = "numericUpDownKeyHeldDelay";
      this.numericUpDownKeyHeldDelay.Size = new System.Drawing.Size(80, 20);
      this.numericUpDownKeyHeldDelay.TabIndex = 3;
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
      this.numericUpDownKeyRepeatDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.numericUpDownKeyRepeatDelay.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
      this.numericUpDownKeyRepeatDelay.Location = new System.Drawing.Point(144, 19);
      this.numericUpDownKeyRepeatDelay.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
      this.numericUpDownKeyRepeatDelay.Name = "numericUpDownKeyRepeatDelay";
      this.numericUpDownKeyRepeatDelay.Size = new System.Drawing.Size(80, 20);
      this.numericUpDownKeyRepeatDelay.TabIndex = 1;
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
      this.checkBoxHandleKeyboardLocal.Location = new System.Drawing.Point(6, 3);
      this.checkBoxHandleKeyboardLocal.Name = "checkBoxHandleKeyboardLocal";
      this.checkBoxHandleKeyboardLocal.Size = new System.Drawing.Size(139, 17);
      this.checkBoxHandleKeyboardLocal.TabIndex = 1;
      this.checkBoxHandleKeyboardLocal.Text = "Handle keyboard locally";
      this.toolTips.SetToolTip(this.checkBoxHandleKeyboardLocal, "Act on key presses locally (on the machine Input Servie is running on)");
      this.checkBoxHandleKeyboardLocal.UseVisualStyleBackColor = true;
      // 
      // checkBoxHandleMouseLocal
      // 
      this.checkBoxHandleMouseLocal.AutoSize = true;
      this.checkBoxHandleMouseLocal.Checked = true;
      this.checkBoxHandleMouseLocal.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxHandleMouseLocal.Location = new System.Drawing.Point(6, 3);
      this.checkBoxHandleMouseLocal.Name = "checkBoxHandleMouseLocal";
      this.checkBoxHandleMouseLocal.Size = new System.Drawing.Size(126, 17);
      this.checkBoxHandleMouseLocal.TabIndex = 1;
      this.checkBoxHandleMouseLocal.Text = "Handle mouse locally";
      this.toolTips.SetToolTip(this.checkBoxHandleMouseLocal, "Act on mouse locally (on the machine IR Server is running on)");
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
      this.numericUpDownMouseSensitivity.Location = new System.Drawing.Point(156, 27);
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
      // checkBoxDisableMCEServices
      // 
      this.checkBoxDisableMCEServices.AutoSize = true;
      this.checkBoxDisableMCEServices.Checked = true;
      this.checkBoxDisableMCEServices.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxDisableMCEServices.Location = new System.Drawing.Point(6, 36);
      this.checkBoxDisableMCEServices.Name = "checkBoxDisableMCEServices";
      this.checkBoxDisableMCEServices.Size = new System.Drawing.Size(216, 17);
      this.checkBoxDisableMCEServices.TabIndex = 2;
      this.checkBoxDisableMCEServices.Text = "Disable Windows Media Center services";
      this.toolTips.SetToolTip(this.checkBoxDisableMCEServices, "Disable Microsoft Windows Media Center services to prevent interference with the " +
              "IR Server");
      this.checkBoxDisableMCEServices.UseVisualStyleBackColor = true;
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
      this.checkBoxEnableRemote.CheckedChanged += new System.EventHandler(this.checkBoxEnableRemote_CheckedChanged);
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
      this.checkBoxEnableKeyboard.CheckedChanged += new System.EventHandler(this.checkBoxEnableKeyboard_CheckedChanged);
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
      this.checkBoxEnableMouse.CheckedChanged += new System.EventHandler(this.checkBoxEnableMouse_CheckedChanged);
      // 
      // checkBoxUseSystemRatesRemote
      // 
      this.checkBoxUseSystemRatesRemote.AutoSize = true;
      this.checkBoxUseSystemRatesRemote.Location = new System.Drawing.Point(6, 26);
      this.checkBoxUseSystemRatesRemote.Name = "checkBoxUseSystemRatesRemote";
      this.checkBoxUseSystemRatesRemote.Size = new System.Drawing.Size(187, 17);
      this.checkBoxUseSystemRatesRemote.TabIndex = 2;
      this.checkBoxUseSystemRatesRemote.Text = "Use system keyboard rate settings";
      this.toolTips.SetToolTip(this.checkBoxUseSystemRatesRemote, "Use the system keyboard repeat rate settings for remote button timing");
      this.checkBoxUseSystemRatesRemote.UseVisualStyleBackColor = true;
      this.checkBoxUseSystemRatesRemote.CheckedChanged += new System.EventHandler(this.checkBoxUseSystemRatesRemote_CheckedChanged);
      // 
      // checkBoxUseSystemRatesKeyboard
      // 
      this.checkBoxUseSystemRatesKeyboard.AutoSize = true;
      this.checkBoxUseSystemRatesKeyboard.Location = new System.Drawing.Point(6, 26);
      this.checkBoxUseSystemRatesKeyboard.Name = "checkBoxUseSystemRatesKeyboard";
      this.checkBoxUseSystemRatesKeyboard.Size = new System.Drawing.Size(187, 17);
      this.checkBoxUseSystemRatesKeyboard.TabIndex = 2;
      this.checkBoxUseSystemRatesKeyboard.Text = "Use system keyboard rate settings";
      this.toolTips.SetToolTip(this.checkBoxUseSystemRatesKeyboard, "Use the system keyboard repeat rate settings for remote keyboard repeat rates");
      this.checkBoxUseSystemRatesKeyboard.UseVisualStyleBackColor = true;
      this.checkBoxUseSystemRatesKeyboard.CheckedChanged += new System.EventHandler(this.checkBoxUseSystemRatesKeyboard_CheckedChanged);
      // 
      // checkBoxDisableAutomaticButtons
      // 
      this.checkBoxDisableAutomaticButtons.AutoSize = true;
      this.checkBoxDisableAutomaticButtons.Checked = true;
      this.checkBoxDisableAutomaticButtons.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxDisableAutomaticButtons.Location = new System.Drawing.Point(6, 3);
      this.checkBoxDisableAutomaticButtons.Name = "checkBoxDisableAutomaticButtons";
      this.checkBoxDisableAutomaticButtons.Size = new System.Drawing.Size(148, 17);
      this.checkBoxDisableAutomaticButtons.TabIndex = 1;
      this.checkBoxDisableAutomaticButtons.Text = "Disable automatic buttons";
      this.toolTips.SetToolTip(this.checkBoxDisableAutomaticButtons, "Prevent the operating system from automatically handling some buttons");
      this.checkBoxDisableAutomaticButtons.UseVisualStyleBackColor = true;
      // 
      // tabPageBasic
      // 
      this.tabPageBasic.Controls.Add(this.groupBoxMceServices);
      this.tabPageBasic.Controls.Add(this.checkBoxDisableMCEServices);
      this.tabPageBasic.Controls.Add(this.labelLearnIRTimeout);
      this.tabPageBasic.Controls.Add(this.numericUpDownLearnTimeout);
      this.tabPageBasic.Location = new System.Drawing.Point(4, 22);
      this.tabPageBasic.Name = "tabPageBasic";
      this.tabPageBasic.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageBasic.Size = new System.Drawing.Size(242, 184);
      this.tabPageBasic.TabIndex = 0;
      this.tabPageBasic.Text = "Basic";
      this.toolTips.SetToolTip(this.tabPageBasic, "Basic settings");
      this.tabPageBasic.UseVisualStyleBackColor = true;
      // 
      // groupBoxMceServices
      // 
      this.groupBoxMceServices.Controls.Add(this.radioButton1);
      this.groupBoxMceServices.Controls.Add(this.radioButtonDoNothing);
      this.groupBoxMceServices.Controls.Add(this.radioButtonStopAtStartup);
      this.groupBoxMceServices.Enabled = false;
      this.groupBoxMceServices.Location = new System.Drawing.Point(6, 59);
      this.groupBoxMceServices.Name = "groupBoxMceServices";
      this.groupBoxMceServices.Size = new System.Drawing.Size(230, 96);
      this.groupBoxMceServices.TabIndex = 3;
      this.groupBoxMceServices.TabStop = false;
      this.groupBoxMceServices.Text = "Windows Media Center Services";
      // 
      // radioButton1
      // 
      this.radioButton1.AutoSize = true;
      this.radioButton1.Location = new System.Drawing.Point(8, 72);
      this.radioButton1.Name = "radioButton1";
      this.radioButton1.Size = new System.Drawing.Size(119, 17);
      this.radioButton1.TabIndex = 2;
      this.radioButton1.TabStop = true;
      this.radioButton1.Text = "Permanently disable";
      this.radioButton1.UseVisualStyleBackColor = true;
      // 
      // radioButtonDoNothing
      // 
      this.radioButtonDoNothing.AutoSize = true;
      this.radioButtonDoNothing.Location = new System.Drawing.Point(8, 24);
      this.radioButtonDoNothing.Name = "radioButtonDoNothing";
      this.radioButtonDoNothing.Size = new System.Drawing.Size(77, 17);
      this.radioButtonDoNothing.TabIndex = 0;
      this.radioButtonDoNothing.TabStop = true;
      this.radioButtonDoNothing.Text = "Do nothing";
      this.radioButtonDoNothing.UseVisualStyleBackColor = true;
      // 
      // radioButtonStopAtStartup
      // 
      this.radioButtonStopAtStartup.AutoSize = true;
      this.radioButtonStopAtStartup.Location = new System.Drawing.Point(8, 48);
      this.radioButtonStopAtStartup.Name = "radioButtonStopAtStartup";
      this.radioButtonStopAtStartup.Size = new System.Drawing.Size(93, 17);
      this.radioButtonStopAtStartup.TabIndex = 1;
      this.radioButtonStopAtStartup.TabStop = true;
      this.radioButtonStopAtStartup.Text = "Stop if running";
      this.radioButtonStopAtStartup.UseVisualStyleBackColor = true;
      // 
      // labelLearnIRTimeout
      // 
      this.labelLearnIRTimeout.AutoSize = true;
      this.labelLearnIRTimeout.Location = new System.Drawing.Point(6, 12);
      this.labelLearnIRTimeout.Name = "labelLearnIRTimeout";
      this.labelLearnIRTimeout.Size = new System.Drawing.Size(88, 13);
      this.labelLearnIRTimeout.TabIndex = 0;
      this.labelLearnIRTimeout.Text = "Learn IR timeout:";
      this.labelLearnIRTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // tabPageRemote
      // 
      this.tabPageRemote.Controls.Add(this.remotePanel);
      this.tabPageRemote.Controls.Add(this.checkBoxEnableRemote);
      this.tabPageRemote.Location = new System.Drawing.Point(4, 22);
      this.tabPageRemote.Name = "tabPageRemote";
      this.tabPageRemote.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageRemote.Size = new System.Drawing.Size(242, 184);
      this.tabPageRemote.TabIndex = 1;
      this.tabPageRemote.Text = "Remote";
      this.toolTips.SetToolTip(this.tabPageRemote, "Remote control settings");
      this.tabPageRemote.UseVisualStyleBackColor = true;
      // 
      // remotePanel
      // 
      this.remotePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.remotePanel.Controls.Add(this.checkBoxDisableAutomaticButtons);
      this.remotePanel.Controls.Add(this.groupBoxRemoteTiming);
      this.remotePanel.Controls.Add(this.checkBoxUseSystemRatesRemote);
      this.remotePanel.Location = new System.Drawing.Point(0, 29);
      this.remotePanel.Name = "remotePanel";
      this.remotePanel.Size = new System.Drawing.Size(242, 155);
      this.remotePanel.TabIndex = 3;
      // 
      // tabPageKeyboard
      // 
      this.tabPageKeyboard.Controls.Add(this.keyboardPanel);
      this.tabPageKeyboard.Controls.Add(this.checkBoxEnableKeyboard);
      this.tabPageKeyboard.Location = new System.Drawing.Point(4, 22);
      this.tabPageKeyboard.Name = "tabPageKeyboard";
      this.tabPageKeyboard.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageKeyboard.Size = new System.Drawing.Size(242, 184);
      this.tabPageKeyboard.TabIndex = 2;
      this.tabPageKeyboard.Text = "Keyboard";
      this.toolTips.SetToolTip(this.tabPageKeyboard, "Keyboard settings for use with the MCE Replacement Driver");
      this.tabPageKeyboard.UseVisualStyleBackColor = true;
      // 
      // keyboardPanel
      // 
      this.keyboardPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.keyboardPanel.AutoSize = true;
      this.keyboardPanel.Controls.Add(this.checkBoxHandleKeyboardLocal);
      this.keyboardPanel.Controls.Add(this.checkBoxKeyboardQwertz);
      this.keyboardPanel.Controls.Add(this.groupBoxKeypressTiming);
      this.keyboardPanel.Controls.Add(this.checkBoxUseSystemRatesKeyboard);
      this.keyboardPanel.Location = new System.Drawing.Point(0, 29);
      this.keyboardPanel.Name = "keyboardPanel";
      this.keyboardPanel.Size = new System.Drawing.Size(239, 155);
      this.keyboardPanel.TabIndex = 5;
      // 
      // checkBoxKeyboardQwertz
      // 
      this.checkBoxKeyboardQwertz.AutoSize = true;
      this.checkBoxKeyboardQwertz.Location = new System.Drawing.Point(6, 130);
      this.checkBoxKeyboardQwertz.Name = "checkBoxKeyboardQwertz";
      this.checkBoxKeyboardQwertz.Size = new System.Drawing.Size(127, 17);
      this.checkBoxKeyboardQwertz.TabIndex = 4;
      this.checkBoxKeyboardQwertz.Text = "Use QWERTZ layout";
      this.toolTips.SetToolTip(this.checkBoxKeyboardQwertz, "Use the QWERTZ keyboard layout instead of QWERTY");
      this.checkBoxKeyboardQwertz.UseVisualStyleBackColor = true;
      // 
      // groupBoxKeypressTiming
      // 
      this.groupBoxKeypressTiming.Controls.Add(this.labelKeyRepeatDelay);
      this.groupBoxKeypressTiming.Controls.Add(this.numericUpDownKeyHeldDelay);
      this.groupBoxKeypressTiming.Controls.Add(this.labelKeyHeldDelay);
      this.groupBoxKeypressTiming.Controls.Add(this.numericUpDownKeyRepeatDelay);
      this.groupBoxKeypressTiming.Location = new System.Drawing.Point(6, 49);
      this.groupBoxKeypressTiming.Name = "groupBoxKeypressTiming";
      this.groupBoxKeypressTiming.Size = new System.Drawing.Size(230, 75);
      this.groupBoxKeypressTiming.TabIndex = 3;
      this.groupBoxKeypressTiming.TabStop = false;
      this.groupBoxKeypressTiming.Text = "Key press timing (in milliseconds)";
      // 
      // labelKeyRepeatDelay
      // 
      this.labelKeyRepeatDelay.AutoSize = true;
      this.labelKeyRepeatDelay.Location = new System.Drawing.Point(6, 21);
      this.labelKeyRepeatDelay.Name = "labelKeyRepeatDelay";
      this.labelKeyRepeatDelay.Size = new System.Drawing.Size(89, 13);
      this.labelKeyRepeatDelay.TabIndex = 0;
      this.labelKeyRepeatDelay.Text = "Key repeat delay:";
      this.labelKeyRepeatDelay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelKeyHeldDelay
      // 
      this.labelKeyHeldDelay.AutoSize = true;
      this.labelKeyHeldDelay.Location = new System.Drawing.Point(6, 47);
      this.labelKeyHeldDelay.Name = "labelKeyHeldDelay";
      this.labelKeyHeldDelay.Size = new System.Drawing.Size(79, 13);
      this.labelKeyHeldDelay.TabIndex = 2;
      this.labelKeyHeldDelay.Text = "Key held delay:";
      this.labelKeyHeldDelay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // tabPageMouse
      // 
      this.tabPageMouse.Controls.Add(this.mousePanel);
      this.tabPageMouse.Controls.Add(this.checkBoxEnableMouse);
      this.tabPageMouse.Location = new System.Drawing.Point(4, 22);
      this.tabPageMouse.Name = "tabPageMouse";
      this.tabPageMouse.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageMouse.Size = new System.Drawing.Size(242, 184);
      this.tabPageMouse.TabIndex = 3;
      this.tabPageMouse.Text = "Mouse";
      this.toolTips.SetToolTip(this.tabPageMouse, "Mouse settings for use with the MCE Replacement Driver");
      this.tabPageMouse.UseVisualStyleBackColor = true;
      // 
      // mousePanel
      // 
      this.mousePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.mousePanel.Controls.Add(this.checkBoxHandleMouseLocal);
      this.mousePanel.Controls.Add(this.labelMouseSensitivity);
      this.mousePanel.Controls.Add(this.numericUpDownMouseSensitivity);
      this.mousePanel.Location = new System.Drawing.Point(0, 29);
      this.mousePanel.Name = "mousePanel";
      this.mousePanel.Size = new System.Drawing.Size(242, 155);
      this.mousePanel.TabIndex = 4;
      // 
      // labelMouseSensitivity
      // 
      this.labelMouseSensitivity.AutoSize = true;
      this.labelMouseSensitivity.Location = new System.Drawing.Point(24, 29);
      this.labelMouseSensitivity.Name = "labelMouseSensitivity";
      this.labelMouseSensitivity.Size = new System.Drawing.Size(90, 13);
      this.labelMouseSensitivity.TabIndex = 2;
      this.labelMouseSensitivity.Text = "Mouse sensitivity:";
      this.labelMouseSensitivity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // tabControl
      // 
      this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.tabControl.Controls.Add(this.tabPageBasic);
      this.tabControl.Controls.Add(this.tabPageRemote);
      this.tabControl.Controls.Add(this.tabPageKeyboard);
      this.tabControl.Controls.Add(this.tabPageMouse);
      this.tabControl.Location = new System.Drawing.Point(12, 12);
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new System.Drawing.Size(250, 210);
      this.tabControl.TabIndex = 0;
      // 
      // Configure
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(274, 268);
      this.Controls.Add(this.tabControl);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "Configure";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Microsoft MCE Configuration";
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButtonRepeatDelay)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButtonHeldDelay)).EndInit();
      this.groupBoxRemoteTiming.ResumeLayout(false);
      this.groupBoxRemoteTiming.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLearnTimeout)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownKeyHeldDelay)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownKeyRepeatDelay)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMouseSensitivity)).EndInit();
      this.tabPageBasic.ResumeLayout(false);
      this.tabPageBasic.PerformLayout();
      this.groupBoxMceServices.ResumeLayout(false);
      this.groupBoxMceServices.PerformLayout();
      this.tabPageRemote.ResumeLayout(false);
      this.tabPageRemote.PerformLayout();
      this.remotePanel.ResumeLayout(false);
      this.remotePanel.PerformLayout();
      this.tabPageKeyboard.ResumeLayout(false);
      this.tabPageKeyboard.PerformLayout();
      this.keyboardPanel.ResumeLayout(false);
      this.keyboardPanel.PerformLayout();
      this.groupBoxKeypressTiming.ResumeLayout(false);
      this.groupBoxKeypressTiming.PerformLayout();
      this.tabPageMouse.ResumeLayout(false);
      this.tabPageMouse.PerformLayout();
      this.mousePanel.ResumeLayout(false);
      this.mousePanel.PerformLayout();
      this.tabControl.ResumeLayout(false);
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
    private System.Windows.Forms.Label labelLearnIRTimeout;
    private System.Windows.Forms.NumericUpDown numericUpDownLearnTimeout;
    private System.Windows.Forms.TabControl tabControl;
    private System.Windows.Forms.TabPage tabPageBasic;
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
    private System.Windows.Forms.CheckBox checkBoxDisableMCEServices;
    private System.Windows.Forms.CheckBox checkBoxUseSystemRatesRemote;
    private System.Windows.Forms.CheckBox checkBoxUseSystemRatesKeyboard;
    private System.Windows.Forms.CheckBox checkBoxDisableAutomaticButtons;
    private System.Windows.Forms.GroupBox groupBoxMceServices;
    private System.Windows.Forms.RadioButton radioButton1;
    private System.Windows.Forms.RadioButton radioButtonDoNothing;
    private System.Windows.Forms.RadioButton radioButtonStopAtStartup;
    private System.Windows.Forms.CheckBox checkBoxKeyboardQwertz;
    private System.Windows.Forms.Panel remotePanel;
    private System.Windows.Forms.Panel keyboardPanel;
    private System.Windows.Forms.Panel mousePanel;
  }
}