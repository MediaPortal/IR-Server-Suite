namespace CustomHIDReceiver
{

  partial class AdvancedSettings
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
      this.numericUpDownInputByte = new System.Windows.Forms.NumericUpDown();
      this.labelInputByte = new System.Windows.Forms.Label();
      this.checkBoxUseAllBytes = new System.Windows.Forms.CheckBox();
      this.labelInputByteMask = new System.Windows.Forms.Label();
      this.numericUpDownInputByteMask = new System.Windows.Forms.NumericUpDown();
      this.labelRepeatDelay = new System.Windows.Forms.Label();
      this.numericUpDownRepeatDelay = new System.Windows.Forms.NumericUpDown();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInputByte)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInputByteMask)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRepeatDelay)).BeginInit();
      this.SuspendLayout();
      // 
      // numericUpDownInputByte
      // 
      this.numericUpDownInputByte.Location = new System.Drawing.Point(112, 8);
      this.numericUpDownInputByte.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
      this.numericUpDownInputByte.Name = "numericUpDownInputByte";
      this.numericUpDownInputByte.Size = new System.Drawing.Size(56, 20);
      this.numericUpDownInputByte.TabIndex = 0;
      this.numericUpDownInputByte.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // labelInputByte
      // 
      this.labelInputByte.Location = new System.Drawing.Point(8, 8);
      this.labelInputByte.Name = "labelInputByte";
      this.labelInputByte.Size = new System.Drawing.Size(104, 20);
      this.labelInputByte.TabIndex = 1;
      this.labelInputByte.Text = "Input byte:";
      this.labelInputByte.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // checkBoxUseAllBytes
      // 
      this.checkBoxUseAllBytes.AutoSize = true;
      this.checkBoxUseAllBytes.Location = new System.Drawing.Point(8, 72);
      this.checkBoxUseAllBytes.Name = "checkBoxUseAllBytes";
      this.checkBoxUseAllBytes.Size = new System.Drawing.Size(112, 17);
      this.checkBoxUseAllBytes.TabIndex = 2;
      this.checkBoxUseAllBytes.Text = "Use all input bytes";
      this.checkBoxUseAllBytes.UseVisualStyleBackColor = true;
      // 
      // labelInputByteMask
      // 
      this.labelInputByteMask.Location = new System.Drawing.Point(8, 40);
      this.labelInputByteMask.Name = "labelInputByteMask";
      this.labelInputByteMask.Size = new System.Drawing.Size(104, 20);
      this.labelInputByteMask.TabIndex = 4;
      this.labelInputByteMask.Text = "Input byte mask:";
      this.labelInputByteMask.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // numericUpDownInputByteMask
      // 
      this.numericUpDownInputByteMask.Location = new System.Drawing.Point(112, 40);
      this.numericUpDownInputByteMask.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
      this.numericUpDownInputByteMask.Name = "numericUpDownInputByteMask";
      this.numericUpDownInputByteMask.Size = new System.Drawing.Size(56, 20);
      this.numericUpDownInputByteMask.TabIndex = 3;
      this.numericUpDownInputByteMask.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // labelRepeatDelay
      // 
      this.labelRepeatDelay.Location = new System.Drawing.Point(8, 104);
      this.labelRepeatDelay.Name = "labelRepeatDelay";
      this.labelRepeatDelay.Size = new System.Drawing.Size(104, 20);
      this.labelRepeatDelay.TabIndex = 6;
      this.labelRepeatDelay.Text = "Repeat delay:";
      this.labelRepeatDelay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // numericUpDownRepeatDelay
      // 
      this.numericUpDownRepeatDelay.Location = new System.Drawing.Point(112, 104);
      this.numericUpDownRepeatDelay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this.numericUpDownRepeatDelay.Name = "numericUpDownRepeatDelay";
      this.numericUpDownRepeatDelay.Size = new System.Drawing.Size(56, 20);
      this.numericUpDownRepeatDelay.TabIndex = 5;
      this.numericUpDownRepeatDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // AdvancedSettings
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(176, 137);
      this.Controls.Add(this.labelRepeatDelay);
      this.Controls.Add(this.numericUpDownRepeatDelay);
      this.Controls.Add(this.labelInputByteMask);
      this.Controls.Add(this.numericUpDownInputByteMask);
      this.Controls.Add(this.checkBoxUseAllBytes);
      this.Controls.Add(this.labelInputByte);
      this.Controls.Add(this.numericUpDownInputByte);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AdvancedSettings";
      this.ShowIcon = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Advanced Settings";
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInputByte)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInputByteMask)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRepeatDelay)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.NumericUpDown numericUpDownInputByte;
    private System.Windows.Forms.Label labelInputByte;
    private System.Windows.Forms.CheckBox checkBoxUseAllBytes;
    private System.Windows.Forms.Label labelInputByteMask;
    private System.Windows.Forms.NumericUpDown numericUpDownInputByteMask;
    private System.Windows.Forms.Label labelRepeatDelay;
    private System.Windows.Forms.NumericUpDown numericUpDownRepeatDelay;

  }

}
