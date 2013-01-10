namespace IrssCommands.General
{
  partial class BeepConfig
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
      this.numericUpDownDuration = new System.Windows.Forms.NumericUpDown();
      this.labelDuration = new System.Windows.Forms.Label();
      this.labelFrequency = new System.Windows.Forms.Label();
      this.numericUpDownFreq = new System.Windows.Forms.NumericUpDown();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFreq)).BeginInit();
      this.SuspendLayout();
      // 
      // numericUpDownDuration
      // 
      this.numericUpDownDuration.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.numericUpDownDuration.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
      this.numericUpDownDuration.Location = new System.Drawing.Point(89, 40);
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
      this.numericUpDownDuration.TabIndex = 10;
      this.numericUpDownDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownDuration.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
      // 
      // labelDuration
      // 
      this.labelDuration.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.labelDuration.Location = new System.Drawing.Point(3, 40);
      this.labelDuration.Name = "labelDuration";
      this.labelDuration.Size = new System.Drawing.Size(80, 20);
      this.labelDuration.TabIndex = 9;
      this.labelDuration.Text = "Duration:";
      this.labelDuration.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelFrequency
      // 
      this.labelFrequency.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.labelFrequency.Location = new System.Drawing.Point(3, 8);
      this.labelFrequency.Name = "labelFrequency";
      this.labelFrequency.Size = new System.Drawing.Size(80, 20);
      this.labelFrequency.TabIndex = 8;
      this.labelFrequency.Text = "Frequency:";
      this.labelFrequency.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // numericUpDownFreq
      // 
      this.numericUpDownFreq.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.numericUpDownFreq.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
      this.numericUpDownFreq.Location = new System.Drawing.Point(89, 8);
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
      this.numericUpDownFreq.TabIndex = 7;
      this.numericUpDownFreq.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownFreq.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
      // 
      // BeepConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.numericUpDownDuration);
      this.Controls.Add(this.labelDuration);
      this.Controls.Add(this.labelFrequency);
      this.Controls.Add(this.numericUpDownFreq);
      this.Name = "BeepConfig";
      this.Size = new System.Drawing.Size(169, 70);
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFreq)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.NumericUpDown numericUpDownDuration;
    private System.Windows.Forms.Label labelDuration;
    private System.Windows.Forms.Label labelFrequency;
    private System.Windows.Forms.NumericUpDown numericUpDownFreq;
  }
}
