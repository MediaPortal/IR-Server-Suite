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
      this.numericUpDownButtonRepeatDelay = new System.Windows.Forms.NumericUpDown();
      this.labelButtonRepeatDelay = new System.Windows.Forms.Label();
      this.linkHCWDriverDownload = new System.Windows.Forms.LinkLabel();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButtonRepeatDelay)).BeginInit();
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
      // labelButtonRepeatDelay
      // 
      this.labelButtonRepeatDelay.Location = new System.Drawing.Point(8, 8);
      this.labelButtonRepeatDelay.Name = "labelButtonRepeatDelay";
      this.labelButtonRepeatDelay.Size = new System.Drawing.Size(144, 20);
      this.labelButtonRepeatDelay.TabIndex = 0;
      this.labelButtonRepeatDelay.Text = "Button repeat delay:";
      this.labelButtonRepeatDelay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // linkHCWDriverDownload
      // 
      this.linkHCWDriverDownload.AutoSize = true;
      this.linkHCWDriverDownload.Location = new System.Drawing.Point(21, 56);
      this.linkHCWDriverDownload.Name = "linkHCWDriverDownload";
      this.linkHCWDriverDownload.Size = new System.Drawing.Size(191, 26);
      this.linkHCWDriverDownload.TabIndex = 6;
      this.linkHCWDriverDownload.TabStop = true;
      this.linkHCWDriverDownload.Text = "Click here for the latest \"IR.exe\" driver \r\n(inside WinTV cd).";
      this.linkHCWDriverDownload.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkHCWDriverDownload_LinkClicked);
      // 
      // Configure
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(248, 137);
      this.Controls.Add(this.linkHCWDriverDownload);
      this.Controls.Add(this.labelButtonRepeatDelay);
      this.Controls.Add(this.numericUpDownButtonRepeatDelay);
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
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.NumericUpDown numericUpDownButtonRepeatDelay;
    private System.Windows.Forms.Label labelButtonRepeatDelay;
    private System.Windows.Forms.LinkLabel linkHCWDriverDownload;
  }
}