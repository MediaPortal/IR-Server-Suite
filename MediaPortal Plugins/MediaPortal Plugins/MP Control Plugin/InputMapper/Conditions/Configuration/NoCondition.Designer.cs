namespace MediaPortal.Input
{
  partial class NoConditionConfig
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
      this.label = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // label
      // 
      this.label.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.label.AutoSize = true;
      this.label.Location = new System.Drawing.Point(40, 21);
      this.label.Name = "label";
      this.label.Size = new System.Drawing.Size(106, 13);
      this.label.TabIndex = 0;
      this.label.Text = "Nothing to configure.";
      // 
      // NoConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.label);
      this.Name = "NoConfig";
      this.Size = new System.Drawing.Size(187, 54);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label;
  }
}
