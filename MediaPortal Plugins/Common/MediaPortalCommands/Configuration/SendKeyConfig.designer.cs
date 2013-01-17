namespace IrssCommands.MediaPortal
{
  partial class SendKeyConfig
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
      this.numericUpDownCode = new System.Windows.Forms.NumericUpDown();
      this.labelCode = new System.Windows.Forms.Label();
      this.labelChar = new System.Windows.Forms.Label();
      this.numericUpDownChar = new System.Windows.Forms.NumericUpDown();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCode)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownChar)).BeginInit();
      this.SuspendLayout();
      // 
      // numericUpDownCode
      // 
      this.numericUpDownCode.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.numericUpDownCode.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
      this.numericUpDownCode.Location = new System.Drawing.Point(89, 40);
      this.numericUpDownCode.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
      this.numericUpDownCode.Name = "numericUpDownCode";
      this.numericUpDownCode.Size = new System.Drawing.Size(73, 20);
      this.numericUpDownCode.TabIndex = 10;
      this.numericUpDownCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownCode.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
      // 
      // labelCode
      // 
      this.labelCode.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.labelCode.Location = new System.Drawing.Point(3, 40);
      this.labelCode.Name = "labelCode";
      this.labelCode.Size = new System.Drawing.Size(80, 20);
      this.labelCode.TabIndex = 9;
      this.labelCode.Text = "Key Code:";
      this.labelCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelChar
      // 
      this.labelChar.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.labelChar.Location = new System.Drawing.Point(3, 8);
      this.labelChar.Name = "labelChar";
      this.labelChar.Size = new System.Drawing.Size(80, 20);
      this.labelChar.TabIndex = 8;
      this.labelChar.Text = "Key Char:";
      this.labelChar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // numericUpDownChar
      // 
      this.numericUpDownChar.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.numericUpDownChar.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
      this.numericUpDownChar.Location = new System.Drawing.Point(89, 8);
      this.numericUpDownChar.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
      this.numericUpDownChar.Name = "numericUpDownChar";
      this.numericUpDownChar.Size = new System.Drawing.Size(73, 20);
      this.numericUpDownChar.TabIndex = 7;
      this.numericUpDownChar.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownChar.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
      // 
      // CommandSendKeyConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.numericUpDownCode);
      this.Controls.Add(this.labelCode);
      this.Controls.Add(this.labelChar);
      this.Controls.Add(this.numericUpDownChar);
      this.Name = "CommandSendKeyConfig";
      this.Size = new System.Drawing.Size(169, 70);
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCode)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownChar)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.NumericUpDown numericUpDownCode;
    private System.Windows.Forms.Label labelCode;
    private System.Windows.Forms.Label labelChar;
    private System.Windows.Forms.NumericUpDown numericUpDownChar;
  }
}
