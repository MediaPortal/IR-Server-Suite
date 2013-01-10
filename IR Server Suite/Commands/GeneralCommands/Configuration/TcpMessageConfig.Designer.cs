namespace IrssCommands.General
{
  partial class TcpMessageConfig
  {
    /// <summary> 
    /// Erforderliche Designervariable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Verwendete Ressourcen bereinigen.
    /// </summary>
    /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Vom Komponenten-Designer generierter Code

    /// <summary> 
    /// Erforderliche Methode für die Designerunterstützung. 
    /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.toolTip = new System.Windows.Forms.ToolTip(this.components);
      this.groupBoxMessageDetails = new System.Windows.Forms.GroupBox();
      this.numericUpDownPort = new System.Windows.Forms.NumericUpDown();
      this.labelPort = new System.Windows.Forms.Label();
      this.textBoxText = new System.Windows.Forms.TextBox();
      this.labelText = new System.Windows.Forms.Label();
      this.textBoxIP = new System.Windows.Forms.TextBox();
      this.labelIP = new System.Windows.Forms.Label();
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
      this.groupBoxMessageDetails.Location = new System.Drawing.Point(3, 3);
      this.groupBoxMessageDetails.Name = "groupBoxMessageDetails";
      this.groupBoxMessageDetails.Size = new System.Drawing.Size(294, 105);
      this.groupBoxMessageDetails.TabIndex = 1;
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
      this.numericUpDownPort.Size = new System.Drawing.Size(78, 20);
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
      this.textBoxText.Size = new System.Drawing.Size(206, 20);
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
      this.textBoxIP.Size = new System.Drawing.Size(206, 20);
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
      // TcpMessageCommandConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBoxMessageDetails);
      this.Name = "TcpMessageCommandConfig";
      this.Size = new System.Drawing.Size(300, 111);
      this.groupBoxMessageDetails.ResumeLayout(false);
      this.groupBoxMessageDetails.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.GroupBox groupBoxMessageDetails;
    private System.Windows.Forms.NumericUpDown numericUpDownPort;
    private System.Windows.Forms.Label labelPort;
    private System.Windows.Forms.TextBox textBoxText;
    private System.Windows.Forms.Label labelText;
    private System.Windows.Forms.TextBox textBoxIP;
    private System.Windows.Forms.Label labelIP;
  }
}
