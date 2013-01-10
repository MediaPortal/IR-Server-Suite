namespace IrssCommands.General
{
  partial class PopupConfig
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
      this.labelSeconds = new System.Windows.Forms.Label();
      this.numericUpDownTimeout = new System.Windows.Forms.NumericUpDown();
      this.labelTimeout = new System.Windows.Forms.Label();
      this.textBoxText = new System.Windows.Forms.TextBox();
      this.labelText = new System.Windows.Forms.Label();
      this.textBoxHeading = new System.Windows.Forms.TextBox();
      this.labelHeading = new System.Windows.Forms.Label();
      this.groupBoxMessageDetails.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeout)).BeginInit();
      this.SuspendLayout();
      // 
      // groupBoxMessageDetails
      // 
      this.groupBoxMessageDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxMessageDetails.Controls.Add(this.labelSeconds);
      this.groupBoxMessageDetails.Controls.Add(this.numericUpDownTimeout);
      this.groupBoxMessageDetails.Controls.Add(this.labelTimeout);
      this.groupBoxMessageDetails.Controls.Add(this.textBoxText);
      this.groupBoxMessageDetails.Controls.Add(this.labelText);
      this.groupBoxMessageDetails.Controls.Add(this.textBoxHeading);
      this.groupBoxMessageDetails.Controls.Add(this.labelHeading);
      this.groupBoxMessageDetails.Location = new System.Drawing.Point(3, 3);
      this.groupBoxMessageDetails.Name = "groupBoxMessageDetails";
      this.groupBoxMessageDetails.Size = new System.Drawing.Size(284, 98);
      this.groupBoxMessageDetails.TabIndex = 1;
      this.groupBoxMessageDetails.TabStop = false;
      this.groupBoxMessageDetails.Text = "Message details";
      // 
      // labelSeconds
      // 
      this.labelSeconds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.labelSeconds.Location = new System.Drawing.Point(156, 72);
      this.labelSeconds.Name = "labelSeconds";
      this.labelSeconds.Size = new System.Drawing.Size(120, 20);
      this.labelSeconds.TabIndex = 6;
      this.labelSeconds.Text = "seconds";
      this.labelSeconds.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // numericUpDownTimeout
      // 
      this.numericUpDownTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.numericUpDownTimeout.Location = new System.Drawing.Point(72, 72);
      this.numericUpDownTimeout.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
      this.numericUpDownTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numericUpDownTimeout.Name = "numericUpDownTimeout";
      this.numericUpDownTimeout.Size = new System.Drawing.Size(76, 20);
      this.numericUpDownTimeout.TabIndex = 5;
      this.numericUpDownTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownTimeout.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // labelTimeout
      // 
      this.labelTimeout.Location = new System.Drawing.Point(8, 72);
      this.labelTimeout.Name = "labelTimeout";
      this.labelTimeout.Size = new System.Drawing.Size(64, 20);
      this.labelTimeout.TabIndex = 4;
      this.labelTimeout.Text = "Timeout:";
      this.labelTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxText
      // 
      this.textBoxText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxText.Location = new System.Drawing.Point(72, 48);
      this.textBoxText.Name = "textBoxText";
      this.textBoxText.Size = new System.Drawing.Size(204, 20);
      this.textBoxText.TabIndex = 3;
      // 
      // labelText
      // 
      this.labelText.Location = new System.Drawing.Point(8, 48);
      this.labelText.Name = "labelText";
      this.labelText.Size = new System.Drawing.Size(64, 20);
      this.labelText.TabIndex = 2;
      this.labelText.Text = "Text:";
      this.labelText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxHeading
      // 
      this.textBoxHeading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxHeading.Location = new System.Drawing.Point(72, 24);
      this.textBoxHeading.Name = "textBoxHeading";
      this.textBoxHeading.Size = new System.Drawing.Size(204, 20);
      this.textBoxHeading.TabIndex = 1;
      // 
      // labelHeading
      // 
      this.labelHeading.Location = new System.Drawing.Point(8, 24);
      this.labelHeading.Name = "labelHeading";
      this.labelHeading.Size = new System.Drawing.Size(64, 20);
      this.labelHeading.TabIndex = 0;
      this.labelHeading.Text = "Heading:";
      this.labelHeading.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // PopupCommandConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBoxMessageDetails);
      this.Name = "PopupCommandConfig";
      this.Size = new System.Drawing.Size(290, 104);
      this.groupBoxMessageDetails.ResumeLayout(false);
      this.groupBoxMessageDetails.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeout)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.GroupBox groupBoxMessageDetails;
    private System.Windows.Forms.Label labelSeconds;
    private System.Windows.Forms.NumericUpDown numericUpDownTimeout;
    private System.Windows.Forms.Label labelTimeout;
    private System.Windows.Forms.TextBox textBoxText;
    private System.Windows.Forms.Label labelText;
    private System.Windows.Forms.TextBox textBoxHeading;
    private System.Windows.Forms.Label labelHeading;
  }
}
