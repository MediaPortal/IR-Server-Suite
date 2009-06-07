namespace IrssUtils.Forms
{
  partial class TcpMessageCommand
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
      this.groupBoxMessageDetails = new System.Windows.Forms.GroupBox();
      this.numericUpDownPort = new System.Windows.Forms.NumericUpDown();
      this.labelPort = new System.Windows.Forms.Label();
      this.textBoxText = new System.Windows.Forms.TextBox();
      this.labelText = new System.Windows.Forms.Label();
      this.textBoxIP = new System.Windows.Forms.TextBox();
      this.labelIP = new System.Windows.Forms.Label();
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.groupBoxMessageDetails.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).BeginInit();
      this.SuspendLayout();
      // 
      // groupBoxMessageDetails
      // 
      this.groupBoxMessageDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxMessageDetails.Controls.Add(this.numericUpDownPort);
      this.groupBoxMessageDetails.Controls.Add(this.labelPort);
      this.groupBoxMessageDetails.Controls.Add(this.textBoxText);
      this.groupBoxMessageDetails.Controls.Add(this.labelText);
      this.groupBoxMessageDetails.Controls.Add(this.textBoxIP);
      this.groupBoxMessageDetails.Controls.Add(this.labelIP);
      this.groupBoxMessageDetails.Location = new System.Drawing.Point(8, 8);
      this.groupBoxMessageDetails.Name = "groupBoxMessageDetails";
      this.groupBoxMessageDetails.Size = new System.Drawing.Size(288, 111);
      this.groupBoxMessageDetails.TabIndex = 0;
      this.groupBoxMessageDetails.TabStop = false;
      this.groupBoxMessageDetails.Text = "Message details";
      // 
      // numericUpDownPort
      // 
      this.numericUpDownPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.numericUpDownPort.Location = new System.Drawing.Point(80, 48);
      this.numericUpDownPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
      this.numericUpDownPort.Name = "numericUpDownPort";
      this.numericUpDownPort.Size = new System.Drawing.Size(72, 20);
      this.numericUpDownPort.TabIndex = 5;
      this.numericUpDownPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownPort.Value = new decimal(new int[] {
            49152,
            0,
            0,
            0});
      // 
      // labelPort
      // 
      this.labelPort.Location = new System.Drawing.Point(8, 48);
      this.labelPort.Name = "labelPort";
      this.labelPort.Size = new System.Drawing.Size(72, 20);
      this.labelPort.TabIndex = 4;
      this.labelPort.Text = "Port:";
      this.labelPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxText
      // 
      this.textBoxText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxText.Location = new System.Drawing.Point(80, 80);
      this.textBoxText.Name = "textBoxText";
      this.textBoxText.Size = new System.Drawing.Size(200, 20);
      this.textBoxText.TabIndex = 3;
      // 
      // labelText
      // 
      this.labelText.Location = new System.Drawing.Point(8, 80);
      this.labelText.Name = "labelText";
      this.labelText.Size = new System.Drawing.Size(72, 20);
      this.labelText.TabIndex = 2;
      this.labelText.Text = "Text:";
      this.labelText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxIP
      // 
      this.textBoxIP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxIP.Location = new System.Drawing.Point(80, 24);
      this.textBoxIP.Name = "textBoxIP";
      this.textBoxIP.Size = new System.Drawing.Size(200, 20);
      this.textBoxIP.TabIndex = 1;
      this.textBoxIP.Text = "localhost";
      // 
      // labelIP
      // 
      this.labelIP.Location = new System.Drawing.Point(8, 24);
      this.labelIP.Name = "labelIP";
      this.labelIP.Size = new System.Drawing.Size(72, 20);
      this.labelIP.TabIndex = 0;
      this.labelIP.Text = "IP address:";
      this.labelIP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(160, 128);
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
      this.buttonCancel.Location = new System.Drawing.Point(232, 128);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 2;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // TcpMessageCommand
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(304, 160);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.groupBoxMessageDetails);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(312, 194);
      this.Name = "TcpMessageCommand";
      this.ShowIcon = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "TCP Message Command";
      this.groupBoxMessageDetails.ResumeLayout(false);
      this.groupBoxMessageDetails.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBoxMessageDetails;
    private System.Windows.Forms.Label labelIP;
    private System.Windows.Forms.NumericUpDown numericUpDownPort;
    private System.Windows.Forms.Label labelPort;
    private System.Windows.Forms.TextBox textBoxText;
    private System.Windows.Forms.Label labelText;
    private System.Windows.Forms.TextBox textBoxIP;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
  }
}