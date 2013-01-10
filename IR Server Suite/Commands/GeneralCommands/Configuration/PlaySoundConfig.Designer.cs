namespace IrssCommands.General
{
  partial class PlaySoundConfig
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
      this.labelPath = new System.Windows.Forms.Label();
      this.textBoxPath = new System.Windows.Forms.TextBox();
      this.buttonBrowse = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // labelPath
      // 
      this.labelPath.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.labelPath.AutoSize = true;
      this.labelPath.Location = new System.Drawing.Point(3, 3);
      this.labelPath.Name = "labelPath";
      this.labelPath.Size = new System.Drawing.Size(91, 13);
      this.labelPath.TabIndex = 0;
      this.labelPath.Text = "Sound file to play:";
      // 
      // textBoxPath
      // 
      this.textBoxPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxPath.Location = new System.Drawing.Point(3, 19);
      this.textBoxPath.Name = "textBoxPath";
      this.textBoxPath.Size = new System.Drawing.Size(252, 20);
      this.textBoxPath.TabIndex = 1;
      // 
      // buttonBrowse
      // 
      this.buttonBrowse.Anchor = System.Windows.Forms.AnchorStyles.Right;
      this.buttonBrowse.Location = new System.Drawing.Point(261, 17);
      this.buttonBrowse.Name = "buttonBrowse";
      this.buttonBrowse.Size = new System.Drawing.Size(32, 23);
      this.buttonBrowse.TabIndex = 2;
      this.buttonBrowse.Text = "...";
      this.buttonBrowse.UseVisualStyleBackColor = true;
      this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
      // 
      // PlaySoundConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.buttonBrowse);
      this.Controls.Add(this.textBoxPath);
      this.Controls.Add(this.labelPath);
      this.Name = "PlaySoundConfig";
      this.Size = new System.Drawing.Size(296, 51);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.Label labelPath;
    private System.Windows.Forms.TextBox textBoxPath;
    private System.Windows.Forms.Button buttonBrowse;
  }
}
