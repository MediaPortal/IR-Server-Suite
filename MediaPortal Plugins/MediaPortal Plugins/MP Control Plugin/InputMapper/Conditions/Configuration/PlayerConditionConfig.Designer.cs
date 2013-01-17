namespace MediaPortal.Input
{
  partial class PlayerConditionConfig
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
      this.comboBox = new System.Windows.Forms.ComboBox();
      this.SuspendLayout();
      // 
      // comboBox
      // 
      this.comboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBox.FormattingEnabled = true;
      this.comboBox.Location = new System.Drawing.Point(21, 32);
      this.comboBox.Name = "comboBox";
      this.comboBox.Size = new System.Drawing.Size(121, 21);
      this.comboBox.TabIndex = 0;
      this.comboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
      // 
      // PlayerConditionConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.comboBox);
      this.MaximumSize = new System.Drawing.Size(544, 393);
      this.MinimumSize = new System.Drawing.Size(162, 84);
      this.Name = "PlayerConditionConfig";
      this.Size = new System.Drawing.Size(162, 84);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ComboBox comboBox;


  }
}
