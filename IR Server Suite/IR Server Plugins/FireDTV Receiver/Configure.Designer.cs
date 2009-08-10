namespace IRServer.Plugin
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
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.labelDevice = new System.Windows.Forms.Label();
      this.comboBoxDevice = new System.Windows.Forms.ComboBox();
      this.labelFireDTV = new System.Windows.Forms.Label();
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
      // labelDevice
      // 
      this.labelDevice.Location = new System.Drawing.Point(12, 9);
      this.labelDevice.Name = "labelDevice";
      this.labelDevice.Size = new System.Drawing.Size(134, 20);
      this.labelDevice.TabIndex = 6;
      this.labelDevice.Text = "Please select the device:";
      this.labelDevice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // comboBoxDevice
      // 
      this.comboBoxDevice.FormattingEnabled = true;
      this.comboBoxDevice.Location = new System.Drawing.Point(15, 35);
      this.comboBoxDevice.Name = "comboBoxDevice";
      this.comboBoxDevice.Size = new System.Drawing.Size(258, 21);
      this.comboBoxDevice.TabIndex = 9;
      // 
      // labelFireDTV
      // 
      this.labelFireDTV.AutoSize = true;
      this.labelFireDTV.Location = new System.Drawing.Point(12, 91);
      this.labelFireDTV.Name = "labelFireDTV";
      this.labelFireDTV.Size = new System.Drawing.Size(134, 13);
      this.labelFireDTV.TabIndex = 10;
      this.labelFireDTV.Text = "FireDTV API Version: 1.1.1";
      // 
      // Configure
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(281, 167);
      this.Controls.Add(this.labelFireDTV);
      this.Controls.Add(this.comboBoxDevice);
      this.Controls.Add(this.labelDevice);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(256, 164);
      this.Name = "Configure";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "FireDTV Configuration";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.Timer timer1;
    private System.Windows.Forms.Label labelDevice;
    private System.Windows.Forms.ComboBox comboBoxDevice;
    private System.Windows.Forms.Label labelFireDTV;
  }
}