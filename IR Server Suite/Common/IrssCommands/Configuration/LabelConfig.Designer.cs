namespace IrssCommands
{
  partial class LabelConfig
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
      this.textBoxLabel = new System.Windows.Forms.TextBox();
      this.labelLabelName = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // textBoxLabel
      // 
      this.textBoxLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxLabel.Location = new System.Drawing.Point(59, 32);
      this.textBoxLabel.Name = "textBoxLabel";
      this.textBoxLabel.Size = new System.Drawing.Size(100, 20);
      this.textBoxLabel.TabIndex = 3;
      // 
      // labelLabelName
      // 
      this.labelLabelName.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.labelLabelName.Location = new System.Drawing.Point(3, 32);
      this.labelLabelName.Name = "labelLabelName";
      this.labelLabelName.Size = new System.Drawing.Size(56, 20);
      this.labelLabelName.TabIndex = 2;
      this.labelLabelName.Text = "Label:";
      this.labelLabelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // LabelConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.textBoxLabel);
      this.Controls.Add(this.labelLabelName);
      this.MaximumSize = new System.Drawing.Size(544, 393);
      this.MinimumSize = new System.Drawing.Size(162, 84);
      this.Name = "LabelConfig";
      this.Size = new System.Drawing.Size(162, 84);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox textBoxLabel;
    private System.Windows.Forms.Label labelLabelName;


  }
}
