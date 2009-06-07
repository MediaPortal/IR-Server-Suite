namespace InputService.Plugin
{
  partial class Configure
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
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.toolTips = new System.Windows.Forms.ToolTip(this.components);
      this.numericUpDownButtonChannelNumber = new System.Windows.Forms.NumericUpDown();
      this.labelChannelNumber = new System.Windows.Forms.Label();
      this.labelUseChannelContrl = new System.Windows.Forms.Label();
      this.checkBoxUseChannelControl = new System.Windows.Forms.CheckBox();
      this.buttonGetChannelNumber = new System.Windows.Forms.Button();
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButtonChannelNumber)).BeginInit();
      this.SuspendLayout();
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(137, 134);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 4;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(209, 134);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 5;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // numericUpDownButtonChannelNumber
      // 
      this.numericUpDownButtonChannelNumber.Location = new System.Drawing.Point(152, 35);
      this.numericUpDownButtonChannelNumber.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
      this.numericUpDownButtonChannelNumber.Name = "numericUpDownButtonChannelNumber";
      this.numericUpDownButtonChannelNumber.Size = new System.Drawing.Size(117, 20);
      this.numericUpDownButtonChannelNumber.TabIndex = 1;
      this.numericUpDownButtonChannelNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownButtonChannelNumber.ThousandsSeparator = true;
      this.toolTips.SetToolTip(this.numericUpDownButtonChannelNumber, "How long between repeated buttons (in milliseconds)");
      // 
      // labelChannelNumber
      // 
      this.labelChannelNumber.Location = new System.Drawing.Point(12, 33);
      this.labelChannelNumber.Name = "labelChannelNumber";
      this.labelChannelNumber.Size = new System.Drawing.Size(134, 20);
      this.labelChannelNumber.TabIndex = 0;
      this.labelChannelNumber.Text = "Channel number: ";
      this.labelChannelNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelUseChannelContrl
      // 
      this.labelUseChannelContrl.Location = new System.Drawing.Point(12, 9);
      this.labelUseChannelContrl.Name = "labelUseChannelContrl";
      this.labelUseChannelContrl.Size = new System.Drawing.Size(134, 20);
      this.labelUseChannelContrl.TabIndex = 6;
      this.labelUseChannelContrl.Text = "Use channel control:";
      this.labelUseChannelContrl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // checkBoxUseChannelControl
      // 
      this.checkBoxUseChannelControl.AutoSize = true;
      this.checkBoxUseChannelControl.Location = new System.Drawing.Point(152, 12);
      this.checkBoxUseChannelControl.Name = "checkBoxUseChannelControl";
      this.checkBoxUseChannelControl.Size = new System.Drawing.Size(15, 14);
      this.checkBoxUseChannelControl.TabIndex = 7;
      this.checkBoxUseChannelControl.UseVisualStyleBackColor = true;
      this.checkBoxUseChannelControl.CheckedChanged += new System.EventHandler(this.checkBoxUseChannelControl_CheckedChanged);
      // 
      // buttonGetChannelNumber
      // 
      this.buttonGetChannelNumber.Location = new System.Drawing.Point(152, 61);
      this.buttonGetChannelNumber.Name = "buttonGetChannelNumber";
      this.buttonGetChannelNumber.Size = new System.Drawing.Size(117, 23);
      this.buttonGetChannelNumber.TabIndex = 8;
      this.buttonGetChannelNumber.Text = "Get Channel number";
      this.buttonGetChannelNumber.UseVisualStyleBackColor = true;
      this.buttonGetChannelNumber.Click += new System.EventHandler(this.buttonGetChannelNumber_Click);
      // 
      // timer1
      // 
      this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
      // 
      // Configure
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(281, 167);
      this.Controls.Add(this.buttonGetChannelNumber);
      this.Controls.Add(this.checkBoxUseChannelControl);
      this.Controls.Add(this.labelUseChannelContrl);
      this.Controls.Add(this.labelChannelNumber);
      this.Controls.Add(this.numericUpDownButtonChannelNumber);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(256, 164);
      this.Name = "Configure";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "X10 Configuration";
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButtonChannelNumber)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.Label labelChannelNumber;
    private System.Windows.Forms.Label labelUseChannelContrl;
    private System.Windows.Forms.CheckBox checkBoxUseChannelControl;
    private System.Windows.Forms.NumericUpDown numericUpDownButtonChannelNumber;
    private System.Windows.Forms.Button buttonGetChannelNumber;
    private System.Windows.Forms.Timer timer1;
  }
}