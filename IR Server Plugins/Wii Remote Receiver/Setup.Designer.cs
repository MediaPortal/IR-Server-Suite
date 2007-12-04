namespace WiiRemoteReceiver
{
  partial class Setup
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
      this.labelMouseSensitivity = new System.Windows.Forms.Label();
      this.numericUpDownMouseSensitivity = new System.Windows.Forms.NumericUpDown();
      this.checkBoxHandleMouseLocal = new System.Windows.Forms.CheckBox();
      this.checkBoxNunchukMouse = new System.Windows.Forms.CheckBox();
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.checkBoxLED1 = new System.Windows.Forms.CheckBox();
      this.checkBoxLED2 = new System.Windows.Forms.CheckBox();
      this.checkBoxLED3 = new System.Windows.Forms.CheckBox();
      this.checkBoxLED4 = new System.Windows.Forms.CheckBox();
      this.groupBoxLEDs = new System.Windows.Forms.GroupBox();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMouseSensitivity)).BeginInit();
      this.groupBoxLEDs.SuspendLayout();
      this.SuspendLayout();
      // 
      // labelMouseSensitivity
      // 
      this.labelMouseSensitivity.Location = new System.Drawing.Point(8, 80);
      this.labelMouseSensitivity.Name = "labelMouseSensitivity";
      this.labelMouseSensitivity.Size = new System.Drawing.Size(112, 20);
      this.labelMouseSensitivity.TabIndex = 2;
      this.labelMouseSensitivity.Text = "Mouse sensitivity:";
      this.labelMouseSensitivity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // numericUpDownMouseSensitivity
      // 
      this.numericUpDownMouseSensitivity.DecimalPlaces = 1;
      this.numericUpDownMouseSensitivity.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
      this.numericUpDownMouseSensitivity.Location = new System.Drawing.Point(120, 80);
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
      this.numericUpDownMouseSensitivity.Value = new decimal(new int[] {
            15,
            0,
            0,
            65536});
      // 
      // checkBoxHandleMouseLocal
      // 
      this.checkBoxHandleMouseLocal.AutoSize = true;
      this.checkBoxHandleMouseLocal.Checked = true;
      this.checkBoxHandleMouseLocal.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxHandleMouseLocal.Location = new System.Drawing.Point(8, 16);
      this.checkBoxHandleMouseLocal.Name = "checkBoxHandleMouseLocal";
      this.checkBoxHandleMouseLocal.Size = new System.Drawing.Size(126, 17);
      this.checkBoxHandleMouseLocal.TabIndex = 0;
      this.checkBoxHandleMouseLocal.Text = "Handle mouse locally";
      this.checkBoxHandleMouseLocal.UseVisualStyleBackColor = true;
      // 
      // checkBoxNunchukMouse
      // 
      this.checkBoxNunchukMouse.AutoSize = true;
      this.checkBoxNunchukMouse.Checked = true;
      this.checkBoxNunchukMouse.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxNunchukMouse.Location = new System.Drawing.Point(8, 48);
      this.checkBoxNunchukMouse.Name = "checkBoxNunchukMouse";
      this.checkBoxNunchukMouse.Size = new System.Drawing.Size(140, 17);
      this.checkBoxNunchukMouse.TabIndex = 1;
      this.checkBoxNunchukMouse.Text = "Use Nunchuk as mouse";
      this.checkBoxNunchukMouse.UseVisualStyleBackColor = true;
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(88, 184);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(56, 24);
      this.buttonOK.TabIndex = 5;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(152, 184);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(56, 24);
      this.buttonCancel.TabIndex = 6;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // checkBoxLED1
      // 
      this.checkBoxLED1.Location = new System.Drawing.Point(8, 24);
      this.checkBoxLED1.Name = "checkBoxLED1";
      this.checkBoxLED1.Size = new System.Drawing.Size(40, 24);
      this.checkBoxLED1.TabIndex = 0;
      this.checkBoxLED1.Text = "1";
      this.checkBoxLED1.UseVisualStyleBackColor = true;
      // 
      // checkBoxLED2
      // 
      this.checkBoxLED2.Checked = true;
      this.checkBoxLED2.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxLED2.Location = new System.Drawing.Point(56, 24);
      this.checkBoxLED2.Name = "checkBoxLED2";
      this.checkBoxLED2.Size = new System.Drawing.Size(40, 24);
      this.checkBoxLED2.TabIndex = 1;
      this.checkBoxLED2.Text = "2";
      this.checkBoxLED2.UseVisualStyleBackColor = true;
      // 
      // checkBoxLED3
      // 
      this.checkBoxLED3.Checked = true;
      this.checkBoxLED3.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxLED3.Location = new System.Drawing.Point(104, 24);
      this.checkBoxLED3.Name = "checkBoxLED3";
      this.checkBoxLED3.Size = new System.Drawing.Size(40, 24);
      this.checkBoxLED3.TabIndex = 2;
      this.checkBoxLED3.Text = "3";
      this.checkBoxLED3.UseVisualStyleBackColor = true;
      // 
      // checkBoxLED4
      // 
      this.checkBoxLED4.Location = new System.Drawing.Point(152, 24);
      this.checkBoxLED4.Name = "checkBoxLED4";
      this.checkBoxLED4.Size = new System.Drawing.Size(40, 24);
      this.checkBoxLED4.TabIndex = 3;
      this.checkBoxLED4.Text = "4";
      this.checkBoxLED4.UseVisualStyleBackColor = true;
      // 
      // groupBoxLEDs
      // 
      this.groupBoxLEDs.Controls.Add(this.checkBoxLED1);
      this.groupBoxLEDs.Controls.Add(this.checkBoxLED4);
      this.groupBoxLEDs.Controls.Add(this.checkBoxLED2);
      this.groupBoxLEDs.Controls.Add(this.checkBoxLED3);
      this.groupBoxLEDs.Location = new System.Drawing.Point(8, 112);
      this.groupBoxLEDs.Name = "groupBoxLEDs";
      this.groupBoxLEDs.Size = new System.Drawing.Size(200, 56);
      this.groupBoxLEDs.TabIndex = 4;
      this.groupBoxLEDs.TabStop = false;
      this.groupBoxLEDs.Text = "Remote LEDs";
      // 
      // Setup
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(216, 217);
      this.Controls.Add(this.groupBoxLEDs);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.checkBoxNunchukMouse);
      this.Controls.Add(this.labelMouseSensitivity);
      this.Controls.Add(this.numericUpDownMouseSensitivity);
      this.Controls.Add(this.checkBoxHandleMouseLocal);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(224, 244);
      this.Name = "Setup";
      this.ShowIcon = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Wii Remote Setup";
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMouseSensitivity)).EndInit();
      this.groupBoxLEDs.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label labelMouseSensitivity;
    private System.Windows.Forms.NumericUpDown numericUpDownMouseSensitivity;
    private System.Windows.Forms.CheckBox checkBoxHandleMouseLocal;
    private System.Windows.Forms.CheckBox checkBoxNunchukMouse;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.CheckBox checkBoxLED1;
    private System.Windows.Forms.CheckBox checkBoxLED2;
    private System.Windows.Forms.CheckBox checkBoxLED3;
    private System.Windows.Forms.CheckBox checkBoxLED4;
    private System.Windows.Forms.GroupBox groupBoxLEDs;
  }
}