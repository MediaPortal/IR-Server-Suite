namespace MediaPortal.Input
{
  partial class FullscreenConditionConfig
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
      this.radioButtonFullscreen = new System.Windows.Forms.RadioButton();
      this.radioButtonNoFullscreen = new System.Windows.Forms.RadioButton();
      this.SuspendLayout();
      // 
      // radioButtonFullscreen
      // 
      this.radioButtonFullscreen.AutoSize = true;
      this.radioButtonFullscreen.Location = new System.Drawing.Point(39, 22);
      this.radioButtonFullscreen.Name = "radioButtonFullscreen";
      this.radioButtonFullscreen.Size = new System.Drawing.Size(73, 17);
      this.radioButtonFullscreen.TabIndex = 0;
      this.radioButtonFullscreen.TabStop = true;
      this.radioButtonFullscreen.Text = "Fullscreen";
      this.radioButtonFullscreen.UseVisualStyleBackColor = true;
      this.radioButtonFullscreen.CheckedChanged += new System.EventHandler(this.CheckedChanged);
      // 
      // radioButtonNoFullscreen
      // 
      this.radioButtonNoFullscreen.AutoSize = true;
      this.radioButtonNoFullscreen.Location = new System.Drawing.Point(39, 45);
      this.radioButtonNoFullscreen.Name = "radioButtonNoFullscreen";
      this.radioButtonNoFullscreen.Size = new System.Drawing.Size(90, 17);
      this.radioButtonNoFullscreen.TabIndex = 1;
      this.radioButtonNoFullscreen.TabStop = true;
      this.radioButtonNoFullscreen.Text = "No Fullscreen";
      this.radioButtonNoFullscreen.UseVisualStyleBackColor = true;
      this.radioButtonNoFullscreen.CheckedChanged += new System.EventHandler(this.CheckedChanged);
      // 
      // FullscreenConditionConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.radioButtonNoFullscreen);
      this.Controls.Add(this.radioButtonFullscreen);
      this.MaximumSize = new System.Drawing.Size(544, 393);
      this.MinimumSize = new System.Drawing.Size(162, 84);
      this.Name = "FullscreenConditionConfig";
      this.Size = new System.Drawing.Size(162, 84);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.RadioButton radioButtonFullscreen;
    private System.Windows.Forms.RadioButton radioButtonNoFullscreen;



  }
}
