namespace IrssCommands.General
{
  partial class PauseConfig
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
      this.numericUpDownPause = new System.Windows.Forms.NumericUpDown();
      this.labelSpecify = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPause)).BeginInit();
      this.SuspendLayout();
      // 
      // numericUpDownPause
      // 
      this.numericUpDownPause.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.numericUpDownPause.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
      this.numericUpDownPause.Location = new System.Drawing.Point(201, 11);
      this.numericUpDownPause.Maximum = new decimal(new int[] {
            600000,
            0,
            0,
            0});
      this.numericUpDownPause.Name = "numericUpDownPause";
      this.numericUpDownPause.Size = new System.Drawing.Size(80, 20);
      this.numericUpDownPause.TabIndex = 12;
      this.numericUpDownPause.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownPause.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
      // 
      // labelSpecify
      // 
      this.labelSpecify.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.labelSpecify.Location = new System.Drawing.Point(9, 11);
      this.labelSpecify.Name = "labelSpecify";
      this.labelSpecify.Size = new System.Drawing.Size(192, 20);
      this.labelSpecify.TabIndex = 11;
      this.labelSpecify.Text = "Specify pause time (in milliseconds):";
      this.labelSpecify.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // PauseConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.numericUpDownPause);
      this.Controls.Add(this.labelSpecify);
      this.Name = "PauseConfig";
      this.Size = new System.Drawing.Size(291, 42);
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPause)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.NumericUpDown numericUpDownPause;
    private System.Windows.Forms.Label labelSpecify;
  }
}
