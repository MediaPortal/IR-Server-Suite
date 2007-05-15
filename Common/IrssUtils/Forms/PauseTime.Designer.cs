namespace IrssUtils.Forms
{
  partial class PauseTime
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
      this.labelHeader = new System.Windows.Forms.Label();
      this.numericUpDownPause = new System.Windows.Forms.NumericUpDown();
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPause)).BeginInit();
      this.SuspendLayout();
      // 
      // labelHeader
      // 
      this.labelHeader.Dock = System.Windows.Forms.DockStyle.Top;
      this.labelHeader.Location = new System.Drawing.Point(0, 0);
      this.labelHeader.Name = "labelHeader";
      this.labelHeader.Size = new System.Drawing.Size(154, 32);
      this.labelHeader.TabIndex = 0;
      this.labelHeader.Text = "Specify pause time\r\n(in milliseconds):";
      this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // numericUpDownPause
      // 
      this.numericUpDownPause.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.numericUpDownPause.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
      this.numericUpDownPause.Location = new System.Drawing.Point(25, 40);
      this.numericUpDownPause.Maximum = new decimal(new int[] {
            600000,
            0,
            0,
            0});
      this.numericUpDownPause.Name = "numericUpDownPause";
      this.numericUpDownPause.Size = new System.Drawing.Size(104, 20);
      this.numericUpDownPause.TabIndex = 1;
      this.numericUpDownPause.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownPause.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(10, 72);
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
      this.buttonCancel.Location = new System.Drawing.Point(82, 72);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 3;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // PauseTime
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(152, 104);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.numericUpDownPause);
      this.Controls.Add(this.labelHeader);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(160, 136);
      this.Name = "PauseTime";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Pause Time";
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPause)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label labelHeader;
    private System.Windows.Forms.NumericUpDown numericUpDownPause;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
  }
}