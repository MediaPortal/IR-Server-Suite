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
      this.labelFirstRepeatDelay = new System.Windows.Forms.Label();
      this.labelHeldRepeatDelay = new System.Windows.Forms.Label();
      this.numericUpDownFirstRepeatDelay = new System.Windows.Forms.NumericUpDown();
      this.numericUpDownHeldRepeatDelay = new System.Windows.Forms.NumericUpDown();
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.groupBoxRemoteTiming = new System.Windows.Forms.GroupBox();
      this.toolTips = new System.Windows.Forms.ToolTip(this.components);
      this.checkBoxUseSystemRatesDelay = new System.Windows.Forms.CheckBox();
      this.tabControl = new System.Windows.Forms.TabControl();
      this.tabPageRemote = new System.Windows.Forms.TabPage();
      this.checkBoxDoRepeats = new System.Windows.Forms.CheckBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.trackBar1 = new System.Windows.Forms.TrackBar();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFirstRepeatDelay)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeldRepeatDelay)).BeginInit();
      this.groupBoxRemoteTiming.SuspendLayout();
      this.tabControl.SuspendLayout();
      this.tabPageRemote.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
      this.SuspendLayout();
      // 
      // labelFirstRepeatDelay
      // 
      this.labelFirstRepeatDelay.Location = new System.Drawing.Point(6, 17);
      this.labelFirstRepeatDelay.Name = "labelFirstRepeatDelay";
      this.labelFirstRepeatDelay.Size = new System.Drawing.Size(128, 20);
      this.labelFirstRepeatDelay.TabIndex = 0;
      this.labelFirstRepeatDelay.Text = "Button repeat delay:";
      this.labelFirstRepeatDelay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelHeldRepeatDelay
      // 
      this.labelHeldRepeatDelay.Location = new System.Drawing.Point(6, 43);
      this.labelHeldRepeatDelay.Name = "labelHeldRepeatDelay";
      this.labelHeldRepeatDelay.Size = new System.Drawing.Size(128, 20);
      this.labelHeldRepeatDelay.TabIndex = 2;
      this.labelHeldRepeatDelay.Text = "Button held delay:";
      this.labelHeldRepeatDelay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // numericUpDownFirstRepeatDelay
      // 
      this.numericUpDownFirstRepeatDelay.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
      this.numericUpDownFirstRepeatDelay.Location = new System.Drawing.Point(150, 19);
      this.numericUpDownFirstRepeatDelay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this.numericUpDownFirstRepeatDelay.Name = "numericUpDownFirstRepeatDelay";
      this.numericUpDownFirstRepeatDelay.Size = new System.Drawing.Size(80, 20);
      this.numericUpDownFirstRepeatDelay.TabIndex = 1;
      this.numericUpDownFirstRepeatDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownFirstRepeatDelay.ThousandsSeparator = true;
      this.toolTips.SetToolTip(this.numericUpDownFirstRepeatDelay, "When the button is held this is the time between the first press and the first re" +
        "peat");
      this.numericUpDownFirstRepeatDelay.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      // 
      // numericUpDownHeldRepeatDelay
      // 
      this.numericUpDownHeldRepeatDelay.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
      this.numericUpDownHeldRepeatDelay.Location = new System.Drawing.Point(150, 45);
      this.numericUpDownHeldRepeatDelay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this.numericUpDownHeldRepeatDelay.Name = "numericUpDownHeldRepeatDelay";
      this.numericUpDownHeldRepeatDelay.Size = new System.Drawing.Size(80, 20);
      this.numericUpDownHeldRepeatDelay.TabIndex = 3;
      this.numericUpDownHeldRepeatDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownHeldRepeatDelay.ThousandsSeparator = true;
      this.toolTips.SetToolTip(this.numericUpDownHeldRepeatDelay, "When the button is held this is the time between repeats");
      this.numericUpDownHeldRepeatDelay.Value = new decimal(new int[] {
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
      this.groupBoxRemoteTiming.Controls.Add(this.labelFirstRepeatDelay);
      this.groupBoxRemoteTiming.Controls.Add(this.numericUpDownHeldRepeatDelay);
      this.groupBoxRemoteTiming.Controls.Add(this.labelHeldRepeatDelay);
      this.groupBoxRemoteTiming.Controls.Add(this.numericUpDownFirstRepeatDelay);
      this.groupBoxRemoteTiming.Location = new System.Drawing.Point(6, 154);
      this.groupBoxRemoteTiming.Name = "groupBoxRemoteTiming";
      this.groupBoxRemoteTiming.Size = new System.Drawing.Size(236, 73);
      this.groupBoxRemoteTiming.TabIndex = 2;
      this.groupBoxRemoteTiming.TabStop = false;
      this.groupBoxRemoteTiming.Text = "Remote button timing (in milliseconds)";
      // 
      // checkBoxUseSystemRatesDelay
      // 
      this.checkBoxUseSystemRatesDelay.AutoSize = true;
      this.checkBoxUseSystemRatesDelay.Location = new System.Drawing.Point(6, 131);
      this.checkBoxUseSystemRatesDelay.Name = "checkBoxUseSystemRatesDelay";
      this.checkBoxUseSystemRatesDelay.Size = new System.Drawing.Size(187, 17);
      this.checkBoxUseSystemRatesDelay.TabIndex = 1;
      this.checkBoxUseSystemRatesDelay.Text = "Use system keyboard rate settings";
      this.toolTips.SetToolTip(this.checkBoxUseSystemRatesDelay, "Use the system keyboard repeat rate settings for remote button timing");
      this.checkBoxUseSystemRatesDelay.UseVisualStyleBackColor = true;
      this.checkBoxUseSystemRatesDelay.CheckedChanged += new System.EventHandler(this.checkBoxUseSystemRatesDelay_CheckedChanged);
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
      this.tabPageRemote.Controls.Add(this.checkBoxDoRepeats);
      this.tabPageRemote.Controls.Add(this.checkBoxUseSystemRatesDelay);
      this.tabPageRemote.Controls.Add(this.groupBoxRemoteTiming);
      this.tabPageRemote.Location = new System.Drawing.Point(4, 22);
      this.tabPageRemote.Name = "tabPageRemote";
      this.tabPageRemote.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageRemote.Size = new System.Drawing.Size(248, 233);
      this.tabPageRemote.TabIndex = 1;
      this.tabPageRemote.Text = "Remote";
      this.tabPageRemote.UseVisualStyleBackColor = true;
      // 
      // checkBoxDoRepeats
      // 
      this.checkBoxDoRepeats.AutoSize = true;
      this.checkBoxDoRepeats.Location = new System.Drawing.Point(6, 108);
      this.checkBoxDoRepeats.Name = "checkBoxDoRepeats";
      this.checkBoxDoRepeats.Size = new System.Drawing.Size(83, 17);
      this.checkBoxDoRepeats.TabIndex = 1;
      this.checkBoxDoRepeats.Text = "Do Repeats";
      this.checkBoxDoRepeats.UseVisualStyleBackColor = true;
      this.checkBoxDoRepeats.CheckedChanged += new System.EventHandler(this.checkBoxDoRepeats_CheckedChanged);
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
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFirstRepeatDelay)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeldRepeatDelay)).EndInit();
      this.groupBoxRemoteTiming.ResumeLayout(false);
      this.tabControl.ResumeLayout(false);
      this.tabPageRemote.ResumeLayout(false);
      this.tabPageRemote.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelFirstRepeatDelay;
        private System.Windows.Forms.Label labelHeldRepeatDelay;
        private System.Windows.Forms.NumericUpDown numericUpDownFirstRepeatDelay;
        private System.Windows.Forms.NumericUpDown numericUpDownHeldRepeatDelay;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBoxRemoteTiming;
        private System.Windows.Forms.ToolTip toolTips;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageRemote;
        private System.Windows.Forms.CheckBox checkBoxUseSystemRatesDelay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.CheckBox checkBoxDoRepeats;

    }

}
