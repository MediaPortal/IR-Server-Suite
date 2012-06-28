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
            this.checkBoxEnableRemote = new System.Windows.Forms.CheckBox();
            this.checkBoxUseSystemRatesRemote = new System.Windows.Forms.CheckBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageRemote = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButtonRepeatDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButtonHeldDelay)).BeginInit();
            this.groupBoxRemoteTiming.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageRemote.SuspendLayout();
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
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageRemote);
            this.tabControl.Location = new System.Drawing.Point(8, 8);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(256, 259);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageRemote
            // 
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
            this.Text = "Philips MCE USB IR Receiver- Spinel plus Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButtonRepeatDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButtonHeldDelay)).EndInit();
            this.groupBoxRemoteTiming.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPageRemote.ResumeLayout(false);
            this.tabPageRemote.PerformLayout();
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
        private System.Windows.Forms.CheckBox checkBoxUseSystemRatesRemote;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trackBar1;

    }

}
