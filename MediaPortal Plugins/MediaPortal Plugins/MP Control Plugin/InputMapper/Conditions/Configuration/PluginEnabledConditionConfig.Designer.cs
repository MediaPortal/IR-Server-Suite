namespace MediaPortal.Input
{
  partial class PluginEnabledConditionConfig
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
      this.comboBox = new System.Windows.Forms.ComboBox();
      this.SuspendLayout();
      // 
      // label
      // 
      this.label.AutoSize = true;
      this.label.Location = new System.Drawing.Point(3, 35);
      this.label.Name = "label";
      this.label.Size = new System.Drawing.Size(68, 13);
      this.label.TabIndex = 1;
      this.label.Text = "Plugin name:";
      // 
      // comboBox
      // 
      this.comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBox.FormattingEnabled = true;
      this.comboBox.Location = new System.Drawing.Point(77, 32);
      this.comboBox.Name = "comboBox";
      this.comboBox.Size = new System.Drawing.Size(82, 21);
      this.comboBox.TabIndex = 2;
      this.comboBox.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
      // 
      // PluginEnabledConditionConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.comboBox);
      this.Controls.Add(this.label);
      this.MaximumSize = new System.Drawing.Size(544, 393);
      this.MinimumSize = new System.Drawing.Size(162, 84);
      this.Name = "PluginEnabledConditionConfig";
      this.Size = new System.Drawing.Size(162, 84);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label;
    private System.Windows.Forms.ComboBox comboBox;

  }
}
