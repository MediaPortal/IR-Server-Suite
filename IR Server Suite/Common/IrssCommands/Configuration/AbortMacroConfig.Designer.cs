namespace IrssCommands
{
  partial class AbortMacroConfig
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
      this.textBoxMessage = new System.Windows.Forms.TextBox();
      this.labelMessage = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // textBoxMessage
      // 
      this.textBoxMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxMessage.Location = new System.Drawing.Point(59, 32);
      this.textBoxMessage.Name = "textBoxMessage";
      this.textBoxMessage.Size = new System.Drawing.Size(100, 20);
      this.textBoxMessage.TabIndex = 3;
      // 
      // labelMessage
      // 
      this.labelMessage.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.labelMessage.Location = new System.Drawing.Point(3, 32);
      this.labelMessage.Name = "labelMessage";
      this.labelMessage.Size = new System.Drawing.Size(56, 20);
      this.labelMessage.TabIndex = 2;
      this.labelMessage.Text = "Message:";
      this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // LabelConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.textBoxMessage);
      this.Controls.Add(this.labelMessage);
      this.MaximumSize = new System.Drawing.Size(544, 393);
      this.MinimumSize = new System.Drawing.Size(162, 84);
      this.Name = "LabelConfig";
      this.Size = new System.Drawing.Size(162, 84);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox textBoxMessage;
    private System.Windows.Forms.Label labelMessage;


  }
}
