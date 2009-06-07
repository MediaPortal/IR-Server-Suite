namespace IrssUtils.Forms
{
  partial class BeepCommand
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
      this.numericUpDownFreq = new System.Windows.Forms.NumericUpDown();
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.labelFrequency = new System.Windows.Forms.Label();
      this.labelDuration = new System.Windows.Forms.Label();
      this.numericUpDownDuration = new System.Windows.Forms.NumericUpDown();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFreq)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).BeginInit();
      this.SuspendLayout();
      // 
      // numericUpDownFreq
      // 
      this.numericUpDownFreq.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
      this.numericUpDownFreq.Location = new System.Drawing.Point(87, 8);
      this.numericUpDownFreq.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
      this.numericUpDownFreq.Minimum = new decimal(new int[] {
            37,
            0,
            0,
            0});
      this.numericUpDownFreq.Name = "numericUpDownFreq";
      this.numericUpDownFreq.Size = new System.Drawing.Size(73, 20);
      this.numericUpDownFreq.TabIndex = 1;
      this.numericUpDownFreq.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownFreq.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(24, 80);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 2;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(96, 80);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 3;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // labelFrequency
      // 
      this.labelFrequency.Location = new System.Drawing.Point(8, 8);
      this.labelFrequency.Name = "labelFrequency";
      this.labelFrequency.Size = new System.Drawing.Size(80, 20);
      this.labelFrequency.TabIndex = 4;
      this.labelFrequency.Text = "Frequency:";
      this.labelFrequency.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelDuration
      // 
      this.labelDuration.Location = new System.Drawing.Point(8, 40);
      this.labelDuration.Name = "labelDuration";
      this.labelDuration.Size = new System.Drawing.Size(80, 20);
      this.labelDuration.TabIndex = 5;
      this.labelDuration.Text = "Duration:";
      this.labelDuration.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // numericUpDownDuration
      // 
      this.numericUpDownDuration.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
      this.numericUpDownDuration.Location = new System.Drawing.Point(88, 40);
      this.numericUpDownDuration.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
      this.numericUpDownDuration.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
      this.numericUpDownDuration.Name = "numericUpDownDuration";
      this.numericUpDownDuration.Size = new System.Drawing.Size(73, 20);
      this.numericUpDownDuration.TabIndex = 6;
      this.numericUpDownDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownDuration.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
      // 
      // BeepCommand
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(168, 113);
      this.Controls.Add(this.numericUpDownDuration);
      this.Controls.Add(this.labelDuration);
      this.Controls.Add(this.labelFrequency);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.numericUpDownFreq);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(174, 138);
      this.Name = "BeepCommand";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Beep Command";
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFreq)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.NumericUpDown numericUpDownFreq;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Label labelFrequency;
    private System.Windows.Forms.Label labelDuration;
    private System.Windows.Forms.NumericUpDown numericUpDownDuration;
  }
}