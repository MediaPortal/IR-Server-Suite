namespace HcwTransceiver
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
      this.labelRepeatCount = new System.Windows.Forms.Label();
      this.numericUpDownButtonRepeatDelay = new System.Windows.Forms.NumericUpDown();
      this.numericUpDownRepeatCount = new System.Windows.Forms.NumericUpDown();
      this.labelButtonRepeatDelay = new System.Windows.Forms.Label();
      this.numericUpDownLearnTimeout = new System.Windows.Forms.NumericUpDown();
      this.labelLearnIRTimeout = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButtonRepeatDelay)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRepeatCount)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLearnTimeout)).BeginInit();
      this.SuspendLayout();
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(104, 104);
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
      this.buttonCancel.Location = new System.Drawing.Point(176, 104);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 5;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // labelRepeatCount
      // 
      this.labelRepeatCount.Location = new System.Drawing.Point(8, 40);
      this.labelRepeatCount.Name = "labelRepeatCount";
      this.labelRepeatCount.Size = new System.Drawing.Size(144, 20);
      this.labelRepeatCount.TabIndex = 2;
      this.labelRepeatCount.Text = "IR blast repeat count:";
      this.labelRepeatCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // numericUpDownButtonRepeatDelay
      // 
      this.numericUpDownButtonRepeatDelay.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
      this.numericUpDownButtonRepeatDelay.Location = new System.Drawing.Point(152, 8);
      this.numericUpDownButtonRepeatDelay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this.numericUpDownButtonRepeatDelay.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
      this.numericUpDownButtonRepeatDelay.Name = "numericUpDownButtonRepeatDelay";
      this.numericUpDownButtonRepeatDelay.Size = new System.Drawing.Size(88, 20);
      this.numericUpDownButtonRepeatDelay.TabIndex = 1;
      this.numericUpDownButtonRepeatDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownButtonRepeatDelay.ThousandsSeparator = true;
      this.toolTips.SetToolTip(this.numericUpDownButtonRepeatDelay, "How long between repeated buttons (in milliseconds)");
      this.numericUpDownButtonRepeatDelay.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      // 
      // numericUpDownRepeatCount
      // 
      this.numericUpDownRepeatCount.Location = new System.Drawing.Point(152, 40);
      this.numericUpDownRepeatCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numericUpDownRepeatCount.Name = "numericUpDownRepeatCount";
      this.numericUpDownRepeatCount.Size = new System.Drawing.Size(88, 20);
      this.numericUpDownRepeatCount.TabIndex = 3;
      this.numericUpDownRepeatCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownRepeatCount.ThousandsSeparator = true;
      this.numericUpDownRepeatCount.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
      // 
      // labelButtonRepeatDelay
      // 
      this.labelButtonRepeatDelay.Location = new System.Drawing.Point(8, 8);
      this.labelButtonRepeatDelay.Name = "labelButtonRepeatDelay";
      this.labelButtonRepeatDelay.Size = new System.Drawing.Size(144, 20);
      this.labelButtonRepeatDelay.TabIndex = 0;
      this.labelButtonRepeatDelay.Text = "Button repeat delay:";
      this.labelButtonRepeatDelay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // numericUpDownLearnTimeout
      // 
      this.numericUpDownLearnTimeout.Increment = new decimal(new int[] {
            500,
            0,
            0,
            0});
      this.numericUpDownLearnTimeout.Location = new System.Drawing.Point(152, 72);
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
      this.numericUpDownLearnTimeout.TabIndex = 7;
      this.numericUpDownLearnTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownLearnTimeout.ThousandsSeparator = true;
      this.toolTips.SetToolTip(this.numericUpDownLearnTimeout, "When teaching IR commands this is how long before the process times out");
      this.numericUpDownLearnTimeout.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      // 
      // labelLearnIRTimeout
      // 
      this.labelLearnIRTimeout.Location = new System.Drawing.Point(8, 72);
      this.labelLearnIRTimeout.Name = "labelLearnIRTimeout";
      this.labelLearnIRTimeout.Size = new System.Drawing.Size(144, 20);
      this.labelLearnIRTimeout.TabIndex = 6;
      this.labelLearnIRTimeout.Text = "Learn IR timeout:";
      this.labelLearnIRTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // Configure
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(248, 137);
      this.Controls.Add(this.labelLearnIRTimeout);
      this.Controls.Add(this.numericUpDownLearnTimeout);
      this.Controls.Add(this.labelButtonRepeatDelay);
      this.Controls.Add(this.numericUpDownRepeatCount);
      this.Controls.Add(this.numericUpDownButtonRepeatDelay);
      this.Controls.Add(this.labelRepeatCount);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(256, 164);
      this.Name = "Configure";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "HCW Configuration";
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButtonRepeatDelay)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRepeatCount)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLearnTimeout)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.Label labelRepeatCount;
    private System.Windows.Forms.NumericUpDown numericUpDownButtonRepeatDelay;
    private System.Windows.Forms.NumericUpDown numericUpDownRepeatCount;
    private System.Windows.Forms.Label labelButtonRepeatDelay;
    private System.Windows.Forms.NumericUpDown numericUpDownLearnTimeout;
    private System.Windows.Forms.Label labelLearnIRTimeout;
  }
}